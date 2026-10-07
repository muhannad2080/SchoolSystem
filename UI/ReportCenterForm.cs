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
using SchoolSystem.DataAccess;


namespace SchoolSystem.UI
{

    public partial class ReportCenterForm : UserControl
    {
        // تعريف الدالة ReportService لتنفيذ العملية المرتبطة بها.
        private readonly ReportService reportService = new ReportService();
        // تعريف الدالة AnnualClosingService لتنفيذ العملية المرتبطة بها.
        private readonly AnnualClosingService annualClosingService = new AnnualClosingService();
        // تعريف الدالة ClassService لتنفيذ العملية المرتبطة بها.
        private readonly ClassService classService = new ClassService();
        // تعريف الدالة AcademicReferenceRepository لتنفيذ العملية المرتبطة بها.
        private readonly AcademicReferenceRepository academicReferenceRepository = new AcademicReferenceRepository();


        private DataTable currentReportData;

        private bool isLoadingFilters;

        private int sectionsLoadVersion;

        private int reportLoadVersion;

        private readonly Timer studentSearchTimer;

        private int printRowIndex = 0;

        private int printPageNumber = 0;

        // تعريف الدالة ReportCenterForm لتنفيذ العملية المرتبطة بها.
        public ReportCenterForm()
        {

            InitializeComponent();

            SchoolSystem.Helpers.UIHelper.ApplyStyle(this);
            ApplyCustomStyles();
            Dock = DockStyle.Fill;
            studentSearchTimer = new Timer { Interval = 350 };

            studentSearchTimer.Tick += studentSearchTimer_Tick;
            WireUpEvents();
            DataRefreshNotifier.DataChanged += DataRefreshNotifier_DataChanged;
            Disposed += (sender, e) =>
                DataRefreshNotifier.DataChanged -= DataRefreshNotifier_DataChanged;
        }

        private async void DataRefreshNotifier_DataChanged(
            object sender,
            DataRefreshEventArgs e)
        {
            if (ReferenceEquals(sender, this) || IsDisposed || Disposing)
                return;

            if (!DataRefreshNotifier.IsRelated(e, "Students") &&
                !DataRefreshNotifier.IsRelated(e, "Teachers") &&
                !DataRefreshNotifier.IsRelated(e, "Enrollment") &&
                !DataRefreshNotifier.IsRelated(e, "ClassAssignment") &&
                !DataRefreshNotifier.IsRelated(e, "Subjects") &&
                !DataRefreshNotifier.IsRelated(e, "Classes") &&
                !DataRefreshNotifier.IsRelated(e, "Sections") &&
                !DataRefreshNotifier.IsRelated(e, "Fees") &&
                !DataRefreshNotifier.IsRelated(e, "Users") &&
                !DataRefreshNotifier.IsRelated(e, "AcademicYears") &&
                !DataRefreshNotifier.IsRelated(e, "All"))
                return;

            try
            {
                isLoadingFilters = true;
                await LoadAcademicYearsAsync();
                await LoadClassesAsync();
                await LoadTermsAsync();
                await LoadSectionsAsync();
            }
            catch (Exception ex)
            {
                ApplicationLogger.LogException("تحديث مركز التقارير تلقائياً", ex);
            }
            finally
            {
                isLoadingFilters = false;
            }
        }

        // تعريف الدالة ApplyCustomStyles لتنفيذ العملية المرتبطة بها.
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
            UIHelper.StyleComboBox(cmbTerm);
            UIHelper.StyleComboBox(txtAcademicYear);
            UIHelper.StyleTextBox(txtSearch);
            lblSearch.Text = "طالب/طلاب (اسم أو رقم):";
            txtSearch.CharacterCasing = CharacterCasing.Normal;
            ToolTip searchToolTip = new ToolTip();
            searchToolTip.SetToolTip(txtSearch, "اكتب جزءاً من اسم الطالب أو رقمه. لإظهار عدة طلاب افصل الأسماء أو الأرقام بفاصلة أو فاصلة عربية.");
            lblSummary.ForeColor = UIHelper.MutedTextColor;
            lblRecordCount.ForeColor = UIHelper.MutedTextColor;
        }

        // تعريف الدالة WireUpEvents لتنفيذ العملية المرتبطة بها.
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

            txtAcademicYear.SelectedIndexChanged -= txtAcademicYear_SelectedIndexChanged;

            txtAcademicYear.SelectedIndexChanged += txtAcademicYear_SelectedIndexChanged;

            txtSearch.TextChanged -= txtSearch_TextChanged;

