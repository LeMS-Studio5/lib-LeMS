using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace libProChic
{
    public class ExplorerTreeView : System.Windows.Forms.TreeView
    {
        public ExplorerTreeView()
        {
            ImageList = new System.Windows.Forms.ImageList();
        }
        private String dRoot = "";
        public String DirectoryRoot
        {
            get
            {
                return dRoot;
            }
            set
            {
                this.Nodes.Clear();
                dRoot = value;
                if (!this.DesignMode) BuildTreeFromPathData(dRoot, this.Nodes.Add(dRoot));
            }
        }
        public Boolean BuildTreeFromPathData(String RootPath, TreeNode NodeParent)
        {
            try
            {
                    NodeParent.Tag = RootPath;
                foreach (string fil in System.IO.Directory.GetDirectories(RootPath).OrderBy(f => f))
                {                    
                    ImageList.Images.Add(fil, FileIcon.GetLargeIcon(fil));
                    System.GC.Collect();
                    NodeParent.Nodes.Add( fil, new DirectoryInfo(fil).Name);
                     BuildTreeFromPathData(fil, NodeParent.LastNode);
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                //MessageBox.Show(ex.ToString() + "Build");
                return false;
            }
        }
    }
    public static class FileIcon
    {
        [DllImport("shell32.dll")]
        private static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);
        [StructLayout(LayoutKind.Sequential)]
        private struct SHFILEINFO
        {
            public IntPtr hIcon;
            public IntPtr iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        };
        private const uint SHGFI_ICON = 0x100;
        private const uint SHGFI_LARGEICON = 0x0; // 'Large icon
        private const uint SHGFI_SMALLICON = 0x1; // 'Small icon

        public static System.Drawing.Image GetLargeIcon(string file)
        {
            FileIcon.SHFILEINFO shinfo = new FileIcon.SHFILEINFO();
            IntPtr hImgLarge = FileIcon.SHGetFileInfo(file, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), FileIcon.SHGFI_ICON | FileIcon.SHGFI_LARGEICON);
            return System.Drawing.Icon.FromHandle(shinfo.hIcon).ToBitmap();
        }

        public static System.Drawing.Image GetSmallIcon(string file)
        {
            FileIcon.SHFILEINFO shinfo = new FileIcon.SHFILEINFO();
            IntPtr hImgLarge = FileIcon.SHGetFileInfo(file, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), FileIcon.SHGFI_ICON | FileIcon.SHGFI_SMALLICON);
            return System.Drawing.Icon.FromHandle(shinfo.hIcon).ToBitmap();
        }
    }

}
