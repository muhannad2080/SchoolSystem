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

        // ================================================================
        // CONSTRUCTOR
        // ================================================================

        public StudentsForm()
        {
            InitializeComponent();

            // ------------------------------------------------------------
            // خصائص مرتبطة بالسلوك فقط.
            // لا يوجد هنا أي Styling أو تغيير Layout.
            // ------------------------------------------------------------

            txtStudentNumber.ReadOnly = true;
            txtStudentNumber.TabStop = false;

            // تطبيق قواعد التحقق من الحقول (فلترة الأحرف، أقصى طول، والتحقق عند
            // المغادرة) دون تغيير تنسيق المصمم لهذه الشاشة.
            SchoolSystem.Helpers.UIHelper.ApplyFieldValidation(this);

            // منع اختيار تاريخ ميلاد في المستقبل.
            dtpBirthDate.MaxDate = DateTime.Today;

            _studentService = new StudentService();

            _currentStudents = new List<Student>();

            // ------------------------------------------------------------
            // البحث الفوري
            // ------------------------------------------------------------

            txtSearch.TextChanged +=
                (sender, e) => ApplyFilters();

            // ------------------------------------------------------------
            // الفلاتر
            // ------------------------------------------------------------

            cmbFilterClass.SelectedIndexChanged +=
                cmbFilterClass_SelectedIndexChanged;

            cmbFilterStatus.SelectedIndexChanged +=
                cmbFilterStatus_SelectedIndexChanged;

            // ------------------------------------------------------------
            // فتح ملف الطالب بالنقر المزدوج
            // ------------------------------------------------------------

            dgvStudents.CellDoubleClick +=
                dgvStudents_CellDoubleClick;

            // ------------------------------------------------------------
            // الطباعة
            // ------------------------------------------------------------

            _studentCardPrintDocument.PrintPage +=
                StudentCardPrintDocument_PrintPage;

            // لا يوجد هنا أي:
            // UIHelper.ApplyStyle(...)
            // UIHelper.StyleButton(...)
            // StyleTextBox(...)
            // StyleComboBox(...)
            // StyleDataGridView(...)
        }

        // ================================================================
        // STUDENT PROFILE
        // ================================================================

        private void ConfigureStudentProfileButton()
        {
            // تم إلغاء Styling الزر هنا عمدًا.
            //
            // زر btnStudentProfile يأخذ تصميمه بالكامل من
            // StudentsForm.Designer.cs.
            //
            // لا يوجد تغيير في الحجم أو الموضع أو الاتجاه أو Dock أو Anchor.
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

            MainForm.Instance.LoadUserControl(
                "Students",
                "ليس لديك صلاحية عرض ملف الطالب.",
                new StudentProfileForm(_selectedStudentId),
                PermissionKeys.StudentsView,
                PermissionKeys.StudentsManage);
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
                    DateTime.Now.ToString("yyyy/MM/dd"),
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

                if (_currentStudents == null ||
                    _currentStudents.Count == 0)
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
                             in _currentStudents)
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
                        "إجمالي الطلاب: " +
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
            ClearForm();
        }

        // ================================================================
        // UPDATE
        // ================================================================

        private void btnUpdate_Click(
            object sender,
            EventArgs e)
        {
            if (_selectedStudentId == 0)
            {
                UIHelper.ShowWarning(
                    "اختر طالبًا من الجدول أولاً.");

                return;
            }

            btnSave_Click(
                sender,
                e);
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
            var filtered =
                _currentStudents.AsEnumerable();

            string search =
                txtSearch.Text.Trim();

            if (!string.IsNullOrEmpty(search))
            {
                filtered =
                    filtered.Where(s =>
                        (s.FullName != null &&
                         s.FullName.Contains(search))

                        ||

                        (s.StudentNumber != null &&
                         s.StudentNumber.Contains(search))

                        ||

                        (s.NationalId != null &&
                         s.NationalId.Contains(search))

                        ||

                        (s.StudentPhone != null &&
                         s.StudentPhone.Contains(search))

                        ||

                        (s.GuardianName != null &&
                         s.GuardianName.Contains(search))

                        ||

                        (s.GuardianPhone != null &&
                         s.GuardianPhone.Contains(search)));
            }

            if (cmbFilterStatus.SelectedIndex > 0)
            {
                filtered =
                    filtered.Where(
                        s => s.Status ==
                             cmbFilterStatus.Text);
            }

            if (cmbFilterClass.SelectedIndex > 0 &&
                !string.IsNullOrWhiteSpace(
                    cmbFilterClass.Text))
            {
                filtered =
                    filtered.Where(
                        s =>
                            string.Equals(
                                s.CurrentClassName,
                                cmbFilterClass.Text,
                                StringComparison.OrdinalIgnoreCase));
            }

            BindGrid(
                filtered.ToList());
        }

        // ================================================================
        // BIND GRID
        // ================================================================

        private void BindGrid(
            List<Student> students)
        {
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

            if (dgvStudents.Columns["StudentId"] != null)
            {
                dgvStudents.Columns[
                    "StudentId"].Visible = false;
            }

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
                $"عدد الطلاب: {students.Count}";
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

            _isSaving = true;

            btnSave.Enabled = false;

            try
            {
                Student student =
                    GetStudentFromForm();

                if (_selectedStudentId == 0)
                {
                    _studentService.Add(
                        student);
                }
                else
                {
                    _studentService.Update(
                        student);
                }

                UIHelper.ShowInfo(
                    string.IsNullOrWhiteSpace(
                        student.StudentNumber)

                        ? "تم حفظ بيانات الطالب بنجاح."

                        : "تم حفظ بيانات الطالب بنجاح. رقم الطالب المولد: " +
                          student.StudentNumber);

                LoadStudents();

                ClearForm();
            }
            catch (Exception ex)
            {
                UIHelper.ShowException(
                    "العملية المطلوبة",
                    ex);
            }
            finally
            {
                _isSaving = false;

                btnSave.Enabled = true;
            }
        }

        // ================================================================
        // GET STUDENT FROM FORM
        // ================================================================

        private Student GetStudentFromForm()
        {
            string fullName =
                txtFullName.Text.Trim();

            if (!UIHelper.IsValidArabicOrLatinName(
                    fullName))
            {
                throw new Exception(
                    "اسم الطالب مطلوب ويجب أن يحتوي على أحرف فقط وبطول مناسب.");
            }

            if (cmbGender.SelectedIndex < 0 ||
                string.IsNullOrWhiteSpace(
                    cmbGender.Text))
            {
                throw new Exception(
                    "يرجى اختيار جنس الطالب.");
            }

            if (cmbStatus.SelectedIndex < 0 ||
                string.IsNullOrWhiteSpace(
                    cmbStatus.Text))
            {
                throw new Exception(
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

            if (dtpBirthDate.Value >
                DateTime.Today)
            {
                throw new Exception(
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
                    dtpBirthDate.Value,

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

            txtStudentNumber.Text =
                "جاري تجهيز الرقم...";

            TryPreviewNextStudentNumber();

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

            dtpBirthDate.Value =
                DateTime.Today.AddYears(-6);
        }

        // ================================================================
        // PREVIEW NEXT STUDENT NUMBER
        // ================================================================

        private void TryPreviewNextStudentNumber()
        {
            if (_studentService == null ||
                _selectedStudentId != 0)
            {
                return;
            }

            try
            {
                string nextNumber =
                    _studentService.GenerateNextStudentNumber();

                txtStudentNumber.Text =
                    string.IsNullOrWhiteSpace(
                        nextNumber)

                        ? "يُولّد تلقائياً عند الحفظ"

                        : nextNumber;
            }
            catch (Exception ex)
            {
                ApplicationLogger.LogException("شاشة الطلاب: معاينة رقم الطالب التالي", ex);
                txtStudentNumber.Text =
                    "يُولّد تلقائياً عند الحفظ";
            }
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

        // ================================================================
        // ADD
        // ================================================================

        private void btnAdd_Click(
            object sender,
            EventArgs e)
        {
            ClearForm();

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
            _selectedStudentId =
                s.StudentId;

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

            dtpBirthDate.Value =
                s.BirthDate.HasValue &&
                s.BirthDate.Value <=
                DateTime.Today

                    ? s.BirthDate.Value

                    : DateTime.Today.AddYears(-6);

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

            DialogResult result =
                MessageBox.Show(
                    "هل أنت متأكد من حذف هذا الطالب؟",
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
                _studentService.Delete(
                    _selectedStudentId);

                LoadStudents();

                ClearForm();
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
