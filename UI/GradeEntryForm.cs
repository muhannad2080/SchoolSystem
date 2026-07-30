using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using SchoolSystem.Models;
using SchoolSystem.Services;

namespace SchoolSystem.UI
{
    public partial class GradeEntryForm : UserControl
    {
        private readonly GradeService gradeService = new GradeService();
        private readonly ClassService classService = new ClassService();

        private DataTable currentGradesTable;
        private int selectedGradeId = 0;
        private bool isLoading = false;

        public GradeEntryForm()
        {
            InitializeComponent();

            Dock = DockStyle.Fill;

            Load += GradeEntryForm_Load;
            txtSearch.TextChanged += txtSearch_TextChanged;

            cmbClass.SelectedIndexChanged += cmbClass_SelectedIndexChanged;
        }

        private async void GradeEntryForm_Load(object sender, EventArgs e)
        {
            try
            {
                isLoading = true;

                LoadStaticData();
                await LoadClassesAsync();

                isLoading = false;

                await LoadSubjectsForSelectedClassAsync();
            }
            catch (Exception ex)
            {
                isLoading = false;
                ShowError("حدث خطأ أثناء تحميل واجهة الدرجات:\n" + ex.Message);
            }
        }

        private void LoadStaticData()
        {
            int year = DateTime.Now.Year;
            txtAcademicYear.Text = year + "/" + (year + 1);

            cmbSection.Items.Clear();
            cmbSection.Items.Add("أ");
            cmbSection.Items.Add("ب");
            cmbSection.Items.Add("ج");
            cmbSection.Items.Add("د");

            if (cmbSection.Items.Count > 0)
                cmbSection.SelectedIndex = 0;

            cmbTerm.Items.Clear();
            cmbTerm.Items.Add("الفصل الأول");
            cmbTerm.Items.Add("الفصل الثاني");
            cmbTerm.Items.Add("الدور النهائي");

            if (cmbTerm.Items.Count > 0)
                cmbTerm.SelectedIndex = 0;
        }

        private async Task LoadClassesAsync()
        {
            try
            {
                DataTable classes = await Task.Run(() => classService.GetAllClasses());

                cmbClass.DataSource = classes;
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

                fallback.Rows.Add(1, "الصف الأول الابتدائي");
                fallback.Rows.Add(2, "الصف الثاني الابتدائي");
                fallback.Rows.Add(3, "الصف الثالث الابتدائي");

                cmbClass.DataSource = fallback;
                cmbClass.DisplayMember = "ClassName";
                cmbClass.ValueMember = "ClassID";

                if (cmbClass.Items.Count > 0)
                    cmbClass.SelectedIndex = 0;
            }
        }

        private int GetClassId()
        {
            if (cmbClass.SelectedValue == null || cmbClass.SelectedValue is DataRowView)
                return 0;

            int value;

            if (int.TryParse(cmbClass.SelectedValue.ToString(), out value))
                return value;

            return 0;
        }

        private int GetSubjectId()
        {
            if (cmbSubject.SelectedValue == null || cmbSubject.SelectedValue is DataRowView)
                return 0;

            int value;

            if (int.TryParse(cmbSubject.SelectedValue.ToString(), out value))
                return value;

            return 0;
        }

        private async void cmbClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoading)
                return;

            await LoadSubjectsForSelectedClassAsync();

