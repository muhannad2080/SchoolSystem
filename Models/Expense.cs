using System;

namespace SchoolSystem.Models
{
    public class Expense
    {
        public int ExpenseID { get; set; }

        public string ExpenseNumber { get; set; }

        public decimal Amount { get; set; }
        public DateTime ExpenseDate { get; set; }

        public string Category { get; set; }

        public string PayeeName { get; set; }
        public string PaymentMethod { get; set; }

        public string Description { get; set; }
        public string Notes { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
