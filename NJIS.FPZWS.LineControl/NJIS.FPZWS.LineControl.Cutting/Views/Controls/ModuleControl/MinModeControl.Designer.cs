namespace NJIS.FPZWS.LineControl.Cutting.UI.Views.Controls.ModuleControl
{
    partial class MinModeControl
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
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.txtCurTask = new Telerik.WinControls.UI.RadTextBox();
            this.radLabel4 = new Telerik.WinControls.UI.RadLabel();
            this.lbCurCuttingPattern = new Telerik.WinControls.UI.RadLabel();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.lbNextCuttingPattern = new Telerik.WinControls.UI.RadLabel();
            this.radLabel3 = new Telerik.WinControls.UI.RadLabel();
            this.txtBatchName = new Telerik.WinControls.UI.RadTextBox();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurTask)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbCurCuttingPattern)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbNextCuttingPattern)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBatchName)).BeginInit();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.radLabel1);
            this.flowLayoutPanel1.Controls.Add(this.txtCurTask);
            this.flowLayoutPanel1.Controls.Add(this.radLabel4);
            this.flowLayoutPanel1.Controls.Add(this.lbCurCuttingPattern);
            this.flowLayoutPanel1.Controls.Add(this.radLabel2);
            this.flowLayoutPanel1.Controls.Add(this.lbNextCuttingPattern);
            this.flowLayoutPanel1.Controls.Add(this.radLabel3);
            this.flowLayoutPanel1.Controls.Add(this.txtBatchName);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(745, 36);
            this.flowLayoutPanel1.TabIndex = 11;
            // 
            // radLabel1
            // 
            this.radLabel1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radLabel1.Location = new System.Drawing.Point(3, 3);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(39, 22);
            this.radLabel1.TabIndex = 1;
            this.radLabel1.Text = "任务:";
            // 
            // txtCurTask
            // 
            this.txtCurTask.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtCurTask.Location = new System.Drawing.Point(48, 3);
            this.txtCurTask.Name = "txtCurTask";
            this.txtCurTask.Size = new System.Drawing.Size(213, 27);
            this.txtCurTask.TabIndex = 5;
            // 
            // radLabel4
            // 
            this.radLabel4.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radLabel4.Location = new System.Drawing.Point(267, 3);
            this.radLabel4.Name = "radLabel4";
            this.radLabel4.Size = new System.Drawing.Size(39, 22);
            this.radLabel4.TabIndex = 7;
            this.radLabel4.Text = "当前:";
            // 
            // lbCurCuttingPattern
            // 
            this.lbCurCuttingPattern.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbCurCuttingPattern.ForeColor = System.Drawing.Color.OrangeRed;
            this.lbCurCuttingPattern.Location = new System.Drawing.Point(312, 3);
            this.lbCurCuttingPattern.Margin = new System.Windows.Forms.Padding(3, 3, 3, 8);
            this.lbCurCuttingPattern.Name = "lbCurCuttingPattern";
            this.lbCurCuttingPattern.Size = new System.Drawing.Size(17, 25);
            this.lbCurCuttingPattern.TabIndex = 8;
            this.lbCurCuttingPattern.Text = "0";
            // 
            // radLabel2
            // 
            this.radLabel2.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radLabel2.Location = new System.Drawing.Point(338, 3);
            this.radLabel2.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(54, 22);
            this.radLabel2.TabIndex = 7;
            this.radLabel2.Text = "下一个:";
            // 
            // lbNextCuttingPattern
            // 
            this.lbNextCuttingPattern.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbNextCuttingPattern.Location = new System.Drawing.Point(398, 3);
            this.lbNextCuttingPattern.Name = "lbNextCuttingPattern";
            this.lbNextCuttingPattern.Size = new System.Drawing.Size(17, 25);
            this.lbNextCuttingPattern.TabIndex = 8;
            this.lbNextCuttingPattern.Text = "0";
            // 
            // radLabel3
            // 
            this.radLabel3.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radLabel3.Location = new System.Drawing.Point(421, 3);
            this.radLabel3.Name = "radLabel3";
            this.radLabel3.Size = new System.Drawing.Size(50, 22);
            this.radLabel3.TabIndex = 1;
            this.radLabel3.Text = "批次：";
            // 
            // txtBatchName
            // 
            this.txtBatchName.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtBatchName.Location = new System.Drawing.Point(477, 3);
            this.txtBatchName.Name = "txtBatchName";
            this.txtBatchName.Size = new System.Drawing.Size(253, 27);
            this.txtBatchName.TabIndex = 5;
            // 
            // MinModeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "MinModeControl";
            this.Size = new System.Drawing.Size(745, 36);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurTask)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbCurCuttingPattern)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbNextCuttingPattern)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBatchName)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadTextBox txtCurTask;
        private Telerik.WinControls.UI.RadLabel radLabel4;
        private Telerik.WinControls.UI.RadLabel lbCurCuttingPattern;
        private Telerik.WinControls.UI.RadLabel radLabel2;
        private Telerik.WinControls.UI.RadLabel lbNextCuttingPattern;
        private Telerik.WinControls.UI.RadLabel radLabel3;
        private Telerik.WinControls.UI.RadTextBox txtBatchName;
    }
}
