using System;
using System.Linq;
using System.Windows.Forms;
using SchoolSystem.Helpers;
using SchoolSystem.Models;
using SchoolSystem.Services;


namespace SchoolSystem.UI
{
    // تعريف الصنف ChangePasswordForm المسؤول عن وظائف طبقة UI.
    public partial class ChangePasswordForm : Form
    {
        // تعريف الدالة UserService لتنفيذ العملية المرتبطة بها.
        private readonly UserService userService = new UserService();

        private readonly bool isForced;

        // تعريف الدالة ChangePasswordForm لتنفيذ العملية المرتبطة بها.
        public ChangePasswordForm()
            // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
            : this(null)
        {
        }

        // تعريف الدالة ChangePasswordForm لتنفيذ العملية المرتبطة بها.
        public ChangePasswordForm(User user)
        {

            InitializeComponent();
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            isForced = user != null && user.MustChangePassword;

            UIHelper.ApplyStyle(this);
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            UIHelper.StyleTextBox(txtCurrentPassword);
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            UIHelper.StyleTextBox(txtNewPassword);
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            UIHelper.StyleTextBox(txtConfirmPassword);
            // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
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

        // تعريف الدالة btnSave_Click لتنفيذ العملية المرتبطة بها.
        private void btnSave_Click(object sender, EventArgs e)
        {
            // التحقق من الحقول قبل تنفيذ عملية تغيير كلمة المرور.
            if (!ValidateInputs())
                return;

            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                btnSave.Enabled = false;
                btnCancel.Enabled = false;
                UseWaitCursor = true;

                // التأكد من أن الجلسة تملك الصلاحية اللازمة للعملية.
                userService.ChangeCurrentUserPassword(
                    // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
                    txtCurrentPassword.Text,
                    // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
                    txtNewPassword.Text,
                    // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
                    txtConfirmPassword.Text);

                // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
                UIHelper.ShowInformation("تم تغيير كلمة المرور بنجاح.");
                ApplicationLogger.LogUsage("شاشة تغيير كلمة المرور", "حفظ", "تم تغيير كلمة المرور بنجاح");
                DialogResult = DialogResult.OK;

                Close();
            }

            catch (Exception ex)
            {
                ApplicationLogger.LogException("تغيير كلمة المرور في شاشة تغيير كلمة المرور", ex);
                UIHelper.ShowError(ex.Message);
                // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
                txtNewPassword.Clear();
                // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
                txtConfirmPassword.Clear();
                // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
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

        // تعريف الدالة ValidateInputs للتحقق من حقول تغيير كلمة المرور قبل الحفظ.
        private bool ValidateInputs()
        {
            string current = txtCurrentPassword.Text;
            string newPassword = txtNewPassword.Text;
            string confirm = txtConfirmPassword.Text;

            if (string.IsNullOrEmpty(current))
            {
                UIHelper.FocusAndWarn(txtCurrentPassword, "أدخل كلمة المرور الحالية.");

                return false;
            }

            if (string.IsNullOrEmpty(newPassword))
            {
                UIHelper.FocusAndWarn(txtNewPassword, "أدخل كلمة المرور الجديدة.");

                return false;
            }

            if (newPassword.Length < 10 ||
                !newPassword.Any(char.IsLetter) ||
                !newPassword.Any(char.IsDigit))
            {
                UIHelper.FocusAndWarn(
                    txtNewPassword,
                    "كلمة المرور الجديدة يجب ألا تقل عن 10 أحرف وأن تحتوي على أحرف وأرقام.");

                return false;
            }

            if (current == newPassword)
            {
                UIHelper.FocusAndWarn(
                    txtNewPassword,
                    "كلمة المرور الجديدة يجب أن تختلف عن كلمة المرور الحالية.");

                return false;
            }

            if (newPassword != confirm)
            {
                UIHelper.FocusAndWarn(
                    txtConfirmPassword,
                    "كلمة المرور الجديدة وتأكيدها غير متطابقين.");

                return false;
            }

            return true;
        }

        // تعريف الدالة btnCancel_Click لتنفيذ العملية المرتبطة بها.
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;

            Close();
        }

        // تعريف الدالة ChangePasswordForm_FormClosing لتنفيذ العملية المرتبطة بها.
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
