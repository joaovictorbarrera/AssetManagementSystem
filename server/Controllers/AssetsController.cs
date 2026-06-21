using AssetManagementSystem.DTOs.Assets.Requests;
using AssetManagementSystem.DTOs.Assets.Responses;
using AssetManagementSystem.DTOs.Pagination;
using AssetManagementSystem.Enums;
using AssetManagementSystem.Extensions;
using AssetManagementSystem.Models.Entities;
using AssetManagementSystem.Models.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagementSystem.Controllers
{
    [Authorize]
    [Route("api/assets")]
    [ApiController]
    public class AssetsController : ControllerBase
    {
        private readonly AssetRepository _assetRepository;

        public AssetsController(AssetRepository assetRepository)
        {
            _assetRepository = assetRepository;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponse<AssetDto>>> GetAssets([FromQuery] GetAssetsRequest request)
        {
            bool usingManagerFeatures = request.Inventory || request.ViewArchived;

            // TODO: Can I use policy instead?
            bool isManager = User.IsInRole("AssetManager") || User.IsInRole("Admin");

            if (usingManagerFeatures && !isManager)
            {
                return Forbid();
            }

            Guid requestorId = User.GetUserId();

            return Ok(await _assetRepository.GetAssets(request, requestorId));
        }

        [HttpPost]
        [Authorize(Policy = "AssetManager+")]
        public async Task<ActionResult<AssetDto>> CreateAsset(CreateAssetRequest request)
        {
            bool assetTagExists = await _assetRepository.GetByAssetTag(request.AssetTag) != null;
            if (assetTagExists)
            {
                return BadRequest("Asset Tag is taken");
            }

            if (!Enum.IsDefined(typeof(AssetStatus), request.Status))
                return BadRequest("Invalid Status");

            if (!Enum.IsDefined(typeof(AssetCondition), request.Condition))
                return BadRequest("Invalid Condition");

            if (!Enum.IsDefined(typeof(AssetCategory), request.Category))
                return BadRequest("Invalid Category");

            AssetDto asset = await _assetRepository.CreateAsset(request);

            return Ok(asset);
        }

        [HttpGet("available")]
        [Authorize(Policy = "AssetManager+")]
        public async Task<ActionResult<List<AvailableAsset>>> GetAvailableAssetsByCategory(
            [FromQuery] GetAvailableAssetsByCategoryRequest request
        ){
            Console.WriteLine(request.Category);
            if (!Enum.IsDefined(typeof(AssetCategory), request.Category))
                return BadRequest("Invalid Category");

            List<AvailableAsset> availableAssets = await _assetRepository.GetAvailableAssetsByCategory(request);
            return Ok(availableAssets);
        }

        [HttpGet("fields")]
        [Authorize]
        public ActionResult<AssetFields> GetAssetFields()
        {
            return Ok(new AssetFields());
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Asset>> GetAssetDetail(Guid id)
        {
            Asset? asset = await _assetRepository.GetById(id);
            if (asset == null) return NotFound();

            return Ok(asset);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Policy = "AssetManager+")]
        public async Task<IActionResult> UpdateAsset(Guid id, [FromBody] UpdateAssetRequest request)
        {
            Asset? existingAsset = await _assetRepository.GetByAssetTag(request.AssetTag);
            if (existingAsset != null && request.AssetTag != request.AssetTag)
            {
                return BadRequest("Asset Tag is taken");
            }

            Asset? asset = await _assetRepository.UpdateById(id, request);
            if (asset == null) return NotFound();

            return Ok(asset);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ArchiveAsset(Guid id)
        {
            bool success = await _assetRepository.ArchiveById(id);
            if (!success) return NotFound();

            return NoContent();
        }

        [HttpPatch("{id:guid}/status")]
        [Authorize(Policy = "AssetManager+")]
        public async Task<IActionResult> UpdateAssetStatus(
            Guid id,
            [FromBody] UpdateAssetStatusRequest request)
        {
            if (!Enum.IsDefined(typeof(AssetStatus), request.Status))
                return BadRequest("Invalid status");

            bool success = await _assetRepository.UpdateAssetStatus(id, request.Status);

            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpPatch("{id:guid}/condition")]
        [Authorize(Policy = "AssetManager+")]
        public async Task<IActionResult> UpdateAssetCondition(
            Guid id,
            [FromBody] UpdateAssetConditionRequest request)
        {
            if (!Enum.IsDefined(typeof(AssetCondition), request.Condition))
                return BadRequest("Invalid condition");

            bool success = await _assetRepository.UpdateAssetCondition(id, request.Condition);

            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpGet("{id:guid}/history")]
        [Authorize(Policy = "AssetManager+")]
        public async Task<IActionResult> GetAssetHistory(Guid id)
        {
            List<AssetHistory> history = await _assetRepository.GetAssetHistory(id);

            return Ok(history);
        }
    }
}