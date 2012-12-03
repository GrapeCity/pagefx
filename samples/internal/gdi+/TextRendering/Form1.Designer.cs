namespace TextRendering
{
    partial class Form1
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
            System.Drawing.StringFormat stringFormat1 = new System.Drawing.StringFormat();
            this.textArea1 = new TextRendering.TextArea();
            this.SuspendLayout();
            // 
            // textArea1
            // 
            this.textArea1.Align = System.Drawing.StringAlignment.Near;
            this.textArea1.Brush = null;
            this.textArea1.DirectionRightToLeft = false;
            this.textArea1.DirectionVertical = false;
            this.textArea1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            stringFormat1.Alignment = System.Drawing.StringAlignment.Near;
            stringFormat1.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
            stringFormat1.LineAlignment = System.Drawing.StringAlignment.Near;
            stringFormat1.Trimming = System.Drawing.StringTrimming.None;
            this.textArea1.Format = stringFormat1;
            this.textArea1.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
            this.textArea1.LineAlign = System.Drawing.StringAlignment.Near;
            this.textArea1.Location = new System.Drawing.Point(81, 95);
            this.textArea1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.textArea1.Name = "textArea1";
            this.textArea1.NoClip = false;
            this.textArea1.NoWrap = false;
            this.textArea1.Size = new System.Drawing.Size(371, 223);
            this.textArea1.TabIndex = 0;
            this.textArea1.Trimming = System.Drawing.StringTrimming.None;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 566);
            this.Controls.Add(this.textArea1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private TextArea textArea1;
    }
}

