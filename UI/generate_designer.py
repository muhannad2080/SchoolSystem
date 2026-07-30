# -*- coding: utf-8 -*-
import sys
import os

controls = [
    # (name, label, type, group, isCombo)
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
    
    # Declarations
    for name, lbl, typ, grp, isCombo in controls:
        res.append(f"            this.lbl{name} = new System.Windows.Forms.Label();")
        res.append(f"            this.{'cmb' if isCombo else ('dtp' if typ=='DateTimePicker' else 'txt')}{name} = new System.Windows.Forms.{typ}();")
    
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
    res.append("            this.btnReload = new System.Windows.Forms.Button();")
    res.append("            this.cmbFilterClass = new System.Windows.Forms.ComboBox();")
    res.append("            this.cmbFilterStatus = new System.Windows.Forms.ComboBox();")
    res.append("            this.lblCount = new System.Windows.Forms.Label();")
    res.append("            this.pnlButtons = new System.Windows.Forms.Panel();")
    res.append("            this.btnAdd = new System.Windows.Forms.Button();")
    res.append("            this.btnSave = new System.Windows.Forms.Button();")
    res.append("            this.btnUpdate = new System.Windows.Forms.Button();")
    res.append("            this.btnDelete = new System.Windows.Forms.Button();")
    res.append("            this.btnCancel = new System.Windows.Forms.Button();")
    res.append("            this.btnRefresh = new System.Windows.Forms.Button();")
    res.append("            this.btnExportExcel = new System.Windows.Forms.Button();")
    res.append("            this.btnPrint = new System.Windows.Forms.Button();")
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

    # Create GroupBoxes (Added backwards so Top Docking stacks them correctly top-to-bottom)
    # Actually DockStyle.Top stacks in reverse order of addition.
    # We want: Photo, Address, Guardian, Student (to appear top to bottom, the last added goes to the bottom? No, first added is Top-most)
    # Let's add them to pnlRight in this order: Student, Guardian, Address, Photo. So we add them in the code that way!
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
    
    # Add controls in reverse order of how we want them to layout top-to-bottom.
    # To place gbStudent at the Top, it should be brought to front or added first? 
    # Winforms Dock=Top adds items so the first one added is Top-most.
    res.append("            this.pnlRight.Controls.Add(this.gbPhoto);")
    res.append("            this.pnlRight.Controls.Add(this.gbAddress);")
    res.append("            this.pnlRight.Controls.Add(this.gbGuardian);")
    res.append("            this.pnlRight.Controls.Add(this.gbStudent);")
    
    # Reorder bringToFront so they stack Student(Top), Guardian, Address, Photo
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

    res.append("            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;")
    res.append("            this.pnlButtons.Height = 70;")
    res.append("            this.pnlButtons.Padding = new System.Windows.Forms.Padding(10);")
    
    b_list = ["btnClose", "btnPrint", "btnExportExcel", "btnRefresh", "btnCancel", "btnDelete", "btnUpdate", "btnSave", "btnAdd"]
    b_txts = ["إغلاق", "طباعة", "تصدير Excel", "تحديث", "إلغاء", "حذف", "تعديل", "حفظ", "جديد"]
    for i, (b, t) in enumerate(zip(b_list, b_txts)):
        res.append(f"            this.{b}.Text = \"{t}\";")
        res.append(f"            this.{b}.Dock = System.Windows.Forms.DockStyle.Right;")
        res.append(f"            this.{b}.Width = 90;")
        res.append(f"            this.{b}.Margin = new System.Windows.Forms.Padding(5);")
        res.append(f"            this.pnlButtons.Controls.Add(this.{b});")
        res.append(f"            this.{b}.BringToFront();") # So they order correctly
        if b=="btnCancel": 
            res.append(f"            this.{b}.Click += new System.EventHandler(this.btnCancel_Click);")
        elif b=="btnRefresh":
            res.append(f"            this.{b}.Click += new System.EventHandler(this.btnRefresh_Click);")
        else:
            res.append(f"            this.{b}.Click += new System.EventHandler(this.{b}_Click);")

    res.append("            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;")
    res.append("            this.pnlSearch.Height = 60;")
    res.append("            this.pnlSearch.Padding = new System.Windows.Forms.Padding(10);")
    res.append("            this.btnReload.Text = \"تحديث\";")
    res.append("            this.btnReload.Dock = System.Windows.Forms.DockStyle.Right;")
    res.append("            this.btnReload.Width = 80;")
    res.append("            this.btnReload.Click += new System.EventHandler(this.btnRefresh_Click);")
    res.append("            this.btnSearch.Text = \"بحث\";")
    res.append("            this.btnSearch.Dock = System.Windows.Forms.DockStyle.Right;")
    res.append("            this.btnSearch.Width = 80;")
    res.append("            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);")
    res.append("            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Right;")
    res.append("            this.txtSearch.Width = 250;")
    res.append("            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);")
    res.append("            this.cmbFilterStatus.Dock = System.Windows.Forms.DockStyle.Right;")
    res.append("            this.cmbFilterStatus.Width = 120;")
    res.append("            this.cmbFilterClass.Dock = System.Windows.Forms.DockStyle.Right;")
    res.append("            this.cmbFilterClass.Width = 120;")
    res.append("            this.lblCount.Dock = System.Windows.Forms.DockStyle.Left;")
    res.append("            this.lblCount.AutoSize = true;")
    
    res.append("            this.pnlSearch.Controls.Add(this.cmbFilterClass);")
    res.append("            this.pnlSearch.Controls.Add(this.cmbFilterStatus);")
    res.append("            this.pnlSearch.Controls.Add(this.txtSearch);")
    res.append("            this.pnlSearch.Controls.Add(this.btnSearch);")
    res.append("            this.pnlSearch.Controls.Add(this.btnReload);")
    res.append("            this.pnlSearch.Controls.Add(this.lblCount);")
    
    # reorder
    res.append("            this.lblCount.BringToFront();")
    res.append("            this.btnReload.BringToFront();")
    res.append("            this.btnSearch.BringToFront();")
    res.append("            this.txtSearch.BringToFront();")
    res.append("            this.cmbFilterStatus.BringToFront();")
    res.append("            this.cmbFilterClass.BringToFront();")

    res.append("            this.Controls.Add(this.splitContainerMain);")
    res.append("            this.Controls.Add(this.pnlSearch);")
    res.append("            this.Controls.Add(this.pnlButtons);")
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
        "        private System.Windows.Forms.SplitContainer splitContainerMain;",
        "        private System.Windows.Forms.Panel pnlSearch;",
        "        private System.Windows.Forms.TextBox txtSearch;",
        "        private System.Windows.Forms.Button btnSearch;",
        "        private System.Windows.Forms.Button btnReload;",
        "        private System.Windows.Forms.ComboBox cmbFilterClass;",
        "        private System.Windows.Forms.ComboBox cmbFilterStatus;",
        "        private System.Windows.Forms.Label lblCount;",
        "        private System.Windows.Forms.Panel pnlButtons;",
        "        private System.Windows.Forms.Button btnAdd;",
        "        private System.Windows.Forms.Button btnSave;",
        "        private System.Windows.Forms.Button btnUpdate;",
        "        private System.Windows.Forms.Button btnDelete;",
        "        private System.Windows.Forms.Button btnCancel;",
        "        private System.Windows.Forms.Button btnRefresh;",
        "        private System.Windows.Forms.Button btnExportExcel;",
        "        private System.Windows.Forms.Button btnPrint;",
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
