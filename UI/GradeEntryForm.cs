using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using SchoolSystem.Models;
using SchoolSystem.Helpers;
using SchoolSystem.Services;
using SchoolSystem.Security;
using SchoolSystem.DataAccess;

namespace SchoolSystem.UI
{
    public partial class GradeEntryForm : UserControl
    {
        // خدمة إدارة درجات الطلاب وتنفيذ عمليات الحفظ والتعديل والحذف.
        private readonly GradeService gradeService = new GradeService();

        // خدمة الصفوف المستخدمة لتحميل الصفوف الدراسية.
        private readonly ClassService classService = new ClassService();

        // خدمة الشعب المستخدمة لقراءة الشعب الأكاديمية حسب الصف والعام.
        private readonly StudentClassService sectionService = new StudentClassService();

        // مستودع البيانات المرجعية المستخدم لتحميل الأعوام والفصول الدراسية.
        private readonly AcademicReferenceRepository academicReferenceRepository =
            new AcademicReferenceRepository();

        // جدول الدرجات المحمّل حاليًا والمستخدم للعرض والتصفية والتصدير.
        private DataTable currentGradesTable;

        // رقم الدرجة المحددة حاليًا في الجدول.
        private int selectedGradeId = 0;

        // يحدد ما إذا كانت الواجهة في حالة تحميل بيانات لمنع الأحداث المتداخلة.
        private bool isLoading = false;

        // يحدد ما إذا كان الجدول يعرض الطلاب غير المكتملة درجاتهم فقط.
        private bool showIncompleteOnly = false;

        // إنشاء الواجهة وربط الأحداث الخاصة بها دون فرض أي تنسيق إضافي.
        public GradeEntryForm()
        {
            InitializeComponent();

            // تطبيق قواعد التحقق من الحقول (فلترة الأحرف، أقصى طول، والتحقق عند
            // المغادرة) دون تغيير تنسيق الشاشة.
            SchoolSystem.Helpers.UIHelper.ApplyFieldValidation(this);

            Load += GradeEntryForm_Load;

            txtSearch.TextChanged += txtSearch_TextChanged;

            cmbClass.SelectedIndexChanged += cmbClass_SelectedIndexChanged;

            cmbTerm.SelectedIndexChanged += cmbTerm_SelectedIndexChanged;

            dataGridViewGrades.CellValidating += dataGridViewGrades_CellValidating;
        }

        // تحميل البيانات الأساسية والصفوف والشعب والمواد عند فتح الواجهة.
        private async void GradeEntryForm_Load(object sender, EventArgs e)
        {
            try
            {
                isLoading = true;

                LoadStaticData();

                await LoadClassesAsync();

                isLoading = false;

                await LoadSubjectsForSelectedClassAsync();

                await LoadSectionsAsync();
            }
            catch (Exception ex)
            {
                isLoading = false;
                UIHelper.ShowException("تحميل واجهة الدرجات", ex);
            }
        }

        // تحميل الأعوام الدراسية والفصول والشعب والبيانات المرجعية الثابتة للواجهة.
        private void LoadStaticData()
        {
            DataTable years = academicReferenceRepository.GetAcademicYears();

            cmbAcademicYear.DataSource = years;
            cmbAcademicYear.DisplayMember = "AcademicYear";
            cmbAcademicYear.ValueMember = "AcademicYearID";
            cmbAcademicYear.Enabled = years != null && years.Rows.Count > 0;

            string activeYear =
                SchoolSystem.Helpers.AcademicYearHelper.GetCurrentAcademicYear();

            if (!string.IsNullOrWhiteSpace(activeYear) &&
                cmbAcademicYear.Items.Count > 0 &&
                years != null)
            {
                foreach (DataRow row in years.Rows)
                {
                    if (string.Equals(
                        Convert.ToString(row["AcademicYear"]),
                        activeYear,
                        StringComparison.OrdinalIgnoreCase))
                    {
                        cmbAcademicYear.SelectedValue = row["AcademicYearID"];
                        break;
                    }
                }
            }

            if (cmbAcademicYear.SelectedIndex < 0 &&
                cmbAcademicYear.Items.Count > 0)
            {
                cmbAcademicYear.SelectedIndex = 0;
            }

            cmbSection.DataSource = null;
            cmbSection.Items.Clear();
            cmbSection.Enabled = false;

            cmbTerm.DataSource = null;
            cmbTerm.Items.Clear();
            cmbTerm.DisplayMember = "TermName";
            cmbTerm.ValueMember = "TermID";

            DataTable terms = academicReferenceRepository.GetActiveTerms();

            cmbTerm.DataSource = terms;
            cmbTerm.Enabled = terms != null && terms.Rows.Count > 0;

            if (terms != null && terms.Rows.Count > 0)
            {
                cmbTerm.SelectedIndex = 0;
            }
        }

