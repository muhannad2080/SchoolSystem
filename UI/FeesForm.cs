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
using SchoolSystem.DataAccess;

namespace SchoolSystem.UI
{
    public partial class FeesForm : UserControl
    {
        // ============================================================
        // SERVICES
        // ============================================================

        private readonly FeeService feeService = new FeeService();
        private readonly StudentService studentService = new StudentService();
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

        private bool refreshInProgress = false;

        private bool showOutstandingOnly = false;

        // ============================================================
        // CONSTRUCTOR
        // ============================================================

        public FeesForm()
        {
            InitializeComponent();

            UIHelper.ApplyStyle(this);
            ConfigureFeesGridAppearance();
            Dock = DockStyle.Fill;
            ConfigureResponsiveActionPanel();


            // التحقق والسلوك الخاصان بشاشة الرسوم.

            // تطبيق قواعد التحقق من الحقول (فلترة الأحرف، أقصى طول، والتحقق عند
            // المغادرة) دون تغيير تنسيق المصمم لهذه الشاشة.
            SchoolSystem.Helpers.UIHelper.ApplyFieldValidation(this);

            txtSearch.TextChanged += (sender, e) => ApplyFilter();

            DataRefreshNotifier.DataChanged += DataRefreshNotifier_DataChanged;
            Disposed += (sender, e) =>
            {
                DataRefreshNotifier.DataChanged -= DataRefreshNotifier_DataChanged;
                VisibleChanged -= FeesForm_VisibleChanged;
            };

            cmbStudent.SelectedIndexChanged += async (sender, e) =>
                await ApplySelectedFeePlanAsync();

            cmbAcademicYear.SelectedIndexChanged += async (sender, e) =>
                await ApplySelectedFeePlanAsync();

            cmbFeeType.SelectedIndexChanged += async (sender, e) =>
                await ApplySelectedFeePlanAsync();

            Load += FeesForm_Load;
            VisibleChanged += FeesForm_VisibleChanged;
        }

        // جدول الرسوم بأسلوب Excel موحد مع بقية شاشات النظام.
        private void ConfigureFeesGridAppearance()
        {
            if (dataGridViewFees == null)
                return;

            dataGridViewFees.SuspendLayout();
            try
            {
                dataGridViewFees.AllowUserToAddRows = false;
                dataGridViewFees.AllowUserToDeleteRows = false;
                dataGridViewFees.AllowUserToResizeRows = false;
                dataGridViewFees.AllowUserToResizeColumns = true;
                dataGridViewFees.AllowUserToOrderColumns = true;
                dataGridViewFees.ReadOnly = true;
                dataGridViewFees.MultiSelect = false;
                dataGridViewFees.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridViewFees.RowHeadersVisible = false;
                dataGridViewFees.RightToLeft = RightToLeft.Yes;
                dataGridViewFees.ScrollBars = ScrollBars.Both;
                dataGridViewFees.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                dataGridViewFees.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
                dataGridViewFees.BackgroundColor = Color.White;
                dataGridViewFees.BorderStyle = BorderStyle.FixedSingle;
                dataGridViewFees.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                dataGridViewFees.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                dataGridViewFees.EnableHeadersVisualStyles = false;
                dataGridViewFees.GridColor = UIHelper.BorderColor;
                dataGridViewFees.ColumnHeadersHeight = 44;
                dataGridViewFees.ColumnHeadersHeightSizeMode =
                    DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                dataGridViewFees.RowTemplate.Height = 38;
                dataGridViewFees.ClipboardCopyMode =
                    DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;

                dataGridViewFees.ColumnHeadersDefaultCellStyle.BackColor = UIHelper.PrimaryColor;
                dataGridViewFees.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dataGridViewFees.ColumnHeadersDefaultCellStyle.Font =
                    new Font(UIHelper.FontFamily, 10F, FontStyle.Bold);
                dataGridViewFees.ColumnHeadersDefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleCenter;
                dataGridViewFees.ColumnHeadersDefaultCellStyle.WrapMode =
                    DataGridViewTriState.True;
                dataGridViewFees.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                    UIHelper.PrimaryColor;
                dataGridViewFees.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
                dataGridViewFees.ColumnHeadersDefaultCellStyle.Padding =
                    new Padding(8, 4, 8, 4);

                dataGridViewFees.DefaultCellStyle.BackColor = Color.White;
                dataGridViewFees.DefaultCellStyle.ForeColor = UIHelper.TextColor;
                dataGridViewFees.DefaultCellStyle.Font =
                    new Font(UIHelper.FontFamily, 9.5F, FontStyle.Regular);
                dataGridViewFees.DefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleRight;
                dataGridViewFees.DefaultCellStyle.SelectionBackColor = UIHelper.AccentColor;
                dataGridViewFees.DefaultCellStyle.SelectionForeColor = Color.White;
                dataGridViewFees.DefaultCellStyle.Padding = new Padding(8, 4, 8, 4);
                dataGridViewFees.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

                dataGridViewFees.AlternatingRowsDefaultCellStyle.BackColor =
                    Color.FromArgb(241, 245, 249);
                dataGridViewFees.AlternatingRowsDefaultCellStyle.ForeColor = UIHelper.TextColor;
                dataGridViewFees.AlternatingRowsDefaultCellStyle.SelectionBackColor =
                    UIHelper.AccentColor;
                dataGridViewFees.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.White;
                dataGridViewFees.RowsDefaultCellStyle.BackColor = Color.White;
                dataGridViewFees.RowsDefaultCellStyle.ForeColor = UIHelper.TextColor;
                dataGridViewFees.RowsDefaultCellStyle.SelectionBackColor = UIHelper.AccentColor;
                dataGridViewFees.RowsDefaultCellStyle.SelectionForeColor = Color.White;
            }
            finally
            {
                dataGridViewFees.ResumeLayout();
            }
        }

