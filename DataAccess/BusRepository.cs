using System;
using System.Data;
using System.Data.SqlClient;
using SchoolSystem.Models;


namespace SchoolSystem.DataAccess
{

    public class BusRepository
    {
        // تعريف الدالة GetAllBuses لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة AddBus لتنفيذ العملية المرتبطة بها.
        public bool AddBus(Bus bus)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                con.Open();
                using (SqlTransaction transaction = con.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        const string validation = @"
                            IF EXISTS
                            (
                                SELECT 1
                                FROM dbo.Buses WITH (UPDLOCK, HOLDLOCK)
                                WHERE LTRIM(RTRIM(BusNumber)) = LTRIM(RTRIM(@BusNumber))
                            )
                                THROW 51004, N'رقم الحافلة مستخدم مسبقاً.', 1;";

                        using (SqlCommand check = new SqlCommand(validation, con, transaction))
                        {
                            check.Parameters.Add("@BusNumber", SqlDbType.NVarChar, 50).Value = bus.BusNumber.Trim();
                            check.ExecuteNonQuery();
                        }

                        const string query = @"
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
                            );";

                        using (SqlCommand cmd = new SqlCommand(query, con, transaction))
                        {
                            AddParameters(cmd, bus);
                            if (cmd.ExecuteNonQuery() == 0)
                            {
                                transaction.Rollback();
                                return false;
                            }
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        SchoolSystem.Helpers.ApplicationLogger.LogException("مستودع " + nameof(BusRepository) + ": إضافة حافلة", ex);
                        throw;
                    }
                }
            }
        }

        // تعريف الدالة UpdateBus لتنفيذ العملية المرتبطة بها.
        public bool UpdateBus(Bus bus)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                con.Open();
                using (SqlTransaction transaction = con.BeginTransaction(IsolationLevel.Serializable))
                {
                    const string validation = @"
                        IF NOT EXISTS
                        (
                            SELECT 1
                            FROM dbo.Buses WITH (UPDLOCK, HOLDLOCK)
                            WHERE BusID = @BusID
                        )
                            THROW 51002, N'الحافلة المحددة غير موجودة.', 1;

                        IF EXISTS
                        (
                            SELECT 1
                            FROM dbo.BusRoutes r WITH (UPDLOCK, HOLDLOCK)
                            INNER JOIN dbo.StudentTransportAssignments a WITH (UPDLOCK, HOLDLOCK)
                                ON a.RouteID = r.RouteID
                               AND a.IsActive = 1
                            WHERE r.BusID = @BusID
                            GROUP BY r.BusID
                            HAVING COUNT(*) > @Capacity
                        )
                            THROW 51003, N'لا يمكن تقليل سعة الحافلة إلى أقل من عدد الطلاب المسجلين عليها حالياً.', 1;";

                    using (SqlCommand check = new SqlCommand(validation, con, transaction))
                    {
                        check.Parameters.Add("@BusID", SqlDbType.Int).Value = bus.BusID;
                        check.Parameters.Add("@Capacity", SqlDbType.Int).Value = bus.Capacity;
                        check.ExecuteNonQuery();
                    }

                    const string query = @"
                        UPDATE Buses SET
                            BusNumber = @BusNumber,
                            DriverName = @DriverName,
                            DriverPhone = @DriverPhone,
                            Capacity = @Capacity,
                            Notes = @Notes,
                            UpdatedAt = GETDATE()
                        WHERE BusID = @BusID";

                    using (SqlCommand cmd = new SqlCommand(query, con, transaction))
                    {
                        cmd.Parameters.Add("@BusID", SqlDbType.Int).Value = bus.BusID;
                        AddParameters(cmd, bus);

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

        // تعريف الدالة DeleteBus لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة BusNumberExists لتنفيذ العملية المرتبطة بها.
        public bool BusNumberExists(string busNumber)
        {

            return BusNumberExists(busNumber, 0);
        }

        // تعريف الدالة BusNumberExists لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة AddParameters لتنفيذ العملية المرتبطة بها.
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
