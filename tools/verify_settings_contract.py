#!/usr/bin/env python3
"""Static, read-only contract checks for settings, backup/restore, RBAC, and safe deletion."""
from pathlib import Path
import subprocess
import sys

ROOT = Path(__file__).resolve().parents[1]
settings = (ROOT / "Services" / "ApplicationSettingsService.cs").read_text(encoding="utf-8")
db_connection = (ROOT / "DataAccess" / "DbConnection.cs").read_text(encoding="utf-8")
settings_form = (ROOT / "UI" / "SettingsForm.cs").read_text(encoding="utf-8")
backup_service = (ROOT / "Services" / "DatabaseBackupService.cs").read_text(encoding="utf-8")
current_user = (ROOT / "Security" / "CurrentUser.cs").read_text(encoding="utf-8")
permission_keys = (ROOT / "Security" / "PermissionKeys.cs").read_text(encoding="utf-8")
settings_ui = (ROOT / "UI" / "SettingsForm.cs").read_text(encoding="utf-8")
student_service = (ROOT / "Services" / "StudentService.cs").read_text(encoding="utf-8")
teacher_service = (ROOT / "Services" / "TeacherService.cs").read_text(encoding="utf-8")
teacher_repository = (ROOT / "DataAccess" / "TeacherRepository.cs").read_text(encoding="utf-8")
student_repository = (ROOT / "DataAccess" / "StudentRepository.cs").read_text(encoding="utf-8")
fee_repository = (ROOT / "DataAccess" / "FeeRepository.cs").read_text(encoding="utf-8")
expense_repository = (ROOT / "DataAccess" / "ExpenseRepository.cs").read_text(encoding="utf-8")
payroll_repository = (ROOT / "DataAccess" / "PayrollRepository.cs").read_text(encoding="utf-8")
voucher_repository = (ROOT / "DataAccess" / "VoucherRepository.cs").read_text(encoding="utf-8")
report_repository = (ROOT / "DataAccess" / "ReportRepository.cs").read_text(encoding="utf-8")
student_attendance_repository = (ROOT / "DataAccess" / "StudentAttendanceRepository.cs").read_text(encoding="utf-8")
grade_repository = (ROOT / "DataAccess" / "GradeRepository.cs").read_text(encoding="utf-8")
enrollment_repository = (ROOT / "DataAccess" / "EnrollmentRepository.cs").read_text(encoding="utf-8")
borrowing_repository = (ROOT / "DataAccess" / "BorrowingRepository.cs").read_text(encoding="utf-8")
student_class_repository = (ROOT / "DataAccess" / "StudentClassRepository.cs").read_text(encoding="utf-8")
student_attendance_service = (ROOT / "Services" / "StudentAttendanceService.cs").read_text(encoding="utf-8")
student_class_service = (ROOT / "Services" / "StudentClassService.cs").read_text(encoding="utf-8")
subject_service = (ROOT / "Services" / "SubjectService.cs").read_text(encoding="utf-8")
room_service = (ROOT / "Services" / "RoomService.cs").read_text(encoding="utf-8")
contract_service = (ROOT / "Services" / "ContractService.cs").read_text(encoding="utf-8")
fee_plan_service = (ROOT / "Services" / "FeePlanService.cs").read_text(encoding="utf-8")
room_repository = (ROOT / "DataAccess" / "RoomRepository.cs").read_text(encoding="utf-8")
class_repository = (ROOT / "DataAccess" / "ClassRepository.cs").read_text(encoding="utf-8")
class_service = (ROOT / "Services" / "ClassService.cs").read_text(encoding="utf-8")
user_repository = (ROOT / "DataAccess" / "UserRepository.cs").read_text(encoding="utf-8")
user_service = (ROOT / "Services" / "UserService.cs").read_text(encoding="utf-8")
payroll_ui = (ROOT / "UI" / "PayrollForm.cs").read_text(encoding="utf-8")
staff_attendance_ui = (ROOT / "UI" / "StaffAttendanceForm.cs").read_text(encoding="utf-8")
main_form = (ROOT / "MainForm.cs").read_text(encoding="utf-8")
main_designer = (ROOT / "MainForm.Designer.cs").read_text(encoding="utf-8")
library_ui = (ROOT / "UI" / "LibraryForm.cs").read_text(encoding="utf-8")
enrollment_ui = (ROOT / "UI" / "EnrollmentForm.cs").read_text(encoding="utf-8")
financial_services = "\\n".join(
    (ROOT / "Services" / name).read_text(encoding="utf-8")
    for name in ("FeeService.cs", "ExpenseService.cs", "PayrollService.cs", "VoucherService.cs")
)
academic_services = "\\n".join(
    (ROOT / "Services" / name).read_text(encoding="utf-8")
    for name in ("EnrollmentService.cs", "GradeService.cs", "TimetableService.cs")
)

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
    "permission_demand_throws_on_missing_permission": "throw new UnauthorizedAccessException" in current_user
    and "public static void DemandPermission" in current_user,
    "permission_keys_include_settings_management": "SettingsManage" in permission_keys,
    "settings_backup_requires_settings_permission": "PermissionKeys.SettingsManage" in settings_ui
    and "BackupButton_Click" in settings_ui
    and "RestoreButton_Click" in settings_ui,
    "student_mutations_require_students_manage": student_service.count("EnsureCanManageStudents();") >= 4,
    "teacher_mutations_require_teachers_manage": teacher_service.count("PermissionKeys.TeachersManage") >= 3,
    "teacher_delete_checks_all_historical_dependencies": all(
        table in teacher_repository
        for table in ("TeacherContracts", "TeacherAttendance", "Payroll", "SchoolTimetable")
    ),
    "teacher_delete_is_atomic_and_serializable": "BeginTransaction(IsolationLevel.Serializable)" in teacher_repository
    and "transaction.Commit();" in teacher_repository,
    "teacher_delete_explains_dependency_protection": "عطّل المعلم بدلاً من حذفه" in teacher_repository,
    "financial_mutations_require_financial_permissions": all(
        token in financial_services
        for token in (
            "PermissionKeys.FeesManage",
            "PermissionKeys.ExpensesManage",
            "PermissionKeys.PayrollManage",
            "PermissionKeys.VouchersManage",
        )
    ),
    "paid_fee_deletion_is_blocked": "PaidAmount" in fee_repository
    and "PaymentDate IS NOT NULL" in fee_repository
    and "لا يمكن حذف رسم تم تسجيل دفعة عليه" in fee_repository,
    "expense_deletion_is_blocked_when_vouchered": "FROM Vouchers" in expense_repository
    and "ReferenceType = N'مصروفات'" in expense_repository
    and "لا يمكن حذف المصروف لأنه مرتبط بسند مالي أو تسوية" in expense_repository,
    "paid_payroll_deletion_is_blocked": "PaymentDate IS NOT NULL" in payroll_repository
    and "PaymentDate IS NULL" in payroll_repository
    and "لا يمكن حذف راتب تم صرفه" in payroll_repository,
    "auto_voucher_update_is_blocked": "IsAutoGenerated = 1" in voucher_repository
    and "IsAutoGenerated = 0" in voucher_repository
    and "لا يمكن تعديل السند المنشأ تلقائياً" in voucher_repository,
    "reports_use_explicit_columns": "SELECT * FROM StudentFees" not in report_repository
    and "SELECT * FROM Receipts" not in report_repository
    and "SELECT * FROM Grades" not in report_repository
    and "StudentFeeID AS [المعرف]" in report_repository
    and "GradeID AS [المعرف]" in report_repository,
    "academic_mutations_require_academic_permissions": all(
        token in academic_services
        for token in (
            "PermissionKeys.EnrollmentManage",
            "PermissionKeys.GradesManage",
            "PermissionKeys.TimetableManage",
        )
    ),
    "student_delete_is_soft_delete": "SET Status = N'محذوف'" in (ROOT / "DataAccess" / "StudentRepository.cs").read_text(encoding="utf-8"),
    "room_delete_is_soft_delete": "SET IsActive = 0" in room_repository
    and "DELETE FROM Rooms" not in room_repository,
    "class_update_protects_last_active_class": "IsolationLevel.Serializable" in class_repository
    and "WHERE ISNULL(IsActive, 1) = 1" in class_repository
    and "لا يمكن تعطيل آخر فصل نشط" in class_repository,
    "class_updates_are_audited": "AuditLogService" in class_service
    and "auditLogService.Record" in class_service,
    "user_delete_is_atomic": "BeginTransaction(IsolationLevel.Serializable)" in user_repository
    and "transaction.Commit();" in user_repository,
    "user_delete_protects_last_admin": "لا يمكن حذف آخر مدير نظام نشط" in user_repository
    and "activeAdminCount <= 1" in user_repository,
    "user_delete_protects_current_user": "protectedUserId" in user_repository
    and "لا يمكن حذف المستخدم المسجل دخوله حالياً" in user_repository
    and "DeleteUser(userId, protectedUserId)" in user_service,
    "active_teacher_lookup_is_available": "public DataTable GetActiveTeachers()" in teacher_repository
    and "return GetTeachers(\"نشط\")" in teacher_repository
    and "_repository.GetActiveTeachers()" in teacher_service,
    "operational_teacher_lists_exclude_inactive": all(
        "GetActiveTeachers()" in content
        for content in (payroll_ui, staff_attendance_ui, library_ui)
    ),
    "active_student_lookup_is_available": "public List<Student> GetActive()" in student_repository
    and "Status, \"نشط\"" in student_repository
    and "public DataTable GetActiveStudents()" in student_service,
    "operational_student_lists_exclude_inactive": all(
        "GetActiveStudents()" in content
        for content in (
            (ROOT / "UI" / "EnrollmentForm.cs").read_text(encoding="utf-8"),
            (ROOT / "UI" / "FeesForm.cs").read_text(encoding="utf-8"),
            (ROOT / "UI" / "LibraryForm.cs").read_text(encoding="utf-8"),
        )
    ),
    "attendance_uses_active_student_status": "ISNULL(Status, N'نشط') = N'نشط'" in student_attendance_repository
    and "لا يمكن تسجيل حضور لطالب غير نشط" in student_attendance_repository,
    "grades_use_active_student_status": "ISNULL(s.Status, N'نشط') = N'نشط'" in grade_repository
    and "لا يمكن حفظ درجة لطالب غير نشط" in grade_repository,
    "enrollment_requires_active_student": "لا يمكن تسجيل طالب غير نشط" in enrollment_repository
    and "لا يمكن ربط تسجيل بطالب غير نشط" in enrollment_repository,
    "borrowing_requires_active_borrower": "لا يمكن إنشاء إعارة لطالب غير نشط" in borrowing_repository
    and "لا يمكن إنشاء إعارة لمعلم غير نشط" in borrowing_repository,
    "student_assignment_uses_active_status": "ISNULL(s.Status, N'نشط') = N'نشط'" in student_class_repository
    and "لا يمكن تعيين طالب غير نشط" in student_class_repository,
    "student_assignment_requires_active_class": "ISNULL(IsActive, 1) = 1" in student_class_repository
    and "لا يمكن التعيين إلى فصل غير نشط" in student_class_repository,
    "enrollment_form_validates_selected_ids": "GetSelectedId(cmbStudentID" in enrollment_ui
    and "GetSelectedId(cmbClassID" in enrollment_ui
    and "int.TryParse(cmbStudentID.SelectedValue.ToString()" in enrollment_ui,
    "enrollment_form_validates_record_id": "int.TryParse(txtEnrollmentID.Text.Trim(), out int enrollmentId)" in enrollment_ui
    and "int.TryParse(txtEnrollmentID.Text.Trim(), out int id)" in enrollment_ui,
    "student_attendance_mutations_are_audited": "private readonly AuditLogService auditLogService" in student_attendance_service
    and "auditLogService.Record(" in student_attendance_service
    and "if (saved)" in student_attendance_service,
    "student_class_assignment_mutations_are_audited": "private readonly AuditLogService auditLogService" in student_class_service
    and student_class_service.count("auditLogService.Record(") >= 2
    and "if (assigned)" in student_class_service
    and "if (removed)" in student_class_service,
    "subject_mutations_are_audited": "private readonly AuditLogService auditLogService" in subject_service
    and "bool updated = repository.UpdateSubject(subject);" in subject_service
    and "auditLogService.Record(" in subject_service
    and "if (updated)" in subject_service,
    "room_mutations_are_audited": "private readonly AuditLogService auditLogService" in room_service
    and room_service.count("auditLogService.Record(") >= 3
    and "if (added)" in room_service
    and "if (updated)" in room_service
    and "if (deleted)" in room_service,
    "contract_mutations_are_audited": "private readonly AuditLogService auditLogService" in contract_service
    and contract_service.count("auditLogService.Record(") >= 3
    and "if (added)" in contract_service
    and "if (updated)" in contract_service
    and "if (deleted)" in contract_service,
    "user_password_reset_is_audited_without_secret": "ResetPasswordByUserName" in user_service
    and "إعادة تعيين كلمة المرور" in user_service
    and "دون تسجيل كلمة المرور" in user_service
    and "auditLogService.Record(" in user_service,
    "automatic_permission_sync_is_audited": "تحديث صلاحيات تلقائي" in user_service
    and "userRepository.UpdatePermissions(user.UserID, normalized);" in user_service
    and "auditLogService.Record(" in user_service,
    "fee_plan_mutations_are_audited": "private readonly AuditLogService auditLogService" in fee_plan_service
    and fee_plan_service.count("auditLogService.Record(") >= 3
    and "if (added)" in fee_plan_service
    and "if (updated)" in fee_plan_service
    and "if (deleted)" in fee_plan_service,
    "main_logout_preserves_application_owner": "Hide();" in main_form
    and "using (LoginForm loginForm = new LoginForm())" in main_form
    and "Show();" in main_form
    and "Application.Exit();" in main_form
    and "MainForm_FormClosed" in main_form,
    "main_session_refresh_reapplies_permissions": "RefreshCurrentUserSession();" in main_form
    and "ApplyCurrentUserPermissions();" in main_form
    and "CurrentUser.Clear();" in main_form,
    "main_admin_sees_complete_menu_catalog": all(
        token + ".Visible = true;" in main_form
        for token in (
            "tsmiDashboard", "tsmiStudents", "tsmiTeachers", "tsmiAcademic",
            "tsmiAttendance", "tsmiFinancial", "tsmiTransport", "tsmiLibrary",
            "tsmiUsers", "tsmiReports", "tsmiAuditLogs", "tsmiSettings"
        )
    ),
    "main_logout_is_last_design_menu_item": (
        "this.tsmiUsers,\n            this.tsmiAuditLogs,\n            this.tsmiSettings" in main_designer
        and "this.tsmiReports,\n            this.tsmiLogout" in main_designer
        and main_designer.rfind("this.tsmiLogout") > main_designer.rfind("this.tsmiReports")
    ),
}

