using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;
using SchoolSystem.Helpers;
using SchoolSystem.Models;
using SchoolSystem.Security;
using SchoolSystem.Services;


namespace SchoolSystem.UI
{

    public partial class StudentProfileForm : UserControl
    {

        private readonly int studentId;
        // تعريف الدالة StudentProfileService لتنفيذ العملية المرتبطة بها.
        private readonly StudentProfileService profileService = new StudentProfileService();

        private StudentProfile profile;
        // تعريف الدالة PrintDocument لتنفيذ العملية المرتبطة بها.
        private readonly PrintDocument profilePrintDocument = new PrintDocument();

        // تعريف الدالة StudentProfileForm لتنفيذ العملية المرتبطة بها.
        public StudentProfileForm()
        {

            InitializeComponent();

            ApplyArabicRtlLayout();
            UIHelper.ApplyStyle(this);
            ConfigureGrid(dgvAttendance);
            ConfigureGrid(dgvMarks);
            ConfigureGrid(dgvFees);
            ConfigureOutputButtons();
        }

        // تعريف الدالة StudentProfileForm لتنفيذ العملية المرتبطة بها.
        public StudentProfileForm(int studentId)
        {
            if (studentId <= 0)

                throw new ArgumentException("رقم الطالب غير صحيح.");

            this.studentId = studentId;

            InitializeComponent();

            ApplyArabicRtlLayout();
            UIHelper.ApplyStyle(this);
            ConfigureGrid(dgvAttendance);
            ConfigureGrid(dgvMarks);
            ConfigureGrid(dgvFees);
            ConfigureOutputButtons();
            Load += StudentProfileForm_Load;
        }

        // تعريف الدالة ApplyArabicRtlLayout لتنفيذ العملية المرتبطة بها.
        private void ApplyArabicRtlLayout()
        {
            RightToLeft = RightToLeft.Yes;
            foreach (DataGridView grid in new[] { dgvMarks, dgvAttendance, dgvFees })
            {
                grid.RightToLeft = RightToLeft.Yes;
                grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                grid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                grid.RowHeadersVisible = false;
            }
            lblTitle.Text = "ملف الطالب الموحد";
        }

        // تعريف الدالة StudentProfileForm_Load لتنفيذ العملية المرتبطة بها.
        private async void StudentProfileForm_Load(object sender, EventArgs e)
        {
            if (studentId > 0)

                await LoadProfileAsync();
        }

        // تعريف الدالة btnRefresh_Click لتنفيذ العملية المرتبطة بها.
        private async void btnRefresh_Click(object sender, EventArgs e)
        {

            await LoadProfileAsync();
        }

        // تعريف الدالة btnBack_Click لتنفيذ العملية المرتبطة بها.
        private void btnBack_Click(object sender, EventArgs e)
        {
            CloseProfile();
        }

        // تعريف الدالة LoadProfileAsync لتنفيذ العملية المرتبطة بها.
        private async System.Threading.Tasks.Task LoadProfileAsync()
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
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

        // تعريف الدالة BindProfile لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة BindStudentPhoto لتنفيذ العملية المرتبطة بها.
        private void BindStudentPhoto(byte[] photoBytes)
        {
            Image previous = studentPictureBox.Image;
            studentPictureBox.Image = null;
            if (previous != null)
                previous.Dispose();


            if (photoBytes == null || photoBytes.Length == 0)
                return;

            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                using (MemoryStream stream = new MemoryStream(photoBytes, false))
                using (Image source = Image.FromStream(stream))
                {


                    studentPictureBox.Image = new Bitmap(source);
                }
            }

            catch (ArgumentException ex)
            {
                ApplicationLogger.LogException("شاشة ملف الطالب: عرض بيانات الطالب", ex);
                studentPictureBox.Image = null;
            }

