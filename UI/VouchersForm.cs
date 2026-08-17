using System;
using System.Data;
using System.Globalization;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SchoolSystem.Models;
using SchoolSystem.Helpers;
using SchoolSystem.Services;

namespace SchoolSystem.UI
{
    public partial class VouchersForm : UserControl
    {
        private readonly VoucherService voucherService = new VoucherService();
        private readonly PartyService partyService = new PartyService();
        private DataTable voucherParties;

        private int selectedVoucherId = 0;
        private DataTable allVouchers;
        private bool isLoading = false;
        private Button btnNewReceipt;
        private Button btnNewPayment;
        private Button btnPreview;
        private Button btnPrint;
        private Button btnExportExcel;
        private FlowLayoutPanel actionLayout;
        private DateTimePicker dtpFilterFrom;
        private DateTimePicker dtpFilterTo;
        private Label lblFilterFrom;
        private Label lblFilterTo;
        private Label lblTotalReceipts;
        private Label lblTotalPayments;
        private Label lblNetBalance;
        private readonly PrintDocument voucherPrintDocument = new PrintDocument();
        private Voucher voucherToPrint;

        public VouchersForm()
        {
            InitializeComponent();
            SchoolSystem.Helpers.UIHelper.ApplyStyle(this);
            txtSearch.TextChanged += (sender, e) => ApplyFilter();
            cmbVoucherType.SelectedIndexChanged += cmbVoucherType_SelectedIndexChanged;
            Dock = DockStyle.Fill;
            ConfigureFinancialActions();
            ConfigureMovementSummary();
            panelSearch.Resize += panelSearch_Resize;
            voucherPrintDocument.PrintPage += VoucherPrintDocument_PrintPage;
            Load += VouchersForm_Load;
        }

        private void ConfigureFinancialActions()
        {
            btnNewReceipt = CreateActionButton("سند قبض جديد", Color.FromArgb(22, 160, 133));
            btnNewPayment = CreateActionButton("سند صرف جديد", Color.FromArgb(142, 68, 173));
            btnPreview = CreateActionButton("معاينة", Color.FromArgb(52, 152, 219));
            btnPrint = CreateActionButton("طباعة", Color.FromArgb(41, 128, 185));
            btnExportExcel = CreateActionButton("تصدير Excel", Color.FromArgb(39, 174, 96));

            btnNewReceipt.Click += (s, e) => PrepareNewVoucher("قبض");
            btnNewPayment.Click += (s, e) => PrepareNewVoucher("صرف");
            btnPreview.Click += (s, e) => PreviewSelectedVoucher();
            btnPrint.Click += (s, e) => PrintSelectedVoucher();
            btnExportExcel.Click += (s, e) => ExportVisibleVouchersExcel();

            // شريط مرن يدعم RTL وتغيير حجم النافذة بدل الاعتماد على إحداثيات ثابتة.
            actionLayout = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false,
                AutoScroll = true,
                Padding = new Padding(8),
                Margin = Padding.Empty,
                RightToLeft = RightToLeft.Yes,
                BackColor = panelButtons.BackColor
            };

            panelButtons.Controls.Clear();
            panelButtons.Padding = Padding.Empty;
            panelButtons.Controls.Add(actionLayout);

            // ترتيب الأزرار من اليمين إلى اليسار: إضافة، تعديل، حذف، مسح، ثم العمليات المالية.
            actionLayout.Controls.Add(btnAdd);
            actionLayout.Controls.Add(btnUpdate);
            actionLayout.Controls.Add(btnDelete);
            actionLayout.Controls.Add(btnClear);
            actionLayout.Controls.Add(btnNewReceipt);
            actionLayout.Controls.Add(btnNewPayment);
            actionLayout.Controls.Add(btnPreview);
            actionLayout.Controls.Add(btnPrint);
            actionLayout.Controls.Add(btnExportExcel);

            foreach (Control control in actionLayout.Controls)
            {
                Button actionButton = control as Button;
                if (actionButton != null)
                {
                    actionButton.AutoSize = false;
                    actionButton.Size = new Size(115, 35);
                    actionButton.Margin = new Padding(4, 2, 4, 2);
                    UIHelper.StyleActionButton(actionButton);
                }
            }
        }

