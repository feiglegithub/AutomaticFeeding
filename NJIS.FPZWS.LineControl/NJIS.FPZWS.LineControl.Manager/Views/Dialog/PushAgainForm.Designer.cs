namespace NJIS.FPZWS.LineControl.Manager.Views.Dialog
{
    partial class PushAgainForm
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
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.radTextBoxBoardCount = new Telerik.WinControls.UI.RadTextBox();
            this.radButtonOK = new Telerik.WinControls.UI.RadButton();
            this.radButtonCancel = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxBoardCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonOK)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonCancel)).BeginInit();
            this.SuspendLayout();
            // 
            // radLabel1
            // 
            this.radLabel1.Location = new System.Drawing.Point(39, 15);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(54, 18);
            this.radLabel1.TabIndex = 0;
            this.radLabel1.Text = "叠板数量";
            // 
            // radTextBoxBoardCount
            // 
            this.radTextBoxBoardCount.Location = new System.Drawing.Point(128, 14);
            this.radTextBoxBoardCount.Name = "radTextBoxBoardCount";
            this.radTextBoxBoardCount.Size = new System.Drawing.Size(100, 20);
            this.radTextBoxBoardCount.TabIndex = 1;
            this.radTextBoxBoardCount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.radTextBoxBoardCount_KeyPress);
            // 
            // radButtonOK
            // 
            this.radButtonOK.Location = new System.Drawing.Point(12, 76);
            this.radButtonOK.Name = "radButtonOK";
            this.radButtonOK.Size = new System.Drawing.Size(110, 24);
            this.radButtonOK.TabIndex = 2;
            this.radButtonOK.Text = "确定";
            this.radButtonOK.Click += new System.EventHandler(this.radButtonOK_Click);
            // 
            // radButtonCancel
            // 
            this.radButtonCancel.Location = new System.Drawing.Point(128, 76);
            this.radButtonCancel.Name = "radButtonCancel";
            this.radButtonCancel.Size = new System.Drawing.Size(110, 24);
            this.radButtonCancel.TabIndex = 3;
            this.radButtonCancel.Text = "取消";
            this.radButtonCancel.Click += new System.EventHandler(this.radButtonCancel_Click);
            // 
            // PushAgainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(249, 121);
            this.Controls.Add(this.radButtonCancel);
            this.Controls.Add(this.radButtonOK);
            this.Controls.Add(this.radTextBoxBoardCount);
            this.Controls.Add(this.radLabel1);
            this.Name = "PushAgainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PushAgainForm";
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxBoardCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonOK)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonCancel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadTextBox radTextBoxBoardCount;
        private Telerik.WinControls.UI.RadButton radButtonOK;
        private Telerik.WinControls.UI.RadButton radButtonCancel;
    }
}