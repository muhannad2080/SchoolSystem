using System;
using System.Data;
using SchoolSystem.DataAccess;
using SchoolSystem.Models;
using SchoolSystem.Security;

namespace SchoolSystem.Services
{
    public class BusRouteService
    {
        private readonly BusRouteRepository routeRepository;
        private readonly AuditLogService auditLogService;

        public BusRouteService()
        {
            routeRepository = new BusRouteRepository();
            auditLogService = new AuditLogService();
        }

        public DataTable GetAllRoutes()
        {
            CurrentUser.DemandAction("Transport", "View", "ليس لديك صلاحية عرض مسارات النقل.");
            return routeRepository.GetAllRoutes();
        }

        public bool AddRoute(BusRoute route)
        {
            CurrentUser.DemandAction("Transport", "Add", "ليس لديك صلاحية إضافة مسارات النقل.");
            ValidateRoute(route);
            bool added = routeRepository.AddRoute(route);
            if (added)
                auditLogService.Record("إنشاء", "BusRoute", route.RouteID.ToString(), "إضافة مسار نقل: " + route.RouteName);
            return added;
        }

        public bool UpdateRoute(BusRoute route)
        {
            CurrentUser.DemandAction("Transport", "Edit", "ليس لديك صلاحية تعديل مسارات النقل.");
            if (route == null || route.RouteID <= 0)
                throw new Exception("رقم المسار غير صحيح.");

            ValidateRoute(route);
            bool updated = routeRepository.UpdateRoute(route);
            if (updated)
                auditLogService.Record("تعديل", "BusRoute", route.RouteID.ToString(), "تعديل مسار النقل: " + route.RouteName);
            return updated;
        }

        public bool DeleteRoute(int routeId)
        {
            CurrentUser.DemandAction("Transport", "Delete", "ليس لديك صلاحية حذف مسارات النقل.");
            if (routeId <= 0)
                throw new Exception("رقم المسار غير صحيح.");

            bool deleted = routeRepository.DeleteRoute(routeId);
            if (deleted)
                auditLogService.Record("حذف", "BusRoute", routeId.ToString(), "حذف مسار نقل.");
            return deleted;
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
