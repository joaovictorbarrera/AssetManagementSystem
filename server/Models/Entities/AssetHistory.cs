namespace ThreatlockerAssetManagementSystem.Models.Entities
{
    public class AssetHistory
    {
        public required Guid Id { get; set; } = Guid.NewGuid();
        public required Guid AssetId { get; set; }
        public required Guid UserId { get; set; }
        public required string Action { get; set; }
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}