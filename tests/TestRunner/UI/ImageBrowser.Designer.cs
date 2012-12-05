namespace DataDynamics.PageFX.TestRunner.UI
{
    partial class ImageBrowser
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
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.statusbar = new System.Windows.Forms.StatusStrip();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.toolbar = new System.Windows.Forms.ToolStrip();
            this.thumbViewer = new ThumbnailViewer();
            this.imageViewer = new ImageViewer();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.statusbar);
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.splitMain);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(640, 553);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(640, 600);
            this.toolStripContainer1.TabIndex = 0;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolbar);
            // 
            // statusbar
            // 
            this.statusbar.Dock = System.Windows.Forms.DockStyle.None;
            this.statusbar.Location = new System.Drawing.Point(0, 0);
            this.statusbar.Name = "statusbar";
            this.statusbar.Size = new System.Drawing.Size(640, 22);
            this.statusbar.TabIndex = 0;
            // 
            // splitMain
            // 
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.Location = new System.Drawing.Point(0, 0);
            this.splitMain.Name = "splitMain";
            this.splitMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitMain.Panel1
            // 
            this.splitMain.Panel1.Controls.Add(this.thumbViewer);
            // 
            // splitMain.Panel2
            // 
            this.splitMain.Panel2.Controls.Add(this.imageViewer);
            this.splitMain.Size = new System.Drawing.Size(640, 553);
            this.splitMain.SplitterDistance = 343;
            this.splitMain.TabIndex = 0;
            // 
            // toolbar
            // 
            this.toolbar.Dock = System.Windows.Forms.DockStyle.None;
            this.toolbar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolbar.Location = new System.Drawing.Point(0, 0);
            this.toolbar.Name = "toolbar";
            this.toolbar.Size = new System.Drawing.Size(640, 25);
            this.toolbar.Stretch = true;
            this.toolbar.TabIndex = 0;
            // 
            // thumbViewer
            // 
            this.thumbViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.thumbViewer.ImageSize = 64;
            this.thumbViewer.Location = new System.Drawing.Point(0, 0);
            this.thumbViewer.Name = "thumbViewer";
            this.thumbViewer.Size = new System.Drawing.Size(640, 343);
            this.thumbViewer.TabIndex = 0;
            this.thumbViewer.ImageAdded += new AddEventHandler<ImageViewer>(this.OnImageAdded);
            // 
            // imageViewer
            // 
            this.imageViewer.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.imageViewer.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.imageViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageViewer.Image = null;
            this.imageViewer.ImageLocation = null;
            this.imageViewer.IsActive = false;
            this.imageViewer.IsThumbnail = false;
            this.imageViewer.Location = new System.Drawing.Point(0, 0);
            this.imageViewer.Name = "imageViewer";
            this.imageViewer.Size = new System.Drawing.Size(640, 206);
            this.imageViewer.TabIndex = 0;
            // 
            // ImageBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStripContainer1);
            this.Name = "ImageBrowser";
            this.Size = new System.Drawing.Size(640, 600);
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            this.splitMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.StatusStrip statusbar;
        private System.Windows.Forms.ToolStrip toolbar;
        private System.Windows.Forms.SplitContainer splitMain;
        private ThumbnailViewer thumbViewer;
        private ImageViewer imageViewer;
    }
}
