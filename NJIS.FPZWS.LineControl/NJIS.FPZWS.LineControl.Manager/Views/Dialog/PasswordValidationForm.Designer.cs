namespace NJIS.FPZWS.LineControl.Manager.Views.Dialog
{
    partial class PasswordValidationForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PasswordValidationForm));
            this.radTextBoxPassword = new Telerik.WinControls.UI.RadTextBox();
            this.radButtonOK = new Telerik.WinControls.UI.RadButton();
            this.radButtonCancel = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxPassword)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonOK)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonCancel)).BeginInit();
            this.SuspendLayout();
            // 
            // radTextBoxPassword
            // 
            this.radTextBoxPassword.Location = new System.Drawing.Point(12, 12);
            this.radTextBoxPassword.Name = "radTextBoxPassword";
            this.radTextBoxPassword.PasswordChar = '*';
            this.radTextBoxPassword.Size = new System.Drawing.Size(226, 20);
            this.radTextBoxPassword.TabIndex = 0;
            // 
            // radButtonOK
            // 
            this.radButtonOK.Location = new System.Drawing.Point(12, 68);
            this.radButtonOK.Name = "radButtonOK";
            this.radButtonOK.Size = new System.Drawing.Size(110, 24);
            this.radButtonOK.TabIndex = 1;
            this.radButtonOK.Text = "确定";
            this.radButtonOK.Click += new System.EventHandler(this.radButtonOK_Click);
            // 
            // radButtonCancel
            // 
            this.radButtonCancel.Location = new System.Drawing.Point(128, 68);
            this.radButtonCancel.Name = "radButtonCancel";
            this.radButtonCancel.Size = new System.Drawing.Size(110, 24);
            this.radButtonCancel.TabIndex = 2;
            this.radButtonCancel.Text = "取消";
            this.radButtonCancel.Click += new System.EventHandler(this.radButtonCancel_Click);
            // 
            // PasswordValidationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(250, 104);
            this.Controls.Add(this.radButtonCancel);
            this.Controls.Add(this.radButtonOK);
            this.Controls.Add(this.radTextBoxPassword);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PasswordValidationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "密码验证";
            this.Load += new System.EventHandler(this.PasswordValidationForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxPassword)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonOK)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonCancel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadTextBox radTextBoxPassword;
        private Telerik.WinControls.UI.RadButton radButtonOK;
        private Telerik.WinControls.UI.RadButton radButtonCancel;
    }
}