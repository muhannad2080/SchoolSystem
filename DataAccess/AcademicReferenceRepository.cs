using System;
using System.Data;
using System.Data.SqlClient;


namespace SchoolSystem.DataAccess
{

    public sealed class AcademicReferenceRepository
    {
        // تعريف الدالة GetAcademicYears لتنفيذ العملية المرتبطة بها.
        public DataTable GetAcademicYears()
        {
            const string sql = @"
                SELECT AcademicYearID, AcademicYear, StartDate, EndDate, IsActive
                FROM dbo.AcademicYears
                WHERE ISNULL(IsActive, 1) = 1
                ORDER BY StartDate DESC, AcademicYearID DESC;";

            using (SqlConnection connection = DbConnection.GetConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                DataTable table = new DataTable();

                adapter.Fill(table);

                return table;
            }
        }

        // تعريف الدالة GetActiveTerms لتنفيذ العملية المرتبطة بها.
        public DataTable GetActiveTerms()
        {
            const string sql = @"
                SELECT TermID, TermName
                FROM dbo.AcademicTerms
                WHERE ISNULL(IsActive, 1) = 1
                ORDER BY TermOrder, TermID;";

            using (SqlConnection connection = DbConnection.GetConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                DataTable table = new DataTable();

                adapter.Fill(table);

                return table;
            }
        }
    }
}
