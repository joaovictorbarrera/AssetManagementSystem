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
        public async Task<ActionResult<PagedResponse<CheckoutRequest>>> GetCheckoutRequests(
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
        public async Task<IActionResult> CreateCheckoutRequest(
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
        public async Task<IActionResult> GetCheckoutRequest(Guid id)
        {
            CheckoutRequest? request = await _repository.GetById(id);

            if (request == null)
                return NotFound();

            bool isManager =
                User.IsInRole("AssetManager") ||
                User.IsInRole("Admin");

            if (!isManager &&
                request.RequestedByUserId != User.GetUserId())
            {
                return Forbid("Request does not belong to you");
            }

            return Ok(request);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ArchiveCheckoutRequest(Guid id)
        {
            bool success = await _repository.ArchiveById(id);

            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpPatch("{id:guid}/cancel")]
        public async Task<IActionResult> CancelCheckoutRequest(Guid id)
        {
            CheckoutRequest? request = await _repository.GetById(id);

            if (request == null)
                return NotFound();

            if (request.RequestedByUserId != User.GetUserId())
                return Forbid("Request does not belong to you");

            if (request.Status != CheckoutRequestStatus.Pending)
                return BadRequest("Only pending requests can be cancelled");

            bool success = await _repository.CancelById(id);

            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpPatch("{id:guid}/approve")]
        [Authorize(Policy = "AssetManager+")]
        public async Task<IActionResult> ApproveCheckoutRequest(Guid id)
        {
            CheckoutRequest? request = await _repository.GetById(id);
            Guid reviewedByUserId = User.GetUserId();

            if (request == null)
                return NotFound();

            if (request.Status != CheckoutRequestStatus.Pending)
                return BadRequest("Only pending requests can be approved");

            if (request.RequestType != CheckoutRequestType.Checkout)
                return BadRequest("Only checkout requests can be approved");

            bool success = await _repository.ApproveById(id, reviewedByUserId);

            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpPatch("{id:guid}/reject")]
        [Authorize(Policy = "AssetManager+")]
        public async Task<IActionResult> RejectCheckoutRequest(Guid id)
        {
            CheckoutRequest? request = await _repository.GetById(id);
            Guid reviewedByUserId = User.GetUserId();

            if (request == null)
                return NotFound();

            if (request.Status != CheckoutRequestStatus.Pending)
                return BadRequest("Only pending requests can be rejected");

            bool success = await _repository.RejectById(id, reviewedByUserId);

            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpPatch("{id:guid}/assign-asset")]
        [Authorize(Policy = "AssetManager+")]
        public async Task<IActionResult> AssignCheckoutRequest(
            Guid id,
            [FromBody] AssignAssetRequest request)
        {
            CheckoutRequest? checkoutRequest = await _repository.GetById(id);

            if (checkoutRequest == null)
                return NotFound();

            if (checkoutRequest.Status != CheckoutRequestStatus.Approved)
                return BadRequest("Request must be approved");

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
                checkoutRequest.RequestedByUserId);

            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpPatch("{id:guid}/return")]
        [Authorize(Policy = "AssetManager+")]
        public async Task<IActionResult> ReturnCheckoutRequest(Guid id)
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

            bool success = await _repository.ReturnById(
                id,
                request.AssignedAssetId.Value);

            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
