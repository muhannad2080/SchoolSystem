using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

using SchoolSystem.Models;
using SchoolSystem.Services;
using SchoolSystem.Helpers;
using SchoolSystem.Security;

namespace SchoolSystem.UI
{
    public partial class EnrollmentForm : Form
    {
        // ================================================================
        // SERVICES
        // ================================================================

        private readonly EnrollmentService enrollmentService;

        private readonly StudentService studentService;

        private readonly ClassService classService;

        private readonly StudentClassService sectionService;

        private readonly FeePlanService feePlanService;

        // ================================================================
        // DATA
        // ================================================================

        private DataTable enrollmentsTable;

        private DataView enrollmentsView;

        // ================================================================
        // STATE
        // ================================================================

        private bool isEditMode = false;

        private bool isLoading = true;

        private bool printingReceipt = false;

        private bool isSaving;

        private bool isFinancialEditLocked;

        // ================================================================
        // PRINTING
        // ================================================================
         
        private readonly PrintDocument enrollmentPrintDocument =
            new PrintDocument();

        // ================================================================
        // CONSTRUCTOR
        // ================================================================

        public EnrollmentForm()
        {
            InitializeComponent();

            UIHelper.ApplyStyle(this);

            ConfigureEnrollmentLayout();
            ConfigureEnrollmentGridAppearance();

            DataRefreshNotifier.DataChanged += DataRefreshNotifier_DataChanged;
            Disposed += (sender, e) =>
                DataRefreshNotifier.DataChanged -= DataRefreshNotifier_DataChanged;

            // ------------------------------------------------------------
            // Behavior-only settings.
            // No visual styling or layout manipulation is performed here.
            // ------------------------------------------------------------

            txtSeatNumber.ReadOnly = true;
            txtSeatNumber.TabStop = false;

            txtAcademicYear.ReadOnly = true;

            dtpApplicationDate.MaxDate = DateTime.Today;

            txtSection.DropDownStyle =
                ComboBoxStyle.DropDownList;

            // تطبيق قواعد التحقق من الحقول (فلترة الأحرف، أقصى طول، والتحقق عند
            // المغادرة) دون تغيير تنسيق الشاشة.
            SchoolSystem.Helpers.UIHelper.ApplyFieldValidation(this);

            txtAcademicYear.Leave +=
                txtAcademicYear_Leave;

            txtRegistrationFee.ReadOnly = true;
            txtRegistrationFee.TabStop = false;

            // ------------------------------------------------------------
            // Events
            // ------------------------------------------------------------

            // ------------------------------------------------------------
            // Services
            // ------------------------------------------------------------

            enrollmentService =
                new EnrollmentService();

            studentService =
                new StudentService();

            classService =
                new ClassService();

            sectionService =
                new StudentClassService();

            feePlanService =
                new FeePlanService();

            // ------------------------------------------------------------
            // Printing
            // ------------------------------------------------------------

            enrollmentPrintDocument.PrintPage +=
                enrollmentPrintDocument_PrintPage;

            // ------------------------------------------------------------
            // Output button event wiring only.
            // No styling is performed here.
            // ------------------------------------------------------------

            ConfigureOutputButtons();
        }

        private async void DataRefreshNotifier_DataChanged(
            object sender,
            DataRefreshEventArgs e)
        {
            if (IsDisposed || Disposing || !IsHandleCreated)
                return;

            // إشعارات الخدمات قد تصدر من خيط خلفي؛ جميع عمليات WinForms
            // وإعادة ربط الجداول يجب أن تتم على خيط الواجهة.
            if (InvokeRequired)
            {
                try
                {
                    BeginInvoke(new MethodInvoker(
                        () => DataRefreshNotifier_DataChanged(sender, e)));
                }
                catch (InvalidOperationException)
                {
                    // أُغلقت الشاشة أثناء جدولة التحديث.
                }

                return;
            }

            if (ReferenceEquals(sender, this) ||
                isLoading ||
                isEditMode ||
                isSaving)
            {
                return;
            }

            bool related =
                DataRefreshNotifier.IsRelated(e, "Enrollment") ||
                DataRefreshNotifier.IsRelated(e, "ClassAssignment") ||
                DataRefreshNotifier.IsRelated(e, "Students") ||
                DataRefreshNotifier.IsRelated(e, "Fees") ||
                DataRefreshNotifier.IsRelated(e, "Vouchers") ||
                DataRefreshNotifier.IsRelated(e, "Classes") ||
                DataRefreshNotifier.IsRelated(e, "Sections") ||
                DataRefreshNotifier.IsRelated(e, "FeePlans") ||
                DataRefreshNotifier.IsRelated(e, "AcademicYears");

            if (!related)
                return;

            try
            {
                isLoading = true;

                int displayedEnrollmentId;
                if (!int.TryParse(txtEnrollmentID.Text.Trim(), out displayedEnrollmentId) ||
                    displayedEnrollmentId <= 0)
                {
                    displayedEnrollmentId = 0;
                }

                LoadStudentChoicesForAcademicYear(
                    txtAcademicYear.Text,
                    displayedEnrollmentId,
                    TryGetSelectedId(cmbStudentID));

                LoadData();
                await LoadSectionsAsync();
                await LoadRegistrationFeePlanAsync(false);
            }
            catch (Exception ex)
            {
                ApplicationLogger.LogException(
                    "شاشة القيد: تحديث تلقائي للبيانات المرتبطة",
                    ex);
            }
            finally
            {
                isLoading = false;
            }
        }

        // ================================================================
        // PRESENTATION LAYOUT
        // ================================================================

        private void ConfigureEnrollmentLayout()
        {
            SuspendLayout();

            try
            {
                layoutMain.Dock = DockStyle.Fill;

                panelTitle.Dock = DockStyle.Fill;
                panelTitle.Height = 54;

                pnlSearch.Dock = DockStyle.Fill;
                pnlSearch.Height = 66;
                pnlSearch.Visible = true;

                splitContainerMain.Dock = DockStyle.Fill;

                pnlButtons.Dock = DockStyle.Fill;
                pnlButtons.Height = 120;
                pnlButtons.AutoScroll = true;
                pnlButtons.WrapContents = true;

                // Keep the two-row action bar fully visible on shorter screens.
                if (layoutMain.RowStyles.Count > 3)
                    layoutMain.RowStyles[3].Height = 120F;

                foreach (Control actionControl in pnlButtons.Controls)
                {
                    Button actionButton = actionControl as Button;
                    if (actionButton != null)
                        actionButton.Margin = new Padding(4, 2, 4, 2);
                }

                txtSearch.Visible = true;
                btnSearch.Visible = true;
                btnReload.Visible = true;
                lblSearch.Visible = true;
                lblCount.Visible = true;
                tlpSearch.Dock = DockStyle.Fill;

                PerformLayout();
            }
            finally
            {
                ResumeLayout(true);
            }
        }

        // جدول التسجيل بأسلوب Excel موحّد مع ملف الطالب: خلايا واضحة، صفوف متناوبة
        // وتحديد بلون الهوية مع عرض أعمدة طبيعي وتمرير أفقي عند كثرة البيانات.
        private void ConfigureEnrollmentGridAppearance()
        {
            if (dgvEnrollments == null)
                return;

            dgvEnrollments.SuspendLayout();
            try
            {
                dgvEnrollments.AllowUserToAddRows = false;
                dgvEnrollments.AllowUserToDeleteRows = false;
                dgvEnrollments.AllowUserToResizeRows = false;
                dgvEnrollments.AllowUserToResizeColumns = true;
                dgvEnrollments.AllowUserToOrderColumns = true;
                dgvEnrollments.ReadOnly = true;
                dgvEnrollments.EditMode = DataGridViewEditMode.EditProgrammatically;
                dgvEnrollments.MultiSelect = false;
                dgvEnrollments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvEnrollments.RowHeadersVisible = false;
                dgvEnrollments.RightToLeft = RightToLeft.Yes;
                dgvEnrollments.ScrollBars = ScrollBars.Both;
                dgvEnrollments.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                dgvEnrollments.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
                dgvEnrollments.BackgroundColor = Color.White;
                dgvEnrollments.BorderStyle = BorderStyle.FixedSingle;
                dgvEnrollments.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                dgvEnrollments.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                dgvEnrollments.EnableHeadersVisualStyles = false;
                dgvEnrollments.GridColor = UIHelper.BorderColor;
                dgvEnrollments.ColumnHeadersHeight = 44;
                dgvEnrollments.ColumnHeadersHeightSizeMode =
                    DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                dgvEnrollments.RowTemplate.Height = 38;
                dgvEnrollments.ClipboardCopyMode =
                    DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;

                dgvEnrollments.ColumnHeadersDefaultCellStyle.BackColor = UIHelper.PrimaryColor;
                dgvEnrollments.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgvEnrollments.ColumnHeadersDefaultCellStyle.Font =
                    new Font(UIHelper.FontFamily, 10F, FontStyle.Bold);
                dgvEnrollments.ColumnHeadersDefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleCenter;
                dgvEnrollments.ColumnHeadersDefaultCellStyle.WrapMode =
                    DataGridViewTriState.True;
                dgvEnrollments.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                    UIHelper.PrimaryColor;
                dgvEnrollments.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
                dgvEnrollments.ColumnHeadersDefaultCellStyle.Padding =
                    new Padding(8, 4, 8, 4);

                dgvEnrollments.DefaultCellStyle.BackColor = Color.White;
                dgvEnrollments.DefaultCellStyle.ForeColor = UIHelper.TextColor;
                dgvEnrollments.DefaultCellStyle.Font =
                    new Font(UIHelper.FontFamily, 9.5F, FontStyle.Regular);
                dgvEnrollments.DefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleRight;
                dgvEnrollments.DefaultCellStyle.SelectionBackColor = UIHelper.AccentColor;
                dgvEnrollments.DefaultCellStyle.SelectionForeColor = Color.White;
                dgvEnrollments.DefaultCellStyle.Padding = new Padding(8, 4, 8, 4);
                dgvEnrollments.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

                dgvEnrollments.AlternatingRowsDefaultCellStyle.BackColor =
                    Color.FromArgb(241, 245, 249);
                dgvEnrollments.AlternatingRowsDefaultCellStyle.ForeColor = UIHelper.TextColor;
                dgvEnrollments.AlternatingRowsDefaultCellStyle.SelectionBackColor =
                    UIHelper.AccentColor;
                dgvEnrollments.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.White;
                dgvEnrollments.RowsDefaultCellStyle.BackColor = Color.White;
                dgvEnrollments.RowsDefaultCellStyle.ForeColor = UIHelper.TextColor;
                dgvEnrollments.RowsDefaultCellStyle.SelectionBackColor = UIHelper.AccentColor;
                dgvEnrollments.RowsDefaultCellStyle.SelectionForeColor = Color.White;
            }
            finally
            {
                dgvEnrollments.ResumeLayout();
            }
        }

