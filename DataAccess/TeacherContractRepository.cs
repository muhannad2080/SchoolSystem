using System;
using System.Data;
using System.Data.SqlClient;
using SchoolSystem.Models;

namespace SchoolSystem.DataAccess
{
    public class TeacherContractRepository
    {
        public DataTable GetAllContracts()
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = @"
                    SELECT
                        c.ContractID,
                        c.TeacherID,
                        t.FullName AS TeacherName,
                        c.ContractNumber,
                        c.ContractType,
                        c.ContractStatus,
                        c.BasicSalary,
                        c.HousingAllowance,
                        c.TransportAllowance,
                        c.OtherAllowances,
                        c.Deductions,
                        c.TotalSalary,
                        c.NetSalary,
                        c.StartDate,
                        c.EndDate,
                        c.PaymentMethod,
                        c.Notes,
                        c.CreatedAt,
                        c.UpdatedAt
                    FROM TeacherContracts c
                    INNER JOIN Teachers t ON c.TeacherID = t.TeacherID
                    ORDER BY c.StartDate DESC, t.FullName ASC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        public bool AddContract(TeacherContract contract)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = @"
                    INSERT INTO TeacherContracts
                    (
                        TeacherID,
                        ContractNumber,
                        ContractType,
                        ContractStatus,
                        BasicSalary,
                        HousingAllowance,
                        TransportAllowance,
                        OtherAllowances,
                        Deductions,
                        TotalSalary,
                        NetSalary,
                        StartDate,
                        EndDate,
                        PaymentMethod,
                        Notes,
                        CreatedAt
                    )
                    VALUES
                    (
                        @TeacherID,
                        @ContractNumber,
                        @ContractType,
                        @ContractStatus,
                        @BasicSalary,
                        @HousingAllowance,
                        @TransportAllowance,
                        @OtherAllowances,
                        @Deductions,
                        @TotalSalary,
                        @NetSalary,
                        @StartDate,
                        @EndDate,
                        @PaymentMethod,
                        @Notes,
                        GETDATE()
                    )";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    AddParameters(cmd, contract, false);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool UpdateContract(TeacherContract contract)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = @"
                    UPDATE TeacherContracts
                    SET
                        TeacherID = @TeacherID,
                        ContractNumber = @ContractNumber,
                        ContractType = @ContractType,
                        ContractStatus = @ContractStatus,
                        BasicSalary = @BasicSalary,
                        HousingAllowance = @HousingAllowance,
                        TransportAllowance = @TransportAllowance,
                        OtherAllowances = @OtherAllowances,
                        Deductions = @Deductions,
                        TotalSalary = @TotalSalary,
                        NetSalary = @NetSalary,
                        StartDate = @StartDate,
                        EndDate = @EndDate,
                        PaymentMethod = @PaymentMethod,
                        Notes = @Notes,
                        UpdatedAt = GETDATE()
                    WHERE ContractID = @ContractID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    AddParameters(cmd, contract, true);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool DeleteContract(int contractId)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = "DELETE FROM TeacherContracts WHERE ContractID = @ContractID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ContractID", contractId);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool HasActiveContract(int teacherId)
        {
            return HasActiveContract(teacherId, 0);
        }

        public bool HasActiveContract(int teacherId, int excludedContractId)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = @"
                    SELECT COUNT(*)
                    FROM TeacherContracts
                    WHERE TeacherID = @TeacherID
                      AND ContractStatus = N'ساري'
                      AND (EndDate IS NULL OR EndDate >= CAST(GETDATE() AS DATE))";

                if (excludedContractId > 0)
                    query += " AND ContractID <> @ContractID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TeacherID", teacherId);

                    if (excludedContractId > 0)
                        cmd.Parameters.AddWithValue("@ContractID", excludedContractId);

                    conn.Open();
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        public bool ContractNumberExists(string contractNumber)
        {
            return ContractNumberExists(contractNumber, 0);
        }

        public bool ContractNumberExists(string contractNumber, int excludedContractId)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = @"
                    SELECT COUNT(*)
                    FROM TeacherContracts
                    WHERE ContractNumber = @ContractNumber";

                if (excludedContractId > 0)
                    query += " AND ContractID <> @ContractID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ContractNumber", contractNumber);

                    if (excludedContractId > 0)
                        cmd.Parameters.AddWithValue("@ContractID", excludedContractId);

                    conn.Open();
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        private void AddParameters(SqlCommand cmd, TeacherContract contract, bool includeId)
        {
            if (includeId)
                cmd.Parameters.AddWithValue("@ContractID", contract.ContractID);

            cmd.Parameters.AddWithValue("@TeacherID", contract.TeacherID);
            cmd.Parameters.AddWithValue("@ContractNumber", contract.ContractNumber);
            cmd.Parameters.AddWithValue("@ContractType", contract.ContractType);
            cmd.Parameters.AddWithValue("@ContractStatus", contract.ContractStatus);
            cmd.Parameters.AddWithValue("@BasicSalary", contract.BasicSalary);
            cmd.Parameters.AddWithValue("@HousingAllowance", contract.HousingAllowance);
            cmd.Parameters.AddWithValue("@TransportAllowance", contract.TransportAllowance);
            cmd.Parameters.AddWithValue("@OtherAllowances", contract.OtherAllowances);
            cmd.Parameters.AddWithValue("@Deductions", contract.Deductions);
            cmd.Parameters.AddWithValue("@TotalSalary", contract.TotalSalary);
            cmd.Parameters.AddWithValue("@NetSalary", contract.NetSalary);
            cmd.Parameters.AddWithValue("@StartDate", contract.StartDate.Date);

            if (contract.EndDate.HasValue)
                cmd.Parameters.AddWithValue("@EndDate", contract.EndDate.Value.Date);
            else
                cmd.Parameters.AddWithValue("@EndDate", DBNull.Value);

            if (string.IsNullOrWhiteSpace(contract.PaymentMethod))
                cmd.Parameters.AddWithValue("@PaymentMethod", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@PaymentMethod", contract.PaymentMethod.Trim());

            if (string.IsNullOrWhiteSpace(contract.Notes))
                cmd.Parameters.AddWithValue("@Notes", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@Notes", contract.Notes.Trim());
        }
    }
}
