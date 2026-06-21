using AssetManagementSystem.Enums;

namespace AssetManagementSystem.DTOs.Assets.Requests
{
    public class GetAvailableAssetsByCategoryRequest
    {
        public required AssetCategory Category { get; set; }
    }
}
