using System;
using System.Data;
using System.Text.RegularExpressions;
using SchoolSystem.DataAccess;
using SchoolSystem.Models;
using SchoolSystem.Security;

namespace SchoolSystem.Services
{
    public class RoomService
    {
        private readonly RoomRepository repository = new RoomRepository();
        private readonly AuditLogService auditLogService = new AuditLogService();

        public DataTable GetAllRooms()
        {
            CurrentUser.DemandAny(
                "ليس لديك صلاحية عرض القاعات.",
                "Rooms.View",
                "Rooms.Manage",
                "Classes.View",
                "Classes.Manage");
            return repository.GetAllRooms();
        }

        public DataTable GetActiveRooms()
        {
            CurrentUser.DemandAny(
                "ليس لديك صلاحية عرض القاعات النشطة.",
                "Rooms.View",
                "Rooms.Manage",
                "Classes.View",
                "Classes.Manage");
            return repository.GetActiveRooms();
        }

        public int AddRoom(Room room)
        {
            CurrentUser.DemandAction("Rooms", "Add", "ليس لديك صلاحية إضافة القاعات.");
            Validate(room, false);

            int roomId = repository.AddRoom(room);
            auditLogService.Record(
                "إضافة قاعة",
                "Room",
                roomId.ToString(),
                "تمت إضافة القاعة برقم تلقائي " + roomId);
            return roomId;
        }

        public bool UpdateRoom(Room room)
        {
            CurrentUser.DemandAction("Rooms", "Edit", "ليس لديك صلاحية تعديل القاعات.");
            Validate(room, true);

            if (repository.RoomCodeExists(room.RoomCode, room.RoomID))
                throw new ArgumentException("كود القاعة مستخدم لقاعة أخرى.");

            bool updated = repository.UpdateRoom(room);
            if (updated)
            {
                auditLogService.Record(
                    "تعديل قاعة",
                    "Room",
                    room.RoomID.ToString(),
                    "تم تعديل القاعة " + room.RoomCode);
            }
            return updated;
        }

        public bool DeleteRoom(int roomId)
        {
            CurrentUser.DemandAction("Rooms", "Delete", "ليس لديك صلاحية تعطيل القاعات.");
            if (roomId <= 0)
                throw new ArgumentException("رقم القاعة غير صحيح.");

            bool deleted = repository.DeleteRoom(roomId);
            if (deleted)
            {
                auditLogService.Record(
                    "تعطيل قاعة",
                    "Room",
                    roomId.ToString(),
                    "تم تعطيل القاعة بدلاً من حذف سجلها نهائياً");
            }
            return deleted;
        }

        private void Validate(Room room, bool isUpdate)
        {
            if (room == null)
                throw new ArgumentException("بيانات القاعة غير صحيحة.");

            if (isUpdate && room.RoomID <= 0)
                throw new ArgumentException("اختر قاعة صحيحة للتعديل.");

            if (isUpdate)
            {
                if (string.IsNullOrWhiteSpace(room.RoomCode))
                    throw new ArgumentException("كود القاعة مطلوب عند التعديل.");
                if (!Regex.IsMatch(room.RoomCode.Trim(), @"^[a-zA-Z0-9_-]{2,30}$"))
                    throw new ArgumentException("كود القاعة يجب أن يحتوي على حروف أو أرقام فقط.");
            }

            if (string.IsNullOrWhiteSpace(room.RoomName))
                throw new ArgumentException("اسم القاعة مطلوب.");

            if (string.IsNullOrWhiteSpace(room.RoomType))
                throw new ArgumentException("نوع القاعة مطلوب.");

            if (room.Capacity <= 0)
                throw new ArgumentException("سعة القاعة يجب أن تكون أكبر من صفر.");
        }
    }
}
