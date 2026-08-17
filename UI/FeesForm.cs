using System;
using System.Data;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;
using SchoolSystem.Models;
using SchoolSystem.Helpers;
using SchoolSystem.Services;
using SchoolSystem.Security;

namespace SchoolSystem.UI
{
    public partial class FeesForm : UserControl
    {
        private readonly FeeService feeService = new FeeService();
        private readonly StudentService studentService = new StudentService();
        private readonly VoucherService voucherService = new VoucherService();

        private int selectedFeeId = 0;
        private int? selectedFeePlanId = null;
        private decimal originalPaidAmount = 0;
        private DataTable allFees;
        private bool isLoading = false;
        private bool showOutstandingOnly = false;

        public FeesForm()
        {
            InitializeComponent();
            SchoolSystem.Helpers.UIHelper.ApplyStyle(this);
            txtSearch.TextChanged += (sender, e) => ApplyFilter();
            Dock = DockStyle.Fill;
            Load += FeesForm_Load;
        }

        private async void FeesForm_Load(object sender, EventArgs e)
        {
            try
            {
                isLoading = true;

                InitializeLookups();

                await LoadStudentsAsync();
                await LoadFeesAsync();

                ClearInputs();
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("حدث خطأ أثناء تحميل شاشة الرسوم:\n", ex);
            }
            finally
            {
                isLoading = false;
                CalculateAmounts();
            }
        }

        private void InitializeLookups()
        {
            cmbFeeType.Items.Clear();
            cmbFeeType.Items.AddRange(new object[]
            {
                "رسوم تسجيل",
                "رسوم دراسية",
                "رسوم كتب",
                "رسوم مواصلات",
                "رسوم زي مدرسي",
                "رسوم امتحانات",
                "رسوم أنشطة",
                "أخرى"
            });
            cmbFeeType.SelectedIndex = 1;

            cmbStatus.Items.Clear();
            cmbStatus.Items.AddRange(new object[]
            {
                "غير مسدد",
                "مسدد جزئياً",
                "مسدد",
                "متأخر",
                "مؤجل",
                "معفى"
            });
            cmbStatus.SelectedIndex = 0;

            cmbFilterStatus.Items.Clear();
            cmbFilterStatus.Items.AddRange(new object[]
            {
                "كل الحالات",
                "غير مسدد",
                "مسدد جزئياً",
                "مسدد",
                "متأخر",
                "مؤجل",
                "معفى"
            });
            cmbFilterStatus.SelectedIndex = 0;

            cmbPaymentMethod.Items.Clear();
            cmbPaymentMethod.Items.AddRange(new object[]
            {
                "نقداً",
                "حوالة",
                "شيك",
                "محفظة إلكترونية",
                "أخرى"
            });
            cmbPaymentMethod.SelectedIndex = 0;

            cmbAcademicYear.Items.Clear();

            int year = DateTime.Today.Year;
            cmbAcademicYear.Items.Add((year - 1) + " / " + year);
            cmbAcademicYear.Items.Add(year + " / " + (year + 1));
            cmbAcademicYear.Items.Add((year + 1) + " / " + (year + 2));
            cmbAcademicYear.SelectedIndex = 1;
        }

        private async Task LoadStudentsAsync()
        {
            DataTable students = await Task.Run(() => studentService.GetActiveStudents());

            cmbStudent.DataSource = students;
            cmbStudent.DisplayMember = "StudentName";
            cmbStudent.ValueMember = "StudentID";

            if (cmbStudent.Items.Count > 0)
                cmbStudent.SelectedIndex = 0;
        }

        private async Task LoadFeesAsync()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                allFees = await Task.Run(() => feeService.GetAllFees());

