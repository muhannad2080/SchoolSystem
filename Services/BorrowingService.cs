using System;
using System.Data;
using SchoolSystem.DataAccess;
using SchoolSystem.Models;
using SchoolSystem.Security;

namespace SchoolSystem.Services
{
    public class BorrowingService
    {
        private readonly BorrowingRepository borrowingRepository;
        private readonly BookRepository bookRepository;
        private readonly AuditLogService auditLogService;

        public BorrowingService()
        {
            borrowingRepository = new BorrowingRepository();
            bookRepository = new BookRepository();
            auditLogService = new AuditLogService();
        }

        public DataTable GetAllBorrowings()
        {
            CurrentUser.DemandAction("Library", "View", "ليس لديك صلاحية عرض الإعارات.");
            return borrowingRepository.GetAllBorrowings();
        }

        public bool AddBorrowing(Borrowing borrowing)
        {
            CurrentUser.DemandAction("Library", "Add", "ليس لديك صلاحية إضافة الإعارات.");
            ValidateBorrowing(borrowing);

            int available = bookRepository.GetAvailableCopies(borrowing.BookID);

            if (available <= 0)
                throw new Exception("لا توجد نسخ متاحة من هذا الكتاب للإعارة.");

            if (borrowingRepository.HasActiveBorrowing(
                borrowing.BookID,
                borrowing.BorrowerType,
                borrowing.BorrowerID))
            {
                throw new Exception("هذا المستعير لديه نفس الكتاب معار حالياً.");
            }

            bool added = borrowingRepository.AddBorrowing(borrowing);
            if (added)
            {
                auditLogService.Record("إنشاء", "BookBorrowing", borrowing.BorrowingID.ToString(),
                    "إنشاء إعارة للكتاب رقم " + borrowing.BookID + " إلى " + borrowing.BorrowerType + " رقم " + borrowing.BorrowerID);
            }
            return added;
        }

        public bool ReturnBook(int borrowingId, DateTime returnDate)
        {
            CurrentUser.DemandAction("Library", "Edit", "ليس لديك صلاحية إرجاع الكتب.");
            if (borrowingId <= 0)
                throw new Exception("رقم الإعارة غير صحيح.");

            bool returned = borrowingRepository.ReturnBook(borrowingId, returnDate);
            if (returned)
                auditLogService.Record("إرجاع", "BookBorrowing", borrowingId.ToString(), "إرجاع كتاب معار.");
            return returned;
        }

        private void ValidateBorrowing(Borrowing borrowing)
        {
            if (borrowing == null)
                throw new Exception("بيانات الإعارة غير موجودة.");

            if (borrowing.BookID <= 0)
                throw new Exception("يجب اختيار الكتاب.");

            if (borrowing.BorrowerType != "طالب" && borrowing.BorrowerType != "معلم")
                throw new Exception("نوع المستعير يجب أن يكون طالب أو معلم.");

            if (borrowing.BorrowerID <= 0)
                throw new Exception("يجب اختيار المستعير.");

            if (borrowing.DueDate.Date < borrowing.BorrowDate.Date)
                throw new Exception("تاريخ الإرجاع يجب أن يكون بعد تاريخ الإعارة أو مساويًا له.");
        }
    }
}
