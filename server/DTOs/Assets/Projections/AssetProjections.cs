using AssetManagementSystem.DTOs.Assets.Responses;
using AssetManagementSystem.Enums;
using AssetManagementSystem.Models.Entities;
using System.Linq.Expressions;

namespace AssetManagementSystem.DTOs.Assets.Projections
{
    public static class AssetExpressions
    {
        public static Expression<Func<Asset, AssetDto>> ToDto =>
            a => new AssetDto
            {
                Id = a.Id,
                AssetTag = a.AssetTag,
                Name = a.Name,
                Category = a.Category,
                Status = a.Status,
                Condition = a.Condition,
                UserId = a.AssignedToUserId,
                UserFirstName = a.AssignedToUser != null ? a.AssignedToUser.FirstName : null,
                UserLastName = a.AssignedToUser != null ? a.AssignedToUser.LastName : null,
                IsArchived = a.IsArchived,
                IsPendingReturn = a.Requests.Any(r => r.RequestType == CheckoutRequestType.Return &&
                                                      !r.IsArchived &&
                                                      r.Status == CheckoutRequestStatus.Pending)
            };

        public static Expression<Func<Asset, AssetDetail>> ToDetail =>
            a => new AssetDetail
            {
                Id = a.Id,
                AssetTag = a.AssetTag,
                Name = a.Name,
                Category = a.Category,
                Status = a.Status,
                Condition = a.Condition,
                UserId = a.AssignedToUserId,
                UserFirstName = a.AssignedToUser != null ? a.AssignedToUser.FirstName : null,
                UserLastName = a.AssignedToUser != null ? a.AssignedToUser.LastName : null,
                UpdatedAt = a.UpdatedAt,
                CreatedAt = a.CreatedAt,
                IsArchived = a.IsArchived,
                IsPendingReturn = a.Requests.Any(r => r.RequestType == CheckoutRequestType.Return &&
                                                      !r.IsArchived &&
                                                      r.Status == CheckoutRequestStatus.Pending)
            };
    }
}
