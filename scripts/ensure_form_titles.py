from pathlib import Path

TITLES = {
    "AuditLogForm": "سجل الأنشطة والتدقيق",
    "ClassAssignmentForm": "توزيع الطلاب على الفصول",
    "ClassesForm": "إدارة الفصول والقاعات",
    "DailyAttendanceForm": "التحضير اليومي",
    "ExpensesForm": "إدارة المصروفات",
    "FeePlansForm": "تعريف خطط الرسوم",
    "FeesForm": "إدارة الرسوم الدراسية",
    "GradeEntryForm": "إدخال الدرجات",
    "LibraryForm": "إدارة المكتبة",
    "PayrollForm": "العقود والرواتب",
    "ReportCenterForm": "مركز التقارير",
    "SettingsForm": "إعدادات النظام والنسخ الاحتياطي",
    "StaffAttendanceForm": "حضور وانصراف الموظفين",
    "StudentProfileForm": "ملف الطالب الموحد",
    "SubjectsForm": "إدارة المواد الدراسية",
    "TeachersForm": "إدارة المعلمين",
    "TimetableForm": "إدارة الجدول الدراسي",
    "TransportForm": "النقل المدرسي",
    "UsersForm": "إدارة المستخدمين والصلاحيات",
    "VouchersForm": "إدارة السندات المالية",
}

root = Path(__file__).resolve().parents[1] / "UI"
changed = []
for cls, title in TITLES.items():
    path = root / f"{cls}.Designer.cs"
    if not path.exists():
        continue
    text = path.read_text(encoding="utf-8")
    if "this.Text =" in text:
        continue
    marker = f'            this.Name = "{cls}";'
    if marker not in text:
        continue
    replacement = marker + f'\n            this.Text = "{title}";'
    path.write_text(text.replace(marker, replacement, 1), encoding="utf-8")
    changed.append(str(path.relative_to(root.parent)))
print("Updated:")
print("\n".join(changed))
print(f"Count: {len(changed)}")
