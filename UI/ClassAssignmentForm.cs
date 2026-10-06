using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using SchoolSystem.Models;
using SchoolSystem.Helpers;
using SchoolSystem.Services;


namespace SchoolSystem.UI.Students
{

    public partial class ClassAssignmentForm : UserControl
    {
        // تعريف الدالة StudentClassService لتنفيذ العملية المرتبطة بها.
        private readonly StudentClassService studentClassService = new StudentClassService();
        // تعريف الدالة ClassService لتنفيذ العملية المرتبطة بها.
        private readonly ClassService classService = new ClassService();


        private DataTable unassignedStudents;

        private int selectedStudentClassId = 0;

        private bool isLoading = false;

        // تعريف الدالة ClassAssignmentForm لتنفيذ العملية المرتبطة بها.
        public ClassAssignmentForm()
        {

            InitializeComponent();

            SchoolSystem.Helpers.UIHelper.ApplyStyle(this);
            ApplyCustomStyles();
            this.Dock = DockStyle.Fill;

            this.Load += ClassAssignmentForm_Load;
        }

        // تعريف الدالة ApplyCustomStyles لتنفيذ العملية المرتبطة بها.
        private void ApplyCustomStyles()
        {
            UIHelper.StyleDataGridView(dataGridViewAssigned);
            UIHelper.StylePrimaryButton(btnLoad);
            UIHelper.StylePrimaryButton(btnAssign);
            UIHelper.StyleButton(btnSelectAll, UIHelper.NeutralColor);
            UIHelper.StyleButton(btnRemove, UIHelper.DangerColor);
            UIHelper.StyleButton(btnRemoveAll, Color.FromArgb(185, 28, 28));
            UIHelper.StyleButton(btnTransfer, Color.FromArgb(124, 58, 237));
            UIHelper.StyleComboBox(cmbTransferSection);
            UIHelper.StyleTextBox(txtAcademicYear);
            UIHelper.StyleTextBox(txtSearch);
            UIHelper.StyleComboBox(cmbClass);
            UIHelper.StyleComboBox(cmbSection);

            listBoxUnassigned.BackColor = UIHelper.SurfaceColor;
            listBoxUnassigned.ForeColor = UIHelper.TextColor;
            listBoxUnassigned.BorderStyle = BorderStyle.FixedSingle;
            lblStatus.ForeColor = UIHelper.MutedTextColor;
        }

        // تعريف الدالة ClassAssignmentForm_Load لتنفيذ العملية المرتبطة بها.
        private async void ClassAssignmentForm_Load(object sender, EventArgs e)
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
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

        // تعريف الدالة LoadStaticData لتنفيذ العملية المرتبطة بها.
        private void LoadStaticData()
        {
            cmbSection.DataSource = null;
            cmbSection.Items.Clear();
            cmbSection.Enabled = false;
            cmbTransferSection.DataSource = null;
            cmbTransferSection.Items.Clear();
            cmbTransferSection.Enabled = false;

            DateTime today = DateTime.Today;
            int year = today.Month >= 8 ? today.Year : today.Year - 1;
            txtAcademicYear.Text = year + "/" + (year + 1);

            lblStatus.Text = "جاهز";
            lblRecordCount.Text = "عدد الطلاب: 0";
            lblUnassigned.Text = "الطلاب غير الموزعين: 0";
            lblAssignedTitle.Text = "الطلاب الموزعون على الصف والشعبة المحددة";
        }

        // تعريف الدالة LoadClassesAsync لتنفيذ العملية المرتبطة بها.
        private async Task LoadClassesAsync()
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
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

