using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using SchoolSystem.DataAccess.Repositories;
using SchoolSystem.Models;
using SchoolSystem.Security;


namespace SchoolSystem.Services
{

    public class StudentService
    {

        private readonly StudentRepository _studentRepository;
        // تعريف الدالة AuditLogService لتنفيذ العملية المرتبطة بها.
        private readonly AuditLogService _auditLogService = new AuditLogService();

        // تعريف الدالة StudentService لتنفيذ العملية المرتبطة بها.
        public StudentService()
        {
            _studentRepository = new StudentRepository();
        }





        // تعريف الدالة GetAll لتنفيذ العملية المرتبطة بها.
        public List<Student> GetAll()
        {
            EnsureCanLookupStudents();

            return _studentRepository.GetAll();
        }

        // تعريف الدالة GetById لتنفيذ العملية المرتبطة بها.
        public Student GetById(int studentId)
        {
            EnsureCanLookupStudents();
            if (studentId <= 0)

                throw new ArgumentException("رقم الطالب غير صحيح.");


            return _studentRepository.GetById(studentId);
        }

        // تعريف الدالة Search لتنفيذ العملية المرتبطة بها.
        public List<Student> Search(string keyword)
        {
            EnsureCanLookupStudents();
            if (string.IsNullOrWhiteSpace(keyword))
                return GetAll();


            return _studentRepository.Search(keyword.Trim());
        }

        // تعريف الدالة Add لتنفيذ العملية المرتبطة بها.
        public int Add(Student student)
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.DemandAction("Students", "Add", "ليس لديك صلاحية إضافة الطلاب.");
            ValidateStudent(student);

            if (_studentRepository.IsNationalIdExists(student.NationalId))

                throw new Exception("رقم الهوية مستخدم مسبقاً لطالب آخر.");


            if (_studentRepository.IsPhoneExists(student.StudentPhone))

                throw new Exception("رقم هاتف الطالب مستخدم مسبقاً.");

            int studentId = _studentRepository.Add(student);
            _auditLogService.Record("إنشاء", "Student", studentId.ToString(),
                "إضافة طالب: " + (student.FullName ?? string.Empty));

            return studentId;
        }

        // تعريف الدالة Update لتنفيذ العملية المرتبطة بها.
        public void Update(Student student)
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.DemandAction("Students", "Edit", "ليس لديك صلاحية تعديل الطلاب.");
            if (student == null)

                throw new Exception("بيانات الطالب غير صحيحة.");


            if (student.StudentId <= 0)

                throw new Exception("يرجى اختيار طالب من الجدول قبل التعديل.");

            ValidateStudent(student);

            if (_studentRepository.IsNationalIdExists(student.NationalId, student.StudentId))

                throw new Exception("رقم الهوية مستخدم مسبقاً لطالب آخر.");


            if (_studentRepository.IsPhoneExists(student.StudentPhone, student.StudentId))

                throw new Exception("رقم هاتف الطالب مستخدم مسبقاً.");


