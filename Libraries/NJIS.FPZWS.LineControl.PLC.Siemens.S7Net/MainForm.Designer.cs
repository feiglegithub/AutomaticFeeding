namespace NJIS.FPZWS.LineControl.PLC.Siemens.S7Net
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.btnWritePlcVarToString = new System.Windows.Forms.Button();
            this.btnReadPlcVarToString = new System.Windows.Forms.Button();
            this.txtPlcVarStringReadInteral = new System.Windows.Forms.TextBox();
            this.txtPlcVarStringWriteInteral = new System.Windows.Forms.TextBox();
            this.txtIp = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.RichTextBox();
            this.cb_string = new System.Windows.Forms.CheckBox();
            this.cb_int = new System.Windows.Forms.CheckBox();
            this.cb_lint = new System.Windows.Forms.CheckBox();
            this.cb_bool = new System.Windows.Forms.CheckBox();
            this.cb_real = new System.Windows.Forms.CheckBox();
            this.btnReadOnce = new System.Windows.Forms.Button();
            this.btnWriteOnce = new System.Windows.Forms.Button();
            this.btnClearMsg = new System.Windows.Forms.Button();
            this.btnStatistics = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.lbPlcStringVars = new System.Windows.Forms.TextBox();
            this.lbPlcIntVars = new System.Windows.Forms.TextBox();
            this.lbPlcLintVars = new System.Windows.Forms.TextBox();
            this.lbPlcBoolVars = new System.Windows.Forms.TextBox();
            this.lbPlcRealVars = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnWritePlcVarToString
            // 
            this.btnWritePlcVarToString.Location = new System.Drawing.Point(406, 325);
            this.btnWritePlcVarToString.Name = "btnWritePlcVarToString";
            this.btnWritePlcVarToString.Size = new System.Drawing.Size(85, 31);
            this.btnWritePlcVarToString.TabIndex = 1;
            this.btnWritePlcVarToString.Text = "写入(>>)";
            this.btnWritePlcVarToString.UseVisualStyleBackColor = true;
            this.btnWritePlcVarToString.Click += new System.EventHandler(this.btnWritePlcVarToString_Click);
            // 
            // btnReadPlcVarToString
            // 
            this.btnReadPlcVarToString.Location = new System.Drawing.Point(82, 323);
            this.btnReadPlcVarToString.Name = "btnReadPlcVarToString";
            this.btnReadPlcVarToString.Size = new System.Drawing.Size(85, 31);
            this.btnReadPlcVarToString.TabIndex = 3;
            this.btnReadPlcVarToString.Text = "读取(>>)";
            this.btnReadPlcVarToString.UseVisualStyleBackColor = true;
            this.btnReadPlcVarToString.Click += new System.EventHandler(this.btnReadPlcVarToString_Click);
            // 
            // txtPlcVarStringReadInteral
            // 
            this.txtPlcVarStringReadInteral.Location = new System.Drawing.Point(12, 323);
            this.txtPlcVarStringReadInteral.Name = "txtPlcVarStringReadInteral";
            this.txtPlcVarStringReadInteral.Size = new System.Drawing.Size(64, 29);
            this.txtPlcVarStringReadInteral.TabIndex = 17;
            this.txtPlcVarStringReadInteral.Text = "100";
            // 
            // txtPlcVarStringWriteInteral
            // 
            this.txtPlcVarStringWriteInteral.Location = new System.Drawing.Point(336, 325);
            this.txtPlcVarStringWriteInteral.Name = "txtPlcVarStringWriteInteral";
            this.txtPlcVarStringWriteInteral.Size = new System.Drawing.Size(64, 29);
            this.txtPlcVarStringWriteInteral.TabIndex = 18;
            this.txtPlcVarStringWriteInteral.Text = "100";
            // 
            // txtIp
            // 
            this.txtIp.Location = new System.Drawing.Point(12, 12);
            this.txtIp.Name = "txtIp";
            this.txtIp.Size = new System.Drawing.Size(156, 29);
            this.txtIp.TabIndex = 19;
            this.txtIp.Text = "10.30.42.10";
            // 
            // txtPort
            // 
            this.txtPort.Enabled = false;
            this.txtPort.Location = new System.Drawing.Point(174, 12);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(156, 29);
            this.txtPort.TabIndex = 20;
            this.txtPort.Text = "102";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(336, 10);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 31);
            this.btnConnect.TabIndex = 21;
            this.btnConnect.Text = "连接";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(12, 360);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(804, 437);
            this.txtMessage.TabIndex = 22;
            this.txtMessage.Text = "";
            // 
            // cb_string
            // 
            this.cb_string.AutoSize = true;
            this.cb_string.Checked = true;
            this.cb_string.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_string.Location = new System.Drawing.Point(12, 292);
            this.cb_string.Name = "cb_string";
            this.cb_string.Size = new System.Drawing.Size(114, 25);
            this.cb_string.TabIndex = 23;
            this.cb_string.Text = "启用(string)";
            this.cb_string.UseVisualStyleBackColor = true;
            // 
            // cb_int
            // 
            this.cb_int.AutoSize = true;
            this.cb_int.Checked = true;
            this.cb_int.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_int.Location = new System.Drawing.Point(174, 292);
            this.cb_int.Name = "cb_int";
            this.cb_int.Size = new System.Drawing.Size(110, 25);
            this.cb_int.TabIndex = 24;
            this.cb_int.Text = "启用(short)";
            this.cb_int.UseVisualStyleBackColor = true;
            // 
            // cb_lint
            // 
            this.cb_lint.AutoSize = true;
            this.cb_lint.Checked = true;
            this.cb_lint.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_lint.Location = new System.Drawing.Point(336, 292);
            this.cb_lint.Name = "cb_lint";
            this.cb_lint.Size = new System.Drawing.Size(91, 25);
            this.cb_lint.TabIndex = 25;
            this.cb_lint.Text = "启用(int)";
            this.cb_lint.UseVisualStyleBackColor = true;
            // 
            // cb_bool
            // 
            this.cb_bool.AutoSize = true;
            this.cb_bool.Checked = true;
            this.cb_bool.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_bool.Location = new System.Drawing.Point(498, 292);
            this.cb_bool.Name = "cb_bool";
            this.cb_bool.Size = new System.Drawing.Size(105, 25);
            this.cb_bool.TabIndex = 26;
            this.cb_bool.Text = "启用(bool)";
            this.cb_bool.UseVisualStyleBackColor = true;
            // 
            // cb_real
            // 
            this.cb_real.AutoSize = true;
            this.cb_real.Checked = true;
            this.cb_real.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_real.Location = new System.Drawing.Point(660, 292);
            this.cb_real.Name = "cb_real";
            this.cb_real.Size = new System.Drawing.Size(99, 25);
            this.cb_real.TabIndex = 27;
            this.cb_real.Text = "启用(real)";
            this.cb_real.UseVisualStyleBackColor = true;
            // 
            // btnReadOnce
            // 
            this.btnReadOnce.Location = new System.Drawing.Point(173, 323);
            this.btnReadOnce.Name = "btnReadOnce";
            this.btnReadOnce.Size = new System.Drawing.Size(85, 31);
            this.btnReadOnce.TabIndex = 28;
            this.btnReadOnce.Text = "读一次";
            this.btnReadOnce.UseVisualStyleBackColor = true;
            this.btnReadOnce.Click += new System.EventHandler(this.btnReadOnce_Click);
            // 
            // btnWriteOnce
            // 
            this.btnWriteOnce.Location = new System.Drawing.Point(498, 325);
            this.btnWriteOnce.Name = "btnWriteOnce";
            this.btnWriteOnce.Size = new System.Drawing.Size(85, 31);
            this.btnWriteOnce.TabIndex = 29;
            this.btnWriteOnce.Text = "写一次";
            this.btnWriteOnce.UseVisualStyleBackColor = true;
            this.btnWriteOnce.Click += new System.EventHandler(this.btnWriteOnce_Click);
            // 
            // btnClearMsg
            // 
            this.btnClearMsg.Location = new System.Drawing.Point(723, 323);
            this.btnClearMsg.Name = "btnClearMsg";
            this.btnClearMsg.Size = new System.Drawing.Size(85, 31);
            this.btnClearMsg.TabIndex = 30;
            this.btnClearMsg.Text = "清除消息";
            this.btnClearMsg.UseVisualStyleBackColor = true;
            this.btnClearMsg.Click += new System.EventHandler(this.btnClearMsg_Click);
            // 
            // btnStatistics
            // 
            this.btnStatistics.Location = new System.Drawing.Point(589, 325);
            this.btnStatistics.Name = "btnStatistics";
            this.btnStatistics.Size = new System.Drawing.Size(85, 31);
            this.btnStatistics.TabIndex = 31;
            this.btnStatistics.Text = "统计";
            this.btnStatistics.UseVisualStyleBackColor = true;
            this.btnStatistics.Click += new System.EventHandler(this.btnStatistics_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(264, 325);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(66, 31);
            this.button1.TabIndex = 32;
            this.button1.Text = "全部读";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lbPlcStringVars
            // 
            this.lbPlcStringVars.Location = new System.Drawing.Point(12, 51);
            this.lbPlcStringVars.Multiline = true;
            this.lbPlcStringVars.Name = "lbPlcStringVars";
            this.lbPlcStringVars.Size = new System.Drawing.Size(155, 235);
            this.lbPlcStringVars.TabIndex = 33;
            this.lbPlcStringVars.Text = "DB1.0\r\nDB1.256\r\nDB1.512\r\nDB1.768\r\nDB1.1024\r\nDB1.1280\r\nDB1.1536\r\nDB1.1792\r\nDB1.204" +
    "8\r\nDB1.2304";
            // 
            // lbPlcIntVars
            // 
            this.lbPlcIntVars.Location = new System.Drawing.Point(174, 51);
            this.lbPlcIntVars.Multiline = true;
            this.lbPlcIntVars.Name = "lbPlcIntVars";
            this.lbPlcIntVars.Size = new System.Drawing.Size(156, 235);
            this.lbPlcIntVars.TabIndex = 34;
            this.lbPlcIntVars.Text = resources.GetString("lbPlcIntVars.Text");
            // 
            // lbPlcLintVars
            // 
            this.lbPlcLintVars.Location = new System.Drawing.Point(336, 51);
            this.lbPlcLintVars.Multiline = true;
            this.lbPlcLintVars.Name = "lbPlcLintVars";
            this.lbPlcLintVars.Size = new System.Drawing.Size(155, 235);
            this.lbPlcLintVars.TabIndex = 35;
            this.lbPlcLintVars.Text = resources.GetString("lbPlcLintVars.Text");
            // 
            // lbPlcBoolVars
            // 
            this.lbPlcBoolVars.Location = new System.Drawing.Point(497, 51);
            this.lbPlcBoolVars.Multiline = true;
            this.lbPlcBoolVars.Name = "lbPlcBoolVars";
            this.lbPlcBoolVars.Size = new System.Drawing.Size(157, 235);
            this.lbPlcBoolVars.TabIndex = 36;
            this.lbPlcBoolVars.Text = "DB1.3100\r\nDB1.3100.1\r\nDB1.3100.2\r\nDB1.3100.3\r\nDB1.3100.4\r\nDB1.3100.5\r\nDB1.3100.6\r" +
    "\nDB1.3100.7";
            // 
            // lbPlcRealVars
            // 
            this.lbPlcRealVars.Location = new System.Drawing.Point(660, 51);
            this.lbPlcRealVars.Multiline = true;
            this.lbPlcRealVars.Name = "lbPlcRealVars";
            this.lbPlcRealVars.Size = new System.Drawing.Size(156, 235);
            this.lbPlcRealVars.TabIndex = 37;
            this.lbPlcRealVars.Text = "DB1.3102\r\nDB1.3106\r\nDB1.3110\r\nDB1.3114\r\nDB1.3118\r\nDB1.3122\r\nDB1.3126\r\nDB1.3130\r\nD" +
    "B1.3134\r\nDB1.3138\r\nDB1.3142";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(820, 809);
            this.Controls.Add(this.lbPlcRealVars);
            this.Controls.Add(this.lbPlcBoolVars);
            this.Controls.Add(this.lbPlcLintVars);
            this.Controls.Add(this.lbPlcIntVars);
            this.Controls.Add(this.lbPlcStringVars);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnStatistics);
            this.Controls.Add(this.btnClearMsg);
            this.Controls.Add(this.btnWriteOnce);
            this.Controls.Add(this.btnReadOnce);
            this.Controls.Add(this.cb_real);
            this.Controls.Add(this.cb_bool);
            this.Controls.Add(this.cb_lint);
            this.Controls.Add(this.cb_int);
            this.Controls.Add(this.cb_string);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.txtIp);
            this.Controls.Add(this.txtPlcVarStringWriteInteral);
            this.Controls.Add(this.txtPlcVarStringReadInteral);
            this.Controls.Add(this.btnReadPlcVarToString);
            this.Controls.Add(this.btnWritePlcVarToString);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnWritePlcVarToString;
        private System.Windows.Forms.Button btnReadPlcVarToString;
        private System.Windows.Forms.TextBox txtPlcVarStringReadInteral;
        private System.Windows.Forms.TextBox txtPlcVarStringWriteInteral;
        private System.Windows.Forms.TextBox txtIp;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.RichTextBox txtMessage;
        private System.Windows.Forms.CheckBox cb_string;
        private System.Windows.Forms.CheckBox cb_int;
        private System.Windows.Forms.CheckBox cb_lint;
        private System.Windows.Forms.CheckBox cb_bool;
        private System.Windows.Forms.CheckBox cb_real;
        private System.Windows.Forms.Button btnReadOnce;
        private System.Windows.Forms.Button btnWriteOnce;
        private System.Windows.Forms.Button btnClearMsg;
        private System.Windows.Forms.Button btnStatistics;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox lbPlcStringVars;
        private System.Windows.Forms.TextBox lbPlcIntVars;
        private System.Windows.Forms.TextBox lbPlcLintVars;
        private System.Windows.Forms.TextBox lbPlcBoolVars;
        private System.Windows.Forms.TextBox lbPlcRealVars;
    }
}