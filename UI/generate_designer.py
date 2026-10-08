# -*- coding: utf-8 -*-
import sys
import os

controls = [
    ("StudentNumber", "رقم الطالب", "TextBox", "gbStudent", False),
    ("FullName", "الاسم الرباعي", "TextBox", "gbStudent", False),
    ("Gender", "الجنس", "ComboBox", "gbStudent", True),
    ("BirthDate", "تاريخ الميلاد", "DateTimePicker", "gbStudent", False),
    ("BirthPlace", "مكان الميلاد", "TextBox", "gbStudent", False),
    ("Nationality", "الجنسية", "TextBox", "gbStudent", False),
    ("NationalId", "رقم الهوية", "TextBox", "gbStudent", False),
    ("Phone", "هاتف الطالب", "TextBox", "gbStudent", False),
    ("Status", "الحالة", "ComboBox", "gbStudent", True),

    ("GuardianName", "اسم ولي الأمر", "TextBox", "gbGuardian", False),
    ("GuardianRelation", "صلة القرابة", "TextBox", "gbGuardian", False),
    ("GuardianPhone", "رقم الهاتف", "TextBox", "gbGuardian", False),
    ("GuardianEmail", "البريد الإلكتروني", "TextBox", "gbGuardian", False),
    ("GuardianJob", "الوظيفة", "TextBox", "gbGuardian", False),

    ("Governorate", "المحافظة", "TextBox", "gbAddress", False),
    ("District", "المديرية", "TextBox", "gbAddress", False),
    ("Address", "العنوان", "TextBox", "gbAddress", False)
]

