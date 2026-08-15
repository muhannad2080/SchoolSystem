using System;
using System.Data;
using SchoolSystem.DataAccess;
using SchoolSystem.Models;
using SchoolSystem.Security;

namespace SchoolSystem.Services
{
    public class ReportService
    {
        private readonly ReportRepository repository = new ReportRepository();

        public DataTable GetSections(int classId, string academicYear)
        {
            CurrentUser.DemandPermission(PermissionKeys.ReportsView, "ليس لديك صلاحية عرض التقارير.");
            if (classId <= 0)
                return new DataTable();

            ValidateAcademicYear(academicYear);
            return repository.GetSections(classId, academicYear.Trim());
        }

        public DataTable GetReportData(ReportRequest request)
        {
            CurrentUser.DemandPermission(PermissionKeys.ReportsView, "ليس لديك صلاحية عرض التقارير.");
            ValidateRequest(request);
            return repository.GetReportData(request);
        }

        // توافق مع الكود القديم لديك الذي يستدعي GetReport("Students")
        public DataTable GetReport(string key)
        {
            CurrentUser.DemandPermission(PermissionKeys.ReportsView, "ليس لديك صلاحية عرض التقارير.");
            if (string.IsNullOrWhiteSpace(key))
                return new DataTable();

            return repository.GetReport(key);
        }

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

        private void ValidateRequest(ReportRequest request)
        {
            if (request == null)
                throw new ArgumentException("بيانات التقرير غير صحيحة.");

            if (string.IsNullOrWhiteSpace(request.ReportType))
                throw new ArgumentException("يجب اختيار نوع التقرير.");

            if (request.FromDate > request.ToDate)
                throw new ArgumentException("تاريخ البداية يجب أن يكون قبل تاريخ النهاية.");
        }
    }
}
