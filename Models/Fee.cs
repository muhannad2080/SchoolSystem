using System;

namespace SchoolSystem.Models
{
    public class Fee
    {
        public int FeeID { get; set; }

        public int StudentID { get; set; }
        public string StudentName { get; set; }

        public int? FeePlanID { get; set; }

        public string AcademicYear { get; set; }
        public string FeeType { get; set; }

        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal NetAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal RemainingAmount { get; set; }

        public DateTime DueDate { get; set; }
        public DateTime? PaymentDate { get; set; }

        public string PaymentMethod { get; set; }
        public string ReceiptNumber { get; set; }

        public string Status { get; set; }
        public string Notes { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
