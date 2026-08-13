using System;
using System.Drawing;
using System.Windows.Forms;
using SchoolSystem.Helpers;
using SchoolSystem.Models;
using SchoolSystem.Security;
using SchoolSystem.Services;

namespace SchoolSystem.UI
{
    public partial class LoginForm : Form
    {
        private readonly UserService userService;

        public LoginForm()
        {
            InitializeComponent();

            userService = new UserService();

            ApplyLoginFormStyle();
            InitializeLoginForm();
        }

        private void ApplyLoginFormStyle()
        {
            UIHelper.ApplyTheme(this);

            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;

            panelBackground.BackColor =
                UIHelper.PrimaryColor;

            panelCard.BackColor =
                UIHelper.CardColor;

            panelCard.Padding =
                new Padding(18);

            lblIcon.ForeColor =
                UIHelper.AccentColor;

            lblTitle.ForeColor =
                UIHelper.PrimaryColor;

            lblSubtitle.ForeColor =
                UIHelper.MutedTextColor;

            UIHelper.StyleTextBox(txtUserName);
            UIHelper.StyleTextBox(txtPassword);

            UIHelper.StyleButton(
                btnLogin,
                UIHelper.SuccessColor);

            UIHelper.StyleButton(
                btnExit,
                UIHelper.NeutralColor);
        }

        private void InitializeLoginForm()
        {
            try
            {
                userService.EnsureDefaultAdmin();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                UIHelper.ShowError(
                    "تعذر تهيئة النظام تلقائيًا. تحقق من إعدادات قاعدة البيانات قبل تسجيل الدخول.");
            }

            Resize += LoginForm_Resize;

            CenterCard();

            AcceptButton = btnLogin;
            CancelButton = btnExit;
        }

        private void LoginForm_Resize(
            object sender,
            EventArgs e)
        {
            CenterCard();
        }

        private void CenterCard()
        {
            if (panelCard == null)
                return;

            int x = Math.Max(
                0,
                (ClientSize.Width - panelCard.Width) / 2);

            int y = Math.Max(
                0,
                (ClientSize.Height - panelCard.Height) / 2);

            panelCard.Location = new Point(x, y);
        }

        private void txtUserName_Enter(
            object sender,
            EventArgs e)
        {
            if (txtUserName.Text == "اسم المستخدم")
            {
                txtUserName.Clear();
                txtUserName.ForeColor =
                    UIHelper.TextColor;
            }
        }

        private void txtUserName_Leave(
            object sender,
            EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(
                txtUserName.Text))
            {
                txtUserName.Text = "اسم المستخدم";
                txtUserName.ForeColor =
                    UIHelper.MutedTextColor;
            }
        }

        private void txtPassword_Enter(
            object sender,
            EventArgs e)
        {
            if (txtPassword.Text == "كلمة المرور")
            {
                txtPassword.Clear();
                txtPassword.ForeColor =
                    UIHelper.TextColor;

                txtPassword.PasswordChar = '●';
            }
        }

        private void txtPassword_Leave(
            object sender,
            EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(
                txtPassword.Text))
            {
                txtPassword.Text = "كلمة المرور";
                txtPassword.ForeColor =
                    UIHelper.MutedTextColor;

                txtPassword.PasswordChar = '\0';
            }
        }

        private void btnLogin_Click(
            object sender,
            EventArgs e)
        {
            string userName =
                NormalizeDigits(txtUserName.Text).Trim();

            string password =
                NormalizeDigits(txtPassword.Text).Trim();

            if (string.IsNullOrWhiteSpace(userName) ||
                userName == "اسم المستخدم")
            {
                UIHelper.ShowWarning(
                    "يرجى إدخال اسم المستخدم.");

                txtUserName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(password) ||
                password == "كلمة المرور")
            {
                UIHelper.ShowWarning(
                    "يرجى إدخال كلمة المرور.");

                txtPassword.Focus();
                return;
            }

            try
            {
                SetLoginState(true);

                User user =
                    userService.Authenticate(
                        userName,
                        password);

                if (user == null)
                {
                    CurrentUser.Clear();

                    UIHelper.ShowError(
                        "اسم المستخدم أو كلمة المرور غير صحيحة.");

                    ResetPasswordField();
                    return;
                }

                if (!user.IsActive)
                {
                    CurrentUser.Clear();

                    UIHelper.ShowWarning(
                        "هذا المستخدم غير نشط. " +
                        "يرجى التواصل مع مدير النظام.");

                    return;
                }

                CurrentUser.Set(user);

                if (!CurrentUser.IsLoggedIn)
                {
                    throw new InvalidOperationException(
                        "تعذر تهيئة جلسة المستخدم.");
                }

#if DEBUG
                System.Diagnostics.Debug.WriteLine(
                    "Logged user: " +
                    (user.UserName ?? string.Empty));

                System.Diagnostics.Debug.WriteLine(
                    "Role: " +
                    (user.RoleName ?? string.Empty));

                System.Diagnostics.Debug.WriteLine(
                    "Permissions: " +
                    (user.Permissions ?? string.Empty));
#endif

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                CurrentUser.Clear();
                System.Diagnostics.Debug.WriteLine(ex.ToString());

                UIHelper.ShowError(
                    "تعذر تسجيل الدخول حاليًا. تحقق من الاتصال بقاعدة البيانات أو تواصل مع مسؤول النظام.");

                ResetPasswordField();
            }
            finally
            {
                if (!IsDisposed && !Disposing)
                    SetLoginState(false);
            }
        }

        private void SetLoginState(bool isLoggingIn)
        {
            btnLogin.Enabled = !isLoggingIn;
            btnExit.Enabled = !isLoggingIn;
            txtUserName.Enabled = !isLoggingIn;
            txtPassword.Enabled = !isLoggingIn;

            btnLogin.Text = isLoggingIn
                ? "جاري تسجيل الدخول..."
                : "تسجيل الدخول";

            Cursor = isLoggingIn
                ? Cursors.WaitCursor
                : Cursors.Default;
        }

        private void ResetPasswordField()
        {
            txtPassword.Clear();
            txtPassword.PasswordChar = '●';
            txtPassword.ForeColor =
                UIHelper.TextColor;

            txtPassword.Focus();
        }

        private void btnExit_Click(
            object sender,
            EventArgs e)
        {
            CurrentUser.Clear();
            Application.Exit();
        }

        private string NormalizeDigits(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

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
    }
}