        /// <summary>
        /// Reserves enough space for fee actions to wrap into two visible rows
        /// on standard and smaller window sizes.
        /// </summary>
        private void ConfigureResponsiveActionPanel()
        {
            panelButtons.SuspendLayout();
            rootLayout.SuspendLayout();

            try
            {
                panelButtons.AutoScroll = true;
                panelButtons.WrapContents = true;
                panelButtons.FlowDirection = FlowDirection.RightToLeft;
                panelButtons.Padding = new Padding(12, 6, 12, 6);

                foreach (Control control in panelButtons.Controls)
                {
                    Button button = control as Button;
                    if (button == null)
                        continue;

                    button.Margin = new Padding(4, 2, 4, 2);
                    button.Height = 40;
                }

                // Row 2 is the financial summary; row 3 contains the actions.
                if (rootLayout.RowStyles.Count > 3)
                {
                    rootLayout.RowStyles[2].Height = 80F;
                    rootLayout.RowStyles[3].Height = 104F;
                }
            }
            finally
            {
                rootLayout.ResumeLayout(true);
                panelButtons.ResumeLayout(true);
            }
        }

        private async void DataRefreshNotifier_DataChanged(
            object sender,
            DataRefreshEventArgs e)
        {
            if (IsDisposed || Disposing || !IsHandleCreated)
                return;

            // أحداث التغيير قد تأتي من خدمات خلفية، وربط DataGridView يجب أن يحدث على UI thread.
            if (InvokeRequired)
            {
                try
                {
                    BeginInvoke(new MethodInvoker(
                        () => DataRefreshNotifier_DataChanged(sender, e)));
                }
                catch (InvalidOperationException)
                {
                    // أُغلقت الواجهة أثناء جدولة التحديث.
                }

                return;
            }

            if (ReferenceEquals(sender, this) ||
                (!DataRefreshNotifier.IsRelated(e, "Fees") &&
                 !DataRefreshNotifier.IsRelated(e, "Vouchers") &&
                 !DataRefreshNotifier.IsRelated(e, "Enrollment") &&
                 !DataRefreshNotifier.IsRelated(e, "Classes") &&
                 !DataRefreshNotifier.IsRelated(e, "Sections") &&
                 !DataRefreshNotifier.IsRelated(e, "FeePlans") &&
                 !DataRefreshNotifier.IsRelated(e, "AcademicYears") &&
                 !DataRefreshNotifier.IsRelated(e, "All")))
            {
                return;
            }

            await RefreshFeesViewAsync();
        }

