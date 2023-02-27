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
using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows.Forms;
using System.Net;
using System.Security.Permissions;

namespace libLeMS
{
    public class Shell: System.Windows.Forms.Panel
    {
        protected IntPtr id;
        private MasterClass com = new MasterClass();               
        public Shell()
        {
           // if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
                InitializeComponent();
            if (themeLocation == "") themeLocation = com.toSystemPath(com.Config.GetConfig("Windows", "ThemeLocation").Setting);
            themeConfig = new ConfigHelper(themeLocation + "Config.ini");
            themeRefresh();
            id = this.Handle;
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
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.program = new System.Windows.Forms.Panel();
            this.GuestArea = new System.Windows.Forms.Panel();
            this.programtopbar = new System.Windows.Forms.Panel();
            this.programIcon = new System.Windows.Forms.PictureBox();
            this.maximizebutton = new Button();
            this.minimizebutton = new Button();
            this.programname = new Label();
            this.closebutton = new Button();
            this.toprightcorner = new System.Windows.Forms.Panel();
            this.bottomrightcorner = new System.Windows.Forms.Panel();
            this.bottomleftcorner = new System.Windows.Forms.Panel();
            this.topleftcorner = new System.Windows.Forms.Panel();
            this.lefts = new System.Windows.Forms.Panel();
            this.bottoms = new System.Windows.Forms.Panel();
            this.rights = new System.Windows.Forms.Panel();
            this.tops = new System.Windows.Forms.Panel();
            this.pullbs = new System.Windows.Forms.Timer(this.components);
            this.pullbottom = new System.Windows.Forms.Timer(this.components);
            this.pullside = new System.Windows.Forms.Timer(this.components);
            this.timData = new System.Windows.Forms.Timer(this.components);
            this.program.SuspendLayout();
            this.programtopbar.SuspendLayout();
            this.SuspendLayout();
            // 
            // program
            // 
            this.program.BackColor = System.Drawing.Color.Silver;
            this.program.Controls.Add(this.GuestArea);
            this.program.Controls.Add(this.programtopbar);
            this.program.Controls.Add(this.toprightcorner);
            this.program.Controls.Add(this.bottomrightcorner);
            this.program.Controls.Add(this.bottomleftcorner);
            this.program.Controls.Add(this.topleftcorner);
            this.program.Controls.Add(this.lefts);
            this.program.Controls.Add(this.bottoms);
            this.program.Controls.Add(this.rights);
            this.program.Controls.Add(this.tops);
            this.program.Dock = System.Windows.Forms.DockStyle.Fill;
            this.program.Location = new System.Drawing.Point(0, 0);
            this.program.Name = "program";
            this.program.Size = new System.Drawing.Size(359, 310);
            this.program.TabIndex = 3;
            // 
            // GuestArea
            // 
            this.GuestArea.BackColor = Color.Red;
            this.GuestArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GuestArea.Location = new System.Drawing.Point(4, 22);
            this.GuestArea.Name = "GuestArea";
            this.GuestArea.Size = new System.Drawing.Size(351, 284);
            this.GuestArea.TabIndex = 9;
            // 
            // programtopbar
            // 
            this.programtopbar.BackColor = System.Drawing.Color.DarkBlue;
            this.programtopbar.Controls.Add(this.programIcon);
            this.programtopbar.Controls.Add(this.maximizebutton);
            this.programtopbar.Controls.Add(this.minimizebutton);
            this.programtopbar.Controls.Add(this.programname);
            this.programtopbar.Controls.Add(this.closebutton);
            this.programtopbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.programtopbar.Location = new System.Drawing.Point(4, 4);
            this.programtopbar.Name = "programtopbar";
            this.programtopbar.Size = new System.Drawing.Size(351, 18);
            this.programtopbar.TabIndex = 0;
            this.programtopbar.MouseDown += programtopbar_MouseDown;
            // 
            // programIcon
            // 
            this.programIcon.BackColor = System.Drawing.Color.Transparent;
            this.programIcon.Location = new System.Drawing.Point(2, 1);
            this.programIcon.Name = "programIcon";
            this.programIcon.Size = new System.Drawing.Size(16, 16);
            this.programIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.programIcon.TabIndex = 7;
            this.programIcon.TabStop = false;
            // 
            // maximizebutton
            // 
            this.maximizebutton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.maximizebutton.Location = new System.Drawing.Point(317, 2);
            this.maximizebutton.Name = "maximizebutton";
            this.maximizebutton.ShowFocusRectangle = false;
            this.maximizebutton.Size = new System.Drawing.Size(16, 14);
            this.maximizebutton.TabIndex = 6;
            this.maximizebutton.TabStop = false;
            this.maximizebutton.Tag = "6";
            this.maximizebutton.Click += maximizebutton_Click;
            // 
            // minimizebutton
            // 
            this.minimizebutton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.minimizebutton.Location = new System.Drawing.Point(301, 2);
            this.minimizebutton.Name = "minimizebutton";
            this.minimizebutton.ShowFocusRectangle = false;
            this.minimizebutton.Size = new System.Drawing.Size(16, 14);
            this.minimizebutton.TabIndex = 5;
            this.minimizebutton.TabStop = false;
            this.minimizebutton.Tag = "2";
            this.minimizebutton.Click += minimizebutton_Click;
            // 
            // programname
            // 
            this.programname.AutoSize = true;
            this.programname.BackColor = System.Drawing.Color.Transparent;
            this.programname.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, System.Convert.ToByte(0));
            this.programname.ForeColor = System.Drawing.Color.White;
            this.programname.Location = new System.Drawing.Point(26, 3);
            this.programname.Name = "programname";
            this.programname.Size = new System.Drawing.Size(90, 13);
            this.programname.TabIndex = 3;
            this.programname.Text = "Program1name";
            // 
            // closebutton
            // 
            this.closebutton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.closebutton.Location = new System.Drawing.Point(334, 2);
            this.closebutton.Name = "closebutton";
            this.closebutton.ShowFocusRectangle = false;
            this.closebutton.Size = new System.Drawing.Size(16, 14);
            this.closebutton.TabIndex = 4;
            this.closebutton.TabStop = false;
            this.closebutton.Tag = "0";
            this.closebutton.Click += closebutton_Click;
            // 
            // toprightcorner
            // 
            this.toprightcorner.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
            this.toprightcorner.Location = new System.Drawing.Point(355, 0);
            this.toprightcorner.Name = "toprightcorner";
            this.toprightcorner.Size = new System.Drawing.Size(4, 4);
            this.toprightcorner.TabIndex = 6;
            // 
            // bottomrightcorner
            // 
            this.bottomrightcorner.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
            this.bottomrightcorner.Cursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.bottomrightcorner.Location = new System.Drawing.Point(355, 306);
            this.bottomrightcorner.Name = "bottomrightcorner";
            this.bottomrightcorner.Size = new System.Drawing.Size(4, 4);
            this.bottomrightcorner.TabIndex = 4;
            this.bottomrightcorner.MouseDown += bspull_MouseDown;
            this.bottomrightcorner.MouseUp += bspull_MouseUp;
            // 
            // bottomleftcorner
            // 
            this.bottomleftcorner.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
            this.bottomleftcorner.Location = new System.Drawing.Point(0, 306);
            this.bottomleftcorner.Name = "bottomleftcorner";
            this.bottomleftcorner.Size = new System.Drawing.Size(4, 4);
            this.bottomleftcorner.TabIndex = 2;
            // 
            // topleftcorner
            // 
            this.topleftcorner.Location = new System.Drawing.Point(0, 0);
            this.topleftcorner.Name = "topleftcorner";
            this.topleftcorner.Size = new System.Drawing.Size(4, 4);
            this.topleftcorner.TabIndex = 1;
            // 
            // lefts
            // 
            this.lefts.Dock = System.Windows.Forms.DockStyle.Left;
            this.lefts.Location = new System.Drawing.Point(0, 4);
            this.lefts.Name = "lefts";
            this.lefts.Size = new System.Drawing.Size(4, 302);
            this.lefts.TabIndex = 3;
            // 
            // bottoms
            // 
            this.bottoms.Cursor = System.Windows.Forms.Cursors.SizeNS;
            this.bottoms.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottoms.Location = new System.Drawing.Point(0, 306);
            this.bottoms.Name = "bottoms";
            this.bottoms.Size = new System.Drawing.Size(355, 4);
            this.bottoms.TabIndex = 5;
            this.bottoms.MouseDown += bottompull_MouseDown;
            this.bottoms.MouseUp += buttompull_MouseUp;
            // 
            // rights
            // 
            this.rights.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.rights.Dock = System.Windows.Forms.DockStyle.Right;
            this.rights.Location = new System.Drawing.Point(355, 4);
            this.rights.Name = "rights";
            this.rights.Size = new System.Drawing.Size(4, 306);
            this.rights.TabIndex = 7;
            this.rights.MouseDown += Rightpull_MouseDown;
            this.rights.MouseUp += rightpull_MouseUp;
            // 
            // tops
            // 
            this.tops.Dock = System.Windows.Forms.DockStyle.Top;
            this.tops.Location = new System.Drawing.Point(0, 0);
            this.tops.Name = "tops";
            this.tops.Size = new System.Drawing.Size(359, 4);
            this.tops.TabIndex = 8;
            // 
            // pullbs
            // 
            this.pullbs.Interval = 1;
            this.pullbs.Tick += pullbs_Tick;
            // 
            // pullbottom
            // 
            this.pullbottom.Interval = 1;
            this.pullbottom.Tick += pullbottom_Tick;
            // 
            // pullside
            // 
            this.pullside.Interval = 1;
            this.pullside.Tick += pullside_Tick;
            // 
            // timData
            // 
            this.timData.Enabled = true;
            this.timData.Interval = 90;
            // 
            // Shell
            // 
            this.ClientSize = new System.Drawing.Size(359, 310);
            this.Controls.Add(this.program);
            this.DoubleBuffered = true;
            this.BorderStyle = BorderStyle.None;
            this.Name = "Shell";
            this.Text = "templete";
            this.program.ResumeLayout(false);
            this.programtopbar.ResumeLayout(false);
            this.programtopbar.PerformLayout();
            this.ResumeLayout(false);
            base.Resize += resizing;
        }
        private void resizing(object sender, EventArgs e)
        {
            Resize(sender, e);
        }
        public new Boolean Focused
        {
            get
            {
                return foc;
            }
            set
            {
                if (foc != value){
                    foc = value;
                    Debug.WriteLine(Text + " focus has changed to " + foc);
                    themeRefresh();
                }
            }
        }
        public new event EventHandler Resize = new EventHandler((e, a) => { });
        public event EventHandler Closed = new EventHandler((e, a) => { });
        private Rectangle siz = new Rectangle();
        private static ConfigHelper themeConfig;
        private static String themeLocation = "";
        private Boolean foc = true;
     //   private string data = "";
       // private static List<string> strC;
     //   private static List<string> add; // , cmd New CommandPromptTextBox, outputProgramtopbar New Panel, outputClosebutton New PictureBox, outputProgram New Panel, outputProgramname New Label, outputTopleftcorner New Panel, outputBottomleftcorner New Panel, outputLefts New Panel, outputBottomrightcorner New Panel, outputBottoms New Panel, outputRights New Panel, outputTops New Panel, outputToprightcorner New Panel    
        // <DllImport("user32.dll")>
        // public Shared Function SetParent(ByVal hWndChild IntPtr, ByVal hWndNewParent IntPtr) IntPtr
        // End Function
        // <DllImport("user32.dll", SetLastError:=True)>
        // public Shared Function MoveWindow(ByVal hWnd IntPtr, ByVal X Integer, ByVal Y Integer, ByVal nWidth Integer, ByVal nHeight Integer, ByVal bRepaint Boolean) Boolean
        // End Function
        // <DllImport("user32.dll")>
        // Private Shared Function GetClientRect(ByVal HWND IntPtr, ByRef LPRECT Rectangle) Boolean
        // End Function
        // <DllImport("user32.dll", SetLastError:=True)>
        // Private Shared Function SetWindowPos(ByVal hWnd IntPtr, ByVal hWndInsertAfter IntPtr, ByVal X Integer, ByVal Y Integer, ByVal cx Integer, ByVal cy Integer, ByVal uFlags Integer) Boolean
        // End Function
        public bool moveable = true;
        public bool Minimized;
        public int mexlocation;
        public int meylocation;
        public int mewidth;
        public int meheight;
        public bool maximize;

