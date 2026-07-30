using System;

namespace SchoolSystem.Models
{
    public class Teacher
    {
        public int TeacherID { get; set; }
        public string EmployeeNumber { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public string BirthPlace { get; set; }
        public string Nationality { get; set; }
        public string NationalID { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Qualification { get; set; }
        public string Specialization { get; set; }
        public DateTime? HireDate { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal TransportAllowance { get; set; }
        public decimal HousingAllowance { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}