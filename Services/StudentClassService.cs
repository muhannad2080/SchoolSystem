using System;
using System.Data;
using SchoolSystem.DataAccess;
using SchoolSystem.Models;
using SchoolSystem.Security;

namespace SchoolSystem.Services
{
    public class StudentClassService
    {
        private readonly StudentClassRepository repository = new StudentClassRepository();
        private readonly AuditLogService auditLogService = new AuditLogService();

        public DataTable GetUnassignedStudents(string academicYear)
        {
            CurrentUser.DemandPermission(PermissionKeys.ClassAssignmentManage, "ليس لديك صلاحية توزيع الطلاب.");
            ValidateAcademicYear(academicYear);
            return repository.GetUnassignedStudents(academicYear);
        }

        public DataTable GetSections(int classId, string academicYear)
        {
            CurrentUser.DemandAny("ليس لديك صلاحية قراءة الشعب الدراسية.",
                PermissionKeys.EnrollmentManage,
                PermissionKeys.ClassAssignmentManage,
                PermissionKeys.AttendanceManage,
                PermissionKeys.GradesManage,
                PermissionKeys.TimetableManage,
                PermissionKeys.ReportsView);
            if (classId <= 0)
                throw new ArgumentException("يجب اختيار الصف.");

            ValidateAcademicYear(academicYear);
            return repository.GetSections(classId, academicYear.Trim());
        }

        public DataTable GetAssignedStudents(int classId, string section, string academicYear)
        {
            CurrentUser.DemandPermission(PermissionKeys.ClassAssignmentManage, "ليس لديك صلاحية توزيع الطلاب.");
            if (classId <= 0)
                throw new ArgumentException("يجب اختيار الصف.");

            if (string.IsNullOrWhiteSpace(section))
                throw new ArgumentException("يجب اختيار الشعبة.");

            ValidateAcademicYear(academicYear);

            return repository.GetAssignedStudents(classId, section.Trim(), academicYear.Trim());
        }

        public bool AssignStudent(StudentClass assignment)
        {
            CurrentUser.DemandPermission(PermissionKeys.ClassAssignmentManage, "ليس لديك صلاحية توزيع الطلاب.");
            ValidateAssignment(assignment);

            if (repository.IsStudentAssignedInYear(assignment.StudentID, assignment.AcademicYear))
                throw new ArgumentException("هذا الطالب موزع مسبقاً في نفس العام الدراسي.");

            bool assigned = repository.AssignStudent(assignment);
            if (assigned)
            {
                auditLogService.Record(
                    "توزيع طالب على فصل",
                    "StudentClass",
                    assignment.StudentClassID.ToString(),
                    "تم توزيع الطالب رقم " + assignment.StudentID + " على الصف رقم " + assignment.ClassID + " للعام " + assignment.AcademicYear);
            }
            return assigned;
        }

        public bool RemoveAssignment(int studentClassId)
        {
            CurrentUser.DemandPermission(PermissionKeys.ClassAssignmentManage, "ليس لديك صلاحية توزيع الطلاب.");
            if (studentClassId <= 0)
                throw new ArgumentException("اختر طالباً موزعاً من الجدول أولاً.");

            bool removed = repository.RemoveAssignment(studentClassId);
            if (removed)
            {
                auditLogService.Record(
                    "إلغاء توزيع طالب",
                    "StudentClass",
                    studentClassId.ToString(),
                    "تم إلغاء توزيع الطالب من السجل رقم " + studentClassId);
            }
            return removed;
        }

        private void ValidateAssignment(StudentClass assignment)
        {
            if (assignment == null)
                throw new ArgumentException("بيانات التوزيع غير صحيحة.");

            if (assignment.StudentID <= 0)
                throw new ArgumentException("بيانات الطالب غير صحيحة.");

            if (assignment.ClassID <= 0)
                throw new ArgumentException("يجب اختيار الصف.");

            assignment.Section = (assignment.Section ?? string.Empty).Trim();
            if (assignment.Section.Length == 0 || assignment.Section.Length > 100)
                throw new ArgumentException("يجب اختيار شعبة صحيحة بطول لا يتجاوز 100 حرف.");

            assignment.AcademicYear = (assignment.AcademicYear ?? string.Empty).Trim();
            ValidateAcademicYear(assignment.AcademicYear);

            if (!repository.StudentExists(assignment.StudentID))
                throw new ArgumentException("الطالب المحدد غير موجود أو غير نشط.");

            if (!repository.ClassExists(assignment.ClassID))
                throw new ArgumentException("الصف المحدد غير موجود أو غير نشط.");
        }

        private void ValidateAcademicYear(string academicYear)
        {
            if (string.IsNullOrWhiteSpace(academicYear))
                throw new ArgumentException("العام الدراسي مطلوب.");

            string normalized = academicYear.Trim().Replace('-', '/');
            string[] parts = normalized.Split('/');
            int firstYear;
            int secondYear;
            if (parts.Length != 2 ||
                parts[0].Length != 4 ||
                parts[1].Length != 4 ||
                !int.TryParse(parts[0], out firstYear) ||
                !int.TryParse(parts[1], out secondYear) ||
                secondYear != firstYear + 1)
            {
                throw new ArgumentException("صيغة العام الدراسي يجب أن تكون متسلسلة مثل 2026/2027 أو 1447-1448.");
            }
        }
    }
}
