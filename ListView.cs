using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace libLeMS
{
    public class ListView : System.Windows.Forms.ListView
    {
        private System.ComponentModel.IContainer components;
        protected MasterClass com;

        public ListView() : base()
        {
            try
            {
                com = new MasterClass();
                SmallImageList = new System.Windows.Forms.ImageList();
                LargeImageList = new System.Windows.Forms.ImageList();
                //   Console.WriteLine(Items == null);
                InitializeComponent();
                Invalidate();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components != null)
                    components.Dispose();
            }
            finally
            {
                base.Dispose(disposing);
            }
        }
        //private Color bColour;
        public new Color BackColor
        {
            get
            {
                return base.BackColor;
            }set
            {
                base.BackColor = value;
                if (com.ColourDistance(base.BackColor, Color.White) > com.ColourDistance(base.BackColor, Color.Black)) ForeColor = Color.White; else ForeColor = Color.Black;
            }
        }
        public Color SelectedColor { get; set; }
        public Color SelectedTextColor { get; set; }
        //public new System.Windows.Forms.ImageList LargeImageList { get; set; } = new System.Windows.Forms.ImageList();
        //public new System.Windows.Forms.ImageList SmallImageList { get; set; } = new System.Windows.Forms.ImageList();
        public Size LargeImageListSize { get { return LargeImageList.ImageSize; } set { LargeImageList.ImageSize = value; } }
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            DoubleBuffered = true;
            if (com != null)
            {
                OwnerDraw = false;
                SelectedColor = com.convertColour(com.Config.GetConfig("Colors", "Hilight").Setting); //This and next few lines requires 95 INI file TODO: Update
                SelectedTextColor = com.convertColour(com.Config.GetConfig("Colors", "HilightText").Setting);
                BackColor = com.convertColour(com.Config.GetConfig("Colors", "Window").Setting);
                ForeColor = com.convertColour(com.Config.GetConfig("Colors", "WindowText").Setting);
                this.DrawItem += mybase_DrawItem;
            }
            //this.DrawSubItem += mybase_DrawItem;
            GridLines = true;
        }
        private void mybase_DrawItem(object sender, DrawListViewItemEventArgs e)        //http://vb-helper.com/howto_net_owner_draw_listview.html
        {
            try
            {
                //  e.DrawText();// = false;
                // Draw Details view items in the DrawSubItem event handler.
                //      if (e.Item.ListView.View == View.Details) return;

                // Get the ListView item and the ServerStatus object.
                //      ListViewItem item = e.Item;

                // Clear.
                //e.DrawBackground();
                if (e.State == ListViewItemStates.Selected) ItemColour(e.ItemIndex, SelectedTextColor, SelectedColor); else ItemColour(e.ItemIndex, ForeColor, BackColor);
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                sf.FormatFlags = StringFormatFlags.DirectionRightToLeft;

                // Draw a status indicator.
                //        e.Graphics.SmoothingMode =
                //            System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                Rectangle rect = new Rectangle(e.Bounds.Left + 1, e.Bounds.Top + 1,e.Bounds.Height - 2, e.Bounds.Height - 2);
                //        using (SolidBrush br =
                //            new SolidBrush(ForeColor))
                //        {
                //            e.Graphics.FillEllipse(br, rect);
                //        }
                //        e.Graphics.DrawEllipse(Pens.Black, rect);
                int left = rect.Right + 2;
                //
                Bitmap img = new Bitmap(1, 1);
                //if (SmallImageList.Images.Get(e.Item.ImageIndex) != null) img = new Bitmap(SmallImageList.Images.Get(e.Item.ImageIndex), ImageSize);
                //         // See how much we must scale it.
                //         float scale;
                //  Console.WriteLine(e.Bounds.Height + ","+ ImageSize.Height);
                float scale = 1F;// ImageSize.Height;//e.Bounds.Height;// / (float)ImageSize.Height;//img.Height;

                // Scale and position the image.
                e.Graphics.ScaleTransform(scale, scale);
                e.Graphics.TranslateTransform(left,left+(e.ItemIndex*((LargeImageListSize.Height+30)*scale)) ,System.Drawing.Drawing2D.MatrixOrder.Append);  //+ (e.Bounds.Height - img.Height * scale) /1
                                                                                                                                                             //
                                                                                                                                                             //         // Draw the image.
                e.Graphics.DrawImage(com.prepareImage(img, true), 0, 0);
                SizeF sizTxt = e.Graphics.MeasureString(e.Item.Text, e.Item.Font, e.Bounds.Width, sf);
                Rectangle txtRec = new Rectangle(new Point(0, img.Height), new Size((int)sizTxt.Width,(int)sizTxt.Height));//TextRenderer.MeasureText(e.Item.Text, e.Item.Font, new Size(img.Width, img.Height)));

                e.Graphics.FillRectangle(new SolidBrush(e.Item.BackColor),txtRec);
                e.Graphics.DrawString(e.Item.Text, e.Item.Font, new SolidBrush(e.Item.ForeColor), txtRec,sf);
                // Draw the focus rectangle if appropriate.
                e.Graphics.ResetTransform();
                //  e.DrawFocusRectangle();
            }catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
    }
        private void ItemColour(int index, Color fColour, Color bColour)
        {
            Items[index].BackColor = bColour;
            Items[index].ForeColor = fColour;
            
        }
    }
}
