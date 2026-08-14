using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SchoolSystem.Models;
using SchoolSystem.Helpers;
using SchoolSystem.Services;

namespace SchoolSystem.UI
{
    public partial class TransportForm : UserControl
    {
        private readonly BusService busService = new BusService();
        private readonly BusRouteService routeService = new BusRouteService();

        private int selectedBusId = 0;
        private int selectedRouteId = 0;

        private DataTable allBuses;
        private DataTable allRoutes;

        private bool isLoading = false;

        public TransportForm()
        {
            InitializeComponent();
            SchoolSystem.Helpers.UIHelper.ApplyStyle(this);
            ApplyCustomStyles();
            Dock = DockStyle.Fill;

            Load += TransportForm_Load;
            tabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;
        }

        private void ApplyCustomStyles()
        {
            UIHelper.StyleDataGridView(dataGridViewBuses);
            UIHelper.StyleDataGridView(dataGridViewRoutes);
            UIHelper.StylePrimaryButton(btnAddBus);
            UIHelper.StylePrimaryButton(btnUpdateBus);
            UIHelper.StyleDangerButton(btnDeleteBus);
            UIHelper.StyleButton(btnClearBus, UIHelper.NeutralColor);
            UIHelper.StylePrimaryButton(btnAddRoute);
            UIHelper.StylePrimaryButton(btnUpdateRoute);
            UIHelper.StyleDangerButton(btnDeleteRoute);
            UIHelper.StyleButton(btnClearRoute, UIHelper.NeutralColor);
            UIHelper.StyleTextBox(txtBusNumber);
            UIHelper.StyleTextBox(txtDriverName);
            UIHelper.StyleTextBox(txtDriverPhone);
            UIHelper.StyleTextBox(txtCapacity);
            UIHelper.StyleTextBox(txtNotes);
            UIHelper.StyleTextBox(txtRouteName);
            UIHelper.StyleTextBox(txtStartPoint);
            UIHelper.StyleTextBox(txtEndPoint);
            UIHelper.StyleTextBox(txtFee);
            UIHelper.StyleTextBox(txtRouteNotes);
            UIHelper.StyleComboBox(cmbBus);
            lblRecordCount.ForeColor = UIHelper.MutedTextColor;
        }

