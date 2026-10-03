using System;
using System.Data;
using System.Data.SqlClient;
using SchoolSystem.Models;


namespace SchoolSystem.DataAccess
{

    public class BusRouteRepository
    {
        // تعريف الدالة GetAllRoutes لتنفيذ العملية المرتبطة بها.
        public DataTable GetAllRoutes()
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string query = @"
                    SELECT
                        r.RouteID,
                        r.RouteName,
                        r.BusID,
                        b.BusNumber,
                        r.StartPoint,
                        r.EndPoint,
                        r.DepartureTime,
                        r.ArrivalTime,
                        r.Fee,
                        r.Notes,
                        r.CreatedAt,
                        r.UpdatedAt
                    FROM BusRoutes r
                    INNER JOIN Buses b ON r.BusID = b.BusID
                    ORDER BY r.RouteID DESC";

                using (SqlDataAdapter da = new SqlDataAdapter(query, con))
                {
                    DataTable dt = new DataTable();

                    da.Fill(dt);

                    return dt;
                }
            }
        }

        // تعريف الدالة AddRoute لتنفيذ العملية المرتبطة بها.
        public bool AddRoute(BusRoute route)
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
                            THROW 51010, N'الحافلة المحددة غير موجودة.', 1;

                        IF @DepartureTime IS NOT NULL
                           AND @ArrivalTime IS NOT NULL
                           AND @DepartureTime >= @ArrivalTime
                            THROW 51011, N'وقت المغادرة يجب أن يكون قبل وقت الوصول.', 1;

                        IF EXISTS
                        (
                            SELECT 1
                            FROM dbo.BusRoutes WITH (UPDLOCK, HOLDLOCK)
                            WHERE BusID = @BusID
                              AND LTRIM(RTRIM(RouteName)) = LTRIM(RTRIM(@RouteName))
                        )
                            THROW 51012, N'اسم المسار مستخدم مسبقاً لهذه الحافلة.', 1;";

                    using (SqlCommand check = new SqlCommand(validation, con, transaction))
                    {
                        AddParameters(check, route);
                        check.ExecuteNonQuery();
                    }

                    const string query = @"
                        INSERT INTO BusRoutes
                        (
                            RouteName,
                            BusID,
                            StartPoint,
                            EndPoint,
                            DepartureTime,
                            ArrivalTime,
                            Fee,
                            Notes
                        )
                        VALUES
                        (
                            @RouteName,
                            @BusID,
                            @StartPoint,
                            @EndPoint,
                            @DepartureTime,
                            @ArrivalTime,
                            @Fee,
                            @Notes
                        )";

                    using (SqlCommand cmd = new SqlCommand(query, con, transaction))
                    {
                        AddParameters(cmd, route);
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

        // تعريف الدالة UpdateRoute لتنفيذ العملية المرتبطة بها.
        public bool UpdateRoute(BusRoute route)
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
                            FROM dbo.BusRoutes WITH (UPDLOCK, HOLDLOCK)
                            WHERE RouteID = @RouteID
                        )
                            THROW 51013, N'المسار المحدد غير موجود.', 1;

                        IF NOT EXISTS
                        (
                            SELECT 1
                            FROM dbo.Buses WITH (UPDLOCK, HOLDLOCK)
                            WHERE BusID = @BusID
                        )
                            THROW 51014, N'الحافلة المحددة غير موجودة.', 1;

                        IF @DepartureTime IS NOT NULL
                           AND @ArrivalTime IS NOT NULL
                           AND @DepartureTime >= @ArrivalTime
                            THROW 51015, N'وقت المغادرة يجب أن يكون قبل وقت الوصول.', 1;

                        IF EXISTS
                        (
                            SELECT 1
                            FROM dbo.BusRoutes WITH (UPDLOCK, HOLDLOCK)
                            WHERE BusID = @BusID
                              AND RouteID <> @RouteID
                              AND LTRIM(RTRIM(RouteName)) = LTRIM(RTRIM(@RouteName))
                        )
                            THROW 51016, N'اسم المسار مستخدم مسبقاً لهذه الحافلة.', 1;";

                    using (SqlCommand check = new SqlCommand(validation, con, transaction))
                    {
                        check.Parameters.Add("@RouteID", SqlDbType.Int).Value = route.RouteID;
                        AddParameters(check, route);
                        check.ExecuteNonQuery();
                    }

                    const string query = @"
                        UPDATE BusRoutes SET
                            RouteName = @RouteName,
                            BusID = @BusID,
                            StartPoint = @StartPoint,
                            EndPoint = @EndPoint,
                            DepartureTime = @DepartureTime,
                            ArrivalTime = @ArrivalTime,
                            Fee = @Fee,
                            Notes = @Notes,
                            UpdatedAt = GETDATE()
                        WHERE RouteID = @RouteID";

                    using (SqlCommand cmd = new SqlCommand(query, con, transaction))
                    {
                        cmd.Parameters.Add("@RouteID", SqlDbType.Int).Value = route.RouteID;
                        AddParameters(cmd, route);

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

        // تعريف الدالة DeleteRoute لتنفيذ العملية المرتبطة بها.
        public bool DeleteRoute(int routeId)
        {
            if (routeId <= 0)
                throw new ArgumentException("رقم المسار غير صحيح.");

            using (SqlConnection con = DbConnection.GetConnection())
            {
                con.Open();
                using (SqlTransaction transaction = con.BeginTransaction(IsolationLevel.Serializable))
                {
                    const string validation = @"
                        IF NOT EXISTS
                        (
                            SELECT 1
                            FROM dbo.BusRoutes WITH (UPDLOCK, HOLDLOCK)
                            WHERE RouteID = @RouteID
                        )
                            THROW 51017, N'المسار المحدد غير موجود.', 1;

                        IF EXISTS
                        (
                            SELECT 1
                            FROM dbo.StudentTransportAssignments WITH (UPDLOCK, HOLDLOCK)
                            WHERE RouteID = @RouteID
                              AND IsActive = 1
                        )
                            THROW 51018, N'لا يمكن حذف المسار لأنه مرتبط بطلاب نشطين في النقل. ألغِ توزيعات الطلاب أولاً.', 1;";

                    using (SqlCommand check = new SqlCommand(validation, con, transaction))
                    {
                        check.Parameters.Add("@RouteID", SqlDbType.Int).Value = routeId;
                        check.ExecuteNonQuery();
                    }

                    using (SqlCommand delete = new SqlCommand(
                        "DELETE FROM dbo.BusRoutes WHERE RouteID = @RouteID;", con, transaction))
                    {
                        delete.Parameters.Add("@RouteID", SqlDbType.Int).Value = routeId;
                        if (delete.ExecuteNonQuery() == 0)
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

        // تعريف الدالة AddParameters لتنفيذ العملية المرتبطة بها.
        private void AddParameters(SqlCommand cmd, BusRoute route)
        {

            cmd.Parameters.AddWithValue("@RouteName", route.RouteName ?? "");

            cmd.Parameters.AddWithValue("@BusID", route.BusID);


            cmd.Parameters.AddWithValue(

                "@StartPoint",
                string.IsNullOrWhiteSpace(route.StartPoint) ? (object)DBNull.Value : route.StartPoint
            );


            cmd.Parameters.AddWithValue(

                "@EndPoint",
                string.IsNullOrWhiteSpace(route.EndPoint) ? (object)DBNull.Value : route.EndPoint
            );

            if (route.DepartureTime.HasValue)
                cmd.Parameters.AddWithValue("@DepartureTime", route.DepartureTime.Value);

            else

                cmd.Parameters.AddWithValue("@DepartureTime", DBNull.Value);


            if (route.ArrivalTime.HasValue)
                cmd.Parameters.AddWithValue("@ArrivalTime", route.ArrivalTime.Value);

            else

                cmd.Parameters.AddWithValue("@ArrivalTime", DBNull.Value);


            if (route.Fee.HasValue)
                cmd.Parameters.AddWithValue("@Fee", route.Fee.Value);

            else

                cmd.Parameters.AddWithValue("@Fee", DBNull.Value);


            cmd.Parameters.AddWithValue(

                "@Notes",
                string.IsNullOrWhiteSpace(route.Notes) ? (object)DBNull.Value : route.Notes
            );
        }
    }
}