            catch (OutOfMemoryException ex)
            {
                ApplicationLogger.LogException("شاشة ملف الطالب: عرض بيانات الطالب", ex);
                studentPictureBox.Image = null;
            }
        }

        // تعريف الدالة BindAttendanceSummary لتنفيذ العملية المرتبطة بها.
        private void BindAttendanceSummary()
        {
            int total = profile.Attendance == null ? 0 : profile.Attendance.Rows.Count;
            int present = CountStatus(profile.Attendance, "حاضر", "Present");
            int absent = CountStatus(profile.Attendance, "غائب", "Absent");
            decimal rate = total == 0 ? 0 : (present * 100m) / total;
            lblAttendanceSummary.Text = "الحضور والانتظام\r\nالسجلات: " + total + " | حاضر: " + present + " | غائب: " + absent + "\r\nنسبة الحضور: " + rate.ToString("N1") + "%";
        }

        // تعريف الدالة BindAcademicSummary لتنفيذ العملية المرتبطة بها.
        private void BindAcademicSummary()
        {
            int count = profile.Marks == null ? 0 : profile.Marks.Rows.Count;
            decimal average = Average(profile.Marks, "MarkValue");
            lblAcademicSummary.Text = "الأداء الأكاديمي\r\nعدد الدرجات: " + count + "\r\nالمتوسط العام: " + average.ToString("N2");
        }

        // تعريف الدالة CountStatus لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة Average لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة FormatAttendanceGrid لتنفيذ العملية المرتبطة بها.
        private void FormatAttendanceGrid()
        {
            SetHeader(dgvAttendance, "AttendanceDate", "التاريخ");
            SetHeader(dgvAttendance, "Status", "الحالة");
            SetHeader(dgvAttendance, "ExcuseStatus", "العذر");
            SetHeader(dgvAttendance, "ArrivalTime", "وقت الوصول");
            SetHeader(dgvAttendance, "Notes", "ملاحظات");
            FormatDate(dgvAttendance, "AttendanceDate");
        }

        // تعريف الدالة FormatMarksGrid لتنفيذ العملية المرتبطة بها.
        private void FormatMarksGrid()
        {
            SetHeader(dgvMarks, "SubjectName", "المادة");
            SetHeader(dgvMarks, "ExamType", "نوع الاختبار");
            SetHeader(dgvMarks, "MarkValue", "الدرجة");
            SetHeader(dgvMarks, "CreatedAt", "تاريخ الإدخال");
            FormatDate(dgvMarks, "CreatedAt");
            FormatNumber(dgvMarks, "MarkValue");
        }

        // تعريف الدالة FormatFeesGrid لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة ConfigureGrid لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة Sum لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة FormatBirthDate لتنفيذ العملية المرتبطة بها.
        private static string FormatBirthDate(DateTime? date)
        {

            return date.HasValue ? date.Value.ToString("dd/MM/yyyy") : "-";
        }

        // تعريف الدالة Safe لتنفيذ العملية المرتبطة بها.
        private static string Safe(string value)
        {

            return string.IsNullOrWhiteSpace(value) ? "-" : value.Trim();
        }

        // تعريف الدالة SetHeader لتنفيذ العملية المرتبطة بها.
        private static void SetHeader(DataGridView grid, string name, string text)
        {
            if (grid.Columns.Contains(name))
                grid.Columns[name].HeaderText = text;
        }

        // تعريف الدالة FormatDate لتنفيذ العملية المرتبطة بها.
        private static void FormatDate(DataGridView grid, string name)
        {
            if (grid.Columns.Contains(name))
                grid.Columns[name].DefaultCellStyle.Format = "dd/MM/yyyy";
        }

        // تعريف الدالة FormatNumber لتنفيذ العملية المرتبطة بها.
        private static void FormatNumber(DataGridView grid, string name)
        {
            if (grid.Columns.Contains(name))
                grid.Columns[name].DefaultCellStyle.Format = "N2";
        }