        // ================================================================
        // OUTPUT BUTTON EVENTS
        // ================================================================

        private void ConfigureOutputButtons()
        {
            // IMPORTANT:
            // This method intentionally performs event wiring only.
            // It does NOT change:
            // - BackColor
            // - ForeColor
            // - Font
            // - Size
            // - Location
            // - Dock
            // - Anchor
            // - Padding
            // - BorderStyle
            // - RightToLeft
            // - FlatStyle

            btnPreviewOutput.Click -=
                btnPreviewOutput_Click;

            btnExportPdf.Click -=
                btnExportPdf_Click;

            btnExportExcel.Click -=
                btnExportExcel_Click;

            btnPreviewOutput.Click +=
                btnPreviewOutput_Click;

            btnExportPdf.Click +=
                btnExportPdf_Click;

            btnExportExcel.Click +=
                btnExportExcel_Click;
        }

        // ================================================================
        // PREVIEW
        // ================================================================

        private void btnPreviewOutput_Click(
            object sender,
            EventArgs e)
        {
            PreviewSelectedEnrollment(false);
        }

        // ================================================================
        // EXPORT PDF
        // ================================================================

        private void btnExportPdf_Click(
            object sender,
            EventArgs e)
        {
            ExportEnrollment(false, true);
        }

        // ================================================================
        // EXPORT EXCEL
        // ================================================================

        private void btnExportExcel_Click(
            object sender,
            EventArgs e)
        {
            ExportEnrollment(false, false);
        }

        // ================================================================
        // BUILD OUTPUT TABLE
        // ================================================================

        private DataTable BuildEnrollmentOutputTable(
            bool receipt)
        {
            DataTable table =
                new DataTable();

            table.Columns.Add("الحقل");
            table.Columns.Add("القيمة");

            table.Rows.Add(
                "رقم الطلب",
                txtEnrollmentID.Text);

            table.Rows.Add(
                "اسم الطالب",
                txtStudentName.Text);

            table.Rows.Add(
                "العام الدراسي",
                txtAcademicYear.Text);

            table.Rows.Add(
                "الصف",
                cmbClassID.Text);

            table.Rows.Add(
                "الشعبة",
                txtSection.Text);

            table.Rows.Add(
                "تاريخ التسجيل",
                dtpApplicationDate.Value.ToString(
                    "yyyy/MM/dd"));

            table.Rows.Add(
                "الحالة",
                cmbStatus.Text);

            if (receipt)
            {
                table.Rows.Add(
                    "رسوم التسجيل",
                    txtRegistrationFee.Text);

                table.Rows.Add(
                    "المبلغ المدفوع",
                    txtPaidAmount.Text);

                table.Rows.Add(
                    "المتبقي",
                    txtRemainingAmount.Text);

                table.Rows.Add(
                    "طريقة الدفع",
                    cmbPaymentMethod.Text);

                table.Rows.Add(
                    "رقم السند",
                    txtReceiptNo.Text);
            }

            return table;
        }

        // ================================================================
        // PREVIEW SELECTED ENROLLMENT
        // ================================================================

