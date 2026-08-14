using System;
using System.Data;
using System.Text.RegularExpressions;
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

            ValidateAcademicYear(assignment.AcademicYear);
        }

        private void ValidateAcademicYear(string academicYear)
        {
            if (string.IsNullOrWhiteSpace(academicYear))
                throw new ArgumentException("العام الدراسي مطلوب.");

            if (!Regex.IsMatch(academicYear.Trim(), @"^[0-9]{4}/[0-9]{4}$"))
                throw new ArgumentException("صيغة العام الدراسي يجب أن تكون مثل 2026/2027.");
        }
    }
}
