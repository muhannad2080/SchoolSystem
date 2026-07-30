using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SchoolSystem.Models;
using SchoolSystem.Services;

namespace SchoolSystem.UI
{
    public partial class StudentsForm : Form
    {
        private readonly StudentService _studentService;
        private int _selectedStudentId;
        private byte[] _selectedPhoto;
        private List<Student> _currentStudents;

        private readonly Color _primaryColor = Color.FromArgb(21, 101, 192);
        private readonly Color _dangerColor = Color.FromArgb(198, 40, 40);
        private readonly Color _neutralColor = Color.FromArgb(80, 80, 80);

        public StudentsForm()
        {
            InitializeComponent();

            _studentService = new StudentService();
            _currentStudents = new List<Student>();

            ApplyModernStyle();
            ApplyArabicDirection();
        }

        private void StudentsForm_Load(object sender, EventArgs e)
        {
            LoadComboBoxes();
            ClearForm();
            LoadStudents();
            AdjustSplitContainer();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            AdjustSplitContainer();
        }

        private void AdjustSplitContainer()
        {
            try
            {
                if (splitContainerMain == null)
                    return;

                if (splitContainerMain.Width <= 800)
                    return;

                int rightPanelWidth = 450;
                int distance = splitContainerMain.Width - rightPanelWidth;

                if (distance > 300)
                    splitContainerMain.SplitterDistance = distance;
            }
            catch
            {
            }
        }

        private void ApplyModernStyle()
        {
            this.BackColor = Color.FromArgb(245, 247, 250);

            // Styling Input Controls
            TextBox[] textBoxes = { txtStudentNumber, txtFullName, txtBirthPlace, txtNationality, txtNationalId, txtPhone, 
                                    txtGuardianName, txtGuardianRelation, txtGuardianPhone, txtGuardianEmail, txtGuardianJob, 
                                    txtGovernorate, txtDistrict, txtAddress, txtSearch };
            foreach (var tb in textBoxes)
                StyleInput(tb);

            ComboBox[] comboBoxes = { cmbGender, cmbStatus, cmbFilterClass, cmbFilterStatus };
            foreach (var cb in comboBoxes)
                StyleCombo(cb);

            StylePrimaryButton(btnChooseImage);
            StyleDangerButton(btnRemoveImage);
            StylePrimaryButton(btnSearch);
            StylePrimaryButton(btnReload);

            StylePrimaryButton(btnAdd);
            StylePrimaryButton(btnUpdate);
            StyleDangerButton(btnDelete);
            StylePrimaryButton(btnSave);
            StyleNeutralButton(btnCancel);
            StylePrimaryButton(btnRefresh);
            StylePrimaryButton(btnExportExcel);
            StylePrimaryButton(btnPrint);
            StyleNeutralButton(btnClose);

            dgvStudents.ColumnHeadersDefaultCellStyle.BackColor = _primaryColor;
            dgvStudents.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvStudents.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvStudents.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            dgvStudents.DefaultCellStyle.SelectionBackColor = Color.FromArgb(187, 222, 251);
            dgvStudents.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvStudents.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 249, 255);
            dgvStudents.GridColor = Color.FromArgb(225, 225, 225);
        }

        private void ApplyArabicDirection()
        {
            this.RightToLeft = RightToLeft.Yes;
            this.RightToLeftLayout = true;
        }

        private void StyleInput(TextBox textBox)
        {
            if (textBox == null) return;
            textBox.Font = new Font("Segoe UI", 10F);
            textBox.BorderStyle = BorderStyle.FixedSingle;
            textBox.RightToLeft = RightToLeft.Yes;
        }

        private void StyleCombo(ComboBox comboBox)
        {
            if (comboBox == null) return;
            comboBox.Font = new Font("Segoe UI", 10F);
            comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox.RightToLeft = RightToLeft.Yes;
        }

        private void StylePrimaryButton(Button button)
        {
            if (button == null) return;
            button.BackColor = _primaryColor;
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            button.UseVisualStyleBackColor = false;
        }

        private void StyleDangerButton(Button button)
        {
            if (button == null) return;
            button.BackColor = _dangerColor;
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            button.UseVisualStyleBackColor = false;
        }

        private void StyleNeutralButton(Button button)
        {
            if (button == null) return;
            button.BackColor = _neutralColor;
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            button.UseVisualStyleBackColor = false;
        }

