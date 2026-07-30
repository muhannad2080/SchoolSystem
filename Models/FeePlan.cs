using System;

namespace SchoolSystem.Models
{
    public class FeePlan
    {
        public int FeePlanID { get; set; }

        public string AcademicYear { get; set; }

        public int ClassID { get; set; }
        public string ClassName { get; set; }

        public string FeeType { get; set; }
        public decimal Amount { get; set; }

        public DateTime DueDate { get; set; }

        public bool IsRequired { get; set; }

        public string Notes { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
