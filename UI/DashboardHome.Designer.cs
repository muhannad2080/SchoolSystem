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
            this.panelChart = new Krypton.Toolkit.KryptonPanel();
            this.lblChartTitle = new Krypton.Toolkit.KryptonLabel();
            this.panelAlerts = new Krypton.Toolkit.KryptonPanel();
            this.lblAlertsTitle = new Krypton.Toolkit.KryptonLabel();
            this.lblPendingFees = new Krypton.Toolkit.KryptonLabel();
            this.lblTodayAbsence = new Krypton.Toolkit.KryptonLabel();

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
                        this.panelChart.Padding = new System.Windows.Forms.Padding(10);

            this.lblChartTitle.Text = "توزيع الطلاب في الفصول";
            this.lblChartTitle.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.lblChartTitle.ForeColor = System.Drawing.Color.FromArgb(33, 42, 57);
            this.lblChartTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblChartTitle.Height = 25;

            // الرسم البياني سيتم بناؤه برمجياً أيضاً

            // 
            // panelAlerts (التنبيهات)
            // 
            this.panelAlerts.BackColor = System.Drawing.Color.FromArgb(255, 245, 238);
            this.panelAlerts.Controls.Add(this.lblTodayAbsence);
            this.panelAlerts.Controls.Add(this.lblPendingFees);
            this.panelAlerts.Controls.Add(this.lblAlertsTitle);
            this.tableLayoutMain.SetRowSpan(this.panelAlerts, 2);
            this.panelAlerts.Dock = System.Windows.Forms.DockStyle.Fill;
                        this.panelAlerts.Padding = new System.Windows.Forms.Padding(10);

            this.lblAlertsTitle.Text = "التنبيهات";
            this.lblAlertsTitle.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.lblAlertsTitle.ForeColor = System.Drawing.Color.FromArgb(33, 42, 57);
            this.lblAlertsTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblAlertsTitle.Height = 28;
            this.lblAlertsTitle.Padding = new System.Windows.Forms.Padding(6, 2, 6, 2);

            // 
            // lblPendingFees
            // 
            this.lblPendingFees.AutoSize = false;
            this.lblPendingFees.BackColor = System.Drawing.Color.White;
            this.lblPendingFees.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPendingFees.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblPendingFees.ForeColor = System.Drawing.Color.FromArgb(194, 65, 12);
            this.lblPendingFees.Height = 30;
            this.lblPendingFees.Margin = new System.Windows.Forms.Padding(3);
            this.lblPendingFees.Padding = new System.Windows.Forms.Padding(6, 4, 6, 4);

            // 
            // lblTodayAbsence
            // 
            this.lblTodayAbsence.AutoSize = false;
            this.lblTodayAbsence.BackColor = System.Drawing.Color.White;
            this.lblTodayAbsence.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTodayAbsence.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblTodayAbsence.ForeColor = System.Drawing.Color.FromArgb(180, 39, 39);
            this.lblTodayAbsence.Height = 30;
            this.lblTodayAbsence.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.lblTodayAbsence.Padding = new System.Windows.Forms.Padding(6, 4, 6, 4);

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
        private Krypton.Toolkit.KryptonPanel panelChart;
        private Krypton.Toolkit.KryptonLabel lblChartTitle;
        private Krypton.Toolkit.KryptonPanel panelAlerts;
        private Krypton.Toolkit.KryptonLabel lblAlertsTitle;
        private Krypton.Toolkit.KryptonLabel lblPendingFees;
        private Krypton.Toolkit.KryptonLabel lblTodayAbsence;
    }
}