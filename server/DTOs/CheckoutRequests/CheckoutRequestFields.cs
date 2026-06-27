using System;
using System.Linq;
using System.Collections.Generic;
using AssetManagementSystem.Enums;
using AssetManagementSystem.Helpers;

namespace AssetManagementSystem.DTOs.CheckoutRequests
{
    public class CheckoutRequestFields
    {
        public List<string> Statuses { get; set; } = [.. Enum.GetNames(typeof(CheckoutRequestStatus)).Select(s => TextHelper.ToCamelCase(s))];
        public List<string> Types { get; set; } = [.. Enum.GetNames(typeof(CheckoutRequestType)).Select(t => TextHelper.ToCamelCase(t))];
    }
}
