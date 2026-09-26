using System.Drawing;
using System.Windows.Forms;
using SchoolSystem.Helpers;


namespace SchoolSystem
{

    public class CustomMenuRenderer : ToolStripProfessionalRenderer
    {
        // تعريف الدالة CustomMenuRenderer لتنفيذ العملية المرتبطة بها.
        public CustomMenuRenderer() : base(new CustomProfessionalColors()) { }

        // تعريف الدالة OnRenderMenuItemBackground لتنفيذ العملية المرتبطة بها.
        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            bool onMenuStrip = e.Item.Owner != null && e.Item.Owner is MenuStrip;

            if (e.Item.Selected || e.Item.Pressed)
            {

                SetItemTextColor(e.Item, UIHelper.TextColor);
                using (SolidBrush brush = new SolidBrush(e.Item.Selected ? UIHelper.HoverColor : UIHelper.PressedColor))
                    e.Graphics.FillRectangle(brush, e.Item.ContentRectangle);

                return;
            }



            SetItemTextColor(e.Item, onMenuStrip ? Color.White : UIHelper.TextColor);
            base.OnRenderMenuItemBackground(e);
        }

        // تعريف الدالة SetItemTextColor لتنفيذ العملية المرتبطة بها.
        private static void SetItemTextColor(ToolStripItem item, Color color)
        {
            if (item.ForeColor != color)
                item.ForeColor = color;
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
