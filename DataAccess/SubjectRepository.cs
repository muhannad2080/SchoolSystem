using System;
using System.Data;
using System.Data.SqlClient;
using SchoolSystem.Models;

namespace SchoolSystem.DataAccess
{
    public class SubjectRepository
    {
        public DataTable GetAllSubjects()
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = @"
                    SELECT
                        s.SubjectID,
                        s.SubjectCode,
                        s.SubjectName,
                        s.ClassID,
                        c.ClassName,
                        s.MaxDegree,
                        s.PassDegree,
                        s.IsActive,
                        s.Notes,
                        s.CreatedAt,
                        s.UpdatedAt
                    FROM Subjects s
                    LEFT JOIN Classes c ON s.ClassID = c.ClassID
                    ORDER BY c.ClassID, s.SubjectName";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetActiveSubjects()
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = @"
                    SELECT
                        SubjectID,
                        SubjectName
                    FROM Subjects
                    WHERE IsActive = 1
                    ORDER BY SubjectName";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetActiveSubjectsByClass(int classId)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = @"
                    SELECT
                        SubjectID,
                        SubjectName
                    FROM Subjects
                    WHERE IsActive = 1
                      AND (ClassID = @ClassID OR ClassID IS NULL)
                    ORDER BY SubjectName";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ClassID", classId);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        public bool UpdateSubject(Subject subject)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = @"
                    UPDATE Subjects
                    SET
                        MaxDegree = @MaxDegree,
                        PassDegree = @PassDegree,
                        IsActive = @IsActive,
                        Notes = @Notes,
                        UpdatedAt = GETDATE()
                    WHERE SubjectID = @SubjectID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@SubjectID", subject.SubjectID);
                    cmd.Parameters.AddWithValue("@MaxDegree", subject.MaxDegree);
                    cmd.Parameters.AddWithValue("@PassDegree", subject.PassDegree);
                    cmd.Parameters.AddWithValue("@IsActive", subject.IsActive);

                    if (string.IsNullOrWhiteSpace(subject.Notes))
                        cmd.Parameters.AddWithValue("@Notes", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@Notes", subject.Notes.Trim());

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public int GetSubjectCount()
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = "SELECT COUNT(*) FROM Subjects WHERE ISNULL(IsActive, 1) = 1";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }
    }
}
