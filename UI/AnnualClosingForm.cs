using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using SchoolSystem.Helpers;
using SchoolSystem.Security;
using SchoolSystem.Services;


namespace SchoolSystem.UI
{

    public sealed partial class AnnualClosingForm : Form
    {
        // تعريف الدالة AnnualClosingService لتنفيذ العملية المرتبطة بها.
        private readonly AnnualClosingService service = new AnnualClosingService();

        // تعريف الدالة AnnualClosingForm لتنفيذ العملية المرتبطة بها.
        public AnnualClosingForm()
        {

            InitializeComponent();

            UIHelper.ApplyStyle(this);
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
                LoadYears();
        }

        // تعريف الدالة LoadYears لتنفيذ العملية المرتبطة بها.
        private void LoadYears()
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                foreach (string year in service.GetAvailableYears())
                {
                    if (!cmbYear.Items.Contains(year))
                        cmbYear.Items.Add(year);
                }
            }

            catch (Exception ex)
            {
                // فشل تحميل السنوات لا يمنع فتح الشاشة؛ نسجل السبب ونعرض العام المحسوب فقط.
                ApplicationLogger.LogException("تحميل قائمة السنوات الدراسية", ex);
            }

            string current = DateTime.Today.Year + "/" + (DateTime.Today.Year + 1);
            if (!cmbYear.Items.Contains(current))
                cmbYear.Items.Add(current);

            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                string active = service.GetActiveAcademicYear();
                if (!string.IsNullOrWhiteSpace(active) && cmbYear.Items.Contains(active))
                    cmbYear.SelectedItem = active;

                else if (cmbYear.Items.Count > 0)

