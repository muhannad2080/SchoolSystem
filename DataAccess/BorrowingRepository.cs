using System;
using System.Data;
using System.Data.SqlClient;
using SchoolSystem.Models;

namespace SchoolSystem.DataAccess
{
    public class BorrowingRepository
    {
        public DataTable GetAllBorrowings()
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string teacherNameColumn = GetTeacherNameColumn();

                string query = @"
                    SELECT
                        br.BorrowingID,
                        br.BookID,
                        b.Title AS BookTitle,
                        br.BorrowerType,
                        br.BorrowerID,
                        CASE 
                            WHEN br.BorrowerType = N'طالب' THEN s.FullName
                            WHEN br.BorrowerType = N'معلم' THEN " + teacherNameColumn + @"
                            ELSE N''
                        END AS BorrowerName,
                        br.BorrowDate,
                        br.DueDate,
                        br.ReturnDate,
                        CASE
                            WHEN br.Status = N'معار' AND br.DueDate < CAST(GETDATE() AS DATE) THEN N'متأخر'
                            ELSE br.Status
                        END AS Status,
                        br.Notes,
                        br.CreatedAt,
                        br.UpdatedAt
                    FROM BookBorrowings br
                    INNER JOIN Books b ON br.BookID = b.BookID
                    LEFT JOIN Students s ON br.BorrowerType = N'طالب' AND br.BorrowerID = s.StudentID
                    LEFT JOIN Teachers t ON br.BorrowerType = N'معلم' AND br.BorrowerID = t.TeacherID
                    ORDER BY br.BorrowingID DESC";

                using (SqlDataAdapter da = new SqlDataAdapter(query, con))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        public bool AddBorrowing(Borrowing borrowing)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string query = @"
                    INSERT INTO BookBorrowings
                    (
                        BookID,
                        BorrowerType,
                        BorrowerID,
                        BorrowDate,
                        DueDate,
                        ReturnDate,
                        Status,
                        Notes
                    )
                    VALUES
                    (
                        @BookID,
                        @BorrowerType,
                        @BorrowerID,
                        @BorrowDate,
                        @DueDate,
                        NULL,
                        N'معار',
                        @Notes
                    )";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@BookID", borrowing.BookID);
                    cmd.Parameters.AddWithValue("@BorrowerType", borrowing.BorrowerType ?? "");
                    cmd.Parameters.AddWithValue("@BorrowerID", borrowing.BorrowerID);
                    cmd.Parameters.AddWithValue("@BorrowDate", borrowing.BorrowDate.Date);
                    cmd.Parameters.AddWithValue("@DueDate", borrowing.DueDate.Date);

                    cmd.Parameters.AddWithValue(
                        "@Notes",
                        string.IsNullOrWhiteSpace(borrowing.Notes)
                            ? (object)DBNull.Value
                            : borrowing.Notes
                    );

                    con.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool ReturnBook(int borrowingId, DateTime returnDate)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string query = @"
                    UPDATE BookBorrowings SET
                        ReturnDate = @ReturnDate,
                        Status = N'مسترجع',
                        UpdatedAt = GETDATE()
                    WHERE BorrowingID = @BorrowingID
                      AND Status = N'معار'";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@BorrowingID", borrowingId);
                    cmd.Parameters.AddWithValue("@ReturnDate", returnDate.Date);

                    con.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool HasActiveBorrowing(int bookId, string borrowerType, int borrowerId)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string query = @"
                    SELECT COUNT(*)
                    FROM BookBorrowings
                    WHERE BookID = @BookID
                      AND BorrowerType = @BorrowerType
                      AND BorrowerID = @BorrowerID
                      AND Status = N'معار'";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@BookID", bookId);
                    cmd.Parameters.AddWithValue("@BorrowerType", borrowerType);
                    cmd.Parameters.AddWithValue("@BorrowerID", borrowerId);

                    con.Open();

                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        private string GetTeacherNameColumn()
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string query = @"
                    SELECT TOP 1 COLUMN_NAME
                    FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_NAME = 'Teachers'
                      AND COLUMN_NAME IN 
                      (
                          'TeacherName',
                          'FullName',
                          'TeacherFullName',
                          'Name',
                          'EmployeeName',
                          'StaffName'
                      )
                    ORDER BY 
                        CASE COLUMN_NAME
                            WHEN 'TeacherName' THEN 1
                            WHEN 'FullName' THEN 2
                            WHEN 'TeacherFullName' THEN 3
                            WHEN 'Name' THEN 4
                            WHEN 'EmployeeName' THEN 5
                            WHEN 'StaffName' THEN 6
                            ELSE 100
                        END";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();

                    object result = cmd.ExecuteScalar();

                    if (result == null || result == DBNull.Value)
                        return "CAST(t.TeacherID AS NVARCHAR(50))";

                    string columnName = result.ToString();

                    return "t.[" + columnName + "]";
                }
            }
        }
    }
}
