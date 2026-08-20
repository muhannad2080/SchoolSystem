using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using SchoolSystem.DataAccess;
using SchoolSystem.Helpers;
using SchoolSystem.Models;
using SchoolSystem.Security;

namespace SchoolSystem.Services
{
    public class UserService
    {
        private readonly UserRepository userRepository;
        private readonly AuditLogService auditLogService = new AuditLogService();

        public UserService()
        {
            userRepository = new UserRepository();
        }

        public DataTable GetAllUsers()
        {
            CurrentUser.DemandAny("ليس لديك صلاحية عرض المستخدمين.", PermissionKeys.UsersView, PermissionKeys.UsersManage);
            return userRepository.GetAllUsers();
        }

        public bool AddUser(User user, string password)
        {
            // السماح بالتهيئة الأولى فقط عندما لا يوجد أي حساب.
            // بعد التهيئة يلزم مفتاح الإضافة الصريح أو المفتاح القديم للتوافق.
            if (userRepository.CountUsers() > 0)
                CurrentUser.DemandAny("ليس لديك صلاحية إضافة المستخدمين.", PermissionKeys.UsersAdd, PermissionKeys.UsersManage);

            NormalizeUser(user);

            if (PermissionKeys.IsSystemAdministratorRole(user.RoleName) &&
                userRepository.CountUsers() > 0 && !CurrentUser.IsAdmin())
                throw new UnauthorizedAccessException("لا يمكن إلا لمدير النظام إنشاء حساب مدير نظام.");

            if (PermissionKeys.IsSystemAdministratorRole(user.RoleName))
            {
                user.Permissions = PermissionKeys.GetRoleDefaults(user.RoleName);
                user.MustChangePassword = false;
            }
            else
            {
                // كل حساب غير إداري جديد يغيّر كلمة المرور التي أنشأها المدير عند أول دخول.
                user.MustChangePassword = true;
            }

            ValidateUser(user, false);

            password = NormalizeDigits(password);

            if (string.IsNullOrWhiteSpace(password))
                throw new Exception("كلمة المرور مطلوبة.");

            ValidatePasswordPolicy(password);

            if (userRepository.UserNameExists(user.UserName))
                throw new Exception("اسم المستخدم موجود مسبقاً.");

            PasswordHasher.CreatePasswordHash(password, out string hash, out string salt);

            user.PasswordHash = hash;
            user.PasswordSalt = salt;

            bool added = userRepository.AddUser(user);
            if (added)
            {
                VerifyPersistedPermissions(user);
                auditLogService.Record("إنشاء", "User", user.UserID.ToString(),
                    string.Format("الحساب: {0}، الدور: {1}، الصلاحيات: {2}", user.UserName, user.RoleName, user.Permissions));
            }
            return added;
        }

