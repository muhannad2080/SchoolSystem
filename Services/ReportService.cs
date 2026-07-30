using System;
using System.Data;
using SchoolSystem.DataAccess;
using SchoolSystem.Models;

namespace SchoolSystem.Services
{
    public class ReportService
    {
        private readonly ReportRepository repository = new ReportRepository();

        public DataTable GetReportData(ReportRequest request)
        {
            ValidateRequest(request);
            return repository.GetReportData(request);
        }

        // توافق مع الكود القديم لديك الذي يستدعي GetReport("Students")
        public DataTable GetReport(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return new DataTable();

            return repository.GetReport(key);
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
