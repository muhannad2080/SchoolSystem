using System;
using System.Collections.Generic;
using System.Linq;
using SchoolSystem.Models;

namespace SchoolSystem.Security
{
    /// <summary>
    /// يحتفظ ببيانات المستخدم المُسجَّل دخوله حالياً وصلاحياته المُحمَّلة.
    /// جميع الفحوصات مركزية هنا لضمان الاتساق عبر كامل التطبيق.
    /// </summary>
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
                // تُطبَّع الصلاحيات لضمان تنسيق موحد دون حذف أي مفتاح صالح
                user.Permissions = PermissionKeys.NormalizePermissions(user.Permissions);

            lock (SyncRoot)
            {
                User = user;
            }

            // تسجيل تشخيصي: يُساعد في تتبع مشاكل الصلاحيات أثناء الاختبار
            LogPermissions(user);
        }

        private static void LogPermissions(User user)
        {
            try
            {
                if (user == null) return;
                int count = CountPermissions(user.Permissions);
                System.Diagnostics.Debug.WriteLine(
                    string.Format("[RBAC] تسجيل دخول: المستخدم={0}, الدور={1}, عدد الصلاحيات={2}",
                        user.UserName, user.RoleName, count));
            }
            catch
            {
                // لا نسمح لفشل التسجيل بإيقاف التطبيق
            }
        }

        private static int CountPermissions(string permissions)
        {
            if (string.IsNullOrWhiteSpace(permissions)) return 0;
            return permissions.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Length;
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

        /// <summary>
        /// يتحقق إذا كان المستخدم يملك صلاحية محددة.
        /// يدعم المفاتيح بتنسيق Module.Action أو المفاتيح القديمة *.Manage.
        /// </summary>
        public static bool HasPermission(string permissionKey)
        {
            if (User == null || !User.IsActive || string.IsNullOrWhiteSpace(permissionKey))
                return false;

            // مدير النظام سياسة وصول ثابتة: أي فحص لصلاحية مباشرة ينجح
            // حتى لو كان المفتاح غير موجود في نسخة قاعدة البيانات الحالية.
            if (IsAdmin())
                return true;

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

        /// <summary>
        /// يتحقق إذا كان المستخدم يملك أي صلاحية تتعلق بالوحدة (الشاشة) المحددة.
        /// مثال: CanAccessModule("Students") = true إذا كان لديه Students.View.
        /// مدير النظام يملك الوصول لكل الشاشات كسياسة مركزية واحدة.
        /// </summary>
        public static bool CanAccessModule(string module)
        {
            if (string.IsNullOrWhiteSpace(module) || User == null || !User.IsActive)
                return false;

            if (IsAdmin())
                return true;

            string prefix = module.Trim() + ".";
            HashSet<string> permissions = ParsePermissions(User.Permissions);
            return permissions.Any(permission =>
                permission.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));
        }

        public static void DemandModule(string module, string message)
        {
            if (!CanAccessModule(module))
                throw new UnauthorizedAccessException(message);
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
            // نظام الشاشات: امتلاك الشاشة يمنح كامل عملياتها (طباعة/تصدير/اعتماد...).
            return CanAccessModule(module);
        }

        public static bool CanApprove(string module)
        {
            return CanAccessModule(module);
        }

        public static void DemandAction(string module, string action, string message)
        {
            if (!HasActionOrManage(module, action))
                throw new UnauthorizedAccessException(message);
        }

        /// <summary>
        /// نظام الشاشات: الوصول للشاشة يمنح كامل عملياتها، لذلك أي فحص عملية
        /// (Add/Edit/Delete/Search/Print/Approve...) يعتمد على صلاحية الشاشة نفسها.
        /// </summary>
        private static bool HasActionOrManage(string module, string action)
        {
            if (string.IsNullOrWhiteSpace(module) || string.IsNullOrWhiteSpace(action))
                return false;
            return CanAccessModule(module);
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

        /// <summary>
        /// يحلل سلسلة الصلاحيات إلى HashSet للبحث السريع.
        /// يُطبَّع كل مفتاح ويُحذف الفارغ، لكن لا يُحذف أي مفتاح Module.Action صالح.
        /// </summary>
        private static HashSet<string> ParsePermissions(string permissions)
        {
            if (string.IsNullOrWhiteSpace(permissions))
                return new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (string raw in permissions.Split(new[] { ',', ';', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string trimmed = raw.Trim();
                if (string.IsNullOrWhiteSpace(trimmed)) continue;

                string normalized = PermissionKeys.NormalizePermissionKey(trimmed);
                if (!string.IsNullOrWhiteSpace(normalized))
                    result.Add(normalized);
            }

            return result;
        }

        /// <summary>
        /// يُعيد عدد الصلاحيات الحالية للمستخدم المُسجَّل - مفيد للتشخيص.
        /// </summary>
        public static int GetPermissionCount()
        {
            if (User == null || string.IsNullOrWhiteSpace(User.Permissions))
                return 0;
            return ParsePermissions(User.Permissions).Count;
        }

        /// <summary>
        /// يُعيد قائمة بجميع الأوحدات التي يمتلك المستخدم أي صلاحية لها.
        /// </summary>
        public static IEnumerable<string> GetAccessibleModules()
        {
            if (User == null || string.IsNullOrWhiteSpace(User.Permissions))
                return Enumerable.Empty<string>();

            var permissions = ParsePermissions(User.Permissions);
            return permissions
                .Where(p => p.Contains("."))
                .Select(p => p.Split('.')[0])
                .Distinct(StringComparer.OrdinalIgnoreCase);
        }
    }
}
