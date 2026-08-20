using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using SchoolSystem.Models;
using SchoolSystem.Security;

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
                        u.UserID,
                        u.FullName,
                        u.UserName,
                        u.PasswordHash,
                        u.PasswordSalt,
                        u.RoleName,
                        u.Permissions,
                        u.Email,
                        u.Phone,
                        u.IsActive,
                        u.MustChangePassword,
                        u.FailedLoginAttempts,
                        u.LockedAt,
                        u.LastLoginAt,
                        u.CreatedAt,
                        u.UpdatedAt,
                        (SELECT TOP 1 ur.RoleID FROM dbo.UserRoles ur WHERE ur.UserID = u.UserID) AS RoleId
                    FROM Users u
                    WHERE LTRIM(RTRIM(u.UserName)) = LTRIM(RTRIM(@UserName))";

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

        public List<string> GetRolePermissions(int userId)
        {
            return GetRolePermissionsByRoleId(GetUserRoleId(userId));
        }

        /// <summary>
        /// يُعيد مفاتيح صلاحيات الدور (الشاشات Module.View) من جدول RolePermissions.
        /// </summary>
        public List<string> GetRolePermissionsByRoleId(int roleId)
        {
            List<string> permissions = new List<string>();

            if (roleId <= 0)
                return permissions;

            using (SqlConnection con = DbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT DISTINCT P.PermissionKey
                FROM dbo.RolePermissions RP
                INNER JOIN dbo.Permissions P ON P.PermissionID = RP.PermissionID
                WHERE RP.RoleID = @RoleID
                  AND P.IsActive = 1
                  AND NULLIF(LTRIM(RTRIM(P.PermissionKey)), N'') IS NOT NULL
                ORDER BY P.PermissionKey;", con))
            {
                cmd.Parameters.Add("@RoleID", SqlDbType.Int).Value = roleId;
                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["PermissionKey"] != DBNull.Value)
                        {
                            string key = reader["PermissionKey"].ToString().Trim();
                            if (!string.IsNullOrWhiteSpace(key))
                                permissions.Add(key);
                        }
                    }
                }
            }

            return permissions;
        }

        /// <summary>
        /// يُعيد رقم دور المستخدم من جدول UserRoles (0 إن لم يوجد).
        /// </summary>
        public int GetUserRoleId(int userId)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT TOP 1 RoleID
                FROM dbo.UserRoles
                WHERE UserID = @UserID;", con))
            {
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userId;
                con.Open();
                object result = cmd.ExecuteScalar();
                if (result == null || result == DBNull.Value)
                    return 0;
                int roleId = Convert.ToInt32(result);
                return roleId > 0 ? roleId : 0;
            }
        }

        /// <summary>
        /// يُعيد مفاتيح الشاشات الإضافية للمستخدم (UserPermissions) من جدول UserPermissions.
        /// </summary>
        public List<string> GetUserPermissions(int userId)
        {
            List<string> permissions = new List<string>();

            using (SqlConnection con = DbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT DISTINCT P.PermissionKey
                FROM dbo.UserPermissions UP
                INNER JOIN dbo.Permissions P ON P.PermissionID = UP.PermissionID
                WHERE UP.UserID = @UserID
                  AND P.IsActive = 1
                  AND NULLIF(LTRIM(RTRIM(P.PermissionKey)), N'') IS NOT NULL
                ORDER BY P.PermissionKey;", con))
            {
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userId;
                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["PermissionKey"] != DBNull.Value)
                        {
                            string key = reader["PermissionKey"].ToString().Trim();
                            if (!string.IsNullOrWhiteSpace(key))
                                permissions.Add(key);
                        }
                    }
                }
            }

            return permissions;
        }

        /// <summary>
        /// الصلاحيات الفعالة للمستخدم (الشاشات Module.View):
        /// إذا وُجدت صلاحيات إضافية محفوظة (UserPermissions) تُعدّ هي المرجع النهائي،
        /// وإلا تُعاد صلاحيات دوره (RolePermissions) كقيمة افتراضية.
        /// </summary>
        public List<string> GetEffectivePermissions(int userId)
        {
            List<string> userPermissions = GetUserPermissions(userId);

            List<string> source = userPermissions.Count > 0
                ? userPermissions
                : GetRolePermissionsByRoleId(GetUserRoleId(userId));

            return source
                .Where(key => PermissionKeys.IsScreenPermission(key))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(key => key, StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        /// <summary>
        /// يستبدل صلاحيات المستخدم الإضافية بالكامل داخل معاملة واحدة (حذف + إدراج).
        /// يقبل مفاتيح الشاشات Module.View فقط، ويتجاهل أي مفتاح غير صالح.
        /// </summary>
        public int ReplaceUserPermissions(int userId, IList<string> screenKeys)
        {
            if (userId <= 0)
                return 0;

            List<string> validKeys = new List<string>();
            if (screenKeys != null)
            {
                foreach (string key in screenKeys)
                {
                    string normalized = PermissionKeys.NormalizePermissionKey(key);
                    if (PermissionKeys.IsScreenPermission(normalized))
                        validKeys.Add(normalized);
                }
            }

            using (SqlConnection con = DbConnection.GetConnection())
            {
                con.Open();
                using (SqlTransaction transaction = con.BeginTransaction())
                {
                    using (SqlCommand deleteCommand = new SqlCommand(
                        "DELETE FROM dbo.UserPermissions WHERE UserID = @UserID", con, transaction))
                    {
                        deleteCommand.Parameters.Add("@UserID", SqlDbType.Int).Value = userId;
                        deleteCommand.ExecuteNonQuery();
                    }

                    int inserted = 0;
                    if (validKeys.Count > 0)
                    {
                        using (SqlCommand insertCommand = new SqlCommand(@"
                            INSERT INTO dbo.UserPermissions (UserID, PermissionID, GrantedAt, GrantedBy)
                            SELECT @UserID, P.PermissionID, GETDATE(), @GrantedBy
                            FROM dbo.Permissions P
                            WHERE P.PermissionKey = @PermissionKey
                              AND P.IsActive = 1;", con, transaction))
                        {
                            insertCommand.Parameters.Add("@UserID", SqlDbType.Int).Value = userId;
                            insertCommand.Parameters.Add("@GrantedBy", SqlDbType.Int).Value =
                                CurrentUser.IsLoggedIn && CurrentUser.User != null
                                    ? (object)CurrentUser.User.UserID
                                    : DBNull.Value;
                            SqlParameter keyParameter = insertCommand.Parameters.Add("@PermissionKey", SqlDbType.NVarChar, 150);

                            foreach (string key in validKeys)
                            {
                                keyParameter.Value = key;
                                inserted += insertCommand.ExecuteNonQuery();
                            }
                        }
                    }

                    transaction.Commit();
                    return inserted;
                }
            }
        }

        public User GetUserById(int userId)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                string query = @"
                    SELECT TOP 1
                        u.UserID,
                        u.FullName,
                        u.UserName,
                        u.PasswordHash,
                        u.PasswordSalt,
                        u.RoleName,
                        u.Permissions,
                        u.Email,
                        u.Phone,
                        u.IsActive,
                        u.MustChangePassword,
                        u.FailedLoginAttempts,
                        u.LockedAt,
                        u.LastLoginAt,
                        u.CreatedAt,
                        u.UpdatedAt,
                        (SELECT TOP 1 ur.RoleID FROM dbo.UserRoles ur WHERE ur.UserID = u.UserID) AS RoleId
                    FROM Users u
                    WHERE u.UserID = @UserID";

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
                con.Open();

                using (SqlTransaction transaction = con.BeginTransaction())
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

                    int newUserId;
                    using (SqlCommand cmd = new SqlCommand(query, con, transaction))
                    {
                        AddParameters(cmd, user, true);

                        object result = cmd.ExecuteScalar();
                        if (result == null || result == DBNull.Value)
                        {
                            transaction.Rollback();
                            return false;
                        }

                        newUserId = Convert.ToInt32(result);
                    }

                    if (newUserId > 0)
                        SyncUserRole(con, transaction, newUserId, user.RoleName);

                    if (newUserId > 0)
                        SyncUserPermissions(con, transaction, newUserId, user.Permissions);

                    transaction.Commit();
                    user.UserID = newUserId;
                    return newUserId > 0;
                }
            }
        }

        public bool UpdateUser(User user, bool updatePassword)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            {
                con.Open();

                using (SqlTransaction transaction = con.BeginTransaction())
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

                    bool updated;
                    using (SqlCommand cmd = new SqlCommand(query, con, transaction))
                    {
                        cmd.Parameters.AddWithValue("@UserID", user.UserID);
                        AddParameters(cmd, user, updatePassword);

                        updated = cmd.ExecuteNonQuery() > 0;
                    }

                    if (!updated)
                    {
                        transaction.Rollback();
                        return false;
                    }

                    SyncUserRole(con, transaction, user.UserID, user.RoleName);

                    SyncUserPermissions(con, transaction, user.UserID, user.Permissions);

                    transaction.Commit();
                    return true;
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

                    using (SqlCommand deleteRolesCommand = new SqlCommand(
                        "DELETE FROM dbo.UserRoles WHERE UserID = @UserID", con, transaction))
                    {
                        deleteRolesCommand.Parameters.Add("@UserID", SqlDbType.Int).Value = userId;
                        deleteRolesCommand.ExecuteNonQuery();
                    }

                    using (SqlCommand deleteUserPermissionsCommand = new SqlCommand(
                        "DELETE FROM dbo.UserPermissions WHERE UserID = @UserID", con, transaction))
                    {
                        deleteUserPermissionsCommand.Parameters.Add("@UserID", SqlDbType.Int).Value = userId;
                        deleteUserPermissionsCommand.ExecuteNonQuery();
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
                    AddPermissionsParameter(cmd, permissions);

                    con.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // يحافظ على تطابق الجداول المعيارية (UserRoles) مع الدور المخزن في Users.RoleName
        // حتى تبقى بيانات الاستعادة للمستخدمين القدامى متسقة دائمًا مع الاختيار الحالي.
        private void SyncUserRole(SqlConnection con, SqlTransaction transaction, int userId, string roleName)
        {
            if (userId <= 0)
                return;

            using (SqlCommand deleteCommand = new SqlCommand(
                "DELETE FROM dbo.UserRoles WHERE UserID = @UserID", con, transaction))
            {
                deleteCommand.Parameters.Add("@UserID", SqlDbType.Int).Value = userId;
                deleteCommand.ExecuteNonQuery();
            }

            int roleId = EnsureRole(con, transaction, roleName);
            if (roleId <= 0)
                return;

            using (SqlCommand insertCommand = new SqlCommand(@"
                IF NOT EXISTS (SELECT 1 FROM dbo.UserRoles WHERE UserID = @UserID AND RoleID = @RoleID)
                BEGIN
                    INSERT INTO dbo.UserRoles (UserID, RoleID, AssignedAt)
                    VALUES (@UserID, @RoleID, GETDATE());
                END;", con, transaction))
            {
                insertCommand.Parameters.Add("@UserID", SqlDbType.Int).Value = userId;
                insertCommand.Parameters.Add("@RoleID", SqlDbType.Int).Value = roleId;
                insertCommand.ExecuteNonQuery();
            }
        }

        private int EnsureRole(SqlConnection con, SqlTransaction transaction, string roleName)
        {
            string normalizedRole = string.IsNullOrWhiteSpace(roleName)
                ? string.Empty
                : roleName.Trim();

            if (string.IsNullOrEmpty(normalizedRole))
                return 0;

            using (SqlCommand selectCommand = new SqlCommand(@"
                SELECT TOP 1 RoleID
                FROM dbo.Roles
                WHERE LTRIM(RTRIM(RoleName)) = @RoleName", con, transaction))
            {
                selectCommand.Parameters.Add("@RoleName", SqlDbType.NVarChar, 100).Value = normalizedRole;
                object existing = selectCommand.ExecuteScalar();
                if (existing != null && existing != DBNull.Value)
                    return Convert.ToInt32(existing);
            }

            using (SqlCommand insertCommand = new SqlCommand(@"
                INSERT INTO dbo.Roles (RoleName, IsSystemRole, IsActive, CreatedAt)
                OUTPUT INSERTED.RoleID
                VALUES (@RoleName, 0, 1, GETDATE());", con, transaction))
            {
                insertCommand.Parameters.Add("@RoleName", SqlDbType.NVarChar, 100).Value = normalizedRole;
                object inserted = insertCommand.ExecuteScalar();
                return inserted != null && inserted != DBNull.Value ? Convert.ToInt32(inserted) : 0;
            }
        }

        // يزامن جدول UserPermissions (الصلاحيات الإضافية للمستخدم فوق صلاحيات دوره)
        // مع الصلاحيات المختارة في واجهة المستخدمين. يقبل مفاتيح الشاشات Module.View فقط.
        private void SyncUserPermissions(SqlConnection con, SqlTransaction transaction, int userId, string permissions)
        {
            if (userId <= 0)
                return;

            IReadOnlyList<string> screenKeys = PermissionKeys.GetScreenKeysFromPermissions(permissions);

            using (SqlCommand deleteCommand = new SqlCommand(
                "DELETE FROM dbo.UserPermissions WHERE UserID = @UserID", con, transaction))
            {
                deleteCommand.Parameters.Add("@UserID", SqlDbType.Int).Value = userId;
                deleteCommand.ExecuteNonQuery();
            }

            if (screenKeys.Count == 0)
                return;

            using (SqlCommand insertCommand = new SqlCommand(@"
                INSERT INTO dbo.UserPermissions (UserID, PermissionID, GrantedAt, GrantedBy)
                SELECT @UserID, P.PermissionID, GETDATE(), @GrantedBy
                FROM dbo.Permissions P
                WHERE P.PermissionKey = @PermissionKey
                  AND P.IsActive = 1;", con, transaction))
            {
                insertCommand.Parameters.Add("@UserID", SqlDbType.Int).Value = userId;
                insertCommand.Parameters.Add("@GrantedBy", SqlDbType.Int).Value =
                    CurrentUser.IsLoggedIn && CurrentUser.User != null
                        ? (object)CurrentUser.User.UserID
                        : DBNull.Value;
                SqlParameter keyParameter = insertCommand.Parameters.Add("@PermissionKey", SqlDbType.NVarChar, 150);

                foreach (string key in screenKeys)
                {
                    keyParameter.Value = key;
                    insertCommand.ExecuteNonQuery();
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

        public bool ChangePassword(int userId, string passwordHash, string passwordSalt)
        {
            using (SqlConnection con = DbConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(@"
                UPDATE Users SET
                    PasswordHash = @PasswordHash,
                    PasswordSalt = @PasswordSalt,
                    MustChangePassword = 0,
                    UpdatedAt = GETDATE()
                WHERE UserID = @UserID AND IsActive = 1", con))
            {
                cmd.Parameters.AddWithValue("@UserID", userId);
                cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
                cmd.Parameters.AddWithValue("@PasswordSalt", passwordSalt);
                con.Open();
                return cmd.ExecuteNonQuery() > 0;
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
                        MustChangePassword = CASE
                            WHEN LTRIM(RTRIM(RoleName)) IN (N'مدير النظام', N'Admin', N'Administrator') THEN 0
                            ELSE 1
                        END,
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
            AddPermissionsParameter(cmd, user.Permissions);

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

        private void AddPermissionsParameter(SqlCommand cmd, string permissions)
        {
            SqlParameter parameter = cmd.Parameters.Add("@Permissions", SqlDbType.NVarChar, -1);
            // القيمة الفارغة هنا اختيار يدوي من زر "إلغاء كل الصلاحيات".
            // لا نحولها إلى NULL لأن NULL تعني حسابًا قديمًا يحتاج استعادة صلاحيات الدور.
            parameter.Value = string.IsNullOrWhiteSpace(permissions)
                ? string.Empty
                : permissions.Trim();
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
            // NULL تعني سجلًا قديمًا لم يُحسم تخصيصه بعد، بينما النص الفارغ
            // يعني أن المدير ضغط منع كل الصلاحيات عمدًا.
            user.Permissions = reader["Permissions"] == DBNull.Value ? null : reader["Permissions"].ToString();

            user.RoleId = reader["RoleId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["RoleId"]);

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
