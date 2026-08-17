using System;
using System.Data;
using SchoolSystem.Models;

namespace SchoolSystem.Services
{
    // واجهة توافقية للخدمة القديمة؛ جميع قواعد الصلاحيات والتحقق والتدقيق مركزية في ContractService.
    public class TeacherContractService
    {
        private readonly ContractService contractService = new ContractService();

        public DataTable GetAllContracts()
        {
            return contractService.GetAllContracts();
        }

        public void AddContract(TeacherContract contract)
        {
            if (!contractService.AddContract(contract))
                throw new InvalidOperationException("تعذر إضافة العقد.");
        }

        public bool UpdateContract(TeacherContract contract)
        {
            return contractService.UpdateContract(contract);
        }

        public bool DeleteContract(int contractId)
        {
            return contractService.DeleteContract(contractId);
        }

        public bool HasActiveContract(int teacherId)
        {
            return contractService.HasActiveContract(teacherId);
        }

        public bool HasActiveContract(int teacherId, int excludedContractId)
        {
            return contractService.HasActiveContract(teacherId, excludedContractId);
        }
    }
}
