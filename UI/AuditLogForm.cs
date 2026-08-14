using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SchoolSystem.Helpers;
using SchoolSystem.Security;
using SchoolSystem.Services;

namespace SchoolSystem.UI
{
    public class AuditLogForm : UserControl
    {
        private readonly AuditLogService service = new AuditLogService();
        private readonly DateTimePicker fromDate = new DateTimePicker();
        private readonly DateTimePicker toDate = new DateTimePicker();
        private readonly TextBox searchBox = new TextBox();
        private readonly Button refreshButton = new Button();
        private readonly Button exportButton = new Button();
        private readonly DataGridView grid = new DataGridView();
        private readonly Label countLabel = new Label();
        private readonly Label rangeLabel = new Label();
        private DataTable currentData;

        public AuditLogForm()
        {
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            Dock = DockStyle.Fill;
            UIHelper.ApplyStyle(this);
            BackColor = UIHelper.BackgroundColor;
            BuildLayout();
            Load += AuditLogForm_Load;
        }

        private void BuildLayout()
        {
            Panel header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 76,
                Padding = new Padding(14, 8, 14, 6),
                BackColor = UIHelper.SurfaceColor
            };

            Label title = new Label
            {
                Text = "سجل الأنشطة والعمليات الحساسة",
                Dock = DockStyle.Top,
                Height = 34,
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font(UIHelper.FontFamily, UIHelper.TitleFontSize, FontStyle.Bold),
                ForeColor = UIHelper.TextColor
            };

            Label subtitle = new Label
            {
                Text = "مراجعة موثقة لتغييرات السندات والدرجات والرسوم والمستخدمين",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleRight,
                ForeColor = UIHelper.MutedTextColor,
                Font = new Font(UIHelper.FontFamily, UIHelper.SmallFontSize, FontStyle.Regular)
            };

            header.Controls.Add(subtitle);
            header.Controls.Add(title);

            Panel filters = new Panel
            {
                Dock = DockStyle.Top,
                Height = 74,
                Padding = new Padding(14, 8, 14, 8),
                BackColor = UIHelper.BackgroundColor
            };

            Label searchLabel = CreateFilterLabel("بحث:");
            Label toLabel = CreateFilterLabel("إلى:");
            Label fromLabel = CreateFilterLabel("من:");

            searchBox.Width = 220;
            fromDate.Width = 125;
            toDate.Width = 125;
            fromDate.Format = DateTimePickerFormat.Short;
            toDate.Format = DateTimePickerFormat.Short;
            fromDate.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            toDate.Value = DateTime.Today;
            searchBox.AccessibleName = "بحث في سجل الأنشطة";

            refreshButton.Text = "تحديث";
            refreshButton.Width = 96;
            refreshButton.Height = 34;
            refreshButton.Click += async (sender, e) => await LoadLogsAsync();

            exportButton.Text = "تصدير CSV";
            exportButton.Width = 110;
            exportButton.Height = 34;
            exportButton.Click += exportButton_Click;

            UIHelper.StyleTextBox(searchBox);
            UIHelper.StylePrimaryButton(refreshButton);
            UIHelper.StyleButton(exportButton, UIHelper.SuccessColor);

            filters.Controls.Add(exportButton);
            filters.Controls.Add(refreshButton);
            filters.Controls.Add(searchBox);
            filters.Controls.Add(searchLabel);
            filters.Controls.Add(toDate);
            filters.Controls.Add(toLabel);
            filters.Controls.Add(fromDate);
            filters.Controls.Add(fromLabel);

            exportButton.Dock = DockStyle.Left;
            refreshButton.Dock = DockStyle.Left;
            searchBox.Dock = DockStyle.Right;
            searchLabel.Dock = DockStyle.Right;
            toDate.Dock = DockStyle.Right;
            toLabel.Dock = DockStyle.Right;
            fromDate.Dock = DockStyle.Right;
            fromLabel.Dock = DockStyle.Right;

            exportButton.Margin = new Padding(4);
            refreshButton.Margin = new Padding(4);
            searchBox.Margin = new Padding(4);
            fromDate.Margin = new Padding(4);
            toDate.Margin = new Padding(4);

            UIHelper.StyleDataGridView(grid);
            grid.Dock = DockStyle.Fill;
            grid.ReadOnly = true;
            grid.AutoGenerateColumns = true;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.MultiSelect = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.RowHeadersVisible = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.CellFormatting += Grid_CellFormatting;
            grid.DataBindingComplete += Grid_DataBindingComplete;

            Panel footer = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 38,
                BackColor = UIHelper.SurfaceColor,
                Padding = new Padding(14, 0, 14, 0)
            };

            countLabel.Text = "عدد العمليات: 0";
            countLabel.Dock = DockStyle.Right;
            countLabel.Width = 180;
            countLabel.TextAlign = ContentAlignment.MiddleRight;
            countLabel.ForeColor = UIHelper.TextColor;

            rangeLabel.Text = "الفترة: —";
            rangeLabel.Dock = DockStyle.Left;
            rangeLabel.Width = 260;
            rangeLabel.TextAlign = ContentAlignment.MiddleLeft;
            rangeLabel.ForeColor = UIHelper.MutedTextColor;

