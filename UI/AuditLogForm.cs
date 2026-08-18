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
        private bool filtersReady;
        private bool filterLoadInProgress;

        public AuditLogForm()
        {
            InitializeComponent();
            UIHelper.ApplyStyle(this);
            BackColor = UIHelper.BackgroundColor;
            RightToLeft = RightToLeft.Yes;
            fromDate.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            toDate.Value = DateTime.Today;
            searchBox.AccessibleName = "بحث في سجل الأنشطة";
            UIHelper.StyleTextBox(searchBox);
            UIHelper.StylePrimaryButton(refreshButton);
            UIHelper.StyleButton(exportButton, UIHelper.SuccessColor);
            UIHelper.StyleDataGridView(grid);
            userFilter.SelectedIndexChanged += Filter_SelectedIndexChanged;
            actionFilter.SelectedIndexChanged += Filter_SelectedIndexChanged;
            entityFilter.SelectedIndexChanged += Filter_SelectedIndexChanged;
            searchBox.TextChanged += SearchBox_TextChanged;
        }

        private async void AuditLogForm_Load(object sender, EventArgs e)
        {
            await LoadFilterOptionsAsync();
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

                SetBusyState(true);
                DataTable data = await Task.Run(() => service.GetRecent(
                    fromDate.Value.Date,
                    toDate.Value.Date,
                    searchBox.Text,
                    GetSelectedFilterValue(userFilter),
                    GetSelectedFilterValue(actionFilter),
                    GetSelectedFilterValue(entityFilter)));
                currentData = data ?? new DataTable();
                grid.DataSource = currentData;
                SetHeader("AuditLogID", "الرقم");
                SetHeader("CreatedAt", "التاريخ والوقت");
                SetHeader("UserName", "المستخدم");
                SetHeader("ActionName", "العملية");
                SetHeader("EntityName", "الكيان");
                SetHeader("EntityID", "رقم السجل");
                SetHeader("Details", "التفاصيل");
                SetColumnWidths();
                    countLabel.Text = "عدد العمليات: " + currentData.Rows.Count;
                    rangeLabel.Text = "الفترة: " + fromDate.Value.ToString("yyyy-MM-dd") + " إلى " + toDate.Value.ToString("yyyy-MM-dd");
                    statusLabel.Text = currentData.Rows.Count == 0 ? "لا توجد عمليات ضمن الفترة المحددة" : "تم تحميل السجل بنجاح";
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
                SetBusyState(false);
            }
        }

        private void SetBusyState(bool busy)
        {
            Cursor = busy ? Cursors.WaitCursor : Cursors.Default;
            fromDate.Enabled = !busy;
            toDate.Enabled = !busy;
            searchBox.Enabled = !busy;
            userFilter.Enabled = !busy && filtersReady;
            actionFilter.Enabled = !busy && filtersReady;
            entityFilter.Enabled = !busy && filtersReady;
            refreshButton.Enabled = !busy;
            exportButton.Enabled = !busy && currentData != null && currentData.Rows.Count > 0;
            if (busy)
                statusLabel.Text = "جارٍ تحميل سجل الأنشطة...";
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

        private async void Filter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!filtersReady || filterLoadInProgress || !IsHandleCreated)
                return;

            await LoadLogsAsync();
        }

        private async Task LoadFilterOptionsAsync()
        {
            try
            {
                filterLoadInProgress = true;
                SetFilterLoadingState(true);
                await LoadFilterValuesAsync(userFilter, "UserName");
                await LoadFilterValuesAsync(actionFilter, "ActionName");
                await LoadFilterValuesAsync(entityFilter, "EntityName");
                filtersReady = true;
            }
            catch (UnauthorizedAccessException ex)
            {
                UIHelper.ShowWarning(ex.Message);
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("تعذر تحميل فلاتر سجل الأنشطة:\n", ex);
            }
            finally
            {
                filterLoadInProgress = false;
                SetFilterLoadingState(false);
            }
        }

        private async Task LoadFilterValuesAsync(ComboBox combo, string filterName)
        {
            DataTable values = await Task.Run(() => service.GetFilterValues(filterName));
            combo.Items.Clear();
            combo.Items.Add("الكل");
            if (values != null)
            {
                foreach (DataRow row in values.Rows)
                {
                    string value = Convert.ToString(row["Value"]);
                    if (!string.IsNullOrWhiteSpace(value))
                        combo.Items.Add(value);
                }
            }
            combo.SelectedIndex = 0;
        }

        private static string GetSelectedFilterValue(ComboBox combo)
        {
            if (combo == null || combo.SelectedIndex <= 0 || combo.SelectedItem == null)
                return string.Empty;
            return combo.SelectedItem.ToString().Trim();
        }

        private void SetFilterLoadingState(bool loading)
        {
            userFilter.Enabled = !loading;
            actionFilter.Enabled = !loading;
            entityFilter.Enabled = !loading;
        }

        private async void SearchBox_TextChanged(object sender, EventArgs e)
        {
            if (!filtersReady || filterLoadInProgress || !IsHandleCreated || !searchBox.Enabled)
                return;

            await LoadLogsAsync();
        }

        private void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            // البحث فوري مع كل تغيير، لذا يمنع Enter من إطلاق تحميل إضافي مكرر.
            if (e.KeyCode == Keys.Enter)
                e.SuppressKeyPress = true;
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
            try
            {
                CurrentUser.DemandPermission(PermissionKeys.AuditLogsView, "ليس لديك صلاحية تصدير سجل الأنشطة.");
            }
            catch (UnauthorizedAccessException ex)
            {
                UIHelper.ShowWarning(ex.Message);
                return;
            }

            if (currentData == null || currentData.Rows.Count == 0)
            {
                UIHelper.ShowWarning("لا توجد بيانات لتصديرها.");
                return;
            }

            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "Excel أو PDF (*.xlsx;*.pdf)|*.xlsx;*.pdf|Excel (*.xlsx)|*.xlsx|PDF (*.pdf)|*.pdf";
                dialog.FileName = "Audit_Log_" + DateTime.Now.ToString("yyyyMMdd-HHmm") + ".xlsx";
                if (dialog.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    DataTable exportTable = new DataTable();
                    foreach (DataColumn sourceColumn in currentData.Columns)
                        exportTable.Columns.Add(GetExportHeader(sourceColumn.ColumnName));
                    foreach (DataRow sourceRow in currentData.Rows)
                    {
                        DataRow targetRow = exportTable.NewRow();
                        for (int columnIndex = 0; columnIndex < currentData.Columns.Count; columnIndex++)
                        {
                            object value = sourceRow[columnIndex];
                            targetRow[columnIndex] = value == DBNull.Value ? string.Empty :
                                (currentData.Columns[columnIndex].ColumnName == "CreatedAt"
                                    ? Convert.ToDateTime(value).ToString("yyyy-MM-dd HH:mm")
                                    : value.ToString());
                        }
                        exportTable.Rows.Add(targetRow);
                    }
                    string extension = Path.GetExtension(dialog.FileName).ToLowerInvariant();
                    if (extension == ".pdf")
                    {
                        CurrentUser.DemandPermission(PermissionKeys.AuditLogsExportPDF, "ليس لديك صلاحية تصدير سجل الأنشطة إلى PDF.");
                    }
                    else
                    {
                        CurrentUser.DemandPermission(PermissionKeys.AuditLogsExportExcel, "ليس لديك صلاحية تصدير سجل الأنشطة إلى Excel.");
                    }

                    if (extension == ".pdf")
                    {
                        ReportOutputHelper.ExportToPdf(exportTable, dialog.FileName,
                            "سجل الأنشطة | Audit Log", "عدد السجلات | Records: " + exportTable.Rows.Count);
                    }
                    else
                    {
                        ReportOutputHelper.ExportToExcel(exportTable, dialog.FileName,
                            "سجل الأنشطة | Audit Log", "عدد السجلات | Records: " + exportTable.Rows.Count);
                    }
                    statusLabel.Text = "تم تصدير السجل بنجاح";
                    UIHelper.ShowInfo("تم تصدير سجل الأنشطة بنجاح.");
                }
                catch (Exception ex)
                {
                    UIHelper.ShowException("تعذر تصدير سجل الأنشطة:\n", ex);
                }
            }
        }

        private string GetExportHeader(string columnName)
        {
            switch (columnName)
            {
                case "AuditLogID": return "الرقم";
                case "CreatedAt": return "التاريخ والوقت";
                case "UserName": return "المستخدم";
                case "ActionName": return "العملية";
                case "EntityName": return "الكيان";
                case "EntityID": return "رقم السجل";
                case "Details": return "التفاصيل";
                default: return columnName;
            }
        }
    }
}
