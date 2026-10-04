using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using SchoolSystem.Models;
using SchoolSystem.Helpers;
using SchoolSystem.Services;
using SchoolSystem.Security;


namespace SchoolSystem.UI
{

    public partial class TeachersForm : UserControl
    {
        // تعريف الدالة TeacherService لتنفيذ العملية المرتبطة بها.
        private readonly TeacherService _teacherService = new TeacherService();

        private int _selectedTeacherId = 0;

        private DataTable _allTeachers;

        // تعريف الدالة TeachersForm لتنفيذ العملية المرتبطة بها.
        public TeachersForm()
        {

            InitializeComponent();

            SchoolSystem.Helpers.UIHelper.ApplyStyle(this);
            ApplyCustomStyles();
            InitializeEvents();
            PopulateComboBoxes();
            ConfigureDataGridViewStyling();
            GenerateNewEmployeeNumber();

            // منع اختيار تاريخ تعيين مستقبلي.
            dtpHireDate.MaxDate = DateTime.Today;
        }

        // تعريف الدالة ApplyCustomStyles لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة InitializeEvents لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة PopulateComboBoxes لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة ConfigureDataGridViewStyling لتنفيذ العملية المرتبطة بها.
        private void ConfigureDataGridViewStyling()
        {
            dataGridViewTeachers.AlternatingRowsDefaultCellStyle.BackColor = UIHelper.AlternateRowColor;
            dataGridViewTeachers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewTeachers.MultiSelect = false;
            dataGridViewTeachers.ReadOnly = true;
        }

        // تعريف الدالة GenerateNewEmployeeNumber لتنفيذ العملية المرتبطة بها.
        private void GenerateNewEmployeeNumber()
        {
            // الرقم الوظيفي يُولّد تلقائياً داخل معاملة الإضافة.
            txtEmployeeNumber.Clear();
            txtEmployeeNumber.ReadOnly = true;
            txtEmployeeNumber.BackColor = UIHelper.DisabledSurfaceColor;
            txtEmployeeNumber.ForeColor = UIHelper.MutedTextColor;
        }

        // تعريف الدالة TeachersForm_Load لتنفيذ العملية المرتبطة بها.
        private async void TeachersForm_Load(object sender, EventArgs e)
        {

            await LoadDataAsync();
        }

        // تعريف الدالة LoadDataAsync لتنفيذ العملية المرتبطة بها.
        private async Task LoadDataAsync()
        {
            Cursor = Cursors.WaitCursor;
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {

                _allTeachers = await Task.Run(() => _teacherService.GetAllTeachers());
                ApplyFilter(txtSearch.Text.Trim());
            }

            catch (Exception ex)
            {
                UIHelper.ShowException("تحميل بيانات المعلمين", ex);
            }

            finally
            {
                Cursor = Cursors.Default;
            }
        }

