using System;
using System.Data;
using System.Data.SqlClient;
using SchoolSystem.Models;

namespace SchoolSystem.DataAccess
{
    public class RoomRepository
    {
        public DataTable GetAllRooms()
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = @"
                    SELECT
                        RoomID,
                        RoomCode,
                        RoomName,
                        RoomType,
                        Capacity,
                        Location,
                        IsActive,
                        Notes,
                        CreatedAt,
                        UpdatedAt
                    FROM Rooms
                    ORDER BY RoomName";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetActiveRooms()
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = @"
                    SELECT RoomID, RoomName
                    FROM Rooms
                    WHERE IsActive = 1
                    ORDER BY RoomName";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        public int AddRoom(Room room)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction(IsolationLevel.Serializable))
                {
                    const string duplicateQuery = @"
                        SELECT COUNT(1)
                        FROM Rooms
                        WHERE RoomName = @RoomName AND ISNULL(IsActive, 1) = 1";
                    using (SqlCommand duplicateCommand = new SqlCommand(duplicateQuery, conn, transaction))
                    {
                        duplicateCommand.Parameters.Add("@RoomName", SqlDbType.NVarChar, 100).Value = room.RoomName.Trim();
                        if (Convert.ToInt32(duplicateCommand.ExecuteScalar()) > 0)
                            throw new InvalidOperationException("اسم القاعة مستخدم مسبقًا.");
                    }

                    const string query = @"
                        INSERT INTO Rooms
                        (
                            RoomCode,
                            RoomName,
                            RoomType,
                            Capacity,
                            Location,
                            IsActive,
                            Notes,
                            CreatedAt
                        )
                        OUTPUT INSERTED.RoomID
                        VALUES
                        (
                            NULL,
                            @RoomName,
                            @RoomType,
                            @Capacity,
                            @Location,
                            @IsActive,
                            @Notes,
                            GETDATE()
                        )";

                    int roomId;
                    using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
                    {
                        AddParameters(cmd, room, false, false);
                        roomId = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    string roomCode = string.Format("ROOM-{0:000}", roomId);
                    using (SqlCommand codeCommand = new SqlCommand(
                        "UPDATE Rooms SET RoomCode = @RoomCode, UpdatedAt = GETDATE() WHERE RoomID = @RoomID",
                        conn, transaction))
                    {
                        codeCommand.Parameters.Add("@RoomCode", SqlDbType.NVarChar, 30).Value = roomCode;
                        codeCommand.Parameters.Add("@RoomID", SqlDbType.Int).Value = roomId;
                        codeCommand.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    return roomId;
                }
            }
        }

        public bool UpdateRoom(Room room)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = @"
                    UPDATE Rooms
                    SET
                        RoomCode = @RoomCode,
                        RoomName = @RoomName,
                        RoomType = @RoomType,
                        Capacity = @Capacity,
                        Location = @Location,
                        IsActive = @IsActive,
                        Notes = @Notes,
                        UpdatedAt = GETDATE()
                    WHERE RoomID = @RoomID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    AddParameters(cmd, room, true, true);
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool DeleteRoom(int roomId)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                const string query = @"
                    UPDATE Rooms
                    SET IsActive = 0,
                        UpdatedAt = GETDATE()
                    WHERE RoomID = @RoomID
                      AND IsActive = 1";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@RoomID", roomId);
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool RoomCodeExists(string roomCode, int excludedRoomId)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                string query = @"
                    SELECT COUNT(*)
                    FROM Rooms
                    WHERE RoomCode = @RoomCode";

                if (excludedRoomId > 0)
                    query += " AND RoomID <> @RoomID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@RoomCode", roomCode.Trim());

                    if (excludedRoomId > 0)
                        cmd.Parameters.AddWithValue("@RoomID", excludedRoomId);

                    conn.Open();
                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
        }

        private void AddParameters(SqlCommand cmd, Room room, bool includeId, bool includeCode)
        {
            if (includeId)
                cmd.Parameters.AddWithValue("@RoomID", room.RoomID);

            if (includeCode)
                cmd.Parameters.AddWithValue("@RoomCode", room.RoomCode.Trim());
            cmd.Parameters.AddWithValue("@RoomName", room.RoomName.Trim());
            cmd.Parameters.AddWithValue("@RoomType", room.RoomType.Trim());
            cmd.Parameters.AddWithValue("@Capacity", room.Capacity);
            cmd.Parameters.AddWithValue("@IsActive", room.IsActive);

            if (string.IsNullOrWhiteSpace(room.Location))
                cmd.Parameters.AddWithValue("@Location", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@Location", room.Location.Trim());

            if (string.IsNullOrWhiteSpace(room.Notes))
                cmd.Parameters.AddWithValue("@Notes", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@Notes", room.Notes.Trim());
        }
    }
}
