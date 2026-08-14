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
    public partial class PayrollForm : UserControl
    {
        private readonly ContractService contractService = new ContractService();
        private readonly TeacherService teacherService = new TeacherService();

        private int selectedContractId = 0;
        private DataTable allContracts;

        public PayrollForm()
        {
            InitializeComponent();
            SchoolSystem.Helpers.UIHelper.ApplyStyle(this);
            ApplyCustomStyles();
            this.Dock = DockStyle.Fill;
            this.Load += PayrollForm_Load;
        }

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

            txtTotal.BackColor = UIHelper.SurfaceElevatedColor;
            txtNetSalary.BackColor = UIHelper.SurfaceElevatedColor;
        }

        private async void PayrollForm_Load(object sender, EventArgs e)
        {
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
                UIHelper.ShowException("تحميل بيانات الرواتب", ex);
            }
        }

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

        private async Task LoadTeachersAsync()
        {
            DataTable teachers = await Task.Run(() => teacherService.GetAllTeachers());

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

        private async Task LoadContractsAsync()
        {
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
                UIHelper.ShowException("تحميل عقود الرواتب", ex);
            }
        }

        private void ApplyFilter(string searchText)
        {
            if (allContracts == null)
                return;

            DataView dv = allContracts.DefaultView;

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                string safeText = UIHelper.EscapeDataViewFilterValue(searchText);

                dv.RowFilter =
                    "TeacherName LIKE '%" + safeText + "%' OR " +
                    "ContractNumber LIKE '%" + safeText + "%' OR " +
                    "ContractType LIKE '%" + safeText + "%' OR " +
                    "ContractStatus LIKE '%" + safeText + "%' OR " +
                    "PaymentMethod LIKE '%" + safeText + "%'";
            }
            else
            {
                dv.RowFilter = "";
            }

            dataGridViewContracts.DataSource = dv;
            lblRecordCount.Text = "عدد العقود: " + dv.Count;

            FormatGridColumns();
        }

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
                    new Font("Tahoma", 9.5F, FontStyle.Bold);
                dataGridViewContracts.Columns["TotalSalary"].DefaultCellStyle.ForeColor =
                    Color.FromArgb(22, 163, 74);
            }

            if (dataGridViewContracts.Columns.Contains("NetSalary"))
            {
                dataGridViewContracts.Columns["NetSalary"].HeaderText = "الصافي";
                dataGridViewContracts.Columns["NetSalary"].DefaultCellStyle.Font =
                    new Font("Tahoma", 9.5F, FontStyle.Bold);
                dataGridViewContracts.Columns["NetSalary"].DefaultCellStyle.ForeColor =
                    Color.FromArgb(37, 99, 235);
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

            dataGridViewContracts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter(txtSearch.Text.Trim());
        }

        private void SalaryField_TextChanged(object sender, EventArgs e)
        {
            CalculateTotal();
        }

        private decimal ParseMoney(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return 0;

            value = value.Replace(",", "");

            decimal result;
            if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
                return result;

            if (decimal.TryParse(value, out result))
                return result;

            return 0;
        }

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

        private string GenerateContractNumber()
        {
            return "CON-" + DateTime.Now.ToString("yyyyMMddHHmmss");
        }

        private TeacherContract GetContractFromInputs()
        {
            if (cmbTeacher.SelectedValue == null || Convert.ToInt32(cmbTeacher.SelectedValue) <= 0)
                throw new InvalidOperationException("يرجى اختيار المعلم.");
            if (string.IsNullOrWhiteSpace(txtContractNumber.Text))
                throw new InvalidOperationException("رقم العقد مطلوب.");
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
            if (!UIHelper.TryParseDecimal(txtBasicSalary.Text, out basicSalary) || basicSalary < 0)
                throw new InvalidOperationException("الراتب الأساسي يجب أن يكون رقمًا غير سالب.");
            if (!UIHelper.TryParseDecimal(txtHousing.Text, out housingAllowance) || housingAllowance < 0)
                throw new InvalidOperationException("بدل السكن يجب أن يكون رقمًا غير سالب.");
            if (!UIHelper.TryParseDecimal(txtTransport.Text, out transportAllowance) || transportAllowance < 0)
                throw new InvalidOperationException("بدل النقل يجب أن يكون رقمًا غير سالب.");
            if (!UIHelper.TryParseDecimal(txtOther.Text, out otherAllowances) || otherAllowances < 0)
                throw new InvalidOperationException("البدلات الأخرى يجب أن تكون رقمًا غير سالب.");
            if (!UIHelper.TryParseDecimal(txtDeductions.Text, out deductions) || deductions < 0)
                throw new InvalidOperationException("الخصومات يجب أن تكون رقمًا غير سالب.");

            decimal totalSalary = basicSalary + housingAllowance + transportAllowance + otherAllowances;
            if (deductions > totalSalary)
                throw new InvalidOperationException("لا يمكن أن تتجاوز الخصومات إجمالي الراتب.");
            if (dtpEndDate.Checked && dtpEndDate.Value.Date < dtpStartDate.Value.Date)
                throw new InvalidOperationException("تاريخ نهاية العقد يجب أن يكون بعد تاريخ بدايته.");

            TeacherContract contract = new TeacherContract();

            contract.ContractID = selectedContractId;
            contract.TeacherID = Convert.ToInt32(cmbTeacher.SelectedValue);
            contract.ContractNumber = txtContractNumber.Text.Trim();
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

        private void dataGridViewContracts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dataGridViewContracts.Rows.Count == 0)
                return;

            DataRowView rowView =
                dataGridViewContracts.Rows[e.RowIndex].DataBoundItem as DataRowView;

            if (rowView != null)
                FillFieldsFromRow(rowView.Row);
        }

        private void FillFieldsFromRow(DataRow row)
        {
            if (row == null)
                return;

            selectedContractId = Convert.ToInt32(row["ContractID"]);

            if (row["TeacherID"] != DBNull.Value)
                cmbTeacher.SelectedValue = Convert.ToInt32(row["TeacherID"]);

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

            if (row["StartDate"] != DBNull.Value)
                dtpStartDate.Value = Convert.ToDateTime(row["StartDate"]);

            if (row["EndDate"] != DBNull.Value)
            {
                dtpEndDate.Checked = true;
                dtpEndDate.Value = Convert.ToDateTime(row["EndDate"]);
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

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            if (cmbTeacher.SelectedValue == null)
            {
                MessageBox.Show("اختر معلماً.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                TeacherContract contract = GetContractFromInputs();

                Cursor = Cursors.WaitCursor;
                await Task.Run(() => contractService.AddContract(contract));
                Cursor = Cursors.Default;

                MessageBox.Show("تمت إضافة العقد بنجاح.",
                    "تم", MessageBoxButtons.OK, MessageBoxIcon.Information);

                await LoadContractsAsync();
                ClearInputs();
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                UIHelper.ShowException("إضافة سجل الرواتب", ex);
            }
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedContractId == 0)
            {
                MessageBox.Show("اختر عقداً من الجدول.",
                    "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                TeacherContract contract = GetContractFromInputs();

                Cursor = Cursors.WaitCursor;
                bool updated = await Task.Run(() => contractService.UpdateContract(contract));
                Cursor = Cursors.Default;

                MessageBox.Show(updated ? "تم تعديل العقد بنجاح." : "لم يتم العثور على العقد.",
                    "نتيجة العملية", MessageBoxButtons.OK,
                    updated ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

                await LoadContractsAsync();
                ClearInputs();
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                UIHelper.ShowException("تعديل سجل الرواتب", ex);
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedContractId == 0)
            {
                MessageBox.Show("اختر عقداً من الجدول.",
                    "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show(
                "هل تريد حذف هذا العقد؟",
                "تأكيد الحذف",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            try
            {
                Cursor = Cursors.WaitCursor;
                bool deleted = await Task.Run(() => contractService.DeleteContract(selectedContractId));
                Cursor = Cursors.Default;

                MessageBox.Show(deleted ? "تم حذف العقد بنجاح." : "لم يتم العثور على العقد.",
                    "نتيجة العملية", MessageBoxButtons.OK,
                    deleted ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

                await LoadContractsAsync();
                ClearInputs();
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                UIHelper.ShowException("حذف سجل الرواتب", ex);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearInputs();
        }

        private void NumericTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox txt = sender as TextBox;

            if (char.IsControl(e.KeyChar))
                return;

            if (char.IsDigit(e.KeyChar))
                return;

            if (e.KeyChar == '.' && txt != null && !txt.Text.Contains("."))
                return;

            e.Handled = true;
        }

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
