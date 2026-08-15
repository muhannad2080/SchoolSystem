using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
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
        private readonly PrintDocument profilePrintDocument = new PrintDocument();

        public StudentProfileForm()
        {
            InitializeComponent();
            UIHelper.ApplyInputValidation(this);
            ConfigureGrid(dgvAttendance);
            ConfigureGrid(dgvMarks);
            ConfigureGrid(dgvFees);
            ConfigureOutputButtons();
        }

        public StudentProfileForm(int studentId)
        {
            if (studentId <= 0)
                throw new ArgumentException("رقم الطالب غير صحيح.");

            this.studentId = studentId;
            InitializeComponent();
            UIHelper.ApplyInputValidation(this);
            ConfigureGrid(dgvAttendance);
            ConfigureGrid(dgvMarks);
            ConfigureGrid(dgvFees);
            ConfigureOutputButtons();
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
            BindStudentPhoto(student.Photo);

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
            tabs.RightToLeft = RightToLeft.Yes;
            tabs.RightToLeftLayout = true;
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

        private void BindStudentPhoto(byte[] photoBytes)
        {
            Image previous = studentPictureBox.Image;
            studentPictureBox.Image = null;
            if (previous != null)
                previous.Dispose();

            if (photoBytes == null || photoBytes.Length == 0)
                return;

            try
            {
                using (MemoryStream stream = new MemoryStream(photoBytes, false))
                using (Image source = Image.FromStream(stream))
                {
                    // Image.FromStream يعتمد على بقاء stream مفتوحاً؛ نسخ الصورة يمنع
                    // تلف العرض بعد خروجنا من using.
                    studentPictureBox.Image = new Bitmap(source);
                }
            }
            catch (ArgumentException)
            {
                studentPictureBox.Image = null;
            }
            catch (OutOfMemoryException)
            {
                studentPictureBox.Image = null;
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
            grid.RightToLeft = RightToLeft.Yes;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.BackgroundColor = System.Drawing.Color.White;
            grid.BorderStyle = BorderStyle.None;
            grid.Dock = DockStyle.Fill;
            grid.MultiSelect = false;
            grid.ReadOnly = true;
            grid.RowHeadersVisible = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.ScrollBars = ScrollBars.Both;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            UIHelper.StyleDataGridView(grid);
            grid.RightToLeft = RightToLeft.Yes;
            grid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
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

        private void ConfigureOutputButtons()
        {
            if (headerPanel == null)
                return;

            Button printButton = CreateOutputButton("طباعة | Print", UIHelper.PrimaryColor);
            Button pdfButton = CreateOutputButton("PDF", UIHelper.DangerColor);
            Button excelButton = CreateOutputButton("Excel", UIHelper.SuccessColor);
            headerPanel.RightToLeft = RightToLeft.Yes;
            printButton.Click += delegate { PrintProfilePreview(); };
            pdfButton.Click += delegate { ExportProfilePdf(); };
            excelButton.Click += delegate { ExportProfileExcel(); };
            headerPanel.Controls.Add(excelButton);
            headerPanel.Controls.Add(pdfButton);
            headerPanel.Controls.Add(printButton);
            profilePrintDocument.PrintPage += ProfilePrintDocument_PrintPage;
        }

        private Button CreateOutputButton(string text, Color color)
        {
            Button button = new Button
            {
                Text = text,
                Width = 105,
                Height = 32,
                Dock = DockStyle.Right,
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font(UIHelper.FontFamily, 8.5F, FontStyle.Bold),
                UseVisualStyleBackColor = false,
                RightToLeft = RightToLeft.No,
                TextAlign = ContentAlignment.MiddleCenter,
                Margin = new Padding(4)
            };
            button.FlatAppearance.BorderSize = 0;
            return button;
        }

        private DataTable BuildProfileOutputTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("الحقل | Field");
            table.Columns.Add("القيمة | Value");
            if (profile == null || profile.Student == null)
                return table;
            Student student = profile.Student;
            table.Rows.Add("رقم الطالب | Student No.", Safe(student.StudentNumber));
            table.Rows.Add("الاسم | Name", Safe(student.FullName));
            table.Rows.Add("الجنس | Gender", Safe(student.Gender));
            table.Rows.Add("الصف | Class", Safe(student.CurrentClassName));
            table.Rows.Add("الحالة | Status", Safe(student.Status));
            table.Rows.Add("هاتف ولي الأمر | Guardian Phone", Safe(student.GuardianPhone));
            table.Rows.Add("الرقم الوطني | National ID", Safe(student.NationalId));
            return table;
        }

        private bool EnsureProfileOutput()
        {
            if (profile == null || profile.Student == null)
            {
                UIHelper.ShowWarning("انتظر تحميل ملف الطالب أولاً.");
                return false;
            }
            return true;
        }

        private void ExportProfileExcel()
        {
            if (!EnsureProfileOutput()) return;
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "ملفات Excel (*.xlsx)|*.xlsx";
                dialog.FileName = "Student_Profile_" + Safe(profile.Student.StudentNumber) + ".xlsx";
                if (dialog.ShowDialog(FindForm()) != DialogResult.OK) return;
                try
                {
                    DataTable table = BuildProfileOutputTable();
                    ReportOutputHelper.ExportToExcel(table, dialog.FileName,
                        "ملف الطالب | Student Profile - " + Safe(profile.Student.FullName),
                        "بيانات الهوية | Identity details");
                    UIHelper.ShowInfo("تم تصدير ملف الطالب إلى Excel بنجاح.");
                }
                catch (Exception ex) { UIHelper.ShowException("تصدير ملف الطالب إلى Excel", ex); }
            }
        }

        private void ExportProfilePdf()
        {
            if (!EnsureProfileOutput()) return;
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "ملفات PDF (*.pdf)|*.pdf";
                dialog.FileName = "Student_Profile_" + Safe(profile.Student.StudentNumber) + ".pdf";
                if (dialog.ShowDialog(FindForm()) != DialogResult.OK) return;
                try
                {
                    DataTable table = BuildProfileOutputTable();
                    ReportOutputHelper.ExportToPdf(table, dialog.FileName,
                        "ملف الطالب | Student Profile - " + Safe(profile.Student.FullName),
                        "بيانات الهوية | Identity details");
                    UIHelper.ShowInfo("تم تصدير ملف الطالب إلى PDF بنجاح.");
                }
                catch (Exception ex) { UIHelper.ShowException("تصدير ملف الطالب إلى PDF", ex); }
            }
        }

        private void PrintProfilePreview()
        {
            if (!EnsureProfileOutput()) return;
            try
            {
                using (PrintPreviewDialog preview = new PrintPreviewDialog())
                {
                    preview.Document = profilePrintDocument;
                    preview.RightToLeft = RightToLeft.Yes;
                    preview.WindowState = FormWindowState.Maximized;
                    preview.ShowDialog(FindForm());
                }
            }
            catch (Exception ex) { UIHelper.ShowException("معاينة ملف الطالب", ex); }
        }

        private void ProfilePrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            Student student = profile == null ? null : profile.Student;
            if (student == null)
            {
                e.HasMorePages = false;
                return;
            }

            bool rtl = ReportOutputHelper.ContainsArabic(student.FullName)
                       || ReportOutputHelper.ContainsArabic(student.CurrentClassName)
                       || ReportOutputHelper.ContainsArabic(student.Nationality);
            Rectangle area = e.MarginBounds;
            int photoSize = Math.Min(110, Math.Max(76, area.Width / 7));
            int contentTop = area.Top + 58;
            Rectangle photoBounds = rtl
                ? new Rectangle(area.Right - photoSize, area.Top + 50, photoSize, photoSize)
                : new Rectangle(area.Left, area.Top + 50, photoSize, photoSize);

            using (Font title = new Font("Tahoma", 17, FontStyle.Bold))
            using (Font label = new Font("Tahoma", 9.5F, FontStyle.Bold))
            using (Font value = new Font("Tahoma", 10))
            using (Font footer = new Font("Tahoma", 8.5F))
            using (StringFormat titleFormat = CreateProfileFormat(rtl, false))
            using (StringFormat rtlFormat = CreateProfileFormat(true, true))
            using (StringFormat ltrFormat = CreateProfileFormat(false, false))
            {
                e.Graphics.FillRectangle(Brushes.White, area);
                using (Pen border = new Pen(Color.FromArgb(37, 99, 235), 2f))
                    e.Graphics.DrawRectangle(border, area.Left, area.Top, area.Width - 1, area.Height - 1);

                e.Graphics.DrawString("ملف الطالب | Student Profile", title, Brushes.Black,
                    new Rectangle(area.Left, area.Top + 8, area.Width, 36), titleFormat);

                if (studentPictureBox.Image != null)
                {
                    e.Graphics.DrawImage(studentPictureBox.Image, photoBounds);
                    using (Pen photoBorder = new Pen(Color.LightGray, 1f))
                        e.Graphics.DrawRectangle(photoBorder, photoBounds);
                }

                int left = rtl ? area.Left : area.Left + photoSize + 18;
                int right = rtl ? area.Right - photoSize - 18 : area.Right;
                int width = Math.Max(180, right - left);
                int labelWidth = Math.Max(125, width / 3);
                int valueWidth = width - labelWidth - 12;
                contentTop = Math.Max(contentTop, photoBounds.Bottom + 14);
                int y = contentTop;
                string[][] rows =
                {
                    new[] { "رقم الطالب | Student No.", Safe(student.StudentNumber) },
                    new[] { "الاسم | Name", Safe(student.FullName) },
                    new[] { "الجنس | Gender", Safe(student.Gender) },
                    new[] { "الصف | Class", Safe(student.CurrentClassName) },
                    new[] { "الحالة | Status", Safe(student.Status) },
                    new[] { "هاتف ولي الأمر | Guardian Phone", Safe(student.GuardianPhone) },
                    new[] { "الرقم الوطني | National ID", Safe(student.NationalId) }
                };

                foreach (string[] row in rows)
                {
                    Rectangle labelBounds = rtl
                        ? new Rectangle(right - labelWidth, y, labelWidth, 34)
                        : new Rectangle(left, y, labelWidth, 34);
                    Rectangle valueBounds = rtl
                        ? new Rectangle(left, y, valueWidth, 34)
                        : new Rectangle(left + labelWidth + 12, y, valueWidth, 34);
                    StringFormat valueFormat = ReportOutputHelper.ContainsArabic(row[1]) ? rtlFormat : ltrFormat;
                    e.Graphics.DrawString(row[0], label, Brushes.Black, labelBounds, rtlFormat);
                    e.Graphics.DrawString(row[1], value, Brushes.Black, valueBounds, valueFormat);
                    e.Graphics.DrawLine(Pens.LightGray, left, y + 36, right, y + 36);
                    y += 43;
                }

                StringFormat footerFormat = rtl ? rtlFormat : ltrFormat;
                e.Graphics.DrawString("تاريخ الإصدار | Issued: " + DateTime.Now.ToString("yyyy-MM-dd"), footer,
                    Brushes.DimGray, new Rectangle(area.Left, area.Bottom - 28, area.Width, 22), footerFormat);
            }
            e.HasMorePages = false;
        }

        private static StringFormat CreateProfileFormat(bool rtl, bool wrap)
        {
            return new StringFormat
            {
                Alignment = rtl ? StringAlignment.Far : StringAlignment.Near,
                LineAlignment = StringAlignment.Center,
                FormatFlags = (rtl ? StringFormatFlags.DirectionRightToLeft : StringFormatFlags.FitBlackBox)
                              | (wrap ? StringFormatFlags.NoClip : StringFormatFlags.FitBlackBox),
                Trimming = wrap ? StringTrimming.EllipsisWord : StringTrimming.EllipsisCharacter
            };
        }

        private void CloseProfile()
        {
            if (MainForm.Instance != null)
                MainForm.Instance.LoadFormInPanel(new StudentsForm());
        }
    }
}
