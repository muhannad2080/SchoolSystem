using System;
using System.Collections.Generic;
using System.Linq;

namespace SchoolSystem.Security
{
    /// <summary>
    /// مصدر الحقيقة الوحيد لمفاتيح RBAC. المفاتيح القديمة من نوع *.Manage محفوظة
    /// للتوافق، بينما المفاتيح الإجرائية الجديدة تفصل العرض والإضافة والتعديل والحذف
    /// والطباعة والتصدير والاعتماد.
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
            "View", "Add", "Edit", "Delete", "Search", "Print", "ExportExcel", "ExportCsv", "ExportPDF", "Approve", "Cancel"
        };

        private static readonly string[] Modules =
        {
            "Students", "Enrollment", "ClassAssignment", "Teachers", "TeacherAttendance",
            "StaffAttendance", "TeacherContracts", "Payroll", "Subjects", "Classes", "Rooms",
            "Timetable", "Grades", "Attendance", "Fees", "FeePlans", "Vouchers", "Expenses",
            "Transport", "Library", "Reports", "Dashboard", "AuditLogs", "Settings"
        };

        private static readonly IReadOnlyList<string> Catalog = BuildCatalog();

        public static IReadOnlyList<string> All
        {
            get { return Catalog; }
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

        public static string GetDisplayName(string permissionKey)
        {
            string normalized = NormalizePermissionKeyWithoutDisplay(permissionKey);
            if (string.IsNullOrWhiteSpace(normalized))
                return permissionKey ?? string.Empty;

            switch (normalized)
            {
                case DashboardView: return "عرض لوحة التحكم";
                case StudentsManage: return "إدارة الطلاب القديمة";
                case EnrollmentManage: return "إدارة القبول والتسجيل القديمة";
                case ClassAssignmentManage: return "إدارة توزيع الطلاب القديمة";
                case TeachersManage: return "إدارة المعلمين القديمة";
                case StaffAttendanceManage: return "إدارة حضور الموظفين القديمة";
                case PayrollManage: return "إدارة الرواتب والعقود القديمة";
                case SubjectsManage: return "إدارة المواد القديمة";
                case ClassesManage: return "إدارة الصفوف والفصول القديمة";
                case TimetableManage: return "إدارة الجداول القديمة";
                case AttendanceManage: return "إدارة الحضور القديمة";
                case GradesManage: return "إدارة الدرجات القديمة";
                case FeesManage: return "إدارة الرسوم القديمة";
                case VouchersManage: return "إدارة السندات القديمة";
                case ExpensesManage: return "إدارة المصروفات القديمة";
                case LibraryManage: return "إدارة المكتبة القديمة";
                case TransportManage: return "إدارة النقل القديمة";
                case ReportsView: return "عرض التقارير";
                case UsersManage: return "إدارة المستخدمين القديمة";
                case AuditLogsView: return "عرض سجل التدقيق";
                case SettingsManage: return "إدارة الإعدادات القديمة";
                case UsersManageRoles: return "إدارة أدوار المستخدمين";
                case RolesManage: return "إدارة الأدوار";
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

        public static string NormalizePermissionKey(string permissionKey)
        {
            if (string.IsNullOrWhiteSpace(permissionKey))
                return string.Empty;

            string value = permissionKey.Trim();
            int separatorIndex = value.IndexOf(" - ", StringComparison.Ordinal);
            if (separatorIndex > 0)
                value = value.Substring(0, separatorIndex).Trim();

            string exact = Catalog.FirstOrDefault(key => string.Equals(key, value, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(exact))
                return exact;

            string display = Catalog.FirstOrDefault(key => string.Equals(GetDisplayName(key), value, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(display))
                return display;

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
                default: return string.Empty;
            }
        }

        private static string NormalizePermissionKeyWithoutDisplay(string permissionKey)
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
                .Select(NormalizePermissionKey)
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
                    return Serialize(Grant("Dashboard", "View").Concat(Grant("Students", "View", "Add", "Edit", "Search", "Print", "ExportExcel", "ExportPDF"))
                        .Concat(Grant("Enrollment", "View", "Add", "Edit", "Search", "Print"))
                        .Concat(Grant("ClassAssignment", "View", "Add", "Edit", "Search"))
                        .Concat(Grant("Teachers", "View", "Add", "Edit", "Search"))
                        .Concat(Grant("Subjects", "View", "Add", "Edit"))
                        .Concat(Grant("Classes", "View", "Add", "Edit"))
                        .Concat(Grant("Rooms", "View", "Add", "Edit"))
                        .Concat(Grant("Timetable", "View", "Add", "Edit", "Print"))
                        .Concat(Grant("Attendance", "View", "Add", "Edit", "Print"))
                        .Concat(Grant("Grades", "View", "Add", "Edit", "Approve", "Print"))
                        .Concat(Grant("Reports", "View", "Print", "ExportExcel", "ExportCsv", "ExportPDF")));
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
                    return Serialize(Grant("Dashboard", "View").Concat(Grant("Fees", "View", "Add", "Edit", "Search", "Print", "ExportExcel"))
                        .Concat(Grant("FeePlans", "View", "Add", "Edit"))
                        .Concat(Grant("Vouchers", "View", "Add", "Edit", "Print", "ExportExcel"))
                        .Concat(Grant("Expenses", "View", "Add", "Edit", "Print", "ExportExcel"))
                        .Concat(Grant("Payroll", "View", "Search", "Print", "ExportExcel"))
                        .Concat(Grant("Reports", "View", "Print", "ExportExcel", "ExportCsv", "ExportPDF")));
                case "المكتبة":
                    return Serialize(Grant("Dashboard", "View").Concat(Grant("Library", "View", "Add", "Edit", "Delete", "Search", "Print"))
                        .Concat(Grant("Reports", "View")));
                case "النقل":
                    return Serialize(Grant("Dashboard", "View").Concat(Grant("Transport", "View", "Add", "Edit", "Delete", "Search", "Print"))
                        .Concat(Grant("Reports", "View")));
                case "التقارير":
                    return Serialize(Grant("Dashboard", "View").Concat(Grant("Reports", "View", "Print", "ExportExcel", "ExportCsv", "ExportPDF")));
                default:
                    return string.Empty;
            }
        }
    }
}
