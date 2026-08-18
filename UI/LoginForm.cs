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
        private readonly UserService userService = new UserService();

        public LoginForm()
        {
            InitializeComponent();
            SchoolSystem.Helpers.UIHelper.ApplyStyle(this);
            panelBackground.BackColor = UIHelper.PrimaryColor;
            panelCard.BackColor = UIHelper.SurfaceElevatedColor;
            panelCard.Padding = new Padding(18);
            lblIcon.ForeColor = UIHelper.AccentColor;
            lblTitle.ForeColor = UIHelper.PrimaryColor;
            lblSubtitle.ForeColor = UIHelper.MutedTextColor;
            UIHelper.StyleTextBox(txtUserName);
            UIHelper.StyleTextBox(txtPassword);
            UIHelper.StylePrimaryButton(btnLogin);
            UIHelper.StyleButton(btnExit, UIHelper.NeutralColor);
            UIHelper.StyleButton(btnClear, UIHelper.NeutralColor);
            UIHelper.StyleButton(btnTogglePassword, UIHelper.NeutralColor);
            btnTogglePassword.Text = "إظهار";
            btnTogglePassword.TabStop = false;

            try
            {
                userService.EnsureDefaultAdmin();

            }
            catch (Exception ex)
            {
                ShowInitializationError(ex);
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }

            this.Resize += (s, e) => CenterCard();

            CenterCard();

            this.AcceptButton = btnLogin;
            this.CancelButton = btnExit;
            this.Shown += (s, e) =>
            {
                txtUserName.Focus();
                txtUserName.SelectAll();
            };
        }

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
                "تعذر تهيئة بيانات الدخول. تحقق من اتصال قاعدة البيانات وتأكد من تنفيذ Databass\\Migration_Step1.sql ثم حاول مرة أخرى.",
                "تهيئة النظام");
        }

        private void CenterCard()
        {
            panelCard.Location = new Point(
                (this.ClientSize.Width - panelCard.Width) / 2,
                (this.ClientSize.Height - panelCard.Height) / 2);
        }

        private void txtUserName_Enter(object sender, EventArgs e)
        {
            if (txtUserName.Text == "اسم المستخدم")
            {
                txtUserName.Text = "";
                txtUserName.ForeColor = UIHelper.TextColor;
            }
        }

        private void txtUserName_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUserName.Text))
            {
                txtUserName.Text = "اسم المستخدم";
                txtUserName.ForeColor = UIHelper.TextDisabledColor;
            }
        }

        private void txtPassword_Enter(object sender, EventArgs e)
        {
            if (txtPassword.Text == "كلمة المرور")
            {
                txtPassword.Text = "";
                txtPassword.ForeColor = UIHelper.TextColor;
                txtPassword.PasswordChar = '●';
            }
        }

        private void txtPassword_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                txtPassword.Text = "كلمة المرور";
                txtPassword.ForeColor = UIHelper.TextDisabledColor;
                txtPassword.PasswordChar = '\0';
            }
        }

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

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string userName = NormalizeDigits(txtUserName.Text).Trim();
            // لا نستخدم Trim لكلمة المرور؛ المسافات قد تكون جزءاً صحيحاً من كلمة المرور.
            string password = NormalizeDigits(txtPassword.Text);

            if (string.IsNullOrWhiteSpace(userName) ||
                userName == "اسم المستخدم" ||
                string.IsNullOrWhiteSpace(password) ||
                password == "كلمة المرور")
            {
                ShowWarning("يرجى إدخال اسم المستخدم وكلمة المرور.");
                return;
            }

            try
            {
                btnLogin.Enabled = false;
                btnExit.Enabled = false;
                UseWaitCursor = true;
                btnLogin.Text = "جارٍ التحقق...";
                Refresh();

                User authenticatedUser = userService.Authenticate(userName, password);

                if (authenticatedUser.MustChangePassword &&
                    !PermissionKeys.IsSystemAdministratorRole(authenticatedUser.RoleName))
                {
                    using (ChangePasswordForm changePasswordForm = new ChangePasswordForm(authenticatedUser))
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
                ShowLoginFailure(ex);
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                txtPassword.Clear();
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

        private void btnTogglePassword_Click(object sender, EventArgs e)
        {
            if (txtPassword.Text == "كلمة المرور")
                return;

            bool showPassword = txtPassword.PasswordChar != '\0';
            txtPassword.PasswordChar = showPassword ? '\0' : '●';
            btnTogglePassword.Text = showPassword ? "إخفاء" : "إظهار";
            txtPassword.Focus();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtUserName.Text = "اسم المستخدم";
            txtUserName.ForeColor = UIHelper.TextDisabledColor;
            txtPassword.Text = "كلمة المرور";
            txtPassword.ForeColor = UIHelper.TextDisabledColor;
            txtPassword.PasswordChar = '\0';
            btnTogglePassword.Text = "إظهار";
            txtUserName.Focus();
        }

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
                Application.Exit();
        }

        private void ShowWarning(string message)
        {
            UIHelper.ShowWarning(message);
        }

        private void ShowLoginFailure(Exception exception)
        {
            string message = GetSafeLoginError(exception);

            if (message.Contains("تبقت لك"))
            {
                ShowWarning(message);
                return;
            }

            ShowError(message, message.Contains("تم تعطيل") ? "تم تعطيل الحساب" : "فشل الدخول");
        }

        private string GetSafeLoginError(Exception exception)
        {
            string message = exception == null ? string.Empty : exception.Message ?? string.Empty;
            string technical = message.ToLowerInvariant();

            if (message.Contains("Invalid column name") ||
                message.Contains("اسم العمود غير صحيح") ||
                message.IndexOf("permission was denied", StringComparison.OrdinalIgnoreCase) >= 0 ||
                message.Contains("There is already an object named"))
            {
                return "قاعدة البيانات غير محدثة مع نسخة النظام الحالية. نفّذ ملف Databass\\Migration_Step1.sql على قاعدة SchoolDB ثم أعد تشغيل البرنامج.";
            }

            if (message.Contains("SchoolDBConnection") || message.Contains("إعدادات التطبيق") ||
                technical.Contains("sql") || technical.Contains("connection") ||
                technical.Contains("timeout") || technical.Contains("server") ||
                technical.Contains("network") || technical.Contains("exception") ||
                technical.Contains("login failed"))
            {
                if (message.Contains("SchoolDBConnection") || message.Contains("إعدادات التطبيق"))
                    return "ملف إعدادات التطبيق لا يحتوي على اتصال SchoolDBConnection الصحيح. تحقق من SchoolSystem.exe.config.";

                return "تعذر الاتصال بالنظام حاليًا. تحقق من إعدادات قاعدة البيانات أو اتصل بمسؤول النظام.";
            }

            if (message.Contains("غير صحيحة") || message.Contains("غير فعال") ||
                message.Contains("تم تعطيل الحساب") || message.Contains("اطلب من مدير النظام"))
                return message;

            return "تعذر تسجيل الدخول. تحقق من البيانات ثم حاول مرة أخرى.";
        }

        private void ShowError(string message, string title = "فشل الدخول")
        {
            UIHelper.ShowError(message);
        }
    }
}
