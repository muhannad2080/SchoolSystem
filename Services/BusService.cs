using System;
using System.Data;
using SchoolSystem.DataAccess;
using SchoolSystem.Models;
using SchoolSystem.Security;

namespace SchoolSystem.Services
{
    public class BusService
    {
        private readonly BusRepository busRepository;
        private readonly AuditLogService auditLogService;

        public BusService()
        {
            busRepository = new BusRepository();
            auditLogService = new AuditLogService();
        }

        public DataTable GetAllBuses()
        {
            CurrentUser.DemandAction("Transport", "View", "ليس لديك صلاحية عرض الحافلات.");
            return busRepository.GetAllBuses();
        }

        public bool AddBus(Bus bus)
        {
            CurrentUser.DemandAction("Transport", "Add", "ليس لديك صلاحية إضافة الحافلات.");
            ValidateBus(bus);

            if (busRepository.BusNumberExists(bus.BusNumber))
                throw new Exception("رقم الحافلة موجود مسبقاً.");

            bool added = busRepository.AddBus(bus);
            if (added)
                auditLogService.Record("إنشاء", "Bus", bus.BusID.ToString(), "إضافة حافلة رقم " + bus.BusNumber);
            return added;
        }

        public bool UpdateBus(Bus bus)
        {
            CurrentUser.DemandAction("Transport", "Edit", "ليس لديك صلاحية تعديل الحافلات.");
            if (bus == null || bus.BusID <= 0)
                throw new Exception("رقم الحافلة غير صحيح.");

            ValidateBus(bus);

            if (busRepository.BusNumberExists(bus.BusNumber, bus.BusID))
                throw new Exception("رقم الحافلة موجود مسبقاً.");

            bool updated = busRepository.UpdateBus(bus);
            if (updated)
                auditLogService.Record("تعديل", "Bus", bus.BusID.ToString(), "تعديل بيانات الحافلة رقم " + bus.BusNumber);
            return updated;
        }

        public bool DeleteBus(int busId)
        {
            CurrentUser.DemandAction("Transport", "Delete", "ليس لديك صلاحية حذف الحافلات.");
            if (busId <= 0)
                throw new Exception("رقم الحافلة غير صحيح.");

            bool deleted = busRepository.DeleteBus(busId);
            if (deleted)
                auditLogService.Record("حذف", "Bus", busId.ToString(), "حذف حافلة من إدارة النقل.");
            return deleted;
        }

        public bool BusNumberExists(string busNumber)
        {
            CurrentUser.DemandAction("Transport", "View", "ليس لديك صلاحية التحقق من بيانات النقل.");
            return busRepository.BusNumberExists(busNumber);
        }

        private void ValidateBus(Bus bus)
        {
            if (bus == null)
                throw new Exception("بيانات الحافلة غير موجودة.");

            if (string.IsNullOrWhiteSpace(bus.BusNumber))
                throw new Exception("يجب إدخال رقم الحافلة.");

            if (bus.Capacity <= 0)
                throw new Exception("سعة الحافلة يجب أن تكون أكبر من صفر.");
        }
    }
}
