namespace SchoolSystem.UI
{
    partial class DashboardHome
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region كود المصمم

        private void InitializeComponent()
        {
            this.tableLayoutMain = new System.Windows.Forms.TableLayoutPanel();
            this.panelCards = new System.Windows.Forms.TableLayoutPanel();
            this.panelChart = new System.Windows.Forms.Panel();
            this.lblChartTitle = new System.Windows.Forms.Label();
            this.panelAlerts = new System.Windows.Forms.Panel();
            this.lblAlertsTitle = new System.Windows.Forms.Label();
            this.lblPendingFees = new System.Windows.Forms.Label();
            this.lblTodayAbsence = new System.Windows.Forms.Label();

            this.tableLayoutMain.SuspendLayout();
            this.panelCards.SuspendLayout();
            this.panelChart.SuspendLayout();
            this.panelAlerts.SuspendLayout();
            this.SuspendLayout();

            // 
            // tableLayoutMain
            // 
            this.tableLayoutMain.ColumnCount = 2;
            this.tableLayoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutMain.Controls.Add(this.panelCards, 0, 0);
            this.tableLayoutMain.Controls.Add(this.panelChart, 0, 1);
            this.tableLayoutMain.Controls.Add(this.panelAlerts, 1, 0);
            this.tableLayoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutMain.Location = new System.Drawing.Point(15, 15);
            this.tableLayoutMain.Name = "tableLayoutMain";
            this.tableLayoutMain.RowCount = 2;
            this.tableLayoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.tableLayoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutMain.Padding = new System.Windows.Forms.Padding(10);
            this.tableLayoutMain.Size = new System.Drawing.Size(920, 535);
            this.tableLayoutMain.TabIndex = 0;

            // 
            // panelCards (بطاقات الإحصائيات)
            // 
            this.panelCards.ColumnCount = 5;
            this.panelCards.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.panelCards.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.panelCards.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.panelCards.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.panelCards.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.panelCards.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCards.RowCount = 1;
            this.panelCards.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelCards.TabIndex = 0;

            // سنقوم بإنشاء البطاقات برمجياً في الكود (انظر DashboardHome.cs)

            // 
            // panelChart (الرسم البياني)
            // 
            this.panelChart.BackColor = System.Drawing.Color.White;
            this.panelChart.Controls.Add(this.lblChartTitle);
            this.panelChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelChart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelChart.Padding = new System.Windows.Forms.Padding(10);

            this.lblChartTitle.Text = "📊  توزيع الطلاب في الفصول";
            this.lblChartTitle.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.lblChartTitle.ForeColor = System.Drawing.Color.FromArgb(33, 42, 57);
            this.lblChartTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblChartTitle.Height = 25;

            // الرسم البياني سيتم بناؤه برمجياً أيضاً

            // 
            // panelAlerts (التنبيهات)
            // 
            this.panelAlerts.BackColor = System.Drawing.Color.FromArgb(255, 245, 238);
            this.panelAlerts.Controls.Add(this.lblAlertsTitle);
            this.panelAlerts.Controls.Add(this.lblPendingFees);
            this.panelAlerts.Controls.Add(this.lblTodayAbsence);
            this.tableLayoutMain.SetRowSpan(this.panelAlerts, 2);
            this.panelAlerts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelAlerts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelAlerts.Padding = new System.Windows.Forms.Padding(10);

            this.lblAlertsTitle.Text = "🔔  تنبيهات";
            this.lblAlertsTitle.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.lblAlertsTitle.ForeColor = System.Drawing.Color.FromArgb(33, 42, 57);
            this.lblAlertsTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblAlertsTitle.Height = 25;

            // سيتم تحديث النصوص من الكود

            // 
            // DashboardHome
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(245, 245, 245);
            this.Controls.Add(this.tableLayoutMain);
            this.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.Name = "DashboardHome";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Size = new System.Drawing.Size(950, 560);
            this.tableLayoutMain.ResumeLayout(false);
            this.panelCards.ResumeLayout(false);
            this.panelChart.ResumeLayout(false);
            this.panelAlerts.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutMain;
        private System.Windows.Forms.TableLayoutPanel panelCards;
        private System.Windows.Forms.Panel panelChart;
        private System.Windows.Forms.Label lblChartTitle;
        private System.Windows.Forms.Panel panelAlerts;
        private System.Windows.Forms.Label lblAlertsTitle;
        private System.Windows.Forms.Label lblPendingFees;
        private System.Windows.Forms.Label lblTodayAbsence;
    }
}