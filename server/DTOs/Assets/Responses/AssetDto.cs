using AssetManagementSystem.Enums;
using AssetManagementSystem.Models.Entities;

namespace AssetManagementSystem.DTOs.Assets.Responses
{
    public class AssetDto
    {
        public required Guid Id { get; set; }
        public required string AssetTag { get; set; }
        public required string Name { get; set; }
        public required AssetCategory Category { get; set; }
        public required AssetStatus Status { get; set; }
        public required AssetCondition Condition { get; set; }
        public required bool IsArchived { get; set; }
    }
}
