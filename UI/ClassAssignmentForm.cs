using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using SchoolSystem.Models;
using SchoolSystem.Helpers;
using SchoolSystem.Services;

namespace SchoolSystem.UI.Students
{
    public partial class ClassAssignmentForm : UserControl
    {
        private readonly StudentClassService studentClassService = new StudentClassService();
        private readonly ClassService classService = new ClassService();

        private DataTable unassignedStudents;
        private int selectedStudentClassId = 0;
        private bool isLoading = false;

        public ClassAssignmentForm()
        {
            InitializeComponent();
            SchoolSystem.Helpers.UIHelper.ApplyStyle(this);
            ApplyCustomStyles();
            this.Dock = DockStyle.Fill;
            this.Load += ClassAssignmentForm_Load;
        }

        private void ApplyCustomStyles()
        {
            UIHelper.StyleDataGridView(dataGridViewAssigned);
            UIHelper.StylePrimaryButton(btnLoad);
            UIHelper.StylePrimaryButton(btnAssign);
            UIHelper.StyleButton(btnSelectAll, UIHelper.NeutralColor);
            UIHelper.StyleButton(btnRemove, UIHelper.DangerColor);
            UIHelper.StyleTextBox(txtAcademicYear);
            UIHelper.StyleTextBox(txtSearch);
            UIHelper.StyleComboBox(cmbClass);
            UIHelper.StyleComboBox(cmbSection);

            listBoxUnassigned.BackColor = UIHelper.SurfaceColor;
            listBoxUnassigned.ForeColor = UIHelper.TextColor;
            listBoxUnassigned.BorderStyle = BorderStyle.FixedSingle;
            lblStatus.ForeColor = UIHelper.MutedTextColor;
        }

        private async void ClassAssignmentForm_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                isLoading = true;

                LoadStaticData();
                await LoadClassesAsync();
                await LoadSectionsAsync();

                isLoading = false;

                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                isLoading = false;
                UIHelper.ShowException("حدث خطأ أثناء تحميل واجهة توزيع الفصول:\n", ex);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void LoadStaticData()
        {
            cmbSection.DataSource = null;
            cmbSection.Items.Clear();
            cmbSection.Enabled = false;

            int year = DateTime.Now.Year;
            txtAcademicYear.Text = year + "/" + (year + 1);

            lblStatus.Text = "جاهز";
            lblRecordCount.Text = "عدد الطلاب: 0";
            lblUnassigned.Text = "الطلاب غير الموزعين: 0";
            lblAssignedTitle.Text = "الطلاب الموزعون على الصف والشعبة المحددة";
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
            catch (Exception ex)
            {
                cmbClass.DataSource = null;
                cmbClass.Items.Clear();
                cmbClass.Items.Add("تعذر تحميل الصفوف");
                cmbClass.SelectedIndex = 0;
                cmbClass.Enabled = false;
                UIHelper.ShowException("تحميل صفوف توزيع الطلاب", ex);
            }
        }

