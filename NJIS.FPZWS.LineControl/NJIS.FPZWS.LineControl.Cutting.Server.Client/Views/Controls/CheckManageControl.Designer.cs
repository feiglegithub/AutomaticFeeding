namespace NJIS.FPZWS.LineControl.Cutting.Server.Client.Views.Controls
{
    partial class CheckManageControl
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
            this.gridViewBase1 = new NJIS.FPZWS.UI.Common.Controls.GridViewBase();
            this.btnSearch = new Telerik.WinControls.UI.RadButton();
            this.btnSaveDeviceInfo = new Telerik.WinControls.UI.RadButton();
            this.btnAddDeviceInfo = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.btnSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSaveDeviceInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAddDeviceInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // gridViewBase1
            // 
            this.gridViewBase1.AllowAddNewRow = false;
            this.gridViewBase1.AllowCheckSort = false;
            this.gridViewBase1.AllowDeleteRow = false;
            this.gridViewBase1.AllowEditRow = true;
            this.gridViewBase1.AllowSelectAll = true;
            this.gridViewBase1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.gridViewBase1.ChangedValueForeColor = System.Drawing.Color.DeepPink;
            this.gridViewBase1.DataSource = null;
            this.gridViewBase1.DefaultValueForeColor = System.Drawing.Color.Black;
            this.gridViewBase1.EnableFiltering = true;
            this.gridViewBase1.EnablePaging = false;
            this.gridViewBase1.EnableSorting = true;
            this.gridViewBase1.Location = new System.Drawing.Point(3, 33);
            this.gridViewBase1.Name = "gridViewBase1";
            this.gridViewBase1.NewAddRowForeColor = System.Drawing.Color.Fuchsia;
            this.gridViewBase1.PageSize = 20;
            this.gridViewBase1.ReadOnly = false;
            this.gridViewBase1.ShowCheckBox = false;
            this.gridViewBase1.ShowRowHeaderColumn = false;
            this.gridViewBase1.ShowRowNumber = true;
            this.gridViewBase1.Size = new System.Drawing.Size(710, 421);
            this.gridViewBase1.TabIndex = 0;
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnSearch.Location = new System.Drawing.Point(514, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(63, 24);
            this.btnSearch.TabIndex = 8;
            this.btnSearch.Text = "查询";
            // 
            // btnSaveDeviceInfo
            // 
            this.btnSaveDeviceInfo.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnSaveDeviceInfo.Location = new System.Drawing.Point(649, 3);
            this.btnSaveDeviceInfo.Name = "btnSaveDeviceInfo";
            this.btnSaveDeviceInfo.Size = new System.Drawing.Size(59, 24);
            this.btnSaveDeviceInfo.TabIndex = 10;
            this.btnSaveDeviceInfo.Text = "保存";
            // 
            // btnAddDeviceInfo
            // 
            this.btnAddDeviceInfo.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnAddDeviceInfo.Location = new System.Drawing.Point(583, 2);
            this.btnAddDeviceInfo.Name = "btnAddDeviceInfo";
            this.btnAddDeviceInfo.Size = new System.Drawing.Size(60, 24);
            this.btnAddDeviceInfo.TabIndex = 9;
            this.btnAddDeviceInfo.Text = "添加";
            // 
            // CheckManageControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.btnSaveDeviceInfo);
            this.Controls.Add(this.btnAddDeviceInfo);
            this.Controls.Add(this.gridViewBase1);
            this.Name = "CheckManageControl";
            this.Size = new System.Drawing.Size(716, 457);
            ((System.ComponentModel.ISupportInitialize)(this.btnSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSaveDeviceInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAddDeviceInfo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private UI.Common.Controls.GridViewBase gridViewBase1;
        private Telerik.WinControls.UI.RadButton btnSearch;
        private Telerik.WinControls.UI.RadButton btnSaveDeviceInfo;
        private Telerik.WinControls.UI.RadButton btnAddDeviceInfo;
    }
}
