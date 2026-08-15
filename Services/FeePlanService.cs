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
            CurrentUser.DemandPermission(PermissionKeys.FeesManage, "ليس لديك صلاحية إدارة خطط الرسوم.");
            return feePlanRepository.GetAllFeePlans();
        }

        public DataTable GetClasses()
        {
            CurrentUser.DemandPermission(PermissionKeys.FeesManage, "ليس لديك صلاحية إدارة خطط الرسوم.");
            return feePlanRepository.GetClasses();
        }

        public bool AddFeePlan(FeePlan plan)
        {
            CurrentUser.DemandPermission(PermissionKeys.FeesManage, "ليس لديك صلاحية إدارة خطط الرسوم.");
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
            CurrentUser.DemandPermission(PermissionKeys.FeesManage, "ليس لديك صلاحية إدارة خطط الرسوم.");
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
            CurrentUser.DemandPermission(PermissionKeys.FeesManage, "ليس لديك صلاحية إدارة خطط الرسوم.");
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
        }
    }
}
