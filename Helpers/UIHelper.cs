using System;
using System.Drawing;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Krypton.Toolkit;


namespace SchoolSystem.Helpers
{

    public static class UIHelper
    {
        // تعريف الدالة KryptonManager لتنفيذ العملية المرتبطة بها.
        private static readonly KryptonManager KryptonThemeManager = new KryptonManager();


        // تعريف الدالة FromArgb لتنفيذ العملية المرتبطة بها.
        public static readonly Color PrimaryColor = Color.FromArgb(30, 41, 59);
        // تعريف الدالة FromArgb لتنفيذ العملية المرتبطة بها.
        public static readonly Color PrimaryDarkColor = Color.FromArgb(15, 23, 42);
        // تعريف الدالة FromArgb لتنفيذ العملية المرتبطة بها.
        public static readonly Color PrimaryLightColor = Color.FromArgb(226, 232, 240);
        // تعريف الدالة FromArgb لتنفيذ العملية المرتبطة بها.
        public static readonly Color SecondaryColor = Color.FromArgb(71, 85, 105);
        // تعريف الدالة FromArgb لتنفيذ العملية المرتبطة بها.
        public static readonly Color AccentColor = Color.FromArgb(15, 118, 110);
        // تعريف الدالة FromArgb لتنفيذ العملية المرتبطة بها.
        public static readonly Color SuccessColor = Color.FromArgb(22, 163, 74);
        // تعريف الدالة FromArgb لتنفيذ العملية المرتبطة بها.
        public static readonly Color WarningColor = Color.FromArgb(217, 119, 6);
        // تعريف الدالة FromArgb لتنفيذ العملية المرتبطة بها.
        public static readonly Color DangerColor = Color.FromArgb(198, 40, 40);
        // تعريف الدالة FromArgb لتنفيذ العملية المرتبطة بها.
        public static readonly Color InfoColor = Color.FromArgb(37, 99, 235);
        // تعريف الدالة FromArgb لتنفيذ العملية المرتبطة بها.
        public static readonly Color NeutralColor = Color.FromArgb(71, 85, 105);
        // تعريف الدالة FromArgb لتنفيذ العملية المرتبطة بها.
        public static readonly Color ExportColor = Color.FromArgb(13, 148, 136);

        public static readonly Color SearchColor = InfoColor;
        // تعريف الدالة FromArgb لتنفيذ العملية المرتبطة بها.
        public static readonly Color BackgroundColor = Color.FromArgb(248, 250, 252);

        public static readonly Color SurfaceColor = Color.White;
        // تعريف الدالة FromArgb لتنفيذ العملية المرتبطة بها.
        public static readonly Color SurfaceSecondaryColor = Color.FromArgb(241, 245, 249);

        public static readonly Color SurfaceElevatedColor = Color.White;
        // تعريف الدالة FromArgb لتنفيذ العملية المرتبطة بها.
        public static readonly Color DisabledSurfaceColor = Color.FromArgb(243, 244, 246);

        public static readonly Color AlternateRowColor = BackgroundColor;
        // تعريف الدالة FromArgb لتنفيذ العملية المرتبطة بها.
        public static readonly Color BorderColor = Color.FromArgb(203, 213, 225);

        public static readonly Color DividerColor = BorderColor;
        // تعريف الدالة FromArgb لتنفيذ العملية المرتبطة بها.
        public static readonly Color MutedTextColor = Color.FromArgb(71, 85, 105);
        // تعريف الدالة FromArgb لتنفيذ العملية المرتبطة بها.
        public static readonly Color TextDisabledColor = Color.FromArgb(148, 163, 184);
        // تعريف الدالة FromArgb لتنفيذ العملية المرتبطة بها.
        public static readonly Color TextColor = Color.FromArgb(15, 23, 42);

        public static readonly Color TextPrimaryColor = TextColor;

        public static readonly Color TextSecondaryColor = SecondaryColor;
        // تعريف الدالة FromArgb لتنفيذ العملية المرتبطة بها.
        public static readonly Color HoverColor = Color.FromArgb(226, 232, 240);
        // تعريف الدالة FromArgb لتنفيذ العملية المرتبطة بها.
        public static readonly Color PressedColor = Color.FromArgb(203, 213, 225);
        // تعريف الدالة FromArgb لتنفيذ العملية المرتبطة بها.
        public static readonly Color DisabledColor = Color.FromArgb(148, 163, 184);

        public static readonly Color FocusColor = InfoColor;



        public const string FontFamily = "Tahoma";

        public const float TitleFontSize = 16F;

        public const float HeadingFontSize = 10.5F;

        public const float SectionFontSize = 11F;

        public const float BodyFontSize = 10F;

        public const float CaptionFontSize = 9F;

        public const int Space4 = 4;

        public const int Space8 = 8;

        public const int Space12 = 12;

        public const int Space16 = 16;

        public const int Space20 = 20;

        public const int Space24 = 24;

        public const int Space32 = 32;

        // ================================================================
        // SchoolSystem Visual Identity — single source of truth
        // ================================================================
        public static readonly Color BrandColor = Color.FromArgb(13, 148, 136);
        public static readonly Color BrandHoverColor = Color.FromArgb(20, 184, 166);
        public static readonly Color BrandHighlightColor = Color.FromArgb(94, 234, 212);
        public static readonly Color BrandMutedTextColor = Color.FromArgb(203, 213, 225);
        public static readonly Color BrandFeatureTextColor = Color.FromArgb(226, 232, 240);
        public static readonly Color LoginSecondaryTextColor = Color.FromArgb(100, 116, 139);
        public static readonly Color LoginInputFocusColor = Color.FromArgb(240, 253, 250);
        public static readonly Color LoginDangerHoverColor = Color.FromArgb(254, 242, 242);
        public static readonly Color LoginDangerPressedColor = Color.FromArgb(254, 226, 226);
        public static readonly Color LoginDangerTextColor = Color.FromArgb(185, 28, 28);

        public const string LoginDeveloperCredits =
            "تطوير النظام\r\nالمهندسين: مهند علي علي علي سعد • عبد الرحمن سعد علي سعد • سامي الحاج";

        public const string LoginWindowTitle = "تسجيل الدخول | نظام إدارة المدرسة";



        // تعريف الدالة Font لتنفيذ العملية المرتبطة بها.
        private static readonly Font BodyFont = new Font(FontFamily, BodyFontSize);
        // تعريف الدالة Font لتنفيذ العملية المرتبطة بها.
        private static readonly Font SectionFont = new Font(FontFamily, SectionFontSize, FontStyle.Bold);
        // تعريف الدالة Font لتنفيذ العملية المرتبطة بها.
        private static readonly Font HeadingFont = new Font(FontFamily, HeadingFontSize, FontStyle.Bold);
        // تعريف الدالة Font لتنفيذ العملية المرتبطة بها.
        private static readonly Font TitleFont = new Font(FontFamily, TitleFontSize, FontStyle.Bold);
        // تعريف الدالة Font لتنفيذ العملية المرتبطة بها.
        private static readonly Font GridFont = new Font(FontFamily, BodyFontSize);

        // تعريف الدالة ApplyStyle لتنفيذ العملية المرتبطة بها.
        public static void ApplyStyle(Form form)
        {
            ApplyKryptonTheme();
            ApplyTheme(form);
            ApplyKryptonTheme((Control)form);
            ApplyInputValidation(form);
            ApplyResponsiveLayout(form);
        }

        // تعريف الدالة ApplyStyle لتنفيذ العملية المرتبطة بها.
        public static void ApplyStyle(UserControl uc)
        {
            ApplyKryptonTheme();
            ApplyTheme(uc);
            ApplyKryptonTheme((Control)uc);
            ApplyInputValidation(uc);
            ApplyResponsiveLayout(uc);
        }

        /// <summary>
        /// يطبّق قواعد التحقق من الحقول (فلترة الأحرف، أقصى طول، والتحقق عند
        /// مغادرة الحقل) وفق اصطلاح أسماء الحقول دون تغيير أي تنسيق ونمط.
        /// يُستدعى في الشاشات التي يدير المصمم تنسيقها بنفسه ولا تستدعي ApplyStyle.
        /// </summary>
        // تعريف الدالة ApplyFieldValidation لتنفيذ العملية المرتبطة بها.
        public static void ApplyFieldValidation(Control root)
        {
            if (root == null || IsDesignMode(root))
                return;

            ApplyInputValidation(root);
        }

        /// <summary>
        /// يطبق قواعد تخطيط عامة لا تغيّر مواقع المصمم الصريحة، لكنها تمنع
        /// اختفاء العناصر وتوحّد اتجاه الحاويات والأزرار والجداول في جميع الشاشات.
        /// </summary>
        // تعريف الدالة ApplyConsistentLayout لتنفيذ العملية المرتبطة بها.
        public static void ApplyConsistentLayout(Control root)
        {
            if (root == null || IsDesignMode(root))
                return;

            ApplyConsistentLayoutRecursive(root);
        }

        // تعريف الدالة ApplyConsistentLayoutRecursive لتنفيذ العملية المرتبطة بها.
        private static void ApplyConsistentLayoutRecursive(Control parent)
        {
            foreach (Control child in parent.Controls)
            {
                // الاتجاه يُورث من الجذر (Form/UserControl مفعّل RTL)؛ لا نطغى على الحقول
                // التي ضبطها المصمم عمدًا بـ RightToLeft.No (مثل كلمات المرور والبيانات التقنية).

                TableLayoutPanel table = child as TableLayoutPanel;
                if (table != null)
                {

                    // اختلاف حجم الصفوف قد يقص الحقول أو يخفي شريط الأزرار.
                }


                // بعض النماذج تستخدم FlowLayoutPanel كأشرطة أزرار، وأي تغيير عام هنا
                // قد يقص الأزرار أو يزيحها عند اختلاف حجم الشاشة.

                Button button = child as Button;
                if (button != null && ShouldApplyCentralControlStyle(button))
                    StyleActionButton(button);

                KryptonButton kryptonButton = child as KryptonButton;
                if (kryptonButton != null && ShouldApplyCentralControlStyle(kryptonButton))
                    StyleButton(kryptonButton, AccentColor);

                DataGridView grid = child as DataGridView;
                if (grid != null && ShouldApplyCentralGridStyle(grid))
                    StyleDataGridView(grid);

                ApplyConsistentLayoutRecursive(child);
            }
        }

