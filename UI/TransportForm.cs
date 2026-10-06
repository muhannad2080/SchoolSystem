using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SchoolSystem.Models;
using SchoolSystem.Helpers;
using SchoolSystem.Services;
using SchoolSystem.Security;


namespace SchoolSystem.UI
{

    public partial class TransportForm : UserControl
    {
        // تعريف الدالة BusService لتنفيذ العملية المرتبطة بها.
        private readonly BusService busService = new BusService();
        // تعريف الدالة BusRouteService لتنفيذ العملية المرتبطة بها.
        private readonly BusRouteService routeService = new BusRouteService();


        private int selectedBusId = 0;

        private int selectedRouteId = 0;


        private DataTable allBuses;

        private DataTable allRoutes;


        private bool isLoading = false;

        private readonly StudentTransportService studentTransportService = new StudentTransportService();
        private TabPage tabStudentTransport;
        private DataGridView dataGridViewStudentTransport;
        private ComboBox cmbTransportStudent;
        private ComboBox cmbTransportRoute;
        private TextBox txtTransportAcademicYear;
        private TextBox txtPickupPoint;
        private TextBox txtDropoffPoint;
        private TextBox txtTransportNotes;
        private CheckBox chkTransportActive;
        private DateTimePicker dtpTransportStartDate;
        private DateTimePicker dtpTransportEndDate;
        private Button btnAddTransportAssignment;
        private Button btnUpdateTransportAssignment;
        private Button btnDeleteTransportAssignment;
        private Button btnClearTransportAssignment;
        private Label lblTransportAssignmentCount;
        private int selectedTransportAssignmentId;


        // تعريف الدالة TransportForm لتنفيذ العملية المرتبطة بها.
        public TransportForm()
        {

            InitializeComponent();

            ConfigureStudentTransportTab();

            SchoolSystem.Helpers.UIHelper.ApplyStyle(this);
            ApplyCustomStyles();
            Dock = DockStyle.Fill;

            DataRefreshNotifier.DataChanged += DataRefreshNotifier_DataChanged;
            Disposed += (sender, e) =>
                DataRefreshNotifier.DataChanged -= DataRefreshNotifier_DataChanged;

            Load += TransportForm_Load;

            tabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;

            // تقييد حقل رقم الحافلة: أحرف وأرقام وشرطات فقط.
            txtBusNumber.MaxLength = 30;
            txtBusNumber.KeyPress -= BusNumber_KeyPress;
            txtBusNumber.KeyPress += BusNumber_KeyPress;
        }

        private async void DataRefreshNotifier_DataChanged(
            object sender,
            DataRefreshEventArgs e)
        {
            if (ReferenceEquals(sender, this) ||
                IsDisposed ||
                isLoading)
            {
                return;
            }

            if (!DataRefreshNotifier.IsRelated(e, "Students")
                && !DataRefreshNotifier.IsRelated(e, "Classes")
                && !DataRefreshNotifier.IsRelated(e, "Enrollment")
                && !DataRefreshNotifier.IsRelated(e, "All"))
                return;


            try
            {
                isLoading = true;
                await LoadBusesAsync();
                await LoadRoutesAsync();
                await LoadStudentTransportAsync();
            }
            catch (Exception ex)
            {
                ApplicationLogger.LogException(
                    "TransportForm: التحديث التلقائي",
                    ex);
            }
            finally
            {
                isLoading = false;
            }
        }

        // تعريف الدالة BusNumber_KeyPress للسماح بالحروف والأرقام والشرطات فقط.
        private void BusNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e == null)
                return;

            if (char.IsControl(e.KeyChar))
                return;

