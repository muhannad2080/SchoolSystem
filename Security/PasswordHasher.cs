using System;
using System.Security.Cryptography;

// وضع الأنواع التالية ضمن مساحة الأسماء SchoolSystem.Security.
namespace SchoolSystem.Security
{
    // تعريف الصنف PasswordHasher المسؤول عن وظائف طبقة Security.
    public static class PasswordHasher
    {
        // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
        private const int SaltSize = 16;
        // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
        private const int HashSize = 32;

        private const int Iterations = 10000;

        // تعريف الدالة CreatePasswordHash لتنفيذ العملية المرتبطة بها.
        public static void CreatePasswordHash(string password, out string passwordHash, out string passwordSalt)
        {
            if (string.IsNullOrWhiteSpace(password))

                throw new ArgumentException("كلمة المرور فارغة.");

            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            byte[] salt = new byte[SaltSize];

            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations))
            {
                // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
                byte[] hash = pbkdf2.GetBytes(HashSize);

                // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
                passwordSalt = Convert.ToBase64String(salt);
                // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
                passwordHash = Convert.ToBase64String(hash);
            }
        }

        // تعريف الدالة VerifyLegacyPassword لتنفيذ العملية المرتبطة بها.
        public static bool VerifyLegacyPassword(string password, string storedHash, string storedSalt)
        {
            if (string.IsNullOrEmpty(password) || !string.IsNullOrEmpty(storedSalt) || string.IsNullOrEmpty(storedHash))
                return false;

            if (password.Length != storedHash.Length)
                return false;

            int difference = 0;

            for (int i = 0; i < password.Length; i++)
                // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
                difference |= password[i] ^ storedHash[i];

            // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
            return difference == 0;
        }

        // تعريف الدالة VerifyPassword لتنفيذ العملية المرتبطة بها.
        public static bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                if (string.IsNullOrWhiteSpace(password))
                    return false;

                if (string.IsNullOrWhiteSpace(storedHash) || string.IsNullOrWhiteSpace(storedSalt))
                    return false;

                // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
                byte[] salt = Convert.FromBase64String(storedSalt);
                // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
                byte[] expectedHash = Convert.FromBase64String(storedHash);

                using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations))
                {
                    // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
                    byte[] actualHash = pbkdf2.GetBytes(HashSize);

                    if (actualHash.Length != expectedHash.Length)
                        return false;


                    for (int i = 0; i < actualHash.Length; i++)
                    {
                        if (actualHash[i] != expectedHash[i])
                            return false;
                    }


                    return true;
                }
            }

            catch
            {

                return false;
            }
        }
    }
}
