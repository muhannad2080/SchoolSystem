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
            UIHelper.NormalizeActionBar(panelButtons);
            InitializeEvents();
            PopulateComboBoxes();

            DataRefreshNotifier.DataChanged += DataRefreshNotifier_DataChanged;
            Disposed += (sender, e) =>
                DataRefreshNotifier.DataChanged -= DataRefreshNotifier_DataChanged;
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
            dataGridViewTeachers.RowHeadersVisible = false;
            dataGridViewTeachers.AllowUserToResizeRows = false;
            dataGridViewTeachers.AllowUserToOrderColumns = true;
            dataGridViewTeachers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewTeachers.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dataGridViewTeachers.ColumnHeadersHeight = 40;
            dataGridViewTeachers.RowTemplate.Height = 36;
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

        private async void DataRefreshNotifier_DataChanged(
            object sender,
            DataRefreshEventArgs e)
        {
            if (IsDisposed || Disposing || !IsHandleCreated)
                return;

            // التحديثات قد تصدر من خدمات تعمل على خيط خلفي؛ اربط الجدول على خيط الواجهة فقط.
            if (InvokeRequired)
            {
                try
                {
                    BeginInvoke(new MethodInvoker(
                        () => DataRefreshNotifier_DataChanged(sender, e)));
                }
                catch (InvalidOperationException)
                {
                    // قد تُغلق الشاشة أثناء جدولة التحديث.
                }

                return;
            }

            if (ReferenceEquals(sender, this) ||
                !DataRefreshNotifier.IsRelated(e, "Teachers"))
            {
                return;
            }

            await LoadDataAsync();
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
            if (dataGridViewTeachers == null)
                return;

            dataGridViewTeachers.SuspendLayout();
            try
            {
                dataGridViewTeachers.AllowUserToAddRows = false;
                dataGridViewTeachers.AllowUserToDeleteRows = false;
                dataGridViewTeachers.AllowUserToResizeRows = false;
                dataGridViewTeachers.AllowUserToResizeColumns = true;
                dataGridViewTeachers.AllowUserToOrderColumns = true;
                dataGridViewTeachers.ReadOnly = true;
                dataGridViewTeachers.MultiSelect = false;
                dataGridViewTeachers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridViewTeachers.RowHeadersVisible = false;
                dataGridViewTeachers.RightToLeft = RightToLeft.Yes;
                dataGridViewTeachers.ScrollBars = ScrollBars.Both;
                dataGridViewTeachers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                dataGridViewTeachers.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
                dataGridViewTeachers.BackgroundColor = Color.White;
                dataGridViewTeachers.BorderStyle = BorderStyle.FixedSingle;
                dataGridViewTeachers.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                dataGridViewTeachers.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                dataGridViewTeachers.EnableHeadersVisualStyles = false;
                dataGridViewTeachers.GridColor = UIHelper.BorderColor;
                dataGridViewTeachers.ColumnHeadersHeight = 44;
                dataGridViewTeachers.ColumnHeadersHeightSizeMode =
                    DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                dataGridViewTeachers.RowTemplate.Height = 38;
                dataGridViewTeachers.ClipboardCopyMode =
                    DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;

                dataGridViewTeachers.ColumnHeadersDefaultCellStyle.BackColor = UIHelper.PrimaryColor;
                dataGridViewTeachers.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dataGridViewTeachers.ColumnHeadersDefaultCellStyle.Font =
                    new Font(UIHelper.FontFamily, 10F, FontStyle.Bold);
                dataGridViewTeachers.ColumnHeadersDefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleCenter;
                dataGridViewTeachers.ColumnHeadersDefaultCellStyle.WrapMode =
                    DataGridViewTriState.True;
                dataGridViewTeachers.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                    UIHelper.PrimaryColor;
                dataGridViewTeachers.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
                dataGridViewTeachers.ColumnHeadersDefaultCellStyle.Padding =
                    new Padding(8, 4, 8, 4);

                dataGridViewTeachers.DefaultCellStyle.BackColor = Color.White;
                dataGridViewTeachers.DefaultCellStyle.ForeColor = UIHelper.TextColor;
                dataGridViewTeachers.DefaultCellStyle.Font =
                    new Font(UIHelper.FontFamily, 9.5F, FontStyle.Regular);
                dataGridViewTeachers.DefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleRight;
                dataGridViewTeachers.DefaultCellStyle.SelectionBackColor = UIHelper.AccentColor;
                dataGridViewTeachers.DefaultCellStyle.SelectionForeColor = Color.White;
                dataGridViewTeachers.DefaultCellStyle.Padding = new Padding(8, 4, 8, 4);
                dataGridViewTeachers.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

                dataGridViewTeachers.AlternatingRowsDefaultCellStyle.BackColor =
                    Color.FromArgb(241, 245, 249);
                dataGridViewTeachers.AlternatingRowsDefaultCellStyle.ForeColor =
                    UIHelper.TextColor;
                dataGridViewTeachers.AlternatingRowsDefaultCellStyle.SelectionBackColor =
                    UIHelper.AccentColor;
                dataGridViewTeachers.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.White;
                dataGridViewTeachers.RowsDefaultCellStyle.BackColor = Color.White;
                dataGridViewTeachers.RowsDefaultCellStyle.ForeColor = UIHelper.TextColor;
                dataGridViewTeachers.RowsDefaultCellStyle.SelectionBackColor = UIHelper.AccentColor;
                dataGridViewTeachers.RowsDefaultCellStyle.SelectionForeColor = Color.White;
            }
            finally
            {
                dataGridViewTeachers.ResumeLayout();
            }
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

            ConfigureTeachersGridColumns();
        }

        // تنظيم جدول المعلمين ليكون جدولًا عمليًا شبيهًا بـExcel:
        // عناوين عربية، ترتيب ثابت، فرز من العناوين، وأحجام واضحة.
        private void ConfigureTeachersGridColumns()
        {
            if (dataGridViewTeachers.Columns.Count == 0)
                return;

            dataGridViewTeachers.RightToLeft = RightToLeft.Yes;
            dataGridViewTeachers.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.None;
            dataGridViewTeachers.ScrollBars = ScrollBars.Both;
            dataGridViewTeachers.AutoSizeRowsMode =
                DataGridViewAutoSizeRowsMode.None;
            dataGridViewTeachers.ColumnHeadersHeight = 40;
            dataGridViewTeachers.RowTemplate.Height = 36;
            dataGridViewTeachers.AllowUserToResizeColumns = true;
            dataGridViewTeachers.AllowUserToResizeRows = false;
            dataGridViewTeachers.AllowUserToOrderColumns = true;
            dataGridViewTeachers.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dataGridViewTeachers.MultiSelect = false;
            dataGridViewTeachers.ReadOnly = true;
            dataGridViewTeachers.RowHeadersVisible = false;

            ConfigureTeacherColumn("TeacherID", "المعرف", 0, 0, false, 60);
            ConfigureTeacherColumn("EmployeeNumber", "الرقم الوظيفي", 1, 105, true, 95);
            ConfigureTeacherColumn("FullName", "اسم المعلم", 2, 180, true, 150);
            ConfigureTeacherColumn("Gender", "الجنس", 3, 75, true, 65);
            ConfigureTeacherColumn("BirthDate", "تاريخ الميلاد", 4, 105, true, 95);
            ConfigureTeacherColumn("BirthPlace", "مكان الميلاد", 5, 110, true, 90);
            ConfigureTeacherColumn("Nationality", "الجنسية", 6, 90, true, 75);
            ConfigureTeacherColumn("NationalID", "رقم الهوية", 7, 115, true, 100);
            ConfigureTeacherColumn("Phone", "الهاتف", 8, 110, true, 95);
            ConfigureTeacherColumn("Email", "البريد الإلكتروني", 9, 150, true, 120);
            ConfigureTeacherColumn("Address", "العنوان", 10, 150, true, 120);
            ConfigureTeacherColumn("Qualification", "المؤهل العلمي", 11, 120, true, 105);
            ConfigureTeacherColumn("Specialization", "التخصص", 12, 120, true, 105);
            ConfigureTeacherColumn("HireDate", "تاريخ التعيين", 13, 105, true, 95);
            ConfigureTeacherColumn("BasicSalary", "الراتب الأساسي", 14, 115, true, 100);
            ConfigureTeacherColumn("TransportAllowance", "بدل النقل", 15, 105, true, 95);
            ConfigureTeacherColumn("HousingAllowance", "بدل السكن", 16, 105, true, 95);
            ConfigureTeacherColumn("Status", "الحالة الوظيفية", 17, 110, true, 95);
            ConfigureTeacherColumn("Notes", "ملاحظات", 18, 180, true, 120);
            ConfigureTeacherColumn("CreatedAt", "تاريخ الإنشاء", 19, 105, false, 95);

            FormatTeacherGridCell("BirthDate", "dd/MM/yyyy");
            FormatTeacherGridCell("HireDate", "dd/MM/yyyy");
            FormatTeacherGridCell("BasicSalary", "N2");
            FormatTeacherGridCell("TransportAllowance", "N2");
            FormatTeacherGridCell("HousingAllowance", "N2");

            dataGridViewTeachers.DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleRight;
            dataGridViewTeachers.ColumnHeadersDefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;
        }

        private void ConfigureTeacherColumn(
            string name,
            string header,
            int displayIndex,
            float fillWeight,
            bool visible,
            int minimumWidth)
        {
            if (!dataGridViewTeachers.Columns.Contains(name))
                return;

            DataGridViewColumn column =
                dataGridViewTeachers.Columns[name];

            column.HeaderText = header;
            column.Visible = visible;
            column.DisplayIndex = displayIndex;

            if (visible)
            {
                // fillWeight holds the intended width in pixels for this spreadsheet-style table.
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                column.MinimumWidth = Math.Min(95, Math.Max(55, minimumWidth));
                column.Width = Math.Max(minimumWidth, (int)fillWeight);
                column.Resizable = DataGridViewTriState.True;
            }

            column.SortMode = DataGridViewColumnSortMode.Automatic;
        }

        private void FormatTeacherGridCell(
            string name,
            string format)
        {
            if (!dataGridViewTeachers.Columns.Contains(name))
                return;

            dataGridViewTeachers.Columns[name]
                .DefaultCellStyle.Format = format;
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
                DataRefreshNotifier.Notify("All", this);
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
                DataRefreshNotifier.Notify("All", this);
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
                    DataRefreshNotifier.Notify("All", this);
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
