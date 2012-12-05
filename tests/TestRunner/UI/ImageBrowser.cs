using System.Windows.Forms;

namespace DataDynamics.PageFX.TestRunner.UI
{
    public partial class ImageBrowser : UserControl
    {
        public ImageBrowser()
        {
            InitializeComponent();
        }

        public void AddFolder(string path)
        {
            thumbViewer.AddFolder(path);
        }

        public void Clear()
        {
            thumbViewer.Clear();
        }

        private void OnImageAdded(object sender, ImageViewer iv)
        {
            iv.MouseClick += ImageViewer_MouseClick;
        }

        private void ImageViewer_MouseClick(object sender, MouseEventArgs e)
        {
        }
    }
}
