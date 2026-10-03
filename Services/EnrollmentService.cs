using System;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using SchoolSystem.DataAccess;
using SchoolSystem.Models;
using SchoolSystem.Security;


namespace SchoolSystem.Services
{

    public class EnrollmentService
    {
        // تعريف الدالة EnrollmentRepository لتنفيذ العملية المرتبطة بها.
        private readonly EnrollmentRepository repository = new EnrollmentRepository();
        // تعريف الدالة AuditLogService لتنفيذ العملية المرتبطة بها.
        private readonly AuditLogService auditLogService = new AuditLogService();
        // تعريف الدالة FeeService لتنفيذ العملية المرتبطة بها.
        private readonly FeeService feeService = new FeeService();
        // تعريف الدالة VoucherService لتنفيذ العملية المرتبطة بها.
        private readonly VoucherService voucherService = new VoucherService();

        // تعريف الدالة GetAllEnrollments لتنفيذ العملية المرتبطة بها.
        public DataTable GetAllEnrollments()
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.DemandAction("Enrollment", "View", "ليس لديك صلاحية عرض التسجيل.");

            return repository.GetAllEnrollments();
        }

        // تعريف الدالة GenerateNextSeatNumber لتنفيذ العملية المرتبطة بها.
        public string GenerateNextSeatNumber(string academicYear, int classId, string section)
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.DemandAction("Enrollment", "Add", "ليس لديك صلاحية إضافة التسجيل.");

            string normalizedYear = (academicYear ?? string.Empty).Trim().Replace('-', '/');
            string normalizedSection = (section ?? string.Empty).Trim();
            if (classId <= 0)
                return string.Empty;

            ValidateAcademicYear(normalizedYear);

