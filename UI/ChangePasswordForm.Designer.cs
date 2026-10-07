
namespace SchoolSystem.UI
{
    // تعريف الصنف ChangePasswordForm المسؤول عن وظائف طبقة UI.
    partial class ChangePasswordForm
    {

        private System.ComponentModel.IContainer components = null;

        // تعريف الدالة Dispose لتنفيذ العملية المرتبطة بها.
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();

            base.Dispose(disposing);
        }


        private void InitializeComponent()
        {
            this.panelBackground = new Krypton.Toolkit.KryptonPanel();
            this.panelCard = new Krypton.Toolkit.KryptonPanel();
            this.lblTitle = new Krypton.Toolkit.KryptonLabel();
            this.lblUserName = new Krypton.Toolkit.KryptonLabel();
            this.lblNotice = new Krypton.Toolkit.KryptonLabel();
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            this.lblCurrentPassword = new Krypton.Toolkit.KryptonLabel();
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            this.lblNewPassword = new Krypton.Toolkit.KryptonLabel();
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            this.lblConfirmPassword = new Krypton.Toolkit.KryptonLabel();
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            this.txtCurrentPassword = new Krypton.Toolkit.KryptonTextBox();
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            this.txtNewPassword = new Krypton.Toolkit.KryptonTextBox();
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            this.txtConfirmPassword = new Krypton.Toolkit.KryptonTextBox();
            // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
            this.btnSave = new Krypton.Toolkit.KryptonButton();
            this.btnCancel = new Krypton.Toolkit.KryptonButton();

            this.panelBackground.SuspendLayout();

            this.panelCard.SuspendLayout();

            this.SuspendLayout();


            this.panelBackground.Controls.Add(this.panelCard);
            this.panelBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBackground.Location = new System.Drawing.Point(0, 0);
            this.panelBackground.Name = "panelBackground";
            this.panelBackground.Size = new System.Drawing.Size(900, 640);
            this.panelBackground.TabIndex = 0;

            this.panelCard.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panelCard.Controls.Add(this.lblTitle);
            this.panelCard.Controls.Add(this.lblUserName);
            this.panelCard.Controls.Add(this.lblNotice);
            // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
            this.panelCard.Controls.Add(this.lblCurrentPassword);
            // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
            this.panelCard.Controls.Add(this.lblNewPassword);
            // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
            this.panelCard.Controls.Add(this.lblConfirmPassword);
            // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
            this.panelCard.Controls.Add(this.txtCurrentPassword);
            // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
            this.panelCard.Controls.Add(this.txtNewPassword);
            // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
            this.panelCard.Controls.Add(this.txtConfirmPassword);
            // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
            this.panelCard.Controls.Add(this.btnSave);
            this.panelCard.Controls.Add(this.btnCancel);
            this.panelCard.Location = new System.Drawing.Point(240, 115);
            this.panelCard.Name = "panelCard";
            this.panelCard.Size = new System.Drawing.Size(420, 410);
            this.panelCard.TabIndex = 0;

            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(115, 24);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(190, 20);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "تغيير كلمة المرور";
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold);

