using System;
using System.Data;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;
using SchoolSystem.Models;
using SchoolSystem.Services;

namespace SchoolSystem.UI
{
    public partial class ExpensesForm : UserControl
    {
        private readonly ExpenseService expenseService = new ExpenseService();

        private int selectedExpenseId = 0;
        private DataTable allExpenses;
        private bool isLoading = false;

        public ExpensesForm()
        {
            InitializeComponent();
            Dock = DockStyle.Fill;
            Load += ExpensesForm_Load;
        }

        private async void ExpensesForm_Load(object sender, EventArgs e)
        {
            try
            {
                isLoading = true;

                InitializeLookups();

                dtpExpenseDate.Value = DateTime.Today;

                await LoadExpensesAsync();

                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء تحميل المصروفات:\n" + ex.Message);
            }
            finally
            {
                isLoading = false;
            }
        }

        private void InitializeLookups()
        {
            cmbCategory.Items.Clear();
            cmbCategory.Items.AddRange(new object[]
            {
                "مصروفات إدارية",
                "قرطاسية ومكتبية",
                "صيانة",
                "نظافة",
                "كهرباء ومياه",
                "اتصالات وإنترنت",
                "نقل ومواصلات",
                "أنشطة مدرسية",
                "مشتريات",
                "رواتب وأجور",
                "إيجارات",
                "أخرى"
            });
            cmbCategory.SelectedIndex = 0;

            cmbPaymentMethod.Items.Clear();
            cmbPaymentMethod.Items.AddRange(new object[]
            {
                "نقداً",
                "حوالة",
                "شيك",
                "محفظة إلكترونية",
                "أخرى"
            });
            cmbPaymentMethod.SelectedIndex = 0;

            cmbFilterCategory.Items.Clear();
            cmbFilterCategory.Items.Add("كل الفئات");
            foreach (object item in cmbCategory.Items)
                cmbFilterCategory.Items.Add(item);

            cmbFilterCategory.SelectedIndex = 0;
        }

        private async Task LoadExpensesAsync()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                allExpenses = await Task.Run(() => expenseService.GetAllExpenses());

                ApplyFilter();
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void ApplyFilter()
        {
            if (allExpenses == null)
                return;

            DataView dv = allExpenses.DefaultView;

            string searchText = EscapeFilter(txtSearch.Text.Trim());
            string selectedCategory = cmbFilterCategory.SelectedItem == null
                ? "كل الفئات"
                : cmbFilterCategory.SelectedItem.ToString();

            string filter = "";

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                filter =
                    "(ExpenseNumber LIKE '%" + searchText + "%' " +
                    "OR Category LIKE '%" + searchText + "%' " +
                    "OR PayeeName LIKE '%" + searchText + "%' " +
                    "OR PaymentMethod LIKE '%" + searchText + "%' " +
                    "OR Description LIKE '%" + searchText + "%' " +
                    "OR Notes LIKE '%" + searchText + "%')";
            }

            if (selectedCategory != "كل الفئات")
            {
                if (!string.IsNullOrWhiteSpace(filter))
                    filter += " AND ";

                filter += "Category = '" + EscapeFilter(selectedCategory) + "'";
            }

            dv.RowFilter = filter;

            dataGridViewExpenses.DataSource = dv;

            lblRecordCount.Text = "عدد المصروفات: " + dv.Count;

            FormatGrid();
        }

        private string EscapeFilter(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "";

            return value
                .Replace("'", "''")
                .Replace("[", "[[]")
                .Replace("%", "[%]")
                .Replace("*", "[*]");
        }

        private void FormatGrid()
        {
            if (dataGridViewExpenses.Columns.Count == 0)
                return;

            HideColumn("ExpenseID");

            SetHeader("ExpenseNumber", "رقم المصروف");
            SetHeader("Amount", "المبلغ");
            SetHeader("ExpenseDate", "التاريخ");
            SetHeader("Category", "الفئة");
            SetHeader("PayeeName", "المستفيد");
            SetHeader("PaymentMethod", "طريقة الدفع");
            SetHeader("Description", "البيان");
            SetHeader("Notes", "ملاحظات");
            SetHeader("CreatedAt", "تاريخ الإدخال");
            SetHeader("UpdatedAt", "آخر تعديل");

            FormatMoney("Amount");
            FormatDate("ExpenseDate");
            FormatDate("CreatedAt");
            FormatDate("UpdatedAt");

            dataGridViewExpenses.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void HideColumn(string columnName)
        {
            if (dataGridViewExpenses.Columns.Contains(columnName))
                dataGridViewExpenses.Columns[columnName].Visible = false;
        }

        private void SetHeader(string columnName, string headerText)
        {
            if (dataGridViewExpenses.Columns.Contains(columnName))
                dataGridViewExpenses.Columns[columnName].HeaderText = headerText;
        }

        private void FormatMoney(string columnName)
        {
            if (dataGridViewExpenses.Columns.Contains(columnName))
                dataGridViewExpenses.Columns[columnName].DefaultCellStyle.Format = "N2";
        }

        private void FormatDate(string columnName)
        {
            if (dataGridViewExpenses.Columns.Contains(columnName))
                dataGridViewExpenses.Columns[columnName].DefaultCellStyle.Format = "dd/MM/yyyy";
        }

        private void FilterControls_Changed(object sender, EventArgs e)
        {
            if (!isLoading)
                ApplyFilter();
        }

        private decimal ReadDecimal(string text)
        {
            if (decimal.TryParse(text, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal value))
                return value;

            if (decimal.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out value))
                return value;

            return 0;
        }

