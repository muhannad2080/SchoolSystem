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
        private readonly FeeService feeService = new FeeService();
        private readonly VoucherService voucherService = new VoucherService();

        public DataTable GetAllEnrollments()
        {
            CurrentUser.DemandAction("Enrollment", "View", "ليس لديك صلاحية عرض التسجيل.");
            return repository.GetAllEnrollments();
        }

        public string GenerateNextSeatNumber(string academicYear, int classId, string section)
        {
            CurrentUser.DemandAction("Enrollment", "Add", "ليس لديك صلاحية إضافة التسجيل.");

            string normalizedYear = (academicYear ?? string.Empty).Trim().Replace('-', '/');
            string normalizedSection = (section ?? string.Empty).Trim();
            if (classId <= 0)
                return string.Empty;

            ValidateAcademicYear(normalizedYear);
            return repository.GenerateNextSeatNumber(normalizedYear, classId, normalizedSection);
        }

        public bool AddEnrollment(Enrollment enrollment)
        {
            CurrentUser.DemandAction("Enrollment", "Add", "ليس لديك صلاحية إضافة التسجيل.");
            NormalizeEnrollment(enrollment);
            ValidateEnrollment(enrollment, false);
            EnsureFinancePermissionIfRequired(enrollment, false);
            if (repository.IsStudentEnrolled(enrollment.StudentID, enrollment.AcademicYear))
                throw new Exception("هذا الطالب مسجل بالفعل في هذا العام الدراسي.");

            bool added = repository.AddEnrollment(enrollment);
            if (added)
            {
                auditLogService.Record("إنشاء", "Enrollment", enrollment.EnrollmentID.ToString(),
                    "إضافة طلب تسجيل للطالب رقم " + enrollment.StudentID);
                SynchronizeRegistrationFinance(enrollment);
            }

            return added;
        }

        public bool UpdateEnrollment(Enrollment enrollment)
        {
            CurrentUser.DemandAction("Enrollment", "Edit", "ليس لديك صلاحية تعديل التسجيل.");
            NormalizeEnrollment(enrollment);
            ValidateEnrollment(enrollment, true);
            EnsureFinancePermissionIfRequired(enrollment, true);

            if (repository.IsStudentEnrolled(enrollment.StudentID, enrollment.AcademicYear, enrollment.EnrollmentID))
                throw new Exception("لا يمكن تعديل التسجيل: الطالب لديه تسجيل آخر فعال في هذا العام الدراسي.");

            bool updated = repository.UpdateEnrollment(enrollment);
            if (updated)
            {
                auditLogService.Record("تعديل", "Enrollment", enrollment.EnrollmentID.ToString(),
                    "تعديل طلب تسجيل الطالب رقم " + enrollment.StudentID);
                SynchronizeRegistrationFinance(enrollment);
            }

            return updated;
        }

        public bool DeleteEnrollment(int enrollmentId)
        {
            CurrentUser.DemandAction("Enrollment", "Delete", "ليس لديك صلاحية حذف التسجيل.");
            if (enrollmentId <= 0)
                throw new ArgumentException("رقم طلب التسجيل غير صحيح.");

            bool deleted = repository.DeleteEnrollment(enrollmentId);
            if (deleted)
                auditLogService.Record("حذف", "Enrollment", enrollmentId.ToString(), "حذف طلب تسجيل.");

            return deleted;
        }

        private void EnsureFinancePermissionIfRequired(Enrollment enrollment, bool isUpdate)
        {
            if (enrollment == null || enrollment.RegistrationFee <= 0)
                return;

            bool canManageFee = isUpdate ? CurrentUser.CanEdit("Fees") : CurrentUser.CanAdd("Fees");
            if (!canManageFee)
                throw new UnauthorizedAccessException("يحتاج التسجيل برسوم إلى صلاحية إضافة أو تعديل الرسوم.");

            if (enrollment.PaidAmount > 0 && !CurrentUser.CanAdd("Vouchers"))
                throw new UnauthorizedAccessException("يحتاج تسجيل دفعة إلى صلاحية إضافة السندات.");
        }

        private void SynchronizeRegistrationFinance(Enrollment enrollment)
        {
            if (enrollment == null || enrollment.RegistrationFee <= 0)
                return;

            string marker = "EnrollmentID=" + enrollment.EnrollmentID;
            int feeId = feeService.CreateRegistrationFeeIfMissing(
                enrollment.StudentID,
                enrollment.AcademicYear,
                enrollment.RegistrationFee,
                enrollment.PaidAmount,
                enrollment.PaymentMethod,
                enrollment.ApplicationDate,
                marker);

            if (feeId > 0 && enrollment.PaidAmount > 0)
            {
                voucherService.CreateReceiptVoucherForFeePayment(
                    enrollment.PaidAmount,
                    enrollment.ApplicationDate,
                    "طالب رقم " + enrollment.StudentID,
                    feeId,
                    enrollment.PaymentMethod,
                    "سند قبض تلقائي من التسجيل؛ " + marker);
            }
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

        private void NormalizeEnrollment(Enrollment enrollment)
        {
            if (enrollment == null) return;
            enrollment.AcademicYear = (enrollment.AcademicYear ?? string.Empty).Trim().Replace('-', '/');
            enrollment.Section = (enrollment.Section ?? string.Empty).Trim();
            // Section nullable في قاعدة البيانات؛ إبقاء القيمة فارغة يسمح بالتسجيل قبل توزيع الطالب على شعبة.
            if (enrollment.Section == "بدون شعبة" || enrollment.Section == "(بدون شعبة)")
                enrollment.Section = string.Empty;
        }

        private void ValidateAcademicYear(string academicYear)
        {
            string value = academicYear == null ? "" : academicYear.Trim().Replace('-', '/');
            string[] parts = value.Replace('-', '/').Split('/');
            int firstYear;
            int secondYear;

            if (parts.Length != 2 ||
                parts[0].Length != 4 ||
                parts[1].Length != 4 ||
                !int.TryParse(parts[0], out firstYear) ||
                !int.TryParse(parts[1], out secondYear) ||
                secondYear != firstYear + 1)
            {
                throw new ArgumentException("صيغة العام الدراسي يجب أن تكون متسلسلة مثل 2026/2027 أو 1447-1448.");
            }
        }
    }
}
