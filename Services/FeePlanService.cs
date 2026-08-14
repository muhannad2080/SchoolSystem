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
            return feePlanRepository.AddFeePlan(plan);
        }

        public bool UpdateFeePlan(FeePlan plan)
        {
            CurrentUser.DemandPermission(PermissionKeys.FeesManage, "ليس لديك صلاحية إدارة خطط الرسوم.");
            if (plan == null || plan.FeePlanID <= 0)
                throw new Exception("رقم خطة الرسوم غير صحيح.");

            Validate(plan);
            return feePlanRepository.UpdateFeePlan(plan);
        }

        public bool DeleteFeePlan(int feePlanId)
        {
            CurrentUser.DemandPermission(PermissionKeys.FeesManage, "ليس لديك صلاحية إدارة خطط الرسوم.");
            if (feePlanId <= 0)
                throw new Exception("رقم خطة الرسوم غير صحيح.");

            return feePlanRepository.DeleteFeePlan(feePlanId);
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
