using AssetManagementSystem.DTOs.Pagination;

namespace AssetManagementSystem.DTOs.CheckoutRequests
{
    public class GetCheckoutRequestsRequest : PaginatedRequest
    {
        public bool Review { get; set; }
        public bool IncludeClosedRequests { get; set; }
    }
}
