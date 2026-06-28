using AssetManagementSystem.Data;
using AssetManagementSystem.DTOs.Assets.Projections;
using AssetManagementSystem.DTOs.Assets.Requests;
using AssetManagementSystem.DTOs.Assets.Responses;
using AssetManagementSystem.DTOs.Pagination;
using AssetManagementSystem.Enums;
using AssetManagementSystem.Extensions;
using AssetManagementSystem.Helpers;
using AssetManagementSystem.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AssetManagementSystem.Models.Repositories
{
    public class AssetRepository
    {
        private readonly AppDbContext _context;

        public AssetRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsTagTakenAndNotId(string assetTag, Guid id)
        {
            return await _context.Assets.AnyAsync(a => a.AssetTag == assetTag && a.Id != id);
        }

        public async Task<PagedResponse<AssetDto>> GetAssets(GetAssetsRequest request, Guid requestorId)
        {
            IQueryable<Asset> query = _context.Assets;

            if (!request.IncludeArchived)
            {
                query = query.Where(a => !a.IsArchived && a.Status != AssetStatus.Retired);
            }

            if (!request.Inventory)
            {
                query = query.Where(a => a.AssignedToUserId == requestorId);
            }

            if (!string.IsNullOrEmpty(request.SearchText))
            {
                query = query.Where(a =>
                    a.Name.Contains(request.SearchText) ||
                    a.AssetTag.Contains(request.SearchText));
            }

            if (request.Status != null)
            {
                query = query.Where(a => a.Status == request.Status);
            }

            if (request.Category != null)
            {
                query = query.Where(a => a.Category == request.Category);
            }

            if (request.Condition != null)
            {
                query = query.Where(a => a.Condition == request.Condition);
            }

            int totalCount = await query.CountAsync();

            List<AssetDto> assets = await query
                .OrderBy(a => a.AssetTag)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(AssetExpressions.ToDto)
                .ToListAsync();

            int totalPages = PaginationHelper.GetTotalPageCount(totalCount, request.PageSize);

            return new PagedResponse<AssetDto>
            {
                Items = assets,
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

        public async Task<Guid> CreateAsset(CreateAssetRequest request, Guid createdByUserId)
        {
            Asset asset = new()
            {
                AssetTag = request.AssetTag,
                Name = request.Name,
                Condition = request.Condition,
                SerialNumber = request.SerialNumber,
                Status = request.Status,
                Category = request.Category
            };

            _context.Assets.Add(asset);
            _context.AddAssetHistory(asset.Id, createdByUserId, "Created Asset");

            await _context.SaveChangesAsync();

            return asset.Id;
        }

        public async Task<List<AvailableAsset>> GetAvailableByCategory(AssetCategory category)
        {
            return await _context.Assets
                .Where(a => a.Status == AssetStatus.Available && a.Category == category && !a.IsArchived)
                .Select(a => new AvailableAsset { Id = a.Id, Name = a.Name })
                .ToListAsync();
        }

        public async Task<Asset?> GetById(Guid id)
        {
            return await _context.Assets
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<AssetDto?> GetDtoById(Guid id)
        {
            return await _context.Assets
                .Where(a => a.Id == id)
                .Select(AssetExpressions.ToDto)
                .FirstOrDefaultAsync();
        }

        public async Task<AssetDetail?> GetDetailById(Guid id)
        {
            return await _context.Assets
                .Where(a => a.Id == id)
                .Select(AssetExpressions.ToDetail)
                .FirstOrDefaultAsync();
        }

        public async Task<Asset> UpdateById(
            Asset asset, 
            UpdateAssetRequest request, 
            Guid updatedByUserId)
        {
            if (asset.SerialNumber != request.SerialNumber)
                _context.AddAssetHistory(asset.Id, updatedByUserId, "Updated Serial Number", asset.SerialNumber, request.SerialNumber);
            asset.SerialNumber = request.SerialNumber;

            if (asset.AssetTag != request.AssetTag)
                _context.AddAssetHistory(asset.Id, updatedByUserId, "Updated Asset Tag", asset.AssetTag, request.AssetTag);
            asset.AssetTag = request.AssetTag;

            if (asset.Name != request.Name)
                _context.AddAssetHistory(asset.Id, updatedByUserId, "Updated Asset Name", asset.Name, request.Name);
            asset.Name = request.Name;



            asset.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return asset;
        }

        public async Task ArchiveById(Asset asset, Guid archivedByUserId)
        {
            _context.AddAssetHistory(asset.Id, archivedByUserId, "Archived Asset");
            asset.IsArchived = true;
            asset.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAssetStatus(
            Asset asset,
            AssetStatus status,
            Guid updatedByUserId,
            bool shouldUnassign)
        {
            _context.AddAssetHistory(
                asset.Id,
                updatedByUserId,
                "Updated Asset Status",
                asset.Status.ToString(),
                status.ToString());

            asset.Status = status;
            asset.UpdatedAt = DateTime.UtcNow;

            if (shouldUnassign)
            {
                string userEmail = asset.AssignedToUser?.EmailAddress ?? "Unknown";
                _context.AddAssetHistory(asset.Id, updatedByUserId, $"Unassigned Asset from {userEmail}");
                asset.AssignedToUserId = null;
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAssetCondition(Asset asset, AssetCondition condition, Guid updatedByUserId)
        {
            _context.AddAssetHistory(
                asset.Id,
                updatedByUserId,
                "Updated Asset Condition",
                asset.Condition.ToString(),
                condition.ToString());

            asset.Condition = condition;
            asset.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task<List<AssetHistory>> GetAssetHistory(Guid id)
        {
            return await _context.AssetHistories
                .Where(h => h.AssetId == id)
                .ToListAsync();
        }
    }
}