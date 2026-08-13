using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using SchoolSystem.Models;
using SchoolSystem.Helpers;
using SchoolSystem.Services;

namespace SchoolSystem.UI
{
    public partial class DailyAttendanceForm : UserControl
    {
        private readonly StudentAttendanceService attendanceService = new StudentAttendanceService();
        private readonly ClassService classService = new ClassService();

        private DataTable currentAttendanceTable;

        public DailyAttendanceForm()
        {
            InitializeComponent();

            Dock = DockStyle.Fill;

            Load += DailyAttendanceForm_Load;
            txtSearch.TextChanged += txtSearch_TextChanged;
        }

        private async void DailyAttendanceForm_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                LoadStaticData();
                await LoadClassesAsync();

                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                ShowError("حدث خطأ أثناء تحميل واجهة التحضير:\n" + ex.Message);
            }
        }

        private void LoadStaticData()
        {
            dtpDate.Value = DateTime.Today;

            int year = DateTime.Now.Year;
            txtAcademicYear.Text = year + "/" + (year + 1);

            cmbSection.Items.Clear();
            cmbSection.Items.Add("أ");
            cmbSection.Items.Add("ب");
            cmbSection.Items.Add("ج");
            cmbSection.Items.Add("د");

            if (cmbSection.Items.Count > 0)
                cmbSection.SelectedIndex = 0;

            lblSummary.Text = "ملخص الحضور: لا توجد بيانات محملة.";
            lblRecordCount.Text = "عدد الطلاب: 0";
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

        private int GetClassId()
        {
            if (cmbClass.SelectedValue == null || cmbClass.SelectedValue is DataRowView)
                return 0;

            int value;
            int.TryParse(cmbClass.SelectedValue.ToString(), out value);
            return value;
        }

        private async void btnLoad_Click(object sender, EventArgs e)
        {
            await LoadAttendanceAsync();
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await LoadAttendanceAsync();
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

        private bool ValidateAttendanceFilters()
        {
            if (GetClassId() <= 0)
            {
                ShowWarning("يرجى اختيار الصف.");
                return false;
            }
            if (cmbSection.SelectedIndex < 0 || string.IsNullOrWhiteSpace(cmbSection.Text))
            {
                ShowWarning("يرجى اختيار الشعبة.");
                cmbSection.Focus();
                return false;
            }
            if (!IsValidAcademicYear(txtAcademicYear.Text))
            {
                ShowWarning("أدخل العام الدراسي بالصيغة الصحيحة: 2025/2026.");
                txtAcademicYear.Focus();
                return false;
            }
            if (dtpDate.Value.Date > DateTime.Today)
            {
                ShowWarning("لا يمكن تسجيل حضور بتاريخ مستقبلي.");
                dtpDate.Focus();
                return false;
            }
            return true;
        }

        private async Task LoadAttendanceAsync()
        {
            try
            {
                if (!ValidateAttendanceFilters())
                    return;

                int classId = GetClassId();

                Cursor = Cursors.WaitCursor;

                currentAttendanceTable = await Task.Run(() =>
                    attendanceService.GetAttendanceSheet(
                        classId,
                        cmbSection.Text,
                        txtAcademicYear.Text.Trim(),
                        dtpDate.Value.Date));

                dataGridViewAttendance.DataSource = currentAttendanceTable;

                FormatGrid();
                BuildSummary();

                lblRecordCount.Text = "عدد الطلاب: " + currentAttendanceTable.Rows.Count;

                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                ShowError("خطأ أثناء تحميل التحضير:\n" + ex.Message);
            }
        }

        private void FormatGrid()
        {
            if (dataGridViewAttendance.Columns.Count == 0)
                return;

            HideColumn("StudentID");
            HideColumn("AttendanceID");

            SetHeader("StudentNumber", "الرقم الأكاديمي");
            SetHeader("StudentName", "اسم الطالب");
            SetHeader("Gender", "الجنس");
            SetHeader("ArrivalTime", "وقت الوصول");
            SetHeader("Notes", "ملاحظات");

            MakeReadOnly("StudentNumber");
            MakeReadOnly("StudentName");
            MakeReadOnly("Gender");

            if (dataGridViewAttendance.Columns.Contains("Status"))
            {
                int index = dataGridViewAttendance.Columns["Status"].Index;
                dataGridViewAttendance.Columns.Remove("Status");

                DataGridViewComboBoxColumn col = new DataGridViewComboBoxColumn();
                col.Name = "Status";
                col.HeaderText = "الحالة";
                col.DataPropertyName = "Status";
                col.Items.Add("حاضر");
                col.Items.Add("غائب");
                col.Items.Add("متأخر");
                col.Items.Add("مستأذن");
                col.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                col.FlatStyle = FlatStyle.Flat;

                dataGridViewAttendance.Columns.Insert(index, col);
            }

            if (dataGridViewAttendance.Columns.Contains("ExcuseStatus"))
            {
                int index = dataGridViewAttendance.Columns["ExcuseStatus"].Index;
                dataGridViewAttendance.Columns.Remove("ExcuseStatus");

                DataGridViewComboBoxColumn col = new DataGridViewComboBoxColumn();
                col.Name = "ExcuseStatus";
                col.HeaderText = "العذر";
                col.DataPropertyName = "ExcuseStatus";
                col.Items.Add("بدون عذر");
                col.Items.Add("بعذر");
                col.Items.Add("غير محدد");
                col.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                col.FlatStyle = FlatStyle.Flat;

                dataGridViewAttendance.Columns.Insert(index, col);
            }

            dataGridViewAttendance.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void SetHeader(string columnName, string header)
        {
            if (dataGridViewAttendance.Columns.Contains(columnName))
                dataGridViewAttendance.Columns[columnName].HeaderText = header;
        }

        private void HideColumn(string columnName)
        {
            if (dataGridViewAttendance.Columns.Contains(columnName))
                dataGridViewAttendance.Columns[columnName].Visible = false;
        }

        private void MakeReadOnly(string columnName)
        {
            if (dataGridViewAttendance.Columns.Contains(columnName))
                dataGridViewAttendance.Columns[columnName].ReadOnly = true;
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (dataGridViewAttendance.Rows.Count == 0)
            {
                ShowWarning("لا توجد بيانات للحفظ.");
                return;
            }

            try
            {
                if (!ValidateAttendanceFilters())
                    return;

                if (dataGridViewAttendance.Columns.Contains("Status") && dataGridViewAttendance.Rows.Count > 0)
                {
                    foreach (DataGridViewRow validationRow in dataGridViewAttendance.Rows)
                    {
                        if (validationRow.IsNewRow)
                            continue;

                        string status = Convert.ToString(validationRow.Cells["Status"].Value).Trim();
                        if (status != "حاضر" && status != "غائب" && status != "متأخر" && status != "مستأذن")
                        {
                            ShowWarning("توجد حالة حضور غير صالحة. يرجى اختيار حالة لكل طالب.");
                            return;
                        }
                    }
                }

                int classId = GetClassId();
                Cursor = Cursors.WaitCursor;

                int savedCount = 0;

                foreach (DataGridViewRow row in dataGridViewAttendance.Rows)
                {
                    if (row.IsNewRow)
                        continue;

                    StudentAttendance item = new StudentAttendance();

                    item.StudentID = Convert.ToInt32(row.Cells["StudentID"].Value);
                    item.ClassID = classId;
                    item.Section = cmbSection.Text;
                    item.AcademicYear = txtAcademicYear.Text.Trim();
                    item.AttendanceDate = dtpDate.Value.Date;
                    item.Status = row.Cells["Status"].Value == null
                        ? "حاضر"
                        : row.Cells["Status"].Value.ToString();

                    item.ExcuseStatus = row.Cells["ExcuseStatus"].Value == null
                        ? "بدون عذر"
                        : row.Cells["ExcuseStatus"].Value.ToString();

                    item.Notes = row.Cells["Notes"].Value == null
                        ? ""
                        : row.Cells["Notes"].Value.ToString();

                    string arrival = row.Cells["ArrivalTime"].Value == null
                        ? ""
                        : row.Cells["ArrivalTime"].Value.ToString();

                    TimeSpan arrivalTime;

                    if (TimeSpan.TryParse(arrival, out arrivalTime))
                        item.ArrivalTime = arrivalTime;
                    else
                        item.ArrivalTime = null;

                    bool saved = await Task.Run(() => attendanceService.SaveAttendance(item));

                    if (saved)
                        savedCount++;
                }

                Cursor = Cursors.Default;

                ShowInfo("تم حفظ حضور " + savedCount + " طالب بنجاح.");

                await LoadAttendanceAsync();
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                ShowError("خطأ أثناء حفظ الحضور:\n" + ex.Message);
            }
        }

        private void btnMarkAllPresent_Click(object sender, EventArgs e)
        {
            if (dataGridViewAttendance.Rows.Count == 0)
            {
                ShowWarning("لا توجد بيانات.");
                return;
            }

            foreach (DataGridViewRow row in dataGridViewAttendance.Rows)
            {
                if (row.IsNewRow)
                    continue;

                row.Cells["Status"].Value = "حاضر";
                row.Cells["ExcuseStatus"].Value = "بدون عذر";
            }

            BuildSummary();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            currentAttendanceTable = null;
            dataGridViewAttendance.DataSource = null;
            lblSummary.Text = "ملخص الحضور: لا توجد بيانات محملة.";
            lblRecordCount.Text = "عدد الطلاب: 0";
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (currentAttendanceTable == null)
                return;

            string safe = UIHelper.EscapeDataViewFilterValue(txtSearch.Text);

            if (string.IsNullOrWhiteSpace(safe))
            {
                currentAttendanceTable.DefaultView.RowFilter = "";
            }
            else
            {
                currentAttendanceTable.DefaultView.RowFilter =
                    "StudentName LIKE '%" + safe + "%' OR StudentNumber LIKE '%" + safe + "%'";
            }

            lblRecordCount.Text = "عدد الطلاب: " + currentAttendanceTable.DefaultView.Count;
        }

        private void dataGridViewAttendance_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridViewAttendance.IsCurrentCellDirty)
                dataGridViewAttendance.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dataGridViewAttendance_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            BuildSummary();
        }

        private void BuildSummary()
        {
            int present = 0;
            int absent = 0;
            int late = 0;
            int excused = 0;

            foreach (DataGridViewRow row in dataGridViewAttendance.Rows)
            {
                if (row.IsNewRow)
                    continue;

                if (!dataGridViewAttendance.Columns.Contains("Status"))
                    continue;

                string status = row.Cells["Status"].Value == null
                    ? ""
                    : row.Cells["Status"].Value.ToString();

                if (status == "حاضر")
                    present++;
                else if (status == "غائب")
                    absent++;
                else if (status == "متأخر")
                    late++;
                else if (status == "مستأذن")
                    excused++;
            }

            int total = present + absent + late + excused;

            lblSummary.Text =
                "ملخص الحضور: " +
                "الإجمالي: " + total +
                " | حاضر: " + present +
                " | غائب: " + absent +
                " | متأخر: " + late +
                " | مستأذن: " + excused;
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
