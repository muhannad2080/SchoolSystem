using System;
using System.Data;
using System.Windows.Forms;
using SchoolSystem.Helpers;
using SchoolSystem.Models;
using SchoolSystem.Services;

namespace SchoolSystem.UI
{
    public partial class StudentProfileForm : UserControl
    {
        private readonly int studentId;
        private readonly StudentProfileService profileService = new StudentProfileService();
        private StudentProfile profile;

        public StudentProfileForm()
        {
            InitializeComponent();
        }

        public StudentProfileForm(int studentId)
        {
            if (studentId <= 0)
                throw new ArgumentException("رقم الطالب غير صحيح.");

            this.studentId = studentId;
            InitializeComponent();
            Load += StudentProfileForm_Load;
        }

        private async void StudentProfileForm_Load(object sender, EventArgs e)
        {
            if (studentId > 0)
                await LoadProfileAsync();
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await LoadProfileAsync();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            CloseProfile();
        }

        private async System.Threading.Tasks.Task LoadProfileAsync()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                profile = await System.Threading.Tasks.Task.Run(() => profileService.GetProfile(studentId));
                if (profile == null || profile.Student == null)
                {
                    UIHelper.ShowWarning("لم يتم العثور على بيانات الطالب المحدد.");
                    CloseProfile();
                    return;
                }

                BindProfile();
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("تحميل ملف الطالب", ex);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void BindProfile()
        {
            Student student = profile.Student;
            lblTitle.Text = "ملف الطالب: " + Safe(student.FullName);
            lblIdentity.Text = "الرقم: " + Safe(student.StudentNumber) + "\r\nالاسم: " + Safe(student.FullName);
            lblContact.Text = "هاتف الطالب: " + Safe(student.StudentPhone) + "\r\nولي الأمر: " + Safe(student.GuardianName) + " - " + Safe(student.GuardianPhone);
            lblClassStatus.Text = "الصف: " + Safe(student.CurrentClassName) + "\r\nالحالة: " + Safe(student.Status);

            dgvAttendance.DataSource = profile.Attendance;
            dgvMarks.DataSource = profile.Marks;
            dgvFees.DataSource = profile.Fees;
            FormatAttendanceGrid();
            FormatMarksGrid();
            FormatFeesGrid();

            if (profile.CanViewFinancials)
            {
                decimal total = Sum(profile.Fees, "TotalAmount");
                decimal paid = Sum(profile.Fees, "PaidAmount");
                decimal remaining = Sum(profile.Fees, "RemainingAmount");
                lblFinancialSummary.Text = "الرسوم: " + total.ToString("N2") + " ريال | المدفوع: " + paid.ToString("N2") + " ريال | المتبقي: " + remaining.ToString("N2");
            }
            else
            {
                lblFinancialSummary.Text = "الوضع المالي: غير متاح حسب صلاحية المستخدم";
            }
        }

        private void FormatAttendanceGrid()
        {
            SetHeader(dgvAttendance, "AttendanceDate", "التاريخ");
            SetHeader(dgvAttendance, "Status", "الحالة");
            SetHeader(dgvAttendance, "ExcuseStatus", "العذر");
            SetHeader(dgvAttendance, "ArrivalTime", "وقت الوصول");
            SetHeader(dgvAttendance, "Notes", "ملاحظات");
            FormatDate(dgvAttendance, "AttendanceDate");
        }

        private void FormatMarksGrid()
        {
            SetHeader(dgvMarks, "SubjectName", "المادة");
            SetHeader(dgvMarks, "ExamType", "نوع الاختبار");
            SetHeader(dgvMarks, "MarkValue", "الدرجة");
            SetHeader(dgvMarks, "CreatedAt", "تاريخ الإدخال");
            FormatDate(dgvMarks, "CreatedAt");
            FormatNumber(dgvMarks, "MarkValue");
        }

        private void FormatFeesGrid()
        {
            SetHeader(dgvFees, "AcademicYear", "العام الدراسي");
            SetHeader(dgvFees, "FeeType", "نوع الرسوم");
            SetHeader(dgvFees, "TotalAmount", "الإجمالي");
            SetHeader(dgvFees, "DiscountAmount", "الخصم");
            SetHeader(dgvFees, "NetAmount", "الصافي");
            SetHeader(dgvFees, "PaidAmount", "المدفوع");
            SetHeader(dgvFees, "RemainingAmount", "المتبقي");
            SetHeader(dgvFees, "DueDate", "الاستحقاق");
            SetHeader(dgvFees, "PaymentDate", "الدفع");
            SetHeader(dgvFees, "Status", "الحالة");
            FormatDate(dgvFees, "DueDate");
            FormatDate(dgvFees, "PaymentDate");
            FormatNumber(dgvFees, "TotalAmount");
            FormatNumber(dgvFees, "DiscountAmount");
            FormatNumber(dgvFees, "NetAmount");
            FormatNumber(dgvFees, "PaidAmount");
            FormatNumber(dgvFees, "RemainingAmount");
        }

        private static decimal Sum(DataTable table, string column)
        {
            decimal total = 0m;
            if (table == null || !table.Columns.Contains(column))
                return total;
            foreach (DataRow row in table.Rows)
                if (row[column] != DBNull.Value)
                    total += Convert.ToDecimal(row[column]);
            return total;
        }

        private static string Safe(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? "-" : value.Trim();
        }

        private static void SetHeader(DataGridView grid, string name, string text)
        {
            if (grid.Columns.Contains(name))
                grid.Columns[name].HeaderText = text;
        }

        private static void FormatDate(DataGridView grid, string name)
        {
            if (grid.Columns.Contains(name))
                grid.Columns[name].DefaultCellStyle.Format = "dd/MM/yyyy";
        }

        private static void FormatNumber(DataGridView grid, string name)
        {
            if (grid.Columns.Contains(name))
                grid.Columns[name].DefaultCellStyle.Format = "N2";
        }

        private void CloseProfile()
        {
            if (MainForm.Instance != null)
                MainForm.Instance.LoadFormInPanel(new StudentsForm());
        }
    }
}
