using System;
using System.Data;
using SchoolSystem.DataAccess;
using SchoolSystem.Models;

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
            return feePlanRepository.GetAllFeePlans();
        }

        public DataTable GetClasses()
        {
            return feePlanRepository.GetClasses();
        }

        public bool AddFeePlan(FeePlan plan)
        {
            Validate(plan);
            return feePlanRepository.AddFeePlan(plan);
        }

        public bool UpdateFeePlan(FeePlan plan)
        {
            if (plan.FeePlanID <= 0)
                throw new Exception("رقم خطة الرسوم غير صحيح.");

            Validate(plan);
            return feePlanRepository.UpdateFeePlan(plan);
        }

        public bool DeleteFeePlan(int feePlanId)
        {
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
