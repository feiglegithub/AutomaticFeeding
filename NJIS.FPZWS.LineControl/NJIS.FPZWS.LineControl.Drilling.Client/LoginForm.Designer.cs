namespace NJIS.FPZWS.LineControl.Drilling.Client
{
    partial class LoginForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.rbtnLogin = new Telerik.WinControls.UI.RadButton();
            this.rbtnCancel = new Telerik.WinControls.UI.RadButton();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.rtxtUserName = new Telerik.WinControls.UI.RadTextBox();
            this.rtxtPassword = new Telerik.WinControls.UI.RadTextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.radPanel1 = new Telerik.WinControls.UI.RadPanel();
            this.rlblMsg = new Telerik.WinControls.UI.RadLabel();
            this.rcbRemember = new Telerik.WinControls.UI.RadCheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.rbtnLogin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtnCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rtxtUserName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rtxtPassword)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).BeginInit();
            this.radPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rlblMsg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rcbRemember)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // rbtnLogin
            // 
            this.rbtnLogin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rbtnLogin.Font = new System.Drawing.Font("微软雅黑", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbtnLogin.Location = new System.Drawing.Point(331, 394);
            this.rbtnLogin.Name = "rbtnLogin";
            this.rbtnLogin.Size = new System.Drawing.Size(155, 44);
            this.rbtnLogin.TabIndex = 1;
            this.rbtnLogin.Text = "登录";
            this.rbtnLogin.Click += new System.EventHandler(this.rbtnLogin_Click);
            // 
            // rbtnCancel
            // 
            this.rbtnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rbtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.rbtnCancel.Font = new System.Drawing.Font("微软雅黑", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbtnCancel.Location = new System.Drawing.Point(492, 394);
            this.rbtnCancel.Name = "rbtnCancel";
            this.rbtnCancel.Size = new System.Drawing.Size(155, 44);
            this.rbtnCancel.TabIndex = 2;
            this.rbtnCancel.Text = "取消";
            // 
            // radLabel1
            // 
            this.radLabel1.Font = new System.Drawing.Font("微软雅黑", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radLabel1.Location = new System.Drawing.Point(75, 218);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(102, 44);
            this.radLabel1.TabIndex = 3;
            this.radLabel1.Text = "用户名";
            // 
            // radLabel2
            // 
            this.radLabel2.Font = new System.Drawing.Font("微软雅黑", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radLabel2.Location = new System.Drawing.Point(75, 276);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(72, 44);
            this.radLabel2.TabIndex = 4;
            this.radLabel2.Text = "密码";
            // 
            // rtxtUserName
            // 
            this.rtxtUserName.Font = new System.Drawing.Font("微软雅黑", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rtxtUserName.Location = new System.Drawing.Point(203, 217);
            this.rtxtUserName.Name = "rtxtUserName";
            this.rtxtUserName.Size = new System.Drawing.Size(378, 44);
            this.rtxtUserName.TabIndex = 5;
            // 
            // rtxtPassword
            // 
            this.rtxtPassword.Font = new System.Drawing.Font("微软雅黑", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rtxtPassword.Location = new System.Drawing.Point(203, 277);
            this.rtxtPassword.Name = "rtxtPassword";
            this.rtxtPassword.PasswordChar = '*';
            this.rtxtPassword.Size = new System.Drawing.Size(378, 44);
            this.rtxtPassword.TabIndex = 6;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::NJIS.FPZWS.LineControl.Drilling.Client.Properties.Resources.logo5;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(321, 147);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::NJIS.FPZWS.LineControl.Drilling.Client.Properties.Resources.logo4;
            this.pictureBox2.Location = new System.Drawing.Point(339, 12);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(312, 147);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 8;
            this.pictureBox2.TabStop = false;
            // 
            // radPanel1
            // 
            this.radPanel1.Controls.Add(this.pictureBox1);
            this.radPanel1.Controls.Add(this.pictureBox2);
            this.radPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.radPanel1.Location = new System.Drawing.Point(0, 0);
            this.radPanel1.Name = "radPanel1";
            this.radPanel1.Size = new System.Drawing.Size(659, 167);
            this.radPanel1.TabIndex = 9;
            // 
            // rlblMsg
            // 
            this.rlblMsg.Font = new System.Drawing.Font("微软雅黑", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rlblMsg.ForeColor = System.Drawing.Color.Red;
            this.rlblMsg.Location = new System.Drawing.Point(12, 394);
            this.rlblMsg.Name = "rlblMsg";
            this.rlblMsg.Size = new System.Drawing.Size(2, 2);
            this.rlblMsg.TabIndex = 10;
            // 
            // rcbRemember
            // 
            this.rcbRemember.CheckState = System.Windows.Forms.CheckState.Checked;
            this.rcbRemember.Font = new System.Drawing.Font("微软雅黑", 21.75F);
            this.rcbRemember.Location = new System.Drawing.Point(203, 327);
            this.rcbRemember.Name = "rcbRemember";
            this.rcbRemember.Size = new System.Drawing.Size(146, 44);
            this.rcbRemember.TabIndex = 11;
            this.rcbRemember.Text = "记住密码";
            this.rcbRemember.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On;
            // 
            // LoginForm
            // 
            this.AcceptButton = this.rbtnLogin;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.rbtnCancel;
            this.ClientSize = new System.Drawing.Size(659, 450);
            this.Controls.Add(this.rcbRemember);
            this.Controls.Add(this.rlblMsg);
            this.Controls.Add(this.radPanel1);
            this.Controls.Add(this.rtxtPassword);
            this.Controls.Add(this.rtxtUserName);
            this.Controls.Add(this.radLabel2);
            this.Controls.Add(this.radLabel1);
            this.Controls.Add(this.rbtnCancel);
            this.Controls.Add(this.rbtnLogin);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SFY生产数据管理-系统登录";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.rbtnLogin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtnCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rtxtUserName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rtxtPassword)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).EndInit();
            this.radPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rlblMsg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rcbRemember)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Telerik.WinControls.UI.RadButton rbtnLogin;
        private Telerik.WinControls.UI.RadButton rbtnCancel;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadLabel radLabel2;
        private Telerik.WinControls.UI.RadTextBox rtxtUserName;
        private Telerik.WinControls.UI.RadTextBox rtxtPassword;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private Telerik.WinControls.UI.RadPanel radPanel1;
        private Telerik.WinControls.UI.RadLabel rlblMsg;
        private Telerik.WinControls.UI.RadCheckBox rcbRemember;
    }
}