        /// <summary>
        /// يضمن إنهاء دورة التخطيط بعد تطبيق التنسيق حتى تظهر أشرطة الأزرار
        /// والجداول الثنائية الأبعاد بالشكل الصحيح من أول عرض للشاشة، لا بعد أول Resize.
        /// </summary>
        private static void FinalizeLayout(Control root)
        {
            if (root == null || IsDesignMode(root))
                return;

            root.SuspendLayout();
            try
            {
                NormalizeLayoutRecursive(root);
            }
            finally
            {
                root.ResumeLayout(true);
            }
        }

        // يرمم الحاويات ذات العمودين عندما تكون أعمدة المصمم أوسع من الحاوية.
        private static void NormalizeLayoutRecursive(Control parent)
        {
            foreach (Control child in parent.Controls)
            {
                TableLayoutPanel table = child as TableLayoutPanel;
                if (table != null)
                {
                    NormalizeTwoColumnTable(table);
                }

                FlowLayoutPanel flow = child as FlowLayoutPanel;
                if (flow != null)
                {
                    NormalizeButtonFlowPanel(flow);
                }

                ApplyLayoutDirectionDefaults(child);
                NormalizeLayoutRecursive(child);
            }

            parent.PerformLayout();
        }

        private static void NormalizeTwoColumnTable(TableLayoutPanel table)
        {
            if (table == null || table.ColumnCount != 2 || table.ClientSize.Width <= 0)
                return;

            int[] widths = table.GetColumnWidths();
            if (widths == null || widths.Length != 2)
                return;

            int usableWidth = Math.Max(0, table.ClientSize.Width - table.Padding.Horizontal);
            if (widths[0] + widths[1] <= usableWidth + 1)
                return;

            ColumnStyle second = table.ColumnStyles[1];
            second.SizeType = SizeType.Percent;
            second.Width = 100F;
        }

        private static void NormalizeButtonFlowPanel(FlowLayoutPanel flow)
        {
            if (flow == null)
                return;

            int buttonCount = 0;
            int preferredWidth = flow.Padding.Horizontal;

            int maxButtonHeight = 0;

            foreach (Control child in flow.Controls)
            {
                Button button = child as Button;
                KryptonButton kryptonButton = child as KryptonButton;

                if (button == null && kryptonButton == null)
                    continue;

                buttonCount++;

                int width = child.Width;
                int height = child.Height;

                if (button != null)
                    EnsureButtonTextFits(button);
                else
                    EnsureKryptonButtonTextFits(kryptonButton);

                width = child.Width;
                height = child.Height;

                preferredWidth += width + child.Margin.Horizontal;
                maxButtonHeight = Math.Max(
                    maxButtonHeight,
                    height + child.Margin.Vertical);
            }

            if (buttonCount == 0)
                return;

            flow.AutoScroll = true;

            bool horizontalOverflow =
                flow.ClientSize.Width > 0 &&
                preferredWidth > flow.ClientSize.Width;

            bool enoughHeightForSecondRow =
                flow.ClientSize.Height > 0 &&
                flow.ClientSize.Height >= (maxButtonHeight * 2) + flow.Padding.Vertical;

            // نلتف إلى سطرين فقط عندما يكون هناك فائض أفقي ومساحة رأسية حقيقية.
            // خلاف ذلك نُبقي السطر واحدًا مع تمرير أفقي، منعًا لقص الصف الثاني.
            if (horizontalOverflow && enoughHeightForSecondRow)
                flow.WrapContents = true;

            foreach (Control child in flow.Controls)
            {
                if (child is Button || child is KryptonButton)
                    child.Anchor = AnchorStyles.None;
            }

            flow.PerformLayout();
        }

        private static void ApplyLayoutDirectionDefaults(Control control)
        {
            if (control == null)
                return;

            if (control is Label label &&
                label.RightToLeft == RightToLeft.Inherit)
            {
                label.RightToLeft = RightToLeft.Yes;
            }

            else if (control is TextBox textBox &&
                     textBox.RightToLeft == RightToLeft.Inherit)
            {
                textBox.RightToLeft = RightToLeft.Yes;
            }

            else if (control is ComboBox comboBox &&
                     comboBox.RightToLeft == RightToLeft.Inherit)
            {
                comboBox.RightToLeft = RightToLeft.Yes;
            }

            else if (control is CheckBox checkBox &&
                     checkBox.RightToLeft == RightToLeft.Inherit)
            {
                checkBox.RightToLeft = RightToLeft.Yes;
            }
        }

        // تعريف الدالة ApplyResponsiveLayout لتنفيذ العملية المرتبطة بها.
        public static void ApplyResponsiveLayout(Control root)
        {
            if (root == null)
                return;

            if (root is Form form)
            {
                form.AutoScaleMode = AutoScaleMode.Font;
                form.AutoSizeMode = AutoSizeMode.GrowOnly;
                form.RightToLeft = RightToLeft.Yes;
                form.RightToLeftLayout = true;
                // AutoScroll يظل تحت تحكم Designer؛ فرضه على كل Form كان يضيف

            }

            else if (root is UserControl userControl)
            {
                userControl.RightToLeft = RightToLeft.Yes;
            }

            ApplyResponsiveLayoutRecursive(root);
        }

        // تعريف الدالة ApplyResponsiveLayoutRecursive لتنفيذ العملية المرتبطة بها.
        private static void ApplyResponsiveLayoutRecursive(Control control)
        {
            foreach (Control child in control.Controls)
            {
                // الحفاظ على هندسة المصمم كما هي؛ الاتجاه يورث من الجذر.

                if (child is DataGridView grid)
                {
                    grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;


                }

                ApplyResponsiveLayoutRecursive(child);
            }
        }

        /// <summary>
        /// يربط التحقق الأساسي بالحقول النصية في جميع النماذج. التحقق الدلالي
        /// لا يحل محل ValidateInputs داخل النموذج أو Validate داخل الخدمة، بل يمنع
        /// الإدخال الواضح الخطأ قبل وصوله إلى مراحل الحفظ.
        /// </summary>
        // تعريف الدالة ApplyInputValidation لتنفيذ العملية المرتبطة بها.
        public static void ApplyInputValidation(Control root)
        {
            if (root == null || IsDesignMode(root))
                return;

            foreach (Control child in root.Controls)
            {
                TextBox textBox = child as TextBox;
                if (textBox != null)
                {
                    ConfigureTextInput(textBox, textBox.Name);
                    textBox.KeyPress -= TextInput_KeyPress;

                    textBox.KeyPress += TextInput_KeyPress;
                    textBox.Leave -= TextInput_Leave;

                    textBox.Leave += TextInput_Leave;
                    textBox.Validating -= TextInput_Validating;

                    textBox.Validating += TextInput_Validating;
                }

                DateTimePicker dateTimePicker = child as DateTimePicker;
                if (dateTimePicker != null)
                    ConfigureDateInput(dateTimePicker);

                ComboBox comboBox = child as ComboBox;
                if (comboBox != null)
                    ConfigureComboInput(comboBox);

                NumericUpDown numericUpDown = child as NumericUpDown;
                if (numericUpDown != null)
                    ConfigureNumericInput(numericUpDown);

                KryptonTextBox kryptonTextBox = child as KryptonTextBox;
                if (kryptonTextBox != null)
                {
                    ConfigureTextInput(kryptonTextBox, kryptonTextBox.Name);
                    kryptonTextBox.KeyPress -= TextInput_KeyPress;

                    kryptonTextBox.KeyPress += TextInput_KeyPress;
                    kryptonTextBox.Leave -= TextInput_Leave;

                    kryptonTextBox.Leave += TextInput_Leave;
                    kryptonTextBox.Validating -= TextInput_Validating;

                    kryptonTextBox.Validating += TextInput_Validating;
                }

                ApplyInputValidation(child);
            }
        }

        // تعريف الدالة ConfigureDateInput لتنفيذ العملية المرتبطة بها.
        private static readonly ConditionalWeakTable<DateTimePicker, ToolTip> DateToolTips =
            new ConditionalWeakTable<DateTimePicker, ToolTip>();

        private static void ConfigureDateInput(DateTimePicker picker)
        {
            if (picker == null)
                return;

            string key = (picker.Name ?? string.Empty).ToLowerInvariant();

            // حقول الوقت فقط (مثل بداية/نهاية الحصة ووقت الحضور والانصراف) لا تُعامل كتاريخ.
            if (IsTimeOnlyDatePicker(picker, key))
                return;

            picker.RightToLeft = RightToLeft.Yes;
            picker.RightToLeftLayout = true;
            picker.Format = DateTimePickerFormat.Custom;
            picker.CustomFormat = "dd/MM/yyyy";
            picker.ShowUpDown = false;

            if (ContainsAny(key, "birth", "ميلاد"))
            {
                picker.MaxDate = DateTime.Today;
                if (picker.Value.Date > picker.MaxDate)
                    picker.Value = picker.MaxDate;
            }

            ToolTip tip;
            if (!DateToolTips.TryGetValue(picker, out tip))
            {
                tip = new ToolTip
                {
                    AutoPopDelay = 12000,
                    InitialDelay = 350,
                    ReshowDelay = 100,
                    ShowAlways = true
                };
                DateToolTips.Add(picker, tip);
            }

            picker.ValueChanged -= DatePicker_ValueChanged;
            picker.ValueChanged += DatePicker_ValueChanged;

            picker.KeyDown -= DatePicker_KeyDown;
            picker.KeyDown += DatePicker_KeyDown;

            ContextMenuStrip menu = picker.ContextMenuStrip;
            if (menu == null)
            {
                menu = new ContextMenuStrip();
                picker.ContextMenuStrip = menu;
            }

            EnsureDateMenuItem(
                menu,
                "إدخال تاريخ هجري",
                "حوّل تاريخًا هجريًا إلى ميلادي ثم استخدمه في الحقل.",
                delegate { ShowHijriDateInputDialog(picker); });

            EnsureDateMenuItem(
                menu,
                "نسخ التاريخين",
                "نسخ التاريخ الميلادي والهجري إلى الحافظة.",
                delegate
                {
                    try
                    {
                        Clipboard.SetText(FormatDateDual(picker.Value.Date));
                    }
                    catch
                    {
                        // فشل الحافظة لا يؤثر على عمل التاريخ.
                    }
                });

            UpdateDatePresentation(picker, tip);
        }

