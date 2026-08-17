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

            // صلاحيات مدير النظام تُشتق من القاموس المركزي عند إنشاء الجلسة.
            // هذا يمنع أن تؤدي قيمة Permissions قديمة/ناقصة أو نسخة قاعدة مختلفة
            // إلى إخفاء واجهات الإدارة بعد نجاح تسجيل الدخول.
            if (PermissionKeys.IsSystemAdministratorRole(user.RoleName))
                user.Permissions = PermissionKeys.GetRoleDefaults(user.RoleName);
            else
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

        public static bool CanAccessModule(string module)
        {
            if (string.IsNullOrWhiteSpace(module) || User == null || !User.IsActive)
                return false;

            string prefix = module.Trim() + ".";
            HashSet<string> permissions = ParsePermissions(User.Permissions);
            return permissions.Any(permission =>
                permission.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));
        }

        public static bool CanView(string module)
        {
            return HasActionOrManage(module, "View");
        }

        public static bool CanAdd(string module)
        {
            return HasActionOrManage(module, "Add");
        }

        public static bool CanEdit(string module)
        {
            return HasActionOrManage(module, "Edit");
        }

        public static bool CanDelete(string module)
        {
            return HasActionOrManage(module, "Delete");
        }

        public static bool CanSearch(string module)
        {
            return HasActionOrManage(module, "Search");
        }

        public static bool CanPrint(string module)
        {
            return HasActionOrManage(module, "Print");
        }

        public static bool CanExport(string module)
        {
            return HasAny(module + ".ExportExcel", module + ".ExportPDF", module + ".Export") || HasPermission(module + ".Manage");
        }

        public static bool CanApprove(string module)
        {
            return HasActionOrManage(module, "Approve");
        }

        public static void DemandAction(string module, string action, string message)
        {
            if (!HasActionOrManage(module, action))
                throw new UnauthorizedAccessException(message);
        }

        private static bool HasActionOrManage(string module, string action)
        {
            if (string.IsNullOrWhiteSpace(module) || string.IsNullOrWhiteSpace(action))
                return false;
            return HasPermission(module + "." + action) || HasPermission(module + ".Manage");
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
