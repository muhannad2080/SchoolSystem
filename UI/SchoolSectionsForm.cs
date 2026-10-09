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

        private bool isRefreshingData;

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
            ConfigureSectionsGridAppearance();
            Dock = DockStyle.Fill;

            DataRefreshNotifier.DataChanged += DataRefreshNotifier_DataChanged;
            Disposed += (sender, e) =>
                DataRefreshNotifier.DataChanged -= DataRefreshNotifier_DataChanged;

            Load += SchoolSectionsForm_Load;
        }

        private void InitializeComponent()
        {
            BackColor = UIHelper.BackgroundColor;
            RightToLeft = RightToLeft.Yes;
            Font = new Font("Tahoma", 9.5F);
            Padding = new Padding(12);

            lblTitle = new Label
            {
                Dock = DockStyle.Top,
                Height = 60,
                Text = "إدارة الشعب الدراسية",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Tahoma", 15F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = UIHelper.PrimaryColor
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
                Height = 48,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(0, 4, 0, 4)
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
                Height = 50,
                BackColor = Color.White,
                Padding = new Padding(10, 6, 10, 6)
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
                RowHeadersVisible = false,
                AllowUserToResizeRows = false,
                EnableHeadersVisualStyles = false,
                ColumnHeadersHeight = 42,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                ColumnHeadersDefaultCellStyle =
                {
                    BackColor = UIHelper.PrimaryColor,
                    ForeColor = Color.White,
                    Font = new Font("Tahoma", 10F, FontStyle.Bold),
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                },
                DefaultCellStyle =
                {
                    BackColor = Color.White,
                    ForeColor = UIHelper.TextColor,
                    Font = new Font("Tahoma", 10F),
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    SelectionBackColor = Color.FromArgb(219, 234, 254),
                    SelectionForeColor = UIHelper.TextColor,
                    Padding = new Padding(6, 3, 6, 3)
                },
                AlternatingRowsDefaultCellStyle =
                {
                    BackColor = UIHelper.BackgroundColor
                },
                RowsDefaultCellStyle =
                {
                    BackColor = Color.White,
                    ForeColor = UIHelper.TextColor
                },
                GridColor = UIHelper.BorderColor,
                RowTemplate = { Height = 36 }
            };

            Panel bottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 28,
                BackColor = UIHelper.BackgroundColor
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
                Height = 40,
                MinimumSize = new Size(110, 40),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Tahoma", 10.5F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = UIHelper.AccentColor,
                FlatAppearance = { BorderSize = 0 },
                UseVisualStyleBackColor = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Cursor = Cursors.Hand,
                Margin = new Padding(4)
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

        private async void DataRefreshNotifier_DataChanged(
            object sender,
            DataRefreshEventArgs e)
        {
            if (IsDisposed || Disposing || !IsHandleCreated)
                return;

            if (InvokeRequired)
            {
                try
                {
                    BeginInvoke(new MethodInvoker(
                        () => DataRefreshNotifier_DataChanged(sender, e)));
                }
                catch (InvalidOperationException)
                {
                    // أُغلقت الشاشة أثناء جدولة التحديث.
                }

                return;
            }

            if (ReferenceEquals(sender, this) || isRefreshingData)
                return;

            if (!DataRefreshNotifier.IsRelated(e, "Sections") &&
                !DataRefreshNotifier.IsRelated(e, "Classes") &&
                !DataRefreshNotifier.IsRelated(e, "All"))
                return;

            try
            {
                isRefreshingData = true;
                await LoadClassesAsync();
                await LoadSectionsAsync();
            }
            catch (Exception ex)
            {
                ApplicationLogger.LogException("تحديث الشعب الدراسية تلقائياً", ex);
            }
            finally
            {
                isRefreshingData = false;
            }
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
            return AcademicYearHelper
                .GetCurrentAcademicYear()
                .Replace('-', '/');
        }

        private async Task LoadClassesAsync()
        {
            DataTable classes =
                await Task.Run(() => classService.GetAllClasses());

            DataTable choices = classes == null
                ? new DataTable()
                : classes.Copy();

            if (!choices.Columns.Contains("ClassID"))
                choices.Columns.Add("ClassID", typeof(int));

            if (!choices.Columns.Contains("ClassName"))
                choices.Columns.Add("ClassName", typeof(string));

            DataRow allRow = choices.NewRow();
            allRow["ClassID"] = 0;
            allRow["ClassName"] = "كل الصفوف";
            choices.Rows.InsertAt(allRow, 0);

            cmbClass.DataSource = choices;
            cmbClass.DisplayMember = "ClassName";
            cmbClass.ValueMember = "ClassID";
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

        // جدول الشعب قابل للقراءة والفرز، مع خطوط واضحة وتمرير أفقي دون ضغط الأعمدة.
        private void ConfigureSectionsGridAppearance()
        {
            if (grid == null)
                return;

            grid.SuspendLayout();
            try
            {
                grid.AllowUserToAddRows = false;
                grid.AllowUserToDeleteRows = false;
                grid.AllowUserToResizeRows = false;
                grid.AllowUserToResizeColumns = true;
                grid.AllowUserToOrderColumns = true;
                grid.ReadOnly = true;
                grid.MultiSelect = false;
                grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                grid.RowHeadersVisible = false;
                grid.RightToLeft = RightToLeft.Yes;
                grid.ScrollBars = ScrollBars.Both;
                grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
                grid.BackgroundColor = Color.White;
                grid.BorderStyle = BorderStyle.FixedSingle;
                grid.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                grid.EnableHeadersVisualStyles = false;
                grid.GridColor = UIHelper.BorderColor;
                grid.ColumnHeadersHeight = 44;
                grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                grid.RowTemplate.Height = 38;
                grid.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;

                grid.ColumnHeadersDefaultCellStyle.BackColor = UIHelper.PrimaryColor;
                grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                grid.ColumnHeadersDefaultCellStyle.Font = new Font(UIHelper.FontFamily, 10F, FontStyle.Bold);
                grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                grid.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
                grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = UIHelper.PrimaryColor;
                grid.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
                grid.DefaultCellStyle.BackColor = Color.White;
                grid.DefaultCellStyle.ForeColor = UIHelper.TextColor;
                grid.DefaultCellStyle.Font = new Font(UIHelper.FontFamily, 9.5F);
                grid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                grid.DefaultCellStyle.SelectionBackColor = UIHelper.AccentColor;
                grid.DefaultCellStyle.SelectionForeColor = Color.White;
                grid.DefaultCellStyle.Padding = new Padding(8, 4, 8, 4);
                grid.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
                grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(241, 245, 249);
                grid.AlternatingRowsDefaultCellStyle.ForeColor = UIHelper.TextColor;
                grid.AlternatingRowsDefaultCellStyle.SelectionBackColor = UIHelper.AccentColor;
                grid.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.White;
                grid.RowsDefaultCellStyle.BackColor = Color.White;
                grid.RowsDefaultCellStyle.ForeColor = UIHelper.TextColor;
                grid.RowsDefaultCellStyle.SelectionBackColor = UIHelper.AccentColor;
                grid.RowsDefaultCellStyle.SelectionForeColor = Color.White;
            }
            finally
            {
                grid.ResumeLayout();
            }
        }

        private void SetSectionColumnWidth(string name, int width)
        {
            if (!grid.Columns.Contains(name))
                return;

            DataGridViewColumn column = grid.Columns[name];
            if (!column.Visible)
                return;

            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            column.MinimumWidth = Math.Min(100, Math.Max(55, width));
            column.Width = width;
            column.Resizable = DataGridViewTriState.True;
            column.SortMode = DataGridViewColumnSortMode.Automatic;
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

            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            SetSectionColumnWidth("SectionID", 85);
            SetSectionColumnWidth("ClassName", 165);
            SetSectionColumnWidth("AcademicYear", 125);
            SetSectionColumnWidth("SectionName", 160);
            SetSectionColumnWidth("Capacity", 95);
            SetSectionColumnWidth("AssignedCount", 100);
            SetSectionColumnWidth("AllowedGender", 110);
            SetSectionColumnWidth("IsActive", 90);
            SetSectionColumnWidth("CreatedAt", 145);
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
                DataRefreshNotifier.Notify("Sections", this);
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
                    DataRefreshNotifier.Notify("Sections", this);
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

                if (!UIHelper.ShowConfirmation(
                    "لن يتم السماح بالحذف إذا كانت الشعبة مرتبطة بطلاب أو حضور أو درجات أو جدول. هل تريد المتابعة؟",
                    "تأكيد الحذف"))
                    return;

                bool deleted = await Task.Run(() => sectionService.DeleteSection(selectedSectionId));
                if (deleted)
                {
                    UIHelper.ShowInfo("تم حذف الشعبة بنجاح.");
                    await LoadSectionsAsync();
                    DataRefreshNotifier.Notify("Sections", this);
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