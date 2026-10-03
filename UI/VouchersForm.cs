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
using SchoolSystem.Security;

namespace SchoolSystem.UI
{
    public partial class VouchersForm : UserControl
    {
        // تعريف الدالة VoucherService لتنفيذ العملية المرتبطة بها.
        private readonly VoucherService voucherService = new VoucherService();

        // تعريف الدالة PartyService لتنفيذ العملية المرتبطة بها.
        private readonly PartyService partyService = new PartyService();

        private DataTable voucherParties;

        private int selectedVoucherId = 0;

        private bool currentVoucherProtected = false;

        private bool currentVoucherLinkedToStudentFee = false;

        private DataTable allVouchers;

        private bool isLoading = false;

        // تعريف الدالة PrintDocument لتنفيذ العملية المرتبطة بها.
        private readonly PrintDocument voucherPrintDocument = new PrintDocument();

        private Voucher voucherToPrint;

        // تعريف الدالة VouchersForm لتنفيذ العملية المرتبطة بها.
        public VouchersForm()
        {
            InitializeComponent();

            // ============================================================
            // تمت إزالة جميع أوامر التصميم والتنسيق من هنا.
            // التصميم مسؤول عنه VouchersForm.Designer.cs فقط.
            //
            // لم يتم تغيير أي منطق أو معالجة أو أحداث.
            // ============================================================

            // تطبيق قواعد التحقق من الحقول (فلترة الأحرف، أقصى طول، والتحقق عند
            // المغادرة) دون تغيير تنسيق المصمم لهذه الشاشة.
            SchoolSystem.Helpers.UIHelper.ApplyFieldValidation(this);

            txtSearch.TextChanged += (sender, e) => ApplyFilter();

            cmbVoucherType.SelectedIndexChanged += cmbVoucherType_SelectedIndexChanged;

            cmbPartyName.SelectedIndexChanged += cmbPartyName_SelectedIndexChanged;

            // أزرار العمليات المالية: الربط فقط (الهيكل في Designer).
            btnNewReceipt.Click += (s, e) => PrepareNewVoucher("قبض");
            btnNewPayment.Click += (s, e) => PrepareNewVoucher("صرف");
            btnPreview.Click += (s, e) => PreviewSelectedVoucher();
            btnPrint.Click += (s, e) => PrintSelectedVoucher();
            btnExportExcel.Click += (s, e) => ExportVisibleVouchersExcel();
            btnExportPdf.Click += (s, e) => ExportSelectedVoucherPdf();

            // قيم التصفية الابتدائية: من بداية الشهر حتى اليوم.
            dtpFilterFrom.Value =
                new DateTime(
                    DateTime.Today.Year,
                    DateTime.Today.Month,
                    1);

            dtpFilterTo.Value = DateTime.Today;

            voucherPrintDocument.PrintPage += VoucherPrintDocument_PrintPage;

            Load += VouchersForm_Load;
        }

        // تعريف الدالة PrepareNewVoucher لتنفيذ العملية المرتبطة بها.
        private void PrepareNewVoucher(string voucherType)
        {
            ClearInputs();

            cmbVoucherType.Text = voucherType;

            SetNextVoucherNumber();

            txtAmount.Focus();
        }

