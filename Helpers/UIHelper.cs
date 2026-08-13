using System;
using System.Drawing;
using System.Windows.Forms;
using Krypton.Toolkit;

namespace SchoolSystem.Helpers
{
    public static class UIHelper
    {
        private static readonly KryptonManager KryptonThemeManager = new KryptonManager();

        public static readonly Color PrimaryColor = Color.FromArgb(30, 41, 59);
        public static readonly Color AccentColor = Color.FromArgb(15, 118, 110);
        public static readonly Color SuccessColor = Color.FromArgb(22, 163, 74);
        public static readonly Color DangerColor = Color.FromArgb(198, 40, 40);
        public static readonly Color NeutralColor = Color.FromArgb(71, 85, 105);
        public static readonly Color ExportColor = Color.FromArgb(13, 148, 136);
        public static readonly Color SearchColor = Color.FromArgb(37, 99, 235);
        public static readonly Color BackgroundColor = Color.FromArgb(248, 250, 252);
        public static readonly Color TextColor = Color.FromArgb(30, 41, 59);

        public static void ApplyStyle(Form form)
        {
            ApplyKryptonTheme();
            ApplyTheme(form);
        }

        public static void ApplyStyle(UserControl uc)
        {
            ApplyKryptonTheme();
            ApplyTheme(uc);
        }

        public static void ApplyKryptonTheme()
        {
            KryptonThemeManager.GlobalPaletteMode = PaletteMode.Office2010Blue;
            KryptonThemeManager.BaseFont = new Font("Tahoma", 10F);
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
                child.Font = new Font("Tahoma", 10F);

                if (child is KryptonButton kryptonButton)
                {
                    kryptonButton.ForeColor = Color.White;
                    kryptonButton.Font = new Font("Tahoma", 10F, FontStyle.Bold);
                    kryptonButton.MinimumSize = new Size(90, 38);
                    kryptonButton.Cursor = Cursors.Hand;
                }
                else if (child is KryptonTextBox kryptonTextBox)
                {
                    kryptonTextBox.ForeColor = TextColor;
                    kryptonTextBox.Font = new Font("Tahoma", 10F);
                    kryptonTextBox.MinimumSize = new Size(120, 34);
                }
                else if (child is KryptonComboBox kryptonComboBox)
                {
                    kryptonComboBox.ForeColor = TextColor;
                    kryptonComboBox.Font = new Font("Tahoma", 10F);
                    kryptonComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                    kryptonComboBox.MinimumSize = new Size(120, 34);
                }
                else if (child is KryptonLabel kryptonLabel)
                {
                    kryptonLabel.ForeColor = TextColor;
                    kryptonLabel.Font = new Font("Tahoma", 10F);
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
                form.Font = new Font("Tahoma", 10F);
                form.RightToLeft = RightToLeft.Yes;
                form.RightToLeftLayout = true;
            }
            else if (root is UserControl userControl)
            {
                userControl.BackColor = BackgroundColor;
                userControl.Font = new Font("Tahoma", 10F);
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
                    panel.BackColor = Color.White;
                }
                else if (child is GroupBox groupBox)
                {
                    groupBox.BackColor = Color.White;
                    groupBox.ForeColor = TextColor;
                    groupBox.Font = new Font("Tahoma", 10F, FontStyle.Bold);
                }
                else if (child is Label label)
                {
                    label.ForeColor = TextColor;
                    if (!label.AutoSize)
                        label.AutoEllipsis = true;
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
                    dateTimePicker.Font = new Font("Tahoma", 10F);
                }
                else if (child is NumericUpDown numericUpDown)
                {
                    numericUpDown.Font = new Font("Tahoma", 10F);
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
                    tabControl.Font = new Font("Tahoma", 10F);
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
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = PrimaryColor;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgv.ColumnHeadersHeight = 40;

            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(224, 231, 255);
            dgv.DefaultCellStyle.SelectionForeColor = PrimaryColor;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(249, 250, 251);
            dgv.RowTemplate.Height = 35;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AllowUserToAddRows = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.GridColor = Color.FromArgb(229, 231, 235);
        }

        public static void StyleButton(Button btn, Color backColor)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.BackColor = backColor;
            btn.ForeColor = Color.White;
            btn.FlatAppearance.BorderSize = 0;
            btn.Cursor = Cursors.Hand;
            btn.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btn.Height = 38;
        }

        public static void StyleButton(KryptonButton btn, Color backColor)
        {
            if (btn == null)
                return;

            btn.BackColor = backColor;
            btn.ForeColor = Color.White;
            btn.Cursor = Cursors.Hand;
            btn.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btn.Height = 38;
            btn.MinimumSize = new Size(90, 38);
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
            txt.Font = new Font("Tahoma", 10F);
        }

        public static void StyleTextBox(KryptonTextBox txt)
        {
            if (txt == null)
                return;

            txt.Font = new Font("Tahoma", 10F);
            txt.ForeColor = TextColor;
            txt.BackColor = Color.White;
        }

        public static void StyleComboBox(ComboBox cmb)
        {
            cmb.DropDownStyle = ComboBoxStyle.DropDownList;
            cmb.Font = new Font("Tahoma", 10F);
        }

        public static void ShowException(string operation, Exception exception)
        {
            string safeOperation = string.IsNullOrWhiteSpace(operation) ? "العملية" : operation.Trim();
            ApplicationLogger.LogException(safeOperation, exception, "errors.log");
            ShowError("تعذر إتمام " + safeOperation + ". تحقق من البيانات أو الاتصال بقاعدة البيانات ثم حاول مرة أخرى.");
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
    }
}