            if (!char.IsLetterOrDigit(e.KeyChar) &&
                !char.IsWhiteSpace(e.KeyChar) &&
                e.KeyChar != '-')
            {
                e.Handled = true;
            }
        }

        // تعريف الدالة ApplyCustomStyles لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة TransportForm_Load لتنفيذ العملية المرتبطة بها.
        private async void TransportForm_Load(object sender, EventArgs e)
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
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

                await LoadStudentTransportAsync();

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

        // تعريف الدالة TabControl_SelectedIndexChanged لتنفيذ العملية المرتبطة بها.
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
            else if (tabControl.SelectedTab == tabStudentTransport)
            {
                await LoadStudentTransportAsync();
                lblRecordCount.Text = "عدد توزيعات النقل: " +
                    (dataGridViewStudentTransport == null ? 0 : dataGridViewStudentTransport.Rows.Count);
            }
        }





        // تعريف الدالة LoadBusesAsync لتنفيذ العملية المرتبطة بها.
        private async Task LoadBusesAsync()
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
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

        // تعريف الدالة FormatBusesGrid لتنفيذ العملية المرتبطة بها.
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

                    }

        // تعريف الدالة HideBusColumn لتنفيذ العملية المرتبطة بها.
        private void HideBusColumn(string columnName)
        {
            if (dataGridViewBuses.Columns.Contains(columnName))
                dataGridViewBuses.Columns[columnName].Visible = false;
        }

        // تعريف الدالة SetBusHeader لتنفيذ العملية المرتبطة بها.
        private void SetBusHeader(string columnName, string headerText)
        {
            if (dataGridViewBuses.Columns.Contains(columnName))
                dataGridViewBuses.Columns[columnName].HeaderText = headerText;
        }

        // تعريف الدالة LoadBusCombo لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة ValidateBusInputs لتنفيذ العملية المرتبطة بها.
        private bool ValidateBusInputs()
        {
            if (string.IsNullOrWhiteSpace(txtBusNumber.Text))
            {
                UIHelper.ShowWarning("أدخل رقم الحافلة.");
                txtBusNumber.Focus();

                return false;
            }

            int capacity;
            if (!TryParsePositiveInt(txtCapacity.Text, out capacity))
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

        // تعريف الدالة GetBusFromInputs لتنفيذ العملية المرتبطة بها.
        private Bus GetBusFromInputs()
        {
            int capacity;

            if (!TryParsePositiveInt(txtCapacity.Text, out capacity))
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

        // تعريف الدالة TryParsePositiveInt لتنفيذ العملية المرتبطة بها.
        private bool TryParsePositiveInt(string value, out int number)
        {
            number = 0;
            if (!UIHelper.TryParseDecimal(value, out decimal parsed) || parsed <= 0 || parsed != decimal.Truncate(parsed) || parsed > int.MaxValue)
                return false;

            number = (int)parsed;

            return true;
        }

        // تعريف الدالة ClearBusInputs لتنفيذ العملية المرتبطة بها.
        private void ClearBusInputs()
        {
            selectedBusId = 0;

            txtBusNumber.Clear();
            txtDriverName.Clear();
            txtDriverPhone.Clear();
            txtCapacity.Text = "30";
            txtNotes.Clear();
        }

        // تعريف الدالة dataGridViewBuses_CellClick لتنفيذ العملية المرتبطة بها.
        private void dataGridViewBuses_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dataGridViewBuses.Rows.Count == 0)
                return;

            DataRowView rowView = dataGridViewBuses.Rows[e.RowIndex].DataBoundItem as DataRowView;

            if (rowView == null)
                return;

            DataRow row = rowView.Row;

            selectedBusId = row["BusID"] != DBNull.Value && int.TryParse(row["BusID"].ToString(), out int busId)
                ? busId
                : 0;
            txtBusNumber.Text = row["BusNumber"].ToString();
            txtDriverName.Text = row["DriverName"] == DBNull.Value ? "" : row["DriverName"].ToString();
            txtDriverPhone.Text = row["DriverPhone"] == DBNull.Value ? "" : row["DriverPhone"].ToString();
            txtCapacity.Text = row["Capacity"].ToString();
            txtNotes.Text = row["Notes"] == DBNull.Value ? "" : row["Notes"].ToString();
        }

        // تعريف الدالة btnAddBus_Click لتنفيذ العملية المرتبطة بها.
        private async void btnAddBus_Click(object sender, EventArgs e)
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                CurrentUser.DemandAction("Transport", "Add", "ليس لديك صلاحية إضافة الحافلات.");
                if (!ValidateBusInputs())
                    return;

                Bus bus = GetBusFromInputs();


                await Task.Run(() => busService.AddBus(bus));

                UIHelper.ShowInfo("تمت إضافة الحافلة بنجاح.");
                ApplicationLogger.LogUsage("شاشة النقل", "إضافة حافلة", "تمت إضافة الحافلة بنجاح");


                await LoadBusesAsync();
                ClearBusInputs();

                lblRecordCount.Text = "عدد الحافلات: " + (allBuses == null ? 0 : allBuses.Rows.Count);
            }

            catch (Exception ex)
            {
                UIHelper.ShowException("إضافة الحافلة", ex);
            }
        }

                // تعريف الدالة btnUpdateBus_Click لتنفيذ العملية المرتبطة بها.
                private async void btnUpdateBus_Click(object sender, EventArgs e)
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
                {
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                CurrentUser.DemandAction("Transport", "Edit", "ليس لديك صلاحية تعديل الحافلات.");
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

                if (result) ApplicationLogger.LogUsage("شاشة النقل", "تعديل حافلة", "تم تعديل الحافلة بنجاح");


                await LoadBusesAsync();

                await LoadRoutesAsync();
                ClearBusInputs();
            }

            catch (Exception ex)
            {
                UIHelper.ShowException("تعديل الحافلة", ex);
            }
        }

        // تعريف الدالة btnDeleteBus_Click لتنفيذ العملية المرتبطة بها.
        private async void btnDeleteBus_Click(object sender, EventArgs e)
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                CurrentUser.DemandAction("Transport", "Delete", "ليس لديك صلاحية حذف الحافلات.");
                if (selectedBusId == 0)
                {
                    UIHelper.ShowWarning("اختر الحافلة من الجدول أولاً.");

                    return;
                }


                if (!UIHelper.ShowConfirmation(
                    "هل تريد حذف الحافلة المحددة؟\nإذا كانت الحافلة مرتبطة بمسارات قد يظهر خطأ لوجود ارتباطات.",
                    "تأكيد الحذف"))
                    return;


                bool result = await Task.Run(() => busService.DeleteBus(selectedBusId));

                if (result)
                    UIHelper.ShowInfo("تم حذف الحافلة بنجاح.");

                else

                    UIHelper.ShowWarning("لم يتم العثور على الحافلة أو لم يتم حذفها.");

                if (result) ApplicationLogger.LogUsage("شاشة النقل", "حذف حافلة", "تم حذف الحافلة بنجاح");


                await LoadBusesAsync();

                await LoadRoutesAsync();
                ClearBusInputs();
            }

            catch (Exception ex)
            {
                UIHelper.ShowException("حذف الحافلة", ex);
            }
        }

        // تعريف الدالة btnClearBus_Click لتنفيذ العملية المرتبطة بها.
        private void btnClearBus_Click(object sender, EventArgs e)
        {
            ClearBusInputs();
        }





        // تعريف الدالة LoadRoutesAsync لتنفيذ العملية المرتبطة بها.
        private async Task LoadRoutesAsync()
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
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

        // تعريف الدالة FormatRoutesGrid لتنفيذ العملية المرتبطة بها.
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

                    }

        // تعريف الدالة HideRouteColumn لتنفيذ العملية المرتبطة بها.
        private void HideRouteColumn(string columnName)
        {
            if (dataGridViewRoutes.Columns.Contains(columnName))
                dataGridViewRoutes.Columns[columnName].Visible = false;
        }

        // تعريف الدالة SetRouteHeader لتنفيذ العملية المرتبطة بها.
        private void SetRouteHeader(string columnName, string headerText)
        {
            if (dataGridViewRoutes.Columns.Contains(columnName))
                dataGridViewRoutes.Columns[columnName].HeaderText = headerText;
        }

        // تعريف الدالة ValidateRouteInputs لتنفيذ العملية المرتبطة بها.
        private bool ValidateRouteInputs()
        {
            if (string.IsNullOrWhiteSpace(txtRouteName.Text))
            {

                UIHelper.ShowWarning("أدخل اسم المسار.");

                txtRouteName.Focus();

                return false;
            }

            if (cmbBus.SelectedValue == null || cmbBus.Items.Count == 0)
            {

                UIHelper.ShowWarning("اختر الحافلة.");

                cmbBus.Focus();

                return false;
            }

            if (string.IsNullOrWhiteSpace(txtStartPoint.Text) || string.IsNullOrWhiteSpace(txtEndPoint.Text))
            {

                UIHelper.ShowWarning("أدخل نقطة البداية ونقطة النهاية.");

                txtStartPoint.Focus();

                return false;
            }

            if (dtpArrival.Value.TimeOfDay <= dtpDeparture.Value.TimeOfDay)
            {

                UIHelper.ShowWarning("وقت الوصول يجب أن يكون بعد وقت الانطلاق.");

                dtpArrival.Focus();

                return false;
            }

            decimal fee = 0;
            if (!string.IsNullOrWhiteSpace(txtFee.Text) &&
                (!UIHelper.TryParseDecimal(txtFee.Text, out fee) || fee < 0))
            {

                UIHelper.ShowWarning("أدخل رسوم نقل رقمية غير سالبة.");

                txtFee.Focus();

                return false;
            }

            if (txtRouteName.Text.Trim().Length > 150 || txtStartPoint.Text.Trim().Length > 200 ||
                txtEndPoint.Text.Trim().Length > 200 || txtRouteNotes.Text.Trim().Length > 1000)
            {

                UIHelper.ShowWarning("تجاوز أحد حقول المسار الحد المسموح به.");

                return false;
            }


            return true;
        }

        // تعريف الدالة ReadNullableDecimal لتنفيذ العملية المرتبطة بها.
        private decimal? ReadNullableDecimal(string text)
        {

            return string.IsNullOrWhiteSpace(text) ? (decimal?)null :
                (UIHelper.TryParseDecimal(text, out decimal value) ? value : (decimal?)null);
        }

        // تعريف الدالة GetRouteFromInputs لتنفيذ العملية المرتبطة بها.
        private BusRoute GetRouteFromInputs()
        {

            return new BusRoute
            {
                RouteID = selectedRouteId,
                RouteName = txtRouteName.Text.Trim(),
                BusID = cmbBus.SelectedValue != null && int.TryParse(cmbBus.SelectedValue.ToString(), out int busId) ? busId : 0,
                StartPoint = txtStartPoint.Text.Trim(),
                EndPoint = txtEndPoint.Text.Trim(),
                DepartureTime = dtpDeparture.Value.TimeOfDay,
                ArrivalTime = dtpArrival.Value.TimeOfDay,
                Fee = ReadNullableDecimal(txtFee.Text),
                Notes = txtRouteNotes.Text.Trim()

            };
        }

        // تعريف الدالة ClearRouteInputs لتنفيذ العملية المرتبطة بها.
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

        // تعريف الدالة dataGridViewRoutes_CellClick لتنفيذ العملية المرتبطة بها.
        private void dataGridViewRoutes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dataGridViewRoutes.Rows.Count == 0)
                return;

            DataRowView rowView = dataGridViewRoutes.Rows[e.RowIndex].DataBoundItem as DataRowView;

            if (rowView == null)
                return;

            DataRow row = rowView.Row;

            selectedRouteId = row["RouteID"] != DBNull.Value && int.TryParse(row["RouteID"].ToString(), out int routeId)
                ? routeId
                : 0;

            txtRouteName.Text = row["RouteName"].ToString();

            if (row["BusID"] != DBNull.Value && cmbBus.Items.Count > 0 && int.TryParse(row["BusID"].ToString(), out int busId))
                cmbBus.SelectedValue = busId;

            txtStartPoint.Text = row["StartPoint"] == DBNull.Value ? "" : row["StartPoint"].ToString();
            txtEndPoint.Text = row["EndPoint"] == DBNull.Value ? "" : row["EndPoint"].ToString();

            if (row["DepartureTime"] != DBNull.Value && TryReadTime(row["DepartureTime"], out TimeSpan departure))
                dtpDeparture.Value = DateTime.Today.Add(departure);

            else

                dtpDeparture.Value = DateTime.Today.AddHours(7);

            if (row["ArrivalTime"] != DBNull.Value && TryReadTime(row["ArrivalTime"], out TimeSpan arrival))
                dtpArrival.Value = DateTime.Today.Add(arrival);

            else

                dtpArrival.Value = DateTime.Today.AddHours(8);

            txtFee.Text = row["Fee"] == DBNull.Value ? "0" :
                (UIHelper.TryParseDecimal(row["Fee"].ToString(), out decimal fee) ? fee.ToString("N2") : "0");

            txtRouteNotes.Text = row["Notes"] == DBNull.Value ? "" : row["Notes"].ToString();
        }

        // تعريف الدالة TryReadTime لتنفيذ العملية المرتبطة بها.
        private bool TryReadTime(object value, out TimeSpan time)
        {
            if (value is TimeSpan span)
            {
                time = span;

                return true;
            }


            return TimeSpan.TryParse(value == null ? string.Empty : value.ToString(), out time);
        }

        private void ConfigureStudentTransportTab()
        {
            tabStudentTransport = new TabPage("توزيع الطلاب");
            tabStudentTransport.Name = "tabStudentTransport";
            tabStudentTransport.Padding = new Padding(8);

            TableLayoutPanel editor = new TableLayoutPanel();
            editor.Dock = DockStyle.Top;
            editor.AutoSize = true;
            editor.ColumnCount = 4;
            editor.RowCount = 5;
            editor.Padding = new Padding(4);
            editor.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            editor.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            editor.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            editor.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));

            txtTransportAcademicYear = new TextBox { Dock = DockStyle.Fill };
            cmbTransportStudent = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Dock = DockStyle.Fill };
            cmbTransportRoute = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Dock = DockStyle.Fill };
            txtPickupPoint = new TextBox { Dock = DockStyle.Fill, MaxLength = 200 };
            txtDropoffPoint = new TextBox { Dock = DockStyle.Fill, MaxLength = 200 };
            txtTransportNotes = new TextBox { Dock = DockStyle.Fill, MaxLength = 1000 };
            chkTransportActive = new CheckBox { Text = "الاشتراك نشط", Checked = true, Dock = DockStyle.Fill };
            dtpTransportStartDate = new DateTimePicker { Format = DateTimePickerFormat.Short, Value = DateTime.Today, Dock = DockStyle.Fill };
            dtpTransportEndDate = new DateTimePicker { Format = DateTimePickerFormat.Short, ShowCheckBox = true, Checked = false, Value = DateTime.Today, Dock = DockStyle.Fill };

            AddEditorField(editor, 0, 0, "العام الدراسي", txtTransportAcademicYear);
            AddEditorField(editor, 1, 0, "الطالب", cmbTransportStudent);
            AddEditorField(editor, 2, 0, "المسار", cmbTransportRoute);
            AddEditorField(editor, 3, 0, "الحالة", chkTransportActive);
            AddEditorField(editor, 0, 1, "نقطة الصعود", txtPickupPoint);
            AddEditorField(editor, 1, 1, "نقطة النزول", txtDropoffPoint);
            AddEditorField(editor, 2, 1, "بداية النقل", dtpTransportStartDate);
            AddEditorField(editor, 3, 1, "نهاية النقل", dtpTransportEndDate);
            AddEditorField(editor, 0, 2, "ملاحظات", txtTransportNotes);
            editor.Controls.Add(new Label { Text = "", Dock = DockStyle.Fill }, 1, 2);

            FlowLayoutPanel actions = new FlowLayoutPanel();
            actions.Dock = DockStyle.Fill;
            actions.AutoSize = true;

            btnAddTransportAssignment = new Button { Text = "إضافة", Width = 100 };
            btnUpdateTransportAssignment = new Button { Text = "تعديل", Width = 100 };
            btnDeleteTransportAssignment = new Button { Text = "إلغاء التوزيع", Width = 120 };
            btnClearTransportAssignment = new Button { Text = "تفريغ", Width = 100 };
            lblTransportAssignmentCount = new Label { Text = "عدد التوزيعات: 0", AutoSize = true, Padding = new Padding(10, 8, 10, 0) };

            btnAddTransportAssignment.Click += async (s, e) => await AddTransportAssignmentAsync();
            btnUpdateTransportAssignment.Click += async (s, e) => await UpdateTransportAssignmentAsync();
            btnDeleteTransportAssignment.Click += async (s, e) => await DeleteTransportAssignmentAsync();
            btnClearTransportAssignment.Click += (s, e) => ClearTransportAssignmentInputs();

            actions.Controls.Add(btnAddTransportAssignment);
            actions.Controls.Add(btnUpdateTransportAssignment);
            actions.Controls.Add(btnDeleteTransportAssignment);
            actions.Controls.Add(btnClearTransportAssignment);
            actions.Controls.Add(lblTransportAssignmentCount);

            editor.Controls.Add(actions, 0, 3);
            editor.SetColumnSpan(actions, 4);

            dataGridViewStudentTransport = new DataGridView();
            dataGridViewStudentTransport.Dock = DockStyle.Fill;
            dataGridViewStudentTransport.AutoGenerateColumns = true;
            dataGridViewStudentTransport.ReadOnly = true;
            dataGridViewStudentTransport.AllowUserToAddRows = false;
            dataGridViewStudentTransport.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewStudentTransport.MultiSelect = false;
            dataGridViewStudentTransport.CellClick += DataGridViewStudentTransport_CellClick;

            tabStudentTransport.Controls.Add(dataGridViewStudentTransport);
            tabStudentTransport.Controls.Add(editor);
            tabControl.TabPages.Add(tabStudentTransport);

            TrySetDefaultTransportYear();
        }

        private static void AddEditorField(TableLayoutPanel layout, int column, int row, string labelText, Control control)
        {
            Panel panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(3) };
            Label label = new Label { Text = labelText, Dock = DockStyle.Top, Height = 23 };
            panel.Controls.Add(control);
            panel.Controls.Add(label);
            layout.Controls.Add(panel, column, row);
        }

        private void TrySetDefaultTransportYear()
        {
            try
            {
                string year = AcademicYearHelper.GetCurrentAcademicYear();
                if (!string.IsNullOrWhiteSpace(year))
                    txtTransportAcademicYear.Text = year;
            }
            catch
            {
                // يظل العام الافتراضي الظاهر عند تعذر قراءة المصدر أثناء تهيئة الواجهة.
            }
        }

        private async Task LoadStudentTransportAsync()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                DataTable assignments = await Task.Run(() => studentTransportService.GetAll());
                DataTable students = await Task.Run(() => studentTransportService.GetActiveStudents());
                DataTable routes = await Task.Run(() => studentTransportService.GetRoutes());

                dataGridViewStudentTransport.DataSource = assignments;
                FormatStudentTransportGrid();

                cmbTransportStudent.DataSource = students;
                cmbTransportStudent.DisplayMember = "FullName";
                cmbTransportStudent.ValueMember = "StudentID";

                cmbTransportRoute.DataSource = routes;
                cmbTransportRoute.DisplayMember = "RouteName";
                cmbTransportRoute.ValueMember = "RouteID";

                lblTransportAssignmentCount.Text = "عدد التوزيعات: " + assignments.Rows.Count;
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void FormatStudentTransportGrid()
        {
            if (dataGridViewStudentTransport == null || dataGridViewStudentTransport.Columns.Count == 0)
                return;

            HideTransportAssignmentColumn("AssignmentID");
            HideTransportAssignmentColumn("StudentID");
            HideTransportAssignmentColumn("RouteID");
            HideTransportAssignmentColumn("CreatedAt");
            HideTransportAssignmentColumn("UpdatedAt");

            SetTransportAssignmentHeader("StudentNumber", "رقم الطالب");
            SetTransportAssignmentHeader("StudentName", "الطالب");
            SetTransportAssignmentHeader("RouteName", "المسار");
            SetTransportAssignmentHeader("BusNumber", "الحافلة");
            SetTransportAssignmentHeader("AcademicYear", "العام");
            SetTransportAssignmentHeader("PickupPoint", "نقطة الصعود");
            SetTransportAssignmentHeader("DropoffPoint", "نقطة النزول");
            SetTransportAssignmentHeader("IsActive", "نشط");
            SetTransportAssignmentHeader("StartDate", "بداية النقل");
            SetTransportAssignmentHeader("EndDate", "نهاية النقل");
            SetTransportAssignmentHeader("Notes", "ملاحظات");
        }

        private void HideTransportAssignmentColumn(string columnName)
        {
            if (dataGridViewStudentTransport.Columns.Contains(columnName))
                dataGridViewStudentTransport.Columns[columnName].Visible = false;
        }

        private void SetTransportAssignmentHeader(string columnName, string headerText)
        {
            if (dataGridViewStudentTransport.Columns.Contains(columnName))
                dataGridViewStudentTransport.Columns[columnName].HeaderText = headerText;
        }

        private StudentTransportAssignment GetTransportAssignmentFromInputs()
        {
            int studentId = cmbTransportStudent.SelectedValue != null &&
                            int.TryParse(cmbTransportStudent.SelectedValue.ToString(), out int parsedStudent)
                ? parsedStudent : 0;

            int routeId = cmbTransportRoute.SelectedValue != null &&
                          int.TryParse(cmbTransportRoute.SelectedValue.ToString(), out int parsedRoute)
                ? parsedRoute : 0;

            return new StudentTransportAssignment
            {
                AssignmentID = selectedTransportAssignmentId,
                StudentID = studentId,
                RouteID = routeId,
                AcademicYear = txtTransportAcademicYear.Text.Trim(),
                PickupPoint = txtPickupPoint.Text.Trim(),
                DropoffPoint = txtDropoffPoint.Text.Trim(),
                IsActive = chkTransportActive.Checked,
                StartDate = dtpTransportStartDate.Value.Date,
                EndDate = dtpTransportEndDate.Checked ? (DateTime?)dtpTransportEndDate.Value.Date : null,
                Notes = txtTransportNotes.Text.Trim()
            };
        }

        private async Task AddTransportAssignmentAsync()
        {
            try
            {
                StudentTransportAssignment assignment = GetTransportAssignmentFromInputs();
                await Task.Run(() => studentTransportService.Add(assignment));
                UIHelper.ShowInfo("تم توزيع الطالب على مسار النقل بنجاح.");
                ClearTransportAssignmentInputs();
                await LoadStudentTransportAsync();
            }
            catch (Exception ex) { UIHelper.ShowException("إضافة توزيع النقل", ex); }
        }

        private async Task UpdateTransportAssignmentAsync()
        {
            try
            {
                if (selectedTransportAssignmentId <= 0)
                {
                    UIHelper.ShowWarning("اختر توزيعًا من الجدول أولاً.");
                    return;
                }

                StudentTransportAssignment assignment = GetTransportAssignmentFromInputs();
                bool updated = await Task.Run(() => studentTransportService.Update(assignment));
                UIHelper.ShowInfo(updated ? "تم تعديل توزيع النقل." : "لم يتم تعديل توزيع النقل.");
                if (updated)
                {
                    ClearTransportAssignmentInputs();
                    await LoadStudentTransportAsync();
                }
            }
            catch (Exception ex) { UIHelper.ShowException("تعديل توزيع النقل", ex); }
        }

        private async Task DeleteTransportAssignmentAsync()
        {
            try
            {
                if (selectedTransportAssignmentId <= 0)
                {
                    UIHelper.ShowWarning("اختر توزيعًا من الجدول أولاً.");
                    return;
                }

                if (!UIHelper.ShowConfirmation(
                    "سيتم إلغاء توزيع النقل للطالب المحدد. هل تريد المتابعة؟",
                    "تأكيد الإلغاء"))
                    return;

                bool deleted = await Task.Run(() => studentTransportService.Delete(selectedTransportAssignmentId));
                UIHelper.ShowInfo(deleted ? "تم إلغاء توزيع النقل." : "لم يتم إلغاء التوزيع.");
                if (deleted)
                {
                    ClearTransportAssignmentInputs();
                    await LoadStudentTransportAsync();
                }
            }
            catch (Exception ex) { UIHelper.ShowException("إلغاء توزيع النقل", ex); }
        }

        private void DataGridViewStudentTransport_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dataGridViewStudentTransport.Rows.Count == 0)
                return;

            DataRowView rowView = dataGridViewStudentTransport.Rows[e.RowIndex].DataBoundItem as DataRowView;
            if (rowView == null)
                return;

            DataRow row = rowView.Row;
            selectedTransportAssignmentId = row["AssignmentID"] != DBNull.Value &&
                int.TryParse(row["AssignmentID"].ToString(), out int assignmentId) ? assignmentId : 0;

            if (row["StudentID"] != DBNull.Value)
                cmbTransportStudent.SelectedValue = Convert.ToInt32(row["StudentID"]);
            if (row["RouteID"] != DBNull.Value)
                cmbTransportRoute.SelectedValue = Convert.ToInt32(row["RouteID"]);

            txtTransportAcademicYear.Text = row["AcademicYear"].ToString();
            txtPickupPoint.Text = row["PickupPoint"].ToString();
            txtDropoffPoint.Text = row["DropoffPoint"].ToString();
            chkTransportActive.Checked = row["IsActive"] != DBNull.Value && Convert.ToBoolean(row["IsActive"]);

            if (row["StartDate"] != DBNull.Value)
                dtpTransportStartDate.Value = Convert.ToDateTime(row["StartDate"]);

            if (row["EndDate"] == DBNull.Value)
                dtpTransportEndDate.Checked = false;
            else
            {
                dtpTransportEndDate.Checked = true;
                dtpTransportEndDate.Value = Convert.ToDateTime(row["EndDate"]);
            }

            txtTransportNotes.Text = row["Notes"] == DBNull.Value ? "" : row["Notes"].ToString();
        }

        private void ClearTransportAssignmentInputs()
        {
            selectedTransportAssignmentId = 0;
            if (cmbTransportStudent.Items.Count > 0)
                cmbTransportStudent.SelectedIndex = 0;
            if (cmbTransportRoute.Items.Count > 0)
                cmbTransportRoute.SelectedIndex = 0;
            TrySetDefaultTransportYear();
            txtPickupPoint.Clear();
            txtDropoffPoint.Clear();
            txtTransportNotes.Clear();
            chkTransportActive.Checked = true;
            dtpTransportStartDate.Value = DateTime.Today;
            dtpTransportEndDate.Checked = false;
            dtpTransportEndDate.Value = DateTime.Today;
        }

        // تعريف الدالة btnAddRoute_Click لتنفيذ العملية المرتبطة بها.
        private async void btnAddRoute_Click(object sender, EventArgs e)
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                CurrentUser.DemandAction("Transport", "Add", "ليس لديك صلاحية إضافة مسارات النقل.");
                if (!ValidateRouteInputs())
                    return;

                BusRoute route = GetRouteFromInputs();


                await Task.Run(() => routeService.AddRoute(route));


                UIHelper.ShowInfo("تمت إضافة المسار بنجاح.");
                ApplicationLogger.LogUsage("شاشة النقل", "إضافة مسار", "تمت إضافة المسار بنجاح");


                await LoadRoutesAsync();
                ClearRouteInputs();

                lblRecordCount.Text = "عدد المسارات: " + (allRoutes == null ? 0 : allRoutes.Rows.Count);
            }

            catch (Exception ex)
            {
                UIHelper.ShowException("إضافة المسار", ex);
            }
        }

        // تعريف الدالة btnUpdateRoute_Click لتنفيذ العملية المرتبطة بها.
        private async void btnUpdateRoute_Click(object sender, EventArgs e)
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                CurrentUser.DemandAction("Transport", "Edit", "ليس لديك صلاحية تعديل مسارات النقل.");
                if (selectedRouteId == 0)
                {

                    UIHelper.ShowWarning("اختر المسار من الجدول أولاً.");

                    return;
                }

                if (!ValidateRouteInputs())
                    return;

                BusRoute route = GetRouteFromInputs();


                bool result = await Task.Run(() => routeService.UpdateRoute(route));


                UIHelper.ShowInfo(result ? "تم تعديل المسار بنجاح." : "لم يتم تعديل المسار.");
                if (result) ApplicationLogger.LogUsage("شاشة النقل", "تعديل مسار", "تم تعديل المسار بنجاح");


                await LoadRoutesAsync();
                ClearRouteInputs();
            }

            catch (Exception ex)
            {
                UIHelper.ShowException("تعديل المسار", ex);
            }
        }

        // تعريف الدالة btnDeleteRoute_Click لتنفيذ العملية المرتبطة بها.
        private async void btnDeleteRoute_Click(object sender, EventArgs e)
        {
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                CurrentUser.DemandAction("Transport", "Delete", "ليس لديك صلاحية حذف مسارات النقل.");
                if (selectedRouteId == 0)
                {

                    UIHelper.ShowWarning("اختر المسار من الجدول أولاً.");

                    return;
                }


                if (!UIHelper.ShowConfirmation(
                    "هل تريد حذف المسار المحدد؟",
                    "تأكيد الحذف"))
                    return;


                bool result = await Task.Run(() => routeService.DeleteRoute(selectedRouteId));


                UIHelper.ShowInfo(result ? "تم حذف المسار." : "لم يتم حذف المسار.");
                if (result) ApplicationLogger.LogUsage("شاشة النقل", "حذف مسار", "تم حذف المسار بنجاح");


                await LoadRoutesAsync();
                ClearRouteInputs();
            }

            catch (Exception ex)
            {
                UIHelper.ShowException("حذف المسار", ex);
            }
        }

        // تعريف الدالة btnClearRoute_Click لتنفيذ العملية المرتبطة بها.
        private void btnClearRoute_Click(object sender, EventArgs e)
        {
            ClearRouteInputs();
        }
    }
}