        public bool UpdateUser(User user, string password, bool updatePassword)
        {
            CurrentUser.DemandAny("ليس لديك صلاحية تعديل المستخدمين.", PermissionKeys.UsersEdit, PermissionKeys.UsersManage);
            if (user.UserID <= 0)
                throw new Exception("رقم المستخدم غير صحيح.");

            User existingUser = userRepository.GetUserById(user.UserID);
            if (existingUser == null)
                throw new Exception("المستخدم غير موجود.");

            NormalizeUser(user);

            if (PermissionKeys.IsSystemAdministratorRole(user.RoleName) &&
                !PermissionKeys.IsSystemAdministratorRole(existingUser.RoleName) && !CurrentUser.IsAdmin())
                throw new UnauthorizedAccessException("لا يمكن إلا لمدير النظام رفع حساب إلى مدير نظام.");

            ValidateUser(user, true);

            bool changingRoleOrPermissions = !string.Equals(
                PermissionKeys.NormalizeRoleName(existingUser.RoleName),
                PermissionKeys.NormalizeRoleName(user.RoleName),
                StringComparison.OrdinalIgnoreCase)
                || !string.Equals(
                    PermissionKeys.NormalizePermissions(existingUser.Permissions),
                    PermissionKeys.NormalizePermissions(user.Permissions),
                    StringComparison.OrdinalIgnoreCase);

            if (changingRoleOrPermissions)
                CurrentUser.DemandAny("ليس لديك صلاحية إدارة أدوار وصلاحيات المستخدمين.", PermissionKeys.UsersManageRoles, PermissionKeys.UsersManage);

            bool removingAdministrator = PermissionKeys.IsSystemAdministratorRole(existingUser.RoleName) &&
                (!PermissionKeys.IsSystemAdministratorRole(user.RoleName) || !user.IsActive);

            if (removingAdministrator && userRepository.CountAdmins() <= 1)
                throw new Exception("لا يمكن تعطيل أو إزالة آخر مدير نظام.");

            if (CurrentUser.IsLoggedIn && CurrentUser.User.UserID == user.UserID)
            {
                if (!user.IsActive)
                    throw new Exception("لا يمكن تعطيل الحساب المستخدم حاليًا.");

                if (!PermissionKeys.IsSystemAdministratorRole(existingUser.RoleName) &&
                    PermissionKeys.IsSystemAdministratorRole(user.RoleName))
                    throw new Exception("لا يمكن للمستخدم رفع حسابه إلى مدير النظام.");

                if (PermissionKeys.IsSystemAdministratorRole(existingUser.RoleName) &&
                    !PermissionKeys.IsSystemAdministratorRole(user.RoleName))
                    throw new Exception("لا يمكن للمستخدم خفض صلاحيات مدير النظام لحسابه.");
            }

            if (userRepository.UserNameExists(user.UserName, user.UserID))
                throw new Exception("اسم المستخدم موجود مسبقاً.");

            if (PermissionKeys.IsSystemAdministratorRole(user.RoleName))
            {
                user.Permissions = PermissionKeys.GetRoleDefaults(user.RoleName);
                user.MustChangePassword = false;
            }

            if (updatePassword)
            {
                password = NormalizeDigits(password);

                if (string.IsNullOrWhiteSpace(password))
                    throw new Exception("كلمة المرور مطلوبة.");

                ValidatePasswordPolicy(password);

                PasswordHasher.CreatePasswordHash(password, out string hash, out string salt);

                user.PasswordHash = hash;
                user.PasswordSalt = salt;
            }

            bool updated = userRepository.UpdateUser(user, updatePassword);

            if (updated)
                VerifyPersistedPermissions(user);

            // إذا عدّل المدير الحساب المستخدم حاليًا، أعد تحميل النسخة المحفوظة
            // حتى لا تبقى صلاحيات قديمة داخل الذاكرة.
            if (updated && CurrentUser.IsLoggedIn && CurrentUser.User.UserID == user.UserID)
            {
                User refreshedUser = userRepository.GetUserById(user.UserID);
                if (refreshedUser != null)
                    CurrentUser.Set(refreshedUser);
            }

            if (updated)
                auditLogService.Record("تعديل", "User", user.UserID.ToString(),
                    string.Format("الحساب: {0}، الدور: {1}، نشط: {2}، تغيير كلمة المرور: {3}، الصلاحيات: {4}", user.UserName, user.RoleName, user.IsActive, updatePassword, user.Permissions));

            return updated;
        }

        public bool DeleteUser(int userId)
        {
            CurrentUser.DemandAny("ليس لديك صلاحية حذف المستخدمين.", PermissionKeys.UsersDelete, PermissionKeys.UsersManage);
            if (userId <= 0)
                throw new Exception("رقم المستخدم غير صحيح.");

            User user = userRepository.GetUserById(userId);

            if (user == null)
                throw new Exception("المستخدم غير موجود.");

            if (CurrentUser.IsLoggedIn && CurrentUser.User.UserID == userId)
                throw new Exception("لا يمكن حذف المستخدم المسجل دخوله حاليًا.");

            if (PermissionKeys.IsSystemAdministratorRole(user.RoleName) && userRepository.CountAdmins() <= 1)
                throw new Exception("لا يمكن حذف آخر مدير نظام.");

            int protectedUserId = CurrentUser.IsLoggedIn ? CurrentUser.User.UserID : 0;
            bool deleted = userRepository.DeleteUser(userId, protectedUserId);
            if (deleted)
                auditLogService.Record("حذف", "User", userId.ToString(),
                    string.Format("حذف الحساب: {0}، الدور: {1}", user.UserName, user.RoleName));
            return deleted;
        }

