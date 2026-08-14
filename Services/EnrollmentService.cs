using System;
using System.Data;
using System.Text.RegularExpressions;
using SchoolSystem.DataAccess;
using SchoolSystem.Models;
using SchoolSystem.Security;

namespace SchoolSystem.Services
{
    public class EnrollmentService
    {
        private readonly EnrollmentRepository repository = new EnrollmentRepository();

        public DataTable GetAllEnrollments()
        {
            CurrentUser.DemandPermission(PermissionKeys.EnrollmentManage, "ليس لديك صلاحية إدارة التسجيل.");
            return repository.GetAllEnrollments();
        }

        public bool AddEnrollment(Enrollment enrollment)
        {
            CurrentUser.DemandPermission(PermissionKeys.EnrollmentManage, "ليس لديك صلاحية إدارة التسجيل.");
            ValidateEnrollment(enrollment, false);

            if (repository.IsStudentEnrolled(enrollment.StudentID, enrollment.AcademicYear))
                throw new Exception("هذا الطالب مسجل بالفعل في هذا العام الدراسي.");

            return repository.AddEnrollment(enrollment);
        }

        public bool UpdateEnrollment(Enrollment enrollment)
        {
            CurrentUser.DemandPermission(PermissionKeys.EnrollmentManage, "ليس لديك صلاحية إدارة التسجيل.");
            ValidateEnrollment(enrollment, true);
            return repository.UpdateEnrollment(enrollment);
        }

        public bool DeleteEnrollment(int enrollmentId)
        {
            CurrentUser.DemandPermission(PermissionKeys.EnrollmentManage, "ليس لديك صلاحية إدارة التسجيل.");
            if (enrollmentId <= 0)
                throw new ArgumentException("رقم طلب التسجيل غير صحيح.");

            return repository.DeleteEnrollment(enrollmentId);
        }

        private void ValidateEnrollment(Enrollment enrollment, bool isUpdate)
        {
            if (enrollment == null)
                throw new ArgumentException("بيانات التسجيل غير صحيحة.");

            if (isUpdate && enrollment.EnrollmentID <= 0)
                throw new ArgumentException("اختر طلب تسجيل صحيح للتعديل.");

            if (enrollment.StudentID <= 0)
                throw new ArgumentException("يجب اختيار الطالب.");

            if (enrollment.ClassID <= 0)
                throw new ArgumentException("يجب اختيار الصف المطلوب.");

            if (string.IsNullOrWhiteSpace(enrollment.AcademicYear))
                throw new ArgumentException("العام الدراسي مطلوب.");

            if (!Regex.IsMatch(enrollment.AcademicYear.Trim(), @"^[0-9]{4}/[0-9]{4}$"))
                throw new ArgumentException("صيغة العام الدراسي يجب أن تكون مثل 2026/2027.");

            if (string.IsNullOrWhiteSpace(enrollment.ApplicationType))
                throw new ArgumentException("يجب اختيار نوع التسجيل.");

            if (string.IsNullOrWhiteSpace(enrollment.Status))
                throw new ArgumentException("يجب اختيار حالة الطلب.");

            if (enrollment.RegistrationFee < 0 || enrollment.PaidAmount < 0)
                throw new ArgumentException("الرسوم والمبالغ المدفوعة لا يمكن أن تكون سالبة.");

            if (enrollment.PaidAmount > enrollment.RegistrationFee)
                throw new ArgumentException("المبلغ المدفوع لا يمكن أن يكون أكبر من رسوم التسجيل.");
        }
    }
}
