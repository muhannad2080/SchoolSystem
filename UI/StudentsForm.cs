using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using SchoolSystem.Models;
using SchoolSystem.Services;
using SchoolSystem.Helpers;
using SchoolSystem.Security;

namespace SchoolSystem.UI
{
    public partial class StudentsForm : Form
    {
        private readonly StudentService _studentService;

        private int _selectedStudentId;

        private byte[] _selectedPhoto;

        private List<Student> _currentStudents;

        private readonly PrintDocument _studentCardPrintDocument =
            new PrintDocument();

        private bool _isSaving;

        private enum FormMode { View, Add, Edit }
        private FormMode _formMode = FormMode.View;
        private Student _editingSnapshot;
        private List<Student> _filteredStudents = new List<Student>();
        private readonly ToolTip _toolTip = new ToolTip();

        // ================================================================
        // CONSTRUCTOR
        // ================================================================

        public StudentsForm()
        {
            InitializeComponent();

            UIHelper.ApplyStyle(this);

            txtStudentNumber.ReadOnly = true;
            txtStudentNumber.TabStop = false;

            SchoolSystem.Helpers.UIHelper.ApplyFieldValidation(this);

            dtpBirthDate.MaxDate = DateTime.Today;

            _studentService = new StudentService();
            _currentStudents = new List<Student>();

            txtSearch.TextChanged +=
                (sender, e) => ApplyFilters();

            cmbFilterClass.SelectedIndexChanged +=
                cmbFilterClass_SelectedIndexChanged;

            cmbFilterStatus.SelectedIndexChanged +=
                cmbFilterStatus_SelectedIndexChanged;

            dgvStudents.CellDoubleClick +=
                dgvStudents_CellDoubleClick;

            _studentCardPrintDocument.PrintPage +=
                StudentCardPrintDocument_PrintPage;

            ConfigureFieldConstraints();
            ConfigureTooltips();

            DataRefreshNotifier.DataChanged += DataRefreshNotifier_DataChanged;
            Disposed += (sender, e) =>
                DataRefreshNotifier.DataChanged -= DataRefreshNotifier_DataChanged;

            SetFormMode(FormMode.View);
        }

        private async void DataRefreshNotifier_DataChanged(
            object sender,
            DataRefreshEventArgs e)
        {
            if (ReferenceEquals(sender, this) ||
                IsDisposed ||
                _isSaving)
            {
                return;
            }

            if (!DataRefreshNotifier.IsRelated(e, "Students") &&
                !DataRefreshNotifier.IsRelated(e, "Enrollment") &&
                !DataRefreshNotifier.IsRelated(e, "ClassAssignment") &&
                !DataRefreshNotifier.IsRelated(e, "All"))
            {
                return;
            }

            LoadStudents();
        }

        private void LoadStudents()
        {
            try
            {
                _currentStudents =
                    _studentService.GetAll();

                PopulateClassFilter();

                ApplyFilters();
            }
            catch (Exception ex)
            {
                UIHelper.ShowException(
                    "خطأ في تحميل البيانات: ",
                    ex);
            }
        }

        // ================================================================
        // POPULATE CLASS FILTER
        // ================================================================

        private void PopulateClassFilter()
        {
            string selectedClass =
                cmbFilterClass.Text;

            var classNames =
                _currentStudents
                    .Select(s =>
                        s.CurrentClassName)
                    .Where(name =>
                        !string.IsNullOrWhiteSpace(
                            name))
                    .Distinct(
                        StringComparer.OrdinalIgnoreCase)
                    .OrderBy(
                        name => name)
                    .ToList();

            cmbFilterClass.Items.Clear();

            cmbFilterClass.Items.Add(
                "كل الصفوف");

            foreach (string className
                     in classNames)
            {
                cmbFilterClass.Items.Add(
                    className);
            }

            int selectedIndex =
                cmbFilterClass.Items.IndexOf(
                    selectedClass);

            cmbFilterClass.SelectedIndex =
                selectedIndex >= 0
                    ? selectedIndex
                    : 0;
        }

        // ================================================================
        // APPLY FILTERS
        // ================================================================

