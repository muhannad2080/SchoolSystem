using System;
using System.Data;
using System.Data.SqlClient;
using SchoolSystem.Models;


namespace SchoolSystem.DataAccess
{

    public class FeeRepository
    {
        // تعريف الدالة GetStudentFinancialSummary لتنفيذ العملية المرتبطة بها.
        public DataTable GetStudentFinancialSummary(int studentId, string academicYear)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT
                    COUNT(*) AS FeeCount,
                    ISNULL(SUM(NetAmount), 0) AS TotalNetAmount,
                    ISNULL(SUM(PaidAmount), 0) AS TotalPaidAmount,
                    ISNULL(SUM(RemainingAmount), 0) AS TotalRemainingAmount,
                    ISNULL(SUM(CASE WHEN RemainingAmount > 0 THEN 1 ELSE 0 END), 0) AS UnpaidFeeCount,
                    ISNULL(STUFF((SELECT N'، ' + ISNULL(f2.FeeType, N'غير محدد')
                                  FROM Fees f2
                                  WHERE f2.StudentID = @StudentID
                                    AND REPLACE(ISNULL(f2.AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                                    AND ISNULL(f2.RemainingAmount, 0) > 0
                                    AND LTRIM(RTRIM(ISNULL(f2.FeeType, N''))) NOT IN
                                        (N'الرسوم الكلية', N'رسوم كلية', N'الرسوم الكليه', N'رسوم كليه', N'Total Fee')
                                  ORDER BY f2.FeeID
                                  FOR XML PATH(N''), TYPE).value(N'.', N'nvarchar(max)'), 1, 2, N''), N'') AS UnpaidFeeTypes
                FROM Fees
                WHERE StudentID = @StudentID
                  AND REPLACE(ISNULL(AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                  AND LTRIM(RTRIM(ISNULL(FeeType, N''))) NOT IN
                      (N'الرسوم الكلية', N'رسوم كلية', N'الرسوم الكليه', N'رسوم كليه', N'Total Fee');", con))
            {

                cmd.Parameters.AddWithValue("@StudentID", studentId);

                cmd.Parameters.AddWithValue("@AcademicYear", academicYear ?? string.Empty);

                DataTable table = new DataTable();
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    adapter.Fill(table);

                return table;
            }
        }

        // تعريف الدالة GetAllFees لتنفيذ العملية المرتبطة بها.
        public DataTable GetAllFees()
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string query = @"
                    SELECT
                        f.FeeID,
                        f.StudentID,
                        s.FullName AS StudentName,
                        sc.ClassID,
                        c.ClassName,
                        sc.Section,
                        f.FeePlanID,
                        f.AcademicYear,
                        f.FeeType,
                        f.TotalAmount,
                        f.DiscountAmount,
                        f.NetAmount,
                        f.PaidAmount,
                        f.RemainingAmount,
                        SUM(CASE WHEN LTRIM(RTRIM(ISNULL(f.FeeType, N''))) IN
                                    (N'الرسوم الكلية', N'رسوم كلية', N'الرسوم الكليه', N'رسوم كليه', N'Total Fee')
                                 THEN 0 ELSE ISNULL(f.NetAmount, 0) END)
                            OVER (PARTITION BY f.StudentID, REPLACE(ISNULL(f.AcademicYear, N''), N'-', N'/')) AS StudentTotalNetAmount,
                        SUM(CASE WHEN LTRIM(RTRIM(ISNULL(f.FeeType, N''))) IN
                                    (N'الرسوم الكلية', N'رسوم كلية', N'الرسوم الكليه', N'رسوم كليه', N'Total Fee')
                                 THEN 0 ELSE ISNULL(f.PaidAmount, 0) END)
                            OVER (PARTITION BY f.StudentID, REPLACE(ISNULL(f.AcademicYear, N''), N'-', N'/')) AS StudentTotalPaidAmount,
                        SUM(CASE WHEN LTRIM(RTRIM(ISNULL(f.FeeType, N''))) IN
                                    (N'الرسوم الكلية', N'رسوم كلية', N'الرسوم الكليه', N'رسوم كليه', N'Total Fee')
                                 THEN 0 ELSE ISNULL(f.RemainingAmount, 0) END)
                            OVER (PARTITION BY f.StudentID, REPLACE(ISNULL(f.AcademicYear, N''), N'-', N'/')) AS StudentTotalRemainingAmount,
                        SUM(CASE WHEN LTRIM(RTRIM(ISNULL(f.FeeType, N''))) IN
                                    (N'الرسوم الكلية', N'رسوم كلية', N'الرسوم الكليه', N'رسوم كليه', N'Total Fee')
                                 THEN 0 WHEN ISNULL(f.RemainingAmount, 0) > 0 THEN 1 ELSE 0 END)
                            OVER (PARTITION BY f.StudentID, REPLACE(ISNULL(f.AcademicYear, N''), N'-', N'/')) AS StudentUnpaidFeeCount,
                        f.DueDate,
                        f.PaymentDate,
                        f.PaymentMethod,
                        f.ReceiptNumber,
                        f.Status,
                        f.Notes,
                        f.CreatedAt,
                        f.UpdatedAt
                    FROM Fees f
                    INNER JOIN Students s ON f.StudentID = s.StudentID
                    LEFT JOIN StudentClasses sc ON sc.StudentClassID =
                    (
                        SELECT TOP (1) sc2.StudentClassID
                        FROM StudentClasses sc2
                        WHERE sc2.StudentID = f.StudentID
                          AND REPLACE(ISNULL(sc2.AcademicYear, N''), N'-', N'/') = REPLACE(ISNULL(f.AcademicYear, N''), N'-', N'/')
                        ORDER BY sc2.AssignedDate DESC, sc2.StudentClassID DESC
                    )
                    LEFT JOIN Classes c ON c.ClassID = COALESCE(sc.ClassID, s.ClassID)
                    ORDER BY f.FeeID DESC";

                using (SqlDataAdapter da = new SqlDataAdapter(query, con))
                {
                    DataTable dt = new DataTable();

                    da.Fill(dt);

                    return dt;
                }
            }
        }

        // تعريف الدالة AddFee لتنفيذ العملية المرتبطة بها.
        public int AddFee(Fee fee)
        {
            CalculateAmounts(fee);

            using (SqlConnection con = DbConnection.GetConnection())
            {
                string query = @"
                    SET NOCOUNT ON;
                    SET XACT_ABORT ON;

                    /* إذا اختيرت خطة، يجب أن تطابق توزيع الطالب الفعلي في العام نفسه. */
                    IF @FeePlanID IS NOT NULL AND NOT EXISTS
                    (
                        SELECT 1
                        FROM FeePlans fp
                        WHERE fp.FeePlanID = @FeePlanID
                          AND REPLACE(ISNULL(fp.AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                          AND LTRIM(RTRIM(ISNULL(fp.FeeType, N''))) = LTRIM(RTRIM(ISNULL(@FeeType, N'')))
                          AND
                          (
                              EXISTS
                              (
                                  SELECT 1
                                  FROM StudentClasses sc
                                  WHERE sc.ClassID = fp.ClassID
                                    AND sc.StudentID = @StudentID
                                    AND REPLACE(ISNULL(sc.AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                              )
                              OR EXISTS
                              (
                                  SELECT 1
                                  FROM Enrollments e
                                  WHERE e.ClassID = fp.ClassID
                                    AND e.StudentID = @StudentID
                                    AND REPLACE(ISNULL(e.AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                                    AND LTRIM(RTRIM(ISNULL(e.Status, N''))) IN (N'مقبول', N'Accepted', N'Approved', N'نشط', N'Active')
                              )
                          )
                    )
                    BEGIN
                        THROW 51041, N'خطة الرسوم لا تطابق صف الطالب في العام الدراسي المحدد.', 1;
                    END;

                    IF EXISTS
                    (
                        SELECT 1
                        FROM Fees WITH (UPDLOCK, HOLDLOCK)
                        WHERE StudentID = @StudentID
                          AND REPLACE(ISNULL(AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                          AND (
                                (@FeePlanID IS NOT NULL AND FeePlanID = @FeePlanID)
                                OR (@FeePlanID IS NULL AND FeePlanID IS NULL
                                    AND LTRIM(RTRIM(ISNULL(FeeType, N''))) = LTRIM(RTRIM(ISNULL(@FeeType, N'')))
                                    )
                              )
                    )
                    BEGIN
                        THROW 51042, N'يوجد بند رسوم مماثل للطالب في العام الدراسي نفسه؛ لا يمكن إنشاء رسوم مكررة.', 1;
                    END;

                    INSERT INTO Fees
                    (
                        StudentID,
                        FeePlanID,
                        AcademicYear,
                        FeeType,
                        TotalAmount,
                        DiscountAmount,
                        NetAmount,
                        PaidAmount,
                        RemainingAmount,
                        DueDate,
                        PaymentDate,
                        PaymentMethod,
                        ReceiptNumber,
                        Status,
                        Notes
                    )
                    VALUES
                    (
                        @StudentID,
                        @FeePlanID,
                        @AcademicYear,
                        @FeeType,
                        @TotalAmount,
                        @DiscountAmount,
                        @NetAmount,
                        @PaidAmount,
                        @RemainingAmount,
                        @DueDate,
                        @PaymentDate,
                        @PaymentMethod,
                        @ReceiptNumber,
                        @Status,
                        @Notes
                    );

                    SELECT CAST(SCOPE_IDENTITY() AS INT);";


                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    AddParameters(cmd, fee);


                    con.Open();

                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        // تعريف الدالة UpdateFee لتنفيذ العملية المرتبطة بها.
        public bool UpdateFee(Fee fee)
        {
            CalculateAmounts(fee);

            using (SqlConnection con = DbConnection.GetConnection())
            {
                string query = @"
                    SET NOCOUNT ON;
                    SET XACT_ABORT ON;

                    IF @FeePlanID IS NOT NULL AND NOT EXISTS
                    (
                        SELECT 1
                        FROM FeePlans fp
                        WHERE fp.FeePlanID = @FeePlanID
                          AND REPLACE(ISNULL(fp.AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                          AND LTRIM(RTRIM(ISNULL(fp.FeeType, N''))) = LTRIM(RTRIM(ISNULL(@FeeType, N'')))
                          AND
                          (
                              EXISTS
                              (
                                  SELECT 1
                                  FROM StudentClasses sc
                                  WHERE sc.ClassID = fp.ClassID
                                    AND sc.StudentID = @StudentID
                                    AND REPLACE(ISNULL(sc.AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                              )
                              OR EXISTS
                              (
                                  SELECT 1
                                  FROM Enrollments e
                                  WHERE e.ClassID = fp.ClassID
                                    AND e.StudentID = @StudentID
                                    AND REPLACE(ISNULL(e.AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                                    AND LTRIM(RTRIM(ISNULL(e.Status, N''))) IN (N'مقبول', N'Accepted', N'Approved', N'نشط', N'Active')
                              )
                          )
                    )
                    BEGIN
                        THROW 51041, N'خطة الرسوم لا تطابق صف الطالب في العام الدراسي المحدد.', 1;
                    END;

                    IF EXISTS
                    (
                        SELECT 1 FROM Fees WITH (UPDLOCK, HOLDLOCK)
                        WHERE FeeID <> @FeeID
                          AND StudentID = @StudentID
                          AND REPLACE(ISNULL(AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                          AND (
                                (@FeePlanID IS NOT NULL AND FeePlanID = @FeePlanID)
                                OR (@FeePlanID IS NULL AND FeePlanID IS NULL
                                    AND LTRIM(RTRIM(ISNULL(FeeType, N''))) = LTRIM(RTRIM(ISNULL(@FeeType, N'')))
                                    )
                              )
                    )
                        THROW 51043, N'التعديل سيؤدي إلى تكرار بند رسوم موجود للطالب في العام نفسه.', 1;

                    UPDATE Fees SET
                        StudentID = @StudentID,
                        FeePlanID = @FeePlanID,
                        AcademicYear = @AcademicYear,
                        FeeType = @FeeType,
                        TotalAmount = @TotalAmount,
                        DiscountAmount = @DiscountAmount,
                        NetAmount = @NetAmount,
                        PaidAmount = @PaidAmount,
                        RemainingAmount = @RemainingAmount,
                        DueDate = @DueDate,
                        PaymentDate = @PaymentDate,
                        PaymentMethod = @PaymentMethod,
                        ReceiptNumber = @ReceiptNumber,
                        Status = @Status,
                        Notes = @Notes,
                        UpdatedAt = GETDATE()
                    WHERE FeeID = @FeeID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {

                    cmd.Parameters.AddWithValue("@FeeID", fee.FeeID);

                    AddParameters(cmd, fee);


                    con.Open();

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // تعريف الدالة RecordPayment لتنفيذ العملية المرتبطة بها.
        public DataTable RecordPayment(int feeId, decimal paymentAmount, DateTime paymentDate, string paymentMethod, string receiptNumber, string notes)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                const string query = @"
                    SET NOCOUNT ON;
                    SET XACT_ABORT ON;

                    DECLARE @StudentID INT;
                    DECLARE @AcademicYear NVARCHAR(100);
                    DECLARE @RemainingPayment DECIMAL(18,2) = @PaymentAmount;
                    DECLARE @CurrentFeeID INT;
                    DECLARE @CurrentRemaining DECIMAL(18,2);
                    DECLARE @AppliedAmount DECIMAL(18,2);

                    SELECT @StudentID = StudentID, @AcademicYear = AcademicYear
                    FROM Fees WITH (UPDLOCK, HOLDLOCK)
                    WHERE FeeID = @FeeID;

                    IF @StudentID IS NULL OR @PaymentAmount <= 0
                        THROW 51044, N'بيانات السداد غير صحيحة.', 1;

                    WHILE @RemainingPayment > 0
                    BEGIN
                        SET @CurrentFeeID = NULL;
                        SELECT TOP 1
                            @CurrentFeeID = FeeID,
                            @CurrentRemaining = ISNULL(RemainingAmount, 0)
                        FROM Fees WITH (UPDLOCK, HOLDLOCK)
                        WHERE StudentID = @StudentID
                          AND REPLACE(ISNULL(AcademicYear, N''), N'-', N'/') = REPLACE(ISNULL(@AcademicYear, N''), N'-', N'/')
                          AND ISNULL(RemainingAmount, 0) > 0
                          AND LTRIM(RTRIM(ISNULL(FeeType, N''))) NOT IN
                              (N'الرسوم الكلية', N'رسوم كلية', N'الرسوم الكليه', N'رسوم كليه', N'Total Fee')
                        ORDER BY CASE WHEN FeeID = @FeeID THEN 0 ELSE 1 END, FeeID;

                        IF @CurrentFeeID IS NULL BREAK;

                        SET @AppliedAmount = CASE
                            WHEN @RemainingPayment < @CurrentRemaining THEN @RemainingPayment
                            ELSE @CurrentRemaining
                        END;

                        UPDATE Fees
                        SET PaidAmount = ISNULL(PaidAmount, 0) + @AppliedAmount,
                            RemainingAmount = ISNULL(RemainingAmount, 0) - @AppliedAmount,
                            PaymentDate = @PaymentDate,
                            PaymentMethod = @PaymentMethod,
                            ReceiptNumber = @ReceiptNumber,
                            Notes = CASE
                                WHEN NULLIF(@Notes, N'') IS NULL THEN Notes
                                WHEN NULLIF(Notes, N'') IS NULL THEN @Notes
                                ELSE LEFT(Notes + N' | دفعة: ' + @Notes, 500)
                            END,
                            Status = CASE
                                WHEN ISNULL(RemainingAmount, 0) - @AppliedAmount <= 0 THEN N'مسدد'
                                ELSE N'مسدد جزئياً'
                            END,
                            UpdatedAt = GETDATE()
                        WHERE FeeID = @CurrentFeeID;

                        SET @RemainingPayment = @RemainingPayment - @AppliedAmount;
                    END;

                    /* المبلغ المستهلك فعليًا لتسوية بنود الطالب الفرعية. */
                    DECLARE @AppliedToFees DECIMAL(18,2) = @PaymentAmount - @RemainingPayment;

                    /* أي مبلغ زائد عن إجمالي الرسوم يتحول إلى رصيد دائن بالسالب
                       في الرسم الذي بدأت منه العملية، مع بقاء الدفعة كاملة في PaidAmount. */
                    IF @RemainingPayment > 0
                    BEGIN
                        UPDATE Fees
                        SET PaidAmount = ISNULL(PaidAmount, 0) + @RemainingPayment,
                            RemainingAmount = ISNULL(RemainingAmount, 0) - @RemainingPayment,
                            PaymentDate = @PaymentDate,
                            PaymentMethod = @PaymentMethod,
                            ReceiptNumber = @ReceiptNumber,
                            Notes = CASE
                                WHEN NULLIF(@Notes, N'') IS NULL THEN Notes
                                WHEN NULLIF(Notes, N'') IS NULL THEN @Notes
                                ELSE LEFT(Notes + N' | دفعة زائدة: ' + @Notes, 500)
                            END,
                            Status = N'رصيد دائن'
                        WHERE FeeID = @FeeID;
                    END;

                    /* أي سداد على بنود الطالب ينعكس تلقائيًا على بند الرسوم الكلية
                       الحقيقي: يزداد المدفوع ويتناقص المتبقي بنفس القيمة دون تغيير
                       إجمالي المبلغ، حتى يساوي المتبقي الكلي مجموع المتبقي الفرعي. */
                    IF @AppliedToFees > 0
                    BEGIN
                        UPDATE Fees
                        SET PaidAmount = ISNULL(PaidAmount, 0) + @AppliedToFees,
                            RemainingAmount = CASE
                                WHEN ISNULL(RemainingAmount, 0) - @AppliedToFees < 0 THEN 0
                                ELSE ISNULL(RemainingAmount, 0) - @AppliedToFees
                            END,
                            Status = CASE
                                WHEN ISNULL(RemainingAmount, 0) - @AppliedToFees <= 0 THEN N'مسدد'
                                ELSE N'مسدد جزئياً'
                            END,
                            UpdatedAt = GETDATE()
                        WHERE StudentID = @StudentID
                          AND REPLACE(ISNULL(AcademicYear, N''), N'-', N'/') = REPLACE(ISNULL(@AcademicYear, N''), N'-', N'/')
                          AND LTRIM(RTRIM(ISNULL(FeeType, N''))) IN
                              (N'الرسوم الكلية', N'رسوم كلية', N'الرسوم الكليه', N'رسوم كليه', N'Total Fee')
                          AND FeeID <> @FeeID;
                    END;

                    /* سند القبض التلقائي يُنشأ في نفس المعاملة مع الدفعة:
                       إما أن تُسجل الدفعة مع سندها، أو لا يُسجل شيء، لضمان التطابق
                       بين رسوم الطالب وسندات القبض — ReferenceType قياسي N'رسوم'. */
                    DECLARE @StudentName NVARCHAR(200);
                    SELECT @StudentName = ISNULL(s.FullName, N'')
                    FROM Students s
                    INNER JOIN Fees f ON f.StudentID = s.StudentID
                    WHERE f.FeeID = @FeeID;

                    DECLARE @VoucherID INT;
                    INSERT INTO Vouchers
                    (
                        VoucherNumber,
                        VoucherType,
                        Amount,
                        VoucherDate,
                        PartyName,
                        Description,
                        PaymentMethod,
                        ReferenceType,
                        ReferenceID,
                        Notes,
                        IsAutoGenerated
                    )
                    VALUES
                    (
                        NULL,
                        N'قبض',
                        @PaymentAmount,
                        @PaymentDate,
                        @StudentName,
                        N'سند قبض رسوم دراسية',
                        @PaymentMethod,
                        N'رسوم',
                        @FeeID,
                        @Notes,
                        1
                    );
                    SET @VoucherID = CONVERT(INT, SCOPE_IDENTITY());

                    SELECT
                        f.FeeID, f.StudentID, s.FullName AS StudentName, f.AcademicYear,
                        f.FeeType, f.TotalAmount, f.DiscountAmount, f.NetAmount,
                        f.PaidAmount, f.RemainingAmount, f.DueDate, f.PaymentDate,
                        f.PaymentMethod, f.ReceiptNumber, f.Status, f.Notes,
                        @VoucherID AS VoucherID
                    FROM Fees f
                    INNER JOIN Students s ON f.StudentID = s.StudentID
                    WHERE f.FeeID = @FeeID;";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {

                    cmd.Parameters.AddWithValue("@FeeID", feeId);

                    cmd.Parameters.AddWithValue("@PaymentAmount", paymentAmount);

                    cmd.Parameters.AddWithValue("@PaymentDate", paymentDate.Date);

                    cmd.Parameters.AddWithValue("@PaymentMethod", (object)(paymentMethod ?? string.Empty));

                    cmd.Parameters.AddWithValue("@ReceiptNumber", (object)(receiptNumber ?? string.Empty));

                    cmd.Parameters.AddWithValue("@Notes", (object)(notes ?? string.Empty));

                    con.Open();

                    using (SqlTransaction transaction = con.BeginTransaction(IsolationLevel.Serializable))
                    {
                        cmd.Transaction = transaction;
                        DataTable result = new DataTable();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {

                            result.Load(reader);
                        }

                        if (result.Rows.Count == 0)
                        {
                            transaction.Rollback();

                            return result;
                        }

                        /* ترقيم السند التلقائي داخل نفس المعاملة قبل الالتزام. */
                        object voucherIdValue = result.Rows[0]["VoucherID"];

                        if (voucherIdValue != null && voucherIdValue != DBNull.Value &&
                            Convert.ToInt32(voucherIdValue) > 0)
                        {
                            int generatedVoucherId = Convert.ToInt32(voucherIdValue);
                            string voucherNumber = "REC-" + paymentDate.Year + "-" + generatedVoucherId.ToString("00000");

                            using (SqlCommand numberCommand = new SqlCommand(
                                "UPDATE Vouchers SET VoucherNumber = @VoucherNumber WHERE VoucherID = @VoucherID", con, transaction))
                            {
                                numberCommand.Parameters.AddWithValue("@VoucherNumber", voucherNumber);
                                numberCommand.Parameters.AddWithValue("@VoucherID", generatedVoucherId);
                                numberCommand.ExecuteNonQuery();
                            }

                            // رقم الإيصال في سجل الرسوم مرجع عرض، ومصدره authoritative
                            // هو رقم سند القبض المنشأ في Vouchers ضمن نفس المعاملة.
                            using (SqlCommand receiptCommand = new SqlCommand(
                                "UPDATE Fees SET ReceiptNumber = @VoucherNumber WHERE FeeID = @FeeID", con, transaction))
                            {
                                receiptCommand.Parameters.AddWithValue("@VoucherNumber", voucherNumber);
                                receiptCommand.Parameters.AddWithValue("@FeeID", feeId);
                                receiptCommand.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();

                        return result;
                    }
                }
            }
        }

        // تعريف الدالة RefundPayment لتنفيذ العملية المرتبطة بها.
        // تخفيض المبلغ المدفوع (خصم دفعة مسجلة) مع التراجع تلقائياً على رسوم
        // الطالب ككل مع تحديث الحالة والمتبقي في نفس المعاملة.
        public DataTable RefundPayment(int feeId, decimal refundAmount, string notes)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                const string query = @"
                    SET NOCOUNT ON;
                    SET XACT_ABORT ON;

                    DECLARE @StudentID INT;
                    DECLARE @AcademicYear NVARCHAR(100);
                    DECLARE @RemainingRefund DECIMAL(18,2) = @RefundAmount;
                    DECLARE @CurrentFeeID INT;
                    DECLARE @CurrentPaid DECIMAL(18,2);
                    DECLARE @AppliedAmount DECIMAL(18,2);

                    SELECT @StudentID = StudentID, @AcademicYear = AcademicYear
                    FROM Fees WITH (UPDLOCK, HOLDLOCK)
                    WHERE FeeID = @FeeID;

                    IF @StudentID IS NULL OR @RefundAmount <= 0
                        THROW 51044, N'بيانات الخصم غير صحيحة.', 1;

                    /* خصم المبلغ من الدفعات المسجلة للطالب في نفس السنة؛ نبدأ من
                       الرسم المحدد ثم بقية الرسوم حتى يغطي المبلغ المطلوب. */
                    WHILE @RemainingRefund > 0
                    BEGIN
                        SET @CurrentFeeID = NULL;
                        SELECT TOP 1
                            @CurrentFeeID = FeeID,
                            @CurrentPaid = ISNULL(PaidAmount, 0)
                        FROM Fees WITH (UPDLOCK, HOLDLOCK)
                        WHERE StudentID = @StudentID
                          AND REPLACE(ISNULL(AcademicYear, N''), N'-', N'/') = REPLACE(ISNULL(@AcademicYear, N''), N'-', N'/')
                          AND ISNULL(PaidAmount, 0) > 0
                          AND LTRIM(RTRIM(ISNULL(FeeType, N''))) NOT IN
                              (N'الرسوم الكلية', N'رسوم كلية', N'الرسوم الكليه', N'رسوم كليه', N'Total Fee')
                        ORDER BY CASE WHEN FeeID = @FeeID THEN 0 ELSE 1 END, FeeID;

                        IF @CurrentFeeID IS NULL BREAK;

                        SET @AppliedAmount = CASE
                            WHEN @RemainingRefund < @CurrentPaid THEN @RemainingRefund
                            ELSE @CurrentPaid
                        END;

                        UPDATE Fees
                        SET PaidAmount = ISNULL(PaidAmount, 0) - @AppliedAmount,
                            RemainingAmount = ISNULL(RemainingAmount, 0) + @AppliedAmount,
                            PaymentDate = CASE
                                WHEN ISNULL(PaidAmount, 0) - @AppliedAmount <= 0 THEN NULL
                                ELSE PaymentDate
                            END,
                            PaymentMethod = CASE
                                WHEN ISNULL(PaidAmount, 0) - @AppliedAmount <= 0 THEN NULL
                                ELSE PaymentMethod
                            END,
                            ReceiptNumber = CASE
                                WHEN ISNULL(PaidAmount, 0) - @AppliedAmount <= 0 THEN NULL
                                ELSE ReceiptNumber
                            END,
                            Notes = CASE
                                WHEN NULLIF(@Notes, N'') IS NULL THEN Notes
                                WHEN NULLIF(Notes, N'') IS NULL THEN @Notes
                                ELSE LEFT(Notes + N' | خصم تعديل: ' + @Notes, 500)
                            END,
                            Status = CASE
                                WHEN ISNULL(PaidAmount, 0) - @AppliedAmount <= 0 THEN
                                    CASE WHEN DueDate < CONVERT(date, GETDATE()) THEN N'متأخر'
                                         ELSE N'غير مسدد'
                                    END
                                WHEN ISNULL(RemainingAmount, 0) + @AppliedAmount <= 0 THEN N'مسدد'
                                ELSE N'مسدد جزئياً'
                            END,
                            UpdatedAt = GETDATE()
                        WHERE FeeID = @CurrentFeeID;

                        SET @RemainingRefund = @RemainingRefund - @AppliedAmount;
                    END;

                    /* الخصم ينعكس تلقائيًا على بند الرسوم الكلية الحقيقي:
                       يتناقص المدفوع ويزداد المتبقي بنفس القيمة. */
                    DECLARE @RefundedTotal DECIMAL(18,2) = @RefundAmount - @RemainingRefund;
                    IF @RefundedTotal > 0
                    BEGIN
                        UPDATE Fees
                        SET PaidAmount = CASE
                                WHEN ISNULL(PaidAmount, 0) - @RefundedTotal < 0 THEN 0
                                ELSE ISNULL(PaidAmount, 0) - @RefundedTotal
                            END,
                            RemainingAmount = CASE
                                WHEN ISNULL(PaidAmount, 0) - @RefundedTotal < 0
                                    THEN ISNULL(RemainingAmount, 0) + (@RefundedTotal - ISNULL(PaidAmount, 0))
                                ELSE ISNULL(RemainingAmount, 0) + @RefundedTotal
                            END,
                            Status = CASE
                                WHEN ISNULL(PaidAmount, 0) - @RefundedTotal > 0 THEN N'مسدد جزئياً'
                                ELSE N'غير مسدد'
                            END,
                            PaymentDate = CASE
                                WHEN ISNULL(PaidAmount, 0) - @RefundedTotal <= 0 THEN NULL
                                ELSE PaymentDate
                            END,
                            UpdatedAt = GETDATE()
                        WHERE StudentID = @StudentID
                          AND REPLACE(ISNULL(AcademicYear, N''), N'-', N'/') = REPLACE(ISNULL(@AcademicYear, N''), N'-', N'/')
                          AND LTRIM(RTRIM(ISNULL(FeeType, N''))) IN
                              (N'الرسوم الكلية', N'رسوم كلية', N'الرسوم الكليه', N'رسوم كليه', N'Total Fee')
                          AND FeeID <> @FeeID;
                    END;

                    IF @RemainingRefund > 0
                        THROW 51044, N'المبلغ المطلوب خصمه أكبر من إجمالي المدفوع للطالب في هذه السنة.', 1;

                    SELECT
                        f.FeeID, f.StudentID, s.FullName AS StudentName, f.AcademicYear,
                        f.FeeType, f.TotalAmount, f.DiscountAmount, f.NetAmount,
                        f.PaidAmount, f.RemainingAmount, f.DueDate, f.PaymentDate,
                        f.PaymentMethod, f.ReceiptNumber, f.Status, f.Notes
                    FROM Fees f
                    INNER JOIN Students s ON f.StudentID = s.StudentID
                    WHERE f.FeeID = @FeeID;";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {

                    cmd.Parameters.AddWithValue("@FeeID", feeId);

                    cmd.Parameters.AddWithValue("@RefundAmount", refundAmount);

                    cmd.Parameters.AddWithValue("@Notes", (object)(notes ?? string.Empty));

                    con.Open();

                    DataTable result = new DataTable();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        adapter.Fill(result);

                    return result;
                }
            }
        }

        // تعريف الدالة DeleteFee لتنفيذ العملية المرتبطة بها.
        public bool DeleteFee(int feeId)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                const string query = @"
                    SET NOCOUNT ON;
                    SET XACT_ABORT ON;
                    BEGIN TRANSACTION;

                    DECLARE @StudentID INT, @AcademicYear NVARCHAR(100),
                            @DeletedNet DECIMAL(18,2), @DeletedTotal DECIMAL(18,2),
                            @IsTotal BIT = 0;

                    SELECT @StudentID = StudentID, @AcademicYear = AcademicYear,
                           @DeletedNet = ISNULL(NetAmount, 0),
                           @DeletedTotal = ISNULL(TotalAmount, 0)
                    FROM Fees WITH (UPDLOCK, HOLDLOCK)
                    WHERE FeeID = @FeeID;

                    IF @StudentID IS NULL
                    BEGIN
                        ROLLBACK TRANSACTION;
                        RETURN 0;
                    END

                    IF LTRIM(RTRIM(ISNULL((SELECT FeeType FROM Fees WHERE FeeID = @FeeID), N''))) IN
                        (N'الرسوم الكلية', N'رسوم كلية', N'الرسوم الكليه', N'رسوم كليه', N'Total Fee')
                        SET @IsTotal = 1;

                    DELETE FROM Fees
                    WHERE FeeID = @FeeID;

                    /* عند حذف بند فرعي، يُحدَّث بند الرسوم الكلية تلقائيًا
                       ليظل متطابقًا مع مجموع البنود المتبقية. */
                    IF @IsTotal = 0
                    BEGIN
                        UPDATE Fees
                        SET TotalAmount = ISNULL(TotalAmount, 0) - @DeletedTotal,
                            NetAmount = ISNULL(NetAmount, 0) - @DeletedNet,
                            RemainingAmount = CASE
                                WHEN ISNULL(RemainingAmount, 0) - @DeletedNet < 0 THEN 0
                                ELSE ISNULL(RemainingAmount, 0) - @DeletedNet
                            END,
                            Status = CASE
                                WHEN ISNULL(RemainingAmount, 0) - @DeletedNet <= 0 THEN N'مسدد'
                                ELSE N'مسدد جزئياً'
                            END,
                            UpdatedAt = GETDATE()
                        WHERE StudentID = @StudentID
                          AND REPLACE(ISNULL(AcademicYear, N''), N'-', N'/') = REPLACE(ISNULL(@AcademicYear, N''), N'-', N'/')
                          AND LTRIM(RTRIM(ISNULL(FeeType, N''))) IN
                              (N'الرسوم الكلية', N'رسوم كلية', N'الرسوم الكليه', N'رسوم كليه', N'Total Fee');
                    END

                    COMMIT TRANSACTION;
                    SELECT CASE WHEN @@ROWCOUNT > 0 THEN 1 ELSE 0 END;";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {

                    cmd.Parameters.AddWithValue("@FeeID", feeId);


                    con.Open();

                    object result = cmd.ExecuteScalar();
                    return result != null && result != DBNull.Value && Convert.ToInt32(result) > 0;
                }
            }
        }

        // تعريف الدالة CreateRegistrationFeeIfMissing لتنفيذ العملية المرتبطة بها.
        public int CreateRegistrationFeeIfMissing(Fee fee, string enrollmentMarker)
        {
            CalculateAmounts(fee);

            using (SqlConnection con = DbConnection.GetConnection())
            {
                const string query = @"
                    DECLARE @ResolvedFeePlanID INT = NULL;

                    /* اربط رسوم التسجيل بخطة الصف والعام إن كانت معرفة،
                       مع الإبقاء عليها كبند مستقل عن الرسوم الدراسية. */
                    /* استخدم التوزيع الفعلي، وإن لم يوجد فصف التسجيل المقبول.
                       هذا يسمح بإظهار رسوم التسجيل فور قبول الطالب وقبل التوزيع. */
                    SELECT TOP (1) @ResolvedFeePlanID = x.FeePlanID
                    FROM
                    (
                        SELECT fp.FeePlanID
                        FROM StudentClasses sc
                        INNER JOIN FeePlans fp ON fp.ClassID = sc.ClassID
                        WHERE sc.StudentID = @StudentID
                          AND REPLACE(ISNULL(sc.AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                          AND REPLACE(ISNULL(fp.AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                          AND LTRIM(RTRIM(ISNULL(fp.FeeType, N''))) IN (N'رسوم تسجيل', N'رسوم التسجيل', N'التسجيل', N'Registration Fee')

                        UNION ALL

                        SELECT fp.FeePlanID
                        FROM Enrollments e
                        INNER JOIN FeePlans fp ON fp.ClassID = e.ClassID
                        WHERE e.StudentID = @StudentID
                          AND REPLACE(ISNULL(e.AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                          AND LTRIM(RTRIM(ISNULL(e.Status, N''))) IN (N'مقبول', N'Accepted', N'Approved', N'نشط', N'Active')
                          AND REPLACE(ISNULL(fp.AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                          AND LTRIM(RTRIM(ISNULL(fp.FeeType, N''))) IN (N'رسوم تسجيل', N'رسوم التسجيل', N'التسجيل', N'Registration Fee')
                    ) x
                    ORDER BY x.FeePlanID;

                    IF @ResolvedFeePlanID IS NULL
                        THROW 51049, N'لا توجد خطة رسوم تسجيل مطابقة لصف الطالب في العام الدراسي المحدد.', 1;

                    IF @ResolvedFeePlanID IS NOT NULL
                    BEGIN
                        UPDATE Fees
                        SET FeePlanID = @ResolvedFeePlanID
                        WHERE StudentID = @StudentID
                          AND REPLACE(ISNULL(AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                          AND LTRIM(RTRIM(ISNULL(FeeType, N''))) = LTRIM(RTRIM(ISNULL(@FeeType, N'')))
                          AND FeePlanID IS NULL;
                    END;

                    DECLARE @ExistingFeeID INT = NULL;
                    DECLARE @ExistingTotalAmount DECIMAL(18,2) = NULL;
                    DECLARE @ExistingPaidAmount DECIMAL(18,2) = NULL;
                    DECLARE @ExistingPaymentMethod NVARCHAR(100) = NULL;

                    SELECT TOP 1
                        @ExistingFeeID = FeeID,
                        @ExistingTotalAmount = ISNULL(TotalAmount, 0),
                        @ExistingPaidAmount = ISNULL(PaidAmount, 0),
                        @ExistingPaymentMethod = PaymentMethod
                    FROM Fees WITH (UPDLOCK, HOLDLOCK)
                    WHERE StudentID = @StudentID
                      AND REPLACE(ISNULL(AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                      AND LTRIM(RTRIM(ISNULL(FeeType, N''))) = LTRIM(RTRIM(ISNULL(@FeeType, N'')))
                    ORDER BY FeeID;

                    IF @ExistingFeeID IS NOT NULL
                    BEGIN
                        IF ABS(ISNULL(@ExistingTotalAmount, 0) - ISNULL(@TotalAmount, 0)) > 0.01
                           OR ABS(ISNULL(@ExistingPaidAmount, 0) - ISNULL(@PaidAmount, 0)) > 0.01
                           OR
                           (
                               ISNULL(@PaidAmount, 0) > 0
                               AND LTRIM(RTRIM(ISNULL(@ExistingPaymentMethod, N''))) <>
                                   LTRIM(RTRIM(ISNULL(@PaymentMethod, N'')))
                           )
                        BEGIN
                            THROW 51050,
                                N'يوجد رسم تسجيل مالي سابق للطالب والعام الدراسي بمبالغ أو طريقة دفع مختلفة. عدّل الحركة من شاشة الرسوم بدلاً من إنشاء تسجيل غير متزامن.',
                                1;
                        END;

                        SELECT @ExistingFeeID;
                        RETURN;
                    END;

                    DECLARE @NewFeeID INT;

                    INSERT INTO Fees
                    (StudentID, FeePlanID, AcademicYear, FeeType, TotalAmount, DiscountAmount, NetAmount,
                     PaidAmount, RemainingAmount, DueDate, PaymentDate, PaymentMethod, ReceiptNumber, Status, Notes)
                    VALUES
                    (@StudentID, @ResolvedFeePlanID, @AcademicYear, @FeeType, @TotalAmount, @DiscountAmount, @NetAmount,
                     @PaidAmount, @RemainingAmount, @DueDate, @PaymentDate, @PaymentMethod, @ReceiptNumber, @Status, @Notes);

                    SET @NewFeeID = CONVERT(INT, SCOPE_IDENTITY());

                    /* سند قبض رسوم التسجيل يُنشأ في نفس المعاملة مع الرسم الجديد
                       وعند وجود دفعة فقط؛ إعادة حفظ التسجيل لا تكرر السند. */
                    IF @PaidAmount > 0
                    BEGIN
                        DECLARE @NewVoucherID INT;

                        INSERT INTO Vouchers
                        (VoucherNumber, VoucherType, Amount, VoucherDate, PartyName, Description,
                         PaymentMethod, ReferenceType, ReferenceID, Notes, IsAutoGenerated)
                        VALUES
                        (NULL, N'قبض', @PaidAmount, @PaymentDate,
                         N'طالب رقم ' + CONVERT(NVARCHAR(20), @StudentID),
                         N'سند قبض رسوم دراسية', @PaymentMethod, N'رسوم', @NewFeeID, @Notes, 1);

                        SET @NewVoucherID = CONVERT(INT, SCOPE_IDENTITY());

                        UPDATE Vouchers
                        SET VoucherNumber = N'REC-'
                            + CONVERT(NVARCHAR(4), YEAR(@PaymentDate))
                            + N'-'
                            + RIGHT(N'00000' + CONVERT(NVARCHAR(10), @NewVoucherID), 5)
                        WHERE VoucherID = @NewVoucherID;
                    END;

                    SELECT @NewFeeID;";


                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    AddParameters(cmd, fee);

                    con.Open();

                    using (SqlTransaction transaction = con.BeginTransaction(IsolationLevel.Serializable))
                    {
                        cmd.Transaction = transaction;

                        object result = cmd.ExecuteScalar();

                        transaction.Commit();

                        return result == null || result == DBNull.Value ? 0 : Convert.ToInt32(result);
                    }
                }
            }
        }

        // تعريف الدالة GenerateStudentFeesFromPlans لتنفيذ العملية المرتبطة بها.
        public int GenerateStudentFeesFromPlans(int studentId, string academicYear)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string query = @"
                    SET NOCOUNT ON;
                    SET XACT_ABORT ON;
                    BEGIN TRANSACTION;
                    DECLARE @ClassID INT;

                    /* لا رسوم صفية قبل قبول التسجيل في العام المحدد. */
                    IF NOT EXISTS
                    (
                        SELECT 1
                        FROM Enrollments e
                        WHERE e.StudentID = @StudentID
                          AND REPLACE(ISNULL(e.AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                          AND LTRIM(RTRIM(ISNULL(e.Status, N''))) IN (N'مقبول', N'Accepted', N'Approved', N'نشط', N'Active')
                    )
                    BEGIN
                        ROLLBACK TRANSACTION;
                        THROW 51040, N'لا يمكن توليد رسوم الصف قبل قبول تسجيل الطالب في العام الدراسي المحدد.', 1;
                    END;

                    /* الأولوية للتوزيع الفعلي؛ وقبل التوزيع نستخدم صف التسجيل المقبول
                       حتى تظهر خطط الرسوم مباشرة في سيناريو التسجيل والقبول. */
                    SELECT TOP (1) @ClassID = sc.ClassID
                    FROM StudentClasses sc
                    WHERE sc.StudentID = @StudentID
                      AND REPLACE(ISNULL(sc.AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                    ORDER BY sc.AssignedDate DESC, sc.StudentClassID DESC;

                    IF @ClassID IS NULL
                    BEGIN
                        SELECT TOP (1) @ClassID = e.ClassID
                        FROM Enrollments e
                        WHERE e.StudentID = @StudentID
                          AND REPLACE(ISNULL(e.AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                          AND LTRIM(RTRIM(ISNULL(e.Status, N''))) IN (N'مقبول', N'Accepted', N'Approved', N'نشط', N'Active')
                        ORDER BY e.EnrollmentID DESC;
                    END

                    IF @ClassID IS NULL
                    BEGIN
                        ROLLBACK TRANSACTION;
                        THROW 51041, N'لا يوجد صف مرتبط بتسجيل الطالب المقبول؛ أكمل الصف أو التوزيع أولاً.', 1;
                    END

                    /*
                       الرسوم الكلية بند حقيقي يظهر لكل طالب كسطر مستقل بنفس
                       القيمة الإجمالية، بينما تُوزَّع قيمة البنود الفرعية
                       (تسجيل، دراسية، كتب، زي، مواصلات، امتحانات، أنشطة...)
                       نسبيًا بحيث يساوي مجموعها القيمة الكلية تمامًا.
                    */
                    ;WITH ChildPlans AS
                    (
                        SELECT fp.*,
                               ROW_NUMBER() OVER (ORDER BY fp.FeePlanID) AS PlanNo,
                               COUNT(*) OVER () AS PlanCount,
                               SUM(fp.Amount) OVER () AS ChildTotal
                        FROM FeePlans fp
                        WHERE fp.ClassID = @ClassID
                          AND REPLACE(ISNULL(fp.AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                          AND LTRIM(RTRIM(ISNULL(fp.FeeType, N''))) NOT IN
                              (N'الرسوم الكلية', N'رسوم كلية', N'الرسوم الكليه', N'رسوم كليه', N'Total Fee')
                    ), AllocatedPlans AS
                    (
                        SELECT cp.*,
                               MAX(CASE WHEN LTRIM(RTRIM(ISNULL(fp.FeeType, N''))) IN
                                    (N'الرسوم الكلية', N'رسوم كلية', N'الرسوم الكليه', N'رسوم كليه', N'Total Fee')
                                    THEN fp.Amount END)
                                   OVER () AS TotalPlanAmount,
                               ISNULL(MAX(CASE WHEN LTRIM(RTRIM(ISNULL(fp.FeeType, N''))) IN
                                    (N'الرسوم الكلية', N'رسوم كلية', N'الرسوم الكليه', N'رسوم كليه', N'Total Fee')
                                    THEN fp.FeePlanID END)
                                   OVER (), 0) AS TotalPlanID
                        FROM ChildPlans cp
                        LEFT JOIN FeePlans fp
                          ON fp.ClassID = @ClassID
                         AND REPLACE(ISNULL(fp.AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                         AND LTRIM(RTRIM(ISNULL(fp.FeeType, N''))) IN
                             (N'الرسوم الكلية', N'رسوم كلية', N'الرسوم الكليه', N'رسوم كليه', N'Total Fee')
                    )
                    INSERT INTO Fees
                    (
                        StudentID, FeePlanID, AcademicYear, FeeType, TotalAmount,
                        DiscountAmount, NetAmount, PaidAmount, RemainingAmount,
                        DueDate, PaymentDate, PaymentMethod, ReceiptNumber, Status, Notes
                    )
                    SELECT
                        @StudentID,
                        ap.FeePlanID,
                        ap.AcademicYear,
                        ap.FeeType,
                        allocation.Amount,
                        0,
                        allocation.Amount,
                        0,
                        allocation.Amount,
                        ap.DueDate,
                        NULL,
                        NULL,
                        NULL,
                        N'غير مسدد',
                        ap.Notes
                    FROM AllocatedPlans ap
                    CROSS APPLY
                    (
                        SELECT CAST(
                            CASE
                                WHEN ap.TotalPlanAmount IS NULL OR ap.ChildTotal <= 0 THEN ap.Amount
                                WHEN ap.PlanNo < ap.PlanCount THEN ROUND(ap.TotalPlanAmount * ap.Amount / ap.ChildTotal, 2)
                                ELSE CASE WHEN ap.TotalPlanAmount -
                                              ISNULL((SELECT SUM(ROUND(ap2.TotalPlanAmount * ap2.Amount / ap2.ChildTotal, 2))
                                                      FROM AllocatedPlans ap2 WHERE ap2.PlanNo < ap.PlanNo), 0) < 0
                                          THEN 0
                                          ELSE ap.TotalPlanAmount -
                                               ISNULL((SELECT SUM(ROUND(ap2.TotalPlanAmount * ap2.Amount / ap2.ChildTotal, 2))
                                                       FROM AllocatedPlans ap2 WHERE ap2.PlanNo < ap.PlanNo), 0)
                                     END
                            END AS DECIMAL(18,2)) AS Amount
                    ) allocation
                    WHERE NOT EXISTS
                    (
                        SELECT 1 FROM Fees f
                        WHERE f.StudentID = @StudentID
                          AND f.FeePlanID = ap.FeePlanID
                          AND REPLACE(ISNULL(f.AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                    );

                    DECLARE @Generated INT = @@ROWCOUNT;

                    /* بند الرسوم الكلية الحقيقي للطالب: سطر مستقل بقيمة
                       إجمالية تساوي مجموع البنود الفرعية (أو قيمة الخطة الكلية). */
                    DECLARE @TotalFeePlanID INT;
                    DECLARE @TotalFeeAmount DECIMAL(18,2);
                    SELECT TOP (1)
                        @TotalFeePlanID = FeePlanID,
                        @TotalFeeAmount = Amount
                    FROM FeePlans
                    WHERE ClassID = @ClassID
                      AND REPLACE(ISNULL(AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                      AND LTRIM(RTRIM(ISNULL(FeeType, N''))) IN
                          (N'الرسوم الكلية', N'رسوم كلية', N'الرسوم الكليه', N'رسوم كليه', N'Total Fee')
                    ORDER BY FeePlanID;

                    IF @TotalFeePlanID IS NULL
                    BEGIN
                        SELECT @TotalFeeAmount = ISNULL(SUM(Amount), 0)
                        FROM FeePlans
                        WHERE ClassID = @ClassID
                          AND REPLACE(ISNULL(AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                          AND LTRIM(RTRIM(ISNULL(FeeType, N''))) NOT IN
                              (N'الرسوم الكلية', N'رسوم كلية', N'الرسوم الكليه', N'رسوم كليه', N'Total Fee');
                    END

                    IF @TotalFeeAmount > 0 AND NOT EXISTS
                    (
                        SELECT 1 FROM Fees f
                        WHERE f.StudentID = @StudentID
                          AND REPLACE(ISNULL(f.AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                          AND LTRIM(RTRIM(ISNULL(f.FeeType, N''))) IN
                              (N'الرسوم الكلية', N'رسوم كلية', N'الرسوم الكليه', N'رسوم كليه', N'Total Fee')
                    )
                    BEGIN
                        INSERT INTO Fees
                        (
                            StudentID, FeePlanID, AcademicYear, FeeType, TotalAmount,
                            DiscountAmount, NetAmount, PaidAmount, RemainingAmount,
                            DueDate, PaymentDate, PaymentMethod, ReceiptNumber, Status, Notes
                        )
                        SELECT TOP (1)
                            @StudentID,
                            NULLIF(@TotalFeePlanID, 0),
                            @AcademicYear,
                            N'الرسوم الكلية',
                            @TotalFeeAmount,
                            0,
                            @TotalFeeAmount,
                            0,
                            @TotalFeeAmount,
                            fp.DueDate,
                            NULL,
                            NULL,
                            NULL,
                            N'غير مسدد',
                            fp.Notes
                        FROM FeePlans fp
                        WHERE fp.ClassID = @ClassID
                          AND REPLACE(ISNULL(fp.AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                        ORDER BY fp.FeePlanID;

                        SET @Generated = @Generated + 1;
                    END

                    COMMIT TRANSACTION;
                    SELECT @Generated;";


                using (SqlCommand cmd = new SqlCommand(query, con))
                {

                    cmd.Parameters.AddWithValue("@StudentID", studentId);

                    cmd.Parameters.AddWithValue("@AcademicYear", (academicYear ?? string.Empty).Trim().Replace('-', '/'));


                    con.Open();


                    object result = cmd.ExecuteScalar();


                    if (result == null || result == DBNull.Value)
                        return 0;


                    return Convert.ToInt32(result);
                }
            }
        }

        // تعريف الدالة AddParameters لتنفيذ العملية المرتبطة بها.
        private void AddParameters(SqlCommand cmd, Fee fee)
        {

            cmd.Parameters.AddWithValue("@StudentID", fee.StudentID);


            if (fee.FeePlanID.HasValue)
                cmd.Parameters.AddWithValue("@FeePlanID", fee.FeePlanID.Value);

            else

                cmd.Parameters.AddWithValue("@FeePlanID", DBNull.Value);


            cmd.Parameters.AddWithValue("@AcademicYear", fee.AcademicYear ?? "");

            cmd.Parameters.AddWithValue("@FeeType", fee.FeeType ?? "");


            cmd.Parameters.AddWithValue("@TotalAmount", fee.TotalAmount);

            cmd.Parameters.AddWithValue("@DiscountAmount", fee.DiscountAmount);

            cmd.Parameters.AddWithValue("@NetAmount", fee.NetAmount);

            cmd.Parameters.AddWithValue("@PaidAmount", fee.PaidAmount);

            cmd.Parameters.AddWithValue("@RemainingAmount", fee.RemainingAmount);


            cmd.Parameters.AddWithValue("@DueDate", fee.DueDate.Date);


            if (fee.PaymentDate.HasValue)
                cmd.Parameters.AddWithValue("@PaymentDate", fee.PaymentDate.Value.Date);

            else

                cmd.Parameters.AddWithValue("@PaymentDate", DBNull.Value);


            cmd.Parameters.AddWithValue(

                "@PaymentMethod",
                string.IsNullOrWhiteSpace(fee.PaymentMethod)
                    ? (object)DBNull.Value
                    : fee.PaymentMethod
            );


            cmd.Parameters.AddWithValue(

                "@ReceiptNumber",
                string.IsNullOrWhiteSpace(fee.ReceiptNumber)
                    ? (object)DBNull.Value
                    : fee.ReceiptNumber
            );


            cmd.Parameters.AddWithValue("@Status", fee.Status ?? "غير مسدد");


            cmd.Parameters.AddWithValue(

                "@Notes",
                string.IsNullOrWhiteSpace(fee.Notes)
                    ? (object)DBNull.Value
                    : fee.Notes
            );
        }

        // تعريف الدالة CalculateAmounts لتنفيذ العملية المرتبطة بها.
        private void CalculateAmounts(Fee fee)
        {
            if (fee.TotalAmount < 0)
                fee.TotalAmount = 0;

            if (fee.DiscountAmount < 0)
                fee.DiscountAmount = 0;

            if (fee.PaidAmount < 0)
                fee.PaidAmount = 0;

            if (fee.DiscountAmount > fee.TotalAmount)
                fee.DiscountAmount = fee.TotalAmount;

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
                /* الرصيد الدائن يُحفظ بالسالب ولا يقصّ؛ لا نخسر مبلغ الدفعة الزائد. */
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
