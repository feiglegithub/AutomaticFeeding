namespace NJIS.FPZWS.LineControl.CuttingDevice.Views.Controls.ModuleControl
{
    partial class CurTaskDetailControl
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
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.txtCurTask = new Telerik.WinControls.UI.RadTextBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.radLabel3 = new Telerik.WinControls.UI.RadLabel();
            this.txtNextTask = new Telerik.WinControls.UI.RadTextBox();
            this.btnDeviceError = new Telerik.WinControls.UI.RadButton();
            this.btnPatternError = new Telerik.WinControls.UI.RadButton();
            this.radLabel5 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel6 = new Telerik.WinControls.UI.RadLabel();
            this.cmbCurStatus = new Telerik.WinControls.UI.RadDropDownList();
            this.cmbNextStatus = new Telerik.WinControls.UI.RadDropDownList();
            this.gvbStackDetail = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurTask)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNextTask)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnDeviceError)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnPatternError)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCurStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbNextStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvbStackDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // radLabel1
            // 
            this.radLabel1.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radLabel1.Location = new System.Drawing.Point(3, 3);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(57, 25);
            this.radLabel1.TabIndex = 1;
            this.radLabel1.Text = "任务：";
            // 
            // txtCurTask
            // 
            this.txtCurTask.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtCurTask.Location = new System.Drawing.Point(66, 3);
            this.txtCurTask.Name = "txtCurTask";
            this.txtCurTask.Size = new System.Drawing.Size(251, 32);
            this.txtCurTask.TabIndex = 5;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.radLabel1);
            this.flowLayoutPanel1.Controls.Add(this.txtCurTask);
            this.flowLayoutPanel1.Controls.Add(this.cmbCurStatus);
            this.flowLayoutPanel1.Controls.Add(this.radLabel3);
            this.flowLayoutPanel1.Controls.Add(this.txtNextTask);
            this.flowLayoutPanel1.Controls.Add(this.cmbNextStatus);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 14);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1067, 39);
            this.flowLayoutPanel1.TabIndex = 9;
            // 
            // radLabel3
            // 
            this.radLabel3.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radLabel3.Location = new System.Drawing.Point(413, 3);
            this.radLabel3.Name = "radLabel3";
            this.radLabel3.Size = new System.Drawing.Size(112, 31);
            this.radLabel3.TabIndex = 1;
            this.radLabel3.Text = "下一任务：";
            // 
            // txtNextTask
            // 
            this.txtNextTask.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtNextTask.Location = new System.Drawing.Point(531, 3);
            this.txtNextTask.Name = "txtNextTask";
            this.txtNextTask.Size = new System.Drawing.Size(253, 32);
            this.txtNextTask.TabIndex = 5;
            // 
            // btnDeviceError
            // 
            this.btnDeviceError.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeviceError.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDeviceError.Location = new System.Drawing.Point(9, 568);
            this.btnDeviceError.Name = "btnDeviceError";
            this.btnDeviceError.Size = new System.Drawing.Size(107, 28);
            this.btnDeviceError.TabIndex = 27;
            this.btnDeviceError.Text = "设备异常";
            this.btnDeviceError.Click += new System.EventHandler(this.btnDeviceError_Click);
            // 
            // btnPatternError
            // 
            this.btnPatternError.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPatternError.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPatternError.Location = new System.Drawing.Point(908, 568);
            this.btnPatternError.Name = "btnPatternError";
            this.btnPatternError.Size = new System.Drawing.Size(160, 28);
            this.btnPatternError.TabIndex = 28;
            this.btnPatternError.Text = "提交异常工件";
            this.btnPatternError.Click += new System.EventHandler(this.btnPatternError_Click);
            // 
            // radLabel5
            // 
            this.radLabel5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.radLabel5.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radLabel5.ForeColor = System.Drawing.Color.Red;
            this.radLabel5.Location = new System.Drawing.Point(9, 602);
            this.radLabel5.Name = "radLabel5";
            this.radLabel5.Size = new System.Drawing.Size(337, 25);
            this.radLabel5.TabIndex = 29;
            this.radLabel5.Text = "设备发生故障需要维修无法继续生产时，提交";
            this.radLabel5.Visible = false;
            // 
            // radLabel6
            // 
            this.radLabel6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.radLabel6.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radLabel6.ForeColor = System.Drawing.Color.Red;
            this.radLabel6.Location = new System.Drawing.Point(632, 602);
            this.radLabel6.Name = "radLabel6";
            this.radLabel6.Size = new System.Drawing.Size(436, 25);
            this.radLabel6.TabIndex = 30;
            this.radLabel6.Text = "当前锯切任务完毕后，如果有大板切割时发生异常时，提交";
            // 
            // cmbCurStatus
            // 
            this.cmbCurStatus.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
            this.cmbCurStatus.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.cmbCurStatus.Location = new System.Drawing.Point(323, 3);
            this.cmbCurStatus.Name = "cmbCurStatus";
            this.cmbCurStatus.ReadOnly = true;
            this.cmbCurStatus.Size = new System.Drawing.Size(84, 27);
            this.cmbCurStatus.TabIndex = 7;
            // 
            // cmbNextStatus
            // 
            this.cmbNextStatus.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
            this.cmbNextStatus.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.cmbNextStatus.Location = new System.Drawing.Point(790, 3);
            this.cmbNextStatus.Name = "cmbNextStatus";
            this.cmbNextStatus.ReadOnly = true;
            this.cmbNextStatus.Size = new System.Drawing.Size(84, 27);
            this.cmbNextStatus.TabIndex = 8;
            // 
            // gvbStackDetail
            // 
            this.gvbStackDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gvbStackDetail.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.gvbStackDetail.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.gvbStackDetail.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gvbStackDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvbStackDetail.Location = new System.Drawing.Point(0, 59);
            this.gvbStackDetail.Name = "gvbStackDetail";
            this.gvbStackDetail.RowHeadersVisible = false;
            this.gvbStackDetail.RowTemplate.Height = 23;
            this.gvbStackDetail.Size = new System.Drawing.Size(1068, 503);
            this.gvbStackDetail.TabIndex = 31;
            // 
            // CurTaskDetailControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.Controls.Add(this.gvbStackDetail);
            this.Controls.Add(this.radLabel6);
            this.Controls.Add(this.radLabel5);
            this.Controls.Add(this.btnDeviceError);
            this.Controls.Add(this.btnPatternError);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "CurTaskDetailControl";
            this.Size = new System.Drawing.Size(1073, 636);
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurTask)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNextTask)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnDeviceError)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnPatternError)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCurStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbNextStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvbStackDetail)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadTextBox txtCurTask;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private Telerik.WinControls.UI.RadLabel radLabel3;
        private Telerik.WinControls.UI.RadTextBox txtNextTask;
        private Telerik.WinControls.UI.RadLabel radLabel5;
        private Telerik.WinControls.UI.RadButton btnDeviceError;
        private Telerik.WinControls.UI.RadButton btnPatternError;
        private Telerik.WinControls.UI.RadLabel radLabel6;
        private Telerik.WinControls.UI.RadDropDownList cmbCurStatus;
        private Telerik.WinControls.UI.RadDropDownList cmbNextStatus;
        private System.Windows.Forms.DataGridView gvbStackDetail;
    }
}
