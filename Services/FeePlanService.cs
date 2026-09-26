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
        // تعريف الدالة AuditLogService لتنفيذ العملية المرتبطة بها.
        private readonly AuditLogService auditLogService = new AuditLogService();

        // تعريف الدالة FeePlanService لتنفيذ العملية المرتبطة بها.
        public FeePlanService()
        {
            feePlanRepository = new FeePlanRepository();
        }

        // تعريف الدالة GetAllFeePlans لتنفيذ العملية المرتبطة بها.
        public DataTable GetAllFeePlans()
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.DemandAction("FeePlans", "View", "ليس لديك صلاحية عرض خطط الرسوم.");

            return feePlanRepository.GetAllFeePlans();
        }

        // تعريف الدالة GetFeeTypes لتنفيذ العملية المرتبطة بها.
        public DataTable GetFeeTypes()
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.DemandAction("FeePlans", "View", "ليس لديك صلاحية عرض أنواع الرسوم.");

            return feePlanRepository.GetFeeTypes();
        }

        // تعريف الدالة GetRegistrationFeePlan لتنفيذ العملية المرتبطة بها.
        public FeePlan GetRegistrationFeePlan(int studentId, string academicYear, int classId)
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.DemandAction("Enrollment", "View", "ليس لديك صلاحية عرض خطة رسوم التسجيل.");
            if (studentId <= 0) throw new Exception("يجب اختيار الطالب.");
            if (classId <= 0) throw new Exception("يجب اختيار الصف.");
            if (string.IsNullOrWhiteSpace(academicYear)) throw new Exception("يجب اختيار العام الدراسي.");
            return feePlanRepository.GetRegistrationFeePlan(studentId, academicYear, classId);
        }

        // تعريف الدالة GetStudentFeePlan لتنفيذ العملية المرتبطة بها.
        public FeePlan GetStudentFeePlan(int studentId, string academicYear, string feeType)
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.DemandAction("Fees", "View", "ليس لديك صلاحية عرض الرسوم وخططها.");
            if (studentId <= 0)

                throw new Exception("يجب اختيار الطالب.");

            if (string.IsNullOrWhiteSpace(academicYear))

                throw new Exception("يجب اختيار العام الدراسي.");

            if (string.IsNullOrWhiteSpace(feeType))

                throw new Exception("يجب اختيار نوع الرسوم.");

            return feePlanRepository.GetStudentFeePlan(studentId, academicYear, feeType);
        }

        // تعريف الدالة GetClasses لتنفيذ العملية المرتبطة بها.
        public DataTable GetClasses()
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.DemandAction("FeePlans", "View", "ليس لديك صلاحية عرض صفوف خطط الرسوم.");

            return feePlanRepository.GetClasses();
        }

        // تعريف الدالة AddFeePlan لتنفيذ العملية المرتبطة بها.
        public bool AddFeePlan(FeePlan plan)
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
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

        // تعريف الدالة DistributeTotalFee لتنفيذ العملية المرتبطة بها.
        public int DistributeTotalFee(FeePlan plan)
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.DemandAction("FeePlans", "Add", "ليس لديك صلاحية توزيع الرسوم الكلية.");
            Validate(plan);
            if (!IsTotalFeeType(plan.FeeType))

                throw new Exception("اختر نوع الرسوم الكلية لتنفيذ التوزيع.");

            int affected = feePlanRepository.DistributeTotalFee(plan);
            auditLogService.Record(
                "توزيع الرسوم الكلية",
                "FeePlan",
                plan.ClassID.ToString(),
                "تم توزيع مبلغ " + plan.Amount.ToString("N2") + " على أنواع الرسوم الموجودة في الصف نفسه للعام " + plan.AcademicYear);

            return affected;
        }

        // تعريف الدالة IsTotalFeeType لتنفيذ العملية المرتبطة بها.
        private bool IsTotalFeeType(string feeType)
        {
            string value = (feeType ?? string.Empty).Trim();

            return value == "الرسوم الكلية" || value == "رسوم كلية" || value == "الرسوم الكليه" || value == "رسوم كليه" || value.Equals("Total Fee", StringComparison.OrdinalIgnoreCase);
        }

        // تعريف الدالة UpdateFeePlan لتنفيذ العملية المرتبطة بها.
        public bool UpdateFeePlan(FeePlan plan)
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
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

        // تعريف الدالة DeleteFeePlan لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة Validate لتنفيذ العملية المرتبطة بها.
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
