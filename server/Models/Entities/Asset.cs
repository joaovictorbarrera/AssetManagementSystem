using ThreatlockerAssetManagementSystem.Enums;

namespace ThreatlockerAssetManagementSystem.Models.Entities
{
    public class Asset
    {
        public required Guid Id { get; set; } = Guid.NewGuid();
        public required string AssetTag { get; set; }
        public required string Name { get; set; }
        public required AssetCategory Category { get; set; }
        public string? SerialNumber { get; set; }
        public required AssetStatus Status { get; set; }
        public required AssetCondition Condition { get; set; }
        public Guid? AssignedToUserId { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public required bool IsArchived { get; set; }

        public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
