using System;
using System.Collections.Generic;
using System.Linq;
using SchoolSystem.Models;

// وضع الأنواع التالية ضمن مساحة الأسماء SchoolSystem.Security.
namespace SchoolSystem.Security
{
    /// <summary>
    /// يحتفظ ببيانات المستخدم المُسجَّل دخوله حالياً وصلاحياته المُحمَّلة.
    /// جميع الفحوصات مركزية هنا لضمان الاتساق عبر كامل التطبيق.
    /// </summary>
    // تعريف الصنف CurrentUser المسؤول عن وظائف طبقة Security.
    public static class CurrentUser
    {
        // تعريف الدالة object لتنفيذ العملية المرتبطة بها.
        private static readonly object SyncRoot = new object();


        public static User User { get; private set; }

        public static bool IsLoggedIn
        {
            get { return User != null; }
        }

        // تعريف الدالة Set لتنفيذ العملية المرتبطة بها.
        public static void Set(User user)
        {
            if (user == null)

                throw new ArgumentNullException("user");


            if (!user.IsActive)

                throw new InvalidOperationException("لا يمكن إنشاء جلسة لحساب غير فعال.");

            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            user.RoleName = PermissionKeys.NormalizeRoleName(user.RoleName);

            // صلاحيات مدير النظام تُشتق من القاموس المركزي عند إنشاء الجلسة.
            // هذا يمنع أن تؤدي قيمة Permissions قديمة/ناقصة أو نسخة قاعدة مختلفة

            if (PermissionKeys.IsSystemAdministratorRole(user.RoleName))
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                user.Permissions = PermissionKeys.GetRoleDefaults(user.RoleName);

            else
                // تُطبَّع الصلاحيات لضمان تنسيق موحد دون حذف أي مفتاح صالح
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                user.Permissions = PermissionKeys.NormalizePermissions(user.Permissions);


            lock (SyncRoot)
            {
                User = user;
            }

            // تسجيل تشخيصي: يُساعد في تتبع مشاكل الصلاحيات أثناء الاختبار
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            LogPermissions(user);
        }

        // تعريف الدالة LogPermissions لتنفيذ العملية المرتبطة بها.
        private static void LogPermissions(User user)
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                if (user == null) return;
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                int count = CountPermissions(user.Permissions);
                System.Diagnostics.Debug.WriteLine(
                    string.Format("[RBAC] تسجيل دخول: المستخدم={0}, الدور={1}, عدد الصلاحيات={2}",
                        user.UserName, user.RoleName, count));
            }