        // تعريف الدالة cmbVoucherType_SelectedIndexChanged لتنفيذ العملية المرتبطة بها.
        private void cmbVoucherType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isLoading && selectedVoucherId == 0)
            {
                SetNextVoucherNumber();
                ApplyPartyFilter();
                ApplyVoucherTypeRules();
            }
        }

        // تعريف الدالة ApplyVoucherTypeRules لتنفيذ العملية المرتبطة بها.
        private void ApplyVoucherTypeRules()
        {
            bool isReceipt =
                string.Equals(
                    cmbVoucherType.Text.Trim(),
                    "قبض",
                    StringComparison.OrdinalIgnoreCase);

            int? studentId = GetSelectedStudentId();

            if (isReceipt && studentId.HasValue)
            {
                cmbReferenceType.Text = "رسوم";

                txtReferenceID.Clear();

                if (string.IsNullOrWhiteSpace(txtDescription.Text))
                    txtDescription.Text = "سند قبض رسوم الطالب";
            }
        }

        // تعريف الدالة cmbPartyName_SelectedIndexChanged لتنفيذ العملية المرتبطة بها.
        private void cmbPartyName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoading ||
                selectedVoucherId != 0 ||
                cmbPartyName.SelectedIndex < 0)
            {
                return;
            }

            DataRowView selected =
                cmbPartyName.SelectedItem as DataRowView;

            if (selected != null &&
                selected.Row.Table.Columns.Contains("StudentID") &&
                selected.Row["StudentID"] != DBNull.Value &&
                cmbVoucherType.Text == "قبض")
            {
                cmbReferenceType.Text = "رسوم";

                txtReferenceID.Clear();

                if (string.IsNullOrWhiteSpace(txtDescription.Text))
                    txtDescription.Text = "سند قبض رسوم الطالب";
            }
        }

        // تعريف الدالة GetSelectedPartyType لتنفيذ العملية المرتبطة بها.
        private string GetSelectedPartyType()
        {
            DataRowView selected =
                cmbPartyName.SelectedItem as DataRowView;

            return selected == null ||
                   !selected.Row.Table.Columns.Contains("PartyType") ||
                   selected.Row["PartyType"] == DBNull.Value
                ? string.Empty
                : selected.Row["PartyType"].ToString();
        }

        // تعريف الدالة GetSelectedStudentId لتنفيذ العملية المرتبطة بها.
        private int? GetSelectedStudentId()
        {
            DataRowView selected =
                cmbPartyName.SelectedItem as DataRowView;

            if (selected == null ||
                !selected.Row.Table.Columns.Contains("StudentID") ||
                selected.Row["StudentID"] == DBNull.Value)
            {
                return null;
            }

            int studentId;

            return
                int.TryParse(
                    selected.Row["StudentID"].ToString(),
                    out studentId) &&
                studentId > 0
                    ? (int?)studentId
                    : null;
        }

        // تعريف الدالة SetNextVoucherNumber لتنفيذ العملية المرتبطة بها.
        private void SetNextVoucherNumber()
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                string voucherType =
                    cmbVoucherType == null
                        ? string.Empty
                        : cmbVoucherType.Text.Trim();

                if (selectedVoucherId == 0 &&
                    (voucherType == "قبض" ||
                     voucherType == "صرف"))
                {
                    txtVoucherNumber.Text =
                        voucherService.GenerateVoucherNumber(voucherType);
                }
            }
            catch (Exception ex)
            {
                txtVoucherNumber.Clear();

                UIHelper.ShowException(
                    "توليد رقم السند",
                    ex);
            }
        }

        // تعريف الدالة LoadVoucherPartiesAsync لتنفيذ العملية المرتبطة بها.
        private async Task LoadVoucherPartiesAsync()
        {
            voucherParties =
                await Task.Run(
                    () => partyService.GetVoucherParties());

            ApplyPartyFilter();
        }

        // تعريف الدالة ApplyPartyFilter لتنفيذ العملية المرتبطة بها.
        private void ApplyPartyFilter()
        {
            if (voucherParties == null ||
                cmbPartyName == null)
            {
                return;
            }

            string voucherType =
                cmbVoucherType == null
                    ? string.Empty
                    : cmbVoucherType.Text.Trim();

            DataView view =
                new DataView(voucherParties);

            if (string.Equals(
                    voucherType,
                    "قبض",
                    StringComparison.OrdinalIgnoreCase))
            {
                view.RowFilter = "PartyType = 'طلاب'";
            }
            else if (string.Equals(
                         voucherType,
                         "صرف",
                         StringComparison.OrdinalIgnoreCase))
            {
                view.RowFilter =
                    "PartyType IN ('معلمون', 'موظفون')";
            }
            else
            {
                view.RowFilter = "1 = 0";
            }

            cmbPartyName.DataSource = view;
            cmbPartyName.DisplayMember = "DisplayName";
            cmbPartyName.ValueMember = "PartyKey";
            cmbPartyName.SelectedIndex = -1;
            cmbPartyName.Text = string.Empty;
        }

        // تعريف الدالة TryGetSelectedVoucherForOutput لتنفيذ العملية المرتبطة بها.
        private bool TryGetSelectedVoucherForOutput(out Voucher voucher)
        {
            voucher = null;

            if (selectedVoucherId <= 0)
            {
                UIHelper.ShowWarning(
                    "اختر سنداً من الجدول أولاً للطباعة أو المعاينة.");

                return false;
            }

            voucher = GetVoucherFromInputs();

            if (voucher == null ||
                voucher.VoucherID <= 0)
            {
                UIHelper.ShowWarning(
                    "بيانات السند المحدد غير مكتملة.");

                return false;
            }

            return true;
        }

        // تعريف الدالة PreviewSelectedVoucher لتنفيذ العملية المرتبطة بها.
        private void PreviewSelectedVoucher()
        {
            try
            {
                CurrentUser.DemandAction(
                    "Vouchers",
                    "Print",
                    "ليس لديك صلاحية معاينة السندات.");
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("صلاحية الطباعة", ex);
                return;
            }

            if (!TryGetSelectedVoucherForOutput(
                    out voucherToPrint))
            {
                return;
            }

            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                using (PrintPreviewDialog preview =
                       new PrintPreviewDialog())
                {
                    preview.Document =
                        voucherPrintDocument;

                    preview.RightToLeft =
                        RightToLeft.Yes;

                    preview.WindowState =
                        FormWindowState.Maximized;

                    preview.ShowDialog(
                        FindForm());
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowException(
                    "معاينة السند",
                    ex);
            }
        }

        // تعريف الدالة PrintSelectedVoucher لتنفيذ العملية المرتبطة بها.
        private void PrintSelectedVoucher()
        {
            try
            {
                CurrentUser.DemandAction(
                    "Vouchers",
                    "Print",
                    "ليس لديك صلاحية طباعة السندات.");
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("صلاحية الطباعة", ex);
                return;
            }

            if (!TryGetSelectedVoucherForOutput(
                    out voucherToPrint))
            {
                return;
            }

            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                using (PrintDialog dialog =
                       new PrintDialog())
                {
                    dialog.Document =
                        voucherPrintDocument;

                    if (dialog.ShowDialog(
                            FindForm()) ==
                        DialogResult.OK)
                    {
                        voucherPrintDocument.Print();
                    }
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowException(
                    "طباعة السند",
                    ex);
            }
        }

        // تعريف الدالة VoucherPrintDocument_PrintPage لتنفيذ العملية المرتبطة بها.
        private void VoucherPrintDocument_PrintPage(
            object sender,
            PrintPageEventArgs e)
        {
            if (voucherToPrint == null)
            {
                e.HasMorePages = false;

                return;
            }

            Rectangle bounds =
                e.MarginBounds;

            using (Font labelFont =
                   new Font(
                       "Tahoma",
                       10F,
                       FontStyle.Bold))

            using (Font valueFont =
                   new Font(
                       "Tahoma",
                       10F,
                       FontStyle.Regular))

            using (Font amountFont =
                   new Font(
                       "Tahoma",
                       15F,
                       FontStyle.Bold))

            using (Font signatureFont =
                   new Font(
                       "Tahoma",
                       9F,
                       FontStyle.Regular))

            using (StringFormat rtl =
                   new StringFormat
                   {
                       Alignment = StringAlignment.Far,
                       LineAlignment = StringAlignment.Center,
                       FormatFlags =
                           StringFormatFlags.DirectionRightToLeft,
                       Trimming =
                           StringTrimming.EllipsisCharacter
                   })

            using (StringFormat center =
                   new StringFormat
                   {
                       Alignment = StringAlignment.Center,
                       LineAlignment = StringAlignment.Center,
                       FormatFlags =
                           StringFormatFlags.DirectionRightToLeft
                   })

            using (Pen borderPen =
                   new Pen(
                       Color.FromArgb(
                           203,
                           213,
                           225),
                       1F))

            using (Pen accentPen =
                   new Pen(
                       Color.FromArgb(
                           15,
                           118,
                           110),
                       2F))

            using (Brush labelBrush =
                   new SolidBrush(
                       Color.FromArgb(
                           22,
                           50,
                           79)))

            using (Brush softBrush =
                   new SolidBrush(
                       Color.FromArgb(
                           248,
                           250,
                           252)))
            {
                int y =
                    ReportOutputHelper.DrawSchoolPrintHeader(
                        e.Graphics,
                        bounds,
                        "سند " +
                            (voucherToPrint.VoucherType ??
                             string.Empty),
                        true);

                int labelWidth = 155;
                int gap = 8;

                DrawPrintField(
                    e.Graphics,
                    bounds,
                    ref y,
                    "رقم السند",
                    voucherToPrint.VoucherNumber,
                    labelFont,
                    valueFont,
                    rtl,
                    borderPen,
                    labelBrush,
                    softBrush,
                    labelWidth,
                    gap);

                DrawPrintField(
                    e.Graphics,
                    bounds,
                    ref y,
                    "التاريخ",
                    voucherToPrint.VoucherDate
                        .ToString("dd/MM/yyyy"),
                    labelFont,
                    valueFont,
                    rtl,
                    borderPen,
                    labelBrush,
                    softBrush,
                    labelWidth,
                    gap);

                DrawPrintField(
                    e.Graphics,
                    bounds,
                    ref y,
                    "الطرف",
                    voucherToPrint.PartyName,
                    labelFont,
                    valueFont,
                    rtl,
                    borderPen,
                    labelBrush,
                    softBrush,
                    labelWidth,
                    gap);

                DrawPrintField(
                    e.Graphics,
                    bounds,
                    ref y,
                    "طريقة الدفع",
                    voucherToPrint.PaymentMethod,
                    labelFont,
                    valueFont,
                    rtl,
                    borderPen,
                    labelBrush,
                    softBrush,
                    labelWidth,
                    gap);

                DrawPrintField(
                    e.Graphics,
                    bounds,
                    ref y,
                    "البيان",
                    voucherToPrint.Description,
                    labelFont,
                    valueFont,
                    rtl,
                    borderPen,
                    labelBrush,
                    softBrush,
                    labelWidth,
                    gap);

                DrawPrintField(
                    e.Graphics,
                    bounds,
                    ref y,
                    "المرجع",
                    voucherToPrint.ReferenceType +
                    (voucherToPrint.ReferenceID.HasValue
                        ? " - " +
                          voucherToPrint.ReferenceID.Value
                        : string.Empty),
                    labelFont,
                    valueFont,
                    rtl,
                    borderPen,
                    labelBrush,
                    softBrush,
                    labelWidth,
                    gap);

                DrawPrintField(
                    e.Graphics,
                    bounds,
                    ref y,
                    "ملاحظات",
                    voucherToPrint.Notes,
                    labelFont,
                    valueFont,
                    rtl,
                    borderPen,
                    labelBrush,
                    softBrush,
                    labelWidth,
                    gap);

                y += 8;

                Rectangle amountRect =
                    new Rectangle(
                        bounds.Left,
                        y,
                        bounds.Width,
                        54);

                e.Graphics.DrawRectangle(
                    accentPen,
                    amountRect);

                using (Brush amountBrush =
                       new SolidBrush(
                           Color.FromArgb(
                               232,
                               247,
                               244)))
                {
                    e.Graphics.FillRectangle(
                        amountBrush,
                        amountRect);
                }

                e.Graphics.DrawString(
                    "المبلغ المستلم: " +
                    voucherToPrint.Amount.ToString("N2") +
                    " ريال",
                    amountFont,
                    labelBrush,
                    amountRect,
                    center);

                y +=
                    amountRect.Height +
                    28;

                e.Graphics.DrawLine(
                    accentPen,
                    bounds.Left,
                    y,
                    bounds.Right,
                    y);

                y += 20;

                using (StringFormat signature =
                       new StringFormat
                       {
                           Alignment =
                               StringAlignment.Center,

                           LineAlignment =
                               StringAlignment.Center,

                           FormatFlags =
                               StringFormatFlags.DirectionRightToLeft
                       })
                {
                    e.Graphics.DrawString(
                        "المستلم: ____________________",
                        signatureFont,
                        Brushes.Black,
                        new RectangleF(
                            bounds.Left,
                            y,
                            bounds.Width / 2F - 10F,
                            28F),
                        signature);

                    e.Graphics.DrawString(
                        "المحاسب: ____________________",
                        signatureFont,
                        Brushes.Black,
                        new RectangleF(
                            bounds.Left +
                            bounds.Width / 2F +
                            10F,
                            y,
                            bounds.Width / 2F - 10F,
                            28F),
                        signature);
                }
            }

            e.HasMorePages = false;
        }

        private void DrawPrintField(
            Graphics graphics,
            Rectangle bounds,
            ref int y,
            string label,
            string value,
            Font labelFont,
            Font valueFont,
            StringFormat rtl,
            Pen borderPen,
            Brush labelBrush,
            Brush softBrush,
            int labelWidth,
            int gap)
        {
            int rowHeight = 36;

            int valueWidth =
                Math.Max(
                    120,
                    bounds.Width -
                    labelWidth -
                    gap);

            Rectangle labelRect =
                new Rectangle(
                    bounds.Right -
                    labelWidth,
                    y,
                    labelWidth,
                    rowHeight);

            Rectangle valueRect =
                new Rectangle(
                    bounds.Left,
                    y,
                    valueWidth,
                    rowHeight);

            graphics.FillRectangle(
                labelBrush,
                labelRect);

            graphics.FillRectangle(
                softBrush,
                valueRect);

            graphics.DrawRectangle(
                borderPen,
                labelRect);

            graphics.DrawRectangle(
                borderPen,
                valueRect);

            graphics.DrawString(
                label + ":",
                labelFont,
                Brushes.White,
                labelRect,
                rtl);

            graphics.DrawString(
                string.IsNullOrWhiteSpace(value)
                    ? "-"
                    : value,
                valueFont,
                Brushes.Black,
                valueRect,
                rtl);

            y += rowHeight + 5;
        }

        // تعريف الدالة ExportSelectedVoucherPdf لتنفيذ العملية المرتبطة بها.
        private void ExportSelectedVoucherPdf()
        {
            try
            {
                CurrentUser.DemandAction(
                    "Vouchers",
                    "ExportPDF",
                    "ليس لديك صلاحية تصدير السندات إلى PDF.");
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("صلاحية التصدير", ex);
                return;
            }

            if (!TryGetSelectedVoucherForOutput(
                    out voucherToPrint))
            {
                return;
            }

            using (SaveFileDialog dialog =
                   new SaveFileDialog())
            {
                dialog.Filter =
                    "ملفات PDF (*.pdf)|*.pdf";

                dialog.FileName =
                    "سند_" +
                    SafeVoucherFileName(
                        voucherToPrint.VoucherNumber) +
                    ".pdf";

                dialog.Title =
                    "تصدير السند إلى PDF";

                if (dialog.ShowDialog(
                        FindForm()) !=
                    DialogResult.OK)
                {
                    return;
                }

                // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
                try
                {
                    DataTable table =
                        new DataTable();

                    table.Columns.Add("البيان");
                    table.Columns.Add("القيمة");

                    AddVoucherPdfRow(
                        table,
                        "رقم السند",
                        voucherToPrint.VoucherNumber);

                    AddVoucherPdfRow(
                        table,
                        "نوع السند",
                        voucherToPrint.VoucherType);

                    AddVoucherPdfRow(
                        table,
                        "التاريخ",
                        voucherToPrint.VoucherDate
                            .ToString("dd/MM/yyyy"));

                    AddVoucherPdfRow(
                        table,
                        "الطرف",
                        voucherToPrint.PartyName);

                    AddVoucherPdfRow(
                        table,
                        "طريقة الدفع",
                        voucherToPrint.PaymentMethod);

                    AddVoucherPdfRow(
                        table,
                        "البيان",
                        voucherToPrint.Description);

                    AddVoucherPdfRow(
                        table,
                        "نوع المرجع",
                        voucherToPrint.ReferenceType);

                    AddVoucherPdfRow(
                        table,
                        "رقم المرجع",
                        voucherToPrint.ReferenceID.HasValue
                            ? voucherToPrint.ReferenceID.Value
                                .ToString()
                            : "-");

                    AddVoucherPdfRow(
                        table,
                        "الملاحظات",
                        voucherToPrint.Notes);

                    AddVoucherPdfRow(
                        table,
                        "المبلغ",
                        voucherToPrint.Amount
                            .ToString("N2") +
                        " ريال");

                    ReportOutputHelper.ExportToPdf(
                        table,
                        dialog.FileName,
                        "سند " +
                        (voucherToPrint.VoucherType ??
                         string.Empty),
                        "رقم السند: " +
                        voucherToPrint.VoucherNumber +
                        " | المبلغ: " +
                        voucherToPrint.Amount
                            .ToString("N2") +
                        " ريال");

                    UIHelper.ShowInfo(
                        "تم تصدير السند إلى PDF بنجاح.");
                }
                catch (Exception ex)
                {
                    UIHelper.ShowException(
                        "تصدير السند إلى PDF",
                        ex);
                }
            }
        }

        // تعريف الدالة AddVoucherPdfRow لتنفيذ العملية المرتبطة بها.
        private static void AddVoucherPdfRow(
            DataTable table,
            string label,
            string value)
        {
            DataRow row =
                table.NewRow();

            row[0] =
                label ?? string.Empty;

            row[1] =
                string.IsNullOrWhiteSpace(value)
                    ? "-"
                    : value;

            table.Rows.Add(row);
        }

        // تعريف الدالة SafeVoucherFileName لتنفيذ العملية المرتبطة بها.
        private static string SafeVoucherFileName(
            string value)
        {
            string result =
                string.IsNullOrWhiteSpace(value)
                    ? "سند"
                    : value;

            foreach (char invalid in
                     Path.GetInvalidFileNameChars())
            {
                result =
                    result.Replace(
                        invalid.ToString(),
                        "_");
            }

            return result;
        }

        // تعريف الدالة ExportVisibleVouchersExcel لتنفيذ العملية المرتبطة بها.
        private void ExportVisibleVouchersExcel()
        {
            try
            {
                CurrentUser.DemandAction(
                    "Vouchers",
                    "ExportExcel",
                    "ليس لديك صلاحية تصدير السندات إلى Excel.");
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("صلاحية التصدير", ex);
                return;
            }

            if (dataGridViewVouchers.Rows.Count == 0)
            {
                UIHelper.ShowWarning(
                    "لا توجد سندات لتصديرها.");

                return;
            }

            using (SaveFileDialog dialog =
                   new SaveFileDialog())
            {
                dialog.Filter =
                    "ملفات Excel (*.xlsx)|*.xlsx";

                dialog.FileName =
                    "قائمة_السندات_" +
                    DateTime.Now.ToString(
                        "yyyyMMdd_HHmm") +
                    ".xlsx";

                dialog.Title =
                    "تصدير السندات";

                if (dialog.ShowDialog(
                        FindForm()) !=
                    DialogResult.OK)
                {
                    return;
                }

                // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
                try
                {
                    DataTable exportTable =
                        new DataTable();

                    foreach (
                        DataGridViewColumn column
                        in dataGridViewVouchers.Columns)
                    {
                        exportTable.Columns.Add(
                            column.HeaderText ??
                            column.Name);
                    }

                    foreach (
                        DataGridViewRow row
                        in dataGridViewVouchers.Rows)
                    {
                        if (row.IsNewRow)
                            continue;

                        DataRow exportRow =
                            exportTable.NewRow();

                        for (
                            int index = 0;
                            index <
                            dataGridViewVouchers
                                .Columns.Count;
                            index++)
                        {
                            exportRow[index] =
                                row.Cells[index].Value ==
                                    null ||
                                row.Cells[index].Value ==
                                    DBNull.Value
                                    ? string.Empty
                                    : row.Cells[index]
                                        .Value.ToString();
                        }

                        exportTable.Rows.Add(
                            exportRow);
                    }

                    ReportOutputHelper.ExportToExcel(
                        exportTable,
                        dialog.FileName,
                        "الحركة المالية",
                        "عدد السجلات: " +
                        exportTable.Rows.Count);

                    UIHelper.ShowInfo(
                        "تم تصدير السندات إلى Excel بنجاح.");
                }
                catch (Exception ex)
                {
                    UIHelper.ShowException(
                        "تصدير السندات",
                        ex);
                }
            }
        }

        // تعريف الدالة VouchersForm_Load لتنفيذ العملية المرتبطة بها.
        private async void VouchersForm_Load(
            object sender,
            EventArgs e)
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                isLoading = true;

                InitializeLookups();

                await LoadVoucherPartiesAsync();

                dtpVoucherDate.Value =
                    DateTime.Today;

                await LoadVouchersAsync();

                ClearInputs();
            }
            catch (Exception ex)
            {
                UIHelper.ShowException(
                    "تحميل السندات",
                    ex);
            }
            finally
            {
                isLoading = false;
            }
        }

        // تعريف الدالة InitializeLookups لتنفيذ العملية المرتبطة بها.
        private void InitializeLookups()
        {
            cmbVoucherType.Items.Clear();

            cmbVoucherType.Items.AddRange(
                new object[]
                {
                    "قبض",
                    "صرف"
                });

            cmbVoucherType.SelectedIndex = 0;

            cmbPaymentMethod.Items.Clear();

            cmbPaymentMethod.Items.AddRange(
                new object[]
                {
                    "نقداً",
                    "حوالة",
                    "شيك",
                    "محفظة إلكترونية",
                    "أخرى"
                });

            cmbPaymentMethod.SelectedIndex = 0;

            cmbReferenceType.Items.Clear();

            cmbReferenceType.Items.AddRange(
                new object[]
                {
                    "عام",
                    "رسوم",
                    "مصروفات",
                    "رواتب",
                    "نقل",
                    "مكتبة"
                });

            cmbReferenceType.SelectedIndex = 0;

            cmbFilterType.Items.Clear();

            cmbFilterType.Items.AddRange(
                new object[]
                {
                    "كل السندات",
                    "قبض",
                    "صرف"
                });

            cmbFilterType.SelectedIndex = 0;

            // ============================================================
            // تمت إزالة إعدادات تنسيق وسلوك ComboBox من هنا.
            //
            // DropDownStyle
            // AutoCompleteSource
            // AutoCompleteMode
            // IntegralHeight
            //
            // لأن هذه الإعدادات يجب أن يحددها Designer.
            // ============================================================
        }

        // تعريف الدالة LoadVouchersAsync لتنفيذ العملية المرتبطة بها.
        private async Task LoadVouchersAsync()
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                Cursor =
                    Cursors.WaitCursor;

                allVouchers =
                    await Task.Run(
                        () => voucherService.GetAllVouchers());

                ApplyFilter();
            }
            finally
            {
                Cursor =
                    Cursors.Default;
            }
        }

        // تعريف الدالة ApplyFilter لتنفيذ العملية المرتبطة بها.
        private void ApplyFilter()
        {
            if (allVouchers == null)
                return;

            DataView dv =
                allVouchers.DefaultView;

            string searchText =
                UIHelper.EscapeDataViewFilterValue(
                    txtSearch.Text);

            string selectedType =
                cmbFilterType.SelectedItem == null
                    ? "كل السندات"
                    : cmbFilterType
                        .SelectedItem
                        .ToString();

            string filter = "";

            if (!string.IsNullOrWhiteSpace(
                    searchText))
            {
                filter =
                    "(VoucherNumber LIKE '%" +
                    searchText +
                    "%' " +

                    "OR VoucherType LIKE '%" +
                    searchText +
                    "%' " +

                    "OR PartyName LIKE '%" +
                    searchText +
                    "%' " +

                    "OR Description LIKE '%" +
                    searchText +
                    "%' " +

                    "OR PaymentMethod LIKE '%" +
                    searchText +
                    "%' " +

                    "OR ReferenceType LIKE '%" +
                    searchText +
                    "%' " +

                    "OR Notes LIKE '%" +
                    searchText +
                    "%')";
            }

            if (selectedType != "كل السندات")
            {
                if (!string.IsNullOrWhiteSpace(
                        filter))
                {
                    filter += " AND ";
                }

                filter +=
                    "VoucherType = '" +
                    UIHelper.EscapeDataViewFilterValue(
                        selectedType) +
                    "'";
            }

            dv.RowFilter = filter;

            DateTime fromDate =
                dtpFilterFrom == null
                    ? DateTime.MinValue.Date
                    : dtpFilterFrom.Value.Date;

            DateTime toDate =
                dtpFilterTo == null
                    ? DateTime.MaxValue.Date
                    : dtpFilterTo.Value.Date;

            DataTable filtered =
                allVouchers.Clone();

            decimal receipts = 0m;

            decimal payments = 0m;

            foreach (DataRowView rowView in dv)
            {
                DataRow row =
                    rowView.Row;

                DateTime voucherDate =
                    row["VoucherDate"] ==
                        DBNull.Value
                        ? DateTime.MinValue.Date
                        : Convert.ToDateTime(
                              row["VoucherDate"])
                            .Date;

                if (voucherDate < fromDate ||
                    voucherDate > toDate)
                {
                    continue;
                }

                filtered.ImportRow(row);

                decimal amount =
                    row["Amount"] ==
                        DBNull.Value
                        ? 0m
                        : Convert.ToDecimal(
                              row["Amount"]);

                string voucherType =
                    row["VoucherType"] ==
                        DBNull.Value
                        ? string.Empty
                        : row["VoucherType"]
                            .ToString();

                if (string.Equals(
                        voucherType,
                        "قبض",
                        StringComparison.OrdinalIgnoreCase))
                {
                    receipts += amount;
                }
                else if (string.Equals(
                             voucherType,
                             "صرف",
                             StringComparison.OrdinalIgnoreCase))
                {
                    payments += amount;
                }
            }

            dataGridViewVouchers.DataSource =
                filtered;

            UpdateMovementSummary(
                filtered.Rows.Count,
                receipts,
                payments);

            FormatGrid();
        }

        // تعريف الدالة UpdateMovementSummary لتنفيذ العملية المرتبطة بها.
        private void UpdateMovementSummary(
            int recordCount,
            decimal receipts,
            decimal payments)
        {
            decimal net =
                receipts -
                payments;

            if (lblTotalReceipts != null)
            {
                lblTotalReceipts.Text =
                    "إجمالي القبض: " +
                    receipts.ToString("N2") +
                    " ريال";
            }

            if (lblTotalPayments != null)
            {
                lblTotalPayments.Text =
                    "إجمالي الصرف: " +
                    payments.ToString("N2") +
                    " ريال";
            }

            if (lblNetBalance != null)
            {
                lblNetBalance.Text =
                    "الصافي: " +
                    net.ToString("N2") +
                    " ريال";

                lblNetBalance.ForeColor =
                    net >= 0
                        ? Color.FromArgb(
                            39,
                            174,
                            96)
                        : Color.FromArgb(
                            192,
                            57,
                            43);
            }

            lblRecordCount.Text =
                "عدد السندات ضمن الفترة: " +
                recordCount;
        }

        // تعريف الدالة FormatGrid لتنفيذ العملية المرتبطة بها.
        private void FormatGrid()
        {
            if (dataGridViewVouchers.Columns.Count == 0)
                return;

            HideColumn("VoucherID");
            HideColumn("ReferenceID");
            HideColumn("StudentID");
            HideColumn("PartyKey");

            SetHeader(
                "VoucherNumber",
                "رقم السند");

            SetHeader(
                "VoucherType",
                "النوع");

            SetHeader(
                "Amount",
                "المبلغ");

            SetHeader(
                "VoucherDate",
                "التاريخ");

            SetHeader(
                "PartyName",
                "الطرف");

            SetHeader(
                "Description",
                "البيان");

            SetHeader(
                "PaymentMethod",
                "طريقة الدفع");

            SetHeader(
                "ReferenceType",
                "المرجع");

            SetHeader(
                "Notes",
                "ملاحظات");

            SetHeader(
                "IsAutoGenerated",
                "تلقائي");

            SetHeader(
                "CreatedAt",
                "تاريخ الإدخال");

            FormatMoney("Amount");

            FormatDate("VoucherDate");

            FormatDate("CreatedAt");

            // ============================================================
            // أبقي هذا لأنه جزء من تحديث عرض الجدول المرتبط بالبيانات
            // وليس تهيئة التصميم الأساسية من خارج الـDesigner.
            // ============================================================

            dataGridViewVouchers.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
        }

        // تعريف الدالة HideColumn لتنفيذ العملية المرتبطة بها.
        private void HideColumn(string columnName)
        {
            if (dataGridViewVouchers.Columns.Contains(
                    columnName))
            {
                dataGridViewVouchers.Columns[
                    columnName].Visible = false;
            }
        }

        // تعريف الدالة SetHeader لتنفيذ العملية المرتبطة بها.
        private void SetHeader(
            string columnName,
            string headerText)
        {
            if (dataGridViewVouchers.Columns.Contains(
                    columnName))
            {
                dataGridViewVouchers.Columns[
                    columnName].HeaderText =
                    headerText;
            }
        }

        // تعريف الدالة FormatMoney لتنفيذ العملية المرتبطة بها.
        private void FormatMoney(
            string columnName)
        {
            if (dataGridViewVouchers.Columns.Contains(
                    columnName))
            {
                dataGridViewVouchers.Columns[
                    columnName]
                    .DefaultCellStyle
                    .Format = "N2";
            }
        }

        // تعريف الدالة FormatDate لتنفيذ العملية المرتبطة بها.
        private void FormatDate(
            string columnName)
        {
            if (dataGridViewVouchers.Columns.Contains(
                    columnName))
            {
                dataGridViewVouchers.Columns[
                    columnName]
                    .DefaultCellStyle
                    .Format =
                    "dd/MM/yyyy";
            }
        }

        // تعريف الدالة FilterControls_Changed لتنفيذ العملية المرتبطة بها.
        private void FilterControls_Changed(
            object sender,
            EventArgs e)
        {
            if (dtpFilterFrom != null &&
                dtpFilterTo != null &&
                dtpFilterFrom.Value.Date >
                dtpFilterTo.Value.Date)
            {
                UIHelper.ShowWarning(
                    "تاريخ البداية يجب ألا يتجاوز تاريخ النهاية.");

                return;
            }

            if (!isLoading)
                ApplyFilter();
        }

        // تعريف الدالة ClearInputs لتنفيذ العملية المرتبطة بها.
        private void ClearInputs()
        {
selectedVoucherId = 0;

            currentVoucherProtected =
                false;

            currentVoucherLinkedToStudentFee = false;

            txtVoucherNumber.Clear();

            if (cmbVoucherType.Items.Count > 0)
                cmbVoucherType.SelectedIndex = 0;

            SetNextVoucherNumber();

            txtAmount.Text = "0";

            dtpVoucherDate.Value =
                DateTime.Today;

            cmbPartyName.SelectedIndex = -1;
            cmbPartyName.Text =
                string.Empty;

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

            txtAmount.ReadOnly = false;

            cmbVoucherType.Enabled = true;

            cmbReferenceType.Enabled = true;

            txtReferenceID.ReadOnly = false;
        }

        // تعريف الدالة ValidateInputs لتنفيذ العملية المرتبطة بها.
        private bool ValidateInputs()
        {
            string voucherNumber =
                txtVoucherNumber.Text == null
                    ? string.Empty
                    : txtVoucherNumber.Text.Trim();

            if (string.IsNullOrWhiteSpace(
                    voucherNumber) ||
                voucherNumber.Length > 50 ||
                voucherNumber.IndexOfAny(
                    new[] { '\r', '\n', '\t' }) >= 0)
            {
                UIHelper.FocusAndWarn(
                    txtVoucherNumber,
                    "أدخل رقم سند صحيحاً بطول لا يتجاوز 50 حرفاً.");

                return false;
            }

            if (cmbVoucherType.SelectedItem == null ||
                string.IsNullOrWhiteSpace(
                    cmbVoucherType.Text))
            {
                UIHelper.ShowWarning(
                    "اختر نوع السند.");

                cmbVoucherType.Focus();

                return false;
            }

            decimal amount;

            if (!UIHelper.TryParseDecimal(
                    txtAmount.Text,
                    out amount))
            {
                UIHelper.ShowWarning(
                    "أدخل مبلغاً رقمياً صحيحاً.");

                txtAmount.Focus();

                return false;
            }

            if (amount <= 0 ||
                amount > 100000000m)
            {
                UIHelper.ShowWarning(
                    "يجب أن يكون مبلغ السند أكبر من صفر ولا يتجاوز 100,000,000.");

                txtAmount.Focus();

                return false;
            }

            if (string.IsNullOrWhiteSpace(
                    cmbReferenceType.Text))
            {
                UIHelper.ShowWarning(
                    "اختر نوع المرجع.");

                cmbReferenceType.Focus();

                return false;
            }

            if (cmbPartyName.SelectedIndex < 0 ||
                string.IsNullOrWhiteSpace(
                    cmbPartyName.Text) ||
                cmbPartyName.Text.Trim().Length > 200)
            {
                UIHelper.FocusAndWarn(
                    cmbPartyName,
                    "اختر الطرف من القائمة.");

                return false;
            }

            if (string.IsNullOrWhiteSpace(
                    txtDescription.Text) ||
                txtDescription.Text.Trim().Length > 500)
            {
                UIHelper.FocusAndWarn(
                    txtDescription,
                    "أدخل بيان السند بطول لا يتجاوز 500 حرف.");

                return false;
            }

            if (txtNotes.Text.Trim().Length > 1000)
            {
                UIHelper.FocusAndWarn(
                    txtNotes,
                    "الملاحظات يجب ألا تتجاوز 1000 حرف.");

                return false;
            }

            if (cmbPaymentMethod.SelectedItem == null ||
                string.IsNullOrWhiteSpace(
                    cmbPaymentMethod.Text))
            {
                UIHelper.ShowWarning(
                    "اختر طريقة الدفع.");

                cmbPaymentMethod.Focus();

                return false;
            }

            if (dtpVoucherDate.Value.Date >
                DateTime.Today)
            {
                UIHelper.ShowWarning(
                    "تاريخ السند لا يمكن أن يكون في المستقبل.");

                dtpVoucherDate.Focus();

                return false;
            }

            int? referenceId =
                ReadNullableInt(
                    txtReferenceID.Text);

            if (!string.IsNullOrWhiteSpace(
                    txtReferenceID.Text) &&
                !referenceId.HasValue)
            {
                UIHelper.ShowWarning(
                    "رقم المرجع يجب أن يكون رقمًا صحيحًا موجبًا.");

                txtReferenceID.Focus();

                return false;
            }

            int? selectedStudentId =
                GetSelectedStudentId();

            bool isManualStudentReceipt =
                cmbVoucherType.Text.Trim() == "قبض" &&
                selectedStudentId.HasValue;

            if (!isManualStudentReceipt &&
                !string.Equals(
                    cmbReferenceType.Text.Trim(),
                    "عام",
                    StringComparison.OrdinalIgnoreCase) &&
                !referenceId.HasValue)
            {
                UIHelper.ShowWarning(
                    "أدخل رقم المرجع عند اختيار نوع مرجع مرتبط مثل الرسوم أو المصروفات.");

                txtReferenceID.Focus();

                return false;
            }

            if (isManualStudentReceipt)
                cmbReferenceType.Text =
                    "رسوم";

            if (!isManualStudentReceipt &&
                cmbVoucherType.Text.Trim() == "صرف" &&
                string.Equals(
                    cmbReferenceType.Text.Trim(),
                    "رسوم",
                    StringComparison.OrdinalIgnoreCase))
            {
                UIHelper.ShowWarning(
                    "سند الصرف لا يرتبط برسوم الطالب. استخدم سند القبض لتحصيل الرسوم.");

                cmbReferenceType.Focus();

                return false;
            }

            if (cmbVoucherType.Text.Trim() == "صرف" &&
                selectedStudentId.HasValue)
            {
                UIHelper.ShowWarning(
                    "سند الصرف مخصص لخروج الأموال، ولا يجوز تسجيله على طالب. اختر طرفاً مستفيداً مثل المورد أو المعلم.");

                cmbPartyName.Focus();

                return false;
            }

            if (string.Equals(
                    cmbReferenceType.Text.Trim(),
                    "عام",
                    StringComparison.OrdinalIgnoreCase) &&
                referenceId.HasValue)
            {
                UIHelper.ShowWarning(
                    "لا يمكن إدخال رقم مرجع مع النوع العام؛ اختر نوع المرجع الصحيح أولاً.");

                cmbReferenceType.Focus();

                return false;
            }

            if (chkIsAutoGenerated.Checked)
            {
                UIHelper.ShowWarning(
                    "السندات التلقائية ينشئها النظام من شاشة الرسوم أو المصروفات ولا تُضاف يدويًا.");

                chkIsAutoGenerated.Focus();

                return false;
            }

            return true;
        }

        // تعريف الدالة ReadDecimal لتنفيذ العملية المرتبطة بها.
        private decimal ReadDecimal(
            string text)
        {
            return UIHelper.TryParseDecimal(
                text,
                out decimal value)
                ? value
                : 0m;
        }

        // تعريف الدالة ReadNullableInt لتنفيذ العملية المرتبطة بها.
        private int? ReadNullableInt(
            string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return null;

            string normalized =
                text.Trim();

            if (int.TryParse(
                    normalized,
                    NumberStyles.Integer,
                    CultureInfo.CurrentCulture,
                    out int value) &&
                value > 0)
            {
                return value;
            }

            if (int.TryParse(
                    normalized,
                    NumberStyles.Integer,
                    CultureInfo.InvariantCulture,
                    out value) &&
                value > 0)
            {
                return value;
            }

            return null;
        }

        // تعريف الدالة GetVoucherFromInputs لتنفيذ العملية المرتبطة بها.
        private Voucher GetVoucherFromInputs()
        {
            return new Voucher
            {
                VoucherID = selectedVoucherId,

                StudentID =
                    GetSelectedStudentId(),

                VoucherNumber =
                    txtVoucherNumber.Text.Trim(),

                VoucherType =
                    cmbVoucherType.Text.Trim(),

                Amount =
                    ReadDecimal(txtAmount.Text),

                VoucherDate =
                    dtpVoucherDate.Value.Date,

                PartyName =
                    cmbPartyName.Text.Trim(),

                PartyType =
                    GetSelectedPartyType(),

                Description =
                    txtDescription.Text.Trim(),

                PaymentMethod =
                    cmbPaymentMethod.Text.Trim(),

                ReferenceType =
                    cmbReferenceType.Text.Trim(),

                ReferenceID =
                    ReadNullableInt(
                        txtReferenceID.Text),

                Notes =
                    txtNotes.Text.Trim(),

                IsAutoGenerated =
                    chkIsAutoGenerated.Checked
            };
        }

        // تعريف الدالة dataGridViewVouchers_CellClick لتنفيذ العملية المرتبطة بها.
        private void dataGridViewVouchers_CellClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 ||
                dataGridViewVouchers.Rows.Count == 0)
            {
                return;
            }

            DataRowView rowView =
                dataGridViewVouchers
                    .Rows[e.RowIndex]
                    .DataBoundItem as DataRowView;

            if (rowView != null)
                FillFieldsFromRow(rowView.Row);
        }

        // تعريف الدالة FillFieldsFromRow لتنفيذ العملية المرتبطة بها.
        private void FillFieldsFromRow(
            DataRow row)
        {
            if (row == null)
                return;

            selectedVoucherId =
                row["VoucherID"] ==
                    DBNull.Value ||
                !int.TryParse(
                    row["VoucherID"].ToString(),
                    out int voucherId)
                    ? 0
                    : voucherId;

            txtVoucherNumber.Text =
                row["VoucherNumber"] ==
                    DBNull.Value
                    ? ""
                    : row["VoucherNumber"]
                        .ToString();

            cmbVoucherType.Text =
                row["VoucherType"] ==
                    DBNull.Value
                    ? ""
                    : row["VoucherType"]
                        .ToString();

            decimal amount =
                row["Amount"] ==
                    DBNull.Value
                    ? 0m
                    : ReadDecimal(
                        row["Amount"]
                            .ToString());

            txtAmount.Text =
                amount.ToString("N2");

            if (row["VoucherDate"] != DBNull.Value &&
                DateTime.TryParse(
                    row["VoucherDate"].ToString(),
                    out DateTime voucherDate))
            {
                dtpVoucherDate.Value =
                    voucherDate <= DateTime.Today
                        ? voucherDate
                        : DateTime.Today;
            }

            cmbPartyName.SelectedIndex = -1;

            if (row.Table.Columns.Contains(
                    "PartyKey") &&
                row["PartyKey"] != DBNull.Value)
            {
                cmbPartyName.SelectedValue =
                    row["PartyKey"].ToString();
            }
            else
            {
                cmbPartyName.Text =
                    row["PartyName"] ==
                        DBNull.Value
                        ? ""
                        : row["PartyName"]
                            .ToString();
            }

            txtDescription.Text =
                row["Description"] ==
                    DBNull.Value
                    ? ""
                    : row["Description"]
                        .ToString();

            cmbPaymentMethod.Text =
                row["PaymentMethod"] ==
                    DBNull.Value
                    ? ""
                    : row["PaymentMethod"]
                        .ToString();

            cmbReferenceType.Text =
                row["ReferenceType"] ==
                    DBNull.Value
                    ? "عام"
                    : row["ReferenceType"]
                        .ToString();

            txtReferenceID.Text =
                row["ReferenceID"] ==
                    DBNull.Value
                    ? ""
                    : row["ReferenceID"]
                        .ToString();

            txtNotes.Text =
                row["Notes"] ==
                    DBNull.Value
                    ? ""
                    : row["Notes"]
                        .ToString();

            chkIsAutoGenerated.Checked =
                row["IsAutoGenerated"] !=
                    DBNull.Value &&
                Convert.ToBoolean(
                    row["IsAutoGenerated"]);

            chkIsAutoGenerated.Enabled =
                false;

            bool isStudentFeeReceipt =
                string.Equals(
                    cmbVoucherType.Text.Trim(),
                    "قبض",
                    StringComparison.OrdinalIgnoreCase) &&

                string.Equals(
                    cmbReferenceType.Text.Trim(),
                    "رسوم",
                    StringComparison.OrdinalIgnoreCase) &&

                row.Table.Columns.Contains(
                    "StudentID") &&

                row["StudentID"] !=
                    DBNull.Value;

currentVoucherProtected =
                chkIsAutoGenerated.Checked ||
                isStudentFeeReceipt;

            currentVoucherLinkedToStudentFee =
                isStudentFeeReceipt;

            txtAmount.ReadOnly =
                chkIsAutoGenerated.Checked ||
                currentVoucherLinkedToStudentFee;

            cmbVoucherType.Enabled =
                !chkIsAutoGenerated.Checked &&
                !currentVoucherLinkedToStudentFee;

            cmbReferenceType.Enabled =
                !chkIsAutoGenerated.Checked &&
                !currentVoucherLinkedToStudentFee;

            txtReferenceID.ReadOnly =
                chkIsAutoGenerated.Checked ||
                currentVoucherLinkedToStudentFee;

            btnUpdate.Enabled =
                true;

            btnDelete.Enabled =
                true;
        }

        // تعريف الدالة btnAdd_Click لتنفيذ العملية المرتبطة بها.
        private async void btnAdd_Click(
            object sender,
            EventArgs e)
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                CurrentUser.DemandAction(
                    "Vouchers",
                    "Add",
                    "ليس لديك صلاحية إضافة السندات.");

                if (!ValidateInputs())
                    return;

                Voucher voucher =
                    GetVoucherFromInputs();

                await Task.Run(
                    () =>
                        voucherService.AddVoucher(
                            voucher));

                UIHelper.ShowInfo(
                    "تمت إضافة السند بنجاح.");

                ApplicationLogger.LogUsage("شاشة السندات", "إضافة سند", "تمت إضافة السند بنجاح");

                await LoadVouchersAsync();

                ClearInputs();
            }
            catch (Exception ex)
            {
                UIHelper.ShowException(
                    "إضافة السند",
                    ex);
            }
        }

        // تعريف الدالة btnUpdate_Click لتنفيذ العملية المرتبطة بها.
        private async void btnUpdate_Click(
            object sender,
            EventArgs e)
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                CurrentUser.DemandAction(
                    "Vouchers",
                    "Edit",
                    "ليس لديك صلاحية تعديل السندات.");

                if (selectedVoucherId == 0)
                {
                    UIHelper.ShowWarning(
                        "اختر سنداً من الجدول أولاً.");

                    return;
                }

