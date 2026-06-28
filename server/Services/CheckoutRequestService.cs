using AssetManagementSystem.DTOs.Assets.Responses;
using AssetManagementSystem.DTOs.CheckoutRequests;
using AssetManagementSystem.DTOs.CheckoutRequests.Requests;
using AssetManagementSystem.DTOs.Pagination;
using AssetManagementSystem.DTOs.Users;
using AssetManagementSystem.Enums;
using AssetManagementSystem.Models.Entities;
using AssetManagementSystem.Models.Repositories;

namespace AssetManagementSystem.Services
{
    public class CheckoutRequestService
    {
        private readonly CheckoutRequestRepository _requestRepository;
        private readonly AssetRepository _assetRepository;

        public CheckoutRequestService(
            CheckoutRequestRepository requestRepository,
            AssetRepository assetRepository)
        {
            _requestRepository = requestRepository;
            _assetRepository = assetRepository;
        }

        public async Task<ServiceResult<PagedResponse<CheckoutRequestDto>>> GetRequests(
            GetCheckoutRequestsRequest request,
            Requestor requestor)
        {
            if (request.Review && !requestor.IsAssetManager)
                return ServiceResult<PagedResponse<CheckoutRequestDto>>.Forbidden("No permission to view all requests");

            var result = await _requestRepository.GetRequests(request, requestor.UserId);

            return ServiceResult<PagedResponse<CheckoutRequestDto>>.Success(result);
        }

        public async Task<ServiceResult<Guid>> Create(
            CreateCheckoutRequestRequest request,
            Requestor requestor)
        {
            if (request.RequestType == CheckoutRequestType.Return)
            {
                if (request.AssetId == null)
                    return ServiceResult<Guid>.BadRequest(
                        "Return requests require an AssetId");

                AssetDto? asset = await _assetRepository.GetDtoById(request.AssetId.Value);

                if (asset == null)
                    return ServiceResult<Guid>.BadRequest(
                        "Asset does not exist");

                if (asset.UserId != requestor.UserId)
                    return ServiceResult<Guid>.Forbidden(
                        "Asset is not assigned to you");

                if (asset.IsPendingReturn)
                {
                    return ServiceResult<Guid>.BadRequest(
                        "Asset is already pending return");
                }
            }

            Guid createdId = await _requestRepository.Create(request, requestor.UserId);
            return ServiceResult<Guid>.Success(createdId);
        }

        public async Task<ServiceResult<CheckoutRequestDetail>> GetDetail(
            Guid id,
            Requestor requestor)
        {
            CheckoutRequestDetail? checkoutRequest = await _requestRepository.GetDetailById(id);

            if (checkoutRequest == null)
                return ServiceResult<CheckoutRequestDetail>.NotFound();

            if (!requestor.IsAssetManager && checkoutRequest.RequestedByUser.Id != requestor.UserId)
                return ServiceResult<CheckoutRequestDetail>.Forbidden(
                    "Request does not belong to you");

            return ServiceResult<CheckoutRequestDetail>.Success(checkoutRequest);
        }

        public async Task<ServiceResult> Archive(Guid id)
        {
            bool success = await _requestRepository.ArchiveById(id);

            return success
                ? ServiceResult.Success()
                : ServiceResult.NotFound();
        }

        public async Task<ServiceResult> Cancel(Guid id, Requestor requestor)
        {
            CheckoutRequest? checkoutRequest = await _requestRepository.GetById(id);

            if (checkoutRequest == null)
                return ServiceResult.NotFound();

            if (checkoutRequest.RequestedByUserId != requestor.UserId)
                return ServiceResult.Forbidden("Request does not belong to you");

            if (checkoutRequest.Status != CheckoutRequestStatus.Pending && 
                checkoutRequest.Status != CheckoutRequestStatus.Approved)
                return ServiceResult.BadRequest("Only pending requests can be cancelled");

            if (checkoutRequest.IsArchived)
                return ServiceResult.BadRequest("Cannot update archived requests");

            bool success = await _requestRepository.CancelById(id);

            return success
                ? ServiceResult.Success()
                : ServiceResult.NotFound();
        }

        public async Task<ServiceResult> Approve(Guid id, Requestor requestor)
        {
            CheckoutRequest? checkoutRequest = await _requestRepository.GetById(id);

            if (checkoutRequest == null)
                return ServiceResult.NotFound();

            if (checkoutRequest.Status != CheckoutRequestStatus.Pending)
                return ServiceResult.BadRequest("Only pending requests can be approved");

            if (checkoutRequest.RequestType != CheckoutRequestType.Checkout)
                return ServiceResult.BadRequest("Only checkout requests can be approved");

            if (checkoutRequest.IsArchived)
                return ServiceResult.BadRequest("Cannot update archived requests");

            bool success = await _requestRepository.ApproveById(id, requestor.UserId);

            return success
                ? ServiceResult.Success()
                : ServiceResult.NotFound();
        }

        public async Task<ServiceResult> Reject(Guid id, Requestor requestor)
        {
            CheckoutRequest? checkoutRequest = await _requestRepository.GetById(id);

            if (checkoutRequest == null)
                return ServiceResult.NotFound();

            if (checkoutRequest.Status != CheckoutRequestStatus.Pending)
                return ServiceResult.BadRequest("Only pending requests can be rejected");

            if (checkoutRequest.IsArchived)
                return ServiceResult.BadRequest("Cannot update archived requests");

            bool success = await _requestRepository.RejectById(id, requestor.UserId);

            return success
                ? ServiceResult.Success()
                : ServiceResult.NotFound();
        }

        public async Task<ServiceResult> AssignAsset(
            Guid id,
            AssignAssetRequest request,
            Requestor requestor)
        {
            CheckoutRequest? checkoutRequest = await _requestRepository.GetById(id);

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

            await _requestRepository.AssignAssetById(
                checkoutRequest,
                asset,
                requestor.UserId);

            return ServiceResult.Success();
        }

        public async Task<ServiceResult> Return(Guid id, Requestor requestor)
        {
            CheckoutRequest? request = await _requestRepository.GetById(id);

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

            bool shouldBeAvailable = asset.Condition != AssetCondition.Damaged && asset.Status != AssetStatus.Maintenance;

            await _requestRepository.ReturnById(
                request,
                asset,
                requestor.UserId,
                shouldBeAvailable);

            return ServiceResult.Success();
        }
    }
}