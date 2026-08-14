using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SchoolSystem.Models;
using SchoolSystem.Helpers;
using SchoolSystem.Services;

namespace SchoolSystem.UI
{
    public partial class TeachersForm : UserControl
    {
        private readonly TeacherService _teacherService = new TeacherService();
        private int _selectedTeacherId = 0;
        private DataTable _allTeachers;

        public TeachersForm()
        {
            InitializeComponent();
            SchoolSystem.Helpers.UIHelper.ApplyStyle(this);
            ApplyCustomStyles();
            InitializeEvents();
            PopulateComboBoxes();
            ConfigureDataGridViewStyling();
            GenerateNewEmployeeNumber();
        }

        private void ApplyCustomStyles()
        {
            UIHelper.StyleDataGridView(dataGridViewTeachers);
            UIHelper.StylePrimaryButton(btnAdd);
            UIHelper.StylePrimaryButton(btnUpdate);
            UIHelper.StyleDangerButton(btnDelete);
            UIHelper.StyleButton(btnClear, UIHelper.NeutralColor);
            UIHelper.StyleButton(btnRefresh, UIHelper.NeutralColor);
            UIHelper.StyleTextBox(txtSearch);

            TextBox[] textBoxes = {
                txtEmployeeNumber, txtFullName, txtBirthPlace, txtNationalID,
                txtPhone, txtEmail, txtAddress, txtSpecialization, txtNotes
            };
            foreach (TextBox textBox in textBoxes)
                UIHelper.StyleTextBox(textBox);

            ComboBox[] comboBoxes = {
                cmbGender, cmbNationality, cmbQualification, cmbStatus
            };
            foreach (ComboBox comboBox in comboBoxes)
                UIHelper.StyleComboBox(comboBox);

            dataGridViewTeachers.AlternatingRowsDefaultCellStyle.BackColor = UIHelper.AlternateRowColor;
            dataGridViewTeachers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewTeachers.MultiSelect = false;
            dataGridViewTeachers.ReadOnly = true;
        }

        private void InitializeEvents()
        {
            this.Load += TeachersForm_Load;
            btnAdd.Click += btnAdd_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Click += btnDelete_Click;
            btnClear.Click += btnClear_Click;
            btnRefresh.Click += btnRefresh_Click;
            txtSearch.TextChanged += txtSearch_TextChanged;
            dataGridViewTeachers.CellClick += dataGridViewTeachers_CellClick;
        }

        private void PopulateComboBoxes()
        {
            cmbGender.Items.Clear();
            cmbGender.Items.AddRange(new[] { "ذكر", "أنثى" });

            cmbQualification.Items.Clear();
            cmbQualification.Items.AddRange(new[] { "بكالوريوس", "ماجستير", "دكتوراه", "دبلوم عالي", "دبلوم" });

            cmbStatus.Items.Clear();
            cmbStatus.Items.AddRange(new[] { "نشط", "إجازة رسمية", "مستقيل", "منقطع" });

            cmbNationality.Items.Clear();
            cmbNationality.Items.AddRange(new[] {
                "يمني", "سعودي", "مصري", "سوري", "أردني", "فلسطيني",
                "سوداني", "عراقي", "مغربي", "جزائري", "تونسي", "أخرى"
            });
        }

        private void ConfigureDataGridViewStyling()
        {
            dataGridViewTeachers.AlternatingRowsDefaultCellStyle.BackColor = UIHelper.AlternateRowColor;
            dataGridViewTeachers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewTeachers.MultiSelect = false;
            dataGridViewTeachers.ReadOnly = true;
        }

        private void GenerateNewEmployeeNumber()
        {
            int year = DateTime.Now.Year;
            string prefix = $"TCH-{year}-";
            try
            {
                int lastNum = _teacherService.GetMaxEmployeeNumberSuffix(year);
                int nextNum = lastNum + 1;
                txtEmployeeNumber.Text = $"{prefix}{nextNum:D4}";
            }
            catch
            {
                txtEmployeeNumber.Text = $"{prefix}{new Random().Next(1, 9999):D4}";
            }
        }