            currentGradesTable = null;
            dataGridViewGrades.DataSource = null;
            lblRecordCount.Text = "عدد الطلاب: 0";
            selectedGradeId = 0;
        }

        private async Task LoadSubjectsForSelectedClassAsync()
        {
            try
            {
                int classId = GetClassId();

                if (classId <= 0)
                    return;

                DataTable subjects = await Task.Run(() => gradeService.GetSubjectsByClass(classId));

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
                    ShowWarning("لا توجد مواد مفعلة لهذا الصف. يرجى مراجعة واجهة إدارة المواد.");
                }
            }
            catch (Exception ex)
            {
                ShowError("خطأ أثناء تحميل مواد الصف:\n" + ex.Message);
            }
        }

        private async void btnLoad_Click(object sender, EventArgs e)
        {
            await LoadGradesAsync();
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await LoadSubjectsForSelectedClassAsync();
            await LoadGradesAsync();
        }

        private async Task LoadGradesAsync()
        {
            try
            {
                int classId = GetClassId();
                int subjectId = GetSubjectId();

                if (classId <= 0)
                {
                    ShowWarning("يرجى اختيار الصف.");
                    return;
                }

                if (subjectId <= 0)
                {
                    ShowWarning("لا توجد مادة محددة لهذا الصف.");
                    return;
                }

                Cursor = Cursors.WaitCursor;

                currentGradesTable = await Task.Run(() =>
                    gradeService.GetGradeEntryStudents(
                        classId,
                        cmbSection.Text,
                        txtAcademicYear.Text.Trim(),
                        subjectId,
                        cmbTerm.Text));

                dataGridViewGrades.DataSource = currentGradesTable;

                FormatGrid();

                lblRecordCount.Text = "عدد الطلاب: " + currentGradesTable.Rows.Count;

                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                ShowError("خطأ أثناء تحميل الطلاب والدرجات:\n" + ex.Message);
            }
        }

        private void FormatGrid()
        {
            if (dataGridViewGrades.Columns.Count == 0)
                return;

            SetColumnHeader("StudentNumber", "الرقم الأكاديمي");
            SetColumnHeader("StudentName", "اسم الطالب");
            SetColumnHeader("Gender", "الجنس");
            SetColumnHeader("Quiz1", "اختبار 1");
            SetColumnHeader("Quiz2", "اختبار 2");
            SetColumnHeader("CourseWork", "أعمال السنة");
            SetColumnHeader("FinalExam", "الاختبار النهائي");
            SetColumnHeader("Total", "المجموع");
            SetColumnHeader("GradeLetter", "التقدير");
            SetColumnHeader("ResultStatus", "الحالة");
            SetColumnHeader("Notes", "ملاحظات");

            HideColumn("StudentID");
            HideColumn("GradeID");

            MakeReadOnly("StudentNumber");
            MakeReadOnly("StudentName");
            MakeReadOnly("Gender");
            MakeReadOnly("Total");
            MakeReadOnly("GradeLetter");
            MakeReadOnly("ResultStatus");

            dataGridViewGrades.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void SetColumnHeader(string columnName, string header)
        {
            if (dataGridViewGrades.Columns.Contains(columnName))
                dataGridViewGrades.Columns[columnName].HeaderText = header;
        }

        private void HideColumn(string columnName)
        {
            if (dataGridViewGrades.Columns.Contains(columnName))
                dataGridViewGrades.Columns[columnName].Visible = false;
        }

        private void MakeReadOnly(string columnName)
        {
            if (dataGridViewGrades.Columns.Contains(columnName))
                dataGridViewGrades.Columns[columnName].ReadOnly = true;
        }

        private void dataGridViewGrades_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            RecalculateRow(e.RowIndex);
        }

        private void RecalculateRow(int rowIndex)
        {
            DataGridViewRow row = dataGridViewGrades.Rows[rowIndex];

            decimal q1 = GetDecimal(row, "Quiz1");
            decimal q2 = GetDecimal(row, "Quiz2");
            decimal cw = GetDecimal(row, "CourseWork");
            decimal final = GetDecimal(row, "FinalExam");

            decimal total = q1 + q2 + cw + final;

            if (total > 100)
            {
                ShowWarning("المجموع لا يمكن أن يتجاوز 100.");
                return;
            }

            if (dataGridViewGrades.Columns.Contains("Total"))
                row.Cells["Total"].Value = total;

            if (dataGridViewGrades.Columns.Contains("GradeLetter"))
                row.Cells["GradeLetter"].Value = GetGradeLetter(total);

            if (dataGridViewGrades.Columns.Contains("ResultStatus"))
                row.Cells["ResultStatus"].Value = total >= 50 ? "ناجح" : "راسب";
        }

        private decimal GetDecimal(DataGridViewRow row, string columnName)
        {
            if (!dataGridViewGrades.Columns.Contains(columnName))
                return 0;

            object value = row.Cells[columnName].Value;

            if (value == null || value == DBNull.Value)
                return 0;

            decimal result;

            if (decimal.TryParse(value.ToString(), out result))
                return result;

            return 0;
        }

        private string GetGradeLetter(decimal total)
        {
            if (total >= 90)
                return "ممتاز";

            if (total >= 80)
                return "جيد جدًا";

            if (total >= 70)
                return "جيد";

            if (total >= 60)
                return "مقبول";

            return "ضعيف";
        }

        private async void btnSaveAll_Click(object sender, EventArgs e)
        {
            if (dataGridViewGrades.Rows.Count == 0)
            {
                ShowWarning("لا توجد بيانات للحفظ.");
                return;
            }

            int classId = GetClassId();
            int subjectId = GetSubjectId();

            if (classId <= 0)
            {
                ShowWarning("يرجى اختيار الصف.");
                return;
            }

            if (subjectId <= 0)
            {
                ShowWarning("يرجى اختيار المادة.");
                return;
            }

            try
            {
                Cursor = Cursors.WaitCursor;

                int success = 0;

                foreach (DataGridViewRow row in dataGridViewGrades.Rows)
                {
                    if (row.IsNewRow)
                        continue;

                    StudentGrade grade = new StudentGrade();

                    grade.StudentID = Convert.ToInt32(row.Cells["StudentID"].Value);
                    grade.SubjectID = subjectId;
                    grade.ClassID = classId;
                    grade.Section = cmbSection.Text;
                    grade.AcademicYear = txtAcademicYear.Text.Trim();
                    grade.TermName = cmbTerm.Text;
                    grade.Quiz1 = GetDecimal(row, "Quiz1");
                    grade.Quiz2 = GetDecimal(row, "Quiz2");
                    grade.CourseWork = GetDecimal(row, "CourseWork");
                    grade.FinalExam = GetDecimal(row, "FinalExam");

                    if (row.Cells["Notes"].Value == null || row.Cells["Notes"].Value == DBNull.Value)
                        grade.Notes = "";
                    else
                        grade.Notes = row.Cells["Notes"].Value.ToString();

                    bool saved = await Task.Run(() => gradeService.SaveGrade(grade));

                    if (saved)
                        success++;
                }

                Cursor = Cursors.Default;

                ShowInfo("تم حفظ درجات " + success + " طالب بنجاح.");

                await LoadGradesAsync();
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                ShowError("خطأ أثناء حفظ الدرجات:\n" + ex.Message);
            }
        }

        private void dataGridViewGrades_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (!dataGridViewGrades.Columns.Contains("GradeID"))
                return;

            object value = dataGridViewGrades.Rows[e.RowIndex].Cells["GradeID"].Value;

            if (value != null && value != DBNull.Value)
                selectedGradeId = Convert.ToInt32(value);
            else
                selectedGradeId = 0;
        }

        private async void btnDeleteGrade_Click(object sender, EventArgs e)
        {
            if (selectedGradeId <= 0)
            {
                ShowWarning("اختر درجة محفوظة من الجدول أولاً.");
                return;
            }

            DialogResult result = MessageBox.Show(
                "هل تريد حذف درجة الطالب المحددة؟",
                "تأكيد الحذف",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2,
                MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);

            if (result != DialogResult.Yes)
                return;

            try
            {
                bool deleted = await Task.Run(() => gradeService.DeleteGrade(selectedGradeId));

                if (deleted)
                {
                    ShowInfo("تم حذف الدرجة بنجاح.");
                    selectedGradeId = 0;
                    await LoadGradesAsync();
                }
                else
                {
                    ShowWarning("لم يتم العثور على الدرجة.");
                }
            }
            catch (Exception ex)
            {
                ShowError("خطأ أثناء حذف الدرجة:\n" + ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            currentGradesTable = null;
            dataGridViewGrades.DataSource = null;
            lblRecordCount.Text = "عدد الطلاب: 0";
            selectedGradeId = 0;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (currentGradesTable == null)
                return;

            string keyword = txtSearch.Text.Trim();

            if (string.IsNullOrWhiteSpace(keyword))
            {
                currentGradesTable.DefaultView.RowFilter = "";
            }
            else
            {
                string safe = keyword.Replace("'", "''");

                currentGradesTable.DefaultView.RowFilter =
                    "StudentName LIKE '%" + safe + "%' OR StudentNumber LIKE '%" + safe + "%'";
            }

            lblRecordCount.Text = "عدد الطلاب: " + currentGradesTable.DefaultView.Count;
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