        private void ApplyFilters()
        {
            IEnumerable<Student> filtered =
                _currentStudents.AsEnumerable();

            string search =
                txtSearch.Text.Trim();

            if (!string.IsNullOrWhiteSpace(search))
            {
                string[] terms =
                    search.Split(
                        new[] { ' ' },
                        StringSplitOptions.RemoveEmptyEntries);

                filtered =
                    filtered.Where(
                        student =>
                            terms.All(
                                term =>
                                    ContainsStudentText(student.FullName, term) ||
                                    ContainsStudentText(student.StudentNumber, term) ||
                                    ContainsStudentText(student.NationalId, term) ||
                                    ContainsStudentText(student.StudentPhone, term) ||
                                    ContainsStudentText(student.GuardianName, term) ||
                                    ContainsStudentText(student.GuardianPhone, term) ||
                                    ContainsStudentText(student.CurrentClassName, term) ||
                                    ContainsStudentText(student.CurrentSection, term) ||
                                    ContainsStudentText(student.AcademicYear, term)));
            }

            if (cmbFilterStatus.SelectedIndex > 0)
            {
                filtered = filtered.Where(
                    s => string.Equals(
                        s.Status,
                        cmbFilterStatus.Text,
                        StringComparison.OrdinalIgnoreCase));
            }

            if (cmbFilterClass.SelectedIndex > 0 &&
                !string.IsNullOrWhiteSpace(cmbFilterClass.Text))
            {
                filtered = filtered.Where(
                    s => string.Equals(
                        s.CurrentClassName,
                        cmbFilterClass.Text,
                        StringComparison.OrdinalIgnoreCase));
            }

            BindGrid(filtered.ToList());
        }

