using System.Data;
using SchoolSystem.DataAccess;
using SchoolSystem.Models;

namespace SchoolSystem.Services
{
    public class TeacherContractService
    {
        private readonly TeacherContractRepository repository = new TeacherContractRepository();

        public DataTable GetAllContracts() => repository.GetAllContracts();
        private readonly ContractService contractService = new ContractService();

        public void AddContract(TeacherContract contract) => repository.AddContract(contract);
        public bool UpdateContract(TeacherContract contract) => repository.UpdateContract(contract);
        public bool DeleteContract(int contractId) => repository.DeleteContract(contractId);
    }
}