        private static bool IsTimeOnlyDatePicker(DateTimePicker picker, string key)
        {
            string format = (picker.CustomFormat ?? string.Empty).ToLowerInvariant();

            if (format.Contains("h") || format.Contains("minute") || format.Contains("second"))
                return true;

            return ContainsAny(
                key,
                "start",
                "end",
                "departure",
                "arrival",
                "checkin",
                "checkout",
                "وقت",
                "بداية",
                "نهاية");
        }

        private static void EnsureDateMenuItem(
            ContextMenuStrip menu,
            string text,
            string accessibleDescription,
            EventHandler handler)
        {
            foreach (ToolStripItem item in menu.Items)
            {
                if (string.Equals(item.Text, text, StringComparison.Ordinal))
                    return;
            }

            ToolStripMenuItem menuItem = new ToolStripMenuItem(text);
            menuItem.AccessibleName = text;
            menuItem.AccessibleDescription = accessibleDescription;
            menuItem.RightToLeft = RightToLeft.Yes;
            menuItem.Click += handler;
            menu.Items.Add(menuItem);
        }

        private static void DatePicker_ValueChanged(object sender, EventArgs e)
        {
            DateTimePicker picker = sender as DateTimePicker;
            if (picker == null)
                return;

            ToolTip tip;
            if (DateToolTips.TryGetValue(picker, out tip))
                UpdateDatePresentation(picker, tip);
        }

        private static void DatePicker_KeyDown(object sender, KeyEventArgs e)
        {
            DateTimePicker picker = sender as DateTimePicker;
            if (picker == null)
                return;

            if (e.Control && e.KeyCode == Keys.H)
            {
                ShowHijriDateInputDialog(picker);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private static void UpdateDatePresentation(DateTimePicker picker, ToolTip tip)
        {
            if (picker == null)
                return;

            string dual = FormatDateDual(picker.Value.Date);
            string help =
                "ميلادي: " + dual.Split('|')[0].Trim() +
                Environment.NewLine +
                "هجري: " + GetHijriDateText(picker.Value.Date) +
                Environment.NewLine +
                Environment.NewLine +
                "لإدخال التاريخ الهجري: زر الفأرة الأيمن ← إدخال تاريخ هجري، أو Ctrl+H.";

            tip.SetToolTip(picker, help);
            picker.AccessibleName = string.IsNullOrWhiteSpace(picker.AccessibleName)
                ? "التاريخ"
                : picker.AccessibleName;
            picker.AccessibleDescription = help;
        }

        public static string FormatDateDual(DateTime date)
        {
            return date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) +
                   " م | " +
                   GetHijriDateText(date);
        }

        private static string GetHijriDateText(DateTime date)
        {
            try
            {
                UmAlQuraCalendar calendar = new UmAlQuraCalendar();
                return calendar.GetDayOfMonth(date).ToString("00", CultureInfo.InvariantCulture) +
                       "/" +
                       calendar.GetMonth(date).ToString("00", CultureInfo.InvariantCulture) +
                       "/" +
                       calendar.GetYear(date).ToString("0000", CultureInfo.InvariantCulture) +
                       " هـ";
            }
            catch (ArgumentOutOfRangeException)
            {
                HijriCalendar calendar = new HijriCalendar();
                return calendar.GetDayOfMonth(date).ToString("00", CultureInfo.InvariantCulture) +
                       "/" +
                       calendar.GetMonth(date).ToString("00", CultureInfo.InvariantCulture) +
                       "/" +
                       calendar.GetYear(date).ToString("0000", CultureInfo.InvariantCulture) +
                       " هـ";
            }
        }

        public static bool TryParseDate(string input, out DateTime date)
        {
            date = DateTime.MinValue;

            if (string.IsNullOrWhiteSpace(input))
                return false;

            string normalized = NormalizeDateDigits(input.Trim());
            bool explicitHijri =
                normalized.IndexOf("هـ", StringComparison.OrdinalIgnoreCase) >= 0 ||
                normalized.IndexOf("هجري", StringComparison.OrdinalIgnoreCase) >= 0;

            MatchCollection parts = Regex.Matches(normalized, @"\d{1,4}");
            if (parts.Count != 3)
                return TryParseGregorianDate(normalized, out date);

            int a = int.Parse(parts[0].Value, CultureInfo.InvariantCulture);
            int b = int.Parse(parts[1].Value, CultureInfo.InvariantCulture);
            int c = int.Parse(parts[2].Value, CultureInfo.InvariantCulture);

            if (explicitHijri || a >= 1300 && a <= 1600 || c >= 1300 && c <= 1600)
            {
                int year;
                int month;
                int day;

                if (a >= 1300 && a <= 1600)
                {
                    year = a;
                    month = b;
                    day = c;
                }
                else if (c >= 1300 && c <= 1600)
                {
                    year = c;
                    month = b;
                    day = a;
                }
                else
                {
                    year = a;
                    month = b;
                    day = c;
                }

                return TryConvertHijriToGregorian(year, month, day, out date);
            }

            return TryParseGregorianDate(normalized, out date);
        }

        private static bool TryParseGregorianDate(string input, out DateTime date)
        {
            string[] formats =
            {
                "dd/MM/yyyy", "d/M/yyyy",
                "yyyy/MM/dd", "yyyy/M/d",
                "dd-MM-yyyy", "d-M-yyyy",
                "yyyy-MM-dd", "yyyy-M-d"
            };

            if (DateTime.TryParseExact(
                input,
                formats,
                CultureInfo.InvariantCulture,
                DateTimeStyles.AllowWhiteSpaces,
                out date))
            {
                return true;
            }

            return DateTime.TryParse(
                input,
                CultureInfo.InvariantCulture,
                DateTimeStyles.AllowWhiteSpaces,
                out date);
        }

        private static bool TryConvertHijriToGregorian(
            int year,
            int month,
            int day,
            out DateTime date)
        {
            date = DateTime.MinValue;

            try
            {
                UmAlQuraCalendar calendar = new UmAlQuraCalendar();
                date = calendar.ToDateTime(year, month, day, 0, 0, 0, 0);
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                try
                {
                    HijriCalendar calendar = new HijriCalendar();
                    date = calendar.ToDateTime(year, month, day, 0, 0, 0, 0);
                    return true;
                }
                catch (ArgumentOutOfRangeException)
                {
                    return false;
                }
            }
        }

        private static string NormalizeDateDigits(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            char[] chars = value.ToCharArray();

            for (int i = 0; i < chars.Length; i++)
            {
                if (chars[i] >= '\u0660' && chars[i] <= '\u0669')
                    chars[i] = (char)('0' + (chars[i] - '\u0660'));
                else if (chars[i] >= '\u06F0' && chars[i] <= '\u06F9')
                    chars[i] = (char)('0' + (chars[i] - '\u06F0'));
            }

            return new string(chars);
        }

        private static void ShowHijriDateInputDialog(DateTimePicker picker)
        {
            using (Form dialog = new Form())
            using (Label title = new Label())
            using (Label current = new Label())
            using (Label inputLabel = new Label())
            using (TextBox input = new TextBox())
            using (Button apply = new Button())
            using (Button cancel = new Button())
            {
                dialog.Text = "إدخال التاريخ الهجري";
                dialog.StartPosition = FormStartPosition.CenterParent;
                dialog.ClientSize = new Size(480, 205);
                dialog.MinimumSize = new Size(480, 205);
                dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialog.MaximizeBox = false;
                dialog.MinimizeBox = false;
                dialog.RightToLeft = RightToLeft.Yes;
                dialog.RightToLeftLayout = true;
                dialog.AcceptButton = apply;
                dialog.CancelButton = cancel;

                title.Text = "أدخل التاريخ الهجري";
                title.Font = new Font(FontFamily, 12F, FontStyle.Bold);
                title.Dock = DockStyle.Top;
                title.Height = 34;
                title.TextAlign = ContentAlignment.MiddleCenter;

                current.Text = "التاريخ الحالي: " + FormatDateDual(picker.Value.Date);
                current.Dock = DockStyle.Top;
                current.Height = 40;
                current.TextAlign = ContentAlignment.MiddleCenter;

                inputLabel.Text = "التاريخ الهجري (مثال: 15/04/1448):";
                inputLabel.Dock = DockStyle.Top;
                inputLabel.Height = 30;
                inputLabel.TextAlign = ContentAlignment.MiddleRight;

                input.Dock = DockStyle.Top;
                input.Height = 30;
                input.Font = new Font(FontFamily, 10F);
                input.RightToLeft = RightToLeft.Yes;

                Panel buttons = new Panel();
                buttons.Dock = DockStyle.Bottom;
                buttons.Height = 52;

                apply.Text = "تطبيق";
                apply.DialogResult = DialogResult.OK;
                apply.Width = 100;
                apply.Height = 34;
                apply.Location = new Point(255, 8);

                cancel.Text = "إلغاء";
                cancel.DialogResult = DialogResult.Cancel;
                cancel.Width = 100;
                cancel.Height = 34;
                cancel.Location = new Point(145, 8);

                buttons.Controls.Add(apply);
                buttons.Controls.Add(cancel);

                dialog.Controls.Add(buttons);
                dialog.Controls.Add(input);
                dialog.Controls.Add(inputLabel);
                dialog.Controls.Add(current);
                dialog.Controls.Add(title);

                if (dialog.ShowDialog(picker.FindForm()) != DialogResult.OK)
                    return;

                if (!TryParseDate(input.Text, out DateTime parsed))
                {
                    ShowWarning("صيغة التاريخ الهجري غير صحيحة. استخدم مثلًا: 15/04/1448.");
                    return;
                }

                try
                {
                    if (parsed < picker.MinDate || parsed > picker.MaxDate)
                    {
                        ShowWarning(
                            "التاريخ المحدد خارج النطاق المسموح لهذا الحقل." +
                            Environment.NewLine +
                            "الميلادي: " + FormatDateDual(parsed));
                        return;
                    }

                    picker.Value = parsed;
                    picker.Focus();
                }
                catch (ArgumentOutOfRangeException)
                {
                    ShowWarning("التاريخ المحدد غير صالح لهذا الحقل.");
                }
            }
        }



        // تعريف الدالة ConfigureComboInput لتنفيذ العملية المرتبطة بها.
        private static void ConfigureComboInput(ComboBox comboBox)
        {
            if (comboBox == null)
                return;

            if (comboBox.DropDownStyle == ComboBoxStyle.DropDownList)
                return;

            // مربعات البحث بقوائم سجل مبقا قابلة للكتابة بحكم وظيفتها.
            string key = (comboBox.Name ?? string.Empty).ToLowerInvariant();
            if (ContainsAny(key, "search", "بحث", "filter", "تصفية"))
                return;

            comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        // تعريف الدالة ConfigureNumericInput لتنفيذ العملية المرتبطة بها.
        private static void ConfigureNumericInput(NumericUpDown numericUpDown)
        {
            if (numericUpDown == null)
                return;

            if (numericUpDown.Value < numericUpDown.Minimum)
                numericUpDown.Value = numericUpDown.Minimum;

            else if (numericUpDown.Value > numericUpDown.Maximum)

                numericUpDown.Value = numericUpDown.Maximum;
        }

        // تعريف الدالة ConfigureTextInput لتنفيذ العملية المرتبطة بها.
        private static void ConfigureTextInput(Control control, string controlName)
        {
            string key = (controlName ?? string.Empty).ToLowerInvariant();
            if (IsEmailField(key))
            {
                SetMaxLength(control, 254);

                return;
            }

            if (IsPhoneField(key))
            {
                SetMaxLength(control, 20);

                return;
            }

            if (IsAcademicYearField(key))
            {
                SetMaxLength(control, 9);

                return;
            }

            if (IsMoneyField(key))
            {
                SetMaxLength(control, 40);

                return;
            }

            if (IsIdentityOrNumericField(key))
            {
                SetMaxLength(control, 30);

                return;
            }

            if (IsPersonNameField(key) || IsTextOnlyField(key))
            {
                SetMaxLength(control, 150);

                return;
            }

            if (IsLongTextField(key))
                SetMaxLength(control, 1000);
        }

        // تعريف الدالة SetMaxLength لتنفيذ العملية المرتبطة بها.
        private static void SetMaxLength(Control control, int maxLength)
        {
            TextBox textBox = control as TextBox;
            if (textBox != null && textBox.MaxLength == 32767)
                textBox.MaxLength = maxLength;

            KryptonTextBox kryptonTextBox = control as KryptonTextBox;
            if (kryptonTextBox != null && kryptonTextBox.MaxLength == 32767)
                kryptonTextBox.MaxLength = maxLength;
        }

        // تعريف الدالة TextInput_KeyPress لتنفيذ العملية المرتبطة بها.
        private static void TextInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            Control control = sender as Control;
            string key = ((control == null ? string.Empty : control.Name) ?? string.Empty).ToLowerInvariant();
            if (char.IsControl(e.KeyChar))
                return;

            if (IsEmailField(key))
            {
                if (!(char.IsLetterOrDigit(e.KeyChar) || ".-_+@".IndexOf(e.KeyChar) >= 0))
                    e.Handled = true;

                return;
            }

            if (IsAcademicYearField(key))
            {
                Control textBox = sender as Control;
                if (char.IsDigit(e.KeyChar))
                    return;
                if (e.KeyChar == '/' && textBox != null && textBox.Text.IndexOf('/') < 0)
                    return;
                e.Handled = true;

                return;
            }

            if (IsPhoneField(key) || IsIdentityOrNumericField(key))
            {
                if (!char.IsDigit(e.KeyChar))
                    e.Handled = true;

                return;
            }

            if (IsMoneyField(key))
            {
                Control textBox = sender as Control;
                if (char.IsDigit(e.KeyChar))
                    return;
                if ((e.KeyChar == '.' || e.KeyChar == ',') && textBox != null &&
                    textBox.Text.IndexOfAny(new[] { '.', ',' }) < 0)

                    return;
                e.Handled = true;

                return;
            }

            if ((IsPersonNameField(key) || IsTextOnlyField(key)) &&
                !(char.IsLetter(e.KeyChar) || char.IsWhiteSpace(e.KeyChar) || "-،,.()".IndexOf(e.KeyChar) >= 0))
                e.Handled = true;
        }

        // تعريف الدالة TextInput_Leave لتنفيذ العملية المرتبطة بها.
        private static void TextInput_Leave(object sender, EventArgs e)
        {
            Control control = sender as Control;
            TextBox textBox = control as TextBox;
            if (textBox != null)
                textBox.Text = NormalizeText(textBox.Text);

            KryptonTextBox kryptonTextBox = control as KryptonTextBox;
            if (kryptonTextBox != null)
                kryptonTextBox.Text = NormalizeText(kryptonTextBox.Text);
        }

        // تعريف الدالة TextInput_Validating لتنفيذ العملية المرتبطة بها.
        private static void TextInput_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Control control = sender as Control;
            string key = ((control == null ? string.Empty : control.Name) ?? string.Empty).ToLowerInvariant();
            string value = control == null ? string.Empty : control.Text.Trim();
            if (value.Length == 0)
                return;

            if (IsEmailField(key) && !IsValidEmail(value))
            {
                e.Cancel = true;
                FocusAndWarn(control, "يرجى إدخال بريد إلكتروني صحيح.");
            }

            else if (IsPhoneField(key) && !IsValidPhoneDigitsOnly(value))
            {
                e.Cancel = true;
                FocusAndWarn(control, "يرجى إدخال رقم هاتف مكوّن من أرقام فقط.");
            }

            else if (IsAcademicYearField(key) && !IsValidAcademicYear(value))
            {
                e.Cancel = true;
                FocusAndWarn(control, "يرجى إدخال العام الدراسي بصيغة صحيحة مثل 1448/1449.");
            }

            else if (IsTextOnlyField(key) && !IsValidArabicOrLatinName(value, 2))
            {
                e.Cancel = true;
                FocusAndWarn(control, "هذا الحقل يقبل الأحرف والمسافات فقط.");
            }
        }

