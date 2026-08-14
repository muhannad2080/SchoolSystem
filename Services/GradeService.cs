using System;
using System.Data;
using System.Text.RegularExpressions;
using SchoolSystem.DataAccess;
using SchoolSystem.Models;
using SchoolSystem.Security;

namespace SchoolSystem.Services
{
    public class GradeService
    {
        private readonly GradeRepository repository = new GradeRepository();

        public DataTable GetAllSubjects()
        {
            CurrentUser.DemandPermission(PermissionKeys.GradesManage, "ليس لديك صلاحية إدارة الدرجات.");
            return repository.GetAllSubjects();
        }

        public DataTable GetSubjectsByClass(int classId)
        {
            CurrentUser.DemandPermission(PermissionKeys.GradesManage, "ليس لديك صلاحية إدارة الدرجات.");
            if (classId <= 0)
                return repository.GetAllSubjects();

            return repository.GetSubjectsByClass(classId);
        }

        public DataTable GetGradeEntryStudents(int classId, string section, string academicYear, int subjectId, string termName)
        {
            CurrentUser.DemandPermission(PermissionKeys.GradesManage, "ليس لديك صلاحية إدارة الدرجات.");
            if (classId <= 0)
                throw new ArgumentException("يجب اختيار الصف.");

            if (string.IsNullOrWhiteSpace(section))
                throw new ArgumentException("يجب اختيار الشعبة.");

            if (string.IsNullOrWhiteSpace(academicYear))
                throw new ArgumentException("العام الدراسي مطلوب.");

            if (!Regex.IsMatch(academicYear.Trim(), @"^[0-9]{4}/[0-9]{4}$"))
                throw new ArgumentException("صيغة العام الدراسي يجب أن تكون مثل 2026/2027.");

            if (subjectId <= 0)
                throw new ArgumentException("يجب اختيار المادة.");

            if (string.IsNullOrWhiteSpace(termName))
                throw new ArgumentException("يجب اختيار الفصل الدراسي.");

            return repository.GetGradeEntryStudents(
                classId,
                section.Trim(),
                academicYear.Trim(),
                subjectId,
                termName.Trim());
        }

        public bool SaveGrade(StudentGrade grade)
        {
            CurrentUser.DemandPermission(PermissionKeys.GradesManage, "ليس لديك صلاحية إدارة الدرجات.");
            ValidateGrade(grade);
            CalculateGrade(grade);

            return repository.SaveGrade(grade);
        }

        public bool DeleteGrade(int gradeId)
        {
            CurrentUser.DemandPermission(PermissionKeys.GradesManage, "ليس لديك صلاحية إدارة الدرجات.");
            if (gradeId <= 0)
                throw new ArgumentException("اختر درجة صحيحة للحذف.");

            return repository.DeleteGrade(gradeId);
        }

        public void CalculateGrade(StudentGrade grade)
        {
            CurrentUser.DemandPermission(PermissionKeys.GradesManage, "ليس لديك صلاحية إدارة الدرجات.");
            grade.Total = grade.Quiz1 + grade.Quiz2 + grade.CourseWork + grade.FinalExam;

            if (grade.Total >= 90)
                grade.GradeLetter = "ممتاز";
            else if (grade.Total >= 80)
                grade.GradeLetter = "جيد جدًا";
            else if (grade.Total >= 70)
                grade.GradeLetter = "جيد";
            else if (grade.Total >= 60)
                grade.GradeLetter = "مقبول";
            else
                grade.GradeLetter = "ضعيف";

            grade.ResultStatus = grade.Total >= 50 ? "ناجح" : "راسب";
        }

        private void ValidateGrade(StudentGrade grade)
        {
            if (grade == null)
                throw new ArgumentException("بيانات الدرجة غير صحيحة.");

            if (grade.StudentID <= 0)
                throw new ArgumentException("بيانات الطالب غير صحيحة.");

            if (grade.SubjectID <= 0)
                throw new ArgumentException("بيانات المادة غير صحيحة.");

            if (grade.ClassID <= 0)
                throw new ArgumentException("بيانات الصف غير صحيحة.");

            if (string.IsNullOrWhiteSpace(grade.Section))
                throw new ArgumentException("الشعبة مطلوبة.");

            if (string.IsNullOrWhiteSpace(grade.AcademicYear))
                throw new ArgumentException("العام الدراسي مطلوب.");

            if (!Regex.IsMatch(grade.AcademicYear.Trim(), @"^[0-9]{4}/[0-9]{4}$"))
                throw new ArgumentException("صيغة العام الدراسي يجب أن تكون مثل 2026/2027.");

            if (string.IsNullOrWhiteSpace(grade.TermName))
                throw new ArgumentException("الفصل الدراسي مطلوب.");

            ValidateMark(grade.Quiz1, "الاختبار الأول");
            ValidateMark(grade.Quiz2, "الاختبار الثاني");
            ValidateMark(grade.CourseWork, "أعمال السنة");
            ValidateMark(grade.FinalExam, "الاختبار النهائي");

            decimal total = grade.Quiz1 + grade.Quiz2 + grade.CourseWork + grade.FinalExam;

            if (total > 100)
                throw new ArgumentException("مجموع الدرجات لا يمكن أن يتجاوز 100.");
        }

        private void ValidateMark(decimal value, string fieldName)
        {
            if (value < 0)
                throw new ArgumentException(fieldName + " لا يمكن أن يكون أقل من صفر.");

            if (value > 100)
                throw new ArgumentException(fieldName + " لا يمكن أن يتجاوز 100.");
        }
    }
}
