using System;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using SchoolSystem.Models;
using SchoolSystem.Helpers;
using SchoolSystem.Services;


namespace SchoolSystem.UI
{

    public partial class DailyAttendanceForm : UserControl
    {
        // تعريف الدالة StudentAttendanceService لتنفيذ العملية المرتبطة بها.
        private readonly StudentAttendanceService attendanceService = new StudentAttendanceService();
        // تعريف الدالة ClassService لتنفيذ العملية المرتبطة بها.
        private readonly ClassService classService = new ClassService();


        private DataTable currentAttendanceTable;

        // تعريف الدالة DailyAttendanceForm لتنفيذ العملية المرتبطة بها.
        public DailyAttendanceForm()
        {

            InitializeComponent();

            SchoolSystem.Helpers.UIHelper.ApplyStyle(this);

            Dock = DockStyle.Fill;
            // وضع التحديد المتعدد متطلب وظيفي لتعليم مجموعة طلاب؛ يُضبط بعد ApplyStyle
            // لأن الستايل الموحد يفرض تحديد صف واحد لبقية الشاشات.
            dataGridViewAttendance.MultiSelect = true;
            dataGridViewAttendance.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            Load += DailyAttendanceForm_Load;

            txtSearch.TextChanged += txtSearch_TextChanged;

            cmbClass.SelectedIndexChanged += cmbClass_SelectedIndexChanged;

            txtAcademicYear.Leave += txtAcademicYear_Leave;
        }