        private async Task LoadSectionsAsync()
        {
            int classId = GetSelectedClassId();
            string academicYear = txtAcademicYear.Text.Trim();

            cmbSection.DataSource = null;
            cmbSection.Items.Clear();
            cmbSection.Enabled = false;

            if (classId <= 0 || !IsValidAcademicYear(academicYear))
                return;

            try
            {
                DataTable sections = await Task.Run(() => studentClassService.GetSections(classId, academicYear));

                if (sections == null || sections.Rows.Count == 0)
                {
                    cmbSection.DataSource = null;
                    cmbSection.Items.Clear();
                    cmbSection.Enabled = false;
                    lblStatus.Text = "لا توجد شعب مسجلة لهذا الصف والعام الدراسي.";
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
                UIHelper.ShowException("تحميل شعب توزيع الطلاب", ex);
            }
        }

        private async Task LoadDataAsync()
        {
            int classId = GetSelectedClassId();

            if (classId <= 0)
                return;

            if (!cmbSection.Enabled)
                return;

            string section = cmbSection.Text.Trim();
            string academicYear = txtAcademicYear.Text.Trim();

            if (string.IsNullOrWhiteSpace(section))
                return;

            if (string.IsNullOrWhiteSpace(academicYear))
                return;

            try
            {
                Cursor = Cursors.WaitCursor;

                selectedStudentClassId = 0;

                unassignedStudents = await Task.Run(() =>
                    studentClassService.GetUnassignedStudents(academicYear));

                FillUnassignedList(unassignedStudents);

                DataTable assigned = await Task.Run(() =>
                    studentClassService.GetAssignedStudents(classId, section, academicYear));

                dataGridViewAssigned.DataSource = assigned;

                FormatAssignedGrid();

                lblAssignedTitle.Text = "الطلاب الموزعون على: " + cmbClass.Text + " - شعبة " + section;
                lblRecordCount.Text = "عدد الطلاب: " + assigned.Rows.Count;
                lblStatus.Text = "تم تحميل البيانات بنجاح.";
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("خطأ أثناء تحميل بيانات التوزيع:\n", ex);
                lblStatus.Text = "حدث خطأ أثناء تحميل البيانات.";
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private int GetSelectedClassId()
        {
            if (cmbClass.SelectedValue == null)
                return 0;

            if (cmbClass.SelectedValue is DataRowView)
                return 0;

            int result;

            if (int.TryParse(cmbClass.SelectedValue.ToString(), out result))
                return result;

            return 0;
        }

        private void FillUnassignedList(DataTable dt)
        {
            listBoxUnassigned.Items.Clear();

            if (dt == null)
            {
                lblUnassigned.Text = "الطلاب غير الموزعين: 0";
                return;
            }

            foreach (DataRow row in dt.Rows)
            {
                int id = 0;

                if (row.Table.Columns.Contains("StudentID") && row["StudentID"] != DBNull.Value)
                    id = Convert.ToInt32(row["StudentID"]);

                string number = "";

                if (row.Table.Columns.Contains("StudentNumber") && row["StudentNumber"] != DBNull.Value)
                    number = row["StudentNumber"].ToString();

                string name = "";

                if (row.Table.Columns.Contains("StudentName") && row["StudentName"] != DBNull.Value)
                    name = row["StudentName"].ToString();

                if (id > 0)
                    listBoxUnassigned.Items.Add(new StudentListItem(id, number, name));
            }

            lblUnassigned.Text = "الطلاب غير الموزعين: " + listBoxUnassigned.Items.Count;
        }

        private void FormatAssignedGrid()
        {
            if (dataGridViewAssigned.Columns.Count == 0)
                return;

            if (dataGridViewAssigned.Columns.Contains("StudentClassID"))
            {
                dataGridViewAssigned.Columns["StudentClassID"].HeaderText = "رقم التوزيع";
                dataGridViewAssigned.Columns["StudentClassID"].Width = 80;
            }

            if (dataGridViewAssigned.Columns.Contains("StudentID"))
                dataGridViewAssigned.Columns["StudentID"].Visible = false;

            if (dataGridViewAssigned.Columns.Contains("ClassID"))
                dataGridViewAssigned.Columns["ClassID"].Visible = false;

            if (dataGridViewAssigned.Columns.Contains("StudentNumber"))
                dataGridViewAssigned.Columns["StudentNumber"].HeaderText = "الرقم الأكاديمي";

            if (dataGridViewAssigned.Columns.Contains("StudentName"))
                dataGridViewAssigned.Columns["StudentName"].HeaderText = "اسم الطالب";

            if (dataGridViewAssigned.Columns.Contains("Gender"))
                dataGridViewAssigned.Columns["Gender"].HeaderText = "الجنس";

            if (dataGridViewAssigned.Columns.Contains("Phone"))
                dataGridViewAssigned.Columns["Phone"].HeaderText = "الهاتف";

            if (dataGridViewAssigned.Columns.Contains("ClassName"))
                dataGridViewAssigned.Columns["ClassName"].HeaderText = "الصف";

            if (dataGridViewAssigned.Columns.Contains("Section"))
                dataGridViewAssigned.Columns["Section"].HeaderText = "الشعبة";

            if (dataGridViewAssigned.Columns.Contains("AcademicYear"))
                dataGridViewAssigned.Columns["AcademicYear"].HeaderText = "العام الدراسي";

            if (dataGridViewAssigned.Columns.Contains("AssignedDate"))
                dataGridViewAssigned.Columns["AssignedDate"].HeaderText = "تاريخ التوزيع";

            dataGridViewAssigned.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewAssigned.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewAssigned.MultiSelect = false;
            dataGridViewAssigned.ReadOnly = true;
            dataGridViewAssigned.AllowUserToAddRows = false;
            dataGridViewAssigned.AllowUserToDeleteRows = false;
            dataGridViewAssigned.RowHeadersVisible = false;
        }

        private async void btnLoad_Click(object sender, EventArgs e)
        {
            await LoadDataAsync();
        }

        private async void cmbClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoading)
                return;

            if (GetSelectedClassId() <= 0)
                return;

            await LoadSectionsAsync();
            await LoadDataAsync();
        }

        private async void cmbSection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoading)
                return;

