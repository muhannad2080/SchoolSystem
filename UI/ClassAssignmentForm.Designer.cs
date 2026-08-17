using System;
using System.Drawing;
using System.Windows.Forms;

namespace SchoolSystem.UI.Students
{
    partial class ClassAssignmentForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        #region Designer

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelTitle = new Krypton.Toolkit.KryptonPanel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.mainContainer = new System.Windows.Forms.TableLayoutPanel();
            this.panelTop = new System.Windows.Forms.Panel();
            this.tableLayoutTop = new System.Windows.Forms.TableLayoutPanel();
            this.lblClass = new System.Windows.Forms.Label();
            this.cmbClass = new System.Windows.Forms.ComboBox();
            this.lblSection = new System.Windows.Forms.Label();
            this.cmbSection = new System.Windows.Forms.ComboBox();
            this.lblAcademicYear = new System.Windows.Forms.Label();
            this.txtAcademicYear = new System.Windows.Forms.TextBox();
            this.btnLoad = new System.Windows.Forms.Button();
            this.tableLayoutMain = new System.Windows.Forms.TableLayoutPanel();
            this.groupBoxAssigned = new System.Windows.Forms.GroupBox();
            this.tableLayoutAssigned = new System.Windows.Forms.TableLayoutPanel();
            this.lblAssignedTitle = new System.Windows.Forms.Label();
            this.dataGridViewAssigned = new System.Windows.Forms.DataGridView();
            this.panelLeftButtons = new System.Windows.Forms.Panel();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnRemoveAll = new System.Windows.Forms.Button();
            this.cmbTransferSection = new System.Windows.Forms.ComboBox();
            this.btnTransfer = new System.Windows.Forms.Button();
            this.lblRecordCount = new System.Windows.Forms.Label();
            this.groupBoxUnassigned = new System.Windows.Forms.GroupBox();
            this.tableLayoutUnassigned = new System.Windows.Forms.TableLayoutPanel();
            this.lblUnassigned = new System.Windows.Forms.Label();
            this.panelSearch = new System.Windows.Forms.Panel();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.listBoxUnassigned = new System.Windows.Forms.CheckedListBox();
            this.panelRightButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.btnAssign = new System.Windows.Forms.Button();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.panelTitle.SuspendLayout();
            this.mainContainer.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.tableLayoutTop.SuspendLayout();
            this.tableLayoutMain.SuspendLayout();
            this.groupBoxAssigned.SuspendLayout();
            this.tableLayoutAssigned.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAssigned)).BeginInit();
            this.panelLeftButtons.SuspendLayout();
            this.groupBoxUnassigned.SuspendLayout();
            this.tableLayoutUnassigned.SuspendLayout();
            this.panelSearch.SuspendLayout();
            this.panelRightButtons.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();
            //
            // panelTitle
            //
            this.panelTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.panelTitle.Controls.Add(this.lblTitle);
            this.panelTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTitle.Location = new System.Drawing.Point(0, 0);
            this.panelTitle.Name = "panelTitle";
            this.panelTitle.Size = new System.Drawing.Size(1120, 58);
            this.panelTitle.TabIndex = 0;
            //
            // lblTitle
            //
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(1120, 58);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "توزيع الطلاب على الفصول والشعب";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // mainContainer
            //
            this.mainContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(243)))), ((int)(((byte)(247)))));
            this.mainContainer.ColumnCount = 1;
            this.mainContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainContainer.Controls.Add(this.panelTop, 0, 0);
            this.mainContainer.Controls.Add(this.tableLayoutMain, 0, 1);
            this.mainContainer.Controls.Add(this.panelBottom, 0, 2);
            this.mainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainContainer.Location = new System.Drawing.Point(0, 58);
            this.mainContainer.Name = "mainContainer";
            this.mainContainer.Padding = new System.Windows.Forms.Padding(12, 10, 12, 10);
            this.mainContainer.RowCount = 3;
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 82F));
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.mainContainer.Size = new System.Drawing.Size(1120, 652);
            this.mainContainer.TabIndex = 1;
            //
            // panelTop
            //
            this.panelTop.BackColor = System.Drawing.Color.White;
            this.panelTop.Controls.Add(this.tableLayoutTop);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTop.Location = new System.Drawing.Point(15, 13);
            this.panelTop.Name = "panelTop";
            this.panelTop.Padding = new System.Windows.Forms.Padding(10);
            this.panelTop.Size = new System.Drawing.Size(1090, 76);
            this.panelTop.TabIndex = 0;
            //
            // tableLayoutTop
            //
            this.tableLayoutTop.ColumnCount = 8;
            this.tableLayoutTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28F));
            this.tableLayoutTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18F));
            this.tableLayoutTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 115F));
            this.tableLayoutTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22F));
            this.tableLayoutTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tableLayoutTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32F));
            this.tableLayoutTop.Controls.Add(this.lblClass, 0, 0);
            this.tableLayoutTop.Controls.Add(this.cmbClass, 1, 0);
            this.tableLayoutTop.Controls.Add(this.lblSection, 2, 0);
            this.tableLayoutTop.Controls.Add(this.cmbSection, 3, 0);
            this.tableLayoutTop.Controls.Add(this.lblAcademicYear, 4, 0);
            this.tableLayoutTop.Controls.Add(this.txtAcademicYear, 5, 0);
            this.tableLayoutTop.Controls.Add(this.btnLoad, 7, 0);
            this.tableLayoutTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutTop.Location = new System.Drawing.Point(10, 10);
            this.tableLayoutTop.Name = "tableLayoutTop";
            this.tableLayoutTop.RowCount = 1;
            this.tableLayoutTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutTop.Size = new System.Drawing.Size(1070, 56);
            this.tableLayoutTop.TabIndex = 0;
            //
            // lblClass
            //
            this.lblClass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblClass.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblClass.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.lblClass.Location = new System.Drawing.Point(993, 0);
            this.lblClass.Name = "lblClass";
            this.lblClass.Size = new System.Drawing.Size(74, 56);
            this.lblClass.TabIndex = 0;
            this.lblClass.Text = "الصف:";
            this.lblClass.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // cmbClass
            //
            this.cmbClass.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.cmbClass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbClass.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbClass.Font = new System.Drawing.Font("Tahoma", 10F);
            this.cmbClass.FormattingEnabled = true;
            this.cmbClass.Location = new System.Drawing.Point(776, 13);
            this.cmbClass.Margin = new System.Windows.Forms.Padding(4, 13, 4, 4);
            this.cmbClass.Name = "cmbClass";
            this.cmbClass.Size = new System.Drawing.Size(210, 29);
            this.cmbClass.TabIndex = 1;
            this.cmbClass.SelectedIndexChanged += new System.EventHandler(this.cmbClass_SelectedIndexChanged);
            //
            // lblSection
            //
            this.lblSection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSection.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblSection.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.lblSection.Location = new System.Drawing.Point(695, 0);
            this.lblSection.Name = "lblSection";
            this.lblSection.Size = new System.Drawing.Size(74, 56);
            this.lblSection.TabIndex = 2;
            this.lblSection.Text = "الشعبة:";
            this.lblSection.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // cmbSection
            //
            this.cmbSection.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.cmbSection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbSection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbSection.Font = new System.Drawing.Font("Tahoma", 10F);
            this.cmbSection.FormattingEnabled = true;
            this.cmbSection.Location = new System.Drawing.Point(556, 13);
            this.cmbSection.Margin = new System.Windows.Forms.Padding(4, 13, 4, 4);
            this.cmbSection.Name = "cmbSection";
            this.cmbSection.Size = new System.Drawing.Size(132, 29);
            this.cmbSection.TabIndex = 3;
            this.cmbSection.SelectedIndexChanged += new System.EventHandler(this.cmbSection_SelectedIndexChanged);
            //
            // lblAcademicYear
            //
            this.lblAcademicYear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAcademicYear.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblAcademicYear.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.lblAcademicYear.Location = new System.Drawing.Point(440, 0);
            this.lblAcademicYear.Name = "lblAcademicYear";
            this.lblAcademicYear.Size = new System.Drawing.Size(109, 56);
            this.lblAcademicYear.TabIndex = 4;
            this.lblAcademicYear.Text = "العام الدراسي:";
            this.lblAcademicYear.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // txtAcademicYear
            //
            this.txtAcademicYear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.txtAcademicYear.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAcademicYear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtAcademicYear.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtAcademicYear.Location = new System.Drawing.Point(270, 13);
            this.txtAcademicYear.Margin = new System.Windows.Forms.Padding(4, 13, 4, 4);
            this.txtAcademicYear.Name = "txtAcademicYear";
            this.txtAcademicYear.Size = new System.Drawing.Size(163, 28);
            this.txtAcademicYear.TabIndex = 5;
            this.txtAcademicYear.Text = "2026/2027";
            this.txtAcademicYear.TextChanged += new System.EventHandler(this.txtAcademicYear_TextChanged);
            //
            // btnLoad
            //
            this.btnLoad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.btnLoad.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnLoad.FlatAppearance.BorderSize = 0;
            this.btnLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoad.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnLoad.ForeColor = System.Drawing.Color.White;
            this.btnLoad.Location = new System.Drawing.Point(4, 10);
            this.btnLoad.Margin = new System.Windows.Forms.Padding(4, 10, 4, 4);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(130, 42);
            this.btnLoad.TabIndex = 6;
            this.btnLoad.Text = "تحميل البيانات";
            this.btnLoad.UseVisualStyleBackColor = false;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            //
            // tableLayoutMain
            //
            this.tableLayoutMain.ColumnCount = 2;
            this.tableLayoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutMain.Controls.Add(this.groupBoxAssigned, 0, 0);
            this.tableLayoutMain.Controls.Add(this.groupBoxUnassigned, 1, 0);
            this.tableLayoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutMain.Location = new System.Drawing.Point(15, 95);
            this.tableLayoutMain.Name = "tableLayoutMain";
            this.tableLayoutMain.RowCount = 1;
            this.tableLayoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutMain.Size = new System.Drawing.Size(1090, 510);
            this.tableLayoutMain.TabIndex = 1;
            //
            // groupBoxAssigned
            //
            this.groupBoxAssigned.BackColor = System.Drawing.Color.White;
            this.groupBoxAssigned.Controls.Add(this.tableLayoutAssigned);
            this.groupBoxAssigned.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxAssigned.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.groupBoxAssigned.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.groupBoxAssigned.Location = new System.Drawing.Point(548, 3);
            this.groupBoxAssigned.Name = "groupBoxAssigned";
            this.groupBoxAssigned.Padding = new System.Windows.Forms.Padding(10);
            this.groupBoxAssigned.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.groupBoxAssigned.Size = new System.Drawing.Size(539, 504);
            this.groupBoxAssigned.TabIndex = 1;
            this.groupBoxAssigned.TabStop = false;
            this.groupBoxAssigned.Text = "الطلاب الموزعون";
            //
            // tableLayoutAssigned
            //
            this.tableLayoutAssigned.ColumnCount = 1;
            this.tableLayoutAssigned.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutAssigned.Controls.Add(this.lblAssignedTitle, 0, 0);
            this.tableLayoutAssigned.Controls.Add(this.dataGridViewAssigned, 0, 1);
            this.tableLayoutAssigned.Controls.Add(this.panelLeftButtons, 0, 2);
            this.tableLayoutAssigned.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutAssigned.Location = new System.Drawing.Point(10, 31);
            this.tableLayoutAssigned.Name = "tableLayoutAssigned";
            this.tableLayoutAssigned.RowCount = 3;
            this.tableLayoutAssigned.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutAssigned.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutAssigned.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 88F));
            this.tableLayoutAssigned.Size = new System.Drawing.Size(519, 463);
            this.tableLayoutAssigned.TabIndex = 0;
            //
            // lblAssignedTitle
            //
            this.lblAssignedTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAssignedTitle.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblAssignedTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.lblAssignedTitle.Location = new System.Drawing.Point(3, 0);
            this.lblAssignedTitle.Name = "lblAssignedTitle";
            this.lblAssignedTitle.Size = new System.Drawing.Size(513, 36);
            this.lblAssignedTitle.TabIndex = 0;
            this.lblAssignedTitle.Text = "الطلاب الموزعون على الصف والشعبة المحددة";
            this.lblAssignedTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // dataGridViewAssigned
            //
            this.dataGridViewAssigned.AllowUserToAddRows = false;
            this.dataGridViewAssigned.AllowUserToDeleteRows = false;
            this.dataGridViewAssigned.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewAssigned.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewAssigned.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewAssigned.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dataGridViewAssigned.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            this.dataGridViewAssigned.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewAssigned.ColumnHeadersHeight = 42;
            this.dataGridViewAssigned.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridViewAssigned.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewAssigned.EnableHeadersVisualStyles = false;
            this.dataGridViewAssigned.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(231)))), ((int)(((byte)(235)))));
            this.dataGridViewAssigned.Location = new System.Drawing.Point(3, 39);
            this.dataGridViewAssigned.MultiSelect = true;
            this.dataGridViewAssigned.Name = "dataGridViewAssigned";
            this.dataGridViewAssigned.ReadOnly = true;
            this.dataGridViewAssigned.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dataGridViewAssigned.RowHeadersVisible = false;
            this.dataGridViewAssigned.RowHeadersWidth = 51;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 9.5F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(234)))), ((int)(((byte)(254)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(64)))), ((int)(((byte)(175)))));
            this.dataGridViewAssigned.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewAssigned.RowTemplate.Height = 34;
            this.dataGridViewAssigned.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewAssigned.Size = new System.Drawing.Size(513, 363);
            this.dataGridViewAssigned.TabIndex = 1;
            this.dataGridViewAssigned.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewAssigned_CellClick);
            //
            // panelLeftButtons
            //
            this.panelLeftButtons.BackColor = System.Drawing.Color.White;
            this.panelLeftButtons.Controls.Add(this.btnRemove);
            this.panelLeftButtons.Controls.Add(this.btnRemoveAll);
            this.panelLeftButtons.Controls.Add(this.cmbTransferSection);
            this.panelLeftButtons.Controls.Add(this.btnTransfer);
            this.panelLeftButtons.Controls.Add(this.lblRecordCount);
            this.panelLeftButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLeftButtons.Location = new System.Drawing.Point(3, 408);
            this.panelLeftButtons.Name = "panelLeftButtons";
            this.panelLeftButtons.Size = new System.Drawing.Size(513, 82);
            this.panelLeftButtons.TabIndex = 2;
            //
            // btnRemove
            //
            this.btnRemove.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnRemove.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.btnRemove.FlatAppearance.BorderSize = 0;
            this.btnRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemove.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnRemove.ForeColor = System.Drawing.Color.White;
            this.btnRemove.Location = new System.Drawing.Point(388, 9);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(120, 36);
            this.btnRemove.TabIndex = 0;
            this.btnRemove.Text = "إزالة التوزيع";
            this.btnRemove.UseVisualStyleBackColor = false;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            //
            // btnRemoveAll
            //
            this.btnRemoveAll.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnRemoveAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(185)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.btnRemoveAll.FlatAppearance.BorderSize = 0;
            this.btnRemoveAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveAll.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnRemoveAll.ForeColor = System.Drawing.Color.White;
            this.btnRemoveAll.Location = new System.Drawing.Point(260, 9);
            this.btnRemoveAll.Name = "btnRemoveAll";
            this.btnRemoveAll.Size = new System.Drawing.Size(115, 36);
            this.btnRemoveAll.TabIndex = 2;
            this.btnRemoveAll.Text = "إزالة المحدد";
            this.btnRemoveAll.UseVisualStyleBackColor = false;
            this.btnRemoveAll.Click += new System.EventHandler(this.btnRemoveAll_Click);
            //
            // cmbTransferSection
            //
            this.cmbTransferSection.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbTransferSection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTransferSection.Font = new System.Drawing.Font("Tahoma", 9F);
            this.cmbTransferSection.FormattingEnabled = true;
            this.cmbTransferSection.Location = new System.Drawing.Point(135, 51);
            this.cmbTransferSection.Name = "cmbTransferSection";
            this.cmbTransferSection.Size = new System.Drawing.Size(120, 26);
            this.cmbTransferSection.TabIndex = 3;
            //
            // btnTransfer
            //
            this.btnTransfer.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnTransfer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(58)))), ((int)(((byte)(237)))));
            this.btnTransfer.FlatAppearance.BorderSize = 0;
            this.btnTransfer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTransfer.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnTransfer.ForeColor = System.Drawing.Color.White;
            this.btnTransfer.Location = new System.Drawing.Point(5, 45);
            this.btnTransfer.Name = "btnTransfer";
            this.btnTransfer.Size = new System.Drawing.Size(120, 36);
            this.btnTransfer.TabIndex = 4;
            this.btnTransfer.Text = "نقل إلى شعبة";
            this.btnTransfer.UseVisualStyleBackColor = false;
            this.btnTransfer.Click += new System.EventHandler(this.btnTransfer_Click);
            //
            // lblRecordCount
            //
            this.lblRecordCount.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblRecordCount.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblRecordCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.lblRecordCount.Location = new System.Drawing.Point(10, 14);
            this.lblRecordCount.Name = "lblRecordCount";
            this.lblRecordCount.Size = new System.Drawing.Size(240, 24);
            this.lblRecordCount.TabIndex = 1;
            this.lblRecordCount.Text = "عدد الطلاب: 0";
            this.lblRecordCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // groupBoxUnassigned
            //
            this.groupBoxUnassigned.BackColor = System.Drawing.Color.White;
            this.groupBoxUnassigned.Controls.Add(this.tableLayoutUnassigned);
            this.groupBoxUnassigned.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxUnassigned.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.groupBoxUnassigned.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.groupBoxUnassigned.Location = new System.Drawing.Point(3, 3);
            this.groupBoxUnassigned.Name = "groupBoxUnassigned";
            this.groupBoxUnassigned.Padding = new System.Windows.Forms.Padding(10);
            this.groupBoxUnassigned.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.groupBoxUnassigned.Size = new System.Drawing.Size(539, 504);
            this.groupBoxUnassigned.TabIndex = 0;
            this.groupBoxUnassigned.TabStop = false;
            this.groupBoxUnassigned.Text = "الطلاب غير الموزعين";
            //
            // tableLayoutUnassigned
            //
            this.tableLayoutUnassigned.ColumnCount = 1;
            this.tableLayoutUnassigned.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutUnassigned.Controls.Add(this.lblUnassigned, 0, 0);
            this.tableLayoutUnassigned.Controls.Add(this.panelSearch, 0, 1);
            this.tableLayoutUnassigned.Controls.Add(this.listBoxUnassigned, 0, 2);
            this.tableLayoutUnassigned.Controls.Add(this.panelRightButtons, 0, 3);
            this.tableLayoutUnassigned.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutUnassigned.Location = new System.Drawing.Point(10, 31);
            this.tableLayoutUnassigned.Name = "tableLayoutUnassigned";
            this.tableLayoutUnassigned.RowCount = 4;
            this.tableLayoutUnassigned.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutUnassigned.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutUnassigned.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutUnassigned.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 58F));
            this.tableLayoutUnassigned.Size = new System.Drawing.Size(519, 463);
            this.tableLayoutUnassigned.TabIndex = 0;
            //
            // lblUnassigned
            //
            this.lblUnassigned.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblUnassigned.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblUnassigned.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.lblUnassigned.Location = new System.Drawing.Point(3, 0);
            this.lblUnassigned.Name = "lblUnassigned";
            this.lblUnassigned.Size = new System.Drawing.Size(513, 32);
            this.lblUnassigned.TabIndex = 0;
            this.lblUnassigned.Text = "الطلاب غير الموزعين: 0";
            this.lblUnassigned.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // panelSearch
            //
            this.panelSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.panelSearch.Controls.Add(this.txtSearch);
            this.panelSearch.Controls.Add(this.lblSearch);
            this.panelSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSearch.Location = new System.Drawing.Point(3, 35);
            this.panelSearch.Name = "panelSearch";
            this.panelSearch.Padding = new System.Windows.Forms.Padding(8);
            this.panelSearch.Size = new System.Drawing.Size(513, 42);
            this.panelSearch.TabIndex = 1;
            //
            // txtSearch
            //
            this.txtSearch.BackColor = System.Drawing.Color.White;
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSearch.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtSearch.Location = new System.Drawing.Point(8, 8);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(387, 28);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            //
            // lblSearch
            //
            this.lblSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblSearch.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(85)))), ((int)(((byte)(99)))));
            this.lblSearch.Location = new System.Drawing.Point(395, 8);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(110, 26);
            this.lblSearch.TabIndex = 0;
            this.lblSearch.Text = "بحث طالب:";
            this.lblSearch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // listBoxUnassigned
            //
            this.listBoxUnassigned.BackColor = System.Drawing.Color.White;
            this.listBoxUnassigned.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBoxUnassigned.CheckOnClick = true;
            this.listBoxUnassigned.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxUnassigned.Font = new System.Drawing.Font("Tahoma", 10F);
            this.listBoxUnassigned.FormattingEnabled = true;
            this.listBoxUnassigned.Location = new System.Drawing.Point(3, 83);
            this.listBoxUnassigned.Name = "listBoxUnassigned";
            this.listBoxUnassigned.Size = new System.Drawing.Size(513, 319);
            this.listBoxUnassigned.TabIndex = 2;
            //
            // panelRightButtons
            //
            this.panelRightButtons.BackColor = System.Drawing.Color.White;
            this.panelRightButtons.Controls.Add(this.btnSelectAll);
            this.panelRightButtons.Controls.Add(this.btnAssign);
            this.panelRightButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRightButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.panelRightButtons.Location = new System.Drawing.Point(3, 408);
            this.panelRightButtons.Name = "panelRightButtons";
            this.panelRightButtons.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.panelRightButtons.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.panelRightButtons.Size = new System.Drawing.Size(513, 52);
            this.panelRightButtons.TabIndex = 3;
            //
            // btnSelectAll
            //
            this.btnSelectAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.btnSelectAll.FlatAppearance.BorderSize = 0;
            this.btnSelectAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectAll.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnSelectAll.ForeColor = System.Drawing.Color.White;
            this.btnSelectAll.Location = new System.Drawing.Point(5, 10);
            this.btnSelectAll.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(110, 36);
            this.btnSelectAll.TabIndex = 0;
            this.btnSelectAll.Text = "تحديد الكل";
            this.btnSelectAll.UseVisualStyleBackColor = false;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            //
            // btnAssign
            //
            this.btnAssign.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.btnAssign.FlatAppearance.BorderSize = 0;
            this.btnAssign.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAssign.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnAssign.ForeColor = System.Drawing.Color.White;
            this.btnAssign.Location = new System.Drawing.Point(125, 10);
            this.btnAssign.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.btnAssign.Name = "btnAssign";
            this.btnAssign.Size = new System.Drawing.Size(120, 36);
            this.btnAssign.TabIndex = 1;
            this.btnAssign.Text = "توزيع الطلاب";
            this.btnAssign.UseVisualStyleBackColor = false;
            this.btnAssign.Click += new System.EventHandler(this.btnAssign_Click);
            //
            // panelBottom
            //
            this.panelBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(243)))), ((int)(((byte)(247)))));
            this.panelBottom.Controls.Add(this.lblStatus);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBottom.Location = new System.Drawing.Point(15, 611);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(1090, 28);
            this.panelBottom.TabIndex = 2;
            //
            // lblStatus
            //
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatus.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblStatus.ForeColor = System.Drawing.Color.DimGray;
            this.lblStatus.Location = new System.Drawing.Point(0, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(1090, 28);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "جاهز";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // ClassAssignmentForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(243)))), ((int)(((byte)(247)))));
            this.Controls.Add(this.mainContainer);
            this.Controls.Add(this.panelTitle);
            this.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.Name = "ClassAssignmentForm";
            this.Text = "توزيع الطلاب على الفصول";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Size = new System.Drawing.Size(1120, 710);
            this.panelTitle.ResumeLayout(false);
            this.mainContainer.ResumeLayout(false);
            this.panelTop.ResumeLayout(false);
            this.tableLayoutTop.ResumeLayout(false);
            this.tableLayoutTop.PerformLayout();
            this.tableLayoutMain.ResumeLayout(false);
            this.groupBoxAssigned.ResumeLayout(false);
            this.tableLayoutAssigned.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAssigned)).EndInit();
            this.panelLeftButtons.ResumeLayout(false);
            this.groupBoxUnassigned.ResumeLayout(false);
            this.tableLayoutUnassigned.ResumeLayout(false);
            this.panelSearch.ResumeLayout(false);
            this.panelSearch.PerformLayout();
            this.panelRightButtons.ResumeLayout(false);
            this.panelBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonPanel panelTitle;
        private System.Windows.Forms.Label lblTitle;

        private System.Windows.Forms.TableLayoutPanel mainContainer;

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.TableLayoutPanel tableLayoutTop;

        private System.Windows.Forms.Label lblClass;
        private System.Windows.Forms.ComboBox cmbClass;

        private System.Windows.Forms.Label lblSection;
        private System.Windows.Forms.ComboBox cmbSection;

        private System.Windows.Forms.Label lblAcademicYear;
        private System.Windows.Forms.TextBox txtAcademicYear;

        private System.Windows.Forms.Button btnLoad;

        private System.Windows.Forms.TableLayoutPanel tableLayoutMain;

        private System.Windows.Forms.GroupBox groupBoxUnassigned;
        private System.Windows.Forms.TableLayoutPanel tableLayoutUnassigned;
        private System.Windows.Forms.Panel panelSearch;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblUnassigned;
        private System.Windows.Forms.CheckedListBox listBoxUnassigned;
        private System.Windows.Forms.FlowLayoutPanel panelRightButtons;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Button btnAssign;

        private System.Windows.Forms.GroupBox groupBoxAssigned;
        private System.Windows.Forms.TableLayoutPanel tableLayoutAssigned;
        private System.Windows.Forms.Label lblAssignedTitle;
        private System.Windows.Forms.DataGridView dataGridViewAssigned;
        private System.Windows.Forms.Panel panelLeftButtons;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnRemoveAll;
        private System.Windows.Forms.ComboBox cmbTransferSection;
        private System.Windows.Forms.Button btnTransfer;
        private System.Windows.Forms.Label lblRecordCount;

        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Label lblStatus;
    }
}
