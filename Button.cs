using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Drawing;
using System.Windows.Forms;

namespace libLeMS
{

    public class Button : System.Windows.Forms.Button
    {
        // Implements IButtonControl
        private Color colLightBorder;
        private Color colDarkBorder;
        private bool blnPressed = false;
        private bool blnHeld = false;
        private static MasterClass com = new MasterClass();
        public Button() : base()
        {
            try
            {
                BackColor = com.convertColour(com.Config.GetConfig("Colors", "ButtonFace").Setting);
                DoubleBuffered = true;
                // colLightBorder = ControlPaint.Light(BackColor, 50)
                // colDarkBorder = ControlPaint.Dark(BackColor, 50)
                base.Paint += OnPaint;
                base.MouseUp += Button_MouseUp;
                base.MouseDown += Button_MouseDown;
                base.MouseClick += Button_MouseClick;
                base.Click += Button_Click;
                base.KeyDown += Button_KeyDown;
                base.KeyUp += Button_KeyUp;
                Invalidate();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        private void Button_KeyUp(object sender, KeyEventArgs e)
        {
            blnPressed = false;
            KeyUp?.Invoke(this, e);
        }
        private void Button_KeyDown(object sender, KeyEventArgs e)
        {
            blnPressed = (e.KeyCode==Keys.Enter || e.KeyCode==Keys.Space);
            Invalidate();
            KeyDown?.Invoke(this, e);
        }
        private void Button_MouseDown(object sender, MouseEventArgs e)
        {
            blnPressed = (e.Button == MouseButtons.Left);
            Invalidate();
            MouseDown?.Invoke(this, e);
        }
        private void Button_MouseUp(object sender, MouseEventArgs e)
        {
            if ((blnPressed && HoldOnPush && !Held))
                Held = true;
            if (blnPressed && (e.Button == MouseButtons.Left))
                blnPressed = false;
            Invalidate();
            MouseUp?.Invoke(this, e);
        }
        private void Button_MouseClick(object sender, MouseEventArgs e)
        {
            if (blnPressed && (e.Button == MouseButtons.Left))
                blnPressed = false;
            Invalidate();
            MouseClick(sender, e);
        }
        private void Button_Click(object sender, EventArgs e)
        {
            Click(sender, e);
        }
        private  void OnPaint(object sender, PaintEventArgs e)
        {
            // keep       '   MyBase.OnPaint(e)
            // BackColor = com.convertColour(com.Config.GetConfig("Colors", "ButtonFace"))
            // keep
            // keep
            // keep       ForeColor = com.convertColour(com.Config.GetConfig("Colors", "ButtonText"))
            // Console.WriteLine(Font.Name & Font.Size)
            // Dim f As New Font()
            // keep       'Font = SaveSystem.currentTheme.buttonFont
            // keep
            // keep
            colLightBorder = ControlPaint.Light(BackColor, 50);
            colDarkBorder = ControlPaint.Dark(BackColor, 50);
            // keep
            Graphics g = e.Graphics;
            g.Clear(BackColor);
            // keep
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            TextAlign = (ContentAlignment)512;
            Rectangle textRect = new Rectangle(1, 1, Width - 3, Height - 3);
            if (TextImageRelation == TextImageRelation.ImageBeforeText)
            {
                Int32 imgWidth;
                if (Image != null) imgWidth = Image.Width; else imgWidth = 0;
                textRect.X = 6 + (imgWidth);
                textRect.Width = Width - 6 - (imgWidth);
            }
               // Console.WriteLine(TextAlign);
            sf.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Show;
            // ImageAli
            if ((blnPressed && Enabled && (! Stuck)) || Held)
            {
                g.FillRectangle(new SolidBrush(colLightBorder), new Rectangle(0, 0, Width, Height));
                g.FillRectangle(Brushes.Black, new Rectangle(0, 0, Width - 1, Height - 1));
                g.FillRectangle(new SolidBrush(colDarkBorder), new Rectangle(1, 1, Width - 2, Height - 2));
                if (Held)
                    g.FillRectangle( new TextureBrush(AlphaTexture()),2,2, Width - 3, Height - 3);
                else
                    g.FillRectangle(new SolidBrush(BackColor), new Rectangle(2, 2, Width - 3, Height - 3));
                textRect.Offset(1, 1);
                g.DrawString(Text, Font, new SolidBrush(ForeColor), textRect, sf);
            }
            else
            {
                g.FillRectangle(Brushes.Black, new Rectangle(0, 0, Width, Height));
                g.FillRectangle(new SolidBrush(colLightBorder), new Rectangle(0, 0, Width - 1, Height - 1));
                g.FillRectangle(new SolidBrush(colDarkBorder), new Rectangle(1, 1, Width - 2, Height - 2));
                g.FillRectangle(new SolidBrush(BackColor), new Rectangle(1, 1, Width - 3, Height - 3));                
                if ((Enabled))
                    g.DrawString(Text, Font, new SolidBrush(ForeColor), textRect, sf);
                else
                    g.DrawString(Text, Font, new SolidBrush(colDarkBorder), textRect, sf);
            }
            if (!(Image == null))
            {
                if (TextImageRelation == TextImageRelation.Overlay) g.DrawImage(Image, Convert.ToInt32((Width / (double)2) - (Image.Width / (double)2)), Convert.ToInt32((Height / (double)2) - Convert.ToInt32(Image.Height / (double)2)));
                if (TextImageRelation == TextImageRelation.ImageBeforeText) g.DrawImage(Image, 2, Convert.ToInt32((Height / (double)2) - Convert.ToInt32(Image.Height / (double)2)));
            }
            if ((ShowFocusRectangle && Enabled && Focused))
                g.DrawRectangle(new Pen(Color.Black, 1) { DashPattern = new[] { 1f, 1f } }, new Rectangle(3, 3, Width - 7, Height - 7));
        }
        public Bitmap AlphaTexture()
        {
            Bitmap bmp = new Bitmap(4, 4);
            Graphics g = Graphics.FromImage(bmp);
            g.DrawRectangle(new Pen(Color.Silver), 1, 0, 1, 1);
            g.DrawRectangle(new Pen(Color.Silver), 0, 1, 1, 1);
            return bmp;
        }
        public new FlatStyle FlatStyle
        {
            set
            {
            }
            get
            {
                return FlatStyle.Flat;
            }
        }
        public new TextImageRelation TextImageRelation { get; set; } = TextImageRelation.Overlay;
        public bool HoldOnPush { get; set; } = false;
        public bool ShowFocusRectangle { get; set; } = true;
        public bool Stuck { get; set; } = false;
        public bool Held
        {
            set
            {
                blnHeld = value;
                Invalidate();
                if (value == true)
                {
                    foreach (Control con in Parent.Controls)
                    {
                        if (con is Button)
                        {
                            if (!((Button)con).Name.Equals(Name))
                                ((Button)con).Held = false;
                        }
                    }
                }
            }
            get
            {
                return blnHeld;
            }
        }
        public new event KeyEventHandler KeyUp = new KeyEventHandler((e, a) => { });
        public new event KeyEventHandler KeyDown = new KeyEventHandler((e, a) => { });
        public new event MouseEventHandler MouseUp = new MouseEventHandler((e, a) => { });
        public new event MouseEventHandler MouseClick = new MouseEventHandler((e, a) => { });
        public new  event MouseEventHandler MouseDown = new MouseEventHandler((e, a) => { });
        public new event EventHandler Click = new EventHandler((e, a) => { });
        
    }
}