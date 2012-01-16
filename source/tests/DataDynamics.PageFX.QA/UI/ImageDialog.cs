using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace DataDynamics.UI
{
    public partial class ImageDialog : Form
    {
        public ImageDialog()
        {
            InitializeComponent();
        }

        public void SetImage(string filename)
        {
        	var thread = new Thread(SetImageIntern) {IsBackground = true};
        	thread.Start(filename);
        }

        private void SetImageIntern(object filename)
        {
            imageViewerFull.Image = Image.FromFile((string)filename);
            imageViewerFull.Invalidate();
        }

        private void ImageDialog_Resize(object sender, EventArgs e)
        {
            imageViewerFull.Invalidate();
        }
    }
}