using AssetManagementSystem.DTOs.Pagination;
using AssetManagementSystem.Enums;

namespace AssetManagementSystem.DTOs.CheckoutRequests.Requests
{
    public class GetCheckoutRequestsRequest : PaginatedRequest
    {
        public CheckoutRequestType? Type { get; set; }
        public AssetCategory? AssetCategory { get; set; }
        public CheckoutRequestStatus? Status { get; set; }
        public bool Review { get; set; }
        public bool IncludeClosedRequests { get; set; }
    }
}
