using System;
using System.Data;
using SchoolSystem.DataAccess;
using SchoolSystem.Models;
using SchoolSystem.Security;


namespace SchoolSystem.Services
{

    public class ReportService
    {
        // تعريف الدالة ReportRepository لتنفيذ العملية المرتبطة بها.
        private readonly ReportRepository repository = new ReportRepository();

        // تعريف الدالة GetSections لتنفيذ العملية المرتبطة بها.
        public DataTable GetSections(int classId, string academicYear)
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.DemandPermission(PermissionKeys.ReportsView, "ليس لديك صلاحية عرض التقارير.");
            if (classId <= 0)
                return new DataTable();

            ValidateAcademicYear(academicYear);

            return repository.GetSections(classId, academicYear.Trim());
        }

        // تعريف الدالة FindStudentId لتنفيذ العملية المرتبطة بها.
        public int? FindStudentId(string searchText, string academicYear, int? classId, string section)
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.DemandPermission(PermissionKeys.ReportsView, "ليس لديك صلاحية عرض التقارير.");
            ValidateAcademicYear(academicYear);

            return repository.FindStudentId(searchText, academicYear, classId, section);
        }

        // تعريف الدالة GetReportData لتنفيذ العملية المرتبطة بها.
        public DataTable GetReportData(ReportRequest request)
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.DemandPermission(PermissionKeys.ReportsView, "ليس لديك صلاحية عرض التقارير.");
            ValidateRequest(request);

            return repository.GetReportData(request);
        }

        // توافق مع الكود القديم لديك الذي يستدعي GetReport("Students")
        // تعريف الدالة GetReport لتنفيذ العملية المرتبطة بها.
        public DataTable GetReport(string key)
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.DemandPermission(PermissionKeys.ReportsView, "ليس لديك صلاحية عرض التقارير.");
            if (string.IsNullOrWhiteSpace(key))
                return new DataTable();

            string activeAcademicYear = new AnnualClosingService().GetActiveAcademicYear();
            ValidateAcademicYear(activeAcademicYear);

            return repository.GetReport(key, activeAcademicYear);
        }

        // تعريف الدالة ValidateAcademicYear لتنفيذ العملية المرتبطة بها.
        private void ValidateAcademicYear(string academicYear)
        {
            if (string.IsNullOrWhiteSpace(academicYear))

                throw new ArgumentException("العام الدراسي مطلوب.");

            string[] parts = academicYear.Trim().Replace('-', '/').Split('/');
            int firstYear;
            int secondYear;
            if (parts.Length != 2 || parts[0].Length != 4 || parts[1].Length != 4 ||
                !int.TryParse(parts[0], out firstYear) || !int.TryParse(parts[1], out secondYear) ||
                secondYear != firstYear + 1)

                throw new ArgumentException("صيغة العام الدراسي يجب أن تكون متسلسلة مثل 2026/2027 أو 1447-1448.");
        }

        // تعريف الدالة ValidateRequest لتنفيذ العملية المرتبطة بها.
        private void ValidateRequest(ReportRequest request)
        {
            if (request == null)

                throw new ArgumentException("بيانات التقرير غير صحيحة.");


            if (string.IsNullOrWhiteSpace(request.ReportType))

                throw new ArgumentException("يجب اختيار نوع التقرير.");

            ValidateAcademicYear(request.AcademicYear);

            if (request.FromDate > request.ToDate)

                throw new ArgumentException("تاريخ البداية يجب أن يكون قبل تاريخ النهاية.");
        }
    }
}