        // تعريف الدالة NormalizeText لتنفيذ العملية المرتبطة بها.
        private static string NormalizeText(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            string[] parts = value.Trim().Split(new[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            return string.Join(" ", parts);
        }

        // تعريف الدالة IsPersonNameField لتنفيذ العملية المرتبطة بها.
        private static bool IsPersonNameField(string key)
        {

            return ContainsAny(key, "fullname", "firstname", "lastname", "middlename", "studentname", "teachername", "guardianname", "fathername", "mothername", "drivername", "partyname", "payeename", "routename", "stagename", "classname", "roomname", "subjectname", "اسم", "الاسم", "ولي", "اب", "أب", "ام", "أم");
        }

        // تعريف الدالة IsTextOnlyField لتنفيذ العملية المرتبطة بها.
        private static bool IsTextOnlyField(string key)
        {

            return ContainsAny(key, "birthplace", "nationality", "governorate", "district", "guardianrelation", "guardianjob", "previousschool", "previousclass", "transferreason", "specialization", "department", "jobtitle", "position", "مكانالميلاد", "الجنسية", "المحافظة", "المديرية", "صلة", "وظيفة", "تخصص", "المسمى", "القسم", "المدرسةالسابقة", "الصفالسابق", "سببالنقل");
        }

        // تعريف الدالة IsAcademicYearField لتنفيذ العملية المرتبطة بها.
        private static bool IsAcademicYearField(string key)
        {

            return ContainsAny(key, "academicyear", "العامالدراسي");
        }

        // تعريف الدالة IsPhoneField لتنفيذ العملية المرتبطة بها.
        private static bool IsPhoneField(string key)
        {

            return ContainsAny(key, "phone", "mobile", "tel", "telephone", "هاتف", "جوال", "موبايل");
        }

        // تعريف الدالة IsEmailField لتنفيذ العملية المرتبطة بها.
        private static bool IsEmailField(string key)
        {

            return ContainsAny(key, "email", "mail", "بريد");
        }

        // تعريف الدالة IsMoneyField لتنفيذ العملية المرتبطة بها.
        private static bool IsMoneyField(string key)
        {

            return key == "txtfee" || ContainsAny(key, "amount", "price", "salary", "wage", "voucheramount", "expenseamount", "planamount", "discount", "deduction", "deductions", "paidamount", "remainingamount", "registrationfee", "netamount", "totalamount", "txttotal", "housing", "transport", "allowance", "other", "المبلغ", "السعر", "الراتب", "رسوم", "تكلفة");
        }

        // تعريف الدالة IsIdentityOrNumericField لتنفيذ العملية المرتبطة بها.
        private static bool IsIdentityOrNumericField(string key)
        {

            return ContainsAny(key, "studentnumber", "employeenumber", "teachernumber", "nationalid", "identitynumber", "quantity", "count", "hours", "minutes", "days", "number", "studentid", "teacherid", "employeeid", "classid", "subjectid", "roomid", "userid", "routeid", "bookid", "voucherid", "receiptno", "expenseid", "contractid", "enrollmentid", "attendanceid", "copies", "publicationyear", "seatnumber", "workhours", "capacity", "late", "earlyleave", "periodno", "gradeorder", "port", "رقم", "هوية", "كمية", "عدد", "ساعات", "دقائق", "ايام", "أيام");
        }

        // تعريف الدالة IsLongTextField لتنفيذ العملية المرتبطة بها.
        private static bool IsLongTextField(string key)
        {

            return ContainsAny(key, "notes", "remark", "description", "address", "ملاحظات", "وصف", "عنوان");
        }

        // تعريف الدالة ContainsAny لتنفيذ العملية المرتبطة بها.
        private static bool ContainsAny(string value, params string[] tokens)
        {
            foreach (string token in tokens)
            {
                if (value.Contains(token))
                    return true;
            }

            return false;
        }

        // تعريف الدالة IsValidPhoneDigitsOnly لتنفيذ العملية المرتبطة بها.
        private static bool IsValidPhoneDigitsOnly(string value)
        {

            return value.Length >= 7 && value.Length <= 20 && value.All(char.IsDigit);
        }

        // تعريف الدالة TryParseDecimal لتنفيذ العملية المرتبطة بها.
        public static bool TryParseDecimal(string value, out decimal number)
        {
            number = 0m;

            string text = (value ?? string.Empty).Trim();
            if (text.Length == 0)
                return false;

            if (decimal.TryParse(
                text,
                System.Globalization.NumberStyles.Number,
                System.Globalization.CultureInfo.CurrentCulture,
                out number))
                return true;

            char[] chars = text.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                char c = chars[i];

                if (c >= '٠' && c <= '٩')
                    chars[i] = (char)('0' + (c - '٠'));
                else if (c >= '۰' && c <= '۹')
                    chars[i] = (char)('0' + (c - '۰'));
                else if (c == '٫')
                    chars[i] = '.';
                else if (c == '٬')
                    chars[i] = ',';
            }

            string normalized = new string(chars);

            if (decimal.TryParse(
                normalized,
                System.Globalization.NumberStyles.Number,
                System.Globalization.CultureInfo.InvariantCulture,
                out number))
                return true;

            string compact = normalized.Replace(",", string.Empty);
            return decimal.TryParse(
                compact,
                System.Globalization.NumberStyles.Number,
                System.Globalization.CultureInfo.InvariantCulture,
                out number);
        }

        // تعريف الدالة IsRequired لتنفيذ العملية المرتبطة بها.
        public static bool IsRequired(string value)
        {

            return !string.IsNullOrWhiteSpace(value);
        }

        // تعريف الدالة IsNumeric لتنفيذ العملية المرتبطة بها.
        public static bool IsNumeric(string value)
        {

            return TryParseDecimal(value, out _);
        }

        // تعريف الدالة IsValidPositiveInteger لتنفيذ العملية المرتبطة بها.
        public static bool IsValidPositiveInteger(string value, out int number)
        {

            return int.TryParse((value ?? string.Empty).Trim(), out number) && number > 0;
        }

        /// <summary>
        /// يتحقق من أرقام المعرفات التي تُخزّن كنص، مثل رقم الهوية الوطنية.
        /// لا يحول القيمة إلى int حتى لا يفشل الرقم الصحيح إذا تجاوز سعة 32-bit.
        /// </summary>
        // تعريف الدالة IsValidNumericIdentifier لتنفيذ العملية المرتبطة بها.
        public static bool IsValidNumericIdentifier(string value, int minimumLength = 6, int maximumLength = 20)
        {
            string text = (value ?? string.Empty).Trim();
            if (text.Length < minimumLength || text.Length > maximumLength)
                return false;


            return text.All(char.IsDigit);
        }

        // تعريف الدالة IsValidNonNegativeDecimal لتنفيذ العملية المرتبطة بها.
        public static bool IsValidNonNegativeDecimal(string value, out decimal amount)
        {

            return TryParseDecimal(value, out amount) && amount >= 0m;
        }

        // تعريف الدالة IsValidArabicOrLatinName لتنفيذ العملية المرتبطة بها.
        public static bool IsValidArabicOrLatinName(string value, int minimumLength = 2)
        {
            string text = (value ?? string.Empty).Trim();
            if (text.Length < minimumLength)
                return false;

            foreach (char character in text)
            {
                if (char.IsLetter(character) || char.IsWhiteSpace(character) || character == '-' || character == '_')
                    continue;

                return false;
            }

            return true;
        }

        // تعريف الدالة IsValidTextOnly لتنفيذ العملية المرتبطة بها.
        public static bool IsValidTextOnly(string value, int minimumLength = 2, int maximumLength = 200)
        {
            string text = (value ?? string.Empty).Trim();
            if (text.Length < minimumLength || text.Length > maximumLength)
                return false;

            foreach (char character in text)
            {
                if (char.IsLetter(character) || char.IsWhiteSpace(character) || character == '-' || character == '_' || character == '.' || character == ',' || character == '(' || character == ')' || character == '/')
                    continue;

                return false;
            }

            return true;
        }

        // تعريف الدالة IsValidPhone لتنفيذ العملية المرتبطة بها.
        public static bool IsValidPhone(string value)
        {
            string text = (value ?? string.Empty).Trim();
            if (text.Length < 7 || text.Length > 20)
                return false;

            int digits = 0;
            foreach (char character in text)
            {
                if (char.IsDigit(character))
                {
                    digits++;
                    continue;
                }
                if (character == '+' || character == '-' || character == ' ' || character == '(' || character == ')')
                    continue;

                return false;
            }

            return digits >= 7;
        }

        // تعريف الدالة IsValidEmail لتنفيذ العملية المرتبطة بها.
        public static bool IsValidEmail(string value)
        {
            string text = (value ?? string.Empty).Trim();
            if (text.Length == 0 || text.Length > 254)
                return false;
            // بدء مسار قد ينتج عنه استثناء ليتم التعامل معه بأمان.
            try
            {
                var address = new System.Net.Mail.MailAddress(text);

                return string.Equals(address.Address, text, StringComparison.OrdinalIgnoreCase)
                    && text.Contains("@") && text.IndexOf('.', text.IndexOf('@') + 1) > 0;
            }

            catch
            {

                return false;
            }
        }

        // تعريف الدالة IsValidAcademicYear لتنفيذ العملية المرتبطة بها.
        public static bool IsValidAcademicYear(string value)
        {
            string text = (value ?? string.Empty).Trim().Replace('/', '-');
            string[] parts = text.Split('-');

            return parts.Length == 2
                && int.TryParse(parts[0].Trim(), out int first)
                && int.TryParse(parts[1].Trim(), out int second)
                && second == first + 1;
        }

        // تعريف الدالة FocusAndWarn لتنفيذ العملية المرتبطة بها.
        public static void FocusAndWarn(Control control, string message)
        {
            if (control != null)
            {
                control.Focus();
                if (control is TextBox textBox)
                    textBox.SelectAll();

                else if (control is ComboBox comboBox)

                    comboBox.DroppedDown = false;
            }
            ShowWarning(message);
        }

        // تعريف الدالة IsDesignMode لتنفيذ العملية المرتبطة بها.
        public static bool IsDesignMode(Control control)
        {

            return control != null && (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime ||
                control.Site != null && control.Site.DesignMode);
        }

        // تعريف الدالة ApplyKryptonTheme لتنفيذ العملية المرتبطة بها.
        public static void ApplyKryptonTheme()
        {
            KryptonThemeManager.GlobalPaletteMode = PaletteMode.Office2010Blue;
            KryptonThemeManager.BaseFont = BodyFont;
        }

        // تعريف الدالة ApplyKryptonTheme لتنفيذ العملية المرتبطة بها.
        public static void ApplyKryptonTheme(Control root)
        {
            ApplyKryptonTheme();
            if (root == null)
                return;

            ApplyKryptonThemeRecursive(root);
        }

        // تعريف الدالة ApplyKryptonThemeRecursive لتنفيذ العملية المرتبطة بها.
        private static void ApplyKryptonThemeRecursive(Control control)
        {
            foreach (Control child in control.Controls)
            {
                // لا نبدّل الخط أو الاتجاه لعناصر عادية؛ المصمم هو مصدر الحقيقة لهما،
                // ونكتفي بمواءمة عناصر Krypton مع الهوية البصرية الموحدة.

                if (child is KryptonButton kryptonButton)
                {
                    if (IsDefaultForeColor(kryptonButton))
                        kryptonButton.ForeColor = Color.White;

                    if (IsDefaultFont(kryptonButton))
                        kryptonButton.Font = new Font(FontFamily, BodyFontSize, FontStyle.Bold);

                    kryptonButton.Cursor = Cursors.Hand;
                }

                else if (child is KryptonTextBox kryptonTextBox)
                {
                    if (IsDefaultForeColor(kryptonTextBox))
                        kryptonTextBox.ForeColor = TextColor;
                }

                else if (child is KryptonComboBox kryptonComboBox)
                {
                    if (IsDefaultForeColor(kryptonComboBox))
                        kryptonComboBox.ForeColor = TextColor;
                }

                else if (child is KryptonLabel kryptonLabel)
                {
                    if (IsDefaultForeColor(kryptonLabel))
                        kryptonLabel.ForeColor = TextColor;
                }

                ApplyKryptonThemeRecursive(child);
            }
        }

        private static bool IsDefaultBackColor(Control control)
        {
            if (control == null)
                return true;

            Color expected = control is TextBox
                ? SystemColors.Window
                : SystemColors.Control;

            return control.BackColor == expected || control.BackColor == Color.Empty;
        }

        private static bool IsDefaultForeColor(Control control)
        {
            if (control == null)
                return true;

            return control.ForeColor == SystemColors.ControlText ||
                   control.ForeColor == SystemColors.WindowText ||
                   control.ForeColor == Color.Empty;
        }

        private static bool IsDefaultFont(Control control)
        {
            if (control == null || control.Font == null)
                return true;

            Font defaultFont = Control.DefaultFont;
            Font current = control.Font;

            return string.Equals(
                       current.FontFamily.Name,
                       defaultFont.FontFamily.Name,
                       StringComparison.OrdinalIgnoreCase) &&
                   Math.Abs(current.SizeInPoints - defaultFont.SizeInPoints) < 0.01F &&
                   current.Style == defaultFont.Style &&
                   current.Unit == defaultFont.Unit;
        }

        private static bool ShouldApplyCentralControlStyle(Control control)
        {
            if (control == null)
                return false;

            string tag = Convert.ToString(control.Tag);
            if (string.Equals(tag, "ui:central", StringComparison.OrdinalIgnoreCase))
                return true;

            return IsDefaultBackColor(control) &&
                   IsDefaultForeColor(control) &&
                   IsDefaultFont(control);
        }

        private static bool ShouldApplyCentralGridStyle(DataGridView grid)
        {
            if (grid == null)
                return false;

            string tag = Convert.ToString(grid.Tag);
            if (string.Equals(tag, "ui:central", StringComparison.OrdinalIgnoreCase))
                return true;

            DataGridViewCellStyle header = grid.ColumnHeadersDefaultCellStyle;
            DataGridViewCellStyle cell = grid.DefaultCellStyle;

            bool hasExplicitAppearance =
                (grid.BackgroundColor != SystemColors.Window &&
                 grid.BackgroundColor != Color.Empty) ||
                (header != null && header.BackColor != Color.Empty) ||
                (cell != null && cell.BackColor != Color.Empty) ||
                !IsDefaultFont(grid);

            return !hasExplicitAppearance;
        }

        // تعريف الدالة ApplyTheme لتنفيذ العملية المرتبطة بها.
        public static void ApplyTheme(Control root)
        {
            if (root == null)
                return;

            if (root is Form form)
            {
                if (IsDefaultBackColor(form))
                    form.BackColor = BackgroundColor;

                if (IsDefaultFont(form))
                    form.Font = BodyFont;

                // RTL is an application invariant for the Arabic UI.
                form.RightToLeft = RightToLeft.Yes;
                form.RightToLeftLayout = true;
            }

            else if (root is UserControl userControl)
            {
                if (IsDefaultBackColor(userControl))
                    userControl.BackColor = BackgroundColor;

                if (IsDefaultFont(userControl))
                    userControl.Font = BodyFont;

                userControl.RightToLeft = RightToLeft.Yes;
            }

            ApplyThemeRecursive(root);
        }

        // تعريف الدالة ApplyThemeRecursive لتنفيذ العملية المرتبطة بها.
        private static void ApplyThemeRecursive(Control control)
        {
            foreach (Control child in control.Controls)
            {
                if (child is Panel panel)
                {
                    if (IsDefaultBackColor(panel))
                        panel.BackColor = SurfaceColor;
                }

                else if (child is GroupBox groupBox)
                {
                    if (IsDefaultBackColor(groupBox))
                        groupBox.BackColor = SurfaceColor;

                    if (IsDefaultForeColor(groupBox))
                        groupBox.ForeColor = TextColor;

                    if (IsDefaultFont(groupBox))
                        groupBox.Font = SectionFont;
                }

                else if (child is Label label)
                {
                    if (IsDefaultBackColor(label))
                        label.BackColor = Color.Transparent;

                    if (IsDefaultForeColor(label))
                        label.ForeColor = TextColor;

                    if (IsDefaultFont(label))
                        label.Font = BodyFont;

                    if (!label.AutoSize)
                        label.AutoEllipsis = true;

                    string labelKey = ((label.Name ?? string.Empty) + " " + (label.Text ?? string.Empty)).ToLowerInvariant();
                    if (labelKey.Contains("title") || labelKey.Contains("header") || labelKey.Contains("عنوان") || labelKey.Contains("رئيسي"))
                    {
                        if (IsDefaultForeColor(label))
                            label.ForeColor = PrimaryColor;

                        if (IsDefaultFont(label))
                            label.Font = TitleFont;
                    }

                    else if (labelKey.Contains("section") || labelKey.Contains("قسم") || labelKey.Contains("بيانات"))
                    {
                        if (IsDefaultForeColor(label))
                            label.ForeColor = MutedTextColor;

                        if (IsDefaultFont(label))
                            label.Font = SectionFont;
                    }
                }

                else if (child is LinkLabel linkLabel)
                {
                    if (linkLabel.LinkColor == SystemColors.ControlText ||
                        linkLabel.LinkColor == Color.Empty)
                        linkLabel.LinkColor = SearchColor;

                    if (linkLabel.ActiveLinkColor == SystemColors.ControlText ||
                        linkLabel.ActiveLinkColor == Color.Empty)
                        linkLabel.ActiveLinkColor = AccentColor;

                    if (IsDefaultFont(linkLabel))
                        linkLabel.Font = new Font(FontFamily, BodyFontSize, FontStyle.Underline);
                }

                else if (child is TextBox textBox)
                {
                    if (ShouldApplyCentralControlStyle(textBox))
                        StyleTextBox(textBox);
                }

                else if (child is ComboBox comboBox)
                {
                    if (ShouldApplyCentralControlStyle(comboBox))
                        StyleComboBox(comboBox);
                }

                else if (child is DateTimePicker dateTimePicker)
                {
                    if (ShouldApplyCentralControlStyle(dateTimePicker))
                    {
                        dateTimePicker.Font = new Font(FontFamily, BodyFontSize);
                        dateTimePicker.CalendarFont = new Font(FontFamily, BodyFontSize);
                        dateTimePicker.CalendarForeColor = TextColor;
                        dateTimePicker.CalendarMonthBackground = SurfaceColor;
                        dateTimePicker.BackColor = SurfaceColor;
                        dateTimePicker.ForeColor = TextColor;
                    }
                }

                else if (child is NumericUpDown numericUpDown)
                {
                    if (ShouldApplyCentralControlStyle(numericUpDown))
                    {
                        numericUpDown.Font = new Font(FontFamily, BodyFontSize);
                        numericUpDown.BackColor = SurfaceColor;
                        numericUpDown.ForeColor = TextColor;
                        numericUpDown.BorderStyle = BorderStyle.FixedSingle;
                    }
                }

                else if (child is CheckBox checkBox)
                {
                    if (ShouldApplyCentralControlStyle(checkBox))
                    {
                        checkBox.Font = new Font(FontFamily, BodyFontSize);
                        checkBox.ForeColor = TextColor;
                    }
                }

                else if (child is RadioButton radioButton)
                {
                    if (ShouldApplyCentralControlStyle(radioButton))
                    {
                        radioButton.Font = new Font(FontFamily, BodyFontSize);
                        radioButton.ForeColor = TextColor;
                    }
                }

                else if (child is DataGridView dataGridView)
                {
                    if (ShouldApplyCentralGridStyle(dataGridView))
                        StyleDataGridView(dataGridView);
                }

                else if (child is Button button)
                {
                    if (ShouldApplyCentralControlStyle(button))
                        StyleActionButton(button);
                }

                else if (child is KryptonButton kryptonButton)
                {
                    if (ShouldApplyCentralControlStyle(kryptonButton))
                        StyleActionButton(kryptonButton);
                }

                else if (child is TabControl tabControl)
                {
                    if (ShouldApplyCentralControlStyle(tabControl))
                    {
                        tabControl.Font = new Font(FontFamily, BodyFontSize);
                        tabControl.BackColor = BackgroundColor;
                    }
                }

                ApplyThemeRecursive(child);
            }
        }

        // تعريف الدالة StyleActionButton لتنفيذ العملية المرتبطة بها.
        public static void StyleActionButton(Button btn)
        {
            if (btn == null)
                return;

            NormalizeButtonCaption(btn);
            string text = (btn.Text ?? string.Empty).Trim();

            if (text.Contains("حذف") || text.Contains("أرشفة"))
            {
                StyleButton(btn, DangerColor);

                return;
            }

            if (text.Contains("حفظ") || text.Contains("إضافة") || text.Contains("جديد"))
            {
                StyleButton(btn, SuccessColor);

                return;
            }

            if (text.Contains("بحث"))
            {
                StyleButton(btn, SearchColor);

                return;
            }

            if (text.Contains("تصدير"))
            {
                StyleButton(btn, ExportColor);

                return;
            }

            if (text.Contains("تحديث") || text.Contains("إلغاء") || text.Contains("إغلاق") || text.Contains("مسح") || text.Contains("إزالة"))
            {
                StyleButton(btn, NeutralColor);

                return;
            }

            if (text.Contains("طباعة") || text.Contains("تعديل"))
            {
                StyleButton(btn, AccentColor);

                return;
            }

            StyleButton(btn, AccentColor);
        }

        // تعريف الدالة StyleActionButton لتنفيذ العملية المرتبطة بها.
        public static void StyleActionButton(KryptonButton btn)
        {
            if (btn == null)
                return;

            NormalizeButtonCaption(btn);
            string text = (btn.Text ?? string.Empty).Trim();
            Color color = AccentColor;
            if (text.Contains("حذف"))
                color = DangerColor;

            else if (text.Contains("حفظ") || text.Contains("إضافة") || text.Contains("جديد"))

                color = SuccessColor;

            else if (text.Contains("بحث"))

                color = SearchColor;

            else if (text.Contains("تصدير"))

                color = ExportColor;

            else if (text.Contains("تحديث") || text.Contains("إلغاء") || text.Contains("إغلاق"))

                color = NeutralColor;

            StyleButton(btn, color);
            EnsureKryptonButtonTextFits(btn);
        }

        /// <summary>
        /// يوحّد الاختصارات الشائعة التي ظهرت في بعض ملفات المصمم إلى أسماء كاملة.
        /// لا يغيّر التسميات الصحيحة أو معنى الزر، ويعتمد على اسم التحكم عند وجود
        /// أكثر من صيغة تصدير حتى تبقى الوظيفة واضحة للمستخدم.
        /// </summary>
        // تعريف الدالة NormalizeButtonCaption لتنفيذ العملية المرتبطة بها.
        private static void NormalizeButtonCaption(Button btn)
        {
            if (btn == null)
                return;

            btn.Text = NormalizeCaption(btn.Name, btn.Text);
            btn.Padding = new Padding(10, 4, 10, 4);
            btn.TextAlign = ContentAlignment.MiddleCenter;
            btn.UseCompatibleTextRendering = true;
        }

        // تعريف الدالة NormalizeButtonCaption لتنفيذ العملية المرتبطة بها.
        private static void NormalizeButtonCaption(KryptonButton btn)
        {
            if (btn == null)
                return;

            btn.Text = NormalizeCaption(btn.Name, btn.Text);
            btn.Padding = new Padding(10, 4, 10, 4);
        }

        // تعريف الدالة NormalizeCaption لتنفيذ العملية المرتبطة بها.
        private static string NormalizeCaption(string controlName, string caption)
        {
            string name = (controlName ?? string.Empty).ToLowerInvariant();
            string text = (caption ?? string.Empty).Trim();

            if (text == "جد" || text == "بح")
                text = "بحث";

            else if (text == "تصد" || text == "تصدير فقط")

                text = "تصدير";

            else if (text == "تح")

                text = "تحديث";

            else if (text == "حف")

                text = "حفظ";

            else if (text == "حذ")

                text = "حذف";

            else if (text == "تعد")

                text = "تعديل";

            else if (text == "طب")

                text = "طباعة";

            else if (text == "إض")

                text = "إضافة";

            else if (text == "جدي")

                text = "جديد";

            // التسميات العامة الواضحة تمنع ظهور اختصارات أو أزرار بلا وصف كافٍ.

            if (name == "btnsearch" && text == "بحث")
                text = "بحث في البيانات";

            else if ((name == "btnrefresh" || name == "btnreload") && text == "تحديث")

                text = "تحديث البيانات";

            else if (name == "btnadd" && (text == "إضافة" || text == "جديد"))

                text = "إضافة سجل جديد";

            else if (name == "btnupdate" && text == "تعديل")

                text = "تعديل السجل";

            else if (name == "btndelete" && text == "حذف")

                text = "حذف السجل";

            else if (name == "btnclear" && (text == "مسح" || text == "تفريغ"))

                text = "مسح الحقول";

            else if (name == "btnprint" && text == "طباعة")

                text = "طباعة التقرير";

            else if (name == "btnclose" && text == "إغلاق")

                text = "إغلاق النافذة";

            else if (name == "btncancel" && text == "إلغاء")

                text = "إلغاء العملية";

            else if (name == "btnexport" && text == "تصدير")

                text = "تصدير البيانات";

            if (name.Contains("exportexcel") && (text == "تصدير Excel" || text == "تصدير"))
                return "تصدير إلى Excel";
            if (name.Contains("exportpdf") && (text == "تصدير PDF" || text == "تصدير"))
                return "تصدير إلى PDF";
            if (name.Contains("exportcsv") && (text == "تصدير CSV" || text == "تصدير"))
                return "تصدير إلى CSV";


            return text;
        }

        // تعريف الدالة StyleDataGridView لتنفيذ العملية المرتبطة بها.
        public static void StyleDataGridView(DataGridView dgv)
        {
            if (dgv == null)
                return;

            // إذا كان الـDesigner قد حدد مظهر الجدول صراحةً، نحافظ عليه.
            // العناصر الديناميكية فقط تستفيد من الهوية الافتراضية هنا.
            if (ShouldApplyCentralGridStyle(dgv))
            {
                dgv.BackgroundColor = SurfaceElevatedColor;
                dgv.BorderStyle = BorderStyle.None;
                dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                dgv.RightToLeft = RightToLeft.Yes;
                dgv.RowHeadersVisible = false;
                dgv.MultiSelect = false;
                dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                dgv.AllowUserToResizeRows = false;

                dgv.EnableHeadersVisualStyles = false;
                dgv.ColumnHeadersDefaultCellStyle.BackColor = PrimaryColor;
                dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgv.ColumnHeadersDefaultCellStyle.Font = new Font(FontFamily, BodyFontSize, FontStyle.Bold);
                dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgv.ColumnHeadersHeight = 42;

                dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(219, 234, 254);
                dgv.DefaultCellStyle.SelectionForeColor = TextColor;
                dgv.DefaultCellStyle.ForeColor = TextColor;
                dgv.DefaultCellStyle.Font = GridFont;
                dgv.DefaultCellStyle.Padding = new Padding(6, 3, 6, 3);
                dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

                dgv.AlternatingRowsDefaultCellStyle.BackColor = AlternateRowColor;
                dgv.RowsDefaultCellStyle.BackColor = SurfaceColor;
                dgv.RowsDefaultCellStyle.ForeColor = TextColor;
                dgv.RowTemplate.Height = 36;
                dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgv.AllowUserToAddRows = false;
                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgv.GridColor = BorderColor;
                dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(6, 4, 6, 4);
            }

            dgv.CellFormatting -= DataGridView_DualDateFormatting;
            dgv.CellFormatting += DataGridView_DualDateFormatting;
        }

        private static void DataGridView_DualDateFormatting(
            object sender,
            DataGridViewCellFormattingEventArgs e)
        {
            DataGridView grid = sender as DataGridView;
            if (grid == null || e.RowIndex < 0 || e.ColumnIndex < 0 || e.Value == null)
                return;

            DataGridViewColumn column = grid.Columns[e.ColumnIndex];
            string key =
                ((column.Name ?? string.Empty) + " " +
                 (column.DataPropertyName ?? string.Empty) + " " +
                 (column.HeaderText ?? string.Empty))
                .ToLowerInvariant();

            if (ContainsAny(
                key,
                "start",
                "end",
                "departure",
                "arrival",
                "checkin",
                "checkout",
                "time",
                "وقت",
                "بداية",
                "نهاية"))
                return;

            DateTime date;

            if (e.Value is DateTime)
                date = ((DateTime)e.Value).Date;
            else
                return;

            e.Value =
                date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) +
                " م" +
                Environment.NewLine +
                GetHijriDateText(date);

            e.CellStyle.WrapMode = DataGridViewTriState.True;
            e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            if (grid.Rows[e.RowIndex].Height < 48)
                grid.Rows[e.RowIndex].Height = 48;

            e.FormattingApplied = true;
        }

