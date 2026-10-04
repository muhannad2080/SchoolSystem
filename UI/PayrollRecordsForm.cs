using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using SchoolSystem.Helpers;
using SchoolSystem.Models;
using SchoolSystem.Services;

namespace SchoolSystem.UI
{
    public partial class PayrollRecordsForm : UserControl
    {
        private readonly PayrollService payrollService = new PayrollService();
        private readonly TeacherService teacherService = new TeacherService();

        private DataTable allPayrolls;

        public PayrollRecordsForm()
        {
            InitializeComponent();
            UIHelper.ApplyStyle(this);
            this.Dock = DockStyle.Fill;
            ConfigureControls();
            this.Load += PayrollRecordsForm_Load;
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
            if (allPayrolls == null)
                return;

            DataView view = allPayrolls.DefaultView;

            if (string.IsNullOrWhiteSpace(searchText))
            {
                view.RowFilter = string.Empty;
            }
            else
            {
                view.RowFilter = UIHelper.BuildDataViewSearchFilter(
                    searchText,
                    "TeacherName",
                    "SalaryMonth",
                    "SalaryYear",
                    "PaymentMethod",
                    "Notes");
            }

            dataGridViewPayrolls.DataSource = view;
            lblRecordCount.Text = "عدد سجلات الرواتب: " + view.Count;
            FormatGridColumns();
        }

        private void FormatGridColumns()
        {
            if (dataGridViewPayrolls.Columns.Count == 0)
                return;

            SetHeader("PayrollID", "الرقم", 60);
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
        }

        private void SetHeader(string columnName, string header, int width = 0)
        {
            if (!dataGridViewPayrolls.Columns.Contains(columnName))
                return;

            dataGridViewPayrolls.Columns[columnName].HeaderText = header;
            if (width > 0)
                dataGridViewPayrolls.Columns[columnName].Width = width;
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
            if (cmbTeacher.Items.Count > 0)
                cmbTeacher.SelectedIndex = 0;

            if (cmbMonth.Items.Count > 0)
                cmbMonth.SelectedIndex = DateTime.Today.Month - 1;

            txtSalaryYear.Value = DateTime.Today.Year;
            txtBasicSalary.Text = "0";
            txtAllowances.Text = "0";
            txtDeductions.Text = "0";
            txtNetSalary.Text = "0";
            dtpPaymentDate.Value = DateTime.Today;

            if (cmbPaymentMethod.Items.Count > 0)
                cmbPaymentMethod.SelectedIndex = 0;

            txtNotes.Clear();
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

        private void SetBusy(bool busy)
        {
            btnAdd.Enabled = !busy;
            btnRefresh.Enabled = !busy;
            btnClear.Enabled = !busy;
            UseWaitCursor = busy;
        }
    }
}
