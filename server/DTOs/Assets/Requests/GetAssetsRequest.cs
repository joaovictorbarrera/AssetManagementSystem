using AssetManagementSystem.DTOs.Pagination;
using AssetManagementSystem.Enums;
using System.ComponentModel.DataAnnotations;

namespace AssetManagementSystem.DTOs.Assets.Requests
{
    public class GetAssetsRequest : PaginatedRequest
    {
        [MaxLength(50)]
        public string SearchText { get; set; } = "";
        public AssetStatus? Status { get; set; } = null;

        // ALL below are protected by Manager+ Role
        public bool Inventory { get; set; } = false;
        public bool ViewArchived { get; set; } = false;
    }
}