                ApplyFilter();
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void ApplyFilter()
        {
            if (allFees == null)
                return;

            DataView dv = allFees.DefaultView;

            string searchText = UIHelper.EscapeDataViewFilterValue(txtSearch.Text);
            string selectedStatus = cmbFilterStatus.SelectedItem == null
                ? "كل الحالات"
                : cmbFilterStatus.SelectedItem.ToString();

            string filter = "";

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                filter =
                    "(StudentName LIKE '%" + searchText + "%' " +
                    "OR AcademicYear LIKE '%" + searchText + "%' " +
                    "OR ClassName LIKE '%" + searchText + "%' " +
                    "OR FeeType LIKE '%" + searchText + "%' " +
                    "OR ReceiptNumber LIKE '%" + searchText + "%' " +
                    "OR Status LIKE '%" + searchText + "%')";
            }

            if (selectedStatus != "كل الحالات")
            {
                if (!string.IsNullOrWhiteSpace(filter))
                    filter += " AND ";

                filter += "Status = '" + UIHelper.EscapeDataViewFilterValue(selectedStatus) + "'";
            }

            if (showOutstandingOnly)
            {
                if (!string.IsNullOrWhiteSpace(filter))
                    filter += " AND ";

                filter += "RemainingAmount > 0";
            }

            dv.RowFilter = filter;

            dataGridViewFees.DataSource = dv;
            lblRecordCount.Text = (showOutstandingOnly ? "المتأخرات - " : "الرسوم - ") + "عدد السجلات: " + dv.Count;
            btnOutstandingReport.Text = showOutstandingOnly ? "عرض كل الرسوم" : "تقرير المتأخرات فقط";

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
            if (dataGridViewFees.Columns.Count == 0)
                return;

            HideColumn("FeeID");
            HideColumn("StudentID");
            HideColumn("FeePlanID");
            HideColumn("CreatedAt");
            HideColumn("UpdatedAt");

            SetHeader("StudentName", "الطالب");
            SetHeader("AcademicYear", "العام الدراسي");
            SetHeader("FeeType", "نوع الرسوم");
            SetHeader("TotalAmount", "الإجمالي");
            SetHeader("DiscountAmount", "الخصم");
            SetHeader("NetAmount", "الصافي");
            SetHeader("PaidAmount", "المدفوع");
            SetHeader("RemainingAmount", "المتبقي");
            SetHeader("DueDate", "الاستحقاق");
            SetHeader("PaymentDate", "تاريخ الدفع");
            SetHeader("PaymentMethod", "طريقة الدفع");
            SetHeader("ReceiptNumber", "رقم السند");
            SetHeader("Status", "الحالة");
            SetHeader("Notes", "ملاحظات");

            FormatMoney("TotalAmount");
            FormatMoney("DiscountAmount");
            FormatMoney("NetAmount");
            FormatMoney("PaidAmount");
            FormatMoney("RemainingAmount");

            FormatDate("DueDate");
            FormatDate("PaymentDate");
        }

        private void HideColumn(string columnName)
        {
            if (dataGridViewFees.Columns.Contains(columnName))
                dataGridViewFees.Columns[columnName].Visible = false;
        }

        private void SetHeader(string columnName, string headerText)
        {
            if (dataGridViewFees.Columns.Contains(columnName))
                dataGridViewFees.Columns[columnName].HeaderText = headerText;
        }

        private void FormatMoney(string columnName)
        {
            if (dataGridViewFees.Columns.Contains(columnName))
                dataGridViewFees.Columns[columnName].DefaultCellStyle.Format = "N2";
        }

        private void FormatDate(string columnName)
        {
            if (dataGridViewFees.Columns.Contains(columnName))
                dataGridViewFees.Columns[columnName].DefaultCellStyle.Format = "dd/MM/yyyy";
        }

        private void FilterControls_Changed(object sender, EventArgs e)
        {
            if (!isLoading)
                ApplyFilter();
        }

        private void AmountFields_TextChanged(object sender, EventArgs e)
        {
            if (!isLoading)
                CalculateAmounts();
        }