        private void ConfigureMovementSummary()
        {
            panelSearch.Height = 88;
            panelSearch.Padding = new Padding(12, 6, 12, 4);
            panelSearch.AutoScroll = true;

            lblFilterFrom = CreateSummaryLabel("من:");
            lblFilterTo = CreateSummaryLabel("إلى:");
            dtpFilterFrom = CreateFilterDatePicker();
            dtpFilterTo = CreateFilterDatePicker();

            panelSearch.Controls.Add(lblFilterFrom);
            panelSearch.Controls.Add(dtpFilterFrom);
            panelSearch.Controls.Add(lblFilterTo);
            panelSearch.Controls.Add(dtpFilterTo);

            PositionMovementFilters();

            lblTotalReceipts = CreateMovementLabel(Color.FromArgb(22, 160, 133));
            lblTotalPayments = CreateMovementLabel(Color.FromArgb(142, 68, 173));
            lblNetBalance = CreateMovementLabel(Color.FromArgb(41, 128, 185));

            panelSearch.Controls.Add(lblTotalReceipts);
            panelSearch.Controls.Add(lblTotalPayments);
            panelSearch.Controls.Add(lblNetBalance);

            PositionMovementSummary();

            dtpFilterFrom.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            dtpFilterTo.Value = DateTime.Today;
            dtpFilterFrom.ValueChanged += FilterControls_Changed;
            dtpFilterTo.ValueChanged += FilterControls_Changed;
        }

        private void panelSearch_Resize(object sender, EventArgs e)
        {
            PositionMovementFilters();
            PositionMovementSummary();
        }

        private void PositionMovementFilters()
        {
            if (panelSearch == null || lblFilterFrom == null || dtpFilterFrom == null ||
                lblFilterTo == null || dtpFilterTo == null)
                return;

            // Keep the designer-independent coordinates within the visible client area.
            // AutoScroll remains enabled as a safe fallback for very narrow windows.
            int right = Math.Max(12, panelSearch.ClientSize.Width - 12);
            int dateWidth = 140;
            int labelWidth = 42;
            int gap = 8;
            int rightDateX = Math.Max(10, right - dateWidth);
            int rightLabelX = Math.Max(10, rightDateX - gap - labelWidth);
            int leftDateX = Math.Max(10, rightLabelX - gap - dateWidth);
            int leftLabelX = Math.Max(10, leftDateX - gap - labelWidth);

            lblFilterFrom.SetBounds(rightLabelX, 8, labelWidth, 26);
            dtpFilterFrom.SetBounds(rightDateX, 8, dateWidth, 26);
            lblFilterTo.SetBounds(leftLabelX, 8, labelWidth, 26);
            dtpFilterTo.SetBounds(leftDateX, 8, dateWidth, 26);
        }

        private void PositionMovementSummary()
        {
            if (panelSearch == null || lblTotalReceipts == null || lblTotalPayments == null || lblNetBalance == null)
                return;

            int width = 215;
            int gap = 10;
            int right = Math.Max(12, panelSearch.ClientSize.Width - 12);
            int x1 = Math.Max(10, right - width);
            int x2 = Math.Max(10, x1 - gap - width);
            int x3 = Math.Max(10, x2 - gap - width);

            lblTotalReceipts.SetBounds(x1, 51, width, 28);
            lblTotalPayments.SetBounds(x2, 51, width, 28);
            lblNetBalance.SetBounds(x3, 51, width, 28);
        }

        private Label CreateSummaryLabel(string text)
        {
            return new Label
            {
                Text = text,
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font(UIHelper.FontFamily, UIHelper.SectionFontSize, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 42, 57)
            };
        }

        private DateTimePicker CreateFilterDatePicker()
        {
            return new DateTimePicker
            {
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd/MM/yyyy",
                Font = new Font(UIHelper.FontFamily, UIHelper.BodyFontSize)
            };
        }

        private Label CreateMovementLabel(Color color)
        {
            return new Label
            {
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.White,
                ForeColor = color,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font(UIHelper.FontFamily, UIHelper.BodyFontSize, FontStyle.Bold),
                Padding = new Padding(4, 0, 4, 0)
            };
        }

        private Button CreateActionButton(string text, Color color)
        {
            Button button = new Button
            {
                Text = text,
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font(UIHelper.FontFamily, UIHelper.SectionFontSize, FontStyle.Bold),
                Size = new Size(115, 35),
                UseVisualStyleBackColor = false
            };
            button.FlatAppearance.BorderSize = 0;
            return button;
        }