        private static bool ContainsStudentText(
            string value,
            string term)
        {
            return !string.IsNullOrEmpty(value) &&
                   !string.IsNullOrEmpty(term) &&
                   value.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        // ================================================================
        // BIND GRID
        // ================================================================

        private void BindGrid(
            List<Student> students)
        {
            _filteredStudents =
                students == null
                    ? new List<Student>()
                    : students.ToList();

            if (_selectedStudentId > 0 &&
                !_filteredStudents.Any(
                    s => s.StudentId == _selectedStudentId))
            {
                _selectedStudentId = 0;
                _editingSnapshot = null;
            }

            dgvStudents.DataSource =
                students.Select(s => new
                {
                    s.StudentId,
                    s.StudentNumber,
                    s.FullName,
                    s.Gender,
                    s.CurrentClassName,
                    s.CurrentSection,
                    s.AcademicYear,
                    s.StudentPhone,
                    s.GuardianName,
                    s.Status
                }).ToList();

            ConfigureStudentsGridColumns();

            dgvStudents.Columns[
                "StudentNumber"].HeaderText =
                "رقم الطالب";

            dgvStudents.Columns[
                "FullName"].HeaderText =
                "الاسم";

            dgvStudents.Columns[
                "Gender"].HeaderText =
                "الجنس";

            dgvStudents.Columns[
                "CurrentClassName"].HeaderText =
                "الصف";

            dgvStudents.Columns[
                "CurrentSection"].HeaderText =
                "الشعبة";

            dgvStudents.Columns[
                "AcademicYear"].HeaderText =
                "العام الدراسي";

            dgvStudents.Columns[
                "StudentPhone"].HeaderText =
                "الهاتف";

            dgvStudents.Columns[
                "GuardianName"].HeaderText =
                "ولي الأمر";

            dgvStudents.Columns[
                "Status"].HeaderText =
                "الحالة";

            lblCount.Text =
                students.Count == _currentStudents.Count
                    ? $"إجمالي الطلاب: {students.Count}"
                    : $"النتائج: {students.Count} من إجمالي {_currentStudents.Count}";

            if (_formMode == FormMode.View)
                SetFormMode(FormMode.View);
        }

        private void ConfigureStudentsGridColumns()
        {
            if (dgvStudents.Columns["StudentId"] != null)
                dgvStudents.Columns["StudentId"].Visible = false;

            if (dgvStudents.Columns["AcademicYear"] != null)
                dgvStudents.Columns["AcademicYear"].Visible = false;

            if (dgvStudents.Columns["GuardianName"] != null)
                dgvStudents.Columns["GuardianName"].Visible = false;

            SetGridColumn("StudentNumber", 1.0F, 72);
            SetGridColumn("FullName", 2.35F, 130);
            SetGridColumn("Gender", 0.8F, 62);
            SetGridColumn("CurrentClassName", 1.3F, 80);
            SetGridColumn("CurrentSection", 1.0F, 68);
            SetGridColumn("StudentPhone", 1.2F, 88);
            SetGridColumn("Status", 0.9F, 72);

            if (dgvStudents.Columns["FullName"] != null)
                dgvStudents.Columns["FullName"].DefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleRight;

            foreach (DataGridViewColumn column in dgvStudents.Columns)
            {
                if (!column.Visible)
                    continue;

                column.SortMode = DataGridViewColumnSortMode.Automatic;
                column.HeaderCell.Style.Alignment =
                    DataGridViewContentAlignment.MiddleCenter;
            }
        }

        private void SetGridColumn(
            string columnName,
            float fillWeight,
            int minimumWidth)
        {
            DataGridViewColumn column = dgvStudents.Columns[columnName];

            if (column == null)
                return;

            column.Visible = true;
            column.FillWeight = fillWeight;
            column.MinimumWidth = minimumWidth;
        }

        // ================================================================
        // SAVE
        // ================================================================

        private void btnSave_Click(
            object sender,
            EventArgs e)
        {
            if (_isSaving)
                return;

            if (_formMode != FormMode.Add &&
                _formMode != FormMode.Edit)
            {
                UIHelper.ShowWarning(
                    "استخدم «إضافة طالب» لإنشاء سجل جديد أو «تعديل البيانات» لتعديل السجل المحدد.");
                return;
            }

            if (_formMode == FormMode.Add &&
                !CurrentUser.CanAdd("Students"))
            {
                UIHelper.ShowWarning("ليس لديك صلاحية إضافة الطلاب.");
                return;
            }

            if (_formMode == FormMode.Edit &&
                !CurrentUser.CanEdit("Students"))
            {
                UIHelper.ShowWarning("ليس لديك صلاحية تعديل الطلاب.");
                return;
            }

            _isSaving = true;
            btnSave.Enabled = false;

            try
            {
                errorProvider1.Clear();

                Student student = GetStudentFromForm();
                bool isAdd = _formMode == FormMode.Add;

                int savedStudentId;

                if (isAdd)
                    savedStudentId = _studentService.Add(student);
                else
                {
                    _studentService.Update(student);
                    savedStudentId = student.StudentId;
                }

                UIHelper.ShowInfo(
                    isAdd
                        ? "تمت إضافة الطالب بنجاح. تم تثبيت رقم الطالب تلقائيًا عند الحفظ."
                        : "تم حفظ تعديلات الطالب بنجاح.");

                LoadStudents();
                SelectStudentInGrid(savedStudentId);
                SetFormMode(FormMode.View);
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("العملية المطلوبة", ex);
            }
            finally
            {
                _isSaving = false;
                SetFormMode(_formMode);
            }
        }

        private void SelectStudentInGrid(int studentId)
        {
            if (studentId <= 0 || dgvStudents == null)
                return;

            for (int i = 0; i < dgvStudents.Rows.Count; i++)
            {
                DataGridViewRow row = dgvStudents.Rows[i];
                DataGridViewCell idCell = row.Cells["StudentId"];

                if (idCell == null || idCell.Value == null)
                    continue;

                int rowId;
                if (!int.TryParse(idCell.Value.ToString(), out rowId) ||
                    rowId != studentId)
                {
                    continue;
                }

                dgvStudents.ClearSelection();

                DataGridViewColumn firstVisibleColumn =
                    dgvStudents.Columns.Cast<DataGridViewColumn>()
                        .FirstOrDefault(column =>
                            column.Visible && column.Name != "StudentId");

                if (firstVisibleColumn != null)
                    dgvStudents.CurrentCell =
                        row.Cells[firstVisibleColumn.Index];

                row.Selected = true;

                Student student =
                    _currentStudents.FirstOrDefault(
                        s => s.StudentId == studentId);

                if (student != null)
                    FillForm(student);

                return;
            }
        }

        // ================================================================
        // GET STUDENT FROM FORM
        // ================================================================

        private Student GetStudentFromForm()
        {
            errorProvider1.Clear();

            string fullName =
                txtFullName.Text.Trim();

            if (!UIHelper.IsValidArabicOrLatinName(
                    fullName))
            {
                throw ValidationError(
                    txtFullName,
                    "اسم الطالب مطلوب ويجب أن يحتوي على أحرف فقط وبطول مناسب.");
            }

            string[] nameParts =
                fullName.Split(
                    new[] { ' ' },
                    StringSplitOptions.RemoveEmptyEntries);

            if (nameParts.Length < 4)
            {
                throw ValidationError(
                    txtFullName,
                    "يرجى إدخال الاسم الرباعي للطالب (أربعة أجزاء على الأقل).");
            }

            if (cmbGender.SelectedIndex < 0 ||
                string.IsNullOrWhiteSpace(
                    cmbGender.Text))
            {
                throw ValidationError(
                    cmbGender,
                    "يرجى اختيار جنس الطالب.");
            }

            if (cmbStatus.SelectedIndex < 0 ||
                string.IsNullOrWhiteSpace(
                    cmbStatus.Text))
            {
                throw ValidationError(
                    cmbStatus,
                    "يرجى اختيار حالة الطالب.");
            }

            string[] textValues =
            {
                txtBirthPlace.Text,
                txtNationality.Text,
                txtGovernorate.Text,
                txtDistrict.Text,
                txtGuardianName.Text
            };

            foreach (string value
                     in textValues)
            {
                if (!string.IsNullOrWhiteSpace(value) &&
                    !UIHelper.IsValidArabicOrLatinName(
                        value,
                        2))
                {
                    throw new Exception(
                        "الحقول النصية لا يجب أن تحتوي على أرقام أو رموز غير صالحة.");
                }
            }

            string nationalId =
                txtNationalId.Text.Trim();

            if (!string.IsNullOrEmpty(
                    nationalId) &&
                !UIHelper.IsValidNumericIdentifier(
                    nationalId,
                    6,
                    20))
            {
                throw new Exception(
                    "رقم الهوية يجب أن يتكون من أرقام فقط وبطول من 6 إلى 20 رقماً.");
            }

            string phone =
                txtPhone.Text.Trim();

            if (!string.IsNullOrEmpty(phone) &&
                !UIHelper.IsValidPhone(phone))
            {
                throw new Exception(
                    "رقم هاتف الطالب غير صحيح.");
            }

            string guardianPhone =
                txtGuardianPhone.Text.Trim();

            if (!string.IsNullOrEmpty(
                    guardianPhone) &&
                !UIHelper.IsValidPhone(
                    guardianPhone))
            {
                throw new Exception(
                    "رقم هاتف ولي الأمر غير صحيح.");
            }

            string guardianEmail =
                txtGuardianEmail.Text.Trim();

            if (!string.IsNullOrEmpty(
                    guardianEmail) &&
                !UIHelper.IsValidEmail(
                    guardianEmail))
            {
                throw new Exception(
                    "البريد الإلكتروني لولي الأمر غير صحيح.");
            }

            if (dtpBirthDate.Value.Date >
                DateTime.Today)
            {
                throw ValidationError(
                    dtpBirthDate,
                    "تاريخ الميلاد لا يمكن أن يكون في المستقبل.");
            }

            string guardianRelation =
                txtGuardianRelation.Text.Trim();
            if (guardianRelation.Length > 100 ||
                (guardianRelation.Length > 0 &&
                 !UIHelper.IsValidArabicOrLatinName(guardianRelation, 2)))
            {
                throw new Exception(
                    "صلة القرابة غير صالحة (أحرف فقط وبطول مناسب).");
            }

            string guardianJob =
                txtGuardianJob.Text.Trim();
            if (guardianJob.Length > 100 ||
                (guardianJob.Length > 0 &&
                 !UIHelper.IsValidArabicOrLatinName(guardianJob, 2)))
            {
                throw new Exception(
                    "مهنة ولي الأمر غير صالحة (أحرف فقط وبطول مناسب).");
            }

            string address =
                txtAddress.Text.Trim();
            if (address.Length > 500)
            {
                throw new Exception(
                    "العنوان طويل جداً (الحد الأقصى 500 حرف).");
            }

            return new Student
            {
                StudentId =
                    _selectedStudentId,

                FullName =
                    fullName,

                Gender =
                    cmbGender.Text,

                BirthDate =
                    dtpBirthDate.Value.Date,

                BirthPlace =
                    txtBirthPlace.Text.Trim(),

                Nationality =
                    txtNationality.Text.Trim(),

                NationalId =
                    txtNationalId.Text.Trim(),

                StudentPhone =
                    phone,

                Status =
                    cmbStatus.Text,

                GuardianName =
                    txtGuardianName.Text.Trim(),

                GuardianRelation =
                    txtGuardianRelation.Text.Trim(),

                GuardianPhone =
                    guardianPhone,

                GuardianEmail =
                    txtGuardianEmail.Text.Trim(),

                GuardianJob =
                    txtGuardianJob.Text.Trim(),

                Governorate =
                    txtGovernorate.Text.Trim(),

                District =
                    txtDistrict.Text.Trim(),

                Address =
                    txtAddress.Text.Trim(),

                Photo =
                    _selectedPhoto
            };
        }

        // ================================================================
        // CLEAR FORM
        // ================================================================

        private void ClearForm()
        {
            _selectedStudentId = 0;
            _selectedPhoto = null;
            _editingSnapshot = null;
            errorProvider1.Clear();

            txtStudentNumber.Text =
                "يُولّد تلقائيًا عند الحفظ";

            txtFullName.Clear();
            txtBirthPlace.Clear();
            txtNationality.Clear();
            txtNationalId.Clear();
            txtPhone.Clear();

            txtGuardianName.Clear();
            txtGuardianRelation.Clear();
            txtGuardianPhone.Clear();
            txtGuardianEmail.Clear();
            txtGuardianJob.Clear();

            txtGovernorate.Clear();
            txtDistrict.Clear();
            txtAddress.Clear();

            if (picStudent.Image != null)
            {
                picStudent.Image.Dispose();
                picStudent.Image = null;
            }

            cmbGender.SelectedIndex = -1;

            cmbStatus.SelectedIndex = 0;

            dtpBirthDate.Value = DateTime.Today;
        }

        // ================================================================
        // REFRESH
        // ================================================================

        private void btnRefresh_Click(
            object sender,
            EventArgs e)
        {
            LoadStudents();
        }

        // ================================================================
        // SEARCH
        // ================================================================

        private void btnSearch_Click(
            object sender,
            EventArgs e)
        {
            ApplyFilters();
            txtSearch.Focus();
        }

        private void btnClearFilters_Click(
            object sender,
            EventArgs e)
        {
            txtSearch.Clear();

            if (cmbFilterClass.Items.Count > 0)
                cmbFilterClass.SelectedIndex = 0;

            if (cmbFilterStatus.Items.Count > 0)
                cmbFilterStatus.SelectedIndex = 0;

            ApplyFilters();
            txtSearch.Focus();
        }

        // ================================================================
        // CLASS FILTER
        // ================================================================

        private void cmbFilterClass_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            ApplyFilters();
        }

