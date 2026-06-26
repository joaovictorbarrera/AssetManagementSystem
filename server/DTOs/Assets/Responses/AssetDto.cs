using AssetManagementSystem.Enums;
using AssetManagementSystem.Models.Entities;

namespace AssetManagementSystem.DTOs.Assets.Responses
{
    public class AssetDto
    {
        public Guid Id { get; set; }

        public required string AssetTag { get; set; }
        public required string Name { get; set; }
        public string? SerialNumber { get; set; }

        public required AssetCategory Category { get; set; }
        public required AssetStatus Status { get; set; }
        public required AssetCondition Condition { get; set; }

        public Guid? AssignedToUserId { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public bool IsArchived { get; set; } = false;
        public bool IsPendingReturn { get; set; } = false;
    }
}