        private void themeRefresh()
        {
            if (foc) programtopbar.BackColor = com.convertColour(com.Config.GetConfig("Colors","ActiveTitle").Setting); else programtopbar.BackColor = com.convertColour(com.Config.GetConfig("Colors", "InactiveTitle").Setting);
            minimizebutton.Image = com.prepareImage(themeLocation + themeConfig.GetConfig("ControlButtons","MinUp").Setting);
            minimizebutton.BackColor = com.convertColour(com.Config.GetConfig("Colors", "ButtonFace").Setting);
            maximizebutton.Image = com.prepareImage(themeLocation + themeConfig.GetConfig("ControlButtons", "MaxSmUp").Setting);//
            maximizebutton.BackColor = com.convertColour(com.Config.GetConfig("Colors","ButtonFace").Setting);
            closebutton.Image = com.prepareImage(themeLocation + themeConfig.GetConfig("ControlButtons", "CloseUp").Setting);
            closebutton.BackColor = com.convertColour(com.Config.GetConfig("Colors", "ButtonFace").Setting);
            lefts.BackgroundImage = com.prepareImage(themeLocation + themeConfig.GetConfig("Borders", "left").Setting);
            rights.BackgroundImage = com.prepareImage(themeLocation + themeConfig.GetConfig("Borders", "right").Setting);
            bottoms.BackgroundImage = com.prepareImage(themeLocation + themeConfig.GetConfig("Borders", "bottom").Setting);
            bottomrightcorner.BackgroundImage = com.prepareImage(themeLocation + themeConfig.GetConfig("Borders", "bottomRight").Setting);
            bottomleftcorner.BackgroundImage = com.prepareImage(themeLocation + themeConfig.GetConfig("Borders", "bottomLeft").Setting);
            topleftcorner.BackgroundImage = com.prepareImage(themeLocation + themeConfig.GetConfig("Borders", "topLeft").Setting);
            toprightcorner.BackgroundImage = com.prepareImage(themeLocation + themeConfig.GetConfig("Borders", "topRight").Setting);
        }
        private void pullside_Tick(System.Object sender, System.EventArgs e)
        {
            Width = Cursor.Position.X - Location.X;
        }
        private void pullbottom_Tick(System.Object sender, System.EventArgs e)
        {
            Height = Cursor.Position.Y - Location.Y;
        }
        private void pullbs_Tick(object sender, System.EventArgs e)
        {
            Width = Cursor.Position.X - Location.X;
            Height = Cursor.Position.Y - Location.Y;
        }
        private void Rightpull_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            pullside.Start();
        }
        private void rightpull_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            pullside.Stop();
        }
        private void bottompull_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            pullbottom.Start();
        }
        private void buttompull_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            pullbottom.Stop();
        }
        private void bspull_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            pullbs.Start();
        }
        private void bspull_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            pullbs.Stop();
        }
        private void programtopbar_MouseDown(Object sender, MouseEventArgs e) // , programname.MouseDown, programIcon.MouseDown ', outputProgramtopbar.MouseDown
        {            
            if (e.Button == MouseButtons.Left){
                FocusChanged?.Invoke(id, e);
                ((Control)sender).Capture = false;
                const int WM_NCLBUTTONDOWN = 0xA1;
                const int HTCAPTION = 2; // Console.WriteLine(sender.Parent.Parent.Name)
                Message msg = Message.Create(((Control)sender).Parent.Parent.Handle, WM_NCLBUTTONDOWN, new IntPtr(HTCAPTION), IntPtr.Zero); // 
                this.DefWndProc(ref msg);
            }
        }
        private void minimizebutton_Click(Object sender, EventArgs e)
        {
            this.Visible = false;
        }
        public event FormClosingEventHandler Closing;
        public event EventHandler FocusChanged;
        private void closebutton_Click(object sender, EventArgs e) // , outputClosebutton.Click, pro.Exited
        {
            Close(CloseReason.UserClosing);
        }
        public void Close(CloseReason reason)
        {
            Closing?.Invoke(this, new FormClosingEventArgs(reason, false));
            Dispose();
        }
        private void maximizebutton_Click(System.Object sender, System.EventArgs e)
        {

            Maximise.Invoke(sender, e);
            if (maximize)
                ((Button)sender).Image = com.prepareImage(themeLocation + themeConfig.GetConfig("ControlButtons", "MaxLgUp").Setting);
            else
                ((Button)sender).Image = com.prepareImage(themeLocation + themeConfig.GetConfig("ControlButtons", "MaxSmUp").Setting);
        }
        public event EventHandler Maximise;

        private void Windows_9X_Shell_ResizeEnd(object sender, EventArgs e)
        {
            // If Height < programtopbar.Height + 10 Then Exit Sub Else RaiseEvent Resize(sender, e)
            Resize?.Invoke(sender, e);
        }
        private void com_ConfigUpdate()
        {
            themeRefresh();
        }
        private System.ComponentModel.IContainer components;
        public override String Text { get { return programname.Text; } set { programname.Text = value; } }
        public DialogResult DialogResult { get; set; }
        [SecurityPermissionAttribute(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public DialogResult ShowDialog()
        {
            EnableWindow(this.Handle, false);
            //System.Windows.Threading.DispatcherFrame
            //DispatcherFrame frame = new DispatcherFrame();

            this.Closed += delegate
            {
                EnableWindow(this.Handle, true);
                //frame.Continue = false;
            };

            Show();
            //Dispatcher.PushFrame(frame);
            return DialogResult;
        }
        private Timer pullbs;
        private Timer pullbottom;
        private Timer pullside;
        private Timer timData;
        private Panel program;
        public Panel programtopbar;
        public Button maximizebutton;
        public Button minimizebutton;
        public Label programname;
        public Button closebutton;
        public Panel topleftcorner;
        public Panel bottomleftcorner;
        public Panel lefts;
        public Panel bottomrightcorner;
        public Panel bottoms;
        public Panel toprightcorner;
        public Panel rights;
        public Panel tops;
        public PictureBox programIcon;
        public Panel GuestArea;
        [DllImport("user32")]
        internal static extern bool EnableWindow(IntPtr hwnd, bool bEnable);
    }
}