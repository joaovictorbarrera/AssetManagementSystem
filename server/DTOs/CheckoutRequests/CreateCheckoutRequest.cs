using AssetManagementSystem.Enums;

namespace AssetManagementSystem.DTOs.CheckoutRequests
{
    public class CreateCheckoutRequestRequest
    {
        public required CheckoutRequestType RequestType { get; set; }

        public AssetCategory? AssetCategory { get; set; }

        public Guid? AssetId { get; set; }

        public required string Reason { get; set; }
    }
}