            await LoadDataAsync();
        }

        private async void txtAcademicYear_TextChanged(object sender, EventArgs e)
        {
            if (isLoading)
                return;

            string academicYear = txtAcademicYear.Text.Trim();

            if (academicYear.Length == 9 && IsValidAcademicYear(academicYear))
            {
                await LoadSectionsAsync();
                await LoadDataAsync();
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyUnassignedSearch();
        }

        private void ApplyUnassignedSearch()
        {
            if (unassignedStudents == null)
                return;

            string keyword = txtSearch.Text.Trim();

            if (string.IsNullOrWhiteSpace(keyword))
            {
                FillUnassignedList(unassignedStudents);
                return;
            }

            string safe = UIHelper.EscapeDataViewFilterValue(keyword);

            DataView dv = unassignedStudents.DefaultView;

            string filter = "";

            if (unassignedStudents.Columns.Contains("StudentName"))
                filter = "StudentName LIKE '%" + safe + "%'";

            if (unassignedStudents.Columns.Contains("StudentNumber"))
            {
                if (!string.IsNullOrWhiteSpace(filter))
                    filter += " OR ";

                filter += "StudentNumber LIKE '%" + safe + "%'";
            }

            dv.RowFilter = filter;

            FillUnassignedList(dv.ToTable());
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listBoxUnassigned.Items.Count; i++)
            {
                listBoxUnassigned.SetItemChecked(i, true);
            }

            lblStatus.Text = "تم تحديد جميع الطلاب غير الموزعين.";
        }

        private bool IsValidAcademicYear(string academicYear)
        {
            if (string.IsNullOrWhiteSpace(academicYear))
                return false;

            string[] parts = academicYear.Split('/');
            int firstYear;
            int secondYear;
            if (parts.Length != 2 || parts[0].Length != 4 || parts[1].Length != 4)
                return false;
            if (!int.TryParse(parts[0], out firstYear) || !int.TryParse(parts[1], out secondYear))
                return false;

            return firstYear >= 2000 && firstYear <= 2100 && secondYear == firstYear + 1;
        }

