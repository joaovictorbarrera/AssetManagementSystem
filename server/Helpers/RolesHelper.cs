using AssetManagementSystem.Models.Entities;
using System.Security.Claims;

namespace AssetManagementSystem.Helpers
{
    public class RolesHelper
    {
        public static bool IsAssetManager(ClaimsPrincipal user)
        {
            return user.IsInRole("AssetManager") || user.IsInRole("Admin");
        }
    }
}