            return repository.GenerateNextSeatNumber(normalizedYear, classId, normalizedSection);
        }

        // تعريف الدالة AddEnrollment لتنفيذ العملية المرتبطة بها.
        public bool AddEnrollment(Enrollment enrollment)
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.DemandAction("Enrollment", "Add", "ليس لديك صلاحية إضافة التسجيل.");
            NormalizeEnrollment(enrollment);
            ValidateEnrollment(enrollment, false);
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
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

        // تعريف الدالة UpdateEnrollment لتنفيذ العملية المرتبطة بها.
        public bool UpdateEnrollment(Enrollment enrollment)
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.DemandAction("Enrollment", "Edit", "ليس لديك صلاحية تعديل التسجيل.");
            NormalizeEnrollment(enrollment);
            ValidateEnrollment(enrollment, true);
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
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

        // تعريف الدالة DeleteEnrollment لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة EnsureFinancePermissionIfRequired لتنفيذ العملية المرتبطة بها.
        private void EnsureFinancePermissionIfRequired(Enrollment enrollment, bool isUpdate)
        {
            if (enrollment == null)
                return;

            bool accepted = string.Equals(enrollment.Status, "مقبول", StringComparison.OrdinalIgnoreCase)
                || string.Equals(enrollment.Status, "Accepted", StringComparison.OrdinalIgnoreCase)
                || string.Equals(enrollment.Status, "Approved", StringComparison.OrdinalIgnoreCase)
                || string.Equals(enrollment.Status, "نشط", StringComparison.OrdinalIgnoreCase)
                || string.Equals(enrollment.Status, "Active", StringComparison.OrdinalIgnoreCase);

            if (enrollment.RegistrationFee <= 0 && !accepted)
                return;

            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            bool canManageFee = isUpdate ? CurrentUser.CanEdit("Fees") : CurrentUser.CanAdd("Fees");
            if (!canManageFee)

                throw new UnauthorizedAccessException("يحتاج التسجيل برسوم إلى صلاحية إضافة أو تعديل الرسوم.");


            if (enrollment.PaidAmount > 0 && !CurrentUser.CanAdd("Vouchers"))

                throw new UnauthorizedAccessException("يحتاج تسجيل دفعة إلى صلاحية إضافة السندات.");
        }

        // تعريف الدالة SynchronizeRegistrationFinance لتنفيذ العملية المرتبطة بها.
        private void SynchronizeRegistrationFinance(Enrollment enrollment)
        {
            if (enrollment == null)
                return;




            if (enrollment.RegistrationFee <= 0)
                return;

            string marker = "EnrollmentID=" + enrollment.EnrollmentID;
            string paymentMethod = string.IsNullOrWhiteSpace(enrollment.PaymentMethod)
                ? "نقدي"
                : enrollment.PaymentMethod.Trim();

            int feeId = feeService.CreateRegistrationFeeIfMissing(
                enrollment.StudentID,
                enrollment.AcademicYear,
                enrollment.RegistrationFee,
                enrollment.PaidAmount,
                paymentMethod,
                enrollment.ApplicationDate,
                marker);

            /* سند القبض يُنشأ داخل CreateRegistrationFeeIfMissing في نفس المعاملة
               مع الرسم الجديد عند وجود دفعة؛ إعادة الحفظ لا تكرر السند. */
        }

        private static readonly string[] ValidEnrollmentStatuses = new[]
        {
            "جديد", "قيد المراجعة", "مقبول", "مؤجل", "مرفوض", "ملغي"

        };

        // تعريف الدالة IsValidEnrollmentStatus لتنفيذ العملية المرتبطة بها.
        private bool IsValidEnrollmentStatus(string status)
        {
            if (string.IsNullOrWhiteSpace(status)) return false;
            return ValidEnrollmentStatuses.Any(s => string.Equals(s, status.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        // تعريف الدالة SetEnrollmentStatus لتنفيذ العملية المرتبطة بها.
        public bool SetEnrollmentStatus(int enrollmentId, string newStatus, string performedBy)
        {
            CurrentUser.DemandAny("ليس لديك صلاحية تغيير حالة التسجيل.",
                "Enrollment.Edit", "Enrollment.Approve", PermissionKeys.EnrollmentManage);

            if (enrollmentId <= 0)

                throw new ArgumentException("رقم طلب التسجيل غير صحيح.");


            if (!IsValidEnrollmentStatus(newStatus))

                throw new ArgumentException($"حالة الطلب غير صحيحة. القيم المسموح بها: {string.Join(", ", ValidEnrollmentStatuses)}.");


            using (SqlConnection connection = DbConnection.GetConnection())
            {
                string query = @"
                    UPDATE Enrollments
                    SET Status = @Status, UpdatedAt = GETDATE()
                    WHERE EnrollmentID = @EnrollmentID";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {

                    cmd.Parameters.AddWithValue("@EnrollmentID", enrollmentId);

                    cmd.Parameters.AddWithValue("@Status", newStatus.Trim());

                    connection.Open();

                    int rows = cmd.ExecuteNonQuery();

                    if (rows > 0)
                    {

                        new AuditLogService().Record("تغيير الحالة", "Enrollment", enrollmentId.ToString(),
                            $"Changed status to {newStatus} by {performedBy}");
                    }

                    return rows > 0;
                }
            }
        }

        // تعريف الدالة ApproveEnrollment لتنفيذ العملية المرتبطة بها.
        public bool ApproveEnrollment(int enrollmentId, string approvedBy)
        {

            return SetEnrollmentStatus(enrollmentId, "مقبول", approvedBy);
        }

        // تعريف الدالة RejectEnrollment لتنفيذ العملية المرتبطة بها.
        public bool RejectEnrollment(int enrollmentId, string rejectedBy)
        {

            return SetEnrollmentStatus(enrollmentId, "مرفوض", rejectedBy);
        }

        // تعريف الدالة CancelEnrollment لتنفيذ العملية المرتبطة بها.
        public bool CancelEnrollment(int enrollmentId, string cancelledBy)
        {

            return SetEnrollmentStatus(enrollmentId, "ملغي", cancelledBy);
        }

        // تعريف الدالة ValidateEnrollment لتنفيذ العملية المرتبطة بها.
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



            if (!IsValidEnrollmentStatus(enrollment.Status))

                throw new ArgumentException($"حالة الطلب غير válida. القيم المسموح بها: جديد, قيد المراجعة, مقبول, مرفوض, ملغي.");


            if (enrollment.RegistrationFee < 0 || enrollment.PaidAmount < 0)

                throw new ArgumentException("الرسوم والمبالغ المدفوعة لا يمكن أن تكون سالبة.");


            if (enrollment.PaidAmount > enrollment.RegistrationFee)

                throw new ArgumentException("المبلغ المدفوع لا يمكن أن يكون أكبر من رسوم التسجيل.");
        }

        // تعريف الدالة NormalizeEnrollment لتنفيذ العملية المرتبطة بها.
        private void NormalizeEnrollment(Enrollment enrollment)
        {
            if (enrollment == null) return;
            enrollment.AcademicYear = (enrollment.AcademicYear ?? string.Empty).Trim().Replace('-', '/');
            enrollment.Section = (enrollment.Section ?? string.Empty).Trim();

            if (enrollment.Section == "بدون شعبة" || enrollment.Section == "(بدون شعبة)")
                enrollment.Section = string.Empty;


            if (string.IsNullOrWhiteSpace(enrollment.ReceiptNo))
                enrollment.ReceiptNo = "REG-" + DateTime.Now.ToString("yyyyMMdd-HHmmss")
                    + "-" + Guid.NewGuid().ToString("N").Substring(0, 6).ToUpperInvariant();
        }

        // تعريف الدالة ValidateAcademicYear لتنفيذ العملية المرتبطة بها.
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
