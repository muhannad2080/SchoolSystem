using System;
using System.Collections.Generic;
using System.Linq;

namespace SchoolSystem.Security
{
    /// <summary>
    /// مصدر الحقيقة الوحيد لمفاتيح RBAC. المفاتيح القديمة من نوع *.Manage محفوظة
    /// للتوافق، بينما المفاتيح الإجرائية الجديدة تفصل العرض والإضافة والتعديل والحذف
    /// والطباعة والتصدير والاعتماد.
    ///
    /// قاعدة هامة: أي مفتاح من الشكل Module.Action حيث Module وAction معروفان
    /// يُعتبر مفتاحاً صالحاً ويُعاد كما هو دون حذفه.
    /// </summary>
    public static class PermissionKeys
    {
        public const string DashboardView = "Dashboard.View";

        public const string StudentsView = "Students.View";
        public const string StudentsManage = "Students.Manage";
        public const string EnrollmentManage = "Enrollment.Manage";
        public const string ClassAssignmentView = "ClassAssignment.View";
        public const string ClassAssignmentManage = "ClassAssignment.Manage";
        public const string TeachersManage = "Teachers.Manage";
        public const string StaffAttendanceManage = "StaffAttendance.Manage";
        public const string PayrollManage = "Payroll.Manage";
        public const string SubjectsManage = "Subjects.Manage";
        public const string ClassesManage = "Classes.Manage";
        public const string TimetableManage = "Timetable.Manage";
        public const string AttendanceManage = "Attendance.Manage";
        public const string GradesManage = "Grades.Manage";
        public const string FeesManage = "Fees.Manage";
        public const string VouchersManage = "Vouchers.Manage";
        public const string ExpensesManage = "Expenses.Manage";
        public const string LibraryManage = "Library.Manage";
        public const string TransportManage = "Transport.Manage";
        public const string ReportsView = "Reports.View";
        public const string UsersManage = "Users.Manage";
        public const string AuditLogsView = "AuditLogs.View";
        public const string SettingsManage = "Settings.Manage";

        public const string UsersView = "Users.View";
        public const string UsersAdd = "Users.Add";
        public const string UsersEdit = "Users.Edit";
        public const string UsersDelete = "Users.Delete";
        public const string UsersManageRoles = "Users.ManageRoles";
        public const string RolesView = "Roles.View";
        public const string RolesAdd = "Roles.Add";
        public const string RolesEdit = "Roles.Edit";
        public const string RolesDelete = "Roles.Delete";
        public const string RolesManage = "Roles.Manage";
        public const string PermissionsManage = "Permissions.Manage";
        public const string AuditLogsExportExcel = "AuditLogs.ExportExcel";
        public const string AuditLogsExportPDF = "AuditLogs.ExportPDF";
        public const string AuditLogsPrint = "AuditLogs.Print";
        public const string SettingsView = "Settings.View";
        public const string SettingsEdit = "Settings.Edit";

        public const string SystemAdministratorRole = "مدير النظام";

        private static readonly string[] StandardActions =
        {
            "View", "Add", "Edit", "Delete", "Search", "Print",
            "ExportExcel", "ExportCsv", "ExportPDF",
            "Approve", "Cancel", "Manage", "ManageRoles"
        };

        private static readonly string[] Modules =
        {
            "Students", "Enrollment", "ClassAssignment", "Teachers", "TeacherAttendance",
            "StaffAttendance", "TeacherContracts", "Payroll", "Subjects", "Classes", "Rooms",
            "Timetable", "Grades", "Attendance", "Fees", "FeePlans", "Vouchers", "Expenses",
            "Transport", "Library", "Reports", "Dashboard", "AuditLogs", "Settings",
            "Users", "Roles", "Permissions"
        };

        private static readonly HashSet<string> ModuleSet = new HashSet<string>(Modules, StringComparer.OrdinalIgnoreCase);
        private static readonly HashSet<string> ActionSet = new HashSet<string>(StandardActions, StringComparer.OrdinalIgnoreCase);

        private static readonly IReadOnlyList<string> Catalog = BuildCatalog();

        public static IReadOnlyList<string> All
        {
            get { return Catalog; }
        }

