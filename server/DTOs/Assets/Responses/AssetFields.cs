using AssetManagementSystem.Enums;
using AssetManagementSystem.Helpers;

namespace AssetManagementSystem.DTOs.Assets.Responses
{   
    public class AssetFields
    {
        public List<string> Conditions { get; } = [.. Enum.GetNames(typeof(AssetCondition)).Select(t => TextHelper.ToCamelCase(t))];

        public List<string> Statuses { get; } = [.. Enum.GetNames(typeof(AssetStatus)).Select(t => TextHelper.ToCamelCase(t))];

        public List<string> Categories { get; } = [.. Enum.GetNames(typeof(AssetCategory)).Select(t => TextHelper.ToCamelCase(t))];
    }
}
