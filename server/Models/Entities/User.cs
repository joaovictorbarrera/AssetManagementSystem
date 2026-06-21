using AssetManagementSystem.Enums;

namespace AssetManagementSystem.Models.Entities
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string EmailAddress { get; set; }
        public required Role Role { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
    }
}
