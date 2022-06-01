namespace NJIS.FPZWS.LineControl.Cutting.Server.Client.Views.Controls.ModuleControl
{
    partial class DeviceManageControl
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
            this.btnSearch = new Telerik.WinControls.UI.RadButton();
            this.btnSaveDeviceInfo = new Telerik.WinControls.UI.RadButton();
            this.btnAddDeviceInfo = new Telerik.WinControls.UI.RadButton();
            this.gridViewBase1 = new NJIS.FPZWS.UI.Common.Controls.GridViewBase();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.cmbProcessName = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.btnSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSaveDeviceInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAddDeviceInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnSearch.Location = new System.Drawing.Point(242, 6);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(63, 24);
            this.btnSearch.TabIndex = 5;
            this.btnSearch.Text = "查询";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnSaveDeviceInfo
            // 
            this.btnSaveDeviceInfo.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnSaveDeviceInfo.Location = new System.Drawing.Point(377, 6);
            this.btnSaveDeviceInfo.Name = "btnSaveDeviceInfo";
            this.btnSaveDeviceInfo.Size = new System.Drawing.Size(59, 24);
            this.btnSaveDeviceInfo.TabIndex = 7;
            this.btnSaveDeviceInfo.Text = "保存";
            this.btnSaveDeviceInfo.Click += new System.EventHandler(this.btnSaveDeviceInfo_Click);
            // 
            // btnAddDeviceInfo
            // 
            this.btnAddDeviceInfo.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnAddDeviceInfo.Location = new System.Drawing.Point(311, 5);
            this.btnAddDeviceInfo.Name = "btnAddDeviceInfo";
            this.btnAddDeviceInfo.Size = new System.Drawing.Size(60, 24);
            this.btnAddDeviceInfo.TabIndex = 6;
            this.btnAddDeviceInfo.Text = "添加";
            this.btnAddDeviceInfo.Click += new System.EventHandler(this.btnAddDeviceInfo_Click);
            // 
            // gridViewBase1
            // 
            this.gridViewBase1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridViewBase1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.gridViewBase1.DataSource = null;
            this.gridViewBase1.Location = new System.Drawing.Point(3, 33);
            this.gridViewBase1.Name = "gridViewBase1";
            this.gridViewBase1.Size = new System.Drawing.Size(583, 426);
            this.gridViewBase1.TabIndex = 4;
            // 
            // radLabel1
            // 
            this.radLabel1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radLabel1.Location = new System.Drawing.Point(3, 6);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(90, 25);
            this.radLabel1.TabIndex = 8;
            this.radLabel1.Text = "当前工段：";
            // 
            // cmbProcessName
            // 
            this.cmbProcessName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProcessName.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.cmbProcessName.FormattingEnabled = true;
            this.cmbProcessName.Location = new System.Drawing.Point(87, 3);
            this.cmbProcessName.Name = "cmbProcessName";
            this.cmbProcessName.Size = new System.Drawing.Size(149, 29);
            this.cmbProcessName.TabIndex = 9;
            // 
            // DeviceManageControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.Controls.Add(this.cmbProcessName);
            this.Controls.Add(this.radLabel1);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.btnSaveDeviceInfo);
            this.Controls.Add(this.btnAddDeviceInfo);
            this.Controls.Add(this.gridViewBase1);
            this.Name = "DeviceManageControl";
            this.Size = new System.Drawing.Size(589, 462);
            ((System.ComponentModel.ISupportInitialize)(this.btnSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSaveDeviceInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAddDeviceInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadButton btnSearch;
        private Telerik.WinControls.UI.RadButton btnSaveDeviceInfo;
        private Telerik.WinControls.UI.RadButton btnAddDeviceInfo;
        private UI.Common.Controls.GridViewBase gridViewBase1;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private System.Windows.Forms.ComboBox cmbProcessName;
    }
}
