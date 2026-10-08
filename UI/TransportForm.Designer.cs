
namespace SchoolSystem.UI
{

    partial class TransportForm
    {

        private System.ComponentModel.IContainer components = null;

        // تعريف الدالة Dispose لتنفيذ العملية المرتبطة بها.
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }


        #region كود المصمم


        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelTitle = new Krypton.Toolkit.KryptonPanel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabBuses = new System.Windows.Forms.TabPage();
            this.dataGridViewBuses = new System.Windows.Forms.DataGridView();
            this.panelBusesButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAddBus = new System.Windows.Forms.Button();
            this.btnUpdateBus = new System.Windows.Forms.Button();
            this.btnDeleteBus = new System.Windows.Forms.Button();
            this.btnClearBus = new System.Windows.Forms.Button();
            this.panelBusesFields = new System.Windows.Forms.Panel();
            this.tableBusesFields = new System.Windows.Forms.TableLayoutPanel();
            this.lblBusNumber = new System.Windows.Forms.Label();
            this.txtBusNumber = new System.Windows.Forms.TextBox();
            this.lblDriverName = new System.Windows.Forms.Label();
            this.txtDriverName = new System.Windows.Forms.TextBox();
            this.lblDriverPhone = new System.Windows.Forms.Label();
            this.txtDriverPhone = new System.Windows.Forms.TextBox();
            this.lblCapacity = new System.Windows.Forms.Label();
            this.txtCapacity = new System.Windows.Forms.TextBox();
            this.lblNotes = new System.Windows.Forms.Label();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.tabRoutes = new System.Windows.Forms.TabPage();
            this.dataGridViewRoutes = new System.Windows.Forms.DataGridView();
            this.panelRoutesButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAddRoute = new System.Windows.Forms.Button();
            this.btnUpdateRoute = new System.Windows.Forms.Button();
            this.btnDeleteRoute = new System.Windows.Forms.Button();
            this.btnClearRoute = new System.Windows.Forms.Button();
            this.panelRoutesFields = new System.Windows.Forms.Panel();
            this.tableRoutesFields = new System.Windows.Forms.TableLayoutPanel();
            this.lblRouteName = new System.Windows.Forms.Label();
            this.txtRouteName = new System.Windows.Forms.TextBox();
            this.lblBus = new System.Windows.Forms.Label();
            this.cmbBus = new System.Windows.Forms.ComboBox();
            this.lblStartPoint = new System.Windows.Forms.Label();
            this.txtStartPoint = new System.Windows.Forms.TextBox();
            this.lblEndPoint = new System.Windows.Forms.Label();
            this.txtEndPoint = new System.Windows.Forms.TextBox();
            this.lblDeparture = new System.Windows.Forms.Label();
            this.dtpDeparture = new System.Windows.Forms.DateTimePicker();
            this.lblArrival = new System.Windows.Forms.Label();
            this.dtpArrival = new System.Windows.Forms.DateTimePicker();
            this.lblFee = new System.Windows.Forms.Label();
            this.txtFee = new System.Windows.Forms.TextBox();
            this.lblRouteNotes = new System.Windows.Forms.Label();
            this.txtRouteNotes = new System.Windows.Forms.TextBox();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.lblRecordCount = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.panelTitle)).BeginInit();
            this.panelTitle.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabBuses.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBuses)).BeginInit();
            this.panelBusesButtons.SuspendLayout();
            this.panelBusesFields.SuspendLayout();
            this.tableBusesFields.SuspendLayout();
            this.tabRoutes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRoutes)).BeginInit();
            this.panelRoutesButtons.SuspendLayout();
            this.panelRoutesFields.SuspendLayout();
            this.tableRoutesFields.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTitle
            // 
            this.panelTitle.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
 
            this.panelTitle.Controls.Add(this.lblTitle);
            this.panelTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTitle.Location = new System.Drawing.Point(0, 0);
            this.panelTitle.Name = "panelTitle";
            this.panelTitle.Size = new System.Drawing.Size(950, 60);
            this.panelTitle.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(950, 55);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "النقل المدرسي";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabBuses);
            this.tabControl.Controls.Add(this.tabRoutes);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Font = new System.Drawing.Font("Tahoma", 10F);
            this.tabControl.Location = new System.Drawing.Point(0, 55);
            this.tabControl.Name = "tabControl";
            this.tabControl.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(950, 565);
            this.tabControl.TabIndex = 1;
            // 
            // tabBuses
            // 
            this.tabBuses.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.tabBuses.Controls.Add(this.dataGridViewBuses);
            this.tabBuses.Controls.Add(this.panelBusesButtons);
            this.tabBuses.Controls.Add(this.panelBusesFields);
            this.tabBuses.Location = new System.Drawing.Point(4, 30);
            this.tabBuses.Name = "tabBuses";
            this.tabBuses.Size = new System.Drawing.Size(942, 531);
            this.tabBuses.TabIndex = 0;
            this.tabBuses.Text = "الحافلات";
            // 
            // dataGridViewBuses
            // 
            this.dataGridViewBuses.AllowUserToAddRows = false;
            this.dataGridViewBuses.AllowUserToDeleteRows = false;
            this.dataGridViewBuses.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewBuses.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewBuses.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(42)))), ((int)(((byte)(57)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            this.dataGridViewBuses.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewBuses.ColumnHeadersHeight = 42;
            this.dataGridViewBuses.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewBuses.EnableHeadersVisualStyles = false;
            this.dataGridViewBuses.Location = new System.Drawing.Point(0, 237);
            this.dataGridViewBuses.MultiSelect = false;
            this.dataGridViewBuses.Name = "dataGridViewBuses";
            this.dataGridViewBuses.ReadOnly = true;
            this.dataGridViewBuses.RowHeadersVisible = false;
            this.dataGridViewBuses.RowHeadersWidth = 51;
            this.dataGridViewBuses.RowTemplate.Height = 36;
            this.dataGridViewBuses.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewBuses.Size = new System.Drawing.Size(942, 294);
            this.dataGridViewBuses.TabIndex = 2;
            this.dataGridViewBuses.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewBuses_CellClick);
            this.dataGridViewBuses.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dataGridViewBuses.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dataGridViewBuses.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dataGridViewBuses.AllowUserToResizeRows = false;
            this.dataGridViewBuses.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridViewBuses.GridColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.dataGridViewBuses.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.dataGridViewBuses.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.dataGridViewBuses.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.dataGridViewBuses.ColumnHeadersDefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewBuses.ColumnHeadersDefaultCellStyle.Padding = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.dataGridViewBuses.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.dataGridViewBuses.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dataGridViewBuses.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.dataGridViewBuses.DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 10F);
            this.dataGridViewBuses.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.dataGridViewBuses.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(219, 234, 254);
            this.dataGridViewBuses.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.dataGridViewBuses.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewBuses.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.dataGridViewBuses.RowsDefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dataGridViewBuses.RowsDefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            // 
            // panelBusesButtons
            // 
            this.panelBusesButtons.AutoScroll = true;
            this.panelBusesButtons.AutoSize = true;
            this.panelBusesButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelBusesButtons.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.panelBusesButtons.Controls.Add(this.btnAddBus);
            this.panelBusesButtons.Controls.Add(this.btnUpdateBus);
            this.panelBusesButtons.Controls.Add(this.btnDeleteBus);
            this.panelBusesButtons.Controls.Add(this.btnClearBus);
            this.panelBusesButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelBusesButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.panelBusesButtons.Location = new System.Drawing.Point(0, 180);
            this.panelBusesButtons.Name = "panelBusesButtons";
            this.panelBusesButtons.Padding = new System.Windows.Forms.Padding(8);
            this.panelBusesButtons.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.panelBusesButtons.Size = new System.Drawing.Size(942, 57);
            this.panelBusesButtons.TabIndex = 1;
            // 
            // btnAddBus
            // 
            this.btnAddBus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddBus.BackColor = System.Drawing.Color.FromArgb(22, 163, 74);
            this.btnAddBus.FlatAppearance.BorderSize = 0;
            this.btnAddBus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddBus.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Bold);
            this.btnAddBus.ForeColor = System.Drawing.Color.White;
            this.btnAddBus.Location = new System.Drawing.Point(11, 11);
            this.btnAddBus.Name = "btnAddBus";
            this.btnAddBus.Size = new System.Drawing.Size(95, 35);
            this.btnAddBus.TabIndex = 0;
            this.btnAddBus.Text = "إضافة حافلة";
            this.btnAddBus.UseVisualStyleBackColor = false;
            this.btnAddBus.Click += new System.EventHandler(this.btnAddBus_Click);
            this.btnAddBus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnAddBus.AutoSize = false;
            this.btnAddBus.MinimumSize = new System.Drawing.Size(110, 40);
            this.btnAddBus.Cursor = System.Windows.Forms.Cursors.Hand;
            // 
            // btnUpdateBus
            // 
            this.btnUpdateBus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdateBus.BackColor = System.Drawing.Color.FromArgb(15, 118, 110);
            this.btnUpdateBus.FlatAppearance.BorderSize = 0;
            this.btnUpdateBus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdateBus.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Bold);
            this.btnUpdateBus.ForeColor = System.Drawing.Color.White;
            this.btnUpdateBus.Location = new System.Drawing.Point(112, 11);
            this.btnUpdateBus.Name = "btnUpdateBus";
            this.btnUpdateBus.Size = new System.Drawing.Size(95, 35);
            this.btnUpdateBus.TabIndex = 1;
            this.btnUpdateBus.Text = "تعديل بيانات الحافلة";
            this.btnUpdateBus.UseVisualStyleBackColor = false;
            this.btnUpdateBus.Click += new System.EventHandler(this.btnUpdateBus_Click);
            this.btnUpdateBus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnUpdateBus.AutoSize = false;
            this.btnUpdateBus.MinimumSize = new System.Drawing.Size(110, 40);
            this.btnUpdateBus.Cursor = System.Windows.Forms.Cursors.Hand;
            // 
            // btnDeleteBus
            // 
            this.btnDeleteBus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteBus.BackColor = System.Drawing.Color.FromArgb(198, 40, 40);
            this.btnDeleteBus.FlatAppearance.BorderSize = 0;
            this.btnDeleteBus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteBus.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Bold);
            this.btnDeleteBus.ForeColor = System.Drawing.Color.White;
            this.btnDeleteBus.Location = new System.Drawing.Point(213, 11);
            this.btnDeleteBus.Name = "btnDeleteBus";
            this.btnDeleteBus.Size = new System.Drawing.Size(95, 35);
            this.btnDeleteBus.TabIndex = 2;
            this.btnDeleteBus.Text = "حذف الحافلة";
            this.btnDeleteBus.UseVisualStyleBackColor = false;
            this.btnDeleteBus.Click += new System.EventHandler(this.btnDeleteBus_Click);
            this.btnDeleteBus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnDeleteBus.AutoSize = false;
            this.btnDeleteBus.MinimumSize = new System.Drawing.Size(110, 40);
            this.btnDeleteBus.Cursor = System.Windows.Forms.Cursors.Hand;
            // 
            // btnClearBus
            // 
            this.btnClearBus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearBus.BackColor = System.Drawing.Color.FromArgb(15, 118, 110);
            this.btnClearBus.FlatAppearance.BorderSize = 0;
            this.btnClearBus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearBus.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Bold);
            this.btnClearBus.ForeColor = System.Drawing.Color.White;
            this.btnClearBus.Location = new System.Drawing.Point(314, 11);
            this.btnClearBus.Name = "btnClearBus";
            this.btnClearBus.Size = new System.Drawing.Size(95, 35);
            this.btnClearBus.TabIndex = 3;
            this.btnClearBus.Text = "تفريغ النموذج";
            this.btnClearBus.UseVisualStyleBackColor = false;
            this.btnClearBus.Click += new System.EventHandler(this.btnClearBus_Click);
            this.btnClearBus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnClearBus.AutoSize = false;
            this.btnClearBus.MinimumSize = new System.Drawing.Size(110, 40);
            this.btnClearBus.Cursor = System.Windows.Forms.Cursors.Hand;
            // 
            // panelBusesFields
            // 
            this.panelBusesFields.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.panelBusesFields.Controls.Add(this.tableBusesFields);
            this.panelBusesFields.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelBusesFields.Location = new System.Drawing.Point(0, 0);
            this.panelBusesFields.Name = "panelBusesFields";
            this.panelBusesFields.Padding = new System.Windows.Forms.Padding(15);
            this.panelBusesFields.Size = new System.Drawing.Size(942, 180);
            this.panelBusesFields.TabIndex = 0;
            // 
            // tableBusesFields
            // 
            this.tableBusesFields.ColumnCount = 4;
            this.tableBusesFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableBusesFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableBusesFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableBusesFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableBusesFields.Controls.Add(this.lblBusNumber, 0, 0);
            this.tableBusesFields.Controls.Add(this.txtBusNumber, 1, 0);
            this.tableBusesFields.Controls.Add(this.lblDriverName, 2, 0);
            this.tableBusesFields.Controls.Add(this.txtDriverName, 3, 0);
            this.tableBusesFields.Controls.Add(this.lblDriverPhone, 0, 1);
            this.tableBusesFields.Controls.Add(this.txtDriverPhone, 1, 1);
            this.tableBusesFields.Controls.Add(this.lblCapacity, 2, 1);
            this.tableBusesFields.Controls.Add(this.txtCapacity, 3, 1);
            this.tableBusesFields.Controls.Add(this.lblNotes, 0, 2);
            this.tableBusesFields.Controls.Add(this.txtNotes, 1, 2);
            this.tableBusesFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableBusesFields.Location = new System.Drawing.Point(15, 15);
            this.tableBusesFields.Name = "tableBusesFields";
            this.tableBusesFields.RowCount = 3;
            this.tableBusesFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableBusesFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableBusesFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableBusesFields.Size = new System.Drawing.Size(912, 150);
            this.tableBusesFields.TabIndex = 0;
            // 
            // lblBusNumber
            // 
            this.lblBusNumber.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblBusNumber.AutoSize = true;
            this.lblBusNumber.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblBusNumber.Location = new System.Drawing.Point(815, 1);
            this.lblBusNumber.Name = "lblBusNumber";
            this.lblBusNumber.Size = new System.Drawing.Size(65, 42);
            this.lblBusNumber.TabIndex = 0;
            this.lblBusNumber.Text = "رقم الحافلة:";
            // 
            // txtBusNumber
            // 
            this.txtBusNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBusNumber.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtBusNumber.Location = new System.Drawing.Point(459, 3);
            this.txtBusNumber.Name = "txtBusNumber";
            this.txtBusNumber.Size = new System.Drawing.Size(350, 28);
            this.txtBusNumber.TabIndex = 0;
            // 
            // lblDriverName
            // 
            this.lblDriverName.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblDriverName.AutoSize = true;
            this.lblDriverName.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblDriverName.Location = new System.Drawing.Point(359, 1);
            this.lblDriverName.Name = "lblDriverName";
            this.lblDriverName.Size = new System.Drawing.Size(68, 42);
            this.lblDriverName.TabIndex = 1;
            this.lblDriverName.Text = "اسم السائق:";
            // 
            // txtDriverName
            // 
            this.txtDriverName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDriverName.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtDriverName.Location = new System.Drawing.Point(3, 3);
            this.txtDriverName.Name = "txtDriverName";
            this.txtDriverName.Size = new System.Drawing.Size(350, 28);
            this.txtDriverName.TabIndex = 1;
            // 
            // lblDriverPhone
            // 
            this.lblDriverPhone.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblDriverPhone.AutoSize = true;
            this.lblDriverPhone.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblDriverPhone.Location = new System.Drawing.Point(815, 46);
            this.lblDriverPhone.Name = "lblDriverPhone";
            this.lblDriverPhone.Size = new System.Drawing.Size(68, 42);
            this.lblDriverPhone.TabIndex = 2;
            this.lblDriverPhone.Text = "هاتف السائق:";
            // 
            // txtDriverPhone
            // 
            this.txtDriverPhone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDriverPhone.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtDriverPhone.Location = new System.Drawing.Point(459, 48);
            this.txtDriverPhone.Name = "txtDriverPhone";
            this.txtDriverPhone.Size = new System.Drawing.Size(350, 28);
            this.txtDriverPhone.TabIndex = 2;
            // 
            // lblCapacity
            // 
            this.lblCapacity.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblCapacity.AutoSize = true;
            this.lblCapacity.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblCapacity.Location = new System.Drawing.Point(359, 57);
            this.lblCapacity.Name = "lblCapacity";
            this.lblCapacity.Size = new System.Drawing.Size(61, 21);
            this.lblCapacity.TabIndex = 3;
            this.lblCapacity.Text = "السعة:";
            // 
            // txtCapacity
            // 
            this.txtCapacity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCapacity.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtCapacity.Location = new System.Drawing.Point(3, 48);
            this.txtCapacity.Name = "txtCapacity";
            this.txtCapacity.Size = new System.Drawing.Size(350, 28);
            this.txtCapacity.TabIndex = 3;
            this.txtCapacity.Text = "30";
            // 
            // lblNotes
            // 
            this.lblNotes.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblNotes.AutoSize = true;
            this.lblNotes.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblNotes.Location = new System.Drawing.Point(815, 109);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(79, 21);
            this.lblNotes.TabIndex = 4;
            this.lblNotes.Text = "ملاحظات:";
            // 
            // txtNotes
            // 
            this.txtNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNotes.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtNotes.Location = new System.Drawing.Point(459, 93);
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(350, 28);
            this.txtNotes.TabIndex = 4;
            // 
            // tabRoutes
            // 
            this.tabRoutes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.tabRoutes.Controls.Add(this.dataGridViewRoutes);
            this.tabRoutes.Controls.Add(this.panelRoutesButtons);
            this.tabRoutes.Controls.Add(this.panelRoutesFields);
            this.tabRoutes.Location = new System.Drawing.Point(4, 30);
            this.tabRoutes.Name = "tabRoutes";
            this.tabRoutes.Size = new System.Drawing.Size(942, 531);
            this.tabRoutes.TabIndex = 1;
            this.tabRoutes.Text = "المسارات";
            // 
            // dataGridViewRoutes
            // 
            this.dataGridViewRoutes.AllowUserToAddRows = false;
            this.dataGridViewRoutes.AllowUserToDeleteRows = false;
            this.dataGridViewRoutes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewRoutes.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewRoutes.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(42)))), ((int)(((byte)(57)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 10F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            this.dataGridViewRoutes.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewRoutes.ColumnHeadersHeight = 42;
            this.dataGridViewRoutes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewRoutes.EnableHeadersVisualStyles = false;
            this.dataGridViewRoutes.Location = new System.Drawing.Point(0, 317);
            this.dataGridViewRoutes.MultiSelect = false;
            this.dataGridViewRoutes.Name = "dataGridViewRoutes";
            this.dataGridViewRoutes.ReadOnly = true;
            this.dataGridViewRoutes.RowHeadersVisible = false;
            this.dataGridViewRoutes.RowHeadersWidth = 51;
            this.dataGridViewRoutes.RowTemplate.Height = 36;
            this.dataGridViewRoutes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewRoutes.Size = new System.Drawing.Size(942, 214);
            this.dataGridViewRoutes.TabIndex = 2;
            this.dataGridViewRoutes.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewRoutes_CellClick);
            this.dataGridViewRoutes.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dataGridViewRoutes.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dataGridViewRoutes.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dataGridViewRoutes.AllowUserToResizeRows = false;
            this.dataGridViewRoutes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridViewRoutes.GridColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.dataGridViewRoutes.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.dataGridViewRoutes.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.dataGridViewRoutes.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.dataGridViewRoutes.ColumnHeadersDefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewRoutes.ColumnHeadersDefaultCellStyle.Padding = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.dataGridViewRoutes.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.dataGridViewRoutes.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dataGridViewRoutes.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.dataGridViewRoutes.DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 10F);
            this.dataGridViewRoutes.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.dataGridViewRoutes.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(219, 234, 254);
            this.dataGridViewRoutes.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.dataGridViewRoutes.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewRoutes.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            this.dataGridViewRoutes.RowsDefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dataGridViewRoutes.RowsDefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            // 
            // panelRoutesButtons
            // 
            this.panelRoutesButtons.AutoScroll = true;
            this.panelRoutesButtons.AutoSize = true;
            this.panelRoutesButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelRoutesButtons.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.panelRoutesButtons.Controls.Add(this.btnAddRoute);
            this.panelRoutesButtons.Controls.Add(this.btnUpdateRoute);
            this.panelRoutesButtons.Controls.Add(this.btnDeleteRoute);
            this.panelRoutesButtons.Controls.Add(this.btnClearRoute);
            this.panelRoutesButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelRoutesButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.panelRoutesButtons.Location = new System.Drawing.Point(0, 260);
            this.panelRoutesButtons.Name = "panelRoutesButtons";
            this.panelRoutesButtons.Padding = new System.Windows.Forms.Padding(8);
            this.panelRoutesButtons.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.panelRoutesButtons.Size = new System.Drawing.Size(942, 57);
            this.panelRoutesButtons.TabIndex = 1;
            // 
            // btnAddRoute
            // 
            this.btnAddRoute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddRoute.BackColor = System.Drawing.Color.FromArgb(22, 163, 74);
            this.btnAddRoute.FlatAppearance.BorderSize = 0;
            this.btnAddRoute.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddRoute.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Bold);
            this.btnAddRoute.ForeColor = System.Drawing.Color.White;
            this.btnAddRoute.Location = new System.Drawing.Point(11, 11);
            this.btnAddRoute.Name = "btnAddRoute";
            this.btnAddRoute.Size = new System.Drawing.Size(95, 35);
            this.btnAddRoute.TabIndex = 0;
            this.btnAddRoute.Text = "إضافة مسار";
            this.btnAddRoute.UseVisualStyleBackColor = false;
            this.btnAddRoute.Click += new System.EventHandler(this.btnAddRoute_Click);
            this.btnAddRoute.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnAddRoute.AutoSize = false;
            this.btnAddRoute.MinimumSize = new System.Drawing.Size(110, 40);
            this.btnAddRoute.Cursor = System.Windows.Forms.Cursors.Hand;
            // 
            // btnUpdateRoute
            // 
            this.btnUpdateRoute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdateRoute.BackColor = System.Drawing.Color.FromArgb(15, 118, 110);
            this.btnUpdateRoute.FlatAppearance.BorderSize = 0;
            this.btnUpdateRoute.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdateRoute.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Bold);
            this.btnUpdateRoute.ForeColor = System.Drawing.Color.White;
            this.btnUpdateRoute.Location = new System.Drawing.Point(112, 11);
            this.btnUpdateRoute.Name = "btnUpdateRoute";
            this.btnUpdateRoute.Size = new System.Drawing.Size(95, 35);
            this.btnUpdateRoute.TabIndex = 1;
            this.btnUpdateRoute.Text = "تعديل بيانات المسار";
            this.btnUpdateRoute.UseVisualStyleBackColor = false;
            this.btnUpdateRoute.Click += new System.EventHandler(this.btnUpdateRoute_Click);
            this.btnUpdateRoute.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnUpdateRoute.AutoSize = false;
            this.btnUpdateRoute.MinimumSize = new System.Drawing.Size(110, 40);
            this.btnUpdateRoute.Cursor = System.Windows.Forms.Cursors.Hand;
            // 
            // btnDeleteRoute
            // 
            this.btnDeleteRoute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteRoute.BackColor = System.Drawing.Color.FromArgb(198, 40, 40);
            this.btnDeleteRoute.FlatAppearance.BorderSize = 0;
            this.btnDeleteRoute.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteRoute.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Bold);
            this.btnDeleteRoute.ForeColor = System.Drawing.Color.White;
            this.btnDeleteRoute.Location = new System.Drawing.Point(213, 11);
            this.btnDeleteRoute.Name = "btnDeleteRoute";
            this.btnDeleteRoute.Size = new System.Drawing.Size(95, 35);
            this.btnDeleteRoute.TabIndex = 2;
            this.btnDeleteRoute.Text = "حذف المسار";
            this.btnDeleteRoute.UseVisualStyleBackColor = false;
            this.btnDeleteRoute.Click += new System.EventHandler(this.btnDeleteRoute_Click);
            this.btnDeleteRoute.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnDeleteRoute.AutoSize = false;
            this.btnDeleteRoute.MinimumSize = new System.Drawing.Size(110, 40);
            this.btnDeleteRoute.Cursor = System.Windows.Forms.Cursors.Hand;
            // 
            // btnClearRoute
            // 
            this.btnClearRoute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearRoute.BackColor = System.Drawing.Color.FromArgb(15, 118, 110);
            this.btnClearRoute.FlatAppearance.BorderSize = 0;
            this.btnClearRoute.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearRoute.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Bold);
            this.btnClearRoute.ForeColor = System.Drawing.Color.White;
            this.btnClearRoute.Location = new System.Drawing.Point(314, 11);
            this.btnClearRoute.Name = "btnClearRoute";
            this.btnClearRoute.Size = new System.Drawing.Size(95, 35);
            this.btnClearRoute.TabIndex = 3;
            this.btnClearRoute.Text = "تفريغ النموذج";
            this.btnClearRoute.UseVisualStyleBackColor = false;
            this.btnClearRoute.Click += new System.EventHandler(this.btnClearRoute_Click);
            this.btnClearRoute.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnClearRoute.AutoSize = false;
            this.btnClearRoute.MinimumSize = new System.Drawing.Size(110, 40);
            this.btnClearRoute.Cursor = System.Windows.Forms.Cursors.Hand;
            // 
            // panelRoutesFields
            // 
            this.panelRoutesFields.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.panelRoutesFields.Controls.Add(this.tableRoutesFields);
            this.panelRoutesFields.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelRoutesFields.Location = new System.Drawing.Point(0, 0);
            this.panelRoutesFields.Name = "panelRoutesFields";
            this.panelRoutesFields.Padding = new System.Windows.Forms.Padding(15);
            this.panelRoutesFields.Size = new System.Drawing.Size(942, 260);
            this.panelRoutesFields.TabIndex = 0;
            // 
            // tableRoutesFields
            // 
            this.tableRoutesFields.ColumnCount = 4;
            this.tableRoutesFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableRoutesFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableRoutesFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableRoutesFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableRoutesFields.Controls.Add(this.lblRouteName, 0, 0);
            this.tableRoutesFields.Controls.Add(this.txtRouteName, 1, 0);
            this.tableRoutesFields.Controls.Add(this.lblBus, 2, 0);
            this.tableRoutesFields.Controls.Add(this.cmbBus, 3, 0);
            this.tableRoutesFields.Controls.Add(this.lblStartPoint, 0, 1);
            this.tableRoutesFields.Controls.Add(this.txtStartPoint, 1, 1);
            this.tableRoutesFields.Controls.Add(this.lblEndPoint, 2, 1);
            this.tableRoutesFields.Controls.Add(this.txtEndPoint, 3, 1);
            this.tableRoutesFields.Controls.Add(this.lblDeparture, 0, 2);
            this.tableRoutesFields.Controls.Add(this.dtpDeparture, 1, 2);
            this.tableRoutesFields.Controls.Add(this.lblArrival, 2, 2);
            this.tableRoutesFields.Controls.Add(this.dtpArrival, 3, 2);
            this.tableRoutesFields.Controls.Add(this.lblFee, 0, 3);
            this.tableRoutesFields.Controls.Add(this.txtFee, 1, 3);
            this.tableRoutesFields.Controls.Add(this.lblRouteNotes, 2, 3);
            this.tableRoutesFields.Controls.Add(this.txtRouteNotes, 3, 3);
            this.tableRoutesFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableRoutesFields.Location = new System.Drawing.Point(15, 15);
            this.tableRoutesFields.Name = "tableRoutesFields";
            this.tableRoutesFields.RowCount = 4;
            this.tableRoutesFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableRoutesFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableRoutesFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableRoutesFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableRoutesFields.Size = new System.Drawing.Size(912, 230);
            this.tableRoutesFields.TabIndex = 0;
            // 
            // lblRouteName
            // 
            this.lblRouteName.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblRouteName.AutoSize = true;
            this.lblRouteName.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblRouteName.Location = new System.Drawing.Point(815, 1);
            this.lblRouteName.Name = "lblRouteName";
            this.lblRouteName.Size = new System.Drawing.Size(64, 42);
            this.lblRouteName.TabIndex = 0;
            this.lblRouteName.Text = "اسم المسار:";
            // 
            // txtRouteName
            // 
            this.txtRouteName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRouteName.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtRouteName.Location = new System.Drawing.Point(459, 3);
            this.txtRouteName.Name = "txtRouteName";
            this.txtRouteName.Size = new System.Drawing.Size(350, 28);
            this.txtRouteName.TabIndex = 0;
            // 
            // lblBus
            // 
            this.lblBus.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblBus.AutoSize = true;
            this.lblBus.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblBus.Location = new System.Drawing.Point(359, 12);
            this.lblBus.Name = "lblBus";
            this.lblBus.Size = new System.Drawing.Size(65, 21);
            this.lblBus.TabIndex = 1;
            this.lblBus.Text = "الحافلة:";
            // 
            // cmbBus
            // 
            this.cmbBus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbBus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBus.Font = new System.Drawing.Font("Tahoma", 10F);
            this.cmbBus.FormattingEnabled = true;
            this.cmbBus.Location = new System.Drawing.Point(3, 3);
            this.cmbBus.Name = "cmbBus";
            this.cmbBus.Size = new System.Drawing.Size(350, 29);
            this.cmbBus.TabIndex = 1;
            // 
            // lblStartPoint
            // 
            this.lblStartPoint.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblStartPoint.AutoSize = true;
            this.lblStartPoint.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblStartPoint.Location = new System.Drawing.Point(815, 46);
            this.lblStartPoint.Name = "lblStartPoint";
            this.lblStartPoint.Size = new System.Drawing.Size(59, 42);
            this.lblStartPoint.TabIndex = 2;
            this.lblStartPoint.Text = "نقطة البداية:";
            // 
            // txtStartPoint
            // 
            this.txtStartPoint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtStartPoint.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtStartPoint.Location = new System.Drawing.Point(459, 48);
            this.txtStartPoint.Name = "txtStartPoint";
            this.txtStartPoint.Size = new System.Drawing.Size(350, 28);
            this.txtStartPoint.TabIndex = 2;
            // 
            // lblEndPoint
            // 
            this.lblEndPoint.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblEndPoint.AutoSize = true;
            this.lblEndPoint.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblEndPoint.Location = new System.Drawing.Point(359, 46);
            this.lblEndPoint.Name = "lblEndPoint";
            this.lblEndPoint.Size = new System.Drawing.Size(60, 42);
            this.lblEndPoint.TabIndex = 3;
            this.lblEndPoint.Text = "نقطة النهاية:";
            // 
            // txtEndPoint
            // 
            this.txtEndPoint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtEndPoint.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtEndPoint.Location = new System.Drawing.Point(3, 48);
            this.txtEndPoint.Name = "txtEndPoint";
            this.txtEndPoint.Size = new System.Drawing.Size(350, 28);
            this.txtEndPoint.TabIndex = 3;
            // 
            // lblDeparture
            // 
            this.lblDeparture.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblDeparture.AutoSize = true;
            this.lblDeparture.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblDeparture.Location = new System.Drawing.Point(815, 91);
            this.lblDeparture.Name = "lblDeparture";
            this.lblDeparture.Size = new System.Drawing.Size(74, 42);
            this.lblDeparture.TabIndex = 4;
            this.lblDeparture.Text = "وقت الانطلاق:";
            // 
            // dtpDeparture
            // 
            this.dtpDeparture.CustomFormat = "HH:mm";
            this.dtpDeparture.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtpDeparture.Font = new System.Drawing.Font("Tahoma", 10F);
            this.dtpDeparture.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDeparture.Location = new System.Drawing.Point(459, 93);
            this.dtpDeparture.Name = "dtpDeparture";
            this.dtpDeparture.ShowUpDown = true;
            this.dtpDeparture.Size = new System.Drawing.Size(350, 28);
            this.dtpDeparture.TabIndex = 4;
            // 
            // lblArrival
            // 
            this.lblArrival.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblArrival.AutoSize = true;
            this.lblArrival.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblArrival.Location = new System.Drawing.Point(359, 91);
            this.lblArrival.Name = "lblArrival";
            this.lblArrival.Size = new System.Drawing.Size(65, 42);
            this.lblArrival.TabIndex = 5;
            this.lblArrival.Text = "وقت الوصول:";
            // 
            // dtpArrival
            // 
            this.dtpArrival.CustomFormat = "HH:mm";
            this.dtpArrival.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtpArrival.Font = new System.Drawing.Font("Tahoma", 10F);
            this.dtpArrival.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpArrival.Location = new System.Drawing.Point(3, 93);
            this.dtpArrival.Name = "dtpArrival";
            this.dtpArrival.ShowUpDown = true;
            this.dtpArrival.Size = new System.Drawing.Size(350, 28);
            this.dtpArrival.TabIndex = 5;
            // 
            // lblFee
            // 
            this.lblFee.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblFee.AutoSize = true;
            this.lblFee.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblFee.Location = new System.Drawing.Point(815, 172);
            this.lblFee.Name = "lblFee";
            this.lblFee.Size = new System.Drawing.Size(67, 21);
            this.lblFee.TabIndex = 6;
            this.lblFee.Text = "الرسوم:";
            // 
            // txtFee
            // 
            this.txtFee.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFee.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtFee.Location = new System.Drawing.Point(459, 138);
            this.txtFee.Name = "txtFee";
            this.txtFee.Size = new System.Drawing.Size(350, 28);
            this.txtFee.TabIndex = 6;
            this.txtFee.Text = "0";
            // 
            // lblRouteNotes
            // 
            this.lblRouteNotes.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblRouteNotes.AutoSize = true;
            this.lblRouteNotes.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblRouteNotes.Location = new System.Drawing.Point(359, 172);
            this.lblRouteNotes.Name = "lblRouteNotes";
            this.lblRouteNotes.Size = new System.Drawing.Size(79, 21);
            this.lblRouteNotes.TabIndex = 7;
            this.lblRouteNotes.Text = "ملاحظات:";
            // 
            // txtRouteNotes
            // 
            this.txtRouteNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRouteNotes.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtRouteNotes.Location = new System.Drawing.Point(3, 138);
            this.txtRouteNotes.Name = "txtRouteNotes";
            this.txtRouteNotes.Size = new System.Drawing.Size(350, 28);
            this.txtRouteNotes.TabIndex = 7;
            // 
            // panelBottom
            // 
            this.panelBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.panelBottom.Controls.Add(this.lblRecordCount);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 620);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(950, 30);
            this.panelBottom.TabIndex = 2;
            // 
            // lblRecordCount
            // 
            this.lblRecordCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRecordCount.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblRecordCount.ForeColor = System.Drawing.Color.DimGray;
            this.lblRecordCount.Location = new System.Drawing.Point(0, 0);
            this.lblRecordCount.Name = "lblRecordCount";
            this.lblRecordCount.Size = new System.Drawing.Size(950, 30);
            this.lblRecordCount.TabIndex = 0;
            this.lblRecordCount.Text = "عدد الحافلات: 0";
            this.lblRecordCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TransportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelTitle);
            this.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.MinimumSize = new System.Drawing.Size(850, 600);
            this.Name = "TransportForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Size = new System.Drawing.Size(950, 650);
            ((System.ComponentModel.ISupportInitialize)(this.panelTitle)).EndInit();
            this.panelTitle.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabBuses.ResumeLayout(false);
            this.tabBuses.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBuses)).EndInit();
            this.panelBusesButtons.ResumeLayout(false);
            this.panelBusesFields.ResumeLayout(false);
            this.tableBusesFields.ResumeLayout(false);
            this.tableBusesFields.PerformLayout();
            this.tabRoutes.ResumeLayout(false);
            this.tabRoutes.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRoutes)).EndInit();
            this.panelRoutesButtons.ResumeLayout(false);
            this.panelRoutesFields.ResumeLayout(false);
            this.tableRoutesFields.ResumeLayout(false);
            this.tableRoutesFields.PerformLayout();
            this.panelBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }


        #endregion


        private Krypton.Toolkit.KryptonPanel panelTitle;

        private System.Windows.Forms.Label lblTitle;

        private System.Windows.Forms.TabControl tabControl;

        private System.Windows.Forms.TabPage tabBuses;

        private System.Windows.Forms.TabPage tabRoutes;

        private System.Windows.Forms.DataGridView dataGridViewBuses;

        private System.Windows.Forms.FlowLayoutPanel panelBusesButtons;

        private System.Windows.Forms.Button btnAddBus;

        private System.Windows.Forms.Button btnUpdateBus;

        private System.Windows.Forms.Button btnDeleteBus;

        private System.Windows.Forms.Button btnClearBus;

        private System.Windows.Forms.Panel panelBusesFields;

        private System.Windows.Forms.TableLayoutPanel tableBusesFields;

        private System.Windows.Forms.Label lblBusNumber;

        private System.Windows.Forms.TextBox txtBusNumber;

        private System.Windows.Forms.Label lblDriverName;

        private System.Windows.Forms.TextBox txtDriverName;

        private System.Windows.Forms.Label lblDriverPhone;

        private System.Windows.Forms.TextBox txtDriverPhone;

        private System.Windows.Forms.Label lblCapacity;

        private System.Windows.Forms.TextBox txtCapacity;

        private System.Windows.Forms.Label lblNotes;

        private System.Windows.Forms.TextBox txtNotes;

        private System.Windows.Forms.DataGridView dataGridViewRoutes;

        private System.Windows.Forms.FlowLayoutPanel panelRoutesButtons;

        private System.Windows.Forms.Button btnAddRoute;

        private System.Windows.Forms.Button btnUpdateRoute;

        private System.Windows.Forms.Button btnDeleteRoute;

        private System.Windows.Forms.Button btnClearRoute;

        private System.Windows.Forms.Panel panelRoutesFields;

        private System.Windows.Forms.TableLayoutPanel tableRoutesFields;

        private System.Windows.Forms.Label lblRouteName;

        private System.Windows.Forms.TextBox txtRouteName;

        private System.Windows.Forms.Label lblBus;

        private System.Windows.Forms.ComboBox cmbBus;

        private System.Windows.Forms.Label lblStartPoint;

        private System.Windows.Forms.TextBox txtStartPoint;

        private System.Windows.Forms.Label lblEndPoint;

        private System.Windows.Forms.TextBox txtEndPoint;

        private System.Windows.Forms.Label lblDeparture;

        private System.Windows.Forms.DateTimePicker dtpDeparture;

        private System.Windows.Forms.Label lblArrival;

        private System.Windows.Forms.DateTimePicker dtpArrival;

        private System.Windows.Forms.Label lblFee;

        private System.Windows.Forms.TextBox txtFee;

        private System.Windows.Forms.Label lblRouteNotes;

        private System.Windows.Forms.TextBox txtRouteNotes;

        private System.Windows.Forms.Panel panelBottom;

        private System.Windows.Forms.Label lblRecordCount;
    }
}
