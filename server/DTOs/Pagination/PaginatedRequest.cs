using System.ComponentModel.DataAnnotations;

namespace AssetManagementSystem.DTOs.Pagination
{
    public class PaginatedRequest
    {
        [Range(1, 500)]
        public int PageSize { get; set; } = 25;

        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; } = 1;
    }
}