            footer.Controls.Add(countLabel);
            footer.Controls.Add(rangeLabel);

            Controls.Add(grid);
            Controls.Add(footer);
            Controls.Add(filters);
            Controls.Add(header);
        }

        private Label CreateFilterLabel(string text)
        {
            return new Label
            {
                Text = text,
                Width = 42,
                Height = 34,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = UIHelper.TextColor,
                Font = new Font(UIHelper.FontFamily, UIHelper.SmallFontSize, FontStyle.Bold),
                Margin = new Padding(4)
            };
        }

        private async void AuditLogForm_Load(object sender, EventArgs e)
        {
            await LoadLogsAsync();
        }

        private async Task LoadLogsAsync()
        {
            try
            {
                CurrentUser.DemandPermission(PermissionKeys.AuditLogsView, "ليس لديك صلاحية عرض سجل الأنشطة.");
                if (fromDate.Value.Date > toDate.Value.Date)
                {
                    UIHelper.ShowWarning("تاريخ البداية يجب أن يكون قبل تاريخ النهاية أو مساويًا له.");
                    return;
                }

                Cursor = Cursors.WaitCursor;
                DataTable data = await Task.Run(() => service.GetRecent(fromDate.Value.Date, toDate.Value.Date, searchBox.Text));
                currentData = data;
                grid.DataSource = data;
                SetHeader("AuditLogID", "الرقم");
                SetHeader("CreatedAt", "التاريخ والوقت");
                SetHeader("UserName", "المستخدم");
                SetHeader("ActionName", "العملية");
                SetHeader("EntityName", "الكيان");
                SetHeader("EntityID", "رقم السجل");
                SetHeader("Details", "التفاصيل");
                SetColumnWidths();
                countLabel.Text = "عدد العمليات: " + data.Rows.Count;
                rangeLabel.Text = "الفترة: " + fromDate.Value.ToString("yyyy-MM-dd") + " إلى " + toDate.Value.ToString("yyyy-MM-dd");
            }
            catch (UnauthorizedAccessException ex)
            {
                UIHelper.ShowWarning(ex.Message);
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("تعذر تحميل سجل الأنشطة:\n", ex);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void SetHeader(string name, string header)
        {
            if (grid.Columns.Contains(name))
                grid.Columns[name].HeaderText = header;
        }

        private void SetColumnWidths()
        {
            if (grid.Columns.Contains("Details"))
                grid.Columns["Details"].FillWeight = 220;
            if (grid.Columns.Contains("CreatedAt"))
                grid.Columns["CreatedAt"].FillWeight = 95;
            if (grid.Columns.Contains("AuditLogID"))
                grid.Columns["AuditLogID"].FillWeight = 45;
        }

        private void Grid_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            SetColumnWidths();
        }

        private void Grid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            if (grid.Columns[e.ColumnIndex].Name == "CreatedAt" && e.Value != null && e.Value != DBNull.Value)
                e.Value = Convert.ToDateTime(e.Value).ToString("yyyy-MM-dd HH:mm");
        }

        private void exportButton_Click(object sender, EventArgs e)
        {
            if (currentData == null || currentData.Rows.Count == 0)
            {
                UIHelper.ShowWarning("لا توجد بيانات لتصديرها.");
                return;
            }

            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "CSV UTF-8 (*.csv)|*.csv";
                dialog.FileName = "سجل-الأنشطة-" + DateTime.Now.ToString("yyyyMMdd-HHmm") + ".csv";
                if (dialog.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    StringBuilder csv = new StringBuilder();
                    for (int columnIndex = 0; columnIndex < currentData.Columns.Count; columnIndex++)
                    {
                        if (columnIndex > 0) csv.Append(",");
                        csv.Append(EscapeCsv(currentData.Columns[columnIndex].ColumnName));
                    }
                    csv.AppendLine();

                    foreach (DataRow row in currentData.Rows)
                    {
                        for (int columnIndex = 0; columnIndex < currentData.Columns.Count; columnIndex++)
                        {
                            if (columnIndex > 0) csv.Append(",");
                            object value = row[columnIndex];
                            string text = value == DBNull.Value ? string.Empty : value.ToString();
                            if (currentData.Columns[columnIndex].ColumnName == "CreatedAt" && value != DBNull.Value)
                                text = Convert.ToDateTime(value).ToString("yyyy-MM-dd HH:mm");
                            csv.Append(EscapeCsv(text));
                        }
                        csv.AppendLine();
                    }

                    File.WriteAllText(dialog.FileName, csv.ToString(), new UTF8Encoding(true));
                    UIHelper.ShowInfo("تم تصدير سجل الأنشطة بنجاح.");
                }
                catch (Exception ex)
                {
                    UIHelper.ShowException("تعذر تصدير سجل الأنشطة:\n", ex);
                }
            }
        }

        private string EscapeCsv(string value)
        {
            string safe = value ?? string.Empty;
            if (safe.Contains("\"") || safe.Contains(",") || safe.Contains("\r") || safe.Contains("\n"))
                return "\"" + safe.Replace("\"", "\"\"") + "\"";
            return safe;
        }
    }
}
