using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;

namespace DataDynamics.PageFX.TestRunner.UI
{
    [ToolboxItem(true)]
    public class ImageLoader : Component
    {
        private bool m_CancelScanning;
        private static readonly object _lock = new object();

        public bool CancelScanning
        {
            get
            {
                lock (_lock)
                {
                    return m_CancelScanning;
                }
            }
            set
            {
                lock (_lock)
                {
                    m_CancelScanning = value;
                }
            }
        }

        public event ThumbnailControllerEventHandler Start;
        public event ThumbnailControllerEventHandler AddImage;
        public event ThumbnailControllerEventHandler End;

        public void AddFolder(string path)
        {
            CancelScanning = false;

        	var thread = new Thread(AddFolder) {IsBackground = true};
        	thread.Start(path);
        }

        private void AddFolder(object folderPath)
        {
            string path = (string)folderPath;

            if (Start != null)
            {
                Start(this, new ThumbnailControllerEventArgs(null));
            }

            AddFolderInternal(path);

            if (End != null)
            {
                End(this, new ThumbnailControllerEventArgs(null));
            }

            CancelScanning = false;
        }

        private void AddFolderInternal(string path)
        {
            if (CancelScanning) return;

            var files = Directory.GetFiles(path, "*.bmp;*.png;*.jpg;*.jpeg");
            foreach (var file in files)
            {
                if (CancelScanning) break;

                Image img = null;

                try
                {
                    img = Image.FromFile(file);
                }
                catch
                {
                    // do nothing
                }

                if (img != null)
                {
                    AddImage(this, new ThumbnailControllerEventArgs(file));

                    img.Dispose();
                }
            }

            var dirs = Directory.GetDirectories(path);
            foreach (var dir in dirs)
            {
                if (CancelScanning) break;

                AddFolderInternal(dir);
            }
        }
    }

    public class ThumbnailControllerEventArgs : EventArgs
    {
        public ThumbnailControllerEventArgs(string imageFilename)
        {
            ImageFilename = imageFilename;
        }

        public string ImageFilename;
    }

    public delegate void ThumbnailControllerEventHandler(object sender, ThumbnailControllerEventArgs e);
}