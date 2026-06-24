using AssetManagementSystem.DTOs.CheckoutRequests;
using AssetManagementSystem.DTOs.Pagination;
using AssetManagementSystem.Extensions;
using AssetManagementSystem.Helpers;
using AssetManagementSystem.Models.Entities;
using AssetManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagementSystem.Controllers
{
    [Authorize]
    [Route("api/checkout-requests")]
    [ApiController]
    public class CheckoutRequestsController : ApiControllerBase
    {
        private readonly CheckoutRequestService _requestService;

        public CheckoutRequestsController(CheckoutRequestService requestService)
        {
            _requestService = requestService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponse<CheckoutRequest>>> Get(
            [FromQuery] GetCheckoutRequestsRequest request)
        {
            var result = await _requestService.GetRequests(request, User.GetUserId(), RolesHelper.IsAssetManager(User));

            return result.Succeeded ? Ok(result.Value) : ToActionResult(result);
        }

        [HttpPost]
        public async Task<ActionResult<CheckoutRequest>> Create(
            [FromBody] CreateCheckoutRequestRequest request)
        {
            var result = await _requestService.Create(request, User.GetUserId());
            return result.Succeeded ? Ok(result.Value) : ToActionResult(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CheckoutRequest>> GetDetail(Guid id)
        {
            var result = await _requestService.GetDetail(id, User.GetUserId(), RolesHelper.IsAssetManager(User));
            return result.Succeeded ? Ok(result.Value) : ToActionResult(result);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Archive(Guid id)
        {
            var result = await _requestService.Archive(id);
            return result.Succeeded ? NoContent() : ToActionResult(result);
        }

        [HttpPatch("{id:guid}/cancel")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            var result = await _requestService.Cancel(id, User.GetUserId());
            return result.Succeeded ? NoContent() : ToActionResult(result);
        }

        [HttpPatch("{id:guid}/approve")]
        [Authorize(Policy = "AssetManager+")]
        public async Task<IActionResult> Approve(Guid id)
        {
            var result = await _requestService.Approve(id, User.GetUserId());
            return result.Succeeded ? NoContent() : ToActionResult(result);
        }

        [HttpPatch("{id:guid}/reject")]
        [Authorize(Policy = "AssetManager+")]
        public async Task<IActionResult> Reject(Guid id)
        {
            var result = await _requestService.Reject(id, User.GetUserId());
            return result.Succeeded ? NoContent() : ToActionResult(result);
        }

        [HttpPatch("{id:guid}/assign-asset")]
        [Authorize(Policy = "AssetManager+")]
        public async Task<IActionResult> Assign(
            Guid id,
            [FromBody] AssignAssetRequest request)
        {
            var result = await _requestService.AssignAsset(id, request, User.GetUserId());
            return result.Succeeded ? NoContent() : ToActionResult(result);
        }

        [HttpPatch("{id:guid}/return")]
        [Authorize(Policy = "AssetManager+")]
        public async Task<IActionResult> Return(Guid id)
        {
            var result = await _requestService.Return(id, User.GetUserId());
            return result.Succeeded ? NoContent() : ToActionResult(result);
        }
    }
}