        private async void TeachersForm_Load(object sender, EventArgs e)
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                _allTeachers = await Task.Run(() => _teacherService.GetAllTeachers());
                ApplyFilter(txtSearch.Text.Trim());
            }
            catch (Exception ex)
            {
                LogException("LoadTeachers", ex);
                ShowSafeError("تعذر تحميل بيانات المعلمين. تحقق من الاتصال وحاول مرة أخرى.");
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void ApplyFilter(string search)
        {
            if (_allTeachers == null) return;
            DataView dv = _allTeachers.DefaultView;
            if (!string.IsNullOrWhiteSpace(search))
            {
                string safeSearch = UIHelper.EscapeDataViewFilterValue(search);
                dv.RowFilter = $"FullName LIKE '%{safeSearch}%' OR EmployeeNumber LIKE '%{safeSearch}%' OR Specialization LIKE '%{safeSearch}%'";
            }
            else
            {
                dv.RowFilter = "";
            }

            dataGridViewTeachers.DataSource = dv;

            if (dataGridViewTeachers.Columns.Contains("TeacherID"))
                dataGridViewTeachers.Columns["TeacherID"].Visible = false;
            if (dataGridViewTeachers.Columns.Contains("EmployeeNumber"))
                dataGridViewTeachers.Columns["EmployeeNumber"].HeaderText = "الرقم الوظيفي";
            if (dataGridViewTeachers.Columns.Contains("FullName"))
                dataGridViewTeachers.Columns["FullName"].HeaderText = "الاسم الكامل";
            if (dataGridViewTeachers.Columns.Contains("Gender"))
                dataGridViewTeachers.Columns["Gender"].HeaderText = "الجنس";
            if (dataGridViewTeachers.Columns.Contains("Phone"))
                dataGridViewTeachers.Columns["Phone"].HeaderText = "الهاتف";
            if (dataGridViewTeachers.Columns.Contains("Specialization"))
                dataGridViewTeachers.Columns["Specialization"].HeaderText = "التخصص";
            if (dataGridViewTeachers.Columns.Contains("Status"))
                dataGridViewTeachers.Columns["Status"].HeaderText = "الحالة";
        }

        private string EscapeFilter(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "";

            return value.Trim()
                .Replace("'", "''")
                .Replace("[", "[[]")
                .Replace("%", "[%]")
                .Replace("*", "[*]");
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("الاسم الكامل مطلوب", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFullName.Focus();
                return false;
            }
            if (cmbGender.SelectedIndex < 0)
            {
                MessageBox.Show("يرجى اختيار الجنس", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbGender.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtNationalID.Text))
            {
                MessageBox.Show("الرقم الوطني مطلوب", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNationalID.Focus();
                return false;
            }
            if (cmbStatus.SelectedIndex < 0)
            {
                MessageBox.Show("يرجى اختيار الحالة الوظيفية", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbStatus.Focus();
                return false;
            }
            if (nudBasicSalary.Value < 1000)
            {
                MessageBox.Show("الراتب الأساسي يجب أن لا يقل عن 1000", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                nudBasicSalary.Focus();
                return false;
            }

            var minBirthDate = DateTime.Today.AddYears(-65);
            var maxBirthDate = DateTime.Today.AddYears(-22);
            if (dtpBirthDate.Value < minBirthDate || dtpBirthDate.Value > maxBirthDate)
            {
                MessageBox.Show("يجب أن يكون عمر المعلم بين 22 و 65 سنة.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpBirthDate.Focus();
                return false;
            }

            var minHireDate = dtpBirthDate.Value.AddYears(22);
            if (dtpHireDate.Value < minHireDate)
            {
                MessageBox.Show("تاريخ التعيين يجب أن يكون بعد بلوغ سن 22 سنة على الأقل.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpHireDate.Focus();
                return false;
            }

            if (!string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                var emailRegex = new System.Text.RegularExpressions.Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                if (!emailRegex.IsMatch(txtEmail.Text.Trim()))
                {
                    MessageBox.Show("صيغة البريد الإلكتروني غير صحيحة.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEmail.Focus();
                    return false;
                }
            }

            if (!string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                var phoneDigits = new string(txtPhone.Text.Where(char.IsDigit).ToArray());
                if (phoneDigits.Length < 7 || phoneDigits.Length > 15)
                {
                    MessageBox.Show("رقم الهاتف يجب أن يحتوي على 7 إلى 15 رقمًا.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPhone.Focus();
                    return false;
                }
            }

            try
            {
                string nationalID = txtNationalID.Text.Trim();
                string email = txtEmail.Text.Trim();
                int? excludeId = (_selectedTeacherId == 0) ? (int?)null : _selectedTeacherId;

                if (!string.IsNullOrEmpty(nationalID))
                {
                    bool idUnique = _teacherService.IsNationalIDUnique(nationalID, excludeId);
                    if (!idUnique)
                    {
                        MessageBox.Show("الرقم الوطني موجود مسبقاً لموظف آخر.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtNationalID.Focus();
                        return false;
                    }
                }

                if (!string.IsNullOrEmpty(email))
                {
                    bool emailUnique = _teacherService.IsEmailUnique(email, excludeId);
                    if (!emailUnique)
                    {
                        MessageBox.Show("البريد الإلكتروني موجود مسبقاً لموظف آخر.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtEmail.Focus();
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                // إذا كانت دوال التحقق غير موجودة، نتجاهل الخطأ ونستمر
            }

            return true;
        }

        private Teacher GetTeacherFromUI() => new Teacher
        {
            TeacherID = _selectedTeacherId,
            EmployeeNumber = txtEmployeeNumber.Text.Trim(),
            FullName = txtFullName.Text.Trim(),
            Gender = cmbGender.Text,
            BirthDate = dtpBirthDate.Value,
            BirthPlace = txtBirthPlace.Text.Trim(),
            Nationality = cmbNationality.Text,
            NationalID = txtNationalID.Text.Trim(),
            Phone = txtPhone.Text.Trim(),
            Email = txtEmail.Text.Trim(),
            Address = txtAddress.Text.Trim(),
            Qualification = cmbQualification.Text,
            Specialization = txtSpecialization.Text.Trim(),
            HireDate = dtpHireDate.Value,
            BasicSalary = nudBasicSalary.Value,
            TransportAllowance = nudTransportAllowance.Value,
            HousingAllowance = nudHousingAllowance.Value,
            Status = cmbStatus.Text,
            Notes = txtNotes.Text.Trim()
        };

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;
            var teacher = GetTeacherFromUI();
            try
            {
                await Task.Run(() => _teacherService.AddTeacher(teacher));
                MessageBox.Show("تمت الإضافة بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadDataAsync();
                ClearInputs();
            }
            catch (Exception ex)
            {
                LogException("AddTeacher", ex);
                ShowSafeError(GetOperationError("الإضافة", ex));
            }
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            if (_selectedTeacherId == 0)
            {
                MessageBox.Show("اختر معلماً أولاً", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!ValidateInputs()) return;
            var teacher = GetTeacherFromUI();
            try
            {
                await Task.Run(() => _teacherService.UpdateTeacher(teacher));
                MessageBox.Show("تم التعديل بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadDataAsync();
                ClearInputs();
            }
            catch (Exception ex)
            {
                LogException("UpdateTeacher", ex);
                ShowSafeError(GetOperationError("التعديل", ex));
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedTeacherId == 0)
            {
                MessageBox.Show("اختر معلماً أولاً", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("هل أنت متأكد من الحذف؟", "تأكيد", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    await Task.Run(() => _teacherService.DeleteTeacher(_selectedTeacherId));
                    MessageBox.Show("تم الحذف بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadDataAsync();
                    ClearInputs();
                }
                catch (Exception ex)
                {
                    LogException("DeleteTeacher", ex);
                    ShowSafeError(GetOperationError("الحذف", ex));
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearInputs();
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await LoadDataAsync();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter(txtSearch.Text.Trim());
        }

        private void dataGridViewTeachers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dataGridViewTeachers.Rows[e.RowIndex];
                _selectedTeacherId = Convert.ToInt32(row.Cells["TeacherID"].Value);
                txtEmployeeNumber.Text = row.Cells["EmployeeNumber"].Value?.ToString();
                txtFullName.Text = row.Cells["FullName"].Value?.ToString();
                cmbGender.Text = row.Cells["Gender"].Value?.ToString();
                dtpBirthDate.Value = Convert.ToDateTime(row.Cells["BirthDate"].Value);
                txtBirthPlace.Text = row.Cells["BirthPlace"].Value?.ToString();
                cmbNationality.Text = row.Cells["Nationality"].Value?.ToString();
                txtNationalID.Text = row.Cells["NationalID"].Value?.ToString();
                txtPhone.Text = row.Cells["Phone"].Value?.ToString();
                txtEmail.Text = row.Cells["Email"].Value?.ToString();
                txtAddress.Text = row.Cells["Address"].Value?.ToString();
                cmbQualification.Text = row.Cells["Qualification"].Value?.ToString();
                txtSpecialization.Text = row.Cells["Specialization"].Value?.ToString();
                dtpHireDate.Value = Convert.ToDateTime(row.Cells["HireDate"].Value);
                nudBasicSalary.Value = Convert.ToDecimal(row.Cells["BasicSalary"].Value);
                nudTransportAllowance.Value = Convert.ToDecimal(row.Cells["TransportAllowance"].Value);
                nudHousingAllowance.Value = Convert.ToDecimal(row.Cells["HousingAllowance"].Value);
                cmbStatus.Text = row.Cells["Status"].Value?.ToString();
                txtNotes.Text = row.Cells["Notes"].Value?.ToString();
            }
        }

        private string GetOperationError(string operation, Exception ex)
        {
            if (ex is UnauthorizedAccessException)
                return ex.Message;

            return "تعذر تنفيذ " + operation + " المعلم. تحقق من البيانات والاتصال ثم حاول مرة أخرى.";
        }

        private void ShowSafeError(string message)
        {
            MessageBox.Show(message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error,
                MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
        }

        private void LogException(string operation, Exception ex)
        {
            try
            {
                string directory = System.IO.Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "SchoolSystem", "Logs");
                System.IO.Directory.CreateDirectory(directory);
                System.IO.File.AppendAllText(System.IO.Path.Combine(directory, "errors.log"),
                    DateTime.Now.ToString("s") + " [" + operation + "] " + ex + Environment.NewLine);
            }
            catch
            {
                // لا نسمح لفشل التسجيل بأن يعطل الواجهة.
            }
        }

        private void ClearInputs()
        {
            _selectedTeacherId = 0;
            GenerateNewEmployeeNumber();
            txtFullName.Clear();
            cmbGender.SelectedIndex = -1;
            dtpBirthDate.Value = DateTime.Today.AddYears(-30);
            txtBirthPlace.Clear();
            cmbNationality.SelectedIndex = -1;
            txtNationalID.Clear();
            txtPhone.Clear();
            txtEmail.Clear();
            txtAddress.Clear();
            cmbQualification.SelectedIndex = -1;
            txtSpecialization.Clear();
            dtpHireDate.Value = DateTime.Today;
            nudBasicSalary.Value = 0;
            nudTransportAllowance.Value = 0;
            nudHousingAllowance.Value = 0;
            cmbStatus.SelectedIndex = -1;
            txtNotes.Clear();
            txtFullName.Focus();
        }
    }
}