using ThreatlockerAssetManagementSystem.Enums;

namespace ThreatlockerAssetManagementSystem.Models.Entities
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string EmailAddress { get; set; }
        public required Role Role { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public required string FirstName { get; set; }
        public required string LastName { get; set; }

        public ICollection<Asset> AssignedAssets { get; set; } = [];
        public ICollection<CheckoutRequest> RequestedCheckoutRequests { get; set; } = [];
    }
}
