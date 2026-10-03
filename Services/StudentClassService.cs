using System;
using System.Collections.Generic;
using System.Data;
using SchoolSystem.DataAccess;
using SchoolSystem.Models;
using SchoolSystem.Security;


namespace SchoolSystem.Services
{

    public class StudentClassService
    {
        // تعريف الدالة StudentClassRepository لتنفيذ العملية المرتبطة بها.
        private readonly StudentClassRepository repository = new StudentClassRepository();
        // تعريف الدالة AuditLogService لتنفيذ العملية المرتبطة بها.
        private readonly AuditLogService auditLogService = new AuditLogService();

        // تعريف الدالة GetUnassignedStudents لتنفيذ العملية المرتبطة بها.
        public DataTable GetUnassignedStudents(string academicYear)
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.DemandAction("ClassAssignment", "View", "ليس لديك صلاحية عرض قوائم توزيع الطلاب.");
            ValidateAcademicYear(academicYear);

            return repository.GetUnassignedStudents(academicYear);
        }

        // تعريف الدالة GetSections لتنفيذ العملية المرتبطة بها.
        public DataTable GetSections(int classId, string academicYear)
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.DemandAny("ليس لديك صلاحية قراءة الشعب الدراسية.",
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                PermissionKeys.ClassAssignmentView,
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                PermissionKeys.ClassAssignmentManage,
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                PermissionKeys.EnrollmentManage,
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                PermissionKeys.AttendanceManage,
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                PermissionKeys.GradesManage,
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                PermissionKeys.TimetableManage,
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                PermissionKeys.ReportsView);
            if (classId <= 0)

                throw new ArgumentException("يجب اختيار الصف.");

            ValidateAcademicYear(academicYear);

