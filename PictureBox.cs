using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace libProChic
{
    public class PictureBox : System.Windows.Forms.PictureBox, IMessageFilter
    {
        public PictureBox() : base()
        {
            // reduce flickering
            this.DoubleBuffered = true;
            ManagedMouseWheelStart();
            this.MouseWheel += this_MouseScroll;
            this.MouseEnter += this_MouseEnter;
        }
        private void this_MouseEnter(object sender, EventArgs e)
        {
            this.Focus();
        }

        private SizeF imageSize = new SizeF();
        private Image imgOriginal;
        public Boolean ZoomBeyoundFit { get; set; }
        public new Image Image
        {
            get { return imgOriginal; }
            set
            {
                imgOriginal = value;
                imageSize = new SizeF(-1, -1);
                PictureBoxZoom();
                imageSize = new SizeF(1, 1);
            }
        }
        public void PictureBoxZoom()
        {
            Bitmap bm;
            if (imgOriginal == null) return;
            if (imageSize.Height==-1||(ZoomBeyoundFit && ((imgOriginal.Width * imageSize.Width < this.Width) || (imgOriginal.Height * imageSize.Height < this.Height)))) bm = ExplorerListView.ScaleImage((Bitmap)imgOriginal, this.Width, this.Height); else if ((imgOriginal.Width * imageSize.Width<10) ||(imgOriginal.Width * imageSize.Width<10)) bm = ExplorerListView.ScaleImage((Bitmap)imgOriginal,10,10); else bm = new Bitmap(imgOriginal, Convert.ToInt32(imgOriginal.Width * imageSize.Width), Convert.ToInt32(imgOriginal.Height * imageSize.Height));
            Graphics grap = Graphics.FromImage(bm);
            grap.InterpolationMode = InterpolationMode.HighQualityBicubic;
            base.Image = bm;
            System.GC.Collect();
        }
        private void this_MouseScroll(object sender, MouseEventArgs e)
        {
            imageSize = new SizeF((imageSize.Width + (e.Delta / 120 / 50.0f)), (imageSize.Height + (e.Delta / 120 / 50.0f)));
            PictureBoxZoom();
        }
        #region Scroll
        private bool managed { get; set; } = false;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                ManagedMouseWheelStop();
            base.Dispose(disposing);
        }
        
        /************************************
         * IMessageFilter implementation
         * *********************************/
        private const int WM_MOUSEWHEEL = 0x20a;
        // P/Invoke declarations
        [DllImport("user32.dll")]
        private static extern IntPtr WindowFromPoint(Point pt);
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

        private bool IsChild(Control ctrl)
        {
            Control loopCtrl = ctrl;

            while (loopCtrl != null && loopCtrl != this)
                loopCtrl = loopCtrl.Parent;

            return (loopCtrl == this);
        }

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == WM_MOUSEWHEEL)
            {
                //Ensure the message was sent to a child of the current form
                if (IsChild(Control.FromHandle(m.HWnd)))
                {
                    // Find the control at screen position m.LParam
                    Point pos = new Point(m.LParam.ToInt32() & 0xffff, m.LParam.ToInt32() >> 16);

                    //Ensure control under the mouse is valid and is not the target control
                    //otherwise we'd be trap in a loop.
                    IntPtr hWnd = WindowFromPoint(pos);
                    if (hWnd != IntPtr.Zero && hWnd != m.HWnd && Control.FromHandle(hWnd) != null)
                    {
                        SendMessage(hWnd, m.Msg, m.WParam, m.LParam);
                        return true;
                    }
                }
            }
            return false;
        }

        /****************************************
         * MouseWheelManagedForm specific methods
         * **************************************/
        public void ManagedMouseWheelStart()
        {
            if (!managed)
            {
                managed = true;
                Application.AddMessageFilter(this);
            }
        }

        public void ManagedMouseWheelStop()
        {
            if (managed)
            {
                managed = false;
                Application.RemoveMessageFilter(this);
            }

        }
#endregion

    }
}