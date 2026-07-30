using System;

namespace SchoolSystem.Models
{
    public class Payroll
    {
        public int PayrollID { get; set; }
        public int TeacherID { get; set; }
        public int SalaryMonth { get; set; }
        public int SalaryYear { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal Allowances { get; set; }
        public decimal Deductions { get; set; }
        public decimal NetSalary { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }

        // للعرض فقط
        public string TeacherName { get; set; }
    }
}