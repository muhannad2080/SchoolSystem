using System;
using System.Collections.Generic;
using System.Linq;
using SchoolSystem.Models;

namespace SchoolSystem.Security
{
    public static class CurrentUser
    {
        public static User User { get; private set; }

        public static bool IsLoggedIn
        {
            get { return User != null; }
        }

        public static void Set(User user)
        {
            User = user;
        }

        public static void Clear()
        {
            User = null;
        }

        public static bool IsAdmin()
        {
            return User != null
                && string.Equals((User.RoleName ?? "").Trim(), "مدير النظام", StringComparison.OrdinalIgnoreCase);
        }

        public static bool HasPermission(string permissionKey)
        {
            if (User == null || !User.IsActive || string.IsNullOrWhiteSpace(permissionKey))
                return false;

            if (IsAdmin())
                return true;

            HashSet<string> permissions = ParsePermissions(User.Permissions);
            return permissions.Contains(permissionKey.Trim());
        }

        public static bool HasAny(params string[] permissionKeys)
        {
            if (permissionKeys == null || permissionKeys.Length == 0)
                return false;

            foreach (string permission in permissionKeys)
            {
                if (HasPermission(permission))
                    return true;
            }

            return false;
        }

        private static HashSet<string> ParsePermissions(string permissions)
        {
            if (string.IsNullOrWhiteSpace(permissions))
                return new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            return new HashSet<string>(
                permissions
                    .Split(new[] { ',', ';', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(p => p.Trim())
                    .Where(p => !string.IsNullOrWhiteSpace(p)),
                StringComparer.OrdinalIgnoreCase);
        }
    }
}
