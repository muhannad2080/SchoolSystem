using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using SchoolSystem.Models;
using SchoolSystem.Helpers;
using SchoolSystem.Services;

namespace SchoolSystem.UI
{
    public partial class SubjectsForm : UserControl
    {
        private readonly SubjectService subjectService = new SubjectService();
        private readonly ClassService classService = new ClassService();

        private int selectedSubjectId = 0;
        private DataTable subjectsTable;

        public SubjectsForm()
        {
            InitializeComponent();

            Dock = DockStyle.Fill;

            Load += SubjectsForm_Load;
            txtSearch.TextChanged += txtSearch_TextChanged;
        }

        private async void SubjectsForm_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                ConfigureFixedSubjectsMode();

                await LoadClassesAsync();
                await LoadSubjectsAsync();

                ClearFields();

                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                ShowError("حدث خطأ أثناء تحميل واجهة المواد:\n" + ex.Message);
            }
        }

        private void ConfigureFixedSubjectsMode()
        {
            txtSubjectID.ReadOnly = true;
            txtSubjectCode.ReadOnly = true;
            txtSubjectName.ReadOnly = true;
            cmbClass.Enabled = false;

            txtSubjectCode.BackColor = System.Drawing.Color.FromArgb(243, 244, 246);
            txtSubjectName.BackColor = System.Drawing.Color.FromArgb(243, 244, 246);

            btnAdd.Text = "تثبيت المواد";
            btnDelete.Enabled = false;
            btnDelete.Text = "حذف معطل";
        }

        private async Task LoadClassesAsync()
        {
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
            catch
            {
                DataTable fallback = new DataTable();
                fallback.Columns.Add("ClassID", typeof(int));
                fallback.Columns.Add("ClassName", typeof(string));
                fallback.Rows.Add(0, "كل الصفوف");

                cmbClass.DataSource = fallback;
                cmbClass.DisplayMember = "ClassName";
                cmbClass.ValueMember = "ClassID";
            }
        }

        private async Task LoadSubjectsAsync()
        {
            try
            {
                subjectsTable = await Task.Run(() => subjectService.GetAllSubjects());

                dataGridViewSubjects.DataSource = subjectsTable;

                FormatGrid();
                UpdateRecordCount();
            }
            catch (Exception ex)
            {
                ShowError("خطأ أثناء تحميل المواد:\n" + ex.Message);
            }
        }

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

        private void SetHeader(string columnName, string header)
        {
            if (dataGridViewSubjects.Columns.Contains(columnName))
                dataGridViewSubjects.Columns[columnName].HeaderText = header;
        }

        private void HideColumn(string columnName)
        {
            if (dataGridViewSubjects.Columns.Contains(columnName))
                dataGridViewSubjects.Columns[columnName].Visible = false;
        }

        private void UpdateRecordCount()
        {
            int count = 0;

            if (subjectsTable != null)
                count = subjectsTable.DefaultView.Count;

            lblRecordCount.Text = "عدد المواد: " + count;
        }

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

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            ShowInfo(
                "المواد الأساسية ثابتة في قاعدة البيانات.\n\n" +
                "إذا كانت المواد غير ظاهرة، نفذ سكربت تثبيت المواد الافتراضية في SQL Server.\n" +
                "ثم اضغط تحديث."
            );

            await LoadSubjectsAsync();
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedSubjectId <= 0)
            {
                ShowWarning("اختر مادة من الجدول أولاً.");
                return;
            }

            try
            {
                Subject subject = BuildSubjectModel();
                subject.SubjectID = selectedSubjectId;

                bool updated = await Task.Run(() => subjectService.UpdateSubject(subject));

                if (updated)
                {
                    ShowInfo("تم تعديل إعدادات المادة بنجاح.");

                    await LoadSubjectsAsync();
                    ClearFields();
                }
                else
                {
                    ShowWarning("لم يتم العثور على المادة.");
                }
            }
            catch (Exception ex)
            {
                ShowError("خطأ أثناء التعديل:\n" + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            ShowWarning("حذف المواد الأساسية غير مسموح؛ لأنها مرتبطة بالدرجات والمنهج الدراسي.");
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await LoadSubjectsAsync();
            ClearFields();
        }

        private void dataGridViewSubjects_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dataGridViewSubjects.Rows.Count == 0)
                return;

            DataRowView rowView = dataGridViewSubjects.Rows[e.RowIndex].DataBoundItem as DataRowView;

            if (rowView == null)
                return;

            FillFields(rowView.Row);
        }

        private void FillFields(DataRow row)
        {
            selectedSubjectId = row["SubjectID"] != DBNull.Value
                ? Convert.ToInt32(row["SubjectID"])
                : 0;

            txtSubjectID.Text = selectedSubjectId > 0 ? selectedSubjectId.ToString() : "";

            txtSubjectCode.Text = row["SubjectCode"] == DBNull.Value
                ? ""
                : row["SubjectCode"].ToString();

            txtSubjectName.Text = row["SubjectName"] == DBNull.Value
                ? ""
                : row["SubjectName"].ToString();

            if (row.Table.Columns.Contains("ClassID") &&
                row["ClassID"] != DBNull.Value)
            {
                cmbClass.Enabled = true;
                cmbClass.SelectedValue = Convert.ToInt32(row["ClassID"]);
                cmbClass.Enabled = false;
            }
            else
            {
                if (cmbClass.Items.Count > 0)
                    cmbClass.SelectedIndex = 0;
            }

            nudMaxDegree.Value = row["MaxDegree"] == DBNull.Value
                ? 100
                : Convert.ToDecimal(row["MaxDegree"]);

            nudPassDegree.Value = row["PassDegree"] == DBNull.Value
                ? 50
                : Convert.ToDecimal(row["PassDegree"]);

            chkIsActive.Checked = row["IsActive"] != DBNull.Value &&
                                  Convert.ToBoolean(row["IsActive"]);

            txtNotes.Text = row["Notes"] == DBNull.Value
                ? ""
                : row["Notes"].ToString();

            ConfigureFixedSubjectsMode();
        }

        private void ClearFields()
        {
            selectedSubjectId = 0;

            txtSubjectID.Clear();
            txtSubjectCode.Clear();
            txtSubjectName.Clear();

            cmbClass.Enabled = true;

            if (cmbClass.Items.Count > 0)
                cmbClass.SelectedIndex = 0;

            cmbClass.Enabled = false;

            nudMaxDegree.Value = 100;
            nudPassDegree.Value = 50;
            chkIsActive.Checked = true;
            txtNotes.Clear();

            ConfigureFixedSubjectsMode();

            txtNotes.Focus();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
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

            dataGridViewSubjects.DataSource = subjectsTable.DefaultView;
            UpdateRecordCount();
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
