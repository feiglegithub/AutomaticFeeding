namespace NJIS.FPZWS.LineControl.Edgebanding.Simulator
{
    partial class RadForm1
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
            this.txtIpAddress = new System.Windows.Forms.RichTextBox();
            this.btnDisConnect = new Telerik.WinControls.UI.RadButton();
            this.btnConnect = new Telerik.WinControls.UI.RadButton();
            this.txtPort = new System.Windows.Forms.RichTextBox();
            this.btnInPart10 = new Telerik.WinControls.UI.RadButton();
            this.txtPartId10 = new System.Windows.Forms.RichTextBox();
            this.txtMsg = new System.Windows.Forms.RichTextBox();
            this.radPanel1 = new Telerik.WinControls.UI.RadPanel();
            this.radPanel2 = new Telerik.WinControls.UI.RadPanel();
            this.radButton1 = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.btnDisConnect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnConnect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnInPart10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).BeginInit();
            this.radPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel2)).BeginInit();
            this.radPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // txtIpAddress
            // 
            this.txtIpAddress.Location = new System.Drawing.Point(12, 12);
            this.txtIpAddress.Name = "txtIpAddress";
            this.txtIpAddress.Size = new System.Drawing.Size(110, 23);
            this.txtIpAddress.TabIndex = 73;
            this.txtIpAddress.Text = "10.30.18.235";
            // 
            // btnDisConnect
            // 
            this.btnDisConnect.Enabled = false;
            this.btnDisConnect.Location = new System.Drawing.Point(361, 11);
            this.btnDisConnect.Name = "btnDisConnect";
            this.btnDisConnect.Size = new System.Drawing.Size(110, 24);
            this.btnDisConnect.TabIndex = 76;
            this.btnDisConnect.Text = "断开";
            this.btnDisConnect.Click += new System.EventHandler(this.btnDisConnect_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(245, 11);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(110, 24);
            this.btnConnect.TabIndex = 75;
            this.btnConnect.Text = "连接";
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(128, 12);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(110, 23);
            this.txtPort.TabIndex = 74;
            this.txtPort.Text = "102";
            // 
            // btnInPart10
            // 
            this.btnInPart10.Location = new System.Drawing.Point(128, 40);
            this.btnInPart10.Name = "btnInPart10";
            this.btnInPart10.Size = new System.Drawing.Size(110, 24);
            this.btnInPart10.TabIndex = 77;
            this.btnInPart10.Text = "入板扫码(10)";
            this.btnInPart10.Click += new System.EventHandler(this.btnInPart10_Click);
            // 
            // txtPartId10
            // 
            this.txtPartId10.Location = new System.Drawing.Point(12, 41);
            this.txtPartId10.Name = "txtPartId10";
            this.txtPartId10.Size = new System.Drawing.Size(110, 23);
            this.txtPartId10.TabIndex = 78;
            this.txtPartId10.Text = "";
            // 
            // txtMsg
            // 
            this.txtMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMsg.Location = new System.Drawing.Point(0, 0);
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.Size = new System.Drawing.Size(802, 540);
            this.txtMsg.TabIndex = 79;
            this.txtMsg.Text = "";
            // 
            // radPanel1
            // 
            this.radPanel1.Controls.Add(this.txtIpAddress);
            this.radPanel1.Controls.Add(this.txtPort);
            this.radPanel1.Controls.Add(this.btnInPart10);
            this.radPanel1.Controls.Add(this.radButton1);
            this.radPanel1.Controls.Add(this.btnConnect);
            this.radPanel1.Controls.Add(this.txtPartId10);
            this.radPanel1.Controls.Add(this.btnDisConnect);
            this.radPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.radPanel1.Location = new System.Drawing.Point(0, 0);
            this.radPanel1.Name = "radPanel1";
            this.radPanel1.Size = new System.Drawing.Size(802, 76);
            this.radPanel1.TabIndex = 80;
            // 
            // radPanel2
            // 
            this.radPanel2.Controls.Add(this.txtMsg);
            this.radPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanel2.Location = new System.Drawing.Point(0, 76);
            this.radPanel2.Name = "radPanel2";
            this.radPanel2.Size = new System.Drawing.Size(802, 540);
            this.radPanel2.TabIndex = 81;
            // 
            // radButton1
            // 
            this.radButton1.Location = new System.Drawing.Point(245, 40);
            this.radButton1.Name = "radButton1";
            this.radButton1.Size = new System.Drawing.Size(110, 24);
            this.radButton1.TabIndex = 75;
            this.radButton1.Text = "自动触发";
            this.radButton1.Click += new System.EventHandler(this.radButton1_Click);
            // 
            // RadForm1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(802, 616);
            this.Controls.Add(this.radPanel2);
            this.Controls.Add(this.radPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RadForm1";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RadForm1";
            this.Load += new System.EventHandler(this.RadForm1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.btnDisConnect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnConnect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnInPart10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).EndInit();
            this.radPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radPanel2)).EndInit();
            this.radPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.RichTextBox txtIpAddress;
        private Telerik.WinControls.UI.RadButton btnDisConnect;
        private Telerik.WinControls.UI.RadButton btnConnect;
        private System.Windows.Forms.RichTextBox txtPort;
        private Telerik.WinControls.UI.RadButton btnInPart10;
        private System.Windows.Forms.RichTextBox txtPartId10;
        private System.Windows.Forms.RichTextBox txtMsg;
        private Telerik.WinControls.UI.RadPanel radPanel1;
        private Telerik.WinControls.UI.RadPanel radPanel2;
        private Telerik.WinControls.UI.RadButton radButton1;
    }
}