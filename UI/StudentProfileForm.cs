using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using SchoolSystem.Helpers;
using SchoolSystem.Models;
using SchoolSystem.Services;

namespace SchoolSystem.UI
{
    public class StudentProfileForm : UserControl
    {
        private readonly int studentId;
        private readonly StudentProfileService profileService = new StudentProfileService();
        private StudentProfile profile;
        private readonly Label lblTitle = new Label();
        private readonly Label lblIdentity = new Label();
        private readonly Label lblContact = new Label();
        private readonly Label lblClassStatus = new Label();
        private readonly Label lblFinancialSummary = new Label();
        private readonly TabControl tabs = new TabControl();
        private readonly DataGridView dgvAttendance = CreateGrid();
        private readonly DataGridView dgvMarks = CreateGrid();
        private readonly DataGridView dgvFees = CreateGrid();

        public StudentProfileForm(int studentId)
        {
            if (studentId <= 0)
                throw new ArgumentException("رقم الطالب غير صحيح.");

            this.studentId = studentId;
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            Dock = DockStyle.Fill;
            BackColor = UIHelper.BackgroundColor;
            BuildLayout();
            Load += StudentProfileForm_Load;
        }

        private void BuildLayout()
        {
            Panel header = new Panel { Dock = DockStyle.Top, Height = 58, BackColor = UIHelper.PrimaryColor, Padding = new Padding(16, 8, 16, 8) };
            lblTitle.Text = "ملف الطالب الموحد";
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.ForeColor = Color.White;
            lblTitle.Font = new Font(UIHelper.FontFamily, 16F, FontStyle.Bold);
            lblTitle.TextAlign = ContentAlignment.MiddleRight;
            Button btnRefresh = CreateButton("تحديث", UIHelper.AccentColor);
            btnRefresh.Dock = DockStyle.Left;
            btnRefresh.Width = 100;
            btnRefresh.Click += async (s, e) => await LoadProfileAsync();
            Button btnBack = CreateButton("رجوع", Color.FromArgb(127, 140, 141));
            btnBack.Dock = DockStyle.Left;
            btnBack.Width = 100;
            btnBack.Margin = new Padding(0, 0, 8, 0);
            btnBack.Click += (s, e) => CloseProfile();
            header.Controls.Add(lblTitle);
            header.Controls.Add(btnBack);
            header.Controls.Add(btnRefresh);

            TableLayoutPanel summary = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 128,
                ColumnCount = 3,
                RowCount = 2,
                Padding = new Padding(12, 10, 12, 8),
                BackColor = UIHelper.SurfaceElevatedColor
            };
            for (int i = 0; i < 3; i++)
                summary.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.333F));
            summary.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            summary.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            ConfigureSummaryLabel(lblIdentity);
            ConfigureSummaryLabel(lblContact);
            ConfigureSummaryLabel(lblClassStatus);
            ConfigureSummaryLabel(lblFinancialSummary);
            summary.Controls.Add(lblIdentity, 0, 0);
            summary.Controls.Add(lblContact, 1, 0);
            summary.Controls.Add(lblClassStatus, 2, 0);
            summary.Controls.Add(lblFinancialSummary, 0, 1);

            tabs.Dock = DockStyle.Fill;
            tabs.Font = new Font(UIHelper.FontFamily, 10F, FontStyle.Bold);
            tabs.RightToLeft = RightToLeft.Yes;
            tabs.RightToLeftLayout = true;
            tabs.TabPages.Add(CreateTab("الحضور", dgvAttendance));
            tabs.TabPages.Add(CreateTab("الدرجات", dgvMarks));
            tabs.TabPages.Add(CreateTab("الرسوم والمدفوعات", dgvFees));

            Controls.Add(tabs);
            Controls.Add(summary);
            Controls.Add(header);
        }

        private static void ConfigureSummaryLabel(Label label)
        {
            label.Dock = DockStyle.Fill;
            label.TextAlign = ContentAlignment.MiddleRight;
            label.Padding = new Padding(12, 0, 12, 0);
            label.BackColor = UIHelper.SurfaceSecondaryColor;
            label.ForeColor = UIHelper.TextColor;
            label.Font = new Font(UIHelper.FontFamily, 10F, FontStyle.Bold);
            label.BorderStyle = BorderStyle.FixedSingle;
        }

        private static Button CreateButton(string text, Color color)
        {
            Button button = new Button
            {
                Text = text,
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font(UIHelper.FontFamily, 9.5F, FontStyle.Bold),
                UseVisualStyleBackColor = false
            };
            button.FlatAppearance.BorderSize = 0;
            return button;
        }

        private static TabPage CreateTab(string title, DataGridView grid)
        {
            TabPage page = new TabPage(title) { Padding = new Padding(8), BackColor = Color.White };
            page.Controls.Add(grid);
            return page;
        }

        private static DataGridView CreateGrid()
        {
            DataGridView grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                MultiSelect = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            UIHelper.StyleDataGridView(grid);
            return grid;
        }

        private async void StudentProfileForm_Load(object sender, EventArgs e)
        {
            await LoadProfileAsync();
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
                lblFinancialSummary.Text = "الرسوم: " + total.ToString("N2") + " ريال | المدفوع: " + paid.ToString("N2") + " ريال | المتبقي: " + remaining.ToString("N2") + " ريال";
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
