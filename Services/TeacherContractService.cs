using System;
using System.Data;
using SchoolSystem.DataAccess;
using SchoolSystem.Models;
using SchoolSystem.Security;

namespace SchoolSystem.Services
{
    public class TeacherContractService
    {
        private readonly TeacherContractRepository repository = new TeacherContractRepository();
        private readonly AuditLogService auditLogService = new AuditLogService();

        public DataTable GetAllContracts()
        {
            CurrentUser.DemandAction("Contracts", "View", "ليس لديك صلاحية عرض عقود المعلمين.");
            return repository.GetAllContracts();
        }

        public void AddContract(TeacherContract contract)
        {
            CurrentUser.DemandAction("Contracts", "Add", "ليس لديك صلاحية إضافة عقود المعلمين.");
            if (contract == null)
                throw new ArgumentException("بيانات العقد غير صحيحة.");
            repository.AddContract(contract);
            auditLogService.Record("إنشاء", "TeacherContract", contract.ContractID.ToString(),
                "إنشاء عقد للمعلم رقم " + contract.TeacherID);
        }

        public bool UpdateContract(TeacherContract contract)
        {
            CurrentUser.DemandAction("Contracts", "Edit", "ليس لديك صلاحية تعديل عقود المعلمين.");
            if (contract == null)
                throw new ArgumentException("بيانات العقد غير صحيحة.");
            bool updated = repository.UpdateContract(contract);
            if (updated)
                auditLogService.Record("تعديل", "TeacherContract", contract.ContractID.ToString(),
                    "تعديل عقد المعلم رقم " + contract.TeacherID);
            return updated;
        }

        public bool DeleteContract(int contractId)
        {
            CurrentUser.DemandAction("Contracts", "Delete", "ليس لديك صلاحية حذف عقود المعلمين.");
            if (contractId <= 0)
                throw new ArgumentException("رقم العقد غير صحيح.");
            bool deleted = repository.DeleteContract(contractId);
            if (deleted)
                auditLogService.Record("حذف", "TeacherContract", contractId.ToString(), "حذف عقد معلم.");
            return deleted;
        }

    }
}
