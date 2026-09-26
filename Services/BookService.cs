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

        // تعريف الدالة BookService لتنفيذ العملية المرتبطة بها.
        public BookService()
        {
            bookRepository = new BookRepository();
            auditLogService = new AuditLogService();
        }

        // تعريف الدالة GetAllBooks لتنفيذ العملية المرتبطة بها.
        public DataTable GetAllBooks()
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.DemandAction("Library", "View", "ليس لديك صلاحية عرض كتب المكتبة.");

            return bookRepository.GetAllBooks();
        }

        // تعريف الدالة AddBook لتنفيذ العملية المرتبطة بها.
        public bool AddBook(Book book)
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.DemandAction("Library", "Add", "ليس لديك صلاحية إضافة كتب المكتبة.");
            ValidateBook(book);
            bool added = bookRepository.AddBook(book);
            if (added)
            {
                auditLogService.Record("إنشاء", "Book", book.BookID.ToString(),
                    "إضافة كتاب: " + book.Title);
            }

            return added;
        }

        // تعريف الدالة UpdateBook لتنفيذ العملية المرتبطة بها.
        public bool UpdateBook(Book book)
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.DemandAction("Library", "Edit", "ليس لديك صلاحية تعديل كتب المكتبة.");
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

        // تعريف الدالة DeleteBook لتنفيذ العملية المرتبطة بها.
        public bool DeleteBook(int bookId)
        {
            CurrentUser.DemandAction("Library", "Delete", "ليس لديك صلاحية حذف كتب المكتبة.");
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

        // تعريف الدالة GetAvailableCopies لتنفيذ العملية المرتبطة بها.
        public int GetAvailableCopies(int bookId)
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.DemandAction("Library", "View", "ليس لديك صلاحية عرض مخزون المكتبة.");

            return bookRepository.GetAvailableCopies(bookId);
        }

        // تعريف الدالة ValidateBook لتنفيذ العملية المرتبطة بها.
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
