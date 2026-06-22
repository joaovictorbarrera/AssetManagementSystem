using AssetManagementSystem.DTOs.CheckoutRequests;
using AssetManagementSystem.DTOs.Pagination;
using AssetManagementSystem.Enums;
using AssetManagementSystem.Models.Entities;
using AssetManagementSystem.Models.Repositories;

namespace AssetManagementSystem.Services
{
    public class CheckoutRequestService
    {
        private readonly CheckoutRequestRepository _repository;
        private readonly AssetRepository _assetRepository;

        public CheckoutRequestService(
            CheckoutRequestRepository repository,
            AssetRepository assetRepository)
        {
            _repository = repository;
            _assetRepository = assetRepository;
        }

        public async Task<ServiceResult<PagedResponse<CheckoutRequest>>> GetRequests(
            GetCheckoutRequestsRequest request,
            Guid requestorId,
            bool isManager)
        {
            if (request.Review && !isManager)
                return ServiceResult<PagedResponse<CheckoutRequest>>.Forbidden("No permission to view all requests");

            var result = await _repository.GetRequests(request, requestorId);

            return ServiceResult<PagedResponse<CheckoutRequest>>.Success(result);
        }

        public async Task<ServiceResult<CheckoutRequest>> Create(
            CreateCheckoutRequestRequest request,
            Guid requestorId)
        {
            if (request.RequestType == CheckoutRequestType.Return)
            {
                if (request.AssetId == null)
                    return ServiceResult<CheckoutRequest>.BadRequest(
                        "Return requests require an AssetId");

                Asset? asset = await _assetRepository.GetById(request.AssetId.Value);

                if (asset == null)
                    return ServiceResult<CheckoutRequest>.BadRequest(
                        "Asset does not exist");

                if (asset.AssignedToUserId != requestorId)
                    return ServiceResult<CheckoutRequest>.Forbidden(
                        "Asset is not assigned to you");
            }

            CheckoutRequest created = await _repository.Create(request, requestorId);
            return ServiceResult<CheckoutRequest>.Success(created);
        }

        public async Task<ServiceResult<CheckoutRequest>> GetDetail(
            Guid id,
            Guid requestorId,
            bool isManager)
        {
            CheckoutRequest? checkoutRequest = await _repository.GetById(id);

            if (checkoutRequest == null)
                return ServiceResult<CheckoutRequest>.NotFound();

            if (!isManager && checkoutRequest.RequestedByUserId != requestorId)
                return ServiceResult<CheckoutRequest>.Forbidden(
                    "Request does not belong to you");

            return ServiceResult<CheckoutRequest>.Success(checkoutRequest);
        }

        public async Task<ServiceResult> Archive(Guid id)
        {
            bool success = await _repository.ArchiveById(id);

            return success
                ? ServiceResult.Success()
                : ServiceResult.NotFound();
        }

        public async Task<ServiceResult> Cancel(Guid id, Guid requestorId)
        {
            CheckoutRequest? checkoutRequest = await _repository.GetById(id);

            if (checkoutRequest == null)
                return ServiceResult.NotFound();

            if (checkoutRequest.RequestedByUserId != requestorId)
                return ServiceResult.Forbidden("Request does not belong to you");

            if (checkoutRequest.Status != CheckoutRequestStatus.Pending)
                return ServiceResult.BadRequest("Only pending requests can be cancelled");

            if (checkoutRequest.IsArchived)
                return ServiceResult.BadRequest("Cannot update archived requests");

            bool success = await _repository.CancelById(id);

            return success
                ? ServiceResult.Success()
                : ServiceResult.NotFound();
        }

        public async Task<ServiceResult> Approve(Guid id, Guid reviewedByUserId)
        {
            CheckoutRequest? checkoutRequest = await _repository.GetById(id);

            if (checkoutRequest == null)
                return ServiceResult.NotFound();

            if (checkoutRequest.Status != CheckoutRequestStatus.Pending)
                return ServiceResult.BadRequest("Only pending requests can be approved");

            if (checkoutRequest.RequestType != CheckoutRequestType.Checkout)
                return ServiceResult.BadRequest("Only checkout requests can be approved");

            if (checkoutRequest.IsArchived)
                return ServiceResult.BadRequest("Cannot update archived requests");

            bool success = await _repository.ApproveById(id, reviewedByUserId);

            return success
                ? ServiceResult.Success()
                : ServiceResult.NotFound();
        }

        public async Task<ServiceResult> Reject(Guid id, Guid reviewedByUserId)
        {
            CheckoutRequest? checkoutRequest = await _repository.GetById(id);

            if (checkoutRequest == null)
                return ServiceResult.NotFound();

            if (checkoutRequest.Status != CheckoutRequestStatus.Pending)
                return ServiceResult.BadRequest("Only pending requests can be rejected");

            if (checkoutRequest.IsArchived)
                return ServiceResult.BadRequest("Cannot update archived requests");

            bool success = await _repository.RejectById(id, reviewedByUserId);

            return success
                ? ServiceResult.Success()
                : ServiceResult.NotFound();
        }

        public async Task<ServiceResult> AssignAsset(
            Guid id,
            AssignAssetRequest request,
            Guid reviewedByUserId)
        {
            CheckoutRequest? checkoutRequest = await _repository.GetById(id);

            if (checkoutRequest == null)
                return ServiceResult.NotFound();

            if (checkoutRequest.Status != CheckoutRequestStatus.Approved)
                return ServiceResult.BadRequest("Request must be approved");

            if (checkoutRequest.IsArchived)
                return ServiceResult.BadRequest("Cannot update archived requests");

            Asset? asset = await _assetRepository.GetById(request.AssetId);

            if (asset == null)
                return ServiceResult.BadRequest("Asset not found");

            if (asset.Status != AssetStatus.Available || asset.IsArchived || asset.AssignedToUserId != null)
                return ServiceResult.BadRequest("Asset is not available");

            if (asset.Category != checkoutRequest.AssetCategory)
                return ServiceResult.BadRequest(
                    "Asset Category is incompatible with Request Category");

            bool success = await _repository.AssignAssetById(
                id,
                request.AssetId,
                reviewedByUserId);

            return success
                ? ServiceResult.Success()
                : ServiceResult.NotFound();
        }

        public async Task<ServiceResult> Return(Guid id, Guid reviewedByUserId)
        {
            CheckoutRequest? request = await _repository.GetById(id);

            if (request == null)
                return ServiceResult.NotFound();

            if (request.RequestType != CheckoutRequestType.Return)
                return ServiceResult.BadRequest("Only return requests can be returned");

            if (request.Status != CheckoutRequestStatus.Pending)
                return ServiceResult.BadRequest(
                    "Only pending requests can be marked as returned");

            if (request.AssignedAssetId == null)
                return ServiceResult.BadRequest("No asset assigned");

            if (request.IsArchived)
                return ServiceResult.BadRequest("Cannot update archived requests");

            Asset? asset = await _assetRepository.GetById(request.AssignedAssetId.Value);

            if (asset == null)
                return ServiceResult.BadRequest("Asset does not exist");

            if (asset.IsArchived)
                return ServiceResult.BadRequest("Cannot update archived assets");

            bool success = await _repository.ReturnById(
                id,
                request.AssignedAssetId.Value,
                reviewedByUserId);

            return success
                ? ServiceResult.Success()
                : ServiceResult.NotFound();
        }
    }
}