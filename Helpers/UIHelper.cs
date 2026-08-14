using System;
using System.Drawing;
using System.Data.SqlClient;
using System.Windows.Forms;
using Krypton.Toolkit;

namespace SchoolSystem.Helpers
{
    public static class UIHelper
    {
        private static readonly KryptonManager KryptonThemeManager = new KryptonManager();

        // Global palette: every form must use these tokens instead of local colors.
        public static readonly Color PrimaryColor = Color.FromArgb(30, 41, 59);
        public static readonly Color PrimaryDarkColor = Color.FromArgb(15, 23, 42);
        public static readonly Color PrimaryLightColor = Color.FromArgb(226, 232, 240);
        public static readonly Color SecondaryColor = Color.FromArgb(71, 85, 105);
        public static readonly Color AccentColor = Color.FromArgb(15, 118, 110);
        public static readonly Color SuccessColor = Color.FromArgb(22, 163, 74);
        public static readonly Color WarningColor = Color.FromArgb(217, 119, 6);
        public static readonly Color DangerColor = Color.FromArgb(198, 40, 40);
        public static readonly Color InfoColor = Color.FromArgb(37, 99, 235);
        public static readonly Color NeutralColor = Color.FromArgb(71, 85, 105);
        public static readonly Color ExportColor = Color.FromArgb(13, 148, 136);
        public static readonly Color SearchColor = InfoColor;
        public static readonly Color BackgroundColor = Color.FromArgb(248, 250, 252);
        public static readonly Color SurfaceColor = Color.White;
        public static readonly Color SurfaceSecondaryColor = Color.FromArgb(241, 245, 249);
        public static readonly Color SurfaceElevatedColor = Color.White;
        public static readonly Color DisabledSurfaceColor = Color.FromArgb(243, 244, 246);
        public static readonly Color AlternateRowColor = BackgroundColor;
        public static readonly Color BorderColor = Color.FromArgb(203, 213, 225);
        public static readonly Color DividerColor = BorderColor;
        public static readonly Color MutedTextColor = Color.FromArgb(71, 85, 105);
        public static readonly Color TextDisabledColor = Color.FromArgb(148, 163, 184);
        public static readonly Color TextColor = Color.FromArgb(15, 23, 42);
        public static readonly Color TextPrimaryColor = TextColor;
        public static readonly Color TextSecondaryColor = SecondaryColor;
        public static readonly Color HoverColor = Color.FromArgb(226, 232, 240);
        public static readonly Color PressedColor = Color.FromArgb(203, 213, 225);
        public static readonly Color DisabledColor = Color.FromArgb(148, 163, 184);
        public static readonly Color FocusColor = InfoColor;

        // Typography and spacing tokens for consistent Arabic desktop UX.
        public const string FontFamily = "Tahoma";
        public const float TitleFontSize = 16F;
        public const float HeadingFontSize = 14F;
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

        // Shared fonts keep typography centralized and reduce repeated GDI allocations.
        private static readonly Font BodyFont = new Font(FontFamily, BodyFontSize);
        private static readonly Font SectionFont = new Font(FontFamily, SectionFontSize, FontStyle.Bold);
        private static readonly Font HeadingFont = new Font(FontFamily, HeadingFontSize, FontStyle.Bold);
        private static readonly Font TitleFont = new Font(FontFamily, TitleFontSize, FontStyle.Bold);
        private static readonly Font GridFont = new Font(FontFamily, BodyFontSize);

        public static void ApplyStyle(Form form)
        {
            ApplyKryptonTheme();
            ApplyTheme(form);
            ApplyKryptonTheme((Control)form);
            ApplyResponsiveLayout(form);
        }

        public static void ApplyStyle(UserControl uc)
        {
            ApplyKryptonTheme();
            ApplyTheme(uc);
            ApplyKryptonTheme((Control)uc);
            ApplyResponsiveLayout(uc);
        }

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
                form.AutoScroll = true;
                form.Padding = new Padding(Space16);
            }
            else if (root is UserControl userControl)
            {
                userControl.AutoScroll = true;
                userControl.Padding = new Padding(Space16);
                userControl.RightToLeft = RightToLeft.Yes;
            }