        private void PrepareNewVoucher(string voucherType)
        {
            ClearInputs();
            cmbVoucherType.Text = voucherType;
            SetNextVoucherNumber();
            txtAmount.Focus();
        }

        private void cmbVoucherType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isLoading && selectedVoucherId == 0)
                SetNextVoucherNumber();
        }

        private void SetNextVoucherNumber()
        {
            try
            {
                string voucherType = cmbVoucherType == null ? string.Empty : cmbVoucherType.Text.Trim();
                if (selectedVoucherId == 0 && (voucherType == "قبض" || voucherType == "صرف"))
                    txtVoucherNumber.Text = voucherService.GenerateVoucherNumber(voucherType);
            }
            catch (Exception ex)
            {
                txtVoucherNumber.Clear();
                UIHelper.ShowException("توليد رقم السند", ex);
            }
        }

        private async Task LoadVoucherPartiesAsync()
        {
            voucherParties = await Task.Run(() => partyService.GetVoucherParties());
            cmbPartyName.DataSource = voucherParties;
            cmbPartyName.DisplayMember = "DisplayName";
            cmbPartyName.ValueMember = "PartyKey";
            cmbPartyName.SelectedIndex = -1;
            cmbPartyName.Text = string.Empty;
        }

        private bool TryGetSelectedVoucherForOutput(out Voucher voucher)
        {
            voucher = null;
            if (selectedVoucherId <= 0)
            {
                UIHelper.ShowWarning("اختر سنداً من الجدول أولاً للطباعة أو المعاينة.");
                return false;
            }

            voucher = GetVoucherFromInputs();
            if (voucher == null || voucher.VoucherID <= 0)
            {
                UIHelper.ShowWarning("بيانات السند المحدد غير مكتملة.");
                return false;
            }

            return true;
        }

        private void PreviewSelectedVoucher()
        {
            if (!TryGetSelectedVoucherForOutput(out voucherToPrint))
                return;

            try
            {
                using (PrintPreviewDialog preview = new PrintPreviewDialog())
                {
                    preview.Document = voucherPrintDocument;
                    preview.RightToLeft = RightToLeft.Yes;
                    preview.WindowState = FormWindowState.Maximized;
                    preview.ShowDialog(FindForm());
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("معاينة السند", ex);
            }
        }

        private void PrintSelectedVoucher()
        {
            if (!TryGetSelectedVoucherForOutput(out voucherToPrint))
                return;

            try
            {
                using (PrintDialog dialog = new PrintDialog())
                {
                    dialog.Document = voucherPrintDocument;
                    if (dialog.ShowDialog(FindForm()) == DialogResult.OK)
                        voucherPrintDocument.Print();
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("طباعة السند", ex);
            }
        }

        private void VoucherPrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (voucherToPrint == null)
            {
                e.HasMorePages = false;
                return;
            }

            Graphics graphics = e.Graphics;
            Rectangle bounds = e.MarginBounds;
            using (Font titleFont = new Font("Tahoma", 18, FontStyle.Bold))
            using (Font labelFont = new Font("Tahoma", 11, FontStyle.Bold))
            using (Font valueFont = new Font("Tahoma", 11))
            using (Pen linePen = new Pen(Color.FromArgb(33, 42, 57), 2))
            using (StringFormat rtl = new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center })
            {
                int y = bounds.Top;
                graphics.DrawString("سند " + voucherToPrint.VoucherType, titleFont, Brushes.Black,
                    new Rectangle(bounds.Left, y, bounds.Width, 45), rtl);
                y += 65;
                graphics.DrawLine(linePen, bounds.Left, y, bounds.Right, y);
                y += 20;

                DrawPrintField(graphics, bounds, ref y, "رقم السند", voucherToPrint.VoucherNumber, labelFont, valueFont, rtl);
                DrawPrintField(graphics, bounds, ref y, "التاريخ", voucherToPrint.VoucherDate.ToString("dd/MM/yyyy"), labelFont, valueFont, rtl);
                DrawPrintField(graphics, bounds, ref y, "الطرف", voucherToPrint.PartyName, labelFont, valueFont, rtl);
                DrawPrintField(graphics, bounds, ref y, "المبلغ", voucherToPrint.Amount.ToString("N2") + " ريال", labelFont, valueFont, rtl);
                DrawPrintField(graphics, bounds, ref y, "طريقة الدفع", voucherToPrint.PaymentMethod, labelFont, valueFont, rtl);
                DrawPrintField(graphics, bounds, ref y, "البيان", voucherToPrint.Description, labelFont, valueFont, rtl);
                DrawPrintField(graphics, bounds, ref y, "المرجع", voucherToPrint.ReferenceType +
                    (voucherToPrint.ReferenceID.HasValue ? " - " + voucherToPrint.ReferenceID.Value : ""), labelFont, valueFont, rtl);
                DrawPrintField(graphics, bounds, ref y, "ملاحظات", voucherToPrint.Notes, labelFont, valueFont, rtl);

                y += 30;
                graphics.DrawLine(linePen, bounds.Left, y, bounds.Right, y);
                y += 40;
                graphics.DrawString("المستلم: ____________________", valueFont, Brushes.Black, bounds.Left + 30, y);
                graphics.DrawString("المحاسب: ____________________", valueFont, Brushes.Black, bounds.Right - 250, y);
            }

            e.HasMorePages = false;
        }

        private void DrawPrintField(Graphics graphics, Rectangle bounds, ref int y, string label, string value, Font labelFont, Font valueFont, StringFormat rtl)
        {
            graphics.DrawString(label + ":", labelFont, Brushes.Black, new Rectangle(bounds.Left, y, 150, 30), rtl);
            graphics.DrawString(value ?? string.Empty, valueFont, Brushes.Black, new Rectangle(bounds.Left + 160, y, bounds.Width - 160, 30), rtl);
            y += 38;
        }

        private void ExportVisibleVouchersExcel()
        {
            if (dataGridViewVouchers.Rows.Count == 0)
            {
                UIHelper.ShowWarning("لا توجد سندات لتصديرها.");
                return;
            }

            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "ملفات Excel (*.xlsx)|*.xlsx";
                dialog.FileName = "Vouchers_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".xlsx";
                dialog.Title = "تصدير السندات";
                if (dialog.ShowDialog(FindForm()) != DialogResult.OK)
                    return;

                try
                {
                    DataTable exportTable = new DataTable();
                    foreach (DataGridViewColumn column in dataGridViewVouchers.Columns)
                        exportTable.Columns.Add(column.HeaderText ?? column.Name);
                    foreach (DataGridViewRow row in dataGridViewVouchers.Rows)
                    {
                        if (row.IsNewRow) continue;
                        DataRow exportRow = exportTable.NewRow();
                        for (int index = 0; index < dataGridViewVouchers.Columns.Count; index++)
                            exportRow[index] = row.Cells[index].Value == null || row.Cells[index].Value == DBNull.Value
                                ? string.Empty : row.Cells[index].Value.ToString();
                        exportTable.Rows.Add(exportRow);
                    }
                    ReportOutputHelper.ExportToExcel(
                        exportTable,
                        dialog.FileName,
                        "الحركة المالية | Financial Vouchers",
                        "عدد السجلات | Records: " + exportTable.Rows.Count);
                    UIHelper.ShowInfo("تم تصدير السندات إلى Excel بنجاح.");
                }
                catch (Exception ex)
                {
                    UIHelper.ShowException("تصدير السندات", ex);
                }
            }
        }


        private async void VouchersForm_Load(object sender, EventArgs e)
        {
            try
            {
                isLoading = true;

                InitializeLookups();
                await LoadVoucherPartiesAsync();

                dtpVoucherDate.Value = DateTime.Today;

                await LoadVouchersAsync();

                ClearInputs();
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("تحميل السندات", ex);
            }
            finally
            {
                isLoading = false;
            }
        }

        private void InitializeLookups()
        {
            cmbVoucherType.Items.Clear();
            cmbVoucherType.Items.AddRange(new object[] { "قبض", "صرف" });
            cmbVoucherType.SelectedIndex = 0;

            cmbPaymentMethod.Items.Clear();
            cmbPaymentMethod.Items.AddRange(new object[] { "نقداً", "حوالة", "شيك", "محفظة إلكترونية", "أخرى" });
            cmbPaymentMethod.SelectedIndex = 0;

            cmbReferenceType.Items.Clear();
            cmbReferenceType.Items.AddRange(new object[] { "عام", "رسوم", "مصروفات", "رواتب", "نقل", "مكتبة" });
            cmbReferenceType.SelectedIndex = 0;

            cmbFilterType.Items.Clear();
            cmbFilterType.Items.AddRange(new object[] { "كل السندات", "قبض", "صرف" });
            cmbFilterType.SelectedIndex = 0;

            cmbPartyName.DropDownStyle = ComboBoxStyle.DropDown;
            cmbPartyName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbPartyName.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmbPartyName.IntegralHeight = false;
        }

        private async Task LoadVouchersAsync()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                allVouchers = await Task.Run(() => voucherService.GetAllVouchers());

                ApplyFilter();
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void ApplyFilter()
        {
            if (allVouchers == null)
                return;

            DataView dv = allVouchers.DefaultView;

            string searchText = UIHelper.EscapeDataViewFilterValue(txtSearch.Text);
            string selectedType = cmbFilterType.SelectedItem == null
                ? "كل السندات"
                : cmbFilterType.SelectedItem.ToString();

            string filter = "";

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                filter =
                    "(VoucherNumber LIKE '%" + searchText + "%' " +
                    "OR VoucherType LIKE '%" + searchText + "%' " +
                    "OR PartyName LIKE '%" + searchText + "%' " +
                    "OR Description LIKE '%" + searchText + "%' " +
                    "OR PaymentMethod LIKE '%" + searchText + "%' " +
                    "OR ReferenceType LIKE '%" + searchText + "%' " +
                    "OR Notes LIKE '%" + searchText + "%')";
            }

            if (selectedType != "كل السندات")
            {
                if (!string.IsNullOrWhiteSpace(filter))
                    filter += " AND ";

                filter += "VoucherType = '" + UIHelper.EscapeDataViewFilterValue(selectedType) + "'";
            }

            dv.RowFilter = filter;

            DateTime fromDate = dtpFilterFrom == null ? DateTime.MinValue.Date : dtpFilterFrom.Value.Date;
            DateTime toDate = dtpFilterTo == null ? DateTime.MaxValue.Date : dtpFilterTo.Value.Date;
            DataTable filtered = allVouchers.Clone();
            decimal receipts = 0m;
            decimal payments = 0m;

            foreach (DataRowView rowView in dv)
            {
                DataRow row = rowView.Row;
                DateTime voucherDate = row["VoucherDate"] == DBNull.Value
                    ? DateTime.MinValue.Date
                    : Convert.ToDateTime(row["VoucherDate"]).Date;

                if (voucherDate < fromDate || voucherDate > toDate)
                    continue;

                filtered.ImportRow(row);
                decimal amount = row["Amount"] == DBNull.Value ? 0m : Convert.ToDecimal(row["Amount"]);
                string voucherType = row["VoucherType"] == DBNull.Value ? string.Empty : row["VoucherType"].ToString();
                if (string.Equals(voucherType, "قبض", StringComparison.OrdinalIgnoreCase))
                    receipts += amount;
                else if (string.Equals(voucherType, "صرف", StringComparison.OrdinalIgnoreCase))
                    payments += amount;
            }

            dataGridViewVouchers.DataSource = filtered;
            UpdateMovementSummary(filtered.Rows.Count, receipts, payments);
            FormatGrid();
        }

        private void UpdateMovementSummary(int recordCount, decimal receipts, decimal payments)
        {
            decimal net = receipts - payments;
            if (lblTotalReceipts != null)
                lblTotalReceipts.Text = "إجمالي القبض: " + receipts.ToString("N2") + " ريال";
            if (lblTotalPayments != null)
                lblTotalPayments.Text = "إجمالي الصرف: " + payments.ToString("N2") + " ريال";
            if (lblNetBalance != null)
            {
                lblNetBalance.Text = "الصافي: " + net.ToString("N2") + " ريال";
                lblNetBalance.ForeColor = net >= 0
                    ? Color.FromArgb(39, 174, 96)
                    : Color.FromArgb(192, 57, 43);
            }
            lblRecordCount.Text = "عدد السندات ضمن الفترة: " + recordCount;
        }

        private string EscapeFilter(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "";

            return value
                .Replace("'", "''")
                .Replace("[", "[[]")
                .Replace("%", "[%]")
                .Replace("*", "[*]");
        }

        private void FormatGrid()
        {
            if (dataGridViewVouchers.Columns.Count == 0)
                return;

            HideColumn("VoucherID");
            HideColumn("ReferenceID");

            SetHeader("VoucherNumber", "رقم السند");
            SetHeader("VoucherType", "النوع");
            SetHeader("Amount", "المبلغ");
            SetHeader("VoucherDate", "التاريخ");
            SetHeader("PartyName", "الطرف");
            SetHeader("Description", "البيان");
            SetHeader("PaymentMethod", "طريقة الدفع");
            SetHeader("ReferenceType", "المرجع");
            SetHeader("Notes", "ملاحظات");
            SetHeader("IsAutoGenerated", "تلقائي");
            SetHeader("CreatedAt", "تاريخ الإدخال");

            FormatMoney("Amount");
            FormatDate("VoucherDate");
            FormatDate("CreatedAt");

            dataGridViewVouchers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void HideColumn(string columnName)
        {
            if (dataGridViewVouchers.Columns.Contains(columnName))
                dataGridViewVouchers.Columns[columnName].Visible = false;
        }

        private void SetHeader(string columnName, string headerText)
        {
            if (dataGridViewVouchers.Columns.Contains(columnName))
                dataGridViewVouchers.Columns[columnName].HeaderText = headerText;
        }

        private void FormatMoney(string columnName)
        {
            if (dataGridViewVouchers.Columns.Contains(columnName))
                dataGridViewVouchers.Columns[columnName].DefaultCellStyle.Format = "N2";
        }

        private void FormatDate(string columnName)
        {
            if (dataGridViewVouchers.Columns.Contains(columnName))
                dataGridViewVouchers.Columns[columnName].DefaultCellStyle.Format = "dd/MM/yyyy";
        }

        private void FilterControls_Changed(object sender, EventArgs e)
        {
            if (dtpFilterFrom != null && dtpFilterTo != null && dtpFilterFrom.Value.Date > dtpFilterTo.Value.Date)
            {
                UIHelper.ShowWarning("تاريخ البداية يجب ألا يتجاوز تاريخ النهاية.");
                return;
            }

            if (!isLoading)
                ApplyFilter();
        }

        private void ClearInputs()
        {
            selectedVoucherId = 0;

            txtVoucherNumber.Clear();

            if (cmbVoucherType.Items.Count > 0)
                cmbVoucherType.SelectedIndex = 0;

            SetNextVoucherNumber();

            txtAmount.Text = "0";

            dtpVoucherDate.Value = DateTime.Today;

            cmbPartyName.SelectedIndex = -1;
            cmbPartyName.Text = string.Empty;
            txtDescription.Clear();

            if (cmbPaymentMethod.Items.Count > 0)
                cmbPaymentMethod.SelectedIndex = 0;

            if (cmbReferenceType.Items.Count > 0)
                cmbReferenceType.SelectedIndex = 0;

            txtReferenceID.Clear();

            chkIsAutoGenerated.Checked = false;
            chkIsAutoGenerated.Enabled = true;
            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;

            txtNotes.Clear();
        }

        private bool ValidateInputs()
        {
            string voucherNumber = txtVoucherNumber.Text == null ? string.Empty : txtVoucherNumber.Text.Trim();
            if (string.IsNullOrWhiteSpace(voucherNumber) || voucherNumber.Length > 50 ||
                voucherNumber.IndexOfAny(new[] { '\r', '\n', '\t' }) >= 0)
            {
                UIHelper.FocusAndWarn(txtVoucherNumber, "أدخل رقم سند صحيحاً بطول لا يتجاوز 50 حرفاً.");
                return false;
            }

            if (cmbVoucherType.SelectedItem == null || string.IsNullOrWhiteSpace(cmbVoucherType.Text))
            {
                UIHelper.ShowWarning("اختر نوع السند.");
                cmbVoucherType.Focus();
                return false;
            }

            decimal amount;
            if (!UIHelper.TryParseDecimal(txtAmount.Text, out amount))
            {
                UIHelper.ShowWarning("أدخل مبلغاً رقمياً صحيحاً.");
                txtAmount.Focus();
                return false;
            }

            if (amount <= 0 || amount > 100000000m)
            {
                UIHelper.ShowWarning("يجب أن يكون مبلغ السند أكبر من صفر ولا يتجاوز 100,000,000.");
                txtAmount.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(cmbReferenceType.Text))
            {
                UIHelper.ShowWarning("اختر نوع المرجع.");
                cmbReferenceType.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(cmbPartyName.Text) || cmbPartyName.Text.Trim().Length > 200)
            {
                UIHelper.FocusAndWarn(cmbPartyName, "اختر طرفًا من القائمة أو أدخل اسمًا بطول لا يتجاوز 200 حرف.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDescription.Text) || txtDescription.Text.Trim().Length > 500)
            {
                UIHelper.FocusAndWarn(txtDescription, "أدخل بيان السند بطول لا يتجاوز 500 حرف.");
                return false;
            }

            if (txtNotes.Text.Trim().Length > 1000)
            {
                UIHelper.FocusAndWarn(txtNotes, "الملاحظات يجب ألا تتجاوز 1000 حرف.");
                return false;
            }

            if (cmbPaymentMethod.SelectedItem == null || string.IsNullOrWhiteSpace(cmbPaymentMethod.Text))
            {
                UIHelper.ShowWarning("اختر طريقة الدفع.");
                cmbPaymentMethod.Focus();
                return false;
            }

            if (dtpVoucherDate.Value.Date > DateTime.Today)
            {
                UIHelper.ShowWarning("تاريخ السند لا يمكن أن يكون في المستقبل.");
                dtpVoucherDate.Focus();
                return false;
            }

            int? referenceId = ReadNullableInt(txtReferenceID.Text);
            if (!string.IsNullOrWhiteSpace(txtReferenceID.Text) && !referenceId.HasValue)
            {
                UIHelper.ShowWarning("رقم المرجع يجب أن يكون رقمًا صحيحًا موجبًا.");
                txtReferenceID.Focus();
                return false;
            }

            if (!string.Equals(cmbReferenceType.Text.Trim(), "عام", StringComparison.OrdinalIgnoreCase) && !referenceId.HasValue)
            {
                UIHelper.ShowWarning("أدخل رقم المرجع عند اختيار نوع مرجع مرتبط مثل الرسوم أو المصروفات.");
                txtReferenceID.Focus();
                return false;
            }

            if (string.Equals(cmbReferenceType.Text.Trim(), "عام", StringComparison.OrdinalIgnoreCase) && referenceId.HasValue)
            {
                UIHelper.ShowWarning("لا يمكن إدخال رقم مرجع مع النوع العام؛ اختر نوع المرجع الصحيح أولاً.");
                cmbReferenceType.Focus();
                return false;
            }

            if (chkIsAutoGenerated.Checked)
            {
                UIHelper.ShowWarning("السندات التلقائية ينشئها النظام من شاشة الرسوم أو المصروفات ولا تُضاف يدويًا.");
                chkIsAutoGenerated.Focus();
                return false;
            }

            return true;
        }

        private decimal ReadDecimal(string text)
        {
            return UIHelper.TryParseDecimal(text, out decimal value) ? value : 0m;
        }

        private int? ReadNullableInt(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return null;

            string normalized = text.Trim();
            if (int.TryParse(normalized, NumberStyles.Integer, CultureInfo.CurrentCulture, out int value) && value > 0)
                return value;

            if (int.TryParse(normalized, NumberStyles.Integer, CultureInfo.InvariantCulture, out value) && value > 0)
                return value;

            return null;
        }

        private Voucher GetVoucherFromInputs()
        {
            return new Voucher
            {
                VoucherID = selectedVoucherId,
                VoucherNumber = txtVoucherNumber.Text.Trim(),
                VoucherType = cmbVoucherType.Text.Trim(),

                Amount = ReadDecimal(txtAmount.Text),
                VoucherDate = dtpVoucherDate.Value.Date,

                PartyName = cmbPartyName.Text.Trim(),
                Description = txtDescription.Text.Trim(),

                PaymentMethod = cmbPaymentMethod.Text.Trim(),

                ReferenceType = cmbReferenceType.Text.Trim(),
                ReferenceID = ReadNullableInt(txtReferenceID.Text),

                Notes = txtNotes.Text.Trim(),

                IsAutoGenerated = chkIsAutoGenerated.Checked
            };
        }

        private void dataGridViewVouchers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dataGridViewVouchers.Rows.Count == 0)
                return;

            DataRowView rowView = dataGridViewVouchers.Rows[e.RowIndex].DataBoundItem as DataRowView;

            if (rowView != null)
                FillFieldsFromRow(rowView.Row);
        }

        private void FillFieldsFromRow(DataRow row)
        {
            if (row == null)
                return;

            selectedVoucherId = row["VoucherID"] == DBNull.Value || !int.TryParse(row["VoucherID"].ToString(), out int voucherId)
                ? 0
                : voucherId;

            txtVoucherNumber.Text = row["VoucherNumber"] == DBNull.Value ? "" : row["VoucherNumber"].ToString();
            cmbVoucherType.Text = row["VoucherType"] == DBNull.Value ? "" : row["VoucherType"].ToString();

            decimal amount = row["Amount"] == DBNull.Value ? 0m : ReadDecimal(row["Amount"].ToString());
            txtAmount.Text = amount.ToString("N2");

            if (row["VoucherDate"] != DBNull.Value && DateTime.TryParse(row["VoucherDate"].ToString(), out DateTime voucherDate))
                dtpVoucherDate.Value = voucherDate <= DateTime.Today ? voucherDate : DateTime.Today;

            cmbPartyName.SelectedIndex = -1;
            cmbPartyName.Text = row["PartyName"] == DBNull.Value ? "" : row["PartyName"].ToString();
            txtDescription.Text = row["Description"] == DBNull.Value ? "" : row["Description"].ToString();

            cmbPaymentMethod.Text = row["PaymentMethod"] == DBNull.Value ? "" : row["PaymentMethod"].ToString();
            cmbReferenceType.Text = row["ReferenceType"] == DBNull.Value ? "عام" : row["ReferenceType"].ToString();

            txtReferenceID.Text = row["ReferenceID"] == DBNull.Value ? "" : row["ReferenceID"].ToString();

            txtNotes.Text = row["Notes"] == DBNull.Value ? "" : row["Notes"].ToString();

            chkIsAutoGenerated.Checked = row["IsAutoGenerated"] != DBNull.Value && Convert.ToBoolean(row["IsAutoGenerated"]);
            chkIsAutoGenerated.Enabled = false;
            btnUpdate.Enabled = !chkIsAutoGenerated.Checked;
            btnDelete.Enabled = !chkIsAutoGenerated.Checked;
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateInputs())
                    return;

                Voucher voucher = GetVoucherFromInputs();

                await Task.Run(() => voucherService.AddVoucher(voucher));

                UIHelper.ShowInfo("تمت إضافة السند بنجاح.");

                await LoadVouchersAsync();
                ClearInputs();
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("إضافة السند", ex);
            }
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedVoucherId == 0)
                {
                    UIHelper.ShowWarning("اختر سنداً من الجدول أولاً.");
                    return;
                }

                if (!btnUpdate.Enabled)
                {
                    UIHelper.ShowWarning("السند التلقائي للقراءة فقط ولا يمكن تعديله.");
                    return;
                }

                if (!ValidateInputs())
                    return;

                Voucher voucher = GetVoucherFromInputs();

                bool result = await Task.Run(() => voucherService.UpdateVoucher(voucher));

                if (result)
                    UIHelper.ShowInfo("تم تعديل السند بنجاح.");
                else
                    UIHelper.ShowWarning("لم يتم العثور على السند أو لم يتم تعديله.");

                await LoadVouchersAsync();
                ClearInputs();
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("تعديل السند", ex);
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedVoucherId == 0)
                {
                    UIHelper.ShowWarning("اختر سنداً من الجدول أولاً.");
                    return;
                }

                if (!btnDelete.Enabled)
                {
                    UIHelper.ShowWarning("السند التلقائي للقراءة فقط ولا يمكن حذفه؛ حافظ على الأثر المالي.");
                    return;
                }

                if (!UIHelper.ShowConfirmation("هل تريد حذف السند المحدد؟", "تأكيد الحذف"))
                    return;

                bool result = await Task.Run(() => voucherService.DeleteVoucher(selectedVoucherId));

                if (result)
                    UIHelper.ShowInfo("تم حذف السند بنجاح.");
                else
                    UIHelper.ShowWarning("لم يتم العثور على السند أو لم يتم حذفه.");

                await LoadVouchersAsync();
                ClearInputs();
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("حذف السند", ex);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearInputs();
        }
    }
}
