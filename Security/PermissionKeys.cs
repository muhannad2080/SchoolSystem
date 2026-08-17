using System;
using System.Collections.Generic;
using System.Linq;

namespace SchoolSystem.Security
{
    public static class PermissionKeys
    {
        public const string DashboardView = "Dashboard.View";

        public const string StudentsView = "Students.View";
        public const string StudentsManage = "Students.Manage";
        public const string EnrollmentManage = "Enrollment.Manage";
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

        public const string SystemAdministratorRole = "مدير النظام";

        public static IReadOnlyList<string> All
        {
            get
            {
                return new[]
                {
                    DashboardView,
                    StudentsView,
                    StudentsManage,
                    EnrollmentManage,
                    ClassAssignmentManage,
                    TeachersManage,
                    StaffAttendanceManage,
                    PayrollManage,
                    SubjectsManage,
                    ClassesManage,
                    TimetableManage,
                    AttendanceManage,
                    GradesManage,
                    FeesManage,
                    VouchersManage,
                    ExpensesManage,
                    LibraryManage,
                    TransportManage,
                    ReportsView,
                    UsersManage,
                    AuditLogsView,
                    SettingsManage
                };
            }
        }

        public static string GetDisplayName(string permissionKey)
        {
            switch (NormalizePermissionKey(permissionKey))
            {
                case DashboardView: return "عرض لوحة التحكم";
                case StudentsView: return "عرض الطلاب";
                case StudentsManage: return "إدارة الطلاب";
                case EnrollmentManage: return "القبول والتسجيل";
                case ClassAssignmentManage: return "توزيع الطلاب على الفصول";
                case TeachersManage: return "إدارة المعلمين";
                case StaffAttendanceManage: return "حضور وانصراف الموظفين";
                case PayrollManage: return "الرواتب والعقود";
                case SubjectsManage: return "إدارة المواد";
                case ClassesManage: return "إدارة الصفوف والفصول";
                case TimetableManage: return "الجداول الدراسية";
                case AttendanceManage: return "حضور الطلاب";
                case GradesManage: return "إدارة الدرجات";
                case FeesManage: return "الرسوم الدراسية";
                case VouchersManage: return "السندات قبض/صرف";
                case ExpensesManage: return "المصروفات";
                case LibraryManage: return "المكتبة";
                case TransportManage: return "النقل";
                case ReportsView: return "التقارير والطباعة والتصدير";
                case UsersManage: return "إدارة المستخدمين والصلاحيات";
                case AuditLogsView: return "عرض سجل التدقيق";
                case SettingsManage: return "الإعدادات والنسخ الاحتياطي";
                default: return permissionKey ?? string.Empty;
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
            return string.Equals(
                NormalizeRoleName(roleName),
                SystemAdministratorRole,
                StringComparison.OrdinalIgnoreCase);
        }

        public static string NormalizePermissionKey(string permissionKey)
        {
            if (string.IsNullOrWhiteSpace(permissionKey))
                return string.Empty;

            string value = permissionKey.Trim();

            // تقبل النسخ القديمة التي كانت تحفظ "المفتاح - الوصف" أو اسم العرض
            // بدلاً من المفتاح البرمجي. هذا مهم حتى لا تتحول قيمة قديمة غير مفهومة
            // إلى صلاحيات الدور الافتراضية المختصرة عند تسجيل الدخول.
            int separatorIndex = value.IndexOf(" - ", StringComparison.Ordinal);
            if (separatorIndex > 0)
                value = value.Substring(0, separatorIndex).Trim();

            string exactKey = All.FirstOrDefault(
                key => string.Equals(key, value, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(exactKey))
                return exactKey;

            string displayKey = All.FirstOrDefault(
                key => string.Equals(GetDisplayName(key), value, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(displayKey))
                return displayKey;

            switch (value)
            {
                case "Dashboard": return DashboardView;
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

        public static string NormalizePermissions(string permissions)
        {
            if (string.IsNullOrWhiteSpace(permissions))
                return string.Empty;

            IEnumerable<string> normalized = permissions
                .Split(new[] { ',', ';', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(NormalizePermissionKey)
                .Where(key => !string.IsNullOrWhiteSpace(key));

            return Serialize(normalized);
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

        public static string GetRoleDefaults(string roleName)
        {
            switch (NormalizeRoleName(roleName))
            {
                case SystemAdministratorRole:
                    return Serialize(All);
                case "الإدارة":
                    return Serialize(new[]
                    {
                        DashboardView, StudentsView, StudentsManage,
                        EnrollmentManage, ClassAssignmentManage,
                        TeachersManage, SubjectsManage, ClassesManage,
                        TimetableManage, AttendanceManage, GradesManage,
                        ReportsView
                    });
                case "شؤون الطلاب":
                    return Serialize(new[]
                    {
                        DashboardView, StudentsView, StudentsManage,
                        EnrollmentManage, ClassAssignmentManage,
                        AttendanceManage, GradesManage, ReportsView
                    });
                case "المعلمون":
                    return Serialize(new[]
                    {
                        DashboardView, StudentsView, AttendanceManage,
                        GradesManage, TimetableManage, ReportsView
                    });
                case "المالية":
                    return Serialize(new[]
                    {
                        DashboardView, FeesManage, VouchersManage,
                        ExpensesManage, PayrollManage, ReportsView
                    });
                case "المكتبة":
                    return Serialize(new[] { DashboardView, LibraryManage, ReportsView });
                case "النقل":
                    return Serialize(new[] { DashboardView, TransportManage, ReportsView });
                case "التقارير":
                    return Serialize(new[] { DashboardView, ReportsView });
                default:
                    return string.Empty;
            }
        }
    }
}
