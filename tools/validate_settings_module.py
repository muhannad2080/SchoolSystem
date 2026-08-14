from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]

def text(path):
    return (ROOT / path).read_text(encoding="utf-8")

required = [
    "Services/ApplicationSettingsService.cs",
    "Services/DatabaseBackupService.cs",
    "UI/SettingsForm.cs",
    "UI/SettingsForm.Designer.cs",
    "UI/StudentProfileForm.cs",
    "UI/StudentProfileForm.Designer.cs",
    "Databass/Migration_SettingsBackup.sql",
]
for item in required:
    assert (ROOT / item).exists(), item

project = text("SchoolSystem.csproj")
for item in [
    r"Services\ApplicationSettingsService.cs",
    r"Services\DatabaseBackupService.cs",
    r"UI\SettingsForm.cs",
    r"UI\SettingsForm.Designer.cs",
]:
    assert item in project, item

permission_keys = text("Security/PermissionKeys.cs")
assert 'SettingsManage = "Settings.Manage"' in permission_keys
assert "SettingsManage" in permission_keys.split("public static IReadOnlyList<string> All", 1)[1]

main = text("MainForm.cs")
designer = text("MainForm.Designer.cs")
assert "PermissionKeys.SettingsManage" in main
assert "tsmiSettings.Click" in designer
assert "private ToolStripMenuItem tsmiSettings;" not in main
assert main.count("ConfigureSettingsMenu") == 0

users = text("UI/UsersForm.cs")
assert "PermissionKeys.SettingsManage" in users

for migration in ["Databass/Migration_RBAC_Hardening.sql", "Databass/Migration_Step1.sql", "Databass/Migration_SettingsBackup.sql"]:
    assert "Settings.Manage" in text(migration), migration

backup = text("Services/DatabaseBackupService.cs")
assert "BACKUP DATABASE" in backup
assert "RESTORE DATABASE" in backup
assert "CHECKSUM" in backup
assert "SafeIdentifier" in backup

settings = text("UI/SettingsForm.cs")
assert "ApplicationSettingsService.Save" in settings
settings_designer = text("UI/SettingsForm.Designer.cs")
assert "foreach" not in settings_designer
student = text("UI/StudentProfileForm.cs")
student_designer = text("UI/StudentProfileForm.Designer.cs")
assert "partial class StudentProfileForm" in student
assert "InitializeComponent();" in student
assert "InitializeComponent()" in student_designer
assert "this.RightToLeftLayout" not in student_designer
assert "ConfigureGrid(" not in student_designer
assert "StudentProfileForm.Designer.cs" in project
assert "backupService.Backup" in settings
assert "backupService.Restore" in settings

print("PASS: settings module, designer wiring, RBAC UI, migrations, and backup safeguards")
