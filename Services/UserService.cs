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
        // تعريف الدالة AuditLogService لتنفيذ العملية المرتبطة بها.
        private readonly AuditLogService auditLogService = new AuditLogService();

        // تعريف الدالة UserService لتنفيذ العملية المرتبطة بها.
        public UserService()
        {
            userRepository = new UserRepository();
        }

        // تعريف الدالة GetAllUsers لتنفيذ العملية المرتبطة بها.
        public DataTable GetAllUsers()
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.DemandModule("Users", "ليس لديك صلاحية عرض المستخدمين.");

            return userRepository.GetAllUsers();
        }

        // تعريف الدالة AddUser لتنفيذ العملية المرتبطة بها.
        public bool AddUser(User user, string password)
        {

            // بعد التهيئة يلزم صلاحية الوصول لشاشة المستخدمين.
            if (userRepository.CountUsers() > 0)
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                CurrentUser.DemandModule("Users", "ليس لديك صلاحية إضافة المستخدمين.");


            NormalizeUser(user);

            if (PermissionKeys.IsSystemAdministratorRole(user.RoleName) &&
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                userRepository.CountUsers() > 0 && !CurrentUser.IsAdmin())

                throw new UnauthorizedAccessException("لا يمكن إلا لمدير النظام إنشاء حساب مدير نظام.");


            if (PermissionKeys.IsSystemAdministratorRole(user.RoleName))
            {
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                user.Permissions = PermissionKeys.Serialize(PermissionKeys.ScreenPermissions);
                // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
                user.MustChangePassword = false;
            }

            else
            {
                // كل حساب غير إداري جديد يغيّر كلمة المرور التي أنشأها المدير عند أول دخول.
                // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
                user.MustChangePassword = true;
            }


            ValidateUser(user, false);

            // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
            password = NormalizeDigits(password);

            // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
            if (string.IsNullOrWhiteSpace(password))

                throw new Exception("كلمة المرور مطلوبة.");

            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            ValidatePasswordPolicy(password);

            // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
            if (userRepository.UserNameExists(user.UserName))

                throw new Exception("اسم المستخدم موجود مسبقاً.");

            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            PasswordHasher.CreatePasswordHash(password, out string hash, out string salt);

            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            user.PasswordHash = hash;
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            user.PasswordSalt = salt;

            // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
            bool added = userRepository.AddUser(user);
            if (added)
            {
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                user.Permissions = BuildEffectivePermissions(user.UserID, user.RoleName);
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                userRepository.UpdatePermissions(user.UserID, user.Permissions);
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                VerifyPersistedPermissions(user);
                auditLogService.Record("إنشاء", "User", user.UserID.ToString(),
                    // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                    string.Format("الحساب: {0}، الدور: {1}، الصلاحيات: {2}", user.UserName, user.RoleName, user.Permissions));
            }

            return added;
        }

        // تعريف الدالة UpdateUser لتنفيذ العملية المرتبطة بها.
        public bool UpdateUser(User user, string password, bool updatePassword)
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.DemandModule("Users", "ليس لديك صلاحية تعديل المستخدمين.");
            if (user.UserID <= 0)

                throw new Exception("رقم المستخدم غير صحيح.");

            User existingUser = userRepository.GetUserById(user.UserID);
            if (existingUser == null)

                throw new Exception("المستخدم غير موجود.");


            NormalizeUser(user);

            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            bool existingIsSystemAdministrator = PermissionKeys.IsSystemAdministratorRole(existingUser.RoleName);
            if (existingIsSystemAdministrator)
            {
                // حساب مدير النظام محمي: لا يمكن تغيير دوره أو تعطيله أو خفض صلاحياته.
                user.RoleName = existingUser.RoleName;
                user.IsActive = true;
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                user.Permissions = PermissionKeys.Serialize(PermissionKeys.ScreenPermissions);
                // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
                user.MustChangePassword = false;
            }

            if (PermissionKeys.IsSystemAdministratorRole(user.RoleName) &&
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                !PermissionKeys.IsSystemAdministratorRole(existingUser.RoleName) && !CurrentUser.IsAdmin())

                throw new UnauthorizedAccessException("لا يمكن إلا لمدير النظام رفع حساب إلى مدير نظام.");


            ValidateUser(user, true);

            if (existingIsSystemAdministrator &&
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                (!PermissionKeys.IsSystemAdministratorRole(user.RoleName) || !user.IsActive))

                throw new UnauthorizedAccessException("حساب مدير النظام محمي ولا يمكن خفض دوره أو تعطيله من الواجهة.");


            if (CurrentUser.IsLoggedIn && CurrentUser.User.UserID == user.UserID)
            {
                if (!user.IsActive)

                    throw new Exception("لا يمكن تعطيل الحساب المستخدم حاليًا.");


                if (!PermissionKeys.IsSystemAdministratorRole(existingUser.RoleName) &&
                    // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                    PermissionKeys.IsSystemAdministratorRole(user.RoleName))

                    throw new Exception("لا يمكن للمستخدم رفع حسابه إلى مدير النظام.");


                if (PermissionKeys.IsSystemAdministratorRole(existingUser.RoleName) &&
                    // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                    !PermissionKeys.IsSystemAdministratorRole(user.RoleName))

                    throw new Exception("لا يمكن للمستخدم خفض صلاحيات مدير النظام لحسابه.");
            }

            if (userRepository.UserNameExists(user.UserName, user.UserID))

                throw new Exception("اسم المستخدم موجود مسبقاً.");


            if (PermissionKeys.IsSystemAdministratorRole(user.RoleName))
            {
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                user.Permissions = PermissionKeys.Serialize(PermissionKeys.ScreenPermissions);
                // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
                user.MustChangePassword = false;
                // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
                user.IsActive = true;
            }

            if (updatePassword)
            {
                // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
                password = NormalizeDigits(password);

                // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
                if (string.IsNullOrWhiteSpace(password))

                    throw new Exception("كلمة المرور مطلوبة.");

                // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
                ValidatePasswordPolicy(password);

                // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
                PasswordHasher.CreatePasswordHash(password, out string hash, out string salt);

                // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
                user.PasswordHash = hash;
                // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
                user.PasswordSalt = salt;
            }

            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            bool updated = userRepository.UpdateUser(user, updatePassword);

            // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
            if (updated)
            {
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                user.Permissions = BuildEffectivePermissions(user.UserID, user.RoleName);
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                userRepository.UpdatePermissions(user.UserID, user.Permissions);
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                VerifyPersistedPermissions(user);
            }


            // حتى لا تبقى صلاحيات قديمة داخل الذاكرة.
            if (updated && CurrentUser.IsLoggedIn && CurrentUser.User.UserID == user.UserID)
            {
                User refreshedUser = userRepository.GetUserById(user.UserID);
                if (refreshedUser != null)
                    // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                    CurrentUser.Set(refreshedUser);
            }

            if (updated)
                auditLogService.Record("تعديل", "User", user.UserID.ToString(),
                    // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                    string.Format("الحساب: {0}، الدور: {1}، نشط: {2}، تغيير كلمة المرور: {3}، الصلاحيات: {4}", user.UserName, user.RoleName, user.IsActive, updatePassword, user.Permissions));

            // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
            return updated;
        }

        // تعريف الدالة DeleteUser لتنفيذ العملية المرتبطة بها.
        public bool DeleteUser(int userId)
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.DemandModule("Users", "ليس لديك صلاحية حذف المستخدمين.");
            if (userId <= 0)

                throw new Exception("رقم المستخدم غير صحيح.");

            User user = userRepository.GetUserById(userId);

            if (user == null)

                throw new Exception("المستخدم غير موجود.");


            if (CurrentUser.IsLoggedIn && CurrentUser.User.UserID == userId)

                throw new Exception("لا يمكن حذف المستخدم المسجل دخوله حاليًا.");


            if (PermissionKeys.IsSystemAdministratorRole(user.RoleName))

                throw new UnauthorizedAccessException("حساب مدير النظام محمي ولا يمكن حذفه من الواجهة. للحذف الاستثنائي استخدم إجراءً مباشراً ومصرحاً به في قاعدة البيانات.");

            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            int protectedUserId = CurrentUser.IsLoggedIn ? CurrentUser.User.UserID : 0;
            bool deleted = userRepository.DeleteUser(userId, protectedUserId);
            if (deleted)
                auditLogService.Record("حذف", "User", userId.ToString(),
                    string.Format("حذف الحساب: {0}، الدور: {1}", user.UserName, user.RoleName));

            return deleted;
        }

        // تعريف الدالة Authenticate لتنفيذ العملية المرتبطة بها.
        public User Authenticate(string userName, string password)
        {
            userName = NormalizeDigits(userName).Trim();
            // لا تستخدم Trim لكلمة المرور؛ المسافات قد تكون جزءاً صحيحاً من كلمة المرور.
            // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
            password = NormalizeDigits(password);

            // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
            if (string.IsNullOrWhiteSpace(userName))

                throw new Exception("أدخل اسم المستخدم.");


            if (string.IsNullOrWhiteSpace(password))

                throw new Exception("أدخل كلمة المرور.");


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

            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            bool ok = PasswordHasher.VerifyPassword(password, user.PasswordHash, user.PasswordSalt);
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            bool legacyPassword = !ok && PasswordHasher.VerifyLegacyPassword(password, user.PasswordHash, user.PasswordSalt);

            // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
            if (legacyPassword)
            {
                // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
                PasswordHasher.CreatePasswordHash(password, out string migratedHash, out string migratedSalt);
                // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
                userRepository.ResetPasswordByUserName(user.UserName, migratedHash, migratedSalt);
                // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
                user.PasswordHash = migratedHash;
                // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
                user.PasswordSalt = migratedSalt;
                // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
                ok = true;

                // توثيق ترقية حساب قديم من تخزين غير مشفر إلى PBKDF2.
                ApplicationLogger.Log("تسجيل الدخول", "تمت ترقية كلمة مرور الحساب '" + user.UserName + "' إلى تخزين PBKDF2.");
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
                        // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
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

            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            user.RoleName = PermissionKeys.NormalizeRoleName(user.RoleName);

            // تحميل الصلاحيات الفعالة من قاعدة البيانات (UserPermissions إن وجدت وإلا RolePermissions)
            // وتحديث الكاش Users.Permissions ليتطابق مع ما سيُطبَّق فعلياً عند الدخول.
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            user.Permissions = BuildEffectivePermissions(user.UserID, user.RoleName);
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            userRepository.UpdatePermissions(user.UserID, user.Permissions);
            user.RoleId = userRepository.GetUserRoleId(user.UserID);

            userRepository.RecordSuccessfulLogin(user.UserID);
            user.FailedLoginAttempts = 0;
            user.LockedAt = null;
            user.LastLoginAt = DateTime.Now;

            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                string normalizedPermissions = PermissionKeys.NormalizePermissions(user.Permissions);
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                int permissionCount = string.IsNullOrWhiteSpace(normalizedPermissions)
                    ? 0
                    // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                    : normalizedPermissions.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Length;

                ApplicationLogger.LogException("تشخيص الصلاحيات",
                    new Exception(string.Format("تسجيل دخول ناجح: user={0}, role={1}, permissionsCount={2}",
                        user.UserName, user.RoleName, permissionCount)));
            }

            catch
            {
                // تشخيص اختياري فقط؛ فشل كتابته لا يؤثر على عملية الدخول ولا يستحق مسار تسجيل بديل.
            }

            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.Set(user);

            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
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

        // تعريف الدالة EnsureDefaultAdmin لتنفيذ العملية المرتبطة بها.
        public void EnsureDefaultAdmin()
        {
            if (userRepository.CountUsers() > 0)
                return;


            // يحدد مسؤول النشر كلمة المرور الأولية خارج الكود قبل أول تشغيل.
            // تجهيز قيمة كلمة المرور لمسار الحماية والحماية.
            string initialPassword = Environment.GetEnvironmentVariable("SCHOOL_SYSTEM_INITIAL_ADMIN_PASSWORD");
            bool usingDefaultPassword = false;

            // تجهيز قيمة كلمة المرور لمسار الحماية والحماية.
            if (!IsValidInitialPassword(initialPassword))
            {
                // إذا لم تُحدَّد كلمة مرور آمنة عبر متغير البيئة فإننا نستخدم
                // كلمة مرور افتراضية مؤقتة موثقة، ويُجبَر المدير على تغييرها عند أول دخول.
                initialPassword = "Admin@2026";
                usingDefaultPassword = true;
            }

            // تمرير قيمة كلمة المرور إلى مسار الحماية والحماية.
            User admin = new User
            {
                FullName = "مدير النظام",
                UserName = "admin",
                RoleName = "مدير النظام",
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                Permissions = GetAllPermissionsString(),
                Email = "",
                Phone = "",
                IsActive = true,
                // تجهيز قيمة كلمة المرور لمسار الحماية والحماية.
                MustChangePassword = true
            // تمرير قيمة كلمة المرور إلى مسار الحماية والحماية.
            };

            // تمرير قيمة كلمة المرور إلى مسار الحماية والحماية.
            ApplicationLogger.Log("Security",
                "جاري إنشاء مدير النظام الأول بحساب admin وكلمة مرور مؤقتة.");
            try
            {
                AddUser(admin, initialPassword);

                // إجبار المدير الأول على تغيير كلمة المرور عند أول دخول بغض النظر عن دوره.
                userRepository.SetMustChangePassword(admin.UserID, true);

                if (usingDefaultPassword)
                {
                    ApplicationLogger.Log("Security",
                        "أنشئ مدير النظام الأول بنجاح (UserID=" + admin.UserID + ") بكلمة مرور افتراضية مؤقتة. يجب تغييرها عند أول دخول.");
                }
                else
                {
                    ApplicationLogger.Log("Security",
                        "أنشئ مدير النظام الأول بنجاح (UserID=" + admin.UserID + ") بكلمة مرور من متغير البيئة.");
                }
            }
            catch (Exception ex)
            {
                ApplicationLogger.LogException("Security", ex);
                throw;
            }
        }

        // تعريف الدالة IsValidInitialPassword لتنفيذ العملية المرتبطة بها.
        private static bool IsValidInitialPassword(string password)
        {
            // تمرير قيمة كلمة المرور إلى مسار الحماية والحماية.
            return !string.IsNullOrWhiteSpace(password)
                && password.Length >= 10
                && password.Any(char.IsLetter)
                && password.Any(char.IsDigit);
        }

        // تعريف الدالة ChangeCurrentUserPassword لتنفيذ العملية المرتبطة بها.
        public bool ChangeCurrentUserPassword(string currentPassword, string newPassword, string confirmation)
        {
            if (!CurrentUser.IsLoggedIn || CurrentUser.User == null)

                throw new Exception("انتهت جلسة المستخدم. سجّل الدخول مرة أخرى.");

            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            currentPassword = NormalizeDigits(currentPassword);
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            newPassword = NormalizeDigits(newPassword);
            // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
            confirmation = NormalizeDigits(confirmation);

            if (string.IsNullOrEmpty(currentPassword))

                throw new Exception("أدخل كلمة المرور الحالية.");

            if (string.IsNullOrWhiteSpace(newPassword))

                throw new Exception("أدخل كلمة المرور الجديدة.");
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            ValidatePasswordPolicy(newPassword);
            // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
            if (!string.Equals(newPassword, confirmation, StringComparison.Ordinal))

                throw new Exception("تأكيد كلمة المرور غير مطابق.");

            if (string.Equals(newPassword, currentPassword, StringComparison.Ordinal))

                throw new Exception("يجب أن تختلف كلمة المرور الجديدة عن الحالية.");

            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            User currentUser = userRepository.GetUserById(CurrentUser.User.UserID);
            if (currentUser == null)

                throw new Exception("تعذر العثور على حساب المستخدم.");

            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            bool valid = PasswordHasher.VerifyPassword(currentPassword, currentUser.PasswordHash, currentUser.PasswordSalt)
                // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
                || PasswordHasher.VerifyLegacyPassword(currentPassword, currentUser.PasswordHash, currentUser.PasswordSalt);
            // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
            if (!valid)

                throw new Exception("كلمة المرور الحالية غير صحيحة.");

            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            PasswordHasher.CreatePasswordHash(newPassword, out string hash, out string salt);
            // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
            if (!userRepository.ChangePassword(currentUser.UserID, hash, salt))

                throw new Exception("تعذر حفظ كلمة المرور الجديدة.");

            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            currentUser.PasswordHash = hash;
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            currentUser.PasswordSalt = salt;
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            currentUser.MustChangePassword = false;
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.Set(currentUser);

            auditLogService.Record(
                "تغيير كلمة المرور",
                "User",
                currentUser.UserID.ToString(),
                "تم تغيير كلمة المرور بنجاح من قبل المستخدم نفسه");

            return true;
        }

        // تعريف الدالة ResetPasswordByUserName لتنفيذ العملية المرتبطة بها.
        public bool ResetPasswordByUserName(string userName, string newPassword)
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.DemandModule("Users", "ليس لديك صلاحية إعادة تعيين كلمات مرور المستخدمين.");
            userName = NormalizeDigits(userName).Trim();
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            newPassword = NormalizeDigits(newPassword).Trim();

            // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
            if (string.IsNullOrWhiteSpace(userName))

                throw new Exception("اسم المستخدم مطلوب.");


            if (string.IsNullOrWhiteSpace(newPassword))

                throw new Exception("كلمة المرور الجديدة مطلوبة.");

            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            ValidatePasswordPolicy(newPassword);

            // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
            User targetUser = userRepository.GetUserByUserName(userName);
            if (targetUser == null)

                throw new Exception("المستخدم غير موجود.");

            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            PasswordHasher.CreatePasswordHash(newPassword, out string hash, out string salt);
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            bool reset = userRepository.ResetPasswordByUserName(userName, hash, salt);
            // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
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

        // تعريف الدالة GetAllPermissionsString لتنفيذ العملية المرتبطة بها.
        private string GetAllPermissionsString()
        {

            return PermissionKeys.Serialize(PermissionKeys.ScreenPermissions);
        }

        // تعريف الدالة ValidatePasswordPolicy لتنفيذ العملية المرتبطة بها.
        private void ValidatePasswordPolicy(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 10)

                throw new Exception("كلمة المرور يجب ألا تقل عن 10 أحرف.");

            if (!password.Any(char.IsLetter) || !password.Any(char.IsDigit))

                throw new Exception("كلمة المرور يجب أن تحتوي على أحرف وأرقام.");
        }

        /// <summary>
        /// الصلاحيات الفعلية للمستخدم من قاعدة البيانات:
        /// UserPermissions إن وُجدت (اللقطة المحددة)، وإلا RolePermissions (افتراضيات الدور).
        /// مدير النظام يحصل على كامل كتالوج الشاشات كسياسة مركزية.
        /// </summary>
        // تعريف الدالة BuildEffectivePermissions لتنفيذ العملية المرتبطة بها.
        private string BuildEffectivePermissions(int userId, string roleName)
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            List<string> effective = userRepository.GetEffectivePermissions(userId);

            if (PermissionKeys.IsSystemAdministratorRole(roleName))
                return PermissionKeys.Serialize(PermissionKeys.ScreenPermissions);


            return PermissionKeys.Serialize(effective);
        }

        /// <summary>
        /// يحفظ صلاحيات شاشات مستخدم محدد (استبدال كامل) في جدول UserPermissions
        /// ويُحدِّث الكاش Users.Permissions. يُستخدم من زر "حفظ الصلاحيات" في واجهة المستخدمين.
        /// </summary>
        // تعريف الدالة SaveUserPermissions لتنفيذ العملية المرتبطة بها.
        public string SaveUserPermissions(int userId, IList<string> screenKeys)
        {
            if (!CurrentUser.IsAdmin() && !CurrentUser.HasAny(PermissionKeys.PermissionsManage, PermissionKeys.UsersManageRoles, PermissionKeys.UsersManage))

                throw new UnauthorizedAccessException("ليس لديك صلاحية إدارة أدوار وصلاحيات المستخدمين.");


            if (userId <= 0)

                throw new Exception("رقم المستخدم غير صحيح.");

            User user = userRepository.GetUserById(userId);
            if (user == null)

                throw new Exception("المستخدم غير موجود.");


            if (PermissionKeys.IsSystemAdministratorRole(user.RoleName))
            {
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                string fixedPermissions = PermissionKeys.Serialize(PermissionKeys.ScreenPermissions);
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                userRepository.ReplaceUserPermissions(userId, new List<string>(PermissionKeys.ScreenPermissions));
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                userRepository.UpdatePermissions(userId, fixedPermissions);

                return fixedPermissions;
            }


            screenKeys = screenKeys ?? new List<string>();
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            userRepository.ReplaceUserPermissions(userId, screenKeys);

            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            string effective = BuildEffectivePermissions(userId, user.RoleName);
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            userRepository.UpdatePermissions(userId, effective);

            if (CurrentUser.IsLoggedIn && CurrentUser.User != null && CurrentUser.User.UserID == userId)
            {
                User refreshedUser = userRepository.GetUserById(userId);
                if (refreshedUser != null)
                    // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                    CurrentUser.Set(refreshedUser);
            }

            auditLogService.Record("تعديل الصلاحيات", "User", userId.ToString(),
                string.Format("الحساب: {0}، الدور: {1}، عدد شاشات الصلاحية: {2}",
                    user.UserName, user.RoleName, screenKeys == null ? 0 : screenKeys.Count));


            return effective;
        }

        // تعريف الدالة VerifyPersistedPermissions لتنفيذ العملية المرتبطة بها.
        private void VerifyPersistedPermissions(User expectedUser)
        {
            User persistedUser = userRepository.GetUserById(expectedUser.UserID);
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            string expected = PermissionKeys.NormalizePermissions(expectedUser.Permissions);
            string actual = persistedUser == null
                ? string.Empty
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                : PermissionKeys.NormalizePermissions(persistedUser.Permissions);

            if (persistedUser == null || !string.Equals(expected, actual, StringComparison.OrdinalIgnoreCase))
            {

                throw new InvalidOperationException(
                    "لم يتم حفظ صلاحيات المستخدم كاملة في قاعدة البيانات. " +
                    // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                    "تحقق من نوع العمود Users.Permissions ثم أعد تنفيذ العملية.");
            }
        }

        // تعريف الدالة ValidateUser لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة NormalizeUser لتنفيذ العملية المرتبطة بها.
        private void NormalizeUser(User user)
        {
            if (user == null)
                return;

            user.FullName = NormalizeDigits(user.FullName).Trim();
            user.UserName = NormalizeDigits(user.UserName).Trim();
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            user.RoleName = PermissionKeys.NormalizeRoleName(NormalizeDigits(user.RoleName));
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            user.Permissions = PermissionKeys.NormalizePermissions(NormalizeDigits(user.Permissions));
            user.Email = NormalizeDigits(user.Email).Trim();
            user.Phone = NormalizeDigits(user.Phone).Trim();
        }

        // تعريف الدالة ContainsDigits لتنفيذ العملية المرتبطة بها.
        private bool ContainsDigits(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;


            return NormalizeDigits(value).Any(char.IsDigit);
        }

        // تعريف الدالة IsValidEmail لتنفيذ العملية المرتبطة بها.
        private bool IsValidEmail(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
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

        // تعريف الدالة NormalizeDigits لتنفيذ العملية المرتبطة بها.
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
