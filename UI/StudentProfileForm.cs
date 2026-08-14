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
            ConfigureGrid(dgvAttendance);
            ConfigureGrid(dgvMarks);
            ConfigureGrid(dgvFees);
        }

        public StudentProfileForm(int studentId)
        {
            if (studentId <= 0)
                throw new ArgumentException("رقم الطالب غير صحيح.");

            this.studentId = studentId;
            InitializeComponent();
            ConfigureGrid(dgvAttendance);
            ConfigureGrid(dgvMarks);
            ConfigureGrid(dgvFees);
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
            lblIdentity.Text = "الرقم: " + Safe(student.StudentNumber) + "\r\nالاسم: " + Safe(student.FullName) + "\r\nالجنس: " + Safe(student.Gender) + " | الجنسية: " + Safe(student.Nationality) + "\r\nالميلاد: " + FormatBirthDate(student.BirthDate) + " - " + Safe(student.BirthPlace);
            lblContact.Text = "هاتف الطالب: " + Safe(student.StudentPhone) + "\r\nولي الأمر: " + Safe(student.GuardianName) + " - " + Safe(student.GuardianPhone) + "\r\nصلة القرابة: " + Safe(student.GuardianRelation);
            lblClassStatus.Text = "الصف: " + Safe(student.CurrentClassName) + "\r\nالحالة: " + Safe(student.Status) + "\r\nالرقم الوطني: " + Safe(student.NationalId) + "\r\nالعنوان: " + Safe(student.Governorate) + " - " + Safe(student.District);

            dgvAttendance.DataSource = profile.Attendance;
            dgvMarks.DataSource = profile.Marks;
            dgvFees.DataSource = profile.Fees;
            if (profile.CanViewFinancials)
            {
                if (!tabs.TabPages.Contains(feesTab))
                    tabs.TabPages.Add(feesTab);
            }
            else if (tabs.TabPages.Contains(feesTab))
            {
                tabs.TabPages.Remove(feesTab);
            }
            BindAttendanceSummary();
            BindAcademicSummary();
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

        private void BindAttendanceSummary()
        {
            int total = profile.Attendance == null ? 0 : profile.Attendance.Rows.Count;
            int present = CountStatus(profile.Attendance, "حاضر", "Present");
            int absent = CountStatus(profile.Attendance, "غائب", "Absent");
            decimal rate = total == 0 ? 0 : (present * 100m) / total;
            lblAttendanceSummary.Text = "الحضور والانتظام\r\nالسجلات: " + total + " | حاضر: " + present + " | غائب: " + absent + "\r\nنسبة الحضور: " + rate.ToString("N1") + "%";
        }

        private void BindAcademicSummary()
        {
            int count = profile.Marks == null ? 0 : profile.Marks.Rows.Count;
            decimal average = Average(profile.Marks, "MarkValue");
            lblAcademicSummary.Text = "الأداء الأكاديمي\r\nعدد الدرجات: " + count + "\r\nالمتوسط العام: " + average.ToString("N2");
        }

        private static int CountStatus(DataTable table, params string[] statuses)
        {
            if (table == null || !table.Columns.Contains("Status"))
                return 0;
            int count = 0;
            foreach (DataRow row in table.Rows)
            {
                string value = Convert.ToString(row["Status"]);
                foreach (string status in statuses)
                    if (string.Equals(value, status, StringComparison.OrdinalIgnoreCase))
                    {
                        count++;
                        break;
                    }
            }
            return count;
        }

        private static decimal Average(DataTable table, string column)
        {
            if (table == null || !table.Columns.Contains(column) || table.Rows.Count == 0)
                return 0m;
            decimal total = 0m;
            int count = 0;
            foreach (DataRow row in table.Rows)
                if (row[column] != DBNull.Value)
                {
                    total += Convert.ToDecimal(row[column]);
                    count++;
                }
            return count == 0 ? 0m : total / count;
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

        private static void ConfigureGrid(DataGridView grid)
        {
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.BackgroundColor = System.Drawing.Color.White;
            grid.BorderStyle = BorderStyle.None;
            grid.Dock = DockStyle.Fill;
            grid.MultiSelect = false;
            grid.ReadOnly = true;
            grid.RowHeadersVisible = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            UIHelper.StyleDataGridView(grid);
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

        private static string FormatBirthDate(DateTime? date)
        {
            return date.HasValue ? date.Value.ToString("dd/MM/yyyy") : "-";
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
