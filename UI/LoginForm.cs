using System;
using System.Drawing;
using System.Windows.Forms;
using SchoolSystem.Models;
using SchoolSystem.Services;
using SchoolSystem.Security;
using SchoolSystem.Helpers;

namespace SchoolSystem.UI
{
    public partial class LoginForm : Form
    {
        // تعريف الدالة UserService لتنفيذ العملية المرتبطة بها.
        private readonly UserService userService = new UserService();

        // يُضبط على true فقط عندما يختار المستخدم صراحةً إنهاء النظام عبر زر "خروج".
        public bool AppShouldExit { get; private set; }

        // تعريف الدالة LoginForm لتنفيذ العملية المرتبطة بها.
        public LoginForm()
        {
            InitializeComponent();

            // ============================================================
            // ملاحظة:
            // تم إزالة جميع أوامر التنسيق والتصميم من هنا حتى يكون
            // LoginForm.Designer.cs هو المسؤول بالكامل عن شكل الواجهة.
            //
            // لم يتم تغيير أي منطق متعلق بتسجيل الدخول أو التحقق
            // أو الأحداث أو معالجة الأخطاء.
            // ============================================================

            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                userService.EnsureDefaultAdmin();
            }
            catch (Exception ex)
            {
                ApplicationLogger.LogException("تهيئة مدير النظام في شاشة الدخول", ex);
                ShowInitializationError(ex);
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }

            this.AcceptButton = btnLogin;
            this.CancelButton = btnExit;

            // تقييد أطوال حقول الدخول.
            txtUserName.MaxLength = 100;
            txtPassword.MaxLength = 128;

            this.Shown += (s, e) =>
            {
                txtUserName.Focus();
                txtUserName.SelectAll();
            };
        }

        // تعريف الدالة ShowInitializationError لتنفيذ العملية المرتبطة بها.
        private void ShowInitializationError(Exception exception)
        {
            string message = exception == null ? string.Empty : exception.Message ?? string.Empty;

            if (message.Contains("SCHOOL_SYSTEM_INITIAL_ADMIN_PASSWORD"))
            {
                ShowError(
                    "لم يتم إنشاء مدير النظام الأول. عيّن متغير البيئة SCHOOL_SYSTEM_INITIAL_ADMIN_PASSWORD بطول 10 أحرف على الأقل، ثم أعد تشغيل البرنامج.",
                    "تهيئة مدير النظام");

                return;
            }

            ShowError(
                "تعذر تهيئة بيانات الدخول. تحقق من اتصال قاعدة البيانات وتأكد من تنفيذ محرك الترحيل المدمج ثم حاول مرة أخرى.",
                "تهيئة النظام");
        }

        // تعريف الدالة txtUserName_Enter لتنفيذ العملية المرتبطة بها.
        private void txtUserName_Enter(object sender, EventArgs e)
        {
            if (txtUserName.Text == "اسم المستخدم")
            {
                txtUserName.Text = "";
                txtUserName.ForeColor = UIHelper.TextColor;
            }
        }

