using System;
using System.Data;
using SchoolSystem.DataAccess;
using SchoolSystem.Models;
using SchoolSystem.Security;


namespace SchoolSystem.Services
{

    public class FeeService
    {

        private readonly FeeRepository feeRepository;
        private readonly FeePlanRepository feePlanRepository;
        // تعريف الدالة AuditLogService لتنفيذ العملية المرتبطة بها.
        private readonly AuditLogService auditLogService = new AuditLogService();

        // تعريف الدالة FeeService لتنفيذ العملية المرتبطة بها.
        public FeeService()
        {
            feeRepository = new FeeRepository();
            feePlanRepository = new FeePlanRepository();
        }

        // تعريف الدالة GetAllFees لتنفيذ العملية المرتبطة بها.
        public DataTable GetAllFees()
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.DemandAction("Fees", "View", "ليس لديك صلاحية عرض الرسوم.");

            return feeRepository.GetAllFees();
        }

        // تعريف الدالة GetStudentFinancialSummary لتنفيذ العملية المرتبطة بها.
        public DataTable GetStudentFinancialSummary(int studentId, string academicYear)
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.DemandAction("Fees", "View", "ليس لديك صلاحية عرض ملخص رسوم الطالب.");
            if (studentId <= 0)

                throw new Exception("يجب اختيار الطالب.");

            if (string.IsNullOrWhiteSpace(academicYear))

                throw new Exception("يجب اختيار العام الدراسي.");

