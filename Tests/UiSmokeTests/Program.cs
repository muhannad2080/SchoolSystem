using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SchoolSystem.UiSmokeTests
{
    internal static class Program
    {
        [STAThread]
        private static void ValidateLayout(Control root)
        {
            root.CreateControl();
            root.PerformLayout();
            ValidateLayoutNode(root);
        }

        private static void ValidateLayoutNode(Control parent)
        {
            List<Control> children = parent.Controls
                .Cast<Control>()
                .Where(control => control.Visible && control.Width > 0 && control.Height > 0)
                .ToList();

            // Detect only absolute-position sibling overlaps. Docked/TableLayout/FlowLayout
            // controls are intentionally managed by the layout engine.
            if (!(parent is TableLayoutPanel) && !(parent is FlowLayoutPanel))
            {
                for (int i = 0; i < children.Count; i++)
                {
                    for (int j = i + 1; j < children.Count; j++)
                    {
                        Control left = children[i];
                        Control right = children[j];

                        if (left.Dock != DockStyle.None || right.Dock != DockStyle.None)
                            continue;

                        System.Drawing.Rectangle intersection =
                            System.Drawing.Rectangle.Intersect(left.Bounds, right.Bounds);

                        if (intersection.Width > 2 && intersection.Height > 2)
                        {
                            throw new InvalidOperationException(
                                "UI overlap in " + parent.Name + ": " +
                                left.Name + " overlaps " + right.Name + ".");
                        }
                    }
                }
            }

            foreach (Control child in children)
                ValidateLayoutNode(child);
        }

        private static void ValidateDesignerGeometry()
        {
            string projectRoot = Path.GetFullPath(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", ".."));

            string uiRoot = Path.Combine(projectRoot, "UI");
            if (!Directory.Exists(uiRoot))
                throw new DirectoryNotFoundException("UI source directory was not found.");

            int checkedFiles = 0;

            foreach (string designerPath in Directory.GetFiles(
                uiRoot,
                "*.Designer.cs",
                SearchOption.AllDirectories))
            {
                string source = File.ReadAllText(designerPath);

                MatchCollection negativeLocations = Regex.Matches(
                    source,
                    @"Location\s*=\s*new\s+(?:(?:System\.)?Drawing\.)?Point\(\s*(-\d+)\s*,\s*(-?\d+)\s*\)");

                if (negativeLocations.Count > 0)
                {
                    Match first = negativeLocations[0];
                    throw new InvalidOperationException(
                        "Negative control location in " +
                        Path.GetFileName(designerPath) +
                        ": (" + first.Groups[1].Value + ", " +
                        first.Groups[2].Value + ").");
                }

                Match client = Regex.Match(
                    source,
                    @"this\.ClientSize\s*=\s*new\s+(?:(?:System\.)?Drawing\.)?Size\(\s*(\d+)\s*,\s*(\d+)\s*\)");

                if (!client.Success)
                {
                    client = Regex.Match(
                        source,
                        @"this\.Size\s*=\s*new\s+(?:(?:System\.)?Drawing\.)?Size\(\s*(\d+)\s*,\s*(\d+)\s*\)");
                }

                Match minimum = Regex.Match(
                    source,
                    @"this\.MinimumSize\s*=\s*new\s+(?:(?:System\.)?Drawing\.)?Size\(\s*(\d+)\s*,\s*(\d+)\s*\)");

                if (client.Success && minimum.Success)
                {
                    int clientWidth = int.Parse(client.Groups[1].Value);
                    int clientHeight = int.Parse(client.Groups[2].Value);
                    int minimumWidth = int.Parse(minimum.Groups[1].Value);
                    int minimumHeight = int.Parse(minimum.Groups[2].Value);

                    if (minimumWidth > clientWidth || minimumHeight > clientHeight)
                    {
                        throw new InvalidOperationException(
                            "MinimumSize exceeds base form size in " +
                            Path.GetFileName(designerPath) + ".");
                    }
                }

                checkedFiles++;
            }

            Console.WriteLine("[PASS] Designer geometry: " + checkedFiles + " Designer files checked");
        }

        private static void ValidateButtonFlowContainers()
        {
            string projectRoot = Path.GetFullPath(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", ".."));

            string uiRoot = Path.Combine(projectRoot, "UI");
            int checkedPanels = 0;

            foreach (string designerPath in Directory.GetFiles(
                uiRoot,
                "*.Designer.cs",
                SearchOption.AllDirectories))
            {
                string source = File.ReadAllText(designerPath);

                MatchCollection flowPanels = Regex.Matches(
                    source,
                    @"this\.(\w+)\s*=\s*new\s+System\.Windows\.Forms\.FlowLayoutPanel\(\);");

                foreach (Match flow in flowPanels)
                {
                    string panelName = flow.Groups[1].Value;

                    bool containsButton =
                        Regex.IsMatch(
                            source,
                            @"this\." + Regex.Escape(panelName) +
                            @"\.Controls\.Add\(this\.(?:btn|kryptonBtn)[A-Za-z0-9_]*\)");

                    if (!containsButton)
                        continue;

                    bool autoScroll =
                        Regex.IsMatch(
                            source,
                            @"this\." + Regex.Escape(panelName) +
                            @"\.AutoScroll\s*=\s*true");

                    Match wrapMatch =
                        Regex.Match(
                            source,
                            @"this\." + Regex.Escape(panelName) +
                            @"\.WrapContents\s*=\s*(true|false)");

                    // القيمة الافتراضية لـFlowLayoutPanel.WrapContents هي true.
                    bool wrap =
                        !wrapMatch.Success ||
                        string.Equals(
                            wrapMatch.Groups[1].Value,
                            "true",
                            StringComparison.OrdinalIgnoreCase);

                    if (!autoScroll && !wrap)
                    {
                        throw new InvalidOperationException(
                            "Button FlowLayoutPanel has neither AutoScroll nor WrapContents in " +
                            Path.GetFileName(designerPath) + ": " + panelName);
                    }

                    checkedPanels++;
                }
            }

            Console.WriteLine("[PASS] Button flow containers: " + checkedPanels + " containers checked");
        }

        private static void ValidateSourceWiring()
        {
            string projectRoot = Path.GetFullPath(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", ".."));

            List<string> codeFiles = Directory
                .GetFiles(projectRoot, "*.cs", SearchOption.AllDirectories)
                .Where(path =>
                    path.IndexOf(Path.DirectorySeparatorChar + "UI" + Path.DirectorySeparatorChar,
                        StringComparison.OrdinalIgnoreCase) >= 0 ||
                    string.Equals(
                        Path.GetFileName(path),
                        "MainForm.cs",
                        StringComparison.OrdinalIgnoreCase))
                .Where(path =>
                    path.IndexOf(Path.DirectorySeparatorChar + "bin" + Path.DirectorySeparatorChar,
                        StringComparison.OrdinalIgnoreCase) < 0 &&
                    path.IndexOf(Path.DirectorySeparatorChar + "obj" + Path.DirectorySeparatorChar,
                        StringComparison.OrdinalIgnoreCase) < 0 &&
                    !Path.GetFileName(path).EndsWith(".Designer.cs", StringComparison.OrdinalIgnoreCase))
                .ToList();

            int checkedHandlers = 0;

            foreach (string sourcePath in codeFiles)
            {
                string source = File.ReadAllText(sourcePath);
                string designerPath = Path.Combine(
                    Path.GetDirectoryName(sourcePath),
                    Path.GetFileNameWithoutExtension(sourcePath) + ".Designer.cs");

                if (File.Exists(designerPath))
                    source += Environment.NewLine + File.ReadAllText(designerPath);

                MatchCollection handlers = Regex.Matches(
                    source,
                    @"\b(?:private|protected|public|internal)\s+(?:async\s+)?void\s+(\w+_(?:Click|TextChanged|SelectedIndexChanged|SelectedValueChanged|ValueChanged|CellClick|DoubleClick|KeyPress|Load))\s*\(",
                    RegexOptions.Multiline);

                foreach (Match handler in handlers)
                {
                    string name = handler.Groups[1].Value;
                    int references = Regex.Matches(
                        source,
                        @"\b" + Regex.Escape(name) + @"\b").Count;

                    if (references < 2)
                    {
                        throw new InvalidOperationException(
                            "Unwired UI event handler: " + name +
                            " in " + Path.GetFileName(sourcePath));
                    }

                    checkedHandlers++;
                }
            }

            Console.WriteLine("[PASS] Source event wiring: " + checkedHandlers + " handlers checked");
        }

        private static void ValidateDualDateSupport()
        {
            DateTime source = new DateTime(2026, 10, 7);

            string dual = UIHelper.FormatDateDual(source);
            if (string.IsNullOrWhiteSpace(dual) ||
                dual.IndexOf("م", StringComparison.Ordinal) < 0 ||
                dual.IndexOf("هـ", StringComparison.Ordinal) < 0)
            {
                throw new InvalidOperationException(
                    "Dual date formatting must contain both Gregorian and Hijri values.");
            }

            string[] parts = dual.Split('|');
            if (parts.Length != 2)
                throw new InvalidOperationException("Dual date format must expose two calendar values.");

            // Use the helper's accepted Hijri form for a real round-trip test.
            string hijriValue = new string(
                dual
                    .Substring(
                        dual.LastIndexOf("|", StringComparison.Ordinal) + 1)
                    .Trim()
                    .Where(ch => char.IsDigit(ch) || ch == '/')
                    .ToArray());

            if (!UIHelper.TryParseDate(hijriValue, out DateTime parsed) ||
                parsed.Date != source.Date)
            {
                throw new InvalidOperationException(
                    "Hijri date parsing does not round-trip to the original Gregorian date.");
            }

            if (!UIHelper.TryParseDate("07/10/2026", out DateTime gregorian) ||
                gregorian.Date != source.Date)
            {
                throw new InvalidOperationException(
                    "Gregorian date parsing failed.");
            }

            Console.WriteLine("[PASS] Gregorian/Hijri date formatting and parsing");
        }

        private static void ValidateButtonWiring()
        {
            string projectRoot = Path.GetFullPath(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", ".."));

            string uiRoot = Path.Combine(projectRoot, "UI");
            int checkedButtons = 0;

            foreach (string designerPath in Directory.GetFiles(
                uiRoot,
                "*.Designer.cs",
                SearchOption.AllDirectories))
            {
                string source = File.ReadAllText(designerPath);
                string codePath = Path.Combine(
                    Path.GetDirectoryName(designerPath),
                    Path.GetFileNameWithoutExtension(designerPath) + ".cs");

                if (File.Exists(codePath))
                    source += Environment.NewLine + File.ReadAllText(codePath);

                MatchCollection buttons = Regex.Matches(
                    source,
                    @"this\.(btn[A-Za-z0-9_]+)\s*=\s*new\s+(?:System\.Windows\.Forms\.)?Button\(\);");

                foreach (Match button in buttons)
                {
                    string name = button.Groups[1].Value;
                    bool hasClickSubscription =
                        Regex.IsMatch(
                            source,
                            @"(?:this\.)?" + Regex.Escape(name) +
                            @"\.Click\s*\+=");

                    bool hasClickHandler =
                        Regex.IsMatch(
                            source,
                            @"\b(?:private|protected|public|internal)\s+(?:async\s+)?void\s+" +
                            Regex.Escape(name) +
                            @"_Click\s*\(");

                    if (!hasClickSubscription && !hasClickHandler)
                    {
                        throw new InvalidOperationException(
                            "Button has no click wiring: " +
                            name +
                            " in " +
                            Path.GetFileName(designerPath));
                    }

                    checkedButtons++;
                }
            }

            Console.WriteLine("[PASS] Button wiring: " + checkedButtons + " buttons checked");
        }

        private static void ValidateDatePickers(Control root)
        {
            foreach (Control control in GetAllControls(root))
            {
                DateTimePicker picker = control as DateTimePicker;
                if (picker == null)
                    continue;

                string key = (picker.Name ?? string.Empty).ToLowerInvariant();
                string format = (picker.CustomFormat ?? string.Empty).ToLowerInvariant();

                bool timeOnly =
                    format.Contains("h") ||
                    format.Contains("minute") ||
                    format.Contains("second") ||
                    key.Contains("start") ||
                    key.Contains("end") ||
                    key.Contains("departure") ||
                    key.Contains("arrival") ||
                    key.Contains("checkin") ||
                    key.Contains("checkout") ||
                    key.Contains("وقت") ||
                    key.Contains("بداية") ||
                    key.Contains("نهاية");

                if (timeOnly)
                    continue;

                if (picker.Format != DateTimePickerFormat.Custom ||
                    !string.Equals(picker.CustomFormat, "dd/MM/yyyy", StringComparison.Ordinal))
                {
                    throw new InvalidOperationException(
                        "Date picker is not using the standard Gregorian date format: " +
                        picker.Name);
                }

                if (picker.ContextMenuStrip == null ||
                    !picker.ContextMenuStrip.Items.Cast<ToolStripItem>()
                        .Any(item => string.Equals(
                            item.Text,
                            "إدخال تاريخ هجري",
                            StringComparison.Ordinal)))
                {
                    throw new InvalidOperationException(
                        "Date picker has no Hijri input command: " +
                        picker.Name);
                }

                if (string.IsNullOrWhiteSpace(picker.AccessibleDescription) ||
                    picker.AccessibleDescription.IndexOf("هـ", StringComparison.Ordinal) < 0)
                {
                    throw new InvalidOperationException(
                        "Date picker has no dual Gregorian/Hijri description: " +
                        picker.Name);
                }
            }
        }

        private static void ValidateTextFit(Control root)
        {
            foreach (Control control in GetAllControls(root))
            {
                Button button = control as Button;
                if (button == null ||
                    !button.Visible ||
                    string.IsNullOrWhiteSpace(button.Text) ||
                    button.AutoSize ||
                    string.Equals(button.Name, "btnWindowClose", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(button.Text.Trim(), "×", StringComparison.Ordinal))
                    continue;

                System.Drawing.Size measured = System.Windows.Forms.TextRenderer.MeasureText(
                    button.Text,
                    button.Font);

                int requiredWidth = Math.Max(
                    92,
                    measured.Width + button.Padding.Horizontal + 12);

                int requiredHeight = Math.Max(
                    36,
                    measured.Height + button.Padding.Vertical + 8);

                if (button.Width < requiredWidth || button.Height < requiredHeight)
                {
                    throw new InvalidOperationException(
                        "Button text may be clipped: " + button.Name +
                        " (" + button.Width + "x" + button.Height +
                        ", required " + requiredWidth + "x" + requiredHeight + ").");
                }
            }
        }

        private static IEnumerable<Control> GetAllControls(Control parent)
        {
            foreach (Control child in parent.Controls)
            {
                yield return child;

                foreach (Control nested in GetAllControls(child))
                    yield return nested;
            }
        }

        private static void ValidateLoginLayout(Control control)
        {
            if (!(control is SchoolSystem.UI.LoginForm))
                return;

            Button closeButton = GetPrivateControl<Button>(control, "btnWindowClose");
            Panel background = GetPrivateControl<Panel>(control, "panelBackground");
            TableLayoutPanel root = GetPrivateControl<TableLayoutPanel>(control, "layoutRoot");
            Panel brand = GetPrivateControl<Panel>(control, "panelBrand");
            Panel loginArea = GetPrivateControl<Panel>(control, "panelLoginArea");
            TextBox userName = GetPrivateControl<TextBox>(control, "txtUserName");
            TextBox password = GetPrivateControl<TextBox>(control, "txtPassword");
            Button toggle = GetPrivateControl<Button>(control, "btnTogglePassword");
            Panel passwordPanel = GetPrivateControl<Panel>(control, "pnlPassword");
            Panel userPanel = GetPrivateControl<Panel>(control, "pnlUser");
            TableLayoutPanel bottomActions = GetPrivateControl<TableLayoutPanel>(control, "bottomActions");
            Panel loginCard = GetPrivateControl<Panel>(control, "loginCard");

            if (root.RightToLeft != RightToLeft.Yes)
                throw new InvalidOperationException("Login root must use Arabic RTL column order.");

            if (brand.Parent != root || loginArea.Parent != root)
                throw new InvalidOperationException("Login brand and form areas must be direct children of the RTL root.");

            if (closeButton.Parent != background)
                throw new InvalidOperationException("Login close button must be attached to the window background.");

            if (closeButton.RightToLeft != RightToLeft.No ||
                closeButton.Anchor != (AnchorStyles.Top | AnchorStyles.Right))
                throw new InvalidOperationException("Login close button must stay at the window top-right.");

            if (loginCard.Parent != GetPrivateControl<TableLayoutPanel>(control, "loginAreaLayout"))
                throw new InvalidOperationException("Login card must be centered inside the login area layout.");

            if (userName.RightToLeft != RightToLeft.Yes ||
                userName.TextAlign != HorizontalAlignment.Right ||
                password.RightToLeft != RightToLeft.Yes ||
                password.TextAlign != HorizontalAlignment.Right)
            {
                throw new InvalidOperationException("Login text fields must be Arabic RTL and right-aligned.");
            }

            if (userPanel.RightToLeft != RightToLeft.Yes ||
                passwordPanel.RightToLeft != RightToLeft.Yes)
                throw new InvalidOperationException("Login input panels must use Arabic RTL.");

            if (toggle.Dock != DockStyle.Left)
                throw new InvalidOperationException("Password visibility action must stay at the left side of the password field.");

            if (bottomActions.RightToLeft != RightToLeft.Yes)
                throw new InvalidOperationException("Login secondary actions must use Arabic RTL order.");

            int[] expectedTabs = { 0, 1, 2, 3, 4, 5 };
            int[] actualTabs =
            {
                userName.TabIndex,
                password.TabIndex,
                toggle.TabIndex,
                GetPrivateControl<Button>(control, "btnLogin").TabIndex,
                GetPrivateControl<Button>(control, "btnClear").TabIndex,
                GetPrivateControl<Button>(control, "btnExit").TabIndex
            };

            for (int i = 0; i < expectedTabs.Length; i++)
            {
                if (actualTabs[i] != expectedTabs[i])
                    throw new InvalidOperationException("Login keyboard tab order is not sequential.");
            }

            Console.WriteLine("[PASS] Login RTL visual hierarchy and keyboard order");
        }


        private static void ValidateLoginInteractionSurface(Control control)
        {
            if (!(control is SchoolSystem.UI.LoginForm))
                return;

            TextBox userName = GetPrivateControl<TextBox>(control, "txtUserName");
            TextBox password = GetPrivateControl<TextBox>(control, "txtPassword");
            Button toggle = GetPrivateControl<Button>(control, "btnTogglePassword");
            Button clear = GetPrivateControl<Button>(control, "btnClear");
            Button exit = GetPrivateControl<Button>(control, "btnExit");
            Button login = GetPrivateControl<Button>(control, "btnLogin");

            if (!string.Equals(userName.Text, "اسم المستخدم", StringComparison.Ordinal) ||
                !string.Equals(password.Text, "كلمة المرور", StringComparison.Ordinal))
            {
                throw new InvalidOperationException("Login placeholders must be initialized in Arabic.");
            }

            if (password.PasswordChar != ' ')
                throw new InvalidOperationException("Password placeholder must not be masked.");

            if (toggle.Width < 92 || clear.Height < 36 || exit.Height < 36)
                throw new InvalidOperationException("Login action controls are too small for their captions.");

            if (control.RightToLeft != RightToLeft.Yes ||
                !control.RightToLeftLayout ||
                control.AcceptButton != login ||
                control.CancelButton != exit)
            {
                throw new InvalidOperationException("Login form Arabic direction/chrome configuration is incomplete.");
            }

            password.Text = "DemoPassword123";
            password.PasswordChar = '●';
            toggle.Text = "إخفاء";
            toggle.PerformClick();

            if (password.PasswordChar != ' ' ||
                !string.Equals(toggle.Text, "إظهار", StringComparison.Ordinal))
            {
                throw new InvalidOperationException("Password visibility toggle did not expose the password state correctly.");
            }

            toggle.PerformClick();

            if (password.PasswordChar == ' ' ||
                !string.Equals(toggle.Text, "إخفاء", StringComparison.Ordinal))
            {
                throw new InvalidOperationException("Password visibility toggle did not restore the masked state.");
            }

            clear.PerformClick();

            if (!string.Equals(password.Text, "كلمة المرور", StringComparison.Ordinal) ||
                password.PasswordChar != ' ' ||
                !string.Equals(toggle.Text, "إظهار", StringComparison.Ordinal))
            {
                throw new InvalidOperationException("Clear action did not restore the password input state.");
            }

            Console.WriteLine("[PASS] Login interactions and RTL controls");
        }

        private static void ValidateStudentProfile(Control control)
        {
            if (!(control is SchoolSystem.UI.StudentProfileForm))
                return;

            TabControl tabs = GetPrivateControl<TabControl>(control, "tabs");
            DataGridView attendance = GetPrivateControl<DataGridView>(control, "dgvAttendance");
            DataGridView grades = GetPrivateControl<DataGridView>(control, "dgvMarks");
            DataGridView fees = GetPrivateControl<DataGridView>(control, "dgvFees");
            TabPage attendanceTab = GetPrivateControl<TabPage>(control, "attendanceTab");
            TabPage marksTab = GetPrivateControl<TabPage>(control, "marksTab");
            TabPage feesTab = GetPrivateControl<TabPage>(control, "feesTab");

            if (attendance.Parent != attendanceTab)
                throw new InvalidOperationException("StudentProfile attendance grid is not hosted by attendanceTab.");
            if (grades.Parent != marksTab)
                throw new InvalidOperationException("StudentProfile academic grid is not hosted by marksTab.");
            if (fees.Parent != feesTab)
                throw new InvalidOperationException("StudentProfile fees grid is not hosted by feesTab.");

            if (!attendance.Visible || !grades.Visible || !fees.Visible)
                throw new InvalidOperationException("StudentProfile data grids must be visible after construction.");

            if (attendance.Dock != DockStyle.Fill || grades.Dock != DockStyle.Fill || fees.Dock != DockStyle.Fill)
                throw new InvalidOperationException("StudentProfile data grids must fill their tab pages.");

            if (tabs.TabPages.Count < 3)
                throw new InvalidOperationException("StudentProfile must expose attendance, academic grades, and fees tabs.");
        }

        private static void ValidateDashboard(Control control)
        {
            if (!(control is SchoolSystem.UI.DashboardHome))
                return;

            FlowLayoutPanel quickActions =
                GetPrivateControl<FlowLayoutPanel>(
                    control,
                    "quickActionsPanel");

            Panel summary =
                GetPrivateControl<Panel>(
                    control,
                    "panelSummary");

            FlowLayoutPanel cards =
                GetPrivateControl<FlowLayoutPanel>(
                    control,
                    "panelCards");

            if (summary.Height < 140)
                throw new InvalidOperationException(
                    "Dashboard summary area is too short for the header and quick actions.");

            if (quickActions.Height < 40)
                throw new InvalidOperationException(
                    "Dashboard quick-action bar is too short.");

            if (!quickActions.Controls.Cast<Control>()
                .Any(item => string.Equals(
                    item.Name,
                    "lblQuickActionsTitle",
                    StringComparison.Ordinal)))
            {
                throw new InvalidOperationException(
                    "Dashboard quick-action title is missing.");
            }

            int cardCount =
                cards.Controls.Cast<Control>()
                    .Count(controlItem =>
                        controlItem is Panel &&
                        controlItem.Name.StartsWith(
                            "card",
                            StringComparison.OrdinalIgnoreCase));

            if (cardCount != 5)
                throw new InvalidOperationException(
                    "Dashboard must expose five KPI cards. Found: " + cardCount);

            Control title =
                GetPrivateControl<Control>(
                    control,
                    "lblDashboardTitle");

            if (string.IsNullOrWhiteSpace(title.Text))
                throw new InvalidOperationException(
                    "Dashboard title must not be empty.");

            Console.WriteLine("[PASS] Dashboard structure and quick actions");
        }

        private static void ValidatePayrollScreens(Control control)
        {
            if (control is SchoolSystem.UI.PayrollForm)
            {
                AssertControlText(control, "lblTitle", "إدارة العقود الوظيفية");
                AssertControlText(control, "groupBoxContract", "بيانات العقد الوظيفي والراتب المتفق عليه");
                AssertControlText(control, "btnAdd", "إضافة عقد");
                AssertControlText(control, "btnUpdate", "تعديل العقد");
                AssertControlText(control, "btnDelete", "حذف العقد");
                AssertControlText(control, "btnClear", "تفريغ الحقول");
                AssertControlText(control, "lblSearch", "بحث عن عقد:");

                TextBox total = GetPrivateControl<TextBox>(control, "txtTotal");
                TextBox net = GetPrivateControl<TextBox>(control, "txtNetSalary");
                if (!total.ReadOnly || !net.ReadOnly || total.TabStop || net.TabStop)
                    throw new InvalidOperationException("Calculated contract salary fields must be read-only.");
            }
            else if (control is SchoolSystem.UI.PayrollRecordsForm)
            {
                AssertControlText(control, "lblTitle", "مسير الرواتب وسجلات الصرف");
                AssertControlText(control, "groupBoxPayroll", "بيانات الراتب الشهري");
                AssertControlText(control, "btnAdd", "تسجيل وصرف الراتب");
                AssertControlText(control, "btnRefresh", "تحديث السجلات");
                AssertControlText(control, "btnClear", "تفريغ الحقول");
                AssertControlText(control, "lblSearch", "بحث في سجلات الرواتب");

                DateTimePicker paymentDate =
                    GetPrivateControl<DateTimePicker>(control, "dtpPaymentDate");
                if (paymentDate.MaxDate.Date > DateTime.Today)
                    throw new InvalidOperationException("تاريخ صرف الراتب يسمح بتاريخ مستقبلي.");
            }
        }

        private static void AssertControlText(Control root, string fieldName, string expected)
        {
            Control control = GetPrivateControl<Control>(root, fieldName);
            if (!string.Equals(control.Text, expected, StringComparison.Ordinal))
                throw new InvalidOperationException(
                    fieldName + " expected '" + expected + "' but was '" + control.Text + "'.");
        }

        private static T GetPrivateControl<T>(Control root, string fieldName)
            where T : class
        {
            FieldInfo field = root.GetType().GetField(
                fieldName,
                BindingFlags.Instance | BindingFlags.NonPublic);

            if (field == null)
                throw new MissingFieldException(root.GetType().FullName, fieldName);

            T value = field.GetValue(root) as T;
            if (value == null)
                throw new InvalidOperationException("Field " + fieldName + " is not the expected control type.");

            return value;
        }

        private static int Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Type baseControlType = typeof(Control);
            Assembly assembly = typeof(SchoolSystem.UI.StudentsForm).Assembly;

            List<Type> screenTypes =
                assembly.GetTypes()
                    .Where(type =>
                        type.IsClass &&
                        !type.IsAbstract &&
                        type.IsPublic &&
                        baseControlType.IsAssignableFrom(type) &&
                        type.Namespace != null &&
                        type.Namespace.StartsWith(
                            "SchoolSystem.UI",
                            StringComparison.Ordinal))
                    .OrderBy(type => type.Name)
                    .ToList();

            int passed = 0;
            int failed = 0;
            int manual = 0;

            try
            {
                Assembly.Load("iTextSharp.LGPLv2.Core, Version=3.8.1.0, Culture=neutral, PublicKeyToken=51d712e21b66ad36");
                passed++;
                Console.WriteLine("[PASS] PDF runtime dependency");
            }
            catch (Exception ex)
            {
                failed++;
                Exception root = ex;
                while (root.InnerException != null)
                    root = root.InnerException;

                Console.WriteLine(
                    "[FAIL] PDF runtime dependency: " +
                    root.GetType().Name + " - " +
                    root.Message);
            }

            string pdfAssemblyPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "iTextSharp.LGPLv2.Core.dll");

            if (File.Exists(pdfAssemblyPath))
            {
                passed++;
                Console.WriteLine("[PASS] PDF dependency: iTextSharp.LGPLv2.Core.dll");
            }
            else
            {
                failed++;
                Console.WriteLine(
                    "[FAIL] PDF dependency: iTextSharp.LGPLv2.Core.dll is missing from output.");
            }

            Console.WriteLine("SchoolSystem UI Smoke Test");
            Console.WriteLine("Screens discovered: " + screenTypes.Count);
            Console.WriteLine("UI audit: constructor + layout + source event wiring");

            try
            {
                using (SchoolSystem.MainForm mainForm = new SchoolSystem.MainForm())
                {
                    if (mainForm.IsDisposed)
                        throw new ObjectDisposedException("MainForm");

                    ValidateLayout(mainForm);
                }

                passed++;
                Console.WriteLine("[PASS] MainForm");
            }
            catch (Exception ex)
            {
                failed++;
                Exception root = ex;
                while (root.InnerException != null)
                    root = root.InnerException;

                Console.WriteLine(
                    "[FAIL] MainForm: " +
                    root.GetType().Name + " - " +
                    root.Message);
            }

            try
            {
                ValidateDesignerGeometry();
                ValidateButtonFlowContainers();
                ValidateSourceWiring();
                ValidateButtonWiring();
                ValidateDualDateSupport();
                ValidateDualDateSupport();

                using (SchoolSystem.UI.LoginForm loginForm = new SchoolSystem.UI.LoginForm())
                {
                    ValidateLayout(loginForm);
                    ValidateTextFit(loginForm);
                    ValidateLoginLayout(loginForm);
                    ValidateLoginInteractionSurface(loginForm);
                }

                passed++;
            }
            catch (Exception ex)
            {
                failed++;
                Console.WriteLine("[FAIL] Source/UI wiring audit: " + ex.Message);
            }

            foreach (Type type in screenTypes)
            {
                try
                {
                    if (string.Equals(type.Name, "LoginForm", StringComparison.Ordinal))
                    {
                        manual++;
                        Console.WriteLine("[MANUAL] " + type.Name + " - requires configured database during startup.");
                        continue;
                    }

                    using (Control control = (Control)Activator.CreateInstance(type))
                    {
                        if (control == null)
                            throw new InvalidOperationException("Constructor returned null.");

                        if (control.IsDisposed)
                            throw new ObjectDisposedException(type.Name);

                        ValidateLayout(control);
                        ValidateTextFit(control);
                        ValidateDatePickers(control);
                        ValidateDashboard(control);
                        ValidatePayrollScreens(control);
                        ValidateStudentProfile(control);
                    }

                    passed++;
                    Console.WriteLine("[PASS] " + type.Name);
                }
                catch (Exception ex)
                {
                    failed++;
                    Exception root = ex;
                    while (root.InnerException != null)
                        root = root.InnerException;

                    Console.WriteLine(
                        "[FAIL] " + type.Name + ": " +
                        root.GetType().Name + " - " +
                        root.Message);
                }
            }

            Console.WriteLine(
                "RESULT: Passed=" + passed +
                ", Failed=" + failed +
                ", Manual=" + manual);

            return failed == 0 ? 0 : 1;
        }

    }
}