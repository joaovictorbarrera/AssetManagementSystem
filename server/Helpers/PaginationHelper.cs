using Azure.Core;

namespace AssetManagementSystem.Helpers
{
    public static class PaginationHelper
    {
        public static int GetTotalPageCount(int totalCount, int pageSize)
        {
            return (int)Math.Ceiling((double)totalCount / pageSize);
        }
    }
}
