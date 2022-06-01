namespace NJIS.FPZWS.LineControl.Cutting.Server.Client.Views.Controls.ModuleControl
{
    partial class CuttingSimulationControl
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
            this.btnPushMDB = new Telerik.WinControls.UI.RadButton();
            this.btnGetStackList = new Telerik.WinControls.UI.RadButton();
            this.gridViewBase1 = new NJIS.FPZWS.UI.Common.Controls.GridViewBase();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpPlanDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnPushMDB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnGetStackList)).BeginInit();
            this.SuspendLayout();
            // 
            // radLabel1
            // 
            this.radLabel1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radLabel1.Location = new System.Drawing.Point(3, 3);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(90, 25);
            this.radLabel1.TabIndex = 29;
            this.radLabel1.Text = "生产日期：";
            // 
            // dtpPlanDate
            // 
            this.dtpPlanDate.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpPlanDate.Location = new System.Drawing.Point(99, 2);
            this.dtpPlanDate.Name = "dtpPlanDate";
            this.dtpPlanDate.Size = new System.Drawing.Size(164, 27);
            this.dtpPlanDate.TabIndex = 28;
            this.dtpPlanDate.TabStop = false;
            this.dtpPlanDate.Text = "2018年10月20日";
            this.dtpPlanDate.Value = new System.DateTime(2018, 10, 20, 0, 0, 0, 0);
            // 
            // btnPushMDB
            // 
            this.btnPushMDB.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnPushMDB.Location = new System.Drawing.Point(341, 3);
            this.btnPushMDB.Name = "btnPushMDB";
            this.btnPushMDB.Size = new System.Drawing.Size(76, 28);
            this.btnPushMDB.TabIndex = 25;
            this.btnPushMDB.Text = "板件完成";
            // 
            // btnGetStackList
            // 
            this.btnGetStackList.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnGetStackList.Location = new System.Drawing.Point(269, 1);
            this.btnGetStackList.Name = "btnGetStackList";
            this.btnGetStackList.Size = new System.Drawing.Size(66, 28);
            this.btnGetStackList.TabIndex = 26;
            this.btnGetStackList.Text = "查询";
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
            this.gridViewBase1.Location = new System.Drawing.Point(3, 34);
            this.gridViewBase1.Name = "gridViewBase1";
            this.gridViewBase1.ReadOnly = true;
            this.gridViewBase1.ShowCheckBox = false;
            this.gridViewBase1.ShowRowHeaderColumn = false;
            this.gridViewBase1.ShowRowNumber = true;
            this.gridViewBase1.Size = new System.Drawing.Size(621, 457);
            this.gridViewBase1.TabIndex = 27;
            // 
            // CuttingSimulationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.Controls.Add(this.radLabel1);
            this.Controls.Add(this.dtpPlanDate);
            this.Controls.Add(this.btnPushMDB);
            this.Controls.Add(this.btnGetStackList);
            this.Controls.Add(this.gridViewBase1);
            this.Name = "CuttingSimulationControl";
            this.Size = new System.Drawing.Size(627, 494);
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpPlanDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnPushMDB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnGetStackList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadDateTimePicker dtpPlanDate;
        private Telerik.WinControls.UI.RadButton btnPushMDB;
        private Telerik.WinControls.UI.RadButton btnGetStackList;
        private UI.Common.Controls.GridViewBase gridViewBase1;
    }
}