        private void PreviewSelectedEnrollment(
            bool receipt)
        {
            try
            {
                CurrentUser.DemandAction(
                    "Enrollment",
                    "Print",
                    "ليس لديك صلاحية معاينة وطباعة بيانات التسجيل.");
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("صلاحية الطباعة", ex);
                return;
            }

            if (!CanPrintSelectedEnrollment())
                return;

            printingReceipt = receipt;

            try
            {
                using (PrintPreviewDialog preview =
                       new PrintPreviewDialog())
                {
                    preview.Document =
                        enrollmentPrintDocument;

                    preview.RightToLeft =
                        RightToLeft.Yes;

                    preview.WindowState =
                        FormWindowState.Maximized;

                    preview.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowException(
                    "معاينة استمارة التسجيل",
                    ex);
            }
        }

        // ================================================================
        // EXPORT ENROLLMENT
        // ================================================================

        private void ExportEnrollment(
            bool receipt,
            bool pdf)
        {
            try
            {
                CurrentUser.DemandAction(
                    "Enrollment",
                    pdf ? "ExportPDF" : "ExportExcel",
                    pdf
                        ? "ليس لديك صلاحية تصدير التسجيل إلى PDF."
                        : "ليس لديك صلاحية تصدير التسجيل إلى Excel.");
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("صلاحية التصدير", ex);
                return;
            }

            if (!CanPrintSelectedEnrollment())
                return;

            using (SaveFileDialog dialog =
                   new SaveFileDialog())
            {
                dialog.Filter =
                    pdf
                        ? "ملفات PDF (*.pdf)|*.pdf"
                        : "ملفات Excel (*.xlsx)|*.xlsx";

                dialog.FileName =
                    (receipt
                        ? "إيصال_تسجيل_"
                        : "استمارة_تسجيل_") +

                    DateTime.Now.ToString(
                        "yyyyMMdd_HHmm") +

                    (pdf
                        ? ".pdf"
                        : ".xlsx");

                if (dialog.ShowDialog(this) !=
                    DialogResult.OK)
                {
                    return;
                }

                try
                {
                    DataTable table =
                        BuildEnrollmentOutputTable(
                            receipt);

                    string title =
                        receipt
                            ? "إيصال تسجيل طالب"
                            : "استمارة تسجيل طالب";

                    string summary =
                        "رقم الطلب: " +
                        txtEnrollmentID.Text;

                    if (pdf)
                    {
                        ReportOutputHelper.ExportToPdf(
                            table,
                            dialog.FileName,
                            title,
                            summary);
                    }
                    else
                    {
                        ReportOutputHelper.ExportToExcel(
                            table,
                            dialog.FileName,
                            title,
                            summary);
                    }

                    UIHelper.ShowInfo(
                        pdf
                            ? "تم تصدير استمارة التسجيل إلى PDF بنجاح."
                            : "تم تصدير استمارة التسجيل إلى Excel بنجاح.");
                }
                catch (Exception ex)
                {
                    UIHelper.ShowException(
                        "تصدير استمارة التسجيل",
                        ex);
                }
            }
        }

        // ================================================================
        // FORM LOAD
        // ================================================================

        private async void EnrollmentForm_Load(
            object sender,
            EventArgs e)
        {
            LoadComboBoxes();

            LoadData();

            DisableInputs();

            isLoading = false;

            await LoadSectionsAsync();

            await LoadRegistrationFeePlanAsync(false);
        }

        // ================================================================
        // AUTOMATIC ACADEMIC YEAR
        // ================================================================

        private void SetAutomaticAcademicYear()
        {
            if (isEditMode)
                return;

            string dbYear =
                SchoolSystem.Helpers.AcademicYearHelper
                    .GetCurrentAcademicYear();

            if (!string.IsNullOrWhiteSpace(dbYear))
            {
                txtAcademicYear.Text =
                    dbYear;

                return;
            }

            DateTime date =
                dtpApplicationDate.Value.Date;

            int startYear =
                date.Month >= 8
                    ? date.Year
                    : date.Year - 1;

            txtAcademicYear.Text =
                startYear +
                "/" +
                (startYear + 1);
        }

        // ================================================================
        // APPLICATION DATE CHANGED
        // ================================================================

        private async void dtpApplicationDate_ValueChanged(
            object sender,
            EventArgs e)
        {
            if (isLoading || isEditMode)
                return;

            SetAutomaticAcademicYear();

            if (string.Equals(
                    txtEnrollmentID.Text,
                    "جديد",
                    StringComparison.Ordinal))
            {
                int currentlySelectedStudentId =
                    TryGetSelectedId(cmbStudentID);

                LoadStudentChoicesForAcademicYear(
                    txtAcademicYear.Text,
                    0,
                    currentlySelectedStudentId);
            }

            await LoadSectionsAsync();

            await LoadRegistrationFeePlanAsync(false);
        }

        /// <summary>
        /// Rebuilds the student selector against the academic year. During
        /// editing, only the current registration is excluded from duplicate
        /// detection so its student remains selectable in the existing record.
        /// </summary>
        private DataTable LoadStudentChoicesForAcademicYear(
            string academicYear,
            int excludeEnrollmentId = 0,
            int selectedStudentId = 0)
        {
            bool wasLoading = isLoading;
            isLoading = true;

            try
            {
                DataTable students =
                    studentService.GetStudentsAvailableForEnrollment(
                        academicYear,
                        excludeEnrollmentId);

                cmbStudentID.DataSource = null;
                cmbStudentID.DisplayMember = "FullName";
                cmbStudentID.ValueMember = "StudentId";
                cmbStudentID.DataSource = students;
                cmbStudentID.SelectedIndex = -1;
                txtStudentName.Clear();

                if (selectedStudentId > 0 &&
                    students.Columns.Contains("StudentId") &&
                    students.AsEnumerable().Any(
                        row => Convert.ToInt32(row["StudentId"]) == selectedStudentId))
                {
                    cmbStudentID.SelectedValue = selectedStudentId;
                    DataRowView selectedStudent =
                        cmbStudentID.SelectedItem as DataRowView;

                    if (selectedStudent != null)
                        txtStudentName.Text = Convert.ToString(selectedStudent["FullName"]);
                }

                return students;
            }
            finally
            {
                isLoading = wasLoading;
            }
        }

        // ================================================================
        // LOAD COMBO BOXES
        // ================================================================

        private void LoadComboBoxes()
        {
            try
            {
                 SetAutomaticAcademicYear();

                // --------------------------------------------------------
                // Students
                // --------------------------------------------------------

                LoadStudentChoicesForAcademicYear(txtAcademicYear.Text);

                // --------------------------------------------------------
                // Classes
                // --------------------------------------------------------

                DataTable dtClasses =
                    classService.GetAllClasses();

                if (dtClasses != null &&
                    dtClasses.Rows.Count > 0)
                {
                    cmbClassID.DataSource =
                        dtClasses;

                    cmbClassID.DisplayMember =
                        "ClassName";

                    cmbClassID.ValueMember =
                        "ClassID";

                    cmbClassID.SelectedIndex =
                        -1;
                }

                // --------------------------------------------------------
                // Sections
                // --------------------------------------------------------

                txtSection.DataSource =
                    null;

                txtSection.Items.Clear();

                txtSection.Enabled =
                    false;

                // --------------------------------------------------------
                // Application type
                // --------------------------------------------------------

                cmbApplicationType.Items.Clear();

                cmbApplicationType.Items.AddRange(
                    new string[]
                    {
                        "طالب جديد",
                        "منقول",
                        "إعادة قيد"
                    });

                // --------------------------------------------------------
                // Status
                // --------------------------------------------------------

                cmbStatus.Items.Clear();

                cmbStatus.Items.AddRange(
                    new string[]
                    {
                        "جديد",
                        "مقبول",
                        "مؤجل",
                        "مرفوض"
                    });

                // --------------------------------------------------------
                // Payment methods
                // --------------------------------------------------------

                cmbPaymentMethod.Items.Clear();

                cmbPaymentMethod.Items.AddRange(
                    new string[]
                    {
                        "نقدي",
                        "حوالة",
                        "تحويل بنكي"
                    });

                if (cmbPaymentMethod.Items.Count > 0)
                {
                    cmbPaymentMethod.SelectedIndex =
                        0;
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowException(
                    "تحميل القوائم",
                    ex);
            }
        }

        // ================================================================
        // LOAD DATA
        // ================================================================

        private void LoadData()
        {
            try
            {
                enrollmentsTable =
                    enrollmentService.GetAllEnrollments();

                if (enrollmentsTable != null)
                {
                    enrollmentsView =
                        enrollmentsTable.DefaultView;

                    dgvEnrollments.DataSource =
                        enrollmentsView;

                    FormatGrid();

                    UpdateCount();
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowException(
                    "تحميل بيانات التسجيل",
                    ex);
            }
        }

        // ================================================================
        // FORMAT GRID
        //
        // NOTE:
        // This method only controls data visibility/headers.
        // It does not apply visual theme styling.
        // ================================================================

        private void FormatGrid()
        {
            if (dgvEnrollments.Columns.Count == 0)
                return;

            foreach (DataGridViewColumn col
                     in dgvEnrollments.Columns)
            {
                col.Visible = false;
            }

            ShowColumn(
                "EnrollmentID",
                "رقم الطلب");

            ShowColumn(
                "StudentName",
                "اسم الطالب");

            ShowColumn(
                "ClassName",
                "الصف");

            ShowColumn(
                "Section",
                "الشعبة");

            ShowColumn(
                "AcademicYear",
                "العام الدراسي");

            ShowColumn(
                "Status",
                "الحالة");

            ShowColumn(
                "RegistrationFee",
                "الرسوم");

            ShowColumn(
                "PaidAmount",
                "المدفوع");

            ShowColumn(
                "RemainingAmount",
                "المتبقي");

            ShowColumn(
                "ApplicationDate",
                "تاريخ التسجيل");

            SetGridColumnLayout("EnrollmentID", 78);
            SetGridColumnLayout("StudentName", 190);
            SetGridColumnLayout("ClassName", 100);
            SetGridColumnLayout("Section", 82);
            SetGridColumnLayout("AcademicYear", 118);
            SetGridColumnLayout("Status", 90);
            SetGridColumnLayout("RegistrationFee", 105);
            SetGridColumnLayout("PaidAmount", 100);
            SetGridColumnLayout("RemainingAmount", 105);
            SetGridColumnLayout("ApplicationDate", 118);

            // لا نضغط الأعمدة في عرض الجدول؛ يبقى التمرير الأفقي متاحًا مثل Excel.
            dgvEnrollments.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.None;
            dgvEnrollments.AutoSizeRowsMode =
                DataGridViewAutoSizeRowsMode.None;
            dgvEnrollments.ColumnHeadersHeight = 44;
            dgvEnrollments.RowTemplate.Height = 38;
        }

        private void SetGridColumnLayout(
            string name,
            float width)
        {
            if (!dgvEnrollments.Columns.Contains(name))
                return;

            DataGridViewColumn column = dgvEnrollments.Columns[name];
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            column.MinimumWidth = Math.Min(80, Math.Max(55, (int)width));
            column.Width = (int)width;
            column.Resizable = DataGridViewTriState.True;
            column.SortMode = DataGridViewColumnSortMode.Automatic;
        }

        // ================================================================
        // SHOW GRID COLUMN
        // ================================================================

        private void ShowColumn(
            string name,
            string header)
        {
            if (dgvEnrollments.Columns.Contains(name))
            {
                dgvEnrollments.Columns[name].Visible =
                    true;

                dgvEnrollments.Columns[name].HeaderText =
                    header;
            }
        }

        // ================================================================
        // COUNT
        // ================================================================

        private void UpdateCount()
        {
            lblCount.Text =
                $"العدد: {(enrollmentsView == null ? 0 : enrollmentsView.Count)}";
        }

        // ================================================================
        // STUDENT SELECTED
        // ================================================================

        private async void cmbStudentID_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            if (isLoading ||
                cmbStudentID.SelectedIndex == -1)
            {
                return;
            }

            DataRowView drv =
                cmbStudentID.SelectedItem as DataRowView;

            if (drv != null)
            {
                txtStudentName.Text =
                    drv["FullName"].ToString();
            }

            await LoadRegistrationFeePlanAsync(false);
        }

        // ================================================================
        // FEES CHANGED
        // ================================================================

        private void txtFees_TextChanged(
            object sender,
            EventArgs e)
        {
            CalculateRemaining();
        }

        // ================================================================
        // CALCULATE REMAINING
        // ================================================================

        private void CalculateRemaining()
        {
            decimal fee =
                ParseAmountOrZero(
                    txtRegistrationFee.Text);

            decimal paid =
                ParseAmountOrZero(
                    txtPaidAmount.Text);

            decimal remaining =
                fee - paid;

            txtRemainingAmount.Text =
                remaining.ToString("0.##");
        }

        // ================================================================
        // SEARCH
        // ================================================================

        private void btnSearch_Click(
            object sender,
            EventArgs e)
        {
            ApplyEnrollmentSearch();
        }

        private void txtSearch_TextChanged(
            object sender,
            EventArgs e)
        {
            ApplyEnrollmentSearch();
        }

        // ================================================================
        // APPLY SEARCH
        // ================================================================

        private void ApplyEnrollmentSearch()
        {
            if (enrollmentsView == null)
            {
                UpdateCount();

                return;
            }

            try
            {
                string[] requestedColumns =
                {
                    "StudentName", "StudentNumber", "ApplicationType", "AcademicYear",
                    "ClassName", "Section", "Status", "PreviousSchool", "PreviousClass",
                    "TransferReason", "PaymentMethod", "ReceiptNo", "Notes"
                };

                var availableColumns = requestedColumns
                    .Where(column =>
                        enrollmentsView.Table != null &&
                        enrollmentsView.Table.Columns.Contains(column))
                    .ToArray();

                enrollmentsView.RowFilter =
                    UIHelper.BuildDataViewSearchFilter(txtSearch.Text, availableColumns);

                UpdateCount();
            }
            catch (Exception ex)
            {
                ApplicationLogger.LogException("شاشة القبول والتسجيل: البحث", ex);
                enrollmentsView.RowFilter = string.Empty;
                UpdateCount();
                UIHelper.ShowException("البحث في القبول والتسجيل", ex);
            }
        }

        // ================================================================
        // SEARCH ENTER
        // ================================================================

        private void txtSearch_KeyDown(
            object sender,
            KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch_Click(
                    sender,
                    e);

                e.SuppressKeyPress = true;
            }
        }

        // ================================================================
        // REFRESH
        // ================================================================

        private void btnClearSearch_Click(
            object sender,
            EventArgs e)
        {
            txtSearch.Clear();

            if (enrollmentsView != null)
            {
                enrollmentsView.RowFilter =
                    string.Empty;
            }

            UpdateCount();

            txtSearch.Focus();
        }

        private void btnRefresh_Click(
            object sender,
            EventArgs e)
        {
            if (isEditMode)
            {
                UIHelper.ShowWarning(
                    "لا يمكن تحديث قائمة التسجيل أثناء تعديل سجل. احفظ أو ألغِ التعديل أولاً.");
                return;
            }

            int displayedEnrollmentId;
            if (!int.TryParse(txtEnrollmentID.Text.Trim(), out displayedEnrollmentId) ||
                displayedEnrollmentId <= 0)
            {
                displayedEnrollmentId = 0;
            }

            try
            {
                LoadStudentChoicesForAcademicYear(
                    txtAcademicYear.Text,
                    displayedEnrollmentId,
                    TryGetSelectedId(cmbStudentID));
                LoadData();
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("تحديث بيانات التسجيل والطلاب المتاحين", ex);
            }
        }

        // ================================================================
        // ADD
        // ================================================================

        private void btnAdd_Click(
            object sender,
            EventArgs e)
        {
            CurrentUser.DemandAction(
                "Enrollment",
                "Add",
                "ليس لديك صلاحية بدء تسجيل جديد.");

            isEditMode = false;

            ClearInputs();

            DataTable availableStudents;
            try
            {
                availableStudents =
                    LoadStudentChoicesForAcademicYear(txtAcademicYear.Text);
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("تحميل الطلاب غير المسجلين للعام الدراسي", ex);
                DisableInputs();
                return;
            }

            EnableInputs();

            txtEnrollmentID.Text =
                "جديد";

            dtpApplicationDate.Value =
                DateTime.Today;

            cmbApplicationType.SelectedItem =
                "طالب جديد";

            cmbStatus.SelectedItem =
                "جديد";

            if (availableStudents.Rows.Count == 0)
            {
                lblStatus.Text =
                    "لا يوجد طلاب نشطون غير مسجلين في العام الدراسي " +
                    txtAcademicYear.Text +
                    ". أضف طالباً أو تحقق من التسجيلات الحالية.";

                UIHelper.ShowWarning(lblStatus.Text);
                return;
            }

            lblStatus.Text =
                "عدد الطلاب المتاحين للتسجيل: " +
                availableStudents.Rows.Count;

            cmbStudentID.Focus();
        }

        // ================================================================
        // SAVE
        // ================================================================

        private void btnSave_Click(
            object sender,
            EventArgs e)
        {
            if (isSaving)
                return;

            isSaving = true;

            btnSave.Enabled =
                false;

            errorProvider1.Clear();

            if (!ValidateInputs())
            {
                isSaving = false;

                btnSave.Enabled =
                    true;

                return;
            }

            try
            {
                CurrentUser.DemandAction(
                    "Enrollment",
                    isEditMode ? "Edit" : "Add",
                    isEditMode
                        ? "ليس لديك صلاحية تعديل التسجيل."
                        : "ليس لديك صلاحية إضافة التسجيل.");

                Enrollment enrollment =
                    new Enrollment
                    {
                        StudentID =
                            GetSelectedId(
                                cmbStudentID,
                                "يجب تحديد طالب صالح."),

                        ApplicationDate =
                            dtpApplicationDate.Value,

                        ApplicationType =
                            cmbApplicationType
                                .SelectedItem?
                                .ToString(),

                        AcademicYear =
                            txtAcademicYear.Text,

                        ClassID =
                            GetSelectedId(
                                cmbClassID,
                                "يجب تحديد فصل صالح."),

                        Section =
                            txtSection.Text.Trim(),

                        SeatNumber =
                            isEditMode
                                ? txtSeatNumber.Text
                                : string.Empty,

                        Status =
                            cmbStatus
                                .SelectedItem?
                                .ToString(),

                        PreviousSchool =
                            txtPreviousSchool.Text,

                        PreviousClass =
                            txtPreviousClass.Text,

                        TransferReason =
                            txtTransferReason.Text,

                        RegistrationFee =
                            ParseAmountOrZero(
                                txtRegistrationFee.Text),

                        PaidAmount =
                            ParseAmountOrZero(
                                txtPaidAmount.Text),

                        PaymentMethod =
                            cmbPaymentMethod
                                .SelectedItem?
                                .ToString(),

                        ReceiptNo =
                            txtReceiptNo.Text,

                        HasBirthCertificate =
                            chkHasBirthCertificate.Checked,

                        HasGuardianId =
                            chkHasGuardianId.Checked,

                        HasPhoto =
                            chkHasPhoto.Checked,

                        HasLastCertificate =
                            chkHasLastCertificate.Checked,

                        HasMedicalReport =
                            chkHasMedicalReport.Checked,

                        Notes =
                            rtbNotes.Text
                    };

                bool success;

                if (isEditMode)
                {
                    int enrollmentId;

                    if (!int.TryParse(
                            txtEnrollmentID.Text.Trim(),
                            out enrollmentId) ||
                        enrollmentId <= 0)
                    {
                        errorProvider1.SetError(
                            txtEnrollmentID,
                            "رقم طلب التسجيل غير صالح.");

                        return;
                    }

                    enrollment.EnrollmentID =
                        enrollmentId;

                    success =
                        enrollmentService
                            .UpdateEnrollment(
                                enrollment);
                }
                else
                {
                    success =
                        enrollmentService
                            .AddEnrollment(
                                enrollment);
                }

                if (success)
                {
                    if (enrollment.EnrollmentID > 0)
                        txtEnrollmentID.Text = enrollment.EnrollmentID.ToString();

                    if (!string.IsNullOrWhiteSpace(enrollment.SeatNumber))
                        txtSeatNumber.Text = enrollment.SeatNumber;

                    isEditMode = false;
                    DisableInputs();

                    LoadData();
                    DataRefreshNotifier.Notify("All", this);

                    try
                    {
                        LoadStudentChoicesForAcademicYear(
                            enrollment.AcademicYear,
                            enrollment.EnrollmentID,
                            enrollment.StudentID);
                    }
                    catch (Exception refreshEx)
                    {
                        ApplicationLogger.LogException(
                            "شاشة القيد: تحديث قائمة الطلاب بعد الحفظ",
                            refreshEx);
                        UIHelper.ShowWarning(
                            "تم حفظ التسجيل، لكن تعذر تحديث قائمة الطلاب المتاحين. استخدم زر تحديث البيانات.");
                    }

                    UIHelper.ShowInfo("تم الحفظ بنجاح.");
                }
            }
            catch (Exception ex)
            {
                ApplicationLogger.LogException("شاشة القيد: حفظ التسجيل", ex);
                if (ex.Message.IndexOf(
                        "مسجل بالفعل",
                        StringComparison.OrdinalIgnoreCase)
                    >= 0)
                {
                    UIHelper.ShowWarning(
                        "لا يمكن حفظ التسجيل: هذا الطالب لديه تسجيل غير مرفوض في العام الدراسي " +
                        txtAcademicYear.Text +
                        ". اختر تسجيلاً جديداً لعام مختلف أو عدّل التسجيل الموجود.");
                }
                else
                {
                    UIHelper.ShowException(
                        "حفظ التسجيل",
                        ex);
                }
            }
            finally
            {
                isSaving = false;

                // لا نعيد تفعيل الحفظ بعد نجاح العملية وإغلاق وضع الإدخال.
                btnSave.Enabled =
                    isEditMode ||
                    string.Equals(
                        txtEnrollmentID.Text.Trim(),
                        "جديد",
                        StringComparison.Ordinal);
            }
        }

        // ================================================================
        // UPDATE
        // ================================================================

        private void btnUpdate_Click(
            object sender,
            EventArgs e)
        {
            CurrentUser.DemandAction(
                "Enrollment",
                "Edit",
                "ليس لديك صلاحية تعديل التسجيل.");

            if (string.IsNullOrEmpty(
                    txtEnrollmentID.Text) ||
                txtEnrollmentID.Text == "جديد")
            {
                UIHelper.ShowWarning(
                    "الرجاء تحديد طلب من الجدول أولاً.");

                return;
            }

            isEditMode =
                true;

            EnableInputs();

            ApplyFinancialEditLock();
        }

        // ================================================================
        // FINANCIAL EDIT LOCK
        // ================================================================

        private void ApplyFinancialEditLock()
        {
            int studentId = TryGetSelectedId(cmbStudentID);
            string academicYear = (txtAcademicYear.Text ?? string.Empty).Trim();

            bool hasFinance = false;

            try
            {
                hasFinance =
                    studentId > 0 &&
                    IsSequentialAcademicYear(academicYear) &&
                    enrollmentService.HasRegistrationFinance(
                        studentId,
                        academicYear);
            }
            catch (Exception ex)
            {
                ApplicationLogger.LogException(
                    "شاشة القيد: التحقق من الحركة المالية قبل التعديل",
                    ex);
            }

            isFinancialEditLocked = hasFinance;

            if (!hasFinance)
                return;

            // الطالب والعام وهوية الحركة المالية تبقى ثابتة بعد إنشاء الحركة.
            // الصف والشعبة مسموحان بالتعديل لأن الحفظ سيزامنهما تلقائياً
            // مع سجل التوزيع المرتبط إن كان موجوداً.
            cmbStudentID.Enabled = false;
            txtAcademicYear.Enabled = false;

            txtRegistrationFee.Enabled = false;
            txtPaidAmount.Enabled = false;
            cmbPaymentMethod.Enabled = false;
            txtReceiptNo.Enabled = false;

            cmbClassID.Enabled = true;
            txtSection.Enabled = true;

            lblStatus.Text =
                "السجل مرتبط بحركة مالية؛ الرسوم والسداد والطالب والعام الدراسي مقفلة، وتعديل الصف أو الشعبة سيُزامن تلقائياً مع التوزيع.";

            errorProvider1.SetError(
                txtRegistrationFee,
                "تُدار الرسوم من شاشة الرسوم بعد إنشاء الحركة المالية.");

            errorProvider1.SetError(
                txtPaidAmount,
                "يُدار السداد من شاشة الرسوم والسندات.");

            errorProvider1.SetError(
                cmbStudentID,
                "لا يمكن تغيير الطالب بعد إنشاء الحركة المالية.");

            errorProvider1.SetError(
                cmbClassID,
                "لا يمكن تغيير الصف بعد إنشاء الحركة المالية.");
        }

        // ================================================================
        // DELETE
        // ================================================================

        private void btnDelete_Click(
            object sender,
            EventArgs e)
        {
            CurrentUser.DemandAction(
                "Enrollment",
                "Delete",
                "ليس لديك صلاحية حذف التسجيل.");

            if (string.IsNullOrEmpty(
                    txtEnrollmentID.Text) ||
                txtEnrollmentID.Text == "جديد")
            {
                UIHelper.ShowWarning(
                    "الرجاء تحديد طلب من الجدول أولاً.");

                return;
            }

            if (UIHelper.ShowConfirmation(
                    "هل أنت متأكد من حذف طلب التسجيل المحدد؟",
                    "تأكيد الحذف"))
            {
                try
                {
                    int id;

                    if (!int.TryParse(
                            txtEnrollmentID.Text.Trim(),
                            out id) ||
                        id <= 0)
                    {
                        errorProvider1.SetError(
                            txtEnrollmentID,
                            "رقم طلب التسجيل غير صالح.");

                        return;
                    }

                    if (enrollmentService
                        .DeleteEnrollment(id))
                    {
                        UIHelper.ShowInfo(
                            "تم الحذف بنجاح.");

                        LoadData();

                        ClearInputs();
                        isEditMode = false;

                        try
                        {
                            LoadStudentChoicesForAcademicYear(txtAcademicYear.Text);
                        }
                        catch (Exception refreshEx)
                        {
                            ApplicationLogger.LogException(
                                "شاشة القيد: تحديث قائمة الطلاب بعد حذف الطلب",
                                refreshEx);
                        }

                        DisableInputs();
                        DataRefreshNotifier.Notify("All", this);
                    }
                }
                catch (Exception ex)
                {
                    UIHelper.ShowException(
                        "حذف التسجيل",
                        ex);
                }
            }
        }

        // ================================================================
        // CANCEL
        // ================================================================

        private void btnCancel_Click(
            object sender,
            EventArgs e)
        {
            isEditMode = false;
            ClearInputs();

            try
            {
                LoadStudentChoicesForAcademicYear(txtAcademicYear.Text);
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("تحديث قائمة الطلاب بعد إلغاء الإدخال", ex);
            }

            DisableInputs();
        }

        // ================================================================
        // CLOSE
        // ================================================================

        private void btnClose_Click(
            object sender,
            EventArgs e)
        {
            Close();
        }

        // ================================================================
        // CAN PRINT
        // ================================================================

        private bool CanPrintSelectedEnrollment()
        {
            if (string.IsNullOrWhiteSpace(
                    txtEnrollmentID.Text) ||
                txtEnrollmentID.Text == "جديد")
            {
                UIHelper.ShowWarning(
                    "رجاءً حدد طلبًا قبل الطباعة.");

                return false;
            }

            return true;
        }

        // ================================================================
        // PRINT
        // ================================================================

        private void PrintSelectedEnrollment(
            bool receipt)
        {
            CurrentUser.DemandAction(
                "Enrollment",
                "Print",
                "ليس لديك صلاحية طباعة بيانات التسجيل.");

            if (!CanPrintSelectedEnrollment())
                return;

            printingReceipt =
                receipt;

            using (PrintDialog dialog =
                   new PrintDialog())
            {
                dialog.Document =
                    enrollmentPrintDocument;

                dialog.UseEXDialog =
                    true;

                if (dialog.ShowDialog(this) ==
                    DialogResult.OK)
                {
                    try
                    {
                        enrollmentPrintDocument.Print();
                    }
                    catch (Exception ex)
                    {
                        UIHelper.ShowException(
                            "طباعة تقرير التسجيل",
                            ex);
                    }
                }
            }
        }

        // ================================================================
        // PRINT FORM
        // ================================================================

        private void btnPrintForm_Click(
            object sender,
            EventArgs e)
        {
            PrintSelectedEnrollment(false);
        }

        // ================================================================
        // PRINT RECEIPT
        // ================================================================

        private void btnPrintReceipt_Click(
            object sender,
            EventArgs e)
        {
            PrintSelectedEnrollment(true);
        }

        // ================================================================
        // PRINT PAGE
        // ================================================================

        private void enrollmentPrintDocument_PrintPage(
            object sender,
            PrintPageEventArgs e)
        {
            Rectangle bounds =
                e.MarginBounds;

            bool isRtl =
                true;

            using (Font titleFont =
                   new Font(
                       "Tahoma",
                       16F,
                       FontStyle.Bold))

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

            using (StringFormat rtl =
                   new StringFormat
                   {
                       Alignment =
                           isRtl
                               ? StringAlignment.Far
                               : StringAlignment.Near,

                       LineAlignment =
                           StringAlignment.Center,

                       FormatFlags =
                           isRtl
                               ? StringFormatFlags.DirectionRightToLeft
                               : StringFormatFlags.FitBlackBox,

                       Trimming =
                           StringTrimming.EllipsisCharacter
                   })
            {
                int y =
                    ReportOutputHelper.DrawSchoolPrintHeader(
                        e.Graphics,
                        bounds,
                        printingReceipt
                            ? "إيصال تسجيل طالب"
                            : "استمارة تسجيل طالب",
                        isRtl);

                DrawPrintLine(
                    e.Graphics,
                    bounds,
                    ref y,
                    "رقم الطلب",
                    txtEnrollmentID.Text,
                    labelFont,
                    valueFont,
                    rtl);

                DrawPrintLine(
                    e.Graphics,
                    bounds,
                    ref y,
                    "اسم الطالب",
                    txtStudentName.Text,
                    labelFont,
                    valueFont,
                    rtl);

                DrawPrintLine(
                    e.Graphics,
                    bounds,
                    ref y,
                    "العام الدراسي",
                    txtAcademicYear.Text,
                    labelFont,
                    valueFont,
                    rtl);

                DrawPrintLine(
                    e.Graphics,
                    bounds,
                    ref y,
                    "الصف",
                    cmbClassID.Text,
                    labelFont,
                    valueFont,
                    rtl);

                DrawPrintLine(
                    e.Graphics,
                    bounds,
                    ref y,
                    "الشعبة",
                    txtSection.Text,
                    labelFont,
                    valueFont,
                    rtl);

                DrawPrintLine(
                    e.Graphics,
                    bounds,
                    ref y,
                    "تاريخ التسجيل",
                    dtpApplicationDate.Value.ToString(
                        "yyyy/MM/dd"),
                    labelFont,
                    valueFont,
                    rtl);

                DrawPrintLine(
                    e.Graphics,
                    bounds,
                    ref y,
                    "الحالة",
                    cmbStatus.Text,
                    labelFont,
                    valueFont,
                    rtl);
                DrawPrintLine(
                     e.Graphics,
                     bounds,
                     ref y,
                     "رسوم التسجيل",
                     txtRegistrationFee.Text,
                     labelFont,
                     valueFont,
                     rtl);

                DrawPrintLine(
                    e.Graphics,
                    bounds,
                    ref y,
                    "المبلغ المدفوع",
                    txtPaidAmount.Text,
                    labelFont,
                    valueFont,
                    rtl);

                DrawPrintLine(
                    e.Graphics,
                    bounds,
                    ref y,
                    "المتبقي",
                    txtRemainingAmount.Text,
                    labelFont,
                    valueFont,
                    rtl);

                DrawPrintLine(
                    e.Graphics,
                    bounds,
                    ref y,
                    "طريقة الدفع",
                    cmbPaymentMethod.Text,
                    labelFont,
                    valueFont,
                    rtl);

                DrawPrintLine(
                    e.Graphics,
                    bounds,
                    ref y,
                    "رقم السند",
                    txtReceiptNo.Text,
                    labelFont,
                    valueFont,
                    rtl);

                if (printingReceipt)
                {
                    DrawPrintLine(
                        e.Graphics,
                        bounds,
                        ref y,
                        "رسوم التسجيل",
                        txtRegistrationFee.Text,
                        labelFont,
                        valueFont,
                        rtl);

                    DrawPrintLine(
                        e.Graphics,
                        bounds,
                        ref y,
                        "المبلغ المدفوع",
                        txtPaidAmount.Text,
                        labelFont,
                        valueFont,
                        rtl);

                    DrawPrintLine(
                        e.Graphics,
                        bounds,
                        ref y,
                        "المتبقي",
                        txtRemainingAmount.Text,
                        labelFont,
                        valueFont,
                        rtl);

                    DrawPrintLine(
                        e.Graphics,
                        bounds,
                        ref y,
                        "طريقة الدفع",
                        cmbPaymentMethod.Text,
                        labelFont,
                        valueFont,
                        rtl);

                    DrawPrintLine(
                        e.Graphics,
                        bounds,
                        ref y,
                        "رقم السند",
                        txtReceiptNo.Text,
                        labelFont,
                        valueFont,
                        rtl);
                }

                y += 20;

                e.Graphics.DrawString(
                    "التوقيع: ____________________",
                    valueFont,
                    Brushes.Black,
                    new RectangleF(
                        bounds.Left,
                        y,
                        bounds.Width,
                        30),
                    rtl);
            }
        }

        // ================================================================
        // PRINT LINE
        // ================================================================

        private static void DrawPrintLine(
            Graphics graphics,
            Rectangle bounds,
            ref int y,
            string label,
            string value,
            Font labelFont,
            Font valueFont,
            StringFormat rtl)
        {
            float labelWidth =
                bounds.Width * 0.35F;

            graphics.DrawString(
                label + ":",
                labelFont,
                Brushes.Black,
                new RectangleF(
                    bounds.Right - labelWidth,
                    y,
                    labelWidth,
                    28),
                rtl);

            graphics.DrawString(
                string.IsNullOrWhiteSpace(value)
                    ? "-"
                    : value,
                valueFont,
                Brushes.Black,
                new RectangleF(
                    bounds.Left,
                    y,
                    bounds.Width - labelWidth,
                    28),
                rtl);

            y += 32;
        }

        // ================================================================
        // GRID CELL CLICK
        // ================================================================

        private void dgvEnrollments_CellClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                LoadRecordToScreen(
                    dgvEnrollments.Rows[
                        e.RowIndex]);
            }
        }