        // ================================================================
        // STATUS FILTER
        // ================================================================

        private void cmbFilterStatus_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            ApplyFilters();
        }

        private bool ConfirmDiscardChanges()
        {
            if (!HasUnsavedChanges())
                return true;

            return UIHelper.ShowConfirmation(
                "لديك تغييرات غير محفوظة. هل تريد تجاهلها والمتابعة؟",
                "تغييرات غير محفوظة");
        }

        private bool HasUnsavedChanges()
        {
            if (_formMode == FormMode.View)
                return false;

            if (_formMode == FormMode.Add)
            {
                return !string.IsNullOrWhiteSpace(txtFullName.Text) ||
                       cmbGender.SelectedIndex >= 0 ||
                       !string.IsNullOrWhiteSpace(txtBirthPlace.Text) ||
                       !string.IsNullOrWhiteSpace(txtNationality.Text) ||
                       !string.IsNullOrWhiteSpace(txtNationalId.Text) ||
                       !string.IsNullOrWhiteSpace(txtPhone.Text) ||
                       !string.IsNullOrWhiteSpace(txtGuardianName.Text) ||
                       !string.IsNullOrWhiteSpace(txtGuardianRelation.Text) ||
                       !string.IsNullOrWhiteSpace(txtGuardianPhone.Text) ||
                       !string.IsNullOrWhiteSpace(txtGuardianEmail.Text) ||
                       !string.IsNullOrWhiteSpace(txtGuardianJob.Text) ||
                       !string.IsNullOrWhiteSpace(txtGovernorate.Text) ||
                       !string.IsNullOrWhiteSpace(txtDistrict.Text) ||
                       !string.IsNullOrWhiteSpace(txtAddress.Text) ||
                       _selectedPhoto != null &&
                       _selectedPhoto.Length > 0;
            }

            if (_editingSnapshot == null)
                return true;

            Student current = GetCurrentFormSnapshot();

            return !StudentsEqual(current, _editingSnapshot);
        }

