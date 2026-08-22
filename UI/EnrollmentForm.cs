using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using SchoolSystem.Models;
using SchoolSystem.Services;
using SchoolSystem.Helpers;
using SchoolSystem.Security;

namespace SchoolSystem.UI
{
    public partial class EnrollmentForm : Form
    {
        private readonly EnrollmentService enrollmentService;
        private readonly StudentService studentService; 
        private readonly ClassService classService;
        private readonly StudentClassService sectionService;
        private DataTable enrollmentsTable;
        private DataView enrollmentsView;
        private bool isEditMode = false;
        private bool isLoading = true;
        private bool printingReceipt = false;
        private bool isSaving;
        private readonly PrintDocument enrollmentPrintDocument = new PrintDocument();

        public EnrollmentForm()
        {
            InitializeComponent();
            txtSeatNumber.ReadOnly = true;
            txtSeatNumber.TabStop = false;
            txtAcademicYear.ReadOnly = true;
            txtSection.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbClassID.SelectedIndexChanged += cmbClassID_SelectedIndexChanged;
            txtAcademicYear.Leave += txtAcademicYear_Leave;
            dtpApplicationDate.ValueChanged += dtpApplicationDate_ValueChanged;
            SchoolSystem.Helpers.UIHelper.ApplyStyle(this);
            txtSearch.TextChanged += (sender, e) => btnSearch_Click(sender, e);
            enrollmentService = new EnrollmentService();
            studentService = new StudentService();
            classService = new ClassService();
            sectionService = new StudentClassService();
            enrollmentPrintDocument.PrintPage += enrollmentPrintDocument_PrintPage;
            ApplyCustomStyles();
            ConfigureOutputButtons();
        }

        private void ApplyCustomStyles()
        {
            UIHelper.StyleDataGridView(dgvEnrollments);
            UIHelper.StylePrimaryButton(btnAdd);
            UIHelper.StylePrimaryButton(btnSave);
            UIHelper.StylePrimaryButton(btnUpdate);
            UIHelper.StyleDangerButton(btnDelete);
            UIHelper.StyleButton(btnCancel, UIHelper.NeutralColor);
            UIHelper.StyleButton(btnRefresh, UIHelper.NeutralColor);
            UIHelper.StyleButton(btnReload, UIHelper.NeutralColor);
            UIHelper.StylePrimaryButton(btnSearch);
            UIHelper.StyleButton(btnPrintForm, UIHelper.InfoColor);
            UIHelper.StyleButton(btnPrintReceipt, UIHelper.SuccessColor);
            UIHelper.StyleButton(btnClose, UIHelper.NeutralColor);

            TextBox[] textBoxes = {
                txtEnrollmentID, txtStudentName, txtAcademicYear,
                txtSeatNumber, txtPreviousSchool, txtPreviousClass, txtTransferReason,
                txtRegistrationFee, txtPaidAmount, txtRemainingAmount, txtReceiptNo, txtSearch
            };
            foreach (TextBox textBox in textBoxes)
                UIHelper.StyleTextBox(textBox);

            ComboBox[] comboBoxes = {
                cmbStudentID, cmbApplicationType, cmbClassID, txtSection, cmbStatus, cmbPaymentMethod
            };
            foreach (ComboBox comboBox in comboBoxes)
                UIHelper.StyleComboBox(comboBox);

            rtbNotes.BackColor = UIHelper.SurfaceColor;
            rtbNotes.ForeColor = UIHelper.TextColor;
            rtbNotes.BorderStyle = BorderStyle.FixedSingle;
        }

        private void ConfigureOutputButtons()
        {
            UIHelper.StyleButton(btnPreviewOutput, UIHelper.InfoColor);
            UIHelper.StyleButton(btnExportPdf, UIHelper.DangerColor);
            UIHelper.StyleButton(btnExportExcel, UIHelper.SuccessColor);
            btnPreviewOutput.Click -= btnPreviewOutput_Click;
            btnExportPdf.Click -= btnExportPdf_Click;
            btnExportExcel.Click -= btnExportExcel_Click;
            btnPreviewOutput.Click += btnPreviewOutput_Click;
            btnExportPdf.Click += btnExportPdf_Click;
            btnExportExcel.Click += btnExportExcel_Click;
        }

        private void btnPreviewOutput_Click(object sender, EventArgs e)
        {
            PreviewSelectedEnrollment(false);
        }

