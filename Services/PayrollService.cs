using System;
using System.Data;
using SchoolSystem.DataAccess;
using SchoolSystem.Models;

namespace SchoolSystem.Services
{
    public class PayrollService
    {
        private readonly PayrollRepository repository = new PayrollRepository();

        public DataTable GetAllPayrolls() => repository.GetAllPayrolls();

        public bool PayrollExists(int teacherId, int month, int year, int excludeId = 0)
            => repository.PayrollExists(teacherId, month, year, excludeId);

        public void AddPayroll(Payroll payroll)
        {
            ValidatePayroll(payroll);
            if (repository.PayrollExists(payroll.TeacherID, payroll.SalaryMonth, payroll.SalaryYear))
                throw new Exception("تم صرف راتب هذا المعلم لهذا الشهر مسبقاً.");
            
            repository.AddPayroll(payroll);
        }

        public bool UpdatePayroll(Payroll payroll)
        {
            ValidatePayroll(payroll);
            if (repository.PayrollExists(payroll.TeacherID, payroll.SalaryMonth, payroll.SalaryYear, payroll.PayrollID))
                throw new Exception("تم صرف راتب هذا المعلم لهذا الشهر مسبقاً.");
            
            return repository.UpdatePayroll(payroll);
        }

        public bool DeletePayroll(int payrollId)
        {
            if (payrollId <= 0)
                throw new Exception("رقم سجل الراتب غير صحيح.");
            return repository.DeletePayroll(payrollId);
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
