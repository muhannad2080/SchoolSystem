using System;
using System.Data;
using System.Data.SqlClient;
using SchoolSystem.Models;


namespace SchoolSystem.DataAccess
{

    public class ExpenseRepository
    {
        // تعريف الدالة GetAllExpenses لتنفيذ العملية المرتبطة بها.
        public DataTable GetAllExpenses()
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string query = @"
                    SELECT
                        ExpenseID,
                        ExpenseNumber,
                        Amount,
                        ExpenseDate,
                        Category,
                        PayeeName,
                        PaymentMethod,
                        Description,
                        Notes,
                        CreatedAt,
                        UpdatedAt
                    FROM Expenses
                    ORDER BY ExpenseID DESC";

                using (SqlDataAdapter da = new SqlDataAdapter(query, con))
                {
                    DataTable dt = new DataTable();

                    da.Fill(dt);

                    return dt;
                }
            }
        }

        // تعريف الدالة AddExpense لتنفيذ العملية المرتبطة بها.
        public int AddExpense(Expense expense)
        {
            if (string.IsNullOrWhiteSpace(expense.ExpenseNumber))
                expense.ExpenseNumber = GenerateExpenseNumber();

            using (SqlConnection con = DbConnection.GetConnection())
            {
                string query = @"
                    INSERT INTO Expenses
                    (
                        ExpenseNumber,
                        Amount,
                        ExpenseDate,
                        Category,
                        PayeeName,
                        PaymentMethod,
                        Description,
                        Notes
                    )
                    VALUES
                    (
                        @ExpenseNumber,
                        @Amount,
                        @ExpenseDate,
                        @Category,
                        @PayeeName,
                        @PaymentMethod,
                        @Description,
                        @Notes
                    );

                    SELECT CAST(SCOPE_IDENTITY() AS INT);";


                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    AddParameters(cmd, expense);


                    con.Open();

                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        // تعريف الدالة UpdateExpense لتنفيذ العملية المرتبطة بها.
        public bool UpdateExpense(Expense expense)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string query = @"
                    UPDATE Expenses SET
                        ExpenseNumber = @ExpenseNumber,
                        Amount = @Amount,
                        ExpenseDate = @ExpenseDate,
                        Category = @Category,
                        PayeeName = @PayeeName,
                        PaymentMethod = @PaymentMethod,
                        Description = @Description,
                        Notes = @Notes,
                        UpdatedAt = GETDATE()
                    WHERE ExpenseID = @ExpenseID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {

                    cmd.Parameters.AddWithValue("@ExpenseID", expense.ExpenseID);

                    AddParameters(cmd, expense);


                    con.Open();

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // تعريف الدالة DeleteExpense لتنفيذ العملية المرتبطة بها.
        public bool DeleteExpense(int expenseId)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                con.Open();
                using (SqlTransaction transaction = con.BeginTransaction(IsolationLevel.Serializable))
                {
                    const string validation = @"
                        IF NOT EXISTS
                        (
                            SELECT 1 FROM dbo.Expenses WITH (UPDLOCK, HOLDLOCK)
                            WHERE ExpenseID = @ExpenseID
                        )
                            THROW 51501, N'المصروف المحدد غير موجود.', 1;

                        IF EXISTS
                        (
                            SELECT 1
                            FROM dbo.Vouchers WITH (UPDLOCK, HOLDLOCK)
                            WHERE ReferenceType = N'مصروفات'
                              AND ReferenceID = @ExpenseID
                        )
                            THROW 51502, N'لا يمكن حذف المصروف لأنه مرتبط بسندات مالية. صحح المصروف أو نفّذ التسوية/العكس المالي بدلاً من حذف السجل.', 1;";

                    using (SqlCommand check = new SqlCommand(validation, con, transaction))
                    {
                        check.Parameters.Add("@ExpenseID", SqlDbType.Int).Value = expenseId;
                        check.ExecuteNonQuery();
                    }

                    using (SqlCommand cmd = new SqlCommand(
                        "DELETE FROM dbo.Expenses WHERE ExpenseID = @ExpenseID;", con, transaction))
                    {
                        cmd.Parameters.Add("@ExpenseID", SqlDbType.Int).Value = expenseId;
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

        public Expense GetExpenseById(int expenseId)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT ExpenseID, ExpenseNumber, Amount, ExpenseDate, Category,
                       PayeeName, PaymentMethod, Description, Notes, CreatedAt, UpdatedAt
                FROM dbo.Expenses
                WHERE ExpenseID = @ExpenseID;", con))
            {
                cmd.Parameters.Add("@ExpenseID", SqlDbType.Int).Value = expenseId;
                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                        return null;

                    return new Expense
                    {
                        ExpenseID = Convert.ToInt32(reader["ExpenseID"]),
                        ExpenseNumber = Convert.ToString(reader["ExpenseNumber"]),
                        Amount = Convert.ToDecimal(reader["Amount"]),
                        ExpenseDate = Convert.ToDateTime(reader["ExpenseDate"]),
                        Category = Convert.ToString(reader["Category"]),
                        PayeeName = reader["PayeeName"] == DBNull.Value ? null : Convert.ToString(reader["PayeeName"]),
                        PaymentMethod = reader["PaymentMethod"] == DBNull.Value ? null : Convert.ToString(reader["PaymentMethod"]),
                        Description = Convert.ToString(reader["Description"]),
                        Notes = reader["Notes"] == DBNull.Value ? null : Convert.ToString(reader["Notes"]),
                        CreatedAt = reader["CreatedAt"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["CreatedAt"]),
                        UpdatedAt = reader["UpdatedAt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["UpdatedAt"])
                    };
                }
            }
        }

        // تعريف الدالة GetExpenseAmountById لتنفيذ العملية المرتبطة بها.
        public decimal GetExpenseAmountById(int expenseId)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string query = "SELECT ISNULL(Amount, 0) FROM Expenses WHERE ExpenseID = @ExpenseID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {

                    cmd.Parameters.AddWithValue("@ExpenseID", expenseId);


                    con.Open();


                    object result = cmd.ExecuteScalar();


                    if (result == null || result == DBNull.Value)
                        return 0;


                    return Convert.ToDecimal(result);
                }
            }
        }

        // تعريف الدالة GenerateExpenseNumber لتنفيذ العملية المرتبطة بها.
        public string GenerateExpenseNumber()
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string query = "SELECT ISNULL(MAX(ExpenseID), 0) + 1 FROM Expenses";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {

                    con.Open();


                    int nextId = Convert.ToInt32(cmd.ExecuteScalar());


                    return "EXP-" + DateTime.Today.Year + "-" + nextId.ToString("00000");
                }
            }
        }

        // تعريف الدالة AddParameters لتنفيذ العملية المرتبطة بها.
        private void AddParameters(SqlCommand cmd, Expense expense)
        {

            cmd.Parameters.AddWithValue("@ExpenseNumber", expense.ExpenseNumber ?? "");

            cmd.Parameters.AddWithValue("@Amount", expense.Amount);

            cmd.Parameters.AddWithValue("@ExpenseDate", expense.ExpenseDate.Date);

            cmd.Parameters.AddWithValue("@Category", expense.Category ?? "");


            cmd.Parameters.AddWithValue(

                "@PayeeName",
                string.IsNullOrWhiteSpace(expense.PayeeName)
                    ? (object)DBNull.Value
                    : expense.PayeeName
            );


            cmd.Parameters.AddWithValue(

                "@PaymentMethod",
                string.IsNullOrWhiteSpace(expense.PaymentMethod)
                    ? (object)DBNull.Value
                    : expense.PaymentMethod
            );


            cmd.Parameters.AddWithValue("@Description", expense.Description ?? "");


            cmd.Parameters.AddWithValue(

                "@Notes",
                string.IsNullOrWhiteSpace(expense.Notes)
                    ? (object)DBNull.Value
                    : expense.Notes
            );
        }
    }
}