        // تعريف الدالة LoadSectionsAsync لتنفيذ العملية المرتبطة بها.
        private async Task LoadSectionsAsync()
        {
            int classId = GetSelectedClassId();
            string academicYear = txtAcademicYear.Text.Trim();

            cmbSection.DataSource = null;
            cmbSection.Items.Clear();
            cmbSection.Enabled = false;

            if (classId <= 0 || !IsValidAcademicYear(academicYear))
                return;

            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {

                DataTable sections = await Task.Run(() => studentClassService.GetSections(classId, academicYear));

                if (sections == null || sections.Rows.Count == 0)
                {
                    cmbSection.DataSource = null;
                    cmbSection.Items.Clear();
                    cmbSection.Enabled = false;
                    cmbTransferSection.DataSource = null;
                    cmbTransferSection.Items.Clear();
                    cmbTransferSection.Enabled = false;
                    lblStatus.Text = "لا توجد شعب مسجلة لهذا الصف والعام الدراسي.";

                    return;
                }

                cmbSection.DataSource = sections;
                cmbSection.DisplayMember = "Section";
                cmbSection.ValueMember = "Section";
                cmbSection.Enabled = true;
                cmbSection.SelectedIndex = 0;
                UpdateTransferSections(sections);
            }

            catch (Exception ex)
            {
                cmbSection.DataSource = null;
                cmbSection.Items.Clear();
                cmbSection.Enabled = false;
                cmbTransferSection.DataSource = null;
                cmbTransferSection.Items.Clear();
                cmbTransferSection.Enabled = false;
                UIHelper.ShowException("تحميل شعب توزيع الطلاب", ex);
            }
        }

        // تعريف الدالة LoadDataAsync لتنفيذ العملية المرتبطة بها.
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

            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
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

                await RefreshStatisticsAsync(classId, academicYear, assigned.Rows.Count);
                if (string.IsNullOrWhiteSpace(lblStatus.Text))
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

        // تعريف الدالة GetSelectedClassId لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة UpdateTransferSections لتنفيذ العملية المرتبطة بها.
        private void UpdateTransferSections(DataTable sections)
        {
            string current = cmbSection.Text == null ? string.Empty : cmbSection.Text.Trim();
            cmbTransferSection.DataSource = null;
            cmbTransferSection.Items.Clear();

            if (sections == null || !sections.Columns.Contains("Section"))
            {
                cmbTransferSection.Enabled = false;
                return;
            }

            foreach (DataRow row in sections.Rows)
            {
                string section = Convert.ToString(row["Section"]).Trim();
                if (!string.IsNullOrWhiteSpace(section) && !section.Equals(current, StringComparison.OrdinalIgnoreCase))
                    cmbTransferSection.Items.Add(section);
            }

            cmbTransferSection.Enabled = cmbTransferSection.Items.Count > 0;
            if (cmbTransferSection.Items.Count > 0)
                cmbTransferSection.SelectedIndex = 0;
        }

        // تعريف الدالة RefreshStatisticsAsync لتنفيذ العملية المرتبطة بها.
        private async Task RefreshStatisticsAsync(int classId, string academicYear, int assignedInCurrentSection)
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {

                DataTable stats = await Task.Run(() => studentClassService.GetSectionStatistics(classId, academicYear));
                int totalAssigned = 0;
                int fullSections = 0;
                foreach (DataRow row in stats.Rows)
                {
                    int count = row["AssignedCount"] == DBNull.Value ? 0 : Convert.ToInt32(row["AssignedCount"]);
                    totalAssigned += count;
                    if (row.Table.Columns.Contains("Capacity") && row["Capacity"] != DBNull.Value)
                    {
                        int capacity = Convert.ToInt32(row["Capacity"]);
                        if (capacity > 0 && count >= capacity)
                            fullSections++;
                    }
                }

                int totalUnassigned = unassignedStudents == null ? 0 : unassignedStudents.Rows.Count;
                lblUnassigned.Text = "الطلاب غير الموزعين: " + totalUnassigned;
                lblStatus.Text = "إحصائيات الصف: موزعون " + totalAssigned +
                    " | غير موزعين " + totalUnassigned +
                    " | في الشعبة الحالية " + assignedInCurrentSection +
                    " | عدد الشعب " + stats.Rows.Count +
                    (fullSections > 0 ? " | مكتملة السعة " + fullSections : string.Empty);
            }

