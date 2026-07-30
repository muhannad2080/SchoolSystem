# -*- coding: utf-8 -*-

controls = [
    # General Enrollment Info Group
    ("EnrollmentID", "رقم الطلب", "TextBox", "gbBasic", False),
    ("StudentID", "اختيار الطالب", "ComboBox", "gbBasic", True),
    ("StudentName", "اسم الطالب", "TextBox", "gbBasic", False),
    ("ApplicationDate", "تاريخ التسجيل", "DateTimePicker", "gbBasic", False),
    ("ApplicationType", "نوع الطلب", "ComboBox", "gbBasic", True),
    ("AcademicYear", "العام الدراسي", "TextBox", "gbBasic", False),
    ("ClassID", "الصف", "ComboBox", "gbBasic", True),
    ("Section", "الشعبة", "TextBox", "gbBasic", False),
    ("SeatNumber", "رقم الجلوس", "TextBox", "gbBasic", False),
    ("Status", "حالة الطلب", "ComboBox", "gbBasic", True),
    
    # Previous School
    ("PreviousSchool", "اسم المدرسة", "TextBox", "gbPrevious", False),
    ("PreviousClass", "الصف السابق", "TextBox", "gbPrevious", False),
    ("TransferReason", "سبب النقل", "TextBox", "gbPrevious", False),
    
    # Fees Group
    ("RegistrationFee", "رسوم التسجيل", "TextBox", "gbFees", False),
    ("PaidAmount", "المبلغ المدفوع", "TextBox", "gbFees", False),
    ("RemainingAmount", "المبلغ المتبقي", "TextBox", "gbFees", False),
    ("PaymentMethod", "طريقة الدفع", "ComboBox", "gbFees", True),
    ("ReceiptNo", "رقم السند", "TextBox", "gbFees", False)
]

attachments = [
    ("HasBirthCertificate", "شهادة الميلاد"),
    ("HasGuardianId", "هوية ولي الأمر"),
    ("HasPhoto", "صورة شخصية"),
    ("HasLastCertificate", "آخر شهادة"),
    ("HasMedicalReport", "تقرير طبي")
]

