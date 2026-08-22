using System;
using System.Data;
using SchoolSystem.DataAccess;
using SchoolSystem.Models;
using SchoolSystem.Security;

namespace SchoolSystem.Services
{
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

        public bool PayrollExists(int teacherId, int month, int year, int excludeId = 0)
        {
            CurrentUser.DemandAction("Payroll", "View", "ليس لديك صلاحية التحقق من الرواتب.");
            return repository.PayrollExists(teacherId, month, year, excludeId);
        }

        public int AddPayroll(Payroll payroll)
        {
            CurrentUser.DemandAction("Payroll", "Add", "ليس لديك صلاحية إضافة الرواتب.");
            ValidatePayroll(payroll);
            if (repository.PayrollExists(payroll.TeacherID, payroll.SalaryMonth, payroll.SalaryYear))
                throw new Exception("تم صرف راتب هذا المعلم لهذا الشهر مسبقاً.");

            int payrollId = repository.AddPayroll(payroll);
            payroll.PayrollID = payrollId;

            if (payroll.PaymentDate.HasValue)
            {
                bool voucherCreated = voucherService.CreatePaymentVoucherForPayroll(
                    payroll.NetSalary,
                    payroll.PaymentDate.Value,
                    "المعلم رقم " + payroll.TeacherID,
                    payrollId,
                    "نقدي",
                    payroll.SalaryMonth,
                    payroll.SalaryYear,
                    "تم إنشاء سند صرف الراتب تلقائياً.");
                if (!voucherCreated)
                {
                    repository.DeletePayrollAfterVoucherFailure(payrollId);
                    throw new InvalidOperationException("تعذر إنشاء سند صرف الراتب، وتم التراجع عن حفظ الراتب.");
                }
            }

            auditLogService.Record("إنشاء", "Payroll", payrollId.ToString(),
                "إنشاء سجل راتب للمعلم رقم " + payroll.TeacherID + " عن " + payroll.SalaryMonth + "/" + payroll.SalaryYear);
            return payrollId;
        }

        public bool UpdatePayroll(Payroll payroll)
        {
            CurrentUser.DemandAction("Payroll", "Edit", "ليس لديك صلاحية تعديل الرواتب.");
            ValidatePayroll(payroll);
            if (repository.PayrollExists(payroll.TeacherID, payroll.SalaryMonth, payroll.SalaryYear, payroll.PayrollID))
                throw new Exception("تم صرف راتب هذا المعلم لهذا الشهر مسبقاً.");
            
            bool updated = repository.UpdatePayroll(payroll);
            if (updated && payroll.PaymentDate.HasValue)
            {
                bool voucherCreated = voucherService.CreatePaymentVoucherForPayroll(
                    payroll.NetSalary,
                    payroll.PaymentDate.Value,
                    "المعلم رقم " + payroll.TeacherID,
                    payroll.PayrollID,
                    "نقدي",
                    payroll.SalaryMonth,
                    payroll.SalaryYear,
                    "تم إنشاء أو تثبيت سند صرف الراتب تلقائياً.");
                if (!voucherCreated)
                {
                    repository.ResetPaymentDateAfterVoucherFailure(payroll.PayrollID);
                    throw new InvalidOperationException("تعذر تثبيت سند صرف الراتب، وتم إلغاء حالة الصرف.");
                }
            }

            if (updated)
            {
                auditLogService.Record("تعديل", "Payroll", payroll.PayrollID.ToString(),
                    "تعديل سجل راتب للمعلم رقم " + payroll.TeacherID + " عن " + payroll.SalaryMonth + "/" + payroll.SalaryYear);
            }
            return updated;
        }

        public bool DeletePayroll(int payrollId)
        {
            CurrentUser.DemandAction("Payroll", "Delete", "ليس لديك صلاحية حذف الرواتب.");
            if (payrollId <= 0)
                throw new Exception("رقم سجل الراتب غير صحيح.");
            bool deleted = repository.DeletePayroll(payrollId);
            if (deleted)
                auditLogService.Record("حذف", "Payroll", payrollId.ToString(), "حذف سجل راتب.");
            return deleted;
        }

        private void ValidatePayroll(Payroll payroll)
        {
            if (payroll == null)
                throw new Exception("بيانات الراتب غير صحيحة.");

            if (payroll.TeacherID <= 0)
                throw new Exception("يجب اختيار المعلم.");

            if (payroll.SalaryMonth < 1 || payroll.SalaryMonth > 12)
                throw new Exception("الشهر غير صحيح.");

            if (payroll.SalaryYear < 2000 || payroll.SalaryYear > 2100)
                throw new Exception("السنة غير صحيحة.");

            if (payroll.BasicSalary < 0)
                throw new Exception("الراتب الأساسي لا يمكن أن يكون سالباً.");

            if (payroll.Allowances < 0)
                throw new Exception("البدلات لا يمكن أن تكون سالبة.");

            if (payroll.Deductions < 0)
                throw new Exception("الخصومات لا يمكن أن تكون سالبة.");

            payroll.NetSalary = (payroll.BasicSalary + payroll.Allowances) - payroll.Deductions;
            if (payroll.NetSalary < 0)
                throw new Exception("الخصومات لا يمكن أن تتجاوز إجمالي الراتب والبدلات.");
            if (payroll.PaymentDate.HasValue && payroll.PaymentDate.Value.Date > DateTime.Today)
                throw new Exception("تاريخ صرف الراتب لا يمكن أن يكون في المستقبل.");
            if (!string.IsNullOrWhiteSpace(payroll.Notes) && payroll.Notes.Trim().Length > 200)
                throw new Exception("ملاحظات الراتب لا يمكن أن تتجاوز 200 حرف.");
        }
    }
}
