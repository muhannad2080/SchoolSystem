using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SchoolSystem.DataAccess;
using SchoolSystem.Models;
using SchoolSystem.Helpers;
using SchoolSystem.Security;


namespace SchoolSystem.Services
{
    /// <summary>
    /// طبقة الأعمال الخاصة بالإغلاق السنوي. لا تنفذ أي ترحيل تلقائي للطلاب؛
    /// الفحص والإغلاق والتخطيط للترحيل عمليات منفصلة وقابلة للمراجعة.
    /// </summary>

    public sealed class AnnualClosingService
    {
        // تعريف الدالة GetAvailableYears لتنفيذ العملية المرتبطة بها.
        public List<string> GetAvailableYears()
        {


            const string sql = @"SELECT DISTINCT AcademicYear
                FROM
                (
                    SELECT AcademicYear FROM dbo.AcademicYears
                    WHERE NULLIF(LTRIM(RTRIM(AcademicYear)), N'') IS NOT NULL
                    UNION ALL
                    SELECT AcademicYear FROM dbo.StudentClasses
                    WHERE NULLIF(LTRIM(RTRIM(AcademicYear)), N'') IS NOT NULL
                    UNION ALL
                    SELECT AcademicYear FROM dbo.Enrollments
                    WHERE NULLIF(LTRIM(RTRIM(AcademicYear)), N'') IS NOT NULL
                    UNION ALL
                    SELECT AcademicYear FROM dbo.Fees
                    WHERE NULLIF(LTRIM(RTRIM(AcademicYear)), N'') IS NOT NULL
                ) y
                ORDER BY AcademicYear";
            List<string> years = new List<string>();
            using (SqlConnection connection = DbConnection.GetConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        string normalized = Normalize(reader[0].ToString());
                        if (!string.IsNullOrWhiteSpace(normalized) && !years.Contains(normalized))
                            years.Add(normalized);
                    }
                }
            }

