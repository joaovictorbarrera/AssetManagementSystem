namespace ThreatlockerAssetManagementSystem.DTOs.Users
{
    public class GetUsersRequest
    {
        public bool HideInactive { get; set; } = false;

        public int PageSize { get; set; } = 25;

        public int PageNumber { get; set; } = 1;
    }
}
