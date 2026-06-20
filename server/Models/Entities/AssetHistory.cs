namespace ThreatlockerAssetManagementSystem.Models.Entities
{
    public class AssetHistory
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public required Guid AssetId { get; set; }
        public Asset Asset { get; set; } = null!;

        public required Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public required string Action { get; set; }
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}