        public User Authenticate(string userName, string password)
        {
            userName = NormalizeDigits(userName).Trim();
            // لا تستخدم Trim لكلمة المرور؛ المسافات قد تكون جزءاً صحيحاً من كلمة المرور.
            password = NormalizeDigits(password);

            if (string.IsNullOrWhiteSpace(userName))
                throw new Exception("أدخل اسم المستخدم.");

            if (string.IsNullOrWhiteSpace(password))
                throw new Exception("أدخل كلمة المرور.");

            // لا تسمح ببقاء جلسة سابقة إذا فشلت محاولة الدخول أو بدأت جلسة جديدة.
            CurrentUser.Clear();

            User user = userRepository.GetUserByUserName(userName);

            if (user == null)
                throw new Exception("اسم المستخدم أو كلمة المرور غير صحيحة.");

            if (!user.IsActive)
            {
                if (user.LockedAt.HasValue || user.FailedLoginAttempts >= 3)
                    throw new Exception("تم تعطيل حسابك مؤقتاً بعد ثلاث محاولات دخول فاشلة. اطلب من مدير النظام إعادة تفعيل الحساب.");

                throw new Exception("هذا الحساب غير نشط حالياً. راجع مدير النظام لتفعيل الحساب.");
            }

            bool ok = PasswordHasher.VerifyPassword(password, user.PasswordHash, user.PasswordSalt);
            bool legacyPassword = !ok && PasswordHasher.VerifyLegacyPassword(password, user.PasswordHash, user.PasswordSalt);

            if (legacyPassword)
            {
                PasswordHasher.CreatePasswordHash(password, out string migratedHash, out string migratedSalt);
                userRepository.ResetPasswordByUserName(user.UserName, migratedHash, migratedSalt);
                user.PasswordHash = migratedHash;
                user.PasswordSalt = migratedSalt;
                ok = true;
            }

            if (!ok)
            {
                if (!PermissionKeys.IsSystemAdministratorRole(user.RoleName))
                {
                    int attempts = userRepository.RegisterFailedLoginAttempt(user.UserID);
                    user.FailedLoginAttempts = attempts;
                    user.RemainingLoginAttempts = Math.Max(0, 3 - attempts);
                    if (attempts >= 3)
                    {
                        try
                        {
                            EmailNotificationService.QueueAccountLockedAlert(
                                user,
                                attempts,
                                userRepository.GetSystemAdministratorEmails());
                        }
                        catch (Exception emailException)
                        {
                            ApplicationLogger.LogException("تهيئة تنبيه قفل الحساب", emailException);
                        }

                        auditLogService.Record(
                            "قفل حساب",
                            "User",
                            user.UserID.ToString(),
                            "تم قفل الحساب تلقائياً بعد ثلاث محاولات دخول فاشلة دون تسجيل كلمة المرور.");

                        throw new Exception("تم تعطيل الحساب بعد تجاوز ثلاث محاولات دخول فاشلة. اطلب من مدير النظام إعادة تفعيله.");
                    }

                    throw new Exception(string.Format(
                        "اسم المستخدم أو كلمة المرور غير صحيحة. تبقت لك {0} محاولة قبل تعطيل الحساب.",
                        user.RemainingLoginAttempts));
                }

                throw new Exception("اسم المستخدم أو كلمة المرور غير صحيحة.");
            }

            user.RoleName = PermissionKeys.NormalizeRoleName(user.RoleName);
            user.Permissions = LoadRuntimePermissions(user);
            EnsureRolePermissions(user);
            userRepository.RecordSuccessfulLogin(user.UserID);
            user.FailedLoginAttempts = 0;
            user.LockedAt = null;
            user.LastLoginAt = DateTime.Now;

            try
            {
                string normalizedPermissions = PermissionKeys.NormalizePermissions(user.Permissions);
                int permissionCount = string.IsNullOrWhiteSpace(normalizedPermissions)
                    ? 0
                    : normalizedPermissions.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Length;

                ApplicationLogger.LogException("تشخيص الصلاحيات",
                    new Exception(string.Format("تسجيل دخول ناجح: user={0}, role={1}, permissionsCount={2}",
                        user.UserName, user.RoleName, permissionCount)));
            }
            catch
            {
                // لا نسمح لفشل التسجيل بإيقاف تسجيل الدخول.
            }

            CurrentUser.Set(user);

            try
            {
                auditLogService.Record(
                    "تسجيل الدخول",
                    "User",
                    user.UserID.ToString(),
                    "تم تسجيل الدخول بنجاح للحساب " + user.UserName);
            }
            catch (Exception auditException)
            {
                ApplicationLogger.LogException("تسجيل الدخول في سجل التدقيق", auditException);
            }

            return user;
        }

