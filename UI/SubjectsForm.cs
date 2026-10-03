using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using SchoolSystem.Models;
using SchoolSystem.Helpers;
using SchoolSystem.Services;
using SchoolSystem.Security;


namespace SchoolSystem.UI
{

    public partial class SubjectsForm : UserControl
    {
        // تعريف الدالة SubjectService لتنفيذ العملية المرتبطة بها.
        private readonly SubjectService subjectService = new SubjectService();
        // تعريف الدالة ClassService لتنفيذ العملية المرتبطة بها.
        private readonly ClassService classService = new ClassService();


        private int selectedSubjectId = 0;

        private DataTable subjectsTable;

        // تعريف الدالة SubjectsForm لتنفيذ العملية المرتبطة بها.
        public SubjectsForm()
        {

            InitializeComponent();

            SchoolSystem.Helpers.UIHelper.ApplyStyle(this);
            ApplyCustomStyles();

            Dock = DockStyle.Fill;

            Load += SubjectsForm_Load;

            txtSearch.TextChanged += txtSearch_TextChanged;

            cmbClass.SelectedIndexChanged += cmbClass_SelectedIndexChanged;
        }

        // تعريف الدالة ApplyCustomStyles لتنفيذ العملية المرتبطة بها.
        private void ApplyCustomStyles()
        {
            UIHelper.StyleDataGridView(dataGridViewSubjects);
            UIHelper.StylePrimaryButton(btnAdd);
            UIHelper.StylePrimaryButton(btnUpdate);
            UIHelper.StyleDangerButton(btnDelete);
            UIHelper.StyleButton(btnClear, UIHelper.NeutralColor);
            UIHelper.StyleButton(btnRefresh, UIHelper.NeutralColor);
            UIHelper.StyleTextBox(txtSubjectID);
            UIHelper.StyleTextBox(txtSubjectCode);
            UIHelper.StyleTextBox(txtSubjectName);
            UIHelper.StyleTextBox(txtNotes);
            UIHelper.StyleTextBox(txtSearch);
            UIHelper.StyleComboBox(cmbClass);
            lblRecordCount.ForeColor = UIHelper.MutedTextColor;
        }

        // تعريف الدالة SubjectsForm_Load لتنفيذ العملية المرتبطة بها.
        private async void SubjectsForm_Load(object sender, EventArgs e)
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                Cursor = Cursors.WaitCursor;

                ConfigureSubjectEditingState();


                await LoadClassesAsync();

                await LoadSubjectsAsync();

                ClearFields();

