#!/usr/bin/env python3
"""Static, read-only contract checks for settings, backup/restore, RBAC, and safe deletion."""
from pathlib import Path
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
student_attendance_repository = (ROOT / "DataAccess" / "StudentAttendanceRepository.cs").read_text(encoding="utf-8")
grade_repository = (ROOT / "DataAccess" / "GradeRepository.cs").read_text(encoding="utf-8")
enrollment_repository = (ROOT / "DataAccess" / "EnrollmentRepository.cs").read_text(encoding="utf-8")
borrowing_repository = (ROOT / "DataAccess" / "BorrowingRepository.cs").read_text(encoding="utf-8")
student_class_repository = (ROOT / "DataAccess" / "StudentClassRepository.cs").read_text(encoding="utf-8")
room_repository = (ROOT / "DataAccess" / "RoomRepository.cs").read_text(encoding="utf-8")
class_repository = (ROOT / "DataAccess" / "ClassRepository.cs").read_text(encoding="utf-8")
class_service = (ROOT / "Services" / "ClassService.cs").read_text(encoding="utf-8")
user_repository = (ROOT / "DataAccess" / "UserRepository.cs").read_text(encoding="utf-8")
user_service = (ROOT / "Services" / "UserService.cs").read_text(encoding="utf-8")
payroll_ui = (ROOT / "UI" / "PayrollForm.cs").read_text(encoding="utf-8")
staff_attendance_ui = (ROOT / "UI" / "StaffAttendanceForm.cs").read_text(encoding="utf-8")
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
}

failed = [name for name, passed in checks.items() if not passed]
for name, passed in checks.items():
    print(f"{'PASS' if passed else 'FAIL'}: {name}")

if failed:
    print(f"FAIL: {len(failed)} settings contract check(s) failed", file=sys.stderr)
    sys.exit(1)

print(f"PASS: {len(checks)} settings contract checks")
