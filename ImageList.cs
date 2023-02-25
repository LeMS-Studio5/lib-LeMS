using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace libLeMS
{
    public class ImageList
    {
        public ImageList()
        {
            
        }
        private bool disposedValue = false; // To detect redundant calls
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    /*System.Windows.Forms.ImageList.ImageCollection i = new System.Windows.Forms.ImageList.ImageCollection();
                    */
                }

                // TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                // TODO: set large fields to null.
                Images.Clear();
            }
            this.disposedValue = true;
        }
        // TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
        // Protected Overrides Sub Finalize()
        // ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        // Dispose(False)
        // MyBase.Finalize()
        // End Sub

        // This code added by Visual Basic to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public ImageAlbum Images { get; } = new ImageAlbum();
    }
    public class ImageAlbum
    {
        private Dictionary<String, Image> ilst = new Dictionary<string, Image>();
        private Dictionary<Int32, String> indexRef = new Dictionary<Int32, String>();
        private Int32 nextIndex = 0;
        public void Add(Image img)
        {
        }
        public void Add(string fil, Image img)
        {
            ilst.Add(fil,img);
            indexRef.Add(nextIndex, fil);
            nextIndex++;
        }
        public void Clear()
        {
            ilst.Clear();
            indexRef.Clear();
        }
        public int Count()
        {
            return ilst.Count;
        }
        public Boolean ContainsKey(String key)
        {
            return ilst.ContainsKey(key);
        }
        public Boolean Empty()
        {
            return (ilst.Count == 0);
        }
        public Image Get(int index)
        {
            return ilst[indexRef[index]];
        }
        public void RemoveAt(int index)
        {
            ilst.Remove(indexRef[index]);
            indexRef.Remove(index);
        }
    }
}
