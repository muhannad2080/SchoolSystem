using System;
using System.Data;
using System.Data.SqlClient;
using SchoolSystem.Models;


namespace SchoolSystem.DataAccess
{

    public class BookRepository
    {
        // تعريف الدالة GetAllBooks لتنفيذ العملية المرتبطة بها.
        public DataTable GetAllBooks()
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                LibrarySchemaInitializer.EnsureBooksSchema(con);
                string query = @"
                    SELECT
                        b.BookID,
                        b.Title,
                        b.Author,
                        b.ISBN,
                        b.Category,
                        b.Publisher,
                        b.PublicationYear,
                        b.Copies,
                        b.Copies - ISNULL((
                            SELECT COUNT(*)
                            FROM BookBorrowings br
                            WHERE br.BookID = b.BookID
                              AND br.Status = N'معار'
                        ), 0) AS AvailableCopies,
                        ISNULL(b.IsActive, 1) AS IsActive,
                        b.ShelfLocation,
                        b.Notes,
                        b.CreatedAt,
                        b.UpdatedAt
                    FROM Books b
                    ORDER BY b.BookID DESC";

                using (SqlDataAdapter da = new SqlDataAdapter(query, con))
                {
                    DataTable dt = new DataTable();

                    da.Fill(dt);

                    return dt;
                }
            }
        }

        // تعريف الدالة GetActiveBooks لتنفيذ العملية المرتبطة بها.
        public DataTable GetActiveBooks()
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                LibrarySchemaInitializer.EnsureBooksSchema(con);
                const string query = @"
                    SELECT
                        b.BookID,
                        b.Title,
                        b.Copies - ISNULL((
                            SELECT COUNT(*)
                            FROM BookBorrowings br
                            WHERE br.BookID = b.BookID
                              AND br.Status = N'معار'
                        ), 0) AS AvailableCopies
                    FROM Books b
                    WHERE ISNULL(b.IsActive, 1) = 1
                      AND b.Copies > ISNULL((
                          SELECT COUNT(*)
                          FROM BookBorrowings br
                          WHERE br.BookID = b.BookID
                            AND br.Status = N'معار'
                      ), 0)
                    ORDER BY b.Title;";

                using (SqlDataAdapter da = new SqlDataAdapter(query, con))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        // تعريف الدالة AddBook لتنفيذ العملية المرتبطة بها.
        public bool AddBook(Book book)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                LibrarySchemaInitializer.EnsureBooksSchema(con);
                string query = @"
                    INSERT INTO Books
                    (
                        Title,
                        Author,
                        ISBN,
                        Category,
                        Publisher,
                        PublicationYear,
                        Copies,
                        IsActive,
                        ShelfLocation,
                        Notes
                    )
                    OUTPUT INSERTED.BookID
                    VALUES
                    (
                        @Title,
                        @Author,
                        @ISBN,
                        @Category,
                        @Publisher,
                        @PublicationYear,
                        @Copies,
                        1,
                        @ShelfLocation,
                        @Notes
                    )";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    AddParameters(cmd, book);


                    con.Open();

                    object result = cmd.ExecuteScalar();
                    if (result == null || result == DBNull.Value)
                        return false;

                    book.BookID = Convert.ToInt32(result);
                    return book.BookID > 0;
                }
            }
        }

        // تعريف الدالة UpdateBook لتنفيذ العملية المرتبطة بها.
        // تعريف الدالة UpdateBook لتنفيذ العملية المرتبطة بها.
        public bool UpdateBook(Book book)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                LibrarySchemaInitializer.EnsureBooksSchema(con);
                con.Open();

                using (SqlTransaction transaction = con.BeginTransaction(IsolationLevel.Serializable))
                {
                    const string validationQuery = @"
                        IF NOT EXISTS
                        (
                            SELECT 1
                            FROM dbo.Books WITH (UPDLOCK, HOLDLOCK)
                            WHERE BookID = @BookID
                        )
                            THROW 51301, N'الكتاب المحدد غير موجود.', 1;

                        DECLARE @Borrowed INT =
                        (
                            SELECT COUNT(*)
                            FROM dbo.BookBorrowings WITH (UPDLOCK, HOLDLOCK)
                            WHERE BookID = @BookID
                              AND Status = N'معار'
                        );

                        IF @Copies < @Borrowed
                            THROW 51302, N'عدد النسخ الجديدة لا يمكن أن يكون أقل من عدد النسخ المعارة حالياً.', 1;";

                    using (SqlCommand validation = new SqlCommand(validationQuery, con, transaction))
                    {
                        validation.Parameters.Add("@BookID", SqlDbType.Int).Value = book.BookID;
                        validation.Parameters.Add("@Copies", SqlDbType.Int).Value = book.Copies;
                        validation.ExecuteNonQuery();
                    }

                    const string query = @"
                        UPDATE Books SET
                            Title = @Title,
                            Author = @Author,
                            ISBN = @ISBN,
                            Category = @Category,
                            Publisher = @Publisher,
                            PublicationYear = @PublicationYear,
                            Copies = @Copies,
                            ShelfLocation = @ShelfLocation,
                            Notes = @Notes,
                            UpdatedAt = GETDATE()
                        WHERE BookID = @BookID";

                    using (SqlCommand cmd = new SqlCommand(query, con, transaction))
                    {
                        cmd.Parameters.Add("@BookID", SqlDbType.Int).Value = book.BookID;
                        AddParameters(cmd, book);

                        if (cmd.ExecuteNonQuery() == 0)
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

        // تعريف الدالة DeleteBook لتنفيذ العملية المرتبطة بها.
        public bool DeleteBook(int bookId)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                LibrarySchemaInitializer.EnsureBooksSchema(con);
                con.Open();

                using (SqlTransaction transaction = con.BeginTransaction(IsolationLevel.Serializable))
                {
                    const string validationQuery = @"
                        IF EXISTS
                        (
                            SELECT 1
                            FROM dbo.BookBorrowings WITH (UPDLOCK, HOLDLOCK)
                            WHERE BookID = @BookID
                              AND Status = N'معار'
                        )
                            THROW 51303, N'لا يمكن تعطيل الكتاب لأن هناك نسخاً منه معارة حالياً. أعد الكتب أولاً.', 1;";

                    using (SqlCommand validation = new SqlCommand(validationQuery, con, transaction))
                    {
                        validation.Parameters.Add("@BookID", SqlDbType.Int).Value = bookId;
                        validation.ExecuteNonQuery();
                    }

                    const string query = @"
                        UPDATE Books
                        SET IsActive = 0,
                            UpdatedAt = GETDATE()
                        WHERE BookID = @BookID
                          AND ISNULL(IsActive, 1) = 1;";

                    using (SqlCommand cmd = new SqlCommand(query, con, transaction))
                    {
                        cmd.Parameters.Add("@BookID", SqlDbType.Int).Value = bookId;
                        if (cmd.ExecuteNonQuery() == 0)
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

        public int GetAvailableCopies(int bookId)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                LibrarySchemaInitializer.EnsureBooksSchema(con);
                string query = @"
                    SELECT
                        b.Copies - ISNULL((
                            SELECT COUNT(*)
                            FROM BookBorrowings br
                            WHERE br.BookID = b.BookID
                              AND br.Status = N'معار'
                        ), 0)
                    FROM Books b
                    WHERE b.BookID = @BookID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {

                    cmd.Parameters.AddWithValue("@BookID", bookId);


                    con.Open();


                    object result = cmd.ExecuteScalar();


                    if (result == null || result == DBNull.Value)
                        return 0;


                    return Convert.ToInt32(result);
                }
            }
        }

        // تعريف الدالة AddParameters لتنفيذ العملية المرتبطة بها.
        private void AddParameters(SqlCommand cmd, Book book)
        {

            cmd.Parameters.AddWithValue("@Title", book.Title ?? "");


            cmd.Parameters.AddWithValue(

                "@Author",
                string.IsNullOrWhiteSpace(book.Author) ? (object)DBNull.Value : book.Author
            );


            cmd.Parameters.AddWithValue(

                "@ISBN",
                string.IsNullOrWhiteSpace(book.ISBN) ? (object)DBNull.Value : book.ISBN
            );


            cmd.Parameters.AddWithValue(

                "@Category",
                string.IsNullOrWhiteSpace(book.Category) ? (object)DBNull.Value : book.Category
            );


            cmd.Parameters.AddWithValue(

                "@Publisher",
                string.IsNullOrWhiteSpace(book.Publisher) ? (object)DBNull.Value : book.Publisher
            );

            if (book.PublicationYear > 0)
                cmd.Parameters.AddWithValue("@PublicationYear", book.PublicationYear);

            else

                cmd.Parameters.AddWithValue("@PublicationYear", DBNull.Value);


            cmd.Parameters.AddWithValue("@Copies", book.Copies);


            cmd.Parameters.AddWithValue(

                "@ShelfLocation",
                string.IsNullOrWhiteSpace(book.ShelfLocation) ? (object)DBNull.Value : book.ShelfLocation
            );


            cmd.Parameters.AddWithValue(

                "@Notes",
                string.IsNullOrWhiteSpace(book.Notes) ? (object)DBNull.Value : book.Notes
            );
        }
    }
}
