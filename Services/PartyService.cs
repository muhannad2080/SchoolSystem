using System.Data;
using SchoolSystem.DataAccess;
using SchoolSystem.Security;


namespace SchoolSystem.Services
{

    public class PartyService
    {
        // تعريف الدالة PartyRepository لتنفيذ العملية المرتبطة بها.
        private readonly PartyRepository repository = new PartyRepository();

        // تعريف الدالة GetVoucherParties لتنفيذ العملية المرتبطة بها.
        public DataTable GetVoucherParties()
        {
            // تحميل الأطراف عملية عرض فقط؛ لا نربطها بصلاحية الإدارة القديمة
            // حتى لا تختفي القائمة عن المستخدم الذي يملك View أو صلاحية جزئية.
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.DemandAction("Vouchers", "View", "ليس لديك صلاحية عرض أطراف السندات.");


            return repository.GetVoucherParties();
        }
    }
}
