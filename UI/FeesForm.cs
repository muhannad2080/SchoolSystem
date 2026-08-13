using System;
using System.Data;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;
using SchoolSystem.Models;
using SchoolSystem.Helpers;
using SchoolSystem.Services;

namespace SchoolSystem.UI
{
    public partial class FeesForm : UserControl
    {
        private readonly FeeService feeService = new FeeService();
        private readonly StudentService studentService = new StudentService();
        private readonly VoucherService voucherService = new VoucherService();

        private int selectedFeeId = 0;
        private int? selectedFeePlanId = null;
        private decimal originalPaidAmount = 0;
        private DataTable allFees;
        private bool isLoading = false;

        public FeesForm()
        {
            InitializeComponent();
            Dock = DockStyle.Fill;
            Load += FeesForm_Load;
        }

        private async void FeesForm_Load(object sender, EventArgs e)
        {
            try
            {
                isLoading = true;

                InitializeLookups();

                await LoadStudentsAsync();
                await LoadFeesAsync();

                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء تحميل شاشة الرسوم:\n" + ex.Message);
            }
            finally
            {
                isLoading = false;
                CalculateAmounts();
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

            cmbStatus.Items.Clear();
            cmbStatus.Items.AddRange(new object[]
            {
                "غير مسدد",
                "مسدد جزئياً",
                "مسدد",
                "متأخر",
                "مؤجل",
                "معفى"
            });
            cmbStatus.SelectedIndex = 0;

            cmbFilterStatus.Items.Clear();
            cmbFilterStatus.Items.AddRange(new object[]
            {
                "كل الحالات",
                "غير مسدد",
                "مسدد جزئياً",
                "مسدد",
                "متأخر",
                "مؤجل",
                "معفى"
            });
            cmbFilterStatus.SelectedIndex = 0;

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

            cmbAcademicYear.Items.Clear();

            int year = DateTime.Today.Year;
            cmbAcademicYear.Items.Add((year - 1) + " / " + year);
            cmbAcademicYear.Items.Add(year + " / " + (year + 1));
            cmbAcademicYear.Items.Add((year + 1) + " / " + (year + 2));
            cmbAcademicYear.SelectedIndex = 1;
        }

        private async Task LoadStudentsAsync()
        {
            DataTable students = await Task.Run(() => studentService.GetAllStudents());

            cmbStudent.DataSource = students;
            cmbStudent.DisplayMember = "StudentName";
            cmbStudent.ValueMember = "StudentID";

            if (cmbStudent.Items.Count > 0)
                cmbStudent.SelectedIndex = 0;
        }

        private async Task LoadFeesAsync()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                allFees = await Task.Run(() => feeService.GetAllFees());

                ApplyFilter();
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void ApplyFilter()
        {
            if (allFees == null)
                return;

            DataView dv = allFees.DefaultView;

            string searchText = UIHelper.EscapeDataViewFilterValue(txtSearch.Text);
            string selectedStatus = cmbFilterStatus.SelectedItem == null
                ? "كل الحالات"
                : cmbFilterStatus.SelectedItem.ToString();

            string filter = "";

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                filter =
                    "(StudentName LIKE '%" + searchText + "%' " +
                    "OR AcademicYear LIKE '%" + searchText + "%' " +
                    "OR FeeType LIKE '%" + searchText + "%' " +
                    "OR ReceiptNumber LIKE '%" + searchText + "%' " +
                    "OR Status LIKE '%" + searchText + "%')";
            }

            if (selectedStatus != "كل الحالات")
            {
                if (!string.IsNullOrWhiteSpace(filter))
                    filter += " AND ";

                filter += "Status = '" + UIHelper.EscapeDataViewFilterValue(selectedStatus) + "'";
            }

            dv.RowFilter = filter;

            dataGridViewFees.DataSource = dv;
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
            if (dataGridViewFees.Columns.Count == 0)
                return;

            HideColumn("FeeID");
            HideColumn("StudentID");
            HideColumn("FeePlanID");
            HideColumn("CreatedAt");
            HideColumn("UpdatedAt");

            SetHeader("StudentName", "الطالب");
            SetHeader("AcademicYear", "العام الدراسي");
            SetHeader("FeeType", "نوع الرسوم");
            SetHeader("TotalAmount", "الإجمالي");
            SetHeader("DiscountAmount", "الخصم");
            SetHeader("NetAmount", "الصافي");
            SetHeader("PaidAmount", "المدفوع");
            SetHeader("RemainingAmount", "المتبقي");
            SetHeader("DueDate", "الاستحقاق");
            SetHeader("PaymentDate", "تاريخ الدفع");
            SetHeader("PaymentMethod", "طريقة الدفع");
            SetHeader("ReceiptNumber", "رقم السند");
            SetHeader("Status", "الحالة");
            SetHeader("Notes", "ملاحظات");

            FormatMoney("TotalAmount");
            FormatMoney("DiscountAmount");
            FormatMoney("NetAmount");
            FormatMoney("PaidAmount");
            FormatMoney("RemainingAmount");

            FormatDate("DueDate");
            FormatDate("PaymentDate");
        }