            catch
            {
                // إخراج تشخيصي للفحص (Debug) فقط؛ فشله لا يستحق مسار تسجيل بديل.
            }
        }

        // تعريف الدالة CountPermissions لتنفيذ العملية المرتبطة بها.
        private static int CountPermissions(string permissions)
        {
            if (string.IsNullOrWhiteSpace(permissions)) return 0;
            return permissions.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Length;
        }

        // تعريف الدالة Clear لتنفيذ العملية المرتبطة بها.
        public static void Clear()
        {

            lock (SyncRoot)
            {
                User = null;
            }
        }

        // تعريف الدالة IsAdmin لتنفيذ العملية المرتبطة بها.
        public static bool IsAdmin()
        {

            return User != null && PermissionKeys.IsSystemAdministratorRole(User.RoleName);
        }

        /// <summary>
        /// يتحقق إذا كان المستخدم يملك صلاحية محددة.
        /// يدعم المفاتيح بتنسيق Module.Action أو المفاتيح القديمة *.Manage.
        /// </summary>
        // تعريف الدالة HasPermission لتنفيذ العملية المرتبطة بها.
        public static bool HasPermission(string permissionKey)
        {
            if (User == null || !User.IsActive || string.IsNullOrWhiteSpace(permissionKey))
                return false;

            // مدير النظام سياسة وصول ثابتة: أي فحص لصلاحية مباشرة ينجح

            if (IsAdmin())
                return true;

            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            string normalizedKey = PermissionKeys.NormalizePermissionKey(permissionKey);
            if (string.IsNullOrWhiteSpace(normalizedKey))
                return false;

            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            HashSet<string> permissions = ParsePermissions(User.Permissions);
            if (permissions.Contains(normalizedKey))
                return true;

            // التوافق مع الصلاحيات القديمة: امتلاك Module.Manage يمنح صلاحية
            // عرض الشاشة Module.View، حتى لا تفشل الخدمات التي تستخدم DemandPermission

            if (normalizedKey.EndsWith(".View", StringComparison.OrdinalIgnoreCase))
            {
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                string legacyManageKey = PermissionKeys.GetLegacyManageKey(normalizedKey);
                if (!string.IsNullOrWhiteSpace(legacyManageKey) && permissions.Contains(legacyManageKey))
                    return true;
            }


            return false;
        }

        // تعريف الدالة HasAny لتنفيذ العملية المرتبطة بها.
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
        // تعريف الدالة CanAccessModule لتنفيذ العملية المرتبطة بها.
        public static bool CanAccessModule(string module)
        {
            if (string.IsNullOrWhiteSpace(module) || User == null || !User.IsActive)
                return false;

            if (IsAdmin())
                return true;

            string prefix = module.Trim() + ".";
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            HashSet<string> permissions = ParsePermissions(User.Permissions);

            return permissions.Any(permission =>
                permission.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));
        }

        // تعريف الدالة DemandModule لتنفيذ العملية المرتبطة بها.
        public static void DemandModule(string module, string message)
        {
            if (!CanAccessModule(module))

                throw new UnauthorizedAccessException(message);
        }

        // تعريف الدالة CanView لتنفيذ العملية المرتبطة بها.
        public static bool CanView(string module)
        {

            return HasActionOrManage(module, "View");
        }

        // تعريف الدالة CanAdd لتنفيذ العملية المرتبطة بها.
        public static bool CanAdd(string module)
        {

            return HasActionOrManage(module, "Add");
        }

        // تعريف الدالة CanEdit لتنفيذ العملية المرتبطة بها.
        public static bool CanEdit(string module)
        {

            return HasActionOrManage(module, "Edit");
        }

        // تعريف الدالة CanDelete لتنفيذ العملية المرتبطة بها.
        public static bool CanDelete(string module)
        {

            return HasActionOrManage(module, "Delete");
        }

        // تعريف الدالة CanSearch لتنفيذ العملية المرتبطة بها.
        public static bool CanSearch(string module)
        {

            return HasActionOrManage(module, "Search");
        }

        // تعريف الدالة CanPrint لتنفيذ العملية المرتبطة بها.
        public static bool CanPrint(string module)
        {

            return HasActionOrManage(module, "Print");
        }

        // تعريف الدالة CanExport لتنفيذ العملية المرتبطة بها.
        public static bool CanExport(string module)
        {


            return CanAccessModule(module);
        }

        // تعريف الدالة CanApprove لتنفيذ العملية المرتبطة بها.
        public static bool CanApprove(string module)
        {
            return HasActionOrManage(module, "Approve");
        }

        // تعريف الدالة DemandAction لتنفيذ العملية المرتبطة بها.
        public static void DemandAction(string module, string action, string message)
        {
            if (!HasActionOrManage(module, action))

                throw new UnauthorizedAccessException(message);
        }

        /// <summary>
        /// نظام الشاشات: الوصول للشاشة يمنح كامل عملياتها، لذلك أي فحص عملية
        /// (Add/Edit/Delete/Search/Print/Approve...) يعتمد على صلاحية الشاشة نفسها.
        /// </summary>
        // تعريف الدالة HasActionOrManage لتنفيذ العملية المرتبطة بها.
        private static bool HasActionOrManage(string module, string action)
        {
            if (string.IsNullOrWhiteSpace(module) ||
                string.IsNullOrWhiteSpace(action) ||
                User == null ||
                !User.IsActive)
                return false;

            if (IsAdmin())
                return true;

            string normalizedModule = module.Trim();
            string normalizedAction = action.Trim();

            HashSet<string> permissions = ParsePermissions(User.Permissions);

            // الصلاحية الإجرائية الصريحة هي المصدر الأساسي:
            // Module.Add / Module.Edit / Module.Delete / Module.Search / ...
            string actionKey = normalizedModule + "." + normalizedAction;
            if (permissions.Contains(actionKey))
                return true;

            // توافق مع الصلاحية القديمة الشاملة Module.Manage.
            // امتلاك Module.View وحدها لا يمنح عمليات الإضافة أو التعديل أو الحذف.
            string legacyManageKey = normalizedModule + ".Manage";
            return permissions.Contains(legacyManageKey);
        }

        // تعريف الدالة DemandPermission لتنفيذ العملية المرتبطة بها.
        public static void DemandPermission(string permissionKey, string message)
        {
            if (!HasPermission(permissionKey))

                throw new UnauthorizedAccessException(message);
        }

        // تعريف الدالة DemandAny لتنفيذ العملية المرتبطة بها.
        public static void DemandAny(string message, params string[] permissionKeys)
        {
            if (!HasAny(permissionKeys))

                throw new UnauthorizedAccessException(message);
        }

        /// <summary>
        /// يحلل سلسلة الصلاحيات إلى HashSet للبحث السريع.
        /// يُطبَّع كل مفتاح ويُحذف الفارغ، لكن لا يُحذف أي مفتاح Module.Action صالح.
        /// </summary>
        // تعريف الدالة ParsePermissions لتنفيذ العملية المرتبطة بها.
        private static HashSet<string> ParsePermissions(string permissions)
        {
            if (string.IsNullOrWhiteSpace(permissions))
                return new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (string raw in permissions.Split(new[] { ',', ';', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string trimmed = raw.Trim();
                if (string.IsNullOrWhiteSpace(trimmed)) continue;

                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                string normalized = PermissionKeys.NormalizePermissionKey(trimmed);
                if (!string.IsNullOrWhiteSpace(normalized))
                    result.Add(normalized);
            }


            return result;
        }

        /// <summary>
        /// يُعيد عدد الصلاحيات الحالية للمستخدم المُسجَّل - مفيد للتشخيص.
        /// </summary>
        // تعريف الدالة GetPermissionCount لتنفيذ العملية المرتبطة بها.
        public static int GetPermissionCount()
        {
            if (User == null || string.IsNullOrWhiteSpace(User.Permissions))
                return 0;

            return ParsePermissions(User.Permissions).Count;
        }

        /// <summary>
        /// يُعيد قائمة بجميع الأوحدات التي يمتلك المستخدم أي صلاحية لها.
        /// </summary>
        // تعريف الدالة GetAccessibleModules لتنفيذ العملية المرتبطة بها.
        public static IEnumerable<string> GetAccessibleModules()
        {
            if (User == null || string.IsNullOrWhiteSpace(User.Permissions))
                return Enumerable.Empty<string>();

            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            var permissions = ParsePermissions(User.Permissions);

            return permissions
                .Where(p => p.Contains("."))
                .Select(p => p.Split('.')[0])
                .Distinct(StringComparer.OrdinalIgnoreCase);
        }
    }
}
