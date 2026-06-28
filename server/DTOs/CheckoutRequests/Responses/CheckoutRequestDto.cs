using AssetManagementSystem.DTOs.Assets.Responses;
using AssetManagementSystem.Enums;
using AssetManagementSystem.Models.Entities;

namespace AssetManagementSystem.DTOs.CheckoutRequests
{
    public class CheckoutRequestDto
    {
        public Guid Id { get; set; }
        public required CheckoutRequestType RequestType { get; set; }

        public required Guid RequestedByUserId { get; set; }
        public User RequestedByUser { get; set; } = null!;

        public required CheckoutRequestStatus Status { get; set; }

        public Guid? AssignedAssetId { get; set; }
        public AssetCategory AssetCategory { get; set; }
        public string? AssignedAssetName { get; set; }
        public string? AssignedAssetTag { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsArchived { get; set; } = false;
    }
}
