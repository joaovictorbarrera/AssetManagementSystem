using AssetManagementSystem.Data;
using AssetManagementSystem.DTOs.CheckoutRequests;
using AssetManagementSystem.DTOs.Pagination;
using AssetManagementSystem.Enums;
using AssetManagementSystem.Extensions;
using AssetManagementSystem.Helpers;
using AssetManagementSystem.Models.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AssetManagementSystem.Models.Repositories
{
    public class CheckoutRequestRepository
    {
        private readonly AppDbContext _context;

        public CheckoutRequestRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResponse<CheckoutRequest>> GetRequests(
            GetCheckoutRequestsRequest request,
            Guid currentUserId)
        {
            IQueryable<CheckoutRequest> query = _context.CheckoutRequests;
            if (!request.Review)
            {
                query = query.Where(r => r.RequestedByUserId == currentUserId);
            }

            if (!request.IncludeClosedRequests)
            {
                query = query.Where(r =>
                    r.Status == CheckoutRequestStatus.Pending ||
                    r.Status == CheckoutRequestStatus.Approved &&
                    !r.IsArchived);
            }

            int totalCount = await query.CountAsync();

            List<CheckoutRequest> items = await query
                .OrderByDescending(r => r.CreatedAt)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            int totalPages = PaginationHelper.GetTotalPageCount(totalCount, request.PageSize);

            return new PagedResponse<CheckoutRequest>
            {
                Items = items,
                Pagination = new PaginationMetadata
                {
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize,
                    TotalCount = totalCount,
                    TotalPages = totalPages,
                    HasPreviousPage = request.PageNumber > 1,
                    HasNextPage = request.PageNumber < totalPages
                }
            };
        }

        public async Task<CheckoutRequest?> GetById(Guid id)
        {
            return await _context.CheckoutRequests
                .Include(r => r.RequestedByUser)
                .Include(r => r.ReviewedByUser)
                .Include(r => r.AssignedAsset)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<CheckoutRequest> Create(CreateCheckoutRequestRequest request, Guid userId)
        {
            CheckoutRequest checkoutRequest = new()
            {
                RequestType = request.RequestType,
                RequestedByUserId = userId,
                AssetCategory = request.AssetCategory,
                Reason = request.Reason,
                AssignedAssetId = request.RequestType == CheckoutRequestType.Checkout ? request.AssetId : null,
                Status = CheckoutRequestStatus.Pending
            };

            _context.CheckoutRequests.Add(checkoutRequest);

            await _context.SaveChangesAsync();

            return checkoutRequest;
        }

        public async Task<bool> ArchiveById(Guid id)
        {
            return await _context.CheckoutRequests
                .Where(r => r.Id == id)
                .ExecuteUpdateAsync(r =>
                    r.SetProperty(x => x.IsArchived, true)
                     .SetProperty(x => x.UpdatedAt, DateTime.UtcNow)
                ) > 0;
        }

        public async Task<bool> CancelById(Guid id)
        {
            return await _context.CheckoutRequests
                .Where(r => r.Id == id)
                .ExecuteUpdateAsync(r => r
                    .SetProperty(x => x.Status, CheckoutRequestStatus.Cancelled)
                    .SetProperty(x => x.UpdatedAt, DateTime.UtcNow)
                ) > 0;
        }

        public async Task<bool> ApproveById(Guid id, Guid reviewerId)
        {
            return await _context.CheckoutRequests
                .Where(r => r.Id == id)
                .ExecuteUpdateAsync(r => r
                    .SetProperty(x => x.Status, CheckoutRequestStatus.Approved)
                    .SetProperty(x => x.ReviewedByUserId, reviewerId)
                    .SetProperty(x => x.ApprovedAt, DateTime.UtcNow)
                    .SetProperty(x => x.UpdatedAt, DateTime.UtcNow)
                ) > 0;
        }

        public async Task<bool> RejectById(Guid id, Guid reviewerId)
        {
            return await _context.CheckoutRequests
                .Where(r => r.Id == id)
                .ExecuteUpdateAsync(r => r
                    .SetProperty(x => x.Status, CheckoutRequestStatus.Rejected)
                    .SetProperty(x => x.ReviewedByUserId, reviewerId)
                    .SetProperty(x => x.RejectedAt, DateTime.UtcNow)
                    .SetProperty(x => x.UpdatedAt, DateTime.UtcNow)
                ) > 0;
        }

        public async Task<bool> AssignAssetById(
            Guid requestId,
            Guid assetId,
            Guid reviewedByUserId)
        {
            CheckoutRequest? request = await _context.CheckoutRequests
                .Include(r => r.RequestedByUser)
                .FirstOrDefaultAsync(r =>
                    r.Id == requestId &&
                    r.Status == CheckoutRequestStatus.Approved);

            Asset? asset = await _context.Assets
                .FirstOrDefaultAsync(a =>
                    a.Id == assetId &&
                    a.Status == AssetStatus.Available);

            if (request == null || asset == null)
                return false;

            request.AssignedAssetId = assetId;
            request.Status = CheckoutRequestStatus.Fulfilled;
            request.FulfilledAt = DateTime.UtcNow;
            request.UpdatedAt = DateTime.UtcNow;

            _context.AddAssetHistory(
                assetId,
                reviewedByUserId,
                "Updated Asset Status",
                asset.Status.ToString(),
                AssetStatus.Assigned.ToString());

            asset.Status = AssetStatus.Assigned;

            _context.AddAssetHistory(
                assetId,
                reviewedByUserId,
                $"Assigned Asset to {request?.RequestedByUser?.EmailAddress ?? "Employee"}");

            asset.AssignedToUserId = request!.RequestedByUserId;
            asset.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ReturnById(
            Guid requestId,
            Guid assetId,
            Guid reviewedByUserId)
        {
            CheckoutRequest? request = await _context.CheckoutRequests
                .Include(r => r.RequestedByUser)
                .FirstOrDefaultAsync(r =>
                    r.Id == requestId &&
                    r.Status == CheckoutRequestStatus.Pending &&
                    r.RequestType == CheckoutRequestType.Return);

            Asset? asset = await _context.Assets
                .FirstOrDefaultAsync(a => a.Id == assetId);

            if (request == null || asset == null)
                return false;

            request.Status = CheckoutRequestStatus.Returned;
            request.ReturnedAt = DateTime.UtcNow;
            request.UpdatedAt = DateTime.UtcNow;

            // Do not make available if Asset is damaged or under maintenance.
            if (asset.Condition != AssetCondition.Damaged && asset.Status != AssetStatus.Maintenance)
            {
                _context.AddAssetHistory(
                assetId,
                reviewedByUserId,
                "Updated Asset Status",
                asset.Status.ToString(),
                AssetStatus.Available.ToString());

                asset.Status = AssetStatus.Available;
            }

            _context.AddAssetHistory(
                assetId,
                reviewedByUserId,
                $"Unassigned Asset from {request.RequestedByUser.EmailAddress}");

            asset.AssignedToUserId = null;
            asset.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