        private void HideColumn(string columnName)
        {
            if (dataGridViewFees.Columns.Contains(columnName))
                dataGridViewFees.Columns[columnName].Visible = false;
        }

        private void SetHeader(string columnName, string headerText)
        {
            if (dataGridViewFees.Columns.Contains(columnName))
                dataGridViewFees.Columns[columnName].HeaderText = headerText;
        }

        private void FormatMoney(string columnName)
        {
            if (dataGridViewFees.Columns.Contains(columnName))
                dataGridViewFees.Columns[columnName].DefaultCellStyle.Format = "N2";
        }

        private void FormatDate(string columnName)
        {
            if (dataGridViewFees.Columns.Contains(columnName))
                dataGridViewFees.Columns[columnName].DefaultCellStyle.Format = "dd/MM/yyyy";
        }

        private void FilterControls_Changed(object sender, EventArgs e)
        {
            if (!isLoading)
                ApplyFilter();
        }

        private void AmountFields_TextChanged(object sender, EventArgs e)
        {
            if (!isLoading)
                CalculateAmounts();
        }

        private void CalculateAmounts()
        {
            decimal total = ReadDecimal(txtTotalAmount.Text);
            decimal discount = ReadDecimal(txtDiscountAmount.Text);
            decimal paid = ReadDecimal(txtPaidAmount.Text);

            if (discount > total)
                discount = total;

            decimal net = total - discount;
            decimal remaining = net - paid;

            if (remaining < 0)
                remaining = 0;

            txtNetAmount.Text = net.ToString("N2");
            txtRemainingAmount.Text = remaining.ToString("N2");

            lblSummary.Text =
                $"الإجمالي: {total:N2} | الخصم: {discount:N2} | الصافي: {net:N2} | المدفوع: {paid:N2} | المتبقي: {remaining:N2}";

            if (net == 0)
            {
                SetStatus("معفى");
                dtpPaymentDate.Checked = false;
            }
            else if (paid == 0)
            {
                SetStatus(dtpDueDate.Value.Date < DateTime.Today ? "متأخر" : "غير مسدد");
                dtpPaymentDate.Checked = false;
            }
            else if (paid >= net)
            {
                SetStatus("مسدد");
                dtpPaymentDate.Checked = true;
            }
            else
            {
                SetStatus("مسدد جزئياً");
                dtpPaymentDate.Checked = true;
            }
        }

        private void SetStatus(string status)
        {
            if (cmbStatus.Items.Contains(status))
                cmbStatus.SelectedItem = status;
        }

        private decimal ReadDecimal(string text)
        {
            if (decimal.TryParse(text, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal value))
                return value;

            if (decimal.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out value))
                return value;

            return 0;
        }

