using AssetManagementSystem.Enums;

namespace AssetManagementSystem.DTOs.Assets.Requests
{
    public class UpdateAssetConditionRequest
    {
        public required AssetCondition Condition { get; set; }
    }
}
