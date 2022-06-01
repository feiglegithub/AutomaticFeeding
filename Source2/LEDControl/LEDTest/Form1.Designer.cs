namespace LEDTest
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.TxtContent1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.TxtIP = new System.Windows.Forms.TextBox();
            this.TxtPort = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.TxtContent2 = new System.Windows.Forms.TextBox();
            this.TxtContent3 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(453, 172);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(133, 26);
            this.button1.TabIndex = 0;
            this.button1.Text = "LED_MC_ShowString";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // TxtContent1
            // 
            this.TxtContent1.Location = new System.Drawing.Point(92, 84);
            this.TxtContent1.Margin = new System.Windows.Forms.Padding(2);
            this.TxtContent1.Multiline = true;
            this.TxtContent1.Name = "TxtContent1";
            this.TxtContent1.Size = new System.Drawing.Size(356, 34);
            this.TxtContent1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "IP地址：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(45, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "端口：";
            // 
            // TxtIP
            // 
            this.TxtIP.Location = new System.Drawing.Point(92, 9);
            this.TxtIP.Name = "TxtIP";
            this.TxtIP.Size = new System.Drawing.Size(356, 21);
            this.TxtIP.TabIndex = 6;
            this.TxtIP.Text = "192.168.11.224";
            // 
            // TxtPort
            // 
            this.TxtPort.Location = new System.Drawing.Point(92, 44);
            this.TxtPort.Name = "TxtPort";
            this.TxtPort.Size = new System.Drawing.Size(356, 21);
            this.TxtPort.TabIndex = 6;
            this.TxtPort.Text = "28123";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "发送内容：";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(453, 130);
            this.button2.Margin = new System.Windows.Forms.Padding(2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(133, 26);
            this.button2.TabIndex = 0;
            this.button2.Text = "TxtToXMPXFile";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // TxtContent2
            // 
            this.TxtContent2.Location = new System.Drawing.Point(92, 122);
            this.TxtContent2.Margin = new System.Windows.Forms.Padding(2);
            this.TxtContent2.Multiline = true;
            this.TxtContent2.Name = "TxtContent2";
            this.TxtContent2.Size = new System.Drawing.Size(356, 34);
            this.TxtContent2.TabIndex = 2;
            // 
            // TxtContent3
            // 
            this.TxtContent3.Location = new System.Drawing.Point(92, 164);
            this.TxtContent3.Margin = new System.Windows.Forms.Padding(2);
            this.TxtContent3.Multiline = true;
            this.TxtContent3.Name = "TxtContent3";
            this.TxtContent3.Size = new System.Drawing.Size(356, 34);
            this.TxtContent3.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(597, 215);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TxtPort);
            this.Controls.Add(this.TxtIP);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TxtContent3);
            this.Controls.Add(this.TxtContent2);
            this.Controls.Add(this.TxtContent1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox TxtContent1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TxtIP;
        private System.Windows.Forms.TextBox TxtPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox TxtContent2;
        private System.Windows.Forms.TextBox TxtContent3;
    }
}

