using System;

namespace AssetManagementSystem.Helpers
{
    public static class TextHelper
    {
        public static string ToCamelCase(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            if (s.Length == 1)
            {
                return s.ToLowerInvariant();
            }

            return char.ToLowerInvariant(s[0]) + s.Substring(1);
        }
    }
}       