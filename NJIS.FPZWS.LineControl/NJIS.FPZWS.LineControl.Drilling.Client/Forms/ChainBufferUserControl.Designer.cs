namespace NJIS.FPZWS.LineControl.Drilling.Client.Forms
{
    partial class ChainBufferUserControl
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
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn1 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn2 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn3 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn4 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn5 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            this.radPanel1 = new Telerik.WinControls.UI.RadPanel();
            this.rgvMain = new Telerik.WinControls.UI.RadGridView();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.rtxtCode = new Telerik.WinControls.UI.RadTextBox();
            this.radLabel3 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel4 = new Telerik.WinControls.UI.RadLabel();
            this.rtxtSize = new Telerik.WinControls.UI.RadTextBox();
            this.rtxtStatus = new Telerik.WinControls.UI.RadTextBox();
            this.rtxtRemark = new Telerik.WinControls.UI.RadTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).BeginInit();
            this.radPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rgvMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvMain.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rtxtCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rtxtSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rtxtStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rtxtRemark)).BeginInit();
            this.SuspendLayout();
            // 
            // radPanel1
            // 
            this.radPanel1.Controls.Add(this.rtxtRemark);
            this.radPanel1.Controls.Add(this.rtxtStatus);
            this.radPanel1.Controls.Add(this.rtxtSize);
            this.radPanel1.Controls.Add(this.radLabel4);
            this.radPanel1.Controls.Add(this.radLabel3);
            this.radPanel1.Controls.Add(this.rtxtCode);
            this.radPanel1.Controls.Add(this.radLabel2);
            this.radPanel1.Controls.Add(this.radLabel1);
            this.radPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.radPanel1.Location = new System.Drawing.Point(0, 0);
            this.radPanel1.Name = "radPanel1";
            this.radPanel1.Size = new System.Drawing.Size(415, 90);
            this.radPanel1.TabIndex = 0;
            // 
            // rgvMain
            // 
            this.rgvMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rgvMain.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rgvMain.Location = new System.Drawing.Point(0, 90);
            // 
            // 
            // 
            this.rgvMain.MasterTemplate.AllowAddNewRow = false;
            this.rgvMain.MasterTemplate.AllowCellContextMenu = false;
            this.rgvMain.MasterTemplate.AllowColumnHeaderContextMenu = false;
            this.rgvMain.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            gridViewTextBoxColumn1.HeaderText = "板件";
            gridViewTextBoxColumn1.Name = "PartId";
            gridViewTextBoxColumn1.Width = 93;
            gridViewTextBoxColumn2.HeaderText = "批次";
            gridViewTextBoxColumn2.Name = "BatchName";
            gridViewTextBoxColumn2.Width = 93;
            gridViewTextBoxColumn3.HeaderText = "订单";
            gridViewTextBoxColumn3.Name = "OrderNumber";
            gridViewTextBoxColumn3.Width = 93;
            gridViewTextBoxColumn4.HeaderText = "长";
            gridViewTextBoxColumn4.Name = "Length";
            gridViewTextBoxColumn4.Width = 54;
            gridViewTextBoxColumn5.HeaderText = "宽";
            gridViewTextBoxColumn5.Name = "Width";
            gridViewTextBoxColumn5.Width = 65;
            this.rgvMain.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2,
            gridViewTextBoxColumn3,
            gridViewTextBoxColumn4,
            gridViewTextBoxColumn5});
            this.rgvMain.MasterTemplate.EnableFiltering = true;
            this.rgvMain.MasterTemplate.MultiSelect = true;
            this.rgvMain.MasterTemplate.ShowFilteringRow = false;
            this.rgvMain.MasterTemplate.ShowHeaderCellButtons = true;
            this.rgvMain.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.rgvMain.Name = "rgvMain";
            this.rgvMain.ShowGroupPanel = false;
            this.rgvMain.ShowHeaderCellButtons = true;
            this.rgvMain.Size = new System.Drawing.Size(415, 299);
            this.rgvMain.TabIndex = 0;
            // 
            // radLabel1
            // 
            this.radLabel1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radLabel1.Location = new System.Drawing.Point(3, 12);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(41, 25);
            this.radLabel1.TabIndex = 0;
            this.radLabel1.Text = "编号";
            // 
            // radLabel2
            // 
            this.radLabel2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radLabel2.Location = new System.Drawing.Point(167, 14);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(41, 25);
            this.radLabel2.TabIndex = 1;
            this.radLabel2.Text = "状态";
            // 
            // rtxtCode
            // 
            this.rtxtCode.Enabled = false;
            this.rtxtCode.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtxtCode.Location = new System.Drawing.Point(50, 12);
            this.rtxtCode.Name = "rtxtCode";
            this.rtxtCode.ReadOnly = true;
            this.rtxtCode.Size = new System.Drawing.Size(111, 27);
            this.rtxtCode.TabIndex = 0;
            // 
            // radLabel3
            // 
            this.radLabel3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radLabel3.Location = new System.Drawing.Point(167, 45);
            this.radLabel3.Name = "radLabel3";
            this.radLabel3.Size = new System.Drawing.Size(41, 25);
            this.radLabel3.TabIndex = 4;
            this.radLabel3.Text = "备注";
            // 
            // radLabel4
            // 
            this.radLabel4.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radLabel4.Location = new System.Drawing.Point(3, 47);
            this.radLabel4.Name = "radLabel4";
            this.radLabel4.Size = new System.Drawing.Size(41, 25);
            this.radLabel4.TabIndex = 6;
            this.radLabel4.Text = "容量";
            // 
            // rtxtSize
            // 
            this.rtxtSize.Enabled = false;
            this.rtxtSize.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtxtSize.Location = new System.Drawing.Point(50, 47);
            this.rtxtSize.Name = "rtxtSize";
            this.rtxtSize.ReadOnly = true;
            this.rtxtSize.Size = new System.Drawing.Size(111, 27);
            this.rtxtSize.TabIndex = 1;
            // 
            // rtxtStatus
            // 
            this.rtxtStatus.Enabled = false;
            this.rtxtStatus.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtxtStatus.Location = new System.Drawing.Point(214, 14);
            this.rtxtStatus.Name = "rtxtStatus";
            this.rtxtStatus.ReadOnly = true;
            this.rtxtStatus.Size = new System.Drawing.Size(111, 27);
            this.rtxtStatus.TabIndex = 2;
            // 
            // rtxtRemark
            // 
            this.rtxtRemark.Enabled = false;
            this.rtxtRemark.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtxtRemark.Location = new System.Drawing.Point(214, 45);
            this.rtxtRemark.Name = "rtxtRemark";
            this.rtxtRemark.ReadOnly = true;
            this.rtxtRemark.Size = new System.Drawing.Size(111, 27);
            this.rtxtRemark.TabIndex = 3;
            // 
            // ChainBufferUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rgvMain);
            this.Controls.Add(this.radPanel1);
            this.Name = "ChainBufferUserControl";
            this.Size = new System.Drawing.Size(415, 389);
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).EndInit();
            this.radPanel1.ResumeLayout(false);
            this.radPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rgvMain.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rtxtCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rtxtSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rtxtStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rtxtRemark)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadPanel radPanel1;
        private Telerik.WinControls.UI.RadGridView rgvMain;
        private Telerik.WinControls.UI.RadLabel radLabel3;
        private Telerik.WinControls.UI.RadTextBox rtxtCode;
        private Telerik.WinControls.UI.RadLabel radLabel2;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadLabel radLabel4;
        private Telerik.WinControls.UI.RadTextBox rtxtRemark;
        private Telerik.WinControls.UI.RadTextBox rtxtStatus;
        private Telerik.WinControls.UI.RadTextBox rtxtSize;
    }
}
