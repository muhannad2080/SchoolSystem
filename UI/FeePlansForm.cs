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

    public partial class FeePlansForm : UserControl
    {
        // تعريف الدالة FeePlanService لتنفيذ العملية المرتبطة بها.
        private readonly FeePlanService feePlanService = new FeePlanService();


        private int selectedFeePlanId = 0;

        private DataTable allFeePlans;

        // تعريف الدالة FeePlansForm لتنفيذ العملية المرتبطة بها.
        public FeePlansForm()
        {

            InitializeComponent();

            SchoolSystem.Helpers.UIHelper.ApplyStyle(this);
            Dock = DockStyle.Fill;

            DataRefreshNotifier.DataChanged += DataRefreshNotifier_DataChanged;
            Disposed += (sender, e) =>
                DataRefreshNotifier.DataChanged -= DataRefreshNotifier_DataChanged;

            Load += FeePlansForm_Load;



            cmbClass.SelectedIndexChanged += FeePlanFilter_Changed;

            cmbFeeType.SelectedIndexChanged += FeePlanFilter_Changed;

            cmbAcademicYear.SelectedIndexChanged += FeePlanFilter_Changed;

            cmbAcademicYear.TextChanged += FeePlanFilter_Changed;

        }

        private async void DataRefreshNotifier_DataChanged(
            object sender,
            DataRefreshEventArgs e)
        {
            if (ReferenceEquals(sender, this) ||
                IsDisposed)
                return;

            if (!DataRefreshNotifier.IsRelated(e, "FeePlans") &&
                !DataRefreshNotifier.IsRelated(e, "Classes"))
                return;

            await LoadClassesAsync();
            await LoadFeePlansAsync();
        }

        // تعريف الدالة FeePlansForm_Load لتنفيذ العملية المرتبطة بها.
        private async void FeePlansForm_Load(object sender, EventArgs e)
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                InitializeLookups();

                await LoadFeeTypesAsync();


                await LoadClassesAsync();

                await LoadFeePlansAsync();

                ClearInputs();
            }

            catch (Exception ex)
            {
                UIHelper.ShowException("حدث خطأ أثناء تحميل شاشة تعريف الرسوم:\n", ex);
            }
        }

        // تعريف الدالة InitializeLookups لتنفيذ العملية المرتبطة بها.
        private void InitializeLookups()
        {
            cmbFeeType.Items.Clear();

            cmbAcademicYear.Items.Clear();


            string dbYear = SchoolSystem.Helpers.AcademicYearHelper.GetCurrentAcademicYear();
            if (!string.IsNullOrWhiteSpace(dbYear))
            {
                cmbAcademicYear.Items.Add(dbYear);
                cmbAcademicYear.SelectedIndex = 0;
            }

            else
            {
                int year = DateTime.Today.Year;
                cmbAcademicYear.Items.Add((year - 1) + " / " + year);
                cmbAcademicYear.Items.Add(year + " / " + (year + 1));
                cmbAcademicYear.Items.Add((year + 1) + " / " + (year + 2));
                cmbAcademicYear.SelectedIndex = 1;
            }

            dtpDueDate.Value = DateTime.Today.AddDays(30);
            chkIsRequired.Checked = true;
        }

        // تعريف الدالة LoadFeeTypesAsync لتنفيذ العملية المرتبطة بها.
        private async Task LoadFeeTypesAsync()
        {

            DataTable feeTypes = await Task.Run(() => feePlanService.GetFeeTypes());
            cmbFeeType.Items.Clear();

            foreach (DataRow row in feeTypes.Rows)
            {
                string feeType = Convert.ToString(row["FeeType"]);
                if (!string.IsNullOrWhiteSpace(feeType))
                    cmbFeeType.Items.Add(feeType.Trim());
            }

            if (cmbFeeType.FindStringExact("الرسوم الكلية") < 0)
                cmbFeeType.Items.Insert(0, "الرسوم الكلية");

            if (cmbFeeType.FindStringExact("رسوم دراسية") < 0)
                cmbFeeType.Items.Add("رسوم دراسية");

            int academicIndex = cmbFeeType.FindStringExact("رسوم دراسية");
            cmbFeeType.SelectedIndex = academicIndex >= 0 ? academicIndex : 0;
        }

        // تعريف الدالة LoadClassesAsync لتنفيذ العملية المرتبطة بها.
        private async Task LoadClassesAsync()
        {

            DataTable classes = await Task.Run(() => feePlanService.GetClasses());

            cmbClass.DataSource = classes;
            cmbClass.DisplayMember = "ClassName";
            cmbClass.ValueMember = "ClassID";

            if (cmbClass.Items.Count > 0)
                cmbClass.SelectedIndex = 0;
        }

        // تعريف الدالة LoadFeePlansAsync لتنفيذ العملية المرتبطة بها.
        private async Task LoadFeePlansAsync()
        {
            Cursor = Cursors.WaitCursor;


            allFeePlans = await Task.Run(() => feePlanService.GetAllFeePlans());

            PrepareTotalAmountColumn();

            ApplyFilter();

            Cursor = Cursors.Default;
        }

        private void PrepareTotalAmountColumn()
        {
            if (allFeePlans == null)
                return;

            if (!allFeePlans.Columns.Contains("TotalFeeAmount"))
            {
                allFeePlans.Columns.Add(
                    "TotalFeeAmount",
                    typeof(decimal));
            }

            foreach (DataRow row in allFeePlans.Rows)
            {
                int classId =
                    row["ClassID"] == DBNull.Value
                        ? 0
                        : Convert.ToInt32(row["ClassID"]);

                string year =
                    Convert.ToString(row["AcademicYear"])
                        .Trim()
                        .Replace(" ", "");

                decimal total = 0m;

                foreach (DataRow totalRow in allFeePlans.Rows)
                {
                    int totalClassId =
                        totalRow["ClassID"] == DBNull.Value
                            ? 0
                            : Convert.ToInt32(totalRow["ClassID"]);

                    string totalYear =
                        Convert.ToString(totalRow["AcademicYear"])
                            .Trim()
                            .Replace(" ", "");

                    string totalType =
                        Convert.ToString(totalRow["FeeType"])
                            .Trim();

                    if (totalClassId == classId &&
                        string.Equals(
                            totalYear,
                            year,
                            StringComparison.OrdinalIgnoreCase) &&
                        IsTotalFeeType(totalType))
                    {
                        total =
                            totalRow["Amount"] == DBNull.Value
                                ? 0m
                                : Convert.ToDecimal(
                                    totalRow["Amount"]);
                        break;
                    }
                }

                row["TotalFeeAmount"] = total;
            }
        }

        // تعريف الدالة ApplyFilter لتنفيذ العملية المرتبطة بها.
        private void ApplyFilter()
        {
            if (allFeePlans == null)
                return;

            DataView dv = allFeePlans.DefaultView;

            string search = UIHelper.EscapeDataViewFilterValue(txtSearch.Text);
            var filters = new System.Collections.Generic.List<string>();

            if (!string.IsNullOrWhiteSpace(search))
            {
                filters.Add(
                    "(AcademicYear LIKE '%" + search + "%' " +
                    "OR ClassName LIKE '%" + search + "%' " +
                    "OR FeeType LIKE '%" + search + "%')");
            }

            string academicYear = cmbAcademicYear.Text == null
                ? ""
                : cmbAcademicYear.Text.Trim().Replace(" ", "");
            if (!string.IsNullOrWhiteSpace(academicYear) && allFeePlans.Columns.Contains("AcademicYear"))
                filters.Add("AcademicYear = '" + UIHelper.EscapeDataViewFilterValue(academicYear) + "'");

            int classId;
            if (cmbClass.SelectedValue != null &&
                int.TryParse(Convert.ToString(cmbClass.SelectedValue), out classId) &&
                classId > 0 && allFeePlans.Columns.Contains("ClassID"))
            {
                filters.Add("ClassID = " + classId);
            }

            string feeType =
                cmbFeeType.Text == null
                    ? ""
                    : cmbFeeType.Text.Trim();

            if (!string.IsNullOrWhiteSpace(feeType) &&
                allFeePlans.Columns.Contains("FeeType"))
            {
                if (IsTotalFeeType(feeType))
                {
                    filters.Add(
                        "(FeeType = 'الرسوم الكلية' " +
                        "OR FeeType = 'رسوم كلية' " +
                        "OR FeeType = 'الرسوم الكليه' " +
                        "OR FeeType = 'رسوم كليه' " +
                        "OR FeeType = 'Total Fee')");
                }
                else
                {
                    filters.Add(
                        "FeeType = '" +
                        UIHelper.EscapeDataViewFilterValue(feeType) +
                        "'");
                }
            }

            dv.RowFilter = string.Join(" AND ", filters.ToArray());

            dataGridViewFeePlans.DataSource = dv;
            decimal selectedClassTotal =
                GetTotalAmountForClassYear();

            lblRecordCount.Text =
                "عدد البنود: " +
                dv.Count +
                " | الرسوم الكلية للصف: " +
                selectedClassTotal.ToString("N2") +
                " ريال";

            FormatGrid();
            UpdateSelectedPlanSummary();
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
            if (dataGridViewFeePlans.Columns.Count == 0)
                return;

            HideColumn("FeePlanID");
            HideColumn("ClassID");
            HideColumn("CreatedAt");

            SetHeader("AcademicYear", "العام الدراسي");
            SetHeader("ClassName", "الصف");
            SetHeader("FeeType", "نوع الرسوم");
            SetHeader("Amount", "مبلغ النوع");
            SetHeader("TotalFeeAmount", "الرسوم الكلية للصف");
            SetHeader("DueDate", "تاريخ الاستحقاق");
            SetHeader("IsRequired", "إلزامية");
            SetHeader("Notes", "ملاحظات");

            if (dataGridViewFeePlans.Columns.Contains("Amount"))
                dataGridViewFeePlans.Columns["Amount"].DefaultCellStyle.Format = "N2";

            if (dataGridViewFeePlans.Columns.Contains("TotalFeeAmount"))
                dataGridViewFeePlans.Columns["TotalFeeAmount"].DefaultCellStyle.Format = "N2";

            if (dataGridViewFeePlans.Columns.Contains("TotalFeeAmount"))
            {
                dataGridViewFeePlans.Columns["TotalFeeAmount"]
                    .DefaultCellStyle.Font =
                    new System.Drawing.Font(
                        "Tahoma",
                        9F,
                        System.Drawing.FontStyle.Bold);
            }

            dataGridViewFeePlans.AllowUserToOrderColumns = true;
            dataGridViewFeePlans.AllowUserToResizeColumns = true;
            dataGridViewFeePlans.AllowUserToResizeRows = false;
            dataGridViewFeePlans.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dataGridViewFeePlans.MultiSelect = false;
            dataGridViewFeePlans.RowHeadersVisible = false;
            dataGridViewFeePlans.ColumnHeadersHeight = 40;
            dataGridViewFeePlans.RowTemplate.Height = 36;
            dataGridViewFeePlans.DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleRight;
            dataGridViewFeePlans.ColumnHeadersDefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;

            if (dataGridViewFeePlans.Columns.Contains("DueDate"))
                dataGridViewFeePlans.Columns["DueDate"].DefaultCellStyle.Format = "dd/MM/yyyy";
        }

        // تعريف الدالة HideColumn لتنفيذ العملية المرتبطة بها.
        private void HideColumn(string columnName)
        {
            if (dataGridViewFeePlans.Columns.Contains(columnName))
                dataGridViewFeePlans.Columns[columnName].Visible = false;
        }

        // تعريف الدالة SetHeader لتنفيذ العملية المرتبطة بها.
        private void SetHeader(string columnName, string headerText)
        {
            if (dataGridViewFeePlans.Columns.Contains(columnName))
                dataGridViewFeePlans.Columns[columnName].HeaderText = headerText;
        }

        // تعريف الدالة txtSearch_TextChanged لتنفيذ العملية المرتبطة بها.
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        // تعريف الدالة FeePlanFilter_Changed لتنفيذ العملية المرتبطة بها.
        private void FeePlanFilter_Changed(object sender, EventArgs e)
        {
            if (!IsDisposed)
            {
                ApplyFilter();
                UpdateSelectedPlanSummary();
            }
        }

        private void UpdateSelectedPlanSummary()
        {
            if (txtPlanNumber == null || txtTotalFeeAmount == null)
                return;

            int classId =
                cmbClass.SelectedValue == null
                    ? 0
                    : UIHelper.GetSelectedId(cmbClass);

            string academicYear =
                (cmbAcademicYear.Text ?? string.Empty)
                    .Trim()
                    .Replace(" ", "");

            string feeType =
                (cmbFeeType.Text ?? string.Empty).Trim();

            DataRow selectedRow = null;
            DataRow totalRow = null;

            if (allFeePlans != null &&
                classId > 0 &&
                !string.IsNullOrWhiteSpace(academicYear))
            {
                foreach (DataRow row in allFeePlans.Rows)
                {
                    int rowClassId =
                        row["ClassID"] == DBNull.Value
                            ? 0
                            : Convert.ToInt32(row["ClassID"]);

                    string rowYear =
                        Convert.ToString(row["AcademicYear"])
                            .Trim()
                            .Replace(" ", "");

                    string rowType =
                        Convert.ToString(row["FeeType"])
                            .Trim();

                    if (rowClassId != classId ||
                        !string.Equals(
                            rowYear,
                            academicYear,
                            StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    if (IsTotalFeeType(rowType) &&
                        totalRow == null)
                    {
                        totalRow = row;
                    }

                    bool sameType =
                        IsTotalFeeType(feeType) &&
                        IsTotalFeeType(rowType);

                    if (!sameType)
                    {
                        sameType =
                            string.Equals(
                                rowType,
                                feeType,
                                StringComparison.OrdinalIgnoreCase);
                    }

                    if (sameType)
                    {
                        selectedRow = row;
                    }
                }
            }

            // عند اختيار نوع موجود، نعرض رقمه ومبلغه حتى قبل الضغط على الصف.
            if (selectedRow != null)
            {
                int displayedPlanId =
                    selectedRow["FeePlanID"] == DBNull.Value
                        ? 0
                        : Convert.ToInt32(
                            selectedRow["FeePlanID"]);

                selectedFeePlanId = displayedPlanId;

                txtPlanNumber.Text =
                    displayedPlanId > 0
                        ? displayedPlanId.ToString()
                        : "جديدة";

                txtAmount.Text =
                    selectedRow["Amount"] == DBNull.Value
                        ? "0.00"
                        : Convert.ToDecimal(
                            selectedRow["Amount"])
                            .ToString("N2");
            }
            else
            {
                selectedFeePlanId = 0;
                txtPlanNumber.Text = "جديدة";
                txtAmount.Text = "0.00";
            }

            decimal totalAmount =
                totalRow == null ||
                totalRow["Amount"] == DBNull.Value
                    ? 0m
                    : Convert.ToDecimal(
                        totalRow["Amount"]);

            txtTotalFeeAmount.Text =
                totalAmount.ToString("N2") +
                " ريال";
        }

        // تعريف الدالة ReadDecimal لتنفيذ العملية المرتبطة بها.
        private decimal ReadDecimal(string text)
        {
            if (decimal.TryParse(text, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal value))
                return value;

            if (decimal.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out value))
                return value;


            return 0;
        }

        // تعريف الدالة ValidateInputs لتنفيذ العملية المرتبطة بها.
        private bool ValidateInputs()
        {
            string academicYear = cmbAcademicYear.Text.Trim();
            if (!UIHelper.IsValidAcademicYear(academicYear))
            {
                UIHelper.FocusAndWarn(cmbAcademicYear, "اختر عاماً دراسياً بصيغة صحيحة مثل 2026/2027.");

                return false;
            }

            int classId;
            if (cmbClass.SelectedValue == null || !int.TryParse(cmbClass.SelectedValue.ToString(), out classId) || classId <= 0)
            {
                UIHelper.FocusAndWarn(cmbClass, "اختر صفاً صالحاً.");

                return false;
            }

            if (string.IsNullOrWhiteSpace(cmbFeeType.Text) || cmbFeeType.Text.Trim().Length > 100)
            {
                UIHelper.FocusAndWarn(cmbFeeType, "أدخل نوع رسوم صحيحاً وبطول مناسب.");

                return false;
            }

            decimal amount;
            if (!UIHelper.TryParseDecimal(txtAmount.Text, out amount) || amount <= 0)
            {
                UIHelper.FocusAndWarn(txtAmount, "أدخل مبلغ رسوم رقمي صحيح أكبر من صفر.");

                return false;
            }

            if (dtpDueDate.Value.Date < DateTime.Today && selectedFeePlanId == 0)
            {
                UIHelper.FocusAndWarn(dtpDueDate, "لا يمكن أن يكون تاريخ الاستحقاق في الماضي عند إنشاء خطة جديدة.");

                return false;
            }

            if (txtNotes.Text.Trim().Length > 500)
            {
                UIHelper.FocusAndWarn(txtNotes, "الملاحظات يجب ألا تتجاوز 500 حرف.");

                return false;
            }


            return true;
        }

        // تعريف الدالة GetPlanFromInputs لتنفيذ العملية المرتبطة بها.
        private FeePlan GetPlanFromInputs()
        {

            return new FeePlan
            {
                FeePlanID = selectedFeePlanId,
                AcademicYear = cmbAcademicYear.Text.Trim(),
                ClassID = UIHelper.GetSelectedId(cmbClass),
                FeeType = cmbFeeType.Text.Trim(),
                Amount = ReadDecimal(txtAmount.Text),
                DueDate = dtpDueDate.Value.Date,
                IsRequired = chkIsRequired.Checked,
                Notes = txtNotes.Text.Trim()

            };
        }

        // تعريف الدالة ClearInputs لتنفيذ العملية المرتبطة بها.
        private void ClearInputs()
        {
            selectedFeePlanId = 0;
            txtPlanNumber.Clear();
            txtTotalFeeAmount.Text = "0.00 ريال";

            cmbAcademicYear.Enabled = true;
            cmbClass.Enabled = true;
            cmbFeeType.Enabled = true;

            if (cmbAcademicYear.Items.Count > 0)
                cmbAcademicYear.SelectedIndex = Math.Min(1, cmbAcademicYear.Items.Count - 1);

            if (cmbClass.Items.Count > 0)
                cmbClass.SelectedIndex = 0;

            if (cmbFeeType.Items.Count > 0)
            {
                int academicIndex = cmbFeeType.FindStringExact("رسوم دراسية");
                cmbFeeType.SelectedIndex = academicIndex >= 0
                    ? academicIndex
                    : Math.Min(1, cmbFeeType.Items.Count - 1);
            }

            txtAmount.Text = "0";
            dtpDueDate.Value = DateTime.Today.AddDays(30);
            chkIsRequired.Checked = true;
            txtNotes.Clear();
            ApplyFilter();
            UpdateSelectedPlanSummary();
        }

        // تعريف الدالة dataGridViewFeePlans_CellClick لتنفيذ العملية المرتبطة بها.
        private void dataGridViewFeePlans_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataRowView rowView = dataGridViewFeePlans.Rows[e.RowIndex].DataBoundItem as DataRowView;

            if (rowView != null)
                FillFieldsFromRow(rowView.Row);
        }

        // تعريف الدالة FillFieldsFromRow لتنفيذ العملية المرتبطة بها.
        private void FillFieldsFromRow(DataRow row)
        {
            selectedFeePlanId = Convert.ToInt32(row["FeePlanID"]);

            cmbAcademicYear.Text = row["AcademicYear"].ToString();
            cmbClass.SelectedValue = Convert.ToInt32(row["ClassID"]);
            cmbFeeType.Text = row["FeeType"].ToString();

            txtAmount.Text =
                Convert.ToDecimal(row["Amount"])
                    .ToString("N2");

            txtPlanNumber.Text =
                selectedFeePlanId.ToString();

            cmbAcademicYear.Enabled = false;
            cmbClass.Enabled = false;
            cmbFeeType.Enabled = false;

            dtpDueDate.Value = Convert.ToDateTime(row["DueDate"]);

            chkIsRequired.Checked =
                Convert.ToBoolean(row["IsRequired"]);

            txtNotes.Text =
                row["Notes"] == DBNull.Value
                    ? ""
                    : row["Notes"].ToString();

            UpdateSelectedPlanSummary();
        }

        // تعريف الدالة btnAdd_Click لتنفيذ العملية المرتبطة بها.
        private async void btnAdd_Click(object sender, EventArgs e)
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                CurrentUser.DemandAction("FeePlans", "Add", "ليس لديك صلاحية إضافة خطط الرسوم.");
                if (!ValidateInputs())
                    return;

                FeePlan plan = GetPlanFromInputs();
                int affectedPlans;

                if (selectedFeePlanId > 0 &&
                    !IsTotalFeeType(plan.FeeType))
                {
                    UIHelper.ShowWarning(
                        "هذا النوع من الرسوم موجود مسبقًا لنفس الصف والعام. استخدم «تعديل» لتغيير المبلغ.");

                    return;
                }

                if (IsTotalFeeType(plan.FeeType))
                {
                    affectedPlans =
                        await Task.Run(() =>
                            feePlanService.DistributeTotalFee(plan));
                    if (affectedPlans > 0)
                        UIHelper.ShowInfo("تم إنشاء " + affectedPlans + " نوع رسوم وتوزيع الرسوم الكلية عليها بالنسب المنطقية في الصف نفسه.");
                    else
                        UIHelper.ShowInfo("تم تحديث توزيع الرسوم الكلية على أنواع الرسوم الموجودة في الصف.");
                    ApplicationLogger.LogUsage("شاشة تعريف الرسوم", "توزيع الرسوم الكلية", "تم توزيع الرسوم الكلية على أنواع الرسوم");
                }

                else
                {

                    bool added =
                        await Task.Run(() =>
                            feePlanService.AddFeePlan(plan));

                    if (!added)
                        throw new InvalidOperationException(
                            "تعذر إضافة نوع الرسوم.");

                    UIHelper.ShowInfo(
                        "تمت إضافة نوع الرسوم وتحديث الرسوم الكلية للصف نفسه.");
                    ApplicationLogger.LogUsage("شاشة تعريف الرسوم", "إضافة رسوم", "تمت إضافة رسوم الصف بنجاح");
                }


                await LoadFeePlansAsync();
                SelectPlanByContext(
                    plan.ClassID,
                    plan.AcademicYear,
                    plan.FeeType);
                DataRefreshNotifier.Notify("FeePlans", this);
                DataRefreshNotifier.Notify("Fees", this);
            }

            catch (Exception ex)
            {
                UIHelper.ShowException("فشل إضافة رسوم الصف:\n", ex);
            }
        }

        // تعريف الدالة IsTotalFeeType لتنفيذ العملية المرتبطة بها.
        private decimal GetTotalAmountForClassYear()
        {
            if (allFeePlans == null)
                return 0m;

            int classId =
                cmbClass.SelectedValue == null
                    ? 0
                    : UIHelper.GetSelectedId(cmbClass);

            string academicYear =
                (cmbAcademicYear.Text ?? string.Empty)
                    .Trim()
                    .Replace(" ", "");

            foreach (DataRow row in allFeePlans.Rows)
            {
                int rowClassId =
                    row["ClassID"] == DBNull.Value
                        ? 0
                        : Convert.ToInt32(row["ClassID"]);

                string rowYear =
                    Convert.ToString(row["AcademicYear"])
                        .Trim()
                        .Replace(" ", "");

                if (rowClassId == classId &&
                    string.Equals(
                        rowYear,
                        academicYear,
                        StringComparison.OrdinalIgnoreCase) &&
                    IsTotalFeeType(
                        Convert.ToString(row["FeeType"])))
                {
                    return row["Amount"] == DBNull.Value
                        ? 0m
                        : Convert.ToDecimal(row["Amount"]);
                }
            }

            return 0m;
        }

        private bool IsTotalFeeType(string feeType)
        {
            string value = (feeType ?? string.Empty).Trim();

            return value == "الرسوم الكلية" || value == "رسوم كلية" || value == "الرسوم الكليه" || value == "رسوم كليه" || value.Equals("Total Fee", StringComparison.OrdinalIgnoreCase);
        }

        // تعريف الدالة btnUpdate_Click لتنفيذ العملية المرتبطة بها.
        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentUser.DemandAction(
                    "FeePlans",
                    "Edit",
                    "ليس لديك صلاحية تعديل خطط الرسوم.");

                if (selectedFeePlanId == 0)
                {
                    UIHelper.ShowWarning(
                        "اختر سجل الرسوم من الجدول أولاً.");

                    return;
                }

                if (!ValidateInputs())
                    return;

                FeePlan plan = GetPlanFromInputs();

                if (IsTotalFeeType(plan.FeeType))
                {
                    int affected =
                        await Task.Run(() =>
                            feePlanService.UpdateTotalFeePlanAndDistribute(
                                plan));

                    UIHelper.ShowInfo(
                        "تم تعديل الرسوم الكلية بمبلغ " +
                        plan.Amount.ToString("N2") +
                        " ريال وإعادة توزيعه على " +
                        affected +
                        " بنود للصف نفسه.");
                }
                else
                {
                    bool result =
                        await Task.Run(() =>
                            feePlanService.UpdateChildFeePlanAndRecalculateTotal(
                                plan));

                    if (!result)
                    {
                        UIHelper.ShowWarning(
                            "لم يتم العثور على نوع الرسوم المحدد.");
                        return;
                    }

                    UIHelper.ShowInfo(
                        "تم تعديل نوع الرسوم وإعادة احتساب الرسوم الكلية للصف نفسه.");
                }

                ApplicationLogger.LogUsage(
                    "شاشة تعريف الرسوم",
                    "تعديل خطة رسوم",
                    "تحديث متزامن للكلي والبنود للصف " +
                    plan.ClassID +
                    " والعام " +
                    plan.AcademicYear);

                await LoadFeePlansAsync();

                SelectPlanByContext(
                    plan.ClassID,
                    plan.AcademicYear,
                    plan.FeeType);

                DataRefreshNotifier.Notify("FeePlans", this);
                DataRefreshNotifier.Notify("Fees", this);
                DataRefreshNotifier.Notify("Enrollment", this);
            }
            catch (Exception ex)
            {
                UIHelper.ShowException(
                    "فشل تعديل رسوم الصف:\n",
                    ex);
            }
        }

        private void SelectPlanByContext(
            int classId,
            string academicYear,
            string feeType)
        {
            if (allFeePlans == null)
                return;

            string normalizedYear =
                (academicYear ?? string.Empty)
                    .Trim()
                    .Replace(" ", "")
                    .Replace("-", "/");

            string normalizedType =
                (feeType ?? string.Empty).Trim();

            for (int i = 0; i < dataGridViewFeePlans.Rows.Count; i++)
            {
                DataRowView rowView =
                    dataGridViewFeePlans.Rows[i].DataBoundItem as DataRowView;

                if (rowView == null)
                    continue;

                DataRow row = rowView.Row;

                int rowClassId =
                    row["ClassID"] == DBNull.Value
                        ? 0
                        : Convert.ToInt32(row["ClassID"]);

                string rowYear =
                    Convert.ToString(row["AcademicYear"])
                        .Trim()
                        .Replace(" ", "")
                        .Replace("-", "/");

                string rowType =
                    Convert.ToString(row["FeeType"])
                        .Trim();

                if (rowClassId != classId ||
                    !string.Equals(
                        rowYear,
                        normalizedYear,
                        StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(
                        rowType,
                        normalizedType,
                        StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                dataGridViewFeePlans.ClearSelection();
                dataGridViewFeePlans.Rows[i].Selected = true;

                if (dataGridViewFeePlans.Columns.Count > 0)
                {
                    dataGridViewFeePlans.CurrentCell =
                        dataGridViewFeePlans.Rows[i].Cells[0];
                }

                FillFieldsFromRow(row);
                UpdateSelectedPlanSummary();
                return;
            }

            UpdateSelectedPlanSummary();
        }

        // تعريف الدالة btnDelete_Click لتنفيذ العملية المرتبطة بها.
        private async void btnDelete_Click(object sender, EventArgs e)
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                CurrentUser.DemandAction("FeePlans", "Delete", "ليس لديك صلاحية حذف خطط الرسوم.");
                if (selectedFeePlanId == 0)
                {
                    UIHelper.ShowWarning("اختر سجل الرسوم من الجدول.");

                    return;
                }


                bool confirm = UIHelper.ShowConfirmation(
                    "هل تريد حذف رسوم الصف المحددة؟",
                    "تأكيد الحذف");

                if (!confirm)
                    return;


                bool result = await Task.Run(() => feePlanService.DeleteFeePlan(selectedFeePlanId));

                if (result)
                    UIHelper.ShowInfo("تم حذف رسوم الصف بنجاح.");

                else

                    UIHelper.ShowWarning("لم يتم العثور على سجل الرسوم أو لم يتم حذفه.");

                if (result) ApplicationLogger.LogUsage("شاشة تعريف الرسوم", "حذف رسوم", "تم حذف رسوم الصف بنجاح");


                await LoadFeePlansAsync();
                DataRefreshNotifier.Notify("FeePlans", this);
                ClearInputs();
            }

            catch (InvalidOperationException ex)
            {
                UIHelper.ShowWarning(ex.Message);
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("فشل حذف رسوم الصف:\n", ex);
            }
        }

        // تعريف الدالة btnClear_Click لتنفيذ العملية المرتبطة بها.
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearInputs();
        }
    }
}
