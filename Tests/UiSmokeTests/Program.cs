using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SchoolSystem.UiSmokeTests
{
    internal static class Program
    {
        [STAThread]
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
                        type.Namespace == "SchoolSystem.UI")
                    .OrderBy(type => type.Name)
                    .ToList();

            int passed = 0;
            int failed = 0;

            Console.WriteLine("SchoolSystem UI Smoke Test");
            Console.WriteLine("Screens discovered: " + screenTypes.Count);

            foreach (Type type in screenTypes)
            {
                try
                {
                    if (string.Equals(type.Name, "LoginForm", StringComparison.Ordinal))
                    {
                        Console.WriteLine("[MANUAL] " + type.Name + " - requires configured database during startup.");
                        continue;
                    }

                    using (Control control = (Control)Activator.CreateInstance(type))
                    {
                        if (control == null)
                            throw new InvalidOperationException("Constructor returned null.");

                        if (control.IsDisposed)
                            throw new ObjectDisposedException(type.Name);
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
                ", Failed=" + failed);

            return failed == 0 ? 0 : 1;
        }

    }
}
