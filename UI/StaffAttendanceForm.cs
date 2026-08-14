using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SchoolSystem.Models;
using SchoolSystem.Helpers;
using SchoolSystem.Services;

namespace SchoolSystem.UI
{
    public partial class StaffAttendanceForm : UserControl
    {
        private readonly TeacherAttendanceService attendanceService = new TeacherAttendanceService();
        private readonly TeacherService teacherService = new TeacherService();

        private int selectedAttendanceId = 0;
        private DataTable allAttendance;

        public StaffAttendanceForm()
        {
            InitializeComponent();
            SchoolSystem.Helpers.UIHelper.ApplyStyle(this);
            ApplyCustomStyles();
            this.Dock = DockStyle.Fill;
            this.Load += StaffAttendanceForm_Load;
        }

        private void ApplyCustomStyles()
        {
            UIHelper.StyleDataGridView(dataGridViewAttendance);
            UIHelper.StylePrimaryButton(btnAdd);
            UIHelper.StylePrimaryButton(btnUpdate);
            UIHelper.StyleDangerButton(btnDelete);
            UIHelper.StyleButton(btnClear, UIHelper.NeutralColor);
            UIHelper.StyleButton(btnRefresh, UIHelper.NeutralColor);
            UIHelper.StyleTextBox(txtSearch);
            UIHelper.StyleTextBox(txtAbsenceReason);
            UIHelper.StyleTextBox(txtLateMinutes);
            UIHelper.StyleTextBox(txtEarlyLeaveMinutes);
            UIHelper.StyleTextBox(txtWorkHours);
            UIHelper.StyleTextBox(txtNotes);
            UIHelper.StyleComboBox(cmbTeacher);
            UIHelper.StyleComboBox(cmbStatus);
            lblRecordCount.ForeColor = UIHelper.MutedTextColor;
        }

        private async void StaffAttendanceForm_Load(object sender, EventArgs e)
        {
            try
            {
                LoadAttendanceStatuses();

                await LoadTeachersAsync();
                await LoadAttendanceAsync();

                ClearInputs();
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("تحميل شاشة حضور الموظفين", ex);
            }
        }

        private void LoadAttendanceStatuses()
        {
            cmbStatus.Items.Clear();
            cmbStatus.Items.Add("حاضر");
            cmbStatus.Items.Add("غائب");
            cmbStatus.Items.Add("متأخر");
            cmbStatus.Items.Add("إجازة");
            cmbStatus.Items.Add("مريض");
            cmbStatus.Items.Add("مأذون");
            cmbStatus.Items.Add("انصراف مبكر");
            cmbStatus.SelectedIndex = 0;
        }

        private async Task LoadTeachersAsync()
        {
            DataTable teachers = await Task.Run(() => teacherService.GetAllTeachers());

            cmbTeacher.DataSource = teachers;
            cmbTeacher.DisplayMember = "TeacherName";
            cmbTeacher.ValueMember = "TeacherID";

            if (cmbTeacher.Items.Count > 0)
                cmbTeacher.SelectedIndex = 0;
        }

