using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
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
                    using (Control control = CreateControlWithoutRunningBusinessLoad(type))
                    {
                        MethodInfo initialize =
                            type.GetMethod(
                                "InitializeComponent",
                                BindingFlags.Instance |
                                BindingFlags.Public |
                                BindingFlags.NonPublic);

                        if (initialize == null)
                            throw new MissingMethodException(
                                type.FullName,
                                "InitializeComponent");

                        initialize.Invoke(control, null);
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

        private static Control CreateControlWithoutRunningBusinessLoad(Type type)
        {
            object instance =
                FormatterServices.GetUninitializedObject(type);

            return (Control)instance;
        }
    }
}
