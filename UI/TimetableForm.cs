using System;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using SchoolSystem.Models;
using SchoolSystem.Helpers;
using SchoolSystem.Services;
using SchoolSystem.DataAccess;
using SchoolSystem.Security;


namespace SchoolSystem.UI
{

    public partial class TimetableForm : UserControl
    {
        // تعريف الدالة TimetableService لتنفيذ العملية المرتبطة بها.
        private readonly TimetableService timetableService = new TimetableService();
        // تعريف الدالة AcademicReferenceRepository لتنفيذ العملية المرتبطة بها.
        private readonly AcademicReferenceRepository academicReferenceRepository = new AcademicReferenceRepository();
        // تعريف الدالة ClassService لتنفيذ العملية المرتبطة بها.
        private readonly ClassService classService = new ClassService();


        private int selectedTimetableId = 0;

        private DataTable allTimetable;

        private bool isLoading = false;

        // تعريف الدالة TimetableForm لتنفيذ العملية المرتبطة بها.
        public TimetableForm()
        {

            InitializeComponent();

            SchoolSystem.Helpers.UIHelper.ApplyStyle(this);
            ApplyCustomStyles();
            ConfigureTimetableGridAppearance();
            UIHelper.NormalizeActionBar(panelButtons);
            Dock = DockStyle.Fill;

            DataRefreshNotifier.DataChanged += DataRefreshNotifier_DataChanged;
            Disposed += (sender, e) =>
                DataRefreshNotifier.DataChanged -= DataRefreshNotifier_DataChanged;

            Load += TimetableForm_Load;

            cmbClass.SelectedIndexChanged += cmbClass_SelectedIndexChanged;

                txtYear.TextChanged += TxtYear_TextChanged;

            // تقييد حقل القاعة: أحرف وأرقام ومسافات وشرطات فقط.
            txtRoom.MaxLength = 50;
            txtRoom.KeyPress -= Room_KeyPress;
            txtRoom.KeyPress += Room_KeyPress;

            // تقييد حقل الملاحظات بحد أقصى للطول.
            txtNotes.MaxLength = 500;
        }

        private async void DataRefreshNotifier_DataChanged(
            object sender,
            DataRefreshEventArgs e)
        {
            if (IsDisposed || Disposing || !IsHandleCreated)
                return;

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

            if (ReferenceEquals(sender, this) || isLoading)
                return;

            if (!DataRefreshNotifier.IsRelated(e, "Teachers")
                && !DataRefreshNotifier.IsRelated(e, "Classes")
                && !DataRefreshNotifier.IsRelated(e, "Sections")
                && !DataRefreshNotifier.IsRelated(e, "Subjects")
                && !DataRefreshNotifier.IsRelated(e, "All"))
                return;

            try
            {
                isLoading = true;
                await LoadClassesAsync();
                await LoadSectionsAsync();
                await LoadTeachersAsync();
                await LoadSubjectsForClassAsync();
                await LoadTimetableAsync();
            }
            catch (Exception ex)
            {
                ApplicationLogger.LogException(
                    "TimetableForm: التحديث التلقائي",
                    ex);
            }
            finally
            {
                isLoading = false;
            }
        }

        // تعريف الدالة Room_KeyPress للسماح بحروف وأرقام ومسافات وشرطات فقط.
        private void Room_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e == null)
                return;

            if (char.IsControl(e.KeyChar))
                return;