        private async Task LoadAttendanceAsync()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                allAttendance = await Task.Run(() => attendanceService.GetAllAttendance());
                ApplyFilter(txtSearch.Text.Trim());

                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                UIHelper.ShowException("تحميل حضور الموظفين", ex);
            }
        }

        private void ApplyFilter(string searchText)
        {
            if (allAttendance == null)
                return;

            DataView dv = allAttendance.DefaultView;

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                string safeText = UIHelper.EscapeDataViewFilterValue(searchText);
                dv.RowFilter =
                    "TeacherName LIKE '%" + safeText + "%' OR " +
                    "Status LIKE '%" + safeText + "%' OR " +
                    "AbsenceReason LIKE '%" + safeText + "%' OR " +
                    "Notes LIKE '%" + safeText + "%'";
            }
            else
            {
                dv.RowFilter = "";
            }

            dataGridViewAttendance.DataSource = dv;
            lblRecordCount.Text = "عدد السجلات: " + dv.Count;

            FormatGridColumns();
        }

        private void FormatGridColumns()
        {
            if (dataGridViewAttendance.Columns.Count == 0)
                return;

            if (dataGridViewAttendance.Columns.Contains("AttendanceID"))
            {
                dataGridViewAttendance.Columns["AttendanceID"].HeaderText = "الرقم";
                dataGridViewAttendance.Columns["AttendanceID"].Width = 60;
            }

            if (dataGridViewAttendance.Columns.Contains("TeacherID"))
                dataGridViewAttendance.Columns["TeacherID"].Visible = false;

            if (dataGridViewAttendance.Columns.Contains("TeacherName"))
                dataGridViewAttendance.Columns["TeacherName"].HeaderText = "اسم المعلم";

            if (dataGridViewAttendance.Columns.Contains("AttendanceDate"))
                dataGridViewAttendance.Columns["AttendanceDate"].HeaderText = "التاريخ";

            if (dataGridViewAttendance.Columns.Contains("Status"))
                dataGridViewAttendance.Columns["Status"].HeaderText = "الحالة";

            if (dataGridViewAttendance.Columns.Contains("CheckInTime"))
                dataGridViewAttendance.Columns["CheckInTime"].HeaderText = "وقت الحضور";

            if (dataGridViewAttendance.Columns.Contains("CheckOutTime"))
                dataGridViewAttendance.Columns["CheckOutTime"].HeaderText = "وقت الانصراف";

            if (dataGridViewAttendance.Columns.Contains("LateMinutes"))
                dataGridViewAttendance.Columns["LateMinutes"].HeaderText = "التأخير";

            if (dataGridViewAttendance.Columns.Contains("EarlyLeaveMinutes"))
                dataGridViewAttendance.Columns["EarlyLeaveMinutes"].HeaderText = "خروج مبكر";

            if (dataGridViewAttendance.Columns.Contains("WorkHours"))
                dataGridViewAttendance.Columns["WorkHours"].HeaderText = "ساعات العمل";

            if (dataGridViewAttendance.Columns.Contains("AbsenceReason"))
                dataGridViewAttendance.Columns["AbsenceReason"].HeaderText = "السبب";

            if (dataGridViewAttendance.Columns.Contains("Notes"))
                dataGridViewAttendance.Columns["Notes"].HeaderText = "ملاحظات";

            if (dataGridViewAttendance.Columns.Contains("RecordedAt"))
                dataGridViewAttendance.Columns["RecordedAt"].HeaderText = "وقت التسجيل";

            if (dataGridViewAttendance.Columns.Contains("UpdatedAt"))
                dataGridViewAttendance.Columns["UpdatedAt"].HeaderText = "آخر تعديل";

            dataGridViewAttendance.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter(txtSearch.Text.Trim());
        }

        private void dataGridViewAttendance_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dataGridViewAttendance.Rows.Count == 0)
                return;

            DataRowView rowView = dataGridViewAttendance.Rows[e.RowIndex].DataBoundItem as DataRowView;

            if (rowView != null)
                FillFieldsFromRow(rowView.Row);
        }

        private void FillFieldsFromRow(DataRow row)
        {
            if (row == null)
                return;

            selectedAttendanceId = row["AttendanceID"] != DBNull.Value && int.TryParse(row["AttendanceID"].ToString(), out int attendanceId)
                ? attendanceId
                : 0;

            if (row["TeacherID"] != DBNull.Value && int.TryParse(row["TeacherID"].ToString(), out int teacherId))
                cmbTeacher.SelectedValue = teacherId;

            if (row["AttendanceDate"] != DBNull.Value && DateTime.TryParse(row["AttendanceDate"].ToString(), out DateTime attendanceDate))
                dtpDate.Value = attendanceDate <= DateTime.Today ? attendanceDate : DateTime.Today;

            if (row["Status"] != DBNull.Value)
                cmbStatus.Text = row["Status"].ToString();

            if (row["CheckInTime"] != DBNull.Value && TryReadTime(row["CheckInTime"], out TimeSpan checkIn))
                dtpCheckIn.Value = DateTime.Today.Add(checkIn);
            else
                dtpCheckIn.Value = DateTime.Today.AddHours(8);

            if (row["CheckOutTime"] != DBNull.Value && TryReadTime(row["CheckOutTime"], out TimeSpan checkOut))
                dtpCheckOut.Value = DateTime.Today.Add(checkOut);
            else
                dtpCheckOut.Value = DateTime.Today.AddHours(14);

            txtLateMinutes.Text =
                row["LateMinutes"] == DBNull.Value
                ? "0"
                : row["LateMinutes"].ToString();

            txtEarlyLeaveMinutes.Text =
                row["EarlyLeaveMinutes"] == DBNull.Value
                ? "0"
                : row["EarlyLeaveMinutes"].ToString();

            txtWorkHours.Text =
                row["WorkHours"] == DBNull.Value
                ? "0"
                : row["WorkHours"].ToString();

            txtAbsenceReason.Text =
                row["AbsenceReason"] == DBNull.Value
                ? ""
                : row["AbsenceReason"].ToString();

            txtNotes.Text =
                row["Notes"] == DBNull.Value
                ? ""
                : row["Notes"].ToString();

            UpdateTimeFieldsState();
            CalculatePreviewValues();
        }

        private void ClearInputs()
        {
            selectedAttendanceId = 0;

            if (cmbTeacher.Items.Count > 0)
                cmbTeacher.SelectedIndex = 0;

            dtpDate.Value = DateTime.Today;

            if (cmbStatus.Items.Count > 0)
                cmbStatus.SelectedIndex = 0;

            dtpCheckIn.Value = DateTime.Today.AddHours(8);
            dtpCheckOut.Value = DateTime.Today.AddHours(14);

            txtLateMinutes.Text = "0";
            txtEarlyLeaveMinutes.Text = "0";
            txtWorkHours.Text = "0";
            txtAbsenceReason.Clear();
            txtNotes.Clear();

            UpdateTimeFieldsState();
        }

        private bool ValidateAttendanceInputs()
        {
            if (cmbTeacher.SelectedValue == null || cmbTeacher.SelectedValue is DataRowView || cmbTeacher.SelectedIndex < 0 ||
                !int.TryParse(cmbTeacher.SelectedValue.ToString(), out int teacherId) || teacherId <= 0)
            {
                UIHelper.ShowWarning("اختر معلماً صالحاً أولاً.");
                cmbTeacher.Focus();
                return false;
            }

            if (cmbStatus.SelectedIndex < 0 || string.IsNullOrWhiteSpace(cmbStatus.Text))
            {
                UIHelper.ShowWarning("اختر حالة الحضور.");
                cmbStatus.Focus();
                return false;
            }

            if (dtpDate.Value.Date > DateTime.Today)
            {
                UIHelper.ShowWarning("لا يمكن تسجيل حضور بتاريخ مستقبلي.");
                dtpDate.Focus();
                return false;
            }

            string status = cmbStatus.Text.Trim();
            bool noTimeRequired = status == "غائب" || status == "إجازة" || status == "مريض";
            if (!noTimeRequired && dtpCheckOut.Value.TimeOfDay < dtpCheckIn.Value.TimeOfDay)
            {
                UIHelper.ShowWarning("وقت الانصراف يجب أن يكون بعد وقت الحضور.");
                dtpCheckOut.Focus();
                return false;
            }

            if ((status == "غائب" || status == "إجازة" || status == "مريض") &&
                string.IsNullOrWhiteSpace(txtAbsenceReason.Text))
            {
                UIHelper.ShowWarning("أدخل سبب الغياب أو الإجازة.");
                txtAbsenceReason.Focus();
                return false;
            }

            string[] allowedStatuses = { "حاضر", "غائب", "متأخر", "إجازة", "مريض", "مأذون", "انصراف مبكر" };
            if (!allowedStatuses.Contains(status))
            {
                UIHelper.ShowWarning("اختر حالة حضور صحيحة.");
                cmbStatus.Focus();
                return false;
            }

            if (txtAbsenceReason.Text.Trim().Length > 500 || txtNotes.Text.Trim().Length > 1000)
            {
                UIHelper.ShowWarning("تجاوز أحد النصوص الحد المسموح به.");
                return false;
            }

            return true;
        }

        private TeacherAttendance GetAttendanceFromInputs()
        {
            TeacherAttendance attendance = new TeacherAttendance();

            attendance.AttendanceID = selectedAttendanceId;
            attendance.TeacherID = int.TryParse(cmbTeacher.SelectedValue == null ? string.Empty : cmbTeacher.SelectedValue.ToString(), out int teacherId)
                ? teacherId
                : 0;
            attendance.AttendanceDate = dtpDate.Value.Date;
            attendance.Status = cmbStatus.SelectedItem == null ? string.Empty : cmbStatus.SelectedItem.ToString();

            bool noTimeRequired =
                attendance.Status == "غائب" ||
                attendance.Status == "إجازة" ||
                attendance.Status == "مريض";

            if (noTimeRequired)
            {
                attendance.CheckInTime = null;
                attendance.CheckOutTime = null;
            }
            else
            {
                attendance.CheckInTime = dtpCheckIn.Value.TimeOfDay;
                attendance.CheckOutTime = dtpCheckOut.Value.TimeOfDay;
            }

            attendance.AbsenceReason = txtAbsenceReason.Text.Trim();
            attendance.Notes = txtNotes.Text.Trim();

            attendanceService.ApplyAttendanceCalculations(attendance);

            txtLateMinutes.Text = attendance.LateMinutes.ToString();
            txtEarlyLeaveMinutes.Text = attendance.EarlyLeaveMinutes.ToString();
            txtWorkHours.Text = attendance.WorkHours.ToString();

            return attendance;
        }

        private bool TryReadTime(object value, out TimeSpan time)
        {
            if (value is TimeSpan span)
            {
                time = span;
                return true;
            }

            return TimeSpan.TryParse(value == null ? string.Empty : value.ToString(), out time);
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateAttendanceInputs())
                return;

            try
            {
                TeacherAttendance attendance = GetAttendanceFromInputs();

                if (attendanceService.AttendanceExists(attendance.TeacherID, attendance.AttendanceDate))
                {
                    UIHelper.ShowWarning("تم تسجيل حضور هذا المعلم مسبقاً في نفس اليوم.");
                    return;
                }

                Cursor = Cursors.WaitCursor;

                await Task.Run(() => attendanceService.AddAttendance(attendance));

                Cursor = Cursors.Default;

                UIHelper.ShowInfo("تم تسجيل الحضور بنجاح.");

                await LoadAttendanceAsync();
                ClearInputs();
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                UIHelper.ShowException("إضافة حضور الموظف", ex);
            }
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedAttendanceId == 0)
            {
                UIHelper.ShowWarning("اختر سجل حضور من الجدول أولاً.");
                return;
            }

            if (!ValidateAttendanceInputs())
                return;

            try
            {
                TeacherAttendance attendance = GetAttendanceFromInputs();

                if (attendanceService.AttendanceExists(attendance.TeacherID, attendance.AttendanceDate, selectedAttendanceId))
                {
                    UIHelper.ShowWarning("يوجد سجل حضور آخر لنفس المعلم في نفس اليوم.");
                    return;
                }

                Cursor = Cursors.WaitCursor;

                bool updated = await Task.Run(() => attendanceService.UpdateAttendance(attendance));

                Cursor = Cursors.Default;

                if (updated)
                    UIHelper.ShowInfo("تم تعديل سجل الحضور بنجاح.");
                else
                    UIHelper.ShowWarning("لم يتم العثور على السجل أو لم يتم تعديله.");

                await LoadAttendanceAsync();
                ClearInputs();
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                UIHelper.ShowException("تعديل حضور الموظف", ex);
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedAttendanceId == 0)
            {
                UIHelper.ShowWarning("اختر سجل حضور من الجدول أولاً.");
                return;
            }

            DialogResult result = MessageBox.Show(
                "هل تريد حذف سجل الحضور هذا؟",
                "تأكيد الحذف",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            try
            {
                Cursor = Cursors.WaitCursor;

                bool deleted = await Task.Run(() => attendanceService.DeleteAttendance(selectedAttendanceId));

                Cursor = Cursors.Default;

                if (deleted)
                    UIHelper.ShowInfo("تم حذف السجل بنجاح.");
                else
                    UIHelper.ShowWarning("لم يتم العثور على السجل أو لم يتم حذفه.");

                await LoadAttendanceAsync();
                ClearInputs();
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                UIHelper.ShowException("حذف حضور الموظف", ex);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearInputs();
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await LoadAttendanceAsync();
            ClearInputs();
        }

        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTimeFieldsState();
            CalculatePreviewValues();
        }

        private void dtpCheckIn_ValueChanged(object sender, EventArgs e)
        {
            CalculatePreviewValues();
        }

        private void dtpCheckOut_ValueChanged(object sender, EventArgs e)
        {
            CalculatePreviewValues();
        }

        private void UpdateTimeFieldsState()
        {
            if (cmbStatus.SelectedItem == null)
                return;

            string status = cmbStatus.SelectedItem.ToString();

            bool noTimeRequired =
                status == "غائب" ||
                status == "إجازة" ||
                status == "مريض";

            dtpCheckIn.Enabled = !noTimeRequired;
            dtpCheckOut.Enabled = !noTimeRequired;

            txtAbsenceReason.Enabled =
                status == "غائب" ||
                status == "إجازة" ||
                status == "مريض" ||
                status == "مأذون" ||
                status == "انصراف مبكر";

            if (noTimeRequired)
            {
                txtLateMinutes.Text = "0";
                txtEarlyLeaveMinutes.Text = "0";
                txtWorkHours.Text = "0";
            }
        }

        private void CalculatePreviewValues()
        {
            if (cmbStatus.SelectedItem == null)
                return;

            try
            {
                TeacherAttendance attendance = new TeacherAttendance();
                attendance.Status = cmbStatus.SelectedItem.ToString();

                bool noTimeRequired =
                    attendance.Status == "غائب" ||
                    attendance.Status == "إجازة" ||
                    attendance.Status == "مريض";

                if (!noTimeRequired)
                {
                    attendance.CheckInTime = dtpCheckIn.Value.TimeOfDay;
                    attendance.CheckOutTime = dtpCheckOut.Value.TimeOfDay;
                }

                attendanceService.ApplyAttendanceCalculations(attendance);

                txtLateMinutes.Text = attendance.LateMinutes.ToString();
                txtEarlyLeaveMinutes.Text = attendance.EarlyLeaveMinutes.ToString();
                txtWorkHours.Text = attendance.WorkHours.ToString();
            }
            catch
            {
                txtLateMinutes.Text = "0";
                txtEarlyLeaveMinutes.Text = "0";
                txtWorkHours.Text = "0";
            }
        }

        private void txtAbsenceReason_KeyPress(object sender, KeyPressEventArgs e)
        {
            ValidateTextOnlyKeyPress(e);
        }

        private void txtNotes_KeyPress(object sender, KeyPressEventArgs e)
        {
            ValidateTextOnlyKeyPress(e);
        }

        private void ValidateTextOnlyKeyPress(KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
                return;

            if (char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                return;
            }

            string allowed = "ءآأؤإئابةتثجحخدذرزسشصضطظعغفقكلمنهويىة لاabcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ-_.،, ";
            if (!allowed.Contains(e.KeyChar.ToString()))
                e.Handled = true;
        }
    }
}
