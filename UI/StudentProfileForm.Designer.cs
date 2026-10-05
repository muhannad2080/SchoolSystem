namespace SchoolSystem.UI
{
    partial class StudentProfileForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Panel headerTitlePanel;
        private System.Windows.Forms.FlowLayoutPanel headerActionsPanel;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.PictureBox studentPictureBox;

        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnPrintProfile;
        private System.Windows.Forms.Button btnExportProfilePdf;
        private System.Windows.Forms.Button btnExportProfileExcel;

        private System.Windows.Forms.TableLayoutPanel summaryTable;
        private System.Windows.Forms.Label lblIdentity;
        private System.Windows.Forms.Label lblContact;
        private System.Windows.Forms.Label lblClassStatus;
        private System.Windows.Forms.Label lblFinancialSummary;
        private System.Windows.Forms.Label lblAttendanceSummary;
        private System.Windows.Forms.Label lblAcademicSummary;

        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.TabPage attendanceTab;
        private System.Windows.Forms.TabPage marksTab;
        private System.Windows.Forms.TabPage feesTab;
        private System.Windows.Forms.DataGridView dgvAttendance;
        private System.Windows.Forms.DataGridView dgvMarks;
        private System.Windows.Forms.DataGridView dgvFees;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.headerPanel = new System.Windows.Forms.Panel();
            this.headerTitlePanel = new System.Windows.Forms.Panel();
            this.studentPictureBox = new System.Windows.Forms.PictureBox();
            this.lblTitle = new System.Windows.Forms.Label();

            this.headerActionsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnPrintProfile = new System.Windows.Forms.Button();
            this.btnExportProfilePdf = new System.Windows.Forms.Button();
            this.btnExportProfileExcel = new System.Windows.Forms.Button();

            this.summaryTable = new System.Windows.Forms.TableLayoutPanel();
            this.lblIdentity = new System.Windows.Forms.Label();
            this.lblContact = new System.Windows.Forms.Label();
            this.lblClassStatus = new System.Windows.Forms.Label();
            this.lblAttendanceSummary = new System.Windows.Forms.Label();
            this.lblAcademicSummary = new System.Windows.Forms.Label();
            this.lblFinancialSummary = new System.Windows.Forms.Label();

            this.tabs = new System.Windows.Forms.TabControl();
            this.attendanceTab = new System.Windows.Forms.TabPage();
            this.dgvAttendance = new System.Windows.Forms.DataGridView();
            this.marksTab = new System.Windows.Forms.TabPage();
            this.dgvMarks = new System.Windows.Forms.DataGridView();
            this.feesTab = new System.Windows.Forms.TabPage();
            this.dgvFees = new System.Windows.Forms.DataGridView();

            this.headerPanel.SuspendLayout();
            this.headerTitlePanel.SuspendLayout();
            this.headerActionsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.studentPictureBox)).BeginInit();
            this.summaryTable.SuspendLayout();
            this.tabs.SuspendLayout();
            this.attendanceTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAttendance)).BeginInit();
            this.marksTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMarks)).BeginInit();
            this.feesTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFees)).BeginInit();
            this.SuspendLayout();

            // الرأس الرئيسي: صف العنوان + شريط الإجراءات.
            this.headerPanel.BackColor = SchoolSystem.Helpers.UIHelper.PrimaryColor;
            this.headerPanel.Controls.Add(this.headerActionsPanel);
            this.headerPanel.Controls.Add(this.headerTitlePanel);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Location = new System.Drawing.Point(0, 0);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Padding = new System.Windows.Forms.Padding(12, 10, 12, 10);
            this.headerPanel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.headerPanel.Size = new System.Drawing.Size(1100, 138);
            this.headerPanel.TabIndex = 2;

            // منطقة العنوان والصورة مستقلة عن الأزرار لمنع أي تداخل.
            this.headerTitlePanel.BackColor = System.Drawing.Color.Transparent;
            this.headerTitlePanel.Controls.Add(this.lblTitle);
            this.headerTitlePanel.Controls.Add(this.studentPictureBox);
            this.headerTitlePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerTitlePanel.Location = new System.Drawing.Point(12, 10);
            this.headerTitlePanel.Name = "headerTitlePanel";
            this.headerTitlePanel.Padding = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.headerTitlePanel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.headerTitlePanel.Size = new System.Drawing.Size(1076, 74);
            this.headerTitlePanel.TabIndex = 0;

            this.studentPictureBox.BackColor = SchoolSystem.Helpers.UIHelper.PrimaryLightColor;
            this.studentPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.studentPictureBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.studentPictureBox.Location = new System.Drawing.Point(992, 0);
            this.studentPictureBox.Margin = new System.Windows.Forms.Padding(0);
            this.studentPictureBox.Name = "studentPictureBox";
            this.studentPictureBox.Size = new System.Drawing.Size(84, 68);
            this.studentPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.studentPictureBox.TabIndex = 0;
            this.studentPictureBox.TabStop = false;

            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font(SchoolSystem.Helpers.UIHelper.FontFamily, 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Padding = new System.Windows.Forms.Padding(0, 0, 16, 0);
            this.lblTitle.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblTitle.Size = new System.Drawing.Size(992, 68);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "ملف الطالب الموحد";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            // شريط الإجراءات: لا يعتمد على تموضع مطلق، ويمنع تراكب الأزرار.
            this.headerActionsPanel.AutoScroll = false;
            this.headerActionsPanel.BackColor = System.Drawing.Color.Transparent;
            this.headerActionsPanel.Controls.Add(this.btnBack);
            this.headerActionsPanel.Controls.Add(this.btnRefresh);
            this.headerActionsPanel.Controls.Add(this.btnPrintProfile);
            this.headerActionsPanel.Controls.Add(this.btnExportProfilePdf);
            this.headerActionsPanel.Controls.Add(this.btnExportProfileExcel);
            this.headerActionsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.headerActionsPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.headerActionsPanel.Location = new System.Drawing.Point(12, 84);
            this.headerActionsPanel.Margin = new System.Windows.Forms.Padding(0);
            this.headerActionsPanel.Name = "headerActionsPanel";
            this.headerActionsPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.headerActionsPanel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.headerActionsPanel.Size = new System.Drawing.Size(1076, 44);
            this.headerActionsPanel.TabIndex = 1;
            this.headerActionsPanel.WrapContents = false;

            ConfigureHeaderButton(this.btnBack, "رجوع", 92);
            ConfigureHeaderButton(this.btnRefresh, "تحديث", 92);
            ConfigureHeaderButton(this.btnPrintProfile, "طباعة الملف", 140);
            ConfigureHeaderButton(this.btnExportProfilePdf, "تصدير PDF", 138);
            ConfigureHeaderButton(this.btnExportProfileExcel, "تصدير Excel", 148);

            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);

            // ملخصات الطالب: 3 بطاقات معلومات + 3 بطاقات مؤشرات.
            this.summaryTable.BackColor = SchoolSystem.Helpers.UIHelper.BackgroundColor;
            this.summaryTable.ColumnCount = 3;
            this.summaryTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.3333F));
            this.summaryTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.3333F));
            this.summaryTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.3334F));
            this.summaryTable.Controls.Add(this.lblIdentity, 0, 0);
            this.summaryTable.Controls.Add(this.lblContact, 1, 0);
            this.summaryTable.Controls.Add(this.lblClassStatus, 2, 0);
            this.summaryTable.Controls.Add(this.lblAttendanceSummary, 0, 1);
            this.summaryTable.Controls.Add(this.lblAcademicSummary, 1, 1);
            this.summaryTable.Controls.Add(this.lblFinancialSummary, 2, 1);
            this.summaryTable.Dock = System.Windows.Forms.DockStyle.Top;
            this.summaryTable.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.summaryTable.Location = new System.Drawing.Point(0, 138);
            this.summaryTable.Margin = new System.Windows.Forms.Padding(0);
            this.summaryTable.Name = "summaryTable";
            this.summaryTable.Padding = new System.Windows.Forms.Padding(12, 10, 12, 10);
            this.summaryTable.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.summaryTable.RowCount = 2;
            this.summaryTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 112F));
            this.summaryTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 94F));
            this.summaryTable.Size = new System.Drawing.Size(1100, 226);
            this.summaryTable.TabIndex = 1;

            ConfigureSummaryCard(this.lblIdentity, "بيانات الطالب");
            ConfigureSummaryCard(this.lblContact, "بيانات التواصل");
            ConfigureSummaryCard(this.lblClassStatus, "الدراسة والحالة");
            ConfigureSummaryCard(this.lblAttendanceSummary, "الحضور والانتظام", true);
            ConfigureSummaryCard(this.lblAcademicSummary, "الأداء الأكاديمي", true);
            ConfigureSummaryCard(this.lblFinancialSummary, "الوضع المالي", true);

            // التبويبات والجداول: تملأ المساحة المتبقية بالكامل.
            this.tabs.Controls.Add(this.attendanceTab);
            this.tabs.Controls.Add(this.marksTab);
            this.tabs.Controls.Add(this.feesTab);
            this.tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabs.Font = new System.Drawing.Font(SchoolSystem.Helpers.UIHelper.FontFamily, 10F, System.Drawing.FontStyle.Bold);
            this.tabs.ItemSize = new System.Drawing.Size(130, 38);
            this.tabs.Location = new System.Drawing.Point(0, 364);
            this.tabs.Margin = new System.Windows.Forms.Padding(0);
            this.tabs.Name = "tabs";
            this.tabs.Padding = new System.Drawing.Point(18, 6);
            this.tabs.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tabs.RightToLeftLayout = true;
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(1100, 396);
            this.tabs.TabIndex = 0;

            ConfigureTabPage(this.attendanceTab, this.dgvAttendance, "الحضور والغياب");
            ConfigureTabPage(this.marksTab, this.dgvMarks, "الدرجات الأكاديمية");
            ConfigureTabPage(this.feesTab, this.dgvFees, "الرسوم والمدفوعات");

            this.BackColor = SchoolSystem.Helpers.UIHelper.BackgroundColor;
            this.Controls.Add(this.tabs);
            this.Controls.Add(this.summaryTable);
            this.Controls.Add(this.headerPanel);
            this.MinimumSize = new System.Drawing.Size(760, 560);
            this.Name = "StudentProfileForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Font = new System.Drawing.Font(SchoolSystem.Helpers.UIHelper.FontFamily, 9.5F);
            this.Size = new System.Drawing.Size(1100, 760);

            this.headerActionsPanel.ResumeLayout(false);
            this.headerTitlePanel.ResumeLayout(false);
            this.headerPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.studentPictureBox)).EndInit();
            this.summaryTable.ResumeLayout(false);
            this.tabs.ResumeLayout(false);
            this.attendanceTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAttendance)).EndInit();
            this.marksTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMarks)).EndInit();
            this.feesTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFees)).EndInit();
            this.ResumeLayout(false);
        }

        private void ConfigureHeaderButton(System.Windows.Forms.Button button, string text, int width)
        {
            button.BackColor = SchoolSystem.Helpers.UIHelper.AccentColor;
            button.FlatAppearance.BorderSize = 0;
            button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            button.Font = new System.Drawing.Font(SchoolSystem.Helpers.UIHelper.FontFamily, 9.5F, System.Drawing.FontStyle.Bold);
            button.ForeColor = System.Drawing.Color.White;
            button.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            button.MinimumSize = new System.Drawing.Size(width, 40);
            button.Name = button.Name;
            button.Padding = new System.Windows.Forms.Padding(8, 0, 8, 0);
            button.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            button.Size = new System.Drawing.Size(width, 40);
            button.TabIndex = 0;
            button.Text = text;
            button.UseVisualStyleBackColor = false;
        }

        private void ConfigureSummaryCard(System.Windows.Forms.Label label, string fallbackText, bool compact = false)
        {
            label.BackColor = SchoolSystem.Helpers.UIHelper.SurfaceColor;
            label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label.Dock = System.Windows.Forms.DockStyle.Fill;
            label.Font = new System.Drawing.Font(
                SchoolSystem.Helpers.UIHelper.FontFamily,
                compact ? 9.5F : 10F,
                System.Drawing.FontStyle.Bold);
            label.ForeColor = SchoolSystem.Helpers.UIHelper.TextColor;
            label.Margin = new System.Windows.Forms.Padding(5);
            label.Name = label.Name;
            label.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            label.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            label.Text = fallbackText;
            label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            label.AutoEllipsis = true;
        }

        private void ConfigureTabPage(
            System.Windows.Forms.TabPage tab,
            System.Windows.Forms.DataGridView grid,
            string text)
        {
            tab.BackColor = SchoolSystem.Helpers.UIHelper.SurfaceColor;
            tab.Controls.Add(grid);
            tab.Location = new System.Drawing.Point(4, 42);
            tab.Margin = new System.Windows.Forms.Padding(0);
            tab.Name = tab.Name;
            tab.Padding = new System.Windows.Forms.Padding(10);
            tab.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            tab.Size = new System.Drawing.Size(1092, 350);
            tab.TabIndex = 0;
            tab.Text = text;
            tab.UseVisualStyleBackColor = false;

            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            grid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.None;
            grid.BackgroundColor = System.Drawing.Color.White;
            grid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            grid.ColumnHeadersHeight = 42;
            grid.Dock = System.Windows.Forms.DockStyle.Fill;
            grid.Location = new System.Drawing.Point(10, 10);
            grid.Margin = new System.Windows.Forms.Padding(0);
            grid.MultiSelect = false;
            grid.Name = grid.Name;
            grid.ReadOnly = true;
            grid.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            grid.RowHeadersVisible = false;
            grid.RowHeadersWidth = 1;
            grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            grid.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            grid.TabIndex = 0;
        }
    }
}
