using AssetManagementSystem.DTOs.Assets.Requests;
using AssetManagementSystem.DTOs.Assets.Responses;
using AssetManagementSystem.DTOs.Pagination;
using AssetManagementSystem.DTOs.Users;
using AssetManagementSystem.Enums;
using AssetManagementSystem.Models.Entities;
using AssetManagementSystem.Models.Repositories;

namespace AssetManagementSystem.Services
{
    public class AssetService
    {
        private readonly AssetRepository _assetRepository;

        public AssetService(AssetRepository assetRepository)
        {
            _assetRepository = assetRepository;
        }

        public async Task<ServiceResult<PagedResponse<AssetDto>>> GetAssets(
            GetAssetsRequest request,
            Requestor requestor)
        {
            bool managerFeatures = request.Inventory || request.ViewArchived;
            if (managerFeatures && !requestor.IsAssetManager)
                return ServiceResult<PagedResponse<AssetDto>>.Forbidden("No permission to view all assets");

            var result = await _assetRepository.GetAssets(request, requestor.UserId);

            return ServiceResult<PagedResponse<AssetDto>>.Success(result);
        }

        public async Task<ServiceResult<Asset>> Create(
            CreateAssetRequest request,
            Requestor requestor)
        {
            if (await _assetRepository.IsTagTakenAndNotId(request.AssetTag, Guid.Empty))
                return ServiceResult<Asset>.BadRequest("Asset Tag is taken");

            Asset asset = await _assetRepository.CreateAsset(request, requestor.UserId);
            return ServiceResult<Asset>.Success(asset);
        }

        public async Task<ServiceResult<List<AvailableAsset>>> GetAvailableByCategory(
            GetAvailableAssetsRequest request)
        {
            List<AvailableAsset> assets = await _assetRepository.GetAvailableByCategory(request.Category);
            return ServiceResult<List<AvailableAsset>>.Success(assets);
        }

        public async Task<ServiceResult<AssetDto>> GetDetail(
            Guid id,
            Requestor requestor)
        {
            AssetDto? asset = await _assetRepository.GetDtoById(id);

            if (asset == null)
                return ServiceResult<AssetDto>.NotFound();

            if (asset.AssignedToUserId != requestor.UserId && !requestor.IsAssetManager)
                return ServiceResult<AssetDto>.Forbidden("Asset is not assigned to you");

            return ServiceResult<AssetDto>.Success(asset);
        }

        public async Task<ServiceResult<Asset>> Update(
            Guid id,
            UpdateAssetRequest request,
            Requestor requestor)
        {
            if (await _assetRepository.IsTagTakenAndNotId(request.AssetTag, id))
                return ServiceResult<Asset>.BadRequest("Asset Tag is taken");

            Asset? existing = await _assetRepository.GetById(id);

            if (existing == null)
                return ServiceResult<Asset>.NotFound();

            if (existing.IsArchived)
                return ServiceResult<Asset>.BadRequest("Cannot update archived assets");

            Asset updated = await _assetRepository.UpdateById(existing, request, requestor.UserId);

            return ServiceResult<Asset>.Success(updated);
        }

        public async Task<ServiceResult> Archive(Guid id, Requestor requestor)
        {
            Asset? existing = await _assetRepository.GetById(id);

            if (existing == null)
                return ServiceResult.NotFound();

            if (existing.IsArchived)
                return ServiceResult.BadRequest("Asset is already archived");

            await _assetRepository.ArchiveById(existing, requestor.UserId);

            return ServiceResult.Success();
        }

        public async Task<ServiceResult> UpdateStatus(
            Guid id,
            UpdateAssetStatusRequest request,
            Requestor requestor)
        {
            Asset? existing = await _assetRepository.GetById(id);

            if (existing == null)
                return ServiceResult.NotFound();

            if (existing.IsArchived)
                return ServiceResult.BadRequest("Cannot update archived assets");

            bool shouldUnassign = request.Status == AssetStatus.Available
                && existing.AssignedToUserId != null;

            await _assetRepository.UpdateAssetStatus(
                existing,
                request.Status,
                requestor.UserId,
                shouldUnassign);

            return ServiceResult.Success();
        }

        public async Task<ServiceResult> UpdateCondition(
            Guid id,
            UpdateAssetConditionRequest request,
            Requestor requestor)
        {
            Asset? existing = await _assetRepository.GetById(id);

            if (existing == null)
                return ServiceResult.NotFound();

            if (existing.IsArchived)
                return ServiceResult.BadRequest("Cannot update archived assets");

            await _assetRepository.UpdateAssetCondition(existing, request.Condition, requestor.UserId);

            return ServiceResult.Success();
        }

        public async Task<ServiceResult<List<AssetHistory>>> GetHistory(Guid id)
        {
            Asset? existing = await _assetRepository.GetById(id);

            if (existing == null)
                return ServiceResult<List<AssetHistory>>.NotFound();

            List<AssetHistory> history = await _assetRepository.GetAssetHistory(id);
            return ServiceResult<List<AssetHistory>>.Success(history);
        }
    }
}