            txtSearch.TextChanged += txtSearch_TextChanged;
            cmbReportType.SelectedIndexChanged -= cmbReportType_SelectedIndexChanged;

            cmbReportType.SelectedIndexChanged += cmbReportType_SelectedIndexChanged;
            cmbTerm.SelectedIndexChanged -= cmbTerm_SelectedIndexChanged;

            cmbTerm.SelectedIndexChanged += cmbTerm_SelectedIndexChanged;

            cmbSection.SelectedIndexChanged -= cmbSection_SelectedIndexChanged;

            cmbSection.SelectedIndexChanged += cmbSection_SelectedIndexChanged;

            cmbStatus.SelectedIndexChanged -= cmbStatus_SelectedIndexChanged;

            cmbStatus.SelectedIndexChanged += cmbStatus_SelectedIndexChanged;

            dtpFromDate.ValueChanged -= reportFilter_ValueChanged;

            dtpFromDate.ValueChanged += reportFilter_ValueChanged;

            dtpToDate.ValueChanged -= reportFilter_ValueChanged;

            dtpToDate.ValueChanged += reportFilter_ValueChanged;

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

        // تعريف الدالة cmbTerm_SelectedIndexChanged لتنفيذ العملية المرتبطة بها.
        private void cmbTerm_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoadingFilters || !IsHandleCreated || cmbReportType.SelectedIndex < 0)
                return;

            ScheduleAutomaticReportLoad();
        }

        // تعريف الدالة cmbReportType_SelectedIndexChanged لتنفيذ العملية المرتبطة بها.
        private void cmbReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoadingFilters || !IsHandleCreated || cmbReportType.SelectedIndex < 0)
                return;

            UpdateFilterHintsForReportType();

            string selectedType = cmbReportType.Text;

            bool needsSearch = selectedType == "كشف درجات طالب";
            bool needsTerm = selectedType == "كشف نتائج الفصل";

            if (needsSearch && string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                ClearReportGrid();
                return;
            }

            if (needsTerm && string.IsNullOrWhiteSpace(GetSelectedTermName()))
            {
                ClearReportGrid();
                return;
            }

            studentSearchTimer.Stop();
            studentSearchTimer.Start();
        }

        // تعريف الدالة txtSearch_TextChanged لتنفيذ العملية المرتبطة بها.
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (isLoadingFilters || !IsHandleCreated || cmbReportType.SelectedIndex < 0)
                return;

            if (cmbReportType.Text == "كشف درجات طالب" && string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                ClearReportGrid();
                return;
            }

            ScheduleAutomaticReportLoad();
        }

        // تعريف الدالة ScheduleAutomaticReportLoad لتنفيذ العملية المرتبطة بها.
        private void ScheduleAutomaticReportLoad()
        {
            if (isLoadingFilters || !IsHandleCreated || cmbReportType.SelectedIndex < 0)
                return;

            studentSearchTimer.Stop();

            if (cmbReportType.Text == "كشف درجات طالب" && string.IsNullOrWhiteSpace(txtSearch.Text))
                return;

            if (cmbReportType.Text == "كشف نتائج الفصل" && string.IsNullOrWhiteSpace(GetSelectedTermName()))
                return;

            studentSearchTimer.Start();
        }

        // تعريف الدالة studentSearchTimer_Tick لتنفيذ العملية المرتبطة بها.
        private async void studentSearchTimer_Tick(object sender, EventArgs e)
        {
            studentSearchTimer.Stop();
            if (isLoadingFilters || !IsHandleCreated || cmbReportType.SelectedIndex < 0)
                return;


            await LoadReportAsync(false);
        }

        // تعريف الدالة ReportCenterForm_Load لتنفيذ العملية المرتبطة بها.
        private async void ReportCenterForm_Load(object sender, EventArgs e)
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                Cursor = Cursors.WaitCursor;
                isLoadingFilters = true;

                LoadStaticData();

                await LoadAcademicYearsAsync();

                await LoadClassesAsync();

                await LoadTermsAsync();

                await LoadSectionsAsync();
            }

            catch (Exception ex)
            {
                UIHelper.ShowException("تحميل مركز التقارير", ex);
            }

            finally
            {
                isLoadingFilters = false;
                Cursor = Cursors.Default;

                // بعد تجهيز القوائم، اعرض التقرير الافتراضي تلقائيًا.
                // التقارير التي تتطلب اسم/رقم طالب أو ترمًا ستبقى فارغة حتى يكتمل المدخل المطلوب.
                if (IsHandleCreated &&
                    cmbReportType.SelectedIndex >= 0)
                {
                    ScheduleAutomaticReportLoad();
                }
            }
        }

        // تعريف الدالة LoadStaticData لتنفيذ العملية المرتبطة بها.
        private void LoadStaticData()
        {
            cmbReportType.Items.Clear();
            cmbReportType.Items.Add("تقرير الطلاب");
            cmbReportType.Items.Add("تقرير المعلمين");
            cmbReportType.Items.Add("تقرير القبول والتسجيل");
            cmbReportType.Items.Add("تقرير توزيع الفصول");
            cmbReportType.Items.Add("تقرير حضور الطلاب");
            cmbReportType.Items.Add("تقرير حضور المعلمين");
            cmbReportType.Items.Add("تقرير المواد الدراسية");
            cmbReportType.Items.Add("تقرير الرواتب");
            cmbReportType.Items.Add("تقرير المستخدمين والصلاحيات");
            cmbReportType.Items.Add("تقرير الرسوم");
            cmbReportType.Items.Add("تقرير الدرجات");
            cmbReportType.Items.Add("كشف درجات طالب");
            cmbReportType.Items.Add("كشف نتائج الفصل");
            cmbReportType.Items.Add("تقرير المكتبة والإعارات");
            cmbReportType.Items.Add("تقرير النقل المدرسي");
            cmbReportType.Items.Add("تقرير المصروفات");
            cmbReportType.Items.Add("تقرير الحركة المالية");

            if (cmbReportType.Items.Count > 0)
                cmbReportType.SelectedIndex = 0;

            cmbSection.DataSource = null;
            cmbSection.Items.Clear();
            cmbSection.Enabled = false;

            cmbTerm.DataSource = null;
            cmbTerm.Items.Clear();
            cmbTerm.Enabled = false;

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
            cmbStatus.Items.Add("مصروف");
            cmbStatus.Items.Add("غير مصروف");
            cmbStatus.Items.Add("معار");
            cmbStatus.Items.Add("مسترجع");

            if (cmbStatus.Items.Count > 0)
                cmbStatus.SelectedIndex = 0;

            dtpFromDate.Value = new DateTime(DateTime.Today.Year, 1, 1);
            dtpToDate.Value = DateTime.Today;

            lblSummary.Text = "ملخص التقرير: لا توجد بيانات محملة.";
            lblRecordCount.Text = "عدد السجلات: 0";
        }

        // تعريف الدالة LoadAcademicYearsAsync لتنفيذ العملية المرتبطة بها.
        private async Task LoadAcademicYearsAsync()
        {
            txtAcademicYear.Items.Clear();

            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {

                var years = await Task.Run(() => annualClosingService.GetAvailableYears());

                string activeYear = await Task.Run(() => annualClosingService.GetActiveAcademicYear());

                if (years != null)
                {
                    foreach (string year in years)
                    {
                        if (!string.IsNullOrWhiteSpace(year) && !txtAcademicYear.Items.Contains(year))
                            txtAcademicYear.Items.Add(year);
                    }
                }

                if (!string.IsNullOrWhiteSpace(activeYear) && !txtAcademicYear.Items.Contains(activeYear))
                    txtAcademicYear.Items.Add(activeYear);

                if (txtAcademicYear.Items.Count == 0)
                {
                    UIHelper.ShowWarning("لا توجد أعوام دراسية مسجلة. أنشئ العام الدراسي أولاً قبل تشغيل التقارير.");

                    return;
                }

                int activeIndex = txtAcademicYear.Items.IndexOf(activeYear);
                txtAcademicYear.SelectedIndex = activeIndex >= 0 ? activeIndex : 0;
            }

            catch (Exception ex)
            {
                UIHelper.ShowException("تعذر تحميل الأعوام الدراسية من قاعدة البيانات:\n", ex);
            }
        }

        // تعريف الدالة cmbClass_SelectedIndexChanged لتنفيذ العملية المرتبطة بها.
        private async void cmbClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoadingFilters)
                return;

            await LoadSectionsAsync();
            ScheduleAutomaticReportLoad();
        }

        private async void cmbSection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoadingFilters)
                return;

            ScheduleAutomaticReportLoad();
        }

        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoadingFilters)
                return;

            ScheduleAutomaticReportLoad();
        }

        private void reportFilter_ValueChanged(object sender, EventArgs e)
        {
            if (isLoadingFilters || !IsHandleCreated || cmbReportType.SelectedIndex < 0)
                return;

            ScheduleAutomaticReportLoad();
        }

        // تعريف الدالة txtAcademicYear_SelectedIndexChanged لتنفيذ العملية المرتبطة بها.
        private async void txtAcademicYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoadingFilters)
                return;

            await LoadSectionsAsync();
            ScheduleAutomaticReportLoad();
        }

        // تعريف الدالة LoadTermsAsync لتنفيذ العملية المرتبطة بها.
        private async Task LoadTermsAsync()
        {
            DataTable choices = new DataTable();
            choices.Columns.Add("TermID", typeof(int));
            choices.Columns.Add("TermName", typeof(string));
            choices.Rows.Add(0, string.Empty);

            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {

                DataTable terms = await Task.Run(() => academicReferenceRepository.GetActiveTerms());
                // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
                var names = new System.Collections.Generic.HashSet<string>(StringComparer.OrdinalIgnoreCase);

                if (terms != null && terms.Columns.Contains("TermName"))
                {
                    foreach (DataRow row in terms.Rows)
                    {
                        string termName = row["TermName"] == DBNull.Value
                            ? string.Empty
                            : Convert.ToString(row["TermName"]);
                        termName = (termName ?? string.Empty).Trim();
                        if (string.IsNullOrWhiteSpace(termName) || !names.Add(termName))
                            continue;

                        int termId = 0;
                        if (terms.Columns.Contains("TermID"))
                            int.TryParse(Convert.ToString(row["TermID"]), out termId);
                        choices.Rows.Add(termId, termName);
                    }
                }
            }

            catch (Exception ex)
            {

                UIHelper.ShowException("تحميل الفصول الدراسية للتقارير", ex);
            }

            cmbTerm.DataSource = choices;
            cmbTerm.DisplayMember = "TermName";
            cmbTerm.ValueMember = "TermID";
            SelectFirstItem(cmbTerm);
            cmbTerm.Enabled = choices.Rows.Count > 1;
        }

        // تعريف الدالة LoadSectionsAsync لتنفيذ العملية المرتبطة بها.
        private async Task LoadSectionsAsync()
        {
            int loadVersion = ++sectionsLoadVersion;

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

            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {

                string academicYear = txtAcademicYear.Text.Trim();
                DataTable sections = await Task.Run(() => reportService.GetSections(classId, academicYear));
                if (loadVersion != sectionsLoadVersion || IsDisposed || Disposing || !IsHandleCreated)
                    return;

                if (sections == null)
                    sections = new DataTable();

                DataTable choices = new DataTable();
                choices.Columns.Add("Section", typeof(string));
                // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
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
                    SelectFirstItem(cmbSection);
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
                SelectFirstItem(cmbSection);
            }

            catch (Exception ex)
            {
                cmbSection.DataSource = null;
                cmbSection.Items.Clear();
                cmbSection.Enabled = false;
                UIHelper.ShowException("تحميل شعب مركز التقارير", ex);
            }
        }

        // تعريف الدالة SelectFirstItem لتنفيذ العملية المرتبطة بها.
        private static void SelectFirstItem(ComboBox comboBox)
        {
            if (comboBox == null)
                return;

            comboBox.SelectedIndex = comboBox.Items.Count > 0 ? 0 : -1;
        }

        // تعريف الدالة LoadClassesAsync لتنفيذ العملية المرتبطة بها.
        private async Task LoadClassesAsync()
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
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

        // تعريف الدالة IsValidAcademicYear لتنفيذ العملية المرتبطة بها.
        private bool IsValidAcademicYear(string academicYear)
        {
            if (string.IsNullOrWhiteSpace(academicYear))
                return false;

            string normalized = academicYear.Trim().Replace('-', '/');
            string[] parts = normalized.Split('/');
            if (parts.Length != 2 || parts[0].Length != 4 || parts[1].Length != 4)
                return false;
            if (!int.TryParse(parts[0], out int firstYear) || !int.TryParse(parts[1], out int secondYear))
                return false;


            return secondYear == firstYear + 1;
        }

        // تعريف الدالة ValidateReportFilters لتنفيذ العملية المرتبطة بها.
        private bool ValidateReportFilters(bool showWarnings = true)
        {
            if (cmbReportType.SelectedIndex < 0 || string.IsNullOrWhiteSpace(cmbReportType.Text))
            {
                if (showWarnings) { ShowWarning("يرجى اختيار نوع التقرير."); cmbReportType.Focus(); }
                return false;
            }
            if (!IsValidAcademicYear(txtAcademicYear.Text))
            {
                if (showWarnings) { ShowWarning("أدخل العام الدراسي بالصيغة الصحيحة: 2025/2026 أو 1447-1448."); txtAcademicYear.Focus(); }
                return false;
            }
            if (dtpFromDate.Value.Date > dtpToDate.Value.Date)
            {
                if (showWarnings) { ShowWarning("تاريخ البداية يجب أن يكون قبل تاريخ النهاية أو مساويًا له."); dtpFromDate.Focus(); }
                return false;
            }

            if (cmbReportType.Text == "كشف درجات طالب" && string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                if (showWarnings) { ShowWarning("اكتب اسم الطالب أو رقمه، أو عدة أسماء/أرقام مفصولة بفاصلة، لإظهار الكشف."); txtSearch.Focus(); }
                return false;
            }

            if (cmbReportType.Text == "كشف نتائج الفصل" && string.IsNullOrWhiteSpace(GetSelectedTermName()))
            {
                if (showWarnings) { ShowWarning("اختر الفصل الدراسي المطلوب لتشغيل كشف نتائج الفصل."); cmbTerm.Focus(); }
                return false;
            }


            return true;
        }

        // تعريف الدالة GetSelectedTermName لتنفيذ العملية المرتبطة بها.
        private string GetSelectedTermName()
        {

            Control termControl = Controls.Find("cmbTerm", true).Length > 0 ? Controls.Find("cmbTerm", true)[0] : null;

            return termControl is ComboBox ? termControl.Text.Trim() : string.Empty;
        }

        // تعريف الدالة BuildRequest لتنفيذ العملية المرتبطة بها.
        private ReportRequest BuildRequest()
        {
            ReportRequest request = new ReportRequest();

            request.ReportType = cmbReportType.Text;
            request.AcademicYear = txtAcademicYear.Text.Trim();
            request.TermName = GetSelectedTermName();
            request.Section = cmbSection.Text.Trim();
            if (request.Section == "لا توجد شعب متاحة")
                request.Section = string.Empty;
            request.Status = cmbStatus.Text.Trim();
            request.FromDate = dtpFromDate.Value.Date;
            request.ToDate = dtpToDate.Value.Date;
            request.SearchText = txtSearch.Text.Trim();
            if (cmbReportType.Text == "كشف درجات طالب" || cmbReportType.Text == "كشف نتائج الفصل")
            {
                request.StudentSelection = request.SearchText;
                int studentId;
                if (int.TryParse(request.SearchText, out studentId) && studentId > 0)
                    request.StudentID = studentId;
            }

            if (cmbClass.SelectedValue != null)
            {
                if (int.TryParse(cmbClass.SelectedValue.ToString(), out int classId) && classId > 0)
                    request.ClassID = classId;
            }


            return request;
        }

        // تعريف الدالة btnLoad_Click لتنفيذ العملية المرتبطة بها.
        private async void btnLoad_Click(object sender, EventArgs e)
        {

            await LoadReportAsync(true);
        }

        // تعريف الدالة btnRefresh_Click لتنفيذ العملية المرتبطة بها.
        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            ResetFilters();

            await LoadReportAsync();
        }

        // تعريف الدالة ResetFilters لتنفيذ العملية المرتبطة بها.
        private void ResetFilters()
        {
            if (cmbReportType.Items.Count > 0)
                cmbReportType.SelectedIndex = 0;

            if (txtAcademicYear.Items.Count > 0)
                txtAcademicYear.SelectedIndex = 0;
            txtSearch.Clear();
            if (cmbTerm.Items.Count > 0) cmbTerm.SelectedIndex = 0;
            if (txtAcademicYear.Items.Count > 0 && txtAcademicYear.SelectedIndex < 0)
                txtAcademicYear.SelectedIndex = 0;

            if (cmbClass.Items.Count > 0)
                cmbClass.SelectedIndex = 0;

            if (cmbStatus.Items.Count > 0)
                cmbStatus.SelectedIndex = 0;

            dtpFromDate.Value = new DateTime(DateTime.Today.Year, 1, 1);
            dtpToDate.Value = DateTime.Today;
            currentReportData = null;
            dataGridViewReport.DataSource = null;
            lblRecordCount.Text = "عدد السجلات: 0";
            lblSummary.Text = "ملخص التقرير: لا توجد بيانات محملة.";
        }

        // تعريف الدالة LoadReportAsync لتنفيذ العملية المرتبطة بها.
        private async Task LoadReportAsync(bool showWarnings = true)
        {
            int loadVersion = ++reportLoadVersion;
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                if (!EnsureReportAction("View", "ليس لديك صلاحية عرض التقارير."))
                    return;

                if (!ValidateReportFilters(showWarnings))
                    return;

                Cursor = Cursors.WaitCursor;
                ReportRequest request = BuildRequest();

                DataTable reportData = await Task.Run(() => reportService.GetReportData(request));

                if (loadVersion != reportLoadVersion || IsDisposed || Disposing || !IsHandleCreated)
                    return;

                currentReportData = reportData;
                dataGridViewReport.DataSource = currentReportData;
                FormatReportGrid();
                lblRecordCount.Text = "عدد السجلات: " + (currentReportData != null ? currentReportData.Rows.Count : 0);
                BuildSummary(currentReportData);

                if (showWarnings && (request.ReportType == "كشف درجات طالب" || request.ReportType == "كشف نتائج الفصل")
                    && (currentReportData == null || currentReportData.Rows.Count == 0))
                {
                    ShowWarning("لم يتم العثور على درجات مطابقة. اكتب جزءاً من اسم الطالب أو رقمه، ويمكن إدخال أكثر من طالب بفواصل، وتأكد من العام والصف والشعبة والفصل.");
                    txtSearch.Focus();
                }
            }

            catch (Exception ex)
            {
                if (loadVersion == reportLoadVersion)
                    UIHelper.ShowException("تحميل التقرير", ex);
            }

            finally
            {
                if (loadVersion == reportLoadVersion)
                    Cursor = Cursors.Default;
            }
        }

        // تعريف الدالة FormatReportGrid لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة BuildSummary لتنفيذ العملية المرتبطة بها.
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
            decimal academicAverage = AverageColumnIfExists(dt, "المعدل العام");
            decimal subjectAverage = AverageColumnIfExists(dt, "معدل الطالب في الفصل");
            int passed = CountStatus(dt, "ناجح", "الحالة النهائية", "الحالة العامة للطالب");
            int failed = CountStatus(dt, "راسب", "الحالة النهائية", "الحالة العامة للطالب");

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
            {
                decimal netMovement = receipts - payments;
                summary += " | صافي الحركة: " + netMovement.ToString("N2");
            }

            if (academicAverage > 0)
                summary += " | متوسط الفصل: " + academicAverage.ToString("N2");

            else if (subjectAverage > 0)

                summary += " | متوسط الفصل: " + subjectAverage.ToString("N2");

            if (passed > 0 || failed > 0)
                summary += " | ناجح: " + passed + " | راسب: " + failed;

            lblSummary.Text = summary;
        }

        // تعريف الدالة SumColumnIfExists لتنفيذ العملية المرتبطة بها.
        private decimal SumColumnIfExists(DataTable dt, string columnName)
        {
            if (dt == null || !dt.Columns.Contains(columnName))
                return 0;

            decimal sum = 0;

            foreach (DataRow row in dt.Rows)
            {
                if (row[columnName] == DBNull.Value)
                    continue;

                if (decimal.TryParse(row[columnName].ToString(), out decimal value))
                    sum += value;
            }


            return sum;
        }

        // تعريف الدالة AverageColumnIfExists لتنفيذ العملية المرتبطة بها.
        private decimal AverageColumnIfExists(DataTable dt, string columnName)
        {
            if (dt == null || !dt.Columns.Contains(columnName)) return 0;
            decimal total = 0; int count = 0;
            foreach (DataRow row in dt.Rows)
            {
                decimal value;
                if (row[columnName] != DBNull.Value && decimal.TryParse(Convert.ToString(row[columnName]), out value))
                { total += value; count++; }
            }

            return count == 0 ? 0 : total / count;
        }

        // تعريف الدالة CountStatus لتنفيذ العملية المرتبطة بها.
        private int CountStatus(DataTable dt, string status, params string[] columns)
        {
            if (dt == null) return 0;
            foreach (string column in columns)
            {
                if (!dt.Columns.Contains(column)) continue;
                int count = 0;
                foreach (DataRow row in dt.Rows)
                    if (string.Equals(Convert.ToString(row[column]).Trim(), status, StringComparison.OrdinalIgnoreCase)) count++;
                return count;
            }

            return 0;
        }

        // تعريف الدالة GetCurrentDataTable لتنفيذ العملية المرتبطة بها.
        private DataTable GetCurrentDataTable()
        {
            if (dataGridViewReport.DataSource is DataTable table)
                return table;

            if (dataGridViewReport.DataSource is DataView view)
                return view.ToTable();


            return currentReportData;
        }

        // تعريف الدالة EnsureReportAction لتنفيذ العملية المرتبطة بها.
        private bool EnsureReportAction(string action, string message)
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                CurrentUser.DemandAction("Reports", action, message);

                return true;
            }

            catch (UnauthorizedAccessException ex)
            {
                ApplicationLogger.LogWarning("شاشة مركز التقارير: الصلاحيات", ex.Message);
                ShowWarning(ex.Message);

                return false;
            }
        }

        // تعريف الدالة EnsureData لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة btnExportExcel_Click لتنفيذ العملية المرتبطة بها.
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

                // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
                try
                {
                    Cursor = Cursors.WaitCursor;

                    ReportOutputHelper.ExportToExcel(
                        dt,
                        sfd.FileName,
                        "تقرير - " + cmbReportType.Text,
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


        // تعريف الدالة btnExportCsv_Click لتنفيذ العملية المرتبطة بها.
        private void btnExportCsv_Click(object sender, EventArgs e)
        {
            if (!EnsureReportAction("ExportCsv", "ليس لديك صلاحية تصدير بيانات التقارير."))
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

                // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
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

        // تعريف الدالة ExportToCsv لتنفيذ العملية المرتبطة بها.
        private void ExportToCsv(DataTable dt, string filePath)
        {
            using (StreamWriter sw = new StreamWriter(filePath, false, new UTF8Encoding(true)))
            {
                SchoolSettings branding = SchoolSettingsService.Current();
                sw.WriteLine(EscapeCsv(ReportOutputHelper.GetSchoolName(branding)));
                sw.WriteLine(EscapeCsv(ReportOutputHelper.GetOrganizationLine(branding)));
                sw.WriteLine(EscapeCsv("تاريخ الإنشاء: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm")));
                sw.WriteLine(EscapeCsv("المستخدم المنفذ: " + ReportOutputHelper.GetCurrentOperatorDisplayName()));

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

        // تعريف الدالة EscapeCsv لتنفيذ العملية المرتبطة بها.
        private string EscapeCsv(string value)
        {
            if (value == null)
                return "";

            value = value.Replace("\"", "\"\"");

            return "\"" + value + "\"";
        }

        // تعريف الدالة btnExportPDF_Click لتنفيذ العملية المرتبطة بها.
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

                // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
                try
                {
                    Cursor = Cursors.WaitCursor;

                    ReportOutputHelper.ExportToPdf(
                        dt,
                        sfd.FileName,
                        "تقرير - " + cmbReportType.Text,
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


        // تعريف الدالة btnPrint_Click لتنفيذ العملية المرتبطة بها.
        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (!EnsureReportAction("Print", "ليس لديك صلاحية طباعة التقارير."))
                return;

            if (!EnsureData())
                return;

            printRowIndex = 0;
            printPageNumber = 0;
            printDocument.DefaultPageSettings.Landscape = dataGridViewReport.Columns.Count > 7;
            printDocument.DefaultPageSettings.Margins = new Margins(45, 45, 55, 55);
            printPreviewDialog.Width = 1000;
            printPreviewDialog.Height = 700;
            printPreviewDialog.ShowDialog();
        }

        // تعريف الدالة PrintDocument_PrintPage لتنفيذ العملية المرتبطة بها.
        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            DataTable dt = GetCurrentDataTable();

            if (dt == null)
            {
                e.HasMorePages = false;

                return;
            }

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
                string reportTitle = "تقرير " + cmbReportType.Text;

                bool reportIsRtl = true;
                printPageNumber++;
                y = ReportOutputHelper.DrawSchoolPrintHeader(
                    e.Graphics,
                    e.MarginBounds,
                    reportTitle,
                    reportIsRtl);

                string filterLine = "العام: " + txtAcademicYear.Text.Trim()
                    + (cmbClass.SelectedIndex > 0 ? " | الصف: " + cmbClass.Text : string.Empty)
                    + (!string.IsNullOrWhiteSpace(cmbSection.Text) && cmbSection.Text != "لا توجد شعب متاحة" ? " | الشعبة: " + cmbSection.Text : string.Empty);
                e.Graphics.DrawString("التاريخ: " + DateTime.Now.ToString("yyyy/MM/dd HH:mm") + " | " + filterLine,

                    infoFont, Brushes.Black, new RectangleF(x, y, pageWidth, 22), rtlFormat);
                y += 28;

                int colCount = dt.Columns.Count;
                int colWidth = Math.Max(70, pageWidth / Math.Max(1, colCount));


                for (int i = 0; i < colCount; i++)
                {
                    int logicalColumn = reportIsRtl ? colCount - 1 - i : i;
                    System.Drawing.Rectangle rect = new System.Drawing.Rectangle(x + i * colWidth, y, colWidth, rowHeight);
                    e.Graphics.FillRectangle(headerBrush, rect);
                    e.Graphics.DrawRectangle(Pens.Black, rect);
                    e.Graphics.DrawString(dt.Columns[logicalColumn].ColumnName, headerFont, Brushes.White, rect,
                        SelectPrintFormat(dt.Columns[logicalColumn].ColumnName, rtlFormat, ltrFormat));
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
                        int logicalColumn = reportIsRtl ? colCount - 1 - i : i;
                        System.Drawing.Rectangle rect = new System.Drawing.Rectangle(x + i * colWidth, y, colWidth, rowHeight);
                        e.Graphics.DrawRectangle(Pens.Gray, rect);
                        string cellText = row[logicalColumn] == DBNull.Value ? "" : row[logicalColumn].ToString();
                        e.Graphics.DrawString(cellText, cellFont, Brushes.Black, rect,
                            SelectPrintFormat(cellText, rtlFormat, ltrFormat));
                    }

                    y += rowHeight;
                    printRowIndex++;
                }

                using (StringFormat footerFormat = CreatePrintFormat(true))
                {
                    string footer = ReportOutputHelper.GetSchoolName(SchoolSettingsService.Current())
                        + " | الصفحة " + printPageNumber;
                    e.Graphics.DrawString(footer, infoFont, Brushes.DimGray,

                        new RectangleF(e.MarginBounds.Left, e.MarginBounds.Bottom + 12, e.MarginBounds.Width, 20), footerFormat);
                }
                e.HasMorePages = false;
            }
        }

        // تعريف الدالة CreatePrintFormat لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة SelectPrintFormat لتنفيذ العملية المرتبطة بها.
        private static StringFormat SelectPrintFormat(string text, StringFormat rtlFormat, StringFormat ltrFormat)
        {

            return ReportOutputHelper.ContainsArabic(text) ? rtlFormat : ltrFormat;
        }

        // تعريف الدالة SafeFileName لتنفيذ العملية المرتبطة بها.
        private string SafeFileName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "Report";

            foreach (char c in Path.GetInvalidFileNameChars())
                value = value.Replace(c, '_');


            return value.Replace(" ", "_");
        }

        // تعريف الدالة ClearReportGrid لتنفيذ العملية المرتبطة بها.
        private void ClearReportGrid()
        {
            currentReportData = null;
            dataGridViewReport.DataSource = null;
            lblRecordCount.Text = "عدد السجلات: 0";
            lblSummary.Text = "ملخص التقرير: لا توجد بيانات محملة.";
        }

        // تعريف الدالة UpdateFilterHintsForReportType لتنفيذ العملية المرتبطة بها.
        private void UpdateFilterHintsForReportType()
        {
            string selectedType = cmbReportType.Text;

            switch (selectedType)
            {
                case "كشف درجات طالب":
                    lblSearch.Text = "طالب/طلاب (اسم أو رقم):";
                    txtSearch.Focus();
                    break;

                case "كشف نتائج الفصل":
                    lblSearch.Text = "بحث:";
                    cmbTerm.Enabled = true;
                    cmbTerm.Focus();
                    break;

                case "تقرير الطلاب":
                case "تقرير القبول والتسجيل":
                case "تقرير توزيع الفصول":
                case "تقرير الرسوم":
                    lblSearch.Text = "بحث:";
                    break;

                case "تقرير الدرجات":
                    lblSearch.Text = "بحث:";
                    break;

                case "تقرير المعلمين":
                case "تقرير حضور المعلمين":
                case "تقرير الرواتب":
                    lblSearch.Text = "بحث:";
                    break;

                case "تقرير المستخدمين والصلاحيات":
                    lblSearch.Text = "بحث:";
                    break;

                case "تقرير المواد الدراسية":
                case "تقرير حضور الطلاب":
                case "تقرير المكتبة والإعارات":
                case "تقرير النقل المدرسي":
                case "تقرير المصروفات":
                    lblSearch.Text = "بحث:";
                    break;

                case "تقرير الحركة المالية":
                    lblSearch.Text = "بحث:";
                    break;

                default:
                    lblSearch.Text = "بحث:";
                    break;
            }
        }

        // تعريف الدالة ShowInfo لتنفيذ العملية المرتبطة بها.
        private void ShowInfo(string message)
        {

            UIHelper.ShowInfo(message);
        }

        // تعريف الدالة ShowWarning لتنفيذ العملية المرتبطة بها.
        private void ShowWarning(string message)
        {

            UIHelper.ShowWarning(message);
        }

    }
}
