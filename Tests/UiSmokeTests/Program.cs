using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace SchoolSystem.UiSmokeTests
{
    internal static class Program
    {
        [STAThread]
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

            Console.WriteLine("SchoolSystem UI Smoke Test");
            Console.WriteLine("Screens discovered: " + screenTypes.Count);

            try
            {
                using (SchoolSystem.MainForm mainForm = new SchoolSystem.MainForm())
                {
                    if (mainForm.IsDisposed)
                        throw new ObjectDisposedException("MainForm");
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

                        ValidatePayrollScreens(control);
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
