using AssetManagementSystem.DTOs.CheckoutRequests;
using AssetManagementSystem.DTOs.Pagination;
using AssetManagementSystem.Extensions;
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
        private readonly CheckoutRequestService _service;

        public CheckoutRequestsController(CheckoutRequestService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponse<CheckoutRequest>>> Get(
            [FromQuery] GetCheckoutRequestsRequest request)
        {
            bool isManager =
                User.IsInRole("AssetManager") ||
                User.IsInRole("Admin");

            var result = await _service.GetRequests(request, User.GetUserId(), isManager);

            return result.Succeeded ? Ok(result.Value) : ToActionResult(result);
        }

        [HttpPost]
        public async Task<ActionResult<CheckoutRequest>> Create(
            [FromBody] CreateCheckoutRequestRequest request)
        {
            var result = await _service.Create(request, User.GetUserId());
            return result.Succeeded ? Ok(result.Value) : ToActionResult(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CheckoutRequest>> GetDetail(Guid id)
        {
            bool isManager =
                User.IsInRole("AssetManager") ||
                User.IsInRole("Admin");

            var result = await _service.GetDetail(id, User.GetUserId(), isManager);
            return result.Succeeded ? Ok(result.Value) : ToActionResult(result);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Archive(Guid id)
        {
            var result = await _service.Archive(id);
            return result.Succeeded ? NoContent() : ToActionResult(result);
        }

        [HttpPatch("{id:guid}/cancel")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            var result = await _service.Cancel(id, User.GetUserId());
            return result.Succeeded ? NoContent() : ToActionResult(result);
        }

        [HttpPatch("{id:guid}/approve")]
        [Authorize(Policy = "AssetManager+")]
        public async Task<IActionResult> Approve(Guid id)
        {
            var result = await _service.Approve(id, User.GetUserId());
            return result.Succeeded ? NoContent() : ToActionResult(result);
        }

        [HttpPatch("{id:guid}/reject")]
        [Authorize(Policy = "AssetManager+")]
        public async Task<IActionResult> Reject(Guid id)
        {
            var result = await _service.Reject(id, User.GetUserId());
            return result.Succeeded ? NoContent() : ToActionResult(result);
        }

        [HttpPatch("{id:guid}/assign-asset")]
        [Authorize(Policy = "AssetManager+")]
        public async Task<IActionResult> Assign(
            Guid id,
            [FromBody] AssignAssetRequest request)
        {
            var result = await _service.AssignAsset(id, request, User.GetUserId());
            return result.Succeeded ? NoContent() : ToActionResult(result);
        }

        [HttpPatch("{id:guid}/return")]
        [Authorize(Policy = "AssetManager+")]
        public async Task<IActionResult> Return(Guid id)
        {
            var result = await _service.Return(id, User.GetUserId());
            return result.Succeeded ? NoContent() : ToActionResult(result);
        }
    }
}