            this.lblUserName.AutoSize = true;
            this.lblUserName.Location = new System.Drawing.Point(42, 65);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(120, 17);
            this.lblUserName.TabIndex = 1;
            this.lblUserName.Text = "المستخدم الحالي";
            this.lblUserName.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);

            this.lblNotice.AutoSize = false;
            this.lblNotice.Location = new System.Drawing.Point(42, 91);
            this.lblNotice.Name = "lblNotice";
            this.lblNotice.Size = new System.Drawing.Size(336, 36);
            this.lblNotice.TabIndex = 2;
            this.lblNotice.Text = "يجب تغيير كلمة المرور قبل المتابعة إلى النظام.";

            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            this.lblCurrentPassword.AutoSize = true;
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            this.lblCurrentPassword.Location = new System.Drawing.Point(42, 142);
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            this.lblCurrentPassword.Name = "lblCurrentPassword";
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            this.lblCurrentPassword.Size = new System.Drawing.Size(112, 17);
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            this.lblCurrentPassword.TabIndex = 3;
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            this.lblCurrentPassword.Text = "كلمة المرور الحالية";
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            this.lblNewPassword.AutoSize = true;
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            this.lblNewPassword.Location = new System.Drawing.Point(42, 201);
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            this.lblNewPassword.Name = "lblNewPassword";
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            this.lblNewPassword.Size = new System.Drawing.Size(108, 17);
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            this.lblNewPassword.TabIndex = 4;
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            this.lblNewPassword.Text = "كلمة المرور الجديدة";
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            this.lblConfirmPassword.AutoSize = true;
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            this.lblConfirmPassword.Location = new System.Drawing.Point(42, 260);
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            this.lblConfirmPassword.Name = "lblConfirmPassword";
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            this.lblConfirmPassword.Size = new System.Drawing.Size(112, 17);
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            this.lblConfirmPassword.TabIndex = 5;
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            this.lblConfirmPassword.Text = "تأكيد كلمة المرور";

            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            this.txtCurrentPassword.Location = new System.Drawing.Point(42, 162);
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            this.txtCurrentPassword.Name = "txtCurrentPassword";
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            this.txtCurrentPassword.PasswordChar = '●';
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            this.txtCurrentPassword.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            this.txtCurrentPassword.Size = new System.Drawing.Size(336, 25);
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            this.txtCurrentPassword.TabIndex = 6;
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            this.txtNewPassword.Location = new System.Drawing.Point(42, 221);
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            this.txtNewPassword.Name = "txtNewPassword";
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            this.txtNewPassword.PasswordChar = '●';
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            this.txtNewPassword.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            this.txtNewPassword.Size = new System.Drawing.Size(336, 25);
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            this.txtNewPassword.TabIndex = 7;
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            this.txtConfirmPassword.Location = new System.Drawing.Point(42, 280);
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            this.txtConfirmPassword.Name = "txtConfirmPassword";
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            this.txtConfirmPassword.PasswordChar = '●';
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            this.txtConfirmPassword.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            this.txtConfirmPassword.Size = new System.Drawing.Size(336, 25);
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            this.txtConfirmPassword.TabIndex = 8;
            // buttons
            // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
            this.btnSave.Location = new System.Drawing.Point(42, 332);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(160, 40);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "حفظ كلمة المرور";

            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            this.btnCancel.Location = new System.Drawing.Point(218, 332);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(160, 40);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "إلغاء";

            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // ChangePasswordForm
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 640);
            this.MinimumSize = new System.Drawing.Size(900, 640);
            this.Controls.Add(this.panelBackground);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
            this.Name = "ChangePasswordForm";
            // تمرير قيمة كلمة المرور إلى مسار الحماية المناسب.
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "تغيير كلمة المرور";

            this.panelBackground.ResumeLayout(false);

            this.panelCard.ResumeLayout(false);

            this.panelCard.PerformLayout();

            this.ResumeLayout(false);
        }


        private Krypton.Toolkit.KryptonPanel panelBackground;

        private Krypton.Toolkit.KryptonPanel panelCard;

        private Krypton.Toolkit.KryptonLabel lblTitle;

        private Krypton.Toolkit.KryptonLabel lblUserName;

        private Krypton.Toolkit.KryptonLabel lblNotice;
        // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
        private Krypton.Toolkit.KryptonLabel lblCurrentPassword;
        // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
        private Krypton.Toolkit.KryptonLabel lblNewPassword;
        // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
        private Krypton.Toolkit.KryptonLabel lblConfirmPassword;
        // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
        private Krypton.Toolkit.KryptonTextBox txtCurrentPassword;
        // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
        private Krypton.Toolkit.KryptonTextBox txtNewPassword;
        // تجهيز قيمة كلمة المرور لمسار التحقق والحماية.
        private Krypton.Toolkit.KryptonTextBox txtConfirmPassword;

        private Krypton.Toolkit.KryptonButton btnSave;

        private Krypton.Toolkit.KryptonButton btnCancel;
    }
}
