using System.Drawing;
using System.Windows.Forms;
using SchoolSystem.Helpers;

namespace SchoolSystem
{
    public class CustomMenuRenderer : ToolStripProfessionalRenderer
    {
        public CustomMenuRenderer() : base(new CustomProfessionalColors()) { }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            if (e.Item.Selected)
            {
                using (SolidBrush brush = new SolidBrush(UIHelper.HoverColor))
                    e.Graphics.FillRectangle(brush, e.Item.ContentRectangle);
            }
            else if (e.Item.Pressed)
            {
                using (SolidBrush brush = new SolidBrush(UIHelper.PressedColor))
                    e.Graphics.FillRectangle(brush, e.Item.ContentRectangle);
            }
            else
            {
                base.OnRenderMenuItemBackground(e);
            }
        }
    }

    public class CustomProfessionalColors : ProfessionalColorTable
    {
        public override Color MenuItemSelected => UIHelper.HoverColor;
        public override Color MenuItemBorder => UIHelper.BorderColor;
        public override Color MenuItemPressedGradientBegin => UIHelper.PressedColor;
        public override Color MenuItemPressedGradientEnd => UIHelper.PressedColor;
        public override Color MenuStripGradientBegin => UIHelper.PrimaryColor;
        public override Color MenuStripGradientEnd => UIHelper.PrimaryColor;
        public override Color ToolStripDropDownBackground => UIHelper.SurfaceElevatedColor;
        public override Color ToolStripBorder => UIHelper.BorderColor;
        public override Color ImageMarginGradientBegin => UIHelper.SurfaceSecondaryColor;
        public override Color ImageMarginGradientMiddle => UIHelper.SurfaceSecondaryColor;
        public override Color ImageMarginGradientEnd => UIHelper.SurfaceSecondaryColor;
    }
}