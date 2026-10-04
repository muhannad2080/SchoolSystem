using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SchoolSystem.UI;
using SchoolSystem.Helpers;
using SchoolSystem.Security;


namespace SchoolSystem.UI
{

    public partial class WelcomeScreen : UserControl
    {
        public string SystemName
        {
            get => lblSystemName.Text;

            set => lblSystemName.Text = value;
        }

        // تعريف الدالة WelcomeScreen لتنفيذ العملية المرتبطة بها.
        public WelcomeScreen()
        {

            InitializeComponent();
            LoadWelcomeBackground();

            SchoolSystem.Helpers.UIHelper.ApplyStyle(this);
            this.Dock = DockStyle.Fill;
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            ApplyPermissionVisibility();
        }


        private void LoadWelcomeBackground()
        {
            string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Welcome", "welcome_hero.png");
            if (!File.Exists(imagePath))
            {
                ClearWelcomeImage();
                return;
            }

            try
            {
                Image loadedImage;

                using (FileStream stream = File.OpenRead(imagePath))
                using (Image sourceImage = Image.FromStream(stream))
                    loadedImage = new Bitmap(sourceImage);

                Image previousImage = pictureWelcomeHero.Image;
                pictureWelcomeHero.Image = loadedImage;
                if (previousImage != null)
                    previousImage.Dispose();
            }
            catch (ArgumentException ex)
            {
                ApplicationLogger.LogException("شاشة ترحيبية: تحميل الخلفية", ex);

                ClearWelcomeImage();
            }
            catch (ExternalException ex)
            {
                ApplicationLogger.LogException("شاشة ترحيبية: تحميل الخلفية", ex);

                ClearWelcomeImage();
            }
            catch (IOException ex)
            {
                ApplicationLogger.LogException("شاشة ترحيبية: تحميل الخلفية", ex);

                ClearWelcomeImage();
            }
            catch (UnauthorizedAccessException ex)
            {
                ApplicationLogger.LogException("شاشة ترحيبية: تحميل الخلفية", ex);
                // العودة للحالة الافتراضية عند عدم امتلاك صلاحية قراءة الملف.
                ClearWelcomeImage();
            }
            catch (OutOfMemoryException ex)
            {
                ApplicationLogger.LogException("شاشة ترحيبية: تحميل الخلفية", ex);

                ClearWelcomeImage();
            }
        }


        private void ClearWelcomeImage()
        {
            Image oldImage = pictureWelcomeHero.Image;
            pictureWelcomeHero.Image = null;
            if (oldImage != null)
                oldImage.Dispose();
        }

        // تعريف الدالة ApplyPermissionVisibility لتنفيذ العملية المرتبطة بها.
        private void ApplyPermissionVisibility()
        {
            // هذه الأزرار اختصارات وليست قائمة ثابتة؛ يجب ألا تظهر لأي مستخدم
            // إلا إذا كان يملك صلاحية فعلية للوحدة المرتبطة بها.
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            btnStudents.Visible = CurrentUser.CanAccessModule("Students");
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            btnTeachers.Visible = CurrentUser.CanAccessModule("Teachers");
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            btnFinance.Visible = CurrentUser.CanAccessModule("Fees")
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                || CurrentUser.CanAccessModule("Vouchers")
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                || CurrentUser.CanAccessModule("Expenses");
            // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
            btnAttendance.Visible = CurrentUser.CanAccessModule("Attendance");
        }

        // تعريف الدالة CanOpen لتنفيذ العملية المرتبطة بها.
        private bool CanOpen(string module, string message)
        {
            if (CurrentUser.CanAccessModule(module))
                return true;


            UIHelper.ShowWarning(message);

            return false;
        }

        // تعريف الدالة btnStudents_Click لتنفيذ العملية المرتبطة بها.
        private void btnStudents_Click(object sender, EventArgs e)
        {
            if (CanOpen("Students", "ليس لديك صلاحية عرض الطلاب."))
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                MainForm.Instance?.LoadFormInPanel("Students", "ليس لديك صلاحية عرض الطلاب.", new StudentsForm(), PermissionKeys.StudentsView, PermissionKeys.StudentsManage);
        }

        // تعريف الدالة btnTeachers_Click لتنفيذ العملية المرتبطة بها.
        private void btnTeachers_Click(object sender, EventArgs e)
        {
            if (CanOpen("Teachers", "ليس لديك صلاحية عرض المعلمين."))
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                MainForm.Instance?.LoadUserControl("Teachers", "ليس لديك صلاحية عرض المعلمين.", new TeachersForm(), PermissionKeys.TeachersManage);
        }

        // تعريف الدالة btnFinance_Click لتنفيذ العملية المرتبطة بها.
        private void btnFinance_Click(object sender, EventArgs e)
        {
            if (CurrentUser.CanAccessModule("Fees"))
            {
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                MainForm.Instance?.LoadUserControl("Fees", "ليس لديك صلاحية عرض الرسوم.", new FeesForm(), PermissionKeys.FeesManage);

                return;
            }

            if (CurrentUser.CanAccessModule("Vouchers"))
            {
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                MainForm.Instance?.LoadUserControl("Vouchers", "ليس لديك صلاحية عرض السندات.", new VouchersForm(), PermissionKeys.VouchersManage);

                return;
            }

            if (CurrentUser.CanAccessModule("Expenses"))
            {
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                MainForm.Instance?.LoadUserControl("Expenses", "ليس لديك صلاحية عرض المصروفات.", new ExpensesForm(), PermissionKeys.ExpensesManage);

                return;
            }


            UIHelper.ShowWarning("ليس لديك صلاحية عرض الوحدة المالية.");
        }

        // تعريف الدالة btnAttendance_Click لتنفيذ العملية المرتبطة بها.
        private void btnAttendance_Click(object sender, EventArgs e)
        {
            if (CanOpen("Attendance", "ليس لديك صلاحية عرض الحضور."))
                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                MainForm.Instance?.LoadUserControl("Attendance", "ليس لديك صلاحية عرض الحضور.", new DailyAttendanceForm(), PermissionKeys.AttendanceManage);
        }
    }
}
