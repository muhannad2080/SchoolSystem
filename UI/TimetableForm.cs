using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using SchoolSystem.Models;
using SchoolSystem.Helpers;
using SchoolSystem.Services;

namespace SchoolSystem.UI
{
    public partial class TimetableForm : UserControl
    {
        private readonly TimetableService timetableService = new TimetableService();
        private readonly ClassService classService = new ClassService();

        private int selectedTimetableId = 0;
        private DataTable allTimetable;
        private bool isLoading = false;

        public TimetableForm()
        {
            InitializeComponent();
            SchoolSystem.Helpers.UIHelper.ApplyStyle(this);
            ApplyCustomStyles();
            Dock = DockStyle.Fill;

            Load += TimetableForm_Load;
            cmbClass.SelectedIndexChanged += cmbClass_SelectedIndexChanged;
        }

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

        private async void TimetableForm_Load(object sender, EventArgs e)
        {
            try
            {
                isLoading = true;

                LoadStaticData();
                await LoadClassesAsync();
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

            cmbSection.Items.Clear();
            cmbSection.Items.Add("أ");
            cmbSection.Items.Add("ب");
            cmbSection.Items.Add("ج");
            cmbSection.Items.Add("د");
            cmbSection.SelectedIndex = 0;

            cmbTerm.Items.Clear();
            cmbTerm.Items.Add("الفصل الأول");
            cmbTerm.Items.Add("الفصل الثاني");
            cmbTerm.SelectedIndex = 0;

            nudPeriodNo.Value = 1;

            int year = DateTime.Now.Year;
            txtYear.Text = year + "/" + (year + 1);

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

        private async Task LoadClassesAsync()
        {
            DataTable classes = await Task.Run(() => classService.GetAllClasses());

            cmbClass.DataSource = classes;
            cmbClass.DisplayMember = "ClassName";
            cmbClass.ValueMember = "ClassID";

            if (cmbClass.Items.Count > 0)
                cmbClass.SelectedIndex = 0;
        }

        private async Task LoadTeachersAsync()
        {
            DataTable teachers = await Task.Run(() => timetableService.GetTeachers());

            cmbTeacher.DataSource = teachers;
            cmbTeacher.DisplayMember = "FullName";
            cmbTeacher.ValueMember = "TeacherID";

            if (cmbTeacher.Items.Count > 0)
                cmbTeacher.SelectedIndex = 0;
        }

        private int GetClassId()
        {
            if (cmbClass.SelectedValue == null || cmbClass.SelectedValue is DataRowView)
                return 0;

            int value;
            int.TryParse(cmbClass.SelectedValue.ToString(), out value);
            return value;
        }

        private int GetSubjectId()
        {
            if (cmbSubject.SelectedValue == null || cmbSubject.SelectedValue is DataRowView)
                return 0;

            int value;
            int.TryParse(cmbSubject.SelectedValue.ToString(), out value);
            return value;
        }

        private int GetTeacherId()
        {
            if (cmbTeacher.SelectedValue == null || cmbTeacher.SelectedValue is DataRowView)
                return 0;

            int value;
            int.TryParse(cmbTeacher.SelectedValue.ToString(), out value);
            return value;
        }

        private async void cmbClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoading)
                return;

            await LoadSubjectsForClassAsync();
        }

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

        private async Task LoadTimetableAsync()
        {
            allTimetable = await Task.Run(() => timetableService.GetAllTimetable());
            ApplyFilter(txtSearch.Text.Trim());
        }

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
                string safe = UIHelper.EscapeDataViewFilterValue(searchText);

                dv.RowFilter =
                    "ClassName LIKE '%" + safe + "%' OR " +
                    "SubjectName LIKE '%" + safe + "%' OR " +
                    "TeacherName LIKE '%" + safe + "%' OR " +
                    "DayName LIKE '%" + safe + "%' OR " +
                    "RoomName LIKE '%" + safe + "%'";
            }

            dataGridViewTimetable.DataSource = dv;
            lblRecordCount.Text = "عدد الحصص: " + dv.Count;