                    cmbYear.SelectedIndex = 0;
            }

            catch (Exception ex)
            {
                ApplicationLogger.LogException("قراءة العام الدراسي النشط", ex);
                if (cmbYear.Items.Count > 0)
                    cmbYear.SelectedIndex = 0;
            }
        }

        // تعريف الدالة SelectedYear لتنفيذ العملية المرتبطة بها.
        private string SelectedYear()
        {
            if (cmbYear.SelectedItem == null)

                throw new InvalidOperationException("اختر العام الدراسي أولاً.");

            return cmbYear.SelectedItem.ToString();
        }

        // تعريف الدالة SelectedNextYear لتنفيذ العملية المرتبطة بها.
        private string SelectedNextYear()
        {
            string nextYear = txtNextYear.Text == null ? string.Empty : txtNextYear.Text.Trim();
            if (string.IsNullOrWhiteSpace(nextYear))

                throw new InvalidOperationException("أدخل العام الدراسي التالي قبل التخطيط أو الإغلاق.");

            if (nextYear.Length > 20)

                throw new InvalidOperationException("العام الدراسي التالي طويل أو غير صالح.");

            if (!UIHelper.IsValidAcademicYear(nextYear))

                throw new InvalidOperationException("صيغة العام الدراسي التالي غير صحيحة (مثال: 2025/2026).");

            if (string.Equals(SelectedYear(), nextYear, StringComparison.OrdinalIgnoreCase))

                throw new InvalidOperationException("يجب أن يكون العام التالي مختلفاً عن العام المراد إغلاقه.");

            return nextYear;
        }

        // تعريف الدالة VerifyClick لتنفيذ العملية المرتبطة بها.
        private void VerifyClick(object sender, EventArgs e)
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                DataTable table = service.Verify(SelectedYear());
                grid.DataSource = table;
                bool canClose = true;
                foreach (DataRow row in table.Rows)
                {
                    if (row.Table.Columns.Contains("IssueCount") &&
                        Convert.ToInt32(row["IssueCount"]) > 0 &&
                        row.Table.Columns.Contains("Severity") &&
                        string.Equals(Convert.ToString(row["Severity"]), "حرج", StringComparison.OrdinalIgnoreCase))
                    {
                        canClose = false;
                        break;
                    }
                }

                lblStatus.Text = canClose
                    ? "الفحص ناجح: يمكن إغلاق العام الدراسي."
                    : "الفحص غير ناجح: توجد أخطاء حرجة يجب معالجتها قبل الإغلاق.";
                lblStatus.ForeColor = canClose ? Color.DarkGreen : Color.DarkRed;
            }

            catch (Exception ex)
            {
                ShowOperationError("فحص الإغلاق", ex);
            }
        }

        // تعريف الدالة MigrationReportClick لتنفيذ العملية المرتبطة بها.
        private void MigrationReportClick(object sender, EventArgs e)
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                AuthorizationService.RequireAny("ليس لديك صلاحية عرض تقرير الترحيل.", PermissionKeys.AnnualClosingView, PermissionKeys.AnnualClosingManage);
                DataTable table = service.GetMigrationReport(SelectedYear(), SelectedNextYear());
                grid.DataSource = table;
                lblStatus.Text = "تم عرض تقرير الترحيل. حدد سجلاً مخططاً لاعتماده.";
                lblStatus.ForeColor = Color.DarkSlateGray;
            }

            catch (Exception ex)
            {
                ShowOperationError("تقرير الترحيل", ex);
            }
        }

        // تعريف الدالة ApproveMigrationClick لتنفيذ العملية المرتبطة بها.
        private void ApproveMigrationClick(object sender, EventArgs e)
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                AuthorizationService.RequireAny("ليس لديك صلاحية اعتماد الترحيل.", PermissionKeys.AnnualClosingApprove, PermissionKeys.AnnualClosingManage);
                if (grid.CurrentRow == null || !grid.Columns.Contains("MigrationID"))

                    throw new InvalidOperationException("اعرض تقرير الترحيل وحدد سجلاً مخططاً أولاً.");

                int migrationId;
                if (!int.TryParse(Convert.ToString(grid.CurrentRow.Cells["MigrationID"].Value), out migrationId) || migrationId <= 0)

                    throw new InvalidOperationException("السجل المحدد لا يحتوي على رقم ترحيل صالح.");

                int classId;
                if (!int.TryParse(txtTargetClassId.Text.Trim(), out classId) || classId <= 0 || string.IsNullOrWhiteSpace(txtTargetSection.Text))

                    throw new InvalidOperationException("أدخل رقم الصف الجديد واسم الشعبة الجديدة.");


                if (MessageBox.Show("سيتم إنشاء توزيع الطالب في العام الجديد وتسجيل العملية. هل تريد المتابعة؟", "تأكيد اعتماد الترحيل", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)

                    return;

                service.ApproveMigration(migrationId, classId, txtTargetSection.Text, null);
                lblStatus.Text = "تم اعتماد ترحيل الطالب وإنشاء توزيعه في العام الجديد بنجاح.";
                lblStatus.ForeColor = Color.DarkGreen;
                grid.DataSource = service.GetMigrationReport(SelectedYear(), SelectedNextYear());
            }

            catch (Exception ex)
            {
                ShowOperationError("تعذر اعتماد الترحيل", ex);
            }
        }

        // تعريف الدالة CloseClick لتنفيذ العملية المرتبطة بها.
        private void CloseClick(object sender, EventArgs e)
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                AuthorizationService.RequireAny("ليس لديك صلاحية تنفيذ الإغلاق السنوي.", PermissionKeys.AnnualClosingManage);
                string selectedYear = SelectedYear();
                string nextYear = string.IsNullOrWhiteSpace(txtNextYear.Text) ? string.Empty : SelectedNextYear();


                if (MessageBox.Show("سيتم إنشاء نسخة احتياطية ثم إغلاق العام ومنع العمليات المستقبلية عليه. هل تريد المتابعة؟", "تأكيد الإغلاق", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)

                    return;

                service.CloseWithRequiredBackup(selectedYear, nextYear, CurrentUser.User?.UserID, txtNotes.Text);
                if (!string.IsNullOrWhiteSpace(nextYear))
                    service.SetActiveAcademicYear(nextYear, CurrentUser.User?.UserID);

                lblStatus.Text = "تم إنشاء النسخة الاحتياطية وإغلاق العام وتحديث العام النشط بنجاح.";
                lblStatus.ForeColor = Color.DarkGreen;

                MessageBox.Show(lblStatus.Text, "الإغلاق السنوي", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadYears();
            }

            catch (Exception ex)
            {
                ShowOperationError("تعذر الإغلاق", ex);
            }
        }

        // تعريف الدالة PlanClick لتنفيذ العملية المرتبطة بها.
        private void PlanClick(object sender, EventArgs e)
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                AuthorizationService.RequireAny("ليس لديك صلاحية تخطيط الترحيل.", PermissionKeys.AnnualClosingManage);
                DataTable table = service.PlanMigration(SelectedYear(), SelectedNextYear(), null);
                grid.DataSource = table;
                lblStatus.Text = "تم إنشاء خطة ترحيل قابلة للمراجعة. لم يتم إنشاء توزيع جديد تلقائياً.";
                lblStatus.ForeColor = Color.DarkGreen;
            }

            catch (Exception ex)
            {
                ShowOperationError("تعذر تخطيط الترحيل", ex);
            }
        }

        // تعريف الدالة ShowOperationError لتنفيذ العملية المرتبطة بها.
        private void ShowOperationError(string caption, Exception ex)
        {

            UIHelper.ShowException(caption, ex);
        }
    }
}