        // تعريف الدالة StyleButton لتنفيذ العملية المرتبطة بها.
        public static void StyleButton(Button btn, Color backColor)
        {
            if (btn == null)
                return;

            if (ShouldApplyCentralControlStyle(btn))
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.BackColor = backColor;
                btn.ForeColor = Color.White;
                btn.FlatAppearance.BorderSize = 0;
                btn.FlatAppearance.MouseOverBackColor = Lighten(backColor, 0.10f);
                btn.FlatAppearance.MouseDownBackColor = Darken(backColor, 0.10f);
                btn.Cursor = Cursors.Hand;
                btn.Font = HeadingFont;
            }

            else
            {
                // الزر المصمم صراحةً يحتفظ بحجمه؛ لا نعيد قياسه أثناء التشغيل.
                btn.Cursor = Cursors.Hand;
            }

            if (ShouldApplyCentralControlStyle(btn))
                EnsureButtonTextFits(btn);
        }

        // تعريف الدالة EnsureButtonTextFits لتنفيذ العملية المرتبطة بها.
        private static void EnsureButtonTextFits(Button btn)
        {
            if (btn == null)
                return;

            if (btn.AutoSize)
                btn.AutoSize = false;

            Size measured = TextRenderer.MeasureText(btn.Text ?? string.Empty, btn.Font);
            int requiredWidth = Math.Max(92, measured.Width + btn.Padding.Horizontal + 12);
            int requiredHeight = Math.Max(36, measured.Height + btn.Padding.Vertical + 8);

            if (btn.Width < requiredWidth)
                btn.Width = requiredWidth;
            if (btn.Height < requiredHeight)
                btn.Height = requiredHeight;
        }

