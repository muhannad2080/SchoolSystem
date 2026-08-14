using System;
using System.Data;
using System.Data.SqlClient;
using SchoolSystem.Models;

namespace SchoolSystem.DataAccess
{
    public class BusRepository
    {
        public DataTable GetAllBuses()
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string query = @"
                    SELECT
                        BusID,
                        BusNumber,
                        DriverName,
                        DriverPhone,
                        Capacity,
                        Notes,
                        CreatedAt,
                        UpdatedAt
                    FROM Buses
                    ORDER BY BusID DESC";

                using (SqlDataAdapter da = new SqlDataAdapter(query, con))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        public bool AddBus(Bus bus)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string query = @"
                    INSERT INTO Buses
                    (
                        BusNumber,
                        DriverName,
                        DriverPhone,
                        Capacity,
                        Notes
                    )
                    VALUES
                    (
                        @BusNumber,
                        @DriverName,
                        @DriverPhone,
                        @Capacity,
                        @Notes
                    )";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    AddParameters(cmd, bus);

                    con.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool UpdateBus(Bus bus)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string query = @"
                    UPDATE Buses SET
                        BusNumber = @BusNumber,
                        DriverName = @DriverName,
                        DriverPhone = @DriverPhone,
                        Capacity = @Capacity,
                        Notes = @Notes,
                        UpdatedAt = GETDATE()
                    WHERE BusID = @BusID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@BusID", bus.BusID);
                    AddParameters(cmd, bus);

                    con.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool DeleteBus(int busId)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                const string query = @"
                    IF EXISTS (SELECT 1 FROM BusRoutes WHERE BusID = @BusID)
                        THROW 51001, N'لا يمكن حذف الحافلة لأنها مرتبطة بمسار نقل. احذف ارتباطات المسارات أولاً.', 1;

                    DELETE FROM Buses
                    WHERE BusID = @BusID;";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@BusID", busId);

                    con.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool BusNumberExists(string busNumber)
        {
            return BusNumberExists(busNumber, 0);
        }

        public bool BusNumberExists(string busNumber, int excludedBusId)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string query = @"
                    SELECT COUNT(*)
                    FROM Buses
                    WHERE BusNumber = @BusNumber";

                if (excludedBusId > 0)
                    query += " AND BusID <> @BusID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@BusNumber", busNumber);

                    if (excludedBusId > 0)
                        cmd.Parameters.AddWithValue("@BusID", excludedBusId);

                    con.Open();

                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        private void AddParameters(SqlCommand cmd, Bus bus)
        {
            cmd.Parameters.AddWithValue("@BusNumber", bus.BusNumber ?? "");

            cmd.Parameters.AddWithValue(
                "@DriverName",
                string.IsNullOrWhiteSpace(bus.DriverName) ? (object)DBNull.Value : bus.DriverName
            );

            cmd.Parameters.AddWithValue(
                "@DriverPhone",
                string.IsNullOrWhiteSpace(bus.DriverPhone) ? (object)DBNull.Value : bus.DriverPhone
            );

            cmd.Parameters.AddWithValue("@Capacity", bus.Capacity);

            cmd.Parameters.AddWithValue(
                "@Notes",
                string.IsNullOrWhiteSpace(bus.Notes) ? (object)DBNull.Value : bus.Notes
            );
        }
    }
}
