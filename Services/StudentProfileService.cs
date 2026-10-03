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


            CurrentUser.DemandAction(
                "Students",
                "View",
                "ليس لديك صلاحية عرض ملفات الطلاب.");

            // البيانات المالية تُضمّن فقط لمن يملك صلاحية عرض الرسوم فعلياً.
            bool includeFinancials = CurrentUser.CanView("Fees");

            return repository.GetProfile(studentId, includeFinancials);
        }
    }
}
