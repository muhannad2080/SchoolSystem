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
            EnsureCanManageTeacherContracts();
            return repository.GetAllContracts();
        }

        public void AddContract(TeacherContract contract)
        {
            EnsureCanManageTeacherContracts();
            if (contract == null)
                throw new ArgumentException("بيانات العقد غير صحيحة.");
            repository.AddContract(contract);
            auditLogService.Record("إنشاء", "TeacherContract", contract.ContractID.ToString(),
                "إنشاء عقد للمعلم رقم " + contract.TeacherID);
        }

        public bool UpdateContract(TeacherContract contract)
        {
            EnsureCanManageTeacherContracts();
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
            EnsureCanManageTeacherContracts();
            if (contractId <= 0)
                throw new ArgumentException("رقم العقد غير صحيح.");
            bool deleted = repository.DeleteContract(contractId);
            if (deleted)
                auditLogService.Record("حذف", "TeacherContract", contractId.ToString(), "حذف عقد معلم.");
            return deleted;
        }

        private static void EnsureCanManageTeacherContracts()
        {
            if (!CurrentUser.HasPermission(PermissionKeys.TeachersManage))
                throw new UnauthorizedAccessException("ليس لديك صلاحية إدارة عقود المعلمين.");
        }
    }
}
