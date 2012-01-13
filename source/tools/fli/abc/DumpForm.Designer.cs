namespace abc
{
    partial class DumpForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.tbInputFile = new System.Windows.Forms.TextBox();
            this.btnBrowseInputFile = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.cbExcludeStandardLibs = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Path:";
            // 
            // tbInputFile
            // 
            this.tbInputFile.Location = new System.Drawing.Point(57, 12);
            this.tbInputFile.Name = "tbInputFile";
            this.tbInputFile.Size = new System.Drawing.Size(468, 20);
            this.tbInputFile.TabIndex = 1;
            // 
            // btnBrowseInputFile
            // 
            this.btnBrowseInputFile.Location = new System.Drawing.Point(531, 12);
            this.btnBrowseInputFile.Name = "btnBrowseInputFile";
            this.btnBrowseInputFile.Size = new System.Drawing.Size(25, 21);
            this.btnBrowseInputFile.TabIndex = 2;
            this.btnBrowseInputFile.Text = "...";
            this.btnBrowseInputFile.UseVisualStyleBackColor = true;
            this.btnBrowseInputFile.Click += new System.EventHandler(this.btnBrowseInputFile_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(481, 107);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 3;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // cbExcludeStandardLibs
            // 
            this.cbExcludeStandardLibs.AutoSize = true;
            this.cbExcludeStandardLibs.Location = new System.Drawing.Point(57, 72);
            this.cbExcludeStandardLibs.Name = "cbExcludeStandardLibs";
            this.cbExcludeStandardLibs.Size = new System.Drawing.Size(132, 17);
            this.cbExcludeStandardLibs.TabIndex = 4;
            this.cbExcludeStandardLibs.Text = "Exclude Standard Libs";
            this.cbExcludeStandardLibs.UseVisualStyleBackColor = true;
            // 
            // DumpForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(568, 142);
            this.Controls.Add(this.cbExcludeStandardLibs);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnBrowseInputFile);
            this.Controls.Add(this.tbInputFile);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "DumpForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DumpForm";
            this.Load += new System.EventHandler(this.DumpForm_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DumpForm_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbInputFile;
        private System.Windows.Forms.Button btnBrowseInputFile;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.CheckBox cbExcludeStandardLibs;
    }
}