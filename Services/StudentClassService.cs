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

        public DataTable GetUnassignedStudents(string academicYear)
        {
            CurrentUser.DemandPermission(PermissionKeys.ClassAssignmentManage, "ليس لديك صلاحية توزيع الطلاب.");
            ValidateAcademicYear(academicYear);
            return repository.GetUnassignedStudents(academicYear);
        }

        public DataTable GetSections(int classId, string academicYear)
        {
            CurrentUser.DemandPermission(PermissionKeys.ClassAssignmentManage, "ليس لديك صلاحية توزيع الطلاب.");
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

            return repository.AssignStudent(assignment);
        }

        public bool RemoveAssignment(int studentClassId)
        {
            CurrentUser.DemandPermission(PermissionKeys.ClassAssignmentManage, "ليس لديك صلاحية توزيع الطلاب.");
            if (studentClassId <= 0)
                throw new ArgumentException("اختر طالباً موزعاً من الجدول أولاً.");

            return repository.RemoveAssignment(studentClassId);
        }

        private void ValidateAssignment(StudentClass assignment)
        {
            if (assignment == null)
                throw new ArgumentException("بيانات التوزيع غير صحيحة.");

            if (assignment.StudentID <= 0)
                throw new ArgumentException("بيانات الطالب غير صحيحة.");

            if (assignment.ClassID <= 0)
                throw new ArgumentException("يجب اختيار الصف.");

            if (string.IsNullOrWhiteSpace(assignment.Section))
                throw new ArgumentException("يجب اختيار الشعبة.");

            assignment.Section = assignment.Section.Trim();
            assignment.AcademicYear = assignment.AcademicYear.Trim();
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

            string[] parts = academicYear.Trim().Split('/');
            int firstYear;
            int secondYear;
            if (parts.Length != 2 ||
                parts[0].Length != 4 ||
                parts[1].Length != 4 ||
                !int.TryParse(parts[0], out firstYear) ||
                !int.TryParse(parts[1], out secondYear) ||
                secondYear != firstYear + 1)
            {
                throw new ArgumentException("صيغة العام الدراسي يجب أن تكون متسلسلة مثل 2026/2027.");
            }
        }
    }
}