        // ================================================================
        // GRID SELECTION CHANGED
        // ================================================================

        private void dgvEnrollments_SelectionChanged(
            object sender,
            EventArgs e)
        {
            if (dgvEnrollments.CurrentRow != null &&
                !dgvEnrollments.CurrentRow.IsNewRow)
            {
                LoadRecordToScreen(
                    dgvEnrollments.CurrentRow);
            }
        }

        // ================================================================
        // DATE RANGE
        // ================================================================

        private static readonly DateTime DtpMin =
            new DateTime(
                1900,
                4,
                30);

        private static readonly DateTime DtpMax =
            new DateTime(
                2077,
                11,
                16);

        // ================================================================
        // SAFE DATE
        // ================================================================

        private void SafeSetDate(
            DateTimePicker dtp,
            object value)
        {
            try
            {
                if (value == null ||
                    value == DBNull.Value)
                {
                    dtp.Value =
                        DateTime.Today;

                    return;
                }

                DateTime dt =
                    Convert.ToDateTime(value);

                if (dt < DtpMin ||
                    dt > DtpMax)
                {
                    dt =
                        DateTime.Today;
                }

                dtp.Value =
                    dt;
            }
            catch (Exception ex)
            {
                ApplicationLogger.LogException("شاشة القيد: ضبط التاريخ بأمان", ex);
                dtp.Value =
                    DateTime.Today;
            }
        }

