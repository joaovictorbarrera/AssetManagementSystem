using AssetManagementSystem.Enums;
using System.ComponentModel.DataAnnotations;

namespace AssetManagementSystem.DTOs.CheckoutRequests
{
    public class CreateCheckoutRequestRequest
    {
        public required CheckoutRequestType RequestType { get; set; }

        public AssetCategory? AssetCategory { get; set; }

        public Guid? AssetId { get; set; }

        [MaxLength(500)]
        public required string Reason { get; set; }
    }
}
