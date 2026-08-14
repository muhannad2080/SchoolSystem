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

        public BusService()
        {
            busRepository = new BusRepository();
        }

        public DataTable GetAllBuses()
        {
            CurrentUser.DemandPermission(PermissionKeys.TransportManage, "ليس لديك صلاحية إدارة النقل.");
            return busRepository.GetAllBuses();
        }

        public bool AddBus(Bus bus)
        {
            CurrentUser.DemandPermission(PermissionKeys.TransportManage, "ليس لديك صلاحية إدارة النقل.");
            ValidateBus(bus);

            if (busRepository.BusNumberExists(bus.BusNumber))
                throw new Exception("رقم الحافلة موجود مسبقاً.");

            return busRepository.AddBus(bus);
        }

        public bool UpdateBus(Bus bus)
        {
            CurrentUser.DemandPermission(PermissionKeys.TransportManage, "ليس لديك صلاحية إدارة النقل.");
            if (bus == null || bus.BusID <= 0)
                throw new Exception("رقم الحافلة غير صحيح.");

            ValidateBus(bus);

            if (busRepository.BusNumberExists(bus.BusNumber, bus.BusID))
                throw new Exception("رقم الحافلة موجود مسبقاً.");

            return busRepository.UpdateBus(bus);
        }

        public bool DeleteBus(int busId)
        {
            CurrentUser.DemandPermission(PermissionKeys.TransportManage, "ليس لديك صلاحية إدارة النقل.");
            if (busId <= 0)
                throw new Exception("رقم الحافلة غير صحيح.");

            return busRepository.DeleteBus(busId);
        }

        public bool BusNumberExists(string busNumber)
        {
            CurrentUser.DemandPermission(PermissionKeys.TransportManage, "ليس لديك صلاحية إدارة النقل.");
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
