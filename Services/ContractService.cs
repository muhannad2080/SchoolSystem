using System;
using System.Data;
using System.Text.RegularExpressions;
using SchoolSystem.DataAccess;
using SchoolSystem.Models;
using SchoolSystem.Security;

namespace SchoolSystem.Services
{
    public class ContractService
    {
        private readonly TeacherContractRepository repository = new TeacherContractRepository();
        private readonly AuditLogService auditLogService = new AuditLogService();

        public DataTable GetAllContracts()
        {
            CurrentUser.DemandAction("TeacherContracts", "View", "ليس لديك صلاحية عرض عقود الموظفين.");
            return repository.GetAllContracts();
        }

        public bool AddContract(TeacherContract contract)
        {
            CurrentUser.DemandAction("TeacherContracts", "Add", "ليس لديك صلاحية إضافة عقود الموظفين.");
            CalculateSalary(contract);
            ValidateContract(contract, false);

            if (repository.ContractNumberExists(contract.ContractNumber))
                throw new ArgumentException("رقم العقد مستخدم مسبقاً.");

            if (contract.ContractStatus == "ساري" && repository.HasActiveContract(contract.TeacherID))
                throw new ArgumentException("يوجد عقد ساري لهذا المعلم.");

            bool added = repository.AddContract(contract);
            if (added)
            {
                auditLogService.Record(
                    "إضافة عقد موظف",
                    "Contract",
                    contract.ContractID.ToString(),
                    "تمت إضافة العقد " + contract.ContractNumber + " للمعلم رقم " + contract.TeacherID);
            }
            return added;
        }

        public bool UpdateContract(TeacherContract contract)
        {
            CurrentUser.DemandAction("TeacherContracts", "Edit", "ليس لديك صلاحية تعديل عقود الموظفين.");
            CalculateSalary(contract);
            ValidateContract(contract, true);

            if (repository.ContractNumberExists(contract.ContractNumber, contract.ContractID))
                throw new ArgumentException("رقم العقد مستخدم في عقد آخر.");

            if (contract.ContractStatus == "ساري" && repository.HasActiveContract(contract.TeacherID, contract.ContractID))
                throw new ArgumentException("يوجد عقد ساري آخر لهذا المعلم.");

            bool updated = repository.UpdateContract(contract);
            if (updated)
            {
                auditLogService.Record(
                    "تعديل عقد موظف",
                    "Contract",
                    contract.ContractID.ToString(),
                    "تم تعديل العقد " + contract.ContractNumber);
            }
            return updated;
        }

        public bool DeleteContract(int contractId)
        {
            CurrentUser.DemandAction("TeacherContracts", "Delete", "ليس لديك صلاحية حذف عقود الموظفين.");
            if (contractId <= 0)
                throw new ArgumentException("رقم العقد غير صحيح.");

            bool deleted = repository.DeleteContract(contractId);
            if (deleted)
            {
                auditLogService.Record(
                    "حذف عقد موظف",
                    "Contract",
                    contractId.ToString(),
                    "تم حذف عقد الموظف رقم " + contractId);
            }
            return deleted;
        }

        public bool HasActiveContract(int teacherId)
        {
            CurrentUser.DemandAction("TeacherContracts", "View", "ليس لديك صلاحية التحقق من عقود الموظفين.");
            return repository.HasActiveContract(teacherId);
        }

        public bool HasActiveContract(int teacherId, int excludedContractId)
        {
            CurrentUser.DemandAction("TeacherContracts", "View", "ليس لديك صلاحية التحقق من عقود الموظفين.");
            return repository.HasActiveContract(teacherId, excludedContractId);
        }

        public void CalculateSalary(TeacherContract contract)
        {
            // الحساب عملية داخلية لازمة قبل الحفظ، وتُحكم صلاحية العملية المستدعية نفسها.
            if (contract == null)
                return;

            contract.TotalSalary =
                contract.BasicSalary +
                contract.HousingAllowance +
                contract.TransportAllowance +
                contract.OtherAllowances;

            contract.NetSalary = contract.TotalSalary - contract.Deductions;

            if (contract.NetSalary < 0)
                contract.NetSalary = 0;
        }

