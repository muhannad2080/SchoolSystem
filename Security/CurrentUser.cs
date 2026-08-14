using System;
using System.Collections.Generic;
using System.Linq;
using SchoolSystem.Models;

namespace SchoolSystem.Security
{
    public static class CurrentUser
    {
        private static readonly object SyncRoot = new object();

        public static User User { get; private set; }

        public static bool IsLoggedIn
        {
            get { return User != null; }
        }

        public static void Set(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (!user.IsActive)
                throw new InvalidOperationException("لا يمكن إنشاء جلسة لحساب غير فعال.");

            user.RoleName = PermissionKeys.NormalizeRoleName(user.RoleName);
            user.Permissions = PermissionKeys.NormalizePermissions(user.Permissions);

            lock (SyncRoot)
            {
                User = user;
            }
        }

        public static void Clear()
        {
            lock (SyncRoot)
            {
                User = null;
            }
        }

        public static bool IsAdmin()
        {
            return User != null && PermissionKeys.IsSystemAdministratorRole(User.RoleName);
        }

        public static bool HasPermission(string permissionKey)
        {
            if (User == null || !User.IsActive || string.IsNullOrWhiteSpace(permissionKey))
                return false;

            string normalizedKey = PermissionKeys.NormalizePermissionKey(permissionKey);
            if (string.IsNullOrWhiteSpace(normalizedKey))
                return false;

            HashSet<string> permissions = ParsePermissions(User.Permissions);
            return permissions.Contains(normalizedKey);
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

        public static void DemandPermission(string permissionKey, string message)
        {
            if (!HasPermission(permissionKey))
                throw new UnauthorizedAccessException(message);
        }

        public static void DemandAny(string message, params string[] permissionKeys)
        {
            if (!HasAny(permissionKeys))
                throw new UnauthorizedAccessException(message);
        }

        private static HashSet<string> ParsePermissions(string permissions)
        {
            if (string.IsNullOrWhiteSpace(permissions))
                return new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            return new HashSet<string>(
                permissions
                    .Split(new[] { ',', ';', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(PermissionKeys.NormalizePermissionKey)
                    .Where(p => !string.IsNullOrWhiteSpace(p)),
                StringComparer.OrdinalIgnoreCase);
        }
    }
}