failed = [name for name, passed in checks.items() if not passed]
for name, passed in checks.items():
    print(f"{'PASS' if passed else 'FAIL'}: {name}")

if failed:
    print(f"FAIL: {len(failed)} settings contract check(s) failed", file=sys.stderr)
    sys.exit(1)

coverage_script = ROOT / "tools" / "verify_service_audit_coverage.py"
ui_script = ROOT / "tools" / "verify_rtl_ui_contract.py"
readiness_script = ROOT / "tools" / "verify_operational_readiness.py"
validation_script = ROOT / "tools" / "verify_validation_contract.py"
ui_save_script = ROOT / "tools" / "inventory_ui_save_validation.py"
search_script = ROOT / "tools" / "verify_search_contract.py"
autocomplete_search_script = ROOT / "tools" / "verify_search_autocomplete_contract.py"
coverage_result = subprocess.run(
    [sys.executable, str(coverage_script)],
    cwd=str(ROOT),
    text=True,
    capture_output=True,
)
if coverage_result.stdout:
    print(coverage_result.stdout.rstrip())
if coverage_result.returncode != 0:
    if coverage_result.stderr:
        print(coverage_result.stderr.rstrip(), file=sys.stderr)
    print("FAIL: service audit coverage check", file=sys.stderr)
    sys.exit(1)

