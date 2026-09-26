using System;
using System.Data;
using System.Data.SqlClient;
using SchoolSystem.Helpers;
using SchoolSystem.Models;


namespace SchoolSystem.DataAccess
{

    public class SchoolSettingsRepository
    {

        private bool schemaReady;

        // تعريف الدالة EnsureSchema لتنفيذ العملية المرتبطة بها.
        private void EnsureSchema(SqlConnection connection, SqlTransaction transaction)
        {
            if (schemaReady)
                return;

            const string sql = @"
IF OBJECT_ID(N'dbo.SchoolSettings', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.SchoolSettings
    (
        SchoolSettingsID INT NOT NULL CONSTRAINT PK_SchoolSettings PRIMARY KEY,
        SchoolNameAr NVARCHAR(200) NOT NULL CONSTRAINT CK_SchoolSettings_NameAr CHECK (LEN(LTRIM(RTRIM(SchoolNameAr))) > 0),
        SchoolNameEn NVARCHAR(200) NOT NULL CONSTRAINT CK_SchoolSettings_NameEn CHECK (LEN(LTRIM(RTRIM(SchoolNameEn))) > 0),
        OrganizationNameAr NVARCHAR(200) NULL,
        OrganizationNameEn NVARCHAR(200) NULL,
        AddressAr NVARCHAR(500) NULL,
        AddressEn NVARCHAR(500) NULL,
        Phone NVARCHAR(40) NULL,
        AdditionalPhone NVARCHAR(40) NULL,
        Email NVARCHAR(254) NULL,
        Website NVARCHAR(300) NULL,
        PrincipalNameAr NVARCHAR(200) NULL,
        PrincipalNameEn NVARCHAR(200) NULL,
        LicenseNumber NVARCHAR(100) NULL,
        Logo VARBINARY(MAX) NULL,
        LogoFileName NVARCHAR(260) NULL,
        UpdatedAt DATETIME2(0) NOT NULL CONSTRAINT DF_SchoolSettings_UpdatedAt DEFAULT SYSUTCDATETIME(),
        UpdatedByUserID INT NULL
    );
END
IF NOT EXISTS (SELECT 1 FROM dbo.SchoolSettings WHERE SchoolSettingsID = 1)
BEGIN
    INSERT INTO dbo.SchoolSettings (SchoolSettingsID, SchoolNameAr, SchoolNameEn)
    VALUES (1, N'اسم المدرسة', N'School Name');
END";

            // إنهاء كتلة أو معاملة قاعدة البيانات الحالية.
            using (SqlCommand command = new SqlCommand(sql, connection, transaction))
                command.ExecuteNonQuery();

            schemaReady = true;
        }

        // تعريف الدالة Get لتنفيذ العملية المرتبطة بها.
        public SchoolSettings Get()
        {
            using (SqlConnection connection = DbConnection.GetConnection())
            {

                connection.Open();
                EnsureSchema(connection, null);
                const string sql = @"SELECT TOP 1 SchoolSettingsID, SchoolNameAr, SchoolNameEn,
OrganizationNameAr, OrganizationNameEn, AddressAr, AddressEn, Phone, AdditionalPhone,
Email, Website, PrincipalNameAr, PrincipalNameEn, LicenseNumber, Logo, LogoFileName,
UpdatedAt, UpdatedByUserID FROM dbo.SchoolSettings WHERE SchoolSettingsID = 1;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                        return new SchoolSettings { SchoolSettingsID = 1, SchoolNameAr = "اسم المدرسة", SchoolNameEn = "School Name" };

                    return Map(reader);
                }
            }
        }

        // تعريف الدالة Save لتنفيذ العملية المرتبطة بها.
        public void Save(SchoolSettings settings)
        {
            if (settings == null)

                throw new ArgumentNullException("settings");

            if (string.IsNullOrWhiteSpace(settings.SchoolNameAr) || string.IsNullOrWhiteSpace(settings.SchoolNameEn))

                throw new InvalidOperationException("اسم المدرسة بالعربية والإنجليزية مطلوبان.");


            using (SqlConnection connection = DbConnection.GetConnection())
            {

                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
                    try
                    {
                        EnsureSchema(connection, transaction);
                        const string sql = @"UPDATE dbo.SchoolSettings SET
SchoolNameAr=@SchoolNameAr, SchoolNameEn=@SchoolNameEn,
OrganizationNameAr=@OrganizationNameAr, OrganizationNameEn=@OrganizationNameEn,
AddressAr=@AddressAr, AddressEn=@AddressEn, Phone=@Phone, AdditionalPhone=@AdditionalPhone,
Email=@Email, Website=@Website, PrincipalNameAr=@PrincipalNameAr, PrincipalNameEn=@PrincipalNameEn,
LicenseNumber=@LicenseNumber, Logo=@Logo, LogoFileName=@LogoFileName,
UpdatedAt=SYSUTCDATETIME(), UpdatedByUserID=@UpdatedByUserID
WHERE SchoolSettingsID=1;";
                        using (SqlCommand command = new SqlCommand(sql, connection, transaction))
                        {
                            Add(command, "@SchoolNameAr", settings.SchoolNameAr, 200);
                            Add(command, "@SchoolNameEn", settings.SchoolNameEn, 200);
                            Add(command, "@OrganizationNameAr", settings.OrganizationNameAr, 200);
                            Add(command, "@OrganizationNameEn", settings.OrganizationNameEn, 200);
                            Add(command, "@AddressAr", settings.AddressAr, 500);
                            Add(command, "@AddressEn", settings.AddressEn, 500);
                            Add(command, "@Phone", settings.Phone, 40);
                            Add(command, "@AdditionalPhone", settings.AdditionalPhone, 40);
                            Add(command, "@Email", settings.Email, 254);
                            Add(command, "@Website", settings.Website, 300);
                            Add(command, "@PrincipalNameAr", settings.PrincipalNameAr, 200);
                            Add(command, "@PrincipalNameEn", settings.PrincipalNameEn, 200);
                            Add(command, "@LicenseNumber", settings.LicenseNumber, 100);

                            SqlParameter logo = command.Parameters.Add("@Logo", SqlDbType.VarBinary, -1);

                            logo.Value = (object)settings.Logo ?? DBNull.Value;
                            Add(command, "@LogoFileName", settings.LogoFileName, 260);

                            SqlParameter user = command.Parameters.Add("@UpdatedByUserID", SqlDbType.Int);

                            user.Value = (object)settings.UpdatedByUserID ?? DBNull.Value;

                            command.ExecuteNonQuery();
                        }
                        transaction.Commit();
                    }

                    catch (Exception ex)
                    {
                        ApplicationLogger.LogException("مستودع " + nameof(SchoolSettingsRepository) + ": حفظ إعدادات المدرسة", ex);
                        transaction.Rollback();

                        throw;
                    }
                }
            }
        }

        // تعريف الدالة Add لتنفيذ العملية المرتبطة بها.
        private static void Add(SqlCommand command, string name, string value, int size)
        {

            SqlParameter parameter = command.Parameters.Add(name, SqlDbType.NVarChar, size);

            parameter.Value = (object)value ?? DBNull.Value;
        }

        // تعريف الدالة Map لتنفيذ العملية المرتبطة بها.
        private static SchoolSettings Map(SqlDataReader reader)
        {

            return new SchoolSettings
            {
                SchoolSettingsID = Convert.ToInt32(reader["SchoolSettingsID"]),
                SchoolNameAr = Convert.ToString(reader["SchoolNameAr"]),
                SchoolNameEn = Convert.ToString(reader["SchoolNameEn"]),
                OrganizationNameAr = NullableString(reader, "OrganizationNameAr"),
                OrganizationNameEn = NullableString(reader, "OrganizationNameEn"),
                AddressAr = NullableString(reader, "AddressAr"),
                AddressEn = NullableString(reader, "AddressEn"),
                Phone = NullableString(reader, "Phone"),
                AdditionalPhone = NullableString(reader, "AdditionalPhone"),
                Email = NullableString(reader, "Email"),
                Website = NullableString(reader, "Website"),
                PrincipalNameAr = NullableString(reader, "PrincipalNameAr"),
                PrincipalNameEn = NullableString(reader, "PrincipalNameEn"),
                LicenseNumber = NullableString(reader, "LicenseNumber"),
                Logo = reader["Logo"] == DBNull.Value ? null : (byte[])reader["Logo"],
                LogoFileName = NullableString(reader, "LogoFileName"),
                UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"]),
                UpdatedByUserID = reader["UpdatedByUserID"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["UpdatedByUserID"])

            };
        }

        // تعريف الدالة NullableString لتنفيذ العملية المرتبطة بها.
        private static string NullableString(SqlDataReader reader, string name)
        {

            return reader[name] == DBNull.Value ? null : Convert.ToString(reader[name]);
        }
    }
}
