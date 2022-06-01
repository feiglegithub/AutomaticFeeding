namespace LEDDemo
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
            this.button3 = new System.Windows.Forms.Button();
            this.TxtDisConnIP = new System.Windows.Forms.RichTextBox();
            this.TxtIP = new System.Windows.Forms.RichTextBox();
            this.TxtSendContent = new System.Windows.Forms.RichTextBox();
            this.TxtLog = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(972, 548);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 29);
            this.button1.TabIndex = 7;
            this.button1.Text = "初始化";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(972, 585);
            this.button3.Margin = new System.Windows.Forms.Padding(4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(100, 29);
            this.button3.TabIndex = 8;
            this.button3.Text = "开始";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // TxtDisConnIP
            // 
            this.TxtDisConnIP.Location = new System.Drawing.Point(7, 423);
            this.TxtDisConnIP.Margin = new System.Windows.Forms.Padding(4);
            this.TxtDisConnIP.Name = "TxtDisConnIP";
            this.TxtDisConnIP.Size = new System.Drawing.Size(247, 195);
            this.TxtDisConnIP.TabIndex = 3;
            this.TxtDisConnIP.Text = "";
            // 
            // TxtIP
            // 
            this.TxtIP.Location = new System.Drawing.Point(7, 6);
            this.TxtIP.Margin = new System.Windows.Forms.Padding(4);
            this.TxtIP.Name = "TxtIP";
            this.TxtIP.Size = new System.Drawing.Size(247, 409);
            this.TxtIP.TabIndex = 4;
            this.TxtIP.Text = "";
            // 
            // TxtSendContent
            // 
            this.TxtSendContent.Location = new System.Drawing.Point(272, 423);
            this.TxtSendContent.Margin = new System.Windows.Forms.Padding(4);
            this.TxtSendContent.Name = "TxtSendContent";
            this.TxtSendContent.Size = new System.Drawing.Size(678, 195);
            this.TxtSendContent.TabIndex = 5;
            this.TxtSendContent.Text = "";
            // 
            // TxtLog
            // 
            this.TxtLog.Location = new System.Drawing.Point(272, 8);
            this.TxtLog.Margin = new System.Windows.Forms.Padding(4);
            this.TxtLog.Name = "TxtLog";
            this.TxtLog.Size = new System.Drawing.Size(800, 407);
            this.TxtLog.TabIndex = 6;
            this.TxtLog.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1085, 634);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.TxtDisConnIP);
            this.Controls.Add(this.TxtIP);
            this.Controls.Add(this.TxtSendContent);
            this.Controls.Add(this.TxtLog);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.RichTextBox TxtDisConnIP;
        private System.Windows.Forms.RichTextBox TxtIP;
        private System.Windows.Forms.RichTextBox TxtSendContent;
        private System.Windows.Forms.RichTextBox TxtLog;
    }
}