            return repository.GetSections(classId, academicYear.Trim());
        }

        // تعريف الدالة GetAssignedStudents لتنفيذ العملية المرتبطة بها.
        public DataTable GetAssignedStudents(int classId, string section, string academicYear)
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.DemandAction("ClassAssignment", "View", "ليس لديك صلاحية عرض الطلاب الموزعين.");
            if (classId <= 0)

                throw new ArgumentException("يجب اختيار الصف.");


            if (string.IsNullOrWhiteSpace(section))

                throw new ArgumentException("يجب اختيار الشعبة.");

            ValidateAcademicYear(academicYear);


            return repository.GetAssignedStudents(classId, section.Trim(), academicYear.Trim());
        }

        // تعريف الدالة AssignStudent لتنفيذ العملية المرتبطة بها.
        public bool AssignStudent(StudentClass assignment)
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.DemandAction("ClassAssignment", "Add", "ليس لديك صلاحية توزيع الطلاب.");
            if (CurrentUser.User != null)
                assignment.AssignedBy = CurrentUser.User.UserID;
            ValidateAssignment(assignment);

            if (repository.IsStudentAssignedInYear(assignment.StudentID, assignment.AcademicYear))

                throw new ArgumentException("هذا الطالب موزع مسبقاً في نفس العام الدراسي.");

            bool assigned = repository.AssignStudent(assignment);
            if (assigned)
            {
                auditLogService.Record(
                    "توزيع طالب على فصل",
                    "StudentClass",
                    assignment.StudentClassID.ToString(),
                    "تم توزيع الطالب رقم " + assignment.StudentID + " على الصف رقم " + assignment.ClassID + " للعام " + assignment.AcademicYear);
            }

            return assigned;
        }

        // تعريف الدالة RemoveAssignment لتنفيذ العملية المرتبطة بها.
        public bool RemoveAssignment(int studentClassId)
        {
            CurrentUser.DemandAction("ClassAssignment", "Delete", "ليس لديك صلاحية إلغاء توزيع الطلاب.");
            if (studentClassId <= 0)

                throw new ArgumentException("اختر طالباً موزعاً من الجدول أولاً.");

            bool removed = repository.RemoveAssignment(studentClassId);
            if (removed)
            {
                auditLogService.Record(
                    "إلغاء توزيع طالب",
                    "StudentClass",
                    studentClassId.ToString(),
                    "تم إلغاء توزيع الطالب من السجل رقم " + studentClassId);
            }

            return removed;
        }

        // تعريف الدالة RemoveAssignments لتنفيذ العملية المرتبطة بها.
        public int RemoveAssignments(IList<int> studentClassIds)
        {
            CurrentUser.DemandAction("ClassAssignment", "Delete", "ليس لديك صلاحية الإلغاء الجماعي لتوزيع الطلاب.");
            if (studentClassIds == null || studentClassIds.Count == 0)

                throw new ArgumentException("حدد طالباً موزعاً واحداً على الأقل.");

            int removed = repository.RemoveAssignments(studentClassIds);
            if (removed > 0)
            {
                auditLogService.Record("إلغاء توزيع جماعي", "StudentClass", string.Join(",", studentClassIds),
                    "تم إلغاء توزيع " + removed + " سجل من الطلاب");
            }

            return removed;
        }

        // تعريف الدالة TransferAssignment لتنفيذ العملية المرتبطة بها.
        public bool TransferAssignment(int studentClassId, int targetClassId, string targetSection, string academicYear)
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.DemandAction("ClassAssignment", "Edit", "ليس لديك صلاحية نقل توزيع الطالب.");
            if (studentClassId <= 0)

                throw new ArgumentException("حدد طالباً موزعاً من الجدول.");

            if (targetClassId <= 0)

                throw new ArgumentException("يجب اختيار الصف الهدف.");

            if (string.IsNullOrWhiteSpace(targetSection))

                throw new ArgumentException("يجب اختيار الشعبة الهدف.");
            ValidateAcademicYear(academicYear);
            if (!repository.ClassExists(targetClassId))

                throw new ArgumentException("الصف الهدف غير موجود أو غير نشط.");

            bool transferred = repository.TransferAssignment(studentClassId, targetClassId, targetSection.Trim(), academicYear.Trim());
            if (transferred)
            {
                auditLogService.Record("نقل طالب بين الشعب", "StudentClass", studentClassId.ToString(),
                    "تم نقل سجل التوزيع إلى الصف " + targetClassId + " والشعبة " + targetSection.Trim());
            }

            return transferred;
        }

        // تعريف الدالة GetSectionStatistics لتنفيذ العملية المرتبطة بها.
        public DataTable GetSectionStatistics(int classId, string academicYear)
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.DemandAny("ليس لديك صلاحية قراءة إحصائيات التوزيع.",
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                PermissionKeys.ClassAssignmentView, PermissionKeys.ClassAssignmentManage, PermissionKeys.ReportsView);
            if (classId <= 0)

                throw new ArgumentException("يجب اختيار الصف.");
            ValidateAcademicYear(academicYear);

            return repository.GetSectionStatistics(classId, academicYear.Trim());
        }

        // تعريف الدالة ValidateAssignment لتنفيذ العملية المرتبطة بها.
        private void ValidateAssignment(StudentClass assignment)
        {
            if (assignment == null)

                throw new ArgumentException("بيانات التوزيع غير صحيحة.");


            if (assignment.StudentID <= 0)

                throw new ArgumentException("بيانات الطالب غير صحيحة.");


            if (assignment.ClassID <= 0)

                throw new ArgumentException("يجب اختيار الصف.");

            assignment.Section = (assignment.Section ?? string.Empty).Trim();
            if (assignment.Section.Length == 0 || assignment.Section.Length > 100)

                throw new ArgumentException("يجب اختيار شعبة صحيحة بطول لا يتجاوز 100 حرف.");

            assignment.AcademicYear = (assignment.AcademicYear ?? string.Empty).Trim().Replace('-', '/');
            ValidateAcademicYear(assignment.AcademicYear);

            if (!repository.StudentExists(assignment.StudentID))

                throw new ArgumentException("الطالب المحدد غير موجود أو غير نشط.");


            if (!repository.ClassExists(assignment.ClassID))

                throw new ArgumentException("الصف المحدد غير موجود أو غير نشط.");
        }

        // تعريف الدالة ValidateAcademicYear لتنفيذ العملية المرتبطة بها.
        private void ValidateAcademicYear(string academicYear)
        {
            if (string.IsNullOrWhiteSpace(academicYear))

                throw new ArgumentException("العام الدراسي مطلوب.");

            string normalized = academicYear.Trim().Replace('-', '/');
            string[] parts = normalized.Split('/');
            if (parts.Length != 2 ||
                parts[0].Length != 4 ||
                parts[1].Length != 4 ||
                !int.TryParse(parts[0], out int firstYear) ||
                !int.TryParse(parts[1], out int secondYear) ||
                secondYear != firstYear + 1)
            {

                throw new ArgumentException("صيغة العام الدراسي يجب أن تكون متسلسلة مثل 2026/2027 أو 1447-1448.");
            }
        }
    }
}
