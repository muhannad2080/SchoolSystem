using System;
using System.Data;
using SchoolSystem.Models;


namespace SchoolSystem.Services
{
    // واجهة توافقية للخدمة القديمة؛ جميع قواعد الصلاحيات والتحقق والتدقيق مركزية في ContractService.

    public class TeacherContractService
    {
        // تعريف الدالة ContractService لتنفيذ العملية المرتبطة بها.
        private readonly ContractService contractService = new ContractService();

        // تعريف الدالة GetAllContracts لتنفيذ العملية المرتبطة بها.
        public DataTable GetAllContracts()
        {

            return contractService.GetAllContracts();
        }

        // تعريف الدالة AddContract لتنفيذ العملية المرتبطة بها.
        public void AddContract(TeacherContract contract)
        {
            if (!contractService.AddContract(contract))

                throw new InvalidOperationException("تعذر إضافة العقد.");
        }

        // تعريف الدالة UpdateContract لتنفيذ العملية المرتبطة بها.
        public bool UpdateContract(TeacherContract contract)
        {

            return contractService.UpdateContract(contract);
        }

        // تعريف الدالة DeleteContract لتنفيذ العملية المرتبطة بها.
        public bool DeleteContract(int contractId)
        {

            return contractService.DeleteContract(contractId);
        }

        // تعريف الدالة HasActiveContract لتنفيذ العملية المرتبطة بها.
        public bool HasActiveContract(int teacherId)
        {

            return contractService.HasActiveContract(teacherId);
        }

        // تعريف الدالة HasActiveContract لتنفيذ العملية المرتبطة بها.
        public bool HasActiveContract(int teacherId, int excludedContractId)
        {

            return contractService.HasActiveContract(teacherId, excludedContractId);
        }
    }
}
