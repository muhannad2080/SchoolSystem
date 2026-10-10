using System;
using System.Data;
using System.Data.SqlClient;
using SchoolSystem.Models;

namespace SchoolSystem.DataAccess
{
    public class StudentTransportRepository
    {
        public DataTable GetAll()
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                const string query = @"
SELECT
    a.AssignmentID,
    a.StudentID,
    s.StudentNumber,
    s.FullName AS StudentName,
    a.RouteID,
    r.RouteName,
    b.BusNumber,
    a.AcademicYear,
    a.PickupPoint,
    a.DropoffPoint,
    a.IsActive,
    a.StartDate,
    a.EndDate,
    a.Notes,
    a.CreatedAt,
    a.UpdatedAt
FROM dbo.StudentTransportAssignments a
INNER JOIN dbo.Students s ON s.StudentID = a.StudentID
INNER JOIN dbo.BusRoutes r ON r.RouteID = a.RouteID
INNER JOIN dbo.Buses b ON b.BusID = r.BusID
ORDER BY a.AssignmentID DESC;";

                using (SqlCommand cmd = new SqlCommand(query, con))
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetActiveStudents()
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                const string query = @"
SELECT StudentID, StudentNumber, FullName
FROM dbo.Students
WHERE ISNULL(Status, N'نشط') = N'نشط'
ORDER BY FullName;";

                using (SqlCommand cmd = new SqlCommand(query, con))
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetRoutes()
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                const string query = @"
SELECT r.RouteID, r.RouteName, b.BusNumber, b.Capacity
FROM dbo.BusRoutes r
INNER JOIN dbo.Buses b ON b.BusID = r.BusID
ORDER BY r.RouteName;";

