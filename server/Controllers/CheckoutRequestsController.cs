using AssetManagementSystem.DTOs.CheckoutRequests;
using AssetManagementSystem.DTOs.Pagination;
using AssetManagementSystem.Extensions;
using AssetManagementSystem.Enums;
using AssetManagementSystem.Models.Entities;
using AssetManagementSystem.Models.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagementSystem.Controllers
{
    [Authorize]
    [Route("api/checkout-requests")]
    [ApiController]
    public class CheckoutRequestsController : ControllerBase
    {
        private readonly CheckoutRequestRepository _repository;
        private readonly AssetRepository _assetRepository;

        public CheckoutRequestsController(
            CheckoutRequestRepository repository, 
            AssetRepository assetRepository)
        {
            _repository = repository;
            _assetRepository = assetRepository;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponse<CheckoutRequest>>> Get(
            [FromQuery] GetCheckoutRequestsRequest request)
        {
            bool isManager =
                User.IsInRole("AssetManager") ||
                User.IsInRole("Admin");
            bool managerFeatures = request.Review;

            if (managerFeatures && !isManager)
            {
                return Forbid("Not enough permissions");
            }

            Guid requestorId = User.GetUserId();

            return Ok(await _repository.GetRequests(
                request,
                requestorId));
        }

        [HttpPost]
        public async Task<ActionResult<CheckoutRequest>> Create(
            [FromBody] CreateCheckoutRequestRequest request
        ){
            if (request.RequestType == CheckoutRequestType.Return)
            {
                if (request.AssetId == null)
                {
                    return BadRequest("Return requests required an AssetId");
                } else
                {
                    Asset? asset = await _assetRepository.GetById(request.AssetId.Value);
                    if (asset != null && asset.AssignedToUserId != User.GetUserId())
                        return Forbid("Asset is not assigned to you");

                    if (asset == null)
                        return BadRequest("Asset does not exist");
                }
            }

            CheckoutRequest created =
                await _repository.Create(
                    request,
                    User.GetUserId());

            return Ok(created);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CheckoutRequest>> GetDetail(Guid id)
        {
            CheckoutRequest? checkoutRequest = await _repository.GetById(id);

            if (checkoutRequest == null)
                return NotFound();

            bool isManager =
                User.IsInRole("AssetManager") ||
                User.IsInRole("Admin");

            if (!isManager &&
                checkoutRequest.RequestedByUserId != User.GetUserId())
            {
                return Forbid("Request does not belong to you");
            }

            return Ok(checkoutRequest);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Archive(Guid id)
        {
            bool success = await _repository.ArchiveById(id);

            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpPatch("{id:guid}/cancel")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            CheckoutRequest? checkoutRequest = await _repository.GetById(id);

            if (checkoutRequest == null)
                return NotFound();

            if (checkoutRequest.RequestedByUserId != User.GetUserId())
                return Forbid("Request does not belong to you");

            if (checkoutRequest.Status != CheckoutRequestStatus.Pending)
                return BadRequest("Only pending requests can be cancelled");

            if (checkoutRequest.IsArchived)
                return BadRequest("Cannot update archived requests");

            bool success = await _repository.CancelById(id);

            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpPatch("{id:guid}/approve")]
        [Authorize(Policy = "AssetManager+")]
        public async Task<IActionResult> Approve(Guid id)
        {
            CheckoutRequest? checkoutRequest = await _repository.GetById(id);
            Guid reviewedByUserId = User.GetUserId();

            if (checkoutRequest == null)
                return NotFound();

            if (checkoutRequest.Status != CheckoutRequestStatus.Pending)
                return BadRequest("Only pending requests can be approved");

            if (checkoutRequest.RequestType != CheckoutRequestType.Checkout)
                return BadRequest("Only checkout requests can be approved");

            if (checkoutRequest.IsArchived)
                return BadRequest("Cannot update archived requests");

            bool success = await _repository.ApproveById(id, reviewedByUserId);

            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpPatch("{id:guid}/reject")]
        [Authorize(Policy = "AssetManager+")]
        public async Task<IActionResult> Reject(Guid id)
        {
            CheckoutRequest? checkoutRequest = await _repository.GetById(id);
            Guid reviewedByUserId = User.GetUserId();

            if (checkoutRequest == null)
                return NotFound();

            if (checkoutRequest.Status != CheckoutRequestStatus.Pending)
                return BadRequest("Only pending requests can be rejected");

            if (checkoutRequest.IsArchived)
                return BadRequest("Cannot update archived requests");

            bool success = await _repository.RejectById(id, reviewedByUserId);

            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpPatch("{id:guid}/assign-asset")]
        [Authorize(Policy = "AssetManager+")]
        public async Task<IActionResult> Assign(
            Guid id,
            [FromBody] AssignAssetRequest request)
        {
            CheckoutRequest? checkoutRequest = await _repository.GetById(id);

            if (checkoutRequest == null)
                return NotFound();

            if (checkoutRequest.Status != CheckoutRequestStatus.Approved)
                return BadRequest("Request must be approved");

            if (checkoutRequest.IsArchived)
                return BadRequest("Cannot update archived requests");

            Asset? asset = await _assetRepository.GetById(request.AssetId);

            if (asset == null)
                return BadRequest("Asset not found");

            if (asset.Status != AssetStatus.Available)
                return BadRequest("Asset is not available");

            if (asset.Category != checkoutRequest.AssetCategory)
                return BadRequest("Asset Category is incompatible with Request Category");

            bool success = await _repository.AssignAssetById(
                id,
                request.AssetId,
                User.GetUserId());

            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpPatch("{id:guid}/return")]
        [Authorize(Policy = "AssetManager+")]
        public async Task<IActionResult> Return(Guid id)
        {
            CheckoutRequest? request = await _repository.GetById(id);

            if (request == null)
                return NotFound();

            if (request.RequestType != CheckoutRequestType.Return)
                return BadRequest("Only return requests can be returned");

            if (request.Status != CheckoutRequestStatus.Pending)
                return BadRequest("Request must be pending");

            if (request.AssignedAssetId == null)
                return BadRequest("No asset assigned");

            if (request.IsArchived)
                return BadRequest("Cannot update archived requests");

            bool success = await _repository.ReturnById(
                id,
                request.AssignedAssetId.Value,
                User.GetUserId());

            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
