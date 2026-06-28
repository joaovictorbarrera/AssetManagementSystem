using AssetManagementSystem.Enums;

namespace AssetManagementSystem.DTOs.Assets.Responses
{
    public class AssetDetail
    {
        public Guid Id { get; set; }

        public required string AssetTag { get; set; }
        public required string Name { get; set; }
        public string? SerialNumber { get; set; }
        public required AssetCategory Category { get; set; }
        public required AssetStatus Status { get; set; }
        public required AssetCondition Condition { get; set; }
        public Guid? UserId { get; set; }
        public string? UserFirstName { get; set; }
        public string? UserLastName { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsPendingReturn { get; set; } = false;
        public bool IsArchived { get; set; } = false;
    }
}
