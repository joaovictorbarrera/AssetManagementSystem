using AssetManagementSystem.Models.Entities;
using System.Linq.Expressions;

namespace AssetManagementSystem.DTOs.CheckoutRequests.Projections
{
    public static class CheckoutRequestExpressions
    {
        public static Expression<Func<CheckoutRequest, CheckoutRequestDto>> ToDto =>
            r => new CheckoutRequestDto
            {
                Id = r.Id,
                RequestType = r.RequestType,
                RequestedByUserId = r.RequestedByUserId,
                RequestedByUser = r.RequestedByUser,
                Status = r.Status,
                AssignedAssetId = r.AssignedAssetId,
                IsArchived = r.IsArchived,
                CreatedAt = r.CreatedAt,
                AssetCategory = r.AssetCategory,
                AssignedAssetName = r.AssignedAsset == null ? null : r.AssignedAsset.Name,
                AssignedAssetTag = r.AssignedAsset == null ? null : r.AssignedAsset.AssetTag,
            };

        public static Expression<Func<CheckoutRequest, CheckoutRequestDetail>> ToDetail =>
            r => new CheckoutRequestDetail
            {
                Id = r.Id,
                RequestType = r.RequestType,
                RequestedByUser = r.RequestedByUser,
                Reason = r.Reason,
                Status = r.Status,
                ReviewedByUser = r.ReviewedByUser,
                AssetCategory = r.AssetCategory,
                AssignedAssetId = r.AssignedAssetId,
                AssignedAssetName = r.AssignedAsset == null ? null : r.AssignedAsset.Name,
                AssignedAssetTag = r.AssignedAsset == null ? null : r.AssignedAsset.AssetTag,
                ApprovedAt = r.ApprovedAt,
                RejectedAt = r.RejectedAt,
                FulfilledAt = r.FulfilledAt,
                ReturnedAt = r.ReturnedAt,
                UpdatedAt = r.UpdatedAt,
                CreatedAt = r.CreatedAt,
                IsArchived = r.IsArchived,
            };
    }
}
