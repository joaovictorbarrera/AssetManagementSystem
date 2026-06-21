using AssetManagementSystem.DTOs.Pagination;
using System.ComponentModel.DataAnnotations;

namespace AssetManagementSystem.DTOs.Users
{
    public class GetUsersRequest : PaginatedRequest
    {
        public bool HideInactive { get; set; } = false;
    }
}