            ApplyResponsiveLayoutRecursive(root);
        }

        private static void ApplyResponsiveLayoutRecursive(Control control)
        {
            foreach (Control child in control.Controls)
            {
                child.RightToLeft = RightToLeft.Yes;
                child.Margin = new Padding(Space4);

                if (child is DataGridView grid)
                {
                    grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
                    grid.ScrollBars = ScrollBars.Both;
                    grid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                    grid.MinimumSize = new Size(320, 180);
                }
                else if (child is TableLayoutPanel layout)
                {
                    layout.AutoSize = false;
                    layout.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                    layout.Padding = new Padding(Space8);
                }
                else if (child is FlowLayoutPanel flow)
                {
                    flow.WrapContents = true;
                    flow.AutoScroll = true;
                    flow.Padding = new Padding(Space8);
                }
                else if (child is Button button)
                {
                    button.AutoSize = false;
                    button.MinimumSize = new Size(96, 38);
                }
                else if (child is TextBox textBox)
                {
                    textBox.MinimumSize = new Size(120, 34);
                }
                else if (child is ComboBox comboBox)
                {
                    comboBox.MinimumSize = new Size(120, 34);
                }

                ApplyResponsiveLayoutRecursive(child);
            }
        }

        public static bool TryParseDecimal(string value, out decimal number)
        {
            return decimal.TryParse((value ?? string.Empty).Trim(),
                System.Globalization.NumberStyles.Number,
                System.Globalization.CultureInfo.CurrentCulture,
                out number);
        }

        public static bool IsRequired(string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        public static bool IsNumeric(string value)
        {
            decimal ignored;
            return TryParseDecimal(value, out ignored);
        }

        public static void ApplyKryptonTheme()
        {
            KryptonThemeManager.GlobalPaletteMode = PaletteMode.Office2010Blue;
            KryptonThemeManager.BaseFont = BodyFont;
        }

        public static void ApplyKryptonTheme(Control root)
        {
            ApplyKryptonTheme();
            if (root == null)
                return;

            ApplyKryptonThemeRecursive(root);
        }

        private static void ApplyKryptonThemeRecursive(Control control)
        {
            foreach (Control child in control.Controls)
            {
                child.RightToLeft = RightToLeft.Yes;
                child.Font = new Font(FontFamily, BodyFontSize);

                if (child is KryptonButton kryptonButton)
                {
                    kryptonButton.ForeColor = Color.White;
                    kryptonButton.Font = new Font(FontFamily, BodyFontSize, FontStyle.Bold);
                    kryptonButton.MinimumSize = new Size(90, 38);
                    kryptonButton.Cursor = Cursors.Hand;
                }
                else if (child is KryptonTextBox kryptonTextBox)
                {
                    kryptonTextBox.ForeColor = TextColor;
                    kryptonTextBox.Font = new Font(FontFamily, BodyFontSize);
                    kryptonTextBox.MinimumSize = new Size(120, 34);
                }
                else if (child is KryptonComboBox kryptonComboBox)
                {
                    kryptonComboBox.ForeColor = TextColor;
                    kryptonComboBox.Font = new Font(FontFamily, BodyFontSize);
                    kryptonComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                    kryptonComboBox.MinimumSize = new Size(120, 34);
                }
                else if (child is KryptonLabel kryptonLabel)
                {
                    kryptonLabel.ForeColor = TextColor;
                    kryptonLabel.Font = new Font(FontFamily, BodyFontSize);
                }

                ApplyKryptonThemeRecursive(child);
            }
        }

        public static void ApplyTheme(Control root)
        {
            if (root == null)
                return;

            if (root is Form form)
            {
                form.BackColor = BackgroundColor;
                form.Font = new Font(FontFamily, BodyFontSize);
                form.RightToLeft = RightToLeft.Yes;
                form.RightToLeftLayout = true;
            }
            else if (root is UserControl userControl)
            {
                userControl.BackColor = BackgroundColor;
                userControl.Font = new Font(FontFamily, BodyFontSize);
                userControl.RightToLeft = RightToLeft.Yes;
            }

            ApplyThemeRecursive(root);
        }

        private static void ApplyThemeRecursive(Control control)
        {
            foreach (Control child in control.Controls)
            {
                if (child is Panel panel)
                {
                    panel.BackColor = SurfaceColor;
                }
                else if (child is GroupBox groupBox)
                {
                    groupBox.BackColor = SurfaceColor;
                    groupBox.ForeColor = TextColor;
                    groupBox.Font = SectionFont;
                    groupBox.Padding = new Padding(Space12, Space16, Space12, Space12);
                }
                else if (child is Label label)
                {
                    label.ForeColor = TextColor;
                    label.Margin = new Padding(3, 5, 3, 5);
                    if (!label.AutoSize)
                        label.AutoEllipsis = true;

                    string labelKey = ((label.Name ?? string.Empty) + " " + (label.Text ?? string.Empty)).ToLowerInvariant();
                    if (labelKey.Contains("title") || labelKey.Contains("header") || labelKey.Contains("عنوان") || labelKey.Contains("رئيسي"))
                    {
                        label.ForeColor = PrimaryColor;
                        label.Font = TitleFont;
                        label.Margin = new Padding(3, 8, 3, 8);
                    }
                    else if (labelKey.Contains("section") || labelKey.Contains("قسم") || labelKey.Contains("بيانات"))
                    {
                        label.ForeColor = MutedTextColor;
                        label.Font = SectionFont;
                    }
                }
                else if (child is LinkLabel linkLabel)
                {
                    linkLabel.LinkColor = SearchColor;
                    linkLabel.ActiveLinkColor = AccentColor;
                    linkLabel.Font = new Font(FontFamily, BodyFontSize, FontStyle.Underline);
                }
                else if (child is TextBox textBox)
                {
                    StyleTextBox(textBox);
                    textBox.BackColor = Color.White;
                }
                else if (child is ComboBox comboBox)
                {
                    StyleComboBox(comboBox);
                    comboBox.BackColor = Color.White;
                }
                else if (child is DateTimePicker dateTimePicker)
                {
                    dateTimePicker.Font = new Font(FontFamily, BodyFontSize);
                    dateTimePicker.CalendarFont = new Font(FontFamily, BodyFontSize);
                    dateTimePicker.CalendarForeColor = TextColor;
                    dateTimePicker.CalendarMonthBackground = SurfaceColor;
                    dateTimePicker.BackColor = SurfaceColor;
                    dateTimePicker.ForeColor = TextColor;
                    dateTimePicker.Margin = new Padding(3, 4, 3, 4);
                }
                else if (child is NumericUpDown numericUpDown)
                {
                    numericUpDown.Font = new Font(FontFamily, BodyFontSize);
                    numericUpDown.BackColor = SurfaceColor;
                    numericUpDown.ForeColor = TextColor;
                    numericUpDown.BorderStyle = BorderStyle.FixedSingle;
                }
                else if (child is CheckBox checkBox)
                {
                    checkBox.Font = new Font(FontFamily, BodyFontSize);
                    checkBox.ForeColor = TextColor;
                }
                else if (child is RadioButton radioButton)
                {
                    radioButton.Font = new Font(FontFamily, BodyFontSize);
                    radioButton.ForeColor = TextColor;
                }
                else if (child is DataGridView dataGridView)
                {
                    StyleDataGridView(dataGridView);
                }
                else if (child is Button button)
                {
                    StyleActionButton(button);
                }
                else if (child is TabControl tabControl)
                {
                    tabControl.Font = new Font(FontFamily, BodyFontSize);
                    tabControl.BackColor = BackgroundColor;
                }

                ApplyThemeRecursive(child);
            }
        }

        public static void StyleActionButton(Button btn)
        {
            if (btn == null)
                return;

            string text = (btn.Text ?? string.Empty).Trim();

            if (text.Contains("حذف"))
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

            if (text.Contains("تحديث") || text.Contains("إلغاء") || text.Contains("إغلاق"))
            {
                StyleButton(btn, NeutralColor);
                return;
            }

            if (text.Contains("طباعة"))
            {
                StyleButton(btn, AccentColor);
                return;
            }

            if (text.Contains("تعديل"))
            {
                StyleButton(btn, AccentColor);
                return;
            }

            StyleButton(btn, AccentColor);
        }

        public static void StyleDataGridView(DataGridView dgv)
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

        public static void StyleButton(Button btn, Color backColor)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.BackColor = backColor;
            btn.ForeColor = Color.White;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Lighten(backColor, 0.10f);
            btn.FlatAppearance.MouseDownBackColor = Darken(backColor, 0.10f);
            btn.Cursor = Cursors.Hand;
            btn.Font = HeadingFont;
            btn.Height = 38;
            btn.MinimumSize = new Size(96, 38);
            btn.AutoSize = false;
            btn.Padding = new Padding(10, 0, 10, 0);
        }

        public static void StyleButton(KryptonButton btn, Color backColor)
        {
            if (btn == null)
                return;

            btn.BackColor = backColor;
            btn.ForeColor = Color.White;
            btn.Cursor = Cursors.Hand;
            btn.Font = HeadingFont;
            btn.Height = 38;
            btn.MinimumSize = new Size(90, 38);
        }

        private static Color Lighten(Color color, float amount)
        {
            int r = color.R + (int)((255 - color.R) * amount);
            int g = color.G + (int)((255 - color.G) * amount);
            int b = color.B + (int)((255 - color.B) * amount);
            return Color.FromArgb(r, g, b);
        }

        private static Color Darken(Color color, float amount)
        {
            int r = color.R - (int)(color.R * amount);
            int g = color.G - (int)(color.G * amount);
            int b = color.B - (int)(color.B * amount);
            return Color.FromArgb(r, g, b);
        }

        public static void StylePrimaryButton(Button btn) => StyleButton(btn, AccentColor);
        public static void StylePrimaryButton(KryptonButton btn) => StyleButton(btn, AccentColor);
        public static void StyleDangerButton(Button btn) => StyleButton(btn, DangerColor);
        public static void StyleSuccessButton(Button btn) => StyleButton(btn, SuccessColor);

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

        public static void StyleTextBox(TextBox txt)
        {
            txt.BorderStyle = BorderStyle.FixedSingle;
            txt.BackColor = SurfaceColor;
            txt.ForeColor = TextColor;
            txt.Font = BodyFont;
            txt.Margin = new Padding(3, 4, 3, 4);
        }

        public static void StyleTextBox(KryptonTextBox txt)
        {
            if (txt == null)
                return;

            txt.Font = BodyFont;
            txt.ForeColor = TextColor;
            txt.BackColor = Color.White;
        }

        public static void StyleComboBox(ComboBox cmb)
        {
            cmb.DropDownStyle = ComboBoxStyle.DropDownList;
            cmb.BackColor = SurfaceColor;
            cmb.ForeColor = TextColor;
            cmb.Font = BodyFont;
            cmb.Margin = new Padding(3, 4, 3, 4);
        }

        public static void ShowException(string operation, Exception exception)
        {
            string safeOperation = string.IsNullOrWhiteSpace(operation) ? "العملية" : operation.Trim();
            ApplicationLogger.LogException(safeOperation, exception, "errors.log");

            string detail = GetSafeExceptionDetail(exception);
            string message = "تعذر إتمام " + safeOperation + ".";
            if (!string.IsNullOrWhiteSpace(detail))
                message += Environment.NewLine + Environment.NewLine + "السبب: " + detail;
            message += Environment.NewLine + Environment.NewLine + "راجع ملف errors.log لمزيد من التفاصيل.";
            ShowError(message);
        }

        private static string GetSafeExceptionDetail(Exception exception)
        {
            SqlException sqlException = exception as SqlException;
            if (sqlException != null)
            {
                if (sqlException.Number == 208)
                    return "الجدول المطلوب غير موجود في قاعدة البيانات. شغّل ملف Migration المناسب.";
                if (sqlException.Number == 207)
                    return "أحد الأعمدة المطلوبة غير موجود أو اسمه مختلف في قاعدة البيانات. شغّل آخر Migration.";
                if (sqlException.Number == 18456 || sqlException.Number == 53 || sqlException.Number == -1)
                    return "تعذر الاتصال بخادم SQL Server أو رفضت بيانات الاعتماد الاتصال.";
                return "رفض SQL Server الاستعلام (رقم الخطأ " + sqlException.Number + ").";
            }

            string message = exception == null ? string.Empty : exception.Message;
            if (string.IsNullOrWhiteSpace(message))
                return string.Empty;
            if (message.Length > 240)
                message = message.Substring(0, 240) + "...";
            return message;
        }

        public static void ShowError(string message)
        {
            MessageBox.Show(message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error, 
                MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
        }

        public static void ShowInfo(string message)
        {
            MessageBox.Show(message, "معلومات", MessageBoxButtons.OK, MessageBoxIcon.Information, 
                MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
        }

        public static void ShowWarning(string message)
        {
            MessageBox.Show(message, "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning, 
                MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
        }

        public static bool ShowConfirmation(string message, string title = "تأكيد")
        {
            return MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading) == DialogResult.Yes;
        }
    }
}