        private void btnExportPdf_Click(object sender, EventArgs e)
        {
            ExportEnrollment(false, true);
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            ExportEnrollment(false, false);
        }

        private DataTable BuildEnrollmentOutputTable(bool receipt)
        {
            DataTable table = new DataTable();
            table.Columns.Add("الحقل | Field");
            table.Columns.Add("القيمة | Value");
            table.Rows.Add("رقم الطلب | Application No.", txtEnrollmentID.Text);
            table.Rows.Add("اسم الطالب | Student Name", txtStudentName.Text);
            table.Rows.Add("العام الدراسي | Academic Year", txtAcademicYear.Text);
            table.Rows.Add("الصف | Class", cmbClassID.Text);
            table.Rows.Add("الشعبة | Section", txtSection.Text);
            table.Rows.Add("تاريخ التسجيل | Registration Date", dtpApplicationDate.Value.ToString("yyyy/MM/dd"));
            table.Rows.Add("الحالة | Status", cmbStatus.Text);
            if (receipt)
            {
                table.Rows.Add("رسوم التسجيل | Registration Fee", txtRegistrationFee.Text);
                table.Rows.Add("المبلغ المدفوع | Paid Amount", txtPaidAmount.Text);
                table.Rows.Add("المتبقي | Remaining", txtRemainingAmount.Text);
                table.Rows.Add("طريقة الدفع | Payment Method", cmbPaymentMethod.Text);
                table.Rows.Add("رقم السند | Receipt No.", txtReceiptNo.Text);
            }
            return table;
        }

        private void PreviewSelectedEnrollment(bool receipt)
        {
            if (!CanPrintSelectedEnrollment()) return;
            printingReceipt = receipt;
            try
            {
                using (PrintPreviewDialog preview = new PrintPreviewDialog())
                {
                    preview.Document = enrollmentPrintDocument;
                    preview.RightToLeft = RightToLeft.Yes;
                    preview.WindowState = FormWindowState.Maximized;
                    preview.ShowDialog(this);
                }
            }
            catch (Exception ex) { UIHelper.ShowException("معاينة استمارة التسجيل", ex); }
        }

        private void ExportEnrollment(bool receipt, bool pdf)
        {
            if (!CanPrintSelectedEnrollment()) return;
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = pdf ? "ملفات PDF (*.pdf)|*.pdf" : "ملفات Excel (*.xlsx)|*.xlsx";
                dialog.FileName = (receipt ? "Enrollment_Receipt_" : "Enrollment_Form_") + DateTime.Now.ToString("yyyyMMdd_HHmm") + (pdf ? ".pdf" : ".xlsx");
                if (dialog.ShowDialog(this) != DialogResult.OK) return;
                try
                {
                    DataTable table = BuildEnrollmentOutputTable(receipt);
                    string title = (receipt ? "إيصال التسجيل | Enrollment Receipt" : "استمارة التسجيل | Enrollment Form");
                    if (pdf)
                        ReportOutputHelper.ExportToPdf(table, dialog.FileName, title, "رقم الطلب | Application No.: " + txtEnrollmentID.Text);
                    else
                        ReportOutputHelper.ExportToExcel(table, dialog.FileName, title, "رقم الطلب | Application No.: " + txtEnrollmentID.Text);
                    UIHelper.ShowInfo(pdf ? "تم تصدير استمارة التسجيل إلى PDF بنجاح." : "تم تصدير استمارة التسجيل إلى Excel بنجاح.");
                }
                catch (Exception ex) { UIHelper.ShowException("تصدير استمارة التسجيل", ex); }
            }
        }

        private async void EnrollmentForm_Load(object sender, EventArgs e)
        {
            LoadComboBoxes();
            LoadData();
            DisableInputs();
            isLoading = false;
            await LoadSectionsAsync();
        }

        private void SetAutomaticAcademicYear()
        {
            if (isEditMode) return;
            DateTime date = dtpApplicationDate.Value.Date;
            // يبدأ العام الدراسي في أغسطس، حتى تتطابق القائمة مع الشعب المزروعة للعام القادم.
            int startYear = date.Month >= 8 ? date.Year : date.Year - 1;
            txtAcademicYear.Text = startYear + "/" + (startYear + 1);
        }

        private async void dtpApplicationDate_ValueChanged(object sender, EventArgs e)
        {
            if (isLoading || isEditMode) return;
            SetAutomaticAcademicYear();
            await LoadSectionsAsync();
        }

