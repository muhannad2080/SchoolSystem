
namespace SchoolSystem.UI
{

    partial class StudentProfileForm
    {

        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel headerPanel;

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

        // تعريف الدالة Dispose لتنفيذ العملية المرتبطة بها.
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }


        private void InitializeComponent()
        {
            this.headerPanel = new System.Windows.Forms.Panel();
            this.headerActionsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.studentPictureBox = new System.Windows.Forms.PictureBox();
            this.lblTitle = new System.Windows.Forms.Label();
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




            this.headerPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(78)))), ((int)(((byte)(121)))));
            this.headerPanel.Controls.Add(this.lblTitle);
            this.headerPanel.Controls.Add(this.studentPictureBox);
            this.headerPanel.Controls.Add(this.headerActionsPanel);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Location = new System.Drawing.Point(0, 0);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Padding = new System.Windows.Forms.Padding(16, 8, 16, 8);
            this.headerPanel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.headerPanel.Size = new System.Drawing.Size(1027, 72);
            this.headerPanel.TabIndex = 2;



            this.studentPictureBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.studentPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.studentPictureBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.studentPictureBox.Location = new System.Drawing.Point(939, 8);
            this.studentPictureBox.Name = "studentPictureBox";
            this.studentPictureBox.Size = new System.Drawing.Size(72, 56);
            this.studentPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.studentPictureBox.TabIndex = 0;
            this.studentPictureBox.TabStop = false;



            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(750, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblTitle.Size = new System.Drawing.Size(189, 56);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "ملف الطالب الموحد";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;



            this.btnBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(140)))), ((int)(((byte)(141)))));
            this.btnBack.AutoSize = true;
            this.btnBack.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnBack.FlatAppearance.BorderSize = 0;
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnBack.ForeColor = System.Drawing.Color.White;
            this.btnBack.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnBack.MinimumSize = new System.Drawing.Size(95, 46);
            this.btnBack.Name = "btnBack";
            this.btnBack.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnBack.Size = new System.Drawing.Size(95, 46);
            this.btnBack.TabIndex = 2;
            this.btnBack.Text = "رجوع";
            this.btnBack.UseVisualStyleBackColor = false;

            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);



            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
            this.btnRefresh.AutoSize = true;
            this.btnRefresh.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnRefresh.MinimumSize = new System.Drawing.Size(95, 46);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnRefresh.Size = new System.Drawing.Size(95, 46);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "تحديث";
            this.btnRefresh.UseVisualStyleBackColor = false;

            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);



            this.btnPrintProfile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(78)))), ((int)(((byte)(121)))));
            this.btnPrintProfile.AutoSize = true;
            this.btnPrintProfile.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnPrintProfile.FlatAppearance.BorderSize = 0;
            this.btnPrintProfile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrintProfile.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnPrintProfile.ForeColor = System.Drawing.Color.White;
            this.btnPrintProfile.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnPrintProfile.MinimumSize = new System.Drawing.Size(150, 46);
            this.btnPrintProfile.Name = "btnPrintProfile";
            this.btnPrintProfile.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnPrintProfile.Size = new System.Drawing.Size(150, 46);
            this.btnPrintProfile.TabIndex = 2;
            this.btnPrintProfile.Text = "طباعة ملف الطالب";
            this.btnPrintProfile.UseVisualStyleBackColor = false;



            this.btnExportProfilePdf.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(57)))), ((int)(((byte)(43)))));
            this.btnExportProfilePdf.AutoSize = true;
            this.btnExportProfilePdf.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnExportProfilePdf.FlatAppearance.BorderSize = 0;
            this.btnExportProfilePdf.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportProfilePdf.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnExportProfilePdf.ForeColor = System.Drawing.Color.White;
            this.btnExportProfilePdf.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnExportProfilePdf.MinimumSize = new System.Drawing.Size(175, 46);
            this.btnExportProfilePdf.Name = "btnExportProfilePdf";
            this.btnExportProfilePdf.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnExportProfilePdf.Size = new System.Drawing.Size(175, 46);
            this.btnExportProfilePdf.TabIndex = 3;
            this.btnExportProfilePdf.Text = "تصدير ملف الطالب إلى PDF";
            this.btnExportProfilePdf.UseVisualStyleBackColor = false;



            this.btnExportProfileExcel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.btnExportProfileExcel.AutoSize = true;
            this.btnExportProfileExcel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnExportProfileExcel.FlatAppearance.BorderSize = 0;
            this.btnExportProfileExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportProfileExcel.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnExportProfileExcel.ForeColor = System.Drawing.Color.White;
            this.btnExportProfileExcel.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnExportProfileExcel.MinimumSize = new System.Drawing.Size(185, 46);
            this.btnExportProfileExcel.Name = "btnExportProfileExcel";
            this.btnExportProfileExcel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnExportProfileExcel.Size = new System.Drawing.Size(185, 46);
            this.btnExportProfileExcel.TabIndex = 4;
            this.btnExportProfileExcel.Text = "تصدير ملف الطالب إلى Excel";
            this.btnExportProfileExcel.UseVisualStyleBackColor = false;



            this.headerActionsPanel.AutoScroll = true;
            this.headerActionsPanel.BackColor = System.Drawing.Color.Transparent;
            this.headerActionsPanel.Controls.Add(this.btnBack);
            this.headerActionsPanel.Controls.Add(this.btnRefresh);
            this.headerActionsPanel.Controls.Add(this.btnPrintProfile);
            this.headerActionsPanel.Controls.Add(this.btnExportProfilePdf);
            this.headerActionsPanel.Controls.Add(this.btnExportProfileExcel);
            this.headerActionsPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.headerActionsPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.headerActionsPanel.Location = new System.Drawing.Point(16, 8);
            this.headerActionsPanel.Name = "headerActionsPanel";
            this.headerActionsPanel.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.headerActionsPanel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.headerActionsPanel.Size = new System.Drawing.Size(734, 56);
            this.headerActionsPanel.TabIndex = 5;
            this.headerActionsPanel.WrapContents = false;



            this.summaryTable.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.summaryTable.ColumnCount = 3;
            this.summaryTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.333F));
            this.summaryTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.333F));
            this.summaryTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.334F));
            this.summaryTable.Controls.Add(this.lblIdentity, 0, 0);
            this.summaryTable.Controls.Add(this.lblContact, 1, 0);
            this.summaryTable.Controls.Add(this.lblClassStatus, 2, 0);
            this.summaryTable.Controls.Add(this.lblAttendanceSummary, 0, 1);
            this.summaryTable.Controls.Add(this.lblAcademicSummary, 1, 1);
            this.summaryTable.Controls.Add(this.lblFinancialSummary, 2, 1);
            this.summaryTable.Dock = System.Windows.Forms.DockStyle.Top;
            this.summaryTable.Location = new System.Drawing.Point(0, 72);
            this.summaryTable.Name = "summaryTable";
            this.summaryTable.Padding = new System.Windows.Forms.Padding(14, 12, 14, 12);
            this.summaryTable.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.summaryTable.RowCount = 2;
            this.summaryTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.summaryTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.summaryTable.Size = new System.Drawing.Size(1027, 216);
            this.summaryTable.TabIndex = 1;



            this.lblIdentity.BackColor = System.Drawing.Color.White;
            this.lblIdentity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblIdentity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblIdentity.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblIdentity.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblIdentity.Location = new System.Drawing.Point(686, 17);
            this.lblIdentity.Margin = new System.Windows.Forms.Padding(5);
            this.lblIdentity.Name = "lblIdentity";
            this.lblIdentity.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.lblIdentity.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblIdentity.Size = new System.Drawing.Size(322, 95);
            this.lblIdentity.TabIndex = 0;
            this.lblIdentity.Text = "بيانات الطالب";
            this.lblIdentity.TextAlign = System.Drawing.ContentAlignment.MiddleRight;



            this.lblContact.BackColor = System.Drawing.Color.White;
            this.lblContact.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblContact.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblContact.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblContact.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblContact.Location = new System.Drawing.Point(354, 17);
            this.lblContact.Margin = new System.Windows.Forms.Padding(5);
            this.lblContact.Name = "lblContact";
            this.lblContact.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.lblContact.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblContact.Size = new System.Drawing.Size(322, 95);
            this.lblContact.TabIndex = 1;
            this.lblContact.Text = "بيانات التواصل";
            this.lblContact.TextAlign = System.Drawing.ContentAlignment.MiddleRight;



            this.lblClassStatus.BackColor = System.Drawing.Color.White;
            this.lblClassStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblClassStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblClassStatus.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblClassStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblClassStatus.Location = new System.Drawing.Point(19, 17);
            this.lblClassStatus.Margin = new System.Windows.Forms.Padding(5);
            this.lblClassStatus.Name = "lblClassStatus";
            this.lblClassStatus.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.lblClassStatus.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblClassStatus.Size = new System.Drawing.Size(325, 95);
            this.lblClassStatus.TabIndex = 2;
            this.lblClassStatus.Text = "الصف والحالة";
            this.lblClassStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;



            this.lblAttendanceSummary.BackColor = System.Drawing.Color.White;
            this.lblAttendanceSummary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAttendanceSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAttendanceSummary.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblAttendanceSummary.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblAttendanceSummary.Location = new System.Drawing.Point(686, 122);
            this.lblAttendanceSummary.Margin = new System.Windows.Forms.Padding(5);
            this.lblAttendanceSummary.Name = "lblAttendanceSummary";
            this.lblAttendanceSummary.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
            this.lblAttendanceSummary.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblAttendanceSummary.Size = new System.Drawing.Size(322, 77);
            this.lblAttendanceSummary.TabIndex = 3;
            this.lblAttendanceSummary.Text = "الحضور والانتظام";
            this.lblAttendanceSummary.TextAlign = System.Drawing.ContentAlignment.MiddleRight;



            this.lblAcademicSummary.BackColor = System.Drawing.Color.White;
            this.lblAcademicSummary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAcademicSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAcademicSummary.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblAcademicSummary.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblAcademicSummary.Location = new System.Drawing.Point(354, 122);
            this.lblAcademicSummary.Margin = new System.Windows.Forms.Padding(5);
            this.lblAcademicSummary.Name = "lblAcademicSummary";
            this.lblAcademicSummary.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
            this.lblAcademicSummary.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblAcademicSummary.Size = new System.Drawing.Size(322, 77);
            this.lblAcademicSummary.TabIndex = 4;
            this.lblAcademicSummary.Text = "الأداء الأكاديمي";
            this.lblAcademicSummary.TextAlign = System.Drawing.ContentAlignment.MiddleRight;



            this.lblFinancialSummary.BackColor = System.Drawing.Color.White;
            this.lblFinancialSummary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblFinancialSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFinancialSummary.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblFinancialSummary.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblFinancialSummary.Location = new System.Drawing.Point(19, 122);
            this.lblFinancialSummary.Margin = new System.Windows.Forms.Padding(5);
            this.lblFinancialSummary.Name = "lblFinancialSummary";
            this.lblFinancialSummary.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.lblFinancialSummary.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblFinancialSummary.Size = new System.Drawing.Size(325, 77);
            this.lblFinancialSummary.TabIndex = 5;
            this.lblFinancialSummary.Text = "الوضع المالي";
            this.lblFinancialSummary.TextAlign = System.Drawing.ContentAlignment.MiddleRight;



            this.tabs.Controls.Add(this.attendanceTab);
            this.tabs.Controls.Add(this.marksTab);
            this.tabs.Controls.Add(this.feesTab);
            this.tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabs.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.tabs.Location = new System.Drawing.Point(0, 288);
            this.tabs.Name = "tabs";
            this.tabs.Padding = new System.Drawing.Point(14, 6);
            this.tabs.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tabs.RightToLeftLayout = true;
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(1027, 352);
            this.tabs.TabIndex = 0;



            this.attendanceTab.BackColor = System.Drawing.Color.White;
            this.attendanceTab.Controls.Add(this.dgvAttendance);
            this.attendanceTab.Location = new System.Drawing.Point(4, 36);
            this.attendanceTab.Name = "attendanceTab";
            this.attendanceTab.Padding = new System.Windows.Forms.Padding(10);
            this.attendanceTab.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.attendanceTab.Size = new System.Drawing.Size(1019, 312);
            this.attendanceTab.TabIndex = 0;
            this.attendanceTab.Text = "الحضور والغياب";



            this.dgvAttendance.ColumnHeadersHeight = 29;
            this.dgvAttendance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAttendance.Location = new System.Drawing.Point(10, 10);
            this.dgvAttendance.Name = "dgvAttendance";
            this.dgvAttendance.RowHeadersWidth = 51;
            this.dgvAttendance.Size = new System.Drawing.Size(999, 292);
            this.dgvAttendance.TabIndex = 0;



            this.marksTab.BackColor = System.Drawing.Color.White;
            this.marksTab.Controls.Add(this.dgvMarks);
            this.marksTab.Location = new System.Drawing.Point(4, 36);
            this.marksTab.Name = "marksTab";
            this.marksTab.Padding = new System.Windows.Forms.Padding(10);
            this.marksTab.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.marksTab.Size = new System.Drawing.Size(1019, 312);
            this.marksTab.TabIndex = 1;
            this.marksTab.Text = "الدرجات الأكاديمية";



            this.dgvMarks.ColumnHeadersHeight = 29;
            this.dgvMarks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMarks.Location = new System.Drawing.Point(10, 10);
            this.dgvMarks.Name = "dgvMarks";
            this.dgvMarks.RowHeadersWidth = 51;
            this.dgvMarks.Size = new System.Drawing.Size(999, 292);
            this.dgvMarks.TabIndex = 0;



            this.feesTab.BackColor = System.Drawing.Color.White;
            this.feesTab.Controls.Add(this.dgvFees);
            this.feesTab.Location = new System.Drawing.Point(4, 36);
            this.feesTab.Name = "feesTab";
            this.feesTab.Padding = new System.Windows.Forms.Padding(10);
            this.feesTab.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.feesTab.Size = new System.Drawing.Size(1019, 312);
            this.feesTab.TabIndex = 2;
            this.feesTab.Text = "الرسوم والمدفوعات";



            this.dgvFees.ColumnHeadersHeight = 29;
            this.dgvFees.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvFees.Location = new System.Drawing.Point(10, 10);
            this.dgvFees.Name = "dgvFees";
            this.dgvFees.RowHeadersWidth = 51;
            this.dgvFees.Size = new System.Drawing.Size(999, 292);
            this.dgvFees.TabIndex = 0;



            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.Controls.Add(this.tabs);
            this.Controls.Add(this.summaryTable);
            this.Controls.Add(this.headerPanel);
            this.Name = "StudentProfileForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.Size = new System.Drawing.Size(1027, 640);
            this.headerActionsPanel.ResumeLayout(false);

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