            FormatGrid();
        }

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

            dataGridViewTimetable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void SetHeader(string columnName, string header)
        {
            if (dataGridViewTimetable.Columns.Contains(columnName))
                dataGridViewTimetable.Columns[columnName].HeaderText = header;
        }

        private void HideColumn(string columnName)
        {
            if (dataGridViewTimetable.Columns.Contains(columnName))
                dataGridViewTimetable.Columns[columnName].Visible = false;
        }

        private bool IsValidAcademicYear(string academicYear)
        {
            if (string.IsNullOrWhiteSpace(academicYear))
                return false;

            string[] parts = academicYear.Trim().Split('/');
            int firstYear;
            int secondYear;

            if (parts.Length != 2 || parts[0].Length != 4 || parts[1].Length != 4)
                return false;

            if (!int.TryParse(parts[0], out firstYear) || !int.TryParse(parts[1], out secondYear))
                return false;

            return firstYear >= 2000 && firstYear <= 2100 && secondYear == firstYear + 1;
        }

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

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                TimetableEntry item = BuildModel();

                await Task.Run(() => timetableService.AddTimetable(item));

                ShowInfo("تمت إضافة الحصة بنجاح.");

                await LoadTimetableAsync();
                ClearFields();
            }
            catch (ArgumentException ex)
            {
                ShowWarning(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                ShowWarning(ex.Message);
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("إضافة الحصة", ex);
            }
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedTimetableId <= 0)
            {
                ShowWarning("اختر حصة من الجدول أولاً.");
                return;
            }

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
                ShowWarning(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                ShowWarning(ex.Message);
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("تعديل الحصة", ex);
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedTimetableId <= 0)
            {
                ShowWarning("اختر حصة من الجدول أولاً.");
                return;
            }

            DialogResult result = MessageBox.Show(
                "هل تريد حذف هذه الحصة من الجدول؟",
                "تأكيد الحذف",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2,
                MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);

            if (result != DialogResult.Yes)
                return;

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

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter(txtSearch.Text.Trim());
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await LoadTimetableAsync();
        }

        private void dataGridViewTimetable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dataGridViewTimetable.Rows.Count == 0)
                return;

            DataRowView rowView = dataGridViewTimetable.Rows[e.RowIndex].DataBoundItem as DataRowView;

            if (rowView == null)
                return;

            FillFields(rowView.Row);
        }

        private async void FillFields(DataRow row)
        {
            selectedTimetableId = Convert.ToInt32(row["TimetableID"]);

            txtTimetableID.Text = selectedTimetableId.ToString();
            txtYear.Text = row["AcademicYear"].ToString();
            cmbTerm.Text = row["TermName"].ToString();
            cmbSection.Text = row["Section"].ToString();
            cmbDay.Text = row["DayName"].ToString();

            if (row["ClassID"] != DBNull.Value)
                cmbClass.SelectedValue = Convert.ToInt32(row["ClassID"]);

            await LoadSubjectsForClassAsync();

            if (row["SubjectID"] != DBNull.Value)
                cmbSubject.SelectedValue = Convert.ToInt32(row["SubjectID"]);

            if (row["TeacherID"] != DBNull.Value)
                cmbTeacher.SelectedValue = Convert.ToInt32(row["TeacherID"]);

            nudPeriodNo.Value = Convert.ToDecimal(row["PeriodNo"]);

            TimeSpan start = TimeSpan.Parse(row["StartTime"].ToString());
            TimeSpan end = TimeSpan.Parse(row["EndTime"].ToString());

            dtpStart.Value = DateTime.Today.Add(start);
            dtpEnd.Value = DateTime.Today.Add(end);

            txtRoom.Text = row["RoomName"] == DBNull.Value ? "" : row["RoomName"].ToString();
            txtNotes.Text = row["Notes"] == DBNull.Value ? "" : row["Notes"].ToString();
            chkIsActive.Checked = row["IsActive"] != DBNull.Value && Convert.ToBoolean(row["IsActive"]);
        }

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

        private void ShowInfo(string message)
        {
            MessageBox.Show(message, "معلومة", MessageBoxButtons.OK, MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
        }

        private void ShowWarning(string message)
        {
            MessageBox.Show(message, "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
        }

        private void ShowError(string message)
        {
            MessageBox.Show(message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error,
                MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
        }
    }
}
