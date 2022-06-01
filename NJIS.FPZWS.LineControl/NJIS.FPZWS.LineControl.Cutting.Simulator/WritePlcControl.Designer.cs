namespace NJIS.FPZWS.LineControl.Cutting.Simulator
{
    partial class WritePlcControl
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
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.txtPartId = new Telerik.WinControls.UI.RadTextBox();
            this.btnWritePartId = new Telerik.WinControls.UI.RadButton();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPartId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnWritePartId)).BeginInit();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.txtPartId);
            this.flowLayoutPanel1.Controls.Add(this.btnWritePartId);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(199, 27);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // txtPartId
            // 
            this.txtPartId.Location = new System.Drawing.Point(3, 3);
            this.txtPartId.Name = "txtPartId";
            this.txtPartId.Size = new System.Drawing.Size(124, 20);
            this.txtPartId.TabIndex = 4;
            // 
            // btnWritePartId
            // 
            this.btnWritePartId.Location = new System.Drawing.Point(133, 3);
            this.btnWritePartId.Name = "btnWritePartId";
            this.btnWritePartId.Size = new System.Drawing.Size(61, 20);
            this.btnWritePartId.TabIndex = 3;
            this.btnWritePartId.Text = "扫码10";
            this.btnWritePartId.Click += new System.EventHandler(this.btnInPartId_Click);
            // 
            // WritePlcControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "WritePlcControl";
            this.Size = new System.Drawing.Size(199, 27);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPartId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnWritePartId)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private Telerik.WinControls.UI.RadButton btnWritePartId;
        private Telerik.WinControls.UI.RadTextBox txtPartId;
    }
}
