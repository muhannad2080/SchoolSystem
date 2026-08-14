using System;
using System.Data;
using System.Data.SqlClient;
using SchoolSystem.Models;

namespace SchoolSystem.DataAccess
{
    public class PayrollRepository
    {
        // جلب جميع سجلات الرواتب
        public DataTable GetAllPayrolls()
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                                @"SELECT p.PayrollID, p.TeacherID, t.FullName AS TeacherName, p.SalaryMonth, p.SalaryYear,
                  p.BasicSalary, p.Allowances, p.Deductions, p.NetSalary, p.PaymentDate, p.Notes
                  FROM Payroll p
                  INNER JOIN Teachers t ON p.TeacherID = t.TeacherID
                  ORDER BY p.SalaryYear DESC, p.SalaryMonth DESC", conn))
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

        // التحقق من وجود راتب لمعلم في شهر محدد
        public bool PayrollExists(int teacherId, int month, int year, int excludeId = 0)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                @"SELECT COUNT(1) FROM Payroll 
                  WHERE TeacherID = @TID AND SalaryMonth = @Month AND SalaryYear = @Year 
                  AND PayrollID <> @PID", conn))
            {
                cmd.Parameters.Add("@TID", SqlDbType.Int).Value = teacherId;
                cmd.Parameters.Add("@Month", SqlDbType.Int).Value = month;
                cmd.Parameters.Add("@Year", SqlDbType.Int).Value = year;
                cmd.Parameters.Add("@PID", SqlDbType.Int).Value = excludeId;
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        // إضافة سجل راتب
        public void AddPayroll(Payroll payroll)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                @"INSERT INTO Payroll (TeacherID, SalaryMonth, SalaryYear, BasicSalary, Allowances, Deductions, PaymentDate, Notes)
                  VALUES (@TID, @Month, @Year, @Basic, @Allow, @Deduct, @PayDate, @Notes)", conn))
            {
                AddPayrollParameters(cmd, payroll);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // تحديث سجل راتب
        public bool UpdatePayroll(Payroll payroll)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                @"UPDATE Payroll SET BasicSalary = @Basic, Allowances = @Allow, 
                  Deductions = @Deduct, PaymentDate = @PayDate, Notes = @Notes
                  WHERE PayrollID = @PID", conn))
            {
                cmd.Parameters.Add("@PID", SqlDbType.Int).Value = payroll.PayrollID;
                AddPayrollParameters(cmd, payroll);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // حذف سجل راتب
        public bool DeletePayroll(int payrollId)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                @"IF EXISTS
                  (
                      SELECT 1
                      FROM Payroll
                      WHERE PayrollID = @PID
                        AND PaymentDate IS NOT NULL
                  )
                  BEGIN
                      THROW 51005, N'لا يمكن حذف راتب تم صرفه. استخدم التصحيح أو التسوية للحفاظ على السجل المالي.', 1;
                  END;

                  DELETE FROM Payroll
                  WHERE PayrollID = @PID
                    AND PaymentDate IS NULL;", conn))
            {
                cmd.Parameters.Add("@PID", SqlDbType.Int).Value = payrollId;
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        private void AddPayrollParameters(SqlCommand cmd, Payroll p)
        {
            cmd.Parameters.Add("@TID", SqlDbType.Int).Value = p.TeacherID;
            cmd.Parameters.Add("@Month", SqlDbType.Int).Value = p.SalaryMonth;
            cmd.Parameters.Add("@Year", SqlDbType.Int).Value = p.SalaryYear;
            cmd.Parameters.Add("@Basic", SqlDbType.Decimal).Value = p.BasicSalary;
            cmd.Parameters.Add("@Allow", SqlDbType.Decimal).Value = p.Allowances;
            cmd.Parameters.Add("@Deduct", SqlDbType.Decimal).Value = p.Deductions;
            cmd.Parameters.Add("@PayDate", SqlDbType.Date).Value = p.PaymentDate.HasValue ? (object)p.PaymentDate.Value : DBNull.Value;
            cmd.Parameters.Add("@Notes", SqlDbType.NVarChar, 200).Value = string.IsNullOrWhiteSpace(p.Notes) ? (object)DBNull.Value : p.Notes;
        }
    }
}