using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using SchoolSystem.Security;
using SchoolSystem.Services;

namespace SchoolSystem.UI
{
    public sealed class AnnualClosingForm : Form
    {
        private readonly AnnualClosingService service = new AnnualClosingService();
        private ComboBox cmbYear;
        private TextBox txtNextYear;
        private TextBox txtNotes;
        private DataGridView grid;
        private Button btnVerify;
        private Button btnClose;
        private Button btnPlan;
        private Label lblStatus;

        public AnnualClosingForm()
        {
            InitializeUi();
            LoadYears();
        }

        private void InitializeUi()
        {
            Text = "الإغلاق السنوي والترحيل";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterParent;
            MinimumSize = new Size(900, 580);
            BackColor = Color.White;

            TableLayoutPanel root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(16),
                ColumnCount = 2,
                RowCount = 6,
                BackColor = Color.White
            };
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150));
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 70));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 52));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            Label title = new Label { Text = "الإغلاق السنوي والترحيل", Dock = DockStyle.Fill, Font = new Font("Tahoma", 16, FontStyle.Bold), ForeColor = Color.FromArgb(31, 78, 121), TextAlign = ContentAlignment.MiddleRight };
            root.Controls.Add(title, 0, 0); root.SetColumnSpan(title, 2);
            root.Controls.Add(new Label { Text = "العام المراد إغلاقه", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleRight }, 0, 1);
            cmbYear = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
            root.Controls.Add(cmbYear, 1, 1);
            root.Controls.Add(new Label { Text = "العام التالي (اختياري)", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleRight }, 0, 2);
            TableLayoutPanel nextPanel = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2 };
            nextPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45)); nextPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55));
            txtNextYear = new TextBox { Dock = DockStyle.Fill, MaxLength = 20 };
            txtNotes = new TextBox { Dock = DockStyle.Fill, MaxLength = 1000, Multiline = true, ScrollBars = ScrollBars.Vertical };
            nextPanel.Controls.Add(txtNextYear, 0, 0); nextPanel.Controls.Add(txtNotes, 1, 0);
            root.Controls.Add(nextPanel, 1, 2);

            FlowLayoutPanel actions = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft, WrapContents = false };
            btnVerify = CreateButton("فحص المطابقة", Color.FromArgb(31, 119, 180)); btnVerify.Click += VerifyClick;
            btnClose = CreateButton("إغلاق العام", Color.FromArgb(192, 80, 77)); btnClose.Click += CloseClick;
            btnPlan = CreateButton("تخطيط الترحيل", Color.FromArgb(112, 173, 71)); btnPlan.Click += PlanClick;
            actions.Controls.Add(btnClose); actions.Controls.Add(btnPlan); actions.Controls.Add(btnVerify);
            root.Controls.Add(actions, 0, 3); root.SetColumnSpan(actions, 2);
            lblStatus = new Label { Text = "ابدأ بفحص المطابقة قبل الإغلاق.", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleRight, ForeColor = Color.DarkSlateGray };
            root.Controls.Add(lblStatus, 0, 4); root.SetColumnSpan(lblStatus, 2);

            grid = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, SelectionMode = DataGridViewSelectionMode.FullRowSelect, RightToLeft = RightToLeft.Yes, BackgroundColor = Color.White };
            root.Controls.Add(grid, 0, 5); root.SetColumnSpan(grid, 2);
            Controls.Add(root);
        }

        private Button CreateButton(string text, Color color)
        {
            return new Button { Text = text, AutoSize = true, Height = 34, Margin = new Padding(5, 4, 5, 4), BackColor = color, ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
        }

        private void LoadYears()
        {
            try
            {
                foreach (string year in service.GetAvailableYears())
                    if (!cmbYear.Items.Contains(year)) cmbYear.Items.Add(year);
            }
            catch
            {
                // يبقى التشغيل ممكناً قبل إدخال أول بيانات، مع استخدام العام الحالي كخيار احتياطي.
            }
            string current = DateTime.Today.Year + "/" + (DateTime.Today.Year + 1);
            if (!cmbYear.Items.Contains(current)) cmbYear.Items.Add(current);
            cmbYear.SelectedIndex = 0;
        }

        private string SelectedYear()
        {
            if (cmbYear.SelectedItem == null) throw new InvalidOperationException("اختر العام الدراسي أولاً.");
            return cmbYear.SelectedItem.ToString();
        }

        private void VerifyClick(object sender, EventArgs e)
        {
            try
            {
                DataTable table = service.Verify(SelectedYear());
                grid.DataSource = table;
                bool canClose = true;
                foreach (DataRow row in table.Rows)
                    if (row.Table.Columns.Contains("IssueCount") && Convert.ToInt32(row["IssueCount"]) > 0 && row["Severity"].ToString() == "حرج") canClose = false;
                lblStatus.Text = canClose ? "الفحص ناجح: يمكن إغلاق العام." : "الفحص غير ناجح: توجد أخطاء حرجة يجب معالجتها.";
                lblStatus.ForeColor = canClose ? Color.DarkGreen : Color.DarkRed;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "فحص الإغلاق", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }

        private void CloseClick(object sender, EventArgs e)
        {
            try
            {
                AuthorizationService.RequireAny("ليس لديك صلاحية تنفيذ الإغلاق السنوي.", PermissionKeys.AnnualClosingManage);
                if (MessageBox.Show("سيتم إغلاق العام ومنع العمليات المستقبلية عليه. هل تريد المتابعة؟", "تأكيد الإغلاق", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
                service.Close(SelectedYear(), txtNextYear.Text, null, txtNotes.Text);
                lblStatus.Text = "تم إغلاق العام بنجاح. لم يتم تعديل بيانات السنوات السابقة.";
                MessageBox.Show(lblStatus.Text, "الإغلاق السنوي", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "تعذر الإغلاق", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }

        private void PlanClick(object sender, EventArgs e)
        {
            try
            {
                AuthorizationService.RequireAny("ليس لديك صلاحية تخطيط الترحيل.", PermissionKeys.AnnualClosingManage);
                DataTable table = service.PlanMigration(SelectedYear(), txtNextYear.Text, null);
                grid.DataSource = table;
                lblStatus.Text = "تم إنشاء خطة ترحيل قابلة للمراجعة. لم يتم إنشاء توزيع جديد تلقائياً.";
                lblStatus.ForeColor = Color.DarkGreen;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "تعذر تخطيط الترحيل", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }
    }
}
