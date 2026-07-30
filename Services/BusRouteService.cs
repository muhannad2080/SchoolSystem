using System;
using System.Data;
using SchoolSystem.DataAccess;
using SchoolSystem.Models;

namespace SchoolSystem.Services
{
    public class BusRouteService
    {
        private readonly BusRouteRepository routeRepository;

        public BusRouteService()
        {
            routeRepository = new BusRouteRepository();
        }

        public DataTable GetAllRoutes()
        {
            return routeRepository.GetAllRoutes();
        }

        public bool AddRoute(BusRoute route)
        {
            ValidateRoute(route);
            return routeRepository.AddRoute(route);
        }

        public bool UpdateRoute(BusRoute route)
        {
            if (route.RouteID <= 0)
                throw new Exception("رقم المسار غير صحيح.");

            ValidateRoute(route);
            return routeRepository.UpdateRoute(route);
        }

        public bool DeleteRoute(int routeId)
        {
            if (routeId <= 0)
                throw new Exception("رقم المسار غير صحيح.");

            return routeRepository.DeleteRoute(routeId);
        }

        private void ValidateRoute(BusRoute route)
        {
            if (route == null)
                throw new Exception("بيانات المسار غير موجودة.");

            if (string.IsNullOrWhiteSpace(route.RouteName))
                throw new Exception("يجب إدخال اسم المسار.");

            if (route.BusID <= 0)
                throw new Exception("يجب اختيار الحافلة.");

            if (route.Fee.HasValue && route.Fee.Value < 0)
                throw new Exception("رسوم النقل لا يمكن أن تكون أقل من صفر.");
        }
    }
}
