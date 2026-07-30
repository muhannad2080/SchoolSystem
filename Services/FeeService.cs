using System;
using System.Data;
using SchoolSystem.DataAccess;
using SchoolSystem.Models;

namespace SchoolSystem.Services
{
    public class FeeService
    {
        private readonly FeeRepository feeRepository;

        public FeeService()
        {
            feeRepository = new FeeRepository();
        }

        public DataTable GetAllFees()
        {
            return feeRepository.GetAllFees();
        }

        public int AddFee(Fee fee)
        {
            ValidateFee(fee);
            PrepareFee(fee);
            return feeRepository.AddFee(fee);
        }

        public bool UpdateFee(Fee fee)
        {
            if (fee.FeeID <= 0)
                throw new Exception("رقم سجل الرسوم غير صحيح.");

            ValidateFee(fee);
            PrepareFee(fee);
            return feeRepository.UpdateFee(fee);
        }

        public bool DeleteFee(int feeId)
        {
            if (feeId <= 0)
                throw new Exception("رقم سجل الرسوم غير صحيح.");

            return feeRepository.DeleteFee(feeId);
        }

        public int GenerateStudentFeesFromPlans(int studentId, string academicYear)
        {
            if (studentId <= 0)
                throw new Exception("يجب اختيار الطالب.");

            if (string.IsNullOrWhiteSpace(academicYear))
                throw new Exception("يجب اختيار العام الدراسي.");

            return feeRepository.GenerateStudentFeesFromPlans(studentId, academicYear);
        }

        private void ValidateFee(Fee fee)
        {
            if (fee == null)
                throw new Exception("بيانات الرسوم غير موجودة.");

            if (fee.StudentID <= 0)
                throw new Exception("يجب اختيار الطالب.");

            if (string.IsNullOrWhiteSpace(fee.AcademicYear))
                throw new Exception("يجب اختيار العام الدراسي.");

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
        }

        private void PrepareFee(Fee fee)
        {
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
