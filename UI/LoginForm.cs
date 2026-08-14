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
            UIHelper.ApplyTheme(this);
            panelBackground.BackColor = UIHelper.PrimaryColor;
            panelCard.BackColor = Color.White;
            panelCard.Padding = new Padding(18);
            lblIcon.ForeColor = UIHelper.AccentColor;
            lblTitle.ForeColor = UIHelper.PrimaryColor;
            lblSubtitle.ForeColor = Color.FromArgb(100, 116, 139);
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
                txtUserName.ForeColor = Color.Black;
            }
        }

        private void txtUserName_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUserName.Text))
            {
                txtUserName.Text = "اسم المستخدم";
                txtUserName.ForeColor = Color.Gray;
            }
        }

        private void txtPassword_Enter(object sender, EventArgs e)
        {
            if (txtPassword.Text == "كلمة المرور")
            {
                txtPassword.Text = "";
                txtPassword.ForeColor = Color.Black;
                txtPassword.PasswordChar = '●';
            }
        }

        private void txtPassword_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                txtPassword.Text = "كلمة المرور";
                txtPassword.ForeColor = Color.Gray;
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
                User user = userService.Authenticate(userName, password);

                CurrentUser.Set(user);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                ShowError(GetSafeLoginError(ex));
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                txtPassword.Clear();
                txtPassword.Focus();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ShowWarning(string message)
        {
            MessageBox.Show(
                message,
                "تنبيه",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
        }

        private string GetSafeLoginError(Exception exception)
        {
            string message = exception == null ? string.Empty : exception.Message ?? string.Empty;
            string technical = message.ToLowerInvariant();

            if (technical.Contains("sql") || technical.Contains("connection") ||
                technical.Contains("timeout") || technical.Contains("server") ||
                technical.Contains("network") || technical.Contains("exception") ||
                technical.Contains("login failed"))
            {
                return "تعذر الاتصال بالنظام حاليًا. تحقق من إعدادات قاعدة البيانات أو اتصل بمسؤول النظام.";
            }

            return string.IsNullOrWhiteSpace(message)
                ? "اسم المستخدم أو كلمة المرور غير صحيحة."
                : message;
        }

        private void ShowError(string message, string title = "فشل الدخول")
        {
            MessageBox.Show(
                message,
                title,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
        }
    }
}