        private void CalculateAmounts()
        {
            decimal total = ReadDecimal(txtTotalAmount.Text);
            decimal discount = ReadDecimal(txtDiscountAmount.Text);
            decimal paid = ReadDecimal(txtPaidAmount.Text);

            if (discount > total)
                discount = total;

            decimal net = total - discount;
            decimal remaining = net - paid;

            if (remaining < 0)
                remaining = 0;

            txtNetAmount.Text = net.ToString("N2");
            txtRemainingAmount.Text = remaining.ToString("N2");

            lblSummary.Text =
                $"الإجمالي: {total:N2} | الخصم: {discount:N2} | الصافي: {net:N2} | المدفوع: {paid:N2} | المتبقي: {remaining:N2}";

            if (net == 0)
            {
                SetStatus("معفى");
                dtpPaymentDate.Checked = false;
            }
            else if (paid == 0)
            {
                SetStatus(dtpDueDate.Value.Date < DateTime.Today ? "متأخر" : "غير مسدد");
                dtpPaymentDate.Checked = false;
            }
            else if (paid >= net)
            {
                SetStatus("مسدد");
                dtpPaymentDate.Checked = true;
            }
            else
            {
                SetStatus("مسدد جزئياً");
                dtpPaymentDate.Checked = true;
            }
        }

        private void SetStatus(string status)
        {
            if (cmbStatus.Items.Contains(status))
                cmbStatus.SelectedItem = status;
        }

        private decimal ReadDecimal(string text)
        {
            if (decimal.TryParse(text, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal value))
                return value;

            if (decimal.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out value))
                return value;

