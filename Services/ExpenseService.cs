using System;
using System.Data;
using SchoolSystem.DataAccess;
using SchoolSystem.Models;
using SchoolSystem.Security;

namespace SchoolSystem.Services
{
    public class ExpenseService
    {
        private readonly ExpenseRepository expenseRepository;
        private readonly VoucherService voucherService;
        private readonly AuditLogService auditLogService = new AuditLogService();

        public ExpenseService()
        {
            expenseRepository = new ExpenseRepository();
            voucherService = new VoucherService();
        }

        public DataTable GetAllExpenses()
        {
            CurrentUser.DemandAction("Expenses", "View", "ليس لديك صلاحية عرض المصروفات.");
            return expenseRepository.GetAllExpenses();
        }

        public int AddExpense(Expense expense)
        {
            CurrentUser.DemandAction("Expenses", "Add", "ليس لديك صلاحية إضافة المصروفات.");
            ValidateExpense(expense);

            if (string.IsNullOrWhiteSpace(expense.ExpenseNumber))
                expense.ExpenseNumber = expenseRepository.GenerateExpenseNumber();

            int expenseId = expenseRepository.AddExpense(expense);

            // إنشاء سند صرف تلقائي عند إضافة المصروف
            bool voucherCreated;
            try
            {
                voucherCreated = voucherService.CreatePaymentVoucherForExpense(
                    expense.Amount,
                    expense.ExpenseDate,
                    string.IsNullOrWhiteSpace(expense.PayeeName) ? "مصروفات مدرسية" : expense.PayeeName,
                    expenseId,
                    expense.PaymentMethod,
                    expense.Description,
                    "تم إنشاء سند الصرف تلقائياً من شاشة المصروفات. رقم المصروف: " + expense.ExpenseNumber
                );
            }
            catch
            {
                // لا نترك مصروفاً يتيماً بلا سند مالي عند فشل الإنشاء التلقائي.
                expenseRepository.DeleteExpense(expenseId);
                throw;
            }

            if (!voucherCreated)
            {
                expenseRepository.DeleteExpense(expenseId);
                throw new InvalidOperationException("تعذر إنشاء سند الصرف التلقائي، وتم التراجع عن حفظ المصروف.");
            }

            auditLogService.Record("إنشاء", "Expense", expenseId.ToString(),
                string.Format("المبلغ: {0}، الفئة: {1}، البيان: {2}", expense.Amount, expense.Category, expense.Description));
            return expenseId;
        }

        public bool UpdateExpense(Expense expense)
        {
            CurrentUser.DemandAction("Expenses", "Edit", "ليس لديك صلاحية تعديل المصروفات.");
            if (expense.ExpenseID <= 0)
                throw new Exception("رقم المصروف غير صحيح.");

            ValidateExpense(expense);

            decimal oldAmount = expenseRepository.GetExpenseAmountById(expense.ExpenseID);
            decimal difference = expense.Amount - oldAmount;

            bool updated = expenseRepository.UpdateExpense(expense);

            if (updated)
            {
                auditLogService.Record("تعديل", "Expense", expense.ExpenseID.ToString(),
                    string.Format("المبلغ الجديد: {0}، الفئة: {1}، الفرق: {2}", expense.Amount, expense.Category, difference));
            }

            if (updated && difference != 0)
            {
                if (difference > 0)
                {
                    // زيادة المصروف: سند صرف بالفرق
                    voucherService.CreatePaymentVoucherForExpense(
                        difference,
                        expense.ExpenseDate,
                        string.IsNullOrWhiteSpace(expense.PayeeName) ? "مصروفات مدرسية" : expense.PayeeName,
                        expense.ExpenseID,
                        expense.PaymentMethod,
                        "فرق زيادة مصروف: " + expense.Description,
                        "تم إنشاء سند صرف فرق تلقائياً بسبب تعديل مبلغ المصروف."
                    );
                }
                else
                {
                    // تخفيض المصروف: سند قبض تسوية تلقائي بقيمة النقص.
                    // يجب أن يمر عبر مسار المصروفات، لا مسار السند اليدوي،
                    // حتى لا يُطلب من المستخدم Vouchers.Add أثناء تعديل مصروف مخوّل به.
                    voucherService.CreateReceiptVoucherForExpenseAdjustment(
                        Math.Abs(difference),
                        expense.ExpenseDate,
                        string.IsNullOrWhiteSpace(expense.PayeeName) ? "تسوية مصروفات" : expense.PayeeName,
                        expense.ExpenseID,
                        expense.PaymentMethod,
                        "تسوية تخفيض مصروف: " + expense.Description,
                        "تم إنشاء سند قبض تسوية تلقائياً بسبب تخفيض مبلغ المصروف.");
                }
            }

            return updated;
        }

        public bool DeleteExpense(int expenseId)
        {
            CurrentUser.DemandAction("Expenses", "Delete", "ليس لديك صلاحية حذف المصروفات.");
            if (expenseId <= 0)
                throw new Exception("رقم المصروف غير صحيح.");

            bool deleted = expenseRepository.DeleteExpense(expenseId);
            if (deleted)
                auditLogService.Record("حذف", "Expense", expenseId.ToString(), "حذف سجل مصروفات.");
            return deleted;
        }

        private void ValidateExpense(Expense expense)
        {
            if (expense == null)
                throw new Exception("بيانات المصروف غير موجودة.");

            if (expense.Amount <= 0)
                throw new Exception("مبلغ المصروف يجب أن يكون أكبر من صفر.");

            if (expense.ExpenseDate == DateTime.MinValue || expense.ExpenseDate.Date > DateTime.Today)
                throw new Exception("تاريخ المصروف مطلوب ولا يمكن أن يكون في المستقبل.");

            if (string.IsNullOrWhiteSpace(expense.PaymentMethod))
                throw new Exception("يجب تحديد طريقة الدفع.");

            if (string.IsNullOrWhiteSpace(expense.Category))
                throw new Exception("يجب اختيار فئة المصروف.");

            if (string.IsNullOrWhiteSpace(expense.Description))
                throw new Exception("يجب إدخال بيان المصروف.");

            expense.Category = expense.Category.Trim();
            expense.Description = expense.Description.Trim();
            expense.PayeeName = string.IsNullOrWhiteSpace(expense.PayeeName) ? null : expense.PayeeName.Trim();
            expense.PaymentMethod = expense.PaymentMethod.Trim();
        }
    }
}
