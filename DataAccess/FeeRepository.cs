using System;
using System.Data;
using System.Data.SqlClient;
using SchoolSystem.Models;

namespace SchoolSystem.DataAccess
{
    public class FeeRepository
    {
        public DataTable GetAllFees()
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string query = @"
                    SELECT
                        f.FeeID,
                        f.StudentID,
                        s.FullName AS StudentName,
                        s.ClassID,
                        c.ClassName,
                        f.FeePlanID,
                        f.AcademicYear,
                        f.FeeType,
                        f.TotalAmount,
                        f.DiscountAmount,
                        f.NetAmount,
                        f.PaidAmount,
                        f.RemainingAmount,
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
                    LEFT JOIN Classes c ON s.ClassID = c.ClassID
                    ORDER BY f.FeeID DESC";

                using (SqlDataAdapter da = new SqlDataAdapter(query, con))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        public int AddFee(Fee fee)
        {
            CalculateAmounts(fee);

            using (SqlConnection con = DbConnection.GetConnection())
            {
                string query = @"
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

        public bool UpdateFee(Fee fee)
        {
            CalculateAmounts(fee);

            using (SqlConnection con = DbConnection.GetConnection())
            {
                string query = @"
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

        public DataTable RecordPayment(int feeId, decimal paymentAmount, DateTime paymentDate, string paymentMethod, string receiptNumber, string notes)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                const string query = @"
                    UPDATE Fees
                    SET
                        PaidAmount = ISNULL(PaidAmount, 0) + @PaymentAmount,
                        RemainingAmount = ISNULL(RemainingAmount, 0) - @PaymentAmount,
                        PaymentDate = @PaymentDate,
                        PaymentMethod = @PaymentMethod,
                        ReceiptNumber = @ReceiptNumber,
                        Notes = CASE
                            WHEN NULLIF(@Notes, N'') IS NULL THEN Notes
                            WHEN NULLIF(Notes, N'') IS NULL THEN @Notes
                            ELSE LEFT(Notes + N' | دفعة: ' + @Notes, 500)
                        END,
                        Status = CASE
                            WHEN ISNULL(RemainingAmount, 0) - @PaymentAmount <= 0 THEN N'مسدد'
                            ELSE N'مسدد جزئياً'
                        END,
                        UpdatedAt = GETDATE()
                    WHERE FeeID = @FeeID
                      AND @PaymentAmount > 0
                      AND @PaymentAmount <= ISNULL(RemainingAmount, 0);

                    SELECT
                        f.FeeID,
                        f.StudentID,
                        s.FullName AS StudentName,
                        f.AcademicYear,
                        f.FeeType,
                        f.TotalAmount,
                        f.DiscountAmount,
                        f.NetAmount,
                        f.PaidAmount,
                        f.RemainingAmount,
                        f.DueDate,
                        f.PaymentDate,
                        f.PaymentMethod,
                        f.ReceiptNumber,
                        f.Status,
                        f.Notes
                    FROM Fees f
                    INNER JOIN Students s ON f.StudentID = s.StudentID
                    WHERE f.FeeID = @FeeID
                      AND @PaymentAmount > 0;";

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

                        transaction.Commit();
                        return result;
                    }
                }
            }
        }

        public bool DeleteFee(int feeId)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                const string query = @"
                    IF EXISTS
                    (
                        SELECT 1
                        FROM Fees
                        WHERE FeeID = @FeeID
                          AND (ISNULL(PaidAmount, 0) > 0
                               OR PaymentDate IS NOT NULL
                               OR Status IN (N'مدفوع', N'مدفوع جزئياً'))
                    )
                    BEGIN
                        THROW 51003, N'لا يمكن حذف رسم تم تسجيل دفعة عليه. استخدم التعديل أو التسوية بدلاً من الحذف.', 1;
                    END;

                    DELETE FROM Fees
                    WHERE FeeID = @FeeID
                      AND ISNULL(PaidAmount, 0) = 0
                      AND PaymentDate IS NULL
                      AND ISNULL(Status, N'غير مدفوع') NOT IN (N'مدفوع', N'مدفوع جزئياً');";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@FeeID", feeId);

                    con.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public int CreateRegistrationFeeIfMissing(Fee fee, string enrollmentMarker)
        {
            CalculateAmounts(fee);

            using (SqlConnection con = DbConnection.GetConnection())
            {
                const string query = @"
                    IF EXISTS
                    (
                        SELECT 1
                        FROM Fees WITH (UPDLOCK, HOLDLOCK)
                        WHERE StudentID = @StudentID
                          AND AcademicYear = @AcademicYear
                          AND FeeType = @FeeType
                          AND Notes = @Notes
                    )
                    BEGIN
                        SELECT TOP 1 FeeID
                        FROM Fees
                        WHERE StudentID = @StudentID
                          AND AcademicYear = @AcademicYear
                          AND FeeType = @FeeType
                          AND Notes = @Notes
                        RETURN;
                    END;

                    INSERT INTO Fees
                    (StudentID, FeePlanID, AcademicYear, FeeType, TotalAmount, DiscountAmount, NetAmount,
                     PaidAmount, RemainingAmount, DueDate, PaymentDate, PaymentMethod, ReceiptNumber, Status, Notes)
                    VALUES
                    (@StudentID, NULL, @AcademicYear, @FeeType, @TotalAmount, @DiscountAmount, @NetAmount,
                     @PaidAmount, @RemainingAmount, @DueDate, @PaymentDate, @PaymentMethod, @ReceiptNumber, @Status, @Notes);

                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

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

        public int GenerateStudentFeesFromPlans(int studentId, string academicYear)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string query = @"
                    DECLARE @ClassID INT;

                    SELECT @ClassID = ClassID
                    FROM Students
                    WHERE StudentID = @StudentID;

                    IF @ClassID IS NULL
                    BEGIN
                        SELECT 0;
                        RETURN;
                    END

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
                    SELECT
                        @StudentID,
                        fp.FeePlanID,
                        fp.AcademicYear,
                        fp.FeeType,
                        fp.Amount,
                        0,
                        fp.Amount,
                        0,
                        fp.Amount,
                        fp.DueDate,
                        NULL,
                        NULL,
                        NULL,
                        N'غير مسدد',
                        fp.Notes
                    FROM FeePlans fp
                    WHERE fp.ClassID = @ClassID
                      AND fp.AcademicYear = @AcademicYear
                      AND NOT EXISTS
                      (
                          SELECT 1
                          FROM Fees f
                          WHERE f.StudentID = @StudentID
                            AND f.FeePlanID = fp.FeePlanID
                            AND f.AcademicYear = @AcademicYear
                      );

                    SELECT @@ROWCOUNT;";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@StudentID", studentId);
                    cmd.Parameters.AddWithValue("@AcademicYear", academicYear);

                    con.Open();

                    object result = cmd.ExecuteScalar();

                    if (result == null || result == DBNull.Value)
                        return 0;

                    return Convert.ToInt32(result);
                }
            }
        }

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

            if (fee.RemainingAmount < 0)
                fee.RemainingAmount = 0;

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
