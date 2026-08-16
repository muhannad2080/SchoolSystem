using System.Data;
using SchoolSystem.DataAccess;
using SchoolSystem.Security;

namespace SchoolSystem.Services
{
    public class PartyService
    {
        private readonly PartyRepository repository = new PartyRepository();

        public DataTable GetVoucherParties()
        {
            if (!CurrentUser.HasPermission(PermissionKeys.VouchersManage))
                throw new System.UnauthorizedAccessException("ليس لديك صلاحية تحميل أطراف السندات.");

            return repository.GetVoucherParties();
        }
    }
}