def generate_designer_code():
    res = []
    res.append("namespace SchoolSystem.UI {")
    res.append("    partial class EnrollmentForm {")
    res.append("        private System.ComponentModel.IContainer components = null;")
    res.append("        protected override void Dispose(bool disposing) {")
    res.append("            if (disposing && (components != null)) components.Dispose();")
    res.append("            base.Dispose(disposing);")
    res.append("        }")
    res.append("        private void InitializeComponent() {")
    res.append("            this.components = new System.ComponentModel.Container();")
    
    # TextBoxes/ComboBoxes etc
    for name, lbl, typ, grp, isCombo in controls:
        res.append(f"            this.lbl{name} = new System.Windows.Forms.Label();")
        res.append(f"            this.{'cmb' if isCombo else ('dtp' if typ=='DateTimePicker' else 'txt')}{name} = new System.Windows.Forms.{typ}();")
    
    # Checkboxes
    for name, _ in attachments:
        res.append(f"            this.chk{name} = new System.Windows.Forms.CheckBox();")

    # Notes RichTextBox
    res.append("            this.gbNotes = new System.Windows.Forms.GroupBox();")
    res.append("            this.rtbNotes = new System.Windows.Forms.RichTextBox();")

    # Attachments FLP
    res.append("            this.gbAttachments = new System.Windows.Forms.GroupBox();")
    res.append("            this.flpAttachments = new System.Windows.Forms.FlowLayoutPanel();")

    # Groups & Layouts
    res.append("            this.gbBasic = new System.Windows.Forms.GroupBox();")
    res.append("            this.tlpBasic = new System.Windows.Forms.TableLayoutPanel();")
    res.append("            this.gbPrevious = new System.Windows.Forms.GroupBox();")
    res.append("            this.tlpPrevious = new System.Windows.Forms.TableLayoutPanel();")
    res.append("            this.gbFees = new System.Windows.Forms.GroupBox();")
    res.append("            this.tlpFees = new System.Windows.Forms.TableLayoutPanel();")

    res.append("            this.dgvEnrollments = new System.Windows.Forms.DataGridView();")
    res.append("            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);")
    res.append("            this.splitContainerMain = new System.Windows.Forms.SplitContainer();")
    
    res.append("            this.pnlSearch = new System.Windows.Forms.Panel();")
    res.append("            this.txtSearch = new System.Windows.Forms.TextBox();")
    res.append("            this.btnSearch = new System.Windows.Forms.Button();")
    res.append("            this.btnReload = new System.Windows.Forms.Button();")
    res.append("            this.lblCount = new System.Windows.Forms.Label();")
    
    res.append("            this.pnlButtons = new System.Windows.Forms.Panel();")
    res.append("            this.btnAdd = new System.Windows.Forms.Button();")
    res.append("            this.btnSave = new System.Windows.Forms.Button();")
    res.append("            this.btnUpdate = new System.Windows.Forms.Button();")
    res.append("            this.btnDelete = new System.Windows.Forms.Button();")
    res.append("            this.btnCancel = new System.Windows.Forms.Button();")
    res.append("            this.btnRefresh = new System.Windows.Forms.Button();")
    res.append("            this.btnPrintForm = new System.Windows.Forms.Button();")
    res.append("            this.btnPrintReceipt = new System.Windows.Forms.Button();")
    res.append("            this.btnClose = new System.Windows.Forms.Button();")
    res.append("            this.pnlRight = new System.Windows.Forms.Panel();")

    res.append("            ((System.ComponentModel.ISupportInitialize)(this.dgvEnrollments)).BeginInit();")
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
        r.append(f"            this.{name}.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);")
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

    for idx, (g_id, g_name, g_lbl) in enumerate([("gbBasic", "tlpBasic", "بيانات التسجيل الأساسية"), ("gbPrevious", "tlpPrevious", "المدرسة السابقة"), ("gbFees", "tlpFees", "الرسوم")]):
        c_list = [c for c in controls if c[3] == g_id]
        res.append(f"            this.{g_id}.Text = \"{g_lbl}\";")
        res.append(f"            this.{g_id}.Dock = System.Windows.Forms.DockStyle.Top;")
        res.append(f"            this.{g_id}.AutoSize = true;")
        res.append(f"            this.{g_id}.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;")
        res.append(f"            this.{g_id}.Padding = new System.Windows.Forms.Padding(10, 20, 10, 10);")
        res.append(f"            this.{g_id}.Controls.Add(this.{g_name});")
        res.extend(create_tlp(g_name, c_list))

    # Disable specific fields
    res.append("            this.txtEnrollmentID.ReadOnly = true;")
    res.append("            this.txtStudentName.ReadOnly = true;")
    res.append("            this.txtRemainingAmount.ReadOnly = true;")

    res.append("            this.gbAttachments.Text = \"المرفقات\";")
    res.append("            this.gbAttachments.Dock = System.Windows.Forms.DockStyle.Top;")
    res.append("            this.gbAttachments.AutoSize = true;")
    res.append("            this.gbAttachments.Padding = new System.Windows.Forms.Padding(10, 20, 10, 10);")
    res.append("            this.gbAttachments.Controls.Add(this.flpAttachments);")

    res.append("            this.flpAttachments.Dock = System.Windows.Forms.DockStyle.Top;")
    res.append("            this.flpAttachments.AutoSize = true;")

    for name, lbl in attachments:
        res.append(f"            this.chk{name}.Text = \"{lbl}\";")
        res.append(f"            this.chk{name}.AutoSize = true;")
        res.append(f"            this.chk{name}.Padding = new System.Windows.Forms.Padding(10);")
        res.append(f"            this.flpAttachments.Controls.Add(this.chk{name});")
        
    res.append("            this.gbNotes.Text = \"الملاحظات\";")
    res.append("            this.gbNotes.Dock = System.Windows.Forms.DockStyle.Top;")
    res.append("            this.gbNotes.Height = 100;")
    res.append("            this.gbNotes.Padding = new System.Windows.Forms.Padding(10, 20, 10, 10);")
    res.append("            this.gbNotes.Controls.Add(this.rtbNotes);")
    res.append("            this.rtbNotes.Dock = System.Windows.Forms.DockStyle.Fill;")

    res.append("            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;")
    res.append("            this.pnlRight.AutoScroll = true;")
    res.append("            this.pnlRight.Padding = new System.Windows.Forms.Padding(10);")
    
    res.append("            this.pnlRight.Controls.Add(this.gbNotes);")
    res.append("            this.pnlRight.Controls.Add(this.gbAttachments);")
    res.append("            this.pnlRight.Controls.Add(this.gbFees);")
    res.append("            this.pnlRight.Controls.Add(this.gbPrevious);")
    res.append("            this.pnlRight.Controls.Add(this.gbBasic);")
    
    res.append("            this.gbBasic.BringToFront();")
    res.append("            this.gbPrevious.BringToFront();")
    res.append("            this.gbFees.BringToFront();")
    res.append("            this.gbAttachments.BringToFront();")
    res.append("            this.gbNotes.BringToFront();")
    
    res.append("            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;")
    res.append("            this.splitContainerMain.Panel1.Controls.Add(this.dgvEnrollments);")
    res.append("            this.splitContainerMain.Panel1.Padding = new System.Windows.Forms.Padding(10, 10, 0, 10);")
    res.append("            this.splitContainerMain.Panel2.Controls.Add(this.pnlRight);")
    res.append("            this.splitContainerMain.RightToLeft = System.Windows.Forms.RightToLeft.Yes;")

    res.append("            this.dgvEnrollments.Dock = System.Windows.Forms.DockStyle.Fill;")
    res.append("            this.dgvEnrollments.AllowUserToAddRows = false;")
    res.append("            this.dgvEnrollments.AllowUserToDeleteRows = false;")
    res.append("            this.dgvEnrollments.ReadOnly = true;")
    res.append("            this.dgvEnrollments.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;")
    res.append("            this.dgvEnrollments.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvEnrollments_CellClick);")
    res.append("            this.dgvEnrollments.SelectionChanged += new System.EventHandler(this.dgvEnrollments_SelectionChanged);")

    res.append("            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;")
    res.append("            this.pnlButtons.Height = 70;")
    res.append("            this.pnlButtons.Padding = new System.Windows.Forms.Padding(10);")
    
    b_list = ["btnClose", "btnPrintReceipt", "btnPrintForm", "btnRefresh", "btnCancel", "btnDelete", "btnUpdate", "btnSave", "btnAdd"]
    b_txts = ["إغلاق", "طباعة إيصال", "طباعة استمارة", "تحديث", "إلغاء", "حذف", "تعديل", "حفظ", "جديد"]
    for i, (b, t) in enumerate(zip(b_list, b_txts)):
        res.append(f"            this.{b}.Text = \"{t}\";")
        res.append(f"            this.{b}.Dock = System.Windows.Forms.DockStyle.Right;")
        res.append(f"            this.{b}.Width = 90;") 
        res.append(f"            this.{b}.Margin = new System.Windows.Forms.Padding(5);")
        res.append(f"            this.pnlButtons.Controls.Add(this.{b});")
        res.append(f"            this.{b}.BringToFront();")
        if b=="btnCancel": 
            res.append(f"            this.{b}.Click += new System.EventHandler(this.btnCancel_Click);")
        elif b=="btnRefresh":
            res.append(f"            this.{b}.Click += new System.EventHandler(this.btnRefresh_Click);")
        else:
            res.append(f"            this.{b}.Click += new System.EventHandler(this.{b}_Click);")

    res.append("            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;")
    res.append("            this.pnlSearch.Height = 60;")
    res.append("            this.pnlSearch.Padding = new System.Windows.Forms.Padding(10);")
    res.append("            this.btnReload.Text = \"مسح الفلتر\";")
    res.append("            this.btnReload.Dock = System.Windows.Forms.DockStyle.Right;")
    res.append("            this.btnReload.Width = 80;")
    res.append("            this.btnReload.Click += new System.EventHandler(this.btnRefresh_Click);")
    res.append("            this.btnSearch.Text = \"بحث\";")
    res.append("            this.btnSearch.Dock = System.Windows.Forms.DockStyle.Right;")
    res.append("            this.btnSearch.Width = 80;")
    res.append("            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);")
    res.append("            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Right;")
    res.append("            this.txtSearch.Width = 300;")
    res.append("            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);")
    res.append("            this.lblCount.Dock = System.Windows.Forms.DockStyle.Left;")
    res.append("            this.lblCount.AutoSize = true;")
    
    res.append("            this.pnlSearch.Controls.Add(this.txtSearch);")
    res.append("            this.pnlSearch.Controls.Add(this.btnSearch);")
    res.append("            this.pnlSearch.Controls.Add(this.btnReload);")
    res.append("            this.pnlSearch.Controls.Add(this.lblCount);")
    
    res.append("            this.lblCount.BringToFront();")
    res.append("            this.btnReload.BringToFront();")
    res.append("            this.btnSearch.BringToFront();")
    res.append("            this.txtSearch.BringToFront();")

    res.append("            this.Controls.Add(this.splitContainerMain);")
    res.append("            this.Controls.Add(this.pnlSearch);")
    res.append("            this.Controls.Add(this.pnlButtons);")
    res.append("            this.ClientSize = new System.Drawing.Size(1200, 750);")
    res.append("            this.Name = \"EnrollmentForm\";")
    res.append("            this.Text = \"إدارة التسجيل والقبول\";")
    res.append("            this.Load += new System.EventHandler(this.EnrollmentForm_Load);")
    res.append("            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;")
    res.append("            this.RightToLeftLayout = true;")

    res.append("            this.cmbStudentID.SelectedIndexChanged += new System.EventHandler(this.cmbStudentID_SelectedIndexChanged);")
    res.append("            this.txtPaidAmount.TextChanged += new System.EventHandler(this.txtFees_TextChanged);")
    res.append("            this.txtRegistrationFee.TextChanged += new System.EventHandler(this.txtFees_TextChanged);")

    res.append("            ((System.ComponentModel.ISupportInitialize)(this.dgvEnrollments)).EndInit();")
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
    
    for name, _ in attachments:
        res.append(f"        private System.Windows.Forms.CheckBox chk{name};")

    res.extend([
        "        private System.Windows.Forms.GroupBox gbBasic;",
        "        private System.Windows.Forms.TableLayoutPanel tlpBasic;",
        "        private System.Windows.Forms.GroupBox gbPrevious;",
        "        private System.Windows.Forms.TableLayoutPanel tlpPrevious;",
        "        private System.Windows.Forms.GroupBox gbFees;",
        "        private System.Windows.Forms.TableLayoutPanel tlpFees;",
        "        private System.Windows.Forms.GroupBox gbAttachments;",
        "        private System.Windows.Forms.FlowLayoutPanel flpAttachments;",
        "        private System.Windows.Forms.GroupBox gbNotes;",
        "        private System.Windows.Forms.RichTextBox rtbNotes;",

        "        private System.Windows.Forms.DataGridView dgvEnrollments;",
        "        private System.Windows.Forms.ErrorProvider errorProvider1;",
        "        private System.Windows.Forms.SplitContainer splitContainerMain;",
        "        private System.Windows.Forms.Panel pnlSearch;",
        "        private System.Windows.Forms.TextBox txtSearch;",
        "        private System.Windows.Forms.Button btnSearch;",
        "        private System.Windows.Forms.Button btnReload;",
        "        private System.Windows.Forms.Label lblCount;",
        "        private System.Windows.Forms.Panel pnlButtons;",
        "        private System.Windows.Forms.Button btnAdd;",
        "        private System.Windows.Forms.Button btnSave;",
        "        private System.Windows.Forms.Button btnUpdate;",
        "        private System.Windows.Forms.Button btnDelete;",
        "        private System.Windows.Forms.Button btnCancel;",
        "        private System.Windows.Forms.Button btnRefresh;",
        "        private System.Windows.Forms.Button btnPrintForm;",
        "        private System.Windows.Forms.Button btnPrintReceipt;",
        "        private System.Windows.Forms.Button btnClose;",
        "        private System.Windows.Forms.Panel pnlRight;",
        "    }",
        "}"
    ])
    
    with open(r'd:\_Getintopc.com_VS2022_2_2\SchoolSystem\UI\EnrollmentForm.Designer.cs', 'w', encoding='utf-8') as f:
        f.write('\n'.join(res))

if __name__ == '__main__':
    generate_designer_code()
