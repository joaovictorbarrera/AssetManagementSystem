using AssetManagementSystem.DTOs.Assets.Responses;
using AssetManagementSystem.Enums;
using AssetManagementSystem.Models.Entities;

namespace AssetManagementSystem.DTOs.CheckoutRequests
{
    public class CheckoutRequestDetail
    {
        public Guid Id { get; set; }
        public required CheckoutRequestType RequestType { get; set; }

        public User RequestedByUser { get; set; } = null!;

        public required string Reason { get; set; }
        public required CheckoutRequestStatus Status { get; set; }

        public User? ReviewedByUser { get; set; }

        public Guid? AssignedAssetId { get; set; }
        public AssetCategory AssetCategory { get; set; } 
        public string? AssignedAssetName { get; set; }
        public string? AssignedAssetTag { get; set; }

        public DateTime? ApprovedAt { get; set; }
        public DateTime? RejectedAt { get; set; }
        public DateTime? FulfilledAt { get; set; }
        public DateTime? ReturnedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsArchived { get; set; }
    }
}
