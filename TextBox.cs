﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace libProChic
{
    public class TextBox: System.Windows.Forms.TextBox
        {
            public static Color textboxcolor = Color.Black;

            public static Color _lightBack = Color.Silver;
            public static Color _darkBack = Color.Silver;
            public TextBox():base()
            {

                try
                {
                    // Draw the border    

      //             this.Paint += new PaintEventHandler((object sender, PaintEventArgs e) =>
      //             {
      //                 // Update a bunch of variables!
      //                 textBox1.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular);
      //
      //                 if (SaveSystem.currentTheme != null)
      //                 {
      //                     textBox1.BackColor = SaveSystem.currentTheme.threeDObjectsColor;
      //                     BackColor = SaveSystem.currentTheme.threeDObjectsColor;
      //                 }
      //                 else
      //                 {
      //                     textBox1.BackColor = Color.White;
      //                     BackColor = Color.White;
      //                 }
      //
      //                 if (SaveSystem.currentTheme != null)
      //                 {
      //                     textboxcolor = SaveSystem.currentTheme.windowColor;
      //
      //                     _lightBack = Paintbrush.GetLightFromColor(textboxcolor);
      //                     _darkBack = Paintbrush.GetDarkFromColor(textboxcolor);
      //                 }
      //             });
      //
      //
      //             tborder.Paint += new PaintEventHandler((object sender, PaintEventArgs e) =>
      //             {
      //                 e.Graphics.DrawLine(new Pen(_darkBack), 0, 0, tborder.Width, 0);
      //                 e.Graphics.DrawLine(Pens.Black, 0, 1, tborder.Width, 1);
      //
      //             });
      //
      //             lborder.Paint += new PaintEventHandler((object sender, PaintEventArgs e) =>
      //             {
      //                 e.Graphics.DrawLine(new Pen(_darkBack), 0, 0, 0, Height);
      //                 e.Graphics.DrawLine(Pens.Black, 1, 0, 1, Height);
      //             });
      //
      //             rborder.Paint += new PaintEventHandler((object sender, PaintEventArgs e) =>
      //             {
      //                 e.Graphics.DrawLine(new Pen(_lightBack), 0, 0, 0, Height - 1);
      //                 e.Graphics.DrawLine(new Pen(textboxcolor), 1, 0, 1, Height - 1);
      //             });
      //
      //             bborder.Paint += new PaintEventHandler((object sender, PaintEventArgs e) =>
      //             {
      //                 e.Graphics.DrawLine(new Pen(_lightBack), 0, 0, Width - 1, 0);
      //                 e.Graphics.DrawLine(new Pen(textboxcolor), 0, 1, Width - 2, 1);
      //             });
      //
      //             tborder.Invalidate();
      //             lborder.Invalidate();
      //             rborder.Invalidate();
      //             bborder.Invalidate();
                }
                catch { }
            }
    }
}
