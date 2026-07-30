using System.Drawing;
using System.Windows.Forms;

namespace SchoolSystem
{
    public class CustomMenuRenderer : ToolStripProfessionalRenderer
    {
        public CustomMenuRenderer() : base(new CustomProfessionalColors()) { }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            if (e.Item.Selected)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(52, 73, 94)), e.Item.ContentRectangle);
            }
            else if (e.Item.Pressed)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(41, 128, 185)), e.Item.ContentRectangle);
            }
            else
            {
                base.OnRenderMenuItemBackground(e);
            }
        }
    }

    public class CustomProfessionalColors : ProfessionalColorTable
    {
        public override Color MenuItemSelected => Color.FromArgb(52, 73, 94);
        public override Color MenuItemBorder => Color.FromArgb(33, 42, 57);
        public override Color MenuItemPressedGradientBegin => Color.FromArgb(41, 128, 185);
        public override Color MenuItemPressedGradientEnd => Color.FromArgb(41, 128, 185);
        public override Color MenuStripGradientBegin => Color.FromArgb(33, 42, 57);
        public override Color MenuStripGradientEnd => Color.FromArgb(33, 42, 57);
        public override Color ToolStripDropDownBackground => Color.FromArgb(33, 42, 57);
        public override Color ToolStripBorder => Color.FromArgb(52, 73, 94);
        public override Color ImageMarginGradientBegin => Color.FromArgb(33, 42, 57);
        public override Color ImageMarginGradientMiddle => Color.FromArgb(33, 42, 57);
        public override Color ImageMarginGradientEnd => Color.FromArgb(33, 42, 57);
    }
}