        private async void btnAssign_Click(object sender, EventArgs e)
        {
            int classId = GetSelectedClassId();

            if (classId <= 0)
            {
                ShowWarning("اختر الصف أولاً.");
                return;
            }

            if (!cmbSection.Enabled || cmbSection.SelectedIndex < 0 || string.IsNullOrWhiteSpace(cmbSection.Text))
            {
                ShowWarning("لا توجد شعبة فعلية متاحة لهذا الصف والعام الدراسي.");
                cmbSection.Focus();
                return;
            }

            string academicYear = txtAcademicYear.Text.Trim();
            if (!IsValidAcademicYear(academicYear))
            {
                ShowWarning("أدخل العام الدراسي بالصيغة الصحيحة: 2025/2026.");
                txtAcademicYear.Focus();
                return;
            }

            if (listBoxUnassigned.CheckedItems.Count == 0)
            {
                ShowWarning("حدد طالباً واحداً على الأقل للتوزيع.");
                return;
            }

            try
            {
                Cursor = Cursors.WaitCursor;

                string section = cmbSection.Text.Trim();

                int successCount = 0;
                int failedCount = 0;

                foreach (object item in listBoxUnassigned.CheckedItems)
                {
                    StudentListItem student = item as StudentListItem;

                    if (student == null)
                        continue;

                    try
                    {
                        StudentClass assignment = new StudentClass
                        {
                            StudentID = student.StudentID,
                            ClassID = classId,
                            Section = section,
                            AcademicYear = academicYear,
                            AssignedBy = null
                        };

                        bool assigned = await Task.Run(() => studentClassService.AssignStudent(assignment));

                        if (assigned)
                            successCount++;
                        else
                            failedCount++;
                    }
                    catch
                    {
                        failedCount++;
                    }
                }

                Cursor = Cursors.Default;

                ShowInfo("تم توزيع " + successCount + " طالب بنجاح." +
                         (failedCount > 0 ? "\nلم يتم توزيع " + failedCount + " طالب بسبب وجود توزيع سابق أو خطأ في البيانات." : ""));

                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                UIHelper.ShowException("خطأ أثناء توزيع الطلاب:\n", ex);
            }
        }

        private void dataGridViewAssigned_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dataGridViewAssigned.Rows.Count == 0)
                return;

            if (!dataGridViewAssigned.Columns.Contains("StudentClassID"))
                return;

            object value = dataGridViewAssigned.Rows[e.RowIndex].Cells["StudentClassID"].Value;

            if (value != null && value != DBNull.Value)
            {
                selectedStudentClassId = Convert.ToInt32(value);
                lblStatus.Text = "تم تحديد طالب موزع من الجدول.";
            }
        }

        private async void btnRemove_Click(object sender, EventArgs e)
        {
            if (selectedStudentClassId <= 0)
            {
                ShowWarning("اختر طالباً موزعاً من الجدول أولاً.");
                return;
            }

            DialogResult result = MessageBox.Show(
                "هل تريد إزالة الطالب من هذا التوزيع؟",
                "تأكيد الإزالة",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2,
                MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);

            if (result != DialogResult.Yes)
                return;

            try
            {
                Cursor = Cursors.WaitCursor;

                bool removed = await Task.Run(() =>
                    studentClassService.RemoveAssignment(selectedStudentClassId));

                Cursor = Cursors.Default;

                if (removed)
                {
                    ShowInfo("تمت إزالة التوزيع بنجاح.");
                    selectedStudentClassId = 0;
                    await LoadDataAsync();
                }
                else
                {
                    ShowWarning("لم يتم العثور على سجل التوزيع.");
                }
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                UIHelper.ShowException("خطأ أثناء الإزالة:\n", ex);
            }
        }

        private void ShowInfo(string message)
        {
            UIHelper.ShowInfo(message);
        }

        private void ShowWarning(string message)
        {
            UIHelper.ShowWarning(message);
        }

        private void ShowError(string message)
        {
            UIHelper.ShowError(message);
        }

        private class StudentListItem
        {
            public int StudentID { get; set; }

            public string StudentNumber { get; set; }

            public string StudentName { get; set; }

            public StudentListItem(int studentID, string studentNumber, string studentName)
            {
                StudentID = studentID;
                StudentNumber = studentNumber;
                StudentName = studentName;
            }

            public override string ToString()
            {
                if (string.IsNullOrWhiteSpace(StudentNumber))
                    return StudentName;

                return StudentNumber + " - " + StudentName;
            }
        }
    }
}
