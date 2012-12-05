namespace DataDynamics.PageFX.TestRunner.UI
{
    partial class ImageDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.imageViewerFull = new ImageViewer();
            this.SuspendLayout();
            // 
            // imageViewerFull
            // 
            this.imageViewerFull.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.imageViewerFull.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.imageViewerFull.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageViewerFull.Image = null;
            this.imageViewerFull.ImageLocation = null;
            this.imageViewerFull.IsActive = false;
            this.imageViewerFull.Location = new System.Drawing.Point(0, 0);
            this.imageViewerFull.Name = "imageViewerFull";
            this.imageViewerFull.Size = new System.Drawing.Size(440, 378);
            this.imageViewerFull.TabIndex = 0;
            // 
            // ImageDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 378);
            this.Controls.Add(this.imageViewerFull);
            this.Name = "ImageDialog";
            this.ShowIcon = false;
            this.Text = "Image Viewer";
            this.Resize += new System.EventHandler(this.ImageDialog_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private ImageViewer imageViewerFull;
    }
}