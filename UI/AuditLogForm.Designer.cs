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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.header = new System.Windows.Forms.Panel();
            this.subtitle = new System.Windows.Forms.Label();
            this.title = new System.Windows.Forms.Label();
            this.filters = new System.Windows.Forms.Panel();
            this.exportButton = new System.Windows.Forms.Button();
            this.refreshButton = new System.Windows.Forms.Button();
            this.searchBox = new System.Windows.Forms.TextBox();
            this.searchLabel = new System.Windows.Forms.Label();
            this.toDate = new System.Windows.Forms.DateTimePicker();
            this.toLabel = new System.Windows.Forms.Label();
            this.fromDate = new System.Windows.Forms.DateTimePicker();
            this.fromLabel = new System.Windows.Forms.Label();
            this.grid = new System.Windows.Forms.DataGridView();
            this.footer = new System.Windows.Forms.Panel();
            this.countLabel = new System.Windows.Forms.Label();
            this.statusLabel = new System.Windows.Forms.Label();
            this.rangeLabel = new System.Windows.Forms.Label();
            this.header.SuspendLayout();
            this.filters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.footer.SuspendLayout();
            this.SuspendLayout();
            // 
            // header
            // 
            this.header.BackColor = System.Drawing.Color.White;
            this.header.Controls.Add(this.subtitle);
            this.header.Controls.Add(this.title);
            this.header.Dock = System.Windows.Forms.DockStyle.Top;
            this.header.Location = new System.Drawing.Point(0, 0);
            this.header.Name = "header";
            this.header.Padding = new System.Windows.Forms.Padding(14, 8, 14, 6);
            this.header.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.header.Size = new System.Drawing.Size(1027, 76);
            this.header.TabIndex = 3;
            // 
            // subtitle
            // 
            this.subtitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.subtitle.Font = new System.Drawing.Font("Tahoma", 9F);
            this.subtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.subtitle.Location = new System.Drawing.Point(14, 42);
            this.subtitle.Name = "subtitle";
            this.subtitle.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.subtitle.Size = new System.Drawing.Size(999, 28);
            this.subtitle.TabIndex = 0;
            this.subtitle.Text = "مراجعة موثقة لتغييرات السندات والدرجات والرسوم والمستخدمين";
            this.subtitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // title
            // 
            this.title.Dock = System.Windows.Forms.DockStyle.Top;
            this.title.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold);
            this.title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.title.Location = new System.Drawing.Point(14, 8);
            this.title.Name = "title";
            this.title.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.title.Size = new System.Drawing.Size(999, 34);
            this.title.TabIndex = 1;
            this.title.Text = "سجل الأنشطة والعمليات الحساسة";
            this.title.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // filters
            // 
            this.filters.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.filters.Controls.Add(this.exportButton);
            this.filters.Controls.Add(this.refreshButton);
            this.filters.Controls.Add(this.searchBox);
            this.filters.Controls.Add(this.searchLabel);
            this.filters.Controls.Add(this.toDate);
            this.filters.Controls.Add(this.toLabel);
            this.filters.Controls.Add(this.fromDate);
            this.filters.Controls.Add(this.fromLabel);
            this.filters.Dock = System.Windows.Forms.DockStyle.Top;
            this.filters.Location = new System.Drawing.Point(0, 76);
            this.filters.Name = "filters";
            this.filters.Padding = new System.Windows.Forms.Padding(14, 8, 14, 8);
            this.filters.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.filters.Size = new System.Drawing.Size(1027, 82);
            this.filters.TabIndex = 2;
            // 
            // exportButton
            // 
            this.exportButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.exportButton.FlatAppearance.BorderSize = 0;
            this.exportButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exportButton.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.exportButton.ForeColor = System.Drawing.Color.White;
            this.exportButton.Location = new System.Drawing.Point(114, 8);
            this.exportButton.Margin = new System.Windows.Forms.Padding(4);
            this.exportButton.Name = "exportButton";
            this.exportButton.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.exportButton.Size = new System.Drawing.Size(110, 38);
            this.exportButton.TabIndex = 0;
            this.exportButton.Text = "تصدير Excel / PDF";
            this.exportButton.UseVisualStyleBackColor = false;
            this.exportButton.Click += new System.EventHandler(this.ExportButton_Click);
            // 
            // refreshButton
            // 
            this.refreshButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.refreshButton.FlatAppearance.BorderSize = 0;
            this.refreshButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.refreshButton.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.refreshButton.ForeColor = System.Drawing.Color.White;
            this.refreshButton.Location = new System.Drawing.Point(14, 8);
            this.refreshButton.Margin = new System.Windows.Forms.Padding(4);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.refreshButton.Size = new System.Drawing.Size(96, 38);
            this.refreshButton.TabIndex = 1;
            this.refreshButton.Text = "تحديث";
            this.refreshButton.UseVisualStyleBackColor = false;
            this.refreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // searchBox
            // 
            this.searchBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.searchBox.Location = new System.Drawing.Point(426, 8);
            this.searchBox.Margin = new System.Windows.Forms.Padding(4);
            this.searchBox.Name = "searchBox";
            this.searchBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.searchBox.Size = new System.Drawing.Size(220, 24);
            this.searchBox.TabIndex = 2;
            this.searchBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SearchBox_KeyDown);
            // 
            // searchLabel
            // 
            this.searchLabel.AutoSize = true;
            this.searchLabel.Dock = System.Windows.Forms.DockStyle.Right;
            this.searchLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.searchLabel.Location = new System.Drawing.Point(646, 8);
            this.searchLabel.Margin = new System.Windows.Forms.Padding(4);
            this.searchLabel.Name = "searchLabel";
            this.searchLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.searchLabel.Size = new System.Drawing.Size(44, 18);
            this.searchLabel.TabIndex = 3;
            this.searchLabel.Text = "بحث:";
            this.searchLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // toDate
            // 
            this.toDate.Dock = System.Windows.Forms.DockStyle.Right;
            this.toDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.toDate.Location = new System.Drawing.Point(690, 8);
            this.toDate.Margin = new System.Windows.Forms.Padding(4);
            this.toDate.Name = "toDate";
            this.toDate.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.toDate.Size = new System.Drawing.Size(125, 24);
            this.toDate.TabIndex = 4;
            // 
            // toLabel
            // 
            this.toLabel.AutoSize = true;
            this.toLabel.Dock = System.Windows.Forms.DockStyle.Right;
            this.toLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.toLabel.Location = new System.Drawing.Point(815, 8);
            this.toLabel.Margin = new System.Windows.Forms.Padding(4);
            this.toLabel.Name = "toLabel";
            this.toLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.toLabel.Size = new System.Drawing.Size(38, 18);
            this.toLabel.TabIndex = 5;
            this.toLabel.Text = "إلى:";
            this.toLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // fromDate
            // 
            this.fromDate.Dock = System.Windows.Forms.DockStyle.Right;
            this.fromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.fromDate.Location = new System.Drawing.Point(853, 8);
            this.fromDate.Margin = new System.Windows.Forms.Padding(4);
            this.fromDate.Name = "fromDate";
            this.fromDate.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.fromDate.Size = new System.Drawing.Size(125, 24);
            this.fromDate.TabIndex = 6;
            // 
            // fromLabel
            // 
            this.fromLabel.AutoSize = true;
            this.fromLabel.Dock = System.Windows.Forms.DockStyle.Right;
            this.fromLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.fromLabel.Location = new System.Drawing.Point(978, 8);
            this.fromLabel.Margin = new System.Windows.Forms.Padding(4);
            this.fromLabel.Name = "fromLabel";
            this.fromLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.fromLabel.Size = new System.Drawing.Size(35, 18);
            this.fromLabel.TabIndex = 7;
            this.fromLabel.Text = "من:";
            this.fromLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grid
            // 
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToDeleteRows = false;
            this.grid.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.grid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grid.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.grid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.grid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.grid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.grid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.grid.ColumnHeadersHeight = 42;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Tahoma", 10F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(234)))), ((int)(((byte)(254)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grid.DefaultCellStyle = dataGridViewCellStyle3;
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.EnableHeadersVisualStyles = false;
            this.grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.grid.Location = new System.Drawing.Point(0, 158);
            this.grid.MultiSelect = false;
            this.grid.Name = "grid";
            this.grid.ReadOnly = true;
            this.grid.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.grid.RowHeadersVisible = false;
            this.grid.RowHeadersWidth = 51;
            this.grid.RowTemplate.Height = 36;
            this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid.Size = new System.Drawing.Size(1027, 444);
            this.grid.TabIndex = 0;
            this.grid.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.Grid_CellFormatting);
            this.grid.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.Grid_DataBindingComplete);
            // 
            // footer
            // 
            this.footer.BackColor = System.Drawing.Color.White;
            this.footer.Controls.Add(this.statusLabel);
            this.footer.Controls.Add(this.countLabel);
            this.footer.Controls.Add(this.rangeLabel);
            this.footer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.footer.Location = new System.Drawing.Point(0, 602);
            this.footer.Name = "footer";
            this.footer.Padding = new System.Windows.Forms.Padding(14, 0, 14, 0);
            this.footer.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.footer.Size = new System.Drawing.Size(1027, 38);
            this.footer.TabIndex = 1;
            // 
            // countLabel
            // 
            this.countLabel.Dock = System.Windows.Forms.DockStyle.Right;
            this.countLabel.Font = new System.Drawing.Font("Tahoma", 9F);
            this.countLabel.Location = new System.Drawing.Point(833, 0);
            this.countLabel.Name = "countLabel";
            this.countLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.countLabel.Size = new System.Drawing.Size(180, 38);
            this.countLabel.TabIndex = 0;
            this.countLabel.Text = "عدد العمليات: 0";
            this.countLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // statusLabel
            // 
            this.statusLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusLabel.Font = new System.Drawing.Font("Tahoma", 9F);
            this.statusLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.statusLabel.Location = new System.Drawing.Point(274, 0);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.statusLabel.Size = new System.Drawing.Size(739, 38);
            this.statusLabel.TabIndex = 1;
            this.statusLabel.Text = "جاهز";
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rangeLabel
            // 
            this.rangeLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.rangeLabel.Font = new System.Drawing.Font("Tahoma", 9F);
            this.rangeLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.rangeLabel.Location = new System.Drawing.Point(14, 0);
            this.rangeLabel.Name = "rangeLabel";
            this.rangeLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.rangeLabel.Size = new System.Drawing.Size(260, 38);
            this.rangeLabel.TabIndex = 2;
            this.rangeLabel.Text = "الفترة: —";
            this.rangeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AuditLogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.Controls.Add(this.grid);
            this.Controls.Add(this.footer);
            this.Controls.Add(this.filters);
            this.Controls.Add(this.header);
            this.Name = "AuditLogForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Size = new System.Drawing.Size(1027, 640);
            this.Load += new System.EventHandler(this.AuditLogForm_Load);
            this.header.ResumeLayout(false);
            this.filters.ResumeLayout(false);
            this.filters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.footer.ResumeLayout(false);
            this.ResumeLayout(false);

        }
    }
}
