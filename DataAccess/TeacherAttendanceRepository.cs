using System;
using System.Data;
using System.Data.SqlClient;
using SchoolSystem.Models;

namespace SchoolSystem.DataAccess
{
    public class TeacherAttendanceRepository
    {
        public DataTable GetAllAttendance()
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = @"
                    SELECT 
                        ta.AttendanceID,
                        ta.TeacherID,
                        t.FullName AS TeacherName,
                        ta.AttendanceDate,
                        ta.Status,
                        ta.CheckInTime,
                        ta.CheckOutTime,
                        ta.LateMinutes,
                        ta.EarlyLeaveMinutes,
                        ta.WorkHours,
                        ta.AbsenceReason,
                        ta.Notes,
                        ta.RecordedAt,
                        ta.UpdatedAt
                    FROM TeacherAttendance ta
                    INNER JOIN Teachers t ON ta.TeacherID = t.TeacherID
                    ORDER BY ta.AttendanceDate DESC, t.FullName ASC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        public bool AddAttendance(TeacherAttendance attendance)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = @"
                    INSERT INTO TeacherAttendance
                    (
                        TeacherID,
                        AttendanceDate,
                        Status,
                        CheckInTime,
                        CheckOutTime,
                        LateMinutes,
                        EarlyLeaveMinutes,
                        WorkHours,
                        AbsenceReason,
                        Notes,
                        RecordedAt
                    )
                    VALUES
                    (
                        @TeacherID,
                        @AttendanceDate,
                        @Status,
                        @CheckInTime,
                        @CheckOutTime,
                        @LateMinutes,
                        @EarlyLeaveMinutes,
                        @WorkHours,
                        @AbsenceReason,
                        @Notes,
                        GETDATE()
                    )";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    AddParameters(cmd, attendance, false);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool UpdateAttendance(TeacherAttendance attendance)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = @"
                    UPDATE TeacherAttendance
                    SET
                        TeacherID = @TeacherID,
                        AttendanceDate = @AttendanceDate,
                        Status = @Status,
                        CheckInTime = @CheckInTime,
                        CheckOutTime = @CheckOutTime,
                        LateMinutes = @LateMinutes,
                        EarlyLeaveMinutes = @EarlyLeaveMinutes,
                        WorkHours = @WorkHours,
                        AbsenceReason = @AbsenceReason,
                        Notes = @Notes,
                        UpdatedAt = GETDATE()
                    WHERE AttendanceID = @AttendanceID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    AddParameters(cmd, attendance, true);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool DeleteAttendance(int attendanceId)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = @"
                    DELETE FROM TeacherAttendance
                    WHERE AttendanceID = @AttendanceID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@AttendanceID", attendanceId);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool AttendanceExists(int teacherId, DateTime attendanceDate)
        {
            return AttendanceExists(teacherId, attendanceDate, 0);
        }

        public bool AttendanceExists(int teacherId, DateTime attendanceDate, int excludedAttendanceId)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = @"
                    SELECT COUNT(*)
                    FROM TeacherAttendance
                    WHERE TeacherID = @TeacherID
                      AND AttendanceDate = @AttendanceDate";

                if (excludedAttendanceId > 0)
                {
                    query += " AND AttendanceID <> @AttendanceID";
                }

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TeacherID", teacherId);
                    cmd.Parameters.AddWithValue("@AttendanceDate", attendanceDate.Date);

                    if (excludedAttendanceId > 0)
                    {
                        cmd.Parameters.AddWithValue("@AttendanceID", excludedAttendanceId);
                    }

                    conn.Open();

                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        private void AddParameters(SqlCommand cmd, TeacherAttendance attendance, bool includeId)
        {
            if (includeId)
            {
                cmd.Parameters.AddWithValue("@AttendanceID", attendance.AttendanceID);
            }

            cmd.Parameters.AddWithValue("@TeacherID", attendance.TeacherID);
            cmd.Parameters.AddWithValue("@AttendanceDate", attendance.AttendanceDate.Date);
            cmd.Parameters.AddWithValue("@Status", attendance.Status);

            if (attendance.CheckInTime.HasValue)
            {
                cmd.Parameters.AddWithValue("@CheckInTime", attendance.CheckInTime.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@CheckInTime", DBNull.Value);
            }

            if (attendance.CheckOutTime.HasValue)
            {
                cmd.Parameters.AddWithValue("@CheckOutTime", attendance.CheckOutTime.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@CheckOutTime", DBNull.Value);
            }

            cmd.Parameters.AddWithValue("@LateMinutes", attendance.LateMinutes);
            cmd.Parameters.AddWithValue("@EarlyLeaveMinutes", attendance.EarlyLeaveMinutes);
            cmd.Parameters.AddWithValue("@WorkHours", attendance.WorkHours);

            if (string.IsNullOrWhiteSpace(attendance.AbsenceReason))
            {
                cmd.Parameters.AddWithValue("@AbsenceReason", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@AbsenceReason", attendance.AbsenceReason.Trim());
            }

            if (string.IsNullOrWhiteSpace(attendance.Notes))
            {
                cmd.Parameters.AddWithValue("@Notes", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@Notes", attendance.Notes.Trim());
            }
        }
    }
}
