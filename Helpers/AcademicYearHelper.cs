using System;
using System.Data.SqlClient;
using SchoolSystem.DataAccess;


namespace SchoolSystem.Helpers
{
    /// <summary>
    /// المصدر الوحيد للعام الدراسي النشط في النظام.
    /// الترتيب: الدالة dbo.GetActiveAcademicYear ثم قراءة مباشرة من dbo.AcademicYearCurrent.
    /// لا توجد قيمة افتراضية مخفية؛ عند فشل المصدرين ينتقل الخطأ للمستدعي ليُعرض للمستخدم.
    /// </summary>
    public static class AcademicYearHelper
    {
        /// <summary>استدعاء مخزن للعام النشط يُستخدم من جميع الشاشات والخدمات.</summary>
        public static string GetCurrentAcademicYear()
        {
            try
            {
                using (SqlConnection connection = DbConnection.GetConnection())
                using (SqlCommand command = new SqlCommand(
                    "SELECT dbo.GetActiveAcademicYear()", connection))
                {
                    connection.Open();

                    object result = command.ExecuteScalar();

                    string year = result == null || result == DBNull.Value
                        ? string.Empty
                        : Convert.ToString(result).Trim();

                    if (!string.IsNullOrWhiteSpace(year))
                        return year;
                }
            }
            catch (SqlException ex)
            {
                // قواعد بيانات قديمة قد لا تحتوي الدالة؛ نقرأ جدول المصدر مباشرة قبل الإعلان عن الفشل.
                ApplicationLogger.LogException("استدعاء dbo.GetActiveAcademicYear", ex);
            }

            using (SqlConnection connection = DbConnection.GetConnection())
            using (SqlCommand command = new SqlCommand(
                "SELECT TOP (1) AcademicYear FROM dbo.AcademicYearCurrent WHERE IsCurrent = 1", connection))
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result == null || result == DBNull.Value)
                    throw new InvalidOperationException(
                        "لا يوجد عام دراسي نشط معرّف في قاعدة البيانات. عيّن العام النشط من شاشة الإغلاق السنوي.");

                string year = Convert.ToString(result).Trim();

                if (string.IsNullOrWhiteSpace(year))
                    throw new InvalidOperationException(
                        "العام الدراسي النشط في قاعدة البيانات غير صالح. عيّن العام النشط من شاشة الإغلاق السنوي.");

                return year;
            }
        }

        public static bool TryParseAcademicYear(string academicYear, out int firstYear, out int secondYear)
        {
            firstYear = 0;
            secondYear = 0;

            if (string.IsNullOrWhiteSpace(academicYear))
                return false;

            string normalized = academicYear.Trim().Replace('-', '/').Replace('–', '/').Replace('—', '/');
            string[] parts = normalized.Split('/');
            if (parts.Length != 2) return false;

            if (int.TryParse(parts[0].Trim(), out firstYear) && int.TryParse(parts[1].Trim(), out secondYear))
            {
                return secondYear == firstYear + 1;
            }

            return false;
        }
    }
}
