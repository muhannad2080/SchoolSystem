using System;
using System.Data;
using SchoolSystem.DataAccess;
using SchoolSystem.Models;

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
            return busRepository.GetAllBuses();
        }

        public bool AddBus(Bus bus)
        {
            ValidateBus(bus);

            if (busRepository.BusNumberExists(bus.BusNumber))
                throw new Exception("رقم الحافلة موجود مسبقاً.");

            return busRepository.AddBus(bus);
        }

        public bool UpdateBus(Bus bus)
        {
            if (bus.BusID <= 0)
                throw new Exception("رقم الحافلة غير صحيح.");

            ValidateBus(bus);

            if (busRepository.BusNumberExists(bus.BusNumber, bus.BusID))
                throw new Exception("رقم الحافلة موجود مسبقاً.");

            return busRepository.UpdateBus(bus);
        }

        public bool DeleteBus(int busId)
        {
            if (busId <= 0)
                throw new Exception("رقم الحافلة غير صحيح.");

            return busRepository.DeleteBus(busId);
        }

        public bool BusNumberExists(string busNumber)
        {
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
