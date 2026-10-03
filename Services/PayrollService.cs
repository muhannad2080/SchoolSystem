using System;
using System.Data;
using SchoolSystem.DataAccess.Repositories;
using SchoolSystem.Models;
using SchoolSystem.Security;
using SchoolSystem.Helpers;


namespace SchoolSystem.Services
{

    /// <summary>عمليات الرواتب — العقد مطابق لجدول dbo.Payroll الحقيقي.</summary>
    public class PayrollService
    {
        private readonly PayrollRepository repository = new PayrollRepository();
        private readonly VoucherService voucherService = new VoucherService();
        private readonly AuditLogService auditLogService = new AuditLogService();

        public DataTable GetAllPayrolls()
        {
            CurrentUser.DemandAction("Payroll", "View", "ليس لديك صلاحية عرض الرواتب.");

            return repository.GetAllPayrolls();
        }

        public DataTable GetPayrollsByTeacherAndMonth(int teacherId, int month, int year)
        {
            CurrentUser.DemandAction("Payroll", "View", "ليس لديك صلاحية عرض الرواتب.");

            return repository.GetPayrollsByTeacherAndMonth(teacherId, month, year);
        }

        public bool PayrollExists(int teacherId, int month, int year, int excludeId = 0)
        {
            CurrentUser.DemandAction("Payroll", "View", "ليس لديك صلاحية التحقق من الرواتب.");

            return repository.PayrollExists(teacherId, month, year, excludeId);
        }

        /// <summary>
        /// يضيف راتباً جديداً ثم ينشئ سند صرف مرتبطاً به؛ عند فشل إنشاء السند يُحذف الراتب
        /// حتى لا يبقى سجل بلا مستند مالي مقابل.
        /// </summary>
        public int AddPayroll(Payroll payroll)
        {
            CurrentUser.DemandAction("Payroll", "Add", "ليس لديك صلاحية إضافة الرواتب.");

            ValidatePayroll(payroll);

            if (PayrollExists(payroll.TeacherID, payroll.SalaryMonth, payroll.SalaryYear))
                throw new InvalidOperationException("هناك راتب مسجل لنفس المعلم في نفس الشهر والسنة.");

            int payrollId = repository.AddPayroll(payroll);

            try
            {
                bool voucherCreated = voucherService.CreatePaymentVoucherForPayroll(
                    payroll.NetSalary,
                    payroll.PaymentDate ?? DateTime.Today,
                    repository.GetTeacherName(payroll.TeacherID),
                    payrollId,
                    string.IsNullOrWhiteSpace(payroll.PaymentMethod) ? "نقداً" : payroll.PaymentMethod.Trim(),
                    payroll.SalaryMonth,
                    payroll.SalaryYear,
                    "تم إنشاء سند صرف الراتب تلقائياً من سجل الرواتب.");

                if (!voucherCreated)
                    throw new InvalidOperationException("تعذر إنشاء سند صرف الراتب.");
            }
            catch (Exception ex)
            {
                ApplicationLogger.LogException("خدمة " + nameof(PayrollService) + ": إنشاء سند صرف الراتب تلقائياً", ex);
                repository.DeletePayroll(payrollId);
                throw;
            }

            auditLogService.Record("إنشاء", "Payroll", payrollId.ToString(),
                string.Format("المعلم: {0}، الشهر: {1}، السنة: {2}، الصافي: {3}",
                    payroll.TeacherName ?? payroll.TeacherID.ToString(),
                    payroll.SalaryMonth, payroll.SalaryYear, payroll.NetSalary));

            return payrollId;
        }

        private static void ValidatePayroll(Payroll payroll)
        {
            if (payroll == null)
                throw new ArgumentNullException("payroll");

            if (payroll.TeacherID <= 0)
                throw new ArgumentException("المعلم غير صحيح.");

            if (payroll.SalaryMonth < 1 || payroll.SalaryMonth > 12)
                throw new ArgumentException("شهر الراتب يجب أن يكون بين 1 و12.");

            if (payroll.SalaryYear < 2000 || payroll.SalaryYear > 2100)
                throw new ArgumentException("سنة الراتب غير صحيحة.");

            if (!payroll.PaymentDate.HasValue)
                throw new ArgumentException("تاريخ الصرف مطلوب.");

            if (payroll.BasicSalary <= 0)
                throw new ArgumentException("الراتب الأساسي يجب أن يكون أكبر من صفر.");

            if (payroll.Allowances < 0 || payroll.Deductions < 0)
                throw new ArgumentException("البدلات والخصومات لا يمكن أن تكون سالبة.");

            if (payroll.Deductions > payroll.BasicSalary + payroll.Allowances)
                throw new ArgumentException("الخصومات لا يمكن أن تتجاوز إجمالي المستحق.");

            // الحساب المالي يجب أن يتم داخل طبقة الخدمة قبل إنشاء سند الصرف،
            // ولا يعتمد على قيمة محسوبة في الواجهة.
            payroll.NetSalary =
                payroll.BasicSalary +
                payroll.Allowances -
                payroll.Deductions;

            string method = string.IsNullOrWhiteSpace(payroll.PaymentMethod)
                ? "نقداً"
                : payroll.PaymentMethod.Trim();

            string[] supported = { "نقداً", "حوالة", "بنك", "محفظة إلكترونية" };
            bool valid = false;
            foreach (string item in supported)
            {
                if (string.Equals(item, method, StringComparison.Ordinal))
                {
                    valid = true;
                    break;
                }
            }

            if (!valid)
                throw new ArgumentException("طريقة الصرف غير صحيحة.");

            payroll.PaymentMethod = method;
        }

        public bool UpdatePayroll(Payroll payroll)
        {
            CurrentUser.DemandAction("Payroll", "Edit", "ليس لديك صلاحية تعديل الرواتب.");

            if (PayrollExists(payroll.TeacherID, payroll.SalaryMonth, payroll.SalaryYear, payroll.PayrollID))
                throw new InvalidOperationException("تم صرف راتب هذا المعلم لهذا الشهر مسبقاً.");

            bool updated = repository.UpdatePayroll(payroll);

            if (updated)
                auditLogService.Record("تعديل", "Payroll", payroll.PayrollID.ToString(),
                    string.Format("المعلم: {0}، الشهر: {1}، السنة: {2}، الصافي: {3}",
                        payroll.TeacherName ?? payroll.TeacherID.ToString(),
                        payroll.SalaryMonth, payroll.SalaryYear, payroll.NetSalary));

            return updated;
        }

        public bool DeletePayroll(int payrollId)
        {
            CurrentUser.DemandAction("Payroll", "Delete", "ليس لديك صلاحية حذف الرواتب.");

            if (payrollId <= 0)
                throw new InvalidOperationException("رقم سجل الراتب غير صحيح.");

            bool deleted = repository.DeletePayroll(payrollId);

            if (deleted)
                auditLogService.Record("حذف", "Payroll", payrollId.ToString(), "تم حذف سجل راتب غير مصروف.");

            return deleted;
        }
    }
}
