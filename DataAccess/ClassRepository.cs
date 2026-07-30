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
                string query = @"
                    UPDATE Classes
                    SET
                        ClassName = @ClassName,
                        StageName = @StageName,
                        GradeOrder = @GradeOrder,
                        IsActive = @IsActive,
                        Notes = @Notes,
                        UpdatedAt = GETDATE()
                    WHERE ClassID = @ClassID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ClassID", item.ClassID);
                    cmd.Parameters.AddWithValue("@ClassName", item.ClassName.Trim());
                    cmd.Parameters.AddWithValue("@StageName", item.StageName.Trim());
                    cmd.Parameters.AddWithValue("@GradeOrder", item.GradeOrder);
                    cmd.Parameters.AddWithValue("@IsActive", item.IsActive);

                    if (string.IsNullOrWhiteSpace(item.Notes))
                        cmd.Parameters.AddWithValue("@Notes", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@Notes", item.Notes.Trim());

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}
