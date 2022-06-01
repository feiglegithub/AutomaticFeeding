namespace NJIS.FPZWS.LineControl.PartMill.Simulator.Controls
{
    partial class MachineHandCommandControl
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnExecute = new Telerik.WinControls.UI.RadButton();
            this.cmbCommand = new Telerik.WinControls.UI.RadDropDownList();
            this.checkBox = new Telerik.WinControls.UI.RadCheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.btnExecute)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCommand)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkBox)).BeginInit();
            this.SuspendLayout();
            // 
            // btnExecute
            // 
            this.btnExecute.Location = new System.Drawing.Point(231, 3);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(49, 22);
            this.btnExecute.TabIndex = 1;
            this.btnExecute.Text = "执行";
            // 
            // cmbCommand
            // 
            this.cmbCommand.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
            this.cmbCommand.Location = new System.Drawing.Point(25, 5);
            this.cmbCommand.Name = "cmbCommand";
            this.cmbCommand.Size = new System.Drawing.Size(200, 20);
            this.cmbCommand.TabIndex = 2;
            // 
            // checkBox
            // 
            this.checkBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.checkBox.Location = new System.Drawing.Point(4, 5);
            this.checkBox.Name = "checkBox";
            this.checkBox.Size = new System.Drawing.Size(15, 15);
            this.checkBox.TabIndex = 3;
            // 
            // CommandControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.Controls.Add(this.checkBox);
            this.Controls.Add(this.cmbCommand);
            this.Controls.Add(this.btnExecute);
            this.Name = "CommandControl";
            this.Size = new System.Drawing.Size(287, 28);
            ((System.ComponentModel.ISupportInitialize)(this.btnExecute)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCommand)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Telerik.WinControls.UI.RadButton btnExecute;
        private Telerik.WinControls.UI.RadDropDownList cmbCommand;
        private Telerik.WinControls.UI.RadCheckBox checkBox;
    }
}
