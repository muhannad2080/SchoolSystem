using System;
using System.Security.Cryptography;
using System.Text;

namespace SchoolSystem.Helpers
{
    /// <summary>
    /// يحفظ أسرار التطبيق (مثل كلمة مرور قاعدة البيانات أو كلمة مرور SMTP) مشفّرة
    /// باستخدام DPAPI المرتبط بمستخدم الجهاز الحالي. لا تُخزَّن الأسرار نصًا صريحًا
    /// في ملفات الإعدادات أو السجلات.
    /// </summary>
    public static class SecureStorage
    {
        private static readonly byte[] OptionalEntropy =
            Encoding.UTF8.GetBytes("SchoolSystem.Secrets.v1");

        /// <summary>يُشفّر قيمة نصية ويعيد تمثيلاً base64 قابلاً للحفظ.</summary>
        public static string Protect(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return string.Empty;

            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] protectedBytes = ProtectedData.Protect(
                plainBytes, OptionalEntropy, DataProtectionScope.CurrentUser);

            return Convert.ToBase64String(protectedBytes);
        }

        /// <summary>يفك تشفير قيمة كانت مشفّرة عبر Protect. يعيد نصًا فارغاً عند أي خطأ.</summary>
        public static string Unprotect(string protectedBase64)
        {
            if (string.IsNullOrWhiteSpace(protectedBase64))
                return string.Empty;

            try
            {
                byte[] protectedBytes = Convert.FromBase64String(protectedBase64);
                byte[] plainBytes = ProtectedData.Unprotect(
                    protectedBytes, OptionalEntropy, DataProtectionScope.CurrentUser);

                return Encoding.UTF8.GetString(plainBytes);
            }
            catch (Exception ex)
            {
                ApplicationLogger.LogWarning("التخزين الآمن", "فشل فك تشفير قيمة محمية بالـ DPAPI: " + ex.Message);
                return string.Empty;
            }
        }
    }
}
