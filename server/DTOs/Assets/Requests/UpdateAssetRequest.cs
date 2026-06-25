using AssetManagementSystem.Enums;
using AssetManagementSystem.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace AssetManagementSystem.DTOs.Assets.Requests
{
    public class UpdateAssetRequest
    {
        [MaxLength(50)]
        [MinLength(1)]
        public required string AssetTag { get; set; }

        [MaxLength(50)]
        [MinLength(1)]
        public required string Name { get; set; }

        [MaxLength(50)]
        [MinLength(1)]
        public string? SerialNumber { get; set; }

        public required AssetCategory Category { get; set; }
        public required AssetStatus Status { get; set; }
        public required AssetCondition Condition { get; set; }
    }
}
