using System;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using SchoolSystem.Helpers;
using SchoolSystem.Models;
using SchoolSystem.Services;

namespace SchoolSystem.UI
{
    /// <summary>
    /// واجهة تسجيل وصرف الرواتب الشهرية.
    /// تسجل الراتب الفعلي للشهر وتُنشئ سند الصرف المالي المرتبط به تلقائياً.
    /// </summary>
    public partial class PayrollRecordsForm : UserControl
    {
        private readonly PayrollService payrollService = new PayrollService();
        private readonly TeacherService teacherService = new TeacherService();
        private readonly ContractService contractService = new ContractService();

        private DataTable allPayrolls;
        private bool suppressTeacherChange;
        private bool isBusy;
        private int activeContractId;
        private int entryStateVersion;

        public PayrollRecordsForm()
        {
            InitializeComponent();
            UIHelper.ApplyStyle(this);
            this.Dock = DockStyle.Fill;
            ConfigureControls();
            ConfigurePayrollGridAppearance();
            UIHelper.NormalizeActionBar(panelButtons);

            DataRefreshNotifier.DataChanged += DataRefreshNotifier_DataChanged;
            Disposed += (sender, e) =>
                DataRefreshNotifier.DataChanged -= DataRefreshNotifier_DataChanged;

            this.Load += PayrollRecordsForm_Load;
        }

        private async void DataRefreshNotifier_DataChanged(
            object sender,
            DataRefreshEventArgs e)
        {
            if (IsDisposed || Disposing || !IsHandleCreated)
                return;

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

            if (ReferenceEquals(sender, this) || isBusy)
                return;

            if (!DataRefreshNotifier.IsRelated(e, "Teachers") &&
                !DataRefreshNotifier.IsRelated(e, "TeacherContracts") &&
                !DataRefreshNotifier.IsRelated(e, "Payroll") &&
                !DataRefreshNotifier.IsRelated(e, "Vouchers") &&
                !DataRefreshNotifier.IsRelated(e, "All"))
                return;

            try
            {
                await LoadTeachersAsync();
                await LoadPayrollsAsync();
            }
            catch (Exception ex)
            {
                ApplicationLogger.LogException(
                    "شاشة مسير الرواتب: التحديث التلقائي",
                    ex);
            }
        }

        private void ConfigureControls()
        {
            UIHelper.StyleDataGridView(dataGridViewPayrolls);
            UIHelper.StylePrimaryButton(btnAdd);
            UIHelper.StyleButton(btnRefresh, UIHelper.NeutralColor);
            UIHelper.StyleButton(btnClear, UIHelper.NeutralColor);
            UIHelper.StyleTextBox(txtBasicSalary);
            UIHelper.StyleTextBox(txtAllowances);
            UIHelper.StyleTextBox(txtDeductions);
            UIHelper.StyleTextBox(txtNetSalary);
            UIHelper.StyleTextBox(txtNotes);
            UIHelper.StyleTextBox(txtSearch);
            UIHelper.StyleComboBox(cmbTeacher);
            UIHelper.StyleComboBox(cmbMonth);
            UIHelper.StyleComboBox(cmbPaymentMethod);

            txtNetSalary.ReadOnly = true;
            txtNetSalary.BackColor = UIHelper.SurfaceElevatedColor;
            lblRecordCount.ForeColor = UIHelper.MutedTextColor;

            txtBasicSalary.MaxLength = 40;
            txtAllowances.MaxLength = 40;
            txtDeductions.MaxLength = 40;
            txtNotes.MaxLength = 1000;
            dtpPaymentDate.MaxDate = DateTime.Today;

            txtBasicSalary.TextChanged += SalaryField_TextChanged;
            txtAllowances.TextChanged += SalaryField_TextChanged;
            txtDeductions.TextChanged += SalaryField_TextChanged;

            txtBasicSalary.KeyPress += NumericTextBox_KeyPress;
            txtAllowances.KeyPress += NumericTextBox_KeyPress;
            txtDeductions.KeyPress += NumericTextBox_KeyPress;
            txtSearch.TextChanged += txtSearch_TextChanged;
            cmbTeacher.SelectedIndexChanged += cmbTeacher_SelectedIndexChanged;
            cmbMonth.SelectedIndexChanged += cmbMonth_SelectedIndexChanged;
            txtSalaryYear.ValueChanged += txtSalaryYear_ValueChanged;
            dataGridViewPayrolls.DataBindingComplete += (s, e) => FormatGridColumns();
        }

