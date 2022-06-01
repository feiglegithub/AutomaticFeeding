namespace NJIS.FPZWS.LineControl.Cutting.Simulator.Controls
{
    partial class WMSLoadSimulationControl
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
            this.dtpPlanDate = new Telerik.WinControls.UI.RadDateTimePicker();
            this.btnLoadMaterial = new Telerik.WinControls.UI.RadButton();
            this.btnLoadingMaterial = new Telerik.WinControls.UI.RadButton();
            this.btnLoadedMaterial = new Telerik.WinControls.UI.RadButton();
            this.gridViewBase1 = new NJIS.FPZWS.UI.Common.Controls.GridViewBase();
            this.btnSearch = new Telerik.WinControls.UI.RadButton();
            this.cmbWay = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpPlanDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnLoadMaterial)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnLoadingMaterial)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnLoadedMaterial)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSearch)).BeginInit();
            this.SuspendLayout();
            // 
            // radLabel1
            // 
            this.radLabel1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radLabel1.Location = new System.Drawing.Point(3, 3);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(90, 25);
            this.radLabel1.TabIndex = 24;
            this.radLabel1.Text = "生产日期：";
            // 
            // dtpPlanDate
            // 
            this.dtpPlanDate.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpPlanDate.Location = new System.Drawing.Point(99, 3);
            this.dtpPlanDate.Name = "dtpPlanDate";
            this.dtpPlanDate.Size = new System.Drawing.Size(164, 27);
            this.dtpPlanDate.TabIndex = 23;
            this.dtpPlanDate.TabStop = false;
            this.dtpPlanDate.Text = "2018年10月20日";
            this.dtpPlanDate.Value = new System.DateTime(2018, 10, 20, 0, 0, 0, 0);
            // 
            // btnLoadMaterial
            // 
            this.btnLoadMaterial.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnLoadMaterial.Location = new System.Drawing.Point(476, 3);
            this.btnLoadMaterial.Name = "btnLoadMaterial";
            this.btnLoadMaterial.Size = new System.Drawing.Size(76, 28);
            this.btnLoadMaterial.TabIndex = 20;
            this.btnLoadMaterial.Text = "上料请求";
            this.btnLoadMaterial.Click += new System.EventHandler(this.btnPushMDB_Click);
            // 
            // btnLoadingMaterial
            // 
            this.btnLoadingMaterial.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnLoadingMaterial.Location = new System.Drawing.Point(558, 3);
            this.btnLoadingMaterial.Name = "btnLoadingMaterial";
            this.btnLoadingMaterial.Size = new System.Drawing.Size(76, 28);
            this.btnLoadingMaterial.TabIndex = 20;
            this.btnLoadingMaterial.Text = "上料中";
            this.btnLoadingMaterial.Click += new System.EventHandler(this.btnLoadingMaterial_Click);
            // 
            // btnLoadedMaterial
            // 
            this.btnLoadedMaterial.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnLoadedMaterial.Location = new System.Drawing.Point(640, 3);
            this.btnLoadedMaterial.Name = "btnLoadedMaterial";
            this.btnLoadedMaterial.Size = new System.Drawing.Size(76, 28);
            this.btnLoadedMaterial.TabIndex = 20;
            this.btnLoadedMaterial.Text = "上料完毕";
            this.btnLoadedMaterial.Click += new System.EventHandler(this.btnLoadedMaterial_Click);
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
            this.gridViewBase1.Location = new System.Drawing.Point(3, 69);
            this.gridViewBase1.Name = "gridViewBase1";
            this.gridViewBase1.ReadOnly = true;
            this.gridViewBase1.ShowCheckBox = false;
            this.gridViewBase1.ShowRowHeaderColumn = false;
            this.gridViewBase1.ShowRowNumber = true;
            this.gridViewBase1.Size = new System.Drawing.Size(722, 486);
            this.gridViewBase1.TabIndex = 22;
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnSearch.Location = new System.Drawing.Point(404, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(66, 28);
            this.btnSearch.TabIndex = 21;
            this.btnSearch.Text = "查询";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // cmbWay
            // 
            this.cmbWay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWay.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbWay.FormattingEnabled = true;
            this.cmbWay.Items.AddRange(new object[] {
            "待上料",
            "请求上料",
            "上料中"});
            this.cmbWay.Location = new System.Drawing.Point(269, 4);
            this.cmbWay.Name = "cmbWay";
            this.cmbWay.Size = new System.Drawing.Size(121, 29);
            this.cmbWay.TabIndex = 25;
            // 
            // WMSLoadSimulationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.Controls.Add(this.cmbWay);
            this.Controls.Add(this.radLabel1);
            this.Controls.Add(this.dtpPlanDate);
            this.Controls.Add(this.btnLoadedMaterial);
            this.Controls.Add(this.btnLoadingMaterial);
            this.Controls.Add(this.btnLoadMaterial);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.gridViewBase1);
            this.Name = "WMSLoadSimulationControl";
            this.Size = new System.Drawing.Size(728, 567);
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpPlanDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnLoadMaterial)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnLoadingMaterial)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnLoadedMaterial)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSearch)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadDateTimePicker dtpPlanDate;
        private Telerik.WinControls.UI.RadButton btnLoadMaterial;
        private UI.Common.Controls.GridViewBase gridViewBase1;
        private Telerik.WinControls.UI.RadButton btnLoadingMaterial;
        private Telerik.WinControls.UI.RadButton btnLoadedMaterial;
        private Telerik.WinControls.UI.RadButton btnSearch;
        private System.Windows.Forms.ComboBox cmbWay;
    }
}
