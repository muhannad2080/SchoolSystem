using System;
using System.Data;
using SchoolSystem.DataAccess;
using SchoolSystem.Models;

namespace SchoolSystem.Services
{
    public class ClassService
    {
        private readonly ClassRepository repository = new ClassRepository();

        public DataTable GetAllClasses()
        {
            return repository.GetAllClasses();
        }

        public DataTable GetClassDetails()
        {
            return repository.GetClassDetails();
        }

        public bool UpdateClass(SchoolClass item)
        {
            if (item == null)
                throw new ArgumentException("بيانات الفصل غير صحيحة.");

            if (item.ClassID <= 0)
                throw new ArgumentException("اختر فصلًا صحيحًا.");

            if (string.IsNullOrWhiteSpace(item.ClassName))
                throw new ArgumentException("اسم الفصل مطلوب.");

            if (string.IsNullOrWhiteSpace(item.StageName))
                throw new ArgumentException("اسم المرحلة مطلوب.");

            if (item.GradeOrder <= 0)
                throw new ArgumentException("ترتيب الفصل غير صحيح.");

            return repository.UpdateClass(item);
        }
    }
}