        private async void PayrollRecordsForm_Load(object sender, EventArgs e)
        {
            try
            {
                SetupMonths();
                SetupPaymentMethods();
                await LoadTeachersAsync();
                await LoadPayrollsAsync();
                ClearInputs();
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("تحميل سجلات الرواتب", ex);
            }
        }

        private void SetupMonths()
        {
            cmbMonth.Items.Clear();
            string[] months =
            {
                "يناير", "فبراير", "مارس", "أبريل", "مايو", "يونيو",
                "يوليو", "أغسطس", "سبتمبر", "أكتوبر", "نوفمبر", "ديسمبر"
            };

            for (int i = 0; i < months.Length; i++)
                cmbMonth.Items.Add((i + 1).ToString("00") + " - " + months[i]);

            if (cmbMonth.Items.Count > 0)
                cmbMonth.SelectedIndex = DateTime.Today.Month - 1;
        }

        private void SetupPaymentMethods()
        {
            cmbPaymentMethod.Items.Clear();
            cmbPaymentMethod.Items.Add("نقداً");
            cmbPaymentMethod.Items.Add("حوالة");
            cmbPaymentMethod.Items.Add("بنك");
            cmbPaymentMethod.Items.Add("محفظة إلكترونية");

            if (cmbPaymentMethod.Items.Count > 0)
                cmbPaymentMethod.SelectedIndex = 0;
        }

        private async Task LoadTeachersAsync()
        {
            suppressTeacherChange = true;
            try
            {
                DataTable teachers = await Task.Run(() => teacherService.GetActiveTeachers());
                cmbTeacher.DataSource = teachers;

                if (teachers.Columns.Contains("TeacherName"))
                    cmbTeacher.DisplayMember = "TeacherName";
                else if (teachers.Columns.Contains("FullName"))
                    cmbTeacher.DisplayMember = "FullName";
                else if (teachers.Columns.Count > 1)
                    cmbTeacher.DisplayMember = teachers.Columns[1].ColumnName;

                cmbTeacher.ValueMember = "TeacherID";

                if (cmbTeacher.Items.Count > 0)
                    cmbTeacher.SelectedIndex = 0;
            }
            finally
            {
                suppressTeacherChange = false;
            }

            await LoadActiveContractAsync(false);
        }

        private async void cmbTeacher_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (suppressTeacherChange || isBusy || IsDisposed || Disposing)
                return;

            await LoadActiveContractAsync(true);
        }

        private async Task LoadActiveContractAsync(bool showWarning)
        {
            int stateVersion = ++entryStateVersion;
            activeContractId = 0;

            if (cmbTeacher.SelectedValue == null ||
                cmbTeacher.SelectedValue is DataRowView ||
                !int.TryParse(cmbTeacher.SelectedValue.ToString(), out int teacherId) ||
                teacherId <= 0)
            {
                ClearSalaryFields();
                SetAddState(false);
                return;
            }

            try
            {
                TeacherContract contract = await Task.Run(
                    () => contractService.GetActiveContractForTeacher(teacherId));

                if (stateVersion != entryStateVersion || isBusy || IsDisposed || Disposing)
                    return;

                if (contract == null)
                {
                    ClearSalaryFields();
                    SetAddState(false, "لا يوجد عقد ساري");

                    if (showWarning)
                        UIHelper.ShowWarning("لا يوجد عقد وظيفي سارٍ لهذا الموظف في تاريخ اليوم؛ لا يمكن تسجيل راتب شهري.");
                    return;
                }

                activeContractId = contract.ContractID;
                txtBasicSalary.Text = contract.BasicSalary.ToString("0.##");
                txtAllowances.Text =
                    (contract.HousingAllowance +
                     contract.TransportAllowance +
                     contract.OtherAllowances).ToString("0.##");
                txtDeductions.Text = "0";

                int paymentMethodIndex = cmbPaymentMethod.FindStringExact(
                    (contract.PaymentMethod ?? string.Empty).Trim());
                if (paymentMethodIndex >= 0)
                    cmbPaymentMethod.SelectedIndex = paymentMethodIndex;

                SalaryField_TextChanged(this, EventArgs.Empty);
                await RefreshEntryStateAsync();
            }
            catch (Exception ex)
            {
                ClearSalaryFields();
                SetAddState(false);
                if (showWarning)
                    UIHelper.ShowException("تحميل العقد الساري", ex);
                else
                    ApplicationLogger.LogException("واجهة الرواتب الشهرية: تحميل العقد الساري", ex);
            }
        }