        // تحميل جميع الصفوف الدراسية من الخدمة ووضعها في قائمة الصفوف.
        private async Task LoadClassesAsync()
        {
            try
            {
                DataTable classes =
                    await Task.Run(() => classService.GetAllClasses());

                cmbClass.DataSource = classes;
                cmbClass.DisplayMember = "ClassName";
                cmbClass.ValueMember = "ClassID";

                if (cmbClass.Items.Count > 0)
                {
                    cmbClass.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                cmbClass.DataSource = null;
                cmbClass.Items.Clear();

                cmbSubject.DataSource = null;
                cmbSubject.Items.Clear();
                cmbSubject.Enabled = false;

                UIHelper.ShowException(
                    "تعذر تحميل الصفوف. لا يمكن إدخال الدرجات قبل إصلاح اتصال قاعدة البيانات أو صلاحية الوصول.",
                    ex);
            }
        }

        // الحصول على رقم الصف المحدد من قائمة الصفوف بطريقة آمنة.
        private int GetClassId()
        {
            if (cmbClass.SelectedValue == null ||
                cmbClass.SelectedValue is DataRowView)
            {
                return 0;
            }

            if (int.TryParse(
                cmbClass.SelectedValue.ToString(),
                out int value))
            {
                return value;
            }

            return 0;
        }

        // الحصول على رقم المادة المحددة من قائمة المواد بطريقة آمنة.
        private int GetSubjectId()
        {
            if (cmbSubject.SelectedValue == null ||
                cmbSubject.SelectedValue is DataRowView)
            {
                return 0;
            }

            if (int.TryParse(
                cmbSubject.SelectedValue.ToString(),
                out int value))
            {
                return value;
            }

            return 0;
        }

        // إعادة تحميل المواد والشعب ومسح نتائج الدرجات عند تغيير الصف.
        private async void cmbClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoading)
                return;

            await LoadSubjectsForSelectedClassAsync();

            await LoadSectionsAsync();

            currentGradesTable = null;
            dataGridViewGrades.DataSource = null;
            lblRecordCount.Text = "عدد الطلاب: 0";
            selectedGradeId = 0;
            showIncompleteOnly = false;
            btnIncomplete.Text = "غير المكتملة";
        }