        private async void TransportForm_Load(object sender, EventArgs e)
        {
            try
            {
                isLoading = true;

                dtpDeparture.CustomFormat = "HH:mm";
                dtpDeparture.Format = DateTimePickerFormat.Custom;
                dtpDeparture.ShowUpDown = true;

                dtpArrival.CustomFormat = "HH:mm";
                dtpArrival.Format = DateTimePickerFormat.Custom;
                dtpArrival.ShowUpDown = true;

                dtpDeparture.Value = DateTime.Today.AddHours(7);
                dtpArrival.Value = DateTime.Today.AddHours(8);

                await LoadBusesAsync();
                await LoadRoutesAsync();

                ClearBusInputs();
                ClearRouteInputs();

                tabControl.SelectedTab = tabBuses;
                lblRecordCount.Text = "عدد الحافلات: " + (allBuses == null ? 0 : allBuses.Rows.Count);
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("تحميل شاشة النقل", ex);
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

            if (tabControl.SelectedTab == tabBuses)
            {
                await LoadBusesAsync();
                lblRecordCount.Text = "عدد الحافلات: " + (allBuses == null ? 0 : allBuses.Rows.Count);
            }
            else if (tabControl.SelectedTab == tabRoutes)
            {
                await LoadRoutesAsync();
                lblRecordCount.Text = "عدد المسارات: " + (allRoutes == null ? 0 : allRoutes.Rows.Count);
            }
        }

        // =========================
        // الحافلات
        // =========================

        private async Task LoadBusesAsync()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                allBuses = await Task.Run(() => busService.GetAllBuses());

                dataGridViewBuses.DataSource = allBuses;

                FormatBusesGrid();
                LoadBusCombo();
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void FormatBusesGrid()
        {
            if (dataGridViewBuses.Columns.Count == 0)
                return;

            HideBusColumn("BusID");
            HideBusColumn("CreatedAt");
            HideBusColumn("UpdatedAt");

            SetBusHeader("BusNumber", "رقم الحافلة");
            SetBusHeader("DriverName", "اسم السائق");
            SetBusHeader("DriverPhone", "هاتف السائق");
            SetBusHeader("Capacity", "السعة");
            SetBusHeader("Notes", "ملاحظات");

            dataGridViewBuses.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void HideBusColumn(string columnName)
        {
            if (dataGridViewBuses.Columns.Contains(columnName))
                dataGridViewBuses.Columns[columnName].Visible = false;
        }

        private void SetBusHeader(string columnName, string headerText)
        {
            if (dataGridViewBuses.Columns.Contains(columnName))
                dataGridViewBuses.Columns[columnName].HeaderText = headerText;
        }

        private void LoadBusCombo()
        {
            if (allBuses == null)
                return;

            cmbBus.DataSource = allBuses.Copy();
            cmbBus.DisplayMember = "BusNumber";
            cmbBus.ValueMember = "BusID";

            if (cmbBus.Items.Count > 0)
                cmbBus.SelectedIndex = 0;
        }

        private bool ValidateBusInputs()
        {
            if (string.IsNullOrWhiteSpace(txtBusNumber.Text))
            {
                UIHelper.ShowWarning("أدخل رقم الحافلة.");
                txtBusNumber.Focus();
                return false;
            }

            int capacity;
            if (!int.TryParse(txtCapacity.Text.Trim(), out capacity) || capacity <= 0)
            {
                UIHelper.ShowWarning("أدخل سعة صحيحة للحافلة.");
                txtCapacity.Focus();
                return false;
            }

            if (txtBusNumber.Text.Trim().Length > 30 || txtDriverName.Text.Trim().Length > 150 ||
                txtDriverPhone.Text.Trim().Length > 30 || txtNotes.Text.Trim().Length > 1000)
            {
                UIHelper.ShowWarning("تجاوز أحد حقول الحافلة الحد المسموح به.");
                return false;
            }

            string phone = txtDriverPhone.Text.Trim();
            if (!string.IsNullOrWhiteSpace(phone) && (phone.Length < 7 || phone.Length > 15 ||
                !phone.All(char.IsDigit)))
            {
                UIHelper.ShowWarning("رقم هاتف السائق يجب أن يحتوي على أرقام من 7 إلى 15 خانة.");
                txtDriverPhone.Focus();
                return false;
            }

            return true;
        }

        private Bus GetBusFromInputs()
        {
            int capacity;

            if (!int.TryParse(txtCapacity.Text.Trim(), out capacity))
                capacity = 30;

            return new Bus
            {
                BusID = selectedBusId,
                BusNumber = txtBusNumber.Text.Trim(),
                DriverName = txtDriverName.Text.Trim(),
                DriverPhone = txtDriverPhone.Text.Trim(),
                Capacity = capacity,
                Notes = txtNotes.Text.Trim()
            };
        }

        private void ClearBusInputs()
        {
            selectedBusId = 0;

            txtBusNumber.Clear();
            txtDriverName.Clear();
            txtDriverPhone.Clear();
            txtCapacity.Text = "30";
            txtNotes.Clear();
        }

        private void dataGridViewBuses_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dataGridViewBuses.Rows.Count == 0)
                return;

            DataRowView rowView = dataGridViewBuses.Rows[e.RowIndex].DataBoundItem as DataRowView;

            if (rowView == null)
                return;

            DataRow row = rowView.Row;

            selectedBusId = Convert.ToInt32(row["BusID"]);
            txtBusNumber.Text = row["BusNumber"].ToString();
            txtDriverName.Text = row["DriverName"] == DBNull.Value ? "" : row["DriverName"].ToString();
            txtDriverPhone.Text = row["DriverPhone"] == DBNull.Value ? "" : row["DriverPhone"].ToString();
            txtCapacity.Text = row["Capacity"].ToString();
            txtNotes.Text = row["Notes"] == DBNull.Value ? "" : row["Notes"].ToString();
        }

        private async void btnAddBus_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateBusInputs())
                    return;

                Bus bus = GetBusFromInputs();

                await Task.Run(() => busService.AddBus(bus));

                UIHelper.ShowInfo("تمت إضافة الحافلة بنجاح.");

                await LoadBusesAsync();
                ClearBusInputs();

