using System;
using SchoolSystem.DataAccess;
using SchoolSystem.Models;
using SchoolSystem.Security;


namespace SchoolSystem.Services
{

    public class StudentProfileService
    {
        // تعريف الدالة StudentProfileRepository لتنفيذ العملية المرتبطة بها.
        private readonly StudentProfileRepository repository = new StudentProfileRepository();

        // تعريف الدالة GetProfile لتنفيذ العملية المرتبطة بها.
        public StudentProfile GetProfile(int studentId)
        {
            if (studentId <= 0)

                throw new ArgumentException("رقم الطالب غير صحيح.");


            if (!CurrentUser.CanAccessModule("Students"))

                throw new UnauthorizedAccessException("ليس لديك صلاحية عرض ملفات الطلاب.");

            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            bool includeFinancials = CurrentUser.CanAccessModule("Fees");

            return repository.GetProfile(studentId, includeFinancials);
        }
    }
}
