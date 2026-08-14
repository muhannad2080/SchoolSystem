using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;
using SchoolSystem.Models;
using SchoolSystem.Services;
using SchoolSystem.Helpers;

namespace SchoolSystem.UI
{
    public partial class EnrollmentForm : Form
    {
        private readonly EnrollmentService enrollmentService;
        private readonly StudentService studentService; 
        private readonly ClassService classService;
        private DataTable enrollmentsTable;
        private DataView enrollmentsView;
        private bool isEditMode = false;
        private bool isLoading = true;
        private bool printingReceipt = false;
        private readonly PrintDocument enrollmentPrintDocument = new PrintDocument();

        public EnrollmentForm()
        {
            InitializeComponent();
            SchoolSystem.Helpers.UIHelper.ApplyStyle(this);
            enrollmentService = new EnrollmentService();
            studentService = new StudentService();
            classService = new ClassService();
            enrollmentPrintDocument.PrintPage += enrollmentPrintDocument_PrintPage;
        }

        private void EnrollmentForm_Load(object sender, EventArgs e)
        {
            LoadComboBoxes();
            LoadData();
            DisableInputs();
            isLoading = false;
        }

        private void LoadComboBoxes()
        {
            try
            {
                // Load Students
                var dtStudents = studentService.GetAllStudents();
                cmbStudentID.DataSource = dtStudents;
                cmbStudentID.DisplayMember = "FullName";
                cmbStudentID.ValueMember = "StudentID";
                cmbStudentID.SelectedIndex = -1;

                // Load Classes
                var dtClasses = classService.GetAllClasses();
                if(dtClasses != null && dtClasses.Rows.Count > 0)
                {
                    cmbClassID.DataSource = dtClasses;
                    cmbClassID.DisplayMember = "ClassName";
                    cmbClassID.ValueMember = "ClassID";
                    cmbClassID.SelectedIndex = -1;
                }

                cmbApplicationType.Items.Clear();
                cmbApplicationType.Items.AddRange(new string[] { "طالب جديد", "منقول", "إعادة قيد" });
                
                cmbStatus.Items.Clear();
                cmbStatus.Items.AddRange(new string[] { "جديد", "مقبول", "مؤجل", "مرفوض" });

                cmbPaymentMethod.Items.Clear();
                cmbPaymentMethod.Items.AddRange(new string[] { "نقدي", "حوالة", "تحويل بنكي" });
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("تحميل القوائم", ex);
            }
        }

        private void LoadData()
        {
            try
            {
                enrollmentsTable = enrollmentService.GetAllEnrollments();
                if(enrollmentsTable != null)
                {
                    enrollmentsView = enrollmentsTable.DefaultView;
                    dgvEnrollments.DataSource = enrollmentsView;
                    FormatGrid();
                    UpdateCount();
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("تحميل بيانات التسجيل", ex);
            }
        }

        private void FormatGrid()
        {
            if (dgvEnrollments.Columns.Count == 0) return;

            foreach (DataGridViewColumn col in dgvEnrollments.Columns)
                col.Visible = false;

            ShowColumn("EnrollmentID", "رقم الطلب");
            ShowColumn("StudentName", "اسم الطالب");
            ShowColumn("ClassName",   "الصف");
            ShowColumn("Section",     "الشعبة");
            ShowColumn("AcademicYear","العام الدراسي");
            ShowColumn("Status",      "الحالة");
            ShowColumn("RegistrationFee", "الرسوم");
            ShowColumn("PaidAmount",  "المدفوع");
            ShowColumn("RemainingAmount", "المتبقي");
            ShowColumn("ApplicationDate", "تاريخ التسجيل");
        }

        private void ShowColumn(string name, string header)
        {
            if (dgvEnrollments.Columns.Contains(name))
            {
                dgvEnrollments.Columns[name].Visible = true;
                dgvEnrollments.Columns[name].HeaderText = header;
            }
        }

        private void UpdateCount()
        {
            lblCount.Text = $"العدد: {enrollmentsView.Count}";
        }

        private void cmbStudentID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoading || cmbStudentID.SelectedIndex == -1) return;

            var drv = cmbStudentID.SelectedItem as DataRowView;
            if (drv != null)
            {
                txtStudentName.Text = drv["FullName"].ToString();
            }
        }

