using ThreatlockerAssetManagementSystem.Enums;

namespace ThreatlockerAssetManagementSystem.Models.Entities
{
    public class CheckoutRequest
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required CheckoutRequestType RequestType { get; set; }

        public required Guid RequestedByUserId { get; set; }
        public User RequestedByUser { get; set; } = null!;

        public required AssetCategory AssetCategory { get; set; }
        public required string Reason { get; set; }
        public required CheckoutRequestStatus Status { get; set; }

        public Guid? ReviewedByUserId  { get; set; }
        public User? ReviewedByUser { get; set; }

        public Guid? AssignedAssetId { get; set; }
        public Asset? AssignedAsset { get; set; }

        public DateTime? ApprovedAt { get; set; }
        public DateTime? RejectedAt { get; set; }
        public DateTime? FulfilledAt { get; set; }
        public DateTime? ReturnedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsArchived { get; set; } = false;
    }
}