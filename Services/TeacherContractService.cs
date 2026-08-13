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
        }

        public bool UpdateContract(TeacherContract contract)
        {
            EnsureCanManageTeacherContracts();
            if (contract == null)
                throw new ArgumentException("بيانات العقد غير صحيحة.");
            return repository.UpdateContract(contract);
        }

        public bool DeleteContract(int contractId)
        {
            EnsureCanManageTeacherContracts();
            if (contractId <= 0)
                throw new ArgumentException("رقم العقد غير صحيح.");
            return repository.DeleteContract(contractId);
        }

        private static void EnsureCanManageTeacherContracts()
        {
            if (!CurrentUser.HasPermission(PermissionKeys.TeachersManage))
                throw new UnauthorizedAccessException("ليس لديك صلاحية إدارة عقود المعلمين.");
        }
    }
}