            _studentRepository.Update(student);
            _auditLogService.Record("تعديل", "Student", student.StudentId.ToString(),
                "تعديل بيانات الطالب: " + (student.FullName ?? string.Empty));
        }

        // تعريف الدالة Delete لتنفيذ العملية المرتبطة بها.
        public void Delete(int studentId)
        {
            CurrentUser.DemandAction("Students", "Delete", "ليس لديك صلاحية حذف الطلاب.");
            if (studentId <= 0)

                throw new Exception("يرجى اختيار طالب من الجدول قبل الحذف.");


            _studentRepository.Delete(studentId);
            _auditLogService.Record("حذف", "Student", studentId.ToString(),
                "حذف سجل الطالب.");
        }

        // تعريف الدالة GenerateNextStudentNumber لتنفيذ العملية المرتبطة بها.
        public string GenerateNextStudentNumber()
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.DemandAction("Students", "Add", "ليس لديك صلاحية توليد رقم طالب جديد.");

            return _studentRepository.GenerateNextStudentNumber();
        }


        // دوال توافقية مع الواجهات القديمة



        // تعريف الدالة GetAllStudents لتنفيذ العملية المرتبطة بها.
        public DataTable GetAllStudents()
        {

            return ConvertStudentsToDataTable(GetAll());
        }

        // تعريف الدالة GetActiveStudents لتنفيذ العملية المرتبطة بها.
        public DataTable GetActiveStudents()
        {
            EnsureCanLookupStudents();

            return ConvertStudentsToDataTable(_studentRepository.GetActive());
        }

        /// <summary>
        /// Lists active students who may be enrolled in the requested academic year.
        /// Existing registration is excluded consistently with EnrollmentService.
        /// </summary>
        public DataTable GetStudentsAvailableForEnrollment(string academicYear, int excludeEnrollmentId = 0)
        {
            EnsureCanLookupStudents();

            if (string.IsNullOrWhiteSpace(academicYear))
                throw new ArgumentException("العام الدراسي مطلوب.", nameof(academicYear));

            if (excludeEnrollmentId < 0)
                throw new ArgumentOutOfRangeException(nameof(excludeEnrollmentId));

            return ConvertStudentsToDataTable(
                _studentRepository.GetActiveNotEnrolledForAcademicYear(academicYear, excludeEnrollmentId));
        }

        // تعريف الدالة GetStudentById لتنفيذ العملية المرتبطة بها.
        public Student GetStudentById(int studentId)
        {

            return GetById(studentId);
        }

        // تعريف الدالة SearchStudents لتنفيذ العملية المرتبطة بها.
        public DataTable SearchStudents(string keyword)
        {

            return ConvertStudentsToDataTable(Search(keyword));
        }

        // تعريف الدالة AddStudent لتنفيذ العملية المرتبطة بها.
        public int AddStudent(Student student)
        {

            return Add(student);
        }

        // تعريف الدالة UpdateStudent لتنفيذ العملية المرتبطة بها.
        public void UpdateStudent(Student student)
        {

            Update(student);
        }

        // تعريف الدالة DeleteStudent لتنفيذ العملية المرتبطة بها.
        public void DeleteStudent(int studentId)
        {

            Delete(studentId);
        }





        // تعريف الدالة ConvertStudentsToDataTable لتنفيذ العملية المرتبطة بها.
        private DataTable ConvertStudentsToDataTable(List<Student> students)
        {
            DataTable table = new DataTable();

            table.Columns.Add("StudentId", typeof(int));
            table.Columns.Add("StudentNumber", typeof(string));
            table.Columns.Add("FullName", typeof(string));
            table.Columns.Add("Gender", typeof(string));
            table.Columns.Add("BirthDate", typeof(DateTime));
            table.Columns.Add("BirthPlace", typeof(string));
            table.Columns.Add("Nationality", typeof(string));
            table.Columns.Add("NationalId", typeof(string));
            table.Columns.Add("StudentPhone", typeof(string));
            table.Columns.Add("Phone", typeof(string));          // للتوافق مع بعض الشاشات القديمة
            table.Columns.Add("Status", typeof(string));
            table.Columns.Add("GuardianName", typeof(string));
            table.Columns.Add("GuardianRelation", typeof(string));
            table.Columns.Add("GuardianPhone", typeof(string));
            table.Columns.Add("GuardianEmail", typeof(string));
            table.Columns.Add("GuardianJob", typeof(string));
            table.Columns.Add("Governorate", typeof(string));
            table.Columns.Add("District", typeof(string));
            table.Columns.Add("Address", typeof(string));
            table.Columns.Add("CurrentClassName", typeof(string));

            foreach (Student student in students)
            {
                DataRow row = table.NewRow();

                row["StudentId"] = student.StudentId;
                row["StudentNumber"] = student.StudentNumber ?? string.Empty;
                row["FullName"] = student.FullName ?? string.Empty;
                row["Gender"] = student.Gender ?? string.Empty;

                if (student.BirthDate.HasValue)
                    row["BirthDate"] = student.BirthDate.Value;

                else

                    row["BirthDate"] = DBNull.Value;

                row["BirthPlace"] = student.BirthPlace ?? string.Empty;
                row["Nationality"] = student.Nationality ?? string.Empty;
                row["NationalId"] = student.NationalId ?? string.Empty;
                row["StudentPhone"] = student.StudentPhone ?? string.Empty;
                row["Phone"] = student.StudentPhone ?? string.Empty;
                row["Status"] = student.Status ?? string.Empty;
                row["GuardianName"] = student.GuardianName ?? string.Empty;
                row["GuardianRelation"] = student.GuardianRelation ?? string.Empty;
                row["GuardianPhone"] = student.GuardianPhone ?? string.Empty;
                row["GuardianEmail"] = student.GuardianEmail ?? string.Empty;
                row["GuardianJob"] = student.GuardianJob ?? string.Empty;
                row["Governorate"] = student.Governorate ?? string.Empty;
                row["District"] = student.District ?? string.Empty;
                row["Address"] = student.Address ?? string.Empty;
                row["CurrentClassName"] = student.CurrentClassName ?? string.Empty;

                table.Rows.Add(row);
            }


            return table;
        }





        // تعريف الدالة EnsureCanLookupStudents لتنفيذ العملية المرتبطة بها.
        private static void EnsureCanLookupStudents()
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.DemandAny(
                "ليس لديك صلاحية عرض بيانات الطلاب.",
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                PermissionKeys.StudentsView,
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                PermissionKeys.StudentsManage,
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                PermissionKeys.EnrollmentManage,
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                PermissionKeys.FeesManage,
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                PermissionKeys.LibraryView,
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                PermissionKeys.LibraryManage,
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                PermissionKeys.ClassAssignmentManage,
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                PermissionKeys.AttendanceManage,
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                PermissionKeys.ReportsView,
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                PermissionKeys.DashboardView);
        }

        // تعريف الدالة ValidateStudent لتنفيذ العملية المرتبطة بها.
        private void ValidateStudent(Student student)
        {
            if (student == null)

                throw new Exception("بيانات الطالب غير صحيحة.");


            if (string.IsNullOrWhiteSpace(student.FullName))

                throw new Exception("الاسم الرباعي مطلوب.");

            string fullName = student.FullName.Trim();

            string[] nameParts = fullName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (nameParts.Length < 4)

                throw new Exception("يرجى إدخال الاسم الرباعي بشكل صحيح.");


            if (string.IsNullOrWhiteSpace(student.Gender))

                throw new Exception("يرجى اختيار الجنس.");


            if (student.Gender != "ذكر" && student.Gender != "أنثى")

                throw new Exception("قيمة الجنس غير صحيحة.");


            if (!student.BirthDate.HasValue)

                throw new Exception("تاريخ الميلاد مطلوب.");


            if (student.BirthDate.Value.Date > DateTime.Today)

                throw new Exception("تاريخ الميلاد لا يمكن أن يكون في المستقبل.");


            if (string.IsNullOrWhiteSpace(student.Status))

                throw new Exception("يرجى اختيار حالة الطالب.");


            if (student.Status != "نشط" &&
                student.Status != "موقوف" &&
                student.Status != "منقول" &&
                student.Status != "متخرج")
            {

                throw new Exception("حالة الطالب غير صحيحة.");
            }

            ValidateNationalId(student.NationalId);
            ValidatePhone(student.StudentPhone, "رقم هاتف الطالب غير صحيح.");
            ValidatePhone(student.GuardianPhone, "رقم هاتف ولي الأمر غير صحيح.");
            ValidateEmail(student.GuardianEmail);
        }

        // تعريف الدالة ValidateNationalId لتنفيذ العملية المرتبطة بها.
        private void ValidateNationalId(string nationalId)
        {
            if (string.IsNullOrWhiteSpace(nationalId))
                return;

            nationalId = nationalId.Trim();

            if (!Regex.IsMatch(nationalId, @"^[0-9]{6,20}$"))

                throw new Exception("رقم الهوية يجب أن يحتوي على أرقام فقط من 6 إلى 20 رقم.");
        }

        // تعريف الدالة ValidatePhone لتنفيذ العملية المرتبطة بها.
        private void ValidatePhone(string phone, string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return;

            phone = phone.Trim();

            if (!Regex.IsMatch(phone, @"^[0-9+\-\s]{7,20}$"))

                throw new Exception(errorMessage);
        }

        // تعريف الدالة ValidateEmail لتنفيذ العملية المرتبطة بها.
        private void ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return;

            email = email.Trim();

            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))

                throw new Exception("البريد الإلكتروني لولي الأمر غير صحيح.");
        }
    }
}
