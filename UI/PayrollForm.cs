using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;
using SchoolSystem.Models;
using SchoolSystem.Helpers;
using SchoolSystem.Services;


namespace SchoolSystem.UI
{

    /// <summary>
    /// واجهة إدارة العقود الوظيفية فقط.
    /// الراتب المعروض هنا هو الراتب المتفق عليه في العقد، وليس حركة صرف راتب شهرية.
    /// </summary>
    public partial class PayrollForm : UserControl
    {
        // تعريف الدالة ContractService لتنفيذ العملية المرتبطة بها.
        private readonly ContractService contractService = new ContractService();
        // تعريف الدالة TeacherService لتنفيذ العملية المرتبطة بها.
        private readonly TeacherService teacherService = new TeacherService();


        private int selectedContractId = 0;

        private DataTable allContracts;

        // تعريف الدالة PayrollForm لتنفيذ العملية المرتبطة بها.
        public PayrollForm()
        {

            InitializeComponent();

            SchoolSystem.Helpers.UIHelper.ApplyStyle(this);
            ApplyCustomStyles();
            ConfigureContractsGridAppearance();
            SchoolSystem.Helpers.UIHelper.NormalizeActionBar(panelButtons);
            this.Dock = DockStyle.Fill;

            panelSearch.Visible = true;
            txtSearch.Visible = true;
            btnSearch.Visible = true;

            // تحديد أقصى طول للحقول المالية في عقد الراتب.
            txtBasicSalary.MaxLength = 40;
            txtHousing.MaxLength = 40;
            txtTransport.MaxLength = 40;
            txtOther.MaxLength = 40;
            txtDeductions.MaxLength = 40;

            DataRefreshNotifier.DataChanged += DataRefreshNotifier_DataChanged;
            Disposed += (sender, e) =>
                DataRefreshNotifier.DataChanged -= DataRefreshNotifier_DataChanged;

            this.Load += PayrollForm_Load;
        }

        private async void DataRefreshNotifier_DataChanged(
            object sender,
            DataRefreshEventArgs e)
        {
            if (IsDisposed || Disposing || !IsHandleCreated)
                return;

            // تحديث قائمة العقود يجب أن يجري على خيط واجهة WinForms.
            if (InvokeRequired)
            {
                try
                {
                    BeginInvoke(new MethodInvoker(
                        () => DataRefreshNotifier_DataChanged(sender, e)));
                }
                catch (InvalidOperationException)
                {
                    // أُغلقت الشاشة أثناء جدولة التحديث.
                }

                return;
            }

            if (ReferenceEquals(sender, this) || selectedContractId != 0)
                return;

            if (!DataRefreshNotifier.IsRelated(e, "Teachers") &&
                !DataRefreshNotifier.IsRelated(e, "TeacherContracts") &&
                !DataRefreshNotifier.IsRelated(e, "All"))
                return;

            try
            {
                await LoadTeachersAsync();
                await LoadContractsAsync();
            }
            catch (Exception ex)
            {
                ApplicationLogger.LogException(
                    "شاشة العقود الوظيفية: التحديث التلقائي",
                    ex);
            }
        }

