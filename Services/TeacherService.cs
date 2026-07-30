using System;
using System.Data;
using SchoolSystem.DataAccess;
using SchoolSystem.Models;

namespace SchoolSystem.Services
{
    public class TeacherService
    {
        private readonly TeacherRepository _repository;

        // المُنشئ الافتراضي يحصل على سلسلة الاتصال من متغير بيئة أو يستخدم قيمة ثابتة
        public TeacherService() : this(GetDefaultConnectionString())
        {
        }

        public TeacherService(string connectionString)
        {
            _repository = new TeacherRepository(connectionString);
        }

        private static string GetDefaultConnectionString()
        {
            // يحاول قراءة سلسلة الاتصال من متغير بيئة، وإلا يستخدم السلسلة الافتراضية
            return Environment.GetEnvironmentVariable("SchoolDBConnection")
                   ?? "Server=.;Database=SchoolDB;Integrated Security=True;";
        }

        public DataTable GetAllTeachers() => _repository.GetAllTeachers();
        public void AddTeacher(Teacher teacher) => _repository.AddTeacher(teacher);
        public void UpdateTeacher(Teacher teacher) => _repository.UpdateTeacher(teacher);
        public void DeleteTeacher(int teacherId) => _repository.DeleteTeacher(teacherId);
        public int GetMaxEmployeeNumberSuffix(int year) => _repository.GetMaxEmployeeNumberSuffix(year);
        public bool IsNationalIDUnique(string nationalID, int? excludeTeacherId = null) => _repository.IsNationalIDUnique(nationalID, excludeTeacherId);
        public bool IsEmailUnique(string email, int? excludeTeacherId = null) => _repository.IsEmailUnique(email, excludeTeacherId);
    }
}