        // تعريف الدالة StyleButton لتنفيذ العملية المرتبطة بها.
        public static void StyleButton(KryptonButton btn, Color backColor)
        {
            if (btn == null)
                return;

            NormalizeButtonCaption(btn);
            btn.BackColor = backColor;
            btn.ForeColor = Color.White;
            btn.Cursor = Cursors.Hand;
            btn.Font = HeadingFont;
            EnsureKryptonButtonTextFits(btn);
        }

        // تعريف الدالة EnsureKryptonButtonTextFits لتنفيذ العملية المرتبطة بها.
        private static void EnsureKryptonButtonTextFits(KryptonButton btn)
        {
            if (btn == null)
                return;

            if (btn.AutoSize)
                btn.AutoSize = false;

            Size measured = TextRenderer.MeasureText(btn.Text ?? string.Empty, btn.Font);
            int requiredWidth = Math.Max(92, measured.Width + btn.Padding.Horizontal + 12);
            int requiredHeight = Math.Max(36, measured.Height + btn.Padding.Vertical + 8);

            if (btn.Width < requiredWidth)
                btn.Width = requiredWidth;
            if (btn.Height < requiredHeight)
                btn.Height = requiredHeight;
        }

        // تعريف الدالة Lighten لتنفيذ العملية المرتبطة بها.
        private static Color Lighten(Color color, float amount)
        {
            int r = color.R + (int)((255 - color.R) * amount);
            int g = color.G + (int)((255 - color.G) * amount);
            int b = color.B + (int)((255 - color.B) * amount);

            return Color.FromArgb(r, g, b);
        }

