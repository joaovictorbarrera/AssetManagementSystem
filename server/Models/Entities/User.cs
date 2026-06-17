using ThreatlockerAssetManagementSystem.Enums;

namespace ThreatlockerAssetManagementSystem.Models.Entities
{
    public class User
    {
        public required Guid Id { get; set; } = Guid.NewGuid();
        public required string EmailAddress { get; set; }
        public required Role Role { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public required bool Active { get; set; } = true;
        public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
