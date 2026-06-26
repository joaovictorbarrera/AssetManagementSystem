namespace AssetManagementSystem.DTOs.Users
{
    public class Requestor
    {
        public required Guid UserId { get; set; }
        public bool IsAssetManager { get; set; } = false;
    }
}
