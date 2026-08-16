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

namespace SchoolSystem.UI
{
    public partial class StudentsForm : Form
    {
        private readonly StudentService _studentService;
        private int _selectedStudentId;
        private byte[] _selectedPhoto;
        private List<Student> _currentStudents;
        private readonly PrintDocument _studentCardPrintDocument = new PrintDocument();
        private bool _isSaving;

        public StudentsForm()
        {
            InitializeComponent();
            txtStudentNumber.ReadOnly = true;
            txtStudentNumber.TabStop = false;
            SchoolSystem.Helpers.UIHelper.ApplyStyle(this);
            txtSearch.TextChanged += (sender, e) => ApplyFilters();
            _studentService = new StudentService();
            _currentStudents = new List<Student>();
            cmbFilterClass.SelectedIndexChanged += cmbFilterClass_SelectedIndexChanged;
            cmbFilterStatus.SelectedIndexChanged += cmbFilterStatus_SelectedIndexChanged;
            _studentCardPrintDocument.PrintPage += StudentCardPrintDocument_PrintPage;
            ConfigureStudentProfileButton();
        }

        private void ConfigureStudentProfileButton()
        {
            UIHelper.StyleButton(btnStudentProfile, UIHelper.AccentColor);
            btnStudentProfile.Click -= btnStudentProfile_Click;
            btnStudentProfile.Click += btnStudentProfile_Click;
        }

        private void btnStudentProfile_Click(object sender, EventArgs e)
        {
            if (_selectedStudentId <= 0)
            {
                UIHelper.ShowWarning("اختر طالبًا من الجدول أولاً لفتح ملفه الموحد.");
                return;
            }

            if (MainForm.Instance == null)
            {
                UIHelper.ShowWarning("تعذر فتح ملف الطالب خارج نافذة النظام الرئيسية.");
                return;
            }

            MainForm.Instance.LoadUserControl(new StudentProfileForm(_selectedStudentId));
        }

        private void StudentsForm_Load(object sender, EventArgs e)
        {
            ApplyCustomStyles();
            LoadComboBoxes();
            ClearForm();
            LoadStudents();
        }

        private void btnChooseImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "ملفات الصور|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                ofd.Title = "اختر صورة الطالب";

