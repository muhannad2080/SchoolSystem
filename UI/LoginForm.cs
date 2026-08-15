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

            try
            {
                userService.EnsureDefaultAdmin();

            }
            catch (Exception ex)
            {
                ShowError("تعذر تهيئة بيانات الدخول. تحقق من اتصال قاعدة البيانات ثم حاول مرة أخرى.", "تهيئة النظام");
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }

            this.Resize += (s, e) => CenterCard();

            CenterCard();

            this.AcceptButton = btnLogin;
            this.CancelButton = btnExit;
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
            string password = NormalizeDigits(txtPassword.Text).Trim();

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

                userService.Authenticate(userName, password);

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

        private void btnExit_Click(object sender, EventArgs e)
        {
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