            return years;
        }

        // تعريف الدالة Verify لتنفيذ العملية المرتبطة بها.
        public DataTable Verify(string academicYear)
        {
            CurrentUser.DemandAction("AnnualClosing", "View", "ليس لديك صلاحية فحص الإغلاق السنوي.");
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

        // تعريف الدالة PlanMigration لتنفيذ العملية المرتبطة بها.
        public DataTable PlanMigration(string fromYear, string toYear, int? userId)
        {
            CurrentUser.DemandAny("ليس لديك صلاحية تخطيط الترحيل.",
                PermissionKeys.AnnualClosingManage);
            RequireDifferentYears(fromYear, toYear);
            using (SqlConnection connection = DbConnection.GetConnection())
            using (SqlCommand command = new SqlCommand("dbo.PlanStudentYearMigration", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@FromAcademicYear", SqlDbType.NVarChar, 20).Value = Normalize(fromYear);

                command.Parameters.Add("@ToAcademicYear", SqlDbType.NVarChar, 20).Value = Normalize(toYear);

                command.Parameters.Add("@CreatedByUserID", SqlDbType.Int).Value =
                    (object)(CurrentUser.User?.UserID ?? userId) ?? DBNull.Value;

                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    DataTable result = new DataTable();

                    adapter.Fill(result);

                    return result;
                }
            }
        }

        // تعريف الدالة GetActiveAcademicYear لتنفيذ العملية المرتبطة بها.
        public string GetActiveAcademicYear()
        {
            // مصدر واحد للعام النشط: AcademicYearHelper (الدالة ثم AcademicYearCurrent).
            string year = Helpers.AcademicYearHelper.GetCurrentAcademicYear();

            return Normalize(year);
        }

        // تعريف الدالة SetActiveAcademicYear لتنفيذ العملية المرتبطة بها.
        public void SetActiveAcademicYear(string academicYear, int? userId)
        {
            CurrentUser.DemandAny("ليس لديك صلاحية تغيير العام الدراسي النشط.",
                PermissionKeys.AnnualClosingManage);
            RequireYear(academicYear);
            using (SqlConnection connection = DbConnection.GetConnection())
            using (SqlCommand command = new SqlCommand("dbo.SetActiveAcademicYear", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@AcademicYear", SqlDbType.NVarChar, 20).Value = Normalize(academicYear);

                command.Parameters.Add("@UserID", SqlDbType.Int).Value =
                    (object)(CurrentUser.User?.UserID ?? userId) ?? DBNull.Value;

                connection.Open();

                command.ExecuteNonQuery();
            }

            new AuditLogService().Record("AnnualClosing", "تعيين العام النشط", "SystemAcademicSettings", null, "العام: " + Normalize(academicYear));
        }

        // تعريف الدالة CreatePreClosingBackup لتنفيذ العملية المرتبطة بها.
        public string CreatePreClosingBackup(string academicYear, int? userId)
        {
            CurrentUser.DemandAny("ليس لديك صلاحية إنشاء نسخة احتياطية قبل الإغلاق.",
                PermissionKeys.AnnualClosingManage);
            RequireYear(academicYear);

            ApplicationSettingsData settings = ApplicationSettingsService.Load();
            string file = new DatabaseBackupService().Backup(settings.ServerInstance, settings.DatabaseName, settings.BackupDirectory);
            using (SqlConnection connection = DbConnection.GetConnection())
            using (SqlCommand command = new SqlCommand("dbo.RegisterDatabaseBackup", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@BackupFile", SqlDbType.NVarChar, 500).Value = file;

                command.Parameters.Add("@BackupType", SqlDbType.NVarChar, 50).Value = "قبل الإغلاق";

                command.Parameters.Add("@AcademicYear", SqlDbType.NVarChar, 20).Value = Normalize(academicYear);

                command.Parameters.Add("@UserID", SqlDbType.Int).Value = (object)userId ?? DBNull.Value;

                connection.Open();

                command.ExecuteNonQuery();
            }

            new AuditLogService().Record("AnnualClosing", "إنشاء نسخة احتياطية قبل الإغلاق", "DatabaseBackup", file, "العام: " + Normalize(academicYear));

            return file;
        }

        // تعريف الدالة GetMigrationReport لتنفيذ العملية المرتبطة بها.
        public DataTable GetMigrationReport(string fromYear, string toYear)
        {
            CurrentUser.DemandAction("AnnualClosing", "View", "ليس لديك صلاحية عرض تقرير الترحيل.");
            RequireDifferentYears(fromYear, toYear);
            using (SqlConnection connection = DbConnection.GetConnection())
            using (SqlCommand command = new SqlCommand("dbo.GetStudentMigrationReport", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@FromAcademicYear", SqlDbType.NVarChar, 20).Value = Normalize(fromYear);

                command.Parameters.Add("@ToAcademicYear", SqlDbType.NVarChar, 20).Value = Normalize(toYear);

                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    DataTable result = new DataTable();

                    adapter.Fill(result);

                    return result;
                }
            }
        }

        // تعريف الدالة ApproveMigration لتنفيذ العملية المرتبطة بها.
        public void ApproveMigration(int migrationId, int toClassId, string toSection, int? userId)
        {
            CurrentUser.DemandAny("ليس لديك صلاحية اعتماد الترحيل.",
                PermissionKeys.AnnualClosingApprove,
                PermissionKeys.AnnualClosingManage);

            if (migrationId <= 0 || toClassId <= 0 || string.IsNullOrWhiteSpace(toSection))

                throw new ArgumentException("يجب تحديد سجل الترحيل والصف والشعبة الجديدة.");

            using (SqlConnection connection = DbConnection.GetConnection())
            using (SqlCommand command = new SqlCommand("dbo.ApproveStudentYearMigration", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@MigrationID", SqlDbType.Int).Value = migrationId;

                command.Parameters.Add("@ToClassID", SqlDbType.Int).Value = toClassId;

                command.Parameters.Add("@ToSection", SqlDbType.NVarChar, 50).Value = toSection.Trim();

                command.Parameters.Add("@ApprovedByUserID", SqlDbType.Int).Value =
                    (object)(CurrentUser.User?.UserID ?? userId) ?? DBNull.Value;

                connection.Open();

                command.ExecuteNonQuery();
            }

            new AuditLogService().Record("AnnualClosing", "اعتماد وتنفيذ ترحيل طالب", "AnnualMigrationLog", migrationId.ToString(), "الصف: " + toClassId + "، الشعبة: " + toSection.Trim());
        }

        // تعريف الدالة CloseWithRequiredBackup لتنفيذ العملية المرتبطة بها.
        public void CloseWithRequiredBackup(string academicYear, string nextAcademicYear, int? userId, string notes)
        {
            CurrentUser.DemandAction("AnnualClosing", "Manage", "ليس لديك صلاحية تنفيذ الإغلاق السنوي.");
            string file = CreatePreClosingBackup(academicYear, userId);
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {

                Close(academicYear, nextAcademicYear, userId, notes);

                new AuditLogService().Record("AnnualClosing", "إغلاق عام دراسي", "AnnualClosing", Normalize(academicYear), "النسخة السابقة: " + file);
            }

            catch (Exception ex)
            {

                ApplicationLogger.LogException("خدمة " + nameof(AnnualClosingService) + ": الإغلاق السنوي مع النسخة الاحتياطية", ex);

                new AuditLogService().Record("AnnualClosing", "فشل إغلاق عام دراسي", "AnnualClosing", Normalize(academicYear), ex.Message);

                throw;
            }
        }

        // تعريف الدالة Close لتنفيذ العملية المرتبطة بها.
        public void Close(string academicYear, string nextAcademicYear, int? userId, string notes)
        {
            CurrentUser.DemandAction("AnnualClosing", "Manage", "ليس لديك صلاحية تنفيذ الإغلاق السنوي.");
            RequireYear(academicYear);
            if (!string.IsNullOrWhiteSpace(nextAcademicYear) && Normalize(academicYear) == Normalize(nextAcademicYear))

                throw new ArgumentException("لا يمكن أن يكون العام التالي مطابقاً للعام المراد إغلاقه.");

            string normalizedNextYear = Normalize(nextAcademicYear);

            if (string.IsNullOrWhiteSpace(normalizedNextYear))
            {
                throw new ArgumentException("العام الدراسي التالي مطلوب لإتمام الإغلاق السنوي.");
            }

            using (SqlConnection connection = DbConnection.GetConnection())
            using (SqlCommand command = new SqlCommand("dbo.CloseAndActivateAcademicYear", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@AcademicYear", SqlDbType.NVarChar, 20).Value = Normalize(academicYear);

                command.Parameters.Add("@NextAcademicYear", SqlDbType.NVarChar, 20).Value = normalizedNextYear;

                command.Parameters.Add("@ClosedByUserID", SqlDbType.Int).Value =
                    (object)(CurrentUser.User?.UserID ?? userId) ?? DBNull.Value;

                command.Parameters.Add("@Notes", SqlDbType.NVarChar, 1000).Value =
                    string.IsNullOrWhiteSpace(notes) ? (object)DBNull.Value : notes.Trim();

                connection.Open();

                command.ExecuteNonQuery();
            }
        }

        // تعريف الدالة RequireDifferentYears لتنفيذ العملية المرتبطة بها.
        private static void RequireDifferentYears(string fromYear, string toYear)
        {
            RequireYear(fromYear);
            RequireYear(toYear);
            if (Normalize(fromYear) == Normalize(toYear))

                throw new ArgumentException("يجب اختيار عامين دراسيين مختلفين.");
        }

        // تعريف الدالة RequireYear لتنفيذ العملية المرتبطة بها.
        private static void RequireYear(string year)
        {
            string value = Normalize(year);

            if (string.IsNullOrWhiteSpace(value) || value.Length > 20)
                throw new ArgumentException("العام الدراسي غير صالح.");

            string[] parts = value.Split('/');
            int firstYear;
            int secondYear;

            if (parts.Length != 2 ||
                parts[0].Length != 4 ||
                parts[1].Length != 4 ||
                !int.TryParse(parts[0], out firstYear) ||
                !int.TryParse(parts[1], out secondYear) ||
                secondYear != firstYear + 1)
            {
                throw new ArgumentException(
                    "صيغة العام الدراسي يجب أن تكون متسلسلة مثل 2026/2027 أو 1447-1448.");
            }
        }

        // تعريف الدالة Normalize لتنفيذ العملية المرتبطة بها.
        private static string Normalize(string year)
        {

            return (year ?? string.Empty).Trim().Replace('-', '/');
        }
    }
}