        private Student GetCurrentFormSnapshot()
        {
            return new Student
            {
                StudentId = _selectedStudentId,
                FullName = txtFullName.Text.Trim(),
                Gender = cmbGender.Text.Trim(),
                BirthDate = dtpBirthDate.Value.Date,
                BirthPlace = txtBirthPlace.Text.Trim(),
                Nationality = txtNationality.Text.Trim(),
                NationalId = txtNationalId.Text.Trim(),
                StudentPhone = txtPhone.Text.Trim(),
                Status = cmbStatus.Text.Trim(),
                GuardianName = txtGuardianName.Text.Trim(),
                GuardianRelation = txtGuardianRelation.Text.Trim(),
                GuardianPhone = txtGuardianPhone.Text.Trim(),
                GuardianEmail = txtGuardianEmail.Text.Trim(),
                GuardianJob = txtGuardianJob.Text.Trim(),
                Governorate = txtGovernorate.Text.Trim(),
                District = txtDistrict.Text.Trim(),
                Address = txtAddress.Text.Trim(),
                Photo = _selectedPhoto == null
                    ? null
                    : (byte[])_selectedPhoto.Clone()
            };
        }

        private static bool StudentsEqual(Student left, Student right)
        {
            if (left == null || right == null)
                return left == right;

            return string.Equals(left.FullName, right.FullName, StringComparison.Ordinal) &&
                   string.Equals(left.Gender, right.Gender, StringComparison.Ordinal) &&
                   Nullable.Equals(left.BirthDate, right.BirthDate) &&
                   string.Equals(left.BirthPlace, right.BirthPlace, StringComparison.Ordinal) &&
                   string.Equals(left.Nationality, right.Nationality, StringComparison.Ordinal) &&
                   string.Equals(left.NationalId, right.NationalId, StringComparison.Ordinal) &&
                   string.Equals(left.StudentPhone, right.StudentPhone, StringComparison.Ordinal) &&
                   string.Equals(left.Status, right.Status, StringComparison.Ordinal) &&
                   string.Equals(left.GuardianName, right.GuardianName, StringComparison.Ordinal) &&
                   string.Equals(left.GuardianRelation, right.GuardianRelation, StringComparison.Ordinal) &&
                   string.Equals(left.GuardianPhone, right.GuardianPhone, StringComparison.Ordinal) &&
                   string.Equals(left.GuardianEmail, right.GuardianEmail, StringComparison.Ordinal) &&
                   string.Equals(left.GuardianJob, right.GuardianJob, StringComparison.Ordinal) &&
                   string.Equals(left.Governorate, right.Governorate, StringComparison.Ordinal) &&
                   string.Equals(left.District, right.District, StringComparison.Ordinal) &&
                   string.Equals(left.Address, right.Address, StringComparison.Ordinal) &&
                   PhotosEqual(left.Photo, right.Photo);
        }