                if (ofd.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    FileInfo fileInfo = new FileInfo(ofd.FileName);

                    if (fileInfo.Length > 2 * 1024 * 1024)
                    {
                        UIHelper.ShowWarning("حجم الصورة كبير جدًا. الرجاء اختيار صورة أصغر من 2 ميجابايت.");
                        return;
                    }

                    byte[] imageBytes = File.ReadAllBytes(ofd.FileName);

                    if (picStudent.Image != null)
                    {
                        picStudent.Image.Dispose();
                        picStudent.Image = null;
                    }

                    using (MemoryStream ms = new MemoryStream(imageBytes))
                    using (Image selectedImage = Image.FromStream(ms))
                    {
                        _selectedPhoto = imageBytes;
                        picStudent.Image = new Bitmap(selectedImage);
                    }
                }
                catch (Exception ex)
                {
                    UIHelper.ShowException("تعذر تحميل الصورة: ", ex);
                }
            }
        }

        private void btnRemoveImage_Click(object sender, EventArgs e)
        {
            _selectedPhoto = null;

            if (picStudent.Image != null)
            {
                picStudent.Image.Dispose();
                picStudent.Image = null;
            }
        }

        private void dgvStudents_SelectionChanged(object sender, EventArgs e)
        {
            Student student = GetSelectedStudentFromGrid();
            if (student != null && student.StudentId != _selectedStudentId)
                FillForm(student);
        }

        private Student GetSelectedStudentFromGrid()
        {
            if (dgvStudents.CurrentRow == null || dgvStudents.CurrentRow.DataBoundItem == null)
                return null;

            DataGridViewCell cell = dgvStudents.CurrentRow.Cells["StudentId"];
            if (cell == null || cell.Value == null || cell.Value == DBNull.Value)
                return null;

            int id;
            if (!int.TryParse(cell.Value.ToString(), out id) || id <= 0)
                return null;

            return _currentStudents.FirstOrDefault(s => s.StudentId == id);
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                ApplyFilters();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
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

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (_selectedStudentId == 0)
            {
                UIHelper.ShowWarning("اختر طالبًا أولاً قبل الطباعة.");
                return;
            }

            Student student = _currentStudents.FirstOrDefault(s => s.StudentId == _selectedStudentId);
            if (student == null)
            {
                UIHelper.ShowWarning("تعذر تحميل بيانات الطالب المحدد للطباعة.");
                return;
            }

            using (PrintDialog dialog = new PrintDialog())
            {
                dialog.Document = _studentCardPrintDocument;
                dialog.UseEXDialog = true;
                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;

                try
                {
                    _studentCardPrintDocument.Print();
                }
                catch (Exception ex)
                {
                    UIHelper.ShowException("تعذر تنفيذ طباعة بطاقة الطالب: ", ex);
                }
            }
        }

        private void StudentCardPrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            Student student = _currentStudents.FirstOrDefault(s => s.StudentId == _selectedStudentId);
            if (student == null)
            {
                e.HasMorePages = false;
                return;
            }

            Rectangle bounds = e.MarginBounds;
            Rectangle card = new Rectangle(bounds.Left, bounds.Top, Math.Min(bounds.Width, 500), 310);
            bool isRtl = ReportOutputHelper.ContainsArabic(student.FullName)
                || ReportOutputHelper.ContainsArabic(student.CurrentClassName)
                || ReportOutputHelper.ContainsArabic(student.Status)
                || ReportOutputHelper.ContainsArabic(student.Gender);
            using (Font titleFont = new Font("Tahoma", 16F, FontStyle.Bold))
            using (Font labelFont = new Font("Tahoma", 10F, FontStyle.Bold))
            using (Font valueFont = new Font("Tahoma", 10F, FontStyle.Regular))
            using (StringFormat rtl = new StringFormat
            {
                Alignment = isRtl ? StringAlignment.Far : StringAlignment.Near,
                LineAlignment = StringAlignment.Center,
                FormatFlags = isRtl ? StringFormatFlags.DirectionRightToLeft : StringFormatFlags.FitBlackBox,
                Trimming = StringTrimming.EllipsisCharacter
            })
            using (Pen borderPen = new Pen(Color.FromArgb(31, 78, 121), 2))
            {
                e.Graphics.FillRectangle(Brushes.White, card);
                e.Graphics.DrawRectangle(borderPen, card);
                e.Graphics.DrawString("بطاقة الطالب | Student Card", titleFont, Brushes.Black,
                    new RectangleF(card.Left + 20, card.Top + 14, card.Width - 40, 32), rtl);

                Rectangle photoBounds = new Rectangle(card.Left + 20, card.Top + 62, 105, 125);
                Image photo = null;
                try
                {
                    byte[] bytes = student.Photo ?? _selectedPhoto;
                    if (bytes != null && bytes.Length > 0)
                    {
                        using (MemoryStream stream = new MemoryStream(bytes))
                        using (Image source = Image.FromStream(stream))
                            photo = new Bitmap(source);
                    }

                    if (photo != null)
                        e.Graphics.DrawImage(photo, photoBounds);
                    else
                        e.Graphics.DrawString("لا توجد صورة | No photo", valueFont, Brushes.DimGray, photoBounds,
                            new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                }
                finally
                {
                    if (photo != null)
                        photo.Dispose();
                }

                int x = card.Left + 140;
                int width = card.Width - 160;
                int y = card.Top + 62;
                DrawStudentCardLine(e.Graphics, x, width, ref y, "الاسم | Name", student.FullName, labelFont, valueFont, rtl);
                DrawStudentCardLine(e.Graphics, x, width, ref y, "رقم الطالب | Student No.", student.StudentNumber, labelFont, valueFont, rtl);
                DrawStudentCardLine(e.Graphics, x, width, ref y, "الصف | Class", student.CurrentClassName, labelFont, valueFont, rtl);
                DrawStudentCardLine(e.Graphics, x, width, ref y, "الجنس | Gender", student.Gender, labelFont, valueFont, rtl);
                DrawStudentCardLine(e.Graphics, x, width, ref y, "الحالة | Status", student.Status, labelFont, valueFont, rtl);
                DrawStudentCardLine(e.Graphics, x, width, ref y, "هاتف ولي الأمر | Guardian Phone", student.GuardianPhone, labelFont, valueFont, rtl);

                e.Graphics.DrawString("تاريخ الإصدار | Issued: " + DateTime.Now.ToString("yyyy/MM/dd"), valueFont, Brushes.DimGray,
                    new RectangleF(card.Left + 20, card.Bottom - 38, card.Width - 40, 24), rtl);
            }

            e.HasMorePages = false;
        }

        private static void DrawStudentCardLine(Graphics graphics, int x, int width, ref int y,
            string label, string value, Font labelFont, Font valueFont, StringFormat rtl)
        {
            graphics.DrawString(label + ":", labelFont, Brushes.Black,
                new RectangleF(x, y, width * 0.35F, 25), rtl);
            graphics.DrawString(value ?? "-", valueFont, Brushes.Black,
                new RectangleF(x + width * 0.35F, y, width * 0.65F, 25), rtl);
            y += 29;
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (_currentStudents == null || _currentStudents.Count == 0)
                {
                    UIHelper.ShowWarning("لا توجد بيانات لتصديرها.");
                    return;
                }

                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "ملفات Excel (*.xlsx)|*.xlsx";
                    sfd.FileName = "Students_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".xlsx";

                    if (sfd.ShowDialog() != DialogResult.OK)
                        return;

                    System.Data.DataTable exportTable = new System.Data.DataTable();
                    exportTable.Columns.Add("رقم الطالب | Student No.");
                    exportTable.Columns.Add("الاسم | Name");
                    exportTable.Columns.Add("الجنس | Gender");
                    exportTable.Columns.Add("الصف | Class");
                    exportTable.Columns.Add("الهاتف | Phone");
                    exportTable.Columns.Add("الحالة | Status");
                    foreach (Student s in _currentStudents)
                    {
                        exportTable.Rows.Add(
                            s.StudentNumber ?? string.Empty,
                            s.FullName ?? string.Empty,
                            s.Gender ?? string.Empty,
                            s.CurrentClassName ?? string.Empty,
                            s.StudentPhone ?? string.Empty,
                            s.Status ?? string.Empty);
                    }
                    ReportOutputHelper.ExportToExcel(
                        exportTable,
                        sfd.FileName,
                        "نظام إدارة المدرسة | School Management System - Students",
                        "إجمالي الطلاب | Total students: " + exportTable.Rows.Count);
                    UIHelper.ShowInfo("تم تصدير بيانات الطلاب إلى Excel بنجاح.");
                }

            }
            catch (Exception ex)
            {
                UIHelper.ShowException("فشل التصدير: ", ex);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (_selectedStudentId == 0)
            {
                UIHelper.ShowWarning("اختر طالبًا من الجدول أولاً.");
                return;
            }

            btnSave_Click(sender, e);
        }

        private void ApplyCustomStyles()
        {
            UIHelper.StyleDataGridView(dgvStudents);
            
            // Style Buttons
            UIHelper.StylePrimaryButton(btnSave);
            UIHelper.StylePrimaryButton(btnAdd);
            UIHelper.StylePrimaryButton(btnUpdate);
            UIHelper.StyleDangerButton(btnDelete);
            UIHelper.StylePrimaryButton(btnSearch);
            UIHelper.StylePrimaryButton(btnRefresh);
            UIHelper.StylePrimaryButton(btnExportExcel);
            UIHelper.StylePrimaryButton(btnPrint);
            
            // Style Inputs
            TextBox[] textBoxes = { txtFullName, txtNationalId, txtPhone, txtGuardianName, txtGuardianPhone, txtSearch };
            foreach (var tb in textBoxes) UIHelper.StyleTextBox(tb);
            
            ComboBox[] combos = { cmbGender, cmbStatus, cmbFilterClass, cmbFilterStatus };
            foreach (var cb in combos) UIHelper.StyleComboBox(cb);
        }

        private void LoadComboBoxes()
        {
            cmbGender.Items.Clear();
            cmbGender.Items.AddRange(new[] { "ذكر", "أنثى" });

            cmbStatus.Items.Clear();
            cmbStatus.Items.AddRange(new[] { "نشط", "موقوف", "منقول", "متخرج" });

            cmbFilterStatus.Items.Clear();
            cmbFilterStatus.Items.AddRange(new[] { "كل الحالات", "نشط", "موقوف", "منقول", "متخرج" });
            cmbFilterStatus.SelectedIndex = 0;

            cmbFilterClass.Items.Clear();
            cmbFilterClass.Items.Add("كل الصفوف");
            cmbFilterClass.SelectedIndex = 0;
        }

        private void LoadStudents()
        {
            try
            {
                _currentStudents = _studentService.GetAll();
                PopulateClassFilter();
                ApplyFilters();
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("خطأ في تحميل البيانات: ", ex);
            }
        }

        private void PopulateClassFilter()
        {
            string selectedClass = cmbFilterClass.Text;
            var classNames = _currentStudents
                .Select(s => s.CurrentClassName)
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(name => name)
                .ToList();

            cmbFilterClass.Items.Clear();
            cmbFilterClass.Items.Add("كل الصفوف");
            foreach (string className in classNames)
                cmbFilterClass.Items.Add(className);

            int selectedIndex = cmbFilterClass.Items.IndexOf(selectedClass);
            cmbFilterClass.SelectedIndex = selectedIndex >= 0 ? selectedIndex : 0;
        }

        private void ApplyFilters()
        {
            var filtered = _currentStudents.AsEnumerable();
            
            string search = txtSearch.Text.Trim();
            if (!string.IsNullOrEmpty(search))
            {
                filtered = filtered.Where(s => 
                    (s.FullName != null && s.FullName.Contains(search)) ||
                    (s.StudentNumber != null && s.StudentNumber.Contains(search)) ||
                    (s.NationalId != null && s.NationalId.Contains(search)) ||
                    (s.StudentPhone != null && s.StudentPhone.Contains(search))
                );
            }

            if (cmbFilterStatus.SelectedIndex > 0)
                filtered = filtered.Where(s => s.Status == cmbFilterStatus.Text);

            if (cmbFilterClass.SelectedIndex > 0 && !string.IsNullOrWhiteSpace(cmbFilterClass.Text))
                filtered = filtered.Where(s => string.Equals(s.CurrentClassName, cmbFilterClass.Text, StringComparison.OrdinalIgnoreCase));

            BindGrid(filtered.ToList());
        }

        private void BindGrid(List<Student> students)
        {
            dgvStudents.DataSource = students.Select(s => new {
                s.StudentId,
                s.StudentNumber,
                s.FullName,
                s.Gender,
                s.CurrentClassName,
                s.StudentPhone,
                s.Status
            }).ToList();

            if (dgvStudents.Columns["StudentId"] != null) dgvStudents.Columns["StudentId"].Visible = false;
            
            dgvStudents.Columns["StudentNumber"].HeaderText = "رقم الطالب";
            dgvStudents.Columns["FullName"].HeaderText = "الاسم";
            dgvStudents.Columns["Gender"].HeaderText = "الجنس";
            dgvStudents.Columns["CurrentClassName"].HeaderText = "الصف";
            dgvStudents.Columns["StudentPhone"].HeaderText = "الهاتف";
            dgvStudents.Columns["Status"].HeaderText = "الحالة";

            lblCount.Text = $"عدد الطلاب: {students.Count}";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_isSaving)
                return;

            _isSaving = true;
            btnSave.Enabled = false;
            try
            {
                var student = GetStudentFromForm();
                if (_selectedStudentId == 0)
                    _studentService.Add(student);
                else
                    _studentService.Update(student);

                UIHelper.ShowInfo(string.IsNullOrWhiteSpace(student.StudentNumber)
                    ? "تم حفظ بيانات الطالب بنجاح."
                    : "تم حفظ بيانات الطالب بنجاح. رقم الطالب المولد: " + student.StudentNumber);
                LoadStudents();
                ClearForm();
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("العملية المطلوبة", ex);
            }
            finally
            {
                _isSaving = false;
                btnSave.Enabled = true;
            }
        }

                private Student GetStudentFromForm()
        {
            string fullName = txtFullName.Text.Trim();
            if (!UIHelper.IsValidArabicOrLatinName(fullName))
                throw new Exception("اسم الطالب مطلوب ويجب أن يحتوي على أحرف فقط وبطول مناسب.");

            if (cmbGender.SelectedIndex < 0 || string.IsNullOrWhiteSpace(cmbGender.Text))
                throw new Exception("يرجى اختيار جنس الطالب.");

            if (cmbStatus.SelectedIndex < 0 || string.IsNullOrWhiteSpace(cmbStatus.Text))
                throw new Exception("يرجى اختيار حالة الطالب.");

            string[] textValues = { txtBirthPlace.Text, txtNationality.Text, txtGovernorate.Text, txtDistrict.Text, txtGuardianName.Text };
            foreach (string value in textValues)
            {
                if (!string.IsNullOrWhiteSpace(value) && !UIHelper.IsValidArabicOrLatinName(value, 2))
                    throw new Exception("الحقول النصية لا يجب أن تحتوي على أرقام أو رموز غير صالحة.");
            }

            string nationalId = txtNationalId.Text.Trim();
            int ignoredId;
            if (!string.IsNullOrEmpty(nationalId) && (!UIHelper.IsValidPositiveInteger(nationalId, out ignoredId) || nationalId.Length < 6 || nationalId.Length > 20))
                throw new Exception("رقم الهوية يجب أن يتكون من أرقام فقط وبطول من 6 إلى 20 رقماً.");

            string phone = txtPhone.Text.Trim();
            if (!string.IsNullOrEmpty(phone) && !UIHelper.IsValidPhone(phone))
                throw new Exception("رقم هاتف الطالب غير صحيح.");

            string guardianPhone = txtGuardianPhone.Text.Trim();
            if (!string.IsNullOrEmpty(guardianPhone) && !UIHelper.IsValidPhone(guardianPhone))
                throw new Exception("رقم هاتف ولي الأمر غير صحيح.");

            string guardianEmail = txtGuardianEmail.Text.Trim();
            if (!string.IsNullOrEmpty(guardianEmail) && !UIHelper.IsValidEmail(guardianEmail))
                throw new Exception("البريد الإلكتروني لولي الأمر غير صحيح.");

            if (dtpBirthDate.Value > DateTime.Today)
                throw new Exception("تاريخ الميلاد لا يمكن أن يكون في المستقبل.");

            return new Student
            {
                StudentId = _selectedStudentId,
                FullName = fullName,
                Gender = cmbGender.Text,
                BirthDate = dtpBirthDate.Value,
                BirthPlace = txtBirthPlace.Text.Trim(),
                Nationality = txtNationality.Text.Trim(),
                NationalId = txtNationalId.Text.Trim(),
                StudentPhone = phone,
                Status = cmbStatus.Text,
                GuardianName = txtGuardianName.Text.Trim(),
                GuardianRelation = txtGuardianRelation.Text.Trim(),
                GuardianPhone = guardianPhone,
                GuardianEmail = txtGuardianEmail.Text.Trim(),
                GuardianJob = txtGuardianJob.Text.Trim(),
                Governorate = txtGovernorate.Text.Trim(),
                District = txtDistrict.Text.Trim(),
                Address = txtAddress.Text.Trim(),
                Photo = _selectedPhoto
            };
        }

        private void ClearForm()
        {
            _selectedStudentId = 0;
            _selectedPhoto = null;

            txtStudentNumber.Text = "جاري تجهيز الرقم...";
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
            dtpBirthDate.Value = DateTime.Today.AddYears(-6);
        }

        private void TryPreviewNextStudentNumber()
        {
            if (_studentService == null || _selectedStudentId != 0)
                return;

            try
            {
                string nextNumber = _studentService.GenerateNextStudentNumber();
                txtStudentNumber.Text = string.IsNullOrWhiteSpace(nextNumber)
                    ? "يُولّد تلقائياً عند الحفظ"
                    : nextNumber;
            }
            catch
            {
                // المعاينة تحسين واجهة فقط؛ يبقى التوليد النهائي ذرياً داخل المستودع.
                txtStudentNumber.Text = "يُولّد تلقائياً عند الحفظ";
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e) => LoadStudents();
        private void btnSearch_Click(object sender, EventArgs e) => ApplyFilters();
        private void cmbFilterClass_SelectedIndexChanged(object sender, EventArgs e) => ApplyFilters();
        private void cmbFilterStatus_SelectedIndexChanged(object sender, EventArgs e) => ApplyFilters();
        private void btnAdd_Click(object sender, EventArgs e)
        {
            ClearForm();
            txtFullName.Focus();
        }

        private void dgvStudents_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvStudents.Rows.Count)
                return;

            DataGridViewCell cell = dgvStudents.Rows[e.RowIndex].Cells["StudentId"];
            if (cell == null || cell.Value == null || cell.Value == DBNull.Value)
                return;

            int id;
            if (!int.TryParse(cell.Value.ToString(), out id) || id <= 0)
                return;

            Student student = _currentStudents.FirstOrDefault(s => s.StudentId == id);
            if (student != null)
                FillForm(student);
        }

        private void FillForm(Student s)
        {
            _selectedStudentId = s.StudentId;
            _selectedPhoto = s.Photo == null ? null : (byte[])s.Photo.Clone();

            txtStudentNumber.Text = s.StudentNumber ?? "";
            txtFullName.Text = s.FullName ?? "";
            txtBirthPlace.Text = s.BirthPlace ?? "";
            txtNationality.Text = s.Nationality ?? "";
            txtNationalId.Text = s.NationalId ?? "";
            txtPhone.Text = s.StudentPhone ?? "";
            cmbGender.Text = s.Gender ?? "";
            cmbStatus.Text = s.Status ?? "";
            dtpBirthDate.Value = s.BirthDate.HasValue && s.BirthDate.Value <= DateTime.Today
                ? s.BirthDate.Value
                : DateTime.Today.AddYears(-6);

            txtGuardianName.Text = s.GuardianName ?? "";
            txtGuardianRelation.Text = s.GuardianRelation ?? "";
            txtGuardianPhone.Text = s.GuardianPhone ?? "";
            txtGuardianEmail.Text = s.GuardianEmail ?? "";
            txtGuardianJob.Text = s.GuardianJob ?? "";
            txtGovernorate.Text = s.Governorate ?? "";
            txtDistrict.Text = s.District ?? "";
            txtAddress.Text = s.Address ?? "";

            if (picStudent.Image != null)
            {
                picStudent.Image.Dispose();
                picStudent.Image = null;
            }

            if (_selectedPhoto != null && _selectedPhoto.Length > 0)
            {
                using (var ms = new MemoryStream(_selectedPhoto))
                using (var source = Image.FromStream(ms))
                    picStudent.Image = new Bitmap(source);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedStudentId == 0) return;
            if (MessageBox.Show(
                "هل أنت متأكد من حذف هذا الطالب؟",
                "تأكيد الحذف",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2,
                MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading) == DialogResult.Yes)
            {
                try {
                    _studentService.Delete(_selectedStudentId);
                    LoadStudents();
                    ClearForm();
                } catch (Exception ex) { UIHelper.ShowException("العملية المطلوبة", ex); }
            }
        }
    }
}
