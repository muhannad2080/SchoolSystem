using System;
using System.Drawing;
using System.IO;

using System.Windows.Forms;

namespace SchoolSystem.Helpers
{
    public static class UIHelper
    {
        // =========================
        // ألوان النظام الموحدة
        // =========================

        public static readonly Color PrimaryColor = Color.FromArgb(30, 41, 59);       // كحلي
        public static readonly Color PrimaryDarkColor = Color.FromArgb(15, 23, 42);   // كحلي أغمق
        public static readonly Color AccentColor = Color.FromArgb(15, 118, 110);      // تركوازي
        public static readonly Color SuccessColor = Color.FromArgb(22, 163, 74);      // أخضر
        public static readonly Color DangerColor = Color.FromArgb(220, 38, 38);       // أحمر
        public static readonly Color WarningColor = Color.FromArgb(217, 119, 6);      // برتقالي
        public static readonly Color NeutralColor = Color.FromArgb(71, 85, 105);      // رمادي
        public static readonly Color ExportColor = Color.FromArgb(13, 148, 136);      // تصدير
        public static readonly Color SearchColor = Color.FromArgb(37, 99, 235);       // أزرق
        public static readonly Color PrintColor = Color.FromArgb(124, 58, 237);       // بنفسجي
        public static readonly Color BackgroundColor = Color.FromArgb(248, 250, 252); // خلفية عامة
        public static readonly Color CardColor = Color.White;                         // بطاقات
        public static readonly Color BorderColor = Color.FromArgb(226, 232, 240);     // حدود
        public static readonly Color TextColor = Color.FromArgb(30, 41, 59);          // نص أساسي
        public static readonly Color MutedTextColor = Color.FromArgb(100, 116, 139);  // نص ثانوي
        public static readonly Color GridSelectionColor = Color.FromArgb(219, 234, 254);

        public static readonly Font DefaultFont = new Font("Tahoma", 10F);
        public static readonly Font DefaultBoldFont = new Font("Tahoma", 10F, FontStyle.Bold);
        public static readonly Font TitleFont = new Font("Tahoma", 15F, FontStyle.Bold);
        public static readonly Font GridFont = new Font("Tahoma", 9.5F);
        public static readonly Font GridHeaderFont = new Font("Tahoma", 9.5F, FontStyle.Bold);

        // =========================
        // تطبيق الثيم العام
        // =========================

        public static void ApplyStyle(Form form)
        {
            ApplyTheme(form);
        }

        public static void ApplyStyle(UserControl uc)
        {
            ApplyTheme(uc);
        }

        public static void ApplyTheme(Control root)
        {
            if (root == null)
                return;

            if (root is Form form)
            {
                form.BackColor = BackgroundColor;
                form.Font = DefaultFont;
                form.RightToLeft = RightToLeft.Yes;
                form.RightToLeftLayout = true;
                form.StartPosition = FormStartPosition.CenterScreen;
            }
            else if (root is UserControl userControl)
            {
                userControl.BackColor = BackgroundColor;
                userControl.Font = DefaultFont;
                userControl.RightToLeft = RightToLeft.Yes;
            }

            ApplyThemeRecursive(root);
        }

        private static void ApplyThemeRecursive(Control control)
        {
            foreach (Control child in control.Controls)
            {
                child.RightToLeft = RightToLeft.Yes;

                if (child is Panel panel)
                {
                    StylePanel(panel);
                }
                else if (child is GroupBox groupBox)
                {
                    StyleGroupBox(groupBox);
                }
                else if (child is Label label)
                {
                    StyleLabel(label);
                }
                else if (child is TextBox textBox)
                {
                    StyleTextBox(textBox);
                }
                else if (child is ComboBox comboBox)
                {
                    StyleComboBox(comboBox);
                }
                else if (child is DateTimePicker dateTimePicker)
                {
                    StyleDateTimePicker(dateTimePicker);
                }
                else if (child is NumericUpDown numericUpDown)
                {
                    StyleNumericUpDown(numericUpDown);
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
                    StyleTabControl(tabControl);
                }
                else if (child is TabPage tabPage)
                {
                    StyleTabPage(tabPage);
                }
                else if (child is TableLayoutPanel tableLayoutPanel)
                {
                    StyleTableLayoutPanel(tableLayoutPanel);
                }
                else if (child is FlowLayoutPanel flowLayoutPanel)
                {
                    StyleFlowLayoutPanel(flowLayoutPanel);
                }
                else if (child is PictureBox pictureBox)
                {
                    StylePictureBox(pictureBox);
                }

                ApplyThemeRecursive(child);
            }
        }

        /// <summary>
        /// يهرب قيمة البحث قبل استخدامها داخل DataView.RowFilter.
        /// </summary>
        public static string EscapeDataViewFilterValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "";

            return value.Trim()
                .Replace("'", "''")
                .Replace("[", "[[]")
                .Replace("%", "[%]")
                .Replace("*", "[*]");
        }

