using System;
using System.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SchoolSystem.Helpers;
using SchoolSystem.Security;
using SchoolSystem.Services;

namespace SchoolSystem.UI
{
    public partial class AuditLogForm : UserControl
    {
        private readonly AuditLogService service = new AuditLogService();
        private DataTable currentData;

        public AuditLogForm()
        {
            InitializeComponent();
            UIHelper.ApplyStyle(this);
            BackColor = UIHelper.BackgroundColor;
            fromDate.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            toDate.Value = DateTime.Today;
            searchBox.AccessibleName = "بحث في سجل الأنشطة";
            UIHelper.StyleTextBox(searchBox);
            UIHelper.StylePrimaryButton(refreshButton);
            UIHelper.StyleButton(exportButton, UIHelper.SuccessColor);
            UIHelper.StyleDataGridView(grid);
        }

        private async void AuditLogForm_Load(object sender, EventArgs e)
        {
            await LoadLogsAsync();
        }

        private async void RefreshButton_Click(object sender, EventArgs e)
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

        private void ExportButton_Click(object sender, EventArgs e)
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
