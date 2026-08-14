using System.Data;

namespace SchoolSystem.Models
{
    public class StudentProfile
    {
        public Student Student { get; set; }
        public DataTable Attendance { get; set; }
        public DataTable Marks { get; set; }
        public DataTable Fees { get; set; }
        public bool CanViewFinancials { get; set; }
    }
}
