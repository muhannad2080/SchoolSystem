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
            // تحميل الأطراف عملية عرض فقط؛ لا نربطها بصلاحية الإدارة القديمة
            // حتى لا تختفي القائمة عن المستخدم الذي يملك View أو صلاحية جزئية.
            CurrentUser.DemandAction("Vouchers", "View", "ليس لديك صلاحية عرض أطراف السندات.");

            return repository.GetVoucherParties();
        }
    }
}