        private async void FeesForm_VisibleChanged(
            object sender,
            EventArgs e)
        {
            if (!Visible ||
                IsDisposed ||
                isLoading ||
                isSaving ||
                refreshInProgress)
            {
                return;
            }

            await RefreshFeesViewAsync();
        }

        private async Task RefreshFeesViewAsync()
        {
            if (IsDisposed ||
                isSaving ||
                refreshInProgress)
            {
                return;
            }

            refreshInProgress = true;

            int feeIdToKeep = selectedFeeId;
            int studentIdToKeep = UIHelper.GetSelectedId(cmbStudent);
            string academicYearToKeep = cmbAcademicYear.Text;
            string feeTypeToKeep = cmbFeeType.Text;

            try
            {
                /*
                 * مزامنة كاملة للشاشة: نعيد تحميل القوائم المرجعية مع
                 * الاحتفاظ باختيارات المستخدم، ثم نعيد قراءة الرسوم من SQL Server.
                 * الاستعلام نفسه يستخدم FeePlans كمصدر حي للإجمالي والنوع
                 * والاستحقاق، ثم نعيد تعبئة السجل المختار والملخص المالي.
                 */
                isLoading = true;

                await LoadAcademicYearsAsync();
                await LoadFeeTypesAsync();
                await LoadStudentsAsync();

                if (!string.IsNullOrWhiteSpace(academicYearToKeep) &&
                    cmbAcademicYear.Items.Contains(academicYearToKeep))
                {
                    cmbAcademicYear.SelectedItem = academicYearToKeep;
                }

                if (!string.IsNullOrWhiteSpace(feeTypeToKeep) &&
                    cmbFeeType.Items.Contains(feeTypeToKeep))
                {
                    cmbFeeType.SelectedItem = feeTypeToKeep;
                    cmbFeeType.Text = feeTypeToKeep;
                }

                if (studentIdToKeep > 0)
                    cmbStudent.SelectedValue = studentIdToKeep;

                isLoading = false;

                await LoadFeesAsync();

                if (feeIdToKeep > 0)
                {
                    selectedFeeId = feeIdToKeep;
                    RefreshSelectedFeeFromLoadedData();
                }
                else
                {
                    await ApplySelectedFeePlanAsync();
                    await LoadStudentFinancialSummaryAsync();
                }
            }
            catch (Exception ex)
            {
                if (!IsDisposed && !isLoading)
                {
                    UIHelper.ShowException(
                        "تعذر تحديث بيانات الرسوم تلقائيًا:\n",
                        ex);
                }
            }
            finally
            {
                isLoading = false;
                refreshInProgress = false;
            }
        }

        private void RefreshSelectedFeeFromLoadedData()
        {
            if (selectedFeeId <= 0 || allFees == null)
                return;

            foreach (DataRow row in allFees.Rows)
            {
                if (row["FeeID"] == DBNull.Value ||
                    Convert.ToInt32(row["FeeID"]) != selectedFeeId)
                    continue;

                FillFieldsFromRow(row);
                SelectFeeRowById(selectedFeeId);
                return;
            }

            ClearInputs();
        }

