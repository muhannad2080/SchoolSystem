from pathlib import Path
import re
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parents[1]
service = (ROOT / "Services" / "UserService.cs").read_text(encoding="utf-8")
email = (ROOT / "Services" / "EmailNotificationService.cs").read_text(encoding="utf-8")
config = (ROOT / "App.config").read_text(encoding="utf-8")
project = (ROOT / "SchoolSystem.csproj").read_text(encoding="utf-8")
model = (ROOT / "Models" / "User.cs").read_text(encoding="utf-8")

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

failed = [name for name, ok in checks if not ok]
for name, ok in checks:
    print(("PASS: " if ok else "FAIL: ") + name)
if failed:
    raise SystemExit("Lockout feature checks failed: " + ", ".join(failed))
print(f"PASS: {len(checks)} lockout feature checks")