        /// <summary>
        /// الصلاحيات التي تعرض في واجهة إدارة المستخدمين. هذه القائمة تمثل الشاشة
        /// نفسها فقط، بينما تبقى صلاحيات العمليات داخل الكود والخدمات للأدوار
        /// والصلاحيات الداخلية. أي عنصر هنا هو Module.View.
        /// </summary>
        public static IReadOnlyList<string> ScreenPermissions
        {
            get { return ScreenCatalog; }
        }

        private static readonly IReadOnlyList<string> ScreenCatalog = BuildScreenCatalog();

        private static IReadOnlyList<string> BuildScreenCatalog()
        {
            return Modules
                .Select(module => module + ".View")
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();
        }

        /// <summary>
        /// يتحقق إذا كان المفتاح مفتاح شاشة فقط (Module.View) وليس مفتاح عملية.
        /// </summary>
        public static bool IsScreenPermission(string permissionKey)
        {
            string normalized = NormalizePermissionKey(permissionKey);
            if (string.IsNullOrWhiteSpace(normalized))
                return false;
            int dot = normalized.IndexOf('.');
            if (dot <= 0 || dot == normalized.Length - 1)
                return false;
            string module = normalized.Substring(0, dot);
            string action = normalized.Substring(dot + 1);
            return ModuleSet.Contains(module) && action.Equals("View", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// يستخرج من سلسلة الصلاحيات قائمة مفاتيح الشاشات (Module.View) المرتبة بدون تكرار.
        /// تُستخدم كمصدر موحد لعرض وإدارة صلاحيات المستخدمين على مستوى الشاشات.
        /// </summary>
        public static IReadOnlyList<string> GetScreenKeysFromPermissions(string permissions)
        {
            if (string.IsNullOrWhiteSpace(permissions))
                return Array.Empty<string>();

            List<string> result = new List<string>();
            foreach (string raw in permissions.Split(new[] { ',', ';', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string screen = ToScreenPermission(raw.Trim());
                if (!string.IsNullOrWhiteSpace(screen))
                    result.Add(screen);
            }

            return result.Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
        }

        /// <summary>
        /// يحول صلاحية شاشة أو صلاحية عملية قديمة إلى مفتاح الشاشة الموافق.
        /// </summary>
        public static string ToScreenPermission(string permissionKey)
        {
            string normalized = NormalizePermissionKey(permissionKey);
            if (string.IsNullOrWhiteSpace(normalized))
                return string.Empty;

            int dot = normalized.IndexOf('.');
            if (dot <= 0)
                return string.Empty;

            string module = normalized.Substring(0, dot);
            return ModuleSet.Contains(module) ? module + ".View" : string.Empty;
        }

        private static IReadOnlyList<string> BuildCatalog()
        {
            var values = new List<string>
            {
                DashboardView, StudentsView, StudentsManage, EnrollmentManage, ClassAssignmentView,
                ClassAssignmentManage, TeachersManage, StaffAttendanceManage, PayrollManage, SubjectsManage, ClassesManage,
                TimetableManage, AttendanceManage, GradesManage, FeesManage, VouchersManage,
                ExpensesManage, LibraryManage, TransportManage, ReportsView, UsersManage,
                AuditLogsView, SettingsManage, UsersView, UsersAdd, UsersEdit, UsersDelete,
                UsersManageRoles, RolesView, RolesAdd, RolesEdit, RolesDelete, RolesManage,
                PermissionsManage, AuditLogsExportExcel, AuditLogsExportPDF, AuditLogsPrint,
                SettingsView, SettingsEdit
            };

            foreach (string module in Modules)
            {
                foreach (string action in StandardActions)
                    values.Add(module + "." + action);
            }

            return values.Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
        }

        /// <summary>
        /// يتحقق إذا كان المفتاح مفتاح Module.Action صالح حتى لو لم يكن ضمن Catalog المحدد مسبقاً.
        /// يقبل أي مفتاح بالشكل Module.Action حيث Module معروف والـ Action أي كلمة لاتينية.
        /// هذا يمنع حذف أي صلاحية حقيقية بسبب غيابها من StandardActions.
        /// </summary>
        private static bool IsValidModuleActionKey(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            int dot = value.IndexOf('.');
            if (dot <= 0 || dot == value.Length - 1)
                return false;

            string module = value.Substring(0, dot);
            string action = value.Substring(dot + 1);

            if (string.IsNullOrWhiteSpace(module) || string.IsNullOrWhiteSpace(action))
                return false;

            // الوحدة يجب أن تكون معروفة، لكن الـ Action يُقبل إذا كان من StandardActions
            // أو أي كلمة لاتينية بدون مسافات أو رموز خاصة (للتوافق المستقبلي).
            if (!ModuleSet.Contains(module))
                return false;

            // نقبل أي Action من StandardActions أو ManageRoles كاستثناء مُعتمد
            if (ActionSet.Contains(action))
                return true;

            // نقبل أي action يتكون من أحرف إنجليزية فقط (مثل ManageRoles)
            foreach (char c in action)
            {
                if (!char.IsLetter(c))
                    return false;
            }
            return action.Length > 0;
        }

        public static string GetDisplayName(string permissionKey)
        {
            string normalized = NormalizePermissionKeyInternal(permissionKey);
            if (string.IsNullOrWhiteSpace(normalized))
                return permissionKey ?? string.Empty;

            switch (normalized)
            {
                case DashboardView: return "عرض لوحة التحكم";
                case StudentsManage: return "إدارة الطلاب (شاملة)";
                case EnrollmentManage: return "إدارة القبول والتسجيل";
                case ClassAssignmentManage: return "إدارة توزيع الطلاب";
                case TeachersManage: return "إدارة المعلمين";
                case StaffAttendanceManage: return "إدارة حضور الموظفين";
                case PayrollManage: return "إدارة الرواتب والعقود";
                case SubjectsManage: return "إدارة المواد";
                case ClassesManage: return "إدارة الصفوف والفصول";
                case TimetableManage: return "إدارة الجداول الدراسية";
                case AttendanceManage: return "إدارة حضور الطلاب";
                case GradesManage: return "إدارة الدرجات";
                case FeesManage: return "إدارة الرسوم الدراسية";
                case VouchersManage: return "إدارة السندات";
                case ExpensesManage: return "إدارة المصروفات";
                case LibraryManage: return "إدارة المكتبة";
                case TransportManage: return "إدارة النقل";
                case ReportsView: return "عرض التقارير";
                case UsersManage: return "إدارة المستخدمين (شاملة)";
                case AuditLogsView: return "عرض سجل التدقيق";
                case SettingsManage: return "إدارة الإعدادات";
                case UsersManageRoles: return "إدارة أدوار المستخدمين";
                case RolesManage: return "إدارة الأدوار (شاملة)";
                case PermissionsManage: return "إدارة الصلاحيات";
                case AuditLogsExportExcel: return "تصدير سجل التدقيق إلى Excel";
                case AuditLogsExportPDF: return "تصدير سجل التدقيق إلى PDF";
                case AuditLogsPrint: return "طباعة سجل التدقيق";
            }

            string[] parts = normalized.Split('.');
            if (parts.Length == 2)
                return GetModuleDisplayName(parts[0]) + " - " + GetActionDisplayName(parts[1]);

            return normalized;
        }

        private static string GetModuleDisplayName(string module)
        {
            switch (module)
            {
                case "Students": return "الطلاب";
                case "Enrollment": return "القبول والتسجيل";
                case "ClassAssignment": return "توزيع الطلاب";
                case "Teachers": return "المعلمون";
                case "TeacherAttendance": return "حضور المعلمين";
                case "StaffAttendance": return "حضور الموظفين";
                case "TeacherContracts": return "عقود المعلمين";
                case "Payroll": return "الرواتب";
                case "Subjects": return "المواد";
                case "Classes": return "الفصول الدراسية";
                case "Rooms": return "القاعات";
                case "Timetable": return "الجدول الدراسي";
                case "Grades": return "الدرجات";
                case "Attendance": return "حضور الطلاب";
                case "Fees": return "الرسوم";
                case "FeePlans": return "خطط الرسوم";
                case "Vouchers": return "السندات";
                case "Expenses": return "المصروفات";
                case "Transport": return "النقل";
                case "Library": return "المكتبة";
                case "Reports": return "التقارير";
                case "Dashboard": return "لوحة التحكم";
                case "AuditLogs": return "سجل التدقيق";
                case "Settings": return "الإعدادات";
                case "Users": return "المستخدمون";
                case "Roles": return "الأدوار";
                case "Permissions": return "الصلاحيات";
                default: return module;
            }
        }

        private static string GetActionDisplayName(string action)
        {
            switch (action)
            {
                case "View": return "عرض";
                case "Add": return "إضافة";
                case "Edit": return "تعديل";
                case "Delete": return "حذف";
                case "Search": return "بحث";
                case "Print": return "طباعة";
                case "ExportExcel": return "تصدير Excel";
                case "ExportCsv": return "تصدير CSV";
                case "ExportPDF": return "تصدير PDF";
                case "Approve": return "اعتماد";
                case "Cancel": return "إلغاء";
                case "Manage": return "إدارة";
                default: return action;
            }
        }

        public static string NormalizeRoleName(string roleName)
        {
            string value = (roleName ?? string.Empty).Trim();
            if (value.Equals(SystemAdministratorRole, StringComparison.OrdinalIgnoreCase) ||
                value.Equals("Admin", StringComparison.OrdinalIgnoreCase) ||
                value.Equals("Administrator", StringComparison.OrdinalIgnoreCase))
                return SystemAdministratorRole;
            return value;
        }

        public static bool IsSystemAdministratorRole(string roleName)
        {
            return string.Equals(NormalizeRoleName(roleName), SystemAdministratorRole, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// يُعيد المفتاح المُعيَّر ضمن Catalog، أو يُعيد المفتاح كما هو إذا كان صالح الشكل (Module.Action)،
        /// أو يُعيد string.Empty إذا كان بالفعل غير صالح.
        ///
        /// الإصلاح الجذري: قبل الإرجاع بـ string.Empty، يتحقق إذا كان المفتاح
        /// من الشكل Module.Action ويُعيده كما هو لمنع حذفه خطأً.
        /// </summary>
        public static string NormalizePermissionKey(string permissionKey)
        {
            if (string.IsNullOrWhiteSpace(permissionKey))
                return string.Empty;

            // استخلاص المفتاح من النص المركب "Key - Description"
            string value = permissionKey.Trim();
            int separatorIndex = value.IndexOf(" - ", StringComparison.Ordinal);
            if (separatorIndex > 0)
                value = value.Substring(0, separatorIndex).Trim();

            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            // 1. البحث عن مطابقة دقيقة في Catalog (OrdinalIgnoreCase)
            string exact = Catalog.FirstOrDefault(key => string.Equals(key, value, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(exact))
                return exact;

            // 2. البحث في Catalog باستخدام الاسم المعروض (Display Name)
            string display = Catalog.FirstOrDefault(key => string.Equals(GetDisplayName(key), value, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(display))
                return display;

            // 3. التحويلات من الأسماء العربية القديمة والاختصارات
            switch (value)
            {
                case "Dashboard": return DashboardView;
                case "عرض لوحة التحكم": return DashboardView;
                case "عرض الطلاب": return StudentsView;
                case "إدارة الطلاب": return StudentsManage;
                case "القبول والتسجيل": return EnrollmentManage;
                case "توزيع الطلاب على الفصول": return ClassAssignmentManage;
                case "إدارة المعلمين": return TeachersManage;
                case "حضور وانصراف الموظفين": return StaffAttendanceManage;
                case "الرواتب والعقود": return PayrollManage;
                case "إدارة المواد": return SubjectsManage;
                case "إدارة الصفوف والفصول": return ClassesManage;
                case "الجداول الدراسية": return TimetableManage;
                case "حضور الطلاب": return AttendanceManage;
                case "إدارة الدرجات": return GradesManage;
                case "الرسوم الدراسية": return FeesManage;
                case "السندات قبض/صرف": return VouchersManage;
                case "المصروفات": return ExpensesManage;
                case "المكتبة": return LibraryManage;
                case "النقل": return TransportManage;
                case "التقارير والطباعة والتصدير": return ReportsView;
                case "إدارة المستخدمين والصلاحيات": return UsersManage;
                case "عرض سجل التدقيق": return AuditLogsView;
                case "الإعدادات والنسخ الاحتياطي": return SettingsManage;

                // اختصارات Module فقط (بدون Action) → View افتراضياً
                case "Students": return StudentsView;
                case "Enrollment": return EnrollmentManage;
                case "ClassAssignment": return ClassAssignmentManage;
                case "Teachers": return TeachersManage;
                case "Subjects": return SubjectsManage;
                case "Classes": return ClassesManage;
                case "Timetable": return TimetableManage;
                case "Attendance": return AttendanceManage;
                case "Grades": return GradesManage;
                case "Fees": return FeesManage;
                case "Vouchers": return VouchersManage;
                case "Expenses": return ExpensesManage;
                case "Library": return LibraryManage;
                case "Transport": return TransportManage;
                case "Reports": return ReportsView;
                case "Users": return UsersManage;
                case "AuditLogs": return AuditLogsView;
                case "Settings": return SettingsManage;
                case "Roles": return RolesManage;
                case "Permissions": return PermissionsManage;
            }

            // 4. الإصلاح الجذري: إذا كان المفتاح من الشكل Module.Action الصالح،
            //    أعده كما هو بدلاً من حذفه. هذا يمنع فقدان الصلاحيات التي لم تُضف
            //    بعد إلى Catalog لكنها صالحة منطقياً.
            if (IsValidModuleActionKey(value))
                return value;

            // 5. المفتاح غير صالح تماماً
            return string.Empty;
        }

        /// <summary>
        /// نسخة داخلية لا تستخدم GetDisplayName لتجنب التكرار اللانهائي
        /// </summary>
        private static string NormalizePermissionKeyInternal(string permissionKey)
        {
            if (string.IsNullOrWhiteSpace(permissionKey))
                return string.Empty;
            string value = permissionKey.Trim();
            int separatorIndex = value.IndexOf(" - ", StringComparison.Ordinal);
            if (separatorIndex > 0)
                value = value.Substring(0, separatorIndex).Trim();
            string exact = Catalog.FirstOrDefault(key => string.Equals(key, value, StringComparison.OrdinalIgnoreCase));
            return exact ?? value;
        }

        public static string NormalizePermissions(string permissions)
        {
            if (string.IsNullOrWhiteSpace(permissions))
                return string.Empty;
            return Serialize(permissions.Split(new[] { ',', ';', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));
        }

        public static string Serialize(IEnumerable<string> permissions)
        {
            if (permissions == null)
                return string.Empty;
            return string.Join(",", permissions
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .Select(p => NormalizePermissionKey(p.Trim()))
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .Distinct(StringComparer.OrdinalIgnoreCase));
        }

        public static bool IsActionKey(string permissionKey, string action)
        {
            string normalized = NormalizePermissionKey(permissionKey);
            return normalized.EndsWith("." + action, StringComparison.OrdinalIgnoreCase);
        }

        public static string GetLegacyManageKey(string permissionKey)
        {
            string normalized = NormalizePermissionKey(permissionKey);
            if (string.IsNullOrWhiteSpace(normalized))
                return string.Empty;
            string module = normalized.Split('.')[0];
            return module + ".Manage";
        }

        private static string[] Grant(string module, params string[] actions)
        {
            return actions.Select(action => module + "." + action).ToArray();
        }

        public static string GetRoleDefaults(string roleName)
        {
            switch (NormalizeRoleName(roleName))
            {
                case SystemAdministratorRole:
                    return Serialize(All);
                case "الإدارة":
                    return Serialize(Grant("Dashboard", "View").Concat(Grant("Students", "View", "Add", "Edit", "Delete", "Search", "Print", "ExportExcel", "ExportPDF"))
                        .Concat(Grant("Enrollment", "View", "Add", "Edit", "Delete", "Search", "Print"))
                        .Concat(Grant("ClassAssignment", "View", "Add", "Edit", "Delete", "Search"))
                        .Concat(Grant("Teachers", "View", "Add", "Edit", "Delete", "Search"))
                        .Concat(Grant("Subjects", "View", "Add", "Edit", "Delete"))
                        .Concat(Grant("Classes", "View", "Add", "Edit", "Delete"))
                        .Concat(Grant("Rooms", "View", "Add", "Edit"))
                        .Concat(Grant("Timetable", "View", "Add", "Edit", "Delete", "Print"))
                        .Concat(Grant("Attendance", "View", "Add", "Edit", "Delete", "Print"))
                        .Concat(Grant("Grades", "View", "Add", "Edit", "Delete", "Approve", "Print"))
                        .Concat(Grant("Reports", "View", "Print", "ExportExcel", "ExportCsv", "ExportPDF"))
                        .Concat(Grant("AuditLogs", "View")));
                case "مدير المدرسة":
                    return GetRoleDefaults("الإدارة");
                case "وكيل المدرسة":
                    return GetRoleDefaults("الإدارة");
                case "شؤون الطلاب":
                    return Serialize(Grant("Dashboard", "View").Concat(Grant("Students", "View", "Add", "Edit", "Search", "Print"))
                        .Concat(Grant("Enrollment", "View", "Add", "Edit", "Search"))
                        .Concat(Grant("ClassAssignment", "View", "Add", "Edit", "Search"))
                        .Concat(Grant("Attendance", "View", "Add", "Edit"))
                        .Concat(Grant("Grades", "View", "Print"))
                        .Concat(Grant("Reports", "View", "Print", "ExportExcel", "ExportCsv")));
                case "المعلمون":
                    return Serialize(Grant("Dashboard", "View").Concat(Grant("Students", "View", "Search"))
                        .Concat(Grant("Attendance", "View", "Add", "Edit"))
                        .Concat(Grant("Grades", "View", "Add", "Edit"))
                        .Concat(Grant("Timetable", "View"))
                        .Concat(Grant("Reports", "View")));
                case "المالية":
                    return Serialize(Grant("Dashboard", "View").Concat(Grant("Fees", "View", "Add", "Edit", "Delete", "Search", "Print", "ExportExcel"))
                        .Concat(Grant("FeePlans", "View", "Add", "Edit", "Delete"))
                        .Concat(Grant("Vouchers", "View", "Add", "Edit", "Delete", "Print", "ExportExcel"))
                        .Concat(Grant("Expenses", "View", "Add", "Edit", "Delete", "Print", "ExportExcel"))
                        .Concat(Grant("Payroll", "View", "Search", "Print", "ExportExcel"))
                        .Concat(Grant("Reports", "View", "Print", "ExportExcel", "ExportCsv", "ExportPDF")));
                case "المكتبة":
                    return Serialize(Grant("Dashboard", "View").Concat(Grant("Library", "View", "Add", "Edit", "Delete", "Search", "Print"))
                        .Concat(Grant("Reports", "View")));
                case "أمين المكتبة":
                    return GetRoleDefaults("المكتبة");
                case "النقل":
                    return Serialize(Grant("Dashboard", "View").Concat(Grant("Transport", "View", "Add", "Edit", "Delete", "Search", "Print"))
                        .Concat(Grant("Reports", "View")));
                case "مسؤول النقل":
                    return GetRoleDefaults("النقل");
                case "التقارير":
                    return Serialize(Grant("Dashboard", "View").Concat(Grant("Reports", "View", "Print", "ExportExcel", "ExportCsv", "ExportPDF")));
                case "مدقق":
                    return Serialize(Grant("Dashboard", "View")
                        .Concat(Grant("Reports", "View", "Print", "ExportExcel", "ExportCsv", "ExportPDF"))
                        .Concat(Grant("AuditLogs", "View", "Print", "ExportExcel", "ExportPDF")));
                case "شؤون الموظفين":
                    return Serialize(Grant("Dashboard", "View")
                        .Concat(Grant("Teachers", "View", "Add", "Edit", "Delete", "Search"))
                        .Concat(Grant("StaffAttendance", "View", "Add", "Edit", "Delete", "Search", "Print"))
                        .Concat(Grant("Payroll", "View", "Add", "Edit", "Delete", "Search", "Print"))
                        .Concat(Grant("TeacherContracts", "View", "Add", "Edit", "Delete", "Search"))
                        .Concat(Grant("Reports", "View", "Print", "ExportExcel")));
                case "موظف الاستقبال":
                    return Serialize(Grant("Dashboard", "View")
                        .Concat(Grant("Students", "View", "Search"))
                        .Concat(Grant("Enrollment", "View", "Search"))
                        .Concat(Grant("Reports", "View")));
                default:
                    return string.Empty;
            }
        }
    }
}
