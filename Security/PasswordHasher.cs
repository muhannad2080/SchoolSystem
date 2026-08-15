using System;
using System.Security.Cryptography;

namespace SchoolSystem.Security
{
    public static class PasswordHasher
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 10000;

        public static void CreatePasswordHash(string password, out string passwordHash, out string passwordSalt)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("كلمة المرور فارغة.");

            byte[] salt = new byte[SaltSize];

            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations))
            {
                byte[] hash = pbkdf2.GetBytes(HashSize);

                passwordSalt = Convert.ToBase64String(salt);
                passwordHash = Convert.ToBase64String(hash);
            }
        }

        public static bool VerifyLegacyPassword(string password, string storedHash, string storedSalt)
        {
            if (string.IsNullOrEmpty(password) || !string.IsNullOrEmpty(storedSalt) || string.IsNullOrEmpty(storedHash))
                return false;

            if (password.Length != storedHash.Length)
                return false;

            int difference = 0;
            for (int i = 0; i < password.Length; i++)
                difference |= password[i] ^ storedHash[i];

            return difference == 0;
        }

        public static bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(password))
                    return false;

                if (string.IsNullOrWhiteSpace(storedHash) || string.IsNullOrWhiteSpace(storedSalt))
                    return false;

                byte[] salt = Convert.FromBase64String(storedSalt);
                byte[] expectedHash = Convert.FromBase64String(storedHash);

                using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations))
                {
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
