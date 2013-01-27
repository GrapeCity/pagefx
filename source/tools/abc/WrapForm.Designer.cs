namespace DataDynamics.PageFX
{
    partial class WrapForm
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
            this.tbInputPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.tbOut = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbOptions = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbDir = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tbInputPath
            // 
            this.tbInputPath.Location = new System.Drawing.Point(74, 12);
            this.tbInputPath.Name = "tbInputPath";
            this.tbInputPath.Size = new System.Drawing.Size(447, 20);
            this.tbInputPath.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "ABC Path:";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(527, 12);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(24, 20);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(476, 138);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(75, 23);
            this.btnGenerate.TabIndex = 4;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // tbOut
            // 
            this.tbOut.Location = new System.Drawing.Point(74, 64);
            this.tbOut.Name = "tbOut";
            this.tbOut.Size = new System.Drawing.Size(100, 20);
            this.tbOut.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(41, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(27, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Out:";
            // 
            // tbOptions
            // 
            this.tbOptions.Location = new System.Drawing.Point(74, 90);
            this.tbOptions.Name = "tbOptions";
            this.tbOptions.Size = new System.Drawing.Size(447, 20);
            this.tbOptions.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Options:";
            // 
            // tbDir
            // 
            this.tbDir.Location = new System.Drawing.Point(241, 64);
            this.tbDir.Name = "tbDir";
            this.tbDir.Size = new System.Drawing.Size(100, 20);
            this.tbDir.TabIndex = 15;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(212, 67);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(23, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Dir:";
            // 
            // WrapForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(563, 173);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbDir);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbOptions);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbOut);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbInputPath);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WrapForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Make ABC Wrapper...";
            this.Load += new System.EventHandler(this.WrapForm_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.WrapForm_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbInputPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.TextBox tbOut;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbOptions;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbDir;
        private System.Windows.Forms.Label label5;
    }
}