        // تعريف الدالة txtUserName_Leave لتنفيذ العملية المرتبطة بها.
        private void txtUserName_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUserName.Text))
            {
                txtUserName.Text = "اسم المستخدم";
                txtUserName.ForeColor = UIHelper.TextDisabledColor;
            }
        }

        // تعريف الدالة txtPassword_Enter لتنفيذ العملية المرتبطة بها.
        private void txtPassword_Enter(object sender, EventArgs e)
        {
            if (txtPassword.Text == "كلمة المرور")
            {
                // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
                txtPassword.Text = "";

                // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
                txtPassword.ForeColor = UIHelper.TextColor;

                // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
                txtPassword.PasswordChar = '●';
            }
        }

        // تعريف الدالة txtPassword_Leave لتنفيذ العملية المرتبطة بها.
        private void txtPassword_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
                txtPassword.Text = "كلمة المرور";

                // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
                txtPassword.ForeColor = UIHelper.TextDisabledColor;

                // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
                txtPassword.PasswordChar = '\0';
            }
        }

        // تعريف الدالة NormalizeDigits لتنفيذ العملية المرتبطة بها.
        private string NormalizeDigits(string value)
        {
            if (string.IsNullOrEmpty(value))
                return "";

            return value
                .Replace('٠', '0')
                .Replace('١', '1')
                .Replace('٢', '2')
                .Replace('٣', '3')
                .Replace('٤', '4')
                .Replace('٥', '5')
                .Replace('٦', '6')
                .Replace('٧', '7')
                .Replace('٨', '8')
                .Replace('٩', '9');
        }

        // تعريف الدالة btnLogin_Click لتنفيذ العملية المرتبطة بها.
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string userName = NormalizeDigits(txtUserName.Text).Trim();

            // لا نستخدم Trim لكلمة المرور؛ المسافات قد تكون جزءاً صحيحاً من كلمة المرور.
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            string password = NormalizeDigits(txtPassword.Text);

            // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
            if (string.IsNullOrWhiteSpace(userName) ||
                userName == "اسم المستخدم" ||
                string.IsNullOrWhiteSpace(password) ||
                password == "كلمة المرور")
            {
                ShowWarning("يرجى إدخال اسم المستخدم وكلمة المرور.");

                return;
            }

            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                btnLogin.Enabled = false;
                btnExit.Enabled = false;
                UseWaitCursor = true;
                btnLogin.Text = "جارٍ التحقق...";

                Refresh();

                // تمرير قيمة كلمة المرور إلى مسار التحقق.
                User authenticatedUser = userService.Authenticate(userName, password);

                ApplicationLogger.LogUsage("شاشة الدخول", "تسجيل دخول", "المستخدم '" + userName + "' نجح في تسجيل الدخول");

                // تمرير قيمة كلمة المرور إلى مسار التحقق.
                if (authenticatedUser.MustChangePassword)
                {
                    using (ChangePasswordForm changePasswordForm =
                           new ChangePasswordForm(authenticatedUser))
                    {
                        if (changePasswordForm.ShowDialog(this) != DialogResult.OK)
                        {
                            CurrentUser.Clear();
                            ShowWarning("لم يتم تغيير كلمة المرور؛ لن يتم فتح النظام.");

                            return;
                        }
                    }
                }

                DialogResult = DialogResult.OK;

                Close();
            }
            catch (Exception ex)
            {
                ApplicationLogger.LogWarning("شاشة الدخول", "فشل تسجيل الدخول للمستخدم '" + userName + "': " + ex.Message);
                ApplicationLogger.LogException("تسجيل الدخول في شاشة الدخول", ex);
                ShowLoginFailure(ex);
                System.Diagnostics.Debug.WriteLine(ex.ToString());

                // تمرير قيمة كلمة المرور إلى مسار التحقق والحماية.
                txtPassword.Clear();

                // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
                txtPassword.Focus();
            }
            finally
            {
                if (!IsDisposed)
                {
                    UseWaitCursor = false;
                    btnLogin.Enabled = true;
                    btnExit.Enabled = true;
                    btnLogin.Text = "تسجيل الدخول";
                }
            }
        }

        // تعريف الدالة btnTogglePassword_Click لتنفيذ العملية المرتبطة بها.
        private void btnTogglePassword_Click(object sender, EventArgs e)
        {
            if (txtPassword.Text == "كلمة المرور")
                return;

            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            bool showPassword = txtPassword.PasswordChar != '\0';

            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            txtPassword.PasswordChar = showPassword ? '\0' : '●';

            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            btnTogglePassword.Text = showPassword ? "إخفاء" : "إظهار";

            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            txtPassword.Focus();
        }

        // تعريف الدالة btnClear_Click لتنفيذ العملية المرتبطة بها.
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtUserName.Text = "اسم المستخدم";
            txtUserName.ForeColor = UIHelper.TextDisabledColor;

            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            txtPassword.Text = "كلمة المرور";

            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            txtPassword.ForeColor = UIHelper.TextDisabledColor;

            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            txtPassword.PasswordChar = '\0';

            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            btnTogglePassword.Text = "إظهار";

            // تمرير قيمة كلمة المرور إلى مسار التحقق والحماية.
            txtUserName.Focus();
        }

        // تعريف الدالة btnExit_Click لتنفيذ العملية المرتبطة بها.
        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "هل تريد إغلاق نظام إدارة المدرسة؟",
                "تأكيد الإغلاق",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2,
                MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);

            if (result == DialogResult.Yes)
            {
                ApplicationLogger.LogUsage("شاشة الدخول", "إغلاق النظام", "المستخدم اختار إغلاق التطبيق");
                AppShouldExit = true;
                Application.Exit();
            }
        }

        // تعريف الدالة ShowWarning لتنفيذ العملية المرتبطة بها.
        private void ShowWarning(string message)
        {
            UIHelper.ShowWarning(message);
        }

        // تعريف الدالة ShowLoginFailure لتنفيذ العملية المرتبطة بها.
        private void ShowLoginFailure(Exception exception)
        {
            string message = GetSafeLoginError(exception);

            if (message.Contains("تبقت لك"))
            {
                ShowWarning(message);

                return;
            }

            ShowError(
                message,
                message.Contains("تم تعطيل")
                    ? "تم تعطيل الحساب"
                    : "فشل الدخول");
        }

        // تعريف الدالة GetSafeLoginError لتنفيذ العملية المرتبطة بها.
        private string GetSafeLoginError(Exception exception)
        {
            string message =
                exception == null
                    ? string.Empty
                    : exception.Message ?? string.Empty;

            string technical = message.ToLowerInvariant();

            if (message.Contains("Invalid column name") ||
                message.Contains("اسم العمود غير صحيح") ||
                message.IndexOf(
                    "permission was denied",
                    StringComparison.OrdinalIgnoreCase) >= 0 ||
                message.Contains("There is already an object named"))
            {
                return
                    "قاعدة البيانات غير محدثة مع نسخة النظام الحالية. نفّذ ملف محرك الترحيل المدمج على قاعدة SchoolDB ثم أعد تشغيل البرنامج.";
            }

            if (message.Contains("SchoolDBConnection") ||
                message.Contains("إعدادات التطبيق") ||
                technical.Contains("sql") ||
                technical.Contains("connection") ||
                technical.Contains("timeout") ||
                technical.Contains("server") ||
                technical.Contains("network") ||
                technical.Contains("exception") ||
                technical.Contains("login failed"))
            {
                if (message.Contains("SchoolDBConnection") ||
                    message.Contains("إعدادات التطبيق"))
                {
                    return
                        "ملف إعدادات التطبيق لا يحتوي على اتصال SchoolDBConnection الصحيح. تحقق من SchoolSystem.exe.config.";
                }

                return
                    "تعذر الاتصال بالنظام حاليًا. تحقق من إعدادات قاعدة البيانات أو اتصل بمسؤول النظام.";
            }

            if (message.Contains("غير صحيحة") ||
                message.Contains("غير فعال") ||
                message.Contains("تم تعطيل الحساب") ||
                message.Contains("اطلب من مدير النظام"))
            {
                return message;
            }

            return "تعذر تسجيل الدخول. تحقق من البيانات ثم حاول مرة أخرى.";
        }

        // تعريف الدالة ShowError لتنفيذ العملية المرتبطة بها.
        private void ShowError(string message, string title = "فشل الدخول")
        {
            UIHelper.ShowError(message);
        }
    }
}