            if (!char.IsLetterOrDigit(e.KeyChar) &&
                !char.IsWhiteSpace(e.KeyChar) &&
                e.KeyChar != '-' &&
                e.KeyChar != '/')
            {
                e.Handled = true;
            }
        }

        // عرض الجدول الدراسي بأسلوب Excel؛ التحرير يتم من عناصر الإدخال والأزرار المنفصلة.
        private void ConfigureTimetableGridAppearance()
        {
            if (dataGridViewTimetable == null)
                return;

            dataGridViewTimetable.SuspendLayout();
            try
            {
                dataGridViewTimetable.AllowUserToAddRows = false;
                dataGridViewTimetable.AllowUserToDeleteRows = false;
                dataGridViewTimetable.AllowUserToResizeRows = false;
                dataGridViewTimetable.AllowUserToResizeColumns = true;
                dataGridViewTimetable.AllowUserToOrderColumns = true;
                dataGridViewTimetable.ReadOnly = true;
                dataGridViewTimetable.MultiSelect = false;
                dataGridViewTimetable.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridViewTimetable.RowHeadersVisible = false;
                dataGridViewTimetable.RightToLeft = RightToLeft.Yes;
                dataGridViewTimetable.ScrollBars = ScrollBars.Both;
                dataGridViewTimetable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                dataGridViewTimetable.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
                dataGridViewTimetable.BackgroundColor = Color.White;
                dataGridViewTimetable.BorderStyle = BorderStyle.FixedSingle;
                dataGridViewTimetable.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                dataGridViewTimetable.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                dataGridViewTimetable.EnableHeadersVisualStyles = false;
                dataGridViewTimetable.GridColor = UIHelper.BorderColor;
                dataGridViewTimetable.ColumnHeadersHeight = 44;
                dataGridViewTimetable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                dataGridViewTimetable.RowTemplate.Height = 38;
                dataGridViewTimetable.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;

                dataGridViewTimetable.ColumnHeadersDefaultCellStyle.BackColor = UIHelper.PrimaryColor;
                dataGridViewTimetable.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dataGridViewTimetable.ColumnHeadersDefaultCellStyle.Font = new Font(UIHelper.FontFamily, 10F, FontStyle.Bold);
                dataGridViewTimetable.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewTimetable.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dataGridViewTimetable.ColumnHeadersDefaultCellStyle.SelectionBackColor = UIHelper.PrimaryColor;
                dataGridViewTimetable.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
                dataGridViewTimetable.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 4, 8, 4);

                dataGridViewTimetable.DefaultCellStyle.BackColor = Color.White;
                dataGridViewTimetable.DefaultCellStyle.ForeColor = UIHelper.TextColor;
                dataGridViewTimetable.DefaultCellStyle.Font = new Font(UIHelper.FontFamily, 9.5F);
                dataGridViewTimetable.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridViewTimetable.DefaultCellStyle.SelectionBackColor = UIHelper.AccentColor;
                dataGridViewTimetable.DefaultCellStyle.SelectionForeColor = Color.White;
                dataGridViewTimetable.DefaultCellStyle.Padding = new Padding(8, 4, 8, 4);
                dataGridViewTimetable.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

                dataGridViewTimetable.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(241, 245, 249);
                dataGridViewTimetable.AlternatingRowsDefaultCellStyle.ForeColor = UIHelper.TextColor;
                dataGridViewTimetable.AlternatingRowsDefaultCellStyle.SelectionBackColor = UIHelper.AccentColor;
                dataGridViewTimetable.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.White;
                dataGridViewTimetable.RowsDefaultCellStyle.BackColor = Color.White;
                dataGridViewTimetable.RowsDefaultCellStyle.ForeColor = UIHelper.TextColor;
                dataGridViewTimetable.RowsDefaultCellStyle.SelectionBackColor = UIHelper.AccentColor;
                dataGridViewTimetable.RowsDefaultCellStyle.SelectionForeColor = Color.White;
            }
            finally
            {
                dataGridViewTimetable.ResumeLayout();
            }
        }

        private void SetTimetableColumnWidth(string name, int width)
        {
            if (!dataGridViewTimetable.Columns.Contains(name))
                return;

            DataGridViewColumn column = dataGridViewTimetable.Columns[name];
            if (!column.Visible)
                return;

            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            column.MinimumWidth = Math.Min(100, Math.Max(55, width));
            column.Width = width;
            column.Resizable = DataGridViewTriState.True;
            column.SortMode = DataGridViewColumnSortMode.Automatic;
        }

        // تعريف الدالة ApplyCustomStyles لتنفيذ العملية المرتبطة بها.
        private void ApplyCustomStyles()
        {
            UIHelper.StyleDataGridView(dataGridViewTimetable);
            UIHelper.StylePrimaryButton(btnAdd);
            UIHelper.StylePrimaryButton(btnUpdate);
            UIHelper.StyleDangerButton(btnDelete);
            UIHelper.StyleButton(btnClear, UIHelper.NeutralColor);
            UIHelper.StyleButton(btnRefresh, UIHelper.NeutralColor);
            UIHelper.StyleTextBox(txtTimetableID);
            UIHelper.StyleTextBox(txtYear);
            UIHelper.StyleTextBox(txtRoom);
            UIHelper.StyleTextBox(txtNotes);
            UIHelper.StyleTextBox(txtSearch);
            UIHelper.StyleComboBox(cmbTerm);
            UIHelper.StyleComboBox(cmbClass);
            UIHelper.StyleComboBox(cmbSection);
            UIHelper.StyleComboBox(cmbSubject);
            UIHelper.StyleComboBox(cmbTeacher);
            UIHelper.StyleComboBox(cmbDay);
            lblRecordCount.ForeColor = UIHelper.MutedTextColor;
        }

        // تعريف الدالة TimetableForm_Load لتنفيذ العملية المرتبطة بها.
        private async void TimetableForm_Load(object sender, EventArgs e)
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                isLoading = true;

                LoadStaticData();

                await LoadClassesAsync();

                await LoadSectionsAsync();

                await LoadTeachersAsync();

                isLoading = false;


                await LoadSubjectsForClassAsync();

                await LoadTimetableAsync();

                ClearFields();
            }

            catch (Exception ex)
            {
                isLoading = false;
                UIHelper.ShowException("تحميل واجهة الجدول الدراسي", ex);
            }
        }

        // تعريف الدالة LoadStaticData لتنفيذ العملية المرتبطة بها.
        private void LoadStaticData()
        {
            cmbDay.Items.Clear();
            cmbDay.Items.Add("السبت");
            cmbDay.Items.Add("الأحد");
            cmbDay.Items.Add("الاثنين");
            cmbDay.Items.Add("الثلاثاء");
            cmbDay.Items.Add("الأربعاء");
            cmbDay.Items.Add("الخميس");
            cmbDay.SelectedIndex = 0;

            cmbSection.DataSource = null;
            cmbSection.Items.Clear();
            cmbSection.Enabled = false;

            cmbTerm.DataSource = null;
            cmbTerm.Items.Clear();
            cmbTerm.DisplayMember = "TermName";
            cmbTerm.ValueMember = "TermID";
            DataTable terms = academicReferenceRepository.GetActiveTerms();
            cmbTerm.DataSource = terms;
            cmbTerm.Enabled = terms.Rows.Count > 0;
            if (terms.Rows.Count > 0)
                cmbTerm.SelectedIndex = 0;

            nudPeriodNo.Value = 1;


            string dbYear = SchoolSystem.Helpers.AcademicYearHelper.GetCurrentAcademicYear();
            if (!string.IsNullOrWhiteSpace(dbYear))
            {
                txtYear.Text = dbYear;
            }

            else
            {
                int year = DateTime.Now.Year;
                txtYear.Text = year + "/" + (year + 1);
            }

            dtpStart.Format = DateTimePickerFormat.Custom;
            dtpStart.CustomFormat = "HH:mm";
            dtpStart.ShowUpDown = true;
            dtpStart.Value = DateTime.Today.AddHours(8);

            dtpEnd.Format = DateTimePickerFormat.Custom;
            dtpEnd.CustomFormat = "HH:mm";
            dtpEnd.ShowUpDown = true;
            dtpEnd.Value = DateTime.Today.AddHours(8).AddMinutes(45);

            chkIsActive.Checked = true;
        }

        // تعريف الدالة LoadClassesAsync لتنفيذ العملية المرتبطة بها.
        private async Task LoadClassesAsync()
        {

            DataTable classes = await Task.Run(() => classService.GetAllClasses());

            cmbClass.DataSource = classes;
            cmbClass.DisplayMember = "ClassName";
            cmbClass.ValueMember = "ClassID";

            if (cmbClass.Items.Count > 0)
                cmbClass.SelectedIndex = 0;
        }

        // تعريف الدالة LoadTeachersAsync لتنفيذ العملية المرتبطة بها.
        private async Task LoadTeachersAsync()
        {

            DataTable teachers = await Task.Run(() => timetableService.GetTeachers());

            cmbTeacher.DataSource = teachers;
            cmbTeacher.DisplayMember = "FullName";
            cmbTeacher.ValueMember = "TeacherID";

            if (cmbTeacher.Items.Count > 0)
                cmbTeacher.SelectedIndex = 0;
        }

        // تعريف الدالة GetClassId لتنفيذ العملية المرتبطة بها.
        private int GetClassId()
        {
            if (cmbClass.SelectedValue == null || cmbClass.SelectedValue is DataRowView)
                return 0;

            int value;
            int.TryParse(cmbClass.SelectedValue.ToString(), out value);

            return value;
        }

        // تعريف الدالة GetSubjectId لتنفيذ العملية المرتبطة بها.
        private int GetSubjectId()
        {
            if (cmbSubject.SelectedValue == null || cmbSubject.SelectedValue is DataRowView)
                return 0;

            int value;
            int.TryParse(cmbSubject.SelectedValue.ToString(), out value);

            return value;
        }

        // تعريف الدالة GetTeacherId لتنفيذ العملية المرتبطة بها.
        private int GetTeacherId()
        {
            if (cmbTeacher.SelectedValue == null || cmbTeacher.SelectedValue is DataRowView)
                return 0;

            int value;
            int.TryParse(cmbTeacher.SelectedValue.ToString(), out value);

            return value;
        }

        // تعريف الدالة cmbClass_SelectedIndexChanged لتنفيذ العملية المرتبطة بها.
        private async void cmbClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoading)
                return;


            await LoadSectionsAsync();

            await LoadSubjectsForClassAsync();
        }

        // تعريف الدالة TxtYear_TextChanged لتنفيذ العملية المرتبطة بها.
        private async void TxtYear_TextChanged(object sender, EventArgs e)
        {
            if (isLoading) return;

            await LoadSectionsAsync();
        }

        // تعريف الدالة LoadSectionsAsync لتنفيذ العملية المرتبطة بها.
        private async Task LoadSectionsAsync()
        {
            int classId = GetClassId();
            string academicYear = txtYear.Text.Trim();

            cmbSection.DataSource = null;
            cmbSection.Items.Clear();
            cmbSection.Enabled = false;

            if (classId <= 0 || !IsValidAcademicYear(academicYear))
                return;

            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {

                DataTable sections = await Task.Run(() => timetableService.GetSections(classId, academicYear));
                if (sections == null || sections.Rows.Count == 0)
                {
                    cmbSection.DataSource = null;
                    cmbSection.Items.Clear();
                    cmbSection.Enabled = false;
                    return;
                }

                cmbSection.DataSource = sections;
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
                UIHelper.ShowException("تحميل شعب الجدول الدراسي", ex);
            }
        }

        // تعريف الدالة LoadSubjectsForClassAsync لتنفيذ العملية المرتبطة بها.
        private async Task LoadSubjectsForClassAsync()
        {
            int classId = GetClassId();

            if (classId <= 0)
                return;


            DataTable subjects = await Task.Run(() => timetableService.GetSubjectsByClass(classId));

            cmbSubject.DataSource = subjects;
            cmbSubject.DisplayMember = "SubjectName";
            cmbSubject.ValueMember = "SubjectID";

            if (cmbSubject.Items.Count > 0)
            {
                cmbSubject.SelectedIndex = 0;
                cmbSubject.Enabled = true;
            }

            else
            {
                cmbSubject.Enabled = false;
                ShowWarning("لا توجد مواد مفعلة لهذا الصف. راجع إدارة المواد.");
            }
        }

        // تعريف الدالة LoadTimetableAsync لتنفيذ العملية المرتبطة بها.
        private async Task LoadTimetableAsync()
        {

            allTimetable = await Task.Run(() => timetableService.GetAllTimetable());
            ApplyFilter(txtSearch.Text.Trim());
        }

        // تعريف الدالة ApplyFilter لتنفيذ العملية المرتبطة بها.
        private void ApplyFilter(string searchText)
        {
            if (allTimetable == null)
                return;

            DataView dv = allTimetable.DefaultView;

            if (string.IsNullOrWhiteSpace(searchText))
            {
                dv.RowFilter = "";
            }

            else
            {
                dv.RowFilter = UIHelper.BuildDataViewSearchFilter(searchText,
                    "ClassName", "SubjectName", "TeacherName", "DayName", "RoomName");
            }

            dataGridViewTimetable.DataSource = dv;
            lblRecordCount.Text = "عدد الحصص: " + dv.Count;

            FormatGrid();
        }

        // تعريف الدالة FormatGrid لتنفيذ العملية المرتبطة بها.
        private void FormatGrid()
        {
            if (dataGridViewTimetable.Columns.Count == 0)
                return;

            SetHeader("TimetableID", "الرقم");
            SetHeader("AcademicYear", "العام الدراسي");
            SetHeader("TermName", "الفصل");
            SetHeader("ClassName", "الصف");
            SetHeader("Section", "الشعبة");
            SetHeader("DayName", "اليوم");
            SetHeader("PeriodNo", "الحصة");
            SetHeader("StartTime", "البداية");
            SetHeader("EndTime", "النهاية");
            SetHeader("SubjectName", "المادة");
            SetHeader("TeacherName", "المعلم");
            SetHeader("RoomName", "القاعة");
            SetHeader("Notes", "ملاحظات");
            SetHeader("IsActive", "نشط");

            HideColumn("ClassID");
            HideColumn("SubjectID");
            HideColumn("TeacherID");

            dataGridViewTimetable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            SetTimetableColumnWidth("TimetableID", 80);
            SetTimetableColumnWidth("AcademicYear", 120);
            SetTimetableColumnWidth("TermName", 115);
            SetTimetableColumnWidth("ClassName", 130);
            SetTimetableColumnWidth("Section", 100);
            SetTimetableColumnWidth("DayName", 100);
            SetTimetableColumnWidth("PeriodNo", 85);
            SetTimetableColumnWidth("StartTime", 105);
            SetTimetableColumnWidth("EndTime", 105);
            SetTimetableColumnWidth("SubjectName", 175);
            SetTimetableColumnWidth("TeacherName", 180);
            SetTimetableColumnWidth("RoomName", 130);
            SetTimetableColumnWidth("Notes", 200);
            SetTimetableColumnWidth("IsActive", 85);
        }

        // تعريف الدالة SetHeader لتنفيذ العملية المرتبطة بها.
        private void SetHeader(string columnName, string header)
        {
            if (dataGridViewTimetable.Columns.Contains(columnName))
                dataGridViewTimetable.Columns[columnName].HeaderText = header;
        }

        // تعريف الدالة HideColumn لتنفيذ العملية المرتبطة بها.
        private void HideColumn(string columnName)
        {
            if (dataGridViewTimetable.Columns.Contains(columnName))
                dataGridViewTimetable.Columns[columnName].Visible = false;
        }

        // تعريف الدالة IsValidAcademicYear لتنفيذ العملية المرتبطة بها.
        private bool IsValidAcademicYear(string academicYear)
        {
            if (string.IsNullOrWhiteSpace(academicYear))
                return false;

            string[] parts = academicYear.Trim().Replace('-', '/').Split('/');
            int firstYear;
            int secondYear;

            if (parts.Length != 2 || parts[0].Length != 4 || parts[1].Length != 4)
                return false;

            if (!int.TryParse(parts[0], out firstYear) || !int.TryParse(parts[1], out secondYear))
                return false;


            return secondYear == firstYear + 1;
        }

        // تعريف الدالة BuildModel لتنفيذ العملية المرتبطة بها.
        private TimetableEntry BuildModel()
        {
            int classId = GetClassId();
            int subjectId = GetSubjectId();
            int teacherId = GetTeacherId();
            string academicYear = txtYear.Text.Trim();
            TimeSpan startTime = dtpStart.Value.TimeOfDay;
            TimeSpan endTime = dtpEnd.Value.TimeOfDay;

            if (classId <= 0 || cmbClass.SelectedIndex < 0)

                throw new InvalidOperationException("يرجى اختيار الصف.");

            if (subjectId <= 0 || cmbSubject.SelectedIndex < 0)

                throw new InvalidOperationException("يرجى اختيار المادة.");

            if (teacherId <= 0 || cmbTeacher.SelectedIndex < 0)

                throw new InvalidOperationException("يرجى اختيار المعلم.");

            if (!cmbSection.Enabled || cmbSection.SelectedIndex < 0 || string.IsNullOrWhiteSpace(cmbSection.Text) ||
                cmbSection.Text.StartsWith("لا توجد") || cmbSection.Text.StartsWith("تعذر"))

                throw new InvalidOperationException("لا توجد شعبة فعلية متاحة لهذا الصف والعام الدراسي.");

            if (!IsValidAcademicYear(academicYear))

                throw new InvalidOperationException("أدخل العام الدراسي بالصيغة الصحيحة، مثل 2025/2026.");

            if (cmbDay.SelectedIndex < 0 || cmbTerm.SelectedIndex < 0)

                throw new InvalidOperationException("يرجى اختيار اليوم والفصل الدراسي.");

            if (endTime <= startTime)

                throw new InvalidOperationException("وقت نهاية الحصة يجب أن يكون بعد وقت بدايتها.");

            TimetableEntry item = new TimetableEntry();
            item.TimetableID = selectedTimetableId;
            item.ClassID = classId;
            item.Section = cmbSection.Text.Trim();
            item.SubjectID = subjectId;
            item.TeacherID = teacherId;
            item.AcademicYear = academicYear;
            item.TermName = cmbTerm.Text;
            item.DayName = cmbDay.Text;
            item.PeriodNo = Convert.ToInt32(nudPeriodNo.Value);
            item.StartTime = startTime;
            item.EndTime = endTime;
            item.RoomName = txtRoom.Text.Trim();
            item.Notes = txtNotes.Text.Trim();
            item.IsActive = chkIsActive.Checked;


            return item;
        }

        // تعريف الدالة btnAdd_Click لتنفيذ العملية المرتبطة بها.
        private async void btnAdd_Click(object sender, EventArgs e)
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                CurrentUser.DemandAction("Timetable", "Add", "ليس لديك صلاحية إضافة حصص الجدول الدراسي.");
                TimetableEntry item = BuildModel();


                bool added = await Task.Run(() => timetableService.AddTimetable(item));
                if (!added)
                {
                    ShowWarning("تعذر حفظ الحصة. لم تُنشأ سجلات جديدة.");

                    return;
                }

                ShowInfo("تمت إضافة الحصة بنجاح.");


                await LoadTimetableAsync();
                ClearFields();
            }

            catch (ArgumentException ex)
            {
                ApplicationLogger.LogException("شاشة الجدول الدراسي: إضافة الحصة", ex);
                ShowWarning(ex.Message);
            }

            catch (InvalidOperationException ex)
            {
                ApplicationLogger.LogException("شاشة الجدول الدراسي: إضافة الحصة", ex);
                ShowWarning(ex.Message);
            }

            catch (Exception ex)
            {
                UIHelper.ShowException("إضافة الحصة", ex);
            }
        }

        // تعريف الدالة btnUpdate_Click لتنفيذ العملية المرتبطة بها.
        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.DemandAction("Timetable", "Edit", "ليس لديك صلاحية تعديل الجدول الدراسي.");
            if (selectedTimetableId <= 0)
            {
                ShowWarning("اختر حصة من الجدول أولاً.");

                return;
            }

            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                TimetableEntry item = BuildModel();
                item.TimetableID = selectedTimetableId;


                bool updated = await Task.Run(() => timetableService.UpdateTimetable(item));

                if (updated)
                {
                    ShowInfo("تم تعديل الحصة بنجاح.");

                    await LoadTimetableAsync();
                    ClearFields();
                }

                else
                {
                    ShowWarning("لم يتم العثور على الحصة.");
                }
            }

            catch (ArgumentException ex)
            {
                ApplicationLogger.LogException("شاشة الجدول الدراسي: تعديل الحصة", ex);
                ShowWarning(ex.Message);
            }

            catch (InvalidOperationException ex)
            {
                ApplicationLogger.LogException("شاشة الجدول الدراسي: تعديل الحصة", ex);
                ShowWarning(ex.Message);
            }

            catch (Exception ex)
            {
                UIHelper.ShowException("تعديل الحصة", ex);
            }
        }

        // تعريف الدالة btnDelete_Click لتنفيذ العملية المرتبطة بها.
        private async void btnDelete_Click(object sender, EventArgs e)
        {
            CurrentUser.DemandAction("Timetable", "Delete", "ليس لديك صلاحية حذف حصص الجدول الدراسي.");
            if (selectedTimetableId <= 0)
            {
                ShowWarning("اختر حصة من الجدول أولاً.");

                return;
            }


            bool result = UIHelper.ShowConfirmation(
                "هل تريد حذف هذه الحصة من الجدول؟",
                "تأكيد الحذف");

            if (!result)
                return;

            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {

                bool deleted = await Task.Run(() => timetableService.DeleteTimetable(selectedTimetableId));

                if (deleted)
                {
                    ShowInfo("تم حذف الحصة بنجاح.");

                    await LoadTimetableAsync();
                    ClearFields();
                }

                else
                {
                    ShowWarning("لم يتم العثور على الحصة.");
                }
            }

            catch (Exception ex)
            {
                UIHelper.ShowException("حذف الحصة", ex);
            }
        }

        // تعريف الدالة btnClear_Click لتنفيذ العملية المرتبطة بها.
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        // تعريف الدالة txtSearch_TextChanged لتنفيذ العملية المرتبطة بها.
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter(txtSearch.Text.Trim());
        }

        // تعريف الدالة btnRefresh_Click لتنفيذ العملية المرتبطة بها.
        private async void btnRefresh_Click(object sender, EventArgs e)
        {

            await LoadTimetableAsync();
        }

        // تعريف الدالة dataGridViewTimetable_CellClick لتنفيذ العملية المرتبطة بها.
        private async void dataGridViewTimetable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dataGridViewTimetable.Rows.Count == 0)
                return;

            DataRowView rowView = dataGridViewTimetable.Rows[e.RowIndex].DataBoundItem as DataRowView;

            if (rowView == null)
                return;

            try
            {
                await FillFieldsAsync(rowView.Row);
            }

            catch (Exception ex)
            {
                UIHelper.ShowException("عرض تفاصيل الحصة المحددة", ex);
            }
        }

        // تعريف الدالة FillFieldsAsync لتنفيذ العملية المرتبطة بها.
        private async Task FillFieldsAsync(DataRow row)
        {
            selectedTimetableId = ReadInt(row["TimetableID"], 0);

            txtTimetableID.Text = selectedTimetableId.ToString();
            txtYear.Text = row["AcademicYear"].ToString();
            cmbTerm.Text = row["TermName"].ToString();
            cmbSection.Text = row["Section"].ToString();
            cmbDay.Text = row["DayName"].ToString();

            int classId = ReadInt(row["ClassID"], 0);
            if (classId > 0)
                cmbClass.SelectedValue = classId;


            await LoadSubjectsForClassAsync();

            int subjectId = ReadInt(row["SubjectID"], 0);
            if (subjectId > 0)
                SelectComboValue(cmbSubject, subjectId);

            int teacherId = ReadInt(row["TeacherID"], 0);
            if (teacherId > 0)
                SelectComboValue(cmbTeacher, teacherId);

            nudPeriodNo.Value = ClampToRange(ReadInt(row["PeriodNo"], 1), nudPeriodNo);

            TimeSpan start = ReadTime(row["StartTime"], new TimeSpan(8, 0, 0));
            TimeSpan end = ReadTime(row["EndTime"], start.Add(TimeSpan.FromMinutes(45)));
            if (end <= start)
                end = start.Add(TimeSpan.FromMinutes(45));

            // إنهاء كتلة أو معاملة قاعدة البيانات الحالية.
            dtpStart.Value = DateTime.Today.Add(start);
            dtpEnd.Value = DateTime.Today.Add(end);

            txtRoom.Text = row["RoomName"] == DBNull.Value ? "" : row["RoomName"].ToString();
            txtNotes.Text = row["Notes"] == DBNull.Value ? "" : row["Notes"].ToString();
            chkIsActive.Checked = row["IsActive"] != DBNull.Value && Convert.ToBoolean(row["IsActive"]);
        }

        // تعريف الدالة ReadInt لتنفيذ العملية المرتبطة بها.
        private static int ReadInt(object value, int fallback)
        {
            if (value == null || value == DBNull.Value)
                return fallback;

            int parsed;

            return int.TryParse(value.ToString(), out parsed) ? parsed : fallback;
        }

        // تعريف الدالة SelectComboValue لتنفيذ العملية المرتبطة بها.
        private static void SelectComboValue(ComboBox combo, int value)
        {
            if (combo == null)
                return;

            if (string.IsNullOrEmpty(combo.ValueMember))
            {
                foreach (var item in combo.Items)
                {
                    if (combo.GetItemText(item) == value.ToString())
                    {
                        combo.SelectedItem = item;

                        return;
                    }
                }

                return;
            }

            foreach (var item in combo.Items)
            {
                if (item == null)
                    continue;

                var prop = item.GetType().GetProperty(combo.ValueMember);
                if (prop == null)
                    continue;

                // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
                try
                {
                    if (Convert.ToInt32(prop.GetValue(item, null)) == value)
                    {
                        combo.SelectedItem = item;

                        return;
                    }
                }

                catch (Exception ex)
                {
                    ApplicationLogger.LogException("قراءة قيمة القائمة في الجدول الدراسي", ex);
                }
            }
        }

        // تعريف الدالة ClampToRange لتنفيذ العملية المرتبطة بها.
        private static decimal ClampToRange(int value, NumericUpDown nud)
        {
            if (nud.Minimum > value)
                return nud.Minimum;
            if (nud.Maximum < value)
                return nud.Maximum;

            return value;
        }

        // تعريف الدالة ReadTime لتنفيذ العملية المرتبطة بها.
        private static TimeSpan ReadTime(object value, TimeSpan fallback)
        {
            if (value == null || value == DBNull.Value)
                return fallback;

            TimeSpan parsed;

            return TimeSpan.TryParse(value.ToString(), out parsed) ? parsed : fallback;
        }

        // تعريف الدالة ClearFields لتنفيذ العملية المرتبطة بها.
        private void ClearFields()
        {
            selectedTimetableId = 0;
            txtTimetableID.Clear();

            if (cmbClass.Items.Count > 0)
                cmbClass.SelectedIndex = 0;

            if (cmbSection.Items.Count > 0)
                cmbSection.SelectedIndex = 0;

            if (cmbSubject.Items.Count > 0)
                cmbSubject.SelectedIndex = 0;

            if (cmbTeacher.Items.Count > 0)
                cmbTeacher.SelectedIndex = 0;

            if (cmbDay.Items.Count > 0)
                cmbDay.SelectedIndex = 0;

            if (cmbTerm.Items.Count > 0)
                cmbTerm.SelectedIndex = 0;

            nudPeriodNo.Value = 1;
            dtpStart.Value = DateTime.Today.AddHours(8);
            dtpEnd.Value = DateTime.Today.AddHours(8).AddMinutes(45);
            txtRoom.Clear();
            txtNotes.Clear();
            chkIsActive.Checked = true;
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

        // تعريف الدالة ShowError لتنفيذ العملية المرتبطة بها.
        private void ShowError(string message)
        {

            UIHelper.ShowError(message);
        }
    }
}