        // تعريف الدالة DailyAttendanceForm_Load لتنفيذ العملية المرتبطة بها.
        private async void DailyAttendanceForm_Load(object sender, EventArgs e)
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
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
                UIHelper.ShowException("حدث خطأ أثناء تحميل واجهة التحضير:\n", ex);
            }
        }

        // تعريف الدالة LoadStaticData لتنفيذ العملية المرتبطة بها.
        private void LoadStaticData()
        {
            dtpDate.Value = DateTime.Today;


            string dbYear = SchoolSystem.Helpers.AcademicYearHelper.GetCurrentAcademicYear();
            if (!string.IsNullOrWhiteSpace(dbYear))
            {
                txtAcademicYear.Text = dbYear;
            }

            else
            {
                int year = DateTime.Now.Year;
                txtAcademicYear.Text = year + "/" + (year + 1);
            }

            cmbSection.DataSource = null;
            cmbSection.Items.Clear();
            cmbSection.Enabled = false;

            lblSummary.Text = "ملخص الحضور: لا توجد بيانات محملة.";
            lblRecordCount.Text = "عدد الطلاب: 0";
        }

        // تعريف الدالة LoadClassesAsync لتنفيذ العملية المرتبطة بها.
        private async Task LoadClassesAsync()
        {

            DataTable classes = await Task.Run(() => classService.GetAllClasses());

            cmbClass.DataSource = classes;
            cmbClass.DisplayMember = "ClassName";
            cmbClass.ValueMember = "ClassID";

            if (cmbClass.Items.Count > 0)
            {
                cmbClass.SelectedIndex = 0;

                await LoadSectionsAsync();
            }
        }

        // تعريف الدالة cmbClass_SelectedIndexChanged لتنفيذ العملية المرتبطة بها.
        private async void cmbClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!IsHandleCreated || cmbClass.SelectedValue is DataRowView)
                return;


            await LoadSectionsAsync();
        }

        // تعريف الدالة txtAcademicYear_Leave لتنفيذ العملية المرتبطة بها.
        private async void txtAcademicYear_Leave(object sender, EventArgs e)
        {

            await LoadSectionsAsync();
        }

        // تعريف الدالة LoadSectionsAsync لتنفيذ العملية المرتبطة بها.
        private async Task LoadSectionsAsync()
        {
            int classId = GetClassId();
            string academicYear = txtAcademicYear.Text == null ? string.Empty : txtAcademicYear.Text.Trim();

            cmbSection.DataSource = null;
            cmbSection.Items.Clear();
            cmbSection.Enabled = false;

            if (classId <= 0 || string.IsNullOrWhiteSpace(academicYear))
                return;

            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {

                DataTable sections = await Task.Run(() => attendanceService.GetSections(classId, academicYear));
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

                cmbSection.DataSource = choices;
                cmbSection.DisplayMember = "Section";
                cmbSection.ValueMember = "Section";
                cmbSection.Enabled = choices.Rows.Count > 0;

                if (choices.Rows.Count > 0)
                    cmbSection.SelectedIndex = 0;
            }

            catch (Exception ex)
            {
                UIHelper.ShowException("تعذر تحميل الشعب الفعلية للصف والعام الدراسي المحددين:\n", ex);
            }
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

        // تعريف الدالة btnLoad_Click لتنفيذ العملية المرتبطة بها.
        private async void btnLoad_Click(object sender, EventArgs e)
        {

            await LoadAttendanceAsync();
        }

        // تعريف الدالة btnRefresh_Click لتنفيذ العملية المرتبطة بها.
        private async void btnRefresh_Click(object sender, EventArgs e)
        {

            await LoadAttendanceAsync();
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

        // تعريف الدالة ValidateAttendanceFilters لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة LoadAttendanceAsync لتنفيذ العملية المرتبطة بها.
        private async Task LoadAttendanceAsync()
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                if (!ValidateAttendanceFilters())
                    return;

                int classId = GetClassId();
                // قراءة قيم الواجهة قبل الانتقال لمؤشر العمل؛ لا يُقرأ أي Control داخل Task.Run.
                string section = cmbSection.Text;
                string academicYear = txtAcademicYear.Text.Trim();
                DateTime attendanceDate = dtpDate.Value.Date;

                Cursor = Cursors.WaitCursor;


                currentAttendanceTable = await Task.Run(() =>
                    attendanceService.GetAttendanceSheet(
                        classId,
                        section,
                        academicYear,
                        attendanceDate));

                dataGridViewAttendance.DataSource = currentAttendanceTable;

                FormatGrid();
                BuildSummary();

                lblRecordCount.Text = "عدد الطلاب: " + currentAttendanceTable.Rows.Count;

                Cursor = Cursors.Default;
            }

            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                UIHelper.ShowException("خطأ أثناء تحميل التحضير:\n", ex);
            }
        }

        // تعريف الدالة FormatGrid لتنفيذ العملية المرتبطة بها.
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
            SetHeader("DepartureTime", "وقت الانصراف");
            SetHeader("AbsenceReason", "سبب الغياب");
            SetHeader("Notes", "ملاحظات");

            MakeReadOnly("StudentNumber");
            MakeReadOnly("StudentName");
            MakeReadOnly("Gender");
            MakeReadOnly("StudentID");

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

        // تعريف الدالة SetHeader لتنفيذ العملية المرتبطة بها.
        private void SetHeader(string columnName, string header)
        {
            if (dataGridViewAttendance.Columns.Contains(columnName))
                dataGridViewAttendance.Columns[columnName].HeaderText = header;
        }

        // تعريف الدالة HideColumn لتنفيذ العملية المرتبطة بها.
        private void HideColumn(string columnName)
        {
            if (dataGridViewAttendance.Columns.Contains(columnName))
                dataGridViewAttendance.Columns[columnName].Visible = false;
        }

        // تعريف الدالة MakeReadOnly لتنفيذ العملية المرتبطة بها.
        private void MakeReadOnly(string columnName)
        {
            if (dataGridViewAttendance.Columns.Contains(columnName))
                dataGridViewAttendance.Columns[columnName].ReadOnly = true;
        }

        // تعريف الدالة btnSave_Click لتنفيذ العملية المرتبطة بها.
        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (dataGridViewAttendance.Rows.Count == 0)
            {
                ShowWarning("لا توجد بيانات للحفظ.");

                return;
            }

            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
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

                        string excuseStatus = Convert.ToString(validationRow.Cells["ExcuseStatus"].Value).Trim();
                        if (excuseStatus != "بدون عذر" && excuseStatus != "بعذر" && excuseStatus != "غير محدد")
                        {
                            ShowWarning("توجد حالة عذر غير صالحة. اختر حالة العذر لكل طالب.");

                            return;
                        }

                        string notes = Convert.ToString(validationRow.Cells["Notes"].Value);
                        if (notes.Length > 500)
                        {
                            ShowWarning("تجاوزت ملاحظات أحد الطلاب الحد المسموح به.");

                            return;
                        }

                        string absenceReason = Convert.ToString(validationRow.Cells["AbsenceReason"].Value);
                        if (absenceReason.Length > 500)
                        {
                            ShowWarning("تجاوز سبب الغياب لأحد الطلاب الحد المسموح به.");

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

                    if (!TryGetRowInt(row, "StudentID", out int studentId) || studentId <= 0)
                    {
                        ShowWarning("تعذر تحديد طالب صحيح في قائمة الحضور؛ أعد تحميل البيانات ثم حاول مرة أخرى.");

                        return;
                    }

                    StudentAttendance item = new StudentAttendance();

                    item.StudentID = studentId;
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
                    string departure = row.Cells["DepartureTime"].Value == null
                        ? ""
                        : row.Cells["DepartureTime"].Value.ToString();

                    TimeSpan arrivalTime;
                    TimeSpan departureTime;

                    item.ArrivalTime = TimeSpan.TryParse(arrival, out arrivalTime) ? arrivalTime : (TimeSpan?)null;
                    item.DepartureTime = TimeSpan.TryParse(departure, out departureTime) ? departureTime : (TimeSpan?)null;
                    item.AbsenceReason = row.Cells["AbsenceReason"].Value == null
                        ? ""
                        : row.Cells["AbsenceReason"].Value.ToString().Trim();


                    bool saved = await Task.Run(() => attendanceService.SaveAttendance(item));

                    if (saved)
                        savedCount++;
                }

                Cursor = Cursors.Default;

                ShowInfo("تم حفظ حضور " + savedCount + " طالب بنجاح.");
                ApplicationLogger.LogUsage("شاشة الحضور اليومي", "حفظ الحضور", "تم حفظ حضور " + savedCount + " طالب بنجاح");


                await LoadAttendanceAsync();
            }

            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                UIHelper.ShowException("خطأ أثناء حفظ الحضور:\n", ex);
            }
        }

        // تعريف الدالة TryGetRowInt لتنفيذ العملية المرتبطة بها.
        private bool TryGetRowInt(DataGridViewRow row, string columnName, out int value)
        {
            value = 0;
            if (row == null || !dataGridViewAttendance.Columns.Contains(columnName))
                return false;

            object cellValue = row.Cells[columnName].Value;

            return cellValue != null && cellValue != DBNull.Value && int.TryParse(cellValue.ToString(), out value);
        }

        // تعريف الدالة btnMarkAllAbsent_Click لتنفيذ العملية المرتبطة بها.
        private void btnMarkAllAbsent_Click(object sender, EventArgs e)
        {
            ApplyBulkStatus("غائب", "غير محدد");
        }

        // تعريف الدالة btnMarkAllLate_Click لتنفيذ العملية المرتبطة بها.
        private void btnMarkAllLate_Click(object sender, EventArgs e)
        {
            ApplyBulkStatus("متأخر", "بدون عذر");
        }

        // تعريف الدالة ApplyBulkStatus لتنفيذ العملية المرتبطة بها.
        private void ApplyBulkStatus(string status, string excuseStatus)
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

                row.Cells["Status"].Value = status;
                row.Cells["ExcuseStatus"].Value = excuseStatus;
            }

            BuildSummary();
        }

        // تعريف الدالة btnMarkAllPresent_Click لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة btnClear_Click لتنفيذ العملية المرتبطة بها.
        private void btnClear_Click(object sender, EventArgs e)
        {
            currentAttendanceTable = null;
            dataGridViewAttendance.DataSource = null;
            lblSummary.Text = "ملخص الحضور: لا توجد بيانات محملة.";
            lblRecordCount.Text = "عدد الطلاب: 0";
        }

        // تعريف الدالة txtSearch_TextChanged لتنفيذ العملية المرتبطة بها.
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
                currentAttendanceTable.DefaultView.RowFilter = UIHelper.BuildDataViewSearchFilter(
                    txtSearch.Text, "StudentName", "StudentNumber");
            }

            lblRecordCount.Text = "عدد الطلاب: " + currentAttendanceTable.DefaultView.Count;
        }

        // تعريف الدالة dataGridViewAttendance_CurrentCellDirtyStateChanged لتنفيذ العملية المرتبطة بها.
        private void dataGridViewAttendance_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridViewAttendance.IsCurrentCellDirty)
                dataGridViewAttendance.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        // تعريف الدالة dataGridViewAttendance_CellValueChanged لتنفيذ العملية المرتبطة بها.
        private void dataGridViewAttendance_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            BuildSummary();
        }

        // تعريف الدالة BuildSummary لتنفيذ العملية المرتبطة بها.
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
