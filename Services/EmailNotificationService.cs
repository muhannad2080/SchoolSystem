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

        private static void SendAccountLockedAlert(User user, int failedAttempts, string[] recipients)
        {
            string host = GetSetting("SecurityAlertSmtpHost");
            string userName = GetSetting("SecurityAlertSmtpUser");
            string password = GetSetting("SecurityAlertSmtpPassword");
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

        private static bool IsEnabled()
        {
            return GetBoolSetting("SecurityAlertEmailEnabled", false);
        }

        private static string GetSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        private static int GetIntSetting(string key, int fallback)
        {
            int value;
            return int.TryParse(GetSetting(key), out value) && value > 0 ? value : fallback;
        }

        private static bool GetBoolSetting(string key, bool fallback)
        {
            bool value;
            return bool.TryParse(GetSetting(key), out value) ? value : fallback;
        }

        private static bool IsValidEmail(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

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

