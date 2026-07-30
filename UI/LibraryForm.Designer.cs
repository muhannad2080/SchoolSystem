namespace SchoolSystem.UI
{
    partial class LibraryForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelTitle = new System.Windows.Forms.Panel();
            this.lblFormTitle = new System.Windows.Forms.Label();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabBooks = new System.Windows.Forms.TabPage();
            this.dataGridViewBooks = new System.Windows.Forms.DataGridView();
            this.panelBooksButtons = new System.Windows.Forms.Panel();
            this.btnAddBook = new System.Windows.Forms.Button();
            this.btnUpdateBook = new System.Windows.Forms.Button();
            this.btnDeleteBook = new System.Windows.Forms.Button();
            this.btnClearBook = new System.Windows.Forms.Button();
            this.panelBooksFields = new System.Windows.Forms.Panel();
            this.tableBooksFields = new System.Windows.Forms.TableLayoutPanel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.lblAuthor = new System.Windows.Forms.Label();
            this.txtAuthor = new System.Windows.Forms.TextBox();
            this.lblISBN = new System.Windows.Forms.Label();
            this.txtISBN = new System.Windows.Forms.TextBox();
            this.lblCategory = new System.Windows.Forms.Label();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.lblPublisher = new System.Windows.Forms.Label();
            this.txtPublisher = new System.Windows.Forms.TextBox();
            this.lblPublicationYear = new System.Windows.Forms.Label();
            this.txtPublicationYear = new System.Windows.Forms.TextBox();
            this.lblCopies = new System.Windows.Forms.Label();
            this.txtCopies = new System.Windows.Forms.TextBox();
            this.lblShelf = new System.Windows.Forms.Label();
            this.txtShelf = new System.Windows.Forms.TextBox();
            this.lblBookNotes = new System.Windows.Forms.Label();
            this.txtBookNotes = new System.Windows.Forms.TextBox();
            this.tabBorrowing = new System.Windows.Forms.TabPage();
            this.dataGridViewBorrowings = new System.Windows.Forms.DataGridView();
            this.panelBorrowingButtons = new System.Windows.Forms.Panel();
            this.btnBorrow = new System.Windows.Forms.Button();
            this.btnReturn = new System.Windows.Forms.Button();
            this.panelBorrowingFields = new System.Windows.Forms.Panel();
            this.tableBorrowingFields = new System.Windows.Forms.TableLayoutPanel();
            this.lblBook = new System.Windows.Forms.Label();
            this.cmbBook = new System.Windows.Forms.ComboBox();
            this.lblBorrowerType = new System.Windows.Forms.Label();
            this.cmbBorrowerType = new System.Windows.Forms.ComboBox();
            this.lblBorrower = new System.Windows.Forms.Label();
            this.cmbBorrower = new System.Windows.Forms.ComboBox();
            this.lblBorrowDate = new System.Windows.Forms.Label();
            this.dtpBorrowDate = new System.Windows.Forms.DateTimePicker();
            this.lblDueDate = new System.Windows.Forms.Label();
            this.dtpDueDate = new System.Windows.Forms.DateTimePicker();
            this.lblBorrowNotes = new System.Windows.Forms.Label();
            this.txtBorrowNotes = new System.Windows.Forms.TextBox();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.lblRecordCount = new System.Windows.Forms.Label();
            this.panelTitle.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabBooks.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBooks)).BeginInit();
            this.panelBooksButtons.SuspendLayout();
            this.panelBooksFields.SuspendLayout();
            this.tableBooksFields.SuspendLayout();
            this.tabBorrowing.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBorrowings)).BeginInit();
            this.panelBorrowingButtons.SuspendLayout();
            this.panelBorrowingFields.SuspendLayout();
            this.tableBorrowingFields.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTitle
            // 
            this.panelTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(42)))), ((int)(((byte)(57)))));
            this.panelTitle.Controls.Add(this.lblFormTitle);
            this.panelTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTitle.Location = new System.Drawing.Point(0, 0);
            this.panelTitle.Name = "panelTitle";
            this.panelTitle.Size = new System.Drawing.Size(950, 55);
            this.panelTitle.TabIndex = 0;
            // 
            // lblFormTitle
            // 
            this.lblFormTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFormTitle.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.lblFormTitle.ForeColor = System.Drawing.Color.White;
            this.lblFormTitle.Location = new System.Drawing.Point(0, 0);
            this.lblFormTitle.Name = "lblFormTitle";
            this.lblFormTitle.Size = new System.Drawing.Size(950, 55);
            this.lblFormTitle.TabIndex = 0;
            this.lblFormTitle.Text = "📖  المكتبة المدرسية";
            this.lblFormTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabBooks);
            this.tabControl.Controls.Add(this.tabBorrowing);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Font = new System.Drawing.Font("Tahoma", 10F);
            this.tabControl.Location = new System.Drawing.Point(0, 55);
            this.tabControl.Name = "tabControl";
            this.tabControl.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(950, 565);
            this.tabControl.TabIndex = 1;
            // 
            // tabBooks
            // 
            this.tabBooks.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.tabBooks.Controls.Add(this.dataGridViewBooks);
            this.tabBooks.Controls.Add(this.panelBooksButtons);
            this.tabBooks.Controls.Add(this.panelBooksFields);
            this.tabBooks.Location = new System.Drawing.Point(4, 30);
            this.tabBooks.Name = "tabBooks";
            this.tabBooks.Size = new System.Drawing.Size(942, 531);
            this.tabBooks.TabIndex = 0;
            this.tabBooks.Text = "📚  فهرس الكتب";
            // 
            // dataGridViewBooks
            // 
            this.dataGridViewBooks.AllowUserToAddRows = false;
            this.dataGridViewBooks.AllowUserToDeleteRows = false;
            this.dataGridViewBooks.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewBooks.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(42)))), ((int)(((byte)(57)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            this.dataGridViewBooks.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewBooks.ColumnHeadersHeight = 38;
            this.dataGridViewBooks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewBooks.EnableHeadersVisualStyles = false;
            this.dataGridViewBooks.Location = new System.Drawing.Point(0, 290);
            this.dataGridViewBooks.MultiSelect = false;
            this.dataGridViewBooks.Name = "dataGridViewBooks";
            this.dataGridViewBooks.ReadOnly = true;
            this.dataGridViewBooks.RowHeadersVisible = false;
            this.dataGridViewBooks.RowHeadersWidth = 51;
            this.dataGridViewBooks.RowTemplate.Height = 33;
            this.dataGridViewBooks.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewBooks.Size = new System.Drawing.Size(942, 241);
            this.dataGridViewBooks.TabIndex = 2;
            this.dataGridViewBooks.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewBooks_CellClick);
            // 
            // panelBooksButtons
            // 
            this.panelBooksButtons.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.panelBooksButtons.Controls.Add(this.btnAddBook);
            this.panelBooksButtons.Controls.Add(this.btnUpdateBook);
            this.panelBooksButtons.Controls.Add(this.btnDeleteBook);
            this.panelBooksButtons.Controls.Add(this.btnClearBook);
            this.panelBooksButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelBooksButtons.Location = new System.Drawing.Point(0, 235);
            this.panelBooksButtons.Name = "panelBooksButtons";
            this.panelBooksButtons.Size = new System.Drawing.Size(942, 55);
            this.panelBooksButtons.TabIndex = 1;
            // 
            // btnAddBook
            // 
            this.btnAddBook.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.btnAddBook.FlatAppearance.BorderSize = 0;
            this.btnAddBook.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddBook.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnAddBook.ForeColor = System.Drawing.Color.White;
            this.btnAddBook.Location = new System.Drawing.Point(835, 10);
            this.btnAddBook.Name = "btnAddBook";
            this.btnAddBook.Size = new System.Drawing.Size(95, 35);
            this.btnAddBook.TabIndex = 0;
            this.btnAddBook.Text = "➕  إضافة";
            this.btnAddBook.UseVisualStyleBackColor = false;
            this.btnAddBook.Click += new System.EventHandler(this.btnAddBook_Click);
            // 
            // btnUpdateBook
            // 
            this.btnUpdateBook.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(126)))), ((int)(((byte)(34)))));
            this.btnUpdateBook.FlatAppearance.BorderSize = 0;
            this.btnUpdateBook.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdateBook.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnUpdateBook.ForeColor = System.Drawing.Color.White;
            this.btnUpdateBook.Location = new System.Drawing.Point(734, 10);
            this.btnUpdateBook.Name = "btnUpdateBook";
            this.btnUpdateBook.Size = new System.Drawing.Size(95, 35);
            this.btnUpdateBook.TabIndex = 1;
            this.btnUpdateBook.Text = "✏️  تعديل";
            this.btnUpdateBook.UseVisualStyleBackColor = false;
            this.btnUpdateBook.Click += new System.EventHandler(this.btnUpdateBook_Click);
            // 
            // btnDeleteBook
            // 
            this.btnDeleteBook.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(57)))), ((int)(((byte)(43)))));
            this.btnDeleteBook.FlatAppearance.BorderSize = 0;
            this.btnDeleteBook.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteBook.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnDeleteBook.ForeColor = System.Drawing.Color.White;
            this.btnDeleteBook.Location = new System.Drawing.Point(633, 10);
            this.btnDeleteBook.Name = "btnDeleteBook";
            this.btnDeleteBook.Size = new System.Drawing.Size(95, 35);
            this.btnDeleteBook.TabIndex = 2;
            this.btnDeleteBook.Text = "🗑️  حذف";
            this.btnDeleteBook.UseVisualStyleBackColor = false;
            this.btnDeleteBook.Click += new System.EventHandler(this.btnDeleteBook_Click);
            // 
            // btnClearBook
            // 
            this.btnClearBook.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(140)))), ((int)(((byte)(141)))));
            this.btnClearBook.FlatAppearance.BorderSize = 0;
            this.btnClearBook.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearBook.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnClearBook.ForeColor = System.Drawing.Color.White;
            this.btnClearBook.Location = new System.Drawing.Point(532, 10);
            this.btnClearBook.Name = "btnClearBook";
            this.btnClearBook.Size = new System.Drawing.Size(95, 35);
            this.btnClearBook.TabIndex = 3;
            this.btnClearBook.Text = "🔄  تفريغ";
            this.btnClearBook.UseVisualStyleBackColor = false;
            this.btnClearBook.Click += new System.EventHandler(this.btnClearBook_Click);
            // 
            // panelBooksFields
            // 
            this.panelBooksFields.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.panelBooksFields.Controls.Add(this.tableBooksFields);
            this.panelBooksFields.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelBooksFields.Location = new System.Drawing.Point(0, 0);
            this.panelBooksFields.Name = "panelBooksFields";
            this.panelBooksFields.Padding = new System.Windows.Forms.Padding(15);
            this.panelBooksFields.Size = new System.Drawing.Size(942, 235);
            this.panelBooksFields.TabIndex = 0;
            // 
            // tableBooksFields
            // 
            this.tableBooksFields.ColumnCount = 4;
            this.tableBooksFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableBooksFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableBooksFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableBooksFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableBooksFields.Controls.Add(this.lblTitle, 0, 0);
            this.tableBooksFields.Controls.Add(this.txtTitle, 1, 0);
            this.tableBooksFields.Controls.Add(this.lblAuthor, 2, 0);
            this.tableBooksFields.Controls.Add(this.txtAuthor, 3, 0);
            this.tableBooksFields.Controls.Add(this.lblISBN, 0, 1);
            this.tableBooksFields.Controls.Add(this.txtISBN, 1, 1);
            this.tableBooksFields.Controls.Add(this.lblCategory, 2, 1);
            this.tableBooksFields.Controls.Add(this.cmbCategory, 3, 1);
            this.tableBooksFields.Controls.Add(this.lblPublisher, 0, 2);
            this.tableBooksFields.Controls.Add(this.txtPublisher, 1, 2);
            this.tableBooksFields.Controls.Add(this.lblPublicationYear, 2, 2);
            this.tableBooksFields.Controls.Add(this.txtPublicationYear, 3, 2);
            this.tableBooksFields.Controls.Add(this.lblCopies, 0, 3);
            this.tableBooksFields.Controls.Add(this.txtCopies, 1, 3);
            this.tableBooksFields.Controls.Add(this.lblShelf, 2, 3);
            this.tableBooksFields.Controls.Add(this.txtShelf, 3, 3);
            this.tableBooksFields.Controls.Add(this.lblBookNotes, 0, 4);
            this.tableBooksFields.Controls.Add(this.txtBookNotes, 1, 4);
            this.tableBooksFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableBooksFields.Location = new System.Drawing.Point(15, 15);
            this.tableBooksFields.Name = "tableBooksFields";
            this.tableBooksFields.RowCount = 5;
            this.tableBooksFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableBooksFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableBooksFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableBooksFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableBooksFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableBooksFields.Size = new System.Drawing.Size(912, 205);
            this.tableBooksFields.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblTitle.Location = new System.Drawing.Point(815, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(62, 21);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "العنوان:";
            // 
            // txtTitle
            // 
            this.txtTitle.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtTitle.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtTitle.Location = new System.Drawing.Point(530, 6);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(279, 28);
            this.txtTitle.TabIndex = 0;
            // 
            // lblAuthor
            // 
            this.lblAuthor.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblAuthor.AutoSize = true;
            this.lblAuthor.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblAuthor.Location = new System.Drawing.Point(359, 9);
            this.lblAuthor.Name = "lblAuthor";
            this.lblAuthor.Size = new System.Drawing.Size(63, 21);
            this.lblAuthor.TabIndex = 1;
            this.lblAuthor.Text = "المؤلف:";
            // 
            // txtAuthor
            // 
            this.txtAuthor.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtAuthor.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtAuthor.Location = new System.Drawing.Point(74, 6);
            this.txtAuthor.Name = "txtAuthor";
            this.txtAuthor.Size = new System.Drawing.Size(279, 28);
            this.txtAuthor.TabIndex = 1;
            // 
            // lblISBN
            // 
            this.lblISBN.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblISBN.AutoSize = true;
            this.lblISBN.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblISBN.Location = new System.Drawing.Point(815, 49);
            this.lblISBN.Name = "lblISBN";
            this.lblISBN.Size = new System.Drawing.Size(52, 21);
            this.lblISBN.TabIndex = 2;
            this.lblISBN.Text = "ISBN:";
            // 
            // txtISBN
            // 
            this.txtISBN.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtISBN.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtISBN.Location = new System.Drawing.Point(530, 46);
            this.txtISBN.Name = "txtISBN";
            this.txtISBN.Size = new System.Drawing.Size(279, 28);
            this.txtISBN.TabIndex = 2;
            // 
            // lblCategory
            // 
            this.lblCategory.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblCategory.AutoSize = true;
            this.lblCategory.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblCategory.Location = new System.Drawing.Point(359, 49);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(50, 21);
            this.lblCategory.TabIndex = 3;
            this.lblCategory.Text = "الفئة:";
            // 
            // cmbCategory
            // 
            this.cmbCategory.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategory.Font = new System.Drawing.Font("Tahoma", 10F);
            this.cmbCategory.Location = new System.Drawing.Point(74, 45);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(279, 29);
            this.cmbCategory.TabIndex = 3;
            // 
            // lblPublisher
            // 
            this.lblPublisher.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblPublisher.AutoSize = true;
            this.lblPublisher.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblPublisher.Location = new System.Drawing.Point(815, 89);
            this.lblPublisher.Name = "lblPublisher";
            this.lblPublisher.Size = new System.Drawing.Size(60, 21);
            this.lblPublisher.TabIndex = 4;
            this.lblPublisher.Text = "الناشر:";
            // 
            // txtPublisher
            // 
            this.txtPublisher.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtPublisher.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtPublisher.Location = new System.Drawing.Point(530, 86);
            this.txtPublisher.Name = "txtPublisher";
            this.txtPublisher.Size = new System.Drawing.Size(279, 28);
            this.txtPublisher.TabIndex = 4;
            // 
            // lblPublicationYear
            // 
            this.lblPublicationYear.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblPublicationYear.AutoSize = true;
            this.lblPublicationYear.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblPublicationYear.Location = new System.Drawing.Point(359, 89);
            this.lblPublicationYear.Name = "lblPublicationYear";
            this.lblPublicationYear.Size = new System.Drawing.Size(94, 21);
            this.lblPublicationYear.TabIndex = 5;
            this.lblPublicationYear.Text = "سنة النشر:";
            // 
            // txtPublicationYear
            // 
            this.txtPublicationYear.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtPublicationYear.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtPublicationYear.Location = new System.Drawing.Point(74, 86);
            this.txtPublicationYear.Name = "txtPublicationYear";
            this.txtPublicationYear.Size = new System.Drawing.Size(279, 28);
            this.txtPublicationYear.TabIndex = 5;
            // 
            // lblCopies
            // 
            this.lblCopies.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblCopies.AutoSize = true;
            this.lblCopies.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblCopies.Location = new System.Drawing.Point(815, 129);
            this.lblCopies.Name = "lblCopies";
            this.lblCopies.Size = new System.Drawing.Size(93, 21);
            this.lblCopies.TabIndex = 6;
            this.lblCopies.Text = "عدد النسخ:";
            // 
            // txtCopies
            // 
            this.txtCopies.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtCopies.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtCopies.Location = new System.Drawing.Point(530, 126);
            this.txtCopies.Name = "txtCopies";
            this.txtCopies.Size = new System.Drawing.Size(279, 28);
            this.txtCopies.TabIndex = 6;
            this.txtCopies.Text = "1";
            // 
            // lblShelf
            // 
            this.lblShelf.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblShelf.AutoSize = true;
            this.lblShelf.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblShelf.Location = new System.Drawing.Point(359, 129);
            this.lblShelf.Name = "lblShelf";
            this.lblShelf.Size = new System.Drawing.Size(47, 21);
            this.lblShelf.TabIndex = 7;
            this.lblShelf.Text = "الرف:";
            // 
            // txtShelf
            // 
            this.txtShelf.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtShelf.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtShelf.Location = new System.Drawing.Point(74, 126);
            this.txtShelf.Name = "txtShelf";
            this.txtShelf.Size = new System.Drawing.Size(279, 28);
            this.txtShelf.TabIndex = 7;
            // 
            // lblBookNotes
            // 
            this.lblBookNotes.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblBookNotes.AutoSize = true;
            this.lblBookNotes.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblBookNotes.Location = new System.Drawing.Point(815, 172);
            this.lblBookNotes.Name = "lblBookNotes";
            this.lblBookNotes.Size = new System.Drawing.Size(79, 21);
            this.lblBookNotes.TabIndex = 8;
            this.lblBookNotes.Text = "ملاحظات:";
            // 
            // txtBookNotes
            // 
            this.txtBookNotes.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtBookNotes.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtBookNotes.Location = new System.Drawing.Point(530, 168);
            this.txtBookNotes.Name = "txtBookNotes";
            this.txtBookNotes.Size = new System.Drawing.Size(279, 28);
            this.txtBookNotes.TabIndex = 8;
            // 
            // tabBorrowing
            // 
            this.tabBorrowing.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.tabBorrowing.Controls.Add(this.dataGridViewBorrowings);
            this.tabBorrowing.Controls.Add(this.panelBorrowingButtons);
            this.tabBorrowing.Controls.Add(this.panelBorrowingFields);
            this.tabBorrowing.Location = new System.Drawing.Point(4, 30);
            this.tabBorrowing.Name = "tabBorrowing";
            this.tabBorrowing.Size = new System.Drawing.Size(942, 531);
            this.tabBorrowing.TabIndex = 1;
            this.tabBorrowing.Text = "📤  الإعارة";
            // 
            // dataGridViewBorrowings
            // 
            this.dataGridViewBorrowings.AllowUserToAddRows = false;
            this.dataGridViewBorrowings.AllowUserToDeleteRows = false;
            this.dataGridViewBorrowings.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewBorrowings.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(42)))), ((int)(((byte)(57)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 10F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            this.dataGridViewBorrowings.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewBorrowings.ColumnHeadersHeight = 38;
            this.dataGridViewBorrowings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewBorrowings.EnableHeadersVisualStyles = false;
            this.dataGridViewBorrowings.Location = new System.Drawing.Point(0, 310);
            this.dataGridViewBorrowings.MultiSelect = false;
            this.dataGridViewBorrowings.Name = "dataGridViewBorrowings";
            this.dataGridViewBorrowings.ReadOnly = true;
            this.dataGridViewBorrowings.RowHeadersVisible = false;
            this.dataGridViewBorrowings.RowHeadersWidth = 51;
            this.dataGridViewBorrowings.RowTemplate.Height = 33;
            this.dataGridViewBorrowings.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewBorrowings.Size = new System.Drawing.Size(942, 221);
            this.dataGridViewBorrowings.TabIndex = 2;
            this.dataGridViewBorrowings.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewBorrowings_CellClick);
            // 
            // panelBorrowingButtons
            // 
            this.panelBorrowingButtons.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.panelBorrowingButtons.Controls.Add(this.btnBorrow);
            this.panelBorrowingButtons.Controls.Add(this.btnReturn);
            this.panelBorrowingButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelBorrowingButtons.Location = new System.Drawing.Point(0, 255);
            this.panelBorrowingButtons.Name = "panelBorrowingButtons";
            this.panelBorrowingButtons.Size = new System.Drawing.Size(942, 55);
            this.panelBorrowingButtons.TabIndex = 1;
            // 
            // btnBorrow
            // 
            this.btnBorrow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.btnBorrow.FlatAppearance.BorderSize = 0;
            this.btnBorrow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBorrow.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnBorrow.ForeColor = System.Drawing.Color.White;
            this.btnBorrow.Location = new System.Drawing.Point(835, 10);
            this.btnBorrow.Name = "btnBorrow";
            this.btnBorrow.Size = new System.Drawing.Size(95, 35);
            this.btnBorrow.TabIndex = 0;
            this.btnBorrow.Text = "📤  إعارة";
            this.btnBorrow.UseVisualStyleBackColor = false;
            this.btnBorrow.Click += new System.EventHandler(this.btnBorrow_Click);
            // 
            // btnReturn
            // 
            this.btnReturn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnReturn.FlatAppearance.BorderSize = 0;
            this.btnReturn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReturn.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnReturn.ForeColor = System.Drawing.Color.White;
            this.btnReturn.Location = new System.Drawing.Point(665, 10);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Size = new System.Drawing.Size(164, 35);
            this.btnReturn.TabIndex = 1;
            this.btnReturn.Text = "📥  استرجاع";
            this.btnReturn.UseVisualStyleBackColor = false;
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // panelBorrowingFields
            // 
            this.panelBorrowingFields.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.panelBorrowingFields.Controls.Add(this.tableBorrowingFields);
            this.panelBorrowingFields.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelBorrowingFields.Location = new System.Drawing.Point(0, 0);
            this.panelBorrowingFields.Name = "panelBorrowingFields";
            this.panelBorrowingFields.Padding = new System.Windows.Forms.Padding(15);
            this.panelBorrowingFields.Size = new System.Drawing.Size(942, 255);
            this.panelBorrowingFields.TabIndex = 0;
            // 
            // tableBorrowingFields
            // 
            this.tableBorrowingFields.ColumnCount = 4;
            this.tableBorrowingFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableBorrowingFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableBorrowingFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableBorrowingFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableBorrowingFields.Controls.Add(this.lblBook, 0, 0);
            this.tableBorrowingFields.Controls.Add(this.cmbBook, 1, 0);
            this.tableBorrowingFields.Controls.Add(this.lblBorrowerType, 2, 0);
            this.tableBorrowingFields.Controls.Add(this.cmbBorrowerType, 3, 0);
            this.tableBorrowingFields.Controls.Add(this.lblBorrower, 0, 1);
            this.tableBorrowingFields.Controls.Add(this.cmbBorrower, 1, 1);
            this.tableBorrowingFields.Controls.Add(this.lblBorrowDate, 2, 1);
            this.tableBorrowingFields.Controls.Add(this.dtpBorrowDate, 3, 1);
            this.tableBorrowingFields.Controls.Add(this.lblDueDate, 0, 2);
            this.tableBorrowingFields.Controls.Add(this.dtpDueDate, 1, 2);
            this.tableBorrowingFields.Controls.Add(this.lblBorrowNotes, 2, 2);
            this.tableBorrowingFields.Controls.Add(this.txtBorrowNotes, 3, 2);
            this.tableBorrowingFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableBorrowingFields.Location = new System.Drawing.Point(15, 15);
            this.tableBorrowingFields.Name = "tableBorrowingFields";
            this.tableBorrowingFields.RowCount = 3;
            this.tableBorrowingFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableBorrowingFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableBorrowingFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableBorrowingFields.Size = new System.Drawing.Size(912, 225);
            this.tableBorrowingFields.TabIndex = 0;
            // 
            // lblBook
            // 
            this.lblBook.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblBook.AutoSize = true;
            this.lblBook.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblBook.Location = new System.Drawing.Point(815, 9);
            this.lblBook.Name = "lblBook";
            this.lblBook.Size = new System.Drawing.Size(59, 21);
            this.lblBook.TabIndex = 0;
            this.lblBook.Text = "الكتاب:";
            // 
            // cmbBook
            // 
            this.cmbBook.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbBook.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBook.Font = new System.Drawing.Font("Tahoma", 10F);
            this.cmbBook.Location = new System.Drawing.Point(530, 5);
            this.cmbBook.Name = "cmbBook";
            this.cmbBook.Size = new System.Drawing.Size(279, 29);
            this.cmbBook.TabIndex = 0;
            // 
            // lblBorrowerType
            // 
            this.lblBorrowerType.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblBorrowerType.AutoSize = true;
            this.lblBorrowerType.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblBorrowerType.Location = new System.Drawing.Point(359, 0);
            this.lblBorrowerType.Name = "lblBorrowerType";
            this.lblBorrowerType.Size = new System.Drawing.Size(91, 40);
            this.lblBorrowerType.TabIndex = 1;
            this.lblBorrowerType.Text = "نوع المستعير:";
            // 
            // cmbBorrowerType
            // 
            this.cmbBorrowerType.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbBorrowerType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBorrowerType.Font = new System.Drawing.Font("Tahoma", 10F);
            this.cmbBorrowerType.Location = new System.Drawing.Point(74, 5);
            this.cmbBorrowerType.Name = "cmbBorrowerType";
            this.cmbBorrowerType.Size = new System.Drawing.Size(279, 29);
            this.cmbBorrowerType.TabIndex = 1;
            // 
            // lblBorrower
            // 
            this.lblBorrower.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblBorrower.AutoSize = true;
            this.lblBorrower.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblBorrower.Location = new System.Drawing.Point(815, 49);
            this.lblBorrower.Name = "lblBorrower";
            this.lblBorrower.Size = new System.Drawing.Size(80, 21);
            this.lblBorrower.TabIndex = 2;
            this.lblBorrower.Text = "المستعير:";
            // 
            // cmbBorrower
            // 
            this.cmbBorrower.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbBorrower.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBorrower.Font = new System.Drawing.Font("Tahoma", 10F);
            this.cmbBorrower.Location = new System.Drawing.Point(530, 45);
            this.cmbBorrower.Name = "cmbBorrower";
            this.cmbBorrower.Size = new System.Drawing.Size(279, 29);
            this.cmbBorrower.TabIndex = 2;
            // 
            // lblBorrowDate
            // 
            this.lblBorrowDate.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblBorrowDate.AutoSize = true;
            this.lblBorrowDate.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblBorrowDate.Location = new System.Drawing.Point(359, 40);
            this.lblBorrowDate.Name = "lblBorrowDate";
            this.lblBorrowDate.Size = new System.Drawing.Size(83, 40);
            this.lblBorrowDate.TabIndex = 3;
            this.lblBorrowDate.Text = "تاريخ الإعارة:";
            // 
            // dtpBorrowDate
            // 
            this.dtpBorrowDate.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.dtpBorrowDate.CustomFormat = "dd/MM/yyyy";
            this.dtpBorrowDate.Font = new System.Drawing.Font("Tahoma", 10F);
            this.dtpBorrowDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpBorrowDate.Location = new System.Drawing.Point(74, 46);
            this.dtpBorrowDate.Name = "dtpBorrowDate";
            this.dtpBorrowDate.Size = new System.Drawing.Size(279, 28);
            this.dtpBorrowDate.TabIndex = 3;
            // 
            // lblDueDate
            // 
            this.lblDueDate.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblDueDate.AutoSize = true;
            this.lblDueDate.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblDueDate.Location = new System.Drawing.Point(815, 131);
            this.lblDueDate.Name = "lblDueDate";
            this.lblDueDate.Size = new System.Drawing.Size(84, 42);
            this.lblDueDate.TabIndex = 4;
            this.lblDueDate.Text = "تاريخ الإرجاع:";
            // 
            // dtpDueDate
            // 
            this.dtpDueDate.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.dtpDueDate.CustomFormat = "dd/MM/yyyy";
            this.dtpDueDate.Font = new System.Drawing.Font("Tahoma", 10F);
            this.dtpDueDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDueDate.Location = new System.Drawing.Point(530, 138);
            this.dtpDueDate.Name = "dtpDueDate";
            this.dtpDueDate.Size = new System.Drawing.Size(279, 28);
            this.dtpDueDate.TabIndex = 4;
            // 
            // lblBorrowNotes
            // 
            this.lblBorrowNotes.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblBorrowNotes.AutoSize = true;
            this.lblBorrowNotes.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblBorrowNotes.Location = new System.Drawing.Point(359, 142);
            this.lblBorrowNotes.Name = "lblBorrowNotes";
            this.lblBorrowNotes.Size = new System.Drawing.Size(79, 21);
            this.lblBorrowNotes.TabIndex = 5;
            this.lblBorrowNotes.Text = "ملاحظات:";
            // 
            // txtBorrowNotes
            // 
            this.txtBorrowNotes.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtBorrowNotes.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtBorrowNotes.Location = new System.Drawing.Point(74, 138);
            this.txtBorrowNotes.Name = "txtBorrowNotes";
            this.txtBorrowNotes.Size = new System.Drawing.Size(279, 28);
            this.txtBorrowNotes.TabIndex = 5;
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
            this.lblRecordCount.Text = "عدد الكتب: 0";
            this.lblRecordCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LibraryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelTitle);
            this.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.Name = "LibraryForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Size = new System.Drawing.Size(950, 650);
            this.panelTitle.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabBooks.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBooks)).EndInit();
            this.panelBooksButtons.ResumeLayout(false);
            this.panelBooksFields.ResumeLayout(false);
            this.tableBooksFields.ResumeLayout(false);
            this.tableBooksFields.PerformLayout();
            this.tabBorrowing.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBorrowings)).EndInit();
            this.panelBorrowingButtons.ResumeLayout(false);
            this.panelBorrowingFields.ResumeLayout(false);
            this.tableBorrowingFields.ResumeLayout(false);
            this.tableBorrowingFields.PerformLayout();
            this.panelBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTitle;
        private System.Windows.Forms.Label lblFormTitle;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabBooks;
        private System.Windows.Forms.TabPage tabBorrowing;
        private System.Windows.Forms.DataGridView dataGridViewBooks;
        private System.Windows.Forms.Panel panelBooksButtons;
        private System.Windows.Forms.Button btnAddBook;
        private System.Windows.Forms.Button btnUpdateBook;
        private System.Windows.Forms.Button btnDeleteBook;
        private System.Windows.Forms.Button btnClearBook;
        private System.Windows.Forms.Panel panelBooksFields;
        private System.Windows.Forms.TableLayoutPanel tableBooksFields;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label lblAuthor;
        private System.Windows.Forms.TextBox txtAuthor;
        private System.Windows.Forms.Label lblISBN;
        private System.Windows.Forms.TextBox txtISBN;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.Label lblPublisher;
        private System.Windows.Forms.TextBox txtPublisher;
        private System.Windows.Forms.Label lblPublicationYear;
        private System.Windows.Forms.TextBox txtPublicationYear;
        private System.Windows.Forms.Label lblCopies;
        private System.Windows.Forms.TextBox txtCopies;
        private System.Windows.Forms.Label lblShelf;
        private System.Windows.Forms.TextBox txtShelf;
        private System.Windows.Forms.Label lblBookNotes;
        private System.Windows.Forms.TextBox txtBookNotes;
        private System.Windows.Forms.DataGridView dataGridViewBorrowings;
        private System.Windows.Forms.Panel panelBorrowingButtons;
        private System.Windows.Forms.Button btnBorrow;
        private System.Windows.Forms.Button btnReturn;
        private System.Windows.Forms.Panel panelBorrowingFields;
        private System.Windows.Forms.TableLayoutPanel tableBorrowingFields;
        private System.Windows.Forms.Label lblBook;
        private System.Windows.Forms.ComboBox cmbBook;
        private System.Windows.Forms.Label lblBorrowerType;
        private System.Windows.Forms.ComboBox cmbBorrowerType;
        private System.Windows.Forms.Label lblBorrower;
        private System.Windows.Forms.ComboBox cmbBorrower;
        private System.Windows.Forms.Label lblBorrowDate;
        private System.Windows.Forms.DateTimePicker dtpBorrowDate;
        private System.Windows.Forms.Label lblDueDate;
        private System.Windows.Forms.DateTimePicker dtpDueDate;
        private System.Windows.Forms.Label lblBorrowNotes;
        private System.Windows.Forms.TextBox txtBorrowNotes;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Label lblRecordCount;
    }
}