using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DataDynamics.UI
{
    public partial class ImageViewer : UserControl
    {
        private Image m_Image;
        private string m_ImageLocation;

        private bool m_IsThumbnail;
        private bool m_IsActive;

        public ImageViewer()
        {
            m_IsThumbnail = false;
            m_IsActive = false;

            InitializeComponent();
        }

        public Image Image
        {
            set { m_Image = value; }
            get { return m_Image; }
        }

        public string ImageLocation
        {
            set { m_ImageLocation = value; }
            get { return m_ImageLocation; }
        }

        public bool IsActive
        {
            set
            {
                m_IsActive = value;
                Invalidate();
            }
            get { return m_IsActive; }
        }

        public bool IsThumbnail
        {
            set { m_IsThumbnail = value; }
            get { return m_IsThumbnail; }
        }

        public void SetSizeChanged(int width, int height)
        {
            Width = width;
            Height = height;
            Invalidate();
        }

        public void LoadImage(string imageFilename, int width, int height)
        {
            var tempImage = Image.FromFile(imageFilename);
            m_ImageLocation = imageFilename;

            int dw = tempImage.Width;
            int dh = tempImage.Height;
            int tw = width;
            int th = height;
            double zw = (tw / (double)dw);
            double zh = (th / (double)dh);
            double z = (zw <= zh) ? zw : zh;
            dw = (int)(dw * z);
            dh = (int)(dh * z);

            m_Image = new Bitmap(dw, dh);
            var g = Graphics.FromImage(m_Image);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(tempImage, 0, 0, dw, dh);
            g.Dispose();

            tempImage.Dispose();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
        	if (m_Image == null) return;

            int dw = m_Image.Width;
            int dh = m_Image.Height;
            int tw = Width - 8; // remove border, 4*4 
            int th = Height - 8; // remove border, 4*4 
            double zw = (tw / (double)dw);
            double zh = (th / (double)dh);
            double z = (zw <= zh) ? zw : zh;

            dw = (int)(dw * z);
            dh = (int)(dh * z);
            int dl = 4 + (tw - dw) / 2; // add border 2*2
            int dt = 4 + (th - dh) / 2; // add border 2*2

            g.DrawRectangle(new Pen(Color.Gray), dl, dt, dw, dh);

            if (m_IsThumbnail)
                for (int j = 0; j < 3; j++)
                {
                    g.DrawLine(new Pen(Color.DarkGray),
                               new Point(dl + 3, dt + dh + 1 + j),
                               new Point(dl + dw + 3, dt + dh + 1 + j));
                    g.DrawLine(new Pen(Color.DarkGray),
                               new Point(dl + dw + 1 + j, dt + 3),
                               new Point(dl + dw + 1 + j, dt + dh + 3));
                }

            g.DrawImage(m_Image, dl, dt, dw, dh);

            if (m_IsActive)
            {
                g.DrawRectangle(new Pen(Color.White, 1), dl, dt, dw, dh);
                g.DrawRectangle(new Pen(Color.Blue, 2), dl - 2, dt - 2, dw + 4, dh + 4);
            }
        }
    }
}