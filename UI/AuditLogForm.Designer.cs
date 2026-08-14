using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SchoolSystem.UI
{
    public partial class AuditLogForm
    {
        private IContainer components = null;
        private Panel header;
        private Label title;
        private Label subtitle;
        private Panel filters;
        private Label searchLabel;
        private Label toLabel;
        private Label fromLabel;
        private DateTimePicker fromDate;
        private DateTimePicker toDate;
        private TextBox searchBox;
        private Button refreshButton;
        private Button exportButton;
        private DataGridView grid;
        private Panel footer;
        private Label countLabel;
        private Label rangeLabel;
        private Label statusLabel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle();
            DataGridViewCellStyle cellStyle = new DataGridViewCellStyle();
            DataGridViewCellStyle alternateStyle = new DataGridViewCellStyle();
            components = new Container();
            header = new Panel();
            title = new Label();
            subtitle = new Label();
            filters = new Panel();
            searchLabel = new Label();
            toLabel = new Label();
            fromLabel = new Label();
            fromDate = new DateTimePicker();
            toDate = new DateTimePicker();
            searchBox = new TextBox();
            refreshButton = new Button();
            exportButton = new Button();
            grid = new DataGridView();
            footer = new Panel();
            countLabel = new Label();
            rangeLabel = new Label();
            statusLabel = new Label();
            header.SuspendLayout();
            filters.SuspendLayout();
            ((ISupportInitialize)(grid)).BeginInit();
            footer.SuspendLayout();
            SuspendLayout();
            // 
            // header
            // 
            header.BackColor = Color.White;
            header.Controls.Add(subtitle);
            header.Controls.Add(title);
            header.Dock = DockStyle.Top;
            header.Location = new Point(0, 0);
            header.Name = "header";
            header.Padding = new Padding(14, 8, 14, 6);
            header.Size = new Size(1120, 76);
            // 
            // title
            // 
            title.Dock = DockStyle.Top;
            title.Font = new Font(new FontFamily("Tahoma"), 16F, FontStyle.Bold);
            title.ForeColor = Color.FromArgb(15, 23, 42);
            title.Location = new Point(14, 8);
            title.Name = "title";
            title.Size = new Size(1092, 34);
            title.Text = "سجل الأنشطة والعمليات الحساسة";
            title.TextAlign = ContentAlignment.MiddleRight;
            // 
            // subtitle
            // 
            subtitle.Dock = DockStyle.Fill;
            subtitle.Font = new Font(new FontFamily("Tahoma"), 9F, FontStyle.Regular);
            subtitle.ForeColor = Color.FromArgb(71, 85, 105);
            subtitle.Location = new Point(14, 42);
            subtitle.Name = "subtitle";
            subtitle.Size = new Size(1092, 28);
            subtitle.Text = "مراجعة موثقة لتغييرات السندات والدرجات والرسوم والمستخدمين";
            subtitle.TextAlign = ContentAlignment.MiddleRight;
            // 
            // filters
            // 
            filters.BackColor = Color.FromArgb(248, 250, 252);
            filters.Controls.Add(exportButton);
            filters.Controls.Add(refreshButton);
            filters.Controls.Add(searchBox);
            filters.Controls.Add(searchLabel);
            filters.Controls.Add(toDate);
            filters.Controls.Add(toLabel);
            filters.Controls.Add(fromDate);
            filters.Controls.Add(fromLabel);
            filters.Dock = DockStyle.Top;
            filters.Location = new Point(0, 76);
            filters.Name = "filters";
            filters.Padding = new Padding(14, 8, 14, 8);
            filters.Size = new Size(1120, 74);
            // 
            // searchLabel
            // 
            searchLabel.AutoSize = true;
            searchLabel.Dock = DockStyle.Right;
            searchLabel.Font = new Font(new FontFamily("Tahoma"), 9F, FontStyle.Bold);
            searchLabel.Location = new Point(1060, 8);
            searchLabel.Margin = new Padding(4);
            searchLabel.Name = "searchLabel";
            searchLabel.Size = new Size(46, 58);
            searchLabel.Text = "بحث:";
            searchLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // toLabel
            // 
            toLabel.AutoSize = true;
            toLabel.Dock = DockStyle.Right;
            toLabel.Font = new Font(new FontFamily("Tahoma"), 9F, FontStyle.Bold);
            toLabel.Location = new Point(850, 8);
            toLabel.Margin = new Padding(4);
            toLabel.Name = "toLabel";
            toLabel.Size = new Size(27, 58);
            toLabel.Text = "إلى:";
            toLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // fromLabel
            // 
            fromLabel.AutoSize = true;
            fromLabel.Dock = DockStyle.Right;
            fromLabel.Font = new Font(new FontFamily("Tahoma"), 9F, FontStyle.Bold);
            fromLabel.Location = new Point(675, 8);
            fromLabel.Margin = new Padding(4);
            fromLabel.Name = "fromLabel";
            fromLabel.Size = new Size(27, 58);
            fromLabel.Text = "من:";
            fromLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // fromDate
            // 
            fromDate.Dock = DockStyle.Right;
            fromDate.Format = DateTimePickerFormat.Short;
            fromDate.Location = new Point(705, 8);
            fromDate.Margin = new Padding(4);
            fromDate.Name = "fromDate";
            fromDate.Size = new Size(125, 27);
            // 
            // toDate
            // 
            toDate.Dock = DockStyle.Right;
            toDate.Format = DateTimePickerFormat.Short;
            toDate.Location = new Point(880, 8);
            toDate.Margin = new Padding(4);
            toDate.Name = "toDate";
            toDate.Size = new Size(125, 27);
            // 
            // searchBox
            // 
            searchBox.Dock = DockStyle.Right;
            searchBox.Location = new Point(370, 8);
            searchBox.Margin = new Padding(4);
            searchBox.Name = "searchBox";
            searchBox.Size = new Size(220, 27);
            // 
            // refreshButton
            // 
            refreshButton.BackColor = Color.FromArgb(37, 99, 235);
            refreshButton.FlatAppearance.BorderSize = 0;
            refreshButton.FlatStyle = FlatStyle.Flat;
            refreshButton.ForeColor = Color.White;
            refreshButton.Location = new Point(14, 8);
            refreshButton.Margin = new Padding(4);
            refreshButton.Name = "refreshButton";
            refreshButton.Size = new Size(96, 38);
            refreshButton.Text = "تحديث";
            refreshButton.UseVisualStyleBackColor = false;
            refreshButton.Click += new EventHandler(RefreshButton_Click);
            // 
            // exportButton
            // 
            exportButton.BackColor = Color.FromArgb(22, 163, 74);
            exportButton.FlatAppearance.BorderSize = 0;
            exportButton.FlatStyle = FlatStyle.Flat;
            exportButton.ForeColor = Color.White;
            exportButton.Location = new Point(114, 8);
            exportButton.Margin = new Padding(4);
            exportButton.Name = "exportButton";
            exportButton.Size = new Size(110, 38);
            exportButton.Text = "تصدير CSV";
            exportButton.UseVisualStyleBackColor = false;
            exportButton.Click += new EventHandler(ExportButton_Click);
            // 
            // grid
            // 
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.AllowUserToResizeRows = false;
            grid.AutoGenerateColumns = true;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.BackgroundColor = Color.FromArgb(241, 245, 249);
            grid.BorderStyle = BorderStyle.None;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            headerStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            headerStyle.BackColor = Color.FromArgb(31, 41, 55);
            headerStyle.Font = new Font(new FontFamily("Tahoma"), 10F, FontStyle.Bold);
            headerStyle.ForeColor = Color.White;
            headerStyle.Padding = new Padding(6, 4, 6, 4);
            grid.ColumnHeadersDefaultCellStyle = headerStyle;
            grid.ColumnHeadersHeight = 42;
            grid.EnableHeadersVisualStyles = false;
            grid.GridColor = Color.FromArgb(226, 232, 240);
            grid.Location = new Point(0, 150);
            grid.MultiSelect = false;
            grid.Name = "grid";
            grid.ReadOnly = true;
            grid.RowHeadersVisible = false;
            grid.RowTemplate.Height = 36;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            cellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            cellStyle.BackColor = Color.White;
            cellStyle.Font = new Font(new FontFamily("Tahoma"), 10F);
            cellStyle.ForeColor = Color.FromArgb(15, 23, 42);
            cellStyle.SelectionBackColor = Color.FromArgb(219, 234, 254);
            cellStyle.SelectionForeColor = Color.FromArgb(15, 23, 42);
            cellStyle.WrapMode = DataGridViewTriState.False;
            grid.DefaultCellStyle = cellStyle;
            alternateStyle.BackColor = Color.FromArgb(248, 250, 252);
            grid.AlternatingRowsDefaultCellStyle = alternateStyle;
            grid.Dock = DockStyle.Fill;
            grid.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(Grid_DataBindingComplete);
            grid.CellFormatting += new DataGridViewCellFormattingEventHandler(Grid_CellFormatting);
            // 
            // footer
            // 
            footer.BackColor = Color.White;
            footer.Controls.Add(countLabel);
            footer.Controls.Add(statusLabel);
            footer.Controls.Add(rangeLabel);
            footer.Dock = DockStyle.Bottom;
            footer.Location = new Point(0, 612);
            footer.Name = "footer";
            footer.Padding = new Padding(14, 0, 14, 0);
            footer.Size = new Size(1120, 38);
            // 
            // countLabel
            // 
            countLabel.Dock = DockStyle.Right;
            countLabel.Font = new Font(new FontFamily("Tahoma"), 9F);
            countLabel.Location = new Point(926, 0);
            countLabel.Name = "countLabel";
            countLabel.Size = new Size(180, 38);
            countLabel.Text = "عدد العمليات: 0";
            countLabel.TextAlign = ContentAlignment.MiddleRight;
            //
            // statusLabel
            //
            statusLabel.Dock = DockStyle.Fill;
            statusLabel.Font = new Font(new FontFamily("Tahoma"), 9F);
            statusLabel.ForeColor = Color.FromArgb(71, 85, 105);
            statusLabel.Location = new Point(274, 0);
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new Size(652, 38);
            statusLabel.Text = "جاهز";
            statusLabel.TextAlign = ContentAlignment.MiddleCenter;
            //
            // rangeLabel
            //
            rangeLabel.Dock = DockStyle.Left;
            rangeLabel.Font = new Font(new FontFamily("Tahoma"), 9F);
            rangeLabel.ForeColor = Color.FromArgb(71, 85, 105);
            rangeLabel.Location = new Point(14, 0);
            rangeLabel.Name = "rangeLabel";
            rangeLabel.Size = new Size(260, 38);
            rangeLabel.Text = "الفترة: —";
            rangeLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // AuditLogForm
            // 
            AutoScaleDimensions = new SizeF(7F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(248, 250, 252);
            Controls.Add(grid);
            Controls.Add(footer);
            Controls.Add(filters);
            Controls.Add(header);
            Dock = DockStyle.Fill;
            Name = "AuditLogForm";
            RightToLeft = RightToLeft.Yes;
            Size = new Size(1120, 650);
            Load += new EventHandler(AuditLogForm_Load);
            header.ResumeLayout(false);
            filters.ResumeLayout(false);
            filters.PerformLayout();
            ((ISupportInitialize)(grid)).EndInit();
            footer.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}