        // =========================
        // Panels / Containers
        // =========================

        public static void StylePanel(Panel panel)
        {
            if (panel == null)
                return;

            // لا نغير لون اللوحات التي لها ألوان خاصة مثل العناوين
            if (panel.BackColor == Color.Transparent || panel.BackColor == SystemColors.Control)
                panel.BackColor = CardColor;
        }

        public static void StyleGroupBox(GroupBox groupBox)
        {
            if (groupBox == null)
                return;

            groupBox.BackColor = CardColor;
            groupBox.ForeColor = TextColor;
            groupBox.Font = DefaultBoldFont;
            groupBox.Padding = new Padding(12, 18, 12, 12);
        }

        public static void StyleTableLayoutPanel(TableLayoutPanel table)
        {
            if (table == null)
                return;

            table.BackColor = CardColor;
            table.RightToLeft = RightToLeft.Yes;
        }

        public static void StyleFlowLayoutPanel(FlowLayoutPanel panel)
        {
            if (panel == null)
                return;

            panel.BackColor = CardColor;
            panel.RightToLeft = RightToLeft.Yes;
        }

        public static void StyleTabControl(TabControl tabControl)
        {
            if (tabControl == null)
                return;

            tabControl.Font = DefaultBoldFont;
            tabControl.RightToLeft = RightToLeft.Yes;
            tabControl.RightToLeftLayout = true;
        }

        public static void StyleTabPage(TabPage tabPage)
        {
            if (tabPage == null)
                return;

            tabPage.BackColor = CardColor;
            tabPage.Padding = new Padding(10);
        }

        public static void StylePictureBox(PictureBox pictureBox)
        {
            if (pictureBox == null)
                return;

            pictureBox.BackColor = BackgroundColor;
            pictureBox.BorderStyle = BorderStyle.FixedSingle;
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
        }

        // =========================
        // Labels / Inputs
        // =========================

        public static void StyleLabel(Label label)
        {
            if (label == null)
                return;

            label.ForeColor = TextColor;
            label.Font = DefaultFont;

            if (!label.AutoSize)
                label.AutoEllipsis = true;
        }

        public static void StyleHeaderLabel(Label label)
        {
            if (label == null)
                return;

            label.ForeColor = Color.White;
            label.Font = TitleFont;
            label.TextAlign = ContentAlignment.MiddleCenter;
        }

        public static void StyleTextBox(TextBox txt)
        {
            if (txt == null)
                return;

            txt.BorderStyle = BorderStyle.FixedSingle;
            txt.Font = DefaultFont;
            txt.BackColor = txt.ReadOnly ? Color.FromArgb(241, 245, 249) : Color.White;
            txt.ForeColor = TextColor;
            txt.Margin = new Padding(4);
        }

        public static void StyleComboBox(ComboBox cmb)
        {
            if (cmb == null)
                return;

            cmb.DropDownStyle = ComboBoxStyle.DropDownList;
            cmb.Font = DefaultFont;
            cmb.BackColor = Color.White;
            cmb.ForeColor = TextColor;
            cmb.Margin = new Padding(4);
        }

        public static void StyleDateTimePicker(DateTimePicker picker)
        {
            if (picker == null)
                return;

            picker.Font = DefaultFont;
            picker.Format = DateTimePickerFormat.Custom;
            picker.CustomFormat = "dd/MM/yyyy";
            picker.Margin = new Padding(4);
        }

        public static void StyleNumericUpDown(NumericUpDown numeric)
        {
            if (numeric == null)
                return;

            numeric.Font = DefaultFont;
            numeric.BackColor = Color.White;
            numeric.ForeColor = TextColor;
            numeric.Margin = new Padding(4);
        }

        // =========================
        // Buttons
        // =========================

        public static void StyleActionButton(Button btn)
        {
            if (btn == null)
                return;

            string text = (btn.Text ?? string.Empty).Trim();

            if (ContainsAny(text, "حذف", "إزالة"))
            {
                StyleButton(btn, DangerColor);
                return;
            }

            if (ContainsAny(text, "حفظ", "إضافة", "اعتماد", "قبول"))
            {
                StyleButton(btn, SuccessColor);
                return;
            }

            if (ContainsAny(text, "جديد"))
            {
                StyleButton(btn, PrimaryColor);
                return;
            }

            if (ContainsAny(text, "بحث", "استعلام"))
            {
                StyleButton(btn, SearchColor);
                return;
            }

            if (ContainsAny(text, "تصدير", "Excel", "اكسل"))
            {
                StyleButton(btn, ExportColor);
                return;
            }

            if (ContainsAny(text, "طباعة", "بطاقة", "استمارة", "كشف"))
            {
                StyleButton(btn, PrintColor);
                return;
            }

            if (ContainsAny(text, "تعديل", "تحديث بيانات"))
            {
                StyleButton(btn, SearchColor);
                return;
            }

            if (ContainsAny(text, "تحديث", "إلغاء", "مسح", "إغلاق", "رجوع"))
            {
                StyleButton(btn, NeutralColor);
                return;
            }

            StyleButton(btn, AccentColor);
        }

