using System;

namespace SchoolSystem.Models
{
    public class Borrowing
    {
        public int BorrowingID { get; set; }

        public int BookID { get; set; }
        public string BookTitle { get; set; }

        public string BorrowerType { get; set; } // طالب / معلم
        public int BorrowerID { get; set; }
        public string BorrowerName { get; set; }

        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public string Status { get; set; } // معار / مسترجع / متأخر
        public string Notes { get; set; }
    }
}
