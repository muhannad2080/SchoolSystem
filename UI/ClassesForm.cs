using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using SchoolSystem.Models;
using SchoolSystem.Helpers;
using SchoolSystem.Services;

namespace SchoolSystem.UI
{
    public partial class ClassesForm : UserControl
    {
        private readonly ClassService classService = new ClassService();
        private readonly RoomService roomService = new RoomService();

        private DataTable classesTable;
        private DataTable roomsTable;

        private int selectedClassId = 0;
        private int selectedRoomId = 0;

        public ClassesForm()
        {
            InitializeComponent();
            SchoolSystem.Helpers.UIHelper.ApplyStyle(this);
            Dock = DockStyle.Fill;
            Load += ClassesForm_Load;
        }

        private async void ClassesForm_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                await LoadClassesAsync();
                await LoadRoomsAsync();

                ClearClassFields();
                ClearRoomFields();

                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                ShowError("حدث خطأ أثناء تحميل واجهة الفصول والقاعات:\n" + ex.Message);
            }
        }

        // =========================================================
        // دوال مساعدة للمصمم - مهمة جدًا حتى لا يتوقف Designer
        // =========================================================

        public void AddLabelStyle(System.Windows.Forms.Label label)
        {
            if (label == null)
                return;

            label.Dock = System.Windows.Forms.DockStyle.Fill;
            label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        }

        public void SetupButton(System.Windows.Forms.Button button, string text, System.Drawing.Color color)
        {
            if (button == null)
                return;

            button.Text = text;
            button.Size = new System.Drawing.Size(115, 36);
            button.BackColor = color;
            button.ForeColor = System.Drawing.Color.White;
            button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
        }

        public void SetupGrid(System.Windows.Forms.DataGridView grid)
        {
            if (grid == null)
                return;

            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            grid.BackgroundColor = System.Drawing.Color.White;
            grid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            grid.Dock = System.Windows.Forms.DockStyle.Fill;
            grid.MultiSelect = false;
            grid.ReadOnly = true;
            grid.RowHeadersVisible = false;
            grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        }

        // =========================================================
        // تحميل بيانات الفصول
        // =========================================================

        private async Task LoadClassesAsync()
        {
            try
            {
                classesTable = await Task.Run(() => classService.GetClassDetails());

                dataGridViewClasses.DataSource = classesTable;

                FormatClassesGrid();

                if (classesTable != null)
                    lblClassCount.Text = "عدد الفصول: " + classesTable.DefaultView.Count;
                else
                    lblClassCount.Text = "عدد الفصول: 0";
            }
            catch (Exception ex)
            {
                ShowError("خطأ أثناء تحميل الفصول:\n" + ex.Message);
            }
        }

        private void FormatClassesGrid()
        {
            if (dataGridViewClasses.Columns.Count == 0)
                return;

            SetHeader(dataGridViewClasses, "ClassID", "الرقم");
            SetHeader(dataGridViewClasses, "ClassCode", "الكود");
            SetHeader(dataGridViewClasses, "ClassName", "الفصل");
            SetHeader(dataGridViewClasses, "StageName", "المرحلة");
            SetHeader(dataGridViewClasses, "GradeOrder", "الترتيب");
            SetHeader(dataGridViewClasses, "IsActive", "نشط");
            SetHeader(dataGridViewClasses, "Notes", "ملاحظات");
            SetHeader(dataGridViewClasses, "CreatedAt", "تاريخ الإضافة");
            SetHeader(dataGridViewClasses, "UpdatedAt", "آخر تعديل");

            dataGridViewClasses.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private SchoolClass BuildClassModel()
        {
            SchoolClass item = new SchoolClass();

            item.ClassID = selectedClassId;
            item.ClassCode = txtClassCode.Text.Trim();
            item.ClassName = txtClassName.Text.Trim();
            item.StageName = txtStageName.Text.Trim();
            item.GradeOrder = Convert.ToInt32(nudGradeOrder.Value);
            item.IsActive = chkClassActive.Checked;
            item.Notes = txtClassNotes.Text.Trim();

            return item;
        }

        private bool ValidateClassInputs()
        {
            if (string.IsNullOrWhiteSpace(txtClassName.Text))
            {
                ShowWarning("اسم الفصل مطلوب.");
                txtClassName.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtStageName.Text))
            {
                ShowWarning("اسم المرحلة مطلوب.");
                txtStageName.Focus();
                return false;
            }
            if (nudGradeOrder.Value <= 0)
            {
                ShowWarning("ترتيب الفصل يجب أن يكون أكبر من صفر.");
                nudGradeOrder.Focus();
                return false;
            }
            if (txtClassNotes.Text.Trim().Length > 1000)
            {
                ShowWarning("ملاحظات الفصل لا يمكن أن تتجاوز 1000 حرف.");
                txtClassNotes.Focus();
                return false;
            }
            return true;
        }

        private bool ValidateRoomInputs()
        {
            if (string.IsNullOrWhiteSpace(txtRoomCode.Text))
            {
                ShowWarning("كود القاعة مطلوب.");
                txtRoomCode.Focus();
                return false;
            }
            if (txtRoomCode.Text.Trim().Length < 2 || txtRoomCode.Text.Trim().Length > 30)
            {
                ShowWarning("كود القاعة يجب أن يكون بين حرفين و30 حرفًا.");
                txtRoomCode.Focus();
                return false;
            }
            foreach (char character in txtRoomCode.Text.Trim())
            {
                if (!(char.IsLetterOrDigit(character) || character == '_' || character == '-'))
                {
                    ShowWarning("كود القاعة يجب أن يحتوي على حروف أو أرقام أو (_) أو (-) فقط.");
                    txtRoomCode.Focus();
                    return false;
                }
            }
            if (string.IsNullOrWhiteSpace(txtRoomName.Text))
            {
                ShowWarning("اسم القاعة مطلوب.");
                txtRoomName.Focus();
                return false;
            }
            if (cmbRoomType.SelectedIndex < 0 || string.IsNullOrWhiteSpace(cmbRoomType.Text))
            {
                ShowWarning("يرجى اختيار نوع القاعة.");
                cmbRoomType.Focus();
                return false;
            }
            if (nudCapacity.Value <= 0)
            {
                ShowWarning("سعة القاعة يجب أن تكون أكبر من صفر.");
                nudCapacity.Focus();
                return false;
            }
            if (txtRoomNotes.Text.Trim().Length > 1000)
            {
                ShowWarning("ملاحظات القاعة لا يمكن أن تتجاوز 1000 حرف.");
                txtRoomNotes.Focus();
                return false;
            }
            return true;
        }

        private async void btnClassUpdate_Click(object sender, EventArgs e)
        {
            if (selectedClassId <= 0)
            {
                ShowWarning("اختر فصلًا من الجدول أولًا.");
                return;
            }

            if (!ValidateClassInputs())
                return;

            try
            {
                SchoolClass item = BuildClassModel();

                bool updated = await Task.Run(() => classService.UpdateClass(item));

                if (updated)
                {
                    ShowInfo("تم تعديل بيانات الفصل بنجاح.");
                    await LoadClassesAsync();
                    ClearClassFields();
                }
                else
                {
                    ShowWarning("لم يتم العثور على الفصل.");
                }
            }
            catch (Exception ex)
            {
                ShowError("خطأ أثناء تعديل الفصل:\n" + ex.Message);
            }
        }

        private void btnClassClear_Click(object sender, EventArgs e)
        {
            ClearClassFields();
        }

        private async void btnClassRefresh_Click(object sender, EventArgs e)
        {
            await LoadClassesAsync();
            ClearClassFields();
        }

        private void dataGridViewClasses_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dataGridViewClasses.Rows.Count == 0)
                return;

            DataRowView view = dataGridViewClasses.Rows[e.RowIndex].DataBoundItem as DataRowView;

            if (view == null)
                return;

            FillClassFields(view.Row);
        }

        private void FillClassFields(DataRow row)
        {
            if (row == null)
                return;

            selectedClassId = row["ClassID"] != DBNull.Value
                ? Convert.ToInt32(row["ClassID"])
                : 0;

            txtClassID.Text = selectedClassId > 0 ? selectedClassId.ToString() : "";

            txtClassCode.Text = row.Table.Columns.Contains("ClassCode") && row["ClassCode"] != DBNull.Value
                ? row["ClassCode"].ToString()
                : "";

            txtClassName.Text = row.Table.Columns.Contains("ClassName") && row["ClassName"] != DBNull.Value
                ? row["ClassName"].ToString()
                : "";

            txtStageName.Text = row.Table.Columns.Contains("StageName") && row["StageName"] != DBNull.Value
                ? row["StageName"].ToString()
                : "";

            if (row.Table.Columns.Contains("GradeOrder") && row["GradeOrder"] != DBNull.Value)
                nudGradeOrder.Value = Convert.ToDecimal(row["GradeOrder"]);
            else
                nudGradeOrder.Value = 1;

            chkClassActive.Checked =
                row.Table.Columns.Contains("IsActive") &&
                row["IsActive"] != DBNull.Value &&
                Convert.ToBoolean(row["IsActive"]);

            txtClassNotes.Text = row.Table.Columns.Contains("Notes") && row["Notes"] != DBNull.Value
                ? row["Notes"].ToString()
                : "";
        }

        private void ClearClassFields()
        {
            selectedClassId = 0;

            txtClassID.Clear();
            txtClassCode.Clear();
            txtClassName.Clear();
            txtStageName.Clear();

            nudGradeOrder.Value = 1;
            chkClassActive.Checked = true;
            txtClassNotes.Clear();
        }

        private void txtClassSearch_TextChanged(object sender, EventArgs e)
        {
            if (classesTable == null)
                return;

            string safe = UIHelper.EscapeDataViewFilterValue(txtClassSearch.Text);

            if (string.IsNullOrWhiteSpace(safe))
            {
                classesTable.DefaultView.RowFilter = "";
            }
            else
            {
                string filter = "";

                if (classesTable.Columns.Contains("ClassName"))
                    filter += "ClassName LIKE '%" + safe + "%'";

                if (classesTable.Columns.Contains("StageName"))
                {
                    if (!string.IsNullOrWhiteSpace(filter))
                        filter += " OR ";

                    filter += "StageName LIKE '%" + safe + "%'";
                }

                if (classesTable.Columns.Contains("ClassCode"))
                {
                    if (!string.IsNullOrWhiteSpace(filter))
                        filter += " OR ";

                    filter += "ClassCode LIKE '%" + safe + "%'";
                }

                classesTable.DefaultView.RowFilter = filter;
            }

            lblClassCount.Text = "عدد الفصول: " + classesTable.DefaultView.Count;
        }

        // =========================================================
        // تحميل بيانات القاعات
        // =========================================================

        private async Task LoadRoomsAsync()
        {
            try
            {
                roomsTable = await Task.Run(() => roomService.GetAllRooms());

                dataGridViewRooms.DataSource = roomsTable;

                FormatRoomsGrid();

                if (roomsTable != null)
                    lblRoomCount.Text = "عدد القاعات: " + roomsTable.DefaultView.Count;
                else
                    lblRoomCount.Text = "عدد القاعات: 0";
            }
            catch (Exception ex)
            {
                ShowError("خطأ أثناء تحميل القاعات:\n" + ex.Message);
            }
        }

        private void FormatRoomsGrid()
        {
            if (dataGridViewRooms.Columns.Count == 0)
                return;

            SetHeader(dataGridViewRooms, "RoomID", "الرقم");
            SetHeader(dataGridViewRooms, "RoomCode", "الكود");
            SetHeader(dataGridViewRooms, "RoomName", "القاعة");
            SetHeader(dataGridViewRooms, "RoomType", "النوع");
            SetHeader(dataGridViewRooms, "Capacity", "السعة");
            SetHeader(dataGridViewRooms, "Location", "الموقع");
            SetHeader(dataGridViewRooms, "IsActive", "نشطة");
            SetHeader(dataGridViewRooms, "Notes", "ملاحظات");
            SetHeader(dataGridViewRooms, "CreatedAt", "تاريخ الإضافة");
            SetHeader(dataGridViewRooms, "UpdatedAt", "آخر تعديل");

            dataGridViewRooms.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private Room BuildRoomModel()
        {
            Room room = new Room();

            room.RoomID = selectedRoomId;
            room.RoomCode = txtRoomCode.Text.Trim();
            room.RoomName = txtRoomName.Text.Trim();
            room.RoomType = cmbRoomType.Text;
            room.Capacity = Convert.ToInt32(nudCapacity.Value);
            room.Location = txtLocation.Text.Trim();
            room.IsActive = chkRoomActive.Checked;
            room.Notes = txtRoomNotes.Text.Trim();

            return room;
        }

        private async void btnRoomAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateRoomInputs())
                return;

            try
            {
                Room room = BuildRoomModel();

                await Task.Run(() => roomService.AddRoom(room));

                ShowInfo("تمت إضافة القاعة بنجاح.");

                await LoadRoomsAsync();
                ClearRoomFields();
            }
            catch (Exception ex)
            {
                ShowError("خطأ أثناء إضافة القاعة:\n" + ex.Message);
            }
        }

        private async void btnRoomUpdate_Click(object sender, EventArgs e)
        {
            if (selectedRoomId <= 0)
            {
                ShowWarning("اختر قاعة من الجدول أولًا.");
                return;
            }

            if (!ValidateRoomInputs())
                return;

            try
            {
                Room room = BuildRoomModel();
                room.RoomID = selectedRoomId;

                bool updated = await Task.Run(() => roomService.UpdateRoom(room));

                if (updated)
                {
                    ShowInfo("تم تعديل القاعة بنجاح.");
                    await LoadRoomsAsync();
                    ClearRoomFields();
                }
                else
                {
                    ShowWarning("لم يتم العثور على القاعة.");
                }
            }
            catch (Exception ex)
            {
                ShowError("خطأ أثناء تعديل القاعة:\n" + ex.Message);
            }
        }

        private async void btnRoomDelete_Click(object sender, EventArgs e)
        {
            if (selectedRoomId <= 0)
            {
                ShowWarning("اختر قاعة من الجدول أولًا.");
                return;
            }

            DialogResult result = MessageBox.Show(
                "هل تريد حذف هذه القاعة؟",
                "تأكيد الحذف",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2,
                MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);

            if (result != DialogResult.Yes)
                return;

            try
            {
                bool deleted = await Task.Run(() => roomService.DeleteRoom(selectedRoomId));

                if (deleted)
                {
                    ShowInfo("تم حذف القاعة بنجاح.");
                    await LoadRoomsAsync();
                    ClearRoomFields();
                }
                else
                {
                    ShowWarning("لم يتم العثور على القاعة.");
                }
            }
            catch (Exception ex)
            {
                ShowError("تعذر حذف القاعة، ربما مستخدمة في الجدول الدراسي:\n" + ex.Message);
            }
        }

        private void btnRoomClear_Click(object sender, EventArgs e)
        {
            ClearRoomFields();
        }

        private async void btnRoomRefresh_Click(object sender, EventArgs e)
        {
            await LoadRoomsAsync();
            ClearRoomFields();
        }

        private void dataGridViewRooms_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dataGridViewRooms.Rows.Count == 0)
                return;

            DataRowView view = dataGridViewRooms.Rows[e.RowIndex].DataBoundItem as DataRowView;

            if (view == null)
                return;

            FillRoomFields(view.Row);
        }

        private void FillRoomFields(DataRow row)
        {
            if (row == null)
                return;

            selectedRoomId = row["RoomID"] != DBNull.Value
                ? Convert.ToInt32(row["RoomID"])
                : 0;

            txtRoomID.Text = selectedRoomId > 0 ? selectedRoomId.ToString() : "";

            txtRoomCode.Text = row.Table.Columns.Contains("RoomCode") && row["RoomCode"] != DBNull.Value
                ? row["RoomCode"].ToString()
                : "";

            txtRoomName.Text = row.Table.Columns.Contains("RoomName") && row["RoomName"] != DBNull.Value
                ? row["RoomName"].ToString()
                : "";

            string roomType = row.Table.Columns.Contains("RoomType") && row["RoomType"] != DBNull.Value
                ? row["RoomType"].ToString()
                : "قاعة دراسية";

            if (cmbRoomType.Items.Contains(roomType))
                cmbRoomType.SelectedItem = roomType;
            else if (cmbRoomType.Items.Count > 0)
                cmbRoomType.SelectedIndex = 0;

            if (row.Table.Columns.Contains("Capacity") && row["Capacity"] != DBNull.Value)
                nudCapacity.Value = Convert.ToDecimal(row["Capacity"]);
            else
                nudCapacity.Value = 30;

            txtLocation.Text = row.Table.Columns.Contains("Location") && row["Location"] != DBNull.Value
                ? row["Location"].ToString()
                : "";

            chkRoomActive.Checked =
                row.Table.Columns.Contains("IsActive") &&
                row["IsActive"] != DBNull.Value &&
                Convert.ToBoolean(row["IsActive"]);

            txtRoomNotes.Text = row.Table.Columns.Contains("Notes") && row["Notes"] != DBNull.Value
                ? row["Notes"].ToString()
                : "";
        }

        private void ClearRoomFields()
        {
            selectedRoomId = 0;

            txtRoomID.Clear();
            txtRoomCode.Clear();
            txtRoomName.Clear();

            if (cmbRoomType.Items.Count > 0)
                cmbRoomType.SelectedIndex = 0;

            nudCapacity.Value = 30;
            txtLocation.Clear();
            chkRoomActive.Checked = true;
            txtRoomNotes.Clear();

            txtRoomCode.Focus();
        }

        private void txtRoomSearch_TextChanged(object sender, EventArgs e)
        {
            if (roomsTable == null)
                return;

            string safe = UIHelper.EscapeDataViewFilterValue(txtRoomSearch.Text);

            if (string.IsNullOrWhiteSpace(safe))
            {
                roomsTable.DefaultView.RowFilter = "";
            }
            else
            {
                string filter = "";

                if (roomsTable.Columns.Contains("RoomCode"))
                    filter += "RoomCode LIKE '%" + safe + "%'";

                if (roomsTable.Columns.Contains("RoomName"))
                {
                    if (!string.IsNullOrWhiteSpace(filter))
                        filter += " OR ";

                    filter += "RoomName LIKE '%" + safe + "%'";
                }

                if (roomsTable.Columns.Contains("RoomType"))
                {
                    if (!string.IsNullOrWhiteSpace(filter))
                        filter += " OR ";

                    filter += "RoomType LIKE '%" + safe + "%'";
                }

                if (roomsTable.Columns.Contains("Location"))
                {
                    if (!string.IsNullOrWhiteSpace(filter))
                        filter += " OR ";

                    filter += "Location LIKE '%" + safe + "%'";
                }

                roomsTable.DefaultView.RowFilter = filter;
            }

            lblRoomCount.Text = "عدد القاعات: " + roomsTable.DefaultView.Count;
        }

        // =========================================================
        // Helpers
        // =========================================================

        private void SetHeader(DataGridView grid, string columnName, string header)
        {
            if (grid.Columns.Contains(columnName))
                grid.Columns[columnName].HeaderText = header;
        }

        private void ShowInfo(string message)
        {
            MessageBox.Show(
                message,
                "معلومة",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
        }

        private void ShowWarning(string message)
        {
            MessageBox.Show(
                message,
                "تنبيه",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
        }

        private void ShowError(string message)
        {
            MessageBox.Show(
                message,
                "خطأ",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
        }
    }
}
