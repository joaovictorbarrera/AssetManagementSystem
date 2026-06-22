using AssetManagementSystem.Data;
using AssetManagementSystem.DTOs.Assets.Requests;
using AssetManagementSystem.DTOs.Assets.Responses;
using AssetManagementSystem.DTOs.Pagination;
using AssetManagementSystem.Enums;
using AssetManagementSystem.Extensions;
using AssetManagementSystem.Helpers;
using AssetManagementSystem.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

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
            return await _context.Assets.FirstOrDefaultAsync(a => a.AssetTag == assetTag && a.Id != id) != null;
        }

        public async Task<PagedResponse<AssetDto>> GetAssets(GetAssetsRequest request, Guid requestorId)
        {
            IQueryable<Asset> query = _context.Assets;

            if (!request.ViewArchived)
            {
                query = query.Where(a => !a.IsArchived && a.Status != AssetStatus.Retired);
            }

            if (!request.Inventory)
            {
                query = query.Where(a => a.AssignedToUserId == requestorId);
            }

            if (!String.IsNullOrEmpty(request.SearchText))
            {
                query = query.Where(a => a.Name.Contains(request.SearchText) || a.AssetTag.Contains(request.SearchText));
            }

            int totalCount = await query.CountAsync();

            List<AssetDto> assets = await query
                .OrderBy(a => a.AssetTag)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(a => new AssetDto
                {
                    Id = a.Id,
                    AssetTag = a.AssetTag,
                    Name = a.Name,
                    Category = a.Category,
                    Status = a.Status,
                    Condition = a.Condition,
                    IsArchived = a.IsArchived
                })
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

        public async Task<AssetDto> CreateAsset(CreateAssetRequest request, Guid createdByUserId)
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

            return new AssetDto
            {
                Id = asset.Id,
                AssetTag = asset.AssetTag,
                Name = asset.Name,
                Category = asset.Category,
                Status = asset.Status,
                Condition = asset.Condition,
                IsArchived = asset.IsArchived
            };
        }

        public async Task<List<AvailableAsset>> GetAvailableByCategory(AssetCategory category)
        {
           return await _context.Assets
                    .Where(a => a.Status == AssetStatus.Available && a.Category == category && !a.IsArchived)
                    .Select(a => new AvailableAsset{Id = a.Id, Name = a.Name})
                    .ToListAsync();
        }

        public async Task<Asset?> GetById(Guid id)
        {
            return await _context.Assets
                .Include(a => a.AssignedToUser)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Asset?> UpdateById(Guid id, UpdateAssetRequest request, Guid updatedByUserId)
        {
            Asset? asset = await GetById(id);
            if (asset == null) return null;

            if (asset.SerialNumber != request.SerialNumber)
                _context.AddAssetHistory(id, updatedByUserId, "Updated Serial Number", asset.SerialNumber, request.SerialNumber);
            asset.SerialNumber = request.SerialNumber;

            if (asset.AssetTag != request.AssetTag)
                _context.AddAssetHistory(id, updatedByUserId, "Updated Asset Tag", asset.AssetTag, request.AssetTag);
            asset.AssetTag = request.AssetTag;

            if (asset.Name != request.Name)
                _context.AddAssetHistory(id, updatedByUserId, "Updated Asset Name", asset.Name, request.Name);
            asset.Name = request.Name;
            asset.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return asset;
        }

        public async Task<bool> ArchiveById(Guid id, Guid archivedByUserId)
        {
            Asset? asset = await GetById(id);
            if (asset == null) return false;

            _context.AddAssetHistory(id, archivedByUserId, "Archived Asset");
            asset.IsArchived = true;
            asset.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateAssetStatus(Guid id, AssetStatus status, Guid updatedByUserId)
        {
            Asset? asset = await GetById(id);
            if (asset == null) return false;

            _context.AddAssetHistory(id, updatedByUserId, "Updated Asset Status", asset.Status.ToString(), status.ToString());

            asset.Status = status;
            asset.UpdatedAt = DateTime.UtcNow;

            // If an asset is assigned, and is being marked as available, then it should be unassigned
            if (status == AssetStatus.Available && asset.AssignedToUserId != null)
            {
                string userEmail = asset.AssignedToUser?.EmailAddress ?? "Unknown";
                _context.AddAssetHistory(id, updatedByUserId, $"Unassigned Asset from {userEmail}");
                asset.AssignedToUserId = null;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAssetCondition(Guid id, AssetCondition condition, Guid updatedByUserId)
        {
            Asset? asset = await GetById(id);
            if (asset == null) return false;

            _context.AddAssetHistory(id, updatedByUserId, "Updated Asset Condition", asset.Condition.ToString(), condition.ToString());

            asset.Condition = condition;
            asset.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<AssetHistory>> GetAssetHistory(Guid id)
        {
            return await _context.AssetHistories
                .Where(h => h.AssetId == id)
                .ToListAsync();
        }
    }
}
