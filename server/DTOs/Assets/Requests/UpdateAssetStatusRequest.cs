using AssetManagementSystem.Enums;

namespace AssetManagementSystem.DTOs.Assets.Requests
{
    public class UpdateAssetStatusRequest
    {
        public required AssetStatus Status { get; set; }
    }
}
