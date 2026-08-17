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
        private readonly FeePlanService feePlanService = new FeePlanService();

        private int selectedFeePlanId = 0;
        private DataTable allFeePlans;

        public FeePlansForm()
        {
            InitializeComponent();
            SchoolSystem.Helpers.UIHelper.ApplyStyle(this);
            Dock = DockStyle.Fill;
            Load += FeePlansForm_Load;
        }

        private async void FeePlansForm_Load(object sender, EventArgs e)
        {
            try
            {
                InitializeLookups();

                await LoadClassesAsync();
                await LoadFeePlansAsync();

                ClearInputs();
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("حدث خطأ أثناء تحميل شاشة تعريف الرسوم:\n", ex);
            }
        }

        private void InitializeLookups()
        {
            cmbFeeType.Items.Clear();
            cmbFeeType.Items.AddRange(new object[]
            {
                "رسوم تسجيل",
                "رسوم دراسية",
                "رسوم كتب",
                "رسوم مواصلات",
                "رسوم زي مدرسي",
                "رسوم امتحانات",
                "رسوم أنشطة",
                "أخرى"
            });
            cmbFeeType.SelectedIndex = 1;

            cmbAcademicYear.Items.Clear();

            int year = DateTime.Today.Year;
            cmbAcademicYear.Items.Add((year - 1) + " / " + year);
            cmbAcademicYear.Items.Add(year + " / " + (year + 1));
            cmbAcademicYear.Items.Add((year + 1) + " / " + (year + 2));
            cmbAcademicYear.SelectedIndex = 1;

            dtpDueDate.Value = DateTime.Today.AddDays(30);
            chkIsRequired.Checked = true;
        }

        private async Task LoadClassesAsync()
        {
            DataTable classes = await Task.Run(() => feePlanService.GetClasses());

            cmbClass.DataSource = classes;
            cmbClass.DisplayMember = "ClassName";
            cmbClass.ValueMember = "ClassID";

            if (cmbClass.Items.Count > 0)
                cmbClass.SelectedIndex = 0;
        }

        private async Task LoadFeePlansAsync()
        {
            Cursor = Cursors.WaitCursor;

            allFeePlans = await Task.Run(() => feePlanService.GetAllFeePlans());

            ApplyFilter();

            Cursor = Cursors.Default;
        }

        private void ApplyFilter()
        {
            if (allFeePlans == null)
                return;

            DataView dv = allFeePlans.DefaultView;

            string search = UIHelper.EscapeDataViewFilterValue(txtSearch.Text);

            if (!string.IsNullOrWhiteSpace(search))
            {
                dv.RowFilter =
                    "AcademicYear LIKE '%" + search + "%' " +
                    "OR ClassName LIKE '%" + search + "%' " +
                    "OR FeeType LIKE '%" + search + "%'";
            }
            else
            {
                dv.RowFilter = "";
            }

            dataGridViewFeePlans.DataSource = dv;
            lblRecordCount.Text = "عدد السجلات: " + dv.Count;

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
            if (dataGridViewFeePlans.Columns.Count == 0)
                return;

            HideColumn("FeePlanID");
            HideColumn("ClassID");
            HideColumn("CreatedAt");

            SetHeader("AcademicYear", "العام الدراسي");
            SetHeader("ClassName", "الصف");
            SetHeader("FeeType", "نوع الرسوم");
            SetHeader("Amount", "المبلغ");
            SetHeader("DueDate", "تاريخ الاستحقاق");
            SetHeader("IsRequired", "إلزامية");
            SetHeader("Notes", "ملاحظات");

            if (dataGridViewFeePlans.Columns.Contains("Amount"))
                dataGridViewFeePlans.Columns["Amount"].DefaultCellStyle.Format = "N2";

            if (dataGridViewFeePlans.Columns.Contains("DueDate"))
                dataGridViewFeePlans.Columns["DueDate"].DefaultCellStyle.Format = "dd/MM/yyyy";
        }

        private void HideColumn(string columnName)
        {
            if (dataGridViewFeePlans.Columns.Contains(columnName))
                dataGridViewFeePlans.Columns[columnName].Visible = false;
        }

        private void SetHeader(string columnName, string headerText)
        {
            if (dataGridViewFeePlans.Columns.Contains(columnName))
                dataGridViewFeePlans.Columns[columnName].HeaderText = headerText;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
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

        private FeePlan GetPlanFromInputs()
        {
            return new FeePlan
            {
                FeePlanID = selectedFeePlanId,
                AcademicYear = cmbAcademicYear.Text.Trim(),
                ClassID = Convert.ToInt32(cmbClass.SelectedValue),
                FeeType = cmbFeeType.Text.Trim(),
                Amount = ReadDecimal(txtAmount.Text),
                DueDate = dtpDueDate.Value.Date,
                IsRequired = chkIsRequired.Checked,
                Notes = txtNotes.Text.Trim()
            };
        }

        private void ClearInputs()
        {
            selectedFeePlanId = 0;

            if (cmbAcademicYear.Items.Count > 0)
                cmbAcademicYear.SelectedIndex = 1;

            if (cmbClass.Items.Count > 0)
                cmbClass.SelectedIndex = 0;

            if (cmbFeeType.Items.Count > 0)
                cmbFeeType.SelectedIndex = 1;

            txtAmount.Text = "0";
            dtpDueDate.Value = DateTime.Today.AddDays(30);
            chkIsRequired.Checked = true;
            txtNotes.Clear();
        }

        private void dataGridViewFeePlans_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataRowView rowView = dataGridViewFeePlans.Rows[e.RowIndex].DataBoundItem as DataRowView;

            if (rowView != null)
                FillFieldsFromRow(rowView.Row);
        }

        private void FillFieldsFromRow(DataRow row)
        {
            selectedFeePlanId = Convert.ToInt32(row["FeePlanID"]);

            cmbAcademicYear.Text = row["AcademicYear"].ToString();
            cmbClass.SelectedValue = Convert.ToInt32(row["ClassID"]);
            cmbFeeType.Text = row["FeeType"].ToString();

            txtAmount.Text = Convert.ToDecimal(row["Amount"]).ToString("N2");
            dtpDueDate.Value = Convert.ToDateTime(row["DueDate"]);

            chkIsRequired.Checked = Convert.ToBoolean(row["IsRequired"]);

            txtNotes.Text = row["Notes"] == DBNull.Value ? "" : row["Notes"].ToString();
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentUser.DemandAction("FeePlans", "Add", "ليس لديك صلاحية إضافة خطط الرسوم.");
                if (!ValidateInputs())
                    return;

                FeePlan plan = GetPlanFromInputs();

                await Task.Run(() => feePlanService.AddFeePlan(plan));

                UIHelper.ShowInfo("تمت إضافة رسوم الصف بنجاح.");

                await LoadFeePlansAsync();
                ClearInputs();
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("فشل إضافة رسوم الصف:\n", ex);
            }
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentUser.DemandAction("FeePlans", "Edit", "ليس لديك صلاحية تعديل خطط الرسوم.");
                if (selectedFeePlanId == 0)
                {
                    UIHelper.ShowWarning("اختر سجل الرسوم من الجدول.");
                    return;
                }

                if (!ValidateInputs())
                    return;

                FeePlan plan = GetPlanFromInputs();

                bool result = await Task.Run(() => feePlanService.UpdateFeePlan(plan));

                if (result)
                    UIHelper.ShowInfo("تم تعديل رسوم الصف بنجاح.");
                else
                    UIHelper.ShowWarning("لم يتم العثور على سجل الرسوم أو لم يتم تعديله.");

                await LoadFeePlansAsync();
                ClearInputs();
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("فشل تعديل رسوم الصف:\n", ex);
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentUser.DemandAction("FeePlans", "Delete", "ليس لديك صلاحية حذف خطط الرسوم.");
                if (selectedFeePlanId == 0)
                {
                    UIHelper.ShowWarning("اختر سجل الرسوم من الجدول.");
                    return;
                }

                DialogResult confirm = MessageBox.Show(
                    "هل تريد حذف رسوم الصف المحددة؟",
                    "تأكيد الحذف",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirm != DialogResult.Yes)
                    return;

                bool result = await Task.Run(() => feePlanService.DeleteFeePlan(selectedFeePlanId));

                if (result)
                    UIHelper.ShowInfo("تم حذف رسوم الصف بنجاح.");
                else
                    UIHelper.ShowWarning("لم يتم العثور على سجل الرسوم أو لم يتم حذفه.");

                await LoadFeePlansAsync();
                ClearInputs();
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("فشل حذف رسوم الصف:\n", ex);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearInputs();
        }
    }
}