        private void txtFees_TextChanged(object sender, EventArgs e)
        {
            CalculateRemaining();
        }

        private void CalculateRemaining()
        {
            decimal fee = 0;
            decimal paid = 0;

            decimal.TryParse(txtRegistrationFee.Text, out fee);
            decimal.TryParse(txtPaidAmount.Text, out paid);

            txtRemainingAmount.Text = (fee - paid).ToString();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim().Replace("'", "''");
            if (string.IsNullOrEmpty(keyword))
            {
                enrollmentsView.RowFilter = "";
            }
            else
            {
                enrollmentsView.RowFilter = $@"
                    Convert(EnrollmentID, 'System.String') LIKE '%{keyword}%' OR 
                    StudentName LIKE '%{keyword}%' OR 
                    Convert(StudentID, 'System.String') LIKE '%{keyword}%' OR 
                    AcademicYear LIKE '%{keyword}%' OR 
                    Status LIKE '%{keyword}%'";
            }
            UpdateCount();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch_Click(sender, e);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            enrollmentsView.RowFilter = "";
            LoadData();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            isEditMode = false;
            ClearInputs();
            EnableInputs();
            txtEnrollmentID.Text = "جديد";
            dtpApplicationDate.Value = DateTime.Today;
            cmbStatus.SelectedItem = "جديد";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            if (!ValidateInputs()) return;

            try
            {
                Enrollment enrollment = new Enrollment
                {
                    StudentID = Convert.ToInt32(cmbStudentID.SelectedValue),
                    ApplicationDate = dtpApplicationDate.Value,
                    ApplicationType = cmbApplicationType.SelectedItem?.ToString(),
                    AcademicYear = txtAcademicYear.Text,
                    ClassID = Convert.ToInt32(cmbClassID.SelectedValue),
                    Section = txtSection.Text,
                    SeatNumber = txtSeatNumber.Text,
                    Status = cmbStatus.SelectedItem?.ToString(),
                    
                    PreviousSchool = txtPreviousSchool.Text,
                    PreviousClass = txtPreviousClass.Text,
                    TransferReason = txtTransferReason.Text,
                    
                    RegistrationFee = string.IsNullOrEmpty(txtRegistrationFee.Text) ? 0 : Convert.ToDecimal(txtRegistrationFee.Text),
                    PaidAmount = string.IsNullOrEmpty(txtPaidAmount.Text) ? 0 : Convert.ToDecimal(txtPaidAmount.Text),
                    PaymentMethod = cmbPaymentMethod.SelectedItem?.ToString(),
                    ReceiptNo = txtReceiptNo.Text,
                    
                    HasBirthCertificate = chkHasBirthCertificate.Checked,
                    HasGuardianId = chkHasGuardianId.Checked,
                    HasPhoto = chkHasPhoto.Checked,
                    HasLastCertificate = chkHasLastCertificate.Checked,
                    HasMedicalReport = chkHasMedicalReport.Checked,
                    
                    Notes = rtbNotes.Text
                };

                bool success;
                if (isEditMode)
                {
                    enrollment.EnrollmentID = Convert.ToInt32(txtEnrollmentID.Text);
                    success = enrollmentService.UpdateEnrollment(enrollment);
                }
                else
                {
                    success = enrollmentService.AddEnrollment(enrollment);
                }

                if (success)
                {
                    MessageBox.Show("تم الحفظ بنجاح.", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    DisableInputs();
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("حفظ التسجيل", ex);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtEnrollmentID.Text) || txtEnrollmentID.Text == "جديد")
            {
                MessageBox.Show("الرجاء تحديد طلب من الجدول أولاً.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            isEditMode = true;
            EnableInputs();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtEnrollmentID.Text) || txtEnrollmentID.Text == "جديد")
            {
                MessageBox.Show("الرجاء تحديد طلب من الجدول أولاً.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("هل أنت متأكد من حذف هذا الطلب؟", "تأكيد", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    int id = Convert.ToInt32(txtEnrollmentID.Text);
                    if (enrollmentService.DeleteEnrollment(id))
                    {
                        MessageBox.Show("تم الحذف بنجاح.", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                        ClearInputs();
                        DisableInputs();
                    }
                }
                catch (Exception ex)
                {
                    UIHelper.ShowException("حذف التسجيل", ex);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearInputs();
            DisableInputs();
            isEditMode = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private bool CanPrintSelectedEnrollment()
        {
            if (string.IsNullOrWhiteSpace(txtEnrollmentID.Text) || txtEnrollmentID.Text == "جديد")
            {
                MessageBox.Show("رجاءً حدد طلبًا قبل الطباعة.", "تنبيه", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                return false;
            }
            return true;
        }

        private void PrintSelectedEnrollment(bool receipt)
        {
            if (!CanPrintSelectedEnrollment())
                return;

            printingReceipt = receipt;
            using (PrintDialog dialog = new PrintDialog())
            {
                dialog.Document = enrollmentPrintDocument;
                dialog.UseEXDialog = true;
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        enrollmentPrintDocument.Print();
                    }
                    catch (Exception ex)
                    {
                        UIHelper.ShowException("طباعة تقرير التسجيل", ex);
                    }
                }
            }
        }

        private void btnPrintForm_Click(object sender, EventArgs e)
        {
            PrintSelectedEnrollment(false);
        }

        private void btnPrintReceipt_Click(object sender, EventArgs e)
        {
            PrintSelectedEnrollment(true);
        }

        private void enrollmentPrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            Rectangle bounds = e.MarginBounds;
            using (Font titleFont = new Font("Tahoma", 16F, FontStyle.Bold))
            using (Font labelFont = new Font("Tahoma", 10F, FontStyle.Bold))
            using (Font valueFont = new Font("Tahoma", 10F, FontStyle.Regular))
            using (StringFormat rtl = new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center, FormatFlags = StringFormatFlags.DirectionRightToLeft })
            {
                int y = bounds.Top;
                e.Graphics.DrawString(printingReceipt ? "إيصال تسجيل طالب" : "استمارة تسجيل طالب", titleFont, Brushes.Black,
                    new RectangleF(bounds.Left, y, bounds.Width, 40), rtl);
                y += 55;

                DrawPrintLine(e.Graphics, bounds, ref y, "رقم الطلب", txtEnrollmentID.Text, labelFont, valueFont, rtl);
                DrawPrintLine(e.Graphics, bounds, ref y, "اسم الطالب", txtStudentName.Text, labelFont, valueFont, rtl);
                DrawPrintLine(e.Graphics, bounds, ref y, "العام الدراسي", txtAcademicYear.Text, labelFont, valueFont, rtl);
                DrawPrintLine(e.Graphics, bounds, ref y, "الصف", cmbClassID.Text, labelFont, valueFont, rtl);
                DrawPrintLine(e.Graphics, bounds, ref y, "الشعبة", txtSection.Text, labelFont, valueFont, rtl);
                DrawPrintLine(e.Graphics, bounds, ref y, "تاريخ التسجيل", dtpApplicationDate.Value.ToString("yyyy/MM/dd"), labelFont, valueFont, rtl);
                DrawPrintLine(e.Graphics, bounds, ref y, "الحالة", cmbStatus.Text, labelFont, valueFont, rtl);

                if (printingReceipt)
                {
                    DrawPrintLine(e.Graphics, bounds, ref y, "رسوم التسجيل", txtRegistrationFee.Text, labelFont, valueFont, rtl);
                    DrawPrintLine(e.Graphics, bounds, ref y, "المبلغ المدفوع", txtPaidAmount.Text, labelFont, valueFont, rtl);
                    DrawPrintLine(e.Graphics, bounds, ref y, "المتبقي", txtRemainingAmount.Text, labelFont, valueFont, rtl);
                    DrawPrintLine(e.Graphics, bounds, ref y, "طريقة الدفع", cmbPaymentMethod.Text, labelFont, valueFont, rtl);
                    DrawPrintLine(e.Graphics, bounds, ref y, "رقم السند", txtReceiptNo.Text, labelFont, valueFont, rtl);
                }

                y += 20;
                e.Graphics.DrawString("التوقيع: ____________________", valueFont, Brushes.Black,
                    new RectangleF(bounds.Left, y, bounds.Width, 30), rtl);
            }
        }

        private static void DrawPrintLine(Graphics graphics, Rectangle bounds, ref int y, string label,
            string value, Font labelFont, Font valueFont, StringFormat rtl)
        {
            graphics.DrawString(label + ":", labelFont, Brushes.Black,
                new RectangleF(bounds.Left, y, bounds.Width * 0.35F, 28), rtl);
            graphics.DrawString(value ?? "", valueFont, Brushes.Black,
                new RectangleF(bounds.Left + bounds.Width * 0.35F, y, bounds.Width * 0.65F, 28), rtl);
            y += 32;
        }

        private void dgvEnrollments_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                LoadRecordToScreen(dgvEnrollments.Rows[e.RowIndex]);
            }
        }

