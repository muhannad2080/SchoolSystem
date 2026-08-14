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

        public DataTable GetAllRooms()
        {
            CurrentUser.DemandPermission(PermissionKeys.ClassesManage, "ليس لديك صلاحية إدارة القاعات.");
            return repository.GetAllRooms();
        }

        public DataTable GetActiveRooms()
        {
            CurrentUser.DemandPermission(PermissionKeys.ClassesManage, "ليس لديك صلاحية إدارة القاعات.");
            return repository.GetActiveRooms();
        }

        public bool AddRoom(Room room)
        {
            CurrentUser.DemandPermission(PermissionKeys.ClassesManage, "ليس لديك صلاحية إدارة القاعات.");
            Validate(room, false);

            if (repository.RoomCodeExists(room.RoomCode, 0))
                throw new ArgumentException("كود القاعة موجود مسبقًا.");

            return repository.AddRoom(room);
        }

        public bool UpdateRoom(Room room)
        {
            CurrentUser.DemandPermission(PermissionKeys.ClassesManage, "ليس لديك صلاحية إدارة القاعات.");
            Validate(room, true);

            if (repository.RoomCodeExists(room.RoomCode, room.RoomID))
                throw new ArgumentException("كود القاعة مستخدم لقاعة أخرى.");

            return repository.UpdateRoom(room);
        }

        public bool DeleteRoom(int roomId)
        {
            CurrentUser.DemandPermission(PermissionKeys.ClassesManage, "ليس لديك صلاحية إدارة القاعات.");
            if (roomId <= 0)
                throw new ArgumentException("رقم القاعة غير صحيح.");

            return repository.DeleteRoom(roomId);
        }

        private void Validate(Room room, bool isUpdate)
        {
            if (room == null)
                throw new ArgumentException("بيانات القاعة غير صحيحة.");

            if (isUpdate && room.RoomID <= 0)
                throw new ArgumentException("اختر قاعة صحيحة للتعديل.");

            if (string.IsNullOrWhiteSpace(room.RoomCode))
                throw new ArgumentException("كود القاعة مطلوب.");

            if (!Regex.IsMatch(room.RoomCode.Trim(), @"^[a-zA-Z0-9_-]{2,30}$"))
                throw new ArgumentException("كود القاعة يجب أن يحتوي على حروف أو أرقام فقط.");

            if (string.IsNullOrWhiteSpace(room.RoomName))
                throw new ArgumentException("اسم القاعة مطلوب.");

            if (string.IsNullOrWhiteSpace(room.RoomType))
                throw new ArgumentException("نوع القاعة مطلوب.");

            if (room.Capacity <= 0)
                throw new ArgumentException("سعة القاعة يجب أن تكون أكبر من صفر.");
        }
    }
}