        // إعادة تحميل المواد ومسح الجدول عند تغيير الفصل الدراسي.
        private async void cmbTerm_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoading)
                return;

            await LoadSubjectsForSelectedClassAsync();

            dataGridViewGrades.DataSource = null;
            lblRecordCount.Text = "عدد الطلاب: 0";
        }

        // إعادة تحميل الشعب عند تغيير العام الدراسي.
        private async void cmbAcademicYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isLoading)
            {
                await LoadSectionsAsync();
            }
        }

        // تحميل الشعب الفعلية الخاصة بالصف والعام الدراسي المحددين.
        private async Task LoadSectionsAsync()
        {
            try
            {
                int classId = GetClassId();

                string academicYear =
                    cmbAcademicYear.Text == null
                        ? string.Empty
                        : cmbAcademicYear.Text.Trim();

                cmbSection.DataSource = null;
                cmbSection.Items.Clear();
                cmbSection.Enabled = false;

                if (classId <= 0 ||
                    !IsValidAcademicYear(academicYear))
                {
                    return;
                }

                DataTable sections =
                    await Task.Run(() =>
                        sectionService.GetSections(
                            classId,
                            academicYear));

                DataTable choices = new DataTable();
                choices.Columns.Add(
                    "Section",
                    typeof(string));

                var sectionNames =
                    new System.Collections.Generic.HashSet<string>(
                        StringComparer.OrdinalIgnoreCase);

                if (sections != null &&
                    sections.Columns.Contains("Section"))
                {
                    foreach (DataRow row in sections.Rows)
                    {
                        string sectionName =
                            row["Section"] == DBNull.Value
                                ? string.Empty
                                : Convert.ToString(row["Section"]);

                        sectionName =
                            (sectionName ?? string.Empty).Trim();

                        if (!string.IsNullOrWhiteSpace(sectionName) &&
                            sectionNames.Add(sectionName))
                        {
                            choices.Rows.Add(sectionName);
                        }
                    }
                }

                cmbSection.DataSource = choices;
                cmbSection.DisplayMember = "Section";
                cmbSection.ValueMember = "Section";
                cmbSection.Enabled = choices.Rows.Count > 0;

                if (cmbSection.Items.Count > 0)
                {
                    cmbSection.SelectedIndex = 0;
                }
                else
                {
                    cmbSection.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                cmbSection.DataSource = null;
                cmbSection.Items.Clear();
                cmbSection.Enabled = false;

                UIHelper.ShowException(
                    "تعذر تحميل الشعب الفعلية للصف والعام الدراسي المحددين:",
                    ex);
            }
        }

        // تحميل المواد المرتبطة بالصف والفصل الدراسي المحددين.
        private async Task LoadSubjectsForSelectedClassAsync()
        {
            try
            {
                int classId = GetClassId();

                if (classId <= 0)
                    return;

                if (cmbTerm.SelectedIndex < 0 ||
                    string.IsNullOrWhiteSpace(cmbTerm.Text))
                {
                    cmbSubject.DataSource = null;
                    cmbSubject.Items.Clear();
                    cmbSubject.Enabled = false;

                    return;
                }
                string selectedTerm = cmbTerm.Text;

                DataTable subjects =
                    await Task.Run(() =>
                        gradeService.GetSubjectsByClass(
                            classId,
                            selectedTerm));

                if (subjects != null &&
                    subjects.Rows.Count > 0)
                {
                    DataRow allRow = subjects.NewRow();

                    allRow["SubjectID"] = 0;
                    allRow["SubjectName"] = "كل المواد";

                    subjects.Rows.InsertAt(allRow, 0);
                }

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

                    ShowWarning(
                        "لا توجد مواد مفعلة لهذا الصف والترم. يرجى مراجعة واجهة إدارة المواد.");
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowException(
                    "تحميل مواد الصف",
                    ex);
            }
        }

        // تحميل الطلاب والدرجات وفق المرشحات المحددة حاليًا.
        private async void btnLoad_Click(object sender, EventArgs e)
        {
            await LoadGradesAsync();
        }

        // تحديث المواد والدرجات وإعادة تحميل البيانات الحالية.
        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await LoadSubjectsForSelectedClassAsync();

            await LoadGradesAsync();
        }

        // التحقق من صحة صيغة العام الدراسي وأن العام الثاني يساوي الأول زائد سنة.
        private bool IsValidAcademicYear(string academicYear)
        {
            if (string.IsNullOrWhiteSpace(academicYear))
                return false;

            string[] parts =
                academicYear
                    .Trim()
                    .Replace('-', '/')
                    .Split('/');

            if (parts.Length != 2 ||
                parts[0].Length != 4 ||
                parts[1].Length != 4)
            {
                return false;
            }

            if (!int.TryParse(parts[0], out int firstYear) ||
                !int.TryParse(parts[1], out int secondYear))
            {
                return false;
            }

            return secondYear == firstYear + 1;
        }

        // التحقق من اكتمال وصحة جميع مرشحات تحميل الدرجات.
        private bool ValidateGradeFilters()
        {
            if (GetClassId() <= 0)
            {
                ShowWarning("يرجى اختيار الصف.");
                return false;
            }

            if (GetSubjectId() < 0)
            {
                ShowWarning("اختيار المادة غير صحيح.");
                return false;
            }

            if (cmbSection.SelectedIndex < 0 ||
                string.IsNullOrWhiteSpace(cmbSection.Text))
            {
                ShowWarning("يرجى اختيار الشعبة.");
                cmbSection.Focus();
                return false;
            }

            if (cmbTerm.SelectedIndex < 0 ||
                string.IsNullOrWhiteSpace(cmbTerm.Text))
            {
                ShowWarning("يرجى اختيار الفصل الدراسي.");
                cmbTerm.Focus();
                return false;
            }

            if (!IsValidAcademicYear(cmbAcademicYear.Text))
            {
                ShowWarning(
                    "أدخل العام الدراسي بالصيغة الصحيحة: 2025/2026.");
                cmbAcademicYear.Focus();
                return false;
            }

            return true;
        }

        // تحميل قائمة الطلاب والدرجات حسب الصف والشعبة والعام والمادة والفصل.
        private async Task LoadGradesAsync()
        {
            try
            {
                if (!ValidateGradeFilters())
                    return;

                int classId = GetClassId();
                int subjectId = GetSubjectId();

                string section = cmbSection.Text;
                string academicYear = cmbAcademicYear.Text.Trim();
                string term = cmbTerm.Text;

                Cursor = Cursors.WaitCursor;

                currentGradesTable =
                    await Task.Run(() =>
                        gradeService.GetGradeEntryStudents(
                            classId,
                            section,
                            academicYear,
                            subjectId,
                            term));

                showIncompleteOnly = false;
                btnIncomplete.Text = "غير المكتملة";

                dataGridViewGrades.DataSource =
                    currentGradesTable;

                FormatGrid();
                ApplyGridFilter();

                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;

                UIHelper.ShowException(
                    "تحميل الطلاب والدرجات",
                    ex);
            }
        }

        // ضبط أسماء أعمدة جدول الدرجات وإخفاء الحقول الداخلية وجعل بعض الأعمدة للقراءة فقط.
        private void FormatGrid()
        {
            if (dataGridViewGrades.Columns.Count == 0)
                return;

            SetColumnHeader("StudentNumber", "الرقم الأكاديمي");
            SetColumnHeader("StudentName", "اسم الطالب");
            SetColumnHeader("Gender", "الجنس");
            SetColumnHeader("TermName", "الترم الدراسي");
            SetColumnHeader("SubjectName", "المادة");

            SetColumnHeader("Quiz1", "اختبار 1");
            SetColumnHeader("Quiz2", "اختبار 2");
            SetColumnHeader("CourseWork", "أعمال السنة");
            SetColumnHeader("FinalExam", "الاختبار النهائي");

            SetColumnHeader("Total", "مجموع المادة");
            SetColumnHeader("SubjectAverage", "معدل المادة %");
            SetColumnHeader("GradeLetter", "تقدير المادة");
            SetColumnHeader("ResultStatus", "الحالة");

            SetColumnHeader("StudentTotal", "مجموع الطالب في الترم");
            SetColumnHeader("StudentMaxTotal", "إجمالي الدرجات");
            SetColumnHeader("StudentAverage", "المعدل الكلي %");
            SetColumnHeader("StudentGradeLetter", "التقدير العام");
            SetColumnHeader("StudentResultStatus", "حالة الطالب");

            SetColumnHeader("Notes", "ملاحظات");

            HideColumn("StudentID");
            HideColumn("GradeID");

            MakeReadOnly("StudentNumber");
            MakeReadOnly("StudentName");
            MakeReadOnly("Gender");
            MakeReadOnly("TermName");
            MakeReadOnly("SubjectName");
            MakeReadOnly("Total");
            MakeReadOnly("SubjectAverage");
            MakeReadOnly("StudentTotal");
            MakeReadOnly("StudentMaxTotal");
            MakeReadOnly("StudentAverage");
            MakeReadOnly("StudentGradeLetter");
            MakeReadOnly("StudentResultStatus");
            MakeReadOnly("GradeLetter");
            MakeReadOnly("ResultStatus");

            dataGridViewGrades.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
        }

        // تغيير عنوان عمود داخل جدول الدرجات إذا كان العمود موجودًا.
        private void SetColumnHeader(string columnName, string header)
        {
            if (dataGridViewGrades.Columns.Contains(columnName))
            {
                dataGridViewGrades.Columns[columnName].HeaderText = header;
            }
        }

        // إخفاء عمود داخلي من جدول الدرجات إذا كان موجودًا.
        private void HideColumn(string columnName)
        {
            if (dataGridViewGrades.Columns.Contains(columnName))
            {
                dataGridViewGrades.Columns[columnName].Visible = false;
            }
        }

        // جعل عمود محدد للقراءة فقط لمنع تعديل القيم المحسوبة أو التعريفية.
        private void MakeReadOnly(string columnName)
        {
            if (dataGridViewGrades.Columns.Contains(columnName))
            {
                dataGridViewGrades.Columns[columnName].ReadOnly = true;
            }
        }

        // التحقق من صحة درجات الاختبارات والملاحظات قبل قبول تحرير الخلية.
        private void dataGridViewGrades_CellValidating(
            object sender,
            DataGridViewCellValidatingEventArgs e)
        {
            if (e.RowIndex < 0 ||
                e.ColumnIndex < 0 ||
                dataGridViewGrades.Rows[e.RowIndex].IsNewRow)
            {
                return;
            }

            string columnName =
                dataGridViewGrades
                    .Columns[e.ColumnIndex]
                    .Name;

            if (columnName == "Quiz1" ||
                columnName == "Quiz2" ||
                columnName == "CourseWork" ||
                columnName == "FinalExam")
            {
                string value =
                    e.FormattedValue == null
                        ? string.Empty
                        : e.FormattedValue
                            .ToString()
                            .Trim();

                if (string.IsNullOrWhiteSpace(value))
                    return;

                if (!UIHelper.TryParseDecimal(
                        value,
                        out decimal grade) ||
                    grade < 0 ||
                    grade > 100)
                {
                    e.Cancel = true;

                    ShowWarning(
                        "أدخل درجة رقمية بين 0 و100.");

                    return;
                }

                decimal total = grade;

                string[] gradeColumns =
                {
                    "Quiz1",
                    "Quiz2",
                    "CourseWork",
                    "FinalExam"
                };

                foreach (string gradeColumn in gradeColumns)
                {
                    if (gradeColumn == columnName)
                        continue;

                    object cellValue =
                        dataGridViewGrades
                            .Rows[e.RowIndex]
                            .Cells[gradeColumn]
                            .Value;

                    if (cellValue == null ||
                        cellValue == DBNull.Value ||
                        string.IsNullOrWhiteSpace(
                            cellValue.ToString()))
                    {
                        continue;
                    }

                    if (!UIHelper.TryParseDecimal(
                            cellValue.ToString(),
                            out decimal existingGrade) ||
                        existingGrade < 0 ||
                        existingGrade > 100)
                    {
                        e.Cancel = true;

                        ShowWarning(
                            "توجد درجة أخرى غير صالحة في الصف نفسه.");

                        return;
                    }

                    total += existingGrade;
                }

                if (total > 100)
                {
                    e.Cancel = true;

                    ShowWarning(
                        "مجموع درجات الطالب لا يمكن أن يتجاوز 100.");
                }
            }
            else if (columnName == "Notes")
            {
                string notes =
                    e.FormattedValue == null
                        ? string.Empty
                        : e.FormattedValue.ToString();

                if (notes.Length > 1000)
                {
                    e.Cancel = true;

                    ShowWarning(
                        "ملاحظات الدرجة لا يمكن أن تتجاوز 1000 حرف.");
                }
            }
        }

        // إعادة حساب نتيجة الصف بعد انتهاء تعديل إحدى درجات الطالب.
        private void dataGridViewGrades_CellEndEdit(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            RecalculateRow(e.RowIndex);
        }

        // إعادة حساب مجموع الدرجات والتقدير والحالة للطالب في الصف المحدد.
        private void RecalculateRow(int rowIndex)
        {
            DataGridViewRow row =
                dataGridViewGrades.Rows[rowIndex];

            decimal q1 = GetDecimal(row, "Quiz1");
            decimal q2 = GetDecimal(row, "Quiz2");
            decimal cw = GetDecimal(row, "CourseWork");
            decimal final = GetDecimal(row, "FinalExam");

            decimal total =
                q1 + q2 + cw + final;

            if (q1 < 0 ||
                q2 < 0 ||
                cw < 0 ||
                final < 0)
            {
                ShowWarning(
                    "لا يمكن إدخال درجة سالبة.");

                return;
            }

            if (total > 100)
            {
                ShowWarning(
                    "المجموع لا يمكن أن يتجاوز 100.");

                return;
            }

            if (dataGridViewGrades.Columns.Contains("Total"))
            {
                row.Cells["Total"].Value = total;
            }

            if (dataGridViewGrades.Columns.Contains("GradeLetter"))
            {
                row.Cells["GradeLetter"].Value =
                    GetGradeLetter(total);
            }

            if (dataGridViewGrades.Columns.Contains("ResultStatus"))
            {
                row.Cells["ResultStatus"].Value =
                    total >= 50
                        ? "ناجح"
                        : "راسب";
            }
        }

        // محاولة قراءة درجة من خلية محددة مع السماح بالخلايا الفارغة.
        private bool TryGetGrade(
            DataGridViewRow row,
            string columnName,
            out decimal result)
        {
            result = 0;

            if (!dataGridViewGrades.Columns.Contains(
                    columnName))
            {
                return true;
            }

            object value =
                row.Cells[columnName].Value;

            if (value == null ||
                value == DBNull.Value ||
                string.IsNullOrWhiteSpace(
                    value.ToString()))
            {
                return true;
            }

            return UIHelper.TryParseDecimal(
                value.ToString(),
                out result);
        }

        // قراءة قيمة عشرية من خلية في جدول الدرجات مع إرجاع الصفر عند عدم وجود قيمة صحيحة.
        private decimal GetDecimal(
            DataGridViewRow row,
            string columnName)
        {
            if (!dataGridViewGrades.Columns.Contains(
                    columnName))
            {
                return 0;
            }

            object value =
                row.Cells[columnName].Value;

            if (value == null ||
                value == DBNull.Value)
            {
                return 0;
            }

            return UIHelper.TryParseDecimal(
                value.ToString(),
                out decimal result)
                ? result
                : 0m;
        }

        // تحويل المجموع الرقمي للدرجة إلى التقدير النصي المعتمد في النظام.
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

        // التحقق من جميع الصفوف ثم حفظ درجات الطلاب في قاعدة البيانات.
        private async void btnSaveAll_Click(
            object sender,
            EventArgs e)
        {
            if (dataGridViewGrades.Rows.Count == 0)
            {
                ShowWarning(
                    "لا توجد بيانات للحفظ.");

                return;
            }

            if (!ValidateGradeFilters())
                return;

            int classId = GetClassId();

            try
            {
                foreach (DataGridViewRow validationRow
                         in dataGridViewGrades.Rows)
                {
                    if (validationRow.IsNewRow)
                        continue;

                    decimal q1;
                    decimal q2;
                    decimal cw;
                    decimal final;

                    if (!TryGetGrade(
                            validationRow,
                            "Quiz1",
                            out q1) ||
                        !TryGetGrade(
                            validationRow,
                            "Quiz2",
                            out q2) ||
                        !TryGetGrade(
                            validationRow,
                            "CourseWork",
                            out cw) ||
                        !TryGetGrade(
                            validationRow,
                            "FinalExam",
                            out final))
                    {
                        ShowWarning(
                            "توجد درجة غير صالحة. أدخل أرقامًا فقط بين 0 و100.");

                        return;
                    }

                    if (q1 < 0 ||
                        q2 < 0 ||
                        cw < 0 ||
                        final < 0 ||
                        q1 + q2 + cw + final > 100)
                    {
                        ShowWarning(
                            "يجب أن تكون الدرجات غير سالبة وأن لا يتجاوز مجموع الطالب 100.");

                        return;
                    }
                }

                Cursor = Cursors.WaitCursor;

                int success = 0;

                foreach (DataGridViewRow row
                         in dataGridViewGrades.Rows)
                {
                    if (row.IsNewRow)
                        continue;

                    if (!TryGetRowInt(
                            row,
                            "StudentID",
                            out int studentId) ||
                        studentId <= 0)
                    {
                        ShowWarning(
                            "تعذر تحديد طالب صحيح في قائمة الدرجات؛ أعد تحميل البيانات ثم حاول مرة أخرى.");

                        return;
                    }

                    if (!TryGetRowInt(
                            row,
                            "SubjectID",
                            out int rowSubjectId) ||
                        rowSubjectId <= 0)
                    {
                        ShowWarning(
                            "تعذر تحديد المادة في أحد الصفوف؛ أعد تحميل البيانات ثم حاول مرة أخرى.");

                        return;
                    }

                    StudentGrade grade =
                        new StudentGrade();

                    grade.StudentID = studentId;
                    grade.SubjectID = rowSubjectId;
                    grade.ClassID = classId;
                    grade.Section = cmbSection.Text;
                    grade.AcademicYear =
                        cmbAcademicYear.Text.Trim();

                    grade.TermName = cmbTerm.Text;

                    grade.Quiz1 =
                        GetDecimal(row, "Quiz1");

                    grade.Quiz2 =
                        GetDecimal(row, "Quiz2");

                    grade.CourseWork =
                        GetDecimal(row, "CourseWork");

                    grade.FinalExam =
                        GetDecimal(row, "FinalExam");

                    if (row.Cells["Notes"].Value == null ||
                        row.Cells["Notes"].Value == DBNull.Value)
                    {
                        grade.Notes = "";
                    }
                    else
                    {
                        grade.Notes =
                            row.Cells["Notes"]
                                .Value
                                .ToString()
                                .Trim();
                    }

                    if (grade.Notes.Length > 1000)
                    {
                        ShowWarning(
                            "ملاحظات الدرجة لا يمكن أن تتجاوز 1000 حرف.");

                        return;
                    }

                    bool saved =
                        await Task.Run(() =>
                            gradeService.SaveGrade(grade));

                    if (saved)
                        success++;
                }

                Cursor = Cursors.Default;

                ShowInfo(
                    "تم حفظ درجات " +
                    success +
                    " طالب بنجاح.");

                await LoadGradesAsync();
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;

                UIHelper.ShowException(
                    "حفظ الدرجات",
                    ex);
            }
        }

        // تحديد الدرجة المحفوظة من الصف الذي تم النقر عليه في جدول الدرجات.
        private void dataGridViewGrades_CellClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (!dataGridViewGrades.Columns.Contains(
                    "GradeID"))
            {
                return;
            }

            object value =
                dataGridViewGrades
                    .Rows[e.RowIndex]
                    .Cells["GradeID"]
                    .Value;

            selectedGradeId =
                value != null &&
                value != DBNull.Value &&
                int.TryParse(
                    value.ToString(),
                    out int gradeId)
                    ? gradeId
                    : 0;
        }

        // قراءة قيمة رقمية صحيحة من خلية محددة مع التحقق من وجود العمود والقيمة.
        private bool TryGetRowInt(
            DataGridViewRow row,
            string columnName,
            out int value)
        {
            value = 0;

            if (row == null ||
                !dataGridViewGrades.Columns.Contains(
                    columnName))
            {
                return false;
            }

            object cellValue =
                row.Cells[columnName].Value;

            return cellValue != null &&
                   cellValue != DBNull.Value &&
                   int.TryParse(
                       cellValue.ToString(),
                       out value);
        }

        // حذف الدرجة المحددة بعد التأكيد ثم إعادة تحميل الجدول.
        private async void btnDeleteGrade_Click(
            object sender,
            EventArgs e)
        {
            if (selectedGradeId <= 0)
            {
                ShowWarning(
                    "اختر درجة محفوظة من الجدول أولاً.");

                return;
            }

            DialogResult result =
                MessageBox.Show(
                    "هل تريد حذف درجة الطالب المحددة؟",
                    "تأكيد الحذف",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2,
                    MessageBoxOptions.RightAlign |
                    MessageBoxOptions.RtlReading);

            if (result != DialogResult.Yes)
                return;

            try
            {
                bool deleted =
                    await Task.Run(() =>
                        gradeService.DeleteGrade(
                            selectedGradeId));

                if (deleted)
                {
                    ShowInfo(
                        "تم حذف الدرجة بنجاح.");

                    selectedGradeId = 0;

                    await LoadGradesAsync();
                }
                else
                {
                    ShowWarning(
                        "لم يتم العثور على الدرجة.");
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowException(
                    "حذف الدرجة",
                    ex);
            }
        }

        // تفريغ الجدول وإلغاء الفلاتر وحالة الدرجة المحددة.
        private void btnClear_Click(
            object sender,
            EventArgs e)
        {
            currentGradesTable = null;

            dataGridViewGrades.DataSource = null;

            lblRecordCount.Text =
                "عدد الطلاب: 0";

            selectedGradeId = 0;

            showIncompleteOnly = false;

            btnIncomplete.Text =
                "غير المكتملة";
        }

        // إعادة تطبيق فلتر الجدول تلقائيًا عند تغيير نص البحث.
        private void txtSearch_TextChanged(
            object sender,
            EventArgs e)
        {
            ApplyGridFilter();
        }

        // تطبيق البحث وخيار عرض الدرجات غير المكتملة على بيانات الجدول الحالية.
        private void ApplyGridFilter()
        {
            if (currentGradesTable == null)
                return;

            string filter = string.Empty;

            string keyword =
                txtSearch.Text.Trim();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                filter =
                    UIHelper.BuildDataViewSearchFilter(
                        keyword,
                        "StudentName",
                        "StudentNumber");
            }

            if (showIncompleteOnly)
            {
                if (filter.Length > 0)
                    filter += " AND ";

                filter +=
                    "(GradeID = 0 OR " +
                    "Quiz1 = 0 OR " +
                    "Quiz2 = 0 OR " +
                    "CourseWork = 0 OR " +
                    "FinalExam = 0)";
            }

            currentGradesTable
                .DefaultView
                .RowFilter = filter;

            lblRecordCount.Text =
                showIncompleteOnly
                    ? "الطلاب غير المكتملة درجاتهم: " +
                      currentGradesTable.DefaultView.Count
                    : "عدد الطلاب: " +
                      currentGradesTable.DefaultView.Count;
        }

        // تبديل عرض الجدول بين جميع الطلاب والطلاب غير المكتملة درجاتهم.
        private void btnIncomplete_Click(
            object sender,
            EventArgs e)
        {
            if (currentGradesTable == null)
            {
                ShowWarning(
                    "حمّل الطلاب أولاً قبل عرض غير المكتملة.");

                return;
            }

            showIncompleteOnly =
                !showIncompleteOnly;

            btnIncomplete.Text =
                showIncompleteOnly
                    ? "عرض الكل"
                    : "غير المكتملة";

            ApplyGridFilter();
        }

        // بدء تصدير بيانات الدرجات إلى ملف Excel.
        private void btnExportExcel_Click(
            object sender,
            EventArgs e)
        {
            ExportGrades(false);
        }

        // بدء تصدير بيانات الدرجات إلى ملف PDF.
        private void btnExportPdf_Click(
            object sender,
            EventArgs e)
        {
            ExportGrades(true);
        }

        // تنفيذ عملية التحقق والصلاحيات وإنشاء ملف التصدير المطلوب.
        private void ExportGrades(bool pdf)
        {
            try
            {
                CurrentUser.DemandAction(
                    "Grades",
                    pdf ? "ExportPDF" : "ExportExcel",
                    pdf
                        ? "ليس لديك صلاحية تصدير تقارير الدرجات إلى PDF."
                        : "ليس لديك صلاحية تصدير تقارير الدرجات إلى Excel.");
            }
            catch (UnauthorizedAccessException ex)
            {
                ApplicationLogger.LogWarning("شاشة إدخال الدرجات: تصدير الدرجات", ex.Message);
                ShowWarning(ex.Message);
                return;
            }

            if (currentGradesTable == null ||
                currentGradesTable.DefaultView.Count == 0)
            {
                ShowWarning(
                    "لا توجد درجات ظاهرة للتصدير.");

                return;
            }

            using (SaveFileDialog dialog =
                   new SaveFileDialog())
            {
                dialog.Filter =
                    pdf
                        ? "PDF files (*.pdf)|*.pdf"
                        : "Excel files (*.xlsx)|*.xlsx";

                dialog.FileName =
                    (showIncompleteOnly
                        ? "الدرجات_غير_المكتملة_"
                        : "تقرير_الدرجات_") +
                    DateTime.Now.ToString(
                        "yyyyMMdd_HHmm") +
                    (pdf ? ".pdf" : ".xlsx");

                if (dialog.ShowDialog() !=
                    DialogResult.OK)
                {
                    return;
                }

                try
                {
                    DataTable exportTable =
                        BuildExportTable();

                    string title =
                        showIncompleteOnly
                            ? "تقرير الطلاب غير المكتملة درجاتهم"
                            : "تقرير درجات الطلاب";

                    string summary =
                        "العام: " +
                        cmbAcademicYear.Text.Trim() +
                        " | الصف: " +
                        cmbClass.Text +
                        " | الشعبة: " +
                        cmbSection.Text +
                        " | المادة: " +
                        cmbSubject.Text +
                        " | الفصل: " +
                        cmbTerm.Text;

                    if (pdf)
                    {
                        ReportOutputHelper.ExportToPdf(
                            exportTable,
                            dialog.FileName,
                            title,
                            summary);
                    }
                    else
                    {
                        ReportOutputHelper.ExportToExcel(
                            exportTable,
                            dialog.FileName,
                            title,
                            summary);
                    }

                    ShowInfo(
                        "تم تصدير التقرير بنجاح.");
                }
                catch (Exception ex)
                {
                    UIHelper.ShowException(
                        "تصدير تقرير الدرجات",
                        ex);
                }
            }
        }

        // بناء نسخة مستقلة من البيانات الظاهرة في الجدول لاستخدامها في التصدير.
        private DataTable BuildExportTable()
        {
            DataTable source =
                currentGradesTable
                    .DefaultView
                    .ToTable();

            DataTable result =
                new DataTable();

            string[] columns =
            {
                "StudentNumber",
                "StudentName",
                "Gender",
                "AcademicYear",
                "ClassName",
                "Section",
                "SubjectName",
                "TermName",
                "Quiz1",
                "Quiz2",
                "CourseWork",
                "FinalExam",
                "Total",
                "GradeLetter",
                "ResultStatus",
                "Notes"
            };

            string[] headers =
            {
                "الرقم الأكاديمي",
                "اسم الطالب",
                "الجنس",
                "العام الدراسي",
                "الصف",
                "الشعبة",
                "المادة",
                "الفصل الدراسي",
                "اختبار 1",
                "اختبار 2",
                "أعمال السنة",
                "الاختبار النهائي",
                "المجموع",
                "التقدير",
                "الحالة",
                "ملاحظات"
            };

            for (int i = 0;
                 i < columns.Length;
                 i++)
            {
                result.Columns.Add(
                    headers[i],
                    typeof(string));
            }

            foreach (DataRow row in source.Rows)
            {
                DataRow output =
                    result.NewRow();

                for (int i = 0;
                     i < columns.Length;
                     i++)
                {
                    output[i] =
                        source.Columns.Contains(
                            columns[i]) &&
                        row[columns[i]] != DBNull.Value
                            ? Convert.ToString(
                                row[columns[i]])
                            : string.Empty;
                }

                result.Rows.Add(output);
            }

            return result;
        }

        // عرض رسالة معلومات للمستخدم باستخدام إعدادات الرسائل العربية في النظام.
        private void ShowInfo(string message)
        {
            MessageBox.Show(
                message,
                "معلومة",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.RightAlign |
                MessageBoxOptions.RtlReading);
        }

        // عرض رسالة تحذيرية للمستخدم باستخدام إعدادات الرسائل العربية في النظام.
        private void ShowWarning(string message)
        {
            MessageBox.Show(
                message,
                "تنبيه",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.RightAlign |
                MessageBoxOptions.RtlReading);
        }

        // عرض رسالة خطأ للمستخدم باستخدام إعدادات الرسائل العربية في النظام.
        private void ShowError(string message)
        {
            MessageBox.Show(
                message,
                "خطأ",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.RightAlign |
                MessageBoxOptions.RtlReading);
        }
    }
}
