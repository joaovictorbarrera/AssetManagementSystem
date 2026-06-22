using AssetManagementSystem.Enums;

namespace AssetManagementSystem.DTOs.Assets.Requests
{
    public class GetAvailableAssetsRequest
    {
        public required AssetCategory Category { get; set; }
    }
}
