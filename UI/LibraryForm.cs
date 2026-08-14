using System;
using System.Data;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;
using SchoolSystem.Models;
using SchoolSystem.Helpers;
using SchoolSystem.Services;

namespace SchoolSystem.UI
{
    public partial class LibraryForm : UserControl
    {
        private readonly BookService bookService = new BookService();
        private readonly BorrowingService borrowingService = new BorrowingService();
        private readonly StudentService studentService = new StudentService();
        private readonly TeacherService teacherService = new TeacherService();

        private int selectedBookId = 0;
        private int selectedBorrowingId = 0;

        private DataTable allBooks;
        private DataTable allBorrowings;

        private bool isLoading = false;

        public LibraryForm()
        {
            InitializeComponent();
            SchoolSystem.Helpers.UIHelper.ApplyStyle(this);
            ApplyCustomStyles();
            Dock = DockStyle.Fill;

            Load += LibraryForm_Load;
            tabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;
            cmbBorrowerType.SelectedIndexChanged += async (s, e) => await LoadBorrowersAsync();
        }

        private void ApplyCustomStyles()
        {
            UIHelper.StyleDataGridView(dataGridViewBooks);
            UIHelper.StyleDataGridView(dataGridViewBorrowings);
            UIHelper.StylePrimaryButton(btnAddBook);
            UIHelper.StylePrimaryButton(btnUpdateBook);
            UIHelper.StyleDangerButton(btnDeleteBook);
            UIHelper.StyleButton(btnClearBook, UIHelper.NeutralColor);
            UIHelper.StylePrimaryButton(btnBorrow);
            UIHelper.StyleButton(btnReturn, UIHelper.WarningColor);
            UIHelper.StyleTextBox(txtTitle);
            UIHelper.StyleTextBox(txtAuthor);
            UIHelper.StyleTextBox(txtISBN);
            UIHelper.StyleTextBox(txtPublisher);
            UIHelper.StyleTextBox(txtPublicationYear);
            UIHelper.StyleTextBox(txtCopies);
            UIHelper.StyleTextBox(txtShelf);
            UIHelper.StyleTextBox(txtBookNotes);
            UIHelper.StyleTextBox(txtBorrowNotes);
            UIHelper.StyleComboBox(cmbCategory);
            UIHelper.StyleComboBox(cmbBook);
            UIHelper.StyleComboBox(cmbBorrowerType);
            UIHelper.StyleComboBox(cmbBorrower);
            lblRecordCount.ForeColor = UIHelper.MutedTextColor;
        }

        private async void LibraryForm_Load(object sender, EventArgs e)
        {
            try
            {
                isLoading = true;

                InitializeLookups();

                dtpBorrowDate.Value = DateTime.Today;
                dtpDueDate.Value = DateTime.Today.AddDays(14);

                await LoadBooksAsync();
                await LoadBooksIntoComboAsync();
                await LoadBorrowersAsync();
                await LoadBorrowingsAsync();

                ClearBookInputs();
                ClearBorrowInputs();

                tabControl.SelectedTab = tabBooks;
                lblRecordCount.Text = "عدد الكتب: " + (allBooks == null ? 0 : allBooks.Rows.Count);
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("تحميل شاشة المكتبة", ex);
            }
            finally
            {
                isLoading = false;
            }
        }

        private async void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoading)
                return;