        private static bool PhotosEqual(byte[] left, byte[] right)
        {
            if (ReferenceEquals(left, right))
                return true;

            if (left == null || right == null || left.Length != right.Length)
                return false;

            for (int i = 0; i < left.Length; i++)
            {
                if (left[i] != right[i])
                    return false;
            }

            return true;
        }

        // ================================================================
        // ADD
        // ================================================================

        private void btnAdd_Click(
            object sender,
            EventArgs e)
        {
            if (!CurrentUser.CanAdd("Students"))
            {
                UIHelper.ShowWarning(
                    "ليس لديك صلاحية إضافة الطلاب.");
                return;
            }

            if (!ConfirmDiscardChanges())
                return;

            ClearForm();
            SetFormMode(FormMode.Add);
            txtFullName.Focus();
        }

        // ================================================================
        // DOUBLE CLICK GRID
        // ================================================================

        private void dgvStudents_CellDoubleClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            dgvStudents_CellClick(
                sender,
                e);

            btnStudentProfile_Click(
                sender,
                EventArgs.Empty);
        }

        // ================================================================
        // GRID CELL CLICK
        // ================================================================

        private void dgvStudents_CellClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (_formMode != FormMode.View)
                return;

            if (e.RowIndex < 0 ||
                e.RowIndex >= dgvStudents.Rows.Count)
            {
                return;
            }

            DataGridViewCell cell =
                dgvStudents.Rows[e.RowIndex]
                    .Cells["StudentId"];

            if (cell == null ||
                cell.Value == null ||
                cell.Value == DBNull.Value)
            {
                return;
            }

            int id;

            if (!int.TryParse(
                    cell.Value.ToString(),
                    out id) ||
                id <= 0)
            {
                return;
            }

            Student student =
                _currentStudents.FirstOrDefault(
                    s => s.StudentId == id);

            if (student != null)
            {
                FillForm(student);
            }
        }

        // ================================================================
        // FILL FORM
        // ================================================================

        private void FillForm(
            Student s)
        {
            if (s == null)
                return;

            _selectedStudentId =
                s.StudentId;

            _editingSnapshot =
                CloneStudent(s);

            _selectedPhoto =
                s.Photo == null
                    ? null
                    : (byte[])s.Photo.Clone();

            txtStudentNumber.Text =
                s.StudentNumber ?? "";

            txtFullName.Text =
                s.FullName ?? "";

            txtBirthPlace.Text =
                s.BirthPlace ?? "";

            txtNationality.Text =
                s.Nationality ?? "";

            txtNationalId.Text =
                s.NationalId ?? "";

            txtPhone.Text =
                s.StudentPhone ?? "";

            cmbGender.Text =
                s.Gender ?? "";

            cmbStatus.Text =
                s.Status ?? "";

            if (s.BirthDate.HasValue &&
                s.BirthDate.Value.Date <= DateTime.Today)
            {
                dtpBirthDate.Value = s.BirthDate.Value.Date;
            }
            else
            {
                dtpBirthDate.Value = DateTime.Today;
            }

            txtGuardianName.Text =
                s.GuardianName ?? "";

            txtGuardianRelation.Text =
                s.GuardianRelation ?? "";

            txtGuardianPhone.Text =
                s.GuardianPhone ?? "";

            txtGuardianEmail.Text =
                s.GuardianEmail ?? "";

            txtGuardianJob.Text =
                s.GuardianJob ?? "";

            txtGovernorate.Text =
                s.Governorate ?? "";

            txtDistrict.Text =
                s.District ?? "";

            txtAddress.Text =
                s.Address ?? "";

            if (picStudent.Image != null)
            {
                picStudent.Image.Dispose();
                picStudent.Image = null;
            }

            if (_selectedPhoto != null &&
                _selectedPhoto.Length > 0)
            {
                using (MemoryStream ms =
                       new MemoryStream(
                           _selectedPhoto))

                using (Image source =
                       Image.FromStream(ms))
                {
                    picStudent.Image =
                        new Bitmap(source);
                }
            }
        }

        // ================================================================
        // DELETE
        // ================================================================

        private void btnDelete_Click(
            object sender,
            EventArgs e)
        {
            if (_selectedStudentId == 0)
                return;

            if (!CurrentUser.CanDelete("Students"))
            {
                UIHelper.ShowWarning(
                    "ليس لديك صلاحية أرشفة الطلاب.");
                return;
            }

            bool confirmed = UIHelper.ShowConfirmation(
                "سيتم أرشفة الطالب وإخفاؤه من قائمة الطلاب. لن يُحذف السجل من قاعدة البيانات أو السجلات المرتبطة به. هل تريد المتابعة؟",
                "تأكيد أرشفة الطالب");

            if (!confirmed)
                return;

            try
            {
                _studentService.Delete(_selectedStudentId);

                UIHelper.ShowInfo(
                    "تمت أرشفة الطالب وإخفاؤه من القائمة.");

                LoadStudents();
                ClearForm();
                SetFormMode(FormMode.View);
            }
            catch (Exception ex)
            {
                UIHelper.ShowException(
                    "العملية المطلوبة",
                    ex);
            }
        }

    }
}