        private bool ValidateInputs()
        {
            decimal amount = ReadDecimal(txtAmount.Text);

            if (amount <= 0)
            {
                MessageBox.Show("أدخل مبلغاً صحيحاً أكبر من صفر.");
                txtAmount.Focus();
                return false;
            }

            if (cmbCategory.SelectedItem == null || string.IsNullOrWhiteSpace(cmbCategory.Text))
            {
                MessageBox.Show("اختر فئة المصروف.");
                cmbCategory.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                MessageBox.Show("أدخل بيان المصروف.");
                txtDescription.Focus();
                return false;
            }

            return true;
        }

        private Expense GetExpenseFromInputs()
        {
            return new Expense
            {
                ExpenseID = selectedExpenseId,
                ExpenseNumber = txtExpenseNumber.Text.Trim(),
                Amount = ReadDecimal(txtAmount.Text),
                ExpenseDate = dtpExpenseDate.Value.Date,
                Category = cmbCategory.Text.Trim(),
                PayeeName = txtPayeeName.Text.Trim(),
                PaymentMethod = cmbPaymentMethod.Text.Trim(),
                Description = txtDescription.Text.Trim(),
                Notes = txtNotes.Text.Trim()
            };
        }

        private void ClearInputs()
        {
            selectedExpenseId = 0;

            txtExpenseNumber.Clear();
            txtAmount.Text = "0";
            dtpExpenseDate.Value = DateTime.Today;

            if (cmbCategory.Items.Count > 0)
                cmbCategory.SelectedIndex = 0;

            txtPayeeName.Clear();

            if (cmbPaymentMethod.Items.Count > 0)
                cmbPaymentMethod.SelectedIndex = 0;

            txtDescription.Clear();
            txtNotes.Clear();
        }

        private void dataGridViewExpenses_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dataGridViewExpenses.Rows.Count == 0)
                return;

            DataRowView rowView = dataGridViewExpenses.Rows[e.RowIndex].DataBoundItem as DataRowView;

            if (rowView != null)
                FillFieldsFromRow(rowView.Row);
        }

        private void FillFieldsFromRow(DataRow row)
        {
            selectedExpenseId = Convert.ToInt32(row["ExpenseID"]);

            txtExpenseNumber.Text = row["ExpenseNumber"] == DBNull.Value ? "" : row["ExpenseNumber"].ToString();
            txtAmount.Text = row["Amount"] == DBNull.Value ? "0" : Convert.ToDecimal(row["Amount"]).ToString("N2");

            dtpExpenseDate.Value = Convert.ToDateTime(row["ExpenseDate"]);

            cmbCategory.Text = row["Category"] == DBNull.Value ? "" : row["Category"].ToString();
            txtPayeeName.Text = row["PayeeName"] == DBNull.Value ? "" : row["PayeeName"].ToString();
            cmbPaymentMethod.Text = row["PaymentMethod"] == DBNull.Value ? "نقداً" : row["PaymentMethod"].ToString();

            txtDescription.Text = row["Description"] == DBNull.Value ? "" : row["Description"].ToString();
            txtNotes.Text = row["Notes"] == DBNull.Value ? "" : row["Notes"].ToString();
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateInputs())
                    return;

                Expense expense = GetExpenseFromInputs();

                await Task.Run(() => expenseService.AddExpense(expense));

                MessageBox.Show("تمت إضافة المصروف وإنشاء سند صرف تلقائي بنجاح.");

                await LoadExpensesAsync();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("فشل إضافة المصروف:\n" + ex.Message);
            }
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedExpenseId == 0)
                {
                    MessageBox.Show("اختر المصروف من الجدول أولاً.");
                    return;
                }

                if (!ValidateInputs())
                    return;

                Expense expense = GetExpenseFromInputs();

                bool result = await Task.Run(() => expenseService.UpdateExpense(expense));

                MessageBox.Show(result ? "تم تعديل المصروف بنجاح." : "لم يتم تعديل المصروف.");

                await LoadExpensesAsync();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("فشل تعديل المصروف:\n" + ex.Message);
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedExpenseId == 0)
                {
                    MessageBox.Show("اختر المصروف من الجدول أولاً.");
                    return;
                }

                DialogResult confirm = MessageBox.Show(
                    "هل تريد حذف المصروف المحدد؟\nملاحظة: السندات المرتبطة لن تُحذف للحفاظ على الأثر المالي.",
                    "تأكيد الحذف",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirm != DialogResult.Yes)
                    return;

                bool result = await Task.Run(() => expenseService.DeleteExpense(selectedExpenseId));

                MessageBox.Show(result ? "تم حذف المصروف." : "لم يتم حذف المصروف.");

                await LoadExpensesAsync();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("فشل حذف المصروف:\n" + ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearInputs();
        }
    }
}
