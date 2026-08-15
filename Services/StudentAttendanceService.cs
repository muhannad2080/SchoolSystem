using System;
using System.Data;
using SchoolSystem.DataAccess;
using SchoolSystem.Models;
using SchoolSystem.Security;

namespace SchoolSystem.Services
{
    public class StudentAttendanceService
    {
        private readonly StudentAttendanceRepository repository = new StudentAttendanceRepository();
        private readonly AuditLogService auditLogService = new AuditLogService();

        public DataTable GetSections(int classId, string academicYear)
        {
            CurrentUser.DemandPermission(PermissionKeys.AttendanceManage, "ليس لديك صلاحية إدارة حضور الطلاب.");
            if (classId <= 0)
                throw new ArgumentException("يجب اختيار الصف.");

            if (string.IsNullOrWhiteSpace(academicYear))
                throw new ArgumentException("العام الدراسي مطلوب.");

            if (!IsValidAcademicYear(academicYear))
                throw new ArgumentException("صيغة العام الدراسي يجب أن تكون مثل 2026/2027.");

            return repository.GetSections(classId, academicYear.Trim());
        }

        public DataTable GetAttendanceSheet(int classId, string section, string academicYear, DateTime date)
        {
            CurrentUser.DemandPermission(PermissionKeys.AttendanceManage, "ليس لديك صلاحية إدارة حضور الطلاب.");
            if (classId <= 0)
                throw new ArgumentException("يجب اختيار الصف.");

            if (string.IsNullOrWhiteSpace(section))
                throw new ArgumentException("يجب اختيار الشعبة.");

            if (string.IsNullOrWhiteSpace(academicYear))
                throw new ArgumentException("العام الدراسي مطلوب.");

            if (!IsValidAcademicYear(academicYear))
                throw new ArgumentException("صيغة العام الدراسي يجب أن تكون مثل 2026/2027.");

            if (date.Date > DateTime.Today)
                throw new ArgumentException("لا يمكن تحميل حضور بتاريخ مستقبلي.");

            return repository.GetAttendanceSheet(
                classId,
                section.Trim(),
                academicYear.Trim(),
                date.Date);
        }

        public bool SaveAttendance(StudentAttendance item)
        {
            CurrentUser.DemandPermission(PermissionKeys.AttendanceManage, "ليس لديك صلاحية إدارة حضور الطلاب.");
            Validate(item);
            bool saved = repository.SaveAttendance(item);
            if (saved)
            {
                auditLogService.Record(
                    "حفظ حضور طالب",
                    "StudentAttendance",
                    item.AttendanceID.ToString(),
                    "تم حفظ سجل حضور الطالب رقم " + item.StudentID + " بتاريخ " + item.AttendanceDate.ToString("yyyy-MM-dd"));
            }
            return saved;
        }

        private void Validate(StudentAttendance item)
        {
            if (item == null)
                throw new ArgumentException("بيانات الحضور غير صحيحة.");

            if (item.StudentID <= 0)
                throw new ArgumentException("بيانات الطالب غير صحيحة.");

            if (item.ClassID <= 0)
                throw new ArgumentException("بيانات الصف غير صحيحة.");

            if (string.IsNullOrWhiteSpace(item.Section))
                throw new ArgumentException("الشعبة مطلوبة.");

            if (string.IsNullOrWhiteSpace(item.AcademicYear))
                throw new ArgumentException("العام الدراسي مطلوب.");

            if (!IsValidAcademicYear(item.AcademicYear))
                throw new ArgumentException("صيغة العام الدراسي يجب أن تكون متسلسلة مثل 2026/2027.");

            if (item.AttendanceDate == DateTime.MinValue)
                throw new ArgumentException("تاريخ الحضور مطلوب وصحيح.");

            if (item.AttendanceDate.Date > DateTime.Today)
                throw new ArgumentException("لا يمكن تسجيل حضور بتاريخ مستقبلي.");

            if (string.IsNullOrWhiteSpace(item.Status))
                throw new ArgumentException("حالة الحضور مطلوبة.");

            if (item.Status != "حاضر" &&
                item.Status != "غائب" &&
                item.Status != "متأخر" &&
                item.Status != "مستأذن")
            {
                throw new ArgumentException("حالة الحضور غير صحيحة.");
            }
        }

        private bool IsValidAcademicYear(string academicYear)
        {
            if (string.IsNullOrWhiteSpace(academicYear))
                return false;

            string[] parts = academicYear.Trim().Split('/');
            int firstYear;
            int secondYear;
            if (parts.Length != 2 || parts[0].Length != 4 || parts[1].Length != 4)
                return false;
            if (!int.TryParse(parts[0], out firstYear) || !int.TryParse(parts[1], out secondYear))
                return false;

            return firstYear >= 2000 && firstYear <= 2100 && secondYear == firstYear + 1;
        }
    }
}