        private void LoadComboBoxes()
        {
            cmbGender.Items.Clear();
            cmbGender.Items.Add("ذكر");
            cmbGender.Items.Add("أنثى");

            cmbStatus.Items.Clear();
            cmbStatus.Items.Add("نشط");
            cmbStatus.Items.Add("موقوف");
            cmbStatus.Items.Add("منقول");
            cmbStatus.Items.Add("متخرج");

            cmbFilterStatus.Items.Clear();
            cmbFilterStatus.Items.Add("كل الحالات");
            cmbFilterStatus.Items.Add("نشط");
            cmbFilterStatus.Items.Add("موقوف");
            cmbFilterStatus.Items.Add("منقول");
            cmbFilterStatus.Items.Add("متخرج");

            cmbFilterClass.Items.Clear();
            cmbFilterClass.Items.Add("كل الصفوف");

            cmbGender.SelectedIndex = -1;
            if (cmbStatus.Items.Count > 0) cmbStatus.SelectedIndex = 0;
            if (cmbFilterStatus.Items.Count > 0) cmbFilterStatus.SelectedIndex = 0;
            if (cmbFilterClass.Items.Count > 0) cmbFilterClass.SelectedIndex = 0;
        }

        private void LoadStudents()
        {
            try
            {
                _currentStudents = _studentService.GetAll();
                LoadClassFilterFromData();
                ApplyFiltersAndSearch();
            }
            catch (Exception ex)
            {
                _currentStudents = new List<Student>();
                BindGrid(_currentStudents);
                ShowError("حدث خطأ أثناء تحميل بيانات الطلاب.\n\nتفاصيل الخطأ:\n" + ex.Message);
            }
        }

        private void LoadClassFilterFromData()
        {
            string selected = cmbFilterClass.Text;
            cmbFilterClass.SelectedIndexChanged -= cmbFilterClass_SelectedIndexChanged;
            cmbFilterClass.Items.Clear();
            cmbFilterClass.Items.Add("كل الصفوف");

            var classes = _currentStudents
                .Where(s => !string.IsNullOrWhiteSpace(s.CurrentClassName))
                .Select(s => s.CurrentClassName.Trim())
                .Distinct()
                .OrderBy(s => s)
                .ToList();

            foreach (string className in classes)
                cmbFilterClass.Items.Add(className);

            if (!string.IsNullOrWhiteSpace(selected) && cmbFilterClass.Items.Contains(selected))
                cmbFilterClass.Text = selected;
            else
                cmbFilterClass.SelectedIndex = 0;

            cmbFilterClass.SelectedIndexChanged += cmbFilterClass_SelectedIndexChanged;
        }

        private void BindGrid(List<Student> students)
        {
            dgvStudents.DataSource = null;
            // يعرض فقط: رقم الطالب، الاسم، الجنس، الصف الحالي، الهاتف، الحالة
            var gridData = students.Select(s => new {
                StudentId = s.StudentId,
                StudentNumber = s.StudentNumber,
                FullName = s.FullName,
                Gender = s.Gender,
                CurrentClassName = s.CurrentClassName,
                Phone = s.StudentPhone,
                Status = s.Status
            }).ToList();
            
            dgvStudents.DataSource = gridData;
            dgvStudents.Columns["StudentId"].Visible = false;
            
            dgvStudents.Columns["StudentNumber"].HeaderText = "رقم الطالب";
            dgvStudents.Columns["FullName"].HeaderText = "الاسم";
            dgvStudents.Columns["Gender"].HeaderText = "الجنس";
            dgvStudents.Columns["CurrentClassName"].HeaderText = "الصف الحالي";
            dgvStudents.Columns["Phone"].HeaderText = "الهاتف";
            dgvStudents.Columns["Status"].HeaderText = "الحالة";

            lblCount.Text = "عدد الطلاب: " + students.Count;
        }

        private void ApplyFiltersAndSearch()
        {
            IEnumerable<Student> query = _currentStudents;
            string keyword = txtSearch.Text.Trim();
            string selectedStatus = cmbFilterStatus.Text.Trim();
            string selectedClass = cmbFilterClass.Text.Trim();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(s =>
                    ContainsText(s.StudentNumber, keyword) ||
                    ContainsText(s.FullName, keyword) ||
                    ContainsText(s.NationalId, keyword) ||
                    ContainsText(s.StudentPhone, keyword));
            }

            if (!string.IsNullOrWhiteSpace(selectedStatus) && selectedStatus != "كل الحالات")
                query = query.Where(s => string.Equals(s.Status, selectedStatus, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(selectedClass) && selectedClass != "كل الصفوف")
                query = query.Where(s => string.Equals(s.CurrentClassName, selectedClass, StringComparison.OrdinalIgnoreCase));

            BindGrid(query.ToList());
        }

