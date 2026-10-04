using System;
using System.Data;
using System.Data.SqlClient;
using SchoolSystem.Models;


namespace SchoolSystem.DataAccess
{

    public class TeacherContractRepository
    {
        // تعريف الدالة GetAllContracts لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة AddContract لتنفيذ العملية المرتبطة بها.
        public bool AddContract(TeacherContract contract)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                // TotalSalary وNetSalary أعمدة محسوبة في قاعدة البيانات.
                // لا تُذكر في INSERT نهائياً؛ SQL Server يحسبهما من الحقول الأساسية والبدلات والخصومات.
                string query = @"
                    INSERT INTO dbo.TeacherContracts
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
                        StartDate,
                        EndDate,
                        PaymentMethod,
                        Notes,
                        CreatedAt
                    )
                    OUTPUT INSERTED.ContractID
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

                    object result = cmd.ExecuteScalar();
                    if (result == null || result == DBNull.Value)
                        return false;

                    // حفظ المعرّف المولد في النموذج حتى يمكن تسجيل العملية بدقة.
                    contract.ContractID = Convert.ToInt32(result);
                    return contract.ContractID > 0;
                }
            }
        }

        // تعريف الدالة UpdateContract لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة DeleteContract لتنفيذ العملية المرتبطة بها.
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

        // يعيد العقد الساري الفعلي للموظف في تاريخ اليوم، لاستخدامه كمصدر افتراضي للراتب الشهري.
        public TeacherContract GetActiveContract(int teacherId)
        {
            if (teacherId <= 0)
                return null;

            using (SqlConnection conn = DbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(@"
SELECT TOP (1)
    ContractID,
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
    CreatedAt,
    UpdatedAt
FROM dbo.TeacherContracts
WHERE TeacherID = @TeacherID
  AND ContractStatus = N'ساري'
  AND StartDate <= CAST(GETDATE() AS DATE)
  AND (EndDate IS NULL OR EndDate >= CAST(GETDATE() AS DATE))
ORDER BY StartDate DESC, ContractID DESC;", conn))
            {
                cmd.Parameters.Add("@TeacherID", SqlDbType.Int).Value = teacherId;
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                        return null;

                    return new TeacherContract
                    {
                        ContractID = Convert.ToInt32(reader["ContractID"]),
                        TeacherID = Convert.ToInt32(reader["TeacherID"]),
                        ContractNumber = Convert.ToString(reader["ContractNumber"]),
                        ContractType = Convert.ToString(reader["ContractType"]),
                        ContractStatus = Convert.ToString(reader["ContractStatus"]),
                        BasicSalary = reader["BasicSalary"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["BasicSalary"]),
                        HousingAllowance = reader["HousingAllowance"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["HousingAllowance"]),
                        TransportAllowance = reader["TransportAllowance"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["TransportAllowance"]),
                        OtherAllowances = reader["OtherAllowances"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["OtherAllowances"]),
                        Deductions = reader["Deductions"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["Deductions"]),
                        TotalSalary = reader["TotalSalary"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["TotalSalary"]),
                        NetSalary = reader["NetSalary"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["NetSalary"]),
                        StartDate = Convert.ToDateTime(reader["StartDate"]),
                        EndDate = reader["EndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["EndDate"]),
                        PaymentMethod = reader["PaymentMethod"] == DBNull.Value ? string.Empty : Convert.ToString(reader["PaymentMethod"]),
                        Notes = reader["Notes"] == DBNull.Value ? string.Empty : Convert.ToString(reader["Notes"]),
                        CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                        UpdatedAt = reader["UpdatedAt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["UpdatedAt"])
                    };
                }
            }
        }

        // تعريف الدالة HasActiveContract لتنفيذ العملية المرتبطة بها.
        public bool HasActiveContract(int teacherId)
        {

            return HasActiveContract(teacherId, 0);
        }

        // تعريف الدالة HasActiveContract لتنفيذ العملية المرتبطة بها.
        public bool HasActiveContract(int teacherId, int excludedContractId)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = @"
                    SELECT COUNT(*)
                    FROM TeacherContracts
                    WHERE TeacherID = @TeacherID
                      AND ContractStatus = N'ساري'
                      AND StartDate <= CAST(GETDATE() AS DATE)
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

        // تعريف الدالة ContractNumberExists لتنفيذ العملية المرتبطة بها.
        public bool ContractNumberExists(string contractNumber)
        {

            return ContractNumberExists(contractNumber, 0);
        }

        // تعريف الدالة ContractNumberExists لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة AddParameters لتنفيذ العملية المرتبطة بها.
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

            // TotalSalary وNetSalary أعمدة محسوبة في dbo.TeacherContracts؛
            // لا تُرسل ضمن INSERT/UPDATE إطلاقاً حتى لا ينتج SQL error 271.

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
