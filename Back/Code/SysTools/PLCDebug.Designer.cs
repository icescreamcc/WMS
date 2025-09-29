namespace SysTools
{
    partial class PLCDebug
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
            tbKey = new System.Windows.Forms.TextBox();
            label4 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            textBox1 = new System.Windows.Forms.TextBox();
            SuspendLayout();
            // 
            // tbKey
            // 
            tbKey.Location = new System.Drawing.Point(78, 51);
            tbKey.Margin = new System.Windows.Forms.Padding(4);
            tbKey.Name = "tbKey";
            tbKey.Size = new System.Drawing.Size(278, 27);
            tbKey.TabIndex = 8;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(48, 54);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(22, 20);
            label4.TabIndex = 9;
            label4.Text = "IP";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(395, 54);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(22, 20);
            label1.TabIndex = 11;
            label1.Text = "IP";
            // 
            // textBox1
            // 
            textBox1.Location = new System.Drawing.Point(425, 51);
            textBox1.Margin = new System.Windows.Forms.Padding(4);
            textBox1.Name = "textBox1";
            textBox1.Size = new System.Drawing.Size(278, 27);
            textBox1.TabIndex = 10;
            // 
            // PLCDebug
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 450);
            Controls.Add(label1);
            Controls.Add(textBox1);
            Controls.Add(label4);
            Controls.Add(tbKey);
            Name = "PLCDebug";
            Text = "PLCDebug";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox tbKey;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
    }
}