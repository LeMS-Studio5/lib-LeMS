using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace libProChic
{
    public class Scrollbar : Panel
    {
        private MasterClass com = new MasterClass();
        private ConfigHelper themeConfig;
        private String themeLocation = "";
        private Button downArrow = new Button(), upArrow = new Button(), bar = new Button();
        private Bitmap downArr = new Bitmap(1, 1), upArr = new Bitmap(1, 1);
        private ScrollOrientation scrlOr = ScrollOrientation.VerticalScroll;
        private Boolean barDown = false;
        private Point meOgPos;
        public Scrollbar()
        {
            BackColor = com.convertColour(com.Config.GetConfig("Colors", "ButtonFace").Setting);
            if (themeLocation == "") themeLocation = com.toSystemPath(com.Config.GetConfig("Windows", "ThemeLocation").Setting);
            themeConfig = new ConfigHelper(themeLocation + "Config.ini");
            Orientation = ScrollOrientation.HorizontalScroll;
            downArr = com.prepareImage(themeLocation + themeConfig.GetConfig("Scroll", "downArrow").Setting);
            upArr = com.prepareImage(themeLocation + themeConfig.GetConfig("Scroll", "upArrow").Setting);
            bar.Stuck = true;
            bar.MouseDown += Scrollbar_MouseDown;
            bar.MouseUp += Scrollbar_MouseDown;
            bar.MouseMove += Scrollbar_MouseMove;
            downArrow.Click += LargeChanger;
            downArrow.Tag = 1;
            upArrow.Click += LargeChanger;
            upArrow.Tag = -1;
            this.DoubleBuffered = true;
            this.Controls.Add(bar);
            this.Controls.Add(downArrow);
            this.Controls.Add(upArrow);
            Invalidate();
            Debug.WriteLine(Height);
        }
        public new Size Size
        {
            set
            {
                if (scrlOr == ScrollOrientation.HorizontalScroll) base.Size = new Size(value.Width, themeConfig.GetConfigAsInt32("Scroll", "Height")); else base.Size = new Size(themeConfig.GetConfigAsInt32("Scroll", "Height"), value.Height);
                rotate();
                Debug.WriteLine(value);
            }
            get
            {
                return base.Size;
            }
        }
        public ScrollOrientation Orientation { set {  if (scrlOr !=value) base.Size = new Size(base.Height, base.Width); scrlOr = value; rotate(); } get { return scrlOr; } }
        public Int32 ArrowSize { get; set; } = 20;
        public Int32 LargeChange { get; set; } = 5;
        public Int32 ScrollValue { get; set; } = 0;
        private void rotate()
        {
            Image ua = new Bitmap(upArr), da = new Bitmap(downArr);
            if (scrlOr == ScrollOrientation.VerticalScroll)
            {                
                upArrow.Location = new Point(0, 0);
                upArrow.Size = new Size(Width, ArrowSize);
                upArrow.Image = upArr;
                downArrow.Location = new Point(0, Height - ArrowSize);
                downArrow.Size = new Size(Width, ArrowSize);
                downArrow.Image = downArr;
                bar.Size = new Size(Width, 30);
                bar.Location = new Point(0, (ArrowSize) + 15);
            }
            else
            {
                //if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Runtime && themeConfig != null) this.MaximumSize = new Size(Int32.MaxValue,themeConfig.GetConfigAsInt32("Scroll", "Height"));
                ua.RotateFlip(RotateFlipType.Rotate270FlipNone);
                da.RotateFlip(RotateFlipType.Rotate270FlipNone);
                upArrow.Location = new Point(0, 0);
                upArrow.Size = new Size(ArrowSize, Height);
                upArrow.Image = ua;
                downArrow.Location = new Point(Width - ArrowSize, 0);
                downArrow.Size = new Size(ArrowSize, Height);
                downArrow.Image = da;
                bar.Size = new Size(30, Height);
                bar.Location = new Point((ArrowSize) + 15, 0);
            }
        }
        private Int32 BarArea()
        {
            if (scrlOr == ScrollOrientation.HorizontalScroll) return (Width - (ArrowSize * 2)); else return (Height - (ArrowSize * 2));
        }
        private void Scrollbar_MouseDown(Object sender, MouseEventArgs e) // , programname.MouseDown, programIcon.MouseDown ', outputProgramtopbar.MouseDown
        {
            meOgPos = bar.PointToClient(Cursor.Position);
            barDown = !barDown;
        }
        private void Scrollbar_MouseMove(Object sender, MouseEventArgs e)
        {
            if (barDown)
            {
                if (scrlOr == ScrollOrientation.HorizontalScroll) {
                    Int32 x = bar.Left + e.X - meOgPos.X;
                   barLocation(x, bar.Top);
                } else {
                    Int32 y = bar.Top + e.Y - meOgPos.Y;
                    barLocation(bar.Left, y); }
            }
        }
        private void LargeChanger(Object sender, EventArgs e)
        {
            Int32 change = (Int32)((((int)((Button)sender).Tag) * LargeChange / 100.0) * (BarArea() * 1.0));
            if (scrlOr == ScrollOrientation.HorizontalScroll) barLocation(bar.Location.X + change, bar.Location.Y); else barLocation(bar.Location.X, bar.Location.Y + change);
        }
        private void barLocation(Int32 x, Int32 y)
        {
            if (scrlOr == ScrollOrientation.HorizontalScroll)
            {
                if (x > ArrowSize && x+bar.Width < Width - ArrowSize) bar.Location = new Point(x, y);
            }
            else
            {
                if (y > ArrowSize && y+bar.Height < Height - ArrowSize) bar.Location = new Point(x, y);
            }
        }
    }
}
