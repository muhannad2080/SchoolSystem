using System;
using System.Data;
using System.Data.SqlClient;
using SchoolSystem.Models;

namespace SchoolSystem.DataAccess
{
    public class BookRepository
    {
        public DataTable GetAllBooks()
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
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

        public bool AddBook(Book book)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
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
                        ShelfLocation,
                        Notes
                    )
                    VALUES
                    (
                        @Title,
                        @Author,
                        @ISBN,
                        @Category,
                        @Publisher,
                        @PublicationYear,
                        @Copies,
                        @ShelfLocation,
                        @Notes
                    )";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    AddParameters(cmd, book);

                    con.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool UpdateBook(Book book)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string query = @"
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

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@BookID", book.BookID);
                    AddParameters(cmd, book);

                    con.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool DeleteBook(int bookId)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                const string query = @"
                    IF EXISTS (SELECT 1 FROM BookBorrowings WHERE BookID = @BookID)
                        THROW 51002, N'لا يمكن حذف الكتاب لأنه مرتبط بسجلات إعارة. عطّل الكتاب بدلاً من حذفه.', 1;

                    DELETE FROM Books
                    WHERE BookID = @BookID;";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@BookID", bookId);

                    con.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public int GetAvailableCopies(int bookId)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
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
