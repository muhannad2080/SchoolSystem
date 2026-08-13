using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
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

        public StudentsForm()
        {
            InitializeComponent();

            _studentService = new StudentService();
            _currentStudents = new List<Student>();

            UIHelper.ApplyStyle(this);
            ApplyCustomStyles();
            WireValidationEvents();
        }

        private void StudentsForm_Load(object sender, EventArgs e)
        {
            LoadComboBoxes();
            ClearForm();
            LoadStudents();
        }

        private void ApplyCustomStyles()
        {
            UIHelper.StyleDataGridView(dgvStudents);

            UIHelper.StyleButton(btnAdd, UIHelper.PrimaryColor);
            UIHelper.StyleButton(btnSave, UIHelper.SuccessColor);
            UIHelper.StyleButton(btnUpdate, UIHelper.SearchColor);
            UIHelper.StyleButton(btnDelete, UIHelper.DangerColor);
            UIHelper.StyleButton(btnCancel, UIHelper.NeutralColor);
            UIHelper.StyleButton(btnRefresh, UIHelper.NeutralColor);
            UIHelper.StyleButton(btnExportExcel, UIHelper.ExportColor);
            UIHelper.StyleButton(btnPrint, UIHelper.AccentColor);
            UIHelper.StyleButton(btnClose, UIHelper.NeutralColor);
            UIHelper.StyleButton(btnSearch, UIHelper.SearchColor);
            UIHelper.StyleButton(btnReload, UIHelper.NeutralColor);

            TextBox[] textBoxes =
            {
                txtStudentNumber,
                txtFullName,
                txtBirthPlace,
                txtNationality,
                txtNationalId,
                txtPhone,
                txtGuardianName,
                txtGuardianRelation,
                txtGuardianPhone,
                txtGuardianEmail,
                txtGuardianJob,
                txtGovernorate,
                txtDistrict,
                txtAddress,
                txtSearch
            };

            foreach (TextBox tb in textBoxes)
                UIHelper.StyleTextBox(tb);

            ComboBox[] combos =
            {
                cmbGender,
                cmbStatus,
                cmbFilterClass,
                cmbFilterStatus
            };

            foreach (ComboBox cb in combos)
                UIHelper.StyleComboBox(cb);

            txtStudentNumber.ReadOnly = true;
            txtStudentNumber.BackColor = Color.FromArgb(241, 245, 249);

            picStudent.BackColor = UIHelper.BackgroundColor;
            picStudent.BorderStyle = BorderStyle.FixedSingle;
            picStudent.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void WireValidationEvents()
        {
            UIHelper.PreventNumbers(txtFullName);
            UIHelper.PreventNumbers(txtBirthPlace);
            UIHelper.PreventNumbers(txtNationality);
            UIHelper.PreventNumbers(txtGuardianName);
            UIHelper.PreventNumbers(txtGuardianRelation);
            UIHelper.PreventNumbers(txtGuardianJob);
            UIHelper.PreventNumbers(txtGovernorate);
            UIHelper.PreventNumbers(txtDistrict);

            UIHelper.AllowOnlyNumbers(txtNationalId);
            UIHelper.AllowOnlyNumbers(txtPhone);
            UIHelper.AllowOnlyNumbers(txtGuardianPhone);
        }

        private void LoadComboBoxes()
        {
            cmbGender.Items.Clear();
            cmbGender.Items.AddRange(new object[] { "ذكر", "أنثى" });
            if (cmbGender.Items.Count > 0)
                cmbGender.SelectedIndex = 0;

            cmbStatus.Items.Clear();
            cmbStatus.Items.AddRange(new object[] { "نشط", "موقوف", "منقول", "متخرج" });
            if (cmbStatus.Items.Count > 0)
                cmbStatus.SelectedIndex = 0;

            cmbFilterStatus.Items.Clear();
            cmbFilterStatus.Items.AddRange(new object[] { "كل الحالات", "نشط", "موقوف", "منقول", "متخرج" });
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

                if (_currentStudents == null)
                    _currentStudents = new List<Student>();

                ApplyFilters();
            }
            catch (Exception ex)
            {
                UIHelper.ShowError("خطأ في تحميل بيانات الطلاب:\n" + ex.Message);
            }
        }

        private void ApplyFilters()
        {
            IEnumerable<Student> filtered = _currentStudents.AsEnumerable();

            string search = txtSearch.Text.Trim();

            if (!string.IsNullOrWhiteSpace(search))
            {
                filtered = filtered.Where(s =>
                    ContainsText(s.FullName, search) ||
                    ContainsText(s.StudentNumber, search) ||
                    ContainsText(s.NationalId, search) ||
                    ContainsText(s.StudentPhone, search));
            }

            if (cmbFilterStatus.SelectedIndex > 0)
                filtered = filtered.Where(s => s.Status == cmbFilterStatus.Text);

            BindGrid(filtered.ToList());
        }

        private bool ContainsText(string source, string keyword)
        {
            if (string.IsNullOrWhiteSpace(source))
                return false;

            return source.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private void BindGrid(List<Student> students)
        {
            dgvStudents.DataSource = null;

            dgvStudents.DataSource = students.Select(s => new
            {
                s.StudentId,
                s.StudentNumber,
                s.FullName,
                s.Gender,
                s.CurrentClassName,
                s.StudentPhone,
                s.Status
            }).ToList();

            if (dgvStudents.Columns["StudentId"] != null)
                dgvStudents.Columns["StudentId"].Visible = false;

            if (dgvStudents.Columns["StudentNumber"] != null)
                dgvStudents.Columns["StudentNumber"].HeaderText = "رقم الطالب";

            if (dgvStudents.Columns["FullName"] != null)
                dgvStudents.Columns["FullName"].HeaderText = "الاسم";

            if (dgvStudents.Columns["Gender"] != null)
                dgvStudents.Columns["Gender"].HeaderText = "الجنس";

            if (dgvStudents.Columns["CurrentClassName"] != null)
                dgvStudents.Columns["CurrentClassName"].HeaderText = "الصف";

            if (dgvStudents.Columns["StudentPhone"] != null)
                dgvStudents.Columns["StudentPhone"].HeaderText = "الهاتف";

            if (dgvStudents.Columns["Status"] != null)
                dgvStudents.Columns["Status"].HeaderText = "الحالة";

            lblCount.Text = "عدد الطلاب: " + students.Count;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveStudent();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (_selectedStudentId == 0)
            {
                UIHelper.ShowWarning("اختر طالبًا من الجدول أولًا.");
                return;
            }

            SaveStudent();
        }

        private void SaveStudent()
        {
            try
            {
                Student student = GetStudentFromForm();

                if (_selectedStudentId == 0)
                {
                    _studentService.Add(student);
                    UIHelper.ShowSuccess("تمت إضافة الطالب بنجاح.");
                }
                else
                {
                    _studentService.Update(student);
                    UIHelper.ShowSuccess("تم تعديل بيانات الطالب بنجاح.");
                }

                LoadStudents();
                ClearForm();
            }
            catch (Exception ex)
            {
                UIHelper.ShowError(ex.Message);
            }
        }

        private Student GetStudentFromForm()
        {
            ValidateForm();

            return new Student
            {
                StudentId = _selectedStudentId,
                StudentNumber = txtStudentNumber.Text.Trim(),
                FullName = txtFullName.Text.Trim(),
                Gender = cmbGender.Text,
                BirthDate = dtpBirthDate.Value.Date,
                BirthPlace = txtBirthPlace.Text.Trim(),
                Nationality = txtNationality.Text.Trim(),
                NationalId = txtNationalId.Text.Trim(),
                StudentPhone = txtPhone.Text.Trim(),
                Status = cmbStatus.Text,
                GuardianName = txtGuardianName.Text.Trim(),
                GuardianRelation = txtGuardianRelation.Text.Trim(),
                GuardianPhone = txtGuardianPhone.Text.Trim(),
                GuardianEmail = txtGuardianEmail.Text.Trim(),
                GuardianJob = txtGuardianJob.Text.Trim(),
                Governorate = txtGovernorate.Text.Trim(),
                District = txtDistrict.Text.Trim(),
                Address = txtAddress.Text.Trim(),
                Photo = _selectedPhoto
            };
        }

        private void ValidateForm()
        {
            errorProvider1.Clear();

            string fullName = txtFullName.Text.Trim();
            if (string.IsNullOrWhiteSpace(fullName))
            {
                errorProvider1.SetError(txtFullName, "اسم الطالب مطلوب.");
                txtFullName.Focus();
                throw new Exception("اسم الطالب مطلوب.");
            }

            if (fullName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Length < 4)
            {
                errorProvider1.SetError(txtFullName, "يرجى إدخال الاسم الرباعي.");
                txtFullName.Focus();
                throw new Exception("يرجى إدخال الاسم الرباعي.");
            }

            if (Regex.IsMatch(fullName, @"\d"))
            {
                errorProvider1.SetError(txtFullName, "اسم الطالب لا يجب أن يحتوي على أرقام.");
                txtFullName.Focus();
                throw new Exception("اسم الطالب لا يجب أن يحتوي على أرقام.");
            }

            if (cmbGender.SelectedIndex < 0)
                throw new Exception("يرجى اختيار الجنس.");

            if (cmbStatus.SelectedIndex < 0)
                throw new Exception("يرجى اختيار حالة الطالب.");

            if (dtpBirthDate.Value.Date > DateTime.Today)
            {
                errorProvider1.SetError(dtpBirthDate, "تاريخ الميلاد لا يمكن أن يكون في المستقبل.");
                dtpBirthDate.Focus();
                throw new Exception("تاريخ الميلاد لا يمكن أن يكون في المستقبل.");
            }

            int age = DateTime.Today.Year - dtpBirthDate.Value.Year;
            if (dtpBirthDate.Value.Date > DateTime.Today.AddYears(-age))
                age--;

            if (age < 4 || age > 30)
            {
                errorProvider1.SetError(dtpBirthDate, "العمر غير منطقي لطالب مدرسي.");
                dtpBirthDate.Focus();
                throw new Exception("العمر غير منطقي لطالب مدرسي.");
            }

            string phone = txtPhone.Text.Trim();
            if (!string.IsNullOrWhiteSpace(phone) && !Regex.IsMatch(phone, @"^[0-9]{9,15}$"))
            {
                errorProvider1.SetError(txtPhone, "رقم هاتف الطالب يجب أن يكون من 9 إلى 15 رقم.");
                txtPhone.Focus();
                throw new Exception("رقم هاتف الطالب غير صحيح.");
            }

            string nationalId = txtNationalId.Text.Trim();
            if (!string.IsNullOrWhiteSpace(nationalId) && !Regex.IsMatch(nationalId, @"^[0-9]{5,30}$"))
            {
                errorProvider1.SetError(txtNationalId, "رقم الهوية يجب أن يحتوي على أرقام فقط.");
                txtNationalId.Focus();
                throw new Exception("رقم الهوية غير صحيح.");
            }

            string guardianPhone = txtGuardianPhone.Text.Trim();
            if (!string.IsNullOrWhiteSpace(guardianPhone) && !Regex.IsMatch(guardianPhone, @"^[0-9]{9,15}$"))
            {
                errorProvider1.SetError(txtGuardianPhone, "رقم هاتف ولي الأمر يجب أن يكون من 9 إلى 15 رقم.");
                txtGuardianPhone.Focus();
                throw new Exception("رقم هاتف ولي الأمر غير صحيح.");
            }

            string guardianEmail = txtGuardianEmail.Text.Trim();
            if (!string.IsNullOrWhiteSpace(guardianEmail) &&
                !Regex.IsMatch(guardianEmail, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                errorProvider1.SetError(txtGuardianEmail, "البريد الإلكتروني غير صحيح.");
                txtGuardianEmail.Focus();
                throw new Exception("البريد الإلكتروني غير صحيح.");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedStudentId == 0)
            {
                UIHelper.ShowWarning("اختر طالبًا من الجدول أولًا.");
                return;
            }

            DialogResult result = MessageBox.Show(
                "هل أنت متأكد من حذف هذا الطالب؟",
                "تأكيد الحذف",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2,
                MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);

            if (result != DialogResult.Yes)
                return;

            try
            {
                _studentService.Delete(_selectedStudentId);
                UIHelper.ShowSuccess("تم حذف الطالب بنجاح.");
                LoadStudents();
                ClearForm();
            }
            catch (Exception ex)
            {
                UIHelper.ShowError(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadStudents();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                ApplyFilters();
            }
        }

        private void btnChooseImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "ملفات الصور|*.jpg;*.jpeg;*.png;*.bmp";
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

                    DisposeStudentImage();

                    using (MemoryStream ms = new MemoryStream(imageBytes))
                    using (Image img = Image.FromStream(ms))
                    {
                        _selectedPhoto = imageBytes;
                        picStudent.Image = new Bitmap(img);
                    }
                }
                catch (Exception ex)
                {
                    UIHelper.ShowError("تعذر تحميل الصورة:\n" + ex.Message);
                }
            }
        }

        private void btnRemoveImage_Click(object sender, EventArgs e)
        {
            _selectedPhoto = null;
            DisposeStudentImage();
        }

        private void DisposeStudentImage()
        {
            if (picStudent.Image != null)
            {
                Image oldImage = picStudent.Image;
                picStudent.Image = null;
                oldImage.Dispose();
            }
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

                    using (var workbook = new ClosedXML.Excel.XLWorkbook())
                    {
                        var ws = workbook.Worksheets.Add("الطلاب");
                        ws.RightToLeft = true;

                        ws.Cell(1, 1).Value = "رقم الطالب";
                        ws.Cell(1, 2).Value = "الاسم";
                        ws.Cell(1, 3).Value = "الجنس";
                        ws.Cell(1, 4).Value = "الصف";
                        ws.Cell(1, 5).Value = "الهاتف";
                        ws.Cell(1, 6).Value = "الحالة";

                        for (int i = 0; i < _currentStudents.Count; i++)
                        {
                            Student s = _currentStudents[i];
                            int row = i + 2;

                            ws.Cell(row, 1).Value = s.StudentNumber;
                            ws.Cell(row, 2).Value = s.FullName;
                            ws.Cell(row, 3).Value = s.Gender;
                            ws.Cell(row, 4).Value = s.CurrentClassName;
                            ws.Cell(row, 5).Value = s.StudentPhone;
                            ws.Cell(row, 6).Value = s.Status;
                        }

                        ws.Row(1).Style.Font.Bold = true;
                        ws.Row(1).Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.FromHtml("#1E293B");
                        ws.Row(1).Style.Font.FontColor = ClosedXML.Excel.XLColor.White;
                        ws.Columns().AdjustToContents();

                        workbook.SaveAs(sfd.FileName);
                    }
                }

                UIHelper.ShowSuccess("تم تصدير البيانات بنجاح.");
            }
            catch (Exception ex)
            {
                UIHelper.ShowError("فشل التصدير:\n" + ex.Message);
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (_selectedStudentId == 0)
            {
                UIHelper.ShowWarning("اختر طالبًا أولًا قبل الطباعة.");
                return;
            }

            UIHelper.ShowInfo("ميزة طباعة بطاقة الطالب سيتم ربطها لاحقًا بمركز التقارير.");
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

        private void dgvStudents_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            LoadSelectedStudentFromGridRow(e.RowIndex);
        }

        private void dgvStudents_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvStudents.CurrentRow == null || dgvStudents.CurrentRow.Index < 0)
                return;

            LoadSelectedStudentFromGridRow(dgvStudents.CurrentRow.Index);
        }

        private void LoadSelectedStudentFromGridRow(int rowIndex)
        {
            try
            {
                if (rowIndex < 0 || rowIndex >= dgvStudents.Rows.Count)
                    return;

                object value = dgvStudents.Rows[rowIndex].Cells["StudentId"].Value;

                if (value == null)
                    return;

                int id;
                if (!int.TryParse(value.ToString(), out id))
                    return;

                Student student = _currentStudents.FirstOrDefault(s => s.StudentId == id);

                if (student != null && student.StudentId != _selectedStudentId)
                    FillForm(student);
            }
            catch
            {
                // تجاهل أخطاء اختيار الصف حتى لا تنهار الواجهة
            }
        }

        private void FillForm(Student s)
        {
            if (s == null)
                return;

            _selectedStudentId = s.StudentId;

            txtStudentNumber.Text = s.StudentNumber ?? "";
            txtFullName.Text = s.FullName ?? "";
            txtNationalId.Text = s.NationalId ?? "";
            txtPhone.Text = s.StudentPhone ?? "";

            SetComboValue(cmbGender, s.Gender);
            SetComboValue(cmbStatus, s.Status);

            dtpBirthDate.Value = s.BirthDate ?? DateTime.Today.AddYears(-6);

            txtBirthPlace.Text = s.BirthPlace ?? "";
            txtNationality.Text = s.Nationality ?? "";
            txtGuardianName.Text = s.GuardianName ?? "";
            txtGuardianRelation.Text = s.GuardianRelation ?? "";
            txtGuardianPhone.Text = s.GuardianPhone ?? "";
            txtGuardianEmail.Text = s.GuardianEmail ?? "";
            txtGuardianJob.Text = s.GuardianJob ?? "";
            txtGovernorate.Text = s.Governorate ?? "";
            txtDistrict.Text = s.District ?? "";
            txtAddress.Text = s.Address ?? "";

            _selectedPhoto = s.Photo;

            DisposeStudentImage();

            if (s.Photo != null && s.Photo.Length > 0)
            {
                using (MemoryStream ms = new MemoryStream(s.Photo))
                using (Image img = Image.FromStream(ms))
                {
                    picStudent.Image = new Bitmap(img);
                }
            }
        }

        private void SetComboValue(ComboBox combo, string value)
        {
            if (combo == null)
                return;

            if (string.IsNullOrWhiteSpace(value))
            {
                if (combo.Items.Count > 0)
                    combo.SelectedIndex = 0;

                return;
            }

            if (combo.Items.Contains(value))
                combo.SelectedItem = value;
            else if (combo.Items.Count > 0)
                combo.SelectedIndex = 0;
        }

        private void ClearForm()
        {
            _selectedStudentId = 0;
            _selectedPhoto = null;

            txtStudentNumber.Clear();
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

            DisposeStudentImage();

            if (cmbGender.Items.Count > 0)
                cmbGender.SelectedIndex = 0;

            if (cmbStatus.Items.Count > 0)
                cmbStatus.SelectedIndex = 0;

            dtpBirthDate.Value = DateTime.Today.AddYears(-6);

            errorProvider1.Clear();

            txtFullName.Focus();
        }
    }
}