if (chkIsAutoGenerated.Checked)
                {
                    UIHelper.ShowWarning(
                        "السند التلقائي لا يمكن تعديله؛ استخدم سنداً عكسياً أو تسوية مالية.");

                    return;
                }

                if (!ValidateInputs())
                    return;

                Voucher voucher =
                    GetVoucherFromInputs();

                bool result =
                    await Task.Run(
                        () =>
                            voucherService.UpdateVoucher(
                                voucher));

                if (result)
                {
                    UIHelper.ShowInfo(
                        "تم تعديل السند بنجاح.");

                    ApplicationLogger.LogUsage("شاشة السندات", "تعديل سند", "تم تعديل السند بنجاح");
                }
                else
                {
                    UIHelper.ShowWarning(
                        "لم يتم العثور على السند أو لم يتم تعديله.");
                }

                await LoadVouchersAsync();

                ClearInputs();
            }
            catch (Exception ex)
            {
                UIHelper.ShowException(
                    "تعديل السند",
                    ex);
            }
        }

        // تعريف الدالة btnDelete_Click لتنفيذ العملية المرتبطة بها.
        private async void btnDelete_Click(
            object sender,
            EventArgs e)
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                CurrentUser.DemandAction(
                    "Vouchers",
                    "Delete",
                    "ليس لديك صلاحية حذف السندات.");

                if (selectedVoucherId == 0)
                {
                    UIHelper.ShowWarning(
                        "اختر سنداً من الجدول أولاً.");

                    return;
                }

