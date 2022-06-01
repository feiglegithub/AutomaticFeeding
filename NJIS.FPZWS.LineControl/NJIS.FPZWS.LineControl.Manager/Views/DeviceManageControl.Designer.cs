namespace NJIS.FPZWS.LineControl.Manager.Views
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
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.cmbProcessName = new System.Windows.Forms.ComboBox();
            this.gridViewBase1 = new NJIS.FPZWS.UI.Common.Controls.GridViewBase();
            ((System.ComponentModel.ISupportInitialize)(this.btnSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSaveDeviceInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAddDeviceInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnSearch.ForeColor = System.Drawing.Color.Black;
            this.btnSearch.Location = new System.Drawing.Point(267, 5);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(63, 24);
            this.btnSearch.TabIndex = 5;
            this.btnSearch.Text = "查询";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnSaveDeviceInfo
            // 
            this.btnSaveDeviceInfo.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnSaveDeviceInfo.ForeColor = System.Drawing.Color.Black;
            this.btnSaveDeviceInfo.Location = new System.Drawing.Point(402, 5);
            this.btnSaveDeviceInfo.Name = "btnSaveDeviceInfo";
            this.btnSaveDeviceInfo.Size = new System.Drawing.Size(59, 25);
            this.btnSaveDeviceInfo.TabIndex = 7;
            this.btnSaveDeviceInfo.Text = "保存";
            this.btnSaveDeviceInfo.Click += new System.EventHandler(this.btnSaveDeviceInfo_Click);
            // 
            // btnAddDeviceInfo
            // 
            this.btnAddDeviceInfo.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnAddDeviceInfo.ForeColor = System.Drawing.Color.Black;
            this.btnAddDeviceInfo.Location = new System.Drawing.Point(336, 5);
            this.btnAddDeviceInfo.Name = "btnAddDeviceInfo";
            this.btnAddDeviceInfo.Size = new System.Drawing.Size(60, 25);
            this.btnAddDeviceInfo.TabIndex = 6;
            this.btnAddDeviceInfo.Text = "添加";
            this.btnAddDeviceInfo.Click += new System.EventHandler(this.btnAddDeviceInfo_Click);
            // 
            // radLabel1
            // 
            this.radLabel1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.radLabel1.ForeColor = System.Drawing.Color.Black;
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
            this.cmbProcessName.ForeColor = System.Drawing.Color.Black;
            this.cmbProcessName.FormattingEnabled = true;
            this.cmbProcessName.Location = new System.Drawing.Point(99, 4);
            this.cmbProcessName.Name = "cmbProcessName";
            this.cmbProcessName.Size = new System.Drawing.Size(149, 29);
            this.cmbProcessName.TabIndex = 9;
            // 
            // gridViewBase1
            // 
            this.gridViewBase1.AllowAddNewRow = false;
            this.gridViewBase1.AllowCheckSort = false;
            this.gridViewBase1.AllowDeleteRow = false;
            this.gridViewBase1.AllowEditRow = true;
            this.gridViewBase1.AllowSelectAll = true;
            this.gridViewBase1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridViewBase1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.gridViewBase1.ChangedValueForeColor = System.Drawing.Color.DeepPink;
            this.gridViewBase1.DataSource = null;
            this.gridViewBase1.DefaultValueForeColor = System.Drawing.Color.Black;
            this.gridViewBase1.EnableFiltering = true;
            this.gridViewBase1.EnablePaging = false;
            this.gridViewBase1.EnableSorting = true;
            this.gridViewBase1.Location = new System.Drawing.Point(3, 37);
            this.gridViewBase1.Name = "gridViewBase1";
            this.gridViewBase1.NewAddRowForeColor = System.Drawing.Color.Fuchsia;
            this.gridViewBase1.PageSize = 20;
            this.gridViewBase1.ReadOnly = false;
            this.gridViewBase1.ShowCheckBox = false;
            this.gridViewBase1.ShowRowHeaderColumn = false;
            this.gridViewBase1.ShowRowNumber = true;
            this.gridViewBase1.Size = new System.Drawing.Size(1016, 669);
            this.gridViewBase1.TabIndex = 4;
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
            this.Size = new System.Drawing.Size(1022, 709);
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
