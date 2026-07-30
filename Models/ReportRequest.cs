using System;

namespace SchoolSystem.Models
{
    public class ReportRequest
    {
        public string ReportType { get; set; }

        public string AcademicYear { get; set; }

        public int? ClassID { get; set; }

        public string Section { get; set; }

        public string Status { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string SearchText { get; set; }
    }
}
