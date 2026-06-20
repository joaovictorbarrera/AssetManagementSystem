namespace ThreatlockerAssetManagementSystem.DTOs.Pagination
{
    public class PagedResponse<T>
    {
        public List<T> Items { get; set; } = [];
        public PaginationMetadata Pagination { get; set; } = new();
    }
}