        // تعريف الدالة Darken لتنفيذ العملية المرتبطة بها.
        private static Color Darken(Color color, float amount)
        {
            int r = color.R - (int)(color.R * amount);
            int g = color.G - (int)(color.G * amount);
            int b = color.B - (int)(color.B * amount);

            return Color.FromArgb(r, g, b);
        }

        // تعريف الدالة StylePrimaryButton لتنفيذ العملية المرتبطة بها.
        public static void StylePrimaryButton(Button btn) => StyleButton(btn, AccentColor);
        // تعريف الدالة StylePrimaryButton لتنفيذ العملية المرتبطة بها.
        public static void StylePrimaryButton(KryptonButton btn) => StyleButton(btn, AccentColor);
        // تعريف الدالة StyleDangerButton لتنفيذ العملية المرتبطة بها.
        public static void StyleDangerButton(Button btn) => StyleButton(btn, DangerColor);
        // تعريف الدالة StyleSuccessButton لتنفيذ العملية المرتبطة بها.
        public static void StyleSuccessButton(Button btn) => StyleButton(btn, SuccessColor);

        /// <summary>
        /// يقرأ المعرّف المحدد من ComboBox المرتبط ببيانات بأمان؛ يعيد 0 إذا كان
        /// التحديد فارغاً أو غير جاهز (DataRowView أثناء الربط) بدل رمي InvalidCastException.
        /// </summary>
        public static int GetSelectedId(ComboBox combo)
        {
            if (combo == null || combo.SelectedValue == null)
                return 0;

            object value = combo.SelectedValue;

            if (value is DataRowView)
                return 0;

            return int.TryParse(Convert.ToString(value), out int id) ? id : 0;
        }

