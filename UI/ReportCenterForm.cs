using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SchoolSystem.Models;
using SchoolSystem.Helpers;
using SchoolSystem.Services;
using SchoolSystem.Security;

namespace SchoolSystem.UI
{
    public partial class ReportCenterForm : UserControl
    {
        private readonly ReportService reportService = new ReportService();
        private readonly ClassService classService = new ClassService();

        private DataTable currentReportData;
        private int printRowIndex = 0;

        public ReportCenterForm()
        {
            InitializeComponent();
            SchoolSystem.Helpers.UIHelper.ApplyStyle(this);
            ApplyCustomStyles();
            Dock = DockStyle.Fill;
            WireUpEvents();
        }

        private void ApplyCustomStyles()
        {
            UIHelper.StyleDataGridView(dataGridViewReport);
            UIHelper.StylePrimaryButton(btnLoad);
            UIHelper.StyleButton(btnRefresh, UIHelper.NeutralColor);
            UIHelper.StyleButton(btnExportExcel, UIHelper.SuccessColor);
            UIHelper.StyleButton(btnExportCsv, UIHelper.SuccessColor);
            UIHelper.StyleButton(btnExportPDF, UIHelper.DangerColor);
            UIHelper.StyleButton(btnPrint, UIHelper.PrimaryColor);
            UIHelper.StyleComboBox(cmbReportType);
            UIHelper.StyleComboBox(cmbClass);
            UIHelper.StyleComboBox(cmbSection);
            UIHelper.StyleComboBox(cmbStatus);
            UIHelper.StyleTextBox(txtAcademicYear);
            UIHelper.StyleTextBox(txtSearch);
            lblSummary.ForeColor = UIHelper.MutedTextColor;
            lblRecordCount.ForeColor = UIHelper.MutedTextColor;
        }

        private void WireUpEvents()
        {
            Load -= ReportCenterForm_Load;
            Load += ReportCenterForm_Load;

            btnLoad.Click -= btnLoad_Click;
            btnLoad.Click += btnLoad_Click;

            btnRefresh.Click -= btnRefresh_Click;
            btnRefresh.Click += btnRefresh_Click;

            cmbClass.SelectedIndexChanged -= cmbClass_SelectedIndexChanged;
            cmbClass.SelectedIndexChanged += cmbClass_SelectedIndexChanged;

            txtAcademicYear.Leave -= txtAcademicYear_Leave;
            txtAcademicYear.Leave += txtAcademicYear_Leave;

            txtSearch.TextChanged -= txtSearch_TextChanged;
            txtSearch.TextChanged += txtSearch_TextChanged;

            btnExportExcel.Click -= btnExportExcel_Click;
            btnExportExcel.Click += btnExportExcel_Click;

            btnExportPDF.Click -= btnExportPDF_Click;
            btnExportPDF.Click += btnExportPDF_Click;

            btnPrint.Click -= btnPrint_Click;
            btnPrint.Click += btnPrint_Click;

            btnExportCsv.Click -= btnExportCsv_Click;
            btnExportCsv.Click += btnExportCsv_Click;

            printDocument.PrintPage -= PrintDocument_PrintPage;
            printDocument.PrintPage += PrintDocument_PrintPage;

            printPreviewDialog.Document = printDocument;
        }

        private async void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (!IsHandleCreated || cmbReportType.SelectedIndex < 0)
                return;

            await LoadReportAsync();
        }

