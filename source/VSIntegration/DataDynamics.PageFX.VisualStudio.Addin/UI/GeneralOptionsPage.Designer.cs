namespace DataDynamics.PageFX.UI
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
            this.tbHTMLWDir = new System.Windows.Forms.TextBox();
            this.cbGenHTMLWrapper = new System.Windows.Forms.CheckBox();
            this.cbBreakException = new System.Windows.Forms.CheckBox();
            this.gHTMLWrapper = new System.Windows.Forms.GroupBox();
            this.gCompiler = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.nuFPVersion = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tbLocale = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.tbRSLUrl = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.gHTMLWrapper.SuspendLayout();
            this.gCompiler.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nuFPVersion)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "HTML Wrapper:";
            // 
            // tbHTMLWDir
            // 
            this.tbHTMLWDir.Location = new System.Drawing.Point(89, 54);
            this.tbHTMLWDir.Name = "tbHTMLWDir";
            this.tbHTMLWDir.Size = new System.Drawing.Size(321, 20);
            this.tbHTMLWDir.TabIndex = 1;
            // 
            // cbGenHTMLWrapper
            // 
            this.cbGenHTMLWrapper.AutoSize = true;
            this.cbGenHTMLWrapper.Location = new System.Drawing.Point(6, 19);
            this.cbGenHTMLWrapper.Name = "cbGenHTMLWrapper";
            this.cbGenHTMLWrapper.Size = new System.Drawing.Size(147, 17);
            this.cbGenHTMLWrapper.TabIndex = 2;
            this.cbGenHTMLWrapper.Text = "Generate HTML Wrapper";
            this.cbGenHTMLWrapper.UseVisualStyleBackColor = true;
            // 
            // cbBreakException
            // 
            this.cbBreakException.AutoSize = true;
            this.cbBreakException.Location = new System.Drawing.Point(9, 19);
            this.cbBreakException.Name = "cbBreakException";
            this.cbBreakException.Size = new System.Drawing.Size(177, 17);
            this.cbBreakException.TabIndex = 3;
            this.cbBreakException.Text = "Break when exception is thrown";
            this.cbBreakException.UseVisualStyleBackColor = true;
            // 
            // gHTMLWrapper
            // 
            this.gHTMLWrapper.Controls.Add(this.tbHTMLWDir);
            this.gHTMLWrapper.Controls.Add(this.cbGenHTMLWrapper);
            this.gHTMLWrapper.Controls.Add(this.label1);
            this.gHTMLWrapper.Location = new System.Drawing.Point(12, 64);
            this.gHTMLWrapper.Name = "gHTMLWrapper";
            this.gHTMLWrapper.Size = new System.Drawing.Size(416, 89);
            this.gHTMLWrapper.TabIndex = 4;
            this.gHTMLWrapper.TabStop = false;
            this.gHTMLWrapper.Text = "HTML Wrapper  ";
            // 
            // gCompiler
            // 
            this.gCompiler.Controls.Add(this.cbBreakException);
            this.gCompiler.Location = new System.Drawing.Point(12, 3);
            this.gCompiler.Name = "gCompiler";
            this.gCompiler.Size = new System.Drawing.Size(416, 55);
            this.gCompiler.TabIndex = 5;
            this.gCompiler.TabStop = false;
            this.gCompiler.Text = "Compiler Options  ";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.nuFPVersion);
            this.groupBox1.Location = new System.Drawing.Point(12, 159);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(416, 63);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "SWF Options";
            // 
            // nuFPVersion
            // 
            this.nuFPVersion.Location = new System.Drawing.Point(114, 21);
            this.nuFPVersion.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nuFPVersion.Minimum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.nuFPVersion.Name = "nuFPVersion";
            this.nuFPVersion.Size = new System.Drawing.Size(120, 20);
            this.nuFPVersion.TabIndex = 0;
            this.nuFPVersion.Value = new decimal(new int[] {
            9,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Flash Player Version:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.tbRSLUrl);
            this.groupBox2.Controls.Add(this.checkBox1);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.tbLocale);
            this.groupBox2.Location = new System.Drawing.Point(12, 228);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(416, 127);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Flex Options";
            // 
            // tbLocale
            // 
            this.tbLocale.Location = new System.Drawing.Point(89, 19);
            this.tbLocale.Name = "tbLocale";
            this.tbLocale.Size = new System.Drawing.Size(321, 20);
            this.tbLocale.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Locale:";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(9, 56);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(223, 17);
            this.checkBox1.TabIndex = 2;
            this.checkBox1.Text = "Link Flex Framework Externally using RSL";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // tbRSLUrl
            // 
            this.tbRSLUrl.Location = new System.Drawing.Point(89, 91);
            this.tbRSLUrl.Name = "tbRSLUrl";
            this.tbRSLUrl.Size = new System.Drawing.Size(321, 20);
            this.tbRSLUrl.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 91);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "RSL URL:";
            // 
            // GeneralOptionsPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gCompiler);
            this.Controls.Add(this.gHTMLWrapper);
            this.Name = "GeneralOptionsPage";
            this.Size = new System.Drawing.Size(436, 358);
            this.gHTMLWrapper.ResumeLayout(false);
            this.gHTMLWrapper.PerformLayout();
            this.gCompiler.ResumeLayout(false);
            this.gCompiler.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nuFPVersion)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbHTMLWDir;
        private System.Windows.Forms.CheckBox cbGenHTMLWrapper;
        private System.Windows.Forms.CheckBox cbBreakException;
        private System.Windows.Forms.GroupBox gHTMLWrapper;
        private System.Windows.Forms.GroupBox gCompiler;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nuFPVersion;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbLocale;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbRSLUrl;
    }
}
