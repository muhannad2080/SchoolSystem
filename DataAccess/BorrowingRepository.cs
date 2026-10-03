using System;
using System.Data;
using System.Data.SqlClient;
using SchoolSystem.Models;


namespace SchoolSystem.DataAccess
{

    public class BorrowingRepository
    {
        // تعريف الدالة GetAllBorrowings لتنفيذ العملية المرتبطة بها.
        public DataTable GetAllBorrowings()
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                LibrarySchemaInitializer.EnsureBooksSchema(con);
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

        // تعريف الدالة AddBorrowing لتنفيذ العملية المرتبطة بها.
        public bool AddBorrowing(Borrowing borrowing)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                LibrarySchemaInitializer.EnsureBooksSchema(con);

                const string query = @"
                    IF NOT EXISTS
                    (
                        SELECT 1
                        FROM Books WITH (UPDLOCK, HOLDLOCK)
                        WHERE BookID = @BookID
                          AND ISNULL(IsActive, 1) = 1
                          AND Copies > ISNULL(
                              (
                                  SELECT COUNT(*)
                                  FROM BookBorrowings WITH (UPDLOCK, HOLDLOCK)
                                  WHERE BookID = @BookID
                                    AND Status = N'معار'
                              ), 0)
                    )
                        THROW 50003, N'لا يمكن إنشاء الإعارة: الكتاب غير نشط أو لا توجد نسخ متاحة.', 1;

                    IF (@BorrowerType = N'طالب' AND NOT EXISTS
                    (
                        SELECT 1
                        FROM Students WITH (UPDLOCK, HOLDLOCK)
                        WHERE StudentID = @BorrowerID
                          AND ISNULL(Status, N'نشط') = N'نشط'
                    ))
                        THROW 50004, N'لا يمكن إنشاء إعارة لطالب غير نشط.', 1;

                    IF (@BorrowerType = N'معلم' AND NOT EXISTS
                    (
                        SELECT 1
                        FROM Teachers WITH (UPDLOCK, HOLDLOCK)
                        WHERE TeacherID = @BorrowerID
                          AND ISNULL(Status, N'نشط') = N'نشط'
                    ))
                        THROW 50005, N'لا يمكن إنشاء إعارة لمعلم غير نشط.', 1;

                    IF EXISTS
                    (
                        SELECT 1
                        FROM BookBorrowings WITH (UPDLOCK, HOLDLOCK)
                        WHERE BookID = @BookID
                          AND BorrowerType = @BorrowerType
                          AND BorrowerID = @BorrowerID
                          AND Status = N'معار'
                    )
                        THROW 50006, N'هذا المستعير لديه نفس الكتاب معاراً حالياً.', 1;

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
                    OUTPUT INSERTED.BorrowingID
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
                    );";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@BookID", SqlDbType.Int).Value = borrowing.BookID;
                    cmd.Parameters.Add("@BorrowerType", SqlDbType.NVarChar, 20).Value =
                        (object)(borrowing.BorrowerType ?? string.Empty);
                    cmd.Parameters.Add("@BorrowerID", SqlDbType.Int).Value = borrowing.BorrowerID;
                    cmd.Parameters.Add("@BorrowDate", SqlDbType.Date).Value = borrowing.BorrowDate.Date;
                    cmd.Parameters.Add("@DueDate", SqlDbType.Date).Value = borrowing.DueDate.Date;
                    cmd.Parameters.Add("@Notes", SqlDbType.NVarChar, 1000).Value =
                        string.IsNullOrWhiteSpace(borrowing.Notes)
                            ? (object)DBNull.Value
                            : borrowing.Notes.Trim();

                    con.Open();
                    using (SqlTransaction transaction = con.BeginTransaction(IsolationLevel.Serializable))
                    {
                        cmd.Transaction = transaction;

                        try
                        {
                            object result = cmd.ExecuteScalar();
                            if (result == null || result == DBNull.Value)
                            {
                                transaction.Rollback();
                                return false;
                            }

                            borrowing.BorrowingID = Convert.ToInt32(result);
                            transaction.Commit();
                            return borrowing.BorrowingID > 0;
                        }
                        catch
                        {
                            try { transaction.Rollback(); } catch { }
                            throw;
                        }
                    }
                }
            }
        }

        // تعريف الدالة ReturnBook لتنفيذ العملية المرتبطة بها.
        public bool ReturnBook(int borrowingId, DateTime returnDate)
        {
            if (borrowingId <= 0)
                throw new ArgumentException("رقم الإعارة غير صحيح.");

            using (SqlConnection con = DbConnection.GetConnection())
            {
                con.Open();
                using (SqlTransaction transaction = con.BeginTransaction(IsolationLevel.Serializable))
                {
                    const string validationQuery = @"
                        DECLARE @BorrowDate DATE;

                        SELECT @BorrowDate = BorrowDate
                        FROM dbo.BookBorrowings WITH (UPDLOCK, HOLDLOCK)
                        WHERE BorrowingID = @BorrowingID
                          AND Status = N'معار';

                        IF @BorrowDate IS NULL
                            THROW 50007, N'الإعارة غير موجودة أو تم إرجاع الكتاب مسبقاً.', 1;

                        IF @ReturnDate < @BorrowDate
                            THROW 50008, N'تاريخ الاسترجاع لا يمكن أن يسبق تاريخ الإعارة.', 1;

                        IF @ReturnDate > CAST(GETDATE() AS DATE)
                            THROW 50009, N'تاريخ الاسترجاع لا يمكن أن يكون في المستقبل.', 1;";

                    using (SqlCommand validation = new SqlCommand(validationQuery, con, transaction))
                    {
                        validation.Parameters.Add("@BorrowingID", SqlDbType.Int).Value = borrowingId;
                        validation.Parameters.Add("@ReturnDate", SqlDbType.Date).Value = returnDate.Date;
                        validation.ExecuteNonQuery();
                    }

                    const string updateQuery = @"
                        UPDATE dbo.BookBorrowings
                        SET ReturnDate = @ReturnDate,
                            Status = N'مسترجع',
                            UpdatedAt = GETDATE()
                        WHERE BorrowingID = @BorrowingID
                          AND Status = N'معار';";

                    using (SqlCommand update = new SqlCommand(updateQuery, con, transaction))
                    {
                        update.Parameters.Add("@BorrowingID", SqlDbType.Int).Value = borrowingId;
                        update.Parameters.Add("@ReturnDate", SqlDbType.Date).Value = returnDate.Date;

                        if (update.ExecuteNonQuery() == 0)
                        {
                            transaction.Rollback();
                            return false;
                        }
                    }

                    transaction.Commit();
                    return true;
                }
            }
        }

        // تعريف الدالة HasActiveBorrowing لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة GetTeacherNameColumn لتنفيذ العملية المرتبطة بها.
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

                // إنهاء كتلة أو معاملة قاعدة البيانات الحالية.
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
