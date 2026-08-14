using System;
using System.Data;
using System.Data.SqlClient;
using SchoolSystem.Models;

namespace SchoolSystem.DataAccess
{
    public class EnrollmentRepository
    {
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

        public bool AddEnrollment(Enrollment enrollment)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = @"
                    IF NOT EXISTS
                    (
                        SELECT 1
                        FROM Students
                        WHERE StudentID = @StudentID
                          AND ISNULL(Status, N'نشط') = N'نشط'
                    )
                        THROW 50003, N'لا يمكن تسجيل طالب غير نشط.', 1;

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
                    )";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    AddParameters(cmd, enrollment, false);
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool UpdateEnrollment(Enrollment enrollment)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = @"
                    IF NOT EXISTS
                    (
                        SELECT 1
                        FROM Students
                        WHERE StudentID = @StudentID
                          AND ISNULL(Status, N'نشط') = N'نشط'
                    )
                        THROW 50003, N'لا يمكن ربط تسجيل بطالب غير نشط.', 1;

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
                    WHERE EnrollmentID = @EnrollmentID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    AddParameters(cmd, enrollment, true);
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

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

        public bool IsStudentEnrolled(int studentId, string academicYear, int excludeEnrollmentId = 0)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = @"
                    SELECT COUNT(*)
                    FROM Enrollments
                    WHERE StudentID = @StudentID
                      AND AcademicYear = @AcademicYear
                      AND Status <> N'مرفوض'
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

        private object NullableText(string value) => string.IsNullOrWhiteSpace(value) ? DBNull.Value : (object)value.Trim();
        private string SafeText(string value) => string.IsNullOrWhiteSpace(value) ? "" : value.Trim();
    }
}
