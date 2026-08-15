from pathlib import Path
import re
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parents[1]
service = (ROOT / "Services" / "UserService.cs").read_text(encoding="utf-8")
email = (ROOT / "Services" / "EmailNotificationService.cs").read_text(encoding="utf-8")
config = (ROOT / "App.config").read_text(encoding="utf-8")
project = (ROOT / "SchoolSystem.csproj").read_text(encoding="utf-8")
model = (ROOT / "Models" / "User.cs").read_text(encoding="utf-8")
hasher = (ROOT / "Security" / "PasswordHasher.cs").read_text(encoding="utf-8")
recovery = (ROOT / "Databass" / "Unlock-LockedAccounts.sql").read_text(encoding="utf-8")
login = (ROOT / "UI" / "LoginForm.cs").read_text(encoding="utf-8")

checks = []
def check(name, condition):
    checks.append((name, condition))

check("admin role bypasses failed-attempt registration", "if (!PermissionKeys.IsSystemAdministratorRole(user.RoleName))" in service)
check("failed attempt is persisted", "RegisterFailedLoginAttempt(user.UserID)" in service)
check("remaining attempts are calculated", "Math.Max(0, 3 - attempts)" in service)
check("third attempt locks account", "if (attempts >= 3)" in service and "تم تعطيل الحساب" in service)
check("remaining attempts message is user-facing", "تبقت لك {0} محاولة" in service)
check("lock alert is queued", "QueueAccountLockedAlert" in service)
check("email failures are isolated", "ApplicationLogger.LogException(\"تهيئة تنبيه قفل الحساب\"" in service)
check("email is asynchronous", "Task.Run" in email)
check("email failures are logged", "ApplicationLogger.LogException(\"تنبيه قفل الحساب بالبريد\"" in email)
check("smtp is disabled by default", 'SecurityAlertEmailEnabled" value="false"' in config)
for key in ("SecurityAlertSmtpHost", "SecurityAlertSmtpPort", "SecurityAlertEnableSsl", "SecurityAlertSmtpUser", "SecurityAlertSmtpPassword", "SecurityAlertFromEmail"):
    check(f"config contains {key}", key in config)
check("transient remaining-attempts field exists", "RemainingLoginAttempts" in model)
check("email service is included in project", 'Services\\EmailNotificationService.cs' in project)
check("legacy password migration is supported", "VerifyLegacyPassword" in hasher and "migratedHash" in service)
check("legacy migration uses the existing reset path", "ResetPasswordByUserName(user.UserName, migratedHash, migratedSalt)" in service)
check("recovery script resets lock counters", "FailedLoginAttempts = 0" in recovery and "LockedAt = NULL" in recovery)
check("recovery script does not alter passwords", "PasswordHash" not in recovery and "PasswordSalt" not in recovery)
check("locked account message is explicit", "تم تعطيل حسابك مؤقتاً" in service and "تم تعطيل الحساب" in login)
check("inactive account message is explicit", "هذا الحساب غير نشط حالياً" in service)
check("remaining attempts use warning dialog", "message.Contains(\"تبقت لك\")" in login and "ShowWarning(message)" in login)
check("password whitespace is preserved", "password = NormalizeDigits(password);" in service and "password = NormalizeDigits(password).Trim();" not in service)
check("schema errors have actionable message", "Databass\\\\Migration_Step1.sql" in login and "قاعدة البيانات غير محدثة" in login)

failed = [name for name, ok in checks if not ok]
for name, ok in checks:
    print(("PASS: " if ok else "FAIL: ") + name)
if failed:
    raise SystemExit("Lockout feature checks failed: " + ", ".join(failed))
print(f"PASS: {len(checks)} lockout feature checks")
