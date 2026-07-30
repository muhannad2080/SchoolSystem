using System;
using System.Data;
using System.Data.SqlClient;
using SchoolSystem.Models;

namespace SchoolSystem.DataAccess
{
    public class MarkRepository
    {
        public DataTable GetAllMarks()
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                @"SELECT m.MarkID, m.StudentID, s.StudentName,
                  m.SubjectID, sub.SubjectName, m.TeacherID, t.TeacherName,
                  m.Mark, m.ExamType, m.CreatedAt
                  FROM Marks m
                  INNER JOIN Students s ON m.StudentID = s.StudentID
                  INNER JOIN Subjects sub ON m.SubjectID = sub.SubjectID
                  LEFT JOIN Teachers t ON m.TeacherID = t.TeacherID
                  ORDER BY m.CreatedAt DESC", conn))
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return dt;
                }
            }
        }

        public bool MarkExists(int studentId, int subjectId, string examType, int excludeId = 0)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                @"SELECT COUNT(1) FROM Marks
                  WHERE StudentID = @SID AND SubjectID = @SubID AND ExamType = @Exam
                  AND MarkID <> @MID", conn))
            {
                cmd.Parameters.Add("@SID", SqlDbType.Int).Value = studentId;
                cmd.Parameters.Add("@SubID", SqlDbType.Int).Value = subjectId;
                cmd.Parameters.Add("@Exam", SqlDbType.NVarChar, 50).Value = examType ?? (object)DBNull.Value;
                cmd.Parameters.Add("@MID", SqlDbType.Int).Value = excludeId;
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        public void AddMark(Mark mark)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                @"INSERT INTO Marks (StudentID, SubjectID, TeacherID, Mark, ExamType)
                  VALUES (@SID, @SubID, @TID, @Mark, @Exam)", conn))
            {
                cmd.Parameters.Add("@SID", SqlDbType.Int).Value = mark.StudentID;
                cmd.Parameters.Add("@SubID", SqlDbType.Int).Value = mark.SubjectID;
                cmd.Parameters.Add("@TID", SqlDbType.Int).Value = mark.TeacherID.HasValue ? (object)mark.TeacherID.Value : DBNull.Value;
                cmd.Parameters.Add("@Mark", SqlDbType.Decimal).Value = mark.MarkValue;
                cmd.Parameters.Add("@Exam", SqlDbType.NVarChar, 50).Value = string.IsNullOrWhiteSpace(mark.ExamType) ? (object)DBNull.Value : mark.ExamType;
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public bool UpdateMark(Mark mark)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                @"UPDATE Marks SET StudentID=@SID, SubjectID=@SubID, TeacherID=@TID, Mark=@Mark, ExamType=@Exam
                  WHERE MarkID=@MID", conn))
            {
                cmd.Parameters.Add("@MID", SqlDbType.Int).Value = mark.MarkID;
                cmd.Parameters.Add("@SID", SqlDbType.Int).Value = mark.StudentID;
                cmd.Parameters.Add("@SubID", SqlDbType.Int).Value = mark.SubjectID;
                cmd.Parameters.Add("@TID", SqlDbType.Int).Value = mark.TeacherID.HasValue ? (object)mark.TeacherID.Value : DBNull.Value;
                cmd.Parameters.Add("@Mark", SqlDbType.Decimal).Value = mark.MarkValue;
                cmd.Parameters.Add("@Exam", SqlDbType.NVarChar, 50).Value = string.IsNullOrWhiteSpace(mark.ExamType) ? (object)DBNull.Value : mark.ExamType;
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool DeleteMark(int markId)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("DELETE FROM Marks WHERE MarkID=@MID", conn))
            {
                cmd.Parameters.Add("@MID", SqlDbType.Int).Value = markId;
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}