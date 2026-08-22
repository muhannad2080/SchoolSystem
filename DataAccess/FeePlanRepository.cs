using System;
using System.Data;
using System.Data.SqlClient;
using SchoolSystem.Models;

namespace SchoolSystem.DataAccess
{
    public class FeePlanRepository
    {
        public DataTable GetAllFeePlans()
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string query = @"
                    SELECT 
                        fp.FeePlanID,
                        fp.AcademicYear,
                        fp.ClassID,
                        c.ClassName,
                        fp.FeeType,
                        fp.Amount,
                        fp.DueDate,
                        fp.IsRequired,
                        fp.Notes,
                        fp.CreatedAt
                    FROM FeePlans fp
                    INNER JOIN Classes c ON fp.ClassID = c.ClassID
                    ORDER BY fp.AcademicYear DESC, c.ClassName, fp.FeeType";

                using (SqlDataAdapter da = new SqlDataAdapter(query, con))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetClasses()
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string query = @"
                    SELECT ClassID, ClassName
                    FROM Classes
                    ORDER BY ClassName";

                using (SqlDataAdapter da = new SqlDataAdapter(query, con))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        public bool AddFeePlan(FeePlan plan)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string query = @"
                    IF EXISTS
                    (
                        SELECT 1 FROM FeePlans WITH (UPDLOCK, HOLDLOCK)
                        WHERE REPLACE(ISNULL(AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                          AND ClassID = @ClassID
                          AND FeeType = @FeeType
                    )
                        THROW 51020, N'توجد خطة رسوم مكررة لنفس العام والصف ونوع الرسوم.', 1;

                    INSERT INTO FeePlans
                    (
                        AcademicYear,
                        ClassID,
                        FeeType,
                        Amount,
                        DueDate,
                        IsRequired,
                        Notes
                    )
                    VALUES
                    (
                        @AcademicYear,
                        @ClassID,
                        @FeeType,
                        @Amount,
                        @DueDate,
                        @IsRequired,
                        @Notes
                    );

                    DECLARE @NewFeePlanID INT = CONVERT(INT, SCOPE_IDENTITY());

                    /* نشر الخطة تلقائياً للطلاب المقبولين والموزعين في الصف نفسه. */
                    INSERT INTO Fees
                    (
                        StudentID, FeePlanID, AcademicYear, FeeType, TotalAmount,
                        DiscountAmount, NetAmount, PaidAmount, RemainingAmount,
                        DueDate, PaymentDate, PaymentMethod, ReceiptNumber, Status, Notes
                    )
                    SELECT DISTINCT
                        sc.StudentID,
                        @NewFeePlanID,
                        @AcademicYear,
                        @FeeType,
                        @Amount,
                        0,
                        @Amount,
                        0,
                        @Amount,
                        @DueDate,
                        NULL,
                        NULL,
                        NULL,
                        N'غير مسدد',
                        @Notes
                    FROM StudentClasses sc
                    INNER JOIN Enrollments e ON e.StudentID = sc.StudentID
                        AND REPLACE(ISNULL(e.AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                        AND LTRIM(RTRIM(ISNULL(e.Status, N''))) = N'مقبول'
                    WHERE sc.ClassID = @ClassID
                      AND REPLACE(ISNULL(sc.AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                      AND NOT EXISTS
                      (
                          SELECT 1 FROM Fees f
                          WHERE f.StudentID = sc.StudentID
                            AND f.FeePlanID = @NewFeePlanID
                      )";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    AddParameters(cmd, plan);

                    con.Open();
                    using (SqlTransaction transaction = con.BeginTransaction(IsolationLevel.Serializable))
                    {
                        cmd.Transaction = transaction;
                        int affected = cmd.ExecuteNonQuery();
                        transaction.Commit();
                        return affected > 0;
                    }
                }
            }
        }

        public bool UpdateFeePlan(FeePlan plan)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string query = @"
                    SET NOCOUNT ON;
                    SET XACT_ABORT ON;
                    BEGIN TRANSACTION;

                    IF EXISTS
                    (
                        SELECT 1 FROM FeePlans WITH (UPDLOCK, HOLDLOCK)
                        WHERE REPLACE(ISNULL(AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                          AND ClassID = @ClassID
                          AND FeeType = @FeeType
                          AND FeePlanID <> @FeePlanID
                    )
                        THROW 51021, N'توجد خطة رسوم مكررة لنفس العام والصف ونوع الرسوم.', 1;

                    IF EXISTS
                    (
                        SELECT 1 FROM Fees f
                        INNER JOIN FeePlans oldPlan ON oldPlan.FeePlanID = f.FeePlanID
                        WHERE f.FeePlanID = @FeePlanID
                          AND (
                              REPLACE(ISNULL(oldPlan.AcademicYear, N''), N'-', N'/') <> REPLACE(@AcademicYear, N'-', N'/')
                              OR oldPlan.ClassID <> @ClassID
                              OR oldPlan.FeeType <> @FeeType
                          )
                    )
                        THROW 51022, N'لا يمكن تغيير صف أو عام أو نوع خطة مستخدمة في رسوم موجودة. أنشئ خطة جديدة بدلاً من تغيير هوية الخطة.', 1;

                    UPDATE FeePlans SET
                        AcademicYear = @AcademicYear,
                        ClassID = @ClassID,
                        FeeType = @FeeType,
                        Amount = @Amount,
                        DueDate = @DueDate,
                        IsRequired = @IsRequired,
                        Notes = @Notes
                    WHERE FeePlanID = @FeePlanID;

                    /* تحديث البنود غير المسددة فقط؛ السجلات المسددة تبقى تاريخية. */
                    UPDATE f SET
                        TotalAmount = @Amount,
                        NetAmount = CASE WHEN f.DiscountAmount > @Amount THEN 0 ELSE @Amount - f.DiscountAmount END,
                        RemainingAmount = CASE WHEN f.DiscountAmount > @Amount THEN 0 ELSE @Amount - f.DiscountAmount - f.PaidAmount END,
                        DueDate = @DueDate,
                        Notes = @Notes,
                        UpdatedAt = GETDATE()
                    FROM Fees f
                    WHERE f.FeePlanID = @FeePlanID
                      AND f.PaidAmount = 0;

                    COMMIT TRANSACTION";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@FeePlanID", plan.FeePlanID);
                    AddParameters(cmd, plan);

                    con.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool DeleteFeePlan(int feePlanId)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                const string query = @"
                    IF EXISTS (SELECT 1 FROM Fees WHERE FeePlanID = @FeePlanID)
                        THROW 51003, N'لا يمكن حذف خطة الرسوم لأنها مستخدمة في رسوم طلابية. عطّلها أو عدّلها بدلاً من حذفها.', 1;

                    DELETE FROM FeePlans
                    WHERE FeePlanID = @FeePlanID;";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@FeePlanID", feePlanId);

                    con.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        private void AddParameters(SqlCommand cmd, FeePlan plan)
        {
            cmd.Parameters.AddWithValue("@AcademicYear", plan.AcademicYear ?? "");
            cmd.Parameters.AddWithValue("@ClassID", plan.ClassID);
            cmd.Parameters.AddWithValue("@FeeType", plan.FeeType ?? "");
            cmd.Parameters.AddWithValue("@Amount", plan.Amount);
            cmd.Parameters.AddWithValue("@DueDate", plan.DueDate.Date);
            cmd.Parameters.AddWithValue("@IsRequired", plan.IsRequired);

            cmd.Parameters.AddWithValue(
                "@Notes",
                string.IsNullOrWhiteSpace(plan.Notes)
                    ? (object)DBNull.Value
                    : plan.Notes
            );
        }
    }
}