            return feeRepository.GetStudentFinancialSummary(studentId, academicYear.Trim());
        }

        // تعريف الدالة AddFee لتنفيذ العملية المرتبطة بها.
        public int AddFee(Fee fee)
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.DemandAction("Fees", "Add", "ليس لديك صلاحية إضافة الرسوم.");
            ValidateFee(fee);
            ResolveFeePlan(fee);
            PrepareFee(fee);
            int feeId = feeRepository.AddFee(fee);
            if (feeId > 0)
                auditLogService.Record("إنشاء", "Fee", feeId.ToString(),
                    string.Format("الطالب: {0}، النوع: {1}، الصافي: {2}، المدفوع: {3}", fee.StudentID, fee.FeeType, fee.NetAmount, fee.PaidAmount));

            return feeId;
        }

        // تعريف الدالة UpdateFee لتنفيذ العملية المرتبطة بها.
        public bool UpdateFee(Fee fee)
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.DemandAction("Fees", "Edit", "ليس لديك صلاحية تعديل الرسوم.");
            if (fee.FeeID <= 0)

                throw new Exception("رقم سجل الرسوم غير صحيح.");

            ValidateFee(fee);
            ResolveFeePlan(fee);
            PrepareFee(fee);
            bool updated = feeRepository.UpdateFee(fee);
            if (updated)
                auditLogService.Record("تعديل", "Fee", fee.FeeID.ToString(),
                    string.Format("الطالب: {0}، النوع: {1}، الصافي: {2}، المدفوع: {3}", fee.StudentID, fee.FeeType, fee.NetAmount, fee.PaidAmount));

            return updated;
        }

        // تعريف الدالة RecordPayment لتنفيذ العملية المرتبطة بها.
        public DataTable RecordPayment(int feeId, decimal paymentAmount, DateTime paymentDate, string paymentMethod, string receiptNumber, string notes)
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.DemandAction("Fees", "Edit", "ليس لديك صلاحية تسجيل دفعات الرسوم.");

            /* الدفعة تُنشئ سند قبض تلقائياً في نفس المعاملة؛ يجب أن يملك المستخدم
               صلاحية إنشاء السند المرتبط كما كان معمولاً به في VoucherService. */
            bool canCreateVoucher = CurrentUser.CanAdd("Vouchers") ||
                CurrentUser.HasPermission(PermissionKeys.VouchersManage) ||
                CurrentUser.CanAdd("Fees") ||
                CurrentUser.HasPermission(PermissionKeys.FeesManage);

            if (!canCreateVoucher)

                throw new UnauthorizedAccessException("ليس لديك صلاحية إنشاء سند القبض المرتبط بالدفعة.");

            if (feeId <= 0)

                throw new Exception("رقم سجل الرسوم غير صحيح.");

            if (paymentAmount <= 0)

                throw new Exception("مبلغ الدفعة يجب أن يكون أكبر من صفر.");

            if (paymentDate.Date > DateTime.Today)

                throw new Exception("تاريخ الدفع لا يمكن أن يكون في المستقبل.");

            if (string.IsNullOrWhiteSpace(paymentMethod))

                throw new Exception("طريقة الدفع مطلوبة.");

            if (receiptNumber != null && receiptNumber.Trim().Length > 100)

                throw new Exception("رقم السند لا يمكن أن يتجاوز 100 حرف.");

            if (notes != null && notes.Trim().Length > 500)

                throw new Exception("ملاحظات الدفعة لا يمكن أن تتجاوز 500 حرف.");

            DataTable result = feeRepository.RecordPayment(
                feeId,
                paymentAmount,
                paymentDate.Date,
                paymentMethod.Trim(),
                receiptNumber == null ? string.Empty : receiptNumber.Trim(),
                notes == null ? string.Empty : notes.Trim());

            if (result.Rows.Count == 0)

                throw new InvalidOperationException("تعذر تسجيل الدفعة؛ ربما تجاوزت المبلغ المتبقي أو تم تعديل السجل من مستخدم آخر.");

            DataRow row = result.Rows[0];
            auditLogService.Record("تحصيل دفعة", "Fee", feeId.ToString(),
                string.Format("الطالب: {0}، المبلغ: {1}، المتبقي: {2}، الطريقة: {3}",
                    row["StudentID"], paymentAmount, row["RemainingAmount"], paymentMethod));

            return result;
        }

        // تعريف الدالة RefundPayment لتنفيذ العملية المرتبطة بها.
        public DataTable RefundPayment(int feeId, decimal refundAmount, DateTime paymentDate, string notes)
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.DemandAction("Fees", "Edit", "ليس لديك صلاحية تعديل دفعات الرسوم.");

            if (feeId <= 0)

                throw new Exception("رقم سجل الرسوم غير صحيح.");

            if (refundAmount <= 0)

                throw new Exception("مبلغ الخصم يجب أن يكون أكبر من صفر.");

            if (paymentDate.Date > DateTime.Today)

                throw new Exception("تاريخ التعديل لا يمكن أن يكون في المستقبل.");

            if (notes != null && notes.Trim().Length > 500)

                throw new Exception("ملاحظات التعديل لا يمكن أن تتجاوز 500 حرف.");

            DataTable result = feeRepository.RefundPayment(
                feeId,
                refundAmount,
                paymentDate.Date,
                notes == null ? string.Empty : notes.Trim());

            if (result.Rows.Count == 0)

                throw new InvalidOperationException("تعذر خصم المبلغ؛ ربما تجاوز إجمالي المدفوع أو تم تعديل السجل من مستخدم آخر.");

            DataRow row = result.Rows[0];
            auditLogService.Record("خصم دفعة", "Fee", feeId.ToString(),
                string.Format("الطالب: {0}، المبلغ المخصوم: {1}، المتبقي: {2}",
                    row["StudentID"], refundAmount, row["RemainingAmount"]));

            return result;
        }

        // تعريف الدالة DeleteFee لتنفيذ العملية المرتبطة بها.
        public bool DeleteFee(int feeId)
        {
            CurrentUser.DemandAction("Fees", "Delete", "ليس لديك صلاحية حذف الرسوم.");
            if (feeId <= 0)

                throw new Exception("رقم سجل الرسوم غير صحيح.");

            bool deleted = feeRepository.DeleteFee(feeId);
            if (deleted)
                auditLogService.Record("حذف", "Fee", feeId.ToString(), "حذف سجل الرسوم.");

            return deleted;
        }

        public int CreateRegistrationFeeIfMissing(int studentId, string academicYear, decimal registrationFee,
            decimal paidAmount, string paymentMethod, DateTime dueDate, string enrollmentMarker)
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.DemandAction("Fees", "Add", "ليس لديك صلاحية توليد رسوم التسجيل.");
            if (studentId <= 0)

                throw new Exception("يجب اختيار الطالب.");

            if (string.IsNullOrWhiteSpace(academicYear))

                throw new Exception("العام الدراسي مطلوب.");

            if (registrationFee <= 0)
                return 0;
            if (paidAmount < 0 || paidAmount > registrationFee)

                throw new Exception("المبلغ المدفوع لا يمكن أن يكون أكبر من رسوم التسجيل أو أقل من صفر.");

            /* الدفعة تُنشئ سند قبض تلقائياً مع الرسم في نفس المعاملة؛ يجب أن يملك
               المستخدم صلاحية إنشاء السند المرتبط (كما كان في VoucherService). */
            if (paidAmount > 0)
            {
                bool canCreateVoucher = CurrentUser.CanAdd("Vouchers") ||
                    CurrentUser.HasPermission(PermissionKeys.VouchersManage) ||
                    CurrentUser.CanAdd("Fees") ||
                    CurrentUser.HasPermission(PermissionKeys.FeesManage);

                if (!canCreateVoucher)

                    throw new UnauthorizedAccessException("ليس لديك صلاحية إنشاء سند القبض المرتبط برسوم التسجيل.");
            }

            Fee fee = new Fee
            {
                StudentID = studentId,
                AcademicYear = academicYear.Trim(),
                FeeType = "رسوم تسجيل",
                TotalAmount = registrationFee,
                DiscountAmount = 0,
                PaidAmount = paidAmount,
                DueDate = dueDate == DateTime.MinValue ? DateTime.Today : dueDate.Date,
                PaymentDate = paidAmount > 0 ? DateTime.Today : (DateTime?)null,
                PaymentMethod = paymentMethod,
                ReceiptNumber = string.Empty,
                Notes = enrollmentMarker

            };

            PrepareFee(fee);
            int feeId = feeRepository.CreateRegistrationFeeIfMissing(fee, enrollmentMarker);
            if (feeId > 0)
                auditLogService.Record("إنشاء/تحقق", "Fee", feeId.ToString(),
                    string.Format("رسوم تسجيل للطالب {0}، العام {1}، المدفوع {2}", studentId, academicYear, paidAmount));

            return feeId;
        }

        // تعريف الدالة GenerateStudentFeesFromPlans لتنفيذ العملية المرتبطة بها.
        public int GenerateStudentFeesFromPlans(int studentId, string academicYear)
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.DemandAction("Fees", "Add", "ليس لديك صلاحية توليد رسوم الطلاب.");
            if (studentId <= 0)

                throw new Exception("يجب اختيار الطالب.");


            if (string.IsNullOrWhiteSpace(academicYear))

                throw new Exception("يجب اختيار العام الدراسي.");

            int generated = feeRepository.GenerateStudentFeesFromPlans(studentId, academicYear);
            if (generated > 0)
                auditLogService.Record("توليد", "Fee", studentId.ToString(),
                    string.Format("توليد {0} سجل رسوم من خطط العام الدراسي {1}.", generated, academicYear));

            return generated;
        }

        // تعريف الدالة ValidateFee لتنفيذ العملية المرتبطة بها.
        private void ValidateFee(Fee fee)
        {
            if (fee == null)

                throw new Exception("بيانات الرسوم غير موجودة.");


            if (fee.StudentID <= 0)

                throw new Exception("يجب اختيار الطالب.");


            if (string.IsNullOrWhiteSpace(fee.AcademicYear))

                throw new Exception("يجب اختيار العام الدراسي.");

            ValidateAcademicYear(fee.AcademicYear);

            if (string.IsNullOrWhiteSpace(fee.FeeType))

                throw new Exception("يجب اختيار نوع الرسوم.");


            if (fee.TotalAmount < 0)

                throw new Exception("إجمالي الرسوم لا يمكن أن يكون أقل من صفر.");


            if (fee.DiscountAmount < 0)

                throw new Exception("الخصم لا يمكن أن يكون أقل من صفر.");


            if (fee.PaidAmount < 0)

                throw new Exception("المبلغ المدفوع لا يمكن أن يكون أقل من صفر.");


            if (fee.DiscountAmount > fee.TotalAmount)

                throw new Exception("الخصم لا يمكن أن يكون أكبر من إجمالي الرسوم.");

            /* الرصيد الدائن (الدفع الزائد) مشروع في النموذج المالي ويُحفظ بالسالب في
               RemainingAmount؛ لذلك لا يُرفض أن يكون المدفوع أكبر من الصافي. */


            if (fee.DueDate == DateTime.MinValue)

                throw new Exception("تاريخ الاستحقاق مطلوب وصحيح.");


            if (fee.PaymentDate.HasValue && fee.PaymentDate.Value.Date > DateTime.Today)

                throw new Exception("تاريخ الدفع لا يمكن أن يكون في المستقبل.");
        }

        // تعريف الدالة ResolveFeePlan لتنفيذ العملية المرتبطة بها.
        private void ResolveFeePlan(Fee fee)
        {
            if (fee.FeePlanID.HasValue && fee.FeePlanID.Value > 0)
                return;

            FeePlan matchedPlan = feePlanRepository.GetStudentFeePlan(
                fee.StudentID,
                fee.AcademicYear,
                fee.FeeType);

            if (matchedPlan == null)

                throw new Exception("يجب ربط الرسوم بخطة مطابقة لصف الطالب ونوع الرسوم في العام الدراسي المحدد.");

            fee.FeePlanID = matchedPlan.FeePlanID;
        }

        // تعريف الدالة ValidateAcademicYear لتنفيذ العملية المرتبطة بها.
        private void ValidateAcademicYear(string academicYear)
        {
            string[] parts = SplitAcademicYear(academicYear);
            int firstYear;
            int secondYear;

            if (parts.Length != 2 ||
                parts[0].Length != 4 ||
                parts[1].Length != 4 ||
                !int.TryParse(parts[0], out firstYear) ||
                !int.TryParse(parts[1], out secondYear) ||
                secondYear != firstYear + 1)
            {

                throw new Exception("صيغة العام الدراسي يجب أن تكون متسلسلة مثل 2026/2027 أو 1447-1448.");
            }
        }

        // تعريف الدالة SplitAcademicYear لتنفيذ العملية المرتبطة بها.
        private static string[] SplitAcademicYear(string academicYear)
        {
            string value = (academicYear ?? string.Empty).Trim()
                .Replace('-', '/')
                .Replace('–', '/')
                .Replace('—', '/');

            string[] parts = value.Split('/');

            for (int i = 0; i < parts.Length; i++)
                parts[i] = parts[i].Trim();


            return parts;
        }

        // تعريف الدالة NormalizeAcademicYear لتنفيذ العملية المرتبطة بها.
        private static string NormalizeAcademicYear(string academicYear)
        {
            string[] parts = SplitAcademicYear(academicYear);

            return parts.Length == 2
                ? parts[0] + "/" + parts[1]
                : (academicYear ?? string.Empty).Trim();
        }

        // تعريف الدالة PrepareFee لتنفيذ العملية المرتبطة بها.
        private void PrepareFee(Fee fee)
        {
            fee.AcademicYear = NormalizeAcademicYear(fee.AcademicYear);
            fee.FeeType = fee.FeeType.Trim();
            fee.NetAmount = fee.TotalAmount - fee.DiscountAmount;
            fee.RemainingAmount = fee.NetAmount - fee.PaidAmount;

            if (fee.NetAmount == 0)
            {
                fee.Status = "معفى";
                fee.PaymentDate = null;
            }

            else if (fee.PaidAmount == 0)
            {
                fee.Status = fee.DueDate.Date < DateTime.Today ? "متأخر" : "غير مسدد";
                fee.PaymentDate = null;
            }

            else if (fee.RemainingAmount < 0)
            {
                fee.Status = "رصيد دائن";

                if (!fee.PaymentDate.HasValue)
                    fee.PaymentDate = DateTime.Today;
            }

            else if (fee.PaidAmount >= fee.NetAmount)
            {
                fee.Status = "مسدد";

                if (!fee.PaymentDate.HasValue)
                    fee.PaymentDate = DateTime.Today;
            }

            else
            {
                fee.Status = "مسدد جزئياً";

                if (!fee.PaymentDate.HasValue)
                    fee.PaymentDate = DateTime.Today;
            }
        }
    }
}
