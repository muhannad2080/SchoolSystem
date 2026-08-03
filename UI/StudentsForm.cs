using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
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
        }

        private void StudentsForm_Load(object sender, EventArgs e)
        {
            ApplyCustomStyles();
            LoadComboBoxes();
            ClearForm();
            LoadStudents();
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
                ApplyFilters();
            }
            catch (Exception ex)
            {
                UIHelper.ShowError("خطأ في تحميل البيانات: " + ex.Message);
            }
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
            try
            {
                var student = GetStudentFromForm();
                if (_selectedStudentId == 0)
                    _studentService.Add(student);
                else
                    _studentService.Update(student);

                UIHelper.ShowInfo("تم حفظ البيانات بنجاح.");
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
            return new Student {
                StudentId = _selectedStudentId,
                FullName = txtFullName.Text.Trim(),
                Gender = cmbGender.Text,
                BirthDate = dtpBirthDate.Value,
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

        private void ClearForm()
        {
            _selectedStudentId = 0;
            _selectedPhoto = null;
            txtFullName.Clear();
            txtNationalId.Clear();
            txtPhone.Clear();
            txtGuardianName.Clear();
            txtGuardianPhone.Clear();
            picStudent.Image = null;
            cmbGender.SelectedIndex = -1;
            cmbStatus.SelectedIndex = 0;
            dtpBirthDate.Value = DateTime.Today.AddYears(-6);
        }

        private void btnRefresh_Click(object sender, EventArgs e) => LoadStudents();
        private void btnSearch_Click(object sender, EventArgs e) => ApplyFilters();
        private void btnAdd_Click(object sender, EventArgs e) => ClearForm();

        private void dgvStudents_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            int id = (int)dgvStudents.Rows[e.RowIndex].Cells["StudentId"].Value;
            var student = _currentStudents.FirstOrDefault(s => s.StudentId == id);
            if (student != null) FillForm(student);
        }

        private void FillForm(Student s)
        {
            _selectedStudentId = s.StudentId;
            txtFullName.Text = s.FullName;
            txtNationalId.Text = s.NationalId;
            txtPhone.Text = s.StudentPhone;
            cmbGender.Text = s.Gender;
            cmbStatus.Text = s.Status;
            dtpBirthDate.Value = s.BirthDate ?? DateTime.Today;
            
            txtGuardianName.Text = s.GuardianName;
            txtGuardianPhone.Text = s.GuardianPhone;
            
            if (s.Photo != null)
            {
                using (var ms = new MemoryStream(s.Photo))
                    picStudent.Image = Image.FromStream(ms);
            }
            else picStudent.Image = null;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedStudentId == 0) return;
            if (MessageBox.Show("هل أنت متأكد من حذف هذا الطالب؟", "تأكيد", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try {
                    _studentService.Delete(_selectedStudentId);
                    LoadStudents();
                    ClearForm();
                } catch (Exception ex) { UIHelper.ShowError(ex.Message); }
            }
        }
    }
}
