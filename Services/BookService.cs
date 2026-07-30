using System;
using System.Data;
using SchoolSystem.DataAccess;
using SchoolSystem.Models;

namespace SchoolSystem.Services
{
    public class BookService
    {
        private readonly BookRepository bookRepository;

        public BookService()
        {
            bookRepository = new BookRepository();
        }

        public DataTable GetAllBooks()
        {
            return bookRepository.GetAllBooks();
        }

        public bool AddBook(Book book)
        {
            ValidateBook(book);
            return bookRepository.AddBook(book);
        }

        public bool UpdateBook(Book book)
        {
            if (book.BookID <= 0)
                throw new Exception("رقم الكتاب غير صحيح.");

            ValidateBook(book);
            return bookRepository.UpdateBook(book);
        }

        public bool DeleteBook(int bookId)
        {
            if (bookId <= 0)
                throw new Exception("رقم الكتاب غير صحيح.");

            return bookRepository.DeleteBook(bookId);
        }

        public int GetAvailableCopies(int bookId)
        {
            return bookRepository.GetAvailableCopies(bookId);
        }

        private void ValidateBook(Book book)
        {
            if (book == null)
                throw new Exception("بيانات الكتاب غير موجودة.");

            if (string.IsNullOrWhiteSpace(book.Title))
                throw new Exception("يجب إدخال عنوان الكتاب.");

            if (book.Copies <= 0)
                throw new Exception("عدد النسخ يجب أن يكون أكبر من صفر.");

            if (book.PublicationYear < 0)
                throw new Exception("سنة النشر غير صحيحة.");
        }
    }
}
