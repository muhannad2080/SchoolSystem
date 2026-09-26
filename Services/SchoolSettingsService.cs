using System;
using System.Net.Mail;
using SchoolSystem.DataAccess;
using SchoolSystem.Models;
// يستورد طبقة الأمان للتحقق من صلاحية تعديل إعدادات المدرسة.
using SchoolSystem.Security;
using SchoolSystem.Helpers;


namespace SchoolSystem.Services
{

    public class SchoolSettingsService
    {
        // تعريف الدالة object لتنفيذ العملية المرتبطة بها.
        private static readonly object SyncRoot = new object();

        private static SchoolSettings cached;
        // تعريف الدالة SchoolSettingsRepository لتنفيذ العملية المرتبطة بها.
        private readonly SchoolSettingsRepository repository = new SchoolSettingsRepository();

        private readonly AuditLogService auditLogService = new AuditLogService();

        // تعريف الدالة Load لتنفيذ العملية المرتبطة بها.
        public SchoolSettings Load()
        {

            lock (SyncRoot)
            {
                if (cached == null)
                    cached = repository.Get();

                return Clone(cached);
            }
        }

        // تعريف الدالة Save لتنفيذ العملية المرتبطة بها.
        public void Save(SchoolSettings settings)
        {

            CurrentUser.DemandAny("ليس لديك صلاحية تعديل إعدادات المدرسة.", PermissionKeys.SettingsEdit, PermissionKeys.SettingsManage);
            Validate(settings);

            repository.Save(settings);

            auditLogService.Record("تعديل", "SchoolSettings", settings.SchoolSettingsID.ToString(),
                string.Format("تم تحديث بيانات المدرسة: {0}", settings.SchoolNameAr));

            lock (SyncRoot)
                cached = Clone(settings);
        }

        // تعريف الدالة Current لتنفيذ العملية المرتبطة بها.
        public static SchoolSettings Current()
        {

            lock (SyncRoot)
            {
                if (cached == null)
                {
                    // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
                    try
                    {
                        cached = new SchoolSettingsRepository().Get();
                    }

                    catch (Exception ex)
                    {

                        ApplicationLogger.LogException("خدمة " + nameof(SchoolSettingsService) + ": تحميل إعدادات المدرسة إلى الذاكرة المؤقتة", ex);

                        cached = null;
                    }
                }

                return Clone(cached);
            }
        }

        // تعريف الدالة ClearCache لتنفيذ العملية المرتبطة بها.
        public static void ClearCache()
        {

            lock (SyncRoot)
                cached = null;
        }

        // تعريف الدالة Validate لتنفيذ العملية المرتبطة بها.
        public static void Validate(SchoolSettings settings)
        {
            if (settings == null)

                throw new InvalidOperationException("بيانات المدرسة غير موجودة.");

            if (string.IsNullOrWhiteSpace(settings.SchoolNameAr))

                throw new InvalidOperationException("اسم المدرسة بالعربية مطلوب.");

            if (string.IsNullOrWhiteSpace(settings.SchoolNameEn))

                throw new InvalidOperationException("اسم المدرسة بالإنجليزية مطلوب.");

            if (!string.IsNullOrWhiteSpace(settings.Email))
            {
                // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
                try { new MailAddress(settings.Email.Trim()); }

                catch (Exception ex) { ApplicationLogger.LogException("خدمة " + nameof(SchoolSettingsService) + ": التحقق من صيغة البريد الإلكتروني", ex); throw new InvalidOperationException("صيغة البريد الإلكتروني غير صحيحة."); }
            }
            if (!string.IsNullOrWhiteSpace(settings.Website))
            {
                Uri uri;
                if (!Uri.TryCreate(settings.Website.Trim(), UriKind.Absolute, out uri) ||
                    (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))

                    throw new InvalidOperationException("صيغة الموقع الإلكتروني غير صحيحة. استخدم http أو https.");
            }
            if (settings.Logo != null && settings.Logo.Length > 5 * 1024 * 1024)

                throw new InvalidOperationException("حجم الشعار يجب ألا يتجاوز 5 ميجابايت.");
        }

        // تعريف الدالة Clone لتنفيذ العملية المرتبطة بها.
        private static SchoolSettings Clone(SchoolSettings source)
        {
            if (source == null)
                return null;
            SchoolSettings copy = new SchoolSettings
            {
                SchoolSettingsID = source.SchoolSettingsID,
                SchoolNameAr = source.SchoolNameAr,
                SchoolNameEn = source.SchoolNameEn,
                OrganizationNameAr = source.OrganizationNameAr,
                OrganizationNameEn = source.OrganizationNameEn,
                AddressAr = source.AddressAr,
                AddressEn = source.AddressEn,
                Phone = source.Phone,
                AdditionalPhone = source.AdditionalPhone,
                Email = source.Email,
                Website = source.Website,
                PrincipalNameAr = source.PrincipalNameAr,
                PrincipalNameEn = source.PrincipalNameEn,
                LicenseNumber = source.LicenseNumber,
                LogoFileName = source.LogoFileName,
                UpdatedAt = source.UpdatedAt,
                UpdatedByUserID = source.UpdatedByUserID

            };
            copy.Logo = source.Logo == null ? null : (byte[])source.Logo.Clone();

            return copy;
        }
    }
}
