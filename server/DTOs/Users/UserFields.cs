using System;
using System.Linq;
using System.Collections.Generic;
using AssetManagementSystem.Enums;
using AssetManagementSystem.Helpers;

namespace AssetManagementSystem.DTOs.Users
{
    public class UserFields
    {
        public List<string> Roles { get; } = Enum.GetNames(typeof(Role))
            .Select(t => TextHelper.ToCamelCase(t))
            .ToList();
    }
}