        // ================================================================
        // SAFE GRID CELL
        // ================================================================

        private string SafeCell(
            DataGridViewRow row,
            string colName)
        {
            if (row == null ||
                row.DataGridView == null ||
                !row.DataGridView.Columns.Contains(
                    colName))
            {
                return "";
            }

            return row.Cells[colName].Value?
                       .ToString()
                   ?? "";
        }

        // ================================================================
        // SAFE BOOLEAN GRID CELL
        // ================================================================

        private bool SafeBoolCell(
            DataGridViewRow row,
            string colName)
        {
            if (row == null ||
                row.DataGridView == null ||
                !row.DataGridView.Columns.Contains(
                    colName))
            {
                return false;
            }

            object value =
                row.Cells[colName].Value;

            return value != DBNull.Value &&
                   value != null &&
                   Convert.ToBoolean(value);
        }

        // ================================================================
        // CLASS CHANGED
        // ================================================================

        private async void cmbClassID_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            if (isLoading)
                return;

            await LoadSectionsAsync();

            // عند تعديل سجل عادي، تتغير خطة رسوم التسجيل مع الصف.
            // عند وجود حركة مالية، نحافظ على المبلغ التاريخي ولا نعيد حسابه.
            if (!isFinancialEditLocked)
            {
                await LoadRegistrationFeePlanAsync(false);
            }
        }

        // ================================================================
        // ACADEMIC YEAR LEAVE
        // ================================================================

        private async void txtAcademicYear_Leave(
            object sender,
            EventArgs e)
        {
            if (isLoading)
                return;

            await LoadSectionsAsync();

            await LoadRegistrationFeePlanAsync(false);
        }

        // ================================================================
        // LOAD REGISTRATION FEE PLAN
        // ================================================================

        private async Task LoadRegistrationFeePlanAsync(
            bool showMissingMessage)
        {
            if (isLoading ||
                feePlanService == null)
            {
                return;
            }

            int studentId =
                TryGetSelectedId(
                    cmbStudentID);

            int classId =
                TryGetSelectedId(
                    cmbClassID);

            string academicYear =
                (txtAcademicYear.Text ??
                 string.Empty).Trim();

            txtRegistrationFee.Text =
                string.Empty;

            CalculateRemaining();

            if (studentId <= 0 ||
                classId <= 0 ||
                !IsSequentialAcademicYear(
                    academicYear))
            {
                return;
            }

            try
            {
                FeePlan plan =
                    await Task.Run(
                        () =>
                            feePlanService
                                .GetRegistrationFeePlan(
                                    studentId,
                                    academicYear,
                                    classId));

                if (plan == null)
                {
                    if (showMissingMessage)
                    {
                        UIHelper.ShowWarning(
                            "لا توجد خطة رسوم تسجيل لهذا الصف والعام الدراسي. أضفها أولاً من واجهة خطط الرسوم.");
                    }

                    return;
                }

                txtRegistrationFee.Text =
                    plan.Amount.ToString(
                        "0.##");

                CalculateRemaining();
            }
            catch (Exception ex)
            {
                ApplicationLogger.LogException("شاشة القيد: تحميل خطة رسوم التسجيل", ex);
                txtRegistrationFee.Text =
                    string.Empty;

                CalculateRemaining();

                if (showMissingMessage)
                {
                    UIHelper.ShowException(
                        "تحميل خطة رسوم التسجيل",
                        ex);
                }
            }
        }

        // ================================================================
        // LOAD SECTIONS
        // ================================================================

        private async Task LoadSectionsAsync()
        {
            txtSection.DataSource =
                null;

            txtSection.Items.Clear();

            txtSection.Enabled =
                false;

            int classId =
                TryGetSelectedId(
                    cmbClassID);

            string academicYear =
                txtAcademicYear.Text == null
                    ? string.Empty
                    : txtAcademicYear.Text.Trim();

            if (classId <= 0 ||
                !IsSequentialAcademicYear(
                    academicYear))
            {
                return;
            }

            try
            {
                DataTable sections =
                    await Task.Run(
                        () =>
                            sectionService.GetSections(
                                classId,
                                academicYear));

                if (sections == null ||
                    sections.Rows.Count == 0)
                {
                    return;
                }

                DataTable validSections =
                    sections.Clone();

                foreach (DataRow row
                         in sections.Rows)
                {
                    string sectionName =
                        row["Section"] ==
                        DBNull.Value

                            ? string.Empty

                            : Convert.ToString(
                                row["Section"]);

                    if (!string.IsNullOrWhiteSpace(
                            sectionName))
                    {
                        DataRow cleanRow =
                            validSections.NewRow();

                        cleanRow["Section"] =
                            sectionName.Trim();

                        validSections.Rows.Add(
                            cleanRow);
                    }
                }

                if (validSections.Rows.Count == 0)
                {
                    validSections.Rows.Add(
                        string.Empty);
                }

                txtSection.DataSource =
                    validSections;

                txtSection.DisplayMember =
                    "Section";

                txtSection.ValueMember =
                    "Section";

                txtSection.Enabled =
                    true;

                if (txtSection.Items.Count > 0)
                {
                    txtSection.SelectedIndex =
                        0;
                }

                await PreviewNextSeatNumberAsync();
            }
            catch (Exception ex)
            {
                ApplicationLogger.LogException("شاشة القيد: تحميل شعب التسجيل", ex);
                txtSection.DataSource =
                    null;

                txtSection.Items.Clear();

                txtSection.Enabled =
                    false;

                UIHelper.ShowException(
                    "تحميل شعب التسجيل",
                    ex);
            }
        }

        // ================================================================
        // PREVIEW NEXT SEAT
        // ================================================================

        private async Task PreviewNextSeatNumberAsync()
        {
            if (isLoading ||
                isEditMode)
            {
                return;
            }

            int classId =
                TryGetSelectedId(
                    cmbClassID);

            string academicYear =
                (txtAcademicYear.Text ??
                 string.Empty).Trim();

            string section =
                (txtSection.Text ??
                 string.Empty).Trim();

            if (classId <= 0 ||
                !IsSequentialAcademicYear(
                    academicYear) ||
                string.IsNullOrWhiteSpace(
                    section))
            {
                txtSeatNumber.Text =
                    "يُولّد تلقائياً عند الحفظ";

                return;
            }

            try
            {
                string nextSeat =
                    await Task.Run(
                        () =>
                            enrollmentService
                                .GenerateNextSeatNumber(
                                    academicYear,
                                    classId,
                                    section));

                if (!isEditMode)
                {
                    txtSeatNumber.Text =
                        string.IsNullOrWhiteSpace(
                            nextSeat)
                            ? "يُولّد تلقائياً عند الحفظ"
                            : nextSeat;
                }
            }
            catch (Exception ex)
            {
                ApplicationLogger.LogException("شاشة القيد: معاينة رقم المقعد التالي", ex);
                txtSeatNumber.Text =
                    "يُولّد تلقائياً عند الحفظ";
            }
        }

        // ================================================================
        // SET SECTION
        // ================================================================

        private void SetSectionValue(
            string section)
        {
            if (string.IsNullOrWhiteSpace(
                    section) ||
                txtSection.Items.Count == 0)
            {
                return;
            }

            int index =
                txtSection.FindStringExact(
                    section.Trim());

            if (index >= 0)
            {
                txtSection.SelectedIndex =
                    index;
            }
        }

        // ================================================================
        // LOAD RECORD
        // ================================================================

        private async void LoadRecordToScreen(
            DataGridViewRow row)
        {
            if (row == null ||
                row.IsNewRow ||
                isEditMode ||
                isLoading)
            {
                return;
            }

            isLoading = true;

            try
            {
                txtEnrollmentID.Text =
                    SafeCell(
                        row,
                        "EnrollmentID");

                txtAcademicYear.Text =
                    SafeCell(
                        row,
                        "AcademicYear");

                int recordEnrollmentId;
                int recordStudentId;
                int.TryParse(txtEnrollmentID.Text.Trim(), out recordEnrollmentId);
                int.TryParse(SafeCell(row, "StudentID"), out recordStudentId);

                LoadStudentChoicesForAcademicYear(
                    txtAcademicYear.Text,
                    recordEnrollmentId,
                    recordStudentId);

                txtStudentName.Text =
                    SafeCell(
                        row,
                        "StudentName");

                SafeSetDate(
                    dtpApplicationDate,
                    row.DataGridView.Columns.Contains(
                        "ApplicationDate")
                        ? row.Cells[
                            "ApplicationDate"].Value
                        : null);

                cmbApplicationType.SelectedItem =
                    SafeCell(
                        row,
                        "ApplicationType");

                if (row.DataGridView.Columns.Contains(
                        "ClassID"))
                {
                    cmbClassID.SelectedValue =
                        row.Cells[
                            "ClassID"].Value;
                }

                await LoadSectionsAsync();

                SetSectionValue(
                    SafeCell(
                        row,
                        "Section"));

                txtSeatNumber.Text =
                    SafeCell(
                        row,
                        "SeatNumber");

                cmbStatus.SelectedItem =
                    SafeCell(
                        row,
                        "Status");

                txtPreviousSchool.Text =
                    SafeCell(
                        row,
                        "PreviousSchool");

                txtPreviousClass.Text =
                    SafeCell(
                        row,
                        "PreviousClass");

                txtTransferReason.Text =
                    SafeCell(
                        row,
                        "TransferReason");

                txtRegistrationFee.Text =
                    SafeCell(
                        row,
                        "RegistrationFee");

                txtPaidAmount.Text =
                    SafeCell(
                        row,
                        "PaidAmount");

                cmbPaymentMethod.SelectedItem =
                    SafeCell(
                        row,
                        "PaymentMethod");

                txtReceiptNo.Text =
                    SafeCell(
                        row,
                        "ReceiptNo");

                chkHasBirthCertificate.Checked =
                    SafeBoolCell(
                        row,
                        "HasBirthCertificate");

                chkHasGuardianId.Checked =
                    SafeBoolCell(
                        row,
                        "HasGuardianId");

                chkHasPhoto.Checked =
                    SafeBoolCell(
                        row,
                        "HasPhoto");

                chkHasLastCertificate.Checked =
                    SafeBoolCell(
                        row,
                        "HasLastCertificate");

                chkHasMedicalReport.Checked =
                    SafeBoolCell(
                        row,
                        "HasMedicalReport");

                rtbNotes.Text =
                    SafeCell(
                        row,
                        "Notes");

                CalculateRemaining();

                DisableInputs();
            }
            catch (Exception ex)
            {
                // الفشل في عرض السجل المحدد لا يوقف العمل؛ نسجل التشخيص ونبلغ المستخدم.
                ApplicationLogger.LogException("عرض بيانات القيد المحدد", ex);
                UIHelper.ShowError("تعذر عرض بيانات القيد المحدد. راجع السجل لمزيد من التفاصيل.");
            }
            finally
            {
                isLoading = false;
            }
        }

        // ================================================================
        // PARSE AMOUNT
        // ================================================================

        private decimal ParseAmountOrZero(
            string value)
        {
            decimal amount;

            return string.IsNullOrWhiteSpace(
                       value) ||

                   !UIHelper.TryParseDecimal(
                       value,
                       out amount)

                ? 0m
                : amount;
        }

        // ================================================================
        // TRY GET SELECTED ID
        // ================================================================

        private static int TryGetSelectedId(
            ComboBox comboBox)
        {
            if (comboBox == null ||
                comboBox.SelectedIndex < 0 ||
                comboBox.SelectedValue == null ||
                comboBox.SelectedValue == DBNull.Value)
            {
                return 0;
            }

            int id;

            return int.TryParse(
                       comboBox.SelectedValue.ToString(),
                       out id) &&
                   id > 0

                ? id
                : 0;
        }

        // ================================================================
        // GET SELECTED ID
        // ================================================================

        private int GetSelectedId(
            ComboBox comboBox,
            string errorMessage)
        {
            int id;

            if (comboBox == null ||
                comboBox.SelectedIndex < 0 ||
                comboBox.SelectedValue == null ||
                comboBox.SelectedValue == DBNull.Value ||
                !int.TryParse(
                    comboBox.SelectedValue.ToString(),
                    out id) ||
                id <= 0)
            {
                if (comboBox != null)
                {
                    errorProvider1.SetError(
                        comboBox,
                        errorMessage);
                }

                throw new InvalidOperationException(
                    errorMessage);
            }

            return id;
        }

        // ================================================================
        // VALIDATE
        // ================================================================

        private bool ValidateInputs()
        {
            bool isValid = true;

            int studentId;

            if (cmbStudentID.SelectedIndex == -1 ||
                cmbStudentID.SelectedValue == null ||
                !int.TryParse(
                    cmbStudentID.SelectedValue.ToString(),
                    out studentId) ||
                studentId <= 0)
            {
                errorProvider1.SetError(
                    cmbStudentID,
                    "يجب تحديد طالب صالح.");

                isValid = false;
            }

            int classId;

            if (cmbClassID.SelectedIndex == -1 ||
                cmbClassID.SelectedValue == null ||
                !int.TryParse(
                    cmbClassID.SelectedValue.ToString(),
                    out classId) ||
                classId <= 0)
            {
                errorProvider1.SetError(
                    cmbClassID,
                    "يجب تحديد فصل صالح.");

                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(
                    txtAcademicYear.Text))
            {
                errorProvider1.SetError(
                    txtAcademicYear,
                    "العام الدراسي مطلوب.");

                isValid = false;
            }
            else if (!IsSequentialAcademicYear(
                         txtAcademicYear.Text))
            {
                errorProvider1.SetError(
                    txtAcademicYear,
                    "اكتب العام الدراسي بصيغة متسلسلة مثل 2026/2027 أو 1447-1448.");

                isValid = false;
            }

            if (dtpApplicationDate.Value.Date >
                DateTime.Today)
            {
                errorProvider1.SetError(
                    dtpApplicationDate,
                    "لا يمكن أن يكون تاريخ التسجيل في المستقبل.");

                isValid = false;
            }

            if (cmbApplicationType.SelectedIndex == -1)
            {
                errorProvider1.SetError(
                    cmbApplicationType,
                    "يجب تحديد نوع التسجيل.");

                isValid = false;
            }

            if (cmbStatus.SelectedIndex == -1)
            {
                errorProvider1.SetError(
                    cmbStatus,
                    "يجب تحديد حالة الطلب.");

                isValid = false;
            }

            ValidateTextOnlyField(
                txtPreviousSchool,
                "المدرسة السابقة",
                ref isValid);

            ValidateTextOnlyField(
                txtPreviousClass,
                "الصف السابق",
                ref isValid);

            ValidateTextOnlyField(
                txtTransferReason,
                "سبب النقل",
                ref isValid);

            decimal fee = 0;

            if (!string.IsNullOrWhiteSpace(
                    txtRegistrationFee.Text) &&
                (!UIHelper.TryParseDecimal(
                    txtRegistrationFee.Text,
                    out fee) ||
                 fee < 0))
            {
                errorProvider1.SetError(
                    txtRegistrationFee,
                    "أدخل مبلغاً رقمياً غير سالب.");

                isValid = false;
            }

            decimal paid = 0;

            if (!string.IsNullOrWhiteSpace(
                    txtPaidAmount.Text) &&
                (!UIHelper.TryParseDecimal(
                    txtPaidAmount.Text,
                    out paid) ||
                 paid < 0))
            {
                errorProvider1.SetError(
                    txtPaidAmount,
                    "أدخل مبلغاً رقمياً غير سالب.");

                isValid = false;
            }

            if (isValid &&
                paid > fee)
            {
                errorProvider1.SetError(
                    txtPaidAmount,
                    "لا يجوز أن يتجاوز المدفوع قيمة رسوم التسجيل.");

                isValid = false;
            }

            // الشعبة حقل مطلوب؛ يجب أن تحتوي على اختيار صالح.
            if (!txtSection.Enabled ||
                txtSection.SelectedIndex < 0 ||
                string.IsNullOrWhiteSpace(
                    txtSection.Text))
            {
                errorProvider1.SetError(
                    txtSection,
                    "اختر الشعبة.");

                isValid = false;
            }
            else
            {
                errorProvider1.SetError(
                    txtSection,
                    "");
            }

            // رقم السند مرجعي يُولَّد تلقائياً بصيغة نصية (REG-...) ويُعرض للقراءة فقط؛
            // نتحقق فقط من ألا يتجاوز الطول المخصص في قاعدة البيانات (nvarchar(50)).
            if (!string.IsNullOrWhiteSpace(
                    txtReceiptNo.Text) &&
                txtReceiptNo.Text.Trim().Length > 50)
            {
                errorProvider1.SetError(
                    txtReceiptNo,
                    "رقم السند لا يجب أن يتجاوز 50 حرفاً.");

                isValid = false;
            }

            return isValid;
        }

        // ================================================================
        // VALIDATE TEXT FIELD
        // ================================================================

        private void ValidateTextOnlyField(
            Control control,
            string label,
            ref bool isValid)
        {
            string value =
                control == null
                    ? string.Empty
                    : control.Text.Trim();

            if (!string.IsNullOrWhiteSpace(value) &&
                !UIHelper.IsValidArabicOrLatinName(
                    value,
                    2))
            {
                errorProvider1.SetError(
                    control,
                    label +
                    " يقبل الأحرف والمسافات فقط.");

                isValid = false;
            }
        }

        // ================================================================
        // ACADEMIC YEAR VALIDATION
        // ================================================================

        private bool IsSequentialAcademicYear(
            string value)
        {
            string normalized =
                (value ?? string.Empty)
                    .Trim()
                    .Replace('–', '/')
                    .Replace('-', '/');

            Match match =
                Regex.Match(
                    normalized,
                    @"^(\d{4})\s*/\s*(\d{4})$");

            int firstYear;
            int secondYear;

            return match.Success &&
                   int.TryParse(
                       match.Groups[1].Value,
                       out firstYear) &&
                   int.TryParse(
                       match.Groups[2].Value,
                       out secondYear) &&
                   secondYear == firstYear + 1;
        }

        // ================================================================
        // DISABLE INPUTS
        // ================================================================

        private void DisableInputs()
        {
            foreach (Control c
                     in tlpBasic.Controls)
            {
                if (c is TextBox ||
                    c is ComboBox ||
                    c is DateTimePicker)
                {
                    c.Enabled =
                        false;
                }
            }

            foreach (Control c
                     in tlpPrevious.Controls)
            {
                if (c is TextBox ||
                    c is ComboBox)
                {
                    c.Enabled =
                        false;
                }
            }

            foreach (Control c
                     in tlpFees.Controls)
            {
                if (c is TextBox ||
                    c is ComboBox)
                {
                    c.Enabled =
                        false;
                }
            }

            foreach (Control c
                     in flpAttachments.Controls)
            {
                if (c is CheckBox)
                {
                    c.Enabled =
                        false;
                }
            }

            rtbNotes.Enabled =
                false;

            btnSave.Enabled =
                false;

            btnAdd.Enabled =
                true;

            bool hasSelectedRecord =
                int.TryParse(
                    txtEnrollmentID.Text,
                    out int selectedId) &&
                selectedId > 0;

            btnUpdate.Enabled =
                hasSelectedRecord;

            btnDelete.Enabled =
                hasSelectedRecord;
        }

        // ================================================================
        // ENABLE INPUTS
        // ================================================================

        private void EnableInputs()
        {
            foreach (Control c
                     in tlpBasic.Controls)
            {
                if (c is TextBox ||
                    c is ComboBox ||
                    c is DateTimePicker)
                {
                    c.Enabled =
                        true;
                }
            }

            foreach (Control c
                     in tlpPrevious.Controls)
            {
                if (c is TextBox ||
                    c is ComboBox)
                {
                    c.Enabled =
                        true;
                }
            }

            foreach (Control c
                     in tlpFees.Controls)
            {
                if (c is TextBox ||
                    c is ComboBox)
                {
                    c.Enabled =
                        true;
                }
            }

            foreach (Control c
                     in flpAttachments.Controls)
            {
                if (c is CheckBox)
                {
                    c.Enabled =
                        true;
                }
            }

            rtbNotes.Enabled =
                true;

            // ------------------------------------------------------------
            // These fields remain read-only by behavior.
            // ------------------------------------------------------------

            txtEnrollmentID.Enabled =
                false;

            txtStudentName.Enabled =
                false;

            txtRemainingAmount.Enabled =
                false;

            btnSave.Enabled =
                true;

            btnAdd.Enabled =
                false;

            btnUpdate.Enabled =
                false;

            btnDelete.Enabled =
                false;
        }

        // ================================================================
        // CLEAR INPUTS
        // ================================================================

        private void ClearInputs()
        {
            isLoading =
                true;

            txtEnrollmentID.Clear();

            cmbStudentID.SelectedIndex =
                -1;

            txtStudentName.Clear();

            dtpApplicationDate.Value =
                DateTime.Today;

            cmbApplicationType.SelectedIndex =
                -1;

            SetAutomaticAcademicYear();

            cmbClassID.SelectedIndex =
                -1;

            txtSection.DataSource =
                null;

            txtSection.Items.Clear();

            txtSection.Enabled =
                false;

            txtSeatNumber.Text =
                "يُولّد تلقائياً عند الحفظ";

            cmbStatus.SelectedIndex =
                -1;

            txtPreviousSchool.Clear();

            txtPreviousClass.Clear();

            txtTransferReason.Clear();

            isFinancialEditLocked = false;

            txtRegistrationFee.Clear();

            txtPaidAmount.Clear();

            txtRemainingAmount.Clear();

            cmbPaymentMethod.SelectedIndex =
                cmbPaymentMethod.Items.Count > 0
                    ? 0
                    : -1;

            txtReceiptNo.Clear();

            foreach (Control c
                     in flpAttachments.Controls)
            {
                if (c is CheckBox chk)
                {
                    chk.Checked =
                        false;
                }
            }

            rtbNotes.Clear();

            isLoading =
                false;
        }
    }
}
