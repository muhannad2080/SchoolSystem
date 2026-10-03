using System;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using SchoolSystem.Helpers;
using SchoolSystem.Models;
using SchoolSystem.Security;
using SchoolSystem.Services;

namespace SchoolSystem.UI
{
    /// <summary>
    /// إدارة الشعب الدراسية لكل صف وعام دراسي.
    /// </summary>
    public class SchoolSectionsForm : UserControl
    {
        private readonly SchoolSectionService sectionService = new SchoolSectionService();
        private readonly ClassService classService = new ClassService();

        private DataTable sectionsTable;
        private int selectedSectionId;

        private Label lblTitle;
        private TextBox txtYear;
        private ComboBox cmbClass;
        private TextBox txtSectionName;
        private NumericUpDown nudCapacity;
        private ComboBox cmbGender;
        private CheckBox chkActive;
        private TextBox txtSearch;
        private DataGridView grid;
        private Label lblCount;
        private Button btnAdd;
        private Button btnUpdate;
        private Button btnDelete;
        private Button btnClear;
        private Button btnRefresh;

        public SchoolSectionsForm()
        {
            InitializeComponent();
            SchoolSystem.Helpers.UIHelper.ApplyStyle(this);
            ApplyCustomStyles();
            Dock = DockStyle.Fill;
            Load += SchoolSectionsForm_Load;
        }

