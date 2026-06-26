using AssetManagementSystem.Enums;
using AssetManagementSystem.Models.Entities;
using System.Security.Claims;

namespace AssetManagementSystem.Helpers
{
    public class RolesHelper
    {
        public static bool IsAssetManager(Role role)
        {
            return role == Role.AssetManager || role == Role.Admin;
        }
    }
}