if (chkIsAutoGenerated.Checked || currentVoucherLinkedToStudentFee)
                {
                    if (!UIHelper.ShowConfirmation(
                            currentVoucherLinkedToStudentFee
                                ? "هذا السند مرتبط برسوم الطالب ويمثل أثراً مالياً قائماً.\nسيتم إنشاء سند عكسي بنفس المبلغ وعكس أثره على الرسوم مع إبقاء السند الأصلي وحفظ الأثر التدقيقي.\nهل تريد المتابعة؟"
                                : "هذا السند تلقائي ويمثل أثراً مالياً قائماً.\nسيتم إنشاء سند عكسي بنفس المبلغ مع إبقاء السند الأصلي وحفظ الأثر التدقيقي دون حذف أي بيانات أو كسر العلاقات.\nهل تريد المتابعة؟",
                            "إلغاء السند"))
                    {
                        return;
                    }

                    int reversalId =
                        await Task.Run(
                            () =>
                                voucherService.CancelVoucher(
                                    selectedVoucherId));

                    UIHelper.ShowInfo(
                        "تم إنشاء السند العكسي رقم " +
                        reversalId +
                        " وإلغاء الأثر المالي للسند الأصلي مع حفظ التتبع.");
                    ApplicationLogger.LogUsage("شاشة السندات", "إلغاء سند", "تم إنشاء السند العكسي رقم " + reversalId);

                    await LoadVouchersAsync();

                    ClearInputs();

                    return;
                }

                if (!UIHelper.ShowConfirmation(
                        "هل تريد حذف السند المحدد؟",
                        "تأكيد الحذف"))
                {
                    return;
                }

                bool result =
                    await Task.Run(
                        () =>
                            voucherService.DeleteVoucher(
                                selectedVoucherId));

                if (result)
                {
                    UIHelper.ShowInfo(
                        "تم حذف السند بنجاح.");

                    ApplicationLogger.LogUsage("شاشة السندات", "حذف سند", "تم حذف السند بنجاح");
                }
                else
                {
                    UIHelper.ShowWarning(
                        "لم يتم العثور على السند أو لم يتم حذفه.");
                }

                await LoadVouchersAsync();

                ClearInputs();
            }
            catch (Exception ex)
            {
                UIHelper.ShowException(
                    "حذف السند",
                    ex);
            }
        }

        // تعريف الدالة btnClear_Click لتنفيذ العملية المرتبطة بها.
        private void btnClear_Click(
            object sender,
            EventArgs e)
        {
            ClearInputs();
        }
    }
}
