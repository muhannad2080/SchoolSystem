using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SchoolSystem.DataAccess;

namespace SchoolSystem.Services
{
    /// <summary>
    /// طبقة الأعمال الخاصة بالإغلاق السنوي. لا تنفذ أي ترحيل تلقائي للطلاب؛
    /// الفحص والإغلاق والتخطيط للترحيل عمليات منفصلة وقابلة للمراجعة.
    /// </summary>
    public sealed class AnnualClosingService
    {
        public List<string> GetAvailableYears()
        {
            const string sql = @"SELECT DISTINCT AcademicYear FROM
                (SELECT AcademicYear FROM dbo.StudentClasses WHERE NULLIF(LTRIM(RTRIM(AcademicYear)), N'') IS NOT NULL
                 UNION SELECT AcademicYear FROM dbo.Enrollments WHERE NULLIF(LTRIM(RTRIM(AcademicYear)), N'') IS NOT NULL
                 UNION SELECT AcademicYear FROM dbo.Fees WHERE NULLIF(LTRIM(RTRIM(AcademicYear)), N'') IS NOT NULL) y
                ORDER BY AcademicYear";
            List<string> years = new List<string>();
            using (SqlConnection connection = DbConnection.GetConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                    while (reader.Read()) years.Add(Normalize(reader[0].ToString()));
            }
            return years;
        }

        public DataTable Verify(string academicYear)
        {
            RequireYear(academicYear);
            using (SqlConnection connection = DbConnection.GetConnection())
            using (SqlCommand command = new SqlCommand("dbo.VerifyAnnualClosing", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@AcademicYear", SqlDbType.NVarChar, 20).Value = Normalize(academicYear);
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    DataSet result = new DataSet();
                    adapter.Fill(result);
                    return result.Tables.Count == 0 ? new DataTable() : result.Tables[0];
                }
            }
        }

        public DataTable PlanMigration(string fromYear, string toYear, int? userId)
        {
            RequireDifferentYears(fromYear, toYear);
            using (SqlConnection connection = DbConnection.GetConnection())
            using (SqlCommand command = new SqlCommand("dbo.PlanStudentYearMigration", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@FromAcademicYear", SqlDbType.NVarChar, 20).Value = Normalize(fromYear);
                command.Parameters.Add("@ToAcademicYear", SqlDbType.NVarChar, 20).Value = Normalize(toYear);
                command.Parameters.Add("@CreatedByUserID", SqlDbType.Int).Value = (object)userId ?? DBNull.Value;
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    DataTable result = new DataTable();
                    adapter.Fill(result);
                    return result;
                }
            }
        }

        public void Close(string academicYear, string nextAcademicYear, int? userId, string notes)
        {
            RequireYear(academicYear);
            if (!string.IsNullOrWhiteSpace(nextAcademicYear) && Normalize(academicYear) == Normalize(nextAcademicYear))
                throw new ArgumentException("لا يمكن أن يكون العام التالي مطابقاً للعام المراد إغلاقه.");
            using (SqlConnection connection = DbConnection.GetConnection())
            using (SqlCommand command = new SqlCommand("dbo.CloseAcademicYear", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@AcademicYear", SqlDbType.NVarChar, 20).Value = Normalize(academicYear);
                command.Parameters.Add("@NextAcademicYear", SqlDbType.NVarChar, 20).Value = string.IsNullOrWhiteSpace(nextAcademicYear) ? (object)DBNull.Value : Normalize(nextAcademicYear);
                command.Parameters.Add("@ClosedByUserID", SqlDbType.Int).Value = (object)userId ?? DBNull.Value;
                command.Parameters.Add("@Notes", SqlDbType.NVarChar, 1000).Value = string.IsNullOrWhiteSpace(notes) ? (object)DBNull.Value : notes.Trim();
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        private static void RequireDifferentYears(string fromYear, string toYear)
        {
            RequireYear(fromYear);
            RequireYear(toYear);
            if (Normalize(fromYear) == Normalize(toYear))
                throw new ArgumentException("يجب اختيار عامين دراسيين مختلفين.");
        }

        private static void RequireYear(string year)
        {
            if (string.IsNullOrWhiteSpace(year) || Normalize(year).Length > 20)
                throw new ArgumentException("العام الدراسي غير صالح.");
        }

        private static string Normalize(string year)
        {
            return (year ?? string.Empty).Trim().Replace('-', '/');
        }
    }
}