        private void ValidateContract(TeacherContract contract, bool isUpdate)
        {
            if (contract == null)
                throw new ArgumentException("بيانات العقد غير صحيحة.");

            if (isUpdate && contract.ContractID <= 0)
                throw new ArgumentException("اختر عقداً صحيحاً للتعديل.");

            if (contract.TeacherID <= 0)
                throw new ArgumentException("يجب اختيار المعلم.");

            if (string.IsNullOrWhiteSpace(contract.ContractNumber))
                throw new ArgumentException("رقم العقد مطلوب.");

            if (!IsValidContractNumber(contract.ContractNumber))
                throw new ArgumentException("رقم العقد يجب أن يحتوي على حروف أو أرقام أو شرطة فقط.");

            if (string.IsNullOrWhiteSpace(contract.ContractType))
                throw new ArgumentException("يجب اختيار نوع العقد.");

            if (!IsValidContractType(contract.ContractType))
                throw new ArgumentException("نوع العقد غير صحيح.");

            if (string.IsNullOrWhiteSpace(contract.ContractStatus))
                throw new ArgumentException("يجب اختيار حالة العقد.");

            if (!IsValidContractStatus(contract.ContractStatus))
                throw new ArgumentException("حالة العقد غير صحيحة.");

            if (contract.BasicSalary <= 0)
                throw new ArgumentException("الراتب الأساسي يجب أن يكون أكبر من صفر.");

            if (contract.HousingAllowance < 0 ||
                contract.TransportAllowance < 0 ||
                contract.OtherAllowances < 0 ||
                contract.Deductions < 0)
                throw new ArgumentException("المبالغ المالية لا يمكن أن تكون سالبة.");

            if (contract.Deductions > contract.TotalSalary)
                throw new ArgumentException("الخصومات لا يمكن أن تكون أكبر من إجمالي المستحقات.");

            if (contract.EndDate.HasValue &&
                contract.EndDate.Value.Date < contract.StartDate.Date)
                throw new ArgumentException("تاريخ نهاية العقد يجب أن يكون بعد تاريخ البداية.");

            if (string.IsNullOrWhiteSpace(contract.PaymentMethod))
                throw new ArgumentException("يجب اختيار طريقة الصرف.");

            if (!IsValidPaymentMethod(contract.PaymentMethod))
                throw new ArgumentException("طريقة الصرف غير صحيحة.");

            if (!string.IsNullOrWhiteSpace(contract.Notes) &&
                !IsValidNotes(contract.Notes))
                throw new ArgumentException("الملاحظات تحتوي على رموز غير مسموحة.");
        }

        private bool IsValidContractNumber(string value)
        {
            return Regex.IsMatch(value, @"^[a-zA-Z0-9\-_\/]+$");
        }

        private bool IsValidNotes(string value)
        {
            return Regex.IsMatch(value, @"^[\u0600-\u06FFa-zA-Z0-9\s\.\،\,\-\_\/]+$");
        }

        private bool IsValidContractType(string value)
        {
            return value == "دائم" ||
                   value == "مؤقت" ||
                   value == "موسمي" ||
                   value == "مستشار" ||
                   value == "دوام كامل" ||
                   value == "دوام جزئي" ||
                   value == "بالساعة" ||
                   value == "تعاقد سنوي";
        }

        private bool IsValidContractStatus(string value)
        {
            return value == "ساري" ||
                   value == "منتهي" ||
                   value == "موقوف" ||
                   value == "ملغي";
        }

        private bool IsValidPaymentMethod(string value)
        {
            return value == "نقداً" ||
                   value == "حوالة" ||
                   value == "بنك" ||
                   value == "محفظة إلكترونية";
        }
    }
}
