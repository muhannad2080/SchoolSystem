using System;
using System.Data;
using SchoolSystem.DataAccess;
using SchoolSystem.Models;
using SchoolSystem.Security;

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
            CurrentUser.DemandPermission(PermissionKeys.LibraryManage, "ليس لديك صلاحية إدارة المكتبة.");
            return bookRepository.GetAllBooks();
        }

        public bool AddBook(Book book)
        {
            CurrentUser.DemandPermission(PermissionKeys.LibraryManage, "ليس لديك صلاحية إدارة المكتبة.");
            ValidateBook(book);
            return bookRepository.AddBook(book);
        }

        public bool UpdateBook(Book book)
        {
            CurrentUser.DemandPermission(PermissionKeys.LibraryManage, "ليس لديك صلاحية إدارة المكتبة.");
            if (book == null || book.BookID <= 0)
                throw new Exception("رقم الكتاب غير صحيح.");

            ValidateBook(book);
            return bookRepository.UpdateBook(book);
        }

        public bool DeleteBook(int bookId)
        {
            CurrentUser.DemandPermission(PermissionKeys.LibraryManage, "ليس لديك صلاحية إدارة المكتبة.");
            if (bookId <= 0)
                throw new Exception("رقم الكتاب غير صحيح.");

            return bookRepository.DeleteBook(bookId);
        }

        public int GetAvailableCopies(int bookId)
        {
            CurrentUser.DemandPermission(PermissionKeys.LibraryManage, "ليس لديك صلاحية إدارة المكتبة.");
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
