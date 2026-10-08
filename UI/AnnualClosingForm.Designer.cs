using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;


namespace SchoolSystem.UI
{

    partial class AnnualClosingForm
    {

        private IContainer components = null;

        private TableLayoutPanel rootLayout;

        private Label titleLabel;

        private Label yearLabel;

        private ComboBox cmbYear;

        private Label nextYearLabel;

        private TableLayoutPanel nextYearLayout;

        private TextBox txtNextYear;

        private TextBox txtNotes;

        private Label targetLabel;

        private TableLayoutPanel targetLayout;

        private TextBox txtTargetClassId;

        private TextBox txtTargetSection;

        private FlowLayoutPanel actionsPanel;

        private Button btnVerify;

        private Button btnPlan;

        private Button btnMigrationReport;

        private Button btnApproveMigration;

        private Button btnClose;

        private Label lblStatus;

        private DataGridView grid;

        // تعريف الدالة Dispose لتنفيذ العملية المرتبطة بها.
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();

            base.Dispose(disposing);
        }


        private void InitializeComponent()
        {
            this.rootLayout = new System.Windows.Forms.TableLayoutPanel();
            this.titleLabel = new System.Windows.Forms.Label();
            this.yearLabel = new System.Windows.Forms.Label();
            this.cmbYear = new System.Windows.Forms.ComboBox();
            this.nextYearLabel = new System.Windows.Forms.Label();
            this.nextYearLayout = new System.Windows.Forms.TableLayoutPanel();
            this.txtNextYear = new System.Windows.Forms.TextBox();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.targetLabel = new System.Windows.Forms.Label();
            this.targetLayout = new System.Windows.Forms.TableLayoutPanel();
            this.txtTargetClassId = new System.Windows.Forms.TextBox();
            this.txtTargetSection = new System.Windows.Forms.TextBox();
            this.actionsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btnVerify = new System.Windows.Forms.Button();
            this.btnPlan = new System.Windows.Forms.Button();
            this.btnMigrationReport = new System.Windows.Forms.Button();
            this.btnApproveMigration = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.grid = new System.Windows.Forms.DataGridView();
            this.rootLayout.SuspendLayout();
            this.nextYearLayout.SuspendLayout();
            this.targetLayout.SuspendLayout();
            this.actionsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.SuspendLayout();
            // 
            // rootLayout
            // 
            this.rootLayout.BackColor = System.Drawing.Color.White;
            this.rootLayout.ColumnCount = 2;
            this.rootLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 190F));
            this.rootLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.rootLayout.Controls.Add(this.titleLabel, 0, 0);
            this.rootLayout.Controls.Add(this.yearLabel, 0, 1);
            this.rootLayout.Controls.Add(this.cmbYear, 1, 1);
            this.rootLayout.Controls.Add(this.nextYearLabel, 0, 2);
            this.rootLayout.Controls.Add(this.nextYearLayout, 1, 2);
            this.rootLayout.Controls.Add(this.targetLabel, 0, 3);
            this.rootLayout.Controls.Add(this.targetLayout, 1, 3);
            this.rootLayout.Controls.Add(this.actionsPanel, 0, 4);
            this.rootLayout.Controls.Add(this.lblStatus, 0, 5);
            this.rootLayout.Controls.Add(this.grid, 0, 6);
            this.rootLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rootLayout.Location = new System.Drawing.Point(0, 0);
            this.rootLayout.Name = "rootLayout";
            this.rootLayout.Padding = new System.Windows.Forms.Padding(16);
            this.rootLayout.RowCount = 7;
            this.rootLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.rootLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.rootLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 82F));
            this.rootLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.rootLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 96F));
            this.rootLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.rootLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.rootLayout.Size = new System.Drawing.Size(1100, 680);
            this.rootLayout.TabIndex = 0;
            // 
            // titleLabel
            // 
            this.rootLayout.SetColumnSpan(this.titleLabel, 2);
            this.titleLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.titleLabel.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.titleLabel.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.titleLabel.ForeColor = System.Drawing.Color.White;
            this.titleLabel.Location = new System.Drawing.Point(16, 16);
            this.titleLabel.Margin = new System.Windows.Forms.Padding(0);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(1068, 60);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "الإغلاق السنوي والترحيل";
            this.titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // yearLabel
            // 
            this.yearLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.yearLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.yearLabel.Location = new System.Drawing.Point(897, 71);
            this.yearLabel.Margin = new System.Windows.Forms.Padding(3);
            this.yearLabel.Name = "yearLabel";
            this.yearLabel.Size = new System.Drawing.Size(184, 36);
            this.yearLabel.TabIndex = 1;
            this.yearLabel.Text = "العام المراد إغلاقه";
            this.yearLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbYear
            // 
            this.cmbYear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbYear.Font = new System.Drawing.Font("Tahoma", 10F);
            this.cmbYear.Location = new System.Drawing.Point(19, 74);
            this.cmbYear.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.cmbYear.Name = "cmbYear";
            this.cmbYear.Size = new System.Drawing.Size(872, 29);
            this.cmbYear.TabIndex = 2;
            // 
            // nextYearLabel
            // 
            this.nextYearLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nextYearLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.nextYearLabel.Location = new System.Drawing.Point(897, 113);
            this.nextYearLabel.Margin = new System.Windows.Forms.Padding(3);
            this.nextYearLabel.Name = "nextYearLabel";
            this.nextYearLabel.Size = new System.Drawing.Size(184, 76);
            this.nextYearLabel.TabIndex = 3;
            this.nextYearLabel.Text = "العام التالي والملاحظات";
            this.nextYearLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // nextYearLayout
            // 
            this.nextYearLayout.ColumnCount = 2;
            this.nextYearLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38F));
            this.nextYearLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 62F));
            this.nextYearLayout.Controls.Add(this.txtNextYear, 0, 0);
            this.nextYearLayout.Controls.Add(this.txtNotes, 1, 0);
            this.nextYearLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nextYearLayout.Location = new System.Drawing.Point(19, 113);
            this.nextYearLayout.Name = "nextYearLayout";
            this.nextYearLayout.RowCount = 1;
            this.nextYearLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.nextYearLayout.Size = new System.Drawing.Size(872, 76);
            this.nextYearLayout.TabIndex = 4;
            // 
            // txtNextYear
            // 
            this.txtNextYear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNextYear.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtNextYear.Location = new System.Drawing.Point(549, 14);
            this.txtNextYear.Margin = new System.Windows.Forms.Padding(3, 14, 8, 14);
            this.txtNextYear.MaxLength = 20;
            this.txtNextYear.Name = "txtNextYear";
            this.txtNextYear.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtNextYear.Size = new System.Drawing.Size(320, 28);
            this.txtNextYear.TabIndex = 0;
            // 
            // txtNotes
            // 
            this.txtNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNotes.Font = new System.Drawing.Font("Tahoma", 9F);
            this.txtNotes.Location = new System.Drawing.Point(3, 6);
            this.txtNotes.Margin = new System.Windows.Forms.Padding(8, 6, 3, 6);
            this.txtNotes.MaxLength = 1000;
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtNotes.Size = new System.Drawing.Size(530, 64);
            this.txtNotes.TabIndex = 1;
            // 
            // targetLabel
            // 
            this.targetLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.targetLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.targetLabel.Location = new System.Drawing.Point(897, 195);
            this.targetLabel.Margin = new System.Windows.Forms.Padding(3);
            this.targetLabel.Name = "targetLabel";
            this.targetLabel.Size = new System.Drawing.Size(184, 42);
            this.targetLabel.TabIndex = 5;
            this.targetLabel.Text = "الصف والشعبة الجديدة للاعتماد";
            this.targetLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // targetLayout
            // 
            this.targetLayout.ColumnCount = 2;
            this.targetLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38F));
            this.targetLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 62F));
            this.targetLayout.Controls.Add(this.txtTargetClassId, 0, 0);
            this.targetLayout.Controls.Add(this.txtTargetSection, 1, 0);
            this.targetLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.targetLayout.Location = new System.Drawing.Point(19, 195);
            this.targetLayout.Name = "targetLayout";
            this.targetLayout.RowCount = 1;
            this.targetLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.targetLayout.Size = new System.Drawing.Size(872, 42);
            this.targetLayout.TabIndex = 6;
            // 
            // txtTargetClassId
            // 
            this.txtTargetClassId.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTargetClassId.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtTargetClassId.Location = new System.Drawing.Point(549, 7);
            this.txtTargetClassId.Margin = new System.Windows.Forms.Padding(3, 7, 8, 7);
            this.txtTargetClassId.MaxLength = 10;
            this.txtTargetClassId.Name = "txtTargetClassId";
            this.txtTargetClassId.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtTargetClassId.Size = new System.Drawing.Size(320, 28);
            this.txtTargetClassId.TabIndex = 0;
            // 
            // txtTargetSection
            // 
            this.txtTargetSection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTargetSection.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtTargetSection.Location = new System.Drawing.Point(3, 7);
            this.txtTargetSection.Margin = new System.Windows.Forms.Padding(8, 7, 3, 7);
            this.txtTargetSection.MaxLength = 50;
            this.txtTargetSection.Name = "txtTargetSection";
            this.txtTargetSection.Size = new System.Drawing.Size(530, 28);
            this.txtTargetSection.TabIndex = 1;
            // 
            // actionsPanel
            // 
            this.actionsPanel.AutoScroll = true;
            this.rootLayout.SetColumnSpan(this.actionsPanel, 2);
            this.actionsPanel.Controls.Add(this.btnVerify);
            this.actionsPanel.Controls.Add(this.btnPlan);
            this.actionsPanel.Controls.Add(this.btnMigrationReport);
            this.actionsPanel.Controls.Add(this.btnApproveMigration);
            this.actionsPanel.Controls.Add(this.btnClose);
            this.actionsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.actionsPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.actionsPanel.Location = new System.Drawing.Point(16, 244);
            this.actionsPanel.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.actionsPanel.Name = "actionsPanel";
            this.actionsPanel.Size = new System.Drawing.Size(1068, 88);
            this.actionsPanel.TabIndex = 7;
            // 
            // btnVerify
            // 
            this.btnVerify.AutoSize = true;
            this.btnVerify.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnVerify.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(119)))), ((int)(((byte)(180)))));
            this.btnVerify.FlatAppearance.BorderSize = 0;
            this.btnVerify.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVerify.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnVerify.ForeColor = System.Drawing.Color.White;
            this.btnVerify.Location = new System.Drawing.Point(5, 5);
            this.btnVerify.Margin = new System.Windows.Forms.Padding(5);
            this.btnVerify.MinimumSize = new System.Drawing.Size(160, 40);
            this.btnVerify.Name = "btnVerify";
            this.btnVerify.Padding = new System.Windows.Forms.Padding(12, 0, 12, 0);
            this.btnVerify.Size = new System.Drawing.Size(192, 40);
            this.btnVerify.TabIndex = 0;
            this.btnVerify.Text = "فحص جاهزية الإغلاق";
            this.btnVerify.UseVisualStyleBackColor = false;
            this.btnVerify.Click += new System.EventHandler(this.VerifyClick);
            // 
            // btnPlan
            // 
            this.btnPlan.AutoSize = true;
            this.btnPlan.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnPlan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(173)))), ((int)(((byte)(71)))));
            this.btnPlan.FlatAppearance.BorderSize = 0;
            this.btnPlan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPlan.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnPlan.ForeColor = System.Drawing.Color.White;
            this.btnPlan.Location = new System.Drawing.Point(207, 5);
            this.btnPlan.Margin = new System.Windows.Forms.Padding(5);
            this.btnPlan.MinimumSize = new System.Drawing.Size(160, 40);
            this.btnPlan.Name = "btnPlan";
            this.btnPlan.Padding = new System.Windows.Forms.Padding(12, 0, 12, 0);
            this.btnPlan.Size = new System.Drawing.Size(173, 40);
            this.btnPlan.TabIndex = 1;
            this.btnPlan.Text = "إنشاء خطة الترحيل";
            this.btnPlan.UseVisualStyleBackColor = false;
            this.btnPlan.Click += new System.EventHandler(this.PlanClick);
            // 
            // btnMigrationReport
            // 
            this.btnMigrationReport.AutoSize = true;
            this.btnMigrationReport.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnMigrationReport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(155)))), ((int)(((byte)(213)))));
            this.btnMigrationReport.FlatAppearance.BorderSize = 0;
            this.btnMigrationReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMigrationReport.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnMigrationReport.ForeColor = System.Drawing.Color.White;
            this.btnMigrationReport.Location = new System.Drawing.Point(390, 5);
            this.btnMigrationReport.Margin = new System.Windows.Forms.Padding(5);
            this.btnMigrationReport.MinimumSize = new System.Drawing.Size(160, 40);
            this.btnMigrationReport.Name = "btnMigrationReport";
            this.btnMigrationReport.Padding = new System.Windows.Forms.Padding(12, 0, 12, 0);
            this.btnMigrationReport.Size = new System.Drawing.Size(176, 40);
            this.btnMigrationReport.TabIndex = 2;
            this.btnMigrationReport.Text = "عرض تقرير الترحيل";
            this.btnMigrationReport.UseVisualStyleBackColor = false;
            this.btnMigrationReport.Click += new System.EventHandler(this.MigrationReportClick);
            // 
            // btnApproveMigration
            // 
            this.btnApproveMigration.AutoSize = true;
            this.btnApproveMigration.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnApproveMigration.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(125)))), ((int)(((byte)(49)))));
            this.btnApproveMigration.FlatAppearance.BorderSize = 0;
            this.btnApproveMigration.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApproveMigration.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnApproveMigration.ForeColor = System.Drawing.Color.White;
            this.btnApproveMigration.Location = new System.Drawing.Point(576, 5);
            this.btnApproveMigration.Margin = new System.Windows.Forms.Padding(5);
            this.btnApproveMigration.MinimumSize = new System.Drawing.Size(160, 40);
            this.btnApproveMigration.Name = "btnApproveMigration";
            this.btnApproveMigration.Padding = new System.Windows.Forms.Padding(12, 0, 12, 0);
            this.btnApproveMigration.Size = new System.Drawing.Size(192, 40);
            this.btnApproveMigration.TabIndex = 3;
            this.btnApproveMigration.Text = "اعتماد الترحيل المحدد";
            this.btnApproveMigration.UseVisualStyleBackColor = false;
            this.btnApproveMigration.Click += new System.EventHandler(this.ApproveMigrationClick);
            // 
            // btnClose
            // 
            this.btnClose.AutoSize = true;
            this.btnClose.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(80)))), ((int)(((byte)(77)))));
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(778, 5);
            this.btnClose.Margin = new System.Windows.Forms.Padding(5);
            this.btnClose.MinimumSize = new System.Drawing.Size(160, 40);
            this.btnClose.Name = "btnClose";
            this.btnClose.Padding = new System.Windows.Forms.Padding(12, 0, 12, 0);
            this.btnClose.Size = new System.Drawing.Size(160, 40);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "تنفيذ إغلاق العام";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.CloseClick);
            // 
            // lblStatus
            // 
            this.rootLayout.SetColumnSpan(this.lblStatus, 2);
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatus.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblStatus.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.lblStatus.Location = new System.Drawing.Point(19, 339);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(3);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(1062, 36);
            this.lblStatus.TabIndex = 8;
            this.lblStatus.Text = "ابدأ بفحص المطابقة قبل الإغلاق.";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // grid
            // 
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToDeleteRows = false;
            this.grid.AllowUserToResizeRows = false;
            this.grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grid.BackgroundColor = System.Drawing.Color.White;
            this.grid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.rootLayout.SetColumnSpan(this.grid, 2);
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.Location = new System.Drawing.Point(19, 381);
            this.grid.Name = "grid";
            this.grid.ReadOnly = true;
            this.grid.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.grid.RowHeadersVisible = false;
            this.grid.RowHeadersWidth = 51;
            this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid.Size = new System.Drawing.Size(1062, 280);
            this.grid.TabIndex = 9;
            // 
            // AnnualClosingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1100, 680);
            this.Controls.Add(this.rootLayout);
            this.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(900, 580);
            this.Name = "AnnualClosingForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "الإغلاق السنوي والترحيل";
            this.rootLayout.ResumeLayout(false);
            this.nextYearLayout.ResumeLayout(false);
            this.nextYearLayout.PerformLayout();
            this.targetLayout.ResumeLayout(false);
            this.targetLayout.PerformLayout();
            this.actionsPanel.ResumeLayout(false);
            this.actionsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
