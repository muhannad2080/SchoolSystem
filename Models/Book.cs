using System;

namespace SchoolSystem.Models
{
    public class Book
    {
        public int BookID { get; set; }

        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public string Category { get; set; }
        public string Publisher { get; set; }

        public int PublicationYear { get; set; }

        public int Copies { get; set; }
        public int AvailableCopies { get; set; }

        public string ShelfLocation { get; set; }
        public string Notes { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