        private void SelectFeeRowById(int feeId)
        {
            if (feeId <= 0)
                return;

            for (int i = 0; i < dataGridViewFees.Rows.Count; i++)
            {
                DataRowView rowView =
                    dataGridViewFees.Rows[i].DataBoundItem as DataRowView;

                if (rowView == null ||
                    !rowView.Row.Table.Columns.Contains("FeeID") ||
                    rowView.Row["FeeID"] == DBNull.Value ||
                    Convert.ToInt32(rowView.Row["FeeID"]) != feeId)
                {
                    continue;
                }

                dataGridViewFees.ClearSelection();
                dataGridViewFees.Rows[i].Selected = true;

                if (dataGridViewFees.Columns.Count > 0)
                {
                    dataGridViewFees.CurrentCell =
                        dataGridViewFees.Rows[i].Cells[0];
                }

                return;
            }
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

            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            column.MinimumWidth = Math.Min(width, Math.Max(55, minimumWidth));
            column.Width = Math.Max(width, minimumWidth);
            column.Resizable = DataGridViewTriState.True;
            column.SortMode = DataGridViewColumnSortMode.Automatic;
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

            SetExistingFeeIdentityState(true);
            lblPaidAmount.Text = "المدفوع الحالي";

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

            txtPaymentAmount.Text = "0";

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
            txtPaidAmount.ReadOnly = true;
            txtReceiptNumber.ReadOnly = true;

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

        private void SetExistingFeeIdentityState(bool existing)
        {
            cmbStudent.Enabled = !existing;
            cmbAcademicYear.Enabled = !existing;
            cmbFeeType.Enabled = !existing;

            txtPaymentAmount.Enabled = existing;
            txtPaymentAmount.ReadOnly = !existing;

            if (existing)
            {
                cmbStudent.Cursor = Cursors.No;
                cmbAcademicYear.Cursor = Cursors.No;
                cmbFeeType.Cursor = Cursors.No;
            }
            else
            {
                cmbStudent.Cursor = Cursors.Default;
                cmbAcademicYear.Cursor = Cursors.Default;
                cmbFeeType.Cursor = Cursors.Default;
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
            SetExistingFeeIdentityState(false);
            lblPaidAmount.Text = "المدفوع";
            selectedPlanClassName = "";
            studentFinancialSummary = "";
            originalPaidAmount = 0;
            originalPaymentMethod = "";
            originalReceiptNumber = "";
            originalPaymentDate = null;

            txtPaidAmount.ReadOnly = true;
            txtReceiptNumber.ReadOnly = true;

            if (cmbStudent.Items.Count > 0)
                cmbStudent.SelectedIndex = 0;

            if (cmbAcademicYear.Items.Count > 0)
            {
                string currentYear =
                    AcademicYearHelper.GetCurrentAcademicYear();

                int currentIndex =
                    cmbAcademicYear.Items.IndexOf(currentYear);

                cmbAcademicYear.SelectedIndex =
                    currentIndex >= 0 ? currentIndex : 0;
            }

            if (cmbFeeType.Items.Count > 0)
                cmbFeeType.SelectedIndex = 0;

            txtTotalAmount.ReadOnly = false;

            txtTotalAmount.Text = "0";
            txtDiscountAmount.Text = "0";
            txtNetAmount.Text = "0";
            txtPaidAmount.Text = "0";
            txtRemainingAmount.Text = "0";
            txtPaymentAmount.Text = "0";

            dtpDueDate.Value =
                DateTime.Today.AddDays(30);

            dtpPaymentDate.Value =
                DateTime.Today;

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

            if (selectedFeeId == 0 && paid > 0)
            {
                UIHelper.ShowWarning(
                    "لا تُنشئ الرسم مع مبلغ مدفوع. احفظ الرسم أولًا ثم استخدم زر «تحصيل دفعة» حتى ينشأ سند القبض وتُحدّث الرسوم الكلية تلقائيًا.");

                txtPaidAmount.Focus();
                return false;
            }

            if (selectedFeeId > 0 && paid < 0)
            {
                UIHelper.ShowWarning("المدفوع لا يمكن أن يكون سالبًا.");
                return false;
            }

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
                        : string.Empty,

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

                await Task.Run(() =>
                    feeService.AddFee(fee));

                UIHelper.ShowInfo(
                    "تمت إضافة الرسوم بنجاح.");

                await LoadFeesAsync();

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

                bool result =
                    await Task.Run(() =>
                        feeService.UpdateFee(fee));

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

            if (selectedFeeId <= 0)
            {
                UIHelper.ShowWarning(
                    "اختر سجل الرسوم من الجدول أولاً.");
                return;
            }

            decimal paymentAmount;
            if (!UIHelper.TryParseDecimal(
                    txtPaymentAmount.Text,
                    out paymentAmount) ||
                paymentAmount <= 0)
            {
                UIHelper.ShowWarning(
                    "أدخل مبلغ الدفعة الحالية، ويجب أن يكون أكبر من صفر.");
                txtPaymentAmount.Focus();
                return;
            }

            if (paymentAmount > 100000000m)
            {
                UIHelper.ShowWarning(
                    "مبلغ الدفعة لا يمكن أن يتجاوز 100,000,000.");
                txtPaymentAmount.Focus();
                return;
            }

            isSaving = true;

            try
            {
                if (!dtpPaymentDate.Checked)
                    dtpPaymentDate.Checked = true;

                if (dtpPaymentDate.Value.Date >
                    DateTime.Today)
                {
                    UIHelper.ShowWarning(
                        "تاريخ السداد لا يمكن أن يكون في المستقبل.");
                    return;
                }

                // لا نعيد استخدام رقم سند قديم عند تسجيل دفعة جديدة.
                txtReceiptNumber.Clear();

                string receiptNumber =
                    EnsureReceiptNumber();

                DateTime paymentDate =
                    dtpPaymentDate.Value.Date;

                string paymentMethod =
                    cmbPaymentMethod.Text;

                if (string.IsNullOrWhiteSpace(paymentMethod))
                    paymentMethod = "نقداً";

                string notes =
                    txtNotes.Text;

                string academicYear =
                    cmbAcademicYear.Text.Trim();

                int studentId =
                    UIHelper.GetSelectedId(cmbStudent);

                DataTable result =
                    await Task.Run(() =>
                        feeService.RecordPayment(
                            selectedFeeId,
                            paymentAmount,
                            paymentDate,
                            paymentMethod,
                            receiptNumber,
                            notes));

                if (result == null ||
                    result.Rows.Count == 0)
                {
                    throw new InvalidOperationException(
                        "لم يتم تسجيل الدفعة ولم تتغير أرصدة الرسوم.");
                }

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
                            refreshedSummary.Rows[0]["TotalRemainingAmount"])
                        : Convert.ToDecimal(
                            row["RemainingAmount"]);

                string balanceLabel =
                    totalRemaining < 0
                        ? "الرصيد الدائن للطالب"
                        : "إجمالي المتبقي على الطالب";

                string voucherNumber =
                    row.Table.Columns.Contains("VoucherNumber") &&
                    row["VoucherNumber"] != DBNull.Value
                        ? Convert.ToString(row["VoucherNumber"])
                        : "غير متاح";

                UIHelper.ShowInfo(
                    "تم تسجيل دفعة بمبلغ " +
                    paymentAmount.ToString("N2") +
                    " وإنشاء سند قبض رقم " +
                    voucherNumber +
                    " تلقائياً. " +
                    balanceLabel +
                    ": " +
                    totalRemaining.ToString("N2"));

                await LoadFeesAsync();

                DataRefreshNotifier.Notify("All", this);

                ClearInputs();
            }
            catch (System.Data.SqlClient.SqlException ex)
                when (ex.Number == 51055)
            {
                ApplicationLogger.LogException(
                    "شاشة الرسوم: سداد زائد دون بنود رسوم فعلية",
                    ex);

                UIHelper.ShowWarning(
                    "تعذر ترحيل الدفعة الزائدة لأنه لا توجد بنود رسوم فعلية للطالب في العام الدراسي المحدد.");
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