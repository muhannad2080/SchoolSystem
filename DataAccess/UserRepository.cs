using System;
using System.Data;
using System.Collections.Generic;
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
                        FailedLoginAttempts,
                        LockedAt,
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
                        FailedLoginAttempts,
                        LockedAt,
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
                        FailedLoginAttempts,
                        LockedAt,
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
                        MustChangePassword,
                        FailedLoginAttempts,
                        LockedAt
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
                        @MustChangePassword,
                        0,
                        NULL
                    );
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    AddParameters(cmd, user, true);

                    con.Open();
                    object result = cmd.ExecuteScalar();
                    if (result == null || result == DBNull.Value)
                        return false;
                    user.UserID = Convert.ToInt32(result);
                    return user.UserID > 0;
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
                            FailedLoginAttempts = CASE WHEN @IsActive = 1 THEN 0 ELSE FailedLoginAttempts END,
                            LockedAt = CASE WHEN @IsActive = 1 THEN NULL ELSE LockedAt END,
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
                            FailedLoginAttempts = CASE WHEN @IsActive = 1 THEN 0 ELSE FailedLoginAttempts END,
                            LockedAt = CASE WHEN @IsActive = 1 THEN NULL ELSE LockedAt END,
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
            return DeleteUser(userId, 0);
        }

        public bool DeleteUser(int userId, int protectedUserId)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                con.Open();
                using (SqlTransaction transaction = con.BeginTransaction(IsolationLevel.Serializable))
                {
                    const string targetQuery = @"
                        SELECT RoleName, IsActive
                        FROM Users
                        WHERE UserID = @UserID";

                    string targetRole;
                    bool targetIsActive;
                    using (SqlCommand targetCommand = new SqlCommand(targetQuery, con, transaction))
                    {
                        targetCommand.Parameters.Add("@UserID", SqlDbType.Int).Value = userId;
                        using (SqlDataReader reader = targetCommand.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                transaction.Rollback();
                                return false;
                            }

                            targetRole = reader.IsDBNull(0) ? string.Empty : reader.GetString(0);
                            targetIsActive = !reader.IsDBNull(1) && reader.GetBoolean(1);
                        }
                    }

                    if (protectedUserId > 0 && userId == protectedUserId)
                    {
                        transaction.Rollback();
                        throw new InvalidOperationException("لا يمكن حذف المستخدم المسجل دخوله حالياً.");
                    }

                    if (targetIsActive && IsAdministratorRole(targetRole))
                    {
                        const string adminCountQuery = @"
                            SELECT COUNT(1)
                            FROM Users
                            WHERE IsActive = 1
                              AND LTRIM(RTRIM(RoleName)) IN (N'مدير النظام', N'Admin', N'Administrator')";

                        using (SqlCommand adminCountCommand = new SqlCommand(adminCountQuery, con, transaction))
                        {
                            int activeAdminCount = Convert.ToInt32(adminCountCommand.ExecuteScalar());
                            if (activeAdminCount <= 1)
                            {
                                transaction.Rollback();
                                throw new InvalidOperationException("لا يمكن حذف آخر مدير نظام نشط.");
                            }
                        }
                    }

                    using (SqlCommand deleteCommand = new SqlCommand(
                        "DELETE FROM Users WHERE UserID = @UserID", con, transaction))
                    {
                        deleteCommand.Parameters.Add("@UserID", SqlDbType.Int).Value = userId;
                        bool deleted = deleteCommand.ExecuteNonQuery() > 0;
                        transaction.Commit();
                        return deleted;
                    }
                }
            }
        }

        private bool IsAdministratorRole(string roleName)
        {
            string normalized = (roleName ?? string.Empty).Trim();
            return normalized.Equals("مدير النظام", StringComparison.OrdinalIgnoreCase)
                || normalized.Equals("Admin", StringComparison.OrdinalIgnoreCase)
                || normalized.Equals("Administrator", StringComparison.OrdinalIgnoreCase);
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
                    WHERE LTRIM(RTRIM(RoleName)) IN (N'مدير النظام', N'Admin', N'Administrator')
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

        public bool UpdatePermissions(int userId, string permissions)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                const string query = @"
                    UPDATE Users SET
                        Permissions = @Permissions,
                        UpdatedAt = GETDATE()
                    WHERE UserID = @UserID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    cmd.Parameters.AddWithValue("@Permissions", string.IsNullOrWhiteSpace(permissions) ? (object)DBNull.Value : permissions);

                    con.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public List<string> GetSystemAdministratorEmails()
        {
            List<string> emails = new List<string>();
            using (SqlConnection con = DbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT Email
                FROM Users
                WHERE IsActive = 1
                  AND LTRIM(RTRIM(RoleName)) IN (N'مدير النظام', N'Admin', N'Administrator')
                  AND Email IS NOT NULL
                  AND LTRIM(RTRIM(Email)) <> N''", con))
            {
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["Email"] != DBNull.Value)
                            emails.Add(reader["Email"].ToString().Trim());
                    }
                }
            }
            return emails;
        }

        public int RegisterFailedLoginAttempt(int userId)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(@"
                UPDATE Users
                SET FailedLoginAttempts = CASE
                        WHEN ISNULL(FailedLoginAttempts, 0) + 1 >= 3 THEN 3
                        ELSE ISNULL(FailedLoginAttempts, 0) + 1
                    END,
                    IsActive = CASE
                        WHEN ISNULL(FailedLoginAttempts, 0) + 1 >= 3
                             AND LTRIM(RTRIM(RoleName)) NOT IN (N'مدير النظام', N'Admin', N'Administrator')
                        THEN 0
                        ELSE IsActive
                    END,
                    LockedAt = CASE
                        WHEN ISNULL(FailedLoginAttempts, 0) + 1 >= 3
                             AND LTRIM(RTRIM(RoleName)) NOT IN (N'مدير النظام', N'Admin', N'Administrator')
                        THEN ISNULL(LockedAt, GETDATE())
                        ELSE LockedAt
                    END,
                    UpdatedAt = GETDATE()
                WHERE UserID = @UserID;
                SELECT ISNULL(FailedLoginAttempts, 0) FROM Users WHERE UserID = @UserID;", con))
            {
                cmd.Parameters.AddWithValue("@UserID", userId);
                con.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public void RecordSuccessfulLogin(int userId)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(@"
                UPDATE Users SET
                    FailedLoginAttempts = 0,
                    LockedAt = NULL,
                    LastLoginAt = GETDATE(),
                    UpdatedAt = GETDATE()
                WHERE UserID = @UserID", con))
            {
                cmd.Parameters.AddWithValue("@UserID", userId);
                con.Open();
                cmd.ExecuteNonQuery();
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
                        FailedLoginAttempts = 0,
                        LockedAt = NULL,
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

            user.FailedLoginAttempts = reader["FailedLoginAttempts"] == DBNull.Value ? 0 : Convert.ToInt32(reader["FailedLoginAttempts"]);

            if (reader["LockedAt"] != DBNull.Value)
                user.LockedAt = Convert.ToDateTime(reader["LockedAt"]);

            if (reader["LastLoginAt"] != DBNull.Value)
                user.LastLoginAt = Convert.ToDateTime(reader["LastLoginAt"]);

            user.CreatedAt = reader["CreatedAt"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["CreatedAt"]);

            if (reader["UpdatedAt"] != DBNull.Value)
                user.UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"]);

            return user;
        }
    }
}