                lblRecordCount.Text = "عدد الحافلات: " + (allBuses == null ? 0 : allBuses.Rows.Count);
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("إضافة الحافلة", ex);
            }
        }

        private async void btnUpdateBus_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedBusId == 0)
                {
                    UIHelper.ShowWarning("اختر الحافلة من الجدول أولاً.");
                    return;
                }

                if (!ValidateBusInputs())
                    return;

                Bus bus = GetBusFromInputs();

                bool result = await Task.Run(() => busService.UpdateBus(bus));

                if (result)
                    UIHelper.ShowInfo("تم تعديل الحافلة بنجاح.");
                else
                    UIHelper.ShowWarning("لم يتم العثور على الحافلة أو لم يتم تعديلها.");

                await LoadBusesAsync();
                await LoadRoutesAsync();
                ClearBusInputs();
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("تعديل الحافلة", ex);
            }
        }

        private async void btnDeleteBus_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedBusId == 0)
                {
                    UIHelper.ShowWarning("اختر الحافلة من الجدول أولاً.");
                    return;
                }

                DialogResult confirm = MessageBox.Show(
                    "هل تريد حذف الحافلة المحددة؟\nإذا كانت الحافلة مرتبطة بمسارات قد يظهر خطأ لوجود ارتباطات.",
                    "تأكيد الحذف",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirm != DialogResult.Yes)
                    return;

                bool result = await Task.Run(() => busService.DeleteBus(selectedBusId));

                if (result)
                    UIHelper.ShowInfo("تم حذف الحافلة بنجاح.");
                else
                    UIHelper.ShowWarning("لم يتم العثور على الحافلة أو لم يتم حذفها.");

                await LoadBusesAsync();
                await LoadRoutesAsync();
                ClearBusInputs();
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("حذف الحافلة", ex);
            }
        }

        private void btnClearBus_Click(object sender, EventArgs e)
        {
            ClearBusInputs();
        }

        // =========================
        // المسارات
        // =========================

        private async Task LoadRoutesAsync()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                allRoutes = await Task.Run(() => routeService.GetAllRoutes());

                dataGridViewRoutes.DataSource = allRoutes;

                FormatRoutesGrid();

                if (allBuses == null)
                    await LoadBusesAsync();

                LoadBusCombo();
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void FormatRoutesGrid()
        {
            if (dataGridViewRoutes.Columns.Count == 0)
                return;

            HideRouteColumn("RouteID");
            HideRouteColumn("BusID");
            HideRouteColumn("CreatedAt");
            HideRouteColumn("UpdatedAt");

            SetRouteHeader("RouteName", "اسم المسار");
            SetRouteHeader("BusNumber", "الحافلة");
            SetRouteHeader("StartPoint", "نقطة البداية");
            SetRouteHeader("EndPoint", "نقطة النهاية");
            SetRouteHeader("DepartureTime", "وقت الانطلاق");
            SetRouteHeader("ArrivalTime", "وقت الوصول");
            SetRouteHeader("Fee", "رسوم النقل");
            SetRouteHeader("Notes", "ملاحظات");

            if (dataGridViewRoutes.Columns.Contains("Fee"))
                dataGridViewRoutes.Columns["Fee"].DefaultCellStyle.Format = "N2";

            dataGridViewRoutes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void HideRouteColumn(string columnName)
        {
            if (dataGridViewRoutes.Columns.Contains(columnName))
                dataGridViewRoutes.Columns[columnName].Visible = false;
        }

        private void SetRouteHeader(string columnName, string headerText)
        {
            if (dataGridViewRoutes.Columns.Contains(columnName))
                dataGridViewRoutes.Columns[columnName].HeaderText = headerText;
        }

        private bool ValidateRouteInputs()
        {
            if (string.IsNullOrWhiteSpace(txtRouteName.Text))
            {
                MessageBox.Show("أدخل اسم المسار.");
                txtRouteName.Focus();
                return false;
            }

            if (cmbBus.SelectedValue == null || cmbBus.Items.Count == 0)
            {
                MessageBox.Show("اختر الحافلة.");
                cmbBus.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtStartPoint.Text) || string.IsNullOrWhiteSpace(txtEndPoint.Text))
            {
                MessageBox.Show("أدخل نقطة البداية ونقطة النهاية.");
                txtStartPoint.Focus();
                return false;
            }

            if (dtpArrival.Value.TimeOfDay <= dtpDeparture.Value.TimeOfDay)
            {
                MessageBox.Show("وقت الوصول يجب أن يكون بعد وقت الانطلاق.");
                dtpArrival.Focus();
                return false;
            }

            decimal fee = 0;
            if (!string.IsNullOrWhiteSpace(txtFee.Text) &&
                (!decimal.TryParse(txtFee.Text.Trim(), NumberStyles.Any, CultureInfo.CurrentCulture, out fee) &&
                 !decimal.TryParse(txtFee.Text.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out fee) || fee < 0))
            {
                MessageBox.Show("أدخل رسوم نقل رقمية غير سالبة.");
                txtFee.Focus();
                return false;
            }

            if (txtRouteName.Text.Trim().Length > 150 || txtStartPoint.Text.Trim().Length > 200 ||
                txtEndPoint.Text.Trim().Length > 200 || txtRouteNotes.Text.Trim().Length > 1000)
            {
                MessageBox.Show("تجاوز أحد حقول المسار الحد المسموح به.");
                return false;
            }

            return true;
        }

        private decimal? ReadNullableDecimal(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return null;

            decimal value;

            if (decimal.TryParse(text, NumberStyles.Any, CultureInfo.CurrentCulture, out value))
                return value;

            if (decimal.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out value))
                return value;

            return null;
        }

        private BusRoute GetRouteFromInputs()
        {
            return new BusRoute
            {
                RouteID = selectedRouteId,
                RouteName = txtRouteName.Text.Trim(),
                BusID = cmbBus.SelectedValue == null ? 0 : Convert.ToInt32(cmbBus.SelectedValue),
                StartPoint = txtStartPoint.Text.Trim(),
                EndPoint = txtEndPoint.Text.Trim(),
                DepartureTime = dtpDeparture.Value.TimeOfDay,
                ArrivalTime = dtpArrival.Value.TimeOfDay,
                Fee = ReadNullableDecimal(txtFee.Text),
                Notes = txtRouteNotes.Text.Trim()
            };
        }

        private void ClearRouteInputs()
        {
            selectedRouteId = 0;

            txtRouteName.Clear();

            if (cmbBus.Items.Count > 0)
                cmbBus.SelectedIndex = 0;

            txtStartPoint.Clear();
            txtEndPoint.Clear();

            dtpDeparture.Value = DateTime.Today.AddHours(7);
            dtpArrival.Value = DateTime.Today.AddHours(8);

            txtFee.Text = "0";
            txtRouteNotes.Clear();
        }

        private void dataGridViewRoutes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dataGridViewRoutes.Rows.Count == 0)
                return;

            DataRowView rowView = dataGridViewRoutes.Rows[e.RowIndex].DataBoundItem as DataRowView;

            if (rowView == null)
                return;

            DataRow row = rowView.Row;

            selectedRouteId = Convert.ToInt32(row["RouteID"]);

            txtRouteName.Text = row["RouteName"].ToString();

            if (row["BusID"] != DBNull.Value && cmbBus.Items.Count > 0)
                cmbBus.SelectedValue = Convert.ToInt32(row["BusID"]);

            txtStartPoint.Text = row["StartPoint"] == DBNull.Value ? "" : row["StartPoint"].ToString();
            txtEndPoint.Text = row["EndPoint"] == DBNull.Value ? "" : row["EndPoint"].ToString();

            if (row["DepartureTime"] != DBNull.Value)
                dtpDeparture.Value = DateTime.Today.Add((TimeSpan)row["DepartureTime"]);
            else
                dtpDeparture.Value = DateTime.Today.AddHours(7);

            if (row["ArrivalTime"] != DBNull.Value)
                dtpArrival.Value = DateTime.Today.Add((TimeSpan)row["ArrivalTime"]);
            else
                dtpArrival.Value = DateTime.Today.AddHours(8);

            txtFee.Text = row["Fee"] == DBNull.Value ? "0" : Convert.ToDecimal(row["Fee"]).ToString("N2");

            txtRouteNotes.Text = row["Notes"] == DBNull.Value ? "" : row["Notes"].ToString();
        }

        private async void btnAddRoute_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateRouteInputs())
                    return;

                BusRoute route = GetRouteFromInputs();

                await Task.Run(() => routeService.AddRoute(route));

                MessageBox.Show("تمت إضافة المسار بنجاح.");

                await LoadRoutesAsync();
                ClearRouteInputs();

                lblRecordCount.Text = "عدد المسارات: " + (allRoutes == null ? 0 : allRoutes.Rows.Count);
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("إضافة المسار", ex);
            }
        }

        private async void btnUpdateRoute_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedRouteId == 0)
                {
                    MessageBox.Show("اختر المسار من الجدول أولاً.");
                    return;
                }

                if (!ValidateRouteInputs())
                    return;

                BusRoute route = GetRouteFromInputs();

                bool result = await Task.Run(() => routeService.UpdateRoute(route));

                MessageBox.Show(result ? "تم تعديل المسار بنجاح." : "لم يتم تعديل المسار.");

                await LoadRoutesAsync();
                ClearRouteInputs();
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("تعديل المسار", ex);
            }
        }

        private async void btnDeleteRoute_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedRouteId == 0)
                {
                    MessageBox.Show("اختر المسار من الجدول أولاً.");
                    return;
                }

                DialogResult confirm = MessageBox.Show(
                    "هل تريد حذف المسار المحدد؟",
                    "تأكيد الحذف",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirm != DialogResult.Yes)
                    return;

                bool result = await Task.Run(() => routeService.DeleteRoute(selectedRouteId));

                MessageBox.Show(result ? "تم حذف المسار." : "لم يتم حذف المسار.");

                await LoadRoutesAsync();
                ClearRouteInputs();
            }
            catch (Exception ex)
            {
                UIHelper.ShowException("حذف المسار", ex);
            }
        }

        private void btnClearRoute_Click(object sender, EventArgs e)
        {
            ClearRouteInputs();
        }
    }
}