        private void dataGridViewFees_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dataGridViewFees.Rows.Count == 0)
                return;

            DataRowView rowView = dataGridViewFees.Rows[e.RowIndex].DataBoundItem as DataRowView;

            if (rowView != null)
                FillFieldsFromRow(rowView.Row);
        }

        private void FillFieldsFromRow(DataRow row)
        {
            isLoading = true;

            selectedFeeId = Convert.ToInt32(row["FeeID"]);

            selectedFeePlanId = null;
            if (row.Table.Columns.Contains("FeePlanID") && row["FeePlanID"] != DBNull.Value)
                selectedFeePlanId = Convert.ToInt32(row["FeePlanID"]);

            cmbStudent.SelectedValue = Convert.ToInt32(row["StudentID"]);
            cmbAcademicYear.Text = row["AcademicYear"].ToString();
            cmbFeeType.Text = row["FeeType"].ToString();

            txtTotalAmount.Text = Convert.ToDecimal(row["TotalAmount"]).ToString("N2");
            txtDiscountAmount.Text = Convert.ToDecimal(row["DiscountAmount"]).ToString("N2");
            txtNetAmount.Text = Convert.ToDecimal(row["NetAmount"]).ToString("N2");
            txtPaidAmount.Text = Convert.ToDecimal(row["PaidAmount"]).ToString("N2");
            txtRemainingAmount.Text = Convert.ToDecimal(row["RemainingAmount"]).ToString("N2");

            originalPaidAmount = Convert.ToDecimal(row["PaidAmount"]);

            dtpDueDate.Value = Convert.ToDateTime(row["DueDate"]);

            if (row["PaymentDate"] != DBNull.Value)
            {
                dtpPaymentDate.Checked = true;
                dtpPaymentDate.Value = Convert.ToDateTime(row["PaymentDate"]);
            }
            else
            {
                dtpPaymentDate.Checked = false;
            }

            cmbPaymentMethod.Text = row["PaymentMethod"] == DBNull.Value ? "نقداً" : row["PaymentMethod"].ToString();
            txtReceiptNumber.Text = row["ReceiptNumber"] == DBNull.Value ? "" : row["ReceiptNumber"].ToString();

            cmbStatus.Text = row["Status"].ToString();
            txtNotes.Text = row["Notes"] == DBNull.Value ? "" : row["Notes"].ToString();

            isLoading = false;

            CalculateAmounts();
        }

        private void ClearInputs()
        {
            isLoading = true;

            selectedFeeId = 0;
            selectedFeePlanId = null;
            originalPaidAmount = 0;

            if (cmbStudent.Items.Count > 0)
                cmbStudent.SelectedIndex = 0;

            if (cmbAcademicYear.Items.Count > 0)
                cmbAcademicYear.SelectedIndex = 1;

            if (cmbFeeType.Items.Count > 0)
                cmbFeeType.SelectedIndex = 1;

            txtTotalAmount.Text = "0";
            txtDiscountAmount.Text = "0";
            txtNetAmount.Text = "0";
            txtPaidAmount.Text = "0";
            txtRemainingAmount.Text = "0";

            dtpDueDate.Value = DateTime.Today.AddDays(30);

            dtpPaymentDate.Value = DateTime.Today;
            dtpPaymentDate.Checked = false;

            if (cmbPaymentMethod.Items.Count > 0)
                cmbPaymentMethod.SelectedIndex = 0;

            txtReceiptNumber.Clear();

            if (cmbStatus.Items.Count > 0)
                cmbStatus.SelectedIndex = 0;

            txtNotes.Clear();

            isLoading = false;

            CalculateAmounts();
        }

        private bool ValidateInputs()
        {
            if (cmbStudent.SelectedValue == null)
            {
                MessageBox.Show("يرجى اختيار الطالب.");
                cmbStudent.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(cmbAcademicYear.Text))
            {
                MessageBox.Show("يرجى اختيار العام الدراسي.");
                cmbAcademicYear.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(cmbFeeType.Text))
            {
                MessageBox.Show("يرجى اختيار نوع الرسوم.");
                cmbFeeType.Focus();
                return false;
            }

            decimal total = ReadDecimal(txtTotalAmount.Text);
            decimal discount = ReadDecimal(txtDiscountAmount.Text);
            decimal paid = ReadDecimal(txtPaidAmount.Text);

            if (total < 0 || discount < 0 || paid < 0)
            {
                MessageBox.Show("المبالغ يجب أن تكون موجبة.");
                return false;
            }

            if (discount > total)
            {
                MessageBox.Show("الخصم لا يمكن أن يكون أكبر من إجمالي الرسوم.");
                txtDiscountAmount.Focus();
                return false;
            }

            if (paid > total - discount)
            {
                MessageBox.Show("المبلغ المدفوع لا يمكن أن يكون أكبر من صافي الرسوم.");
                txtPaidAmount.Focus();
                return false;
            }

            return true;
        }

        private Fee GetFeeFromInputs()
        {
            CalculateAmounts();

            return new Fee
            {
                FeeID = selectedFeeId,
                StudentID = Convert.ToInt32(cmbStudent.SelectedValue),
                StudentName = cmbStudent.Text,

                FeePlanID = selectedFeePlanId,

                AcademicYear = cmbAcademicYear.Text.Trim(),
                FeeType = cmbFeeType.Text.Trim(),

                TotalAmount = ReadDecimal(txtTotalAmount.Text),
                DiscountAmount = ReadDecimal(txtDiscountAmount.Text),
                NetAmount = ReadDecimal(txtNetAmount.Text),
                PaidAmount = ReadDecimal(txtPaidAmount.Text),
                RemainingAmount = ReadDecimal(txtRemainingAmount.Text),

                DueDate = dtpDueDate.Value.Date,
                PaymentDate = dtpPaymentDate.Checked ? dtpPaymentDate.Value.Date : (DateTime?)null,

                PaymentMethod = cmbPaymentMethod.Text.Trim(),
                ReceiptNumber = txtReceiptNumber.Text.Trim(),

                Status = cmbStatus.Text.Trim(),
                Notes = txtNotes.Text.Trim()
            };
        }

        private async Task CreateReceiptVoucherIfNeededAsync(int feeId, decimal paymentAmount, string studentName, string paymentMethod)
        {
            if (paymentAmount <= 0)
                return;

            DateTime paymentDate = dtpPaymentDate.Checked ? dtpPaymentDate.Value.Date : DateTime.Today;

            string notes =
                "تم إنشاء سند قبض تلقائياً من شاشة الرسوم الدراسية." + Environment.NewLine +
                "نوع الرسوم: " + cmbFeeType.Text + Environment.NewLine +
                "العام الدراسي: " + cmbAcademicYear.Text;

            await Task.Run(() =>
                voucherService.CreateReceiptVoucherForFeePayment(
                    paymentAmount,
                    paymentDate,
                    studentName,
                    feeId,
                    paymentMethod,
                    notes
                )
            );
        }

        private async void btnGenerateFees_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbStudent.SelectedValue == null)
                {
                    MessageBox.Show("اختر الطالب أولاً.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(cmbAcademicYear.Text))
                {
                    MessageBox.Show("اختر العام الدراسي.");
                    return;
                }

                int studentId = Convert.ToInt32(cmbStudent.SelectedValue);
                string academicYear = cmbAcademicYear.Text.Trim();

                int count = await Task.Run(() =>
                    feeService.GenerateStudentFeesFromPlans(studentId, academicYear)
                );

                MessageBox.Show("تم توليد عدد " + count + " رسوم للطالب.");

                await LoadFeesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("فشل توليد رسوم الطالب:\n" + ex.Message);
            }
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateInputs())
                    return;

                Fee fee = GetFeeFromInputs();

                int newFeeId = await Task.Run(() => feeService.AddFee(fee));

                decimal paidAmount = fee.PaidAmount;

                if (paidAmount > 0)
                {
                    await CreateReceiptVoucherIfNeededAsync(
                        newFeeId,
                        paidAmount,
                        cmbStudent.Text,
                        cmbPaymentMethod.Text
                    );
                }

                MessageBox.Show("تمت إضافة الرسوم بنجاح.");

                await LoadFeesAsync();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("فشل إضافة الرسوم:\n" + ex.Message);
            }
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedFeeId == 0)
                {
                    MessageBox.Show("اختر سجل الرسوم من الجدول أولاً.");
                    return;
                }

                if (!ValidateInputs())
                    return;

                Fee fee = GetFeeFromInputs();

                decimal newPaidAmount = fee.PaidAmount;
                decimal paymentDifference = newPaidAmount - originalPaidAmount;

                bool result = await Task.Run(() => feeService.UpdateFee(fee));

                if (result && paymentDifference > 0)
                {
                    await CreateReceiptVoucherIfNeededAsync(
                        selectedFeeId,
                        paymentDifference,
                        cmbStudent.Text,
                        cmbPaymentMethod.Text
                    );
                }

                MessageBox.Show(result ? "تم تعديل الرسوم بنجاح." : "لم يتم تعديل الرسوم.");

                await LoadFeesAsync();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("فشل تعديل الرسوم:\n" + ex.Message);
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedFeeId == 0)
                {
                    MessageBox.Show("اختر سجل الرسوم من الجدول أولاً.");
                    return;
                }

                DialogResult confirm = MessageBox.Show(
                    "هل أنت متأكد من حذف سجل الرسوم المحدد؟",
                    "تأكيد الحذف",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirm != DialogResult.Yes)
                    return;

                bool result = await Task.Run(() => feeService.DeleteFee(selectedFeeId));

                MessageBox.Show(result ? "تم حذف الرسوم بنجاح." : "لم يتم حذف الرسوم.");

                await LoadFeesAsync();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("فشل حذف الرسوم:\n" + ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearInputs();
        }
    }
}
