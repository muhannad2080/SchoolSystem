using System;
using System.Data;
using SchoolSystem.DataAccess;
using SchoolSystem.Models;
using SchoolSystem.Security;

namespace SchoolSystem.Services
{
    public class EnrollmentService
    {
        private readonly EnrollmentRepository repository = new EnrollmentRepository();
        private readonly AuditLogService auditLogService = new AuditLogService();

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

            bool added = repository.AddEnrollment(enrollment);
            if (added)
            {
                auditLogService.Record("إنشاء", "Enrollment", enrollment.EnrollmentID.ToString(),
                    "إضافة طلب تسجيل للطالب رقم " + enrollment.StudentID);
            }

            return added;
        }

        public bool UpdateEnrollment(Enrollment enrollment)
        {
            CurrentUser.DemandPermission(PermissionKeys.EnrollmentManage, "ليس لديك صلاحية إدارة التسجيل.");
            ValidateEnrollment(enrollment, true);

            if (repository.IsStudentEnrolled(enrollment.StudentID, enrollment.AcademicYear, enrollment.EnrollmentID))
                throw new Exception("لا يمكن تعديل التسجيل: الطالب لديه تسجيل آخر فعال في هذا العام الدراسي.");

            bool updated = repository.UpdateEnrollment(enrollment);
            if (updated)
            {
                auditLogService.Record("تعديل", "Enrollment", enrollment.EnrollmentID.ToString(),
                    "تعديل طلب تسجيل الطالب رقم " + enrollment.StudentID);
            }

            return updated;
        }

        public bool DeleteEnrollment(int enrollmentId)
        {
            CurrentUser.DemandPermission(PermissionKeys.EnrollmentManage, "ليس لديك صلاحية إدارة التسجيل.");
            if (enrollmentId <= 0)
                throw new ArgumentException("رقم طلب التسجيل غير صحيح.");

            bool deleted = repository.DeleteEnrollment(enrollmentId);
            if (deleted)
                auditLogService.Record("حذف", "Enrollment", enrollmentId.ToString(), "حذف طلب تسجيل.");

            return deleted;
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

            ValidateAcademicYear(enrollment.AcademicYear);

            if (enrollment.ApplicationDate.Date > DateTime.Today)
                throw new ArgumentException("لا يمكن أن يكون تاريخ طلب التسجيل في المستقبل.");

            if (string.IsNullOrWhiteSpace(enrollment.ApplicationType))
                throw new ArgumentException("يجب اختيار نوع التسجيل.");

            if (string.IsNullOrWhiteSpace(enrollment.Status))
                throw new ArgumentException("يجب اختيار حالة الطلب.");

            if (enrollment.RegistrationFee < 0 || enrollment.PaidAmount < 0)
                throw new ArgumentException("الرسوم والمبالغ المدفوعة لا يمكن أن تكون سالبة.");

            if (enrollment.PaidAmount > enrollment.RegistrationFee)
                throw new ArgumentException("المبلغ المدفوع لا يمكن أن يكون أكبر من رسوم التسجيل.");
        }

        private void ValidateAcademicYear(string academicYear)
        {
            string value = academicYear == null ? "" : academicYear.Trim();
            string[] parts = value.Split('/');
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