        public static void StyleButton(Button btn, Color backColor)
        {
            if (btn == null)
                return;

            btn.FlatStyle = FlatStyle.Flat;
            btn.BackColor = backColor;
            btn.ForeColor = Color.White;
            btn.FlatAppearance.BorderSize = 0;
            btn.Cursor = Cursors.Hand;
            btn.Font = DefaultBoldFont;
            btn.Height = 40;
            btn.MinimumSize = new Size(85, 36);
            btn.Margin = new Padding(5, 4, 5, 4);
            btn.UseVisualStyleBackColor = false;
        }

        public static void StylePrimaryButton(Button btn)
        {
            StyleButton(btn, AccentColor);
        }

        public static void StyleDangerButton(Button btn)
        {
            StyleButton(btn, DangerColor);
        }

        public static void StyleSuccessButton(Button btn)
        {
            StyleButton(btn, SuccessColor);
        }

        public static void StyleWarningButton(Button btn)
        {
            StyleButton(btn, WarningColor);
        }

        public static void StyleNeutralButton(Button btn)
        {
            StyleButton(btn, NeutralColor);
        }

        // =========================
        // DataGridView
        // =========================

        public static void StyleDataGridView(DataGridView dgv)
        {
            if (dgv == null)
                return;

            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = PrimaryColor;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = GridHeaderFont;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersHeight = 42;

            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = TextColor;
            dgv.DefaultCellStyle.Font = GridFont;
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.DefaultCellStyle.SelectionBackColor = GridSelectionColor;
            dgv.DefaultCellStyle.SelectionForeColor = Color.FromArgb(30, 64, 175);

            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(249, 250, 251);
            dgv.AlternatingRowsDefaultCellStyle.ForeColor = TextColor;

            dgv.GridColor = BorderColor;
            dgv.RowTemplate.Height = 35;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.ReadOnly = true;
            dgv.RowHeadersVisible = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.RightToLeft = RightToLeft.Yes;
        }

        // =========================
        // Validation Helpers
        // =========================

        public static void AllowOnlyNumbers(TextBox textBox)
        {
            if (textBox == null)
                return;

            textBox.KeyPress -= NumbersOnly_KeyPress;
            textBox.KeyPress += NumbersOnly_KeyPress;
        }

        public static void AllowOnlyDecimal(TextBox textBox)
        {
            if (textBox == null)
                return;

            textBox.KeyPress -= DecimalOnly_KeyPress;
            textBox.KeyPress += DecimalOnly_KeyPress;
        }

        public static void PreventNumbers(TextBox textBox)
        {
            if (textBox == null)
                return;

            textBox.KeyPress -= NoNumbers_KeyPress;
            textBox.KeyPress += NoNumbers_KeyPress;
        }

        private static void NumbersOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
                return;

            if (!char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private static void DecimalOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
                return;

            TextBox textBox = sender as TextBox;

            if (char.IsDigit(e.KeyChar))
                return;

            if (e.KeyChar == '.' && textBox != null && !textBox.Text.Contains("."))
                return;

            e.Handled = true;
        }

        private static void NoNumbers_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
                return;

            if (char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        // =========================
        // Messages
        // =========================

        public static void ShowError(string message)
        {
            MessageBox.Show(
                message,
                "خطأ",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
        }

                public static void ShowException(string operation, Exception exception)
        {
            string safeOperation = string.IsNullOrWhiteSpace(operation) ? "العملية" : operation.Trim();
            LogException(safeOperation, exception);
            ShowError("تعذر إتمام " + safeOperation + ". تحقق من البيانات أو الاتصال بقاعدة البيانات ثم حاول مرة أخرى.");
        }

        public static void LogException(string operation, Exception exception)
        {
            try
            {
                string logDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SchoolSystem", "Logs");
                Directory.CreateDirectory(logDirectory);
                string logPath = Path.Combine(logDirectory, "errors.log");
                File.AppendAllText(logPath,
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " | " + operation + " | " + exception + Environment.NewLine);
            }
            catch
            {
                // لا نسمح بفشل التسجيل أن يعطل واجهة المستخدم.
            }
        }

        public static void ShowInfo(string message)

        {
            MessageBox.Show(
                message,
                "معلومات",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
        }

        public static void ShowWarning(string message)
        {
            MessageBox.Show(
                message,
                "تنبيه",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
        }

        public static void ShowSuccess(string message)
        {
            MessageBox.Show(
                message,
                "نجاح",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
        }

        // =========================
        // Private Helpers
        // =========================

        private static bool ContainsAny(string text, params string[] values)
        {
            if (string.IsNullOrWhiteSpace(text))
                return false;

            foreach (string value in values)
            {
                if (text.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0)
                    return true;
            }

            return false;
        }
    }
}