ui_result = subprocess.run(
    [sys.executable, str(ui_script)],
    cwd=str(ROOT),
    text=True,
    capture_output=True,
)
if ui_result.stdout:
    print(ui_result.stdout.rstrip())
if ui_result.returncode != 0:
    if ui_result.stderr:
        print(ui_result.stderr.rstrip(), file=sys.stderr)
    print("FAIL: RTL/designer contract check", file=sys.stderr)
    sys.exit(1)

readiness_result = subprocess.run(
    [sys.executable, str(readiness_script)],
    cwd=str(ROOT),
    text=True,
    capture_output=True,
)
if readiness_result.stdout:
    print(readiness_result.stdout.rstrip())
if readiness_result.returncode != 0:
    if readiness_result.stderr:
        print(readiness_result.stderr.rstrip(), file=sys.stderr)
    print("FAIL: operational readiness check", file=sys.stderr)
    sys.exit(1)
validation_result = subprocess.run(
    [sys.executable, str(validation_script)],
    cwd=str(ROOT),
    text=True,
    capture_output=True,
)
if validation_result.stdout:
    print(validation_result.stdout.rstrip())
if validation_result.returncode != 0:
    if validation_result.stderr:
        print(validation_result.stderr.rstrip(), file=sys.stderr)
    print("FAIL: UI validation coverage check", file=sys.stderr)
    sys.exit(1)
