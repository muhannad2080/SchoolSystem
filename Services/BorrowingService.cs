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

        public BorrowingService()
        {
            borrowingRepository = new BorrowingRepository();
            bookRepository = new BookRepository();
        }

        public DataTable GetAllBorrowings()
        {
            CurrentUser.DemandPermission(PermissionKeys.LibraryManage, "ليس لديك صلاحية إدارة الإعارة.");
            return borrowingRepository.GetAllBorrowings();
        }

        public bool AddBorrowing(Borrowing borrowing)
        {
            CurrentUser.DemandPermission(PermissionKeys.LibraryManage, "ليس لديك صلاحية إدارة الإعارة.");
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

            return borrowingRepository.AddBorrowing(borrowing);
        }

        public bool ReturnBook(int borrowingId, DateTime returnDate)
        {
            CurrentUser.DemandPermission(PermissionKeys.LibraryManage, "ليس لديك صلاحية إدارة الإعارة.");
            if (borrowingId <= 0)
                throw new Exception("رقم الإعارة غير صحيح.");

            return borrowingRepository.ReturnBook(borrowingId, returnDate);
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
