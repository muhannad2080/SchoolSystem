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
        private readonly AuditLogService auditLogService;

        public BookService()
        {
            bookRepository = new BookRepository();
            auditLogService = new AuditLogService();
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
            bool added = bookRepository.AddBook(book);
            if (added)
            {
                auditLogService.Record("إنشاء", "Book", book.BookID.ToString(),
                    "إضافة كتاب: " + book.Title);
            }
            return added;
        }

        public bool UpdateBook(Book book)
        {
            CurrentUser.DemandPermission(PermissionKeys.LibraryManage, "ليس لديك صلاحية إدارة المكتبة.");
            if (book == null || book.BookID <= 0)
                throw new Exception("رقم الكتاب غير صحيح.");

            ValidateBook(book);
            bool updated = bookRepository.UpdateBook(book);
            if (updated)
            {
                auditLogService.Record("تعديل", "Book", book.BookID.ToString(),
                    "تعديل بيانات الكتاب: " + book.Title);
            }
            return updated;
        }

        public bool DeleteBook(int bookId)
        {
            CurrentUser.DemandPermission(PermissionKeys.LibraryManage, "ليس لديك صلاحية إدارة المكتبة.");
            if (bookId <= 0)
                throw new Exception("رقم الكتاب غير صحيح.");

            bool deleted = bookRepository.DeleteBook(bookId);
            if (deleted)
            {
                auditLogService.Record("حذف", "Book", bookId.ToString(),
                    "حذف كتاب من المكتبة.");
            }
            return deleted;
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