        private async void ReportCenterForm_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                LoadStaticData();
                await LoadClassesAsync();
                await LoadSectionsAsync();

                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                UIHelper.ShowException("تحميل مركز التقارير", ex);
            }
        }

        private void LoadStaticData()
        {
            cmbReportType.Items.Clear();
            cmbReportType.Items.Add("تقرير الطلاب");
            cmbReportType.Items.Add("تقرير المعلمين");
            cmbReportType.Items.Add("تقرير القبول والتسجيل");
            cmbReportType.Items.Add("تقرير توزيع الفصول");
            cmbReportType.Items.Add("تقرير حضور المعلمين");
            cmbReportType.Items.Add("تقرير العقود والرواتب");
            cmbReportType.Items.Add("تقرير المستخدمين والصلاحيات");
            cmbReportType.Items.Add("تقرير الرسوم");
            cmbReportType.Items.Add("تقرير الدرجات");
            cmbReportType.Items.Add("تقرير الحركة المالية");

            if (cmbReportType.Items.Count > 0)
                cmbReportType.SelectedIndex = 0;

            cmbSection.DataSource = null;
            cmbSection.Items.Clear();
            cmbSection.Enabled = false;

            cmbStatus.Items.Clear();
            cmbStatus.Items.Add("الكل");
            cmbStatus.Items.Add("منتظم");
            cmbStatus.Items.Add("منقول");
            cmbStatus.Items.Add("موقوف");
            cmbStatus.Items.Add("متخرج");
            cmbStatus.Items.Add("تحت المراجعة");
            cmbStatus.Items.Add("مقبول");
            cmbStatus.Items.Add("مرفوض");
            cmbStatus.Items.Add("مؤجل");
            cmbStatus.Items.Add("حاضر");
            cmbStatus.Items.Add("غائب");
            cmbStatus.Items.Add("متأخر");
            cmbStatus.Items.Add("ساري");
            cmbStatus.Items.Add("منتهي");
            cmbStatus.Items.Add("نشط");
            cmbStatus.Items.Add("غير نشط");
            cmbStatus.Items.Add("قبض");
            cmbStatus.Items.Add("صرف");

            if (cmbStatus.Items.Count > 0)
                cmbStatus.SelectedIndex = 0;

            int year = DateTime.Now.Year;
            txtAcademicYear.Text = year + "/" + (year + 1);

            dtpFromDate.Value = new DateTime(DateTime.Now.Year, 1, 1);
            dtpToDate.Value = DateTime.Today;

            lblSummary.Text = "ملخص التقرير: لا توجد بيانات محملة.";
            lblRecordCount.Text = "عدد السجلات: 0";
        }

        private async void cmbClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            await LoadSectionsAsync();
        }

        private async void txtAcademicYear_Leave(object sender, EventArgs e)
        {
            await LoadSectionsAsync();
        }

        private async Task LoadSectionsAsync()
        {
            cmbSection.DataSource = null;
            cmbSection.Items.Clear();
            cmbSection.Enabled = false;

            int classId = 0;
            if (cmbClass.SelectedValue != null && !(cmbClass.SelectedValue is DataRowView))
                int.TryParse(cmbClass.SelectedValue.ToString(), out classId);

            if (!IsValidAcademicYear(txtAcademicYear.Text))
            {
                cmbSection.DataSource = null;
                cmbSection.Items.Clear();
                cmbSection.Enabled = false;
                return;
            }

            try
            {
                DataTable sections = await Task.Run(() => reportService.GetSections(classId, txtAcademicYear.Text.Trim()));
                if (sections == null)
                    sections = new DataTable();

                DataTable choices = new DataTable();
                choices.Columns.Add("Section", typeof(string));
                var sectionNames = new System.Collections.Generic.HashSet<string>(StringComparer.OrdinalIgnoreCase);

                if (sections != null && sections.Columns.Contains("Section"))
                {
                    foreach (DataRow row in sections.Rows)
                    {
                        string sectionName = row["Section"] == DBNull.Value
                            ? string.Empty
                            : Convert.ToString(row["Section"]);
                        sectionName = (sectionName ?? string.Empty).Trim();
                        if (!string.IsNullOrWhiteSpace(sectionName) && sectionNames.Add(sectionName))
                            choices.Rows.Add(sectionName);
                    }
                }

                if (choices.Rows.Count == 0)
                {
                    choices.Rows.Add("لا توجد شعب متاحة");
                    cmbSection.DataSource = choices;
                    cmbSection.DisplayMember = "Section";
                    cmbSection.ValueMember = "Section";
                    cmbSection.SelectedIndex = 0;
                    cmbSection.Enabled = false;
                    return;
                }

                DataRow allRow = choices.NewRow();
                allRow["Section"] = string.Empty;
                choices.Rows.InsertAt(allRow, 0);

                cmbSection.DataSource = choices;
                cmbSection.DisplayMember = "Section";
                cmbSection.ValueMember = "Section";
                cmbSection.Enabled = true;
                cmbSection.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                cmbSection.DataSource = null;
                cmbSection.Items.Clear();
                cmbSection.Enabled = false;
                UIHelper.ShowException("تحميل شعب مركز التقارير", ex);
            }
        }

        private async Task LoadClassesAsync()
        {
            try
            {
                DataTable classes = await Task.Run(() => classService.GetAllClasses());

                DataTable dt = new DataTable();
                dt.Columns.Add("ClassID", typeof(int));
                dt.Columns.Add("ClassName", typeof(string));

                dt.Rows.Add(0, "الكل");

                foreach (DataRow row in classes.Rows)
                {
                    int classId = Convert.ToInt32(row["ClassID"]);
                    string className = row["ClassName"].ToString();

                    dt.Rows.Add(classId, className);
                }

                cmbClass.DataSource = dt;
                cmbClass.DisplayMember = "ClassName";
                cmbClass.ValueMember = "ClassID";

                if (cmbClass.Items.Count > 0)
                    cmbClass.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                cmbClass.DataSource = null;
                cmbClass.Items.Clear();
                cmbClass.Items.Add("تعذر تحميل الصفوف");
                cmbClass.SelectedIndex = 0;
                cmbClass.Enabled = false;
                UIHelper.ShowException("تحميل صفوف مركز التقارير", ex);
            }
        }

        private bool IsValidAcademicYear(string academicYear)
        {
            if (string.IsNullOrWhiteSpace(academicYear))
                return false;

            string normalized = academicYear.Trim().Replace('-', '/');
            string[] parts = normalized.Split('/');
            int firstYear;
            int secondYear;
            if (parts.Length != 2 || parts[0].Length != 4 || parts[1].Length != 4)
                return false;
            if (!int.TryParse(parts[0], out firstYear) || !int.TryParse(parts[1], out secondYear))
                return false;

            return secondYear == firstYear + 1;
        }

        private bool ValidateReportFilters()
        {
            if (cmbReportType.SelectedIndex < 0 || string.IsNullOrWhiteSpace(cmbReportType.Text))
            {
                ShowWarning("يرجى اختيار نوع التقرير.");
                cmbReportType.Focus();
                return false;
            }
            if (cmbReportType.Text != "تقرير الحركة المالية" && !IsValidAcademicYear(txtAcademicYear.Text))
            {
                ShowWarning("أدخل العام الدراسي بالصيغة الصحيحة: 2025/2026 أو 1447-1448.");
                txtAcademicYear.Focus();
                return false;
            }
            if (dtpFromDate.Value.Date > dtpToDate.Value.Date)
            {
                ShowWarning("تاريخ البداية يجب أن يكون قبل تاريخ النهاية أو مساويًا له.");
                dtpFromDate.Focus();
                return false;
            }
            return true;
        }

        private ReportRequest BuildRequest()
        {
            ReportRequest request = new ReportRequest();

            request.ReportType = cmbReportType.Text;
            request.AcademicYear = txtAcademicYear.Text.Trim();
            request.Section = cmbSection.Text.Trim();
            if (request.Section == "لا توجد شعب متاحة")
                request.Section = string.Empty;
            request.Status = cmbStatus.Text.Trim();
            request.FromDate = dtpFromDate.Value.Date;
            request.ToDate = dtpToDate.Value.Date;
            request.SearchText = txtSearch.Text.Trim();

            if (cmbClass.SelectedValue != null)
            {
                int classId;

                if (int.TryParse(cmbClass.SelectedValue.ToString(), out classId) && classId > 0)
                    request.ClassID = classId;
            }

            return request;
        }

        private async void btnLoad_Click(object sender, EventArgs e)
        {
            await LoadReportAsync();
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await LoadReportAsync();
        }

        private async Task LoadReportAsync()
        {
            try
            {
                if (!EnsureReportAction("View", "ليس لديك صلاحية عرض التقارير."))
                    return;

                if (!ValidateReportFilters())
                    return;

                Cursor = Cursors.WaitCursor;

                ReportRequest request = BuildRequest();

                currentReportData = await Task.Run(() => reportService.GetReportData(request));

                dataGridViewReport.DataSource = currentReportData;

                FormatReportGrid();

                lblRecordCount.Text = "عدد السجلات: " + (currentReportData != null ? currentReportData.Rows.Count : 0);

                BuildSummary(currentReportData);

                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                UIHelper.ShowException("تحميل التقرير", ex);
            }
        }

        private void FormatReportGrid()
        {
            if (dataGridViewReport.Columns.Count == 0)
                return;

            UIHelper.StyleDataGridView(dataGridViewReport);
            dataGridViewReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewReport.MultiSelect = false;
            dataGridViewReport.ReadOnly = true;
            dataGridViewReport.AllowUserToAddRows = false;
            dataGridViewReport.AllowUserToDeleteRows = false;
            dataGridViewReport.RowHeadersVisible = false;

            foreach (DataGridViewColumn column in dataGridViewReport.Columns)
            {
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }

        private void BuildSummary(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                lblSummary.Text = "ملخص التقرير: لا توجد بيانات.";
                return;
            }

            decimal registrationFees = SumColumnIfExists(dt, "رسوم التسجيل");
            decimal total = SumColumnIfExists(dt, "الإجمالي");
            decimal net = SumColumnIfExists(dt, "الصافي");
            decimal paid = SumColumnIfExists(dt, "المدفوع");
            decimal remaining = SumColumnIfExists(dt, "المتبقي");
            decimal receipts = SumColumnIfExists(dt, "القبض");
            decimal payments = SumColumnIfExists(dt, "الصرف");

            string summary = "ملخص التقرير: عدد السجلات " + dt.Rows.Count;

            if (registrationFees > 0)
                summary += " | رسوم التسجيل: " + registrationFees.ToString("N2");

            if (total > 0)
                summary += " | الإجمالي: " + total.ToString("N2");

            if (net > 0)
                summary += " | الصافي: " + net.ToString("N2");

            if (paid > 0)
                summary += " | المدفوع: " + paid.ToString("N2");

            if (remaining > 0)
                summary += " | المتبقي: " + remaining.ToString("N2");

            if (receipts != 0)
                summary += " | إجمالي القبض: " + receipts.ToString("N2");

            if (payments != 0)
                summary += " | إجمالي الصرف: " + payments.ToString("N2");

            if (dt.Columns.Contains("القبض") || dt.Columns.Contains("الصرف"))
                summary += " | صافي الحركة: " + net.ToString("N2");

            lblSummary.Text = summary;
        }

        private decimal SumColumnIfExists(DataTable dt, string columnName)
        {
            if (dt == null || !dt.Columns.Contains(columnName))
                return 0;

            decimal sum = 0;

            foreach (DataRow row in dt.Rows)
            {
                if (row[columnName] == DBNull.Value)
                    continue;

                decimal value;

                if (decimal.TryParse(row[columnName].ToString(), out value))
                    sum += value;
            }

            return sum;
        }

        private DataTable GetCurrentDataTable()
        {
            if (dataGridViewReport.DataSource is DataTable table)
                return table;

            if (dataGridViewReport.DataSource is DataView view)
                return view.ToTable();

            return currentReportData;
        }

        private bool EnsureReportAction(string action, string message)
        {
            try
            {
                CurrentUser.DemandAction("Reports", action, message);
                return true;
            }
            catch (UnauthorizedAccessException ex)
            {
                ShowWarning(ex.Message);
                return false;
            }
        }

        private bool EnsureData()
        {
            DataTable dt = GetCurrentDataTable();

            if (dt == null || dt.Rows.Count == 0)
            {
                ShowWarning("لا توجد بيانات للتصدير أو الطباعة.");
                return false;
            }

            return true;
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            if (!EnsureReportAction("ExportExcel", "ليس لديك صلاحية تصدير التقارير إلى Excel."))
                return;

            if (!EnsureData())
                return;

            DataTable dt = GetCurrentDataTable();

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "ملفات Excel (*.xlsx)|*.xlsx";
                sfd.FileName = "تقرير_" + SafeFileName(cmbReportType.Text) + "_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".xlsx";

                if (sfd.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    Cursor = Cursors.WaitCursor;

                    ReportOutputHelper.ExportToExcel(
                        dt,
                        sfd.FileName,
                        "نظام إدارة المدرسة | School Management System - " + cmbReportType.Text,
                        lblSummary.Text);

                    Cursor = Cursors.Default;

                    ShowInfo("تم تصدير التقرير إلى Excel بنجاح.");
                }
                catch (Exception ex)
                {
                    Cursor = Cursors.Default;
                    UIHelper.ShowException("تصدير تقرير Excel", ex);
                }
            }
        }


        private void btnExportCsv_Click(object sender, EventArgs e)
        {
            if (!EnsureReportAction("ExportExcel", "ليس لديك صلاحية تصدير بيانات التقارير."))
                return;

            if (!EnsureData())
                return;

            DataTable dt = GetCurrentDataTable();

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "ملف CSV (*.csv)|*.csv";
                sfd.FileName = "تقرير_" + SafeFileName(cmbReportType.Text) + "_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".csv";

                if (sfd.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    ExportToCsv(dt, sfd.FileName);
                    ShowInfo("تم تصدير التقرير إلى CSV بنجاح.");
                }
                catch (Exception ex)
                {
                    UIHelper.ShowException("تصدير تقرير CSV", ex);
                }
            }
        }

        private void ExportToCsv(DataTable dt, string filePath)
        {
            using (StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (i > 0)
                        sw.Write(",");

                    sw.Write(EscapeCsv(dt.Columns[i].ColumnName));
                }

                sw.WriteLine();

                foreach (DataRow row in dt.Rows)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (i > 0)
                            sw.Write(",");

                        sw.Write(EscapeCsv(row[i] == DBNull.Value ? "" : row[i].ToString()));
                    }

                    sw.WriteLine();
                }
            }
        }

        private string EscapeCsv(string value)
        {
            if (value == null)
                return "";

            value = value.Replace("\"", "\"\"");
            return "\"" + value + "\"";
        }

        private void btnExportPDF_Click(object sender, EventArgs e)
        {
            if (!EnsureReportAction("ExportPDF", "ليس لديك صلاحية تصدير التقارير إلى PDF."))
                return;

            if (!EnsureData())
                return;

            DataTable dt = GetCurrentDataTable();

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "ملفات PDF (*.pdf)|*.pdf";
                sfd.FileName = "تقرير_" + SafeFileName(cmbReportType.Text) + "_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".pdf";

                if (sfd.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    Cursor = Cursors.WaitCursor;

                    ReportOutputHelper.ExportToPdf(
                        dt,
                        sfd.FileName,
                        "نظام إدارة المدرسة | School Management System - " + cmbReportType.Text,
                        lblSummary.Text);

                    Cursor = Cursors.Default;

                    ShowInfo("تم تصدير التقرير إلى PDF بنجاح.");
                }
                catch (Exception ex)
                {
                    Cursor = Cursors.Default;
                    UIHelper.ShowException("تصدير تقرير PDF", ex);
                }
            }
        }


        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (!EnsureReportAction("Print", "ليس لديك صلاحية طباعة التقارير."))
                return;

            if (!EnsureData())
                return;

            printRowIndex = 0;
            printPreviewDialog.Width = 1000;
            printPreviewDialog.Height = 700;
            printPreviewDialog.ShowDialog();
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            DataTable dt = GetCurrentDataTable();

            if (dt == null)
            {
                e.HasMorePages = false;
                return;
            }

            using (System.Drawing.Font titleFont = new System.Drawing.Font("Tahoma", 14, FontStyle.Bold))
            using (System.Drawing.Font headerFont = new System.Drawing.Font("Tahoma", 8, FontStyle.Bold))
            using (System.Drawing.Font cellFont = new System.Drawing.Font("Tahoma", 7))
            using (System.Drawing.Font infoFont = new System.Drawing.Font("Tahoma", 9))
            using (StringFormat rtlFormat = CreatePrintFormat(true))
            using (StringFormat ltrFormat = CreatePrintFormat(false))
            using (SolidBrush headerBrush = new SolidBrush(Color.FromArgb(31, 41, 55)))
            {
                int x = e.MarginBounds.Left;
                int y = e.MarginBounds.Top;
                int pageWidth = e.MarginBounds.Width;
                int rowHeight = 24;

                e.Graphics.DrawString("نظام إدارة المدرسة", titleFont, Brushes.Black,
                    new RectangleF(x, y, pageWidth, 25), rtlFormat);
                y += 30;

                e.Graphics.DrawString("تقرير: " + cmbReportType.Text, infoFont, Brushes.Black,
                    new RectangleF(x, y, pageWidth, 22), SelectPrintFormat(cmbReportType.Text, rtlFormat, ltrFormat));
                y += 24;

                e.Graphics.DrawString("التاريخ: " + DateTime.Now.ToString("yyyy/MM/dd HH:mm"), infoFont, Brushes.Black,
                    new RectangleF(x, y, pageWidth, 22), ltrFormat);
                y += 28;

                int colCount = dt.Columns.Count;
                int colWidth = Math.Max(70, pageWidth / Math.Max(1, colCount));

                for (int i = 0; i < colCount; i++)
                {
                    System.Drawing.Rectangle rect = new System.Drawing.Rectangle(x + i * colWidth, y, colWidth, rowHeight);
                    e.Graphics.FillRectangle(headerBrush, rect);
                    e.Graphics.DrawRectangle(Pens.Black, rect);
                    e.Graphics.DrawString(dt.Columns[i].ColumnName, headerFont, Brushes.White, rect,
                        SelectPrintFormat(dt.Columns[i].ColumnName, rtlFormat, ltrFormat));
                }

                y += rowHeight;

                while (printRowIndex < dt.Rows.Count)
                {
                    if (y + rowHeight > e.MarginBounds.Bottom)
                    {
                        e.HasMorePages = true;
                        return;
                    }

                    DataRow row = dt.Rows[printRowIndex];
                    for (int i = 0; i < colCount; i++)
                    {
                        System.Drawing.Rectangle rect = new System.Drawing.Rectangle(x + i * colWidth, y, colWidth, rowHeight);
                        e.Graphics.DrawRectangle(Pens.Gray, rect);
                        string cellText = row[i] == DBNull.Value ? "" : row[i].ToString();
                        e.Graphics.DrawString(cellText, cellFont, Brushes.Black, rect,
                            SelectPrintFormat(cellText, rtlFormat, ltrFormat));
                    }

                    y += rowHeight;
                    printRowIndex++;
                }

                e.HasMorePages = false;
            }
        }

        private static StringFormat CreatePrintFormat(bool rtl)
        {
            return new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center,
                FormatFlags = rtl ? StringFormatFlags.DirectionRightToLeft : StringFormatFlags.FitBlackBox,
                Trimming = StringTrimming.EllipsisCharacter
            };
        }

        private static StringFormat SelectPrintFormat(string text, StringFormat rtlFormat, StringFormat ltrFormat)
        {
            return ReportOutputHelper.ContainsArabic(text) ? rtlFormat : ltrFormat;
        }

        private string SafeFileName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "Report";

            foreach (char c in Path.GetInvalidFileNameChars())
                value = value.Replace(c, '_');

            return value.Replace(" ", "_");
        }

        private void ShowInfo(string message)
        {
            MessageBox.Show(
                message,
                "معلومة",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
        }

        private void ShowWarning(string message)
        {
            MessageBox.Show(
                message,
                "تنبيه",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
        }

        private void ShowError(string message)
        {
            MessageBox.Show(
                message,
                "خطأ",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
        }
    }
}