                using (SqlCommand cmd = new SqlCommand(query, con))
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        public bool Add(StudentTransportAssignment assignment)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                con.Open();
                using (SqlTransaction transaction = con.BeginTransaction(IsolationLevel.Serializable))
                {
                    ValidateAndLock(con, transaction, assignment, 0);

                    const string query = @"
INSERT INTO dbo.StudentTransportAssignments
(
    StudentID, RouteID, AcademicYear, PickupPoint, DropoffPoint,
    IsActive, StartDate, EndDate, Notes, CreatedAt
)
OUTPUT INSERTED.AssignmentID
VALUES
(
    @StudentID, @RouteID, @AcademicYear, @PickupPoint, @DropoffPoint,
    @IsActive, @StartDate, @EndDate, @Notes, GETDATE()
);";

                    using (SqlCommand cmd = new SqlCommand(query, con, transaction))
                    {
                        AddParameters(cmd, assignment, false);
                        object result = cmd.ExecuteScalar();
                        if (result == null || result == DBNull.Value)
                        {
                            transaction.Rollback();
                            return false;
                        }

                        assignment.AssignmentID = Convert.ToInt32(result);
                    }

                    transaction.Commit();
                    return assignment.AssignmentID > 0;
                }
            }
        }

        public bool Update(StudentTransportAssignment assignment)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                con.Open();
                using (SqlTransaction transaction = con.BeginTransaction(IsolationLevel.Serializable))
                {
                    ValidateAndLock(con, transaction, assignment, assignment.AssignmentID);

                    const string query = @"
UPDATE dbo.StudentTransportAssignments
SET
    StudentID = @StudentID,
    RouteID = @RouteID,
    AcademicYear = @AcademicYear,
    PickupPoint = @PickupPoint,
    DropoffPoint = @DropoffPoint,
    IsActive = @IsActive,
    StartDate = @StartDate,
    EndDate = @EndDate,
    Notes = @Notes,
    UpdatedAt = GETDATE()
WHERE AssignmentID = @AssignmentID;";

                    using (SqlCommand cmd = new SqlCommand(query, con, transaction))
                    {
                        AddParameters(cmd, assignment, true);
                        if (cmd.ExecuteNonQuery() == 0)
                        {
                            transaction.Rollback();
                            return false;
                        }
                    }

                    transaction.Commit();
                    return true;
                }
            }
        }

        public bool Delete(int assignmentId)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                con.Open();
                using (SqlTransaction transaction = con.BeginTransaction(IsolationLevel.Serializable))
                {
                    const string query = @"
UPDATE dbo.StudentTransportAssignments
SET IsActive = 0, EndDate = CAST(GETDATE() AS DATE), UpdatedAt = GETDATE()
WHERE AssignmentID = @AssignmentID AND IsActive = 1;";

                    using (SqlCommand cmd = new SqlCommand(query, con, transaction))
                    {
                        cmd.Parameters.Add("@AssignmentID", SqlDbType.Int).Value = assignmentId;
                        bool updated = cmd.ExecuteNonQuery() > 0;
                        if (!updated)
                        {
                            transaction.Rollback();
                            return false;
                        }
                    }

                    transaction.Commit();
                    return true;
                }
            }
        }

        private void ValidateAndLock(SqlConnection con, SqlTransaction transaction,
            StudentTransportAssignment assignment, int excludedAssignmentId)
        {
            const string validation = @"
IF NOT EXISTS
(
    SELECT 1 FROM dbo.Students WITH (UPDLOCK, HOLDLOCK)
    WHERE StudentID = @StudentID AND ISNULL(Status, N'نشط') = N'نشط'
)
    THROW 51401, N'الطالب غير موجود أو غير نشط.', 1;

IF NOT EXISTS
(
    SELECT 1
    FROM dbo.BusRoutes r WITH (UPDLOCK, HOLDLOCK)
    INNER JOIN dbo.Buses b WITH (UPDLOCK, HOLDLOCK) ON b.BusID = r.BusID
    WHERE r.RouteID = @RouteID
)
    THROW 51402, N'مسار النقل المحدد غير موجود أو حافلته غير موجودة.', 1;

IF @IsActive = 1
BEGIN
    IF NOT EXISTS
    (
        SELECT 1
        FROM dbo.Enrollments e WITH (UPDLOCK, HOLDLOCK)
        WHERE e.StudentID = @StudentID
          AND REPLACE(LTRIM(RTRIM(ISNULL(e.AcademicYear, N''))), N'-', N'/') =
              REPLACE(LTRIM(RTRIM(ISNULL(@AcademicYear, N''))), N'-', N'/')
          AND LTRIM(RTRIM(ISNULL(e.Status, N''))) IN
              (N'مقبول', N'Accepted', N'Approved', N'نشط', N'Active')
    )
        THROW 51405, N'لا يمكن تفعيل اشتراك النقل قبل قبول تسجيل الطالب في العام الدراسي نفسه.', 1;

    IF EXISTS
    (
        SELECT 1
        FROM dbo.StudentTransportAssignments WITH (UPDLOCK, HOLDLOCK)
        WHERE StudentID = @StudentID
          AND REPLACE(LTRIM(RTRIM(ISNULL(AcademicYear, N''))), N'-', N'/') =
              REPLACE(LTRIM(RTRIM(ISNULL(@AcademicYear, N''))), N'-', N'/')
          AND IsActive = 1
          AND AssignmentID <> @ExcludedAssignmentID
    )
        THROW 51403, N'الطالب لديه اشتراك نقل نشط بالفعل في هذا العام الدراسي.', 1;

    DECLARE @Capacity INT;
    DECLARE @Assigned INT;

    SELECT @Capacity = b.Capacity
    FROM dbo.BusRoutes r
    INNER JOIN dbo.Buses b ON b.BusID = r.BusID
    WHERE r.RouteID = @RouteID;

    SELECT @Assigned = COUNT(*)
    FROM dbo.StudentTransportAssignments
    WHERE RouteID = @RouteID
      AND REPLACE(LTRIM(RTRIM(ISNULL(AcademicYear, N''))), N'-', N'/') =
          REPLACE(LTRIM(RTRIM(ISNULL(@AcademicYear, N''))), N'-', N'/')
      AND IsActive = 1
      AND AssignmentID <> @ExcludedAssignmentID;

    IF ISNULL(@Assigned, 0) >= ISNULL(@Capacity, 0)
        THROW 51404, N'لا توجد مقاعد متاحة في الحافلة المرتبطة بهذا المسار.', 1;
END;";

            using (SqlCommand cmd = new SqlCommand(validation, con, transaction))
            {
                AddParameters(cmd, assignment, true);
                cmd.Parameters.Add("@ExcludedAssignmentID", SqlDbType.Int).Value = excludedAssignmentId;
                cmd.ExecuteNonQuery();
            }
        }

        private void AddParameters(SqlCommand cmd, StudentTransportAssignment assignment, bool includeId)
        {
            if (includeId)
                cmd.Parameters.Add("@AssignmentID", SqlDbType.Int).Value = assignment.AssignmentID;

            cmd.Parameters.Add("@StudentID", SqlDbType.Int).Value = assignment.StudentID;
            cmd.Parameters.Add("@RouteID", SqlDbType.Int).Value = assignment.RouteID;
            cmd.Parameters.Add("@AcademicYear", SqlDbType.NVarChar, 20).Value = assignment.AcademicYear.Trim();
            cmd.Parameters.Add("@PickupPoint", SqlDbType.NVarChar, 200).Value = assignment.PickupPoint.Trim();
            cmd.Parameters.Add("@DropoffPoint", SqlDbType.NVarChar, 200).Value = assignment.DropoffPoint.Trim();
            cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = assignment.IsActive;
            cmd.Parameters.Add("@StartDate", SqlDbType.Date).Value = assignment.StartDate.Date;
            cmd.Parameters.Add("@EndDate", SqlDbType.Date).Value =
                assignment.EndDate.HasValue ? (object)assignment.EndDate.Value.Date : DBNull.Value;
            cmd.Parameters.Add("@Notes", SqlDbType.NVarChar, 1000).Value =
                string.IsNullOrWhiteSpace(assignment.Notes) ? (object)DBNull.Value : assignment.Notes.Trim();
        }
    }
}