        private bool ContainsText(string source, string keyword)
        {
            if (string.IsNullOrWhiteSpace(source)) return false;
            return source.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private bool ValidateForm()
        {
            errorProvider1.Clear();
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(txtFullName.Text)) { errorProvider1.SetError(txtFullName, "الاسم مطلوب"); isValid = false; }
            if (string.IsNullOrWhiteSpace(cmbGender.Text)) { errorProvider1.SetError(cmbGender, "الجنس مطلوب"); isValid = false; }
            if (string.IsNullOrWhiteSpace(txtNationalId.Text)) { errorProvider1.SetError(txtNationalId, "رقم الهوية مطلوب"); isValid = false; }
            if (string.IsNullOrWhiteSpace(txtPhone.Text)) { errorProvider1.SetError(txtPhone, "رقم الهاتف مطلوب"); isValid = false; }

            return isValid;
        }

        private Student ReadStudentFromForm()
        {
            Student student = new Student();
            student.StudentId = _selectedStudentId;
            student.StudentNumber = txtStudentNumber.Text.Trim();
            student.FullName = txtFullName.Text.Trim();
            student.Gender = cmbGender.Text.Trim();
            student.BirthDate = dtpBirthDate.Value.Date;
            student.BirthPlace = txtBirthPlace.Text.Trim();
            student.Nationality = txtNationality.Text.Trim();
            student.NationalId = txtNationalId.Text.Trim();
            student.StudentPhone = txtPhone.Text.Trim();
            student.Status = cmbStatus.Text.Trim();
            
            student.GuardianName = txtGuardianName.Text.Trim();
            student.GuardianRelation = txtGuardianRelation.Text.Trim();
            student.GuardianPhone = txtGuardianPhone.Text.Trim();
            student.GuardianEmail = txtGuardianEmail.Text.Trim();
            student.GuardianJob = txtGuardianJob.Text.Trim();

            student.Governorate = txtGovernorate.Text.Trim();
            student.District = txtDistrict.Text.Trim();
            student.Address = txtAddress.Text.Trim();
            
            student.Photo = _selectedPhoto;

            return student;
        }

        private void FillForm(Student student)
        {
            if (student == null) return;

            _selectedStudentId = student.StudentId;
            _selectedPhoto = student.Photo;

            txtStudentNumber.Text = student.StudentNumber;
            txtFullName.Text = student.FullName;
            cmbGender.Text = student.Gender;
            dtpBirthDate.Value = student.BirthDate.HasValue ? student.BirthDate.Value.Date : DateTime.Today;
            txtBirthPlace.Text = student.BirthPlace;
            txtNationality.Text = student.Nationality;
            txtNationalId.Text = student.NationalId;
            txtPhone.Text = student.StudentPhone;
            cmbStatus.Text = string.IsNullOrEmpty(student.Status) ? "نشط" : student.Status;

            txtGuardianName.Text = student.GuardianName;
            txtGuardianRelation.Text = student.GuardianRelation;
            txtGuardianPhone.Text = student.GuardianPhone;
            txtGuardianEmail.Text = student.GuardianEmail;
            txtGuardianJob.Text = student.GuardianJob;

            txtGovernorate.Text = student.Governorate;
            txtDistrict.Text = student.District;
            txtAddress.Text = student.Address;

            if (student.Photo != null && student.Photo.Length > 0)
            {
                using (MemoryStream ms = new MemoryStream(student.Photo))
                {
                    picStudent.Image = Image.FromStream(ms);
                }
            }
            else
            {
                picStudent.Image = null;
            }

            btnSave.Enabled = false;
            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;
        }

        private void ClearForm()
        {
            _selectedStudentId = 0;
            _selectedPhoto = null;
            errorProvider1.Clear();

            try { txtStudentNumber.Text = _studentService.GenerateNextStudentNumber(); }
            catch { txtStudentNumber.Text = string.Empty; }

            txtFullName.Clear();
            cmbGender.SelectedIndex = -1;
            dtpBirthDate.Value = DateTime.Today;
            txtBirthPlace.Clear();
            txtNationality.Text = "يمني";
            txtNationalId.Clear();
            txtPhone.Clear();
            if (cmbStatus.Items.Count > 0) cmbStatus.SelectedIndex = 0;

            txtGuardianName.Clear();
            txtGuardianRelation.Clear();
            txtGuardianPhone.Clear();
            txtGuardianEmail.Clear();
            txtGuardianJob.Clear();

            txtGovernorate.Clear();
            txtDistrict.Clear();
            txtAddress.Clear();

            picStudent.Image = null;

            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            txtFullName.Focus();
        }

