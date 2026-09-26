using System;
using System.Data;
using System.Data.SqlClient;
using SchoolSystem.Models;


namespace SchoolSystem.DataAccess
{

    public class ContractRepository
    {
        // تعريف الدالة GetAllContracts لتنفيذ العملية المرتبطة بها.
        public DataTable GetAllContracts()
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                                @"SELECT c.ContractID, c.TeacherID, t.FullName AS TeacherName, c.BasicSalary,
                  c.HousingAllowance, c.TransportAllowance, c.OtherAllowances,
                  (c.BasicSalary + c.HousingAllowance + c.TransportAllowance + c.OtherAllowances) AS TotalSalary,
                  c.StartDate, c.EndDate, c.ContractType, c.Notes
                  FROM TeacherContracts c
                  INNER JOIN Teachers t ON c.TeacherID = t.TeacherID
                  ORDER BY c.StartDate DESC", conn))
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

        // تعريف الدالة HasActiveContract لتنفيذ العملية المرتبطة بها.
        public bool HasActiveContract(int teacherId, int excludeId = 0)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                @"SELECT COUNT(1) FROM TeacherContracts
                  WHERE TeacherID = @TID AND (EndDate IS NULL OR EndDate >= GETDATE())
                  AND ContractID <> @CID", conn))
            {

                cmd.Parameters.Add("@TID", SqlDbType.Int).Value = teacherId;

                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = excludeId;

                conn.Open();

                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        // تعريف الدالة AddContract لتنفيذ العملية المرتبطة بها.
        public void AddContract(TeacherContract contract)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                @"INSERT INTO TeacherContracts (TeacherID, BasicSalary, HousingAllowance,
                  TransportAllowance, OtherAllowances, StartDate, EndDate, ContractType, Notes)
                  VALUES (@TID, @Basic, @Housing, @Transport, @Other, @Start, @End, @Type, @Notes)", conn))
            {
                AddContractParameters(cmd, contract);

                conn.Open();

                cmd.ExecuteNonQuery();
            }
        }

        // تعريف الدالة UpdateContract لتنفيذ العملية المرتبطة بها.
        public bool UpdateContract(TeacherContract contract)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                @"UPDATE TeacherContracts SET BasicSalary = @Basic, HousingAllowance = @Housing,
                  TransportAllowance = @Transport, OtherAllowances = @Other,
                  StartDate = @Start, EndDate = @End, ContractType = @Type, Notes = @Notes
                  WHERE ContractID = @CID", conn))
            {

                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = contract.ContractID;

                AddContractParameters(cmd, contract);

                conn.Open();

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // تعريف الدالة DeleteContract لتنفيذ العملية المرتبطة بها.
        public bool DeleteContract(int contractId)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                "DELETE FROM TeacherContracts WHERE ContractID = @CID", conn))
            {

                cmd.Parameters.Add("@CID", SqlDbType.Int).Value = contractId;

                conn.Open();

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // تعريف الدالة AddContractParameters لتنفيذ العملية المرتبطة بها.
        private void AddContractParameters(SqlCommand cmd, TeacherContract c)
        {

            cmd.Parameters.Add("@TID", SqlDbType.Int).Value = c.TeacherID;

            cmd.Parameters.Add("@Basic", SqlDbType.Decimal).Value = c.BasicSalary;

            cmd.Parameters.Add("@Housing", SqlDbType.Decimal).Value = c.HousingAllowance;

            cmd.Parameters.Add("@Transport", SqlDbType.Decimal).Value = c.TransportAllowance;

            cmd.Parameters.Add("@Other", SqlDbType.Decimal).Value = c.OtherAllowances;

            cmd.Parameters.Add("@Start", SqlDbType.Date).Value = c.StartDate;

            cmd.Parameters.Add("@End", SqlDbType.Date).Value = c.EndDate.HasValue ? (object)c.EndDate.Value : DBNull.Value;

            cmd.Parameters.Add("@Type", SqlDbType.NVarChar, 20).Value = c.ContractType;

            cmd.Parameters.Add("@Notes", SqlDbType.NVarChar, 300).Value = string.IsNullOrWhiteSpace(c.Notes) ? (object)DBNull.Value : c.Notes;
        }
    }
}
