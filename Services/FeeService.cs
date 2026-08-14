using System;
using System.Data;
using SchoolSystem.DataAccess;
using SchoolSystem.Models;
using SchoolSystem.Security;

namespace SchoolSystem.Services
{
    public class FeeService
    {
        private readonly FeeRepository feeRepository;
        private readonly AuditLogService auditLogService = new AuditLogService();

        public FeeService()
        {
            feeRepository = new FeeRepository();
        }

        public DataTable GetAllFees()
        {
            EnsureCanManageFees();
            return feeRepository.GetAllFees();
        }

        public int AddFee(Fee fee)
        {
            EnsureCanManageFees();
            ValidateFee(fee);
            PrepareFee(fee);
            int feeId = feeRepository.AddFee(fee);
            if (feeId > 0)
                auditLogService.Record("إنشاء", "Fee", feeId.ToString(),
                    string.Format("الطالب: {0}، النوع: {1}، الصافي: {2}، المدفوع: {3}", fee.StudentID, fee.FeeType, fee.NetAmount, fee.PaidAmount));
            return feeId;
        }

        public bool UpdateFee(Fee fee)
        {
            EnsureCanManageFees();
            if (fee.FeeID <= 0)
                throw new Exception("رقم سجل الرسوم غير صحيح.");

            ValidateFee(fee);
            PrepareFee(fee);
            bool updated = feeRepository.UpdateFee(fee);
            if (updated)
                auditLogService.Record("تعديل", "Fee", fee.FeeID.ToString(),
                    string.Format("الطالب: {0}، النوع: {1}، الصافي: {2}، المدفوع: {3}", fee.StudentID, fee.FeeType, fee.NetAmount, fee.PaidAmount));
            return updated;
        }

        public bool DeleteFee(int feeId)
        {
            EnsureCanManageFees();
            if (feeId <= 0)
                throw new Exception("رقم سجل الرسوم غير صحيح.");

            bool deleted = feeRepository.DeleteFee(feeId);
            if (deleted)
                auditLogService.Record("حذف", "Fee", feeId.ToString(), "حذف سجل الرسوم.");
            return deleted;
        }

        public int GenerateStudentFeesFromPlans(int studentId, string academicYear)
        {
            EnsureCanManageFees();
            if (studentId <= 0)
                throw new Exception("يجب اختيار الطالب.");

            if (string.IsNullOrWhiteSpace(academicYear))
                throw new Exception("يجب اختيار العام الدراسي.");

            int generated = feeRepository.GenerateStudentFeesFromPlans(studentId, academicYear);
            if (generated > 0)
                auditLogService.Record("توليد", "Fee", studentId.ToString(),
                    string.Format("توليد {0} سجل رسوم من خطط العام الدراسي {1}.", generated, academicYear));
            return generated;
        }

        private static void EnsureCanManageFees()
        {
            if (!CurrentUser.HasPermission(PermissionKeys.FeesManage))
                throw new UnauthorizedAccessException("ليس لديك صلاحية إدارة الرسوم.");
        }

        private void ValidateFee(Fee fee)
        {
            if (fee == null)
                throw new Exception("بيانات الرسوم غير موجودة.");

            if (fee.StudentID <= 0)
                throw new Exception("يجب اختيار الطالب.");

            if (string.IsNullOrWhiteSpace(fee.AcademicYear))
                throw new Exception("يجب اختيار العام الدراسي.");

            ValidateAcademicYear(fee.AcademicYear);

            if (string.IsNullOrWhiteSpace(fee.FeeType))
                throw new Exception("يجب اختيار نوع الرسوم.");

            if (fee.TotalAmount < 0)
                throw new Exception("إجمالي الرسوم لا يمكن أن يكون أقل من صفر.");

            if (fee.DiscountAmount < 0)
                throw new Exception("الخصم لا يمكن أن يكون أقل من صفر.");

            if (fee.PaidAmount < 0)
                throw new Exception("المبلغ المدفوع لا يمكن أن يكون أقل من صفر.");

            if (fee.DiscountAmount > fee.TotalAmount)
                throw new Exception("الخصم لا يمكن أن يكون أكبر من إجمالي الرسوم.");

            if (fee.PaidAmount > fee.TotalAmount - fee.DiscountAmount)
                throw new Exception("المبلغ المدفوع لا يمكن أن يكون أكبر من صافي الرسوم.");

            if (fee.DueDate == DateTime.MinValue)
                throw new Exception("تاريخ الاستحقاق مطلوب وصحيح.");

            if (fee.PaymentDate.HasValue && fee.PaymentDate.Value.Date > DateTime.Today)
                throw new Exception("تاريخ الدفع لا يمكن أن يكون في المستقبل.");
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
                firstYear < 2000 ||
                firstYear > 2100 ||
                secondYear != firstYear + 1)
            {
                throw new Exception("صيغة العام الدراسي يجب أن تكون متسلسلة مثل 2026/2027.");
            }
        }

        private void PrepareFee(Fee fee)
        {
            fee.AcademicYear = fee.AcademicYear.Trim();
            fee.FeeType = fee.FeeType.Trim();
            fee.NetAmount = fee.TotalAmount - fee.DiscountAmount;
            fee.RemainingAmount = fee.NetAmount - fee.PaidAmount;

            if (fee.RemainingAmount < 0)
                fee.RemainingAmount = 0;

            if (fee.NetAmount == 0)
            {
                fee.Status = "معفى";
                fee.PaymentDate = null;
            }
            else if (fee.PaidAmount == 0)
            {
                fee.Status = fee.DueDate.Date < DateTime.Today ? "متأخر" : "غير مسدد";
                fee.PaymentDate = null;
            }
            else if (fee.PaidAmount >= fee.NetAmount)
            {
                fee.Status = "مسدد";

                if (!fee.PaymentDate.HasValue)
                    fee.PaymentDate = DateTime.Today;
            }
            else
            {
                fee.Status = "مسدد جزئياً";

                if (!fee.PaymentDate.HasValue)
                    fee.PaymentDate = DateTime.Today;
            }
        }
    }
}