        public void EnsureDefaultAdmin()
        {
            if (userRepository.CountUsers() > 0)
                return;

            // لا تنشئ حساباً بكلمة مرور ثابتة داخل التطبيق أو المستودع.
            // يحدد مسؤول النشر كلمة المرور الأولية خارج الكود قبل أول تشغيل.
            string initialPassword = Environment.GetEnvironmentVariable("SCHOOL_SYSTEM_INITIAL_ADMIN_PASSWORD");
            if (string.IsNullOrWhiteSpace(initialPassword) || initialPassword.Length < 10)
                throw new InvalidOperationException(
                    "لا توجد كلمة مرور تهيئة آمنة. عيّن متغير البيئة SCHOOL_SYSTEM_INITIAL_ADMIN_PASSWORD بطول 10 أحرف على الأقل ثم أعد التشغيل.");

            User admin = new User
            {
                FullName = "مدير النظام",
                UserName = "admin",
                RoleName = "مدير النظام",
                Permissions = GetAllPermissionsString(),
                Email = "",
                Phone = "",
                IsActive = true,
                MustChangePassword = false
            };

            AddUser(admin, initialPassword);
        }

        public bool ChangeCurrentUserPassword(string currentPassword, string newPassword, string confirmation)
        {
            if (!CurrentUser.IsLoggedIn || CurrentUser.User == null)
                throw new Exception("انتهت جلسة المستخدم. سجّل الدخول مرة أخرى.");

            currentPassword = NormalizeDigits(currentPassword);
            newPassword = NormalizeDigits(newPassword);
            confirmation = NormalizeDigits(confirmation);

            if (string.IsNullOrEmpty(currentPassword))
                throw new Exception("أدخل كلمة المرور الحالية.");
            if (string.IsNullOrWhiteSpace(newPassword))
                throw new Exception("أدخل كلمة المرور الجديدة.");
            ValidatePasswordPolicy(newPassword);
            if (!string.Equals(newPassword, confirmation, StringComparison.Ordinal))
                throw new Exception("تأكيد كلمة المرور غير مطابق.");
            if (string.Equals(newPassword, currentPassword, StringComparison.Ordinal))
                throw new Exception("يجب أن تختلف كلمة المرور الجديدة عن الحالية.");

            User currentUser = userRepository.GetUserById(CurrentUser.User.UserID);
            if (currentUser == null)
                throw new Exception("تعذر العثور على حساب المستخدم.");

            bool valid = PasswordHasher.VerifyPassword(currentPassword, currentUser.PasswordHash, currentUser.PasswordSalt)
                || PasswordHasher.VerifyLegacyPassword(currentPassword, currentUser.PasswordHash, currentUser.PasswordSalt);
            if (!valid)
                throw new Exception("كلمة المرور الحالية غير صحيحة.");

            PasswordHasher.CreatePasswordHash(newPassword, out string hash, out string salt);
            if (!userRepository.ChangePassword(currentUser.UserID, hash, salt))
                throw new Exception("تعذر حفظ كلمة المرور الجديدة.");

            currentUser.PasswordHash = hash;
            currentUser.PasswordSalt = salt;
            currentUser.MustChangePassword = false;
            CurrentUser.Set(currentUser);

            auditLogService.Record(
                "تغيير كلمة المرور",
                "User",
                currentUser.UserID.ToString(),
                "تم تغيير كلمة المرور بنجاح من قبل المستخدم نفسه");
            return true;
        }

