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
        private TableLayoutPanel _searchLayout;

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

        private static void ConfigurePreviewButton(
            Button button,
            Color backColor,
            string text,
            float fontSize,
            int width)
        {
            button.Visible = true;
            button.AutoSize = false;
            button.Size = new Size(width, 40);
            button.Margin = new Padding(3, 0, 3, 0);
            button.Text = text;
            button.Font = new Font(
                UIHelper.FontFamily,
                fontSize,
                FontStyle.Bold);
            button.ForeColor = Color.White;
            button.BackColor = backColor;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = backColor;
            button.FlatAppearance.MouseDownBackColor = backColor;
            button.TextAlign = ContentAlignment.MiddleCenter;
            button.Padding = new Padding(3, 0, 3, 0);
            button.UseCompatibleTextRendering = true;
            button.UseVisualStyleBackColor = false;
            button.RightToLeft = RightToLeft.Yes;
            button.Cursor = Cursors.Hand;
        }

        private void ConfigureFieldConstraints()
        {
            txtStudentNumber.ReadOnly = true;

            txtFullName.MaxLength = 200;
            txtBirthPlace.MaxLength = 100;
            txtNationality.MaxLength = 100;
            txtNationalId.MaxLength = 20;
            txtPhone.MaxLength = 30;
            txtGuardianName.MaxLength = 200;
            txtGuardianRelation.MaxLength = 50;
            txtGuardianPhone.MaxLength = 30;
            txtGuardianEmail.MaxLength = 150;
            txtGuardianJob.MaxLength = 100;
            txtGovernorate.MaxLength = 100;
            txtDistrict.MaxLength = 100;
            txtAddress.MaxLength = 300;

            txtStudentNumber.TextAlign = HorizontalAlignment.Center;
            txtNationalId.RightToLeft = RightToLeft.No;
            txtNationalId.TextAlign = HorizontalAlignment.Left;
            txtPhone.RightToLeft = RightToLeft.No;
            txtPhone.TextAlign = HorizontalAlignment.Left;
            txtGuardianPhone.RightToLeft = RightToLeft.No;
            txtGuardianPhone.TextAlign = HorizontalAlignment.Left;
            txtGuardianEmail.RightToLeft = RightToLeft.No;
            txtGuardianEmail.TextAlign = HorizontalAlignment.Left;

            cmbGender.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFilterClass.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFilterStatus.DropDownStyle = ComboBoxStyle.DropDownList;

            dtpBirthDate.MaxDate = DateTime.Today;
            dtpBirthDate.Format = DateTimePickerFormat.Custom;
            dtpBirthDate.CustomFormat = "dd/MM/yyyy";
            dtpBirthDate.ShowCheckBox = false;
            dtpBirthDate.ShowUpDown = false;
        }

        private void ConfigureTooltips()
        {
            _toolTip.SetToolTip(txtStudentNumber, "يتم توليد رقم الطالب تلقائيًا عند الحفظ.");
            _toolTip.SetToolTip(txtNationalId, "رقم الهوية يُدخل يدويًا ويجب أن يكون فريدًا.");
            _toolTip.SetToolTip(txtSearch, "ابحث بالاسم أو الرقم أو الهوية أو الهاتف أو بيانات ولي الأمر.");
            _toolTip.SetToolTip(cmbFilterClass, "تصفية الطلاب حسب الصف الحالي.");
            _toolTip.SetToolTip(cmbFilterStatus, "تصفية الطلاب حسب الحالة.");
            _toolTip.SetToolTip(btnUpdate, "ينقل الطالب المحدد إلى وضع التعديل.");
            _toolTip.SetToolTip(btnDelete, "أرشفة الطالب وإخفاؤه من القائمة دون حذف سجله.");
            _toolTip.SetToolTip(btnRefresh, "يعيد تحميل قائمة الطلاب من قاعدة البيانات.");
            _toolTip.SetToolTip(btnSearch, "تنفيذ البحث بالاسم أو الرقم أو البيانات المرتبطة.");
            _toolTip.SetToolTip(btnClearFilters, "مسح البحث والفلاتر وإظهار جميع الطلاب.");
            _toolTip.SetToolTip(btnReload, "يعيد تحميل بيانات الطلاب من قاعدة البيانات.");
        }

        private void SetFormMode(FormMode mode)
        {
            _formMode = mode;

            bool isAdd = mode == FormMode.Add;
            bool isEdit = mode == FormMode.Edit;
            bool hasSelection = _selectedStudentId > 0;

            bool canAdd = CurrentUser.CanAdd("Students");
            bool canEdit = CurrentUser.CanEdit("Students");
            bool canDelete = CurrentUser.CanDelete("Students");
            bool canPrint = CurrentUser.CanPrint("Students");
            bool canExport =
                CurrentUser.HasAny(
                    "Students.ExportExcel",
                    "Students.Manage");
            bool canSearch = CurrentUser.CanSearch("Students");

            bool editable = (isAdd && canAdd) || (isEdit && canEdit);

            foreach (Control control in new Control[]
            {
                txtFullName, cmbGender, dtpBirthDate, txtBirthPlace,
                txtNationality, txtNationalId, txtPhone, cmbStatus,
                txtGuardianName, txtGuardianRelation, txtGuardianPhone,
                txtGuardianEmail, txtGuardianJob, txtGovernorate,
                txtDistrict, txtAddress
            })
            {
                TextBox textBox = control as TextBox;
                if (textBox != null)
                    textBox.ReadOnly = !editable;

                ComboBox comboBox = control as ComboBox;
                if (comboBox != null)
                    comboBox.Enabled = editable;

                DateTimePicker datePicker = control as DateTimePicker;
                if (datePicker != null)
                    datePicker.Enabled = editable;
            }

            txtStudentNumber.ReadOnly = true;
            btnChooseImage.Enabled = editable;
            btnRemoveImage.Enabled = editable;
            dgvStudents.Enabled = mode == FormMode.View;

            btnStudentProfile.Enabled =
                mode == FormMode.View && hasSelection &&
                CurrentUser.CanView("Students");

            btnPrint.Enabled =
                mode == FormMode.View && hasSelection && canPrint;

            btnExportExcel.Enabled =
                mode == FormMode.View && _filteredStudents.Count > 0 && canExport;

            btnDelete.Enabled =
                mode == FormMode.View && hasSelection && canDelete;

            btnUpdate.Enabled =
                mode == FormMode.View && hasSelection && canEdit;

            btnAdd.Enabled = canAdd;
            btnSave.Enabled = editable;
            btnCancel.Enabled = isAdd || isEdit;
            btnRefresh.Enabled = mode == FormMode.View;

            btnSearch.Enabled = canSearch;
            btnClearFilters.Enabled = canSearch;
            txtSearch.Enabled = canSearch;
            cmbFilterClass.Enabled = canSearch;
            cmbFilterStatus.Enabled = canSearch;

            if (isAdd)
                btnSave.Text = "حفظ الطالب";
            else if (isEdit)
                btnSave.Text = "حفظ التعديلات";
            else
                btnSave.Text = "حفظ الطالب";

            btnUpdate.Text = "تعديل البيانات";
        }

        private Student CloneStudent(Student source)
        {
            if (source == null)
                return null;

            return new Student
            {
                StudentId = source.StudentId,
                StudentNumber = source.StudentNumber,
                FullName = source.FullName,
                Gender = source.Gender,
                BirthDate = source.BirthDate,
                BirthPlace = source.BirthPlace,
                Nationality = source.Nationality,
                NationalId = source.NationalId,
                StudentPhone = source.StudentPhone,
                Status = source.Status,
                GuardianName = source.GuardianName,
                GuardianRelation = source.GuardianRelation,
                GuardianPhone = source.GuardianPhone,
                GuardianEmail = source.GuardianEmail,
                GuardianJob = source.GuardianJob,
                Governorate = source.Governorate,
                District = source.District,
                Address = source.Address,
                Photo = source.Photo == null ? null : (byte[])source.Photo.Clone(),
                CurrentClassName = source.CurrentClassName,
                CurrentSection = source.CurrentSection,
                AcademicYear = source.AcademicYear,
                CreatedAt = source.CreatedAt,
                UpdatedAt = source.UpdatedAt
            };
        }

        private Exception ValidationError(Control control, string message)
        {
            if (control != null)
            {
                errorProvider1.SetError(control, message);
                control.Focus();
            }

            return new ArgumentException(message);
        }

        // ================================================================
        // FORM LOAD
        // ================================================================

        private void StudentsForm_Load(object sender, EventArgs e)
        {
            // ------------------------------------------------------------
            // لا يوجد ApplyCustomStyles هنا.
            //
            // التصميم تم تحديده في Designer فقط.
            // ------------------------------------------------------------

            LoadComboBoxes();

            ClearForm();

            LoadStudents();

            SetFormMode(FormMode.View);
        }

        // ================================================================
        // STUDENT PROFILE BUTTON
        // ================================================================

        private void btnStudentProfile_Click(object sender, EventArgs e)
        {
            if (_selectedStudentId <= 0)
            {
                UIHelper.ShowWarning(
                    "اختر طالبًا من الجدول أولاً لفتح ملفه الموحد.");

                return;
            }

            if (MainForm.Instance == null)
            {
                UIHelper.ShowWarning(
                    "تعذر فتح ملف الطالب خارج نافذة النظام الرئيسية.");

                return;
            }

            try
            {
                CurrentUser.DemandAny(
                    "ليس لديك صلاحية عرض ملفات الطلاب.",
                    PermissionKeys.StudentsView,
                    PermissionKeys.StudentsManage);

                StudentProfileForm profileForm =
                    new StudentProfileForm(_selectedStudentId);

                MainForm.Instance.LoadUserControl(profileForm);
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("فتح ملف الطالب", ex);
            }
        }

        // ================================================================
        // CHOOSE IMAGE
        // ================================================================

        private void btnChooseImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter =
                    "ملفات الصور|*.jpg;*.jpeg;*.png;*.bmp;*.gif";

                ofd.Title =
                    "اختر صورة الطالب";

                if (ofd.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    FileInfo fileInfo =
                        new FileInfo(ofd.FileName);

                    if (fileInfo.Length > 2 * 1024 * 1024)
                    {
                        UIHelper.ShowWarning(
                            "حجم الصورة كبير جدًا. الرجاء اختيار صورة أصغر من 2 ميجابايت.");

                        return;
                    }

                    byte[] imageBytes =
                        File.ReadAllBytes(ofd.FileName);

                    if (picStudent.Image != null)
                    {
                        picStudent.Image.Dispose();
                        picStudent.Image = null;
                    }

                    using (MemoryStream ms =
                           new MemoryStream(imageBytes))
                    using (Image selectedImage =
                           Image.FromStream(ms))
                    {
                        _selectedPhoto = imageBytes;

                        picStudent.Image =
                            new Bitmap(selectedImage);
                    }
                }
                catch (Exception ex)
                {
                    UIHelper.ShowException(
                        "تعذر تحميل الصورة: ",
                        ex);
                }
            }
        }

        // ================================================================
        // REMOVE IMAGE
        // ================================================================

        private void btnRemoveImage_Click(object sender, EventArgs e)
        {
            _selectedPhoto = null;

            if (picStudent.Image != null)
            {
                picStudent.Image.Dispose();
                picStudent.Image = null;
            }
        }

        // ================================================================
        // GRID SELECTION CHANGED
        // ================================================================

        private void dgvStudents_SelectionChanged(
            object sender,
            EventArgs e)
        {
            if (_formMode != FormMode.View)
                return;

            Student student =
                GetSelectedStudentFromGrid();

            if (student != null &&
                student.StudentId != _selectedStudentId)
            {
                FillForm(student);
            }
        }

        // ================================================================
        // GET SELECTED STUDENT
        // ================================================================

        private Student GetSelectedStudentFromGrid()
        {
            if (dgvStudents.CurrentRow == null ||
                dgvStudents.CurrentRow.DataBoundItem == null)
            {
                return null;
            }

            DataGridViewCell cell =
                dgvStudents.CurrentRow.Cells["StudentId"];

            if (cell == null ||
                cell.Value == null ||
                cell.Value == DBNull.Value)
            {
                return null;
            }

            int id;

            if (!int.TryParse(
                    cell.Value.ToString(),
                    out id) ||
                id <= 0)
            {
                return null;
            }

            return _currentStudents.FirstOrDefault(
                s => s.StudentId == id);
        }

        // ================================================================
        // SEARCH ENTER
        // ================================================================

        private void txtSearch_KeyDown(
            object sender,
            KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;

                ApplyFilters();
            }
        }

        // ================================================================
        // CLOSE
        // ================================================================

        private void btnClose_Click(
            object sender,
            EventArgs e)
        {
            if (!ConfirmDiscardChanges())
                return;

            if (Parent != null)
            {
                Parent.Controls.Remove(this);

                Dispose();
            }
            else
            {
                Close();
            }
        }

        // ================================================================
        // PRINT
        // ================================================================

        private void btnPrint_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                CurrentUser.DemandAction(
                    "Students",
                    "Print",
                    "ليس لديك صلاحية طباعة بطاقات الطلاب.");
            }
            catch (UnauthorizedAccessException ex)
            {
                ApplicationLogger.LogWarning("شاشة الطلاب: طباعة بطاقات الطلاب", ex.Message);
                UIHelper.ShowWarning(ex.Message);

                return;
            }

            if (_selectedStudentId == 0)
            {
                UIHelper.ShowWarning(
                    "اختر طالبًا أولاً قبل الطباعة.");

                return;
            }

            Student student =
                _currentStudents.FirstOrDefault(
                    s => s.StudentId == _selectedStudentId);

            if (student == null)
            {
                UIHelper.ShowWarning(
                    "تعذر تحميل بيانات الطالب المحدد للطباعة.");

                return;
            }

            using (PrintDialog dialog =
                   new PrintDialog())
            {
                dialog.Document =
                    _studentCardPrintDocument;

                dialog.UseEXDialog = true;

                if (dialog.ShowDialog(this) !=
                    DialogResult.OK)
                {
                    return;
                }

                try
                {
                    _studentCardPrintDocument.Print();
                }
                catch (Exception ex)
                {
                    UIHelper.ShowException(
                        "تعذر تنفيذ طباعة بطاقة الطالب: ",
                        ex);
                }
            }
        }

        // ================================================================
        // PRINT PAGE
        // ================================================================

        private void StudentCardPrintDocument_PrintPage(
            object sender,
            PrintPageEventArgs e)
        {
            Student student =
                _currentStudents.FirstOrDefault(
                    s => s.StudentId == _selectedStudentId);

            if (student == null)
            {
                e.HasMorePages = false;
                return;
            }

            Rectangle bounds =
                e.MarginBounds;

            SchoolSettings branding =
                SchoolSettingsService.Current();

            int cardWidth =
                Math.Min(
                    bounds.Width - 20,
                    620);

            int cardHeight = 382;

            Rectangle card =
                new Rectangle(
                    bounds.Left +
                    Math.Max(
                        0,
                        (bounds.Width - cardWidth) / 2),
                    bounds.Top + 8,
                    cardWidth,
                    cardHeight);

            bool isRtl = true;

            using (Font schoolFont =
                   new Font(
                       "Tahoma",
                       13F,
                       FontStyle.Bold))

            using (Font organizationFont =
                   new Font(
                       "Tahoma",
                       8F,
                       FontStyle.Regular))

            using (Font titleFont =
                   new Font(
                       "Tahoma",
                       13F,
                       FontStyle.Bold))

            using (Font labelFont =
                   new Font(
                       "Tahoma",
                       10F,
                       FontStyle.Bold))

            using (Font valueFont =
                   new Font(
                       "Tahoma",
                       10F,
                       FontStyle.Regular))

            using (StringFormat rtl =
                   new StringFormat
                   {
                       Alignment =
                           StringAlignment.Center,

                       LineAlignment =
                           StringAlignment.Center,

                       FormatFlags =
                           StringFormatFlags.DirectionRightToLeft,

                       Trimming =
                           StringTrimming.EllipsisCharacter
                   })

            using (StringFormat rightRtl =
                   new StringFormat
                   {
                       Alignment =
                           StringAlignment.Far,

                       LineAlignment =
                           StringAlignment.Center,

                       FormatFlags =
                           StringFormatFlags.DirectionRightToLeft,

                       Trimming =
                           StringTrimming.EllipsisCharacter
                   })

            using (Pen borderPen =
                   new Pen(
                       Color.FromArgb(
                           31,
                           78,
                           121),
                       2F))

            using (Pen dividerPen =
                   new Pen(
                       Color.FromArgb(
                           15,
                           118,
                           110),
                       1.2F))

            using (Brush headerBrush =
                   new SolidBrush(
                       Color.FromArgb(
                           235,
                           243,
                           248)))
            {
                e.Graphics.FillRectangle(
                    Brushes.White,
                    card);

                e.Graphics.DrawRectangle(
                    borderPen,
                    card);

                Rectangle header =
                    new Rectangle(
                        card.Left + 2,
                        card.Top + 2,
                        card.Width - 4,
                        78);

                e.Graphics.FillRectangle(
                    headerBrush,
                    header);

                int logoSize = 58;

                Rectangle logoBounds =
                    new Rectangle(
                        card.Right -
                        logoSize -
                        16,
                        card.Top + 12,
                        logoSize,
                        logoSize);

                if (branding != null &&
                    branding.Logo != null &&
                    branding.Logo.Length > 0)
                {
                    try
                    {
                        using (MemoryStream stream =
                               new MemoryStream(
                                   branding.Logo))

                        using (Image source =
                               Image.FromStream(stream))

                        using (Bitmap logo =
                               new Bitmap(source))
                        {
                            e.Graphics.DrawImage(
                                logo,
                                logoBounds);
                        }
                    }
                    catch (Exception logoEx)
                    {
                        // الشعار تكميلي: فشل رسمه لا يجب أن يوقف الطباعة، لكن يُسجل للتشخيص.
                        ApplicationLogger.LogException("رسم شعار ترويسة الطباعة", logoEx);
                    }
                }

                Rectangle schoolNameBounds =
                    new Rectangle(
                        card.Left + 82,
                        card.Top + 10,
                        card.Width - 180,
                        38);

                e.Graphics.DrawString(
                    ReportOutputHelper.GetSchoolName(
                        branding),
                    schoolFont,
                    Brushes.Black,
                    schoolNameBounds,
                    rtl);

                Rectangle organizationBounds =
                    new Rectangle(
                        card.Left + 82,
                        card.Top + 47,
                        card.Width - 180,
                        27);

                e.Graphics.DrawString(
                    ReportOutputHelper.GetOrganizationLine(
                        branding),
                    organizationFont,
                    Brushes.DimGray,
                    organizationBounds,
                    rtl);

                e.Graphics.DrawLine(
                    dividerPen,
                    card.Left + 18,
                    card.Top + 82,
                    card.Right - 18,
                    card.Top + 82);

                Rectangle titleBounds =
                    new Rectangle(
                        card.Left + 18,
                        card.Top + 90,
                        card.Width - 36,
                        30);

                e.Graphics.DrawString(
                    "بطاقة الطالب",
                    titleFont,
                    Brushes.Black,
                    titleBounds,
                    rtl);

                Rectangle photoBounds =
                    new Rectangle(
                        card.Right - 125,
                        card.Top + 132,
                        105,
                        125);

                Image photo = null;

                try
                {
                    byte[] bytes =
                        student.Photo ?? _selectedPhoto;

                    if (bytes != null &&
                        bytes.Length > 0)
                    {
                        using (MemoryStream stream =
                               new MemoryStream(bytes))

                        using (Image source =
                               Image.FromStream(stream))
                        {
                            photo =
                                new Bitmap(source);
                        }
                    }

                    if (photo != null)
                    {
                        e.Graphics.DrawImage(
                            photo,
                            photoBounds);
                    }
                    else
                    {
                        using (StringFormat emptyPhotoFormat =
                               new StringFormat
                               {
                                   Alignment =
                                       StringAlignment.Center,

                                   LineAlignment =
                                       StringAlignment.Center,

                                   FormatFlags =
                                       StringFormatFlags.DirectionRightToLeft
                               })
                        {
                            e.Graphics.DrawString(
                                "لا توجد صورة",
                                valueFont,
                                Brushes.DimGray,
                                photoBounds,
                                emptyPhotoFormat);
                        }
                    }
                }
                finally
                {
                    if (photo != null)
                    {
                        photo.Dispose();
                    }
                }

                int x =
                    card.Left + 20;

                int width =
                    card.Width - 160;

                int y =
                    card.Top + 132;

                DrawStudentCardLine(
                    e.Graphics,
                    x,
                    width,
                    ref y,
                    "الاسم",
                    student.FullName,
                    labelFont,
                    valueFont,
                    rightRtl,
                    isRtl);

                DrawStudentCardLine(
                    e.Graphics,
                    x,
                    width,
                    ref y,
                    "رقم الطالب",
                    student.StudentNumber,
                    labelFont,
                    valueFont,
                    rightRtl,
                    isRtl);

                DrawStudentCardLine(
                    e.Graphics,
                    x,
                    width,
                    ref y,
                    "الصف",
                    student.CurrentClassName,
                    labelFont,
                    valueFont,
                    rightRtl,
                    isRtl);

                DrawStudentCardLine(
                    e.Graphics,
                    x,
                    width,
                    ref y,
                    "الجنس",
                    student.Gender,
                    labelFont,
                    valueFont,
                    rightRtl,
                    isRtl);

                DrawStudentCardLine(
                    e.Graphics,
                    x,
                    width,
                    ref y,
                    "الحالة",
                    student.Status,
                    labelFont,
                    valueFont,
                    rightRtl,
                    isRtl);

                DrawStudentCardLine(
                    e.Graphics,
                    x,
                    width,
                    ref y,
                    "هاتف ولي الأمر",
                    student.GuardianPhone,
                    labelFont,
                    valueFont,
                    rightRtl,
                    isRtl);

                e.Graphics.DrawString(
                    "تاريخ الإصدار: " +
                    UIHelper.FormatDateDual(DateTime.Now.Date),
                    valueFont,
                    Brushes.DimGray,
                    new RectangleF(
                        card.Left + 20,
                        card.Bottom - 38,
                        card.Width - 40,
                        24),
                    rtl);
            }

            e.HasMorePages = false;
        }

        // ================================================================
        // PRINT CARD LINE
        // ================================================================

        private static void DrawStudentCardLine(
            Graphics graphics,
            int x,
            int width,
            ref int y,
            string label,
            string value,
            Font labelFont,
            Font valueFont,
            StringFormat format,
            bool rtl)
        {
            float labelWidth =
                width * 0.36F;

            float valueWidth =
                width -
                labelWidth -
                8F;

            RectangleF labelRect =
                rtl
                    ? new RectangleF(
                        x + valueWidth + 8F,
                        y,
                        labelWidth,
                        25)

                    : new RectangleF(
                        x,
                        y,
                        labelWidth,
                        25);

            RectangleF valueRect =
                rtl
                    ? new RectangleF(
                        x,
                        y,
                        valueWidth,
                        25)

                    : new RectangleF(
                        x + labelWidth + 8F,
                        y,
                        valueWidth,
                        25);

            StringFormat labelFormat =
                new StringFormat(format);

            StringFormat valueFormat =
                new StringFormat(format);

            labelFormat.Alignment =
                rtl
                    ? StringAlignment.Far
                    : StringAlignment.Near;

            valueFormat.Alignment =
                rtl
                    ? StringAlignment.Near
                    : StringAlignment.Far;

            using (labelFormat)
            using (valueFormat)
            {
                graphics.DrawString(
                    label + ":",
                    labelFont,
                    Brushes.Black,
                    labelRect,
                    labelFormat);

                graphics.DrawString(
                    string.IsNullOrWhiteSpace(value)
                        ? "-"
                        : value,
                    valueFont,
                    Brushes.Black,
                    valueRect,
                    valueFormat);
            }

            y += 29;
        }

        // ================================================================
        // EXPORT EXCEL
        // ================================================================

        private void btnExportExcel_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                CurrentUser.DemandAction(
                    "Students",
                    "ExportExcel",
                    "ليس لديك صلاحية تصدير بيانات الطلاب إلى Excel.");

                if (_filteredStudents == null ||
                    _filteredStudents.Count == 0)
                {
                    UIHelper.ShowWarning(
                        "لا توجد بيانات لتصديرها.");

                    return;
                }

                using (SaveFileDialog sfd =
                       new SaveFileDialog())
                {
                    sfd.Filter =
                        "ملفات Excel (*.xlsx)|*.xlsx";

                    sfd.FileName =
                        "قائمة_الطلاب_" +
                        DateTime.Now.ToString(
                            "yyyyMMdd_HHmm") +
                        ".xlsx";

                    if (sfd.ShowDialog() !=
                        DialogResult.OK)
                    {
                        return;
                    }

                    System.Data.DataTable exportTable =
                        new System.Data.DataTable();

                    exportTable.Columns.Add(
                        "رقم الطالب");

                    exportTable.Columns.Add(
                        "الاسم");

                    exportTable.Columns.Add(
                        "الجنس");

                    exportTable.Columns.Add(
                        "الصف");

                    exportTable.Columns.Add(
                        "الهاتف");

                    exportTable.Columns.Add(
                        "الحالة");

                    foreach (Student s
                             in _filteredStudents)
                    {
                        exportTable.Rows.Add(
                            s.StudentNumber ??
                                string.Empty,

                            s.FullName ??
                                string.Empty,

                            s.Gender ??
                                string.Empty,

                            s.CurrentClassName ??
                                string.Empty,

                            s.StudentPhone ??
                                string.Empty,

                            s.Status ??
                                string.Empty);
                    }

                    ReportOutputHelper.ExportToExcel(
                        exportTable,
                        sfd.FileName,
                        "بيانات الطلاب",
                        "النتائج المصدّرة: " +
                        exportTable.Rows.Count);

                    UIHelper.ShowInfo(
                        "تم تصدير بيانات الطلاب إلى Excel بنجاح.");
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowException(
                    "فشل التصدير: ",
                    ex);
            }
        }

        // ================================================================
        // CANCEL
        // ================================================================

        private void btnCancel_Click(
            object sender,
            EventArgs e)
        {
            errorProvider1.Clear();

            if (_formMode == FormMode.Edit &&
                _editingSnapshot != null)
            {
                FillForm(_editingSnapshot);
                SetFormMode(FormMode.View);
                return;
            }

            ClearForm();
            SetFormMode(FormMode.View);
        }

        // ================================================================
        // UPDATE
        // ================================================================

        private void btnUpdate_Click(
            object sender,
            EventArgs e)
        {
            Student gridStudent = GetSelectedStudentFromGrid();

            if (gridStudent != null &&
                gridStudent.StudentId != _selectedStudentId)
            {
                FillForm(gridStudent);
            }

            if (_selectedStudentId == 0)
            {
                UIHelper.ShowWarning(
                    "اختر طالبًا من الجدول أولاً.");

                return;
            }

            if (!CurrentUser.CanEdit("Students"))
            {
                UIHelper.ShowWarning(
                    "ليس لديك صلاحية تعديل بيانات الطلاب.");

                return;
            }

            Student selected =
                _currentStudents.FirstOrDefault(
                    s => s.StudentId == _selectedStudentId);

            if (selected == null)
            {
                UIHelper.ShowWarning(
                    "تعذر تحميل الطالب المحدد للتعديل.");

                return;
            }

            _editingSnapshot = CloneStudent(selected);
            SetFormMode(FormMode.Edit);
            txtFullName.Focus();
        }

        // ================================================================
        // LOAD COMBO BOXES
        // ================================================================

        private void LoadComboBoxes()
        {
            cmbGender.Items.Clear();

            cmbGender.Items.AddRange(
                new[]
                {
                    "ذكر",
                    "أنثى"
                });

            cmbStatus.Items.Clear();

            cmbStatus.Items.AddRange(
                new[]
                {
                    "نشط",
                    "موقوف",
                    "منقول",
                    "متخرج"
                });

            cmbFilterStatus.Items.Clear();

            cmbFilterStatus.Items.AddRange(
                new[]
                {
                    "كل الحالات",
                    "نشط",
                    "موقوف",
                    "منقول",
                    "متخرج"
                });

            cmbFilterStatus.SelectedIndex = 0;

            cmbFilterClass.Items.Clear();

            cmbFilterClass.Items.Add(
                "كل الصفوف");

            cmbFilterClass.SelectedIndex = 0;
        }

        // ================================================================
        // LOAD STUDENTS
        // ================================================================

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
        // PREVIEW NEXT STUDENT NUMBER
        // ================================================================

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
