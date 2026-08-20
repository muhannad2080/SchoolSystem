using System;
using System.Collections.Generic;
using System.Linq;
using SchoolSystem.Security;

namespace SchoolSystem.Services
{
    /// <summary>
    /// السلطة المركزية الوحيدة لفحص الصلاحيات في التطبيق.
    /// القاعدة: مصدر الحقيقة الوحيد = قاعدة البيانات (RolePermissions + UserPermissions)،
    /// تُحمَّل عند تسجيل الدخول في CurrentUser، وكل فحص وصول يمر عبر هذا الكائن.
    ///
    /// قاعدة مدير النظام: يملك الوصول الكامل لجميع الشاشات كسياسة مركزية واحدة
    /// (وليس عبر أكواد مبعثرة في الواجهات)، مع أن قاعدة البيانات تمنحه الكتالوج كاملاً.
    /// </summary>
    public static class AuthorizationService
    {
        /// <summary>
        /// قائمة صلاحيات الشاشات المتاحة في النظام (Module.View) — مصدر واحد لعرضها.
        /// </summary>
        public static IReadOnlyList<string> ScreenPermissions
        {
            get { return PermissionKeys.ScreenPermissions; }
        }

        /// <summary>
        /// هل المستخدم الحالي مدير نظام؟
        /// </summary>
        public static bool IsAdmin()
        {
            return CurrentUser.IsAdmin();
        }

        /// <summary>
        /// هل يملك المستخدم صلاحية دقيقة محددة (مثل Reports.View)؟
        /// </summary>
        public static bool HasPermission(string permissionKey)
        {
            return CurrentUser.HasPermission(permissionKey);
        }

        /// <summary>
        /// هل يملك المستخدم الوصول لشاشة (وحدة) معينة؟
        /// مدير النظام يملك كل الشاشات تلقائياً.
        /// </summary>
        public static bool CanAccessScreen(string module)
        {
            if (string.IsNullOrWhiteSpace(module))
                return false;
            if (CurrentUser.IsAdmin())
                return true;
            return CurrentUser.CanAccessModule(module);
        }

        /// <summary>
        /// هل يملك المستخدم الوصول لأي شاشة من القائمة؟
        /// </summary>
        public static bool CanAccessAnyScreen(params string[] modules)
        {
            if (modules == null || modules.Length == 0)
                return false;
            foreach (string module in modules)
            {
                if (CanAccessScreen(module))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// يمنع الوصول إذا لم يملك المستخدم صلاحية الشاشة، مع رسالة واضحة.
        /// </summary>
        public static void RequireScreen(string module, string message)
        {
            if (!CanAccessScreen(module))
                throw new UnauthorizedAccessException(message ?? "ليس لديك صلاحية الوصول إلى هذه الشاشة.");
        }

        /// <summary>
        /// يمنع الوصول إذا لم يملك المستخدم أي صلاحية دقيقة من المفاتيح المطلوبة.
        /// </summary>
        public static void RequireAny(string message, params string[] permissionKeys)
        {
            CurrentUser.DemandAny(message, permissionKeys);
        }

        /// <summary>
        /// قائمة الشاشات (الوحدات) التي يملك المستخدم الحالي وصولاً لها.
        /// </summary>
        public static IEnumerable<string> GetAccessibleModules()
        {
            if (CurrentUser.IsAdmin())
                return PermissionKeys.ScreenPermissions
                    .Select(k => k.Split('.')[0])
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .OrderBy(m => m);

            return CurrentUser.GetAccessibleModules();
        }

        /// <summary>
        /// يُعيد مفاتيح الشاشات الفعلية لسلسلة صلاحيات (تُستخدم عند الحفظ).
        /// </summary>
        public static IReadOnlyList<string> GetScreenKeysFromPermissions(string permissions)
        {
            return PermissionKeys.GetScreenKeysFromPermissions(permissions);
        }
    }
}