        public bool ResetPasswordByUserName(string userName, string newPassword)
        {
            CurrentUser.DemandAny("ليس لديك صلاحية إعادة تعيين كلمات مرور المستخدمين.", PermissionKeys.UsersEdit, PermissionKeys.UsersManage);
            userName = NormalizeDigits(userName).Trim();
            newPassword = NormalizeDigits(newPassword).Trim();

            if (string.IsNullOrWhiteSpace(userName))
                throw new Exception("اسم المستخدم مطلوب.");

            if (string.IsNullOrWhiteSpace(newPassword))
                throw new Exception("كلمة المرور الجديدة مطلوبة.");

            ValidatePasswordPolicy(newPassword);

            User targetUser = userRepository.GetUserByUserName(userName);
            if (targetUser == null)
                throw new Exception("المستخدم غير موجود.");

            PasswordHasher.CreatePasswordHash(newPassword, out string hash, out string salt);
            bool reset = userRepository.ResetPasswordByUserName(userName, hash, salt);
            if (reset)
            {
                auditLogService.Record(
                    "إعادة تعيين كلمة المرور",
                    "User",
                    targetUser.UserID.ToString(),
                    "تمت إعادة تعيين كلمة مرور الحساب " + targetUser.UserName + " دون تسجيل كلمة المرور");
            }
            return reset;
        }

        private string GetAllPermissionsString()
        {
            return PermissionKeys.Serialize(PermissionKeys.All);
        }

        private void ValidatePasswordPolicy(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 10)
                throw new Exception("كلمة المرور يجب ألا تقل عن 10 أحرف.");
            if (!password.Any(char.IsLetter) || !password.Any(char.IsDigit))
                throw new Exception("كلمة المرور يجب أن تحتوي على أحرف وأرقام.");
        }

        private string LoadRuntimePermissions(User user)
        {
            if (user == null)
                return string.Empty;

            bool hasExplicitPermissionsValue = user.Permissions != null;
            string directPermissions = PermissionKeys.NormalizePermissions(user.Permissions);
            string rolePermissions = ResolveRolePermissions(user);

            // الصلاحيات الفعالة هي اتحاد صلاحيات الدور مع الصلاحيات الفردية المحفوظة.
            // هذا يمنع فقدان صلاحيات الدور، ويضمن أن صلاحيات الشاشة التي منحها المدير
            // مثل Students.View وTeachers.View تصل إلى CurrentUser ثم MainForm.
            string effectivePermissions = PermissionKeys.Serialize(
                (directPermissions ?? string.Empty)
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Concat((rolePermissions ?? string.Empty)
                        .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)));

            // الحسابات القديمة التي لا تحتوي قيمة Permissions تستعيد صلاحيات الدور
            // وتُحفظ مرة واحدة كقيمة صريحة لتجنب العودة إلى Dashboard وReports فقط.
            if (!hasExplicitPermissionsValue && !string.IsNullOrWhiteSpace(rolePermissions))
            {
                userRepository.UpdatePermissions(user.UserID, rolePermissions);
            }