ui_save_result = subprocess.run(
    [sys.executable, str(ui_save_script)],
    cwd=str(ROOT),
    text=True,
    capture_output=True,
)
if ui_save_result.stdout:
    print(ui_save_result.stdout.rstrip())
if ui_save_result.returncode != 0 or "REVIEW:" in ui_save_result.stdout:
    if ui_save_result.stderr:
        print(ui_save_result.stderr.rstrip(), file=sys.stderr)
    print("FAIL: UI save-handler validation check", file=sys.stderr)
    sys.exit(1)
print("PASS: UI save-handler validation check")
search_result = subprocess.run(
    [sys.executable, str(search_script)],
    cwd=str(ROOT),
    text=True,
    capture_output=True,
)
if search_result.stdout:
    print(search_result.stdout.rstrip())
if search_result.returncode != 0:
    if search_result.stderr:
        print(search_result.stderr.rstrip(), file=sys.stderr)
    print("FAIL: DataView search safety contract", file=sys.stderr)
    sys.exit(1)
print("PASS: DataView search safety contract")
autocomplete_search_result = subprocess.run(
    [sys.executable, str(autocomplete_search_script)],
    cwd=str(ROOT),
    text=True,
    capture_output=True,
)
if autocomplete_search_result.stdout:
    print(autocomplete_search_result.stdout.rstrip())
if autocomplete_search_result.returncode != 0:
    if autocomplete_search_result.stderr:
        print(autocomplete_search_result.stderr.rstrip(), file=sys.stderr)
    print("FAIL: search autocomplete contract", file=sys.stderr)
    sys.exit(1)
print("PASS: search autocomplete contract")
print(f"PASS: {len(checks)} settings contract checks")