        // جدول العقود بأسلوب Excel؛ القيم الإجمالية والصافية تبقى للقراءة فقط ومميزة بصريًا.
        private void ConfigureContractsGridAppearance()
        {
            if (dataGridViewContracts == null)
                return;

            dataGridViewContracts.SuspendLayout();
            try
            {
                dataGridViewContracts.AllowUserToAddRows = false;
                dataGridViewContracts.AllowUserToDeleteRows = false;
                dataGridViewContracts.AllowUserToResizeRows = false;
                dataGridViewContracts.AllowUserToResizeColumns = true;
                dataGridViewContracts.AllowUserToOrderColumns = true;
                dataGridViewContracts.ReadOnly = true;
                dataGridViewContracts.MultiSelect = false;
                dataGridViewContracts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridViewContracts.RowHeadersVisible = false;
                dataGridViewContracts.RightToLeft = RightToLeft.Yes;
                dataGridViewContracts.ScrollBars = ScrollBars.Both;
                dataGridViewContracts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                dataGridViewContracts.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
                dataGridViewContracts.BackgroundColor = System.Drawing.Color.White;
                dataGridViewContracts.BorderStyle = BorderStyle.FixedSingle;
                dataGridViewContracts.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                dataGridViewContracts.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                dataGridViewContracts.EnableHeadersVisualStyles = false;
                dataGridViewContracts.GridColor = UIHelper.BorderColor;
                dataGridViewContracts.ColumnHeadersHeight = 44;
                dataGridViewContracts.ColumnHeadersHeightSizeMode =
                    DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                dataGridViewContracts.RowTemplate.Height = 38;
                dataGridViewContracts.ClipboardCopyMode =
                    DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;

                dataGridViewContracts.ColumnHeadersDefaultCellStyle.BackColor = UIHelper.PrimaryColor;
                dataGridViewContracts.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
                dataGridViewContracts.ColumnHeadersDefaultCellStyle.Font =
                    new Font(UIHelper.FontFamily, 10F, FontStyle.Bold);
                dataGridViewContracts.ColumnHeadersDefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleCenter;
                dataGridViewContracts.ColumnHeadersDefaultCellStyle.WrapMode =
                    DataGridViewTriState.True;
                dataGridViewContracts.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                    UIHelper.PrimaryColor;
                dataGridViewContracts.ColumnHeadersDefaultCellStyle.SelectionForeColor =
                    System.Drawing.Color.White;
                dataGridViewContracts.ColumnHeadersDefaultCellStyle.Padding =
                    new Padding(8, 4, 8, 4);

                dataGridViewContracts.DefaultCellStyle.BackColor = System.Drawing.Color.White;
                dataGridViewContracts.DefaultCellStyle.ForeColor = UIHelper.TextColor;
                dataGridViewContracts.DefaultCellStyle.Font =
                    new Font(UIHelper.FontFamily, 9.5F, FontStyle.Regular);
                dataGridViewContracts.DefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleRight;
                dataGridViewContracts.DefaultCellStyle.SelectionBackColor = UIHelper.AccentColor;
                dataGridViewContracts.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
                dataGridViewContracts.DefaultCellStyle.Padding = new Padding(8, 4, 8, 4);
                dataGridViewContracts.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

                dataGridViewContracts.AlternatingRowsDefaultCellStyle.BackColor =
                    System.Drawing.Color.FromArgb(241, 245, 249);
                dataGridViewContracts.AlternatingRowsDefaultCellStyle.ForeColor = UIHelper.TextColor;
                dataGridViewContracts.AlternatingRowsDefaultCellStyle.SelectionBackColor = UIHelper.AccentColor;
                dataGridViewContracts.AlternatingRowsDefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
                dataGridViewContracts.RowsDefaultCellStyle.BackColor = System.Drawing.Color.White;
                dataGridViewContracts.RowsDefaultCellStyle.ForeColor = UIHelper.TextColor;
                dataGridViewContracts.RowsDefaultCellStyle.SelectionBackColor = UIHelper.AccentColor;
                dataGridViewContracts.RowsDefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            }
            finally
            {
                dataGridViewContracts.ResumeLayout();
            }
        }

        // تعريف الدالة ApplyCustomStyles لتنفيذ العملية المرتبطة بها.
        private void ApplyCustomStyles()
        {
            UIHelper.StyleDataGridView(dataGridViewContracts);
            UIHelper.StylePrimaryButton(btnAdd);
            UIHelper.StylePrimaryButton(btnUpdate);
            UIHelper.StyleDangerButton(btnDelete);
            UIHelper.StyleButton(btnClear, UIHelper.NeutralColor);
            UIHelper.StyleTextBox(txtSearch);
            UIHelper.StyleTextBox(txtContractNumber);
            UIHelper.StyleTextBox(txtBasicSalary);
            UIHelper.StyleTextBox(txtHousing);
            UIHelper.StyleTextBox(txtTransport);
            UIHelper.StyleTextBox(txtOther);
            UIHelper.StyleTextBox(txtDeductions);
            UIHelper.StyleTextBox(txtTotal);
            UIHelper.StyleTextBox(txtNetSalary);
            UIHelper.StyleTextBox(txtNotes);
            UIHelper.StyleComboBox(cmbTeacher);
            UIHelper.StyleComboBox(cmbContractType);
            UIHelper.StyleComboBox(cmbContractStatus);
            UIHelper.StyleComboBox(cmbPaymentMethod);
            lblRecordCount.ForeColor = UIHelper.MutedTextColor;

            // القيم المحسوبة لا تُحرر يدويًا؛ مصدرها هو مكونات الراتب التي يدخلها المستخدم.
            txtTotal.ReadOnly = true;
            txtTotal.TabStop = false;
            txtNetSalary.ReadOnly = true;
            txtNetSalary.TabStop = false;
            txtTotal.BackColor = UIHelper.SurfaceElevatedColor;
            txtNetSalary.BackColor = UIHelper.SurfaceElevatedColor;
        }

