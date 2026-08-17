namespace SchoolSystem.UI
{
    partial class ChangePasswordForm
    {
        private System.ComponentModel.IContainer components = null;

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
            this.lblCurrentPassword = new Krypton.Toolkit.KryptonLabel();
            this.lblNewPassword = new Krypton.Toolkit.KryptonLabel();
            this.lblConfirmPassword = new Krypton.Toolkit.KryptonLabel();
            this.txtCurrentPassword = new Krypton.Toolkit.KryptonTextBox();
            this.txtNewPassword = new Krypton.Toolkit.KryptonTextBox();
            this.txtConfirmPassword = new Krypton.Toolkit.KryptonTextBox();
            this.btnSave = new Krypton.Toolkit.KryptonButton();
            this.btnCancel = new Krypton.Toolkit.KryptonButton();
            this.panelBackground.SuspendLayout();
            this.panelCard.SuspendLayout();
            this.SuspendLayout();
            // panelBackground
            this.panelBackground.Controls.Add(this.panelCard);
            this.panelBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBackground.Location = new System.Drawing.Point(0, 0);
            this.panelBackground.Name = "panelBackground";
            this.panelBackground.Size = new System.Drawing.Size(760, 520);
            this.panelBackground.TabIndex = 0;
            // panelCard
            this.panelCard.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panelCard.Controls.Add(this.lblTitle);
            this.panelCard.Controls.Add(this.lblUserName);
            this.panelCard.Controls.Add(this.lblNotice);
            this.panelCard.Controls.Add(this.lblCurrentPassword);
            this.panelCard.Controls.Add(this.lblNewPassword);
            this.panelCard.Controls.Add(this.lblConfirmPassword);
            this.panelCard.Controls.Add(this.txtCurrentPassword);
            this.panelCard.Controls.Add(this.txtNewPassword);
            this.panelCard.Controls.Add(this.txtConfirmPassword);
            this.panelCard.Controls.Add(this.btnSave);
            this.panelCard.Controls.Add(this.btnCancel);
            this.panelCard.Location = new System.Drawing.Point(170, 55);
            this.panelCard.Name = "panelCard";
            this.panelCard.Size = new System.Drawing.Size(420, 410);
            this.panelCard.TabIndex = 0;
            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(115, 24);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(190, 20);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "تغيير كلمة المرور";
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold);
            // lblUserName
            this.lblUserName.AutoSize = true;
            this.lblUserName.Location = new System.Drawing.Point(42, 65);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(120, 17);
            this.lblUserName.TabIndex = 1;
            this.lblUserName.Text = "المستخدم الحالي";
            this.lblUserName.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            // lblNotice
            this.lblNotice.AutoSize = false;
            this.lblNotice.Location = new System.Drawing.Point(42, 91);
            this.lblNotice.Name = "lblNotice";
            this.lblNotice.Size = new System.Drawing.Size(336, 36);
            this.lblNotice.TabIndex = 2;
            this.lblNotice.Text = "يجب تغيير كلمة المرور قبل المتابعة إلى النظام.";
            // labels
            this.lblCurrentPassword.AutoSize = true;
            this.lblCurrentPassword.Location = new System.Drawing.Point(42, 142);
            this.lblCurrentPassword.Name = "lblCurrentPassword";
            this.lblCurrentPassword.Size = new System.Drawing.Size(112, 17);
            this.lblCurrentPassword.TabIndex = 3;
            this.lblCurrentPassword.Text = "كلمة المرور الحالية";
            this.lblNewPassword.AutoSize = true;
            this.lblNewPassword.Location = new System.Drawing.Point(42, 201);
            this.lblNewPassword.Name = "lblNewPassword";
            this.lblNewPassword.Size = new System.Drawing.Size(108, 17);
            this.lblNewPassword.TabIndex = 4;
            this.lblNewPassword.Text = "كلمة المرور الجديدة";
            this.lblConfirmPassword.AutoSize = true;
            this.lblConfirmPassword.Location = new System.Drawing.Point(42, 260);
            this.lblConfirmPassword.Name = "lblConfirmPassword";
            this.lblConfirmPassword.Size = new System.Drawing.Size(112, 17);
            this.lblConfirmPassword.TabIndex = 5;
            this.lblConfirmPassword.Text = "تأكيد كلمة المرور";
            // text boxes
            this.txtCurrentPassword.Location = new System.Drawing.Point(42, 162);
            this.txtCurrentPassword.Name = "txtCurrentPassword";
            this.txtCurrentPassword.PasswordChar = '●';
            this.txtCurrentPassword.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtCurrentPassword.Size = new System.Drawing.Size(336, 25);
            this.txtCurrentPassword.TabIndex = 6;
            this.txtNewPassword.Location = new System.Drawing.Point(42, 221);
            this.txtNewPassword.Name = "txtNewPassword";
            this.txtNewPassword.PasswordChar = '●';
            this.txtNewPassword.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtNewPassword.Size = new System.Drawing.Size(336, 25);
            this.txtNewPassword.TabIndex = 7;
            this.txtConfirmPassword.Location = new System.Drawing.Point(42, 280);
            this.txtConfirmPassword.Name = "txtConfirmPassword";
            this.txtConfirmPassword.PasswordChar = '●';
            this.txtConfirmPassword.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtConfirmPassword.Size = new System.Drawing.Size(336, 25);
            this.txtConfirmPassword.TabIndex = 8;
            // buttons
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
            this.ClientSize = new System.Drawing.Size(760, 520);
            this.Controls.Add(this.panelBackground);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChangePasswordForm";
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
        private Krypton.Toolkit.KryptonLabel lblCurrentPassword;
        private Krypton.Toolkit.KryptonLabel lblNewPassword;
        private Krypton.Toolkit.KryptonLabel lblConfirmPassword;
        private Krypton.Toolkit.KryptonTextBox txtCurrentPassword;
        private Krypton.Toolkit.KryptonTextBox txtNewPassword;
        private Krypton.Toolkit.KryptonTextBox txtConfirmPassword;
        private Krypton.Toolkit.KryptonButton btnSave;
        private Krypton.Toolkit.KryptonButton btnCancel;
    }
}