        // تعريف الدالة ConfigureOutputButtons لتنفيذ العملية المرتبطة بها.
        private void ConfigureOutputButtons()
        {
            if (headerPanel == null)
                return;

            headerPanel.RightToLeft = RightToLeft.Yes;
            UIHelper.StyleButton(btnPrintProfile, UIHelper.PrimaryColor);
            UIHelper.StyleButton(btnExportProfilePdf, UIHelper.DangerColor);
            UIHelper.StyleButton(btnExportProfileExcel, UIHelper.SuccessColor);
            btnPrintProfile.Click -= btnPrintProfile_Click;
            btnExportProfilePdf.Click -= btnExportProfilePdf_Click;
            btnExportProfileExcel.Click -= btnExportProfileExcel_Click;

            btnPrintProfile.Click += btnPrintProfile_Click;

            btnExportProfilePdf.Click += btnExportProfilePdf_Click;

            btnExportProfileExcel.Click += btnExportProfileExcel_Click;

            profilePrintDocument.PrintPage += ProfilePrintDocument_PrintPage;
        }

        // تعريف الدالة btnPrintProfile_Click لتنفيذ العملية المرتبطة بها.
        private void btnPrintProfile_Click(object sender, EventArgs e)
        {
            PrintProfilePreview();
        }

        // تعريف الدالة btnExportProfilePdf_Click لتنفيذ العملية المرتبطة بها.
        private void btnExportProfilePdf_Click(object sender, EventArgs e)
        {
            ExportProfilePdf();
        }

        // تعريف الدالة btnExportProfileExcel_Click لتنفيذ العملية المرتبطة بها.
        private void btnExportProfileExcel_Click(object sender, EventArgs e)
        {
            ExportProfileExcel();
        }

        // تعريف الدالة BuildProfileOutputTable لتنفيذ العملية المرتبطة بها.
        private DataTable BuildProfileOutputTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("البيان");
            table.Columns.Add("القيمة");
            if (profile == null || profile.Student == null)
                return table;
            Student student = profile.Student;
            table.Rows.Add("رقم الطالب", Safe(student.StudentNumber));
            table.Rows.Add("اسم الطالب", Safe(student.FullName));
            table.Rows.Add("الجنس", Safe(student.Gender));
            table.Rows.Add("الصف والشعبة", Safe(student.CurrentClassName));
            table.Rows.Add("الحالة", Safe(student.Status));
            table.Rows.Add("هاتف ولي الأمر", Safe(student.GuardianPhone));
            table.Rows.Add("الرقم الوطني", Safe(student.NationalId));

