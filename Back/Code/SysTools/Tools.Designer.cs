
namespace SysTools
{
    partial class Tools
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tbDescrypt = new System.Windows.Forms.Button();
            this.tbSourceStr = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbResult = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbKey = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnEncrypt = new System.Windows.Forms.Button();
            this.lbMsg = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbDescrypt
            // 
            this.tbDescrypt.Location = new System.Drawing.Point(646, 285);
            this.tbDescrypt.Name = "tbDescrypt";
            this.tbDescrypt.Size = new System.Drawing.Size(75, 23);
            this.tbDescrypt.TabIndex = 0;
            this.tbDescrypt.Text = "解密";
            this.tbDescrypt.UseVisualStyleBackColor = true;
            this.tbDescrypt.Click += new System.EventHandler(this.tbDescrypt_Click);
            // 
            // tbSourceStr
            // 
            this.tbSourceStr.Location = new System.Drawing.Point(151, 45);
            this.tbSourceStr.Multiline = true;
            this.tbSourceStr.Name = "tbSourceStr";
            this.tbSourceStr.Size = new System.Drawing.Size(570, 75);
            this.tbSourceStr.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "原字符串";
            // 
            // cbType
            // 
            this.cbType.FormattingEnabled = true;
            this.cbType.Location = new System.Drawing.Point(151, 131);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(117, 25);
            this.cbType.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 134);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "加解密类型";
            // 
            // tbResult
            // 
            this.tbResult.Location = new System.Drawing.Point(151, 162);
            this.tbResult.Multiline = true;
            this.tbResult.Name = "tbResult";
            this.tbResult.Size = new System.Drawing.Size(570, 81);
            this.tbResult.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 185);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "输出结果";
            // 
            // tbKey
            // 
            this.tbKey.Location = new System.Drawing.Point(334, 133);
            this.tbKey.Name = "tbKey";
            this.tbKey.Size = new System.Drawing.Size(387, 23);
            this.tbKey.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(296, 136);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 17);
            this.label4.TabIndex = 8;
            this.label4.Text = "秘钥";
            // 
            // btnEncrypt
            // 
            this.btnEncrypt.Location = new System.Drawing.Point(532, 285);
            this.btnEncrypt.Name = "btnEncrypt";
            this.btnEncrypt.Size = new System.Drawing.Size(75, 23);
            this.btnEncrypt.TabIndex = 9;
            this.btnEncrypt.Text = "加密";
            this.btnEncrypt.UseVisualStyleBackColor = true;
            this.btnEncrypt.Click += new System.EventHandler(this.btnEncrypt_Click);
            // 
            // lbMsg
            // 
            this.lbMsg.AutoSize = true;
            this.lbMsg.ForeColor = System.Drawing.Color.Red;
            this.lbMsg.Location = new System.Drawing.Point(0, 12);
            this.lbMsg.Name = "lbMsg";
            this.lbMsg.Size = new System.Drawing.Size(50, 17);
            this.lbMsg.TabIndex = 10;
            this.lbMsg.Text = "234567";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lbMsg);
            this.panel1.Location = new System.Drawing.Point(151, 321);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(569, 42);
            this.panel1.TabIndex = 11;
            // 
            // Tools
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(796, 375);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnEncrypt);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbKey);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbResult);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbType);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbSourceStr);
            this.Controls.Add(this.tbDescrypt);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Tools";
            this.Text = "系统工具";
            this.Load += new System.EventHandler(this.Tools_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button tbDescrypt;
        private System.Windows.Forms.TextBox tbSourceStr;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbResult;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbKey;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnEncrypt;
        private System.Windows.Forms.Label lbMsg;
        private System.Windows.Forms.Panel panel1;
    }
}

