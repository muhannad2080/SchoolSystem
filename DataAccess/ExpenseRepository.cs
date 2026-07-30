using System;
using System.Data;
using System.Data.SqlClient;
using SchoolSystem.Models;

namespace SchoolSystem.DataAccess
{
    public class ExpenseRepository
    {
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

        public bool DeleteExpense(int expenseId)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string query = "DELETE FROM Expenses WHERE ExpenseID = @ExpenseID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ExpenseID", expenseId);

                    con.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

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
