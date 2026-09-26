using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using SchoolSystem.Helpers;
using SchoolSystem.Models;


namespace SchoolSystem.Services
{

    public static class EmailNotificationService
    {
        // تعريف الدالة QueueAccountLockedAlert لتنفيذ العملية المرتبطة بها.
        public static void QueueAccountLockedAlert(User user, int failedAttempts, IEnumerable<string> administratorEmails)
        {
            if (user == null || failedAttempts < 3 || administratorEmails == null)
                return;

            string[] recipients = administratorEmails
                .Where(IsValidEmail)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();

            if (recipients.Length == 0 || !IsEnabled())
                return;

            Task.Run(() =>
            {
                // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
                try
                {
                    SendAccountLockedAlert(user, failedAttempts, recipients);
                }

                catch (Exception ex)
                {
                    ApplicationLogger.LogException("تنبيه قفل الحساب بالبريد", ex);
                }
            });
        }

        // تعريف الدالة SendAccountLockedAlert لتنفيذ العملية المرتبطة بها.
        private static void SendAccountLockedAlert(User user, int failedAttempts, string[] recipients)
        {
            string host = GetSetting("SecurityAlertSmtpHost");
            string userName = GetSetting("SecurityAlertSmtpUser");
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            string password = GetSetting("SecurityAlertSmtpPassword");
            // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
            string fromAddress = GetSetting("SecurityAlertFromEmail");
            int port = GetIntSetting("SecurityAlertSmtpPort", 587);
            bool enableSsl = GetBoolSetting("SecurityAlertEnableSsl", true);

            if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(fromAddress))
                return;

            using (SmtpClient client = new SmtpClient(host, port))
            using (MailMessage message = new MailMessage())
            {
                client.EnableSsl = enableSsl;
                client.Timeout = 10000;
                if (!string.IsNullOrWhiteSpace(userName))
                    client.Credentials = new NetworkCredential(userName, password ?? string.Empty);

                // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
                message.From = new MailAddress(fromAddress, GetSetting("SecurityAlertFromName") ?? "SchoolSystem", Encoding.UTF8);
                foreach (string recipient in recipients)
                    message.To.Add(recipient);
                message.Subject = "تنبيه أمني: تم قفل حساب مستخدم";
                message.SubjectEncoding = Encoding.UTF8;
                message.BodyEncoding = Encoding.UTF8;
                message.IsBodyHtml = false;
                message.Body = string.Format(
                    "تم تعطيل حساب مستخدم بعد تجاوز الحد المسموح لمحاولات تسجيل الدخول الفاشلة.\r\n\r\n" +
                    "اسم المستخدم: {0}\r\n" +
                    "الاسم الكامل: {1}\r\n" +
                    "عدد المحاولات: {2}\r\n" +
                    "وقت التنبيه: {3:yyyy-MM-dd HH:mm:ss}\r\n\r\n" +
                    "يرجى فتح إدارة المستخدمين وإعادة تفعيل الحساب بعد التحقق.",
                    user.UserName ?? string.Empty,
                    user.FullName ?? string.Empty,
                    failedAttempts,
                    DateTime.Now);

                client.Send(message);
            }
        }

        // تعريف الدالة IsEnabled لتنفيذ العملية المرتبطة بها.
        private static bool IsEnabled()
        {

            return GetBoolSetting("SecurityAlertEmailEnabled", false);
        }

        // تعريف الدالة GetSetting لتنفيذ العملية المرتبطة بها.
        private static string GetSetting(string key)
        {

            return ConfigurationManager.AppSettings[key];
        }

        // تعريف الدالة GetIntSetting لتنفيذ العملية المرتبطة بها.
        private static int GetIntSetting(string key, int fallback)
        {
            int value;

            return int.TryParse(GetSetting(key), out value) && value > 0 ? value : fallback;
        }

        // تعريف الدالة GetBoolSetting لتنفيذ العملية المرتبطة بها.
        private static bool GetBoolSetting(string key, bool fallback)
        {
            bool value;

            return bool.TryParse(GetSetting(key), out value) ? value : fallback;
        }

        // تعريف الدالة IsValidEmail لتنفيذ العملية المرتبطة بها.
        private static bool IsValidEmail(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                MailAddress address = new MailAddress(value.Trim());

                return string.Equals(address.Address, value.Trim(), StringComparison.OrdinalIgnoreCase);
            }

            catch
            {

                return false;
            }
        }
    }
}
