using System;
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
            return User != null && User.RoleName == "مدير النظام";
        }

        public static bool HasPermission(string permissionKey)
        {
            if (User == null)
                return false;

            if (!User.IsActive)
                return false;

            if (IsAdmin())
                return true;

            if (string.IsNullOrWhiteSpace(User.Permissions))
                return false;

            string[] permissions = User.Permissions
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(p => p.Trim())
                .ToArray();

            return permissions.Contains(permissionKey);
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
    }
}
