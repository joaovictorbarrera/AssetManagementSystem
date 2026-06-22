using AssetManagementSystem.DTOs.Assets.Requests;
using AssetManagementSystem.DTOs.Assets.Responses;
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
    [Route("api/assets")]
    [ApiController]
    public class AssetsController : ApiControllerBase
    {
        private readonly AssetService _service;

        public AssetsController(AssetService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponse<Asset>>> Get(
            [FromQuery] GetAssetsRequest request)
        {
            var result = await _service.GetAssets(request, User.GetUserId(), RolesHelper.IsAssetManager(User));
            return result.Succeeded ? Ok(result.Value) : ToActionResult(result);
        }

        [HttpPost]
        [Authorize(Policy = "AssetManager+")]
        public async Task<ActionResult<Asset>> Create([FromBody] CreateAssetRequest request)
        {
            var result = await _service.Create(request, User.GetUserId());
            return result.Succeeded ? Ok(result.Value) : ToActionResult(result);
        }

        [HttpGet("available")]
        [Authorize(Policy = "AssetManager+")]
        public async Task<ActionResult<List<AvailableAsset>>> GetAvailable(
            [FromQuery] GetAvailableAssetsRequest request)
        {
            var result = await _service.GetAvailableByCategory(request);
            return result.Succeeded ? Ok(result.Value) : ToActionResult(result);
        }

        [HttpGet("fields")]
        public ActionResult<AssetFields> GetFields()
        {
            return Ok(new AssetFields());
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Asset>> GetDetail(Guid id)
        {
            var result = await _service.GetDetail(id, User.GetUserId(), RolesHelper.IsAssetManager(User));
            return result.Succeeded ? Ok(result.Value) : ToActionResult(result);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Policy = "AssetManager+")]
        public async Task<ActionResult<Asset>> Update(
            Guid id,
            [FromBody] UpdateAssetRequest request)
        {
            var result = await _service.Update(id, request, User.GetUserId());
            return result.Succeeded ? Ok(result.Value) : ToActionResult(result);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Archive(Guid id)
        {
            var result = await _service.Archive(id, User.GetUserId());
            return result.Succeeded ? NoContent() : ToActionResult(result);
        }

        [HttpPatch("{id:guid}/status")]
        [Authorize(Policy = "AssetManager+")]
        public async Task<IActionResult> UpdateStatus(
            Guid id,
            [FromBody] UpdateAssetStatusRequest request)
        {
            var result = await _service.UpdateStatus(id, request, User.GetUserId());
            return result.Succeeded ? NoContent() : ToActionResult(result);
        }

        [HttpPatch("{id:guid}/condition")]
        [Authorize(Policy = "AssetManager+")]
        public async Task<IActionResult> UpdateCondition(
            Guid id,
            [FromBody] UpdateAssetConditionRequest request)
        {
            var result = await _service.UpdateCondition(id, request, User.GetUserId());
            return result.Succeeded ? NoContent() : ToActionResult(result);
        }

        [HttpGet("{id:guid}/history")]
        [Authorize(Policy = "AssetManager+")]
        public async Task<ActionResult<List<AssetHistory>>> GetHistory(Guid id)
        {
            var result = await _service.GetHistory(id);
            return result.Succeeded ? Ok(result.Value) : ToActionResult(result);
        }
    }
}