        private void InitializeComponent()
        {
            BackColor = Color.FromArgb(240, 243, 247);
            RightToLeft = RightToLeft.Yes;
            Font = new Font("Tahoma", 9.5F);
            Padding = new Padding(12);

            lblTitle = new Label
            {
                Dock = DockStyle.Top,
                Height = 54,
                Text = "إدارة الشعب الدراسية",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Tahoma", 15F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(31, 41, 55)
            };

            TableLayoutPanel fields = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 150,
                ColumnCount = 6,
                RowCount = 2,
                BackColor = Color.White,
                Padding = new Padding(10)
            };
            fields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 115));
            fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            fields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 115));
            fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            fields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 115));
            fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            fields.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            fields.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

            Label lYear = MakeLabel("العام الدراسي:");
            Label lClass = MakeLabel("الصف:");
            Label lName = MakeLabel("اسم الشعبة:");
            Label lCapacity = MakeLabel("السعة:");
            Label lGender = MakeLabel("الفئة:");
            Label lActive = MakeLabel("الحالة:");

            txtYear = new TextBox { Dock = DockStyle.Fill };
            cmbClass = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
            txtSectionName = new TextBox { Dock = DockStyle.Fill, MaxLength = 50 };
            nudCapacity = new NumericUpDown
            {
                Dock = DockStyle.Fill,
                Minimum = 1,
                Maximum = 500,
                Value = 30
            };
            cmbGender = new ComboBox
            {
                Dock = DockStyle.Fill,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbGender.Items.AddRange(new object[] { "مختلط", "ذكر", "أنثى" });
            chkActive = new CheckBox
            {
                Dock = DockStyle.Fill,
                Text = "الشعبة نشطة",
                Checked = true
            };

            fields.Controls.Add(lYear, 0, 0);
            fields.Controls.Add(txtYear, 1, 0);
            fields.Controls.Add(lClass, 2, 0);
            fields.Controls.Add(cmbClass, 3, 0);
            fields.Controls.Add(lName, 4, 0);
            fields.Controls.Add(txtSectionName, 5, 0);

            fields.Controls.Add(lCapacity, 0, 1);
            fields.Controls.Add(nudCapacity, 1, 1);
            fields.Controls.Add(lGender, 2, 1);
            fields.Controls.Add(cmbGender, 3, 1);
            fields.Controls.Add(lActive, 4, 1);
            fields.Controls.Add(chkActive, 5, 1);

            FlowLayoutPanel buttons = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 54,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(0, 8, 0, 0)
            };

            btnRefresh = MakeButton("تحديث", 110);
            btnClear = MakeButton("تفريغ", 110);
            btnDelete = MakeButton("حذف", 110);
            btnUpdate = MakeButton("تعديل", 110);
            btnAdd = MakeButton("إضافة", 110);

            buttons.Controls.Add(btnRefresh);
            buttons.Controls.Add(btnClear);
            buttons.Controls.Add(btnDelete);
            buttons.Controls.Add(btnUpdate);
            buttons.Controls.Add(btnAdd);

            Panel searchPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 44,
                BackColor = Color.White,
                Padding = new Padding(10, 7, 10, 7)
            };

            Label searchLabel = MakeLabel("بحث سريع:");
            searchLabel.Dock = DockStyle.Right;
            searchLabel.Width = 110;
            txtSearch = new TextBox { Dock = DockStyle.Fill };
            searchPanel.Controls.Add(txtSearch);
            searchPanel.Controls.Add(searchLabel);

            grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false
            };

            Panel bottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 28,
                BackColor = Color.FromArgb(240, 243, 247)
            };
            lblCount = new Label
            {
                Dock = DockStyle.Fill,
                Text = "عدد الشعب: 0",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Tahoma", 9F, FontStyle.Bold)
            };
            bottom.Controls.Add(lblCount);

            Controls.Add(grid);
            Controls.Add(bottom);
            Controls.Add(searchPanel);
            Controls.Add(buttons);
            Controls.Add(fields);
            Controls.Add(lblTitle);

            btnAdd.Click += btnAdd_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Click += btnDelete_Click;
            btnClear.Click += btnClear_Click;
            btnRefresh.Click += btnRefresh_Click;
            txtSearch.TextChanged += txtSearch_TextChanged;
            grid.CellClick += grid_CellClick;
        }

        private static Label MakeLabel(string text)
        {
            return new Label
            {
                Text = text,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Tahoma", 9.5F, FontStyle.Bold)
            };
        }

        private static Button MakeButton(string text, int width)
        {
            return new Button
            {
                Text = text,
                Width = width,
                Height = 36,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Tahoma", 9F, FontStyle.Bold),
                ForeColor = Color.White
            };
        }

        private void ApplyCustomStyles()
        {
            UIHelper.StyleTextBox(txtYear);
            UIHelper.StyleTextBox(txtSectionName);
            UIHelper.StyleTextBox(txtSearch);
            UIHelper.StyleComboBox(cmbClass);
            UIHelper.StyleComboBox(cmbGender);
            UIHelper.StyleDataGridView(grid);
            UIHelper.StylePrimaryButton(btnAdd);
            UIHelper.StylePrimaryButton(btnUpdate);
            UIHelper.StyleDangerButton(btnDelete);
            UIHelper.StyleButton(btnClear, UIHelper.NeutralColor);
            UIHelper.StyleButton(btnRefresh, UIHelper.NeutralColor);
            lblCount.ForeColor = UIHelper.MutedTextColor;
        }

        private async void SchoolSectionsForm_Load(object sender, EventArgs e)
        {
            try
            {
                CurrentUser.DemandAny(
                    "ليس لديك صلاحية عرض الشعب الدراسية.",
                    "Sections.View",
                    "Sections.Manage");

                txtYear.Text = GetCurrentYear();
                cmbGender.SelectedIndex = 0;
                await LoadClassesAsync();
                await LoadSectionsAsync();
                ClearFields();
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("تحميل واجهة الشعب الدراسية", ex);
            }
        }

        private string GetCurrentYear()
        {
            string year = AcademicYearHelper.GetCurrentAcademicYear();
            if (!string.IsNullOrWhiteSpace(year))
                return year.Replace('-', '/');

            int start = DateTime.Today.Month >= 8 ? DateTime.Today.Year : DateTime.Today.Year - 1;
            return start + "/" + (start + 1);
        }

        private async Task LoadClassesAsync()
        {
            DataTable classes = await Task.Run(() => classService.GetAllClasses());
            cmbClass.DataSource = classes;
            cmbClass.DisplayMember = "ClassName";
            cmbClass.ValueMember = "ClassID";
            if (classes.Rows.Count > 0)
                cmbClass.SelectedIndex = 0;
        }

        private async Task LoadSectionsAsync()
        {
            ValidateYearOrThrow(txtYear.Text);
            sectionsTable = await Task.Run(() => sectionService.GetAllSections(txtYear.Text.Trim()));
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            if (sectionsTable == null)
                return;

            string keyword = txtSearch.Text.Trim();
            string filter = UIHelper.BuildDataViewSearchFilter(
                keyword,
                "ClassName",
                "SectionName",
                "AcademicYear",
                "AllowedGender");

            int classId;
            if (cmbClass.SelectedValue != null &&
                int.TryParse(cmbClass.SelectedValue.ToString(), out classId) &&
                classId > 0)
            {
                string classFilter = "ClassID = " + classId;
                filter = string.IsNullOrWhiteSpace(filter)
                    ? classFilter
                    : "(" + filter + ") AND " + classFilter;
            }

            sectionsTable.DefaultView.RowFilter = filter;
            grid.DataSource = sectionsTable.DefaultView;
            FormatGrid();
            lblCount.Text = "عدد الشعب: " + sectionsTable.DefaultView.Count;
        }

        private void FormatGrid()
        {
            SetHeader("SectionID", "الرقم");
            SetHeader("ClassName", "الصف");
            SetHeader("AcademicYear", "العام الدراسي");
            SetHeader("SectionName", "الشعبة");
            SetHeader("Capacity", "السعة");
            SetHeader("AssignedCount", "الطلاب");
            SetHeader("AllowedGender", "الفئة");
            SetHeader("IsActive", "نشطة");
            SetHeader("CreatedAt", "تاريخ الإنشاء");
            if (grid.Columns.Contains("ClassID"))
                grid.Columns["ClassID"].Visible = false;
        }

        private void SetHeader(string column, string title)
        {
            if (grid.Columns.Contains(column))
                grid.Columns[column].HeaderText = title;
        }

        private int GetSelectedClassId()
        {
            int value;
            return cmbClass.SelectedValue != null && !(cmbClass.SelectedValue is DataRowView) &&
                   int.TryParse(cmbClass.SelectedValue.ToString(), out value) ? value : 0;
        }

        private SchoolSection BuildModel()
        {
            SchoolSection item = new SchoolSection
            {
                SectionID = selectedSectionId,
                ClassID = GetSelectedClassId(),
                SectionName = txtSectionName.Text.Trim(),
                AcademicYear = txtYear.Text.Trim(),
                Capacity = Convert.ToInt32(nudCapacity.Value),
                AllowedGender = cmbGender.Text.Trim(),
                IsActive = chkActive.Checked
            };
            return item;
        }

        private void ValidateYearOrThrow(string year)
        {
            string normalized = (year ?? string.Empty).Trim().Replace('-', '/');
            string[] p = normalized.Split('/');
            int y1;
            int y2;
            if (p.Length != 2 || p[0].Length != 4 || p[1].Length != 4 ||
                !int.TryParse(p[0], out y1) || !int.TryParse(p[1], out y2) ||
                y2 != y1 + 1)
                throw new ArgumentException("العام الدراسي يجب أن يكون بصيغة متسلسلة مثل 2026/2027.");
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                SchoolSection item = BuildModel();
                ValidateYearOrThrow(item.AcademicYear);
                int id = await Task.Run(() => sectionService.AddSection(item));
                UIHelper.ShowInfo("تمت إضافة الشعبة بنجاح. الرقم: " + id);
                await LoadSectionsAsync();
                ClearFields();
            }
            catch (ArgumentException ex)
            {
                UIHelper.ShowWarning(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                UIHelper.ShowWarning(ex.Message);
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("إضافة الشعبة", ex);
            }
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedSectionId <= 0)
                {
                    UIHelper.ShowWarning("اختر شعبة من الجدول أولاً.");
                    return;
                }

                SchoolSection item = BuildModel();
                ValidateYearOrThrow(item.AcademicYear);
                bool updated = await Task.Run(() => sectionService.UpdateSection(item));
                if (updated)
                {
                    UIHelper.ShowInfo("تم تعديل الشعبة بنجاح.");
                    await LoadSectionsAsync();
                    ClearFields();
                }
            }
            catch (ArgumentException ex)
            {
                UIHelper.ShowWarning(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                UIHelper.ShowWarning(ex.Message);
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("تعديل الشعبة", ex);
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentUser.DemandAction("Sections", "Delete", "ليس لديك صلاحية حذف الشعب الدراسية.");

                if (selectedSectionId <= 0)
                {
                    UIHelper.ShowWarning("اختر شعبة من الجدول أولاً.");
                    return;
                }

                DialogResult answer = MessageBox.Show(
                    "لن يتم السماح بالحذف إذا كانت الشعبة مرتبطة بطلاب أو حضور أو درجات أو جدول. هل تريد المتابعة؟",
                    "تأكيد الحذف",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2,
                    MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);

                if (answer != DialogResult.Yes)
                    return;

                bool deleted = await Task.Run(() => sectionService.DeleteSection(selectedSectionId));
                if (deleted)
                {
                    UIHelper.ShowInfo("تم حذف الشعبة بنجاح.");
                    await LoadSectionsAsync();
                    ClearFields();
                }
            }
            catch (ArgumentException ex)
            {
                UIHelper.ShowWarning(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                UIHelper.ShowWarning(ex.Message);
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("حذف الشعبة", ex);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                await LoadClassesAsync();
                await LoadSectionsAsync();
                ClearFields();
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("تحديث الشعب الدراسية", ex);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataRowView row = grid.Rows[e.RowIndex].DataBoundItem as DataRowView;
            if (row == null)
                return;

            selectedSectionId = Convert.ToInt32(row["SectionID"]);
            txtYear.Text = Convert.ToString(row["AcademicYear"]);
            SelectClass(Convert.ToInt32(row["ClassID"]));
            txtSectionName.Text = Convert.ToString(row["SectionName"]);
            int capacity = row["Capacity"] == DBNull.Value ? 30 : Convert.ToInt32(row["Capacity"]);
            nudCapacity.Value = Math.Max(nudCapacity.Minimum, Math.Min(nudCapacity.Maximum, capacity));
            SelectGender(Convert.ToString(row["AllowedGender"]));
            chkActive.Checked = row["IsActive"] != DBNull.Value && Convert.ToBoolean(row["IsActive"]);
        }

        private void SelectClass(int classId)
        {
            if (cmbClass.DataSource == null)
                return;
            for (int i = 0; i < cmbClass.Items.Count; i++)
            {
                DataRowView row = cmbClass.Items[i] as DataRowView;
                if (row != null && row.Row.Table.Columns.Contains("ClassID") &&
                    Convert.ToInt32(row["ClassID"]) == classId)
                {
                    cmbClass.SelectedIndex = i;
                    return;
                }
            }
        }

        private void SelectGender(string gender)
        {
            string value = string.IsNullOrWhiteSpace(gender) ? "مختلط" : gender.Trim();
            for (int i = 0; i < cmbGender.Items.Count; i++)
            {
                if (string.Equals(cmbGender.Items[i].ToString(), value, StringComparison.OrdinalIgnoreCase))
                {
                    cmbGender.SelectedIndex = i;
                    return;
                }
            }
            cmbGender.SelectedIndex = 0;
        }

        private void ClearFields()
        {
            selectedSectionId = 0;
            txtSectionName.Clear();
            nudCapacity.Value = 30;
            chkActive.Checked = true;

            if (cmbGender.Items.Count > 0)
                cmbGender.SelectedIndex = 0;
            if (cmbClass.Items.Count > 0)
                cmbClass.SelectedIndex = 0;

            if (string.IsNullOrWhiteSpace(txtYear.Text))
                txtYear.Text = GetCurrentYear();
        }
    }
}