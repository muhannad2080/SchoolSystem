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
        private readonly AuditLogService auditLogService = new AuditLogService();

        public DataTable GetAllPayrolls()
        {
            EnsureCanManagePayroll();
            return repository.GetAllPayrolls();
        }

        public bool PayrollExists(int teacherId, int month, int year, int excludeId = 0)
        {
            EnsureCanManagePayroll();
            return repository.PayrollExists(teacherId, month, year, excludeId);
        }

        public void AddPayroll(Payroll payroll)
        {
            EnsureCanManagePayroll();
            ValidatePayroll(payroll);
            if (repository.PayrollExists(payroll.TeacherID, payroll.SalaryMonth, payroll.SalaryYear))
                throw new Exception("تم صرف راتب هذا المعلم لهذا الشهر مسبقاً.");
            
            repository.AddPayroll(payroll);
            auditLogService.Record("إنشاء", "Payroll", payroll.PayrollID.ToString(),
                "إنشاء سجل راتب للمعلم رقم " + payroll.TeacherID + " عن " + payroll.SalaryMonth + "/" + payroll.SalaryYear);
        }

        public bool UpdatePayroll(Payroll payroll)
        {
            EnsureCanManagePayroll();
            ValidatePayroll(payroll);
            if (repository.PayrollExists(payroll.TeacherID, payroll.SalaryMonth, payroll.SalaryYear, payroll.PayrollID))
                throw new Exception("تم صرف راتب هذا المعلم لهذا الشهر مسبقاً.");
            
            bool updated = repository.UpdatePayroll(payroll);
            if (updated)
            {
                auditLogService.Record("تعديل", "Payroll", payroll.PayrollID.ToString(),
                    "تعديل سجل راتب للمعلم رقم " + payroll.TeacherID + " عن " + payroll.SalaryMonth + "/" + payroll.SalaryYear);
            }
            return updated;
        }

        public bool DeletePayroll(int payrollId)
        {
            EnsureCanManagePayroll();
            if (payrollId <= 0)
                throw new Exception("رقم سجل الراتب غير صحيح.");
            bool deleted = repository.DeletePayroll(payrollId);
            if (deleted)
                auditLogService.Record("حذف", "Payroll", payrollId.ToString(), "حذف سجل راتب.");
            return deleted;
        }

        private static void EnsureCanManagePayroll()
        {
            if (!CurrentUser.HasPermission(PermissionKeys.PayrollManage))
                throw new UnauthorizedAccessException("ليس لديك صلاحية إدارة الرواتب.");
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
        }
    }
}