            return effectivePermissions;
        }

        private string ResolveRolePermissions(User user)
        {
            if (user == null)
                return string.Empty;

            List<string> rolePermissions = userRepository.GetRolePermissions(user.UserID)
                .Select(PermissionKeys.NormalizePermissionKey)
                .Where(key => !string.IsNullOrWhiteSpace(key))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            return rolePermissions.Count > 0
                ? PermissionKeys.Serialize(rolePermissions)
                : PermissionKeys.NormalizePermissions(PermissionKeys.GetRoleDefaults(user.RoleName));
        }

        private void EnsureRolePermissions(User user)
        {
            if (user == null)
                return;

            // طبّع الصلاحيات المحفوظة لإزالة المسافات والتنسيق غير المتسق
            string normalized = PermissionKeys.NormalizePermissions(user.Permissions);

            if (PermissionKeys.IsSystemAdministratorRole(user.RoleName))
            {
                // مدير النظام يحصل دائمًا على كامل الكتالوج من القاموس المركزي
                // بغض النظر عما هو محفوظ في قاعدة البيانات
                string adminPermissions = PermissionKeys.GetRoleDefaults(user.RoleName);
                if (!string.Equals(user.Permissions ?? string.Empty, adminPermissions, StringComparison.Ordinal))
                {
                    user.Permissions = adminPermissions;
                    userRepository.UpdatePermissions(user.UserID, adminPermissions);
                }
                return;
            }

            // للمستخدمين العاديين: فقط نُصلح التنسيق (normalization)
            // ولا نُعيد الكتابة بصلاحيات الدور الافتراضية لأن المدير قد يكون
            // خصص صلاحيات مختلفة عن اقتراحات الدور عن قصد.
            //
            // لكن نُحدِّث قاعدة البيانات إذا تغير النص بعد التطبيع فقط
            // (مثل إزالة مسافات زائدة أو توحيد فاصلة).
            if (!string.IsNullOrWhiteSpace(user.Permissions) &&
                !string.Equals(user.Permissions, normalized, StringComparison.Ordinal))
            {
                user.Permissions = normalized;
                userRepository.UpdatePermissions(user.UserID, normalized);
            }
            else if (!string.IsNullOrWhiteSpace(normalized))
            {
                user.Permissions = normalized;
            }
        }

        private void VerifyPersistedPermissions(User expectedUser)
        {
            User persistedUser = userRepository.GetUserById(expectedUser.UserID);
            string expected = PermissionKeys.NormalizePermissions(expectedUser.Permissions);
            string actual = persistedUser == null
                ? string.Empty
                : PermissionKeys.NormalizePermissions(persistedUser.Permissions);

            if (persistedUser == null || !string.Equals(expected, actual, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    "لم يتم حفظ صلاحيات المستخدم كاملة في قاعدة البيانات. " +
                    "تحقق من نوع العمود Users.Permissions ثم أعد تنفيذ العملية.");
            }
        }

        private void ValidateUser(User user, bool allowEmptyPermissions)
        {
            if (user == null)
                throw new Exception("بيانات المستخدم غير موجودة.");

            if (string.IsNullOrWhiteSpace(user.FullName))
                throw new Exception("أدخل الاسم الكامل.");

            if (ContainsDigits(user.FullName))
                throw new Exception("الاسم الكامل لا يجب أن يحتوي على أرقام.");

            if (string.IsNullOrWhiteSpace(user.UserName))
                throw new Exception("أدخل اسم المستخدم.");

            if (user.UserName.Contains(" "))
                throw new Exception("اسم المستخدم لا يجب أن يحتوي على مسافات.");

            if (string.IsNullOrWhiteSpace(user.RoleName))
                throw new Exception("اختر الدور.");

            if (!allowEmptyPermissions && string.IsNullOrWhiteSpace(user.Permissions))
                throw new Exception("يجب اختيار صلاحية واحدة على الأقل.");

            if (!string.IsNullOrWhiteSpace(user.Email) && !IsValidEmail(user.Email))
                throw new Exception("البريد الإلكتروني غير صحيح.");

            if (!string.IsNullOrWhiteSpace(user.Phone))
            {
                string phone = NormalizeDigits(user.Phone).Trim();

                if (!phone.All(char.IsDigit))
                    throw new Exception("رقم الهاتف يجب أن يحتوي على أرقام فقط.");

                if (phone.Length < 7 || phone.Length > 15)
                    throw new Exception("رقم الهاتف غير صحيح.");
            }
        }

        private void NormalizeUser(User user)
        {
            if (user == null)
                return;

            user.FullName = NormalizeDigits(user.FullName).Trim();
            user.UserName = NormalizeDigits(user.UserName).Trim();
            user.RoleName = PermissionKeys.NormalizeRoleName(NormalizeDigits(user.RoleName));
            user.Permissions = PermissionKeys.NormalizePermissions(NormalizeDigits(user.Permissions));
            user.Email = NormalizeDigits(user.Email).Trim();
            user.Phone = NormalizeDigits(user.Phone).Trim();
        }

        private bool ContainsDigits(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            return NormalizeDigits(value).Any(char.IsDigit);
        }

        private bool IsValidEmail(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            try
            {
                var address = new System.Net.Mail.MailAddress(value.Trim());
                return address.Address.Equals(value.Trim(), StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        private string NormalizeDigits(string value)
        {
            if (string.IsNullOrEmpty(value))
                return "";

            return value
                .Replace('٠', '0')
                .Replace('١', '1')
                .Replace('٢', '2')
                .Replace('٣', '3')
                .Replace('٤', '4')
                .Replace('٥', '5')
                .Replace('٦', '6')
                .Replace('٧', '7')
                .Replace('٨', '8')
                .Replace('٩', '9');
        }
    }
}
