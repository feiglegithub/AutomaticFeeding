namespace NJIS.FPZWS.LineControl.Cutting.Simulator.Controls
{
    partial class WMSStockSimulationControl
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
            this.cmbWay = new System.Windows.Forms.ComboBox();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.dtpPlanDate = new Telerik.WinControls.UI.RadDateTimePicker();
            this.btnStockMaterial = new Telerik.WinControls.UI.RadButton();
            this.btnBeginStockMaterial = new Telerik.WinControls.UI.RadButton();
            this.btnSearch = new Telerik.WinControls.UI.RadButton();
            this.gridViewBase1 = new NJIS.FPZWS.UI.Common.Controls.GridViewBase();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpPlanDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnStockMaterial)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnBeginStockMaterial)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSearch)).BeginInit();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.Controls.Add(this.radLabel1);
            this.flowLayoutPanel1.Controls.Add(this.dtpPlanDate);
            this.flowLayoutPanel1.Controls.Add(this.cmbWay);
            this.flowLayoutPanel1.Controls.Add(this.btnSearch);
            this.flowLayoutPanel1.Controls.Add(this.btnBeginStockMaterial);
            this.flowLayoutPanel1.Controls.Add(this.btnStockMaterial);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(869, 38);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // cmbWay
            // 
            this.cmbWay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWay.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbWay.FormattingEnabled = true;
            this.cmbWay.Items.AddRange(new object[] {
            "未备料",
            "备料完毕"});
            this.cmbWay.Location = new System.Drawing.Point(269, 3);
            this.cmbWay.Name = "cmbWay";
            this.cmbWay.Size = new System.Drawing.Size(121, 29);
            this.cmbWay.TabIndex = 32;
            // 
            // radLabel1
            // 
            this.radLabel1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radLabel1.Location = new System.Drawing.Point(3, 3);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(90, 25);
            this.radLabel1.TabIndex = 31;
            this.radLabel1.Text = "生产日期：";
            // 
            // dtpPlanDate
            // 
            this.dtpPlanDate.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpPlanDate.Location = new System.Drawing.Point(99, 3);
            this.dtpPlanDate.Name = "dtpPlanDate";
            this.dtpPlanDate.Size = new System.Drawing.Size(164, 27);
            this.dtpPlanDate.TabIndex = 30;
            this.dtpPlanDate.TabStop = false;
            this.dtpPlanDate.Text = "2018年10月20日";
            this.dtpPlanDate.Value = new System.DateTime(2018, 10, 20, 0, 0, 0, 0);
            // 
            // btnStockMaterial
            // 
            this.btnStockMaterial.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnStockMaterial.Location = new System.Drawing.Point(550, 3);
            this.btnStockMaterial.Name = "btnStockMaterial";
            this.btnStockMaterial.Size = new System.Drawing.Size(76, 28);
            this.btnStockMaterial.TabIndex = 26;
            this.btnStockMaterial.Text = "备料完毕";
            this.btnStockMaterial.Click += new System.EventHandler(this.btnStockMaterial_Click);
            // 
            // btnBeginStockMaterial
            // 
            this.btnBeginStockMaterial.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnBeginStockMaterial.Location = new System.Drawing.Point(468, 3);
            this.btnBeginStockMaterial.Name = "btnBeginStockMaterial";
            this.btnBeginStockMaterial.Size = new System.Drawing.Size(76, 28);
            this.btnBeginStockMaterial.TabIndex = 28;
            this.btnBeginStockMaterial.Text = "开始备料";
            this.btnBeginStockMaterial.Click += new System.EventHandler(this.btnBeginStockMaterial_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnSearch.Location = new System.Drawing.Point(396, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(66, 28);
            this.btnSearch.TabIndex = 29;
            this.btnSearch.Text = "查询";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // gridViewBase1
            // 
            this.gridViewBase1.AllowAddNewRow = false;
            this.gridViewBase1.AllowDeleteRow = false;
            this.gridViewBase1.AllowEditRow = true;
            this.gridViewBase1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridViewBase1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.gridViewBase1.DataSource = null;
            this.gridViewBase1.Location = new System.Drawing.Point(6, 47);
            this.gridViewBase1.Name = "gridViewBase1";
            this.gridViewBase1.ReadOnly = true;
            this.gridViewBase1.ShowCheckBox = true;
            this.gridViewBase1.ShowRowHeaderColumn = false;
            this.gridViewBase1.ShowRowNumber = true;
            this.gridViewBase1.Size = new System.Drawing.Size(866, 473);
            this.gridViewBase1.TabIndex = 1;
            // 
            // WMSStockSimulationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.Controls.Add(this.gridViewBase1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "WMSStockSimulationControl";
            this.Size = new System.Drawing.Size(875, 531);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpPlanDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnStockMaterial)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnBeginStockMaterial)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSearch)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadDateTimePicker dtpPlanDate;
        private System.Windows.Forms.ComboBox cmbWay;
        private Telerik.WinControls.UI.RadButton btnSearch;
        private Telerik.WinControls.UI.RadButton btnBeginStockMaterial;
        private Telerik.WinControls.UI.RadButton btnStockMaterial;
        private UI.Common.Controls.GridViewBase gridViewBase1;
    }
}
