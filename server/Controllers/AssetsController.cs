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
        public async Task<ActionResult<PagedResponse<AssetDto>>> Get([FromQuery] GetAssetsRequest request)
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
        public async Task<ActionResult<AssetDto>> Create(CreateAssetRequest request)
        {
            if (await _assetRepository.IsTagTakenAndNotId(request.AssetTag, Guid.Empty))
                return BadRequest("Asset Tag is taken");

            if (!Enum.IsDefined(typeof(AssetStatus), request.Status))
                return BadRequest("Invalid Status");

            if (!Enum.IsDefined(typeof(AssetCondition), request.Condition))
                return BadRequest("Invalid Condition");

            if (!Enum.IsDefined(typeof(AssetCategory), request.Category))
                return BadRequest("Invalid Category");

            AssetDto asset = await _assetRepository.CreateAsset(request, User.GetUserId());

            return Ok(asset);
        }

        [HttpGet("available")]
        [Authorize(Policy = "AssetManager+")]
        public async Task<ActionResult<List<AvailableAsset>>> GetAvailable(
            [FromQuery] GetAvailableAssetsRequest request
        ){
            if (!Enum.IsDefined(typeof(AssetCategory), request.Category))
                return BadRequest("Invalid Category");

            List<AvailableAsset> availableAssets = await _assetRepository.GetAvailableByCategory(request.Category);
            return Ok(availableAssets);
        }

        [HttpGet("fields")]
        [Authorize]
        public ActionResult<AssetFields> GetFields()
        {
            return Ok(new AssetFields());
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Asset>> GetDetail(Guid id)
        {
            Asset? asset = await _assetRepository.GetById(id);
            if (asset == null) return NotFound();

            return Ok(asset);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Policy = "AssetManager+")]
        public async Task<ActionResult<Asset>> Update(Guid id, [FromBody] UpdateAssetRequest request)
        {
            if (await _assetRepository.IsTagTakenAndNotId(request.AssetTag, id))
            {
                return BadRequest("Asset Tag is taken");
            }

            Asset? existingAsset = await _assetRepository.GetById(id);
            if (existingAsset == null) return NotFound();

            if (existingAsset.IsArchived)
                return BadRequest("Cannot update archived assets");

            Asset? asset = await _assetRepository.UpdateById(id, request, User.GetUserId());
            if (asset == null) return NotFound();

            return Ok(asset);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Archive(Guid id)
        {
            bool success = await _assetRepository.ArchiveById(id, User.GetUserId());
            if (!success) return NotFound();

            return NoContent();
        }

        [HttpPatch("{id:guid}/status")]
        [Authorize(Policy = "AssetManager+")]
        public async Task<IActionResult> UpdateStatus(
            Guid id,
            [FromBody] UpdateAssetStatusRequest request)
        {
            if (!Enum.IsDefined(typeof(AssetStatus), request.Status))
                return BadRequest("Invalid status");

            Asset? existingAsset = await _assetRepository.GetById(id);
            if (existingAsset == null) return NotFound();

            if (existingAsset.IsArchived)
                return BadRequest("Cannot update archived assets");

            if (existingAsset.AssignedToUserId != null)
                return BadRequest("Cannot update status of an assigned asset");

            bool success = await _assetRepository.UpdateAssetStatus(id, request.Status, User.GetUserId());

            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpPatch("{id:guid}/condition")]
        [Authorize(Policy = "AssetManager+")]
        public async Task<IActionResult> UpdateCondition (
            Guid id,
            [FromBody] UpdateAssetConditionRequest request)
        {
            if (!Enum.IsDefined(typeof(AssetCondition), request.Condition))
                return BadRequest("Invalid condition");

            Asset? existingAsset = await _assetRepository.GetById(id);
            if (existingAsset == null) return NotFound();

            if (existingAsset.IsArchived)
                return BadRequest("Cannot update archived assets");

            bool success = await _assetRepository.UpdateAssetCondition(id, request.Condition, User.GetUserId());

            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpGet("{id:guid}/history")]
        [Authorize(Policy = "AssetManager+")]
        public async Task<ActionResult<List<AssetHistory>>> GetHistory(Guid id)
        {
            List<AssetHistory> history = await _assetRepository.GetAssetHistory(id);

            return Ok(history);
        }
    }
}