# يعرّف دالة مساعدة ضمن أدوات المشروع.
def generate_designer_code():
    res = []
    res.append("namespace SchoolSystem.UI {")
    res.append("    partial class StudentsForm {")
    res.append("        private System.ComponentModel.IContainer components = null;")
    res.append("        protected override void Dispose(bool disposing) {")
    res.append("            if (disposing && (components != null)) components.Dispose();")
    res.append("            base.Dispose(disposing);")
    res.append("        }")
    res.append("        private void InitializeComponent() {")
    res.append("            this.components = new System.ComponentModel.Container();")
    res.append("            this.panelTitle = new System.Windows.Forms.Panel();")
    res.append("            this.lblTitle = new System.Windows.Forms.Label();")

    for name, lbl, typ, grp, isCombo in controls:
        res.append(f"            this.lbl{name} = new System.Windows.Forms.Label();")
        res.append(f"            this.{'cmb' if isCombo else ('dtp' if typ=='DateTimePicker' else 'txt')}{name} = new System.Windows.Forms.{typ}();")
        control_name = f"{'cmb' if isCombo else ('dtp' if typ=='DateTimePicker' else 'txt')}{name}"
        if isCombo:
            res.append(f"            this.{control_name}.BackColor = System.Drawing.Color.White;")
            res.append(f"            this.{control_name}.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);")
            res.append(f"            this.{control_name}.Font = new System.Drawing.Font(\"Tahoma\", 10F);")
            res.append(f"            this.{control_name}.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);")
            res.append(f"            this.{control_name}.RightToLeft = System.Windows.Forms.RightToLeft.Yes;")
        elif typ == "TextBox":
            res.append(f"            this.{control_name}.BackColor = System.Drawing.Color.White;")
            res.append(f"            this.{control_name}.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);")
            res.append(f"            this.{control_name}.Font = new System.Drawing.Font(\"Tahoma\", 10F);")
            res.append(f"            this.{control_name}.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;")
            res.append(f"            this.{control_name}.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);")
            res.append(f"            this.{control_name}.RightToLeft = System.Windows.Forms.RightToLeft.Yes;")

    res.append("            this.gbStudent = new System.Windows.Forms.GroupBox();")
    res.append("            this.tlpStudent = new System.Windows.Forms.TableLayoutPanel();")
    res.append("            this.gbGuardian = new System.Windows.Forms.GroupBox();")
    res.append("            this.tlpGuardian = new System.Windows.Forms.TableLayoutPanel();")
    res.append("            this.gbAddress = new System.Windows.Forms.GroupBox();")
    res.append("            this.tlpAddress = new System.Windows.Forms.TableLayoutPanel();")
    res.append("            this.gbPhoto = new System.Windows.Forms.GroupBox();")
    res.append("            this.picStudent = new System.Windows.Forms.PictureBox();")
    res.append("            this.btnChooseImage = new System.Windows.Forms.Button();")
    res.append("            this.btnRemoveImage = new System.Windows.Forms.Button();")

    res.append("            this.dgvStudents = new System.Windows.Forms.DataGridView();")
    res.append("            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);")
    res.append("            this.splitContainerMain = new System.Windows.Forms.SplitContainer();")
    res.append("            this.pnlSearch = new System.Windows.Forms.Panel();")
    res.append("            this.txtSearch = new System.Windows.Forms.TextBox();")
    res.append("            this.btnSearch = new System.Windows.Forms.Button();")
    res.append("            this.btnClearFilters = new System.Windows.Forms.Button();")
    res.append("            this.btnReload = new System.Windows.Forms.Button();")
    res.append("            this.cmbFilterClass = new System.Windows.Forms.ComboBox();")
    res.append("            this.cmbFilterStatus = new System.Windows.Forms.ComboBox();")
    res.append("            this.lblSearch = new System.Windows.Forms.Label();")
    res.append("            this.lblCount = new System.Windows.Forms.Label();")
    res.append("            this.pnlButtons = new System.Windows.Forms.FlowLayoutPanel();")
    res.append("            this.btnAdd = new System.Windows.Forms.Button();")
    res.append("            this.btnSave = new System.Windows.Forms.Button();")
    res.append("            this.btnUpdate = new System.Windows.Forms.Button();")
    res.append("            this.btnDelete = new System.Windows.Forms.Button();")
    res.append("            this.btnCancel = new System.Windows.Forms.Button();")
    res.append("            this.btnRefresh = new System.Windows.Forms.Button();")
    res.append("            this.btnExportExcel = new System.Windows.Forms.Button();")
    res.append("            this.btnPrint = new System.Windows.Forms.Button();")
    res.append("            this.btnStudentProfile = new System.Windows.Forms.Button();")
    res.append("            this.btnClose = new System.Windows.Forms.Button();")
    res.append("            this.pnlRight = new System.Windows.Forms.Panel();")

    res.append("            ((System.ComponentModel.ISupportInitialize)(this.dgvStudents)).BeginInit();")
    res.append("            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();")
    res.append("            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();")
    res.append("            this.splitContainerMain.Panel1.SuspendLayout();")
    res.append("            this.splitContainerMain.Panel2.SuspendLayout();")
    res.append("            this.splitContainerMain.SuspendLayout();")
    res.append("            this.pnlRight.SuspendLayout();")
    res.append("            this.pnlButtons.SuspendLayout();")
    res.append("            this.pnlSearch.SuspendLayout();")
    res.append("            this.SuspendLayout();")

    # يعرّف دالة مساعدة ضمن أدوات المشروع.
    def create_tlp(name, c_list):
        r = []
        r.append(f"            this.{name}.ColumnCount = 2;")
        r.append(f"            this.{name}.RowCount = {len(c_list)};")
        r.append(f"            this.{name}.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));")
        r.append(f"            this.{name}.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));")
        r.append(f"            this.{name}.Dock = System.Windows.Forms.DockStyle.Top;")
        r.append(f"            this.{name}.AutoSize = true;")
        r.append(f"            this.{name}.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;")
        r.append(f"            this.{name}.Padding = new System.Windows.Forms.Padding(0, 15, 0, 0);")
        for i, (cname, lbl, typ, _, isCombo) in enumerate(c_list):
            ctrl_name = ('cmb' if isCombo else ('dtp' if typ=='DateTimePicker' else 'txt')) + cname
            r.append(f"            this.{name}.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));")
            r.append(f"            this.lbl{cname}.Text = \"{lbl}\";")
            r.append(f"            this.lbl{cname}.AutoSize = true;")
            r.append(f"            this.lbl{cname}.Anchor = System.Windows.Forms.AnchorStyles.Right;")
            r.append(f"            this.lbl{cname}.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);")
            r.append(f"            this.{ctrl_name}.Dock = System.Windows.Forms.DockStyle.Fill;")
            r.append(f"            this.{ctrl_name}.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);")
            r.append(f"            this.{name}.Controls.Add(this.lbl{cname}, 0, {i});")
            r.append(f"            this.{name}.Controls.Add(this.{ctrl_name}, 1, {i});")
        return r

    for idx, (g_id, g_name, g_lbl) in enumerate([("gbStudent", "tlpStudent", "بيانات الطالب"), ("gbGuardian", "tlpGuardian", "ولي الأمر"), ("gbAddress", "tlpAddress", "العنوان")]):
        c_list = [c for c in controls if c[3] == g_id]
        res.append(f"            this.{g_id}.Text = \"{g_lbl}\";")
        res.append(f"            this.{g_id}.Dock = System.Windows.Forms.DockStyle.Top;")
        res.append(f"            this.{g_id}.AutoSize = true;")
        res.append(f"            this.{g_id}.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;")
        res.append(f"            this.{g_id}.Padding = new System.Windows.Forms.Padding(10, 20, 10, 10);")
        res.append(f"            this.{g_id}.Controls.Add(this.{g_name});")
        res.extend(create_tlp(g_name, c_list))

    res.append("            this.gbPhoto.Text = \"الصورة\";")
    res.append("            this.gbPhoto.Dock = System.Windows.Forms.DockStyle.Top;")
    res.append("            this.gbPhoto.Height = 160;")
    res.append("            this.gbPhoto.Padding = new System.Windows.Forms.Padding(10);")
    res.append("            this.picStudent.Dock = System.Windows.Forms.DockStyle.Right;")
    res.append("            this.picStudent.Width = 110;")
    res.append("            this.picStudent.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;")
    res.append("            this.picStudent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;")
    res.append("            this.btnChooseImage.Text = \"اختيار الصورة\";")
    res.append("            this.btnChooseImage.Location = new System.Drawing.Point(30, 40);")
    res.append("            this.btnChooseImage.Size = new System.Drawing.Size(120, 35);")
    res.append("            this.btnChooseImage.Click += new System.EventHandler(this.btnChooseImage_Click);")
    res.append("            this.btnRemoveImage.Text = \"حذف الصورة\";")
    res.append("            this.btnRemoveImage.Location = new System.Drawing.Point(30, 85);")
    res.append("            this.btnRemoveImage.Size = new System.Drawing.Size(120, 35);")
    res.append("            this.btnRemoveImage.Click += new System.EventHandler(this.btnRemoveImage_Click);")
    res.append("            this.gbPhoto.Controls.Add(this.picStudent);")
    res.append("            this.gbPhoto.Controls.Add(this.btnChooseImage);")
    res.append("            this.gbPhoto.Controls.Add(this.btnRemoveImage);")

    res.append("            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;")
    res.append("            this.pnlRight.AutoScroll = true;")
    res.append("            this.pnlRight.Padding = new System.Windows.Forms.Padding(10);")

    res.append("            this.pnlRight.Controls.Add(this.gbPhoto);")
    res.append("            this.pnlRight.Controls.Add(this.gbAddress);")
    res.append("            this.pnlRight.Controls.Add(this.gbGuardian);")
    res.append("            this.pnlRight.Controls.Add(this.gbStudent);")

    res.append("            this.gbStudent.BringToFront();")
    res.append("            this.gbGuardian.BringToFront();")
    res.append("            this.gbAddress.BringToFront();")
    res.append("            this.gbPhoto.BringToFront();")

    res.append("            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;")
    res.append("            this.splitContainerMain.Panel1.Controls.Add(this.dgvStudents);")
    res.append("            this.splitContainerMain.Panel1.Padding = new System.Windows.Forms.Padding(10, 10, 0, 10);")
    res.append("            this.splitContainerMain.Panel2.Controls.Add(this.pnlRight);")
    res.append("            this.splitContainerMain.RightToLeft = System.Windows.Forms.RightToLeft.Yes;")

    res.append("            this.dgvStudents.Dock = System.Windows.Forms.DockStyle.Fill;")
    res.append("            this.dgvStudents.AllowUserToAddRows = false;")
    res.append("            this.dgvStudents.AllowUserToDeleteRows = false;")
    res.append("            this.dgvStudents.ReadOnly = true;")
    res.append("            this.dgvStudents.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;")
    res.append("            this.dgvStudents.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStudents_CellClick);")
    res.append("            this.dgvStudents.SelectionChanged += new System.EventHandler(this.dgvStudents_SelectionChanged);")
    res.append("            this.dgvStudents.BackgroundColor = System.Drawing.Color.White;")
    res.append("            this.dgvStudents.BorderStyle = System.Windows.Forms.BorderStyle.None;")
    res.append("            this.dgvStudents.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;")
    res.append("            this.dgvStudents.RowHeadersVisible = false;")
    res.append("            this.dgvStudents.MultiSelect = false;")
    res.append("            this.dgvStudents.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;")
    res.append("            this.dgvStudents.AllowUserToResizeRows = false;")
    res.append("            this.dgvStudents.EnableHeadersVisualStyles = false;")
    res.append("            this.dgvStudents.ColumnHeadersHeight = 42;")
    res.append("            this.dgvStudents.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;")
    res.append("            this.dgvStudents.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);")
    res.append("            this.dgvStudents.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;")
    res.append("            this.dgvStudents.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font(\"Tahoma\", 10F, System.Drawing.FontStyle.Bold);")
    res.append("            this.dgvStudents.ColumnHeadersDefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;")
    res.append("            this.dgvStudents.DefaultCellStyle.BackColor = System.Drawing.Color.White;")
    res.append("            this.dgvStudents.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);")
    res.append("            this.dgvStudents.DefaultCellStyle.Font = new System.Drawing.Font(\"Tahoma\", 10F);")
    res.append("            this.dgvStudents.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;")
    res.append("            this.dgvStudents.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(219, 234, 254);")
    res.append("            this.dgvStudents.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(15, 23, 42);")
    res.append("            this.dgvStudents.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(6, 3, 6, 3);")
    res.append("            this.dgvStudents.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);")
    res.append("            this.dgvStudents.RowsDefaultCellStyle.BackColor = System.Drawing.Color.White;")
    res.append("            this.dgvStudents.RowsDefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);")
    res.append("            this.dgvStudents.RowTemplate.Height = 36;")
    res.append("            this.dgvStudents.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;")
    res.append("            this.dgvStudents.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;")
    res.append("            this.dgvStudents.GridColor = System.Drawing.Color.FromArgb(203, 213, 225);")

    res.append("            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;")
    res.append("            this.pnlButtons.Height = 62;")
    res.append("            this.pnlButtons.Padding = new System.Windows.Forms.Padding(8, 8, 8, 8);")
    res.append("            this.pnlButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;")
    res.append("            this.pnlButtons.WrapContents = false;")
    res.append("            this.pnlButtons.AutoScroll = false;")
    res.append("            this.pnlButtons.RightToLeft = System.Windows.Forms.RightToLeft.Yes;")

    b_list = ["btnAdd", "btnSave", "btnUpdate", "btnDelete", "btnCancel", "btnRefresh", "btnExportExcel", "btnPrint", "btnStudentProfile", "btnClose"]
    b_txts = ["إضافة طالب", "حفظ", "تعديل", "أرشفة", "إلغاء", "تحديث القائمة", "تصدير Excel", "طباعة البطاقة", "ملف الطالب", "إغلاق"]
    b_widths = [110, 110, 110, 110, 110, 110, 110, 110, 110, 110]
    b_colors = {
        "btnClose": "71, 85, 105",
        "btnPrint": "15, 118, 110",
        "btnExportExcel": "13, 148, 136",
        "btnRefresh": "71, 85, 105",
        "btnCancel": "71, 85, 105",
        "btnDelete": "198, 40, 40",
        "btnUpdate": "15, 118, 110",
        "btnSave": "22, 163, 74",
        "btnAdd": "22, 163, 74"
    }
    for b, t, w in zip(b_list, b_txts, b_widths):
        res.append(f"            this.{b}.Text = \"{t}\";")
        res.append(f"            this.{b}.Size = new System.Drawing.Size({w}, 40);")
        res.append(f"            this.{b}.MinimumSize = new System.Drawing.Size(110, 40);")
        res.append(f"            this.{b}.Margin = new System.Windows.Forms.Padding(4);")
        res.append(f"            this.{b}.AutoSize = false;")
        res.append(f"            this.{b}.BackColor = System.Drawing.Color.FromArgb({b_colors[b]});")
        res.append(f"            this.{b}.ForeColor = System.Drawing.Color.White;")
        res.append(f"            this.{b}.FlatStyle = System.Windows.Forms.FlatStyle.Flat;")
        res.append(f"            this.{b}.FlatAppearance.BorderSize = 0;")
        res.append(f"            this.{b}.Font = new System.Drawing.Font(\"Tahoma\", 10.5F, System.Drawing.FontStyle.Bold);")
        res.append(f"            this.{b}.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;")
        res.append(f"            this.{b}.UseVisualStyleBackColor = false;")
        res.append(f"            this.{b}.Cursor = System.Windows.Forms.Cursors.Hand;")
        res.append(f"            this.pnlButtons.Controls.Add(this.{b});")
        if b=="btnCancel":
            res.append(f"            this.{b}.Click += new System.EventHandler(this.btnCancel_Click);")
        elif b=="btnRefresh":
            res.append(f"            this.{b}.Click += new System.EventHandler(this.btnRefresh_Click);")
        else:
            res.append(f"            this.{b}.Click += new System.EventHandler(this.{b}_Click);")

    res.append("            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;")
    res.append("            this.pnlSearch.Height = 72;")
    res.append("            this.pnlSearch.Padding = new System.Windows.Forms.Padding(10);")
    res.append("            this.pnlSearch.BackColor = System.Drawing.Color.White;")

    res.append("            this.lblCount.Location = new System.Drawing.Point(20, 24);")
    res.append("            this.lblCount.Size = new System.Drawing.Size(90, 24);")
    res.append("            this.lblCount.AutoSize = false;")
    res.append("            this.lblCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;")

    res.append("            this.lblSearch.Location = new System.Drawing.Point(430, 24);")
    res.append("            this.lblSearch.Size = new System.Drawing.Size(54, 24);")
    res.append("            this.lblSearch.AutoSize = false;")
    res.append("            this.lblSearch.Text = \"بحث:\";")
    res.append("            this.lblSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;")

    for b,x,y,w,h in [
        ("cmbFilterClass",115,21,155,30),
        ("cmbFilterStatus",280,21,135,30),
        ("txtSearch",485,21,300,30),
        ("btnSearch",820,16,96,40),
        ("btnClearFilters",922,16,118,40),
        ("btnReload",1045,16,136,40)
    ]:
        res.append(f"            this.{b}.Location = new System.Drawing.Point({x}, {y});")
        res.append(f"            this.{b}.Size = new System.Drawing.Size({w}, {h});")
        res.append(f"            this.{b}.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));")

    res.append("            this.btnSearch.Text = \"بحث\";")
    res.append("            this.btnSearch.BackColor = System.Drawing.Color.FromArgb(37, 99, 235);")
    res.append("            this.btnSearch.ForeColor = System.Drawing.Color.White;")
    res.append("            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;")
    res.append("            this.btnSearch.FlatAppearance.BorderSize = 0;")
    res.append("            this.btnSearch.Font = new System.Drawing.Font(\"Tahoma\", 10.5F, System.Drawing.FontStyle.Bold);")
    res.append("            this.btnSearch.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;")
    res.append("            this.btnSearch.UseVisualStyleBackColor = false;")
    res.append("            this.btnSearch.MinimumSize = new System.Drawing.Size(110, 40);")
    res.append("            this.btnSearch.Cursor = System.Windows.Forms.Cursors.Hand;")
    res.append("            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);")
    res.append("            this.btnClearFilters.Text = \"مسح الفلاتر\";")
    res.append("            this.btnClearFilters.BackColor = System.Drawing.Color.FromArgb(71, 85, 105);")
    res.append("            this.btnClearFilters.ForeColor = System.Drawing.Color.White;")
    res.append("            this.btnClearFilters.FlatStyle = System.Windows.Forms.FlatStyle.Flat;")
    res.append("            this.btnClearFilters.FlatAppearance.BorderSize = 0;")
    res.append("            this.btnClearFilters.Font = new System.Drawing.Font(\"Tahoma\", 10.5F, System.Drawing.FontStyle.Bold);")
    res.append("            this.btnClearFilters.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;")
    res.append("            this.btnClearFilters.UseVisualStyleBackColor = false;")
    res.append("            this.btnClearFilters.MinimumSize = new System.Drawing.Size(110, 40);")
    res.append("            this.btnClearFilters.Cursor = System.Windows.Forms.Cursors.Hand;")
    res.append("            this.btnClearFilters.Click += new System.EventHandler(this.btnClearFilters_Click);")
    res.append("            this.btnReload.Text = \"تحديث البيانات\";")
    res.append("            this.btnReload.BackColor = System.Drawing.Color.FromArgb(71, 85, 105);")
    res.append("            this.btnReload.ForeColor = System.Drawing.Color.White;")
    res.append("            this.btnReload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;")
    res.append("            this.btnReload.FlatAppearance.BorderSize = 0;")
    res.append("            this.btnReload.Font = new System.Drawing.Font(\"Tahoma\", 10.5F, System.Drawing.FontStyle.Bold);")
    res.append("            this.btnReload.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;")
    res.append("            this.btnReload.UseVisualStyleBackColor = false;")
    res.append("            this.btnReload.MinimumSize = new System.Drawing.Size(110, 40);")
    res.append("            this.btnReload.Cursor = System.Windows.Forms.Cursors.Hand;")
    res.append("            this.btnReload.Click += new System.EventHandler(this.btnRefresh_Click);")
    res.append("            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);")

    res.append("            this.pnlSearch.Controls.Add(this.lblCount);")
    res.append("            this.pnlSearch.Controls.Add(this.cmbFilterClass);")
    res.append("            this.pnlSearch.Controls.Add(this.cmbFilterStatus);")
    res.append("            this.pnlSearch.Controls.Add(this.lblSearch);")
    res.append("            this.pnlSearch.Controls.Add(this.txtSearch);")
    res.append("            this.pnlSearch.Controls.Add(this.btnSearch);")
    res.append("            this.pnlSearch.Controls.Add(this.btnClearFilters);")
    res.append("            this.pnlSearch.Controls.Add(this.btnReload);")

    res.append("            this.lblCount.BringToFront();")
    res.append("            this.btnReload.BringToFront();")
    res.append("            this.btnSearch.BringToFront();")
    res.append("            this.txtSearch.BringToFront();")
    res.append("            this.cmbFilterStatus.BringToFront();")
    res.append("            this.cmbFilterClass.BringToFront();")

    res.append("            this.Controls.Add(this.splitContainerMain);")
    res.append("            this.Controls.Add(this.pnlSearch);")
    res.append("            this.Controls.Add(this.pnlButtons);")
    res.append("            this.panelTitle.Controls.Add(this.lblTitle);")
    res.append("            this.panelTitle.Dock = System.Windows.Forms.DockStyle.Top;")
    res.append("            this.panelTitle.Height = 60;")
    res.append("            this.panelTitle.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);")
    res.append("            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;")
    res.append("            this.lblTitle.Text = \"إدارة الطلاب\";")
    res.append("            this.lblTitle.Font = new System.Drawing.Font(\"Tahoma\", 15F, System.Drawing.FontStyle.Bold);")
    res.append("            this.lblTitle.ForeColor = System.Drawing.Color.White;")
    res.append("            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;")
    res.append("            this.Controls.Add(this.panelTitle);")
    res.append("            this.ClientSize = new System.Drawing.Size(1200, 750);")
    res.append("            this.Name = \"StudentsForm\";")
    res.append("            this.Text = \"إدارة الطلاب\";")
    res.append("            this.Load += new System.EventHandler(this.StudentsForm_Load);")
    res.append("            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;")
    res.append("            this.RightToLeftLayout = true;")

    res.append("            ((System.ComponentModel.ISupportInitialize)(this.dgvStudents)).EndInit();")
    res.append("            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();")
    res.append("            this.splitContainerMain.Panel1.ResumeLayout(false);")
    res.append("            this.splitContainerMain.Panel2.ResumeLayout(false);")
    res.append("            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();")
    res.append("            this.splitContainerMain.ResumeLayout(false);")
    res.append("            this.pnlRight.ResumeLayout(false);")
    res.append("            this.pnlButtons.ResumeLayout(false);")
    res.append("            this.pnlSearch.ResumeLayout(false);")
    res.append("            this.ResumeLayout(false);")
    res.append("        }")

    for name, _, typ, _, isCombo in controls:
        res.append(f"        private System.Windows.Forms.Label lbl{name};")
        res.append(f"        private System.Windows.Forms.{typ} {'cmb' if isCombo else ('dtp' if typ=='DateTimePicker' else 'txt')}{name};")

    res.extend([
        "        private System.Windows.Forms.GroupBox gbStudent;",
        "        private System.Windows.Forms.TableLayoutPanel tlpStudent;",
        "        private System.Windows.Forms.GroupBox gbGuardian;",
        "        private System.Windows.Forms.TableLayoutPanel tlpGuardian;",
        "        private System.Windows.Forms.GroupBox gbAddress;",
        "        private System.Windows.Forms.TableLayoutPanel tlpAddress;",
        "        private System.Windows.Forms.GroupBox gbPhoto;",
        "        private System.Windows.Forms.PictureBox picStudent;",
        "        private System.Windows.Forms.Button btnChooseImage;",
        "        private System.Windows.Forms.Button btnRemoveImage;",
        "        private System.Windows.Forms.DataGridView dgvStudents;",
        "        private System.Windows.Forms.ErrorProvider errorProvider1;",
        "        private System.Windows.Forms.Panel panelTitle;",
        "        private System.Windows.Forms.Label lblTitle;",
        "        private System.Windows.Forms.SplitContainer splitContainerMain;",
        "        private System.Windows.Forms.Panel pnlSearch;",
        "        private System.Windows.Forms.TextBox txtSearch;",
        "        private System.Windows.Forms.Button btnSearch;",
        "        private System.Windows.Forms.Button btnClearFilters;",
        "        private System.Windows.Forms.Button btnReload;",
        "        private System.Windows.Forms.ComboBox cmbFilterClass;",
        "        private System.Windows.Forms.ComboBox cmbFilterStatus;",
        "        private System.Windows.Forms.Label lblSearch;",
        "        private System.Windows.Forms.Label lblCount;",
        "        private System.Windows.Forms.FlowLayoutPanel pnlButtons;",
        "        private System.Windows.Forms.Button btnAdd;",
        "        private System.Windows.Forms.Button btnSave;",
        "        private System.Windows.Forms.Button btnUpdate;",
        "        private System.Windows.Forms.Button btnDelete;",
        "        private System.Windows.Forms.Button btnCancel;",
        "        private System.Windows.Forms.Button btnRefresh;",
        "        private System.Windows.Forms.Button btnExportExcel;",
        "        private System.Windows.Forms.Button btnPrint;",
        "        private System.Windows.Forms.Button btnStudentProfile;",
        "        private System.Windows.Forms.Button btnClose;",
        "        private System.Windows.Forms.Panel pnlRight;",
        "    }",
        "}"
    ])

    with open(r'd:\_Getintopc.com_VS2022_2_2\SchoolSystem\UI\StudentsForm.Designer.cs', 'w', encoding='utf-8') as f:
        f.write('\n'.join(res))

if __name__ == '__main__':
    generate_designer_code()
    print("Done")
