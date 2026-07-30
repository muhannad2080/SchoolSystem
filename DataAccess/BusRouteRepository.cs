using System;
using System.Data;
using System.Data.SqlClient;
using SchoolSystem.Models;

namespace SchoolSystem.DataAccess
{
    public class BusRouteRepository
    {
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

        public bool AddRoute(BusRoute route)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string query = @"
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

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    AddParameters(cmd, route);

                    con.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool UpdateRoute(BusRoute route)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string query = @"
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

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@RouteID", route.RouteID);
                    AddParameters(cmd, route);

                    con.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool DeleteRoute(int routeId)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string query = "DELETE FROM BusRoutes WHERE RouteID = @RouteID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@RouteID", routeId);

                    con.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

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
