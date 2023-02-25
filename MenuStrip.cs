using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace libLeMS
{
    public class MenuStrip : System.Windows.Forms.MenuStrip
    {
        public MenuStrip()
        {
            this.Renderer = new ProChicRender();
        }
    }
    public class ProChicCol : ProfessionalColorTable
    {
        Color bkgrdCol;
        Color bkgrdSelCol;
        MasterClass com = new MasterClass();
        public ProChicCol()
        {
            bkgrdSelCol = com.convertColour(com.Config.GetConfig("Colors", "Hilight").Setting);
            bkgrdCol = com.convertColour(com.Config.GetConfig("Colors", "ButtonFace").Setting);
        }
        public override Color MenuItemSelected
        {
            get { return com.convertColour(com.Config.GetConfig("Colors", "Hilight").Setting); }
        }
        public void test()
        {
            //base.Presse
        }
        public override Color ToolStripDropDownBackground{
            get  {
                return bkgrdCol;
            }
        }
            public override Color ImageMarginGradientBegin{
                get {
                    return bkgrdCol;
                }
        }
        public override Color ImageMarginGradientMiddle{
            get {
                return bkgrdCol;
            }
        }
        public override Color ImageMarginGradientEnd{
            get {
                return bkgrdCol;
            }
        }
        public override Color MenuBorder{
            get  {
                return Color.Black;
            }
        }
        public override Color MenuItemBorder{
            get {
                return Color.Black;
            }
        }
        public override Color MenuStripGradientBegin {
            get {
                return bkgrdCol;
            }
        }
        public override Color MenuStripGradientEnd {
            get{
                return bkgrdCol;
            }
        }
        public override Color MenuItemSelectedGradientBegin {
            get{
                return bkgrdSelCol; 
            }
        }
        public override Color MenuItemSelectedGradientEnd
        {
            get{
                return bkgrdSelCol;
            }
        }
        public override Color MenuItemPressedGradientBegin
        {
            get{
                return bkgrdSelCol;
            }
        }
        public override Color MenuItemPressedGradientEnd {
            get{
                //if (e.Item.Selected) return ; else e.Item.ForeColor = com.convertColour(com.Config.GetConfig("Colors", "MenuText").Setting);
                return bkgrdSelCol;
            }
        }
    }
    public class ProChicRender : ToolStripProfessionalRenderer
    {
        MasterClass com = new MasterClass();
        public ProChicRender() : base(new ProChicCol())
        {
        }
        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            base.OnRenderMenuItemBackground(e);
            Rectangle rc = new Rectangle(Point.Empty, e.Item.Size);
            Color c = Color.PaleGreen;
            //if (e.Item.Selected) c = bkgrdSelCol; else c = bkgrdCol;
            //using (SolidBrush brush = new SolidBrush(c))   e.Graphics.FillRectangle(brush, rc);
            //e.ToolStrip.BackColor = Color.Yellow;//e.Item.BackColor = Color.Yellow
            //if (e.Item.Selected) e.ToolStrip.Col= Color.Yellow;  else e.Item.BackColor = bkgrdCol;
            ////  e.Item.BackColor= com.convertColour(com.Config.GetConfig("Colors", "Hilight").Setting);
        }
        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            base.OnRenderItemText(e);
            //Debug.WriteLineIf(e.Item.Selected, com.convertColour(com.Config.GetConfig("Colors", "HilightText").Setting).ToString());
            if (e.Item.Selected) e.Item.ForeColor = com.convertColour(com.Config.GetConfig("Colors", "HilightText").Setting); else e.Item.ForeColor = com.convertColour(com.Config.GetConfig("Colors", "MenuText").Setting);
        }
    }
}