        // تعريف الدالة PayrollForm_Load لتنفيذ العملية المرتبطة بها.
        private async void PayrollForm_Load(object sender, EventArgs e)
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                SetupComboBoxes();
                HookValidationEvents();


                await LoadTeachersAsync();

                await LoadContractsAsync();

                ClearInputs();
            }

            catch (Exception ex)
            {
                UIHelper.ShowException("تحميل شاشة العقود الوظيفية", ex);
            }
        }

        // تعريف الدالة SetupComboBoxes لتنفيذ العملية المرتبطة بها.
        private void SetupComboBoxes()
        {
            cmbContractType.Items.Clear();
            cmbContractType.Items.Add("دائم");
            cmbContractType.Items.Add("مؤقت");
            cmbContractType.Items.Add("موسمي");
            cmbContractType.Items.Add("مستشار");
            cmbContractType.Items.Add("دوام كامل");
            cmbContractType.Items.Add("دوام جزئي");
            cmbContractType.Items.Add("بالساعة");
            cmbContractType.Items.Add("تعاقد سنوي");

            if (cmbContractType.Items.Count > 0)
                cmbContractType.SelectedIndex = 0;

            cmbContractStatus.Items.Clear();
            cmbContractStatus.Items.Add("ساري");
            cmbContractStatus.Items.Add("منتهي");
            cmbContractStatus.Items.Add("موقوف");
            cmbContractStatus.Items.Add("ملغي");

            if (cmbContractStatus.Items.Count > 0)
                cmbContractStatus.SelectedIndex = 0;

            cmbPaymentMethod.Items.Clear();
            cmbPaymentMethod.Items.Add("نقداً");
            cmbPaymentMethod.Items.Add("حوالة");
            cmbPaymentMethod.Items.Add("بنك");
            cmbPaymentMethod.Items.Add("محفظة إلكترونية");

            if (cmbPaymentMethod.Items.Count > 0)
                cmbPaymentMethod.SelectedIndex = 0;
        }

        // تعريف الدالة HookValidationEvents لتنفيذ العملية المرتبطة بها.
        private void HookValidationEvents()
        {

            txtBasicSalary.KeyPress += NumericTextBox_KeyPress;

            txtHousing.KeyPress += NumericTextBox_KeyPress;

            txtTransport.KeyPress += NumericTextBox_KeyPress;

            txtOther.KeyPress += NumericTextBox_KeyPress;

            txtDeductions.KeyPress += NumericTextBox_KeyPress;


            txtContractNumber.KeyPress += ContractNumber_KeyPress;

            txtNotes.KeyPress += Notes_KeyPress;
        }

        // تعريف الدالة LoadTeachersAsync لتنفيذ العملية المرتبطة بها.
        private async Task LoadTeachersAsync()
        {

            DataTable teachers = await Task.Run(() => teacherService.GetActiveTeachers());

            cmbTeacher.DataSource = teachers;

            if (teachers.Columns.Contains("TeacherName"))
                cmbTeacher.DisplayMember = "TeacherName";

            else if (teachers.Columns.Contains("FullName"))

                cmbTeacher.DisplayMember = "FullName";

            else

                cmbTeacher.DisplayMember = teachers.Columns[1].ColumnName;

            cmbTeacher.ValueMember = "TeacherID";

            if (cmbTeacher.Items.Count > 0)
                cmbTeacher.SelectedIndex = 0;
        }

        // تعريف الدالة LoadContractsAsync لتنفيذ العملية المرتبطة بها.
        private async Task LoadContractsAsync()
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                Cursor = Cursors.WaitCursor;


                allContracts = await Task.Run(() => contractService.GetAllContracts());
                ApplyFilter(txtSearch.Text.Trim());

                Cursor = Cursors.Default;
            }

            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                UIHelper.ShowException("تحميل العقود الوظيفية", ex);
            }
        }

        // تعريف الدالة ApplyFilter لتنفيذ العملية المرتبطة بها.
        private void ApplyFilter(string searchText)
        {
            try
            {
                if (allContracts == null)
                {
                    dataGridViewContracts.DataSource = null;
                    lblRecordCount.Text = "عدد العقود: 0";
                    return;
                }

                string term = (searchText ?? string.Empty).Trim();
                DataTable filtered = allContracts.Clone();

                foreach (DataRow row in allContracts.Rows)
                {
                    if (row == null)
                        continue;

                    if (string.IsNullOrWhiteSpace(term) || ContractRowContains(row, term))
                        filtered.ImportRow(row);
                }

                dataGridViewContracts.DataSource = filtered.DefaultView;
                lblRecordCount.Text = "عدد العقود: " + filtered.Rows.Count;
                FormatGridColumns();
            }
            catch (Exception ex)
            {
                ApplicationLogger.LogException("شاشة العقود الوظيفية: البحث", ex);
                dataGridViewContracts.DataSource = allContracts != null ? allContracts.DefaultView : null;
                lblRecordCount.Text = "عدد العقود: " + (allContracts != null ? allContracts.Rows.Count : 0);
            }
        }

        private static bool ContractRowContains(DataRow row, string term)
        {
            string comparison = term.Trim();

            string[] searchableColumns =
            {
                "TeacherName",
                "ContractNumber",
                "ContractType",
                "ContractStatus",
                "PaymentMethod",
                "StartDate",
                "EndDate",
                "BasicSalary",
                "Allowances",
                "Deductions"
            };

            foreach (string column in searchableColumns)
            {
                if (!row.Table.Columns.Contains(column))
                    continue;

                object value = row[column];
                if (value == null || value == DBNull.Value)
                    continue;

                string text = Convert.ToString(value);
                if (!string.IsNullOrEmpty(text) &&
                    text.IndexOf(comparison, StringComparison.CurrentCultureIgnoreCase) >= 0)
                    return true;
            }

            return false;
        }

        // تعريف الدالة FormatGridColumns لتنفيذ العملية المرتبطة بها.
        private void FormatGridColumns()
        {
            if (dataGridViewContracts.Columns.Count == 0)
                return;

            if (dataGridViewContracts.Columns.Contains("ContractID"))
            {
                dataGridViewContracts.Columns["ContractID"].HeaderText = "الرقم";
                dataGridViewContracts.Columns["ContractID"].Width = 60;
            }

            if (dataGridViewContracts.Columns.Contains("TeacherID"))
                dataGridViewContracts.Columns["TeacherID"].Visible = false;

            if (dataGridViewContracts.Columns.Contains("TeacherName"))
                dataGridViewContracts.Columns["TeacherName"].HeaderText = "المعلم";

            if (dataGridViewContracts.Columns.Contains("ContractNumber"))
                dataGridViewContracts.Columns["ContractNumber"].HeaderText = "رقم العقد";

            if (dataGridViewContracts.Columns.Contains("ContractType"))
                dataGridViewContracts.Columns["ContractType"].HeaderText = "نوع العقد";

            if (dataGridViewContracts.Columns.Contains("ContractStatus"))
                dataGridViewContracts.Columns["ContractStatus"].HeaderText = "الحالة";

            if (dataGridViewContracts.Columns.Contains("BasicSalary"))
                dataGridViewContracts.Columns["BasicSalary"].HeaderText = "الأساسي";

            if (dataGridViewContracts.Columns.Contains("HousingAllowance"))
                dataGridViewContracts.Columns["HousingAllowance"].HeaderText = "بدل السكن";

            if (dataGridViewContracts.Columns.Contains("TransportAllowance"))
                dataGridViewContracts.Columns["TransportAllowance"].HeaderText = "بدل النقل";

            if (dataGridViewContracts.Columns.Contains("OtherAllowances"))
                dataGridViewContracts.Columns["OtherAllowances"].HeaderText = "بدلات أخرى";

            if (dataGridViewContracts.Columns.Contains("Deductions"))
                dataGridViewContracts.Columns["Deductions"].HeaderText = "الخصومات";

            if (dataGridViewContracts.Columns.Contains("TotalSalary"))
            {
                dataGridViewContracts.Columns["TotalSalary"].HeaderText = "الإجمالي";
                dataGridViewContracts.Columns["TotalSalary"].DefaultCellStyle.Font =
                    new Font(UIHelper.FontFamily, UIHelper.BodyFontSize, FontStyle.Bold);
                dataGridViewContracts.Columns["TotalSalary"].DefaultCellStyle.ForeColor =
                    UIHelper.SuccessColor;
            }

            if (dataGridViewContracts.Columns.Contains("NetSalary"))
            {
                dataGridViewContracts.Columns["NetSalary"].HeaderText = "الصافي";
                dataGridViewContracts.Columns["NetSalary"].DefaultCellStyle.Font =
                    new Font(UIHelper.FontFamily, UIHelper.BodyFontSize, FontStyle.Bold);
                dataGridViewContracts.Columns["NetSalary"].DefaultCellStyle.ForeColor =
                    UIHelper.AccentColor;
            }

            if (dataGridViewContracts.Columns.Contains("StartDate"))
                dataGridViewContracts.Columns["StartDate"].HeaderText = "بداية العقد";

            if (dataGridViewContracts.Columns.Contains("EndDate"))
                dataGridViewContracts.Columns["EndDate"].HeaderText = "نهاية العقد";

            if (dataGridViewContracts.Columns.Contains("PaymentMethod"))
                dataGridViewContracts.Columns["PaymentMethod"].HeaderText = "طريقة الصرف";

            if (dataGridViewContracts.Columns.Contains("Notes"))
                dataGridViewContracts.Columns["Notes"].HeaderText = "ملاحظات";

            if (dataGridViewContracts.Columns.Contains("CreatedAt"))
                dataGridViewContracts.Columns["CreatedAt"].HeaderText = "تاريخ الإدخال";

            if (dataGridViewContracts.Columns.Contains("UpdatedAt"))
                dataGridViewContracts.Columns["UpdatedAt"].HeaderText = "آخر تعديل";

            dataGridViewContracts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            SetContractColumnWidth("ContractID", 78);
            SetContractColumnWidth("TeacherName", 180);
            SetContractColumnWidth("ContractNumber", 145);
            SetContractColumnWidth("ContractType", 125);
            SetContractColumnWidth("ContractStatus", 110);
            SetContractColumnWidth("BasicSalary", 120);
            SetContractColumnWidth("HousingAllowance", 120);
            SetContractColumnWidth("TransportAllowance", 120);
            SetContractColumnWidth("OtherAllowances", 120);
            SetContractColumnWidth("Deductions", 115);
            SetContractColumnWidth("TotalSalary", 125);
            SetContractColumnWidth("NetSalary", 125);
            SetContractColumnWidth("StartDate", 120);
            SetContractColumnWidth("EndDate", 120);
            SetContractColumnWidth("PaymentMethod", 125);
            SetContractColumnWidth("Notes", 200);
            SetContractColumnWidth("CreatedAt", 145);
            SetContractColumnWidth("UpdatedAt", 145);

            FormatContractDate("StartDate");
            FormatContractDate("EndDate");
            FormatContractDate("CreatedAt");
            FormatContractDate("UpdatedAt");

            foreach (DataGridViewColumn column in dataGridViewContracts.Columns)
            {
                if (column.Visible)
                    column.SortMode = DataGridViewColumnSortMode.Automatic;
            }
        }

        private void SetContractColumnWidth(string name, int width)
        {
            if (!dataGridViewContracts.Columns.Contains(name))
                return;

            DataGridViewColumn column = dataGridViewContracts.Columns[name];
            if (!column.Visible)
                return;

            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            column.MinimumWidth = Math.Min(100, Math.Max(55, width));
            column.Width = width;
            column.Resizable = DataGridViewTriState.True;
        }

        private void FormatContractDate(string name)
        {
            if (dataGridViewContracts.Columns.Contains(name))
                dataGridViewContracts.Columns[name].DefaultCellStyle.Format = "dd/MM/yyyy";
        }

        // تعريف زر البحث لتنفيذ تصفية العقود.
        private void btnSearch_Click(object sender, EventArgs e)
        {
            ApplyFilter(txtSearch.Text);
            txtSearch.Focus();
        }


        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter(txtSearch.Text.Trim());
        }

        // تعريف الدالة SalaryField_TextChanged لتنفيذ العملية المرتبطة بها.
        private void SalaryField_TextChanged(object sender, EventArgs e)
        {
            CalculateTotal();
        }

        // تعريف الدالة ParseMoney لتنفيذ العملية المرتبطة بها.
        private decimal ParseMoney(string value)
        {

            return UIHelper.TryParseDecimal(value, out decimal result) ? result : 0m;
        }

        // تعريف الدالة CalculateTotal لتنفيذ العملية المرتبطة بها.
        private void CalculateTotal()
        {
            decimal basic = ParseMoney(txtBasicSalary.Text);
            decimal housing = ParseMoney(txtHousing.Text);
            decimal transport = ParseMoney(txtTransport.Text);
            decimal other = ParseMoney(txtOther.Text);
            decimal deductions = ParseMoney(txtDeductions.Text);

            decimal total = basic + housing + transport + other;
            decimal net = total - deductions;

            if (net < 0)
                net = 0;

            txtTotal.Text = total.ToString("N2");
            txtNetSalary.Text = net.ToString("N2");
        }

        // تعريف الدالة ClearInputs لتنفيذ العملية المرتبطة بها.
        private void ClearInputs()
        {
            selectedContractId = 0;

            if (cmbTeacher.Items.Count > 0)
                cmbTeacher.SelectedIndex = 0;

            if (cmbContractType.Items.Count > 0)
                cmbContractType.SelectedIndex = 0;

            if (cmbContractStatus.Items.Count > 0)
                cmbContractStatus.SelectedIndex = 0;

            if (cmbPaymentMethod.Items.Count > 0)
                cmbPaymentMethod.SelectedIndex = 0;

            txtContractNumber.Text = GenerateContractNumber();
            txtBasicSalary.Text = "0";
            txtHousing.Text = "0";
            txtTransport.Text = "0";
            txtOther.Text = "0";
            txtDeductions.Text = "0";
            txtTotal.Text = "0";
            txtNetSalary.Text = "0";

            dtpStartDate.Value = DateTime.Today;
            dtpEndDate.Checked = false;

            txtNotes.Clear();
            CalculateTotal();
        }

        // تعريف الدالة GenerateContractNumber لتنفيذ العملية المرتبطة بها.
        private string GenerateContractNumber()
        {

            return "CON-" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }

        // تعريف الدالة GetContractFromInputs لتنفيذ العملية المرتبطة بها.
        private TeacherContract GetContractFromInputs()
        {
            if (cmbTeacher.SelectedValue == null || !int.TryParse(cmbTeacher.SelectedValue.ToString(), out int teacherId) || teacherId <= 0)

                throw new InvalidOperationException("يرجى اختيار المعلم.");
            string contractNumber = txtContractNumber.Text.Trim();
            if (string.IsNullOrWhiteSpace(contractNumber))

                throw new InvalidOperationException("رقم العقد مطلوب.");

            if (contractNumber.Length > 50)

                throw new InvalidOperationException("رقم العقد يجب ألا يتجاوز 50 حرفاً.");

            if (contractNumber.IndexOfAny(new[] { '\r', '\n', '\t' }) >= 0)

                throw new InvalidOperationException("رقم العقد يحتوي على محارف غير صالحة.");

            if (cmbContractType.SelectedIndex < 0 || string.IsNullOrWhiteSpace(cmbContractType.Text))

                throw new InvalidOperationException("يرجى اختيار نوع العقد.");

            if (cmbContractStatus.SelectedIndex < 0 || string.IsNullOrWhiteSpace(cmbContractStatus.Text))

                throw new InvalidOperationException("يرجى اختيار حالة العقد.");

            if (cmbPaymentMethod.SelectedIndex < 0 || string.IsNullOrWhiteSpace(cmbPaymentMethod.Text))

                throw new InvalidOperationException("يرجى اختيار طريقة الصرف.");

            decimal basicSalary;
            decimal housingAllowance;
            decimal transportAllowance;
            decimal otherAllowances;
            decimal deductions;
            if (!UIHelper.TryParseDecimal(txtBasicSalary.Text, out basicSalary) || basicSalary <= 0)

                throw new InvalidOperationException("الراتب الأساسي يجب أن يكون أكبر من صفر.");

            if (!UIHelper.TryParseDecimal(txtHousing.Text, out housingAllowance) || housingAllowance < 0)

                throw new InvalidOperationException("بدل السكن يجب أن يكون رقمًا غير سالب.");

            if (!UIHelper.TryParseDecimal(txtTransport.Text, out transportAllowance) || transportAllowance < 0)

                throw new InvalidOperationException("بدل النقل يجب أن يكون رقمًا غير سالب.");

            if (!UIHelper.TryParseDecimal(txtOther.Text, out otherAllowances) || otherAllowances < 0)

                throw new InvalidOperationException("البدلات الأخرى يجب أن تكون رقمًا غير سالب.");

            if (!UIHelper.TryParseDecimal(txtDeductions.Text, out deductions) || deductions < 0)

                throw new InvalidOperationException("الخصومات يجب أن تكون رقمًا غير سالب.");

            decimal totalSalary = basicSalary + housingAllowance + transportAllowance + otherAllowances;
            if (basicSalary > 100000000m || housingAllowance > 100000000m ||
                transportAllowance > 100000000m || otherAllowances > 100000000m ||
                deductions > 100000000m)

                throw new InvalidOperationException("القيمة المالية تتجاوز الحد المسموح به.");

            if (totalSalary > 100000000m)

                throw new InvalidOperationException("إجمالي الراتب يتجاوز الحد المسموح به.");

            if (deductions > totalSalary)

                throw new InvalidOperationException("لا يمكن أن تتجاوز الخصومات إجمالي الراتب.");

            if (txtNotes.Text.Trim().Length > 1000)

                throw new InvalidOperationException("الملاحظات يجب ألا تتجاوز 1000 حرف.");

            if (dtpEndDate.Checked && dtpEndDate.Value.Date < dtpStartDate.Value.Date)

                throw new InvalidOperationException("تاريخ نهاية العقد يجب أن يكون بعد تاريخ بدايته.");

            TeacherContract contract = new TeacherContract();

            contract.ContractID = selectedContractId;
            contract.TeacherID = teacherId;
            contract.ContractNumber = contractNumber;
            contract.ContractType = cmbContractType.Text;
            contract.ContractStatus = cmbContractStatus.Text;
            contract.BasicSalary = basicSalary;
            contract.HousingAllowance = housingAllowance;
            contract.TransportAllowance = transportAllowance;
            contract.OtherAllowances = otherAllowances;
            contract.Deductions = deductions;
            contract.StartDate = dtpStartDate.Value.Date;
            contract.EndDate = dtpEndDate.Checked ? dtpEndDate.Value.Date : (DateTime?)null;
            contract.PaymentMethod = cmbPaymentMethod.Text;
            contract.Notes = txtNotes.Text.Trim();

            contractService.CalculateSalary(contract);

            txtTotal.Text = contract.TotalSalary.ToString("N2");
            txtNetSalary.Text = contract.NetSalary.ToString("N2");


            return contract;
        }

        // تعريف الدالة dataGridViewContracts_CellClick لتنفيذ العملية المرتبطة بها.
        private void dataGridViewContracts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dataGridViewContracts.Rows.Count == 0)
                return;

            DataRowView rowView =
                dataGridViewContracts.Rows[e.RowIndex].DataBoundItem as DataRowView;

            if (rowView != null)
                FillFieldsFromRow(rowView.Row);
        }

        // تعريف الدالة FillFieldsFromRow لتنفيذ العملية المرتبطة بها.
        private void FillFieldsFromRow(DataRow row)
        {
            if (row == null)
                return;

            selectedContractId = row["ContractID"] != DBNull.Value && int.TryParse(row["ContractID"].ToString(), out int contractId)
                ? contractId
                : 0;

            if (row["TeacherID"] != DBNull.Value && int.TryParse(row["TeacherID"].ToString(), out int teacherId))
                cmbTeacher.SelectedValue = teacherId;

            txtContractNumber.Text =
                row["ContractNumber"] == DBNull.Value ? "" : row["ContractNumber"].ToString();

            cmbContractType.Text =
                row["ContractType"] == DBNull.Value ? "" : row["ContractType"].ToString();

            cmbContractStatus.Text =
                row["ContractStatus"] == DBNull.Value ? "ساري" : row["ContractStatus"].ToString();

            txtBasicSalary.Text =
                row["BasicSalary"] == DBNull.Value ? "0" : row["BasicSalary"].ToString();

            txtHousing.Text =
                row["HousingAllowance"] == DBNull.Value ? "0" : row["HousingAllowance"].ToString();

            txtTransport.Text =
                row["TransportAllowance"] == DBNull.Value ? "0" : row["TransportAllowance"].ToString();

            txtOther.Text =
                row["OtherAllowances"] == DBNull.Value ? "0" : row["OtherAllowances"].ToString();

            txtDeductions.Text =
                row["Deductions"] == DBNull.Value ? "0" : row["Deductions"].ToString();

            txtTotal.Text =
                row["TotalSalary"] == DBNull.Value ? "0" : row["TotalSalary"].ToString();

            txtNetSalary.Text =
                row["NetSalary"] == DBNull.Value ? "0" : row["NetSalary"].ToString();

            if (row["StartDate"] != DBNull.Value && DateTime.TryParse(row["StartDate"].ToString(), out DateTime startDate))
                dtpStartDate.Value = startDate;

            if (row["EndDate"] != DBNull.Value && DateTime.TryParse(row["EndDate"].ToString(), out DateTime endDate))
            {
                dtpEndDate.Checked = true;
                dtpEndDate.Value = endDate;
            }

            else
            {
                dtpEndDate.Checked = false;
            }

            cmbPaymentMethod.Text =
                row["PaymentMethod"] == DBNull.Value ? "نقداً" : row["PaymentMethod"].ToString();

            txtNotes.Text =
                row["Notes"] == DBNull.Value ? "" : row["Notes"].ToString();

            CalculateTotal();
        }

        // تعريف الدالة btnAdd_Click لتنفيذ العملية المرتبطة بها.
        private async void btnAdd_Click(object sender, EventArgs e)
        {
            if (cmbTeacher.SelectedValue == null)
            {
                UIHelper.ShowWarning("اختر معلماً.");

                return;
            }

            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                TeacherContract contract = GetContractFromInputs();

                Cursor = Cursors.WaitCursor;

                await Task.Run(() => contractService.AddContract(contract));
                Cursor = Cursors.Default;

                UIHelper.ShowInfo("تمت إضافة العقد بنجاح.");


                await LoadContractsAsync();
                DataRefreshNotifier.Notify("TeacherContracts", this);
                ClearInputs();
            }

            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                UIHelper.ShowException("إضافة العقد الوظيفي", ex);
            }
        }

        // تعريف الدالة btnUpdate_Click لتنفيذ العملية المرتبطة بها.
        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedContractId == 0)
            {
                UIHelper.ShowWarning("اختر عقداً من الجدول.");

                return;
            }

            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                TeacherContract contract = GetContractFromInputs();

                Cursor = Cursors.WaitCursor;

                bool updated = await Task.Run(() => contractService.UpdateContract(contract));
                Cursor = Cursors.Default;

                if (updated)
                    UIHelper.ShowInfo("تم تعديل العقد بنجاح.");

                else

                    UIHelper.ShowWarning("لم يتم العثور على العقد أو لم يتم تعديله.");


                await LoadContractsAsync();
                if (updated)
                    DataRefreshNotifier.Notify("TeacherContracts", this);
                ClearInputs();
            }

            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                UIHelper.ShowException("تعديل العقد الوظيفي", ex);
            }
        }

        // تعريف الدالة btnDelete_Click لتنفيذ العملية المرتبطة بها.
        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedContractId == 0)
            {
                UIHelper.ShowWarning("اختر عقداً من الجدول.");

                return;
            }


            bool result = UIHelper.ShowConfirmation(
                "هل تريد حذف هذا العقد؟",
                "تأكيد الحذف");

            if (!result)
                return;

            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                Cursor = Cursors.WaitCursor;

                bool deleted = await Task.Run(() => contractService.DeleteContract(selectedContractId));
                Cursor = Cursors.Default;

                if (deleted)
                    UIHelper.ShowInfo("تم حذف العقد بنجاح.");

                else

                    UIHelper.ShowWarning("لم يتم العثور على العقد أو لم يتم حذفه.");


                await LoadContractsAsync();
                if (deleted)
                    DataRefreshNotifier.Notify("TeacherContracts", this);
                ClearInputs();
            }

            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                UIHelper.ShowException("حذف العقد الوظيفي", ex);
            }
        }

        // تعريف الدالة btnClear_Click لتنفيذ العملية المرتبطة بها.
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearInputs();
        }

        // تعريف الدالة NumericTextBox_KeyPress لتنفيذ العملية المرتبطة بها.
        private void NumericTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox txt = sender as TextBox;

            if (char.IsControl(e.KeyChar))
                return;

            if (char.IsDigit(e.KeyChar))
                return;

            if ((e.KeyChar == '.' || e.KeyChar == ',' || e.KeyChar == '٫' || e.KeyChar == '٬') &&
                txt != null &&
                txt.Text.IndexOfAny(new[] { '.', ',', '٫', '٬' }) < 0)
                return;

            e.Handled = true;
        }

        // تعريف الدالة ContractNumber_KeyPress لتنفيذ العملية المرتبطة بها.
        private void ContractNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
                return;

            if (char.IsLetterOrDigit(e.KeyChar))
                return;

            if (e.KeyChar == '-' || e.KeyChar == '_' || e.KeyChar == '/')
                return;

            e.Handled = true;
        }

        // تعريف الدالة Notes_KeyPress لتنفيذ العملية المرتبطة بها.
        private void Notes_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
                return;

            if (char.IsLetterOrDigit(e.KeyChar))
                return;

            string allowed = "ءآأؤإئابةتثجحخدذرزسشصضطظعغفقكلمنهويىة لاabcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.،,/ ";
            if (!allowed.Contains(e.KeyChar.ToString()))
                e.Handled = true;
        }
    }
}
