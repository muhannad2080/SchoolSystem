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
            Dock = DockStyle.Fill;

            Load += LibraryForm_Load;
            tabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;
            cmbBorrowerType.SelectedIndexChanged += async (s, e) => await LoadBorrowersAsync();
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
                MessageBox.Show("أدخل عنوان الكتاب.");
                txtTitle.Focus();
                return false;
            }

            int copies;
            if (!int.TryParse(txtCopies.Text.Trim(), out copies) || copies <= 0)
            {
                MessageBox.Show("أدخل عدد نسخ صحيح أكبر من صفر.");
                txtCopies.Focus();
                return false;
            }

            if (!string.IsNullOrWhiteSpace(txtPublicationYear.Text))
            {
                int year;
                if (!int.TryParse(txtPublicationYear.Text.Trim(), out year) || year < 0)
                {
                    MessageBox.Show("سنة النشر غير صحيحة.");
                    txtPublicationYear.Focus();
                    return false;
                }
            }

            return true;
        }

        private Book GetBookFromInputs()
        {
            int year = 0;
            int copies = 1;

            int.TryParse(txtPublicationYear.Text.Trim(), out year);
            int.TryParse(txtCopies.Text.Trim(), out copies);

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

            selectedBookId = Convert.ToInt32(row["BookID"]);

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

                MessageBox.Show("تمت إضافة الكتاب بنجاح.");

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
                    MessageBox.Show("اختر كتاباً من الجدول.");
                    return;
                }

                if (!ValidateBookInputs())
                    return;

                Book book = GetBookFromInputs();

                bool result = await Task.Run(() => bookService.UpdateBook(book));

                MessageBox.Show(result ? "تم تعديل الكتاب بنجاح." : "لم يتم تعديل الكتاب.");

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
                    MessageBox.Show("اختر كتاباً من الجدول.");
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

                MessageBox.Show(result ? "تم حذف الكتاب." : "لم يتم حذف الكتاب.");

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
                MessageBox.Show("اختر الكتاب.");
                cmbBook.Focus();
                return false;
            }

            if (cmbBorrower.SelectedValue == null)
            {
                MessageBox.Show("اختر المستعير.");
                cmbBorrower.Focus();
                return false;
            }

            if (dtpDueDate.Value.Date < dtpBorrowDate.Value.Date)
            {
                MessageBox.Show("تاريخ الإرجاع يجب أن يكون بعد تاريخ الإعارة أو مساوياً له.");
                dtpDueDate.Focus();
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

                Borrowing borrowing = new Borrowing
                {
                    BookID = Convert.ToInt32(cmbBook.SelectedValue),
                    BorrowerType = cmbBorrowerType.Text.Trim(),
                    BorrowerID = Convert.ToInt32(cmbBorrower.SelectedValue),
                    BorrowDate = dtpBorrowDate.Value.Date,
                    DueDate = dtpDueDate.Value.Date,
                    Status = "معار",
                    Notes = txtBorrowNotes.Text.Trim()
                };

                await Task.Run(() => borrowingService.AddBorrowing(borrowing));

                MessageBox.Show("تمت إعارة الكتاب بنجاح.");

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
                    MessageBox.Show("اختر عملية إعارة من الجدول.");
                    return;
                }

                bool result = await Task.Run(() =>
                    borrowingService.ReturnBook(selectedBorrowingId, DateTime.Today)
                );

                MessageBox.Show(result ? "تم استرجاع الكتاب بنجاح." : "لم يتم استرجاع الكتاب.");

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

            selectedBorrowingId = Convert.ToInt32(row["BorrowingID"]);

            if (row["BookID"] != DBNull.Value && cmbBook.Items.Count > 0)
                cmbBook.SelectedValue = Convert.ToInt32(row["BookID"]);

            cmbBorrowerType.Text = row["BorrowerType"].ToString();

            if (row["BorrowerID"] != DBNull.Value && cmbBorrower.Items.Count > 0)
                cmbBorrower.SelectedValue = Convert.ToInt32(row["BorrowerID"]);

            if (row["BorrowDate"] != DBNull.Value)
                dtpBorrowDate.Value = Convert.ToDateTime(row["BorrowDate"]);

            if (row["DueDate"] != DBNull.Value)
                dtpDueDate.Value = Convert.ToDateTime(row["DueDate"]);

            txtBorrowNotes.Text = row["Notes"] == DBNull.Value ? "" : row["Notes"].ToString();
        }
    }
}
