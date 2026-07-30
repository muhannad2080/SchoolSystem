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
        public void AddPayroll(Payroll payroll) => repository.AddPayroll(payroll);
        public bool UpdatePayroll(Payroll payroll) => repository.UpdatePayroll(payroll);
        public bool DeletePayroll(int payrollId) => repository.DeletePayroll(payrollId);
    }
}