using System;
using System.Data;
using System.Data.SqlClient;
using SchoolSystem.Helpers;
using SchoolSystem.Models;


namespace SchoolSystem.DataAccess
{

    public class EnrollmentRepository
    {
        // تعريف الدالة GetAllEnrollments لتنفيذ العملية المرتبطة بها.
        public DataTable GetAllEnrollments()
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = @"
                    SELECT
                        e.EnrollmentID,
                        e.StudentID,
                        s.FullName AS StudentName,
                        s.StudentNumber,
                        e.ApplicationDate,
                        e.ApplicationType,
                        e.AcademicYear,
                        e.ClassID,
                        c.ClassName,
                        e.Section,
                        e.SeatNumber,
                        e.Status,
                        e.PreviousSchool,
                        e.PreviousClass,
                        e.TransferReason,
                        e.RegistrationFee,
                        e.PaidAmount,
                        (ISNULL(e.RegistrationFee, 0) - ISNULL(e.PaidAmount, 0)) AS RemainingAmount,
                        e.PaymentMethod,
                        e.ReceiptNo,
                        e.HasBirthCertificate,
                        e.HasGuardianId,
                        e.HasPhoto,
                        e.HasLastCertificate,
                        e.HasMedicalReport,
                        e.GeneralNotes AS Notes
                    FROM Enrollments e
                    LEFT JOIN Students s ON e.StudentID = s.StudentID
                    LEFT JOIN Classes c ON e.ClassID = c.ClassID
                    ORDER BY e.EnrollmentID DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();

                    adapter.Fill(dt);

                    return dt;
                }
            }
        }

        // تعريف الدالة AddEnrollment لتنفيذ العملية المرتبطة بها.
        public bool AddEnrollment(Enrollment enrollment)
        {
            const string nextSeatNumberQuery = @"
SELECT ISNULL(MAX(TRY_CONVERT(INT, NULLIF(LTRIM(RTRIM(SeatNumber)), N''))), 0) + 1
FROM Enrollments WITH (UPDLOCK, HOLDLOCK)
WHERE REPLACE(ISNULL(AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
  AND ClassID = @ClassID
  AND ISNULL(Section, N'') = ISNULL(@Section, N'')
  AND (Status IS NULL OR LTRIM(RTRIM(Status)) <> N'مرفوض');";

            const string insertQuery = @"
IF NOT EXISTS
(
    SELECT 1
    FROM Students
    WHERE StudentID = @StudentID
      AND ISNULL(Status, N'نشط') = N'نشط'
)
    THROW 50003, N'لا يمكن تسجيل طالب غير نشط.', 1;


IF EXISTS
(
    SELECT 1
    FROM Enrollments e WITH (UPDLOCK, HOLDLOCK)
    WHERE e.StudentID = @StudentID
      AND REPLACE(LTRIM(RTRIM(ISNULL(e.AcademicYear, N''))), N'-', N'/') =
          REPLACE(LTRIM(RTRIM(ISNULL(@AcademicYear, N''))), N'-', N'/')
      AND (e.Status IS NULL OR LTRIM(RTRIM(e.Status)) <> N'مرفوض')
)
    THROW 50004, N'هذا الطالب مسجل بالفعل في هذا العام الدراسي.', 1;

INSERT INTO Enrollments
(
    StudentID, ApplicationDate, ApplicationType, AcademicYear, ClassID, Section, SeatNumber, Status,
    PreviousSchool, PreviousClass, TransferReason, RegistrationFee, PaidAmount, PaymentMethod, ReceiptNo,
    HasBirthCertificate, HasGuardianId, HasPhoto, HasLastCertificate, HasMedicalReport, GeneralNotes, CreatedAt
)
VALUES
(
    @StudentID, @ApplicationDate, @ApplicationType, @AcademicYear, @ClassID, @Section, @SeatNumber, @Status,
    @PreviousSchool, @PreviousClass, @TransferReason, @RegistrationFee, @PaidAmount, @PaymentMethod, @ReceiptNo,
    @HasBirthCertificate, @HasGuardianId, @HasPhoto, @HasLastCertificate, @HasMedicalReport, @Notes, GETDATE()
);

SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using (SqlConnection conn = DbConnection.GetConnection())
            {

                conn.Open();

                using (SqlTransaction transaction = conn.BeginTransaction(IsolationLevel.Serializable))
                {
                    // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
                    try
                    {
                        if (string.IsNullOrWhiteSpace(enrollment.SeatNumber))
                        {
                            using (SqlCommand nextSeatCommand = new SqlCommand(nextSeatNumberQuery, conn, transaction))
                            {

                                nextSeatCommand.Parameters.AddWithValue("@AcademicYear", SafeText(enrollment.AcademicYear));

                                nextSeatCommand.Parameters.AddWithValue("@ClassID", enrollment.ClassID);

                                nextSeatCommand.Parameters.AddWithValue("@Section", NullableText(enrollment.Section));

                                enrollment.SeatNumber = Convert.ToInt32(nextSeatCommand.ExecuteScalar()).ToString("0000");
                            }
                        }

                        using (SqlCommand insertCommand = new SqlCommand(insertQuery, conn, transaction))
                        {
                            AddParameters(insertCommand, enrollment, false);

                            object result = insertCommand.ExecuteScalar();

                            if (result == null || result == DBNull.Value)
                            {
                                transaction.Rollback();

                                return false;
                            }

                            enrollment.EnrollmentID = Convert.ToInt32(result);

                            transaction.Commit();

                            return true;
                        }
                    }

                    catch (Exception ex)
                    {
                        ApplicationLogger.LogException("مستودع " + nameof(EnrollmentRepository) + ": إضافة تسجيل", ex);
                        transaction.Rollback();

                        throw;
                    }
                }
            }
        }

        // تعريف الدالة UpdateEnrollment لتنفيذ العملية المرتبطة بها.
        public bool UpdateEnrollment(Enrollment enrollment)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                const string query = @"
                    SET NOCOUNT ON;
                    SET XACT_ABORT ON;
                    SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
                    BEGIN TRANSACTION;

                    IF NOT EXISTS
                    (
                        SELECT 1
                        FROM Students
                        WHERE StudentID = @StudentID
                          AND ISNULL(Status, N'نشط') = N'نشط'
                    )
                        THROW 50003, N'لا يمكن ربط تسجيل بطالب غير نشط.', 1;

                    IF EXISTS
                    (
                        SELECT 1
                        FROM Enrollments e WITH (UPDLOCK, HOLDLOCK)
                        WHERE e.StudentID = @StudentID
                          AND REPLACE(LTRIM(RTRIM(ISNULL(e.AcademicYear, N''))), N'-', N'/') =
                              REPLACE(LTRIM(RTRIM(ISNULL(@AcademicYear, N''))), N'-', N'/')
                          AND e.EnrollmentID <> @EnrollmentID
                          AND (e.Status IS NULL OR LTRIM(RTRIM(e.Status)) <> N'مرفوض')
                    )
                        THROW 50004, N'هذا الطالب مسجل بالفعل في هذا العام الدراسي.', 1;

                    DECLARE @AssignedStudentClassID INT = NULL;
                    DECLARE @StudentClassAcademicYear NVARCHAR(20) = NULL;

                    SELECT TOP (1)
                        @AssignedStudentClassID = sc.StudentClassID,
                        @StudentClassAcademicYear = sc.AcademicYear
                    FROM StudentClasses sc WITH (UPDLOCK, HOLDLOCK)
                    WHERE sc.StudentID = @StudentID
                      AND REPLACE(ISNULL(sc.AcademicYear, N''), N'-', N'/') =
                          REPLACE(@AcademicYear, N'-', N'/')
                    ORDER BY sc.AssignedDate DESC, sc.StudentClassID DESC;

                    /* إذا كان الطالب موزعاً فعلياً، يجب أن يعكس التوزيع
                       الصف والشعبة الجديدين في التسجيل، وإلا ستنشأ نسختان مختلفتان
                       من بيانات الطالب في الواجهات المرتبطة. */
                    IF @AssignedStudentClassID IS NOT NULL
                    BEGIN
                        IF NOT EXISTS
                        (
                            SELECT 1
                            FROM SchoolSections ss
                            INNER JOIN Students s ON s.StudentID = @StudentID
                            WHERE ss.ClassID = @ClassID
                              AND LTRIM(RTRIM(ss.SectionName)) = LTRIM(RTRIM(ISNULL(@Section, N'')))
                              AND REPLACE(ISNULL(ss.AcademicYear, N''), N'-', N'/') =
                                  REPLACE(@AcademicYear, N'-', N'/')
                              AND ISNULL(ss.IsActive, 1) = 1
                              AND
                              (
                                  ss.AllowedGender IS NULL
                                  OR LTRIM(RTRIM(ss.AllowedGender)) = N''
                                  OR LTRIM(RTRIM(ss.AllowedGender)) IN (N'Any', N'All', N'الكل', N'مختلط')
                                  OR (LTRIM(RTRIM(ss.AllowedGender)) IN (N'Male', N'ذكر', N'ذكور')
                                      AND LTRIM(RTRIM(ISNULL(s.Gender, N''))) IN (N'Male', N'ذكر', N'ذكور'))
                                  OR (LTRIM(RTRIM(ss.AllowedGender)) IN (N'Female', N'أنثى', N'إناث')
                                      AND LTRIM(RTRIM(ISNULL(s.Gender, N''))) IN (N'Female', N'أنثى', N'إناث'))
                              )
                        )
                            THROW 50020, N'لا يمكن مزامنة التوزيع: الشعبة الجديدة غير موجودة أو لا تسمح بجنس الطالب.', 1;

                        IF EXISTS
                        (
                            SELECT 1
                            FROM SchoolSections ss
                            WHERE ss.ClassID = @ClassID
                              AND LTRIM(RTRIM(ss.SectionName)) = LTRIM(RTRIM(ISNULL(@Section, N'')))
                              AND REPLACE(ISNULL(ss.AcademicYear, N''), N'-', N'/') =
                                  REPLACE(@AcademicYear, N'-', N'/')
                              AND ISNULL(ss.IsActive, 1) = 1
                              AND ss.Capacity IS NOT NULL
                              AND ss.Capacity > 0
                              AND
                              (
                                  SELECT COUNT(1)
                                  FROM StudentClasses sc
                                  WHERE sc.ClassID = @ClassID
                                    AND LTRIM(RTRIM(sc.Section)) = LTRIM(RTRIM(ISNULL(@Section, N'')))
                                    AND REPLACE(ISNULL(sc.AcademicYear, N''), N'-', N'/') =
                                        REPLACE(@AcademicYear, N'-', N'/')
                                    AND sc.StudentClassID <> @AssignedStudentClassID
                              ) >= ss.Capacity
                        )
                            THROW 50021, N'لا يمكن مزامنة التوزيع: الشعبة الجديدة ممتلئة.', 1;
                    END;

                    UPDATE Enrollments
                    SET
                        StudentID = @StudentID,
                        ApplicationDate = @ApplicationDate,
                        ApplicationType = @ApplicationType,
                        AcademicYear = @AcademicYear,
                        ClassID = @ClassID,
                        Section = @Section,
                        SeatNumber = @SeatNumber,
                        Status = @Status,
                        PreviousSchool = @PreviousSchool,
                        PreviousClass = @PreviousClass,
                        TransferReason = @TransferReason,
                        RegistrationFee = @RegistrationFee,
                        PaidAmount = @PaidAmount,
                        PaymentMethod = @PaymentMethod,
                        ReceiptNo = @ReceiptNo,
                        HasBirthCertificate = @HasBirthCertificate,
                        HasGuardianId = @HasGuardianId,
                        HasPhoto = @HasPhoto,
                        HasLastCertificate = @HasLastCertificate,
                        HasMedicalReport = @HasMedicalReport,
                        GeneralNotes = @Notes,
                        UpdatedAt = GETDATE()
                    WHERE EnrollmentID = @EnrollmentID;

                    IF @@ROWCOUNT = 0
                    BEGIN
                        ROLLBACK TRANSACTION;
                        RETURN 0;
                    END;

                    IF @AssignedStudentClassID IS NOT NULL
                    BEGIN
                        UPDATE StudentClasses
                        SET
                            ClassID = @ClassID,
                            Section = LTRIM(RTRIM(@Section)),
                            AcademicYear = REPLACE(@AcademicYear, N'-', N'/'),
                            AssignedDate = GETDATE()
                        WHERE StudentClassID = @AssignedStudentClassID;

                        UPDATE Students
                        SET
                            ClassID = @ClassID,
                            Section = LTRIM(RTRIM(@Section)),
                            AcademicYear = REPLACE(@AcademicYear, N'-', N'/'),
                            UpdatedAt = GETDATE()
                        WHERE StudentID = @StudentID;
                    END
                    ELSE IF LTRIM(RTRIM(ISNULL(@Status, N''))) IN
                    (
                        N'مقبول',
                        N'Accepted',
                        N'Approved',
                        N'نشط',
                        N'Active'
                    )
                    BEGIN
                        /* لا يوجد توزيع بعد، لكن التسجيل المقبول هو المرجع
                           الحالي لبيانات الصف في شاشة الطلاب. */
                        UPDATE Students
                        SET
                            ClassID = @ClassID,
                            Section = LTRIM(RTRIM(@Section)),
                            AcademicYear = REPLACE(@AcademicYear, N'-', N'/'),
                            UpdatedAt = GETDATE()
                        WHERE StudentID = @StudentID;
                    END;

                    COMMIT TRANSACTION;
                    SELECT 1;";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    AddParameters(cmd, enrollment, true);

                    conn.Open();

                    object result = cmd.ExecuteScalar();

                    return result != null &&
                           result != DBNull.Value &&
                           Convert.ToInt32(result) > 0;
                }
            }
        }

        // تعريف الدالة DeleteEnrollment لتنفيذ العملية المرتبطة بها.
        public bool DeleteEnrollment(int enrollmentId)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = "DELETE FROM Enrollments WHERE EnrollmentID = @EnrollmentID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    cmd.Parameters.AddWithValue("@EnrollmentID", enrollmentId);

                    conn.Open();

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // تعريف الدالة GenerateNextSeatNumber لتنفيذ العملية المرتبطة بها.
        public string GenerateNextSeatNumber(string academicYear, int classId, string section)
        {
            const string query = @"
SELECT ISNULL(MAX(TRY_CONVERT(INT, NULLIF(LTRIM(RTRIM(SeatNumber)), N''))), 0) + 1
FROM Enrollments
WHERE REPLACE(ISNULL(AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
  AND ClassID = @ClassID
  AND ISNULL(Section, N'') = ISNULL(@Section, N'')
  AND (Status IS NULL OR LTRIM(RTRIM(Status)) <> N'مرفوض');";

            using (SqlConnection conn = DbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {

                cmd.Parameters.AddWithValue("@AcademicYear", SafeText(academicYear));

                cmd.Parameters.AddWithValue("@ClassID", classId);

                cmd.Parameters.AddWithValue("@Section", NullableText(section));

                conn.Open();

                int nextNumber = Convert.ToInt32(cmd.ExecuteScalar());

                return nextNumber.ToString("0000");
            }
        }

        // تعريف الدالة GetEnrollmentById لتنفيذ العملية المرتبطة بها.
        public Enrollment GetEnrollmentById(int enrollmentId)
        {
            if (enrollmentId <= 0)
                return null;

            const string query = @"
                SELECT TOP 1
                    e.EnrollmentID,
                    e.StudentID,
                    s.FullName AS StudentName,
                    s.StudentNumber,
                    e.ApplicationDate,
                    e.ApplicationType,
                    e.AcademicYear,
                    e.ClassID,
                    c.ClassName,
                    e.Section,
                    e.SeatNumber,
                    e.Status,
                    e.PreviousSchool,
                    e.PreviousClass,
                    e.TransferReason,
                    ISNULL(e.RegistrationFee, 0) AS RegistrationFee,
                    ISNULL(e.PaidAmount, 0) AS PaidAmount,
                    e.PaymentMethod,
                    e.ReceiptNo,
                    ISNULL(e.HasBirthCertificate, 0) AS HasBirthCertificate,
                    ISNULL(e.HasGuardianId, 0) AS HasGuardianId,
                    ISNULL(e.HasPhoto, 0) AS HasPhoto,
                    ISNULL(e.HasLastCertificate, 0) AS HasLastCertificate,
                    ISNULL(e.HasMedicalReport, 0) AS HasMedicalReport,
                    e.GeneralNotes AS Notes
                FROM Enrollments e
                LEFT JOIN Students s ON e.StudentID = s.StudentID
                LEFT JOIN Classes c ON e.ClassID = c.ClassID
                WHERE e.EnrollmentID = @EnrollmentID;";

            using (SqlConnection conn = DbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@EnrollmentID", enrollmentId);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                        return null;

                    return MapEnrollment(reader);
                }
            }
        }

        // تعريف الدالة HasRegistrationFinance لتنفيذ العملية المرتبطة بها.
        public bool HasRegistrationFinance(int studentId, string academicYear)
        {
            if (studentId <= 0 || string.IsNullOrWhiteSpace(academicYear))
                return false;

            const string query = @"
                SELECT COUNT(*)
                FROM Fees
                WHERE StudentID = @StudentID
                  AND REPLACE(ISNULL(AcademicYear, N''), N'-', N'/') =
                      REPLACE(@AcademicYear, N'-', N'/')
                  AND LTRIM(RTRIM(ISNULL(FeeType, N''))) IN
                      (N'رسوم تسجيل', N'رسوم التسجيل', N'التسجيل', N'Registration Fee');";

            using (SqlConnection conn = DbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@StudentID", studentId);
                cmd.Parameters.AddWithValue("@AcademicYear", academicYear.Trim());
                conn.Open();

                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        // تعريف الدالة MapEnrollment لتنفيذ العملية المرتبطة بها.
        private Enrollment MapEnrollment(SqlDataReader reader)
        {
            return new Enrollment
            {
                EnrollmentID = Convert.ToInt32(reader["EnrollmentID"]),
                StudentID = Convert.ToInt32(reader["StudentID"]),
                StudentName = reader["StudentName"] == DBNull.Value ? string.Empty : reader["StudentName"].ToString(),
                StudentNumber = reader["StudentNumber"] == DBNull.Value ? string.Empty : reader["StudentNumber"].ToString(),
                ApplicationDate = reader["ApplicationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["ApplicationDate"]),
                ApplicationType = reader["ApplicationType"] == DBNull.Value ? string.Empty : reader["ApplicationType"].ToString(),
                AcademicYear = reader["AcademicYear"] == DBNull.Value ? string.Empty : reader["AcademicYear"].ToString(),
                ClassID = reader["ClassID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["ClassID"]),
                ClassName = reader["ClassName"] == DBNull.Value ? string.Empty : reader["ClassName"].ToString(),
                Section = reader["Section"] == DBNull.Value ? string.Empty : reader["Section"].ToString(),
                SeatNumber = reader["SeatNumber"] == DBNull.Value ? string.Empty : reader["SeatNumber"].ToString(),
                Status = reader["Status"] == DBNull.Value ? string.Empty : reader["Status"].ToString(),
                PreviousSchool = reader["PreviousSchool"] == DBNull.Value ? string.Empty : reader["PreviousSchool"].ToString(),
                PreviousClass = reader["PreviousClass"] == DBNull.Value ? string.Empty : reader["PreviousClass"].ToString(),
                TransferReason = reader["TransferReason"] == DBNull.Value ? string.Empty : reader["TransferReason"].ToString(),
                RegistrationFee = reader["RegistrationFee"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["RegistrationFee"]),
                PaidAmount = reader["PaidAmount"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["PaidAmount"]),
                PaymentMethod = reader["PaymentMethod"] == DBNull.Value ? string.Empty : reader["PaymentMethod"].ToString(),
                ReceiptNo = reader["ReceiptNo"] == DBNull.Value ? string.Empty : reader["ReceiptNo"].ToString(),
                HasBirthCertificate = reader["HasBirthCertificate"] != DBNull.Value && Convert.ToBoolean(reader["HasBirthCertificate"]),
                HasGuardianId = reader["HasGuardianId"] != DBNull.Value && Convert.ToBoolean(reader["HasGuardianId"]),
                HasPhoto = reader["HasPhoto"] != DBNull.Value && Convert.ToBoolean(reader["HasPhoto"]),
                HasLastCertificate = reader["HasLastCertificate"] != DBNull.Value && Convert.ToBoolean(reader["HasLastCertificate"]),
                HasMedicalReport = reader["HasMedicalReport"] != DBNull.Value && Convert.ToBoolean(reader["HasMedicalReport"]),
                Notes = reader["Notes"] == DBNull.Value ? string.Empty : reader["Notes"].ToString()
            };
        }

        // تعريف الدالة IsStudentEnrolled لتنفيذ العملية المرتبطة بها.
        public bool IsStudentEnrolled(int studentId, string academicYear, int excludeEnrollmentId = 0)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = @"
                    SELECT COUNT(*)
                    FROM Enrollments
                    WHERE StudentID = @StudentID
                      AND REPLACE(ISNULL(AcademicYear, N''), N'-', N'/') = REPLACE(@AcademicYear, N'-', N'/')
                      AND (Status IS NULL OR LTRIM(RTRIM(Status)) <> N'مرفوض')
                      AND (@ExcludeEnrollmentID = 0 OR EnrollmentID <> @ExcludeEnrollmentID)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    cmd.Parameters.AddWithValue("@StudentID", studentId);

                    cmd.Parameters.AddWithValue("@AcademicYear", academicYear.Trim());

                    cmd.Parameters.AddWithValue("@ExcludeEnrollmentID", excludeEnrollmentId);


                    conn.Open();

                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
        }

        // تعريف الدالة AddParameters لتنفيذ العملية المرتبطة بها.
        private void AddParameters(SqlCommand cmd, Enrollment enrollment, bool includeId)
        {

            if (includeId) cmd.Parameters.AddWithValue("@EnrollmentID", enrollment.EnrollmentID);


            cmd.Parameters.AddWithValue("@StudentID", enrollment.StudentID);

            cmd.Parameters.AddWithValue("@ApplicationDate", enrollment.ApplicationDate.Date);

            cmd.Parameters.AddWithValue("@ApplicationType", SafeText(enrollment.ApplicationType));

            cmd.Parameters.AddWithValue("@AcademicYear", SafeText(enrollment.AcademicYear));

            cmd.Parameters.AddWithValue("@ClassID", enrollment.ClassID);

            cmd.Parameters.AddWithValue("@Section", NullableText(enrollment.Section));

            cmd.Parameters.AddWithValue("@SeatNumber", NullableText(enrollment.SeatNumber));

            cmd.Parameters.AddWithValue("@Status", SafeText(enrollment.Status));


            cmd.Parameters.AddWithValue("@PreviousSchool", NullableText(enrollment.PreviousSchool));

            cmd.Parameters.AddWithValue("@PreviousClass", NullableText(enrollment.PreviousClass));

            cmd.Parameters.AddWithValue("@TransferReason", NullableText(enrollment.TransferReason));


            cmd.Parameters.AddWithValue("@RegistrationFee", enrollment.RegistrationFee);

            cmd.Parameters.AddWithValue("@PaidAmount", enrollment.PaidAmount);

            cmd.Parameters.AddWithValue("@PaymentMethod", NullableText(enrollment.PaymentMethod));

            cmd.Parameters.AddWithValue("@ReceiptNo", NullableText(enrollment.ReceiptNo));


            cmd.Parameters.AddWithValue("@HasBirthCertificate", enrollment.HasBirthCertificate);

            cmd.Parameters.AddWithValue("@HasGuardianId", enrollment.HasGuardianId);

            cmd.Parameters.AddWithValue("@HasPhoto", enrollment.HasPhoto);

            cmd.Parameters.AddWithValue("@HasLastCertificate", enrollment.HasLastCertificate);

            cmd.Parameters.AddWithValue("@HasMedicalReport", enrollment.HasMedicalReport);


            cmd.Parameters.AddWithValue("@Notes", NullableText(enrollment.Notes));
        }

        // تعريف الدالة NullableText لتنفيذ العملية المرتبطة بها.
        private object NullableText(string value) => string.IsNullOrWhiteSpace(value) ? DBNull.Value : (object)value.Trim();
        // تعريف الدالة SafeText لتنفيذ العملية المرتبطة بها.
        private string SafeText(string value) => string.IsNullOrWhiteSpace(value) ? "" : value.Trim();
    }
}
