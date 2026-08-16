using System.Data;
using System.Data.SqlClient;

namespace SchoolSystem.DataAccess
{
    /// <summary>
    /// مصدر موحد للأطراف التي يمكن أن تظهر في السندات.
    /// لا يعتمد على جدول Staff غير الموجود في المخطط الحالي؛ فالموظفون الماليون
    /// يمثلهم المستخدمون النشطون، والمعلمون يمثلهم جدول Teachers.
    /// </summary>
    public class PartyRepository
    {
        public DataTable GetVoucherParties()
        {
            const string query = @"
                SELECT PartyKey, PartyType, DisplayName, SearchText
                FROM
                (
                    SELECT
                        N'STUDENT:' + CONVERT(NVARCHAR(20), s.StudentID) AS PartyKey,
                        N'طلاب' AS PartyType,
                        s.FullName + CASE WHEN NULLIF(s.StudentNumber, N'') IS NULL THEN N'' ELSE N' (' + s.StudentNumber + N')' END AS DisplayName,
                        s.FullName + N' ' + ISNULL(s.StudentNumber, N'') AS SearchText
                    FROM dbo.Students s
                    WHERE ISNULL(s.Status, N'نشط') = N'نشط'

                    UNION ALL

                    SELECT
                        N'TEACHER:' + CONVERT(NVARCHAR(20), t.TeacherID) AS PartyKey,
                        N'معلمون' AS PartyType,
                        t.FullName + CASE WHEN NULLIF(t.EmployeeNumber, N'') IS NULL THEN N'' ELSE N' (' + t.EmployeeNumber + N')' END AS DisplayName,
                        t.FullName + N' ' + ISNULL(t.EmployeeNumber, N'') AS SearchText
                    FROM dbo.Teachers t
                    WHERE ISNULL(t.Status, N'نشط') = N'نشط'

                    UNION ALL

                    SELECT
                        N'STAFF:' + CONVERT(NVARCHAR(20), u.UserID) AS PartyKey,
                        N'موظفون' AS PartyType,
                        u.FullName + CASE WHEN NULLIF(u.UserName, N'') IS NULL THEN N'' ELSE N' (' + u.UserName + N')' END AS DisplayName,
                        u.FullName + N' ' + ISNULL(u.UserName, N'') + N' ' + ISNULL(u.RoleName, N'') AS SearchText
                    FROM dbo.Users u
                    WHERE u.IsActive = 1
                      AND ISNULL(u.RoleName, N'') <> N'مدير النظام'
                ) parties
                ORDER BY PartyType, DisplayName;";

            using (SqlConnection connection = DbConnection.GetConnection())
            using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
            {
                DataTable table = new DataTable();
                adapter.Fill(table);
                return table;
            }
        }
    }
}
