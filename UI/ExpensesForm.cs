using System;
using System.Data;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;
using SchoolSystem.Models;
using SchoolSystem.Helpers;
using SchoolSystem.Services;
using SchoolSystem.Security;


namespace SchoolSystem.UI
{

    public partial class ExpensesForm : UserControl
    {
        // تعريف الدالة ExpenseService لتنفيذ العملية المرتبطة بها.
        private readonly ExpenseService expenseService = new ExpenseService();


        private int selectedExpenseId = 0;

        private DataTable allExpenses;

        private bool isLoading = false;

        // تعريف الدالة ExpensesForm لتنفيذ العملية المرتبطة بها.
        public ExpensesForm()
        {

            InitializeComponent();

            SchoolSystem.Helpers.UIHelper.ApplyStyle(this);
            ConfigureExpensesGridAppearance();
            SchoolSystem.Helpers.UIHelper.NormalizeActionBar(panelButtons);

            txtSearch.TextChanged += (sender, e) => ApplyFilter();
            Dock = DockStyle.Fill;

            DataRefreshNotifier.DataChanged += DataRefreshNotifier_DataChanged;
            Disposed += (sender, e) =>
                DataRefreshNotifier.DataChanged -= DataRefreshNotifier_DataChanged;

            Load += ExpensesForm_Load;
        }

        // جدول المصروفات موحد مع بقية الجداول: حدود كاملة، صفوف متناوبة ونسخ مع رؤوس الأعمدة.
        private void ConfigureExpensesGridAppearance()
        {
            if (dataGridViewExpenses == null)
                return;

            dataGridViewExpenses.SuspendLayout();
            try
            {
                dataGridViewExpenses.AllowUserToAddRows = false;
                dataGridViewExpenses.AllowUserToDeleteRows = false;
                dataGridViewExpenses.AllowUserToResizeRows = false;
                dataGridViewExpenses.AllowUserToResizeColumns = true;
                dataGridViewExpenses.AllowUserToOrderColumns = true;
                dataGridViewExpenses.ReadOnly = true;
                dataGridViewExpenses.MultiSelect = false;
                dataGridViewExpenses.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridViewExpenses.RowHeadersVisible = false;
                dataGridViewExpenses.RightToLeft = RightToLeft.Yes;
                dataGridViewExpenses.ScrollBars = ScrollBars.Both;
                dataGridViewExpenses.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                dataGridViewExpenses.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
                dataGridViewExpenses.BackgroundColor = System.Drawing.Color.White;
                dataGridViewExpenses.BorderStyle = BorderStyle.FixedSingle;
                dataGridViewExpenses.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                dataGridViewExpenses.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                dataGridViewExpenses.EnableHeadersVisualStyles = false;
                dataGridViewExpenses.GridColor = UIHelper.BorderColor;
                dataGridViewExpenses.ColumnHeadersHeight = 44;
                dataGridViewExpenses.ColumnHeadersHeightSizeMode =
                    DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                dataGridViewExpenses.RowTemplate.Height = 38;
                dataGridViewExpenses.ClipboardCopyMode =
                    DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;

                dataGridViewExpenses.ColumnHeadersDefaultCellStyle.BackColor = UIHelper.PrimaryColor;
                dataGridViewExpenses.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
                dataGridViewExpenses.ColumnHeadersDefaultCellStyle.Font =
                    new System.Drawing.Font(UIHelper.FontFamily, 10F, System.Drawing.FontStyle.Bold);
                dataGridViewExpenses.ColumnHeadersDefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleCenter;
                dataGridViewExpenses.ColumnHeadersDefaultCellStyle.WrapMode =
                    DataGridViewTriState.True;
                dataGridViewExpenses.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                    UIHelper.PrimaryColor;
                dataGridViewExpenses.ColumnHeadersDefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
                dataGridViewExpenses.ColumnHeadersDefaultCellStyle.Padding =
                    new Padding(8, 4, 8, 4);

                dataGridViewExpenses.DefaultCellStyle.BackColor = System.Drawing.Color.White;
                dataGridViewExpenses.DefaultCellStyle.ForeColor = UIHelper.TextColor;
                dataGridViewExpenses.DefaultCellStyle.Font =
                    new System.Drawing.Font(UIHelper.FontFamily, 9.5F, System.Drawing.FontStyle.Regular);
                dataGridViewExpenses.DefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleRight;
                dataGridViewExpenses.DefaultCellStyle.SelectionBackColor = UIHelper.AccentColor;
                dataGridViewExpenses.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
                dataGridViewExpenses.DefaultCellStyle.Padding = new Padding(8, 4, 8, 4);
                dataGridViewExpenses.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

                dataGridViewExpenses.AlternatingRowsDefaultCellStyle.BackColor =
                    System.Drawing.Color.FromArgb(241, 245, 249);
                dataGridViewExpenses.AlternatingRowsDefaultCellStyle.ForeColor = UIHelper.TextColor;
                dataGridViewExpenses.AlternatingRowsDefaultCellStyle.SelectionBackColor = UIHelper.AccentColor;
                dataGridViewExpenses.AlternatingRowsDefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
                dataGridViewExpenses.RowsDefaultCellStyle.BackColor = System.Drawing.Color.White;
                dataGridViewExpenses.RowsDefaultCellStyle.ForeColor = UIHelper.TextColor;
                dataGridViewExpenses.RowsDefaultCellStyle.SelectionBackColor = UIHelper.AccentColor;
                dataGridViewExpenses.RowsDefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            }
            finally
            {
                dataGridViewExpenses.ResumeLayout();
            }
        }

