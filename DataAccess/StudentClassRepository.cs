using System;
using System.Data;
using System.Data.SqlClient;
using SchoolSystem.Models;

namespace SchoolSystem.DataAccess
{
    public class StudentClassRepository
    {
        public DataTable GetUnassignedStudents(string academicYear)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = @"
                    SELECT 
                        s.StudentID,
                        s.StudentNumber,
                        s.FullName AS StudentName,
                        s.Gender,
                        s.Phone
                    FROM Students s
                    WHERE ISNULL(s.Status, N'منتظم') = N'منتظم'
                      AND NOT EXISTS
                      (
                          SELECT 1
                          FROM StudentClasses sc
                          WHERE sc.StudentID = s.StudentID
                            AND sc.AcademicYear = @AcademicYear
                      )
                    ORDER BY s.FullName";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@AcademicYear", academicYear);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        public DataTable GetAssignedStudents(int classId, string section, string academicYear)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = @"
                    SELECT
                        sc.StudentClassID,
                        sc.StudentID,
                        s.StudentNumber,
                        s.FullName AS StudentName,
                        s.Gender,
                        s.Phone,
                        sc.ClassID,
                        c.ClassName,
                        sc.Section,
                        sc.AcademicYear,
                        sc.AssignedDate
                    FROM StudentClasses sc
                    INNER JOIN Students s ON sc.StudentID = s.StudentID
                    INNER JOIN Classes c ON sc.ClassID = c.ClassID
                    WHERE sc.ClassID = @ClassID
                      AND sc.Section = @Section
                      AND sc.AcademicYear = @AcademicYear
                    ORDER BY s.FullName";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ClassID", classId);
                    cmd.Parameters.AddWithValue("@Section", section);
                    cmd.Parameters.AddWithValue("@AcademicYear", academicYear);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        public bool AssignStudent(StudentClass assignment)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = @"
                    INSERT INTO StudentClasses
                    (
                        StudentID,
                        ClassID,
                        Section,
                        AcademicYear,
                        AssignedDate,
                        AssignedBy
                    )
                    VALUES
                    (
                        @StudentID,
                        @ClassID,
                        @Section,
                        @AcademicYear,
                        GETDATE(),
                        @AssignedBy
                    );

                    UPDATE Students
                    SET 
                        ClassID = @ClassID,
                        Section = @Section,
                        AcademicYear = @AcademicYear,
                        UpdatedAt = GETDATE()
                    WHERE StudentID = @StudentID;";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    AddParameters(cmd, assignment);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool RemoveAssignment(int studentClassId)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = @"
                    DELETE FROM StudentClasses
                    WHERE StudentClassID = @StudentClassID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@StudentClassID", studentClassId);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool IsStudentAssignedInYear(int studentId, string academicYear)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = @"
                    SELECT COUNT(*)
                    FROM StudentClasses
                    WHERE StudentID = @StudentID
                      AND AcademicYear = @AcademicYear";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@StudentID", studentId);
                    cmd.Parameters.AddWithValue("@AcademicYear", academicYear);

                    conn.Open();
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        private void AddParameters(SqlCommand cmd, StudentClass assignment)
        {
            cmd.Parameters.AddWithValue("@StudentID", assignment.StudentID);
            cmd.Parameters.AddWithValue("@ClassID", assignment.ClassID);
            cmd.Parameters.AddWithValue("@Section", assignment.Section);
            cmd.Parameters.AddWithValue("@AcademicYear", assignment.AcademicYear);

            if (assignment.AssignedBy.HasValue)
                cmd.Parameters.AddWithValue("@AssignedBy", assignment.AssignedBy.Value);
            else
                cmd.Parameters.AddWithValue("@AssignedBy", DBNull.Value);
        }
    }
}