        private void dgvEnrollments_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvEnrollments.CurrentRow != null)
            {
                LoadRecordToScreen(dgvEnrollments.CurrentRow);
            }
        }

        // ─── Safe date assignment for DateTimePicker ─────────────────────────────
        private static readonly DateTime DtpMin = new DateTime(1900, 4, 30);
        private static readonly DateTime DtpMax = new DateTime(2077, 11, 16);

        private void SafeSetDate(DateTimePicker dtp, object value)
        {
            try
            {
                if (value == null || value == DBNull.Value) { dtp.Value = DateTime.Today; return; }
                DateTime dt = Convert.ToDateTime(value);
                if (dt < DtpMin || dt > DtpMax) dt = DateTime.Today;
                dtp.Value = dt;
            }
            catch { dtp.Value = DateTime.Today; }
        }

        private string SafeCell(DataGridViewRow row, string colName)
        {
            if (!row.DataGridView.Columns.Contains(colName)) return "";
            return row.Cells[colName].Value?.ToString() ?? "";
        }

        private bool SafeBoolCell(DataGridViewRow row, string colName)
        {
            if (!row.DataGridView.Columns.Contains(colName)) return false;
            var v = row.Cells[colName].Value;
            return v != DBNull.Value && v != null && Convert.ToBoolean(v);
        }

        private void LoadRecordToScreen(DataGridViewRow row)
        {
            if (isEditMode) return;

            isLoading = true;
            try
            {
                txtEnrollmentID.Text  = SafeCell(row, "EnrollmentID");
                txtStudentName.Text   = SafeCell(row, "StudentName");

                if (row.DataGridView.Columns.Contains("StudentID"))
                    cmbStudentID.SelectedValue = row.Cells["StudentID"].Value;

                SafeSetDate(dtpApplicationDate, row.DataGridView.Columns.Contains("ApplicationDate")
                    ? row.Cells["ApplicationDate"].Value : null);

                cmbApplicationType.SelectedItem = SafeCell(row, "ApplicationType");
                txtAcademicYear.Text = SafeCell(row, "AcademicYear");

                if (row.DataGridView.Columns.Contains("ClassID"))
                    cmbClassID.SelectedValue = row.Cells["ClassID"].Value;

                txtSection.Text      = SafeCell(row, "Section");
                txtSeatNumber.Text   = SafeCell(row, "SeatNumber");
                cmbStatus.SelectedItem = SafeCell(row, "Status");

                txtPreviousSchool.Text  = SafeCell(row, "PreviousSchool");
                txtPreviousClass.Text   = SafeCell(row, "PreviousClass");
                txtTransferReason.Text  = SafeCell(row, "TransferReason");

                txtRegistrationFee.Text = SafeCell(row, "RegistrationFee");
                txtPaidAmount.Text      = SafeCell(row, "PaidAmount");
                cmbPaymentMethod.SelectedItem = SafeCell(row, "PaymentMethod");
                txtReceiptNo.Text       = SafeCell(row, "ReceiptNo");

                chkHasBirthCertificate.Checked = SafeBoolCell(row, "HasBirthCertificate");
                chkHasGuardianId.Checked       = SafeBoolCell(row, "HasGuardianId");
                chkHasPhoto.Checked            = SafeBoolCell(row, "HasPhoto");
                chkHasLastCertificate.Checked  = SafeBoolCell(row, "HasLastCertificate");
                chkHasMedicalReport.Checked    = SafeBoolCell(row, "HasMedicalReport");

                rtbNotes.Text = SafeCell(row, "Notes");

                CalculateRemaining();
                DisableInputs();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("LoadRecordToScreen error: " + ex.Message);
            }
            finally
            {
                isLoading = false;
            }
        }

        private bool ValidateInputs()
        {
            bool isValid = true;

            if (cmbStudentID.SelectedIndex == -1)
            {
                errorProvider1.SetError(cmbStudentID, "يجب تحديد الطالب.");
                isValid = false;
            }

            if (cmbClassID.SelectedIndex == -1)
            {
                errorProvider1.SetError(cmbClassID, "يجب تحديد الصف.");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(txtAcademicYear.Text))
            {
                errorProvider1.SetError(txtAcademicYear, "العام الدراسي مطلوب.");
                isValid = false;
            }

            if (cmbApplicationType.SelectedIndex == -1)
            {
                errorProvider1.SetError(cmbApplicationType, "يجب تحديد نوع التسجيل.");
                isValid = false;
            }

            if (cmbStatus.SelectedIndex == -1)
            {
                errorProvider1.SetError(cmbStatus, "يجب تحديد حالة الطلب.");
                isValid = false;
            }

            decimal fee = 0;
            if (!string.IsNullOrWhiteSpace(txtRegistrationFee.Text) &&
                (!UIHelper.TryParseDecimal(txtRegistrationFee.Text, out fee) || fee < 0))
            {
                errorProvider1.SetError(txtRegistrationFee, "أدخل مبلغاً رقمياً غير سالب.");
                isValid = false;
            }

            decimal paid = 0;
            if (!string.IsNullOrWhiteSpace(txtPaidAmount.Text) &&
                (!UIHelper.TryParseDecimal(txtPaidAmount.Text, out paid) || paid < 0))
            {
                errorProvider1.SetError(txtPaidAmount, "أدخل مبلغاً رقمياً غير سالب.");
                isValid = false;
            }

            if (isValid && paid > fee && fee > 0)
            {
                errorProvider1.SetError(txtPaidAmount, "لا يجوز أن يتجاوز المدفوع قيمة رسوم التسجيل.");
                isValid = false;
            }

            return isValid;
        }

        private void DisableInputs()
        {
            foreach (Control c in tlpBasic.Controls) { if (c is TextBox || c is ComboBox || c is DateTimePicker) c.Enabled = false; }
            foreach (Control c in tlpPrevious.Controls) { if (c is TextBox || c is ComboBox) c.Enabled = false; }
            foreach (Control c in tlpFees.Controls) { if (c is TextBox || c is ComboBox) c.Enabled = false; }
            foreach (Control c in flpAttachments.Controls) { if (c is CheckBox) c.Enabled = false; }
            rtbNotes.Enabled = false;

            btnSave.Enabled = false;
            btnAdd.Enabled = true;
            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;
        }

        private void EnableInputs()
        {
            foreach (Control c in tlpBasic.Controls) { if (c is TextBox || c is ComboBox || c is DateTimePicker) c.Enabled = true; }
            foreach (Control c in tlpPrevious.Controls) { if (c is TextBox || c is ComboBox) c.Enabled = true; }
            foreach (Control c in tlpFees.Controls) { if (c is TextBox || c is ComboBox) c.Enabled = true; }
            foreach (Control c in flpAttachments.Controls) { if (c is CheckBox) c.Enabled = true; }
            rtbNotes.Enabled = true;

            txtEnrollmentID.Enabled = false;
            txtStudentName.Enabled = false;
            txtRemainingAmount.Enabled = false;

            btnSave.Enabled = true;
            btnAdd.Enabled = false;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
        }

        private void ClearInputs()
        {
            isLoading = true;
            txtEnrollmentID.Clear();
            cmbStudentID.SelectedIndex = -1;
            txtStudentName.Clear();
            dtpApplicationDate.Value = DateTime.Today;
            cmbApplicationType.SelectedIndex = -1;
            txtAcademicYear.Clear();
            cmbClassID.SelectedIndex = -1;
            txtSection.Clear();
            txtSeatNumber.Clear();
            cmbStatus.SelectedIndex = -1;
            
            txtPreviousSchool.Clear();
            txtPreviousClass.Clear();
            txtTransferReason.Clear();
            
            txtRegistrationFee.Clear();
            txtPaidAmount.Clear();
            txtRemainingAmount.Clear();
            cmbPaymentMethod.SelectedIndex = -1;
            txtReceiptNo.Clear();
            
            foreach (Control c in flpAttachments.Controls)
            {
                if (c is CheckBox chk) chk.Checked = false;
            }
            rtbNotes.Clear();
            isLoading = false;
        }
    }
}