        private void DataRefreshNotifier_DataChanged(object sender, DataRefreshEventArgs e)
        {
            if (IsDisposed || Disposing || !IsHandleCreated)
                return;

            // خدمات المصروفات والمال قد تبث الحدث من خيط خلفي.
            if (InvokeRequired)
            {
                try
                {
                    BeginInvoke(new MethodInvoker(
                        () => DataRefreshNotifier_DataChanged(sender, e)));
                }
                catch (InvalidOperationException)
                {
                    // أُغلقت الشاشة قبل معالجة الإشعار.
                }

                return;
            }

            if (ReferenceEquals(sender, this) || isLoading || selectedExpenseId > 0)
                return;

            if (!DataRefreshNotifier.IsRelated(e, "Expenses") &&
                !DataRefreshNotifier.IsRelated(e, "Vouchers") &&
                !DataRefreshNotifier.IsRelated(e, "All"))
                return;

            _ = LoadExpensesAsync();
        }

        // تعريف الدالة ExpensesForm_Load لتنفيذ العملية المرتبطة بها.
        private async void ExpensesForm_Load(object sender, EventArgs e)
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
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
                UIHelper.ShowException("حدث خطأ أثناء تحميل المصروفات:\n", ex);
            }

            finally
            {
                isLoading = false;
            }
        }

        // تعريف الدالة InitializeLookups لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة LoadExpensesAsync لتنفيذ العملية المرتبطة بها.
        private async Task LoadExpensesAsync()
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
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

        // تعريف الدالة ApplyFilter لتنفيذ العملية المرتبطة بها.
        private void ApplyFilter()
        {
            if (allExpenses == null)
                return;

            DataView dv = allExpenses.DefaultView;

            string searchText = UIHelper.EscapeDataViewFilterValue(txtSearch.Text);
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


                filter += "Category = '" + UIHelper.EscapeDataViewFilterValue(selectedCategory) + "'";
            }

            dv.RowFilter = filter;

            dataGridViewExpenses.DataSource = dv;

            lblRecordCount.Text = "عدد المصروفات: " + dv.Count;

            FormatGrid();
        }

        // تعريف الدالة EscapeFilter لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة FormatGrid لتنفيذ العملية المرتبطة بها.
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

            dataGridViewExpenses.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            SetExpenseColumnWidth("ExpenseNumber", 120);
            SetExpenseColumnWidth("Amount", 110);
            SetExpenseColumnWidth("ExpenseDate", 115);
            SetExpenseColumnWidth("Category", 145);
            SetExpenseColumnWidth("PayeeName", 180);
            SetExpenseColumnWidth("PaymentMethod", 125);
            SetExpenseColumnWidth("Description", 245);
            SetExpenseColumnWidth("CreatedAt", 145);
            SetExpenseColumnWidth("UpdatedAt", 145);

            foreach (DataGridViewColumn column in dataGridViewExpenses.Columns)
            {
                if (column.Visible)
                    column.SortMode = DataGridViewColumnSortMode.Automatic;
            }
        }

        private void SetExpenseColumnWidth(string name, int width)
        {
            if (!dataGridViewExpenses.Columns.Contains(name))
                return;

            DataGridViewColumn column = dataGridViewExpenses.Columns[name];
            if (!column.Visible)
                return;

            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            column.MinimumWidth = Math.Min(95, Math.Max(55, width));
            column.Width = width;
            column.Resizable = DataGridViewTriState.True;
        }

        // تعريف الدالة HideColumn لتنفيذ العملية المرتبطة بها.
        private void HideColumn(string columnName)
        {
            if (dataGridViewExpenses.Columns.Contains(columnName))
                dataGridViewExpenses.Columns[columnName].Visible = false;
        }

        // تعريف الدالة SetHeader لتنفيذ العملية المرتبطة بها.
        private void SetHeader(string columnName, string headerText)
        {
            if (dataGridViewExpenses.Columns.Contains(columnName))
                dataGridViewExpenses.Columns[columnName].HeaderText = headerText;
        }

        // تعريف الدالة FormatMoney لتنفيذ العملية المرتبطة بها.
        private void FormatMoney(string columnName)
        {
            if (dataGridViewExpenses.Columns.Contains(columnName))
                dataGridViewExpenses.Columns[columnName].DefaultCellStyle.Format = "N2";
        }

        // تعريف الدالة FormatDate لتنفيذ العملية المرتبطة بها.
        private void FormatDate(string columnName)
        {
            if (dataGridViewExpenses.Columns.Contains(columnName))
                dataGridViewExpenses.Columns[columnName].DefaultCellStyle.Format = "dd/MM/yyyy";
        }

        // تعريف الدالة FilterControls_Changed لتنفيذ العملية المرتبطة بها.
        private void FilterControls_Changed(object sender, EventArgs e)
        {
            if (!isLoading)
                ApplyFilter();
        }

        // تعريف الدالة ReadDecimal لتنفيذ العملية المرتبطة بها.
        private decimal ReadDecimal(string text)
        {
            if (UIHelper.TryParseDecimal(text, out decimal value))
                return value;


            return 0;
        }

        // تعريف الدالة ValidateInputs لتنفيذ العملية المرتبطة بها.
        private bool ValidateInputs()
        {
            decimal amount;
            if (!UIHelper.TryParseDecimal(txtAmount.Text, out amount) || amount <= 0)
            {
                UIHelper.FocusAndWarn(txtAmount, "أدخل مبلغاً رقمياً صحيحاً أكبر من صفر.");

                return false;
            }

            if (cmbCategory.SelectedItem == null || string.IsNullOrWhiteSpace(cmbCategory.Text))
            {
                UIHelper.ShowWarning("اختر فئة المصروف.");
                cmbCategory.Focus();

                return false;
            }

            if (cmbPaymentMethod.SelectedIndex < 0 || string.IsNullOrWhiteSpace(cmbPaymentMethod.Text))
            {
                UIHelper.ShowWarning("اختر طريقة دفع المصروف.");
                cmbPaymentMethod.Focus();

                return false;
            }

            if (dtpExpenseDate.Value.Date > DateTime.Today)
            {
                UIHelper.ShowWarning("تاريخ المصروف لا يمكن أن يكون في المستقبل.");
                dtpExpenseDate.Focus();

                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPayeeName.Text))
            {
                UIHelper.FocusAndWarn(txtPayeeName, "اسم المستفيد مطلوب.");

                return false;
            }
            if (!UIHelper.IsValidTextOnly(txtPayeeName.Text, 2, 200))
            {
                UIHelper.FocusAndWarn(txtPayeeName, "اسم المستفيد يجب أن يحتوي على أحرف ومسافات فقط دون أرقام.");

                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDescription.Text) || txtDescription.Text.Trim().Length > 500)
            {
                UIHelper.FocusAndWarn(txtDescription, "أدخل بيان المصروف بطول لا يتجاوز 500 حرف.");

                return false;
            }

            if (txtNotes.Text.Trim().Length > 1000)
            {
                UIHelper.FocusAndWarn(txtNotes, "الملاحظات يجب ألا تتجاوز 1000 حرف.");

                return false;
            }


            return true;
        }

        // تعريف الدالة GetExpenseFromInputs لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة ClearInputs لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة dataGridViewExpenses_CellClick لتنفيذ العملية المرتبطة بها.
        private void dataGridViewExpenses_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dataGridViewExpenses.Rows.Count == 0)
                return;

            DataRowView rowView = dataGridViewExpenses.Rows[e.RowIndex].DataBoundItem as DataRowView;

            if (rowView != null)
                FillFieldsFromRow(rowView.Row);
        }

        // تعريف الدالة FillFieldsFromRow لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة btnAdd_Click لتنفيذ العملية المرتبطة بها.
        private async void btnAdd_Click(object sender, EventArgs e)
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                CurrentUser.DemandAction("Expenses", "Add", "ليس لديك صلاحية إضافة المصروفات.");
                if (!ValidateInputs())
                    return;

                Expense expense = GetExpenseFromInputs();


                await Task.Run(() => expenseService.AddExpense(expense));

                UIHelper.ShowInfo("تمت إضافة المصروف وإنشاء سند صرف تلقائي بنجاح.");
                ApplicationLogger.LogUsage("شاشة المصروفات", "إضافة مصروف", "تمت إضافة مصروف بنجاح");

                await LoadExpensesAsync();
                DataRefreshNotifier.Notify("Expenses", this);
                ClearInputs();
            }

            catch (Exception ex)
            {
                UIHelper.ShowException("إضافة المصروف", ex);
            }
        }

        // تعريف الدالة btnUpdate_Click لتنفيذ العملية المرتبطة بها.
        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                CurrentUser.DemandAction("Expenses", "Edit", "ليس لديك صلاحية تعديل المصروفات.");
                if (selectedExpenseId == 0)
                {
                    UIHelper.ShowWarning("اختر المصروف من الجدول أولاً.");

                    return;
                }

                if (!ValidateInputs())
                    return;

                Expense expense = GetExpenseFromInputs();


                bool result = await Task.Run(() => expenseService.UpdateExpense(expense));

                if (result) UIHelper.ShowInfo("تم تعديل المصروف بنجاح."); else UIHelper.ShowWarning("لم يتم العثور على المصروف.");
                if (result) ApplicationLogger.LogUsage("شاشة المصروفات", "تعديل مصروف", "تم تعديل المصروف بنجاح");

                await LoadExpensesAsync();
                if (result)
                    DataRefreshNotifier.Notify("Expenses", this);
                ClearInputs();
            }

            catch (Exception ex)
            {
                UIHelper.ShowException("تعديل المصروف", ex);
            }
        }

        // تعريف الدالة btnDelete_Click لتنفيذ العملية المرتبطة بها.
        private async void btnDelete_Click(object sender, EventArgs e)
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                CurrentUser.DemandAction("Expenses", "Delete", "ليس لديك صلاحية حذف المصروفات.");
                if (selectedExpenseId == 0)
                {
                    UIHelper.ShowWarning("اختر المصروف من الجدول أولاً.");

                    return;
                }

                bool confirm = UIHelper.ShowConfirmation(
                    "هل تريد حذف المصروف المحدد؟\nملاحظة: السندات المرتبطة لن تُحذف للحفاظ على الأثر المالي.",
                    "تأكيد الحذف");

                if (!confirm)
                    return;


                bool result = await Task.Run(() => expenseService.DeleteExpense(selectedExpenseId));

                if (result) UIHelper.ShowInfo("تم حذف المصروف."); else UIHelper.ShowWarning("لم يتم العثور على المصروف.");
                if (result) ApplicationLogger.LogUsage("شاشة المصروفات", "حذف مصروف", "تم حذف المصروف بنجاح");

                await LoadExpensesAsync();
                if (result)
                    DataRefreshNotifier.Notify("Expenses", this);
                ClearInputs();
            }

            catch (Exception ex)
            {
                UIHelper.ShowException("حذف المصروف", ex);
            }
        }

        // تعريف الدالة btnClear_Click لتنفيذ العملية المرتبطة بها.
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearInputs();
        }
    }
}
