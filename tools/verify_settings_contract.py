#!/usr/bin/env python3
"""Static, read-only contract checks for SchoolSystem settings wiring."""
from pathlib import Path
import sys

ROOT = Path(__file__).resolve().parents[1]
settings = (ROOT / "Services" / "ApplicationSettingsService.cs").read_text(encoding="utf-8")
db_connection = (ROOT / "DataAccess" / "DbConnection.cs").read_text(encoding="utf-8")
settings_form = (ROOT / "UI" / "SettingsForm.cs").read_text(encoding="utf-8")
backup_service = (ROOT / "Services" / "DatabaseBackupService.cs").read_text(encoding="utf-8")

checks = {
    "settings_validation_is_called_before_write": "Validate(value);" in settings and "File.Create(temporaryFile)" in settings,
    "server_and_database_are_trimmed": "ServerInstance = (value.ServerInstance ?? string.Empty).Trim();" in settings
    and "DatabaseName = (value.DatabaseName ?? string.Empty).Trim();" in settings,
    "backup_path_requires_absolute_path": "Path.IsPathRooted(value.BackupDirectory)" in settings,
    "runtime_connection_can_reload": "public static void Reload()" in db_connection,
    "saved_settings_are_loaded_by_runtime_connection": "ApplicationSettingsService.Load()" in db_connection,
    "settings_screen_reloads_connection_after_save": "DbConnection.Reload();" in settings_form,
    "backup_validates_server_identifier": "normalizedServer.Any(char.IsControl)" in backup_service
    and "normalizedServer.IndexOf(';')" in backup_service,
    "backup_validates_database_identifier": "SafeIdentifier.IsMatch(normalizedDatabase)" in backup_service,
    "backup_rejects_program_directory": "TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)" in backup_service,
    "restore_uses_single_user_for_replace": "SET SINGLE_USER WITH ROLLBACK IMMEDIATE" in backup_service,
    "restore_returns_to_multi_user_on_success": "SET MULTI_USER;" in backup_service,
    "restore_attempts_multi_user_after_failure": "BEGIN CATCH" in backup_service
    and "Preserve the original RESTORE error" in backup_service,
    "restore_reports_existing_database": "قاعدة البيانات الهدف موجودة. اختر اسماً جديداً أو فعّل الاستبدال." in backup_service,
}

failed = [name for name, passed in checks.items() if not passed]
for name, passed in checks.items():
    print(f"{'PASS' if passed else 'FAIL'}: {name}")

if failed:
    print(f"FAIL: {len(failed)} settings contract check(s) failed", file=sys.stderr)
    sys.exit(1)

print(f"PASS: {len(checks)} settings contract checks")
