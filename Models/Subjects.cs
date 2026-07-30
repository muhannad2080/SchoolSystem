using System;

namespace SchoolSystem.Models
{
    public class Subject
    {
        public int SubjectID { get; set; }

        public string SubjectCode { get; set; }

        public string SubjectName { get; set; }

        public int? ClassID { get; set; }

        public decimal MaxDegree { get; set; }

        public decimal PassDegree { get; set; }

        public bool IsActive { get; set; }

        public string Notes { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
