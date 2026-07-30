using System;
using System.Data;
using System.Data.SqlClient;
using SchoolSystem.Models;

namespace SchoolSystem.DataAccess
{
    public class UserRepository
    {
        public DataTable GetAllUsers()
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string query = @"
                    SELECT
                        UserID,
                        FullName,
                        UserName,
                        RoleName,
                        Permissions,
                        Email,
                        Phone,
                        IsActive,
                        MustChangePassword,
                        LastLoginAt,
                        CreatedAt,
                        UpdatedAt
                    FROM Users
                    ORDER BY UserID DESC";

                using (SqlDataAdapter da = new SqlDataAdapter(query, con))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        public User GetUserByUserName(string userName)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string query = @"
                    SELECT TOP 1
                        UserID,
                        FullName,
                        UserName,
                        PasswordHash,
                        PasswordSalt,
                        RoleName,
                        Permissions,
                        Email,
                        Phone,
                        IsActive,
                        MustChangePassword,
                        LastLoginAt,
                        CreatedAt,
                        UpdatedAt
                    FROM Users
                    WHERE LTRIM(RTRIM(UserName)) = LTRIM(RTRIM(@UserName))";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UserName", userName);

                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            return MapUser(reader);
                    }
                }
            }

            return null;
        }

        public User GetUserById(int userId)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string query = @"
                    SELECT TOP 1
                        UserID,
                        FullName,
                        UserName,
                        PasswordHash,
                        PasswordSalt,
                        RoleName,
                        Permissions,
                        Email,
                        Phone,
                        IsActive,
                        MustChangePassword,
                        LastLoginAt,
                        CreatedAt,
                        UpdatedAt
                    FROM Users
                    WHERE UserID = @UserID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);

                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            return MapUser(reader);
                    }
                }
            }

            return null;
        }

        public bool AddUser(User user)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string query = @"
                    INSERT INTO Users
                    (
                        FullName,
                        UserName,
                        PasswordHash,
                        PasswordSalt,
                        RoleName,
                        Permissions,
                        Email,
                        Phone,
                        IsActive,
                        MustChangePassword
                    )
                    VALUES
                    (
                        @FullName,
                        @UserName,
                        @PasswordHash,
                        @PasswordSalt,
                        @RoleName,
                        @Permissions,
                        @Email,
                        @Phone,
                        @IsActive,
                        @MustChangePassword
                    )";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    AddParameters(cmd, user, true);

                    con.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool UpdateUser(User user, bool updatePassword)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string query;

                if (updatePassword)
                {
                    query = @"
                        UPDATE Users SET
                            FullName = @FullName,
                            UserName = @UserName,
                            PasswordHash = @PasswordHash,
                            PasswordSalt = @PasswordSalt,
                            RoleName = @RoleName,
                            Permissions = @Permissions,
                            Email = @Email,
                            Phone = @Phone,
                            IsActive = @IsActive,
                            MustChangePassword = @MustChangePassword,
                            UpdatedAt = GETDATE()
                        WHERE UserID = @UserID";
                }
                else
                {
                    query = @"
                        UPDATE Users SET
                            FullName = @FullName,
                            UserName = @UserName,
                            RoleName = @RoleName,
                            Permissions = @Permissions,
                            Email = @Email,
                            Phone = @Phone,
                            IsActive = @IsActive,
                            MustChangePassword = @MustChangePassword,
                            UpdatedAt = GETDATE()
                        WHERE UserID = @UserID";
                }

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UserID", user.UserID);
                    AddParameters(cmd, user, updatePassword);

                    con.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool DeleteUser(int userId)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string query = "DELETE FROM Users WHERE UserID = @UserID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);

                    con.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool UserNameExists(string userName)
        {
            return UserNameExists(userName, 0);
        }

        public bool UserNameExists(string userName, int excludedUserId)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string query = @"
                    SELECT COUNT(*)
                    FROM Users
                    WHERE LTRIM(RTRIM(UserName)) = LTRIM(RTRIM(@UserName))";

                if (excludedUserId > 0)
                    query += " AND UserID <> @UserID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UserName", userName);

                    if (excludedUserId > 0)
                        cmd.Parameters.AddWithValue("@UserID", excludedUserId);

                    con.Open();

                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        public int CountAdmins()
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string query = @"
                    SELECT COUNT(*)
                    FROM Users
                    WHERE RoleName = N'مدير النظام'
                      AND IsActive = 1";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public int CountUsers()
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string query = "SELECT COUNT(*) FROM Users";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public void UpdateLastLogin(int userId)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string query = @"
                    UPDATE Users SET
                        LastLoginAt = GETDATE()
                    WHERE UserID = @UserID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public bool ResetPasswordByUserName(string userName, string passwordHash, string passwordSalt)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string query = @"
                    UPDATE Users SET
                        PasswordHash = @PasswordHash,
                        PasswordSalt = @PasswordSalt,
                        IsActive = 1,
                        MustChangePassword = 0,
                        UpdatedAt = GETDATE()
                    WHERE LTRIM(RTRIM(UserName)) = LTRIM(RTRIM(@UserName))";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UserName", userName);
                    cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
                    cmd.Parameters.AddWithValue("@PasswordSalt", passwordSalt);

                    con.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        private void AddParameters(SqlCommand cmd, User user, bool includePassword)
        {
            cmd.Parameters.AddWithValue("@FullName", user.FullName ?? "");
            cmd.Parameters.AddWithValue("@UserName", user.UserName ?? "");

            if (includePassword)
            {
                cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash ?? "");
                cmd.Parameters.AddWithValue("@PasswordSalt", user.PasswordSalt ?? "");
            }

            cmd.Parameters.AddWithValue("@RoleName", user.RoleName ?? "");
            cmd.Parameters.AddWithValue(
                "@Permissions",
                string.IsNullOrWhiteSpace(user.Permissions) ? (object)DBNull.Value : user.Permissions
            );

            cmd.Parameters.AddWithValue(
                "@Email",
                string.IsNullOrWhiteSpace(user.Email) ? (object)DBNull.Value : user.Email
            );

            cmd.Parameters.AddWithValue(
                "@Phone",
                string.IsNullOrWhiteSpace(user.Phone) ? (object)DBNull.Value : user.Phone
            );

            cmd.Parameters.AddWithValue("@IsActive", user.IsActive);
            cmd.Parameters.AddWithValue("@MustChangePassword", user.MustChangePassword);
        }

        private User MapUser(SqlDataReader reader)
        {
            User user = new User();

            user.UserID = Convert.ToInt32(reader["UserID"]);
            user.FullName = reader["FullName"] == DBNull.Value ? "" : reader["FullName"].ToString();
            user.UserName = reader["UserName"] == DBNull.Value ? "" : reader["UserName"].ToString();

            user.PasswordHash = reader["PasswordHash"] == DBNull.Value ? "" : reader["PasswordHash"].ToString();
            user.PasswordSalt = reader["PasswordSalt"] == DBNull.Value ? "" : reader["PasswordSalt"].ToString();

            user.RoleName = reader["RoleName"] == DBNull.Value ? "" : reader["RoleName"].ToString();
            user.Permissions = reader["Permissions"] == DBNull.Value ? "" : reader["Permissions"].ToString();

            user.Email = reader["Email"] == DBNull.Value ? "" : reader["Email"].ToString();
            user.Phone = reader["Phone"] == DBNull.Value ? "" : reader["Phone"].ToString();

            user.IsActive = reader["IsActive"] != DBNull.Value && Convert.ToBoolean(reader["IsActive"]);
            user.MustChangePassword = reader["MustChangePassword"] != DBNull.Value && Convert.ToBoolean(reader["MustChangePassword"]);

            if (reader["LastLoginAt"] != DBNull.Value)
                user.LastLoginAt = Convert.ToDateTime(reader["LastLoginAt"]);

            user.CreatedAt = reader["CreatedAt"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["CreatedAt"]);

            if (reader["UpdatedAt"] != DBNull.Value)
                user.UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"]);

            return user;
        }
    }
}