        private void btnAdd_Click(object sender, EventArgs e) { ClearForm(); }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateForm()) return;
            try
            {
                Student student = ReadStudentFromForm();
                int newId = _studentService.Add(student);
                ShowInfo("تم حفظ بيانات الطالب بنجاح.");
                ClearForm();
                LoadStudents();
                SelectStudentInGrid(newId);
            }
            catch (Exception ex) { ShowError(ex.Message); }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!ValidateForm()) return;
            try
            {
                if (_selectedStudentId <= 0) { ShowWarning("يرجى اختيار طالب من الجدول أولاً."); return; }
                Student student = ReadStudentFromForm();
                student.StudentId = _selectedStudentId;
                _studentService.Update(student);
                ShowInfo("تم تعديل بيانات الطالب بنجاح.");
                LoadStudents();
                SelectStudentInGrid(_selectedStudentId);
            }
            catch (Exception ex) { ShowError(ex.Message); }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (_selectedStudentId <= 0) { ShowWarning("يرجى اختيار طالب من الجدول أولاً."); return; }
                if (MessageBox.Show("هل أنت متأكد من حذف الطالب المحدد؟", "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
                
                _studentService.Delete(_selectedStudentId);
                ShowInfo("تم حذف بيانات الطالب بنجاح.");
                ClearForm();
                LoadStudents();
            }
            catch (Exception ex) { ShowError("لا يمكن حذف الطالب لأنه قد يكون مرتبطاً بسجلات أخرى.\n\n" + ex.Message); }
        }

        private void btnCancel_Click(object sender, EventArgs e) { txtSearch.Clear(); ClearForm(); }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            if (cmbFilterStatus.Items.Count > 0) cmbFilterStatus.SelectedIndex = 0;
            if (cmbFilterClass.Items.Count > 0) cmbFilterClass.SelectedIndex = 0;
            LoadStudents();
        }

        private void btnSearch_Click(object sender, EventArgs e) { ApplyFiltersAndSearch(); }
        private void cmbFilterClass_SelectedIndexChanged(object sender, EventArgs e) { ApplyFiltersAndSearch(); }
        private void cmbFilterStatus_SelectedIndexChanged(object sender, EventArgs e) { ApplyFiltersAndSearch(); }
        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { ApplyFiltersAndSearch(); e.SuppressKeyPress = true; }
        }

        private void dgvStudents_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            LoadSelectedStudentFromGrid();
        }

        private void dgvStudents_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvStudents.Focused) LoadSelectedStudentFromGrid();
        }

        private void LoadSelectedStudentFromGrid()
        {
            if (dgvStudents.CurrentRow == null) return;
            int id = Convert.ToInt32(dgvStudents.CurrentRow.Cells["StudentId"].Value);
            Student student = _currentStudents.FirstOrDefault(s => s.StudentId == id);
            if (student != null) FillForm(student);
        }

        private void btnChooseImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog { Title = "اختيار صورة الطالب", Filter = "ملفات الصور|*.jpg;*.jpeg;*.png;*.bmp", Multiselect = false })
            {
                if (ofd.ShowDialog() != DialogResult.OK) return;
                FileInfo fileInfo = new FileInfo(ofd.FileName);
                if (fileInfo.Length > 2 * 1024 * 1024) { ShowWarning("حجم الصورة يجب ألا يتجاوز 2 ميجابايت."); return; }
                
                using (Image originalImage = Image.FromFile(ofd.FileName))
                {
                    picStudent.Image = new Bitmap(originalImage);
                }
                
                using (MemoryStream ms = new MemoryStream())
                {
                    picStudent.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    _selectedPhoto = ms.ToArray();
                }
            }
        }
        
        private void btnRemoveImage_Click(object sender, EventArgs e)
        {
            picStudent.Image = null;
            _selectedPhoto = null;
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvStudents.Rows.Count == 0) { ShowWarning("لا توجد بيانات للتصدير."); return; }
                using (SaveFileDialog sfd = new SaveFileDialog { Title = "تصدير بيانات الطلاب", Filter = "Excel CSV File|*.csv", FileName = "Students_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".csv" })
                {
                    if (sfd.ShowDialog() != DialogResult.OK) return;
                    ExportGridToCsv(sfd.FileName);
                    ShowInfo("تم تصدير البيانات بنجاح.");
                }
            }
            catch (Exception ex) { ShowError("حدث خطأ أثناء التصدير:\n" + ex.Message); }
        }

        private void ExportGridToCsv(string filePath)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Join(",", dgvStudents.Columns.Cast<DataGridViewColumn>().Where(c=>c.Visible).Select(c => EscapeCsv(c.HeaderText))));
            
            foreach (DataGridViewRow row in dgvStudents.Rows)
            {
                var cells = row.Cells.Cast<DataGridViewCell>().Where(c=>c.OwningColumn.Visible).Select(c => EscapeCsv(c.Value?.ToString()));
                sb.AppendLine(string.Join(",", cells));
            }
            File.WriteAllText(filePath, sb.ToString(), new UTF8Encoding(true));
        }

        private string EscapeCsv(string value) => string.IsNullOrEmpty(value) ? "" : "\"" + value.Replace("\"", "\"\"") + "\"";

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (_selectedStudentId <= 0) { ShowWarning("يرجى اختيار طالب من الجدول لطباعة البطاقة."); return; }
            using (PrintDocument printDocument = new PrintDocument { DocumentName = "بطاقة طالب" })
            {
                printDocument.PrintPage += PrintStudentCard_PrintPage;
                using (PrintPreviewDialog preview = new PrintPreviewDialog { Document = printDocument, WindowState = FormWindowState.Maximized })
                {
                    preview.ShowDialog();
                }
            }
        }

        private void PrintStudentCard_PrintPage(object sender, PrintPageEventArgs e)
        {
            Font titleFont = new Font("Segoe UI", 16, FontStyle.Bold);
            Font labelFont = new Font("Segoe UI", 10, FontStyle.Bold);
            Font valueFont = new Font("Segoe UI", 10, FontStyle.Regular);
            Pen borderPen = new Pen(_primaryColor, 2);
            Rectangle card = new Rectangle(120, 100, 530, 310);
            e.Graphics.FillRectangle(Brushes.White, card);
            e.Graphics.DrawRectangle(borderPen, card);

            StringFormat rtl = new StringFormat { Alignment = StringAlignment.Far, FormatFlags = StringFormatFlags.DirectionRightToLeft };
            e.Graphics.DrawString("بطاقة طالب", titleFont, Brushes.DarkBlue, new RectangleF(140, 115, 490, 35), rtl);

            if (picStudent.Image != null)
            {
                e.Graphics.DrawImage(picStudent.Image, new Rectangle(145, 160, 120, 140));
                e.Graphics.DrawRectangle(Pens.Gray, new Rectangle(145, 160, 120, 140));
            }

            int xLabel = 610, y = 165, gap = 35;
            DrawPrintRow(e.Graphics, "رقم الطالب:", txtStudentNumber.Text, xLabel, y, labelFont, valueFont, rtl); y += gap;
            DrawPrintRow(e.Graphics, "الاسم:", txtFullName.Text, xLabel, y, labelFont, valueFont, rtl); y += gap;
            DrawPrintRow(e.Graphics, "الجنس:", cmbGender.Text, xLabel, y, labelFont, valueFont, rtl); y += gap;
            DrawPrintRow(e.Graphics, "الهاتف:", txtPhone.Text, xLabel, y, labelFont, valueFont, rtl); y += gap;
            DrawPrintRow(e.Graphics, "الحالة:", cmbStatus.Text, xLabel, y, labelFont, valueFont, rtl);
        }

        private void DrawPrintRow(Graphics g, string label, string value, int x, int y, Font labelFont, Font valueFont, StringFormat rtl)
        {
            g.DrawString(label, labelFont, Brushes.Black, new RectangleF(x - 150, y, 140, 25), rtl);
            g.DrawString(value, valueFont, Brushes.Black, new RectangleF(x - 430, y, 270, 25), rtl);
        }

        private void SelectStudentInGrid(int studentId)
        {
            foreach (DataGridViewRow row in dgvStudents.Rows)
            {
                int id = Convert.ToInt32(row.Cells["StudentId"].Value);
                if (id == studentId)
                {
                    row.Selected = true;
                    dgvStudents.CurrentCell = row.Cells[1];
                    LoadSelectedStudentFromGrid();
                    break;
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e) { this.Close(); }

        private void ShowInfo(string message) { MessageBox.Show(message, "معلومة", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading); }
        private void ShowWarning(string message) { MessageBox.Show(message, "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading); }
        private void ShowError(string message) { MessageBox.Show(message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading); }
    }
}