        // تعريف الدالة EscapeDataViewFilterValue لتنفيذ العملية المرتبطة بها.
        public static string EscapeDataViewFilterValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;


            return value.Trim()
                .Replace("'", "''")
                .Replace("[", "[[]")
                .Replace("%", "[%]")
                .Replace("*", "[*]");
        }

        /// <summary>
        /// يبني شرط DataView يدعم البحث الفوري متعدد الكلمات؛ كل كلمة يجب أن تظهر
        /// في واحد من الأعمدة القابلة للبحث، مع حماية قيمة البحث من صيغة RowFilter.
        /// </summary>
        // تعريف الدالة BuildDataViewSearchFilter لتنفيذ العملية المرتبطة بها.
        public static string BuildDataViewSearchFilter(string searchText, params string[] columns)
        {
            if (string.IsNullOrWhiteSpace(searchText) || columns == null || columns.Length == 0)
                return string.Empty;

            string[] terms = searchText.Trim()
                .Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
            var termFilters = new System.Collections.Generic.List<string>();

            foreach (string term in terms)
            {
                string safeTerm = EscapeDataViewFilterValue(term);
                var columnFilters = new System.Collections.Generic.List<string>();
                foreach (string column in columns)
                {
                    if (!string.IsNullOrWhiteSpace(column))
                        columnFilters.Add(column + " LIKE '%" + safeTerm + "%'");
                }

                if (columnFilters.Count > 0)
                    termFilters.Add("(" + string.Join(" OR ", columnFilters) + ")");
            }


            return string.Join(" AND ", termFilters);
        }

        // تعريف الدالة StyleTextBox لتنفيذ العملية المرتبطة بها.
        public static void StyleTextBox(TextBox txt)
        {
            if (txt == null)
                return;

            if (ShouldApplyCentralControlStyle(txt))
            {
                txt.BorderStyle = BorderStyle.FixedSingle;
                txt.BackColor = SurfaceColor;
                txt.ForeColor = TextColor;
                txt.Font = BodyFont;
                txt.Margin = new Padding(3, 4, 3, 4);
            }
        }

        // تعريف الدالة StyleTextBox لتنفيذ العملية المرتبطة بها.
        public static void StyleTextBox(KryptonTextBox txt)
        {
            if (txt == null)
                return;

            txt.Font = BodyFont;
            txt.ForeColor = TextColor;
            txt.BackColor = Color.White;
        }

        // تعريف الدالة StyleComboBox لتنفيذ العملية المرتبطة بها.
        public static void StyleComboBox(ComboBox cmb)
        {
            if (cmb == null)
                return;

            if (ShouldApplyCentralControlStyle(cmb))
            {
                if (cmb.DropDownStyle != ComboBoxStyle.DropDownList &&
                    !ContainsAny((cmb.Name ?? string.Empty).ToLowerInvariant(), "search", "بحث", "filter", "تصفية"))
                {
                    cmb.DropDownStyle = ComboBoxStyle.DropDownList;
                }

                cmb.BackColor = SurfaceColor;
                cmb.ForeColor = TextColor;
                cmb.Font = BodyFont;
                cmb.Margin = new Padding(3, 4, 3, 4);
            }
        }

        // تعريف الدالة ShowException لتنفيذ العملية المرتبطة بها.
        public static void ShowException(string operation, Exception exception)
        {
            string safeOperation = string.IsNullOrWhiteSpace(operation) ? "العملية" : operation.Trim();
            ApplicationLogger.LogException(safeOperation, exception);

            // أخطاء قواعد العمل (من قاعدة البيانات أو من تحققات الخدمات) تُعرض كرسالة واضحة بدلاً من نافذة خطأ تقنية.
            if (TryGetBusinessMessage(exception, out string businessMessage))
            {
                ShowWarning(businessMessage);

                return;
            }

            string detail = GetSafeExceptionDetail(exception);
            string message = "تعذر إتمام " + safeOperation + ".";
            if (!string.IsNullOrWhiteSpace(detail))
                message += Environment.NewLine + Environment.NewLine + "السبب: " + detail;

            message += Environment.NewLine + Environment.NewLine + "راجع ملف السجلات (Logs\\application-errors.log في مجلد المشروع) لمزيد من التفاصيل.";
            ShowError(message);
        }

        // تعريف الدالة TryGetBusinessMessage لتنفيذ العملية المرتبطة بها.
        private static bool TryGetBusinessMessage(Exception exception, out string message)
        {
            Exception current = exception;

            while (current != null)
            {
                SqlException sqlException = current as SqlException;
                if (sqlException != null)
                {
                    if (sqlException.Number >= 50000 && sqlException.Number <= 51999)
                    {
                        message = string.IsNullOrWhiteSpace(sqlException.Message)
                            ? "رفضت قاعدة البيانات العملية لتعارضها مع بيانات موجودة."
                            : sqlException.Message.Trim();

                        return true;
                    }

                    if (sqlException.Number == 2601 || sqlException.Number == 2627)
                    {
                        message = "البيانات موجودة مسبقاً ولا يمكن تكرارها.";

                        return true;
                    }
                }

                if (current is ArgumentException || current is InvalidOperationException || current is UnauthorizedAccessException)
                {
                    string text = (current.Message ?? string.Empty).Trim();
                    // رسائل قواعد العمل في التطبيق بالعربية؛ تُتجاهل رسائل النظام التقنية بالإنجليزية.
                    if (ContainsArabicText(text))
                    {
                        message = text;

                        return true;
                    }
                }

                current = current.InnerException;
            }

            message = string.Empty;

            return false;
        }

        // تعريف الدالة ContainsArabicText لتنفيذ العملية المرتبطة بها.
        private static bool ContainsArabicText(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            foreach (char c in value)
            {
                if ((c >= '\u0600' && c <= '\u06FF') ||
                    (c >= '\u0750' && c <= '\u077F') ||
                    (c >= '\u08A0' && c <= '\u08FF'))
                {
                    return true;
                }
            }

            return false;
        }

        // تعريف الدالة GetSafeExceptionDetail لتنفيذ العملية المرتبطة بها.
        private static string GetSafeExceptionDetail(Exception exception)
        {
            Exception current = exception;

            while (current != null)
            {
                SqlException sqlException = current as SqlException;
                if (sqlException != null)
                {
                    if (sqlException.Number == 208)
                        return "الجدول المطلوب غير موجود في قاعدة البيانات. شغّل ملف Migration المناسب.";
                    if (sqlException.Number == 207)
                        return "أحد الأعمدة المطلوبة غير موجود أو اسمه مختلف في قاعدة البيانات. شغّل آخر Migration.";
                    if (sqlException.Number == 229)
                        return "لا تملك صلاحية تنفيذ هذه العملية. اطلب من مدير النظام منح الصلاحية المناسبة.";
                    if (sqlException.Number == 2601 || sqlException.Number == 2627)
                        return "البيانات موجودة مسبقاً ولا يمكن تكرارها.";
                    if (sqlException.Number == 547)
                        return "لا يمكن تنفيذ العملية لأن السجل مرتبط ببيانات أخرى. عالج الارتباط أو عطّل السجل بدلاً من حذفه.";
                    if (sqlException.Number == 18456 || sqlException.Number == 53 || sqlException.Number == -1)
                        return "تعذر الاتصال بخادم SQL Server أو رفضت بيانات الاعتماد الاتصال.";
                    if (sqlException.Number >= 51000 && sqlException.Number <= 51999)
                        return string.IsNullOrWhiteSpace(sqlException.Message)
                            ? "رفضت قاعدة البيانات العملية لحماية ترابط البيانات."
                            : sqlException.Message;

                    return "رفض SQL Server الاستعلام (رقم الخطأ " + sqlException.Number + ").";
                }

                current = current.InnerException;
            }

            string message = exception == null ? string.Empty : exception.Message;
            if (string.IsNullOrWhiteSpace(message))
                return string.Empty;
            if (message.Length > 240)
                message = message.Substring(0, 240) + "...";

            return message;
        }

        // تعريف الدالة ShowError لتنفيذ العملية المرتبطة بها.
        public static void ShowError(string message)
        {

            MessageBox.Show(message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error,
                MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
        }

        // تعريف الدالة ShowInfo لتنفيذ العملية المرتبطة بها.
        public static void ShowInfo(string message)
        {

            MessageBox.Show(message, "معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
        }

        // تعريف الدالة ShowInformation لتنفيذ العملية المرتبطة بها.
        public static void ShowInformation(string message)
        {

            MessageBox.Show(message, "تمت العملية", MessageBoxButtons.OK, MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
        }

        // تعريف الدالة ShowWarning لتنفيذ العملية المرتبطة بها.
        public static void ShowWarning(string message)
        {

            MessageBox.Show(message, "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
        }

        // تعريف الدالة ShowConfirmation لتنفيذ العملية المرتبطة بها.
        public static bool ShowConfirmation(string message, string title = "تأكيد")
        {

            return MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question,

                MessageBoxDefaultButton.Button2, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading) == DialogResult.Yes;
        }
    }
}
