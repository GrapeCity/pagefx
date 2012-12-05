namespace DataDynamics.PageFX.TestRunner.UI
{
    partial class ThumbnailViewer
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.flowPanel = new ThumbnailFlowLayoutPanel();
            this.imageLoader = new ImageLoader();
            this.SuspendLayout();
            // 
            // flowPanel
            // 
            this.flowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowPanel.Location = new System.Drawing.Point(0, 0);
            this.flowPanel.Name = "flowPanel";
            this.flowPanel.Size = new System.Drawing.Size(430, 392);
            this.flowPanel.TabIndex = 0;
            // 
            // imageLoader
            // 
            this.imageLoader.CancelScanning = false;
            this.imageLoader.Start += new ThumbnailControllerEventHandler(this.imageLoader_Start);
            this.imageLoader.End += new ThumbnailControllerEventHandler(this.imageLoader_End);
            this.imageLoader.AddImage += new ThumbnailControllerEventHandler(this.imageLoader_AddImage);
            // 
            // ThumbnailViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.flowPanel);
            this.Name = "ThumbnailViewer";
            this.Size = new System.Drawing.Size(430, 392);
            this.ResumeLayout(false);

        }

        #endregion

        private ThumbnailFlowLayoutPanel flowPanel;
        private ImageLoader imageLoader;
    }
}
