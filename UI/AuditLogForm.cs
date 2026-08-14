using System;
using System.Data;
using System.Drawing;
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
        private readonly DataGridView grid = new DataGridView();
        private readonly Label countLabel = new Label();

        public AuditLogForm()
        {
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            Dock = DockStyle.Fill;
            BackColor = UIHelper.BackgroundColor;
            BuildLayout();
            Load += AuditLogForm_Load;
        }

        private void BuildLayout()
        {
            Label title = new Label
            {
                Text = "سجل التدقيق والعمليات الحساسة",
                Dock = DockStyle.Top,
                Height = 52,
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font(UIHelper.FontFamily, UIHelper.TitleFontSize, FontStyle.Bold),
                ForeColor = UIHelper.TextColor,
                Padding = new Padding(12, 0, 12, 0)
            };

            Panel filters = new Panel { Dock = DockStyle.Top, Height = 62, Padding = new Padding(8) };
            fromDate.Format = DateTimePickerFormat.Short;
            toDate.Format = DateTimePickerFormat.Short;
            fromDate.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            toDate.Value = DateTime.Today;
            searchBox.Width = 220;
            refreshButton.Text = "تحديث";
            refreshButton.Width = 100;
            refreshButton.Click += async (sender, e) => await LoadLogsAsync();
            filters.Controls.Add(refreshButton);
            filters.Controls.Add(searchBox);
            filters.Controls.Add(toDate);
            filters.Controls.Add(fromDate);
            refreshButton.Dock = DockStyle.Right;
            searchBox.Dock = DockStyle.Right;
            toDate.Dock = DockStyle.Right;
            fromDate.Dock = DockStyle.Right;
            refreshButton.Margin = new Padding(4);
            UIHelper.StylePrimaryButton(refreshButton);
            UIHelper.StyleTextBox(searchBox);

            UIHelper.StyleDataGridView(grid);
            grid.Dock = DockStyle.Fill;
            grid.ReadOnly = true;
            grid.AutoGenerateColumns = true;
            grid.AllowUserToAddRows = false;
            grid.CellFormatting += Grid_CellFormatting;

            countLabel.Text = "عدد العمليات: 0";
            countLabel.Dock = DockStyle.Bottom;
            countLabel.Height = 34;
            countLabel.TextAlign = ContentAlignment.MiddleRight;
            countLabel.ForeColor = UIHelper.MutedTextColor;
            countLabel.Padding = new Padding(12, 0, 12, 0);

            Controls.Add(grid);
            Controls.Add(countLabel);
            Controls.Add(filters);
            Controls.Add(title);
        }

        private async void AuditLogForm_Load(object sender, EventArgs e)
        {
            await LoadLogsAsync();
        }

        private async Task LoadLogsAsync()
        {
            try
            {
                CurrentUser.DemandPermission(PermissionKeys.AuditLogsView, "ليس لديك صلاحية عرض سجل التدقيق.");
                Cursor = Cursors.WaitCursor;
                DataTable data = await Task.Run(() => service.GetRecent(fromDate.Value, toDate.Value, searchBox.Text));
                grid.DataSource = data;
                SetHeader("AuditLogID", "الرقم");
                SetHeader("CreatedAt", "التاريخ");
                SetHeader("UserName", "المستخدم");
                SetHeader("ActionName", "العملية");
                SetHeader("EntityName", "الكيان");
                SetHeader("EntityID", "رقم السجل");
                SetHeader("Details", "التفاصيل");
                countLabel.Text = "عدد العمليات: " + data.Rows.Count;
            }
            catch (UnauthorizedAccessException ex)
            {
                UIHelper.ShowWarning(ex.Message);
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("تحميل سجل التدقيق", ex);
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

        private void Grid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (grid.Columns[e.ColumnIndex].Name == "CreatedAt" && e.Value != null && e.Value != DBNull.Value)
                e.Value = Convert.ToDateTime(e.Value).ToString("yyyy-MM-dd HH:mm");
        }
    }
}