        private void LoadComboBoxes()
        {
            try
            {
                SetAutomaticAcademicYear();
                // Load Students
                var dtStudents = studentService.GetActiveStudents();
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

                txtSection.DataSource = null;
                txtSection.Items.Clear();
                txtSection.Enabled = false;

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
            lblCount.Text = $"العدد: {(enrollmentsView == null ? 0 : enrollmentsView.Count)}";
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
            decimal fee = ParseAmountOrZero(txtRegistrationFee.Text);
            decimal paid = ParseAmountOrZero(txtPaidAmount.Text);
            decimal remaining = fee - paid;

            txtRemainingAmount.Text = remaining.ToString("0.##");
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            ApplyEnrollmentSearch();
        }

        private void ApplyEnrollmentSearch()
        {
            if (enrollmentsView == null)
            {
                UpdateCount();
                return;
            }

            // بحث فوري من أول حرف، مع دعم عدة كلمات في جميع حقول التسجيل.
            enrollmentsView.RowFilter = UIHelper.BuildDataViewSearchFilter(
                txtSearch.Text,
                "StudentName",
                "AcademicYear",
                "Status",
                "Section",
                "ClassName");
            UpdateCount();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch_Click(sender, e);
                e.SuppressKeyPress = true;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            if (enrollmentsView != null)
                enrollmentsView.RowFilter = "";
            LoadData();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            CurrentUser.DemandAction("Enrollment", "Add", "ليس لديك صلاحية بدء تسجيل جديد.");
            isEditMode = false;
            ClearInputs();
            EnableInputs();
            txtEnrollmentID.Text = "جديد";
            dtpApplicationDate.Value = DateTime.Today;
            cmbApplicationType.SelectedItem = "طالب جديد";
            cmbStatus.SelectedItem = "جديد";
            cmbStudentID.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (isSaving)
                return;

            isSaving = true;
            btnSave.Enabled = false;
            errorProvider1.Clear();
            if (!ValidateInputs())
            {
                isSaving = false;
                btnSave.Enabled = true;
                return;
            }

            try
            {
                CurrentUser.DemandAction("Enrollment", isEditMode ? "Edit" : "Add",
                    isEditMode ? "ليس لديك صلاحية تعديل التسجيل." : "ليس لديك صلاحية إضافة التسجيل.");

                Enrollment enrollment = new Enrollment
                {
                    StudentID = GetSelectedId(cmbStudentID, "يجب تحديد طالب صالح."),
                    ApplicationDate = dtpApplicationDate.Value,
                    ApplicationType = cmbApplicationType.SelectedItem?.ToString(),
                    AcademicYear = txtAcademicYear.Text,
                    ClassID = GetSelectedId(cmbClassID, "يجب تحديد فصل صالح."),
                    Section = txtSection.Text.Trim(),
                    SeatNumber = isEditMode ? txtSeatNumber.Text : string.Empty,
                    Status = cmbStatus.SelectedItem?.ToString(),
                    
                    PreviousSchool = txtPreviousSchool.Text,
                    PreviousClass = txtPreviousClass.Text,
                    TransferReason = txtTransferReason.Text,
                    
                    RegistrationFee = ParseAmountOrZero(txtRegistrationFee.Text),
                    PaidAmount = ParseAmountOrZero(txtPaidAmount.Text),
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
                    if (!int.TryParse(txtEnrollmentID.Text.Trim(), out int enrollmentId) || enrollmentId <= 0)
                    {
                        errorProvider1.SetError(txtEnrollmentID, "رقم طلب التسجيل غير صالح.");
                        return;
                    }

                    enrollment.EnrollmentID = enrollmentId;
                    success = enrollmentService.UpdateEnrollment(enrollment);
                }
                else
                {
                    success = enrollmentService.AddEnrollment(enrollment);
                }

                if (success)
                {
                    UIHelper.ShowInfo("تم الحفظ بنجاح.");
                    LoadData();
                    isEditMode = false;
                    DisableInputs();
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf("مسجل بالفعل", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    UIHelper.ShowWarning("لا يمكن حفظ التسجيل: هذا الطالب لديه تسجيل غير مرفوض في العام الدراسي " + txtAcademicYear.Text + ". اختر تسجيلاً جديداً لعام مختلف أو عدّل التسجيل الموجود.");
                }
                else
                {
                    UIHelper.ShowException("حفظ التسجيل", ex);
                }
            }
            finally
            {
                isSaving = false;
                btnSave.Enabled = true;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            CurrentUser.DemandAction("Enrollment", "Edit", "ليس لديك صلاحية تعديل التسجيل.");
            if (string.IsNullOrEmpty(txtEnrollmentID.Text) || txtEnrollmentID.Text == "جديد")
            {
                UIHelper.ShowWarning("الرجاء تحديد طلب من الجدول أولاً.");
                return;
            }
            isEditMode = true;
            EnableInputs();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            CurrentUser.DemandAction("Enrollment", "Delete", "ليس لديك صلاحية حذف التسجيل.");
            if (string.IsNullOrEmpty(txtEnrollmentID.Text) || txtEnrollmentID.Text == "جديد")
            {
                UIHelper.ShowWarning("الرجاء تحديد طلب من الجدول أولاً.");
                return;
            }

            if (MessageBox.Show("هل أنت متأكد من حذف هذا الطلب؟", "تأكيد", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    if (!int.TryParse(txtEnrollmentID.Text.Trim(), out int id) || id <= 0)
                    {
                        errorProvider1.SetError(txtEnrollmentID, "رقم طلب التسجيل غير صالح.");
                        return;
                    }

                    if (enrollmentService.DeleteEnrollment(id))
                    {
                        UIHelper.ShowInfo("تم الحذف بنجاح.");
                        LoadData();
                        ClearInputs();
                        isEditMode = false;
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
            CurrentUser.DemandAction("Enrollment", "Print", "ليس لديك صلاحية طباعة بيانات التسجيل.");
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
            bool isRtl = ReportOutputHelper.ContainsArabic(txtStudentName.Text)
                || ReportOutputHelper.ContainsArabic(cmbClassID.Text)
                || ReportOutputHelper.ContainsArabic(cmbStatus.Text);
            using (Font titleFont = new Font("Tahoma", 16F, FontStyle.Bold))
            using (Font labelFont = new Font("Tahoma", 10F, FontStyle.Bold))
            using (Font valueFont = new Font("Tahoma", 10F, FontStyle.Regular))
            using (StringFormat rtl = new StringFormat
            {
                Alignment = isRtl ? StringAlignment.Far : StringAlignment.Near,
                LineAlignment = StringAlignment.Center,
                FormatFlags = isRtl ? StringFormatFlags.DirectionRightToLeft : StringFormatFlags.FitBlackBox,
                Trimming = StringTrimming.EllipsisCharacter
            })
            {
                int y = bounds.Top;
                e.Graphics.DrawString(printingReceipt ? "إيصال تسجيل طالب" : "استمارة تسجيل طالب", titleFont, Brushes.Black,
                    new RectangleF(bounds.Left, y, bounds.Width, 40), rtl);
                y += 55;

                DrawPrintLine(e.Graphics, bounds, ref y, "رقم الطلب | Application No.", txtEnrollmentID.Text, labelFont, valueFont, rtl);
                DrawPrintLine(e.Graphics, bounds, ref y, "اسم الطالب | Student Name", txtStudentName.Text, labelFont, valueFont, rtl);
                DrawPrintLine(e.Graphics, bounds, ref y, "العام الدراسي | Academic Year", txtAcademicYear.Text, labelFont, valueFont, rtl);
                DrawPrintLine(e.Graphics, bounds, ref y, "الصف | Class", cmbClassID.Text, labelFont, valueFont, rtl);
                DrawPrintLine(e.Graphics, bounds, ref y, "الشعبة | Section", txtSection.Text, labelFont, valueFont, rtl);
                DrawPrintLine(e.Graphics, bounds, ref y, "تاريخ التسجيل | Registration Date", dtpApplicationDate.Value.ToString("yyyy/MM/dd"), labelFont, valueFont, rtl);
                DrawPrintLine(e.Graphics, bounds, ref y, "الحالة | Status", cmbStatus.Text, labelFont, valueFont, rtl);

                if (printingReceipt)
                {
                    DrawPrintLine(e.Graphics, bounds, ref y, "رسوم التسجيل | Registration Fee", txtRegistrationFee.Text, labelFont, valueFont, rtl);
                    DrawPrintLine(e.Graphics, bounds, ref y, "المبلغ المدفوع | Paid Amount", txtPaidAmount.Text, labelFont, valueFont, rtl);
                    DrawPrintLine(e.Graphics, bounds, ref y, "المتبقي | Remaining", txtRemainingAmount.Text, labelFont, valueFont, rtl);
                    DrawPrintLine(e.Graphics, bounds, ref y, "طريقة الدفع | Payment Method", cmbPaymentMethod.Text, labelFont, valueFont, rtl);
                    DrawPrintLine(e.Graphics, bounds, ref y, "رقم السند | Receipt No.", txtReceiptNo.Text, labelFont, valueFont, rtl);
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
            if (dgvEnrollments.CurrentRow != null && !dgvEnrollments.CurrentRow.IsNewRow)
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

        private async void cmbClassID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoading)
                return;

            await LoadSectionsAsync();
        }

        private async void txtAcademicYear_Leave(object sender, EventArgs e)
        {
            if (isLoading)
                return;

            await LoadSectionsAsync();
        }

        private async Task LoadSectionsAsync()
        {
            txtSection.DataSource = null;
            txtSection.Items.Clear();
            txtSection.Enabled = false;

            int classId = TryGetSelectedId(cmbClassID);
            string academicYear = txtAcademicYear.Text == null ? string.Empty : txtAcademicYear.Text.Trim();
            if (classId <= 0 || !IsSequentialAcademicYear(academicYear))
                return;

            try
            {
                DataTable sections = await Task.Run(() => sectionService.GetSections(classId, academicYear));
                if (sections == null || sections.Rows.Count == 0)
                    return;

                // لا تعرض أي سجل فارغ حتى لو كانت قاعدة البيانات تحتوي على بيانات قديمة غير صحيحة.
                DataTable validSections = sections.Clone();
                foreach (DataRow row in sections.Rows)
                {
                    string sectionName = row["Section"] == DBNull.Value ? string.Empty : Convert.ToString(row["Section"]);
                    if (!string.IsNullOrWhiteSpace(sectionName))
                    {
                        DataRow cleanRow = validSections.NewRow();
                        cleanRow["Section"] = sectionName.Trim();
                        validSections.Rows.Add(cleanRow);
                    }
                }

                // الشعبة اختيارية في مرحلة القبول؛ يمكن توزيع الطالب لاحقًا من واجهة التوزيع.
                if (validSections.Rows.Count == 0)
                {
                    validSections.Rows.Add(string.Empty);
                }

                txtSection.DataSource = validSections;
                txtSection.DisplayMember = "Section";
                txtSection.ValueMember = "Section";
                txtSection.Enabled = true;
                if (txtSection.Items.Count > 0)
                    txtSection.SelectedIndex = 0;

                await PreviewNextSeatNumberAsync();
            }
            catch (Exception ex)
            {
                txtSection.DataSource = null;
                txtSection.Items.Clear();
                txtSection.Enabled = false;
                UIHelper.ShowException("تحميل شعب التسجيل", ex);
            }
        }

        private async Task PreviewNextSeatNumberAsync()
        {
            if (isLoading || isEditMode)
                return;

            int classId = TryGetSelectedId(cmbClassID);
            string academicYear = (txtAcademicYear.Text ?? string.Empty).Trim();
            string section = (txtSection.Text ?? string.Empty).Trim();
            if (classId <= 0 || !IsSequentialAcademicYear(academicYear) || string.IsNullOrWhiteSpace(section))
            {
                txtSeatNumber.Text = "يُولّد تلقائياً عند الحفظ";
                return;
            }

            try
            {
                string nextSeat = await Task.Run(() => enrollmentService.GenerateNextSeatNumber(academicYear, classId, section));
                if (!isEditMode)
                    txtSeatNumber.Text = string.IsNullOrWhiteSpace(nextSeat) ? "يُولّد تلقائياً عند الحفظ" : nextSeat;
            }
            catch
            {
                // المعاينة اختيارية؛ يبقى التوليد النهائي داخل المعاملة في المستودع.
                txtSeatNumber.Text = "يُولّد تلقائياً عند الحفظ";
            }
        }

        private void SetSectionValue(string section)
        {
            if (string.IsNullOrWhiteSpace(section) || txtSection.Items.Count == 0)
                return;

            int index = txtSection.FindStringExact(section.Trim());
            if (index >= 0)
                txtSection.SelectedIndex = index;
        }

        private async void LoadRecordToScreen(DataGridViewRow row)
        {
            if (row == null || row.IsNewRow || isEditMode || isLoading) return;

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

                await LoadSectionsAsync();
                SetSectionValue(SafeCell(row, "Section"));
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

        private decimal ParseAmountOrZero(string value)
        {
            decimal amount;
            return string.IsNullOrWhiteSpace(value) || !UIHelper.TryParseDecimal(value, out amount)
                ? 0m
                : amount;
        }

        private static int TryGetSelectedId(ComboBox comboBox)
        {
            if (comboBox == null || comboBox.SelectedIndex < 0 || comboBox.SelectedValue == null || comboBox.SelectedValue == DBNull.Value)
                return 0;

            int id;
            return int.TryParse(comboBox.SelectedValue.ToString(), out id) && id > 0 ? id : 0;
        }

        private int GetSelectedId(ComboBox comboBox, string errorMessage)
        {
            if (comboBox == null || comboBox.SelectedIndex < 0 || comboBox.SelectedValue == null ||
                comboBox.SelectedValue == DBNull.Value ||
                !int.TryParse(comboBox.SelectedValue.ToString(), out int id) || id <= 0)
            {
                if (comboBox != null) errorProvider1.SetError(comboBox, errorMessage);
                throw new InvalidOperationException(errorMessage);
            }

            return id;
        }

        private bool ValidateInputs()
        {
            bool isValid = true;

            if (cmbStudentID.SelectedIndex == -1 || cmbStudentID.SelectedValue == null ||
                !int.TryParse(cmbStudentID.SelectedValue.ToString(), out int studentId) || studentId <= 0)
            {
                errorProvider1.SetError(cmbStudentID, "يجب تحديد طالب صالح.");
                isValid = false;
            }

            if (cmbClassID.SelectedIndex == -1 || cmbClassID.SelectedValue == null ||
                !int.TryParse(cmbClassID.SelectedValue.ToString(), out int classId) || classId <= 0)
            {
                errorProvider1.SetError(cmbClassID, "يجب تحديد فصل صالح.");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(txtAcademicYear.Text))
            {
                errorProvider1.SetError(txtAcademicYear, "العام الدراسي مطلوب.");
                isValid = false;
            }
            else if (!IsSequentialAcademicYear(txtAcademicYear.Text))
            {
                errorProvider1.SetError(txtAcademicYear, "اكتب العام الدراسي بصيغة متسلسلة مثل 2026/2027 أو 1447-1448.");
                isValid = false;
            }

            if (dtpApplicationDate.Value.Date > DateTime.Today)
            {
                errorProvider1.SetError(dtpApplicationDate, "لا يمكن أن يكون تاريخ التسجيل في المستقبل.");
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

            ValidateTextOnlyField(txtPreviousSchool, "المدرسة السابقة", ref isValid);
            ValidateTextOnlyField(txtPreviousClass, "الصف السابق", ref isValid);
            ValidateTextOnlyField(txtTransferReason, "سبب النقل", ref isValid);

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

            if (isValid && paid > fee)
            {
                errorProvider1.SetError(txtPaidAmount, "لا يجوز أن يتجاوز المدفوع قيمة رسوم التسجيل.");
                isValid = false;
            }

            return isValid;
        }

        private void ValidateTextOnlyField(Control control, string label, ref bool isValid)
        {
            string value = control == null ? string.Empty : control.Text.Trim();
            if (!string.IsNullOrWhiteSpace(value) && !UIHelper.IsValidArabicOrLatinName(value, 2))
            {
                errorProvider1.SetError(control, label + " يقبل الأحرف والمسافات فقط.");
                isValid = false;
            }
        }

        private bool IsSequentialAcademicYear(string value)
        {
            Match match = Regex.Match(value == null ? "" : value.Trim(), @"^(\d{4})[/-](\d{4})$");
            int firstYear;
            int secondYear;
            return match.Success &&
                   int.TryParse(match.Groups[1].Value, out firstYear) &&
                   int.TryParse(match.Groups[2].Value, out secondYear) &&
                   secondYear == firstYear + 1;
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
            bool hasSelectedRecord = int.TryParse(txtEnrollmentID.Text, out int selectedId) && selectedId > 0;
            btnUpdate.Enabled = hasSelectedRecord;
            btnDelete.Enabled = hasSelectedRecord;
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
            SetAutomaticAcademicYear();
            cmbClassID.SelectedIndex = -1;
            txtSection.DataSource = null;
            txtSection.Items.Clear();
            txtSection.Enabled = false;
            txtSeatNumber.Text = "يُولّد تلقائياً عند الحفظ";
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
