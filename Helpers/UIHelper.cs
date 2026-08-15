using System;
using System.Drawing;
using System.Linq;
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
            ApplyInputValidation(form);
            ApplyResponsiveLayout(form);
        }

        public static void ApplyStyle(UserControl uc)
        {
            ApplyKryptonTheme();
            ApplyTheme(uc);
            ApplyKryptonTheme((Control)uc);
            ApplyInputValidation(uc);
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
            }
            else if (root is UserControl userControl)
            {
                userControl.AutoScroll = true;
                userControl.RightToLeft = RightToLeft.Yes;
            }

            ApplyResponsiveLayoutRecursive(root);
        }

        private static void ApplyResponsiveLayoutRecursive(Control control)
        {
            foreach (Control child in control.Controls)
            {
                // Keep the designer-defined geometry intact. The previous implementation
                // overwrote margins, padding, anchors, and minimum sizes at runtime, which
                // caused controls to overlap in fixed TableLayoutPanel designs.
                child.RightToLeft = RightToLeft.Yes;

                if (child is DataGridView grid)
                {
                    grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
                    grid.ScrollBars = ScrollBars.Both;
                }

                ApplyResponsiveLayoutRecursive(child);
            }
        }

        /// <summary>
        /// يربط التحقق الأساسي بالحقول النصية في جميع النماذج. التحقق الدلالي
        /// لا يحل محل ValidateInputs داخل النموذج أو Validate داخل الخدمة، بل يمنع
        /// الإدخال الواضح الخطأ قبل وصوله إلى مراحل الحفظ.
        /// </summary>
        public static void ApplyInputValidation(Control root)
        {
            if (root == null || IsDesignMode(root))
                return;

            foreach (Control child in root.Controls)
            {
                TextBox textBox = child as TextBox;
                if (textBox != null)
                {
                    ConfigureTextInput(textBox, textBox.Name, textBox.Text);
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

                KryptonTextBox kryptonTextBox = child as KryptonTextBox;
                if (kryptonTextBox != null)
                {
                    ConfigureTextInput(kryptonTextBox, kryptonTextBox.Name, kryptonTextBox.Text);
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

        private static void ConfigureDateInput(DateTimePicker picker)
        {
            string key = (picker.Name ?? string.Empty).ToLowerInvariant();
            if (ContainsAny(key, "birth", "ميلاد"))
            {
                picker.MaxDate = DateTime.Today;
                if (picker.Value.Date > picker.MaxDate)
                    picker.Value = picker.MaxDate;
            }
        }

        private static void ConfigureTextInput(Control control, string controlName, string currentText)
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

            if (IsIdentityOrNumericField(key))
            {
                SetMaxLength(control, 30);
                return;
            }

            if (IsPersonNameField(key))
            {
                SetMaxLength(control, 150);
                return;
            }

            if (IsLongTextField(key))
                SetMaxLength(control, 1000);
        }

        private static void SetMaxLength(Control control, int maxLength)
        {
            TextBox textBox = control as TextBox;
            if (textBox != null && textBox.MaxLength == 32767)
                textBox.MaxLength = maxLength;

            KryptonTextBox kryptonTextBox = control as KryptonTextBox;
            if (kryptonTextBox != null && kryptonTextBox.MaxLength == 32767)
                kryptonTextBox.MaxLength = maxLength;
        }

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

            if (IsPersonNameField(key) && !(char.IsLetter(e.KeyChar) || char.IsWhiteSpace(e.KeyChar) || e.KeyChar == '-'))
                e.Handled = true;
        }

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
        }

        private static string NormalizeText(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            string[] parts = value.Trim().Split(new[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            return string.Join(" ", parts);
        }

        private static bool IsPersonNameField(string key)
        {
            return ContainsAny(key, "fullname", "firstname", "lastname", "middlename", "studentname", "teachername", "guardianname", "fathername", "mothername", "drivername", "partyname", "payeename", "routename", "stagename", "classname", "roomname", "subjectname", "اسم", "الاسم", "ولي", "اب", "أب", "ام", "أم");
        }

        private static bool IsPhoneField(string key)
        {
            return ContainsAny(key, "phone", "mobile", "tel", "telephone", "هاتف", "جوال", "موبايل");
        }

        private static bool IsEmailField(string key)
        {
            return ContainsAny(key, "email", "mail", "بريد");
        }

        private static bool IsMoneyField(string key)
        {
            return key == "txtfee" || ContainsAny(key, "amount", "price", "salary", "wage", "voucheramount", "expenseamount", "planamount", "discount", "deduction", "paidamount", "remainingamount", "registrationfee", "netamount", "totalamount", "txttotal", "المبلغ", "السعر", "الراتب", "رسوم", "تكلفة");
        }

        private static bool IsIdentityOrNumericField(string key)
        {
            return ContainsAny(key, "studentnumber", "employeenumber", "teachernumber", "nationalid", "identitynumber", "quantity", "count", "hours", "minutes", "days", "number", "studentid", "teacherid", "employeeid", "classid", "subjectid", "roomid", "userid", "routeid", "bookid", "voucherid", "expenseid", "contractid", "enrollmentid", "attendanceid", "copies", "publicationyear", "seatnumber", "workhours", "capacity", "late", "earlyleave", "periodno", "gradeorder", "رقم", "هوية", "كمية", "عدد", "ساعات", "دقائق", "ايام", "أيام");
        }

        private static bool IsLongTextField(string key)
        {
            return ContainsAny(key, "notes", "remark", "description", "address", "ملاحظات", "وصف", "عنوان");
        }

        private static bool ContainsAny(string value, params string[] tokens)
        {
            foreach (string token in tokens)
            {
                if (value.Contains(token))
                    return true;
            }
            return false;
        }

        private static bool IsValidPhoneDigitsOnly(string value)
        {
            return value.Length >= 7 && value.Length <= 20 && value.All(char.IsDigit);
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

        public static bool IsValidPositiveInteger(string value, out int number)
        {
            return int.TryParse((value ?? string.Empty).Trim(), out number) && number > 0;
        }

        public static bool IsValidNonNegativeDecimal(string value, out decimal amount)
        {
            return TryParseDecimal(value, out amount) && amount >= 0m;
        }

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

        public static bool IsValidEmail(string value)
        {
            string text = (value ?? string.Empty).Trim();
            if (text.Length == 0 || text.Length > 254)
                return false;
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

        public static bool IsValidAcademicYear(string value)
        {
            string text = (value ?? string.Empty).Trim();
            string[] parts = text.Split('-');
            int first;
            int second;
            return parts.Length == 2
                && int.TryParse(parts[0].Trim(), out first)
                && int.TryParse(parts[1].Trim(), out second)
                && second == first + 1;
        }

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

        public static bool IsDesignMode(Control control)
        {
            return control != null && (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime ||
                control.Site != null && control.Site.DesignMode);
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
                    kryptonButton.Cursor = Cursors.Hand;
                }
                else if (child is KryptonTextBox kryptonTextBox)
                {
                    kryptonTextBox.ForeColor = TextColor;
                    kryptonTextBox.Font = new Font(FontFamily, BodyFontSize);
                }
                else if (child is KryptonComboBox kryptonComboBox)
                {
                    kryptonComboBox.ForeColor = TextColor;
                    kryptonComboBox.Font = new Font(FontFamily, BodyFontSize);
                    kryptonComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
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

        public static void ShowInformation(string message)
        {
            MessageBox.Show(message, "تمت العملية", MessageBoxButtons.OK, MessageBoxIcon.Information,
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
