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

        private int selectedVoucherId = 0;
        private DataTable allVouchers;
        private bool isLoading = false;
        private Button btnNewReceipt;
        private Button btnNewPayment;
        private Button btnPreview;
        private Button btnPrint;
        private Button btnExportCsv;
        private readonly PrintDocument voucherPrintDocument = new PrintDocument();
        private Voucher voucherToPrint;

        public VouchersForm()
        {
            InitializeComponent();
            SchoolSystem.Helpers.UIHelper.ApplyStyle(this);
            Dock = DockStyle.Fill;
            ConfigureFinancialActions();
            voucherPrintDocument.PrintPage += VoucherPrintDocument_PrintPage;
            Load += VouchersForm_Load;
        }

        private void ConfigureFinancialActions()
        {
            btnNewReceipt = CreateActionButton("سند قبض جديد", Color.FromArgb(22, 160, 133));
            btnNewPayment = CreateActionButton("سند صرف جديد", Color.FromArgb(142, 68, 173));
            btnPreview = CreateActionButton("معاينة", Color.FromArgb(52, 152, 219));
            btnPrint = CreateActionButton("طباعة", Color.FromArgb(41, 128, 185));
            btnExportCsv = CreateActionButton("تصدير CSV", Color.FromArgb(39, 174, 96));

            btnNewReceipt.Click += (s, e) => PrepareNewVoucher("قبض");
            btnNewPayment.Click += (s, e) => PrepareNewVoucher("صرف");
            btnPreview.Click += (s, e) => PreviewSelectedVoucher();
            btnPrint.Click += (s, e) => PrintSelectedVoucher();
            btnExportCsv.Click += (s, e) => ExportVisibleVouchersCsv();

            panelButtons.Controls.Add(btnExportCsv);
            panelButtons.Controls.Add(btnPrint);
            panelButtons.Controls.Add(btnPreview);
            panelButtons.Controls.Add(btnNewPayment);
            panelButtons.Controls.Add(btnNewReceipt);
            btnExportCsv.Location = new Point(5, 10);
            btnPrint.Location = new Point(120, 10);
            btnPreview.Location = new Point(235, 10);
            btnNewPayment.Location = new Point(350, 10);
            btnNewReceipt.Location = new Point(465, 10);
        }

        private Button CreateActionButton(string text, Color color)
        {
            return new Button
            {
                Text = text,
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Tahoma", 9.5F, FontStyle.Bold),
                Size = new Size(115, 35),
                FlatAppearance = { BorderSize = 0 },
                UseVisualStyleBackColor = false
            };
        }

        private void PrepareNewVoucher(string voucherType)
        {
            ClearInputs();
            cmbVoucherType.Text = voucherType;
            txtAmount.Focus();
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

            using (PrintPreviewDialog preview = new PrintPreviewDialog())
            {
                preview.Document = voucherPrintDocument;
                preview.RightToLeft = RightToLeft.Yes;
                preview.WindowState = FormWindowState.Maximized;
                preview.ShowDialog(FindForm());
            }
        }

        private void PrintSelectedVoucher()
        {
            if (!TryGetSelectedVoucherForOutput(out voucherToPrint))
                return;

            using (PrintDialog dialog = new PrintDialog())
            {
                dialog.Document = voucherPrintDocument;
                if (dialog.ShowDialog(FindForm()) == DialogResult.OK)
                    voucherPrintDocument.Print();
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

        private void ExportVisibleVouchersCsv()
        {
            if (dataGridViewVouchers.Rows.Count == 0)
            {
                UIHelper.ShowWarning("لا توجد سندات لتصديرها.");
                return;
            }

            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "ملفات CSV (*.csv)|*.csv";
                dialog.FileName = "السندات_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".csv";
                dialog.Title = "تصدير السندات";
                if (dialog.ShowDialog(FindForm()) != DialogResult.OK)
                    return;

                try
                {
                    StringBuilder csv = new StringBuilder();
                    csv.AppendLine("رقم السند,النوع,المبلغ,التاريخ,الطرف,البيان,طريقة الدفع,المرجع,ملاحظات");
                    foreach (DataGridViewRow row in dataGridViewVouchers.Rows)
                    {
                        if (row.IsNewRow) continue;
                        csv.AppendLine(string.Join(",", new[]
                        {
                            CsvValue(row.Cells["VoucherNumber"].Value),
                            CsvValue(row.Cells["VoucherType"].Value),
                            CsvValue(row.Cells["Amount"].Value),
                            CsvValue(row.Cells["VoucherDate"].Value),
                            CsvValue(row.Cells["PartyName"].Value),
                            CsvValue(row.Cells["Description"].Value),
                            CsvValue(row.Cells["PaymentMethod"].Value),
                            CsvValue(row.Cells["ReferenceType"].Value),
                            CsvValue(row.Cells["Notes"].Value)
                        }));
                    }

                    File.WriteAllText(dialog.FileName, csv.ToString(), new UTF8Encoding(true));
                    UIHelper.ShowInfo("تم تصدير السندات الظاهرة بنجاح.");
                }
                catch (Exception ex)
                {
                    UIHelper.ShowException("تصدير السندات", ex);
                }
            }
        }

        private string CsvValue(object value)
        {
            string text = value == null || value == DBNull.Value ? string.Empty : value.ToString();
            return "\"" + text.Replace("\"", "\"\"") + "\"";
        }

        private async void VouchersForm_Load(object sender, EventArgs e)
        {
            try
            {
                isLoading = true;

                InitializeLookups();

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

            dataGridViewVouchers.DataSource = dv;

            lblRecordCount.Text = "عدد السندات: " + dv.Count;

            FormatGrid();
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
            if (!isLoading)
                ApplyFilter();
        }

        private void ClearInputs()
        {
            selectedVoucherId = 0;

            txtVoucherNumber.Clear();

            if (cmbVoucherType.Items.Count > 0)
                cmbVoucherType.SelectedIndex = 0;

            txtAmount.Text = "0";

            dtpVoucherDate.Value = DateTime.Today;

            txtPartyName.Clear();
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
            if (cmbVoucherType.SelectedItem == null)
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

            if (amount <= 0)
            {
                UIHelper.ShowWarning("يجب أن يكون مبلغ السند أكبر من صفر.");
                txtAmount.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPartyName.Text))
            {
                UIHelper.ShowWarning("أدخل اسم الطرف.");
                txtPartyName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                UIHelper.ShowWarning("أدخل بيان السند.");
                txtDescription.Focus();
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
            if (decimal.TryParse(text, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal value))
                return value;

            if (decimal.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out value))
                return value;

            return 0;
        }

        private int? ReadNullableInt(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return null;

            if (int.TryParse(text.Trim(), out int value) && value > 0)
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

                PartyName = txtPartyName.Text.Trim(),
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
            selectedVoucherId = Convert.ToInt32(row["VoucherID"]);

            txtVoucherNumber.Text = row["VoucherNumber"] == DBNull.Value ? "" : row["VoucherNumber"].ToString();
            cmbVoucherType.Text = row["VoucherType"].ToString();

            txtAmount.Text = Convert.ToDecimal(row["Amount"]).ToString("N2");

            dtpVoucherDate.Value = Convert.ToDateTime(row["VoucherDate"]);

            txtPartyName.Text = row["PartyName"] == DBNull.Value ? "" : row["PartyName"].ToString();
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