        // تعريف الدالة ApplyFilter لتنفيذ العملية المرتبطة بها.
        private void ApplyFilter(string search)
        {
            if (_allTeachers == null) return;
            DataView dv = _allTeachers.DefaultView;
            if (!string.IsNullOrWhiteSpace(search))
            {
                dv.RowFilter = UIHelper.BuildDataViewSearchFilter(search,
                    "FullName", "EmployeeNumber", "Specialization");
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

        // تعريف الدالة ValidateInputs لتنفيذ العملية المرتبطة بها.
        private bool ValidateInputs()
        {
            if (!UIHelper.IsValidArabicOrLatinName(txtFullName.Text))
            {
                UIHelper.FocusAndWarn(txtFullName, "الاسم الكامل مطلوب ويجب أن يحتوي على أحرف فقط وبطول مناسب.");

                return false;
            }
            if (!string.IsNullOrWhiteSpace(txtBirthPlace.Text) && !UIHelper.IsValidTextOnly(txtBirthPlace.Text, 2, 100))
            {
                UIHelper.FocusAndWarn(txtBirthPlace, "مكان الميلاد يجب أن يحتوي على نص فقط دون أرقام وبطول مناسب.");

                return false;
            }
            if (!string.IsNullOrWhiteSpace(txtAddress.Text) && !UIHelper.IsValidTextOnly(txtAddress.Text, 2, 200))
            {
                UIHelper.FocusAndWarn(txtAddress, "العنوان يجب أن يحتوي على نص صالح دون أرقام غير مسموحة.");

                return false;
            }
            if (!string.IsNullOrWhiteSpace(txtSpecialization.Text) && !UIHelper.IsValidTextOnly(txtSpecialization.Text, 2, 100))
            {
                UIHelper.FocusAndWarn(txtSpecialization, "التخصص يجب أن يحتوي على أحرف فقط وبطول مناسب.");

                return false;
            }
            if (cmbGender.SelectedIndex < 0)
            {

                UIHelper.ShowWarning("يرجى اختيار الجنس");

                cmbGender.Focus();

                return false;
            }
            if (!UIHelper.IsValidPositiveInteger(txtNationalID.Text, out _) || txtNationalID.Text.Trim().Length < 6 || txtNationalID.Text.Trim().Length > 20)
            {
                UIHelper.FocusAndWarn(txtNationalID, "الرقم الوطني مطلوب ويجب أن يتكون من 6 إلى 20 رقماً.");

                return false;
            }
            if (cmbStatus.SelectedIndex < 0)
            {

                UIHelper.ShowWarning("يرجى اختيار الحالة الوظيفية");

                cmbStatus.Focus();

                return false;
            }
            if (nudBasicSalary.Value < 1000)
            {
                UIHelper.FocusAndWarn(nudBasicSalary, "الراتب الأساسي يجب ألا يقل عن 1000.");

                return false;
            }

            // الرقم الوظيفي لا يُدخل يدوياً؛ يتم توليده تلقائياً عند الإضافة.
            var minBirthDate = DateTime.Today.AddYears(-65);
            var maxBirthDate = DateTime.Today.AddYears(-22);
            if (dtpBirthDate.Value < minBirthDate || dtpBirthDate.Value > maxBirthDate)
            {

                UIHelper.ShowWarning("يجب أن يكون عمر المعلم بين 22 و 65 سنة.");

                dtpBirthDate.Focus();

                return false;
            }

            var minHireDate = dtpBirthDate.Value.AddYears(22);
            if (dtpHireDate.Value < minHireDate)
            {

                UIHelper.ShowWarning("تاريخ التعيين يجب أن يكون بعد بلوغ سن 22 سنة على الأقل.");

                dtpHireDate.Focus();

                return false;
            }

            if (dtpHireDate.Value.Date > DateTime.Today)
            {
                UIHelper.FocusAndWarn(dtpHireDate, "تاريخ التعيين لا يمكن أن يكون في المستقبل.");

                return false;
            }

            if (!string.IsNullOrWhiteSpace(txtEmail.Text) && !UIHelper.IsValidEmail(txtEmail.Text))
            {
                UIHelper.FocusAndWarn(txtEmail, "صيغة البريد الإلكتروني غير صحيحة.");

                return false;
            }

            if (!string.IsNullOrWhiteSpace(txtPhone.Text) && !UIHelper.IsValidPhone(txtPhone.Text))
            {
                UIHelper.FocusAndWarn(txtPhone, "رقم الهاتف غير صحيح.");

                return false;
            }

            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
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

                        UIHelper.ShowWarning("الرقم الوطني موجود مسبقاً لموظف آخر.");

                        txtNationalID.Focus();

                        return false;
                    }
                }

                if (!string.IsNullOrEmpty(email))
                {
                    bool emailUnique = _teacherService.IsEmailUnique(email, excludeId);
                    if (!emailUnique)
                    {

                        UIHelper.ShowWarning("البريد الإلكتروني موجود مسبقاً لموظف آخر.");

                        txtEmail.Focus();

                        return false;
                    }
                }
            }

            catch (Exception ex)
            {
                UIHelper.ShowException("التحقق من بيانات المعلم", ex);

                return false;
            }


            return true;
        }

        // تعريف الدالة GetTeacherFromUI لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة btnAdd_Click لتنفيذ العملية المرتبطة بها.
        private async void btnAdd_Click(object sender, EventArgs e)
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.DemandAction("Teachers", "Add", "ليس لديك صلاحية إضافة المعلمين.");
            if (!ValidateInputs()) return;
            var teacher = GetTeacherFromUI();
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {

                await Task.Run(() => _teacherService.AddTeacher(teacher));

                UIHelper.ShowInfo("تمت الإضافة بنجاح");
                ApplicationLogger.LogUsage("شاشة المعلمين", "إضافة معلم", "تمت إضافة المعلم بنجاح");

                await LoadDataAsync();
                ClearInputs();
            }

