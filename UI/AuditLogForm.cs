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
    // تعريف الصنف AuditLogForm المسؤول عن وظائف طبقة UI.
    public partial class AuditLogForm : UserControl
    {
        // تعريف الدالة AuditLogService لتنفيذ العملية المرتبطة بها.
        private readonly AuditLogService service = new AuditLogService();

        private DataTable currentData;

        private bool filtersReady;

        private bool filterLoadInProgress;

        // تعريف الدالة AuditLogForm لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة AuditLogForm_Load لتنفيذ العملية المرتبطة بها.
        private async void AuditLogForm_Load(object sender, EventArgs e)
        {

            await LoadFilterOptionsAsync();

            await LoadLogsAsync();
        }

        // تعريف الدالة RefreshButton_Click لتنفيذ العملية المرتبطة بها.
        private async void RefreshButton_Click(object sender, EventArgs e)
        {

            await LoadLogsAsync();
        }

        // تعريف الدالة LoadLogsAsync لتنفيذ العملية المرتبطة بها.
        private async Task LoadLogsAsync()
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                CurrentUser.DemandPermission(PermissionKeys.AuditLogsView, "ليس لديك صلاحية عرض سجل الأنشطة.");
                if (fromDate.Value.Date > toDate.Value.Date)
                {
                    UIHelper.ShowWarning("تاريخ البداية يجب أن يكون قبل تاريخ النهاية أو مساويًا له.");

                    return;
                }

                SetBusyState(true);

                DateTime startDate = fromDate.Value.Date;
                DateTime endDate = toDate.Value.Date;
                string searchText = searchBox.Text;
                string userValue = GetSelectedFilterValue(userFilter);
                string actionValue = GetSelectedFilterValue(actionFilter);
                string entityValue = GetSelectedFilterValue(entityFilter);

                DataTable data = await Task.Run(() => service.GetRecent(
                    startDate,
                    endDate,
                    searchText,
                    userValue,
                    actionValue,
                    entityValue));
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
                ApplicationLogger.LogWarning("سجل الأنشطة", "صلاحية غير كافية لتحميل السجل: " + ex.Message);
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

        // تعريف الدالة SetBusyState لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة SetHeader لتنفيذ العملية المرتبطة بها.
        private void SetHeader(string name, string header)
        {
            if (grid.Columns.Contains(name))
                grid.Columns[name].HeaderText = header;
        }

        // تعريف الدالة SetColumnWidths لتنفيذ العملية المرتبطة بها.
        private void SetColumnWidths()
        {
            if (grid.Columns.Contains("Details"))
                grid.Columns["Details"].FillWeight = 220;
            if (grid.Columns.Contains("CreatedAt"))
                grid.Columns["CreatedAt"].FillWeight = 95;
            if (grid.Columns.Contains("AuditLogID"))
                grid.Columns["AuditLogID"].FillWeight = 45;
        }

        // تعريف الدالة Filter_SelectedIndexChanged لتنفيذ العملية المرتبطة بها.
        private async void Filter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!filtersReady || filterLoadInProgress || !IsHandleCreated)
                return;


            await LoadLogsAsync();
        }

        // تعريف الدالة LoadFilterOptionsAsync لتنفيذ العملية المرتبطة بها.
        private async Task LoadFilterOptionsAsync()
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
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
                ApplicationLogger.LogWarning("سجل الأنشطة", "صلاحية غير كافية لتحميل فلاتر السجل: " + ex.Message);
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

        // تعريف الدالة LoadFilterValuesAsync لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة GetSelectedFilterValue لتنفيذ العملية المرتبطة بها.
        private static string GetSelectedFilterValue(ComboBox combo)
        {
            if (combo == null || combo.SelectedIndex <= 0 || combo.SelectedItem == null)
                return string.Empty;

            return combo.SelectedItem.ToString().Trim();
        }

        // تعريف الدالة SetFilterLoadingState لتنفيذ العملية المرتبطة بها.
        private void SetFilterLoadingState(bool loading)
        {
            userFilter.Enabled = !loading;
            actionFilter.Enabled = !loading;
            entityFilter.Enabled = !loading;
        }

        // تعريف الدالة SearchBox_TextChanged لتنفيذ العملية المرتبطة بها.
        private void SearchBox_TextChanged(object sender, EventArgs e)
        {
            if (!filtersReady || filterLoadInProgress || !IsHandleCreated || !searchBox.Enabled)
                return;

            // تنقيط البحث (debounce) حتى لا يُعاد تحميل السجل مع كل ضغطة مفتاح.
            searchDebounceTimer.Stop();
            searchDebounceTimer.Start();
        }

        // تعريف الدالة SearchDebounceTimer_Tick لتنفيذ العملية المرتبطة بها.
        private async void SearchDebounceTimer_Tick(object sender, EventArgs e)
        {
            searchDebounceTimer.Stop();

            if (!filtersReady || filterLoadInProgress || !IsHandleCreated || !searchBox.Enabled)
                return;

            await LoadLogsAsync();
        }

        // تعريف الدالة SearchBox_KeyDown لتنفيذ العملية المرتبطة بها.
        private void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
                e.SuppressKeyPress = true;
        }

        // تعريف الدالة Grid_DataBindingComplete لتنفيذ العملية المرتبطة بها.
        private void Grid_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            SetColumnWidths();
        }

        // تعريف الدالة Grid_CellFormatting لتنفيذ العملية المرتبطة بها.
        private void Grid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            if (grid.Columns[e.ColumnIndex].Name == "CreatedAt" && e.Value != null && e.Value != DBNull.Value)
                e.Value = Convert.ToDateTime(e.Value).ToString("yyyy-MM-dd HH:mm");
        }

        // تعريف الدالة ExportButton_Click لتنفيذ العملية المرتبطة بها.
        private void ExportButton_Click(object sender, EventArgs e)
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                CurrentUser.DemandPermission(PermissionKeys.AuditLogsView, "ليس لديك صلاحية تصدير سجل الأنشطة.");
            }

            catch (UnauthorizedAccessException ex)
            {
                ApplicationLogger.LogWarning("سجل الأنشطة", "صلاحية غير كافية للتصدير: " + ex.Message);
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
                dialog.FileName = "سجل_الأنشطة_" + DateTime.Now.ToString("yyyyMMdd-HHmm") + ".xlsx";
                if (dialog.ShowDialog() != DialogResult.OK)
                    return;

                // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
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
                        // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                        CurrentUser.DemandModule("AuditLogs", "ليس لديك صلاحية تصدير سجل الأنشطة إلى PDF.");
                    }

                    else
                    {
                        // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                        CurrentUser.DemandModule("AuditLogs", "ليس لديك صلاحية تصدير سجل الأنشطة إلى Excel.");
                    }

                    if (extension == ".pdf")
                    {
                        ReportOutputHelper.ExportToPdf(exportTable, dialog.FileName,
                            "سجل الأنشطة", "عدد السجلات: " + exportTable.Rows.Count);
                    }

                    else
                    {
                        ReportOutputHelper.ExportToExcel(exportTable, dialog.FileName,
                            "سجل الأنشطة", "عدد السجلات: " + exportTable.Rows.Count);
                    }
                    statusLabel.Text = "تم تصدير السجل بنجاح";
                    UIHelper.ShowInfo("تم تصدير سجل الأنشطة بنجاح.");
                    ApplicationLogger.LogUsage("سجل الأنشطة", "تصدير", "تم تصدير السجل إلى " + dialog.FileName);
                }

                catch (Exception ex)
                {
                    UIHelper.ShowException("تعذر تصدير سجل الأنشطة:\n", ex);
                }
            }
        }

        // تعريف الدالة GetExportHeader لتنفيذ العملية المرتبطة بها.
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
