using System.Drawing;
using System.Windows.Forms;

namespace libLeMS
{
    public class Label:System.Windows.Forms.Label
    {
        public bool DropShadow { get; set; }

        public Label()
        {
            SetStyle(System.Windows.Forms.ControlStyles.SupportsTransparentBackColor, true);
            TextChanged += (s, e) => Invalidate();
        }

       protected override void OnPaint(PaintEventArgs e)
       {
           base.OnPaint(e);
           var g = e.Graphics;
           if (BackColor != Color.Transparent) g.Clear(BackColor);
           StringFormat sf = new StringFormat();
           sf.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Show;
           g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
       
           g.DrawString(Text, Font, new SolidBrush(ForeColor), ClientRectangle, sf);
           Height = (int)g.MeasureString(Text, Font, ClientRectangle.Width).Height;
       }

        private const int CS_DROPSHADOW = 0x00020000;
        protected override System.Windows.Forms.CreateParams CreateParams
        {
            get
            {
                System.Windows.Forms.CreateParams cp = base.CreateParams;
                if (DropShadow) cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }
    }
}