            catch (Exception ex)
            {
                UIHelper.ShowException("إضافة المعلم", ex);
            }
        }

        // تعريف الدالة btnUpdate_Click لتنفيذ العملية المرتبطة بها.
        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            CurrentUser.DemandAction("Teachers", "Edit", "ليس لديك صلاحية تعديل المعلمين.");
            if (_selectedTeacherId == 0)
            {

                UIHelper.ShowWarning("اختر معلماً أولاً");

                return;
            }
            if (!ValidateInputs()) return;
            var teacher = GetTeacherFromUI();
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {

                await Task.Run(() => _teacherService.UpdateTeacher(teacher));

                UIHelper.ShowInfo("تم التعديل بنجاح");
                ApplicationLogger.LogUsage("شاشة المعلمين", "تعديل معلم", "تم تعديل بيانات المعلم بنجاح");

                await LoadDataAsync();
                ClearInputs();
            }

            catch (Exception ex)
            {
                UIHelper.ShowException("تعديل المعلم", ex);
            }
        }

        // تعريف الدالة btnDelete_Click لتنفيذ العملية المرتبطة بها.
        private async void btnDelete_Click(object sender, EventArgs e)
        {
            CurrentUser.DemandAction("Teachers", "Delete", "ليس لديك صلاحية حذف المعلمين.");
            if (_selectedTeacherId == 0)
            {

                UIHelper.ShowWarning("اختر معلماً أولاً");

                return;
            }

            if (UIHelper.ShowConfirmation("هل أنت متأكد من حذف المعلم المحدد؟", "تأكيد الحذف"))
            {
                // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
                try
                {

                    await Task.Run(() => _teacherService.DeleteTeacher(_selectedTeacherId));

                    UIHelper.ShowInfo("تم الحذف بنجاح");
                    ApplicationLogger.LogUsage("شاشة المعلمين", "حذف معلم", "تم حذف المعلم بنجاح");

                    await LoadDataAsync();
                    ClearInputs();
                }

                catch (Exception ex)
                {
                    UIHelper.ShowException("حذف المعلم", ex);
                }
            }
        }

        // تعريف الدالة btnClear_Click لتنفيذ العملية المرتبطة بها.
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearInputs();
        }

        // تعريف الدالة btnRefresh_Click لتنفيذ العملية المرتبطة بها.
        private async void btnRefresh_Click(object sender, EventArgs e)
        {

            await LoadDataAsync();
        }

        // تعريف الدالة txtSearch_TextChanged لتنفيذ العملية المرتبطة بها.
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter(txtSearch.Text.Trim());
        }

        // تعريف الدالة dataGridViewTeachers_CellClick لتنفيذ العملية المرتبطة بها.
        private void dataGridViewTeachers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dataGridViewTeachers.Rows[e.RowIndex];
                _selectedTeacherId = Convert.ToInt32(row.Cells["TeacherID"].Value);
                txtEmployeeNumber.Text = row.Cells["EmployeeNumber"].Value?.ToString();
                txtFullName.Text = row.Cells["FullName"].Value?.ToString();
                cmbGender.Text = row.Cells["Gender"].Value?.ToString();
                dtpBirthDate.Value = ReadDateCell(row, "BirthDate", DateTime.Today.AddYears(-30));
                txtBirthPlace.Text = ReadCellText(row, "BirthPlace");
                cmbNationality.Text = ReadCellText(row, "Nationality");
                txtNationalID.Text = ReadCellText(row, "NationalID");
                txtPhone.Text = ReadCellText(row, "Phone");
                txtEmail.Text = ReadCellText(row, "Email");
                txtAddress.Text = ReadCellText(row, "Address");
                cmbQualification.Text = ReadCellText(row, "Qualification");
                txtSpecialization.Text = ReadCellText(row, "Specialization");
                dtpHireDate.Value = ReadDateCell(row, "HireDate", DateTime.Today);
                nudBasicSalary.Value = ReadDecimalCell(row, "BasicSalary", 0);
                nudTransportAllowance.Value = ReadDecimalCell(row, "TransportAllowance", 0);
                nudHousingAllowance.Value = ReadDecimalCell(row, "HousingAllowance", 0);
                cmbStatus.Text = ReadCellText(row, "Status");
                txtNotes.Text = ReadCellText(row, "Notes");
            }
        }

        // تعريف الدالة ReadCellText لتنفيذ العملية المرتبطة بها.
        private string ReadCellText(DataGridViewRow row, string columnName)
        {
            if (row == null || !row.DataGridView.Columns.Contains(columnName))
                return string.Empty;

            object value = row.Cells[columnName].Value;

            return value == null || value == DBNull.Value ? string.Empty : value.ToString();
        }

        // تعريف الدالة ReadDateCell لتنفيذ العملية المرتبطة بها.
        private DateTime ReadDateCell(DataGridViewRow row, string columnName, DateTime fallback)
        {

            return DateTime.TryParse(ReadCellText(row, columnName), out DateTime value) ? value : fallback;
        }

        // تعريف الدالة ReadDecimalCell لتنفيذ العملية المرتبطة بها.
        private decimal ReadDecimalCell(DataGridViewRow row, string columnName, decimal fallback)
        {

            return decimal.TryParse(ReadCellText(row, columnName), out decimal value) ? value : fallback;
        }

        // تعريف الدالة ClearInputs لتنفيذ العملية المرتبطة بها.
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
