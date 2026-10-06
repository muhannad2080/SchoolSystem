using System;
using System.Data;
using System.Globalization;
using System.Drawing;
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
        // ============================================================
        // SERVICES
        // ============================================================

        private readonly FeeService feeService = new FeeService();
        private readonly StudentService studentService = new StudentService();
        private readonly VoucherService voucherService = new VoucherService();
        private readonly FeePlanService feePlanService = new FeePlanService();
        private readonly AcademicReferenceRepository academicReferenceRepository = new AcademicReferenceRepository();

        // ============================================================
        // STATE
        // ============================================================

        private int selectedFeeId = 0;

        private int? selectedFeePlanId = null;

        private string selectedPlanClassName = "";

        private string studentFinancialSummary = "";

        private decimal originalPaidAmount = 0;

        private string originalPaymentMethod = "";
        private string originalReceiptNumber = "";
        private DateTime? originalPaymentDate = null;

        private DataTable allFees;

        private bool isLoading = false;

        private bool isSaving = false;

        private bool showOutstandingOnly = false;

        // ============================================================
        // CONSTRUCTOR
        // ============================================================

        public FeesForm()
        {
            InitializeComponent();

            UIHelper.ApplyStyle(this);
            Dock = DockStyle.Fill;

            ApplyProfessionalLayout();

            // التحقق والسلوك الخاصان بشاشة الرسوم.

            // تطبيق قواعد التحقق من الحقول (فلترة الأحرف، أقصى طول، والتحقق عند
            // المغادرة) دون تغيير تنسيق المصمم لهذه الشاشة.
            SchoolSystem.Helpers.UIHelper.ApplyFieldValidation(this);

            txtSearch.TextChanged += (sender, e) => ApplyFilter();

            DataRefreshNotifier.DataChanged += DataRefreshNotifier_DataChanged;
            Disposed += (sender, e) =>
                DataRefreshNotifier.DataChanged -= DataRefreshNotifier_DataChanged;

            cmbStudent.SelectedIndexChanged += async (sender, e) =>
                await ApplySelectedFeePlanAsync();

            cmbAcademicYear.SelectedIndexChanged += async (sender, e) =>
                await ApplySelectedFeePlanAsync();

            cmbFeeType.SelectedIndexChanged += async (sender, e) =>
                await ApplySelectedFeePlanAsync();

            Load += FeesForm_Load;
        }

        private async void DataRefreshNotifier_DataChanged(
            object sender,
            DataRefreshEventArgs e)
        {
            if (ReferenceEquals(sender, this) ||
                IsDisposed ||
                (!DataRefreshNotifier.IsRelated(e, "Fees") &&
                 !DataRefreshNotifier.IsRelated(e, "Vouchers") &&
                 !DataRefreshNotifier.IsRelated(e, "Enrollment") &&
                 !DataRefreshNotifier.IsRelated(e, "Classes") &&
                 !DataRefreshNotifier.IsRelated(e, "Sections") &&
                 !DataRefreshNotifier.IsRelated(e, "FeePlans") &&
                 !DataRefreshNotifier.IsRelated(e, "All")))
            {
                return;
            }

            await LoadAcademicYearsAsync();
            await LoadFeeTypesAsync();
            await LoadStudentsAsync();
            await LoadFeesAsync();
            await ApplySelectedFeePlanAsync();
        }

        // ============================================================
        // FORM LOAD
        // ============================================================

        private async void FeesForm_Load(object sender, EventArgs e)
        {
            try
            {
                isLoading = true;

                InitializeLookups();

                await LoadAcademicYearsAsync();
                await LoadFeeTypesAsync();

                await LoadStudentsAsync();

                await LoadFeesAsync();

                ClearInputs();

                await ApplySelectedFeePlanAsync();
            }
            catch (Exception ex)
            {
                UIHelper.ShowException(
                    "حدث خطأ أثناء تحميل شاشة الرسوم:\n",
                    ex);
            }
            finally
            {
                isLoading = false;
                CalculateAmounts();
            }
        }

        // ============================================================
        // PROFESSIONAL LAYOUT
        // ============================================================

        private void ApplyProfessionalLayout()
        {
            RightToLeft = RightToLeft.Yes;
            Dock = DockStyle.Fill;
            MinimumSize = new System.Drawing.Size(1000, 650);

            // ارتفاعات ثابتة للمناطق المهمة، والجدول يأخذ المساحة المتبقية.
            rootLayout.RowStyles.Clear();
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 218F));
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));

            tableFields.Padding = new Padding(8, 4, 8, 4);

            foreach (Control control in tableFields.Controls)
            {
                if (control is Label label)
                {
                    label.TextAlign = ContentAlignment.MiddleRight;
                    label.Margin = new Padding(0, 0, 6, 0);
                }
                else if (control is TextBox textBox)
                {
                    textBox.RightToLeft = RightToLeft.Yes;
                    textBox.TextAlign = HorizontalAlignment.Right;
                    textBox.Margin = new Padding(0, 4, 8, 4);
                }
                else if (control is ComboBox comboBox)
                {
                    comboBox.RightToLeft = RightToLeft.Yes;
                    comboBox.Margin = new Padding(0, 4, 8, 4);
                }
                else if (control is DateTimePicker datePicker)
                {
                    datePicker.RightToLeft = RightToLeft.Yes;
                    datePicker.RightToLeftLayout = true;
                    datePicker.Margin = new Padding(0, 4, 8, 4);
                }
            }

            panelButtons.FlowDirection = FlowDirection.RightToLeft;
            panelButtons.WrapContents = false;
            panelButtons.AutoScroll = true;
            panelButtons.Padding = new Padding(12, 6, 12, 6);

            Control[] buttons =
            {
                btnAdd, btnUpdate, btnDelete, btnClear,
                btnGenerateFees, btnRecordPayment,
                btnOutstandingReport, btnExportExcel, btnExportPdf
            };

            panelButtons.Controls.Clear();
            panelButtons.Controls.AddRange(buttons);

            int[] widths = { 92, 92, 88, 88, 118, 120, 126, 112, 104 };
            for (int i = 0; i < buttons.Length; i++)
            {
                Button button = buttons[i] as Button;
                if (button == null)
                    continue;

                button.AutoSize = false;
                button.Width = widths[i];
                button.Height = 38;
                button.Margin = new Padding(4, 3, 4, 3);
                button.TextAlign = ContentAlignment.MiddleCenter;
                button.RightToLeft = RightToLeft.Yes;
            }

            panelSearch.Padding = new Padding(16, 7, 16, 7);

            txtSearch.RightToLeft = RightToLeft.Yes;
            txtSearch.TextAlign = HorizontalAlignment.Right;
            cmbFilterStatus.RightToLeft = RightToLeft.Yes;

            dataGridViewFees.RightToLeft = RightToLeft.Yes;
            dataGridViewFees.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewFees.AutoSizeRowsMode =
                DataGridViewAutoSizeRowsMode.None;
            dataGridViewFees.AllowUserToOrderColumns = true;
            dataGridViewFees.AllowUserToResizeColumns = true;
            dataGridViewFees.AllowUserToResizeRows = false;
            dataGridViewFees.RowHeadersVisible = false;
            dataGridViewFees.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dataGridViewFees.MultiSelect = false;
            dataGridViewFees.ReadOnly = true;
            dataGridViewFees.ColumnHeadersHeight = 40;
            dataGridViewFees.RowTemplate.Height = 36;
            dataGridViewFees.DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleRight;
            dataGridViewFees.ColumnHeadersDefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;
        }

        // ============================================================
        // LOOKUPS
        // ============================================================

        private void InitializeLookups()
        {
            cmbFeeType.Items.Clear();

            cmbStatus.Items.Clear();
            cmbStatus.Items.AddRange(new object[]
            {
                "غير مسدد",
                "مسدد جزئياً",
                "مسدد",
                "متأخر",
                "مؤجل",
                "معفى",
                "رصيد دائن"
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
                "معفى",
                "رصيد دائن"
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
        }

        private async Task LoadAcademicYearsAsync()
        {
            DataTable years = await Task.Run(() => academicReferenceRepository.GetAcademicYears());
            string current = SchoolSystem.Helpers.AcademicYearHelper.GetCurrentAcademicYear();

            cmbAcademicYear.Items.Clear();

            foreach (DataRow row in years.Rows)
            {
                string year = Convert.ToString(row["AcademicYear"]).Trim();
                if (!string.IsNullOrWhiteSpace(year) && !cmbAcademicYear.Items.Contains(year))
                    cmbAcademicYear.Items.Add(year);
            }

            if (cmbAcademicYear.Items.Count == 0)
            {
                cmbAcademicYear.SelectedIndex = -1;
                cmbAcademicYear.Text = string.Empty;
                return;
            }

            int currentIndex = cmbAcademicYear.Items.IndexOf(current);
            cmbAcademicYear.SelectedIndex = currentIndex >= 0 ? currentIndex : 0;
        }

        // ============================================================
        // LOAD FEE TYPES
        // ============================================================

        private async Task LoadFeeTypesAsync()
        {
            DataTable feeTypes =
                await Task.Run(() => feePlanService.GetFeeTypes());

            cmbFeeType.Items.Clear();

            foreach (DataRow row in feeTypes.Rows)
            {
                string feeType =
                    Convert.ToString(row["FeeType"]);

                if (!string.IsNullOrWhiteSpace(feeType))
                    cmbFeeType.Items.Add(feeType.Trim());
            }

            // لا نعرض أي نوع رسوم غير موجود في قاعدة البيانات.
            // الحقل قابل للكتابة لإدخال نوع جديد عند إنشاء خطة/رسوم فعلية.
        }

        // ============================================================
        // LOAD STUDENTS
        // ============================================================

        private async Task LoadStudentsAsync()
        {
            DataTable students =
                await Task.Run(() =>
                    studentService.GetActiveStudents());

            cmbStudent.DataSource = students;
            cmbStudent.DisplayMember = "StudentName";
            cmbStudent.ValueMember = "StudentId";

            if (cmbStudent.Items.Count > 0)
                cmbStudent.SelectedIndex = 0;
        }

        // ============================================================
        // APPLY SELECTED FEE PLAN
        // ============================================================

        private async Task ApplySelectedFeePlanAsync()
        {
            if (isLoading ||
                selectedFeeId != 0 ||
                cmbStudent.SelectedValue == null ||
                string.IsNullOrWhiteSpace(cmbAcademicYear.Text) ||
                string.IsNullOrWhiteSpace(cmbFeeType.Text))
            {
                return;
            }

            int studentId;

            if (!int.TryParse(
                    Convert.ToString(cmbStudent.SelectedValue),
                    out studentId) ||
                studentId <= 0)
            {
                return;
            }

            try
            {
                selectedFeePlanId = null;
                selectedPlanClassName = "";
                studentFinancialSummary = "";

                txtTotalAmount.ReadOnly = false;
                txtTotalAmount.Text = "0";
                txtNotes.Clear();

                // قراءة القيم من الواجهة قبل تشغيل العمليات الخلفية.
                string academicYear =
                    cmbAcademicYear.Text.Trim();

                string feeType =
                    cmbFeeType.Text.Trim();

                DataTable financialSummary =
                    await Task.Run(() =>
                        feeService.GetStudentFinancialSummary(
                            studentId,
                            academicYear));

                studentFinancialSummary =
                    BuildStudentFinancialSummary(financialSummary);

                FeePlan plan =
                    await Task.Run(() =>
                        feePlanService.GetStudentFeePlan(
                            studentId,
                            academicYear,
                            feeType));

                if (plan == null)
                {
                    txtTotalAmount.ReadOnly = false;

                    dtpDueDate.Value =
                        DateTime.Today.AddDays(30);

                    CalculateAmounts();

                    return;
                }

                selectedFeePlanId =
                    plan.FeePlanID;

                selectedPlanClassName =
                    plan.ClassName ?? "";

                txtTotalAmount.ReadOnly = true;

                txtTotalAmount.Text =
                    plan.Amount.ToString("N2");

                dtpDueDate.Value =
                    plan.DueDate < DateTime.Today
                        ? DateTime.Today
                        : plan.DueDate;

                if (!string.IsNullOrWhiteSpace(plan.Notes))
                    txtNotes.Text = plan.Notes;

                CalculateAmounts();
            }
            catch (Exception ex)
            {
                if (!isLoading)
                {
                    UIHelper.ShowException(
                        "تعذر جلب خطة الرسوم المطابقة للطالب والصف:\n",
                        ex);
                }
            }
        }

        // ============================================================
        // LOAD FEES
        // ============================================================

        private async Task LoadFeesAsync()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                allFees =
                    await Task.Run(() =>
                        feeService.GetAllFees());

                ApplyFilter();
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        // ============================================================
        // FILTER
        // ============================================================

        private void ApplyFilter()
        {
            if (allFees == null)
                return;

            DataView dv =
                allFees.DefaultView;

            string searchText =
                UIHelper.EscapeDataViewFilterValue(
                    txtSearch.Text);

            string selectedStatus =
                cmbFilterStatus.SelectedItem == null
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

                filter +=
                    "Status = '" +
                    UIHelper.EscapeDataViewFilterValue(
                        selectedStatus) +
                    "'";
            }

            if (showOutstandingOnly)
            {
                if (!string.IsNullOrWhiteSpace(filter))
                    filter += " AND ";

                filter += "RemainingAmount > 0";
            }

            dv.RowFilter = filter;

            dataGridViewFees.DataSource = dv;

            lblRecordCount.Text =
                (showOutstandingOnly
                    ? "المتأخرات - "
                    : "الرسوم - ")
                + "عدد السجلات: "
                + dv.Count;

            btnOutstandingReport.Text =
                showOutstandingOnly
                    ? "عرض كل الرسوم"
                    : "تقرير المتأخرات فقط";

            FormatGrid();
        }

        // ============================================================
        // FILTER ESCAPE
        // ============================================================

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

        // ============================================================
        // GRID
        // ============================================================

        private void FormatGrid()
        {
            if (dataGridViewFees.Columns.Count == 0)
                return;

            HideColumn("FeeID");
            HideColumn("StudentID");
            HideColumn("FeePlanID");
            HideColumn("CreatedAt");
            HideColumn("UpdatedAt");

            // أعمدة الملخص المالي للطالب تُعرض في الشريط العلوي،
            // لذلك لا نكررها داخل جدول البنود لتبقى القراءة واضحة.
            HideColumn("StudentTotalNetAmount");
            HideColumn("StudentTotalPaidAmount");
            HideColumn("StudentTotalRemainingAmount");
            HideColumn("StudentUnpaidFeeCount");
            HideColumn("Notes");

            SetHeader("StudentName", "الطالب");
            SetHeader("AcademicYear", "العام الدراسي");
            SetHeader("ClassName", "الصف الفعلي");
            SetHeader("Section", "الشعبة الفعلية");
            SetHeader("FeeType", "نوع الرسوم");
            SetHeader("TotalAmount", "الإجمالي");
            SetHeader("DiscountAmount", "الخصم");
            SetHeader("NetAmount", "الصافي");
            SetHeader("PaidAmount", "المدفوع");
            SetHeader("RemainingAmount", "المتبقي للبند");
            SetHeader("StudentTotalNetAmount", "إجمالي رسوم الطالب");
            SetHeader("StudentTotalPaidAmount", "إجمالي المدفوع");
            SetHeader("StudentTotalRemainingAmount", "المتبقي من كل الرسوم");
            SetHeader("StudentUnpaidFeeCount", "بنود غير مسددة");
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
            FormatMoney("StudentTotalNetAmount");
            FormatMoney("StudentTotalPaidAmount");
            FormatMoney("StudentTotalRemainingAmount");

            FormatDate("DueDate");
            FormatDate("PaymentDate");

            ConfigureFeeColumn("StudentName", 175, 150);
            ConfigureFeeColumn("AcademicYear", 100, 90);
            ConfigureFeeColumn("ClassName", 105, 95);
            ConfigureFeeColumn("Section", 95, 85);
            ConfigureFeeColumn("FeeType", 125, 105);
            ConfigureFeeColumn("TotalAmount", 95, 85);
            ConfigureFeeColumn("DiscountAmount", 85, 75);
            ConfigureFeeColumn("NetAmount", 95, 85);
            ConfigureFeeColumn("PaidAmount", 95, 85);
            ConfigureFeeColumn("RemainingAmount", 105, 90);
            ConfigureFeeColumn("DueDate", 100, 90);
            ConfigureFeeColumn("PaymentDate", 105, 90);
            ConfigureFeeColumn("PaymentMethod", 105, 90);
            ConfigureFeeColumn("ReceiptNumber", 115, 100);
            ConfigureFeeColumn("Status", 100, 90);
        }

        private void ConfigureFeeColumn(
            string columnName,
            int width,
            int minimumWidth)
        {
            if (!dataGridViewFees.Columns.Contains(columnName))
                return;

            DataGridViewColumn column =
                dataGridViewFees.Columns[columnName];

            column.AutoSizeMode =
                DataGridViewAutoSizeColumnMode.Fill;
            column.FillWeight = Math.Max(1, width);
            column.MinimumWidth = minimumWidth;
            column.SortMode =
                DataGridViewColumnSortMode.Automatic;
        }

        private void HideColumn(string columnName)
        {
            if (dataGridViewFees.Columns.Contains(columnName))
                dataGridViewFees.Columns[columnName].Visible = false;
        }

        private void SetHeader(
            string columnName,
            string headerText)
        {
            if (dataGridViewFees.Columns.Contains(columnName))
                dataGridViewFees.Columns[columnName].HeaderText =
                    headerText;
        }

        private void FormatMoney(string columnName)
        {
            if (dataGridViewFees.Columns.Contains(columnName))
                dataGridViewFees.Columns[columnName]
                    .DefaultCellStyle
                    .Format = "N2";
        }

        private void FormatDate(string columnName)
        {
            if (dataGridViewFees.Columns.Contains(columnName))
                dataGridViewFees.Columns[columnName]
                    .DefaultCellStyle
                    .Format = "dd/MM/yyyy";
        }

        // ============================================================
        // EVENTS
        // ============================================================

        private void FilterControls_Changed(
            object sender,
            EventArgs e)
        {
            if (!isLoading)
                ApplyFilter();
        }

        private void AmountFields_TextChanged(
            object sender,
            EventArgs e)
        {
            if (!isLoading)
                CalculateAmounts();
        }

        // ============================================================
        // FINANCIAL SUMMARY
        // ============================================================

        private string BuildStudentFinancialSummary(
            DataTable table)
        {
            if (table == null ||
                table.Rows.Count == 0)
            {
                return "إجمالي رسوم الطالب: لا توجد بنود رسوم";
            }

            DataRow row =
                table.Rows[0];

            decimal total =
                row.Field<decimal>(
                    "TotalNetAmount");

            decimal paid =
                row.Field<decimal>(
                    "TotalPaidAmount");

            decimal remaining =
                row.Field<decimal>(
                    "TotalRemainingAmount");

            int unpaidCount =
                row.Field<int>(
                    "UnpaidFeeCount");

            string unpaidTypes =
                Convert.ToString(
                    row["UnpaidFeeTypes"]);

            string balanceLabel =
                remaining < 0
                    ? "الرصيد الدائن للطالب"
                    : "المتبقي من كل الرسوم";

            return
                "إجمالي رسوم الطالب: "
                + total.ToString("N2")
                + " | المدفوع من الكل: "
                + paid.ToString("N2")
                + " | "
                + balanceLabel
                + ": "
                + remaining.ToString("N2")
                + " | البنود غير المسددة: "
                + unpaidCount
                + (string.IsNullOrWhiteSpace(unpaidTypes)
                    ? ""
                    : " (" + unpaidTypes + ")");
        }

        // ============================================================
        // CALCULATE AMOUNTS
        // ============================================================

        private void CalculateAmounts()
        {
            decimal total =
                ReadDecimal(txtTotalAmount.Text);

            decimal discount =
                ReadDecimal(txtDiscountAmount.Text);

            decimal paid =
                ReadDecimal(txtPaidAmount.Text);

            if (discount > total)
                discount = total;

            decimal net =
                total - discount;

            decimal remaining =
                net - paid;

            txtNetAmount.Text =
                net.ToString("N2");

            txtRemainingAmount.Text =
                remaining.ToString("N2");

            string classInfo =
                string.IsNullOrWhiteSpace(
                    selectedPlanClassName)
                    ? "الصف: غير محدد"
                    : "الصف: " +
                      selectedPlanClassName;

            lblSummary.Text =
                classInfo +
                " | نوع الرسوم: " +
                cmbFeeType.Text.Trim() +
                " | الإجمالي: " +
                total.ToString("N2") +
                " | الخصم: " +
                discount.ToString("N2") +
                " | الصافي: " +
                net.ToString("N2") +
                " | المدفوع: " +
                paid.ToString("N2") +
                " | المتبقي: " +
                remaining.ToString("N2") +
                (string.IsNullOrWhiteSpace(
                    studentFinancialSummary)
                    ? ""
                    : "\n" +
                      studentFinancialSummary);

            if (net == 0)
            {
                SetStatus("معفى");
                dtpPaymentDate.Checked = false;
            }
            else if (paid == 0)
            {
                SetStatus(
                    dtpDueDate.Value.Date < DateTime.Today
                        ? "متأخر"
                        : "غير مسدد");

                dtpPaymentDate.Checked = false;
            }
            else if (remaining < 0)
            {
                SetStatus("رصيد دائن");
                dtpPaymentDate.Checked = true;
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
            if (decimal.TryParse(
                    text,
                    NumberStyles.Any,
                    CultureInfo.CurrentCulture,
                    out decimal value))
            {
                return value;
            }

            if (decimal.TryParse(
                    text,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out value))
            {
                return value;
            }

            return 0;
        }

        // ============================================================
        // GRID SELECTION
        // ============================================================

        private void dataGridViewFees_CellClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 ||
                dataGridViewFees.Rows.Count == 0)
            {
                return;
            }

            DataRowView rowView =
                dataGridViewFees.Rows[e.RowIndex]
                    .DataBoundItem as DataRowView;

            if (rowView != null)
                FillFieldsFromRow(rowView.Row);
        }

        // ============================================================
        // LOAD SELECTED FEE
        // ============================================================

        private void FillFieldsFromRow(DataRow row)
        {
            isLoading = true;

            selectedFeeId =
                Convert.ToInt32(row["FeeID"]);

            selectedFeePlanId = null;

            if (row.Table.Columns.Contains("FeePlanID") &&
                row["FeePlanID"] != DBNull.Value)
            {
                selectedFeePlanId =
                    Convert.ToInt32(row["FeePlanID"]);
            }

            selectedPlanClassName =
                row.Table.Columns.Contains("ClassName") &&
                row["ClassName"] != DBNull.Value
                    ? row["ClassName"].ToString()
                    : "";

            txtTotalAmount.ReadOnly =
                selectedFeePlanId.HasValue;

            cmbStudent.SelectedValue =
                Convert.ToInt32(row["StudentID"]);

            cmbAcademicYear.Text =
                row["AcademicYear"].ToString();

            cmbFeeType.Text =
                row["FeeType"].ToString();

            txtTotalAmount.Text =
                Convert.ToDecimal(
                    row["TotalAmount"])
                    .ToString("N2");

            txtDiscountAmount.Text =
                Convert.ToDecimal(
                    row["DiscountAmount"])
                    .ToString("N2");

            txtNetAmount.Text =
                Convert.ToDecimal(
                    row["NetAmount"])
                    .ToString("N2");

            txtPaidAmount.Text =
                Convert.ToDecimal(
                    row["PaidAmount"])
                    .ToString("N2");

            txtRemainingAmount.Text =
                Convert.ToDecimal(
                    row["RemainingAmount"])
                    .ToString("N2");

            originalPaidAmount =
                Convert.ToDecimal(
                    row["PaidAmount"]);

            dtpDueDate.Value =
                Convert.ToDateTime(
                    row["DueDate"]);

            if (row["PaymentDate"] != DBNull.Value)
            {
                dtpPaymentDate.Checked = true;

                dtpPaymentDate.Value =
                    Convert.ToDateTime(
                        row["PaymentDate"]);
            }
            else
            {
                dtpPaymentDate.Checked = false;
            }

            cmbPaymentMethod.Text =
                row["PaymentMethod"] == DBNull.Value
                    ? "نقداً"
                    : row["PaymentMethod"].ToString();

            txtReceiptNumber.Text =
                row["ReceiptNumber"] == DBNull.Value
                    ? ""
                    : row["ReceiptNumber"].ToString();

            originalPaymentMethod =
                cmbPaymentMethod.Text.Trim();

            originalReceiptNumber =
                txtReceiptNumber.Text.Trim();

            originalPaymentDate =
                row["PaymentDate"] == DBNull.Value
                    ? (DateTime?)null
                    : Convert.ToDateTime(row["PaymentDate"]).Date;

            // بيانات السداد التاريخية لا تُعدّل من زر «تعديل».
            // تغيير المدفوع يتم فقط من «تحصيل دفعة» أو «استرجاع».
            txtPaidAmount.ReadOnly = selectedFeeId > 0;
            txtReceiptNumber.ReadOnly = selectedFeeId > 0;

            if (cmbStatus.Items.Contains(row["Status"].ToString()))
                cmbStatus.SelectedItem = row["Status"].ToString();

            else

                cmbStatus.SelectedIndex = -1;

            txtNotes.Text =
                row["Notes"] == DBNull.Value
                    ? ""
                    : row["Notes"].ToString();

            isLoading = false;

            CalculateAmounts();

            _ = LoadStudentFinancialSummaryAsync();
        }

        // ============================================================
        // STUDENT FINANCIAL SUMMARY
        // ============================================================

        private async Task LoadStudentFinancialSummaryAsync()
        {
            if (cmbStudent.SelectedValue == null ||
                string.IsNullOrWhiteSpace(
                    cmbAcademicYear.Text))
            {
                return;
            }

            int studentId;

            if (!int.TryParse(
                    Convert.ToString(
                        cmbStudent.SelectedValue),
                    out studentId) ||
                studentId <= 0)
            {
                return;
            }

            try
            {
string academicYear = cmbAcademicYear.Text.Trim();
                DataTable summary =
                    await Task.Run(() =>
                        feeService.GetStudentFinancialSummary(
                            studentId,
                            academicYear));

                studentFinancialSummary =
                    BuildStudentFinancialSummary(summary);

                CalculateAmounts();
            }
            catch (Exception ex)
            {
                if (!isLoading)
                {
                    UIHelper.ShowException(
                        "تعذر تحميل الملخص المالي للطالب:\n",
                        ex);
                }
            }
        }

        // ============================================================
        // CLEAR
        // ============================================================

        private void ClearInputs()
        {
            isLoading = true;

            selectedFeeId = 0;
            selectedFeePlanId = null;
            selectedPlanClassName = "";
            studentFinancialSummary = "";
            originalPaidAmount = 0;
            originalPaymentMethod = "";
            originalReceiptNumber = "";
            originalPaymentDate = null;

            txtPaidAmount.ReadOnly = false;
            txtReceiptNumber.ReadOnly = false;

            if (cmbStudent.Items.Count > 0)
                cmbStudent.SelectedIndex = 0;

            if (cmbAcademicYear.Items.Count > 0)
            {
                cmbAcademicYear.SelectedIndex =
                    Math.Min(
                        1,
                        cmbAcademicYear.Items.Count - 1);
            }

            if (cmbFeeType.Items.Count > 0)
            {
                int academicIndex =
                    cmbFeeType.FindStringExact(
                        "رسوم دراسية");

                cmbFeeType.SelectedIndex =
                    academicIndex >= 0
                        ? academicIndex
                        : Math.Min(
                            1,
                            cmbFeeType.Items.Count - 1);
            }

            txtTotalAmount.ReadOnly = false;

            txtTotalAmount.Text = "0";
            txtDiscountAmount.Text = "0";
            txtNetAmount.Text = "0";
            txtPaidAmount.Text = "0";
            txtRemainingAmount.Text = "0";

            dtpDueDate.Value =
                DateTime.Today.AddDays(30);

            dtpPaymentDate.Value =
                DateTime.Today;

            dtpPaymentDate.Checked = false;

            if (cmbPaymentMethod.Items.Count > 0)
                cmbPaymentMethod.SelectedIndex = 0;

            txtReceiptNumber.Text =
                "RCV-" +
                DateTime.Now.ToString(
                    "yyyyMMddHHmmssfff") +
                "-" +
                Guid.NewGuid()
                    .ToString("N")
                    .Substring(0, 6)
                    .ToUpperInvariant();

            if (cmbStatus.Items.Count > 0)
                cmbStatus.SelectedIndex = 0;

            txtNotes.Clear();

            isLoading = false;

            CalculateAmounts();
        }

        // ============================================================
        // RECEIPT NUMBER
        // ============================================================

        private string EnsureReceiptNumber()
        {
            if (string.IsNullOrWhiteSpace(
                txtReceiptNumber.Text))
            {
                txtReceiptNumber.Text =
                    "RCV-" +
                    DateTime.Now.ToString(
                        "yyyyMMddHHmmssfff") +
                    "-" +
                    Guid.NewGuid()
                        .ToString("N")
                        .Substring(0, 6)
                        .ToUpperInvariant();
            }

            return txtReceiptNumber.Text.Trim();
        }

        // ============================================================
        // VALIDATION
        // ============================================================

        private bool ValidateInputs()
        {
            if (cmbStudent.SelectedValue == null)
            {
                UIHelper.ShowWarning(
                    "يرجى اختيار الطالب.");

                cmbStudent.Focus();

                return false;
            }

            if (string.IsNullOrWhiteSpace(
                cmbAcademicYear.Text))
            {
                UIHelper.ShowWarning(
                    "يرجى اختيار العام الدراسي.");

                cmbAcademicYear.Focus();

                return false;
            }

            if (string.IsNullOrWhiteSpace(
                cmbFeeType.Text))
            {
                UIHelper.ShowWarning(
                    "يرجى اختيار نوع الرسوم.");

                cmbFeeType.Focus();

                return false;
            }

            if (selectedFeeId == 0 &&
                !selectedFeePlanId.HasValue)
            {
                UIHelper.ShowWarning(
                    "لا توجد خطة رسوم مطابقة لصف الطالب في العام المحدد. أنشئ الخطة أولاً من واجهة خطط الرسوم.");

                return false;
            }

            decimal total;
            decimal discount;
            decimal paid;

            if (!UIHelper.TryParseDecimal(
                    txtTotalAmount.Text,
                    out total) ||
                !UIHelper.TryParseDecimal(
                    txtDiscountAmount.Text,
                    out discount) ||
                !UIHelper.TryParseDecimal(
                    txtPaidAmount.Text,
                    out paid))
            {
                UIHelper.ShowWarning(
                    "أدخل جميع المبالغ بصيغة رقمية صحيحة.");

                return false;
            }

            if (total < 0 ||
                discount < 0 ||
                paid < 0)
            {
                UIHelper.ShowWarning(
                    "لا يمكن إدخال مبالغ سالبة.");

                return false;
            }

            if (discount > total)
            {
                UIHelper.ShowWarning(
                    "الخصم لا يمكن أن يكون أكبر من إجمالي الرسوم.");

                txtDiscountAmount.Focus();

                return false;
            }

            if (paid > 0)
                EnsureReceiptNumber();

            if (dtpPaymentDate.Checked &&
                dtpPaymentDate.Value.Date >
                DateTime.Today)
            {
                UIHelper.ShowWarning(
                    "لا يمكن تسجيل دفعة بتاريخ مستقبلي.");

                dtpPaymentDate.Focus();

                return false;
            }

            if (txtNotes.Text.Length > 1000)
            {
                UIHelper.ShowWarning(
                    "الملاحظات لا يجب أن تتجاوز 1000 حرف.");

                txtNotes.Focus();

                return false;
            }

            return true;
        }

        // ============================================================
        // MODEL
        // ============================================================

        private Fee GetFeeFromInputs()
        {
            CalculateAmounts();

            return new Fee
            {
                FeeID = selectedFeeId,

                StudentID =
                    UIHelper.GetSelectedId(cmbStudent),

                StudentName =
                    cmbStudent.Text,

                FeePlanID =
                    selectedFeePlanId,

                AcademicYear =
                    cmbAcademicYear.Text.Trim(),

                FeeType =
                    cmbFeeType.Text.Trim(),

                TotalAmount =
                    ReadDecimal(txtTotalAmount.Text),

                DiscountAmount =
                    ReadDecimal(txtDiscountAmount.Text),

                NetAmount =
                    ReadDecimal(txtNetAmount.Text),

                PaidAmount =
                    ReadDecimal(txtPaidAmount.Text),

                RemainingAmount =
                    ReadDecimal(txtRemainingAmount.Text),

                DueDate =
                    dtpDueDate.Value.Date,

                PaymentDate =
                    dtpPaymentDate.Checked
                        ? dtpPaymentDate.Value.Date
                        : (DateTime?)null,

                PaymentMethod =
                    cmbPaymentMethod.Text.Trim(),

                ReceiptNumber =
                    selectedFeeId > 0
                        ? txtReceiptNumber.Text.Trim()
                        : EnsureReceiptNumber(),

                Status =
                    cmbStatus.Text.Trim(),

                Notes =
                    txtNotes.Text.Trim()
            };
        }

        private static bool NullableDatesEqual(
            DateTime? left,
            DateTime? right)
        {
            if (!left.HasValue && !right.HasValue)
                return true;

            if (!left.HasValue || !right.HasValue)
                return false;

            return left.Value.Date == right.Value.Date;
        }

        // ============================================================
        // RECEIPT VOUCHER
        // ============================================================

        private async Task CreateReceiptVoucherIfNeededAsync(
            int feeId,
            decimal paymentAmount,
            string studentName,
            string paymentMethod)
        {
            if (paymentAmount <= 0)
                return;

            DateTime paymentDate =
                dtpPaymentDate.Checked
                    ? dtpPaymentDate.Value.Date
                    : DateTime.Today;

            string notes =
                "تم إنشاء سند قبض تلقائياً من شاشة الرسوم الدراسية."
                + Environment.NewLine +
                "نوع الرسوم: " +
                cmbFeeType.Text +
                Environment.NewLine +
                "العام الدراسي: " +
                cmbAcademicYear.Text;

            await Task.Run(() =>
                voucherService.CreateReceiptVoucherForFeePayment(
                    paymentAmount,
                    paymentDate,
                    studentName,
                    feeId,
                    paymentMethod,
                    notes
                ));
        }

        // ============================================================
        // GENERATE FEES
        // ============================================================

        private async void btnGenerateFees_Click(
            object sender,
            EventArgs e)
        {
            if (isSaving)
            {
                UIHelper.ShowInfo("عملية قيد التنفيذ؛ انتظر انتهاء الحفظ الحالي.");

                return;
            }

            isSaving = true;

            try
            {
                if (cmbStudent.SelectedValue == null)
                {
                    UIHelper.ShowWarning(
                        "اختر الطالب أولاً.");

                    return;
                }

                if (string.IsNullOrWhiteSpace(
                    cmbAcademicYear.Text))
                {
                    UIHelper.ShowWarning(
                        "اختر العام الدراسي.");

                    return;
                }

                int studentId =
                    UIHelper.GetSelectedId(cmbStudent);

                string academicYear =
                    cmbAcademicYear.Text.Trim();

                int count =
                    await Task.Run(() =>
                        feeService.GenerateStudentFeesFromPlans(
                            studentId,
                            academicYear));

                UIHelper.ShowInfo(
                    "تم توليد عدد " +
                    count +
                    " رسوم للطالب.");

                await LoadFeesAsync();

                DataRefreshNotifier.Notify("All", this);
            }
            catch (System.Data.SqlClient.SqlException ex)
                when (ex.Number == 51040)
            {
                ApplicationLogger.LogException("شاشة الرسوم: توليد رسوم الطالب", ex);
                UIHelper.ShowWarning(
                    "لا يمكن توليد الرسوم قبل قبول تسجيل الطالب في العام المحدد. افتح واجهة التسجيل والقبول، اختر العام نفسه، غيّر الحالة إلى (مقبول) واحفظ، ثم نفّذ التوزيع قبل توليد الرسوم.");
            }
            catch (Exception ex)
            {
                UIHelper.ShowException(
                    "فشل توليد رسوم الطالب:\n",
                    ex);
            }
            finally
            {
                isSaving = false;
            }
        }

        // ============================================================
        // ADD
        // ============================================================

        private async void btnAdd_Click(
            object sender,
            EventArgs e)
        {
            if (isSaving)
            {
                UIHelper.ShowInfo("عملية قيد التنفيذ؛ انتظر انتهاء الحفظ الحالي.");

                return;
            }

            isSaving = true;

            try
            {
                await ApplySelectedFeePlanAsync();

                if (!ValidateInputs())
                    return;

                Fee fee =
                    GetFeeFromInputs();

                int newFeeId =
                    await Task.Run(() =>
                        feeService.AddFee(fee));

                decimal paidAmount =
                    fee.PaidAmount;

                if (paidAmount > 0)
                {
                    await CreateReceiptVoucherIfNeededAsync(
                        newFeeId,
                        paidAmount,
                        cmbStudent.Text,
                        cmbPaymentMethod.Text);
                }

                UIHelper.ShowInfo(
                    "تمت إضافة الرسوم بنجاح.");

                await LoadFeesAsync();

                DataRefreshNotifier.Notify("All", this);
                DataRefreshNotifier.Notify("All", this);

                ClearInputs();
            }
            catch (System.Data.SqlClient.SqlException ex)
                when (ex.Number == 51042)
            {
                ApplicationLogger.LogException("شاشة الرسوم: إضافة رسم مكرر", ex);
                UIHelper.ShowWarning(
                    "هذا الرسم موجود مسبقًا لهذا الطالب في العام الدراسي نفسه. اختر الرسم الموجود من الجدول وسجل الدفعة من زر (تسجيل دفعة) بدل إنشاء رسم جديد.");

                await LoadFeesAsync();
            }
            catch (Exception ex)
            {
                UIHelper.ShowException(
                    "فشل إضافة الرسوم:\n",
                    ex);
            }
            finally
            {
                isSaving = false;
            }
        }

        // ============================================================
        // UPDATE
        // ============================================================

        private async void btnUpdate_Click(
            object sender,
            EventArgs e)
        {
            if (isSaving)
            {
                UIHelper.ShowInfo("عملية قيد التنفيذ؛ انتظر انتهاء الحفظ الحالي.");

                return;
            }

            isSaving = true;

            try
            {
                if (selectedFeeId == 0)
                {
                    UIHelper.ShowWarning(
                        "اختر سجل الرسوم من الجدول أولاً.");

                    return;
                }

                if (!ValidateInputs())
                    return;

                Fee fee =
                    GetFeeFromInputs();

                if (Math.Abs(fee.PaidAmount - originalPaidAmount) > 0.01m)
                {
                    UIHelper.ShowWarning(
                        "لا تعدّل حقل المدفوع من «تعديل». استخدم «تحصيل دفعة» أو «استرجاع» حتى يتم تحديث السند والحركة المالية معًا.");

                    txtPaidAmount.Focus();
                    return;
                }

                if (!string.Equals(
                        fee.PaymentMethod.Trim(),
                        originalPaymentMethod,
                        StringComparison.OrdinalIgnoreCase)
                    || !string.Equals(
                        fee.ReceiptNumber.Trim(),
                        originalReceiptNumber,
                        StringComparison.OrdinalIgnoreCase)
                    || !NullableDatesEqual(
                        fee.PaymentDate,
                        originalPaymentDate))
                {
                    UIHelper.ShowWarning(
                        "بيانات السداد ورقم السند لا تُعدّل من «تعديل الرسوم». استخدم مسار التحصيل أو الاسترجاع.");

                    return;
                }

                decimal newPaidAmount =
                    fee.PaidAmount;

                decimal paymentDifference = 0m;

                bool result =
                    await Task.Run(() =>
                        feeService.UpdateFee(fee));

                if (result &&
                    paymentDifference > 0)
                {
                    await CreateReceiptVoucherIfNeededAsync(
                        selectedFeeId,
                        paymentDifference,
                        cmbStudent.Text,
                        cmbPaymentMethod.Text);
                }

                if (result)
                {
                    UIHelper.ShowInfo(
                        "تم تعديل الرسوم بنجاح.");
                }
                else
                {
                    UIHelper.ShowWarning(
                        "لم يتم تعديل الرسوم؛ ربما تغير السجل أو لم يعد موجودًا.");
                }

                await LoadFeesAsync();

                if (result)
                {
                    DataRefreshNotifier.Notify("All", this);
                    DataRefreshNotifier.Notify("All", this);
                }

                ClearInputs();
            }
            catch (Exception ex)
            {
                UIHelper.ShowException(
                    "فشل تعديل الرسوم:\n",
                    ex);
            }
            finally
            {
                isSaving = false;
            }
        }

        // ============================================================
        // RECORD PAYMENT
        // ============================================================

        private async void btnRecordPayment_Click(
            object sender,
            EventArgs e)
        {
            if (isSaving)
            {
                UIHelper.ShowInfo("عملية قيد التنفيذ؛ انتظر انتهاء الحفظ الحالي.");

                return;
            }

            isSaving = true;

            try
            {
                if (selectedFeeId <= 0)
                {
                    UIHelper.ShowWarning(
                        "اختر سجل الرسوم من الجدول أولاً.");

                    return;
                }

                decimal newTotalPaid =
                    ReadDecimal(
                        txtPaidAmount.Text);

                decimal paymentAmount =
                    newTotalPaid -
                    originalPaidAmount;

                int studentId =
                    UIHelper.GetSelectedId(cmbStudent);

                if (paymentAmount == 0)
                {
                    UIHelper.ShowWarning(
                        "أدخل إجمالي المدفوع الجديد في حقل المدفوع، ليختلف عن المدفوع الحالي.");

                    txtPaidAmount.Focus();

                    return;
                }

                if (!dtpPaymentDate.Checked)
                    dtpPaymentDate.Checked = true;

                if (dtpPaymentDate.Value.Date >
                    DateTime.Today)
                {
                    UIHelper.ShowWarning(
                        "تاريخ السداد لا يمكن أن يكون في المستقبل.");

                    return;
                }

string receiptNumber =
                    EnsureReceiptNumber();

                DateTime paymentDate = dtpPaymentDate.Value.Date;
                string paymentMethod = cmbPaymentMethod.Text;
                if (string.IsNullOrWhiteSpace(paymentMethod))
                    paymentMethod = "نقداً";
                string notes = txtNotes.Text;
                string academicYear = cmbAcademicYear.Text.Trim();

                bool isRefund = paymentAmount < 0;

                if (isRefund)
                {
                    decimal refundAmount = -paymentAmount;

                    DataTable refundResult =
                        await Task.Run(() =>
                            feeService.RefundPayment(
                                selectedFeeId,
                                refundAmount,
                                paymentDate,
                                notes));

                    DataRow refundRow =
                        refundResult.Rows[0];

                    DataTable refundedSummary =
                        await Task.Run(() =>
                            feeService.GetStudentFinancialSummary(
                                studentId,
                                academicYear));

                    decimal refundedRemaining =
                        refundedSummary != null &&
                        refundedSummary.Rows.Count > 0
                            ? Convert.ToDecimal(
                                refundedSummary.Rows[0]
                                    ["TotalRemainingAmount"])
                            : Convert.ToDecimal(
                                refundRow["RemainingAmount"]);

                    string refundBalanceLabel =
                        refundedRemaining < 0
                            ? "الرصيد الدائن للطالب"
                            : "إجمالي المتبقي على الطالب";

                    UIHelper.ShowInfo(
                        "تم خصم المبلغ تلقائيًا من رسوم الطالب. "
                        + refundBalanceLabel +
                        ": " +
                        refundedRemaining.ToString("N2"));

                    await LoadFeesAsync();

                    DataRefreshNotifier.Notify("All", this);
                    DataRefreshNotifier.Notify("All", this);

                    ClearInputs();

                    return;
                }

                DataTable result =
                    await Task.Run(() =>
                        feeService.RecordPayment(
                            selectedFeeId,
                            paymentAmount,
                            paymentDate,
                            paymentMethod,
                            receiptNumber,
                            notes));

                DataRow row =
                    result.Rows[0];

                DataTable refreshedSummary =
                    await Task.Run(() =>
                        feeService.GetStudentFinancialSummary(
                            studentId,
                            academicYear));

                decimal totalRemaining =
                    refreshedSummary != null &&
                    refreshedSummary.Rows.Count > 0
                        ? Convert.ToDecimal(
                            refreshedSummary.Rows[0]
                                ["TotalRemainingAmount"])
                        : Convert.ToDecimal(
                            row["RemainingAmount"]);

                string balanceLabel =
                    totalRemaining < 0
                        ? "الرصيد الدائن للطالب"
                        : "إجمالي المتبقي على الطالب";

                UIHelper.ShowInfo(
                    "تم تسجيل الدفعة وإنشاء سند القبض بنجاح. "
                    + balanceLabel +
                    ": " +
                    totalRemaining.ToString("N2"));

                await LoadFeesAsync();

                DataRefreshNotifier.Notify("All", this);
                DataRefreshNotifier.Notify("All", this);

                ClearInputs();
            }
            catch (Exception ex)
            {
                UIHelper.ShowException(
                    "فشل تسجيل الدفعة:",
                    ex);
            }
            finally
            {
                isSaving = false;
            }
        }

        // ============================================================
        // DELETE
        // ============================================================

        private async void btnDelete_Click(
            object sender,
            EventArgs e)
        {
            if (isSaving)
            {
                UIHelper.ShowInfo("عملية قيد التنفيذ؛ انتظر انتهاء الحفظ الحالي.");

                return;
            }

            isSaving = true;

            try
            {
                if (selectedFeeId == 0)
                {
                    UIHelper.ShowWarning(
                        "اختر سجل الرسوم من الجدول أولاً.");

                    return;
                }

                bool confirm =
                    UIHelper.ShowConfirmation(
                        "هل أنت متأكد من حذف سجل الرسوم المحدد؟",
                        "تأكيد الحذف");

                if (!confirm)
                    return;

                bool result =
                    await Task.Run(() =>
                        feeService.DeleteFee(
                            selectedFeeId));

                if (result)
                {
                    UIHelper.ShowInfo(
                        "تم حذف الرسوم بنجاح.");
                }
                else
                {
                    UIHelper.ShowWarning(
                        "لم يتم حذف الرسوم؛ ربما تغير السجل أو لم يعد موجودًا.");
                }

                await LoadFeesAsync();

                if (result)
                {
                    DataRefreshNotifier.Notify("All", this);
                    DataRefreshNotifier.Notify("All", this);
                }

                ClearInputs();
            }
            catch (System.Data.SqlClient.SqlException ex)
                when (ex.Number == 51060)
            {
                ApplicationLogger.LogException(
                    "شاشة الرسوم: محاولة حذف رسم مرتبط بسند",
                    ex);

                UIHelper.ShowWarning(
                    "لا يمكن حذف هذا الرسم لأنه مرتبط بسند قبض. استخدم إلغاء/سندًا عكسيًا من شاشة السندات للحفاظ على السجل المالي.");

                await LoadFeesAsync();
            }
            catch (Exception ex)
            {
                UIHelper.ShowException(
                    "فشل حذف الرسوم:\n",
                    ex);
            }
            finally
            {
                isSaving = false;
            }
        }

        // ============================================================
        // REPORT DATA
        // ============================================================

        private DataTable GetVisibleFeesForReport()
        {
            if (allFees == null)
                return null;

            DataView view =
                allFees.DefaultView;

            DataTable report =
                view.ToTable();

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

            RenameReportColumn(
                report,
                "StudentName",
                "الطالب");

            RenameReportColumn(
                report,
                "ClassName",
                "الصف");

            RenameReportColumn(
                report,
                "AcademicYear",
                "العام الدراسي");

            RenameReportColumn(
                report,
                "FeeType",
                "نوع الرسوم");

            RenameReportColumn(
                report,
                "TotalAmount",
                "الإجمالي");

            RenameReportColumn(
                report,
                "DiscountAmount",
                "الخصم");

            RenameReportColumn(
                report,
                "NetAmount",
                "الصافي");

            RenameReportColumn(
                report,
                "PaidAmount",
                "المدفوع");

            RenameReportColumn(
                report,
                "RemainingAmount",
                "المتبقي");

            RenameReportColumn(
                report,
                "DueDate",
                "تاريخ الاستحقاق");

            RenameReportColumn(
                report,
                "PaymentDate",
                "تاريخ الدفع");

            RenameReportColumn(
                report,
                "PaymentMethod",
                "طريقة الدفع");

            RenameReportColumn(
                report,
                "ReceiptNumber",
                "رقم السند");

            RenameReportColumn(
                report,
                "Status",
                "الحالة");

            RenameReportColumn(
                report,
                "Notes",
                "ملاحظات");

            return report;
        }

        private void RenameReportColumn(
            DataTable table,
            string source,
            string target)
        {
            if (table.Columns.Contains(source))
                table.Columns[source].ColumnName =
                    target;
        }

        // ============================================================
        // OUTSTANDING REPORT
        // ============================================================

        private void btnOutstandingReport_Click(
            object sender,
            EventArgs e)
        {
            showOutstandingOnly =
                !showOutstandingOnly;

            ApplyFilter();
        }

        private string GetFeeReportTitle()
        {
            return showOutstandingOnly
                ? "تقرير المتأخرات المالية للطلاب"
                : "تقرير الرسوم الدراسية والتحصيل";
        }

        // ============================================================
        // EXPORT EXCEL
        // ============================================================

        private void btnExportExcel_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                CurrentUser.DemandAction(
                    "Fees",
                    "ExportExcel",
                    "ليس لديك صلاحية تصدير تقارير الرسوم إلى Excel.");

                DataTable report =
                    GetVisibleFeesForReport();

                if (report == null ||
                    report.Rows.Count == 0)
                {
                    UIHelper.ShowWarning(
                        "لا توجد رسوم ظاهرة للتصدير.");

                    return;
                }

                using (SaveFileDialog dialog =
                    new SaveFileDialog())
                {
                    dialog.Filter =
                        "Excel Workbook (*.xlsx)|*.xlsx";

                    dialog.FileName =
                        (showOutstandingOnly
                            ? "تقرير_المتأخرات_"
                            : "تقرير_الرسوم_") +
                        DateTime.Now.ToString(
                            "yyyyMMdd_HHmm") +
                        ".xlsx";

                    if (dialog.ShowDialog() !=
                        DialogResult.OK)
                    {
                        return;
                    }

                    ReportOutputHelper.ExportToExcel(
                        report,
                        dialog.FileName,
                        GetFeeReportTitle(),
                        "عدد السجلات: " +
                        report.Rows.Count +
                        (showOutstandingOnly
                            ? " | يعرض الأرصدة المتبقية فقط"
                            : ""));

                    UIHelper.ShowInfo(
                        "تم تصدير تقرير الرسوم إلى Excel بنجاح.");
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowException(
                    "فشل تصدير تقرير الرسوم إلى Excel:\n",
                    ex);
            }
        }

        // ============================================================
        // EXPORT PDF
        // ============================================================

        private void btnExportPdf_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                CurrentUser.DemandAction(
                    "Fees",
                    "ExportPDF",
                    "ليس لديك صلاحية تصدير تقارير الرسوم إلى PDF.");

                DataTable report =
                    GetVisibleFeesForReport();

                if (report == null ||
                    report.Rows.Count == 0)
                {
                    UIHelper.ShowWarning(
                        "لا توجد رسوم ظاهرة للتصدير.");

                    return;
                }

                using (SaveFileDialog dialog =
                    new SaveFileDialog())
                {
                    dialog.Filter =
                        "PDF Document (*.pdf)|*.pdf";

                    dialog.FileName =
                        (showOutstandingOnly
                            ? "تقرير_المتأخرات_"
                            : "تقرير_الرسوم_") +
                        DateTime.Now.ToString(
                            "yyyyMMdd_HHmm") +
                        ".pdf";

                    if (dialog.ShowDialog() !=
                        DialogResult.OK)
                    {
                        return;
                    }

                    ReportOutputHelper.ExportToPdf(
                        report,
                        dialog.FileName,
                        GetFeeReportTitle(),
                        "عدد السجلات: " +
                        report.Rows.Count +
                        (showOutstandingOnly
                            ? " | يعرض الأرصدة المتبقية فقط"
                            : ""));

                    UIHelper.ShowInfo(
                        "تم تصدير تقرير الرسوم إلى PDF بنجاح.");
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowException(
                    "فشل تصدير تقرير الرسوم إلى PDF:\n",
                    ex);
            }
        }

        // ============================================================
        // CLEAR BUTTON
        // ============================================================

        private void btnClear_Click(
            object sender,
            EventArgs e)
        {
            ClearInputs();
        }
    }
}
