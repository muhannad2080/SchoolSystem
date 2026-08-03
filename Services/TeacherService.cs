using System;
using System.Configuration;
using System.Data;
using System.Text.RegularExpressions;
using SchoolSystem.DataAccess;
using SchoolSystem.Models;

namespace SchoolSystem.Services
{
    public class TeacherService
    {
        private readonly TeacherRepository _repository;

        public TeacherService()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SchoolDBConnection"]?.ConnectionString 
                                   ?? @"Server=MUHANNADALJRADI;Database=SchoolDB;Trusted_Connection=True;";
            _repository = new TeacherRepository(connectionString);
        }

        public DataTable GetAllTeachers() => _repository.GetAllTeachers();

        public void AddTeacher(Teacher teacher)
        {
            ValidateTeacher(teacher);
            
            if (!_repository.IsNationalIDUnique(teacher.NationalID))
                throw new Exception("رقم الهوية مستخدم مسبقاً لمعلم آخر.");
            
            if (!string.IsNullOrWhiteSpace(teacher.Email) && !_repository.IsEmailUnique(teacher.Email))
                throw new Exception("البريد الإلكتروني مستخدم مسبقاً.");

            _repository.AddTeacher(teacher);
        }

        public void UpdateTeacher(Teacher teacher)
        {
            if (teacher.TeacherID <= 0)
                throw new Exception("يرجى اختيار معلم للتعديل.");

            ValidateTeacher(teacher);

            if (!_repository.IsNationalIDUnique(teacher.NationalID, teacher.TeacherID))
                throw new Exception("رقم الهوية مستخدم مسبقاً لمعلم آخر.");

            if (!string.IsNullOrWhiteSpace(teacher.Email) && !_repository.IsEmailUnique(teacher.Email, teacher.TeacherID))
                throw new Exception("البريد الإلكتروني مستخدم مسبقاً.");

            _repository.UpdateTeacher(teacher);
        }

        public void DeleteTeacher(int teacherId)
        {
            if (teacherId <= 0)
                throw new Exception("يرجى اختيار معلم للحذف.");
            _repository.DeleteTeacher(teacherId);
        }

        public int GetMaxEmployeeNumberSuffix(int year) => _repository.GetMaxEmployeeNumberSuffix(year);

        public void ValidateTeacher(Teacher teacher)
        {
            if (teacher == null)
                throw new Exception("بيانات المعلم غير صحيحة.");

            if (string.IsNullOrWhiteSpace(teacher.FullName))
                throw new Exception("الاسم الكامل مطلوب.");

            if (string.IsNullOrWhiteSpace(teacher.Gender))
                throw new Exception("يرجى اختيار الجنس.");

            if (string.IsNullOrWhiteSpace(teacher.Phone))
                throw new Exception("رقم الهاتف مطلوب.");

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
    }
}
