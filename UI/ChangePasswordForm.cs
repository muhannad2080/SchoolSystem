using System;
using System.Windows.Forms;
using SchoolSystem.Helpers;
using SchoolSystem.Models;
using SchoolSystem.Services;

namespace SchoolSystem.UI
{
    public partial class ChangePasswordForm : Form
    {
        private readonly UserService userService = new UserService();
        private readonly bool isForced;

        public ChangePasswordForm()
            : this(null)
        {
        }

        public ChangePasswordForm(User user)
        {
            InitializeComponent();
            isForced = user != null && user.MustChangePassword &&
                !SchoolSystem.Security.PermissionKeys.IsSystemAdministratorRole(user.RoleName);

            UIHelper.ApplyStyle(this);
            UIHelper.StyleTextBox(txtCurrentPassword);
            UIHelper.StyleTextBox(txtNewPassword);
            UIHelper.StyleTextBox(txtConfirmPassword);
            UIHelper.StylePrimaryButton(btnSave);
            UIHelper.StyleButton(btnCancel, UIHelper.NeutralColor);

            lblUserName.Text = user == null
                ? "المستخدم الحالي"
                : "المستخدم: " + (user.FullName ?? user.UserName ?? string.Empty);
            lblNotice.Text = isForced
                ? "يجب تغيير كلمة المرور قبل المتابعة إلى النظام."
                : "استخدم هذه الشاشة لتحديث كلمة مرور حسابك بأمان.";
            btnCancel.Visible = !isForced;
            FormClosing += ChangePasswordForm_FormClosing;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                btnSave.Enabled = false;
                btnCancel.Enabled = false;
                UseWaitCursor = true;

                userService.ChangeCurrentUserPassword(
                    txtCurrentPassword.Text,
                    txtNewPassword.Text,
                    txtConfirmPassword.Text);

                UIHelper.ShowInformation("تم تغيير كلمة المرور بنجاح.");
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                UIHelper.ShowError(ex.Message);
                txtNewPassword.Clear();
                txtConfirmPassword.Clear();
                txtCurrentPassword.Focus();
            }
            finally
            {
                UseWaitCursor = false;
                if (!IsDisposed)
                {
                    btnSave.Enabled = true;
                    btnCancel.Enabled = !isForced;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void ChangePasswordForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isForced && DialogResult != DialogResult.OK)
            {
                e.Cancel = true;
                UIHelper.ShowWarning("يجب تغيير كلمة المرور للمتابعة إلى النظام.");
            }
        }
    }
}

