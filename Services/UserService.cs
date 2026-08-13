using System;
using System.Data;
using System.Linq;
using SchoolSystem.DataAccess;
using SchoolSystem.Models;
using SchoolSystem.Security;

namespace SchoolSystem.Services
{
    public class UserService
    {
        private readonly UserRepository userRepository;

        public UserService()
        {
            userRepository = new UserRepository();
        }

        public DataTable GetAllUsers()
        {
            return userRepository.GetAllUsers();
        }

        public bool AddUser(User user, string password)
        {
            NormalizeUser(user);
            ValidateUser(user);

            password = NormalizeDigits(password);

            if (string.IsNullOrWhiteSpace(password))
                throw new Exception("كلمة المرور مطلوبة.");

            if (password.Length < 6)
                throw new Exception("كلمة المرور يجب ألا تقل عن 6 أحرف.");

            if (userRepository.UserNameExists(user.UserName))
                throw new Exception("اسم المستخدم موجود مسبقاً.");

            PasswordHasher.CreatePasswordHash(password, out string hash, out string salt);

            user.PasswordHash = hash;
            user.PasswordSalt = salt;

            return userRepository.AddUser(user);
        }

        public bool UpdateUser(User user, string password, bool updatePassword)
        {
            if (user.UserID <= 0)
                throw new Exception("رقم المستخدم غير صحيح.");

            NormalizeUser(user);
            ValidateUser(user);

            if (userRepository.UserNameExists(user.UserName, user.UserID))
                throw new Exception("اسم المستخدم موجود مسبقاً.");

            if (updatePassword)
            {
                password = NormalizeDigits(password);

                if (string.IsNullOrWhiteSpace(password))
                    throw new Exception("كلمة المرور مطلوبة.");

                if (password.Length < 6)
                    throw new Exception("كلمة المرور يجب ألا تقل عن 6 أحرف.");

                PasswordHasher.CreatePasswordHash(password, out string hash, out string salt);

                user.PasswordHash = hash;
                user.PasswordSalt = salt;
            }

            return userRepository.UpdateUser(user, updatePassword);
        }

        public bool DeleteUser(int userId)
        {
            if (userId <= 0)
                throw new Exception("رقم المستخدم غير صحيح.");

            User user = userRepository.GetUserById(userId);

            if (user == null)
                throw new Exception("المستخدم غير موجود.");

            if (user.RoleName == "مدير النظام" && userRepository.CountAdmins() <= 1)
                throw new Exception("لا يمكن حذف آخر مدير نظام.");

            return userRepository.DeleteUser(userId);
        }

        public User Authenticate(string userName, string password)
        {
            userName = NormalizeDigits(userName).Trim();
            password = NormalizeDigits(password).Trim();

            if (string.IsNullOrWhiteSpace(userName))
                throw new Exception("أدخل اسم المستخدم.");

            if (string.IsNullOrWhiteSpace(password))
                throw new Exception("أدخل كلمة المرور.");

            User user = userRepository.GetUserByUserName(userName);

            if (user == null)
                throw new Exception("اسم المستخدم أو كلمة المرور غير صحيحة.");

            if (!user.IsActive)
                throw new Exception("هذا الحساب غير فعال.");

            bool ok = PasswordHasher.VerifyPassword(password, user.PasswordHash, user.PasswordSalt);

            if (!ok)
                throw new Exception("اسم المستخدم أو كلمة المرور غير صحيحة.");

            userRepository.UpdateLastLogin(user.UserID);

            CurrentUser.Set(user);

            return user;
        }

        public void EnsureDefaultAdmin()
        {
            if (userRepository.CountUsers() > 0)
                return;

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

            AddUser(admin, "admin123");
        }

        public bool ResetPasswordByUserName(string userName, string newPassword)
        {
            userName = NormalizeDigits(userName).Trim();
            newPassword = NormalizeDigits(newPassword).Trim();

            if (string.IsNullOrWhiteSpace(userName))
                throw new Exception("اسم المستخدم مطلوب.");

            if (string.IsNullOrWhiteSpace(newPassword))
                throw new Exception("كلمة المرور الجديدة مطلوبة.");

            if (newPassword.Length < 6)
                throw new Exception("كلمة المرور يجب ألا تقل عن 6 أحرف.");

            PasswordHasher.CreatePasswordHash(newPassword, out string hash, out string salt);

            return userRepository.ResetPasswordByUserName(userName, hash, salt);
        }

        private string GetAllPermissionsString()
        {
            return string.Join(",",
                PermissionKeys.DashboardView,
                PermissionKeys.StudentsView,
                PermissionKeys.StudentsManage,
                PermissionKeys.EnrollmentManage,
                PermissionKeys.ClassAssignmentManage,
                PermissionKeys.TeachersManage,
                PermissionKeys.StaffAttendanceManage,
                PermissionKeys.PayrollManage,
                PermissionKeys.SubjectsManage,
                PermissionKeys.ClassesManage,
                PermissionKeys.TimetableManage,
                PermissionKeys.AttendanceManage,
                PermissionKeys.GradesManage,
                PermissionKeys.FeesManage,
                PermissionKeys.VouchersManage,
                PermissionKeys.ExpensesManage,
                PermissionKeys.LibraryManage,
                PermissionKeys.TransportManage,
                PermissionKeys.ReportsView,
                PermissionKeys.UsersManage
            );
        }

        private void ValidateUser(User user)
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

            if (string.IsNullOrWhiteSpace(user.Permissions))
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
            user.RoleName = NormalizeDigits(user.RoleName).Trim();
            user.Permissions = NormalizeDigits(user.Permissions).Trim();
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