                Cursor = Cursors.Default;
            }

            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                UIHelper.ShowException("تحميل واجهة المواد", ex);
            }
        }

        // تعريف الدالة ConfigureSubjectEditingState لتنفيذ العملية المرتبطة بها.
        private void ConfigureSubjectEditingState()
        {
            txtSubjectID.ReadOnly = true;
            txtSubjectCode.ReadOnly = false;
            txtSubjectName.ReadOnly = false;
            cmbClass.Enabled = true;

            txtSubjectID.BackColor = UIHelper.DisabledSurfaceColor;
            txtSubjectCode.BackColor = System.Drawing.Color.White;
            txtSubjectName.BackColor = System.Drawing.Color.White;
            txtSubjectID.ForeColor = UIHelper.MutedTextColor;
            txtSubjectCode.ForeColor = System.Drawing.Color.Black;
            txtSubjectName.ForeColor = System.Drawing.Color.Black;

            btnAdd.Text = selectedSubjectId > 0 ? "إضافة جديدة" : "إضافة مادة";
            btnDelete.Enabled = selectedSubjectId > 0 && CurrentUser.CanDelete("Subjects");
            btnDelete.Text = "حذف المادة";
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

                dt.Rows.Add(0, "كل الصفوف");

                foreach (DataRow row in classes.Rows)
                {
                    dt.Rows.Add(
                        Convert.ToInt32(row["ClassID"]),
                        row["ClassName"].ToString()
                    );
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
                UIHelper.ShowException("تحميل صفوف المواد", ex);
            }
        }

        // تعريف الدالة LoadSubjectsAsync لتنفيذ العملية المرتبطة بها.
        private async Task LoadSubjectsAsync()
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {

                subjectsTable = await Task.Run(() => subjectService.GetAllSubjects());

                dataGridViewSubjects.DataSource = subjectsTable;

                FormatGrid();
                UpdateRecordCount();
            }

            catch (Exception ex)
            {
                UIHelper.ShowException("تحميل المواد", ex);
            }
        }

        // تعريف الدالة FormatGrid لتنفيذ العملية المرتبطة بها.
        private void FormatGrid()
        {
            if (dataGridViewSubjects.Columns.Count == 0)
                return;

            SetHeader("SubjectID", "الرقم");
            SetHeader("SubjectCode", "كود المادة");
            SetHeader("SubjectName", "اسم المادة");
            SetHeader("ClassName", "الصف");
            SetHeader("MaxDegree", "الدرجة الكبرى");
            SetHeader("PassDegree", "درجة النجاح");
            SetHeader("IsActive", "نشطة");
            SetHeader("Notes", "ملاحظات");
            SetHeader("CreatedAt", "تاريخ الإضافة");
            SetHeader("UpdatedAt", "آخر تعديل");

            HideColumn("ClassID");

            dataGridViewSubjects.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // تعريف الدالة SetHeader لتنفيذ العملية المرتبطة بها.
        private void SetHeader(string columnName, string header)
        {
            if (dataGridViewSubjects.Columns.Contains(columnName))
                dataGridViewSubjects.Columns[columnName].HeaderText = header;
        }

        // تعريف الدالة HideColumn لتنفيذ العملية المرتبطة بها.
        private void HideColumn(string columnName)
        {
            if (dataGridViewSubjects.Columns.Contains(columnName))
                dataGridViewSubjects.Columns[columnName].Visible = false;
        }

        // تعريف الدالة UpdateRecordCount لتنفيذ العملية المرتبطة بها.
        private void UpdateRecordCount()
        {
            int count = 0;

            if (subjectsTable != null)
                count = subjectsTable.DefaultView.Count;

            lblRecordCount.Text = "عدد المواد: " + count;
        }

        // تعريف الدالة BuildSubjectModel لتنفيذ العملية المرتبطة بها.
        private Subject BuildSubjectModel()
        {
            Subject subject = new Subject();

            subject.SubjectID = selectedSubjectId;
            subject.SubjectCode = txtSubjectCode.Text.Trim();
            subject.SubjectName = txtSubjectName.Text.Trim();

            int classId = 0;

            if (cmbClass.SelectedValue != null)
                int.TryParse(cmbClass.SelectedValue.ToString(), out classId);

            subject.ClassID = classId > 0 ? (int?)classId : null;
            subject.MaxDegree = nudMaxDegree.Value;
            subject.PassDegree = nudPassDegree.Value;
            subject.IsActive = chkIsActive.Checked;
            subject.Notes = txtNotes.Text.Trim();


            return subject;
        }

        // تعريف الدالة btnAdd_Click لتنفيذ العملية المرتبطة بها.
        private async void btnAdd_Click(object sender, EventArgs e)
        {
            CurrentUser.DemandAction("Subjects", "Add", "ليس لديك صلاحية إضافة المواد.");
            if (!ValidateSubjectInputs())
                return;

            try
            {
                Subject subject = BuildSubjectModel();
                subject.SubjectID = 0;
                int subjectId = await Task.Run(() => subjectService.AddSubject(subject));
                ShowInfo("تمت إضافة المادة بنجاح. الرقم: " + subjectId);
                await LoadSubjectsAsync();
                ClearFields();
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("إضافة المادة", ex);
            }
        }

        // تعريف الدالة ValidateSubjectInputs لتنفيذ العملية المرتبطة بها.
        private bool ValidateSubjectInputs()
        {
            if (string.IsNullOrWhiteSpace(txtSubjectCode.Text) || txtSubjectCode.Text.Trim().Length < 2 || txtSubjectCode.Text.Trim().Length > 30)
            {
                ShowWarning("كود المادة يجب أن يكون بين حرفين و30 حرفاً.");
                txtSubjectCode.Focus();
                return false;
            }

            foreach (char character in txtSubjectCode.Text.Trim())
            {
                if (!(char.IsLetterOrDigit(character) || character == '_' || character == '-'))
                {
                    ShowWarning("كود المادة يجب أن يحتوي على حروف أو أرقام أو (_) أو (-) فقط.");
                    txtSubjectCode.Focus();
                    return false;
                }
            }

            if (string.IsNullOrWhiteSpace(txtSubjectName.Text) || txtSubjectName.Text.Trim().Length < 2 || txtSubjectName.Text.Trim().Length > 100)
            {
                ShowWarning("اسم المادة يجب أن يكون بين حرفين و100 حرف.");
                txtSubjectName.Focus();
                return false;
            }

            return ValidateSubjectSettings();
        }

        // تعريف الدالة ValidateSubjectSettings لتنفيذ العملية المرتبطة بها.
        private bool ValidateSubjectSettings()
        {
            if (nudMaxDegree.Value <= 0)
            {
                ShowWarning("الدرجة الكبرى يجب أن تكون أكبر من صفر.");
                nudMaxDegree.Focus();

                return false;
            }
            if (nudPassDegree.Value < 0 || nudPassDegree.Value > nudMaxDegree.Value)
            {
                ShowWarning("درجة النجاح يجب أن تكون بين صفر والدرجة الكبرى.");
                nudPassDegree.Focus();

                return false;
            }
            if (txtNotes.Text.Trim().Length > 1000)
            {
                ShowWarning("الملاحظات لا يمكن أن تتجاوز 1000 حرف.");
                txtNotes.Focus();

                return false;
            }

            return true;
        }

        // تعريف الدالة btnUpdate_Click لتنفيذ العملية المرتبطة بها.
        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.DemandAction("Subjects", "Edit", "ليس لديك صلاحية تعديل إعدادات المواد.");
            if (selectedSubjectId <= 0)
            {
                ShowWarning("اختر مادة من الجدول أولاً.");

                return;
            }

            if (!ValidateSubjectSettings())
                return;

            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                Subject subject = BuildSubjectModel();
                subject.SubjectID = selectedSubjectId;


                bool updated = await Task.Run(() => subjectService.UpdateSubject(subject));

                if (updated)
                {
                    ShowInfo("تم تعديل إعدادات المادة بنجاح.");
                    ApplicationLogger.LogUsage("شاشة المواد", "تعديل المادة", "تم تعديل إعدادات المادة بنجاح");


                    await LoadSubjectsAsync();
                    ClearFields();
                }

                else
                {
                    ShowWarning("لم يتم العثور على المادة أو لم يتم تعديلها.");
                }
            }

            catch (Exception ex)
            {
                UIHelper.ShowException("تعديل المادة", ex);
            }
        }

        // تعريف الدالة btnDelete_Click لتنفيذ العملية المرتبطة بها.
        private async void btnDelete_Click(object sender, EventArgs e)
        {
            CurrentUser.DemandAction("Subjects", "Delete", "ليس لديك صلاحية حذف المواد.");
            if (selectedSubjectId <= 0)
            {
                ShowWarning("اختر مادة من الجدول أولاً.");
                return;
            }

            DialogResult confirmation = MessageBox.Show(
                "هل تريد حذف المادة المحددة؟\\n\\nإذا كانت المادة مرتبطة بدرجات أو جدول أو مقرر دراسي فسيمنع النظام الحذف حفاظاً على البيانات.",
                "تأكيد حذف المادة",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2,
                MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);

            if (confirmation != DialogResult.Yes)
                return;

            try
            {
                bool deleted = await Task.Run(() => subjectService.DeleteSubject(selectedSubjectId));
                if (deleted)
                {
                    ShowInfo("تم حذف المادة بنجاح.");
                    await LoadSubjectsAsync();
                    ClearFields();
                }
                else
                {
                    ShowWarning("لم يتم العثور على المادة أو لم يتم حذفها.");
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("حذف المادة", ex);
            }
        }

        // تعريف الدالة btnClear_Click لتنفيذ العملية المرتبطة بها.
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        // تعريف الدالة btnRefresh_Click لتنفيذ العملية المرتبطة بها.
        private async void btnRefresh_Click(object sender, EventArgs e)
        {

            await LoadSubjectsAsync();
            ClearFields();
        }

        // تعريف الدالة dataGridViewSubjects_CellClick لتنفيذ العملية المرتبطة بها.
        private void dataGridViewSubjects_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dataGridViewSubjects.Rows.Count == 0)
                return;

            DataRowView rowView = dataGridViewSubjects.Rows[e.RowIndex].DataBoundItem as DataRowView;

            if (rowView == null)
                return;

            FillFields(rowView.Row);
        }

        // تعريف الدالة FillFields لتنفيذ العملية المرتبطة بها.
        private void FillFields(DataRow row)
        {
            if (row == null)
                return;

            selectedSubjectId = ReadRowInt(row, "SubjectID");

            txtSubjectID.Text = selectedSubjectId > 0 ? selectedSubjectId.ToString() : "";

            txtSubjectCode.Text = ReadRowText(row, "SubjectCode");
            txtSubjectName.Text = ReadRowText(row, "SubjectName");

            int classId = ReadRowInt(row, "ClassID");
            if (classId > 0 && cmbClass.DataSource != null)
            {
                            cmbClass.Enabled = true;
            cmbClass.SelectedValue = classId;

            }

            else
            {
                if (cmbClass.Items.Count > 0)
                    cmbClass.SelectedIndex = 0;
            }

            nudMaxDegree.Value = ClampNumeric(ReadRowDecimal(row, "MaxDegree", 100), nudMaxDegree.Minimum, nudMaxDegree.Maximum);
            nudPassDegree.Value = ClampNumeric(ReadRowDecimal(row, "PassDegree", 50), nudPassDegree.Minimum, nudPassDegree.Maximum);
            chkIsActive.Checked = ReadRowBool(row, "IsActive", true);
            txtNotes.Text = ReadRowText(row, "Notes");

            ConfigureSubjectEditingState();
        }

        // تعريف الدالة ReadRowText لتنفيذ العملية المرتبطة بها.
        private string ReadRowText(DataRow row, string columnName)
        {
            if (row == null || row.Table == null || !row.Table.Columns.Contains(columnName))
                return string.Empty;

            object value = row[columnName];

            return value == null || value == DBNull.Value ? string.Empty : value.ToString();
        }

        // تعريف الدالة ReadRowInt لتنفيذ العملية المرتبطة بها.
        private int ReadRowInt(DataRow row, string columnName)
        {
            int value;

            return int.TryParse(ReadRowText(row, columnName), out value) ? value : 0;
        }

        // تعريف الدالة ReadRowDecimal لتنفيذ العملية المرتبطة بها.
        private decimal ReadRowDecimal(DataRow row, string columnName, decimal fallback)
        {
            decimal value;

            return decimal.TryParse(ReadRowText(row, columnName), out value) ? value : fallback;
        }

        // تعريف الدالة ReadRowBool لتنفيذ العملية المرتبطة بها.
        private bool ReadRowBool(DataRow row, string columnName, bool fallback)
        {
            string value = ReadRowText(row, columnName);
            if (string.IsNullOrWhiteSpace(value))
                return fallback;

            bool parsed;

            return bool.TryParse(value, out parsed) ? parsed : fallback;
        }

        // تعريف الدالة ClampNumeric لتنفيذ العملية المرتبطة بها.
        private decimal ClampNumeric(decimal value, decimal minimum, decimal maximum)
        {

            return Math.Min(Math.Max(value, minimum), maximum);
        }

        // تعريف الدالة ClearFields لتنفيذ العملية المرتبطة بها.
        private void ClearFields()
        {
            selectedSubjectId = 0;

            txtSubjectID.Clear();
            txtSubjectCode.Clear();
            txtSubjectName.Clear();

            cmbClass.Enabled = true;

            if (cmbClass.Items.Count > 0)
                cmbClass.SelectedIndex = 0;

            nudMaxDegree.Value = 100;
            nudPassDegree.Value = 50;
            chkIsActive.Checked = true;
            txtNotes.Clear();

            ConfigureSubjectEditingState();

            txtNotes.Focus();
        }

        // تعريف الدالة cmbClass_SelectedIndexChanged لتنفيذ العملية المرتبطة بها.
        private void cmbClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplySubjectsFilter();
        }

        // تعريف الدالة txtSearch_TextChanged لتنفيذ العملية المرتبطة بها.
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplySubjectsFilter();
        }

        // تعريف الدالة ApplySubjectsFilter لتنفيذ العملية المرتبطة بها.
        private void ApplySubjectsFilter()
        {
            if (subjectsTable == null)
                return;

            string keyword = txtSearch.Text.Trim();

            if (string.IsNullOrWhiteSpace(keyword))
            {
                subjectsTable.DefaultView.RowFilter = "";
            }

            else
            {
                string safe = UIHelper.EscapeDataViewFilterValue(keyword);

                string filter =
                    "SubjectCode LIKE '%" + safe + "%' OR " +
                    "SubjectName LIKE '%" + safe + "%'";

                if (subjectsTable.Columns.Contains("ClassName"))
                    filter += " OR ClassName LIKE '%" + safe + "%'";

                subjectsTable.DefaultView.RowFilter = filter;
            }

            string classFilter = string.Empty;
            int classId;
            if (cmbClass.SelectedValue != null && int.TryParse(cmbClass.SelectedValue.ToString(), out classId) && classId > 0)
                classFilter = "ClassID = " + classId.ToString();

            if (!string.IsNullOrWhiteSpace(classFilter))
                subjectsTable.DefaultView.RowFilter = string.IsNullOrWhiteSpace(subjectsTable.DefaultView.RowFilter)
                    ? classFilter
                    : "(" + subjectsTable.DefaultView.RowFilter + ") AND " + classFilter;

            dataGridViewSubjects.DataSource = subjectsTable.DefaultView;
            UpdateRecordCount();
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
