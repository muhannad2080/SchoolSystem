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
                    @"Location\s*=\s*new\s+(?:System\.)?Drawing\.Point\(\s*(-\d+)\s*,\s*(-?\d+)\s*\)");

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
                    @"this\.ClientSize\s*=\s*new\s+(?:System\.)?Drawing\.Size\(\s*(\d+)\s*,\s*(\d+)\s*\)");

                Match minimum = Regex.Match(
                    source,
                    @"this\.MinimumSize\s*=\s*new\s+(?:System\.)?Drawing\.Size\(\s*(\d+)\s*,\s*(\d+)\s*\)");

                if (client.Success && minimum.Success)
                {
                    int clientWidth = int.Parse(client.Groups[1].Value);
                    int clientHeight = int.Parse(client.Groups[2].Value);
                    int minimumWidth = int.Parse(minimum.Groups[1].Value);
                    int minimumHeight = int.Parse(minimum.Groups[2].Value);

                    if (minimumWidth > clientWidth || minimumHeight > clientHeight)
                    {
                        throw new InvalidOperationException(
                            "MinimumSize exceeds ClientSize in " +
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

                    bool wrap =
                        Regex.IsMatch(
                            source,
                            @"this\." + Regex.Escape(panelName) +
                            @"\.WrapContents\s*=\s*true");

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

        private static void ValidateTextFit(Control root)
        {
            foreach (Control control in GetAllControls(root))
            {
                Button button = control as Button;
                if (button == null || !button.Visible || string.IsNullOrWhiteSpace(button.Text) || button.AutoSize)
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
                passed++;
            }
            catch (Exception ex)
            {
                failed++;
                Console.WriteLine("[FAIL] Source event wiring: " + ex.Message);
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
