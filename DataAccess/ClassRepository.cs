using System;
using System.Data;
using System.Data.SqlClient;
using SchoolSystem.Models;

namespace SchoolSystem.DataAccess
{
    public class ClassRepository
    {
        public DataTable GetAllClasses()
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = @"
                    SELECT ClassID, ClassName
                    FROM Classes
                    WHERE ISNULL(IsActive, 1) = 1
                    ORDER BY ISNULL(GradeOrder, ClassID)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetClassDetails()
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = @"
                    SELECT
                        ClassID,
                        ClassCode,
                        ClassName,
                        StageName,
                        GradeOrder,
                        IsActive,
                        Notes,
                        CreatedAt,
                        UpdatedAt
                    FROM Classes
                    ORDER BY ISNULL(GradeOrder, ClassID)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        public bool UpdateClass(SchoolClass item)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction(IsolationLevel.Serializable))
                {
                    if (!item.IsActive)
                    {
                        const string activeClassQuery = @"
                            SELECT COUNT(1)
                            FROM Classes
                            WHERE ISNULL(IsActive, 1) = 1
                              AND ClassID <> @ClassID";

                        using (SqlCommand activeClassCommand = new SqlCommand(activeClassQuery, conn, transaction))
                        {
                            activeClassCommand.Parameters.Add("@ClassID", SqlDbType.Int).Value = item.ClassID;
                            int activeClassCount = Convert.ToInt32(activeClassCommand.ExecuteScalar());
                            if (activeClassCount == 0)
                            {
                                transaction.Rollback();
                                throw new InvalidOperationException(
                                    "لا يمكن تعطيل آخر فصل نشط في النظام. أبقِ فصلاً واحداً نشطاً على الأقل.");
                            }
                        }
                    }

                    const string query = @"
                        UPDATE Classes
                        SET
                            ClassName = @ClassName,
                            StageName = @StageName,
                            GradeOrder = @GradeOrder,
                            IsActive = @IsActive,
                            Notes = @Notes,
                            UpdatedAt = GETDATE()
                        WHERE ClassID = @ClassID";

                    using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
                    {
                        cmd.Parameters.Add("@ClassID", SqlDbType.Int).Value = item.ClassID;
                        cmd.Parameters.Add("@ClassName", SqlDbType.NVarChar, 100).Value = item.ClassName.Trim();
                        cmd.Parameters.Add("@StageName", SqlDbType.NVarChar, 100).Value = item.StageName.Trim();
                        cmd.Parameters.Add("@GradeOrder", SqlDbType.Int).Value = item.GradeOrder;
                        cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = item.IsActive;
                        cmd.Parameters.Add("@Notes", SqlDbType.NVarChar, -1).Value =
                            string.IsNullOrWhiteSpace(item.Notes) ? (object)DBNull.Value : item.Notes.Trim();

                        if (cmd.ExecuteNonQuery() == 0)
                            throw new InvalidOperationException("الفصل غير موجود أو تم تعديله مسبقاً.");
                    }

                    transaction.Commit();
                    return true;
                }
            }
        }
    }
}