            catch (Exception ex)
            {
                ApplicationLogger.LogException("شاشة توزيع الفصول: فشل تحميل الإحصائيات", ex);
                lblStatus.Text = "تم تحميل البيانات؛ تعذر إظهار بعض الإحصائيات.";
            }
        }

        // تعريف الدالة FillUnassignedList لتنفيذ العملية المرتبطة بها.
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

                int enrolledClassId = 0;
                if (row.Table.Columns.Contains("EnrolledClassID") && row["EnrolledClassID"] != DBNull.Value)
                    int.TryParse(row["EnrolledClassID"].ToString(), out enrolledClassId);

                string enrolledClassName = string.Empty;
                if (row.Table.Columns.Contains("EnrolledClassName") && row["EnrolledClassName"] != DBNull.Value)
                    enrolledClassName = row["EnrolledClassName"].ToString().Trim();

                string enrolledSection = string.Empty;
                if (row.Table.Columns.Contains("EnrolledSection") && row["EnrolledSection"] != DBNull.Value)
                    enrolledSection = row["EnrolledSection"].ToString().Trim();

                if (id > 0)
                {
                    listBoxUnassigned.Items.Add(
                        new StudentListItem(
                            id,
                            number,
                            name,
                            enrolledClassId,
                            enrolledClassName,
                            enrolledSection));
                }
            }

            lblUnassigned.Text = "الطلاب غير الموزعين: " + listBoxUnassigned.Items.Count;
        }

        // تعريف الدالة FormatAssignedGrid لتنفيذ العملية المرتبطة بها.
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
            dataGridViewAssigned.MultiSelect = true;
            dataGridViewAssigned.ReadOnly = true;
            dataGridViewAssigned.AllowUserToAddRows = false;
            dataGridViewAssigned.AllowUserToDeleteRows = false;
            dataGridViewAssigned.RowHeadersVisible = false;
        }

        // تعريف الدالة btnLoad_Click لتنفيذ العملية المرتبطة بها.
        private async void btnLoad_Click(object sender, EventArgs e)
        {

            await LoadDataAsync();
        }

        // تعريف الدالة cmbClass_SelectedIndexChanged لتنفيذ العملية المرتبطة بها.
        private async void cmbClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoading || GetSelectedClassId() <= 0)
                return;


            await ReloadAssignmentDataAsync(true);
        }

        // تعريف الدالة cmbSection_SelectedIndexChanged لتنفيذ العملية المرتبطة بها.
        private async void cmbSection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoading)
                return;


            await ReloadAssignmentDataAsync(false);
        }

        // تعريف الدالة txtAcademicYear_TextChanged لتنفيذ العملية المرتبطة بها.
        private async void txtAcademicYear_TextChanged(object sender, EventArgs e)
        {
            if (isLoading)
                return;

            string academicYear = txtAcademicYear.Text.Trim();
            if (academicYear.Length == 9 && IsValidAcademicYear(academicYear))

                await ReloadAssignmentDataAsync(true);
        }

        // تعريف الدالة ReloadAssignmentDataAsync لتنفيذ العملية المرتبطة بها.
        private async Task ReloadAssignmentDataAsync(bool reloadSections)
        {
            if (isLoading)
                return;

            isLoading = true;
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                if (reloadSections)

                    await LoadSectionsAsync();


                await LoadDataAsync();
            }

            finally
            {
                isLoading = false;
            }
        }

        // تعريف الدالة txtSearch_TextChanged لتنفيذ العملية المرتبطة بها.
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyUnassignedSearch();
        }

        // تعريف الدالة ApplyUnassignedSearch لتنفيذ العملية المرتبطة بها.
        private void ApplyUnassignedSearch()
        {
            if (unassignedStudents == null)
                return;

            string keyword = txtSearch.Text.Trim();

            if (string.IsNullOrWhiteSpace(keyword))
            {
                unassignedStudents.DefaultView.RowFilter = string.Empty;
                FillUnassignedList(unassignedStudents);

                return;
            }

            DataView dv = unassignedStudents.DefaultView;
            var searchableColumns = new List<string>();

            if (unassignedStudents.Columns.Contains("StudentName"))
                searchableColumns.Add("StudentName");

            foreach (string column in new[] { "StudentNumber", "Gender", "Phone" })
            {
                if (unassignedStudents.Columns.Contains(column))
                    searchableColumns.Add(column);
            }

            dv.RowFilter = UIHelper.BuildDataViewSearchFilter(keyword, searchableColumns.ToArray());

            FillUnassignedList(dv.ToTable());
        }

        // ربط الطالب المحدد بالصف الذي تم قبوله في واجهة التسجيل والقبول.
        private async void listBoxUnassigned_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoading)
                return;

            StudentListItem student =
                listBoxUnassigned.SelectedItem as StudentListItem;

            if (student == null || student.EnrolledClassID <= 0)
                return;

            if (GetSelectedClassId() == student.EnrolledClassID)
                return;

            try
            {
                isLoading = true;

                cmbClass.SelectedValue = student.EnrolledClassID;

                await LoadSectionsAsync();

                lblStatus.Text =
                    "تم اختيار الصف المسجل للطالب: " +
                    student.EnrolledClassName +
                    ". اختر الشعبة ثم نفّذ التوزيع.";
            }
            catch (Exception ex)
            {
                ApplicationLogger.LogException(
                    "شاشة توزيع الفصول: فشل مزامنة الصف مع التسجيل",
                    ex);

                UIHelper.ShowException(
                    "مزامنة الصف المسجل",
                    ex);
            }
            finally
            {
                isLoading = false;
            }
        }

        // تعريف الدالة btnSelectAll_Click لتنفيذ العملية المرتبطة بها.
        private void btnSelectAll_Click(object sender, EventArgs e)
        {

            for (int i = 0; i < listBoxUnassigned.Items.Count; i++)
            {
                listBoxUnassigned.SetItemChecked(i, true);
            }

            lblStatus.Text = "تم تحديد جميع الطلاب غير الموزعين.";
        }

        // تعريف الدالة IsValidAcademicYear لتنفيذ العملية المرتبطة بها.
        private bool IsValidAcademicYear(string academicYear)
        {
            if (string.IsNullOrWhiteSpace(academicYear))
                return false;

            string[] parts = academicYear.Replace('-', '/').Split('/');
            int firstYear;
            int secondYear;
            if (parts.Length != 2 || parts[0].Length != 4 || parts[1].Length != 4)
                return false;
            if (!int.TryParse(parts[0], out firstYear) || !int.TryParse(parts[1], out secondYear))
                return false;


            return secondYear == firstYear + 1;
        }

        // تعريف الدالة btnAssign_Click لتنفيذ العملية المرتبطة بها.
        private async void btnAssign_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAcademicYear.Text) ||
                !IsValidAcademicYear(txtAcademicYear.Text.Trim()))
            {
                ShowWarning("أدخل العام الدراسي بالصيغة الصحيحة: 2026/2027.");
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

                List<StudentListItem> selectedStudents = new List<StudentListItem>();

                foreach (object item in listBoxUnassigned.CheckedItems)
                {
                    StudentListItem student = item as StudentListItem;
                    if (student != null)
                        selectedStudents.Add(student);
                }

                if (selectedStudents.Count == 0)
                {
                    ShowWarning("حدد طالباً واحداً على الأقل للتوزيع.");
                    return;
                }

                int registeredClassId = selectedStudents[0].EnrolledClassID;
                if (registeredClassId <= 0)
                {
                    ShowWarning(
                        "لا يمكن تحديد الصف المسجل لأحد الطلاب. حدّث بيانات التسجيل ثم أعد تحميل القائمة.");
                    return;
                }

                foreach (StudentListItem student in selectedStudents)
                {
                    if (student.EnrolledClassID <= 0)
                    {
                        ShowWarning(
                            "يوجد طالب بلا صف مسجل. صحح بيانات التسجيل أولاً.");
                        return;
                    }

                    if (student.EnrolledClassID != registeredClassId)
                    {
                        ShowWarning(
                            "الطلاب المحددون من صفوف تسجيل مختلفة. التوزيع الجماعي يجب أن يكون لطلاب من الصف المسجل نفسه.");
                        return;
                    }
                }

                string academicYear = txtAcademicYear.Text.Trim();
                string preferredSection = cmbSection.Text == null
                    ? string.Empty
                    : cmbSection.Text.Trim();

                // الصف المعتمد للتوزيع يُؤخذ دائماً من التسجيل المقبول.
                if (GetSelectedClassId() != registeredClassId)
                {
                    cmbClass.SelectedValue = registeredClassId;
                    await LoadSectionsAsync();
                }
                else if (!cmbSection.Enabled || cmbSection.Items.Count == 0)
                {
                    await LoadSectionsAsync();
                }

                if (!cmbSection.Enabled || cmbSection.Items.Count == 0)
                {
                    ShowWarning(
                        "لا توجد شعب فعالة للصف المسجل في العام الدراسي المحدد.");
                    cmbSection.Focus();
                    return;
                }

                if (!string.IsNullOrWhiteSpace(preferredSection) &&
                    cmbSection.Items.Contains(preferredSection))
                {
                    cmbSection.SelectedItem = preferredSection;
                }

                string section = cmbSection.Text.Trim();
                if (string.IsNullOrWhiteSpace(section))
                {
                    ShowWarning("اختر الشعبة التي تريد توزيع الطلاب عليها.");
                    cmbSection.Focus();
                    return;
                }

                int successCount = 0;
                int failedCount = 0;

                foreach (StudentListItem student in selectedStudents)
                {
                    try
                    {
                        StudentClass assignment = new StudentClass
                        {
                            StudentID = student.StudentID,
                            ClassID = registeredClassId,
                            Section = section,
                            AcademicYear = academicYear,
                            AssignedBy = null
                        };

                        bool assigned = await Task.Run(
                            () => studentClassService.AssignStudent(assignment));

                        if (assigned)
                            successCount++;
                        else
                            failedCount++;
                    }
                    catch (Exception ex)
                    {
                        ApplicationLogger.LogException(
                            "شاشة توزيع الفصول: فشل توزيع طالب",
                            ex);
                        failedCount++;
                    }
                }

                ShowInfo(
                    "تم توزيع " + successCount + " طالب بنجاح." +
                    (failedCount > 0
                        ? "\nتعذر توزيع " + failedCount + " طالب بسبب خطأ في البيانات أو قيود الشعبة."
                        : string.Empty));

                ApplicationLogger.LogUsage(
                    "شاشة توزيع الفصول",
                    "توزيع طلاب",
                    "توزيع " + successCount + " طالب على الصف " +
                    cmbClass.Text + " - شعبة " + section);

                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("خطأ أثناء توزيع الطلاب:\n", ex);
            }
            finally
            {
                Cursor = Cursors.Default;
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

        // تعريف الدالة ShowError لتنفيذ العملية المرتبطة بها.
        private void ShowError(string message)
        {
            UIHelper.ShowError(message);
        }


        private class StudentListItem
        {
            public int StudentID { get; set; }

            public string StudentNumber { get; set; }

            public string StudentName { get; set; }

            public int EnrolledClassID { get; set; }

            public string EnrolledClassName { get; set; }

            public string EnrolledSection { get; set; }

            public StudentListItem(
                int studentID,
                string studentNumber,
                string studentName,
                int enrolledClassId,
                string enrolledClassName,
                string enrolledSection)
            {
                StudentID = studentID;
                StudentNumber = studentNumber;
                StudentName = studentName;
                EnrolledClassID = enrolledClassId;
                EnrolledClassName = enrolledClassName ?? string.Empty;
                EnrolledSection = enrolledSection ?? string.Empty;
            }

            public override string ToString()
            {
                string student = string.IsNullOrWhiteSpace(StudentNumber)
                    ? StudentName
                    : StudentNumber + " - " + StudentName;

                if (string.IsNullOrWhiteSpace(EnrolledClassName))
                    return student;

                return "الصف: " + EnrolledClassName + " | " + student;
            }
        }
    }
}
