using System.Windows.Forms;

namespace DataDynamics.UI
{
    public partial class ThumbnailViewer : UserControl
    {
        public ThumbnailViewer()
        {
            InitializeComponent();

            m_AddImageDelegate = AddImage;
        }

        public int ImageSize
        {
            get { return _imageSize; }
            set
            {
                if (value != _imageSize)
                {
                    _imageSize = value;
                    Invalidate();
                }
            }
        }
        private int _imageSize = 128;

        public void AddFolder(string path)
        {
            imageLoader.AddFolder(path);
        }

        public void Clear()
        {
            flowPanel.Controls.Clear();
        }

        private delegate void DelegateAddImage(string imageFilename);

        private readonly DelegateAddImage m_AddImageDelegate;

        public void AddImage(string path)
        {
            // thread safe
            if (InvokeRequired)
            {
                Invoke(m_AddImageDelegate, path);
            }
            else
            {
                int size = ImageSize;

                var imageViewer = new ImageViewer();
                imageViewer.Dock = DockStyle.Bottom;
                imageViewer.LoadImage(path, 256, 256);
                imageViewer.Width = size;
                imageViewer.Height = size;
                imageViewer.IsThumbnail = true;
                
                if (ImageAdded != null)
                    ImageAdded(this, imageViewer);

                flowPanel.Controls.Add(imageViewer);
            }
        }

        public event AddEventHandler<ImageViewer> ImageAdded;

        private void imageLoader_AddImage(object sender, ThumbnailControllerEventArgs e)
        {
            AddImage(e.ImageFilename);
            Invalidate();
        }

        private void imageLoader_Start(object sender, ThumbnailControllerEventArgs e)
        {
        }

        private void imageLoader_End(object sender, ThumbnailControllerEventArgs e)
        {
        }
    }

    public delegate void AddEventHandler<T>(object sender, T obj);
}