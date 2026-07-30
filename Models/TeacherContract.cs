using System;

namespace SchoolSystem.Models
{
    public class TeacherContract
    {
        public int ContractID { get; set; }

        public int TeacherID { get; set; }

        public string ContractNumber { get; set; }

        public string ContractType { get; set; }

        public string ContractStatus { get; set; }

        public decimal BasicSalary { get; set; }

        public decimal HousingAllowance { get; set; }

        public decimal TransportAllowance { get; set; }

        public decimal OtherAllowances { get; set; }

        public decimal Deductions { get; set; }

        public decimal TotalSalary { get; set; }

        public decimal NetSalary { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string PaymentMethod { get; set; }

        public string Notes { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
