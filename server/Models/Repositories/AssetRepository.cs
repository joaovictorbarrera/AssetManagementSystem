using AssetManagementSystem.Data;
using AssetManagementSystem.DTOs.Assets.Requests;
using AssetManagementSystem.DTOs.Assets.Responses;
using AssetManagementSystem.DTOs.Pagination;
using AssetManagementSystem.Enums;
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

        public async Task<Asset?> GetByAssetTag(string assetTag)
        {
            return await _context.Assets.FirstOrDefaultAsync(a => a.AssetTag == assetTag);
        }

        public async Task<PagedResponse<AssetDto>> GetAssets(GetAssetsRequest request, Guid requestorId)
        {
            IQueryable<Asset> query = _context.Assets;

            if (!request.ViewArchived)
            {
                query = query.Where(a => !a.IsArchived);
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

        public async Task<AssetDto> CreateAsset(CreateAssetRequest request)
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

        public async Task<List<AvailableAsset>> GetAvailableAssetsByCategory(GetAvailableAssetsByCategoryRequest request)
        {
           return await _context.Assets
                    .Where(a => a.Status == AssetStatus.Available && a.Category == request.Category)
                    .Select(a => new AvailableAsset{Id = a.Id, Name = a.Name})
                    .ToListAsync();
        }

        public async Task<Asset?> GetById(Guid id)
        {
            return await _context.Assets
                .Include(a => a.AssignedToUser)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Asset?> UpdateById(Guid id, UpdateAssetRequest request)
        {
            Asset? asset = await GetById(id);
            if (asset == null) return null;

            asset.SerialNumber = request.SerialNumber;
            asset.AssetTag = request.AssetTag;
            asset.Name = request.Name;
            asset.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return asset;
        }

        public async Task<bool> ArchiveById(Guid id)
        {
            return await _context.Assets
                .Where(a => a.Id == id)
                .ExecuteUpdateAsync(a => a.SetProperty(x => x.IsArchived, true)) > 0;
        }

        public async Task<bool> UpdateAssetStatus(Guid id, AssetStatus status)
        {
            return await _context.Assets
                .Where(a => a.Id == id)
                .ExecuteUpdateAsync(a => a
                    .SetProperty(x => x.Status, status)
                    .SetProperty(x => x.UpdatedAt, DateTime.UtcNow)
                ) > 0;
        }

        public async Task<bool> UpdateAssetCondition(Guid id, AssetCondition condition)
        {
            return await _context.Assets
                .Where(a => a.Id == id)
                .ExecuteUpdateAsync(a => a
                    .SetProperty(x => x.Condition, condition)
                    .SetProperty(x => x.UpdatedAt, DateTime.UtcNow)
                ) > 0;
        }

        public async Task<List<AssetHistory>> GetAssetHistory(Guid id)
        {
            return await _context.AssetHistories
                .Where(h => h.AssetId == id)
                .ToListAsync();
        }
    }
}
