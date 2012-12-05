namespace DataDynamics.PageFX.TestRunner.UI
{
    partial class GeneralOptionsPage
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
            this.label1 = new System.Windows.Forms.Label();
            this.tbQaBaseDir = new System.Windows.Forms.TextBox();
            this.cbOptimizeCode = new System.Windows.Forms.CheckBox();
            this.cbDebug = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "QA Base Directory:";
            // 
            // tbQaBaseDir
            // 
            this.tbQaBaseDir.Location = new System.Drawing.Point(116, 16);
            this.tbQaBaseDir.Name = "tbQaBaseDir";
            this.tbQaBaseDir.Size = new System.Drawing.Size(340, 20);
            this.tbQaBaseDir.TabIndex = 1;
            // 
            // cbOptimizeCode
            // 
            this.cbOptimizeCode.AutoSize = true;
            this.cbOptimizeCode.Location = new System.Drawing.Point(16, 77);
            this.cbOptimizeCode.Name = "cbOptimizeCode";
            this.cbOptimizeCode.Size = new System.Drawing.Size(93, 17);
            this.cbOptimizeCode.TabIndex = 2;
            this.cbOptimizeCode.Text = "Optimize code";
            this.cbOptimizeCode.UseVisualStyleBackColor = true;
            // 
            // cbDebug
            // 
            this.cbDebug.AutoSize = true;
            this.cbDebug.Location = new System.Drawing.Point(16, 100);
            this.cbDebug.Name = "cbDebug";
            this.cbDebug.Size = new System.Drawing.Size(153, 17);
            this.cbDebug.TabIndex = 3;
            this.cbDebug.Text = "Emit debugging information";
            this.cbDebug.UseVisualStyleBackColor = true;
            // 
            // GeneralOptionsPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbDebug);
            this.Controls.Add(this.cbOptimizeCode);
            this.Controls.Add(this.tbQaBaseDir);
            this.Controls.Add(this.label1);
            this.Name = "GeneralOptionsPage";
            this.Size = new System.Drawing.Size(486, 424);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbQaBaseDir;
        private System.Windows.Forms.CheckBox cbOptimizeCode;
        private System.Windows.Forms.CheckBox cbDebug;
    }
}
