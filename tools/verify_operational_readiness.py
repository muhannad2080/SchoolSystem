"""Static operational-readiness checks for critical WinForms workflows."""
from pathlib import Path
import sys

ROOT = Path(__file__).resolve().parents[1]
backup = (ROOT / "Services" / "DatabaseBackupService.cs").read_text(encoding="utf-8", errors="ignore")
settings = (ROOT / "UI" / "SettingsForm.cs").read_text(encoding="utf-8", errors="ignore")
current_user = (ROOT / "Security" / "CurrentUser.cs").read_text(encoding="utf-8", errors="ignore")
main_form = (ROOT / "MainForm.cs").read_text(encoding="utf-8", errors="ignore")
forms = list((ROOT / "UI").glob("*.cs"))

checks = {
    "backup_normalizes_full_directory_path": "Path.GetFullPath(backupDirectory.Trim())" in backup,
    "backup_rejects_application_directory": "applicationDirectory" in backup and "StartsWith" in backup,
    "backup_creates_directory": "Directory.CreateDirectory" in backup,
    "backup_uses_database_identifier_validation": "SafeIdentifier.IsMatch" in backup,
    "restore_has_single_user_guard": "SINGLE_USER WITH ROLLBACK IMMEDIATE" in backup,
    "restore_restores_multi_user": "SET MULTI_USER;" in backup,
    "settings_backup_has_error_handling": "BackupButton_Click" in settings and "catch" in settings,
    "settings_restore_has_error_handling": "RestoreButton_Click" in settings and "catch" in settings,
    "permission_failures_are_explicit": "UnauthorizedAccessException" in current_user,
    "main_form_handles_screen_errors": "تعذر تحميل الشاشة" in main_form or "ShowException" in main_form,
    "ui_forms_use_exception_feedback": any("ShowException" in p.read_text(encoding="utf-8", errors="ignore") for p in forms),
}

failed = []
for name, passed in checks.items():
    print(f"{'PASS' if passed else 'FAIL'}: {name}")
    if not passed:
        failed.append(name)

if failed:
    print("FAIL: operational readiness checks: " + ", ".join(failed), file=sys.stderr)
    sys.exit(1)

print(f"PASS: {len(checks)} operational readiness checks")