            if (tabControl.SelectedTab == tabBooks)
            {
                await LoadBooksAsync();
                lblRecordCount.Text = "عدد الكتب: " + (allBooks == null ? 0 : allBooks.Rows.Count);
            }
            else if (tabControl.SelectedTab == tabBorrowing)
            {
                await LoadBooksIntoComboAsync();
                await LoadBorrowersAsync();
                await LoadBorrowingsAsync();
                lblRecordCount.Text = "عدد الإعارات: " + (allBorrowings == null ? 0 : allBorrowings.Rows.Count);
            }
        }

        private void InitializeLookups()
        {
            cmbCategory.Items.Clear();
            cmbCategory.Items.AddRange(new object[]
            {
                "تعليمي",
                "أدبي",
                "علمي",
                "تاريخي",
                "ديني",
                "تقني",
                "ثقافي",
                "آخر"
            });
            cmbCategory.SelectedIndex = 0;

            cmbBorrowerType.Items.Clear();
            cmbBorrowerType.Items.AddRange(new object[] { "طالب", "معلم" });
            cmbBorrowerType.SelectedIndex = 0;
        }

        // ===================== الكتب =====================

        private async Task LoadBooksAsync()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                allBooks = await Task.Run(() => bookService.GetAllBooks());

                dataGridViewBooks.DataSource = allBooks;

                FormatBooksGrid();
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void FormatBooksGrid()
        {
            if (dataGridViewBooks.Columns.Count == 0)
                return;

            HideBookColumn("BookID");
            HideBookColumn("CreatedAt");
            HideBookColumn("UpdatedAt");

            SetBookHeader("Title", "العنوان");
            SetBookHeader("Author", "المؤلف");
            SetBookHeader("ISBN", "ISBN");
            SetBookHeader("Category", "الفئة");
            SetBookHeader("Publisher", "الناشر");
            SetBookHeader("PublicationYear", "سنة النشر");
            SetBookHeader("Copies", "النسخ");
            SetBookHeader("AvailableCopies", "المتاح");
            SetBookHeader("ShelfLocation", "الرف");
            SetBookHeader("Notes", "ملاحظات");

            dataGridViewBooks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void HideBookColumn(string columnName)
        {
            if (dataGridViewBooks.Columns.Contains(columnName))
                dataGridViewBooks.Columns[columnName].Visible = false;
        }

        private void SetBookHeader(string columnName, string headerText)
        {
            if (dataGridViewBooks.Columns.Contains(columnName))
                dataGridViewBooks.Columns[columnName].HeaderText = headerText;
        }

        private bool ValidateBookInputs()
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                UIHelper.ShowWarning("أدخل عنوان الكتاب.");
                txtTitle.Focus();
                return false;
            }

            int copies;
            if (!TryParsePositiveInt(txtCopies.Text, out copies))
            {
                UIHelper.ShowWarning("أدخل عدد نسخ صحيح أكبر من صفر.");
                txtCopies.Focus();
                return false;
            }

            if (!string.IsNullOrWhiteSpace(txtPublicationYear.Text))
            {
                int year;
                if (!TryParseNonNegativeInt(txtPublicationYear.Text, out year) || year < 1000 || year > DateTime.Today.Year)
                {
                    UIHelper.ShowWarning("سنة النشر يجب أن تكون بين 1000 والسنة الحالية.");
                    txtPublicationYear.Focus();
                    return false;
                }
            }

            if (txtTitle.Text.Trim().Length > 250 || txtAuthor.Text.Trim().Length > 150 ||
                txtISBN.Text.Trim().Length > 30 || txtPublisher.Text.Trim().Length > 150 ||
                txtShelf.Text.Trim().Length > 80 || txtBookNotes.Text.Trim().Length > 1000)
            {
                UIHelper.ShowWarning("تجاوز أحد حقول الكتاب الحد المسموح به.");
                return false;
            }

            return true;
        }

        private Book GetBookFromInputs()
        {
            int year = 0;
            int copies = 1;

            TryParseNonNegativeInt(txtPublicationYear.Text, out year);
            if (!TryParsePositiveInt(txtCopies.Text, out copies))
                copies = 1;

            return new Book
            {
                BookID = selectedBookId,
                Title = txtTitle.Text.Trim(),
                Author = txtAuthor.Text.Trim(),
                ISBN = txtISBN.Text.Trim(),
                Category = cmbCategory.Text.Trim(),
                Publisher = txtPublisher.Text.Trim(),
                PublicationYear = year,
                Copies = copies,
                ShelfLocation = txtShelf.Text.Trim(),
                Notes = txtBookNotes.Text.Trim()
            };
        }

        private bool TryParseNonNegativeInt(string value, out int number)
        {
            number = 0;
            if (!UIHelper.TryParseDecimal(value, out decimal parsed) || parsed < 0 || parsed != decimal.Truncate(parsed) || parsed > int.MaxValue)
                return false;

            number = (int)parsed;
            return true;
        }

        private bool TryParsePositiveInt(string value, out int number)
        {
            return TryParseNonNegativeInt(value, out number) && number > 0;
        }

        private void ClearBookInputs()
        {
            selectedBookId = 0;

            txtTitle.Clear();
            txtAuthor.Clear();
            txtISBN.Clear();

            if (cmbCategory.Items.Count > 0)
                cmbCategory.SelectedIndex = 0;

            txtPublisher.Clear();
            txtPublicationYear.Clear();
            txtCopies.Text = "1";
            txtShelf.Clear();
            txtBookNotes.Clear();
        }

        private void dataGridViewBooks_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dataGridViewBooks.Rows.Count == 0)
                return;

            DataRowView rowView = dataGridViewBooks.Rows[e.RowIndex].DataBoundItem as DataRowView;

            if (rowView == null)
                return;

            DataRow row = rowView.Row;

            selectedBookId = row["BookID"] != DBNull.Value && int.TryParse(row["BookID"].ToString(), out int bookId)
                ? bookId
                : 0;

            txtTitle.Text = row["Title"].ToString();
            txtAuthor.Text = row["Author"] == DBNull.Value ? "" : row["Author"].ToString();
            txtISBN.Text = row["ISBN"] == DBNull.Value ? "" : row["ISBN"].ToString();
            cmbCategory.Text = row["Category"] == DBNull.Value ? "آخر" : row["Category"].ToString();
            txtPublisher.Text = row["Publisher"] == DBNull.Value ? "" : row["Publisher"].ToString();
            txtPublicationYear.Text = row["PublicationYear"] == DBNull.Value ? "" : row["PublicationYear"].ToString();
            txtCopies.Text = row["Copies"].ToString();
            txtShelf.Text = row["ShelfLocation"] == DBNull.Value ? "" : row["ShelfLocation"].ToString();
            txtBookNotes.Text = row["Notes"] == DBNull.Value ? "" : row["Notes"].ToString();
        }

        private async void btnAddBook_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateBookInputs())
                    return;

                Book book = GetBookFromInputs();

                await Task.Run(() => bookService.AddBook(book));

                UIHelper.ShowInfo("تمت إضافة الكتاب بنجاح.");

                await LoadBooksAsync();
                await LoadBooksIntoComboAsync();
                ClearBookInputs();
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("إضافة الكتاب", ex);
            }
        }

        private async void btnUpdateBook_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedBookId == 0)
                {
                    UIHelper.ShowWarning("اختر كتاباً من الجدول.");
                    return;
                }

                if (!ValidateBookInputs())
                    return;

                Book book = GetBookFromInputs();

                bool result = await Task.Run(() => bookService.UpdateBook(book));

                if (result)
                    UIHelper.ShowInfo("تم تعديل الكتاب بنجاح.");
                else
                    UIHelper.ShowWarning("لم يتم العثور على الكتاب أو لم يتم تعديله.");

                await LoadBooksAsync();
                await LoadBooksIntoComboAsync();
                ClearBookInputs();
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("تعديل الكتاب", ex);
            }
        }

        private async void btnDeleteBook_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedBookId == 0)
                {
                    UIHelper.ShowWarning("اختر كتاباً من الجدول.");
                    return;
                }

                DialogResult confirm = MessageBox.Show(
                    "هل تريد حذف الكتاب المحدد؟",
                    "تأكيد الحذف",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirm != DialogResult.Yes)
                    return;

                bool result = await Task.Run(() => bookService.DeleteBook(selectedBookId));

                if (result)
                    UIHelper.ShowInfo("تم حذف الكتاب بنجاح.");
                else
                    UIHelper.ShowWarning("لم يتم العثور على الكتاب أو لم يتم حذفه.");

                await LoadBooksAsync();
                await LoadBooksIntoComboAsync();
                ClearBookInputs();
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("حذف الكتاب", ex);
            }
        }

        private void btnClearBook_Click(object sender, EventArgs e)
        {
            ClearBookInputs();
        }

        // ===================== الإعارة =====================

        private async Task LoadBooksIntoComboAsync()
        {
            DataTable books = await Task.Run(() => bookService.GetAllBooks());

            cmbBook.DataSource = books;
            cmbBook.DisplayMember = "Title";
            cmbBook.ValueMember = "BookID";

            if (cmbBook.Items.Count > 0)
                cmbBook.SelectedIndex = 0;
        }

        private async Task LoadBorrowersAsync()
        {
            try
            {
                if (cmbBorrowerType.Text == "طالب")
                {
                    DataTable students = await Task.Run(() => studentService.GetAllStudents());

                    cmbBorrower.DataSource = students;
                    cmbBorrower.DisplayMember = "StudentName";
                    cmbBorrower.ValueMember = "StudentID";
                }
                else
                {
                    DataTable teachers = await Task.Run(() => teacherService.GetAllTeachers());

                    cmbBorrower.DataSource = teachers;
                    cmbBorrower.DisplayMember = "TeacherName";
                    cmbBorrower.ValueMember = "TeacherID";
                }

                if (cmbBorrower.Items.Count > 0)
                    cmbBorrower.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("تحميل المستعيرين", ex);
            }
        }

        private async Task LoadBorrowingsAsync()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                allBorrowings = await Task.Run(() => borrowingService.GetAllBorrowings());

                dataGridViewBorrowings.DataSource = allBorrowings;

                FormatBorrowingsGrid();
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void FormatBorrowingsGrid()
        {
            if (dataGridViewBorrowings.Columns.Count == 0)
                return;

            HideBorrowingColumn("BorrowingID");
            HideBorrowingColumn("BookID");
            HideBorrowingColumn("BorrowerID");
            HideBorrowingColumn("CreatedAt");
            HideBorrowingColumn("UpdatedAt");

            SetBorrowingHeader("BookTitle", "الكتاب");
            SetBorrowingHeader("BorrowerType", "نوع المستعير");
            SetBorrowingHeader("BorrowerName", "المستعير");
            SetBorrowingHeader("BorrowDate", "تاريخ الإعارة");
            SetBorrowingHeader("DueDate", "تاريخ الإرجاع");
            SetBorrowingHeader("ReturnDate", "تاريخ الاسترجاع");
            SetBorrowingHeader("Status", "الحالة");
            SetBorrowingHeader("Notes", "ملاحظات");

            FormatBorrowingDate("BorrowDate");
            FormatBorrowingDate("DueDate");
            FormatBorrowingDate("ReturnDate");

            dataGridViewBorrowings.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void HideBorrowingColumn(string columnName)
        {
            if (dataGridViewBorrowings.Columns.Contains(columnName))
                dataGridViewBorrowings.Columns[columnName].Visible = false;
        }

        private void SetBorrowingHeader(string columnName, string headerText)
        {
            if (dataGridViewBorrowings.Columns.Contains(columnName))
                dataGridViewBorrowings.Columns[columnName].HeaderText = headerText;
        }

        private void FormatBorrowingDate(string columnName)
        {
            if (dataGridViewBorrowings.Columns.Contains(columnName))
                dataGridViewBorrowings.Columns[columnName].DefaultCellStyle.Format = "dd/MM/yyyy";
        }

        private bool ValidateBorrowInputs()
        {
            if (cmbBook.SelectedValue == null)
            {
                UIHelper.ShowWarning("اختر الكتاب.");
                cmbBook.Focus();
                return false;
            }

            if (cmbBorrower.SelectedValue == null)
            {
                UIHelper.ShowWarning("اختر المستعير.");
                cmbBorrower.Focus();
                return false;
            }

            if (cmbBorrowerType.SelectedIndex < 0 ||
                (cmbBorrowerType.Text != "طالب" && cmbBorrowerType.Text != "معلم"))
            {
                UIHelper.ShowWarning("اختر نوع المستعير بشكل صحيح.");
                cmbBorrowerType.Focus();
                return false;
            }

            if (dtpBorrowDate.Value.Date > DateTime.Today)
            {
                UIHelper.ShowWarning("لا يمكن تسجيل إعارة بتاريخ مستقبلي.");
                dtpBorrowDate.Focus();
                return false;
            }

            if (dtpDueDate.Value.Date < dtpBorrowDate.Value.Date)
            {
                UIHelper.ShowWarning("تاريخ الإرجاع يجب أن يكون بعد تاريخ الإعارة أو مساوياً له.");
                dtpDueDate.Focus();
                return false;
            }

            if (txtBorrowNotes.Text.Trim().Length > 1000)
            {
                UIHelper.ShowWarning("تجاوزت الملاحظات الحد المسموح به.");
                txtBorrowNotes.Focus();
                return false;
            }

            return true;
        }

        private void ClearBorrowInputs()
        {
            selectedBorrowingId = 0;

            if (cmbBook.Items.Count > 0)
                cmbBook.SelectedIndex = 0;

            if (cmbBorrowerType.Items.Count > 0)
                cmbBorrowerType.SelectedIndex = 0;

            if (cmbBorrower.Items.Count > 0)
                cmbBorrower.SelectedIndex = 0;

            dtpBorrowDate.Value = DateTime.Today;
            dtpDueDate.Value = DateTime.Today.AddDays(14);
            txtBorrowNotes.Clear();
        }

        private async void btnBorrow_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateBorrowInputs())
                    return;

                if (!int.TryParse(cmbBook.SelectedValue.ToString(), out int bookId) || bookId <= 0 ||
                    !int.TryParse(cmbBorrower.SelectedValue.ToString(), out int borrowerId) || borrowerId <= 0)
                {
                    UIHelper.ShowWarning("بيانات الكتاب أو المستعير غير صالحة، أعد تحميل القائمة ثم حاول مرة أخرى.");
                    return;
                }

                Borrowing borrowing = new Borrowing
                {
                    BookID = bookId,
                    BorrowerType = cmbBorrowerType.Text.Trim(),
                    BorrowerID = borrowerId,
                    BorrowDate = dtpBorrowDate.Value.Date,
                    DueDate = dtpDueDate.Value.Date,
                    Status = "معار",
                    Notes = txtBorrowNotes.Text.Trim()
                };

                await Task.Run(() => borrowingService.AddBorrowing(borrowing));

                UIHelper.ShowInfo("تمت إعارة الكتاب بنجاح.");

                await LoadBooksAsync();
                await LoadBooksIntoComboAsync();
                await LoadBorrowingsAsync();
                ClearBorrowInputs();
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("إعارة الكتاب", ex);
            }
        }

        private async void btnReturn_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedBorrowingId == 0)
                {
                    UIHelper.ShowWarning("اختر عملية إعارة من الجدول.");
                    return;
                }

                bool result = await Task.Run(() =>
                    borrowingService.ReturnBook(selectedBorrowingId, DateTime.Today)
                );

                if (result)
                    UIHelper.ShowInfo("تم استرجاع الكتاب بنجاح.");
                else
                    UIHelper.ShowWarning("لم يتم العثور على عملية الإعارة أو لم يتم استرجاع الكتاب.");

                await LoadBooksAsync();
                await LoadBooksIntoComboAsync();
                await LoadBorrowingsAsync();
                ClearBorrowInputs();
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("استرجاع الكتاب", ex);
            }
        }

        private void dataGridViewBorrowings_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dataGridViewBorrowings.Rows.Count == 0)
                return;

            DataRowView rowView = dataGridViewBorrowings.Rows[e.RowIndex].DataBoundItem as DataRowView;

            if (rowView == null)
                return;

            DataRow row = rowView.Row;

            selectedBorrowingId = row["BorrowingID"] != DBNull.Value && int.TryParse(row["BorrowingID"].ToString(), out int borrowingId)
                ? borrowingId
                : 0;

            if (row["BookID"] != DBNull.Value && cmbBook.Items.Count > 0 && int.TryParse(row["BookID"].ToString(), out int bookId))
                cmbBook.SelectedValue = bookId;

            cmbBorrowerType.Text = row["BorrowerType"].ToString();

            if (row["BorrowerID"] != DBNull.Value && cmbBorrower.Items.Count > 0 && int.TryParse(row["BorrowerID"].ToString(), out int borrowerId))
                cmbBorrower.SelectedValue = borrowerId;

            if (row["BorrowDate"] != DBNull.Value && DateTime.TryParse(row["BorrowDate"].ToString(), out DateTime borrowDate))
                dtpBorrowDate.Value = borrowDate <= DateTime.Today ? borrowDate : DateTime.Today;

            if (row["DueDate"] != DBNull.Value && DateTime.TryParse(row["DueDate"].ToString(), out DateTime dueDate))
                dtpDueDate.Value = dueDate;

            txtBorrowNotes.Text = row["Notes"] == DBNull.Value ? "" : row["Notes"].ToString();
        }
    }
}
