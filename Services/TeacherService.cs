using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using SchoolSystem.DataAccess;
using SchoolSystem.Models;
using SchoolSystem.Security;

namespace SchoolSystem.Services
{
    public class TeacherService
    {
        private readonly TeacherRepository _repository;
        private readonly AuditLogService _auditLogService = new AuditLogService();

        public TeacherService()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SchoolDBConnection"]?.ConnectionString 
                                   ?? @"Data Source=.;Initial Catalog=SchoolDB;Integrated Security=True;MultipleActiveResultSets=True;";
            _repository = new TeacherRepository(connectionString);
        }

        public DataTable GetAllTeachers()
        {
            CurrentUser.DemandAny(
                "ليس لديك صلاحية عرض بيانات المعلمين.",
                PermissionKeys.TeachersManage,
                PermissionKeys.PayrollManage,
                PermissionKeys.StaffAttendanceManage,
                PermissionKeys.LibraryManage,
                PermissionKeys.TimetableManage);
            return _repository.GetAllTeachers();
        }

        public bool IsNationalIDUnique(string nationalID, int? excludeTeacherId = null)
        {
            DemandTeacherLookupAccess();
            return _repository.IsNationalIDUnique(nationalID, excludeTeacherId);
        }

        public bool IsEmailUnique(string email, int? excludeTeacherId = null)
        {
            DemandTeacherLookupAccess();
            return _repository.IsEmailUnique(email, excludeTeacherId);
        }

        public void AddTeacher(Teacher teacher)
        {
            CurrentUser.DemandPermission(PermissionKeys.TeachersManage, "ليس لديك صلاحية إدارة المعلمين.");
            ValidateTeacher(teacher);
            
            if (!_repository.IsNationalIDUnique(teacher.NationalID))
                throw new Exception("رقم الهوية مستخدم مسبقاً لمعلم آخر.");
            
            if (!string.IsNullOrWhiteSpace(teacher.Email) && !_repository.IsEmailUnique(teacher.Email))
                throw new Exception("البريد الإلكتروني مستخدم مسبقاً.");

            _repository.AddTeacher(teacher);
            _auditLogService.Record("إنشاء", "Teacher", teacher.TeacherID.ToString(),
                "إضافة معلم: " + (teacher.FullName ?? string.Empty));
        }

        public void UpdateTeacher(Teacher teacher)
        {
            CurrentUser.DemandPermission(PermissionKeys.TeachersManage, "ليس لديك صلاحية إدارة المعلمين.");
            if (teacher.TeacherID <= 0)
                throw new Exception("يرجى اختيار معلم للتعديل.");

            ValidateTeacher(teacher);

            if (!_repository.IsNationalIDUnique(teacher.NationalID, teacher.TeacherID))
                throw new Exception("رقم الهوية مستخدم مسبقاً لمعلم آخر.");

            if (!string.IsNullOrWhiteSpace(teacher.Email) && !_repository.IsEmailUnique(teacher.Email, teacher.TeacherID))
                throw new Exception("البريد الإلكتروني مستخدم مسبقاً.");

            _repository.UpdateTeacher(teacher);
            _auditLogService.Record("تعديل", "Teacher", teacher.TeacherID.ToString(),
                "تعديل بيانات المعلم: " + (teacher.FullName ?? string.Empty));
        }

        public void DeleteTeacher(int teacherId)
        {
            CurrentUser.DemandPermission(PermissionKeys.TeachersManage, "ليس لديك صلاحية إدارة المعلمين.");
            if (teacherId <= 0)
                throw new Exception("يرجى اختيار معلم للحذف.");
            _repository.DeleteTeacher(teacherId);
            _auditLogService.Record("حذف", "Teacher", teacherId.ToString(),
                "حذف سجل المعلم.");
        }

        public int GetMaxEmployeeNumberSuffix(int year)
        {
            DemandTeacherLookupAccess();
            return _repository.GetMaxEmployeeNumberSuffix(year);
        }

        public void ValidateTeacher(Teacher teacher)
        {
            if (teacher == null)
                throw new Exception("بيانات المعلم غير صحيحة.");

            if (string.IsNullOrWhiteSpace(teacher.FullName))
                throw new Exception("الاسم الكامل مطلوب.");

            if (ContainsDigits(teacher.FullName))
                throw new Exception("الاسم الكامل لا يجب أن يحتوي على أرقام.");

            if (string.IsNullOrWhiteSpace(teacher.Gender))
                throw new Exception("يرجى اختيار الجنس.");

            if (string.IsNullOrWhiteSpace(teacher.Phone))
                throw new Exception("رقم الهاتف مطلوب.");

            string phone = NormalizeDigits(teacher.Phone).Trim();

            if (!phone.All(char.IsDigit))
                throw new Exception("رقم الهاتف يجب أن يحتوي على أرقام فقط.");

            if (phone.Length < 7 || phone.Length > 15)
                throw new Exception("رقم الهاتف غير صحيح.");

            if (!teacher.HireDate.HasValue)
                throw new Exception("تاريخ التعيين مطلوب.");

            if (teacher.BasicSalary < 0)
                throw new Exception("الراتب الأساسي لا يمكن أن يكون سالباً.");

            if (teacher.TransportAllowance < 0)
                throw new Exception("بدل النقل لا يمكن أن يكون سالباً.");

            if (teacher.HousingAllowance < 0)
                throw new Exception("بدل السكن لا يمكن أن يكون سالباً.");

            if (!string.IsNullOrWhiteSpace(teacher.Email))
            {
                if (!Regex.IsMatch(teacher.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                    throw new Exception("البريد الإلكتروني غير صحيح.");
            }

            if (!string.IsNullOrWhiteSpace(teacher.NationalID))
            {
                if (!Regex.IsMatch(teacher.NationalID, @"^[0-9]{6,20}$"))
                    throw new Exception("رقم الهوية يجب أن يحتوي على أرقام فقط.");
            }
        }

        private void DemandTeacherLookupAccess()
        {
            CurrentUser.DemandAny(
                "ليس لديك صلاحية عرض بيانات المعلمين.",
                PermissionKeys.TeachersManage,
                PermissionKeys.PayrollManage,
                PermissionKeys.StaffAttendanceManage,
                PermissionKeys.LibraryManage,
                PermissionKeys.TimetableManage);
        }

        private bool ContainsDigits(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            return NormalizeDigits(value).Any(char.IsDigit);
        }

        private string NormalizeDigits(string value)
        {
            if (string.IsNullOrEmpty(value))
                return "";

            return value
                .Replace('٠', '0')
                .Replace('١', '1')
                .Replace('٢', '2')
                .Replace('٣', '3')
                .Replace('٤', '4')
                .Replace('٥', '5')
                .Replace('٦', '6')
                .Replace('٧', '7')
                .Replace('٨', '8')
                .Replace('٩', '9');
        }
    }
}
