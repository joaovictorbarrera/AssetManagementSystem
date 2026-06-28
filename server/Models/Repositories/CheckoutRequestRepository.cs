using AssetManagementSystem.Data;
using AssetManagementSystem.DTOs.Assets.Responses;
using AssetManagementSystem.DTOs.CheckoutRequests;
using AssetManagementSystem.DTOs.CheckoutRequests.Projections;
using AssetManagementSystem.DTOs.CheckoutRequests.Requests;
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

        public async Task<PagedResponse<CheckoutRequestDto>> GetRequests(
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
                    (r.Status == CheckoutRequestStatus.Pending ||
                    r.Status == CheckoutRequestStatus.Approved) &&
                    !r.IsArchived);
            }

            if (request.AssetCategory != null)
            {
                query = query.Where(r => r.AssetCategory == request.AssetCategory);
            }

            if (request.Status != null)
            {
                query = query.Where(r => r.Status == request.Status);
            }

            if (request.Type != null)
            {
                query = query.Where(r => r.RequestType == request.Type);
            }

            int totalCount = await query.CountAsync();

            List<CheckoutRequestDto> items = await query
            .OrderByDescending(r => r.CreatedAt)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(CheckoutRequestExpressions.ToDto)
            .ToListAsync();

            int totalPages = PaginationHelper.GetTotalPageCount(totalCount, request.PageSize);

            return new PagedResponse<CheckoutRequestDto>
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
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<CheckoutRequestDetail?> GetDetailById(Guid id)
        {
            return await _context.CheckoutRequests
                        .Where(r => r.Id == id)
                        .Select(CheckoutRequestExpressions.ToDetail)
                        .FirstOrDefaultAsync();
        }

        public async Task<Guid> Create(CreateCheckoutRequestRequest request, Guid userId)
        {
            CheckoutRequest checkoutRequest = new()
            {
                RequestType = request.RequestType,
                RequestedByUserId = userId,
                AssetCategory = request.AssetCategory,
                Reason = request.Reason,
                AssignedAssetId = request.RequestType == CheckoutRequestType.Checkout ? null : request.AssetId,
                Status = CheckoutRequestStatus.Pending
            };

            _context.CheckoutRequests.Add(checkoutRequest);

            await _context.SaveChangesAsync();

            return checkoutRequest.Id;
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

        public async Task AssignAssetById(
            CheckoutRequest request,
            Asset asset,
            Guid reviewedByUserId)
        {
            request.AssignedAssetId = asset.Id;
            request.Status = CheckoutRequestStatus.Fulfilled;
            request.FulfilledAt = DateTime.UtcNow;
            request.UpdatedAt = DateTime.UtcNow;

            _context.AddAssetHistory(
                asset.Id,
                reviewedByUserId,
                "Updated Asset Status",
                asset.Status.ToString(),
                AssetStatus.Assigned.ToString());

            asset.Status = AssetStatus.Assigned;

            _context.AddAssetHistory(
                asset.Id,
                reviewedByUserId,
                $"Assigned Asset to {request?.RequestedByUser?.EmailAddress ?? "Unknown"}");

            asset.AssignedToUserId = request!.RequestedByUserId;
            asset.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task ReturnById(
            CheckoutRequest request,
            Asset asset,
            Guid reviewedByUserId,
            bool shouldBeAvailable)
        {
            request.Status = CheckoutRequestStatus.Returned;
            request.ReturnedAt = DateTime.UtcNow;
            request.UpdatedAt = DateTime.UtcNow;

            if (shouldBeAvailable)
            {
                _context.AddAssetHistory(
                asset.Id,
                reviewedByUserId,
                "Updated Asset Status",
                asset.Status.ToString(),
                AssetStatus.Available.ToString());

                asset.Status = AssetStatus.Available;
            } else
            {
                _context.AddAssetHistory(
                asset.Id,
                reviewedByUserId,
                "Updated Asset Status",
                asset.Status.ToString(),
                AssetStatus.Maintenance.ToString());

                asset.Status = AssetStatus.Maintenance;
            }

            _context.AddAssetHistory(
                asset.Id,
                reviewedByUserId,
                $"Unassigned Asset from {request?.RequestedByUser?.EmailAddress ?? "Unknown"}");

            asset.AssignedToUserId = null;
            asset.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
    }
}
