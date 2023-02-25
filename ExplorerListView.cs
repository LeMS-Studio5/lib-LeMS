using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
namespace libLeMS{
    public class ExplorerListView : ListView{
        public Bitmap addImage(string strPath, string strCurrentDir){
            string strFilPath = strPath;
            try
            {
                if (OSIco != null)
                {
                    if (System.IO.Directory.Exists(strPath) == true && (new DirectoryInfo(strPath)).Name.EndsWith("‰ƒ"))//DrivePath
                        return DriveIcon(strPath);
                    if (System.IO.Directory.Exists(strPath) == true && strPath == strCurrentDir)//Open Directory
                        return com.prepareImage(Bitmap.FromHicon(OSIco[68].Handle), FollowPallet);
                    if (System.IO.Directory.Exists(strPath) == true && strPath != strCurrentDir)//Directory
                        return com.prepareImage(Bitmap.FromHicon(OSIco[69].Handle), FollowPallet);
                    Int32 intIconIndex;
                    if (strPath.Contains(',') && Int32.TryParse(strPath.Split(',').Last(), out intIconIndex))//Icon Extraction
                        return Bitmap.FromHicon(TsudaKageyu.IconUtil.Split(new Icon(strPath.Split(',')[0]))[intIconIndex].Handle);//, true);
                    if (!System.IO.File.Exists(strPath))//Error
                        return com.prepareImage(Bitmap.FromHicon(OSIco[52].Handle), FollowPallet);
                    if (strPath.EndsWith("lnk", true, System.Globalization.CultureInfo.CurrentUICulture))
                    {
                        Bitmap ink = new Bitmap(1, 1);
                        Graphics g = Graphics.FromImage(ink);
                        ShortCut lnk = new ShortCut(strPath);
                        if (ink.Width + ink.Height == 2)
                            ink = new Bitmap(addImage(com.toSystemPath(lnk.TargetFile), ""), 32, 32);
                        if (!(File.ReadAllLines(strFilPath).Count() >= 5 && File.ReadAllLines(strFilPath)[4] == "Sys"))
                            g.DrawImage(Bitmap.FromHicon(OSIco[29].Handle), 0, ink.Height - OSIco[29].Height);
                        return com.prepareImage(ink, FollowPallet);
                    }
                    if (strPath.EndsWith("U95exe", true, System.Globalization.CultureInfo.CurrentUICulture) || strPath.EndsWith("U95com", true, System.Globalization.CultureInfo.CurrentUICulture))
                        return null;// Properties.Resources.Attention;
                    if ((ViewType == ExplorerType.ControlPanel && strPath.EndsWith("cpl", true, System.Globalization.CultureInfo.CurrentUICulture)) || strPath.EndsWith("exe", true, System.Globalization.CultureInfo.CurrentUICulture))
                    {
                        TsudaKageyu.IconExtractor i = new TsudaKageyu.IconExtractor(strPath); // addImage(CDrivePath & File.ReadAllLines(strPath).First, "")
                                                                                              // MessageBox.Show(strPath)
                        if (i.Count > 0)
                            return com.prepareImage(i.GetIcon(i.Count - 1).ToBitmap(), FollowPallet);
                        else
                            return com.prepareImage(Bitmap.FromHicon(OSIco[70].Handle), FollowPallet);  // Return setSetting.OSIco(Int(ReadAllLines(strPath).Last)).ToBitmap
                    }
                    if (strPath.EndsWith(".U95ico", true, System.Globalization.CultureInfo.CurrentUICulture))
                        return com.prepareImage(strPath);
                    if (strPath.EndsWith(".txt", true, System.Globalization.CultureInfo.CurrentUICulture))
                        return com.prepareImage(Bitmap.FromHicon(OSIco[42].Handle), FollowPallet);
                    if (strPath.EndsWith(".msi", true, System.Globalization.CultureInfo.CurrentUICulture))
                        return com.prepareImage(Bitmap.FromHicon(OSIco[46].Handle), FollowPallet);
                    return com.prepareImage(Bitmap.FromHicon(OSIco[0].Handle), FollowPallet);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            if (strFilPath.EndsWith(".jpg",StringComparison.CurrentCultureIgnoreCase)) return ScaleImage(strFilPath,48,48);
            if ((OSIco == null) || OSIco.Count() < 1)
            {
                return (Bitmap)FileIcon.GetLargeIcon(strFilPath);
            }
            else return null;
        }
        public static Bitmap ScaleImage(String filePath, int maxWidth, int maxHeight)
        {
            return ScaleImage(new Bitmap(filePath), maxWidth, maxHeight);
        }
        public static Bitmap ScaleImage(Bitmap image, int maxWidth, int maxHeight)
        {
            double ratioX = (double)maxWidth / image.Width;
            double ratioY = (double)maxHeight / image.Height;
            double ratio = Math.Min(ratioX, ratioY);

            int newWidth = (int)(image.Width * ratio);
            int newHeight = (int)(image.Height * ratio);

            Bitmap newImage = new Bitmap(maxWidth, maxHeight);
            using (Graphics graphics = Graphics.FromImage(newImage))
            {
                // Calculate x and y which center the image
                int y = (maxHeight / 2) - newHeight / 2;
                int x = (maxWidth / 2) - newWidth / 2;

                // Draw image on x and y with newWidth and newHeight
                graphics.DrawImage(image, x, y, newWidth, newHeight);
            }
            System.GC.Collect();
            return newImage;
        }

        public Boolean AutoDispose = false;
        public bool AutoRefreshFolder { get; set; } = true;
        public new Color BackColor { get { return base.BackColor; } set { base.BackColor = value; if (upDesk) RefreshImage("col"); } }
        public new System.Drawing.Image BackgroundImage
        {
            get
            {
                return base.BackgroundImage;
            }
        }
        private MasterClass com;
        private System.ComponentModel.IContainer components;
        private string dir = "";
        public string Directory
        {
            set
            {
                if (System.IO.Directory.Exists(value))
                {
                    if (!value.EndsWith(@"\"))
                        value += @"\";
                    if (value != dir)
                    {
                        dir = value;
                        if ( AutoRefreshFolder)
                            RefreshFolder();
                    }
                }
                else
                {
                    base.Items.Clear();
                    dir = "";
                }
            }
            get
            {
                return dir;
            }
        }
        public DirectoryInfo DirInfo()
        {
            if (dir != null)
                return new DirectoryInfo(dir);
            else
                return null;
        }
        private DisplayType display = DisplayType.DirectoriesAndFiles;
        public DisplayType DisplayMode
        {
            set
            {
                display = value;
                if (AutoRefreshFolder)
                    RefreshFolder();
            }
            get
            {
                return display;
            }
        }
        public enum DisplayType
        {
            Directories,
            DirectoriesAndFiles,
            Files
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
        private Bitmap DriveIcon(string path)
        {
            if (File.ReadAllLines(path + @"\(_)drive.info")[0] == "HD")
                return OSIco[8].ToBitmap();
            if (File.ReadAllLines(path + @"\(_)drive.info")[0] == "flp")
                return OSIco[6].ToBitmap();
            if (File.ReadAllLines(path + @"\(_)drive.info")[0] == "cd" || File.ReadAllLines(path + @"\(_)drive.info")[0] == "dvd")
                return OSIco[11].ToBitmap();
            return null;// Properties.Resources.Error95;
        }
        public enum ExplorerType
        {
            Desktop,
            ControlPanel,
            General,
            Unlaunchable
        }
        public SizeType FileSizeType { set; get; } = SizeType.Bytes;
        private String filt = "*.*";
        public String Filter
        {
            set
            {
                filt = value;
                if (AutoRefreshFolder)
                    RefreshFolder();
            }
            get
            {
                return filt;
            }
        }
        public Boolean FollowPallet { get; set; }
        private ConfigHelper foldOptions;
        public String FormatFileName(String filePath)
        {
            if (File.Exists(filePath))
            {
                if (ViewType == ExplorerType.ControlPanel)
                    return System.Diagnostics.FileVersionInfo.GetVersionInfo(filePath).ProductName;
                else
                {
                   if (com==null||(foldOptions != null && Boolean.Parse(foldOptions.GetConfig("View", "HideDosExt").Setting))) return System.IO.Path.GetFileName(filePath); else return System.IO.Path.GetFileNameWithoutExtension(filePath);
                }
            }
            else if (System.IO.Directory.Exists(filePath))
            {
                return Path.GetDirectoryName(filePath).Replace("‰", ":").Replace("ƒ", @"\");
            }
            throw new Exception(filePath + " could not be found");
        }
        private void fswExplorer_Changed(object sender, FileSystemEventArgs e)
        {
            if (!System.IO.Directory.Exists(dir) && AutoDispose)   this.Dispose();
            if (!System.IO.Directory.Exists(dir) && !AutoDispose) UpDirectory();
            if (AutoRefreshFolder)
                RefreshFolder();
        }
        public ExplorerListView(String mode) : this() 
        {
            com = new MasterClass();
            foldOptions = new ConfigHelper(com.toSystemPath(com.Config.GetConfig("Explorer", "ConfigLoc").Setting));
            //pipeServer = new NamedPipeServerStream("ProjectI2padamsNet");
            elvMode = mode;
        }
        public ExplorerListView() : base()
        {
            this.MouseDoubleClick += thisDoubleClick;
        }
        private  void thisDoubleClick(Object sender, EventArgs e)
        {
            if (elvMode == "StandAlone" && FocusedItem != null && File.Exists(FocusedItem.Tag.ToString())) FileOpened?.Invoke(FocusedItem.Tag.ToString());
        }
        public event FileOpenedHandler FileOpened;
        public delegate void FileOpenedHandler(String filePath);
        private String elvMode { get; set; }= "StandAlone";
        public bool OnErrorGoToParentDirectory { get; set; } = false;
        private NamedPipeServerStream pipeServer = new NamedPipeServerStream("ProjectI2padamsNet",PipeDirection.InOut, NamedPipeServerStream.MaxAllowedServerInstances);     // Based on code from https://docs.microsoft.com/en-us/dotnet/standard/io/how-to-use-anonymous-pipes-for-local-interprocess-communication
        private StreamWriter sw;
        public void OpenFile(String filPath)
        {
            //Debug.WriteLine(filPath);
            sw = new StreamWriter(pipeServer);
            if (!pipeServer.IsConnected) pipeServer.WaitForConnection();
            sw.AutoFlush = true;
            pipeServer.WaitForPipeDrain();
            sw.WriteLine(filPath);
        }
        private Icon[] OSIco;
        private string OSPath;
        public string OSIconLocationPath
        {
            set
            {
                if (System.IO.File.Exists(value))
                {
                    OSIco = TsudaKageyu.IconUtil.Split(new Icon(value)); // i.GetAllIcons
                    OSPath = value;
                }
                else
                {
                    OSIco = null;
                    OSPath = "";
                }
                if (AutoRefreshFolder)
                    RefreshFolder();
            }
            get
            {
                return OSPath;
            }
        }
        [System.Runtime.InteropServices.DllImport("shlwapi")]
        public static extern long PathIsDirectory(string pszPath); // return 16 for local folders and 1 for server folders
        private Bitmap pat=new Bitmap(1,1);
        public Bitmap Pattern { get { return pat; } set {pat = value; if (upDesk) RefreshImage("pat"); } }
        public void RefreshFolder()
        {
            try
            {
                if ((Items != null)) Items.Clear();
                if (SmallImageList != null) SmallImageList.Images.Clear();
                if (LargeImageList != null) LargeImageList.Images.Clear();
                if (this.DesignMode) return;//System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime

                //MessageBox.Show(this.DesignMode.ToString());
                if (System.IO.Directory.Exists(dir))
                {
                    if (display == DisplayType.Directories || display == DisplayType.DirectoriesAndFiles)
                    {
                        foreach (string fil in System.IO.Directory.GetDirectories(dir).OrderBy(f => f)) // Get Files In Folder
                        {                           
                            try
                            {
                                SmallImageList.Images.Add(fil, addImage(fil, dir));
                                LargeImageList.Images.Add(fil, addImage(fil, dir));
                                System.GC.Collect();
                                Int64 intTotal = 0;
                                foreach (var SizeFile in System.IO.Directory.GetFiles(fil, "*.*", System.IO.SearchOption.AllDirectories))
                                {
                                    intTotal += (new FileInfo(SizeFile)).Length;
                                }
                                Items.Add(FormatFileName(fil), fil); // Add Files & File Properties To ListView
                                Items[Items.Count - 1].ImageKey = fil;
                                Items[Items.Count - 1].ImageIndex = Items.Count - 1;
                                Items[Items.Count - 1].SubItems.Add((intTotal / FileSizeType.GetHashCode()) + " " + FileSizeType.ToString());
                                Items[Items.Count - 1].SubItems.Add("File Folder");
                                Items[Items.Count - 1].Tag = fil;
                                Items[Items.Count - 1].SubItems.Add(System.IO.Directory.GetLastWriteTime(fil).ToString());
                                intTotal = 0;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.ToString());
                            }
                        }
                    }
                    if ((display == DisplayType.DirectoriesAndFiles || display == DisplayType.Files) && System.IO.Directory.Exists(dir))
                    {
                        foreach (string fil in System.IO.Directory.GetFiles(dir, filt).OrderBy(f=>f)) // Get Files In Folder
                        {
                           
                            if (fil != null && !System.IO.Path.GetFileName(fil).StartsWith("(_)"))
                            {
                                SmallImageList.Images.Add(fil, addImage(fil, dir));
                                LargeImageList.Images.Add(fil, addImage(fil, dir));
                                System.GC.Collect();
                                Items.Add(FormatFileName(fil), fil);
                                Items[Items.Count - 1].SubItems.Add(((new FileInfo(fil)).Length / FileSizeType.GetHashCode()) + " " + FileSizeType.ToString());
                                Items[Items.Count - 1].Tag = fil;
                                Items[Items.Count - 1].ImageKey = fil;
                                //Items[Items.Count - 1].ImageIndex = Items.Count - 1;
                                Items[Items.Count - 1].SubItems.Add(System.IO.File.GetLastWriteTime(fil).ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                if (OnErrorGoToParentDirectory)
                    UpDirectory();
            }
        }
        public void RefreshFolder(ref string Dir)
        {
            Directory = Dir;
        }
        private void RefreshImage(String source)
        {
            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime) return;
            if (wallMode == ImageLayout.Tile)
            {
                base.BackgroundImage = com.prepareImage((Bitmap)Wallpaper, true);//base.BackColor
                base.BackgroundImageTiled = true;
            }
            else
            {
            Console.WriteLine(wallMode + source);
                base.BackgroundImageTiled = false;
                base.BackgroundImage = updateBackground(ref pat,ref wall,wallMode,base.Size, BackColor);
            }
        }
        public String Root
        {
            set {
               if (value == "" || value ==null)
                    value = System.IO.Path.GetPathRoot(Application.ExecutablePath);
                if (!value.EndsWith(@"\"))
                    value += @"\";
                rooted = value;
                if (AutoRefreshFolder) RefreshFolder();
            }
            get
            {
                return rooted;
            }
        }
        private String rooted;
        public struct SHFILEINFO // Contains information about a file object
        {
            public IntPtr hIcon;            // Icon
            public int iIcon;           // Icondex
            public int dwAttributes;    // SFGAO_ flags
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }
        // The signature of SHGetFileInfo (located in Shell32.dll)
        [DllImport("Shell32.dll")]
        public static extern int SHGetFileInfo(string pszPath, int dwFileAttributes, ref SHFILEINFO psfi, int cbFileInfo, uint uFlags);

        [DllImport("Shell32.dll")]
        public static extern int SHGetFileInfo(IntPtr pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, int cbFileInfo, uint uFlags);
        // Retrieves information about an object in the file system, such as a file, folder, directory, or drive root
        public enum SizeType
        {
            Bytes = 1,
            KiloBytes = 1024,
            MegaBytes = 1048576,
            GigaBytes = 1073741824
        }
        public static Bitmap updateBackground(ref Bitmap imgPattern, ref Bitmap imgWallpaper, ImageLayout wallpaperMode, Size imgSize, Color backgroundCol)
        {
            Bitmap bmp = new Bitmap(imgSize.Width, imgSize.Height);
            int imgH = imgWallpaper.Height, imgW = imgWallpaper.Width;
            Graphics g = Graphics.FromImage(bmp);
            g.FillRectangle(new SolidBrush(backgroundCol), 0, 0, imgSize.Width, imgSize.Height);
            if (wallpaperMode == ImageLayout.Center) g.FillRectangle(new TextureBrush(imgPattern), new Rectangle(0, 0, imgSize.Width, imgSize.Height));
                if (wallpaperMode == ImageLayout.Center)  g.DrawImage(imgWallpaper, new Rectangle(Convert.ToInt32((imgSize.Width / (double)2) - (imgW / (double)2)), Convert.ToInt32((imgSize.Height / (double)2) - (imgH / (double)2)),imgW,imgH));
            return bmp;
        }
        public Boolean UpdateDesktop { get { return upDesk; } set { upDesk = value; if (value) RefreshImage("Update"); } }
        private Boolean upDesk = false;
        public void UpDirectory()
        {
            if (dir != rooted)
                Directory = new DirectoryInfo(dir).Parent.FullName;
            else
                AutoRefreshFolder = false;
        }
        public ExplorerType ViewType { get; set; } = ExplorerType.General;
        private Bitmap wall = new Bitmap(1, 1);
        public Bitmap Wallpaper { get { return wall; } set {wall = value; if (upDesk) RefreshImage("Wall"); } }
        public ImageLayout WallpaperLayout { get { return wallMode; } set { wallMode = value; if (upDesk) RefreshImage("WallLayout"); } }
        private ImageLayout wallMode = ImageLayout.None;
    }   

    public class ShortCut
    {  
        private IWshRuntimeLibrary.IWshShortcut lnk;
        private ConfigHelper conLNK;
        public ShortCut(String lnkLocation)
        {
            if (!File.Exists(lnkLocation)) return;
            IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();  //Based on https://stackoverflow.com/questions/234231/creating-application-shortcut-in-a-directory/28417360#28417360
            if (IsSymbolic(lnkLocation))
            {
                Console.WriteLine(lnkLocation);
                lnk = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(lnkLocation);
                lType = lnkType.Windows;

            }
            else
            {
                conLNK = new ConfigHelper(lnkLocation);
                lType = lnkType.ProjectI2padams;
            }
        }
        private bool IsSymbolic(string path)    //Code from https://stackoverflow.com/questions/1485155/check-if-a-file-is-real-or-a-symbolic-link/26473940#26473940
        {
            FileInfo pathInfo = new FileInfo(path);
            return pathInfo.Attributes.HasFlag(FileAttributes.ReparsePoint);
        }
        public String TargetFile
        {
            set
            {
                if (lType == lnkType.Windows) { lnk.TargetPath = value; Save(); } else conLNK.SetConfig("Shortcut", "Target", value);

            }
            get
            {
                if (lType == lnkType.Windows) return lnk.TargetPath; else return conLNK.GetConfig("Shortcut", "Target").Setting;
            }
        }
        public void Save()
        {
            if (lType == lnkType.Windows) lnk.Save();
        }
        private lnkType lType;
        public enum lnkType
        {
            ProjectI2padams, Windows
        }
        public lnkType ShortcutType { get { return lType; } }

    }
}