using AssetManagementSystem.DTOs.CheckoutRequests;
using AssetManagementSystem.DTOs.CheckoutRequests.Requests;
using AssetManagementSystem.DTOs.CheckoutRequests.Responses;
using AssetManagementSystem.DTOs.Pagination;
using AssetManagementSystem.DTOs.Users;
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
        public async Task<ActionResult<PagedResponse<CheckoutRequestDto>>> Get(
            [FromQuery] GetCheckoutRequestsRequest request)
        {
            if (HttpContext.Items["User"] is not User user)
            {
                return BadRequest("User context is missing.");
            }
            Requestor requestor = user.GetRequestor();
            var result = await _requestService.GetRequests(request, requestor);

            return result.Succeeded ? Ok(result.Value) : ToActionResult(result);
        }

        [HttpPost]
        public async Task<ActionResult> Create(
            [FromBody] CreateCheckoutRequestRequest request)
        {
            if (HttpContext.Items["User"] is not User user)
            {
                return BadRequest("User context is missing.");
            }
            Requestor requestor = user.GetRequestor();
            var result = await _requestService.Create(request, requestor);
            return result.Succeeded ? 
                CreatedAtAction(nameof(GetDetail), new {id = result.Value}, null) : 
                ToActionResult(result);
        }

        [HttpGet("fields")]
        public ActionResult<CheckoutRequestFields> GetFields()
        {
            return Ok(new CheckoutRequestFields());
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CheckoutRequestDetail>> GetDetail(Guid id)
        {
            if (HttpContext.Items["User"] is not User user)
            {
                return BadRequest("User context is missing.");
            }
            Requestor requestor = user.GetRequestor();
            var result = await _requestService.GetDetail(id, requestor);
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
            if (HttpContext.Items["User"] is not User user)
            {
                return BadRequest("User context is missing.");
            }
            Requestor requestor = user.GetRequestor();
            var result = await _requestService.Cancel(id, requestor);
            return result.Succeeded ? NoContent() : ToActionResult(result);
        }

        [HttpPatch("{id:guid}/approve")]
        [Authorize(Policy = "AssetManager+")]
        public async Task<IActionResult> Approve(Guid id)
        {
            if (HttpContext.Items["User"] is not User user)
            {
                return BadRequest("User context is missing.");
            }
            Requestor requestor = user.GetRequestor();
            var result = await _requestService.Approve(id, requestor);
            return result.Succeeded ? NoContent() : ToActionResult(result);
        }

        [HttpPatch("{id:guid}/reject")]
        [Authorize(Policy = "AssetManager+")]
        public async Task<IActionResult> Reject(Guid id)
        {
            if (HttpContext.Items["User"] is not User user)
            {
                return BadRequest("User context is missing.");
            }
            Requestor requestor = user.GetRequestor();
            var result = await _requestService.Reject(id, requestor);
            return result.Succeeded ? NoContent() : ToActionResult(result);
        }

        [HttpPatch("{id:guid}/assign-asset")]
        [Authorize(Policy = "AssetManager+")]
        public async Task<IActionResult> Assign(
            Guid id,
            [FromBody] AssignAssetRequest request)
        {
            if (HttpContext.Items["User"] is not User user)
            {
                return BadRequest("User context is missing.");
            }
            Requestor requestor = user.GetRequestor();
            var result = await _requestService.AssignAsset(id, request, requestor);
            return result.Succeeded ? NoContent() : ToActionResult(result);
        }

        [HttpPatch("{id:guid}/return")]
        [Authorize(Policy = "AssetManager+")]
        public async Task<IActionResult> Return(Guid id)
        {
            if (HttpContext.Items["User"] is not User user)
            {
                return BadRequest("User context is missing.");
            }
            Requestor requestor = user.GetRequestor();
            var result = await _requestService.Return(id, requestor);
            return result.Succeeded ? NoContent() : ToActionResult(result);
        }
    }
}