namespace SchoolSystem.UI
{
    partial class StudentProfileForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.PictureBox studentPictureBox;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnRefresh;
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
            this.components = new System.ComponentModel.Container();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.studentPictureBox = new System.Windows.Forms.PictureBox();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.summaryTable = new System.Windows.Forms.TableLayoutPanel();
            this.lblIdentity = new System.Windows.Forms.Label();
            this.lblContact = new System.Windows.Forms.Label();
            this.lblClassStatus = new System.Windows.Forms.Label();
            this.lblFinancialSummary = new System.Windows.Forms.Label();
            this.lblAttendanceSummary = new System.Windows.Forms.Label();
            this.lblAcademicSummary = new System.Windows.Forms.Label();
            this.tabs = new System.Windows.Forms.TabControl();
            this.attendanceTab = new System.Windows.Forms.TabPage();
            this.marksTab = new System.Windows.Forms.TabPage();
            this.feesTab = new System.Windows.Forms.TabPage();
            this.dgvAttendance = new System.Windows.Forms.DataGridView();
            this.dgvMarks = new System.Windows.Forms.DataGridView();
            this.dgvFees = new System.Windows.Forms.DataGridView();
            this.headerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.studentPictureBox)).BeginInit();
            this.summaryTable.SuspendLayout();
            this.tabs.SuspendLayout();
            this.attendanceTab.SuspendLayout();
            this.marksTab.SuspendLayout();
            this.feesTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAttendance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMarks)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFees)).BeginInit();
            this.SuspendLayout();

            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.Name = "StudentProfileForm";
            this.Text = "ملف الطالب الموحد";
            this.Size = new System.Drawing.Size(980, 620);

            this.headerPanel.BackColor = System.Drawing.Color.FromArgb(31, 78, 121);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Padding = new System.Windows.Forms.Padding(16, 8, 16, 8);
            this.headerPanel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.headerPanel.Size = new System.Drawing.Size(980, 72);
            this.headerPanel.Controls.Add(this.studentPictureBox);
            this.headerPanel.Controls.Add(this.lblTitle);
            this.headerPanel.Controls.Add(this.btnBack);
            this.headerPanel.Controls.Add(this.btnRefresh);

            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Text = "ملف الطالب الموحد";
            this.lblTitle.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            this.studentPictureBox.BackColor = System.Drawing.Color.FromArgb(226, 232, 240);
            this.studentPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.studentPictureBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.studentPictureBox.Size = new System.Drawing.Size(72, 54);
            this.studentPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.studentPictureBox.TabStop = false;

            this.btnBack.BackColor = System.Drawing.Color.FromArgb(127, 140, 141);
            this.btnBack.FlatAppearance.BorderSize = 0;
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnBack.ForeColor = System.Drawing.Color.White;
            this.btnBack.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnBack.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnBack.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnBack.Text = "رجوع";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Width = 100;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);

            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(0, 150, 136);
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnRefresh.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnRefresh.Text = "تحديث";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Width = 100;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);

            this.summaryTable.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.summaryTable.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
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
            this.summaryTable.Padding = new System.Windows.Forms.Padding(14, 12, 14, 12);
            this.summaryTable.RowCount = 2;
            this.summaryTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.summaryTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.summaryTable.Size = new System.Drawing.Size(980, 216);

            this.lblIdentity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblIdentity.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblIdentity.BackColor = System.Drawing.Color.White;
            this.lblIdentity.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblIdentity.AutoSize = false;
            this.lblIdentity.Margin = new System.Windows.Forms.Padding(5);
            this.lblIdentity.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.lblIdentity.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblIdentity.Text = "بيانات الطالب";
            this.lblIdentity.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblIdentity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            this.lblContact.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblContact.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblContact.BackColor = System.Drawing.Color.White;
            this.lblContact.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblContact.AutoSize = false;
            this.lblContact.Margin = new System.Windows.Forms.Padding(5);
            this.lblContact.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.lblContact.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblContact.Text = "بيانات التواصل";
            this.lblContact.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblContact.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            this.lblClassStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblClassStatus.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblClassStatus.BackColor = System.Drawing.Color.White;
            this.lblClassStatus.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblClassStatus.AutoSize = false;
            this.lblClassStatus.Margin = new System.Windows.Forms.Padding(5);
            this.lblClassStatus.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.lblClassStatus.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblClassStatus.Text = "الصف والحالة";
            this.lblClassStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblClassStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            this.lblFinancialSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFinancialSummary.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblFinancialSummary.BackColor = System.Drawing.Color.White;
            this.lblFinancialSummary.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblFinancialSummary.AutoSize = false;
            this.lblFinancialSummary.Margin = new System.Windows.Forms.Padding(5);
            this.lblFinancialSummary.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.lblFinancialSummary.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblFinancialSummary.Text = "الوضع المالي";
            this.lblFinancialSummary.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblFinancialSummary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            this.lblAttendanceSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAttendanceSummary.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblAttendanceSummary.BackColor = System.Drawing.Color.White;
            this.lblAttendanceSummary.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblAttendanceSummary.AutoSize = false;
            this.lblAttendanceSummary.Margin = new System.Windows.Forms.Padding(5);
            this.lblAttendanceSummary.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
            this.lblAttendanceSummary.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblAttendanceSummary.Text = "الحضور والانتظام";
            this.lblAttendanceSummary.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblAttendanceSummary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            this.lblAcademicSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAcademicSummary.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblAcademicSummary.BackColor = System.Drawing.Color.White;
            this.lblAcademicSummary.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblAcademicSummary.AutoSize = false;
            this.lblAcademicSummary.Margin = new System.Windows.Forms.Padding(5);
            this.lblAcademicSummary.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
            this.lblAcademicSummary.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblAcademicSummary.Text = "الأداء الأكاديمي";
            this.lblAcademicSummary.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblAcademicSummary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            this.tabs.Controls.Add(this.attendanceTab);
            this.tabs.Controls.Add(this.marksTab);
            this.tabs.Controls.Add(this.feesTab);
            this.tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabs.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.tabs.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tabs.Padding = new System.Drawing.Point(14, 6);
            this.tabs.RightToLeftLayout = true;

            this.attendanceTab.BackColor = System.Drawing.Color.White;
            this.attendanceTab.Controls.Add(this.dgvAttendance);
            this.attendanceTab.Padding = new System.Windows.Forms.Padding(10);
            this.attendanceTab.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.attendanceTab.Text = "الحضور والغياب";

            this.marksTab.BackColor = System.Drawing.Color.White;
            this.marksTab.Controls.Add(this.dgvMarks);
            this.marksTab.Padding = new System.Windows.Forms.Padding(10);
            this.marksTab.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.marksTab.Text = "الدرجات الأكاديمية";

            this.feesTab.BackColor = System.Drawing.Color.White;
            this.feesTab.Controls.Add(this.dgvFees);
            this.feesTab.Padding = new System.Windows.Forms.Padding(10);
            this.feesTab.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.feesTab.Text = "الرسوم والمدفوعات";

            this.Controls.Add(this.tabs);
            this.Controls.Add(this.summaryTable);
            this.Controls.Add(this.headerPanel);
            this.headerPanel.ResumeLayout(false);
            this.summaryTable.ResumeLayout(false);
            this.tabs.ResumeLayout(false);
            this.attendanceTab.ResumeLayout(false);
            this.marksTab.ResumeLayout(false);
            this.feesTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAttendance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMarks)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFees)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.studentPictureBox)).EndInit();
            this.ResumeLayout(false);
        }

    }
}