        private void ClearSalaryFields()
        {
            txtBasicSalary.Text = "0";
            txtAllowances.Text = "0";
            txtDeductions.Text = "0";
            txtNetSalary.Text = "0";
        }

        private void SetAddState(bool canAdd, string reason = null)
        {
            btnAdd.Enabled = canAdd && !isBusy;
            btnAdd.Text = canAdd
                ? "تسجيل الراتب وإنشاء سند الصرف"
                : (string.IsNullOrWhiteSpace(reason) ? "غير متاح" : reason);
        }

        private async void cmbMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            await RefreshEntryStateAsync();
        }

        private async void txtSalaryYear_ValueChanged(object sender, EventArgs e)
        {
            await RefreshEntryStateAsync();
        }

        private bool IsFutureSalaryPeriod(int month, int year)
        {
            return year > DateTime.Today.Year ||
                (year == DateTime.Today.Year && month > DateTime.Today.Month);
        }

        private async Task RefreshEntryStateAsync()
        {
            int stateVersion = ++entryStateVersion;

            if (suppressTeacherChange || isBusy || IsDisposed || Disposing)
                return;

            if (!int.TryParse(cmbTeacher.SelectedValue == null ? "" : cmbTeacher.SelectedValue.ToString(), out int teacherId) ||
                teacherId <= 0)
            {
                SetAddState(false, "اختر معلماً");
                return;
            }

            int month = GetSelectedMonth();
            int year = Convert.ToInt32(txtSalaryYear.Value);

            if (month < 1 || month > 12 || year < 2000 || year > 2100)
            {
                SetAddState(false, "فترة غير صحيحة");
                return;
            }

            if (IsFutureSalaryPeriod(month, year))
            {
                SetAddState(false, "الفترة المستقبلية غير مسموحة");
                return;
            }

            if (activeContractId <= 0)
            {
                SetAddState(false, "لا يوجد عقد ساري");
                return;
            }

            try
            {
                bool exists = await Task.Run(
                    () => payrollService.PayrollExists(teacherId, month, year));

                if (stateVersion != entryStateVersion || isBusy || IsDisposed || Disposing)
                    return;

                if (exists)
                    SetAddState(false, "الراتب مسجل لهذه الفترة");
                else
                    SetAddState(true);
            }
            catch (Exception ex)
            {
                SetAddState(false, "تعذر التحقق من الفترة");
                ApplicationLogger.LogException("واجهة الرواتب الشهرية: التحقق من سجل الفترة", ex);
            }
        }

        private async Task LoadPayrollsAsync()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                allPayrolls = await Task.Run(() => payrollService.GetAllPayrolls());
                ApplyFilter(txtSearch.Text.Trim());
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("تحميل سجلات الرواتب", ex);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void ApplyFilter(string searchText)
        {
            try
            {
                if (allPayrolls == null)
                {
                    dataGridViewPayrolls.DataSource = null;
                    lblRecordCount.Text = "عدد سجلات الرواتب: 0";
                    return;
                }

                string term = (searchText ?? string.Empty).Trim();
                DataTable filtered = allPayrolls.Clone();

                foreach (DataRow row in allPayrolls.Rows)
                {
                    if (row == null)
                        continue;

                    if (string.IsNullOrWhiteSpace(term) || PayrollRowContains(row, term))
                        filtered.ImportRow(row);
                }

                dataGridViewPayrolls.DataSource = filtered.DefaultView;
                lblRecordCount.Text = "عدد سجلات الرواتب: " + filtered.Rows.Count;
                FormatGridColumns();
            }
            catch (Exception ex)
            {
                ApplicationLogger.LogException("شاشة مسير الرواتب: البحث", ex);
                dataGridViewPayrolls.DataSource = allPayrolls != null ? allPayrolls.DefaultView : null;
                lblRecordCount.Text = "عدد سجلات الرواتب: " + (allPayrolls != null ? allPayrolls.Rows.Count : 0);
            }
        }

        private static bool PayrollRowContains(DataRow row, string term)
        {
            string comparison = term.Trim();

            string[] searchableColumns =
            {
                "TeacherName",
                "PaymentMethod",
                "VoucherNumber",
                "Notes",
                "SalaryMonth",
                "SalaryYear",
                "BasicSalary",
                "Allowances",
                "Deductions",
                "NetSalary",
                "PaymentDate"
            };

            foreach (string column in searchableColumns)
            {
                if (!row.Table.Columns.Contains(column))
                    continue;

                object value = row[column];
                if (value == null || value == DBNull.Value)
                    continue;

                string text = Convert.ToString(value, System.Globalization.CultureInfo.CurrentCulture);
                if (!string.IsNullOrEmpty(text) &&
                    text.IndexOf(comparison, StringComparison.CurrentCultureIgnoreCase) >= 0)
                    return true;
            }

            return false;
        }

        private void FormatGridColumns()
        {
            if (dataGridViewPayrolls.Columns.Count == 0)
                return;

            SetHeader("PayrollID", "الرقم");
            SetHeader("TeacherName", "المعلم");
            SetHeader("SalaryMonth", "الشهر");
            SetHeader("SalaryYear", "السنة");
            SetHeader("BasicSalary", "الأساسي");
            SetHeader("Allowances", "البدلات");
            SetHeader("Deductions", "الخصومات");
            SetHeader("NetSalary", "الصافي");
            SetHeader("PaymentDate", "تاريخ الصرف");
            SetHeader("PaymentMethod", "طريقة الصرف");
            SetHeader("VoucherNumber", "رقم سند الصرف");
            SetHeader("Notes", "ملاحظات");
            SetHeader("CreatedAt", "تاريخ الإدخال");

            if (dataGridViewPayrolls.Columns.Contains("TeacherID"))
                dataGridViewPayrolls.Columns["TeacherID"].Visible = false;

            dataGridViewPayrolls.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            SetPayrollColumnWidth("PayrollID", 85);
            SetPayrollColumnWidth("TeacherName", 195);
            SetPayrollColumnWidth("SalaryMonth", 105);
            SetPayrollColumnWidth("SalaryYear", 105);
            SetPayrollColumnWidth("BasicSalary", 120);
            SetPayrollColumnWidth("Allowances", 115);
            SetPayrollColumnWidth("Deductions", 115);
            SetPayrollColumnWidth("NetSalary", 125);
            SetPayrollColumnWidth("PaymentDate", 135);
            SetPayrollColumnWidth("PaymentMethod", 125);
            SetPayrollColumnWidth("VoucherNumber", 155);
            SetPayrollColumnWidth("Notes", 230);
            SetPayrollColumnWidth("CreatedAt", 155);

            FormatPayrollNumber("BasicSalary");
            FormatPayrollNumber("Allowances");
            FormatPayrollNumber("Deductions");
            FormatPayrollNumber("NetSalary");
            FormatPayrollDate("PaymentDate", "dd/MM/yyyy");
            FormatPayrollDate("CreatedAt", "dd/MM/yyyy HH:mm");
        }

        private void ConfigurePayrollGridAppearance()
        {
            if (dataGridViewPayrolls == null)
                return;

            dataGridViewPayrolls.SuspendLayout();
            try
            {
                dataGridViewPayrolls.AllowUserToAddRows = false;
                dataGridViewPayrolls.AllowUserToDeleteRows = false;
                dataGridViewPayrolls.AllowUserToResizeRows = false;
                dataGridViewPayrolls.AllowUserToResizeColumns = true;
                dataGridViewPayrolls.AllowUserToOrderColumns = true;
                dataGridViewPayrolls.ReadOnly = true;
                dataGridViewPayrolls.MultiSelect = false;
                dataGridViewPayrolls.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridViewPayrolls.RowHeadersVisible = false;
                dataGridViewPayrolls.RightToLeft = RightToLeft.Yes;
                dataGridViewPayrolls.ScrollBars = ScrollBars.Both;
                dataGridViewPayrolls.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                dataGridViewPayrolls.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
                dataGridViewPayrolls.BackgroundColor = Color.White;
                dataGridViewPayrolls.BorderStyle = BorderStyle.FixedSingle;
                dataGridViewPayrolls.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                dataGridViewPayrolls.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                dataGridViewPayrolls.EnableHeadersVisualStyles = false;
                dataGridViewPayrolls.GridColor = UIHelper.BorderColor;
                dataGridViewPayrolls.ColumnHeadersHeight = 44;
                dataGridViewPayrolls.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                dataGridViewPayrolls.RowTemplate.Height = 38;
                dataGridViewPayrolls.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;

                dataGridViewPayrolls.ColumnHeadersDefaultCellStyle.BackColor = UIHelper.PrimaryColor;
                dataGridViewPayrolls.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dataGridViewPayrolls.ColumnHeadersDefaultCellStyle.Font = new Font(UIHelper.FontFamily, 10F, FontStyle.Bold);
                dataGridViewPayrolls.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewPayrolls.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dataGridViewPayrolls.ColumnHeadersDefaultCellStyle.SelectionBackColor = UIHelper.PrimaryColor;
                dataGridViewPayrolls.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
                dataGridViewPayrolls.DefaultCellStyle.BackColor = Color.White;
                dataGridViewPayrolls.DefaultCellStyle.ForeColor = UIHelper.TextColor;
                dataGridViewPayrolls.DefaultCellStyle.Font = new Font(UIHelper.FontFamily, 9.5F);
                dataGridViewPayrolls.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridViewPayrolls.DefaultCellStyle.SelectionBackColor = UIHelper.AccentColor;
                dataGridViewPayrolls.DefaultCellStyle.SelectionForeColor = Color.White;
                dataGridViewPayrolls.DefaultCellStyle.Padding = new Padding(8, 4, 8, 4);
                dataGridViewPayrolls.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
                dataGridViewPayrolls.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(241, 245, 249);
                dataGridViewPayrolls.AlternatingRowsDefaultCellStyle.ForeColor = UIHelper.TextColor;
                dataGridViewPayrolls.AlternatingRowsDefaultCellStyle.SelectionBackColor = UIHelper.AccentColor;
                dataGridViewPayrolls.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.White;
                dataGridViewPayrolls.RowsDefaultCellStyle.BackColor = Color.White;
                dataGridViewPayrolls.RowsDefaultCellStyle.ForeColor = UIHelper.TextColor;
                dataGridViewPayrolls.RowsDefaultCellStyle.SelectionBackColor = UIHelper.AccentColor;
                dataGridViewPayrolls.RowsDefaultCellStyle.SelectionForeColor = Color.White;
            }
            finally
            {
                dataGridViewPayrolls.ResumeLayout();
            }
        }

        private void SetPayrollColumnWidth(string name, int width)
        {
            if (!dataGridViewPayrolls.Columns.Contains(name))
                return;

            DataGridViewColumn column = dataGridViewPayrolls.Columns[name];
            if (!column.Visible)
                return;

            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            column.MinimumWidth = Math.Min(115, Math.Max(55, width));
            column.Width = width;
            column.Resizable = DataGridViewTriState.True;
            column.SortMode = DataGridViewColumnSortMode.Automatic;
        }

        private void FormatPayrollNumber(string name)
        {
            if (dataGridViewPayrolls.Columns.Contains(name))
                dataGridViewPayrolls.Columns[name].DefaultCellStyle.Format = "N2";
        }

        private void FormatPayrollDate(string name, string format)
        {
            if (dataGridViewPayrolls.Columns.Contains(name))
                dataGridViewPayrolls.Columns[name].DefaultCellStyle.Format = format;
        }

        private void SetHeader(string columnName, string header)
        {
            if (dataGridViewPayrolls.Columns.Contains(columnName))
                dataGridViewPayrolls.Columns[columnName].HeaderText = header;
        }

        private void SalaryField_TextChanged(object sender, EventArgs e)
        {
            decimal basic = ParseMoney(txtBasicSalary.Text);
            decimal allowances = ParseMoney(txtAllowances.Text);
            decimal deductions = ParseMoney(txtDeductions.Text);
            decimal net = basic + allowances - deductions;

            if (net < 0)
                net = 0;

            txtNetSalary.Text = net.ToString("N2");
        }

        private static decimal ParseMoney(string value)
        {
            return UIHelper.TryParseDecimal(value, out decimal result) ? result : 0m;
        }

        private void ClearInputs()
        {
            suppressTeacherChange = true;
            try
            {
                if (cmbTeacher.Items.Count > 0)
                    cmbTeacher.SelectedIndex = 0;

                if (cmbMonth.Items.Count > 0)
                    cmbMonth.SelectedIndex = DateTime.Today.Month - 1;

                txtSalaryYear.Value = DateTime.Today.Year;
                ClearSalaryFields();
                dtpPaymentDate.Value = DateTime.Today;

                if (cmbPaymentMethod.Items.Count > 0)
                    cmbPaymentMethod.SelectedIndex = 0;

                txtNotes.Clear();
            }
            finally
            {
                suppressTeacherChange = false;
            }

            _ = LoadActiveContractAsync(false);
        }

        private int GetSelectedMonth()
        {
            return cmbMonth.SelectedIndex + 1;
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            if (cmbTeacher.SelectedValue == null)
            {
                UIHelper.ShowWarning("اختر المعلم.");
                return;
            }

            int teacherId = Convert.ToInt32(cmbTeacher.SelectedValue);
            int month = GetSelectedMonth();
            int year = Convert.ToInt32(txtSalaryYear.Value);

            if (IsFutureSalaryPeriod(month, year))
            {
                UIHelper.ShowWarning("لا يمكن تسجيل راتب لفترة شهرية مستقبلية.");
                return;
            }

            decimal basic = ParseMoney(txtBasicSalary.Text);
            decimal allowances = ParseMoney(txtAllowances.Text);
            decimal deductions = ParseMoney(txtDeductions.Text);

            if (basic <= 0)
            {
                UIHelper.ShowWarning("الراتب الأساسي يجب أن يكون أكبر من صفر.");
                txtBasicSalary.Focus();
                return;
            }

            if (allowances < 0 || deductions < 0 || deductions > basic + allowances)
            {
                UIHelper.ShowWarning("تحقق من قيم البدلات والخصومات.");
                return;
            }

            Payroll payroll = new Payroll
            {
                TeacherID = teacherId,
                TeacherName = cmbTeacher.Text,
                SalaryMonth = month,
                SalaryYear = year,
                BasicSalary = basic,
                Allowances = allowances,
                Deductions = deductions,
                PaymentDate = dtpPaymentDate.Value.Date,
                PaymentMethod = cmbPaymentMethod.Text,
                Notes = txtNotes.Text.Trim()
            };

            try
            {
                SetBusy(true);
                int payrollId = await Task.Run(() => payrollService.AddPayroll(payroll));

                UIHelper.ShowInfo(
                    "تم تسجيل الراتب رقم " + payrollId +
                    " وإنشاء سند الصرف المرتبط به بنجاح.");

                await LoadPayrollsAsync();
                DataRefreshNotifier.Notify("Payroll", this);
                ClearInputs();
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("تسجيل الراتب", ex);
            }
            finally
            {
                SetBusy(false);
            }
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await LoadPayrollsAsync();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearInputs();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            ApplyFilter(txtSearch.Text);
            txtSearch.Focus();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter(txtSearch.Text.Trim());
        }

        private void NumericTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (char.IsControl(e.KeyChar) || char.IsDigit(e.KeyChar))
                return;

            if ((e.KeyChar == '.' || e.KeyChar == ',' || e.KeyChar == '٫' || e.KeyChar == '٬') &&
                textBox != null &&
                textBox.Text.IndexOfAny(new[] { '.', ',', '٫', '٬' }) < 0)
                return;

            e.Handled = true;
        }

        private void SetBusy(bool value)
        {
            ++entryStateVersion;
            isBusy = value;
            btnRefresh.Enabled = !value;
            btnClear.Enabled = !value;
            btnAdd.Enabled = !value && activeContractId > 0;
            if (activeContractId > 0)
                btnAdd.Text = "تسجيل وصرف الراتب";
            else
                btnAdd.Text = "لا يوجد عقد ساري";
            UseWaitCursor = value;
        }
    }
}
