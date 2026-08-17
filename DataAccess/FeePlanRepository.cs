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
                    IF EXISTS
                    (
                        SELECT 1 FROM FeePlans WITH (UPDLOCK, HOLDLOCK)
                        WHERE REPLACE(ISNULL(AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                          AND ClassID = @ClassID
                          AND FeeType = @FeeType
                          AND FeePlanID <> @FeePlanID
                    )
                        THROW 51021, N'توجد خطة رسوم مكررة لنفس العام والصف ونوع الرسوم.', 1;

                    UPDATE FeePlans SET
                        AcademicYear = @AcademicYear,
                        ClassID = @ClassID,
                        FeeType = @FeeType,
                        Amount = @Amount,
                        DueDate = @DueDate,
                        IsRequired = @IsRequired,
                        Notes = @Notes
                    WHERE FeePlanID = @FeePlanID";

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
