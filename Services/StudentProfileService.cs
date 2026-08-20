using System;
using SchoolSystem.DataAccess;
using SchoolSystem.Models;
using SchoolSystem.Security;

namespace SchoolSystem.Services
{
    public class StudentProfileService
    {
        private readonly StudentProfileRepository repository = new StudentProfileRepository();

        public StudentProfile GetProfile(int studentId)
        {
            if (studentId <= 0)
                throw new ArgumentException("رقم الطالب غير صحيح.");

            if (!CurrentUser.CanAccessModule("Students"))
                throw new UnauthorizedAccessException("ليس لديك صلاحية عرض ملفات الطلاب.");

            bool includeFinancials = CurrentUser.CanAccessModule("Fees");
            return repository.GetProfile(studentId, includeFinancials);
        }
    }
}