            return table;
        }

        // تعريف الدالة EnsureProfileOutput لتنفيذ العملية المرتبطة بها.
        private bool EnsureProfileOutput()
        {
            if (profile == null || profile.Student == null)
            {
                UIHelper.ShowWarning("انتظر تحميل ملف الطالب أولاً.");

                return false;
            }

            return true;
        }

        // تعريف الدالة ExportProfileExcel لتنفيذ العملية المرتبطة بها.
        private void ExportProfileExcel()
        {
            if (!EnsureProfileOutput()) return;
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "ملفات Excel (*.xlsx)|*.xlsx";
                dialog.FileName = "ملف_الطالب_" + Safe(profile.Student.StudentNumber) + ".xlsx";
                if (dialog.ShowDialog(FindForm()) != DialogResult.OK) return;
                // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
                try
                {
                    DataTable table = BuildProfileOutputTable();
                    ReportOutputHelper.ExportToExcel(table, dialog.FileName,
                        "ملف الطالب - " + Safe(profile.Student.FullName),
                        "بيانات الطالب الأساسية");
                    UIHelper.ShowInfo("تم تصدير ملف الطالب إلى Excel بنجاح.");
                }

                catch (Exception ex) { UIHelper.ShowException("تصدير ملف الطالب إلى Excel", ex); }
            }
        }

        // تعريف الدالة ExportProfilePdf لتنفيذ العملية المرتبطة بها.
        private void ExportProfilePdf()
        {
            if (!EnsureProfileOutput()) return;
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "ملفات PDF (*.pdf)|*.pdf";
                dialog.FileName = "ملف_الطالب_" + Safe(profile.Student.StudentNumber) + ".pdf";
                if (dialog.ShowDialog(FindForm()) != DialogResult.OK) return;
                // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
                try
                {
                    DataTable table = BuildProfileOutputTable();
                    ReportOutputHelper.ExportToPdf(table, dialog.FileName,
                        "ملف الطالب - " + Safe(profile.Student.FullName),
                        "بيانات الطالب الأساسية");
                    UIHelper.ShowInfo("تم تصدير ملف الطالب إلى PDF بنجاح.");
                }

                catch (Exception ex) { UIHelper.ShowException("تصدير ملف الطالب إلى PDF", ex); }
            }
        }

        // تعريف الدالة PrintProfilePreview لتنفيذ العملية المرتبطة بها.
        private void PrintProfilePreview()
        {
            if (!EnsureProfileOutput()) return;
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
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

        // تعريف الدالة ProfilePrintDocument_PrintPage لتنفيذ العملية المرتبطة بها.
        private void ProfilePrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            Student student = profile == null ? null : profile.Student;
            if (student == null)
            {
                e.HasMorePages = false;

                return;
            }


            bool rtl = true;
            Rectangle pageBounds = e.MarginBounds;
            int cardWidth = Math.Min(pageBounds.Width - 24, 620);
            int cardHeight = Math.Min(pageBounds.Height - 24, 680);
            Rectangle area = new Rectangle(
                pageBounds.Left + Math.Max(0, (pageBounds.Width - cardWidth) / 2),
                pageBounds.Top + 12,
                cardWidth,
                Math.Max(520, cardHeight));
            int photoSize = Math.Min(110, Math.Max(76, area.Width / 7));
            int contentTop = area.Top + 145;
            Rectangle photoBounds = new Rectangle(
                area.Right - photoSize - 18,
                area.Top + 145,
                photoSize,
                photoSize);

            SchoolSettings branding = SchoolSettingsService.Current();
            string schoolName = branding == null || string.IsNullOrWhiteSpace(branding.SchoolNameAr)
                ? ReportOutputHelper.GetSchoolName(branding)
                : branding.SchoolNameAr.Trim();
            string organization = branding == null ? string.Empty : (branding.OrganizationNameAr ?? string.Empty).Trim();
            string address = branding == null ? string.Empty : (branding.AddressAr ?? string.Empty).Trim();
            string contact = branding == null ? string.Empty : (branding.Phone ?? string.Empty).Trim();

            using (Font schoolFont = new Font("Tahoma", 13, FontStyle.Bold))
            using (Font organizationFont = new Font("Tahoma", 8.5F, FontStyle.Regular))
            using (Font title = new Font("Tahoma", 17, FontStyle.Bold))
            using (Font label = new Font("Tahoma", 9.5F, FontStyle.Bold))
            using (Font value = new Font("Tahoma", 10))
            using (Font footer = new Font("Tahoma", 8.5F))
            using (StringFormat titleFormat = CreateProfileFormat(rtl, false))
            using (StringFormat centerFormat = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center,
                FormatFlags = StringFormatFlags.FitBlackBox,
                Trimming = StringTrimming.EllipsisCharacter
            })
            using (StringFormat rtlFormat = CreateProfileFormat(true, true))
            using (StringFormat ltrFormat = CreateProfileFormat(false, false))
            {
                e.Graphics.FillRectangle(Brushes.White, area);
                using (Pen border = new Pen(Color.FromArgb(37, 99, 235), 2f))
                    e.Graphics.DrawRectangle(border, area.Left, area.Top, area.Width - 1, area.Height - 1);
                using (Brush headerBrush = new SolidBrush(Color.FromArgb(239, 246, 255)))
                    e.Graphics.FillRectangle(headerBrush, area.Left + 2, area.Top + 2, area.Width - 4, 126);
                using (Pen headerLine = new Pen(Color.FromArgb(37, 99, 235), 1f))
                    e.Graphics.DrawLine(headerLine, area.Left + 12, area.Top + 126, area.Right - 12, area.Top + 126);

                Rectangle logoBounds = new Rectangle(area.Right - 86, area.Top + 14, 64, 64);
                if (branding != null && branding.Logo != null && branding.Logo.Length > 0)
                {
                    // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
                    try
                    {
                        using (MemoryStream logoStream = new MemoryStream(branding.Logo))
                        using (Image logo = Image.FromStream(logoStream))
                        {
                            e.Graphics.DrawImage(logo, logoBounds);
                        }
                    }

                    catch (Exception logoEx)
                    {
                        // الشعار تكميلي في ترويسة بطاقة الطالب؛ فشله يُسجل ولا يوقف الطباعة.
                        ApplicationLogger.LogException("رسم شعار بطاقة الطالب", logoEx);
                    }
                }

                else
                {
                    using (Pen logoPlaceholder = new Pen(Color.LightGray, 1f))
                        e.Graphics.DrawRectangle(logoPlaceholder, logoBounds);
                    e.Graphics.DrawString("الشعار", organizationFont, Brushes.Gray, logoBounds, rtlFormat);
                }

                Rectangle schoolBounds = new Rectangle(area.Left + 18, area.Top + 15, area.Width - 120, 32);
                Rectangle organizationBounds = new Rectangle(area.Left + 18, area.Top + 48, area.Width - 120, 20);
                Rectangle contactBounds = new Rectangle(area.Left + 18, area.Top + 70, area.Width - 120, 20);
                e.Graphics.DrawString(schoolName, schoolFont, Brushes.Black, schoolBounds, titleFormat);
                e.Graphics.DrawString(organization, organizationFont, Brushes.DimGray, organizationBounds, rtlFormat);
                string contactLine = address;
                if (!string.IsNullOrWhiteSpace(contact))
                    contactLine = string.IsNullOrWhiteSpace(contactLine) ? "هاتف: " + contact : contactLine + "   |   هاتف: " + contact;
                e.Graphics.DrawString(contactLine, organizationFont, Brushes.DimGray, contactBounds, rtlFormat);
                e.Graphics.DrawString("ملف الطالب", title, Brushes.Black,

                    new Rectangle(area.Left, area.Top + 96, area.Width, 28), centerFormat);

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
                    new[] { "رقم الطالب", Safe(student.StudentNumber) },
                    new[] { "اسم الطالب", Safe(student.FullName) },
                    new[] { "الجنس", Safe(student.Gender) },
                    new[] { "الصف والشعبة", Safe(student.CurrentClassName) },
                    new[] { "الحالة", Safe(student.Status) },
                    new[] { "هاتف ولي الأمر", Safe(student.GuardianPhone) },
                    new[] { "الرقم الوطني", Safe(student.NationalId) }

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
                e.Graphics.DrawString("تاريخ الإصدار: " + DateTime.Now.ToString("yyyy-MM-dd"), footer,

                    Brushes.DimGray, new Rectangle(area.Left, area.Bottom - 28, area.Width, 22), footerFormat);
            }
            e.HasMorePages = false;
        }

        // تعريف الدالة CreateProfileFormat لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة CloseProfile لتنفيذ العملية المرتبطة بها.
        private void CloseProfile()
        {
            if (MainForm.Instance != null)
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                MainForm.Instance.LoadFormInPanel("Students", "ليس لديك صلاحية عرض الطلاب.", new StudentsForm(), PermissionKeys.StudentsView, PermissionKeys.StudentsManage);
        }
    }
}
