using System;

namespace SchoolSystem.Models
{
    public class SchoolClass
    {
        public int ClassID { get; set; }

        public string ClassCode { get; set; }

        public string ClassName { get; set; }

        public string StageName { get; set; }

        public int GradeOrder { get; set; }

        public bool IsActive { get; set; }

        public string Notes { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
