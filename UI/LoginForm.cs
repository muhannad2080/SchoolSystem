using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
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
        private Image schoolBrandImage;

        // يُضبط فقط بعد تأكيد المستخدم الإغلاق أو بعد نجاح تسجيل الدخول.
        public bool AppShouldExit { get; private set; }

        // يمنع FormClosing من إعادة سؤال المستخدم بعد اتخاذ قرار إغلاق صحيح.
        private bool _allowClose;

        // تعريف الدالة LoginForm لتنفيذ العملية المرتبطة بها.
        public LoginForm()
        {
            InitializeComponent();

            ConfigureLoginExperience();
            ConfigurePremiumSurface();

            FormClosed += delegate
            {
                if (schoolBrandImage != null)
                {
                    schoolBrandImage.Dispose();
                    schoolBrandImage = null;
                }
            };

            // توحيد اللون الرئيسي مع هوية النظام مع الإبقاء على تخطيط شاشة الدخول المخصص.
            btnLogin.BackColor = UIHelper.AccentColor;
            btnLogin.ForeColor = System.Drawing.Color.White;

            lblStatus.Text = "الحالة: جاهز لتسجيل الدخول";

            // ============================================================
            // ملاحظة:
            // تم إزالة جميع أوامر التنسيق والتصميم من هنا حتى يكون
            // LoginForm.Designer.cs هو المسؤول بالكامل عن شكل الواجهة.
            //
            // لم يتم تغيير أي منطق متعلق بتسجيل الدخول أو التحقق
            // أو الأحداث أو معالجة الأخطاء.
            // ============================================================

            // لا تُنفذ تهيئة قاعدة البيانات أثناء فتح WinForms Designer.
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                try
                {
                    userService.EnsureDefaultAdmin();
                }
                catch (Exception ex)
                {
                    ApplicationLogger.LogException("تهيئة مدير النظام في شاشة الدخول", ex);
                    SetStartupStatus(ex);
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
            }

            this.AcceptButton = btnLogin;
            this.CancelButton = btnExit;

            // اعتراض كل طرق إغلاق شاشة الدخول: زر الخروج، زر X المخصص، وAlt+F4.
            FormClosing += LoginForm_FormClosing;

            // تقييد أطوال حقول الدخول.
            txtUserName.MaxLength = 100;
            txtPassword.MaxLength = 128;

            this.Shown += (s, e) =>
            {
                txtUserName.Focus();
                txtUserName.SelectAll();
            };
        }

        /// <summary>
        /// يضبط الهوية البصرية العربية لشاشة الدخول ويضيف تذييل المطورين.
        /// لا يغيّر مسار المصادقة أو الصلاحيات أو التعامل مع قاعدة البيانات.
        /// </summary>
        private void ConfigureLoginExperience()
        {
            DoubleBuffered = true;
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            FormBorderStyle = FormBorderStyle.None;
            BackColor = UIHelper.BackgroundColor;

            SchoolSettings branding = SchoolSettingsService.Current();

            string schoolName =
                branding != null && !string.IsNullOrWhiteSpace(branding.SchoolNameAr)
                    ? branding.SchoolNameAr.Trim()
                    : branding != null && !string.IsNullOrWhiteSpace(branding.SchoolNameEn)
                        ? branding.SchoolNameEn.Trim()
                        : "نظام إدارة المدرسة";

            string organization =
                branding != null && !string.IsNullOrWhiteSpace(branding.OrganizationNameAr)
                    ? branding.OrganizationNameAr.Trim()
                    : string.Empty;

            string address =
                branding != null && !string.IsNullOrWhiteSpace(branding.AddressAr)
                    ? branding.AddressAr.Trim()
                    : string.Empty;

            string phone =
                branding != null && !string.IsNullOrWhiteSpace(branding.Phone)
                    ? "هاتف: " + branding.Phone.Trim()
                    : string.Empty;

            string contact = string.Join("   •   ",
                new[] { organization, address, phone }
                .Where(value => !string.IsNullOrWhiteSpace(value)));

            lblBrandName.Text = schoolName;
            lblBrandTagline.Text =
                string.IsNullOrWhiteSpace(contact)
                    ? "منصة مدرسية موحّدة لإدارة العمليات اليومية"
                    : contact;

            LoadSchoolBrandLogo(branding);
            lblFeatureTitle.Text = "إدارة مدرسية ذكية في منصة واحدة";
            lblFeature1.Text = "✓  بيانات الطلاب والتسجيلات مترابطة ومنظمة";
            lblFeature2.Text = "✓  الأكاديميات والرسوم والحضور في مسار واحد";
            lblFeature3.Text = "✓  أدوار وصلاحيات وسجل تدقيق للعمليات";
            lblBrandFooter.Text = "SchoolSystem • نظام إدارة المدرسة";

            lblWelcome.Text = "مرحبًا بك في SchoolSystem";
            lblTitle.Text = "تسجيل الدخول";
            lblSubtitle.Text = "أدخل بيانات حسابك المصرّح له بالوصول إلى نظام إدارة المدرسة";

            lblUserLabel.Text = "اسم المستخدم";
            lblPasswordLabel.Text = "كلمة المرور";
            txtUserName.TextAlign = HorizontalAlignment.Right;
            txtPassword.TextAlign = HorizontalAlignment.Right;

            btnLogin.Text = "تسجيل الدخول";
            btnClear.Text = "مسح الحقول";
            btnExit.Text = "خروج";
            btnTogglePassword.Text = "إظهار";

            lblStatus.TextAlign = ContentAlignment.MiddleRight;
            lblFooter.Text = "الوصول إلى النظام متاح للحسابات المصرّح لها فقط";
            lblFooter.TextAlign = ContentAlignment.MiddleRight;

            lblDevelopers.Text =
                "تطوير النظام\r\nالمهندس مهند علي علي علي سعد • عبد الرحمن سعد علي سعد • سامي الحاج";
            lblDevelopers.TextAlign = ContentAlignment.MiddleCenter;

            btnWindowClose.Text = "×";
            btnWindowClose.RightToLeft = RightToLeft.No;

            // توحيد اتجاه النصوص داخل الحاويات العربية.
            foreach (Control control in new Control[]
            {
                panelBrand, brandLayout, brandTop, brandFeatureLayout,
                panelLoginArea, loginAreaLayout, loginCard, cardLayout,
                pnlUser, pnlPassword, pnlLogin, bottomActions
            })
            {
                control.RightToLeft = RightToLeft.Yes;
            }
        }

        private void ConfigurePremiumSurface()
        {
            // طبقة بصرية خفيفة تحافظ على طابع عربي RTL دون الاعتماد على صور إضافية.
            panelBrand.Paint -= panelBrand_Paint;
            panelBrand.Paint += panelBrand_Paint;

            panelLoginArea.Paint -= panelLoginArea_Paint;
            panelLoginArea.Paint += panelLoginArea_Paint;

            Resize -= LoginForm_Resize;
            Resize += LoginForm_Resize;

            loginCard.Resize -= LoginCard_Resize;
            loginCard.Resize += LoginCard_Resize;

            ApplyRoundedRegion(this, 16);
            ApplyRoundedRegion(loginCard, 18);
        }

        private void LoginForm_Resize(object sender, EventArgs e)
        {
            ApplyRoundedRegion(this, 16);
            ApplyRoundedRegion(loginCard, 18);
            panelBrand.Invalidate();
            panelLoginArea.Invalidate();
        }

        private void LoginCard_Resize(object sender, EventArgs e)
        {
            ApplyRoundedRegion(loginCard, 18);
        }

        private static void ApplyRoundedRegion(Control control, int radius)
        {
            if (control == null || control.Width <= 0 || control.Height <= 0)
                return;

            int diameter = Math.Min(
                Math.Max(8, radius * 2),
                Math.Min(control.Width, control.Height));

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddArc(0, 0, diameter, diameter, 180, 90);
                path.AddArc(control.Width - diameter, 0, diameter, diameter, 270, 90);
                path.AddArc(
                    control.Width - diameter,
                    control.Height - diameter,
                    diameter,
                    diameter,
                    0,
                    90);
                path.AddArc(
                    0,
                    control.Height - diameter,
                    diameter,
                    diameter,
                    90,
                    90);
                path.CloseFigure();

                Region oldRegion = control.Region;
                control.Region = new Region(path);
                if (oldRegion != null)
                    oldRegion.Dispose();
            }
        }

        private void panelBrand_Paint(object sender, PaintEventArgs e)
        {
            Rectangle bounds = panelBrand.ClientRectangle;
            if (bounds.Width <= 0 || bounds.Height <= 0)
                return;

            using (LinearGradientBrush brush = new LinearGradientBrush(
                bounds,
                Color.FromArgb(15, 23, 42),
                Color.FromArgb(15, 118, 110),
                125F))
            {
                e.Graphics.FillRectangle(brush, bounds);
            }

            using (SolidBrush glow = new SolidBrush(Color.FromArgb(24, 94, 234, 212)))
            {
                e.Graphics.FillEllipse(
                    glow,
                    new Rectangle(bounds.Width - 210, -90, 270, 270));
            }

            using (SolidBrush glow = new SolidBrush(Color.FromArgb(18, 255, 255, 255)))
            {
                e.Graphics.FillEllipse(
                    glow,
                    new Rectangle(-120, bounds.Height - 170, 250, 250));
            }

            using (Pen accent = new Pen(Color.FromArgb(94, 234, 212), 3F))
            {
                e.Graphics.DrawLine(
                    accent,
                    bounds.Width - 6,
                    42,
                    bounds.Width - 6,
                    bounds.Height - 42);
            }
        }

        private void panelLoginArea_Paint(object sender, PaintEventArgs e)
        {
            Rectangle bounds = panelLoginArea.ClientRectangle;
            if (bounds.Width <= 0 || bounds.Height <= 0)
                return;

            using (LinearGradientBrush brush = new LinearGradientBrush(
                bounds,
                Color.White,
                Color.FromArgb(241, 245, 249),
                90F))
            {
                e.Graphics.FillRectangle(brush, bounds);
            }

            using (SolidBrush accent = new SolidBrush(Color.FromArgb(12, 15, 118, 110)))
            {
                e.Graphics.FillEllipse(
                    accent,
                    new Rectangle(bounds.Width - 120, -40, 180, 180));
            }

            using (SolidBrush accent = new SolidBrush(Color.FromArgb(9, 37, 99, 235)))
            {
                e.Graphics.FillEllipse(
                    accent,
                    new Rectangle(-100, bounds.Height - 130, 180, 180));
            }
        }

        private void SetStartupStatus(Exception exception)
        {
            string message =
                exception == null
                    ? string.Empty
                    : exception.Message ?? string.Empty;

            if (message.Contains("SCHOOL_SYSTEM_INITIAL_ADMIN_PASSWORD"))
            {
                lblStatus.Text = "الحالة: يلزم إعداد حساب مدير النظام الأول من مسؤول النشر.";
                lblStatus.ForeColor = UIHelper.WarningColor;
                return;
            }

            lblStatus.Text = "الحالة: وضع الدخول جاهز، لكن قاعدة البيانات غير متاحة حاليًا.";
            lblStatus.ForeColor = UIHelper.WarningColor;
        }

        private void LoadSchoolBrandLogo(SchoolSettings branding)
        {
            if (lblBrandMark == null)
                return;

            if (schoolBrandImage != null)
            {
                lblBrandMark.BackgroundImage = null;
                schoolBrandImage.Dispose();
                schoolBrandImage = null;
            }

            if (branding == null || branding.Logo == null || branding.Logo.Length == 0)
            {
                lblBrandMark.Text = "م";
                lblBrandMark.BackgroundImage = null;
                lblBrandMark.BackColor = Color.FromArgb(13, 148, 136);
                return;
            }

            try
            {
                using (var stream = new System.IO.MemoryStream(branding.Logo))
                using (Image source = Image.FromStream(stream))
                {
                    schoolBrandImage = new Bitmap(source);
                }

                lblBrandMark.Text = string.Empty;
                lblBrandMark.BackgroundImage = schoolBrandImage;
                lblBrandMark.BackgroundImageLayout = ImageLayout.Zoom;
                lblBrandMark.BackColor = Color.White;
            }
            catch (Exception ex)
            {
                lblBrandMark.Text = "م";
                lblBrandMark.BackgroundImage = null;
                lblBrandMark.BackColor = Color.FromArgb(13, 148, 136);
                ApplicationLogger.LogException("تحميل شعار المدرسة في شاشة الدخول", ex);
            }
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

            pnlUser.BackColor = Color.FromArgb(240, 253, 250);
            txtUserName.BackColor = Color.FromArgb(240, 253, 250);
        }

        // تعريف الدالة txtUserName_Leave لتنفيذ العملية المرتبطة بها.
        private void txtUserName_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUserName.Text))
            {
                txtUserName.Text = "اسم المستخدم";
                txtUserName.ForeColor = UIHelper.TextDisabledColor;
            }

            pnlUser.BackColor = UIHelper.BackgroundColor;
            txtUserName.BackColor = UIHelper.BackgroundColor;
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

            pnlPassword.BackColor = Color.FromArgb(240, 253, 250);
            txtPassword.BackColor = Color.FromArgb(240, 253, 250);
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

            pnlPassword.BackColor = UIHelper.BackgroundColor;
            txtPassword.BackColor = UIHelper.BackgroundColor;
        }

        private void Input_Enter(object sender, EventArgs e)
        {
            Control input = sender as Control;
            if (input == null)
                return;

            input.BackColor = Color.FromArgb(240, 253, 250);
        }

        private void Input_Leave(object sender, EventArgs e)
        {
            Control input = sender as Control;
            if (input == null)
                return;

            input.BackColor = UIHelper.BackgroundColor;
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

            // كلمة المرور يجب أن تُرسل كما أدخلها المستخدم؛ لا نغيّر الأحرف أو الأرقام أو المسافات.
            string password = txtPassword.Text;

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
                lblStatus.Text = "الحالة: جارٍ التحقق من بيانات الدخول...";
                lblStatus.ForeColor = UIHelper.InfoColor;

                Refresh();

                // تمرير قيمة كلمة المرور إلى مسار التحقق.
                User authenticatedUser = userService.Authenticate(userName, password);

                ApplicationLogger.LogUsage("شاشة الدخول", "تسجيل دخول", "المستخدم '" + userName + "' نجح في تسجيل الدخول");
                lblStatus.Text = "الحالة: تم التحقق، جاري فتح النظام...";
                lblStatus.ForeColor = UIHelper.SuccessColor;

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
                _allowClose = true;

                Close();
            }
            catch (Exception ex)
            {
                ApplicationLogger.LogWarning("شاشة الدخول", "فشل تسجيل الدخول للمستخدم '" + userName + "': " + ex.Message);
                ApplicationLogger.LogException("تسجيل الدخول في شاشة الدخول", ex);
                ShowLoginFailure(ex);
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                lblStatus.Text = "الحالة: تعذر تسجيل الدخول";
                lblStatus.ForeColor = UIHelper.DangerColor;

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
            RequestCloseApplication();
        }

        // زر X المخصص في شاشة الدخول يستخدم مسار الإغلاق نفسه حتى لا تختلف النتيجة.
        private void btnWindowClose_Click(object sender, EventArgs e)
        {
            RequestCloseApplication();
        }

        private void RequestCloseApplication()
        {
            if (!UIHelper.ShowConfirmation(
                "هل تريد إغلاق نظام إدارة المدرسة؟",
                "تأكيد الإغلاق"))
            {
                return;
            }

            ApplicationLogger.LogUsage(
                "شاشة الدخول",
                "إغلاق النظام",
                "المستخدم اختار إغلاق التطبيق");

            AppShouldExit = true;
            DialogResult = DialogResult.Cancel;
            _allowClose = true;
            Close();
        }

        // يمنع الإغلاق غير المؤكد من زر X أو Alt+F4.
        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_allowClose)
                return;

            if (!UIHelper.ShowConfirmation(
                "هل تريد إغلاق نظام إدارة المدرسة؟",
                "تأكيد الإغلاق"))
            {
                e.Cancel = true;
                return;
            }

            ApplicationLogger.LogUsage(
                "شاشة الدخول",
                "إغلاق النظام",
                "المستخدم أغلق نافذة الدخول بعد تأكيد الإغلاق");

            AppShouldExit = true;
            DialogResult = DialogResult.Cancel;
            _allowClose = true;
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
