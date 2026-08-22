using System;
using System.Data;
using SchoolSystem.DataAccess;
using SchoolSystem.Models;
using SchoolSystem.Security;

namespace SchoolSystem.Services
{
    public class FeePlanService
    {
        private readonly FeePlanRepository feePlanRepository;
        private readonly AuditLogService auditLogService = new AuditLogService();

        public FeePlanService()
        {
            feePlanRepository = new FeePlanRepository();
        }

        public DataTable GetAllFeePlans()
        {
            CurrentUser.DemandAction("FeePlans", "View", "ليس لديك صلاحية عرض خطط الرسوم.");
            return feePlanRepository.GetAllFeePlans();
        }

        public DataTable GetClasses()
        {
            CurrentUser.DemandAction("FeePlans", "View", "ليس لديك صلاحية عرض صفوف خطط الرسوم.");
            return feePlanRepository.GetClasses();
        }

        public bool AddFeePlan(FeePlan plan)
        {
            CurrentUser.DemandAction("FeePlans", "Add", "ليس لديك صلاحية إضافة خطط الرسوم.");
            Validate(plan);
            bool added = feePlanRepository.AddFeePlan(plan);
            if (added)
            {
                auditLogService.Record(
                    "إضافة خطة رسوم",
                    "FeePlan",
                    plan.FeePlanID.ToString(),
                    "تمت إضافة خطة رسوم للصف رقم " + plan.ClassID + " للعام " + plan.AcademicYear);
            }
            return added;
        }

        public bool UpdateFeePlan(FeePlan plan)
        {
            CurrentUser.DemandAction("FeePlans", "Edit", "ليس لديك صلاحية تعديل خطط الرسوم.");
            if (plan == null || plan.FeePlanID <= 0)
                throw new Exception("رقم خطة الرسوم غير صحيح.");

            Validate(plan);
            bool updated = feePlanRepository.UpdateFeePlan(plan);
            if (updated)
            {
                auditLogService.Record(
                    "تعديل خطة رسوم",
                    "FeePlan",
                    plan.FeePlanID.ToString(),
                    "تم تعديل خطة الرسوم رقم " + plan.FeePlanID);
            }
            return updated;
        }

        public bool DeleteFeePlan(int feePlanId)
        {
            CurrentUser.DemandAction("FeePlans", "Delete", "ليس لديك صلاحية حذف خطط الرسوم.");
            if (feePlanId <= 0)
                throw new Exception("رقم خطة الرسوم غير صحيح.");

            bool deleted = feePlanRepository.DeleteFeePlan(feePlanId);
            if (deleted)
            {
                auditLogService.Record(
                    "حذف خطة رسوم",
                    "FeePlan",
                    feePlanId.ToString(),
                    "تم حذف خطة الرسوم رقم " + feePlanId);
            }
            return deleted;
        }

        private void Validate(FeePlan plan)
        {
            if (plan == null)
                throw new Exception("بيانات الرسوم غير موجودة.");

            if (string.IsNullOrWhiteSpace(plan.AcademicYear))
                throw new Exception("يجب اختيار العام الدراسي.");

            if (plan.ClassID <= 0)
                throw new Exception("يجب اختيار الصف.");

            if (string.IsNullOrWhiteSpace(plan.FeeType))
                throw new Exception("يجب إدخال نوع الرسوم.");

            if (plan.Amount <= 0)
                throw new Exception("مبلغ الرسوم يجب أن يكون أكبر من صفر.");

            if (plan.Amount > 1000000000m)
                throw new Exception("مبلغ خطة الرسوم يتجاوز الحد المسموح.");

            if (plan.DueDate == DateTime.MinValue)
                throw new Exception("تاريخ استحقاق الخطة مطلوب.");

            if (plan.DueDate.Date < DateTime.Today.AddYears(-1))
                throw new Exception("تاريخ استحقاق الخطة قديم بشكل غير منطقي.");

            if (!string.IsNullOrWhiteSpace(plan.Notes) && plan.Notes.Trim().Length > 500)
                throw new Exception("ملاحظات الخطة لا يمكن أن تتجاوز 500 حرف.");

            string year = plan.AcademicYear.Trim()
                .Replace('-', '/')
                .Replace('–', '/');
            string[] parts = year.Split('/');
            int firstYear;
            int secondYear;
            if (parts.Length != 2)
            {
                throw new Exception("صيغة العام الدراسي يجب أن تكون متسلسلة مثل 2026/2027.");
            }

            string firstYearText = parts[0].Trim();
            string secondYearText = parts[1].Trim();
            if (firstYearText.Length != 4 || secondYearText.Length != 4 ||
                !int.TryParse(firstYearText, out firstYear) || !int.TryParse(secondYearText, out secondYear) ||
                secondYear != firstYear + 1)
            {
                throw new Exception("صيغة العام الدراسي يجب أن تكون متسلسلة مثل 2026/2027.");
            }

            plan.AcademicYear = year;
            plan.FeeType = plan.FeeType.Trim();
            plan.Notes = string.IsNullOrWhiteSpace(plan.Notes) ? null : plan.Notes.Trim();
        }
    }
}
