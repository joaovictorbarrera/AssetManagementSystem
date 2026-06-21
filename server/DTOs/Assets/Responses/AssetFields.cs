using AssetManagementSystem.Enums;

namespace AssetManagementSystem.DTOs.Assets.Responses
{
    public class AssetFields
    {
        public List<string> Conditions { get; set; } = [.. Enum.GetNames(typeof(AssetCondition))];
        public List<string> Statuses { get; set; } = [.. Enum.GetNames(typeof(AssetStatus))];
        public List<string> Categories { get; set; } = [.. Enum.GetNames(typeof(AssetCategory))];
    }
}
