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
            this.headerPanel.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.headerPanel.Controls.Add(this.headerActionsPanel);
            this.headerPanel.Controls.Add(this.headerTitlePanel);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Location = new System.Drawing.Point(0, 0);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Padding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.headerPanel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.headerPanel.Size = new System.Drawing.Size(1100, 116);
            this.headerPanel.TabIndex = 2;

            // منطقة العنوان والصورة مستقلة عن الأزرار لمنع أي تداخل.
            this.headerTitlePanel.BackColor = System.Drawing.Color.Transparent;
            this.headerTitlePanel.Controls.Add(this.lblTitle);
            this.headerTitlePanel.Controls.Add(this.studentPictureBox);
            this.headerTitlePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerTitlePanel.Location = new System.Drawing.Point(8, 6);
            this.headerTitlePanel.Name = "headerTitlePanel";
            this.headerTitlePanel.Padding = new System.Windows.Forms.Padding(0);
            this.headerTitlePanel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.headerTitlePanel.Size = new System.Drawing.Size(1084, 64);
            this.headerTitlePanel.TabIndex = 0;

            this.studentPictureBox.BackColor = System.Drawing.Color.FromArgb(226, 232, 240);
            this.studentPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.studentPictureBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.studentPictureBox.Location = new System.Drawing.Point(1028, 4);
            this.studentPictureBox.Margin = new System.Windows.Forms.Padding(0);
            this.studentPictureBox.Name = "studentPictureBox";
            this.studentPictureBox.Size = new System.Drawing.Size(56, 56);
            this.studentPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.studentPictureBox.TabIndex = 0;
            this.studentPictureBox.TabStop = false;

            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Padding = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.lblTitle.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblTitle.Size = new System.Drawing.Size(1028, 64);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "ملف الطالب الموحد";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

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
            this.headerActionsPanel.Location = new System.Drawing.Point(8, 70);
            this.headerActionsPanel.Margin = new System.Windows.Forms.Padding(0);
            this.headerActionsPanel.Name = "headerActionsPanel";
            this.headerActionsPanel.Padding = new System.Windows.Forms.Padding(0);
            this.headerActionsPanel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.headerActionsPanel.Size = new System.Drawing.Size(1084, 40);
            this.headerActionsPanel.TabIndex = 1;
            this.headerActionsPanel.WrapContents = false;


            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);


            // أزرار الرأس - هوية موحدة ثابتة.
            this.btnBack.Size = new System.Drawing.Size(92, 40);
            this.btnBack.MinimumSize = new System.Drawing.Size(92, 40);
            this.btnBack.AutoSize = false;
            this.btnBack.BackColor = System.Drawing.Color.FromArgb(15, 118, 110);
            this.btnBack.ForeColor = System.Drawing.Color.White;
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.FlatAppearance.BorderSize = 0;
            this.btnBack.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Bold);
            this.btnBack.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnBack.Text = "رجوع";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Cursor = System.Windows.Forms.Cursors.Hand;

            this.btnRefresh.Size = new System.Drawing.Size(92, 40);
            this.btnRefresh.MinimumSize = new System.Drawing.Size(92, 40);
            this.btnRefresh.AutoSize = false;
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(15, 118, 110);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnRefresh.Text = "تحديث";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;

            this.btnPrintProfile.Size = new System.Drawing.Size(140, 40);
            this.btnPrintProfile.MinimumSize = new System.Drawing.Size(110, 40);
            this.btnPrintProfile.AutoSize = false;
            this.btnPrintProfile.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.btnPrintProfile.ForeColor = System.Drawing.Color.White;
            this.btnPrintProfile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrintProfile.FlatAppearance.BorderSize = 0;
            this.btnPrintProfile.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Bold);
            this.btnPrintProfile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnPrintProfile.Text = "طباعة الملف";
            this.btnPrintProfile.UseVisualStyleBackColor = false;
            this.btnPrintProfile.Cursor = System.Windows.Forms.Cursors.Hand;

            this.btnExportProfilePdf.Size = new System.Drawing.Size(138, 40);
            this.btnExportProfilePdf.MinimumSize = new System.Drawing.Size(110, 40);
            this.btnExportProfilePdf.AutoSize = false;
            this.btnExportProfilePdf.BackColor = System.Drawing.Color.FromArgb(198, 40, 40);
            this.btnExportProfilePdf.ForeColor = System.Drawing.Color.White;
            this.btnExportProfilePdf.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportProfilePdf.FlatAppearance.BorderSize = 0;
            this.btnExportProfilePdf.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Bold);
            this.btnExportProfilePdf.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnExportProfilePdf.Text = "تصدير PDF";
            this.btnExportProfilePdf.UseVisualStyleBackColor = false;
            this.btnExportProfilePdf.Cursor = System.Windows.Forms.Cursors.Hand;

            this.btnExportProfileExcel.Size = new System.Drawing.Size(148, 40);
            this.btnExportProfileExcel.MinimumSize = new System.Drawing.Size(110, 40);
            this.btnExportProfileExcel.AutoSize = false;
            this.btnExportProfileExcel.BackColor = System.Drawing.Color.FromArgb(22, 163, 74);
            this.btnExportProfileExcel.ForeColor = System.Drawing.Color.White;
            this.btnExportProfileExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportProfileExcel.FlatAppearance.BorderSize = 0;
            this.btnExportProfileExcel.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Bold);
            this.btnExportProfileExcel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnExportProfileExcel.Text = "تصدير Excel";
            this.btnExportProfileExcel.UseVisualStyleBackColor = false;
            this.btnExportProfileExcel.Cursor = System.Windows.Forms.Cursors.Hand;

            // ملخصات الطالب: 3 بطاقات معلومات + 3 بطاقات مؤشرات.
            this.summaryTable.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
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
            this.summaryTable.Location = new System.Drawing.Point(0, 116);
            this.summaryTable.Margin = new System.Windows.Forms.Padding(0);
            this.summaryTable.Name = "summaryTable";
            this.summaryTable.Padding = new System.Windows.Forms.Padding(12, 10, 12, 10);
            this.summaryTable.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.summaryTable.RowCount = 2;
            this.summaryTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 112F));
            this.summaryTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 94F));
            this.summaryTable.Size = new System.Drawing.Size(1100, 226);
            this.summaryTable.TabIndex = 1;


            // التبويبات والجداول: تملأ المساحة المتبقية بالكامل.
            // يجب إضافة كل DataGridView إلى الصفحة التابعة له؛ بدون ذلك تكون الجداول
            // موجودة في الكود فقط وغير مرئية للمستخدم.

            // بطاقات الملخص - نفس الهوية في المصمم والتشغيل.
            this.lblIdentity.BackColor = System.Drawing.Color.White;
            this.lblIdentity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblIdentity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblIdentity.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblIdentity.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblIdentity.Margin = new System.Windows.Forms.Padding(5);
            this.lblIdentity.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.lblIdentity.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblIdentity.Text = "بيانات الطالب";
            this.lblIdentity.TextAlign = System.Drawing.ContentAlignment.MiddleRight;


            // بطاقات الملخص - نفس الهوية في المصمم والتشغيل.
            this.lblContact.BackColor = System.Drawing.Color.White;
            this.lblContact.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblContact.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblContact.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblContact.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblContact.Margin = new System.Windows.Forms.Padding(5);
            this.lblContact.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.lblContact.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblContact.Text = "بيانات التواصل";
            this.lblContact.TextAlign = System.Drawing.ContentAlignment.MiddleRight;


            // بطاقات الملخص - نفس الهوية في المصمم والتشغيل.
            this.lblClassStatus.BackColor = System.Drawing.Color.White;
            this.lblClassStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblClassStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblClassStatus.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblClassStatus.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblClassStatus.Margin = new System.Windows.Forms.Padding(5);
            this.lblClassStatus.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.lblClassStatus.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblClassStatus.Text = "الدراسة والحالة";
            this.lblClassStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;


            // بطاقات الملخص - نفس الهوية في المصمم والتشغيل.
            this.lblAttendanceSummary.BackColor = System.Drawing.Color.White;
            this.lblAttendanceSummary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAttendanceSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAttendanceSummary.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblAttendanceSummary.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblAttendanceSummary.Margin = new System.Windows.Forms.Padding(5);
            this.lblAttendanceSummary.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.lblAttendanceSummary.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblAttendanceSummary.Text = "الحضور والانتظام";
            this.lblAttendanceSummary.TextAlign = System.Drawing.ContentAlignment.MiddleRight;


            // بطاقات الملخص - نفس الهوية في المصمم والتشغيل.
            this.lblAcademicSummary.BackColor = System.Drawing.Color.White;
            this.lblAcademicSummary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAcademicSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAcademicSummary.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblAcademicSummary.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblAcademicSummary.Margin = new System.Windows.Forms.Padding(5);
            this.lblAcademicSummary.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.lblAcademicSummary.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblAcademicSummary.Text = "الأداء الأكاديمي";
            this.lblAcademicSummary.TextAlign = System.Drawing.ContentAlignment.MiddleRight;


            // بطاقات الملخص - نفس الهوية في المصمم والتشغيل.
            this.lblFinancialSummary.BackColor = System.Drawing.Color.White;
            this.lblFinancialSummary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblFinancialSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFinancialSummary.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblFinancialSummary.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblFinancialSummary.Margin = new System.Windows.Forms.Padding(5);
            this.lblFinancialSummary.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.lblFinancialSummary.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblFinancialSummary.Text = "الوضع المالي";
            this.lblFinancialSummary.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            this.dgvAttendance.AutoGenerateColumns = true;
            this.dgvAttendance.AllowUserToAddRows = false;
            this.dgvAttendance.AllowUserToDeleteRows = false;
            this.dgvAttendance.AllowUserToResizeRows = false;
            this.dgvAttendance.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAttendance.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.None;
            this.dgvAttendance.BackgroundColor = System.Drawing.Color.White;
            this.dgvAttendance.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvAttendance.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvAttendance.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvAttendance.ColumnHeadersHeight = 42;
            this.dgvAttendance.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvAttendance.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.dgvAttendance.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.dgvAttendance.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.dgvAttendance.ColumnHeadersDefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgvAttendance.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dgvAttendance.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.dgvAttendance.DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 10F);
            this.dgvAttendance.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.dgvAttendance.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(219, 234, 254);
            this.dgvAttendance.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.dgvAttendance.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.dgvAttendance.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.dgvAttendance.RowsDefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dgvAttendance.RowsDefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.dgvAttendance.RowTemplate.Height = 36;
            this.dgvAttendance.RowHeadersVisible = false;
            this.dgvAttendance.RowHeadersWidth = 4;
            this.dgvAttendance.MultiSelect = false;
            this.dgvAttendance.ReadOnly = true;
            this.dgvAttendance.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dgvAttendance.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAttendance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMarks.AutoGenerateColumns = true;
            this.dgvMarks.AllowUserToAddRows = false;
            this.dgvMarks.AllowUserToDeleteRows = false;
            this.dgvMarks.AllowUserToResizeRows = false;
            this.dgvMarks.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMarks.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.None;
            this.dgvMarks.BackgroundColor = System.Drawing.Color.White;
            this.dgvMarks.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvMarks.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvMarks.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvMarks.ColumnHeadersHeight = 42;
            this.dgvMarks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvMarks.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.dgvMarks.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.dgvMarks.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.dgvMarks.ColumnHeadersDefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgvMarks.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dgvMarks.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.dgvMarks.DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 10F);
            this.dgvMarks.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.dgvMarks.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(219, 234, 254);
            this.dgvMarks.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.dgvMarks.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.dgvMarks.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.dgvMarks.RowsDefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dgvMarks.RowsDefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.dgvMarks.RowTemplate.Height = 36;
            this.dgvMarks.RowHeadersVisible = false;
            this.dgvMarks.RowHeadersWidth = 4;
            this.dgvMarks.MultiSelect = false;
            this.dgvMarks.ReadOnly = true;
            this.dgvMarks.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dgvMarks.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMarks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvFees.AutoGenerateColumns = true;
            this.dgvFees.AllowUserToAddRows = false;
            this.dgvFees.AllowUserToDeleteRows = false;
            this.dgvFees.AllowUserToResizeRows = false;
            this.dgvFees.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvFees.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.None;
            this.dgvFees.BackgroundColor = System.Drawing.Color.White;
            this.dgvFees.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvFees.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvFees.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvFees.ColumnHeadersHeight = 42;
            this.dgvFees.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvFees.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.dgvFees.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.dgvFees.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.dgvFees.ColumnHeadersDefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgvFees.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dgvFees.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.dgvFees.DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 10F);
            this.dgvFees.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.dgvFees.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(219, 234, 254);
            this.dgvFees.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.dgvFees.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.dgvFees.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.dgvFees.RowsDefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dgvFees.RowsDefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.dgvFees.RowTemplate.Height = 36;
            this.dgvFees.RowHeadersVisible = false;
            this.dgvFees.RowHeadersWidth = 4;
            this.dgvFees.MultiSelect = false;
            this.dgvFees.ReadOnly = true;
            this.dgvFees.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dgvFees.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvFees.Dock = System.Windows.Forms.DockStyle.Fill;
            this.attendanceTab.Controls.Add(this.dgvAttendance);
            this.marksTab.Controls.Add(this.dgvMarks);
            this.feesTab.Controls.Add(this.dgvFees);

            this.tabs.Controls.Add(this.attendanceTab);
            this.tabs.Controls.Add(this.marksTab);
            this.tabs.Controls.Add(this.feesTab);
            this.tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabs.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.tabs.ItemSize = new System.Drawing.Size(130, 38);
            this.tabs.Location = new System.Drawing.Point(0, 342);
            this.tabs.Margin = new System.Windows.Forms.Padding(0);
            this.tabs.Name = "tabs";
            this.tabs.Padding = new System.Drawing.Point(18, 6);
            this.tabs.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tabs.RightToLeftLayout = true;
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(1100, 396);
            this.tabs.TabIndex = 0;


            this.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.Controls.Add(this.tabs);
            this.Controls.Add(this.summaryTable);
            this.Controls.Add(this.headerPanel);
            this.MinimumSize = new System.Drawing.Size(760, 560);
            this.Name = "StudentProfileForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Font = new System.Drawing.Font("Tahoma", 9.5F);
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

    }
}