            return 0;
        }

        private void dataGridViewFees_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dataGridViewFees.Rows.Count == 0)
                return;

            DataRowView rowView = dataGridViewFees.Rows[e.RowIndex].DataBoundItem as DataRowView;

            if (rowView != null)
                FillFieldsFromRow(rowView.Row);
        }

        private void FillFieldsFromRow(DataRow row)
        {
            isLoading = true;

            selectedFeeId = Convert.ToInt32(row["FeeID"]);

            selectedFeePlanId = null;
            if (row.Table.Columns.Contains("FeePlanID") && row["FeePlanID"] != DBNull.Value)
                selectedFeePlanId = Convert.ToInt32(row["FeePlanID"]);

            cmbStudent.SelectedValue = Convert.ToInt32(row["StudentID"]);
            cmbAcademicYear.Text = row["AcademicYear"].ToString();
            cmbFeeType.Text = row["FeeType"].ToString();

            txtTotalAmount.Text = Convert.ToDecimal(row["TotalAmount"]).ToString("N2");
            txtDiscountAmount.Text = Convert.ToDecimal(row["DiscountAmount"]).ToString("N2");
            txtNetAmount.Text = Convert.ToDecimal(row["NetAmount"]).ToString("N2");
            txtPaidAmount.Text = Convert.ToDecimal(row["PaidAmount"]).ToString("N2");
            txtRemainingAmount.Text = Convert.ToDecimal(row["RemainingAmount"]).ToString("N2");

            originalPaidAmount = Convert.ToDecimal(row["PaidAmount"]);

            dtpDueDate.Value = Convert.ToDateTime(row["DueDate"]);

            if (row["PaymentDate"] != DBNull.Value)
            {
                dtpPaymentDate.Checked = true;
                dtpPaymentDate.Value = Convert.ToDateTime(row["PaymentDate"]);
            }
            else
            {
                dtpPaymentDate.Checked = false;
            }

            cmbPaymentMethod.Text = row["PaymentMethod"] == DBNull.Value ? "نقداً" : row["PaymentMethod"].ToString();
            txtReceiptNumber.Text = row["ReceiptNumber"] == DBNull.Value ? "" : row["ReceiptNumber"].ToString();

            cmbStatus.Text = row["Status"].ToString();
            txtNotes.Text = row["Notes"] == DBNull.Value ? "" : row["Notes"].ToString();

            isLoading = false;

            CalculateAmounts();
        }

        private void ClearInputs()
        {
            isLoading = true;

            selectedFeeId = 0;
            selectedFeePlanId = null;
            originalPaidAmount = 0;

            if (cmbStudent.Items.Count > 0)
                cmbStudent.SelectedIndex = 0;

            if (cmbAcademicYear.Items.Count > 0)
                cmbAcademicYear.SelectedIndex = 1;

            if (cmbFeeType.Items.Count > 0)
                cmbFeeType.SelectedIndex = 1;

            txtTotalAmount.Text = "0";
            txtDiscountAmount.Text = "0";
            txtNetAmount.Text = "0";
            txtPaidAmount.Text = "0";
            txtRemainingAmount.Text = "0";

            dtpDueDate.Value = DateTime.Today.AddDays(30);

            dtpPaymentDate.Value = DateTime.Today;
            dtpPaymentDate.Checked = false;

            if (cmbPaymentMethod.Items.Count > 0)
                cmbPaymentMethod.SelectedIndex = 0;

            txtReceiptNumber.Clear();

            if (cmbStatus.Items.Count > 0)
                cmbStatus.SelectedIndex = 0;

            txtNotes.Clear();

            isLoading = false;

            CalculateAmounts();
        }

        private bool ValidateInputs()
        {
            if (cmbStudent.SelectedValue == null)
            {
                UIHelper.ShowWarning("يرجى اختيار الطالب.");
                cmbStudent.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(cmbAcademicYear.Text))
            {
                UIHelper.ShowWarning("يرجى اختيار العام الدراسي.");
                cmbAcademicYear.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(cmbFeeType.Text))
            {
                UIHelper.ShowWarning("يرجى اختيار نوع الرسوم.");
                cmbFeeType.Focus();
                return false;
            }

            decimal total;
            decimal discount;
            decimal paid;
            if (!UIHelper.TryParseDecimal(txtTotalAmount.Text, out total) ||
                !UIHelper.TryParseDecimal(txtDiscountAmount.Text, out discount) ||
                !UIHelper.TryParseDecimal(txtPaidAmount.Text, out paid))
            {
                UIHelper.ShowWarning("أدخل جميع المبالغ بصيغة رقمية صحيحة.");
                return false;
            }

            if (total < 0 || discount < 0 || paid < 0)
            {
                UIHelper.ShowWarning("لا يمكن إدخال مبالغ سالبة.");
                return false;
            }

            if (discount > total)
            {
                UIHelper.ShowWarning("الخصم لا يمكن أن يكون أكبر من إجمالي الرسوم.");
                txtDiscountAmount.Focus();
                return false;
            }

            if (paid > total - discount)
            {
                UIHelper.ShowWarning("المبلغ المدفوع لا يمكن أن يكون أكبر من صافي الرسوم.");
                txtPaidAmount.Focus();
                return false;
            }

            if (paid > 0 && string.IsNullOrWhiteSpace(txtReceiptNumber.Text))
            {
                UIHelper.ShowWarning("أدخل رقم السند عند تسجيل دفعة مالية.");
                txtReceiptNumber.Focus();
                return false;
            }

            if (dtpPaymentDate.Checked && dtpPaymentDate.Value.Date > DateTime.Today)
            {
                UIHelper.ShowWarning("لا يمكن تسجيل دفعة بتاريخ مستقبلي.");
                dtpPaymentDate.Focus();
                return false;
            }

            return true;
        }

        private Fee GetFeeFromInputs()
        {
            CalculateAmounts();

            return new Fee
            {
                FeeID = selectedFeeId,
                StudentID = Convert.ToInt32(cmbStudent.SelectedValue),
                StudentName = cmbStudent.Text,

                FeePlanID = selectedFeePlanId,

                AcademicYear = cmbAcademicYear.Text.Trim(),
                FeeType = cmbFeeType.Text.Trim(),

                TotalAmount = ReadDecimal(txtTotalAmount.Text),
                DiscountAmount = ReadDecimal(txtDiscountAmount.Text),
                NetAmount = ReadDecimal(txtNetAmount.Text),
                PaidAmount = ReadDecimal(txtPaidAmount.Text),
                RemainingAmount = ReadDecimal(txtRemainingAmount.Text),

                DueDate = dtpDueDate.Value.Date,
                PaymentDate = dtpPaymentDate.Checked ? dtpPaymentDate.Value.Date : (DateTime?)null,

                PaymentMethod = cmbPaymentMethod.Text.Trim(),
                ReceiptNumber = txtReceiptNumber.Text.Trim(),

                Status = cmbStatus.Text.Trim(),
                Notes = txtNotes.Text.Trim()
            };
        }

        private async Task CreateReceiptVoucherIfNeededAsync(int feeId, decimal paymentAmount, string studentName, string paymentMethod)
        {
            if (paymentAmount <= 0)
                return;

            DateTime paymentDate = dtpPaymentDate.Checked ? dtpPaymentDate.Value.Date : DateTime.Today;

            string notes =
                "تم إنشاء سند قبض تلقائياً من شاشة الرسوم الدراسية." + Environment.NewLine +
                "نوع الرسوم: " + cmbFeeType.Text + Environment.NewLine +
                "العام الدراسي: " + cmbAcademicYear.Text;

            await Task.Run(() =>
                voucherService.CreateReceiptVoucherForFeePayment(
                    paymentAmount,
                    paymentDate,
                    studentName,
                    feeId,
                    paymentMethod,
                    notes
                )
            );
        }

        private async void btnGenerateFees_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbStudent.SelectedValue == null)
                {
                    UIHelper.ShowWarning("اختر الطالب أولاً.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(cmbAcademicYear.Text))
                {
                    UIHelper.ShowWarning("اختر العام الدراسي.");
                    return;
                }

                int studentId = Convert.ToInt32(cmbStudent.SelectedValue);
                string academicYear = cmbAcademicYear.Text.Trim();

                int count = await Task.Run(() =>
                    feeService.GenerateStudentFeesFromPlans(studentId, academicYear)
                );

                UIHelper.ShowInfo("تم توليد عدد " + count + " رسوم للطالب.");

                await LoadFeesAsync();
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("فشل توليد رسوم الطالب:\n", ex);
            }
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateInputs())
                    return;

                Fee fee = GetFeeFromInputs();

                int newFeeId = await Task.Run(() => feeService.AddFee(fee));

                decimal paidAmount = fee.PaidAmount;

                if (paidAmount > 0)
                {
                    await CreateReceiptVoucherIfNeededAsync(
                        newFeeId,
                        paidAmount,
                        cmbStudent.Text,
                        cmbPaymentMethod.Text
                    );
                }

                UIHelper.ShowInfo("تمت إضافة الرسوم بنجاح.");

                await LoadFeesAsync();
                ClearInputs();
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("فشل إضافة الرسوم:\n", ex);
            }
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedFeeId == 0)
                {
                    UIHelper.ShowWarning("اختر سجل الرسوم من الجدول أولاً.");
                    return;
                }

                if (!ValidateInputs())
                    return;

                Fee fee = GetFeeFromInputs();

                decimal newPaidAmount = fee.PaidAmount;
                decimal paymentDifference = newPaidAmount - originalPaidAmount;

                bool result = await Task.Run(() => feeService.UpdateFee(fee));

                if (result && paymentDifference > 0)
                {
                    await CreateReceiptVoucherIfNeededAsync(
                        selectedFeeId,
                        paymentDifference,
                        cmbStudent.Text,
                        cmbPaymentMethod.Text
                    );
                }

                if (result)
                    UIHelper.ShowInfo("تم تعديل الرسوم بنجاح.");
                else
                    UIHelper.ShowWarning("لم يتم تعديل الرسوم؛ ربما تغير السجل أو لم يعد موجودًا.");

                await LoadFeesAsync();
                ClearInputs();
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("فشل تعديل الرسوم:\n", ex);
            }
        }

        private async void btnRecordPayment_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedFeeId <= 0)
                {
                    UIHelper.ShowWarning("اختر سجل الرسوم من الجدول أولاً.");
                    return;
                }

                decimal newTotalPaid = ReadDecimal(txtPaidAmount.Text);
                decimal paymentAmount = newTotalPaid - originalPaidAmount;
                decimal remainingBeforePayment = ReadDecimal(txtNetAmount.Text) - originalPaidAmount;
                if (remainingBeforePayment < 0)
                    remainingBeforePayment = 0;

                if (paymentAmount <= 0)
                {
                    UIHelper.ShowWarning("أدخل إجمالي المدفوع الجديد في حقل المدفوع، ويجب أن يكون أكبر من المدفوع السابق.");
                    txtPaidAmount.Focus();
                    return;
                }

                if (paymentAmount > remainingBeforePayment)
                {
                    UIHelper.ShowWarning("مبلغ الدفعة يتجاوز المتبقي على الرسوم.");
                    txtPaidAmount.Focus();
                    return;
                }

                if (!dtpPaymentDate.Checked)
                    dtpPaymentDate.Checked = true;

                if (string.IsNullOrWhiteSpace(txtReceiptNumber.Text))
                {
                    UIHelper.ShowWarning("رقم السند مطلوب قبل تسجيل دفعة مستقلة.");
                    txtReceiptNumber.Focus();
                    return;
                }

                DataTable result = await Task.Run(() => feeService.RecordPayment(
                    selectedFeeId,
                    paymentAmount,
                    dtpPaymentDate.Value.Date,
                    cmbPaymentMethod.Text,
                    txtReceiptNumber.Text,
                    txtNotes.Text));

                DataRow row = result.Rows[0];
                await Task.Run(() => voucherService.CreateReceiptVoucherForFeePayment(
                    paymentAmount,
                    dtpPaymentDate.Value.Date,
                    cmbStudent.Text,
                    selectedFeeId,
                    cmbPaymentMethod.Text,
                    "دفعة مستقلة من شاشة الرسوم الدراسية.",
                    "رسوم دفعة"));

                UIHelper.ShowInfo("تم تسجيل الدفعة وإنشاء سند القبض بنجاح. المتبقي الجديد: " +
                    Convert.ToDecimal(row["RemainingAmount"]).ToString("N2"));

                await LoadFeesAsync();
                ClearInputs();
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("فشل تسجيل الدفعة:", ex);
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedFeeId == 0)
                {
                    UIHelper.ShowWarning("اختر سجل الرسوم من الجدول أولاً.");
                    return;
                }

                DialogResult confirm = MessageBox.Show(
                    "هل أنت متأكد من حذف سجل الرسوم المحدد؟",
                    "تأكيد الحذف",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2,
                    MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);

                if (confirm != DialogResult.Yes)
                    return;

                bool result = await Task.Run(() => feeService.DeleteFee(selectedFeeId));

                if (result)
                    UIHelper.ShowInfo("تم حذف الرسوم بنجاح.");
                else
                    UIHelper.ShowWarning("لم يتم حذف الرسوم؛ ربما تغير السجل أو لم يعد موجودًا.");

                await LoadFeesAsync();
                ClearInputs();
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("فشل حذف الرسوم:\n", ex);
            }
        }

        private DataTable GetVisibleFeesForReport()
        {
            if (allFees == null)
                return null;

            DataView view = allFees.DefaultView;
            DataTable report = view.ToTable();

            if (report.Columns.Contains("FeeID"))
                report.Columns.Remove("FeeID");
            if (report.Columns.Contains("StudentID"))
                report.Columns.Remove("StudentID");
            if (report.Columns.Contains("FeePlanID"))
                report.Columns.Remove("FeePlanID");
            if (report.Columns.Contains("CreatedAt"))
                report.Columns.Remove("CreatedAt");
            if (report.Columns.Contains("UpdatedAt"))
                report.Columns.Remove("UpdatedAt");

            RenameReportColumn(report, "StudentName", "الطالب");
            RenameReportColumn(report, "ClassName", "الصف");
            RenameReportColumn(report, "AcademicYear", "العام الدراسي");
            RenameReportColumn(report, "FeeType", "نوع الرسوم");
            RenameReportColumn(report, "TotalAmount", "الإجمالي");
            RenameReportColumn(report, "DiscountAmount", "الخصم");
            RenameReportColumn(report, "NetAmount", "الصافي");
            RenameReportColumn(report, "PaidAmount", "المدفوع");
            RenameReportColumn(report, "RemainingAmount", "المتبقي");
            RenameReportColumn(report, "DueDate", "تاريخ الاستحقاق");
            RenameReportColumn(report, "PaymentDate", "تاريخ الدفع");
            RenameReportColumn(report, "PaymentMethod", "طريقة الدفع");
            RenameReportColumn(report, "ReceiptNumber", "رقم السند");
            RenameReportColumn(report, "Status", "الحالة");
            RenameReportColumn(report, "Notes", "ملاحظات");
            return report;
        }

        private void RenameReportColumn(DataTable table, string source, string target)
        {
            if (table.Columns.Contains(source))
                table.Columns[source].ColumnName = target;
        }

        private void btnOutstandingReport_Click(object sender, EventArgs e)
        {
            showOutstandingOnly = !showOutstandingOnly;
            ApplyFilter();
        }

        private string GetFeeReportTitle()
        {
            return showOutstandingOnly
                ? "تقرير المتأخرات المالية للطلاب"
                : "تقرير الرسوم الدراسية والتحصيل";
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentUser.DemandAction("Fees", "ExportExcel", "ليس لديك صلاحية تصدير تقارير الرسوم إلى Excel.");

                DataTable report = GetVisibleFeesForReport();
                if (report == null || report.Rows.Count == 0)
                {
                    UIHelper.ShowWarning("لا توجد رسوم ظاهرة للتصدير.");
                    return;
                }

                using (SaveFileDialog dialog = new SaveFileDialog())
                {
                    dialog.Filter = "Excel Workbook (*.xlsx)|*.xlsx";
                    dialog.FileName = (showOutstandingOnly ? "تقرير_المتأخرات_" : "تقرير_الرسوم_") + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".xlsx";
                    if (dialog.ShowDialog() != DialogResult.OK)
                        return;

                    ReportOutputHelper.ExportToExcel(
                        report,
                        dialog.FileName,
                        GetFeeReportTitle(),
                        "عدد السجلات: " + report.Rows.Count + (showOutstandingOnly ? " | يعرض الأرصدة المتبقية فقط" : ""));
                    UIHelper.ShowInfo("تم تصدير تقرير الرسوم إلى Excel بنجاح.");
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("فشل تصدير تقرير الرسوم إلى Excel:\n", ex);
            }
        }

        private void btnExportPdf_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentUser.DemandAction("Fees", "ExportPDF", "ليس لديك صلاحية تصدير تقارير الرسوم إلى PDF.");

                DataTable report = GetVisibleFeesForReport();
                if (report == null || report.Rows.Count == 0)
                {
                    UIHelper.ShowWarning("لا توجد رسوم ظاهرة للتصدير.");
                    return;
                }

                using (SaveFileDialog dialog = new SaveFileDialog())
                {
                    dialog.Filter = "PDF Document (*.pdf)|*.pdf";
                    dialog.FileName = (showOutstandingOnly ? "تقرير_المتأخرات_" : "تقرير_الرسوم_") + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".pdf";
                    if (dialog.ShowDialog() != DialogResult.OK)
                        return;

                    ReportOutputHelper.ExportToPdf(
                        report,
                        dialog.FileName,
                        GetFeeReportTitle(),
                        "عدد السجلات: " + report.Rows.Count + (showOutstandingOnly ? " | يعرض الأرصدة المتبقية فقط" : ""));
                    UIHelper.ShowInfo("تم تصدير تقرير الرسوم إلى PDF بنجاح.");
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("فشل تصدير تقرير الرسوم إلى PDF:\n", ex);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearInputs();
        }
    }
}
