using System.ComponentModel.DataAnnotations;

namespace ThreatlockerAssetManagementSystem.DTOs.Users
{
    public class GetUsersRequest
    {
        public bool HideInactive { get; set; } = false;

        [Range(1, 500)]
        public int PageSize { get; set; } = 25;

        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; } = 1;
    }
}
