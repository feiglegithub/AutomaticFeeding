namespace NJIS.FPZWS.LineControl.PartMill.Simulator.Views
{
    partial class LineTaskView
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
            this.btnExecute = new Telerik.WinControls.UI.RadButton();
            this.tipList = new Telerik.WinControls.UI.RadListControl();
            this.btnAuto = new Telerik.WinControls.UI.RadButton();
            this.btnStopAuto = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.btnSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnExecute)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tipList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAuto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnStopAuto)).BeginInit();
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
            this.gridViewBase1.Size = new System.Drawing.Size(947, 494);
            this.gridViewBase1.TabIndex = 0;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(554, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(54, 24);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "查询";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnExecute
            // 
            this.btnExecute.Location = new System.Drawing.Point(647, 3);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(54, 24);
            this.btnExecute.TabIndex = 1;
            this.btnExecute.Text = "执行";
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // tipList
            // 
            this.tipList.Location = new System.Drawing.Point(3, 535);
            this.tipList.Name = "tipList";
            this.tipList.Size = new System.Drawing.Size(947, 173);
            this.tipList.TabIndex = 2;
            this.tipList.Text = "radListControl1";
            // 
            // btnAuto
            // 
            this.btnAuto.Location = new System.Drawing.Point(725, 3);
            this.btnAuto.Name = "btnAuto";
            this.btnAuto.Size = new System.Drawing.Size(62, 24);
            this.btnAuto.TabIndex = 3;
            this.btnAuto.Text = "自动执行";
            this.btnAuto.Click += new System.EventHandler(this.btnAuto_Click);
            // 
            // btnStopAuto
            // 
            this.btnStopAuto.Enabled = false;
            this.btnStopAuto.Location = new System.Drawing.Point(804, 3);
            this.btnStopAuto.Name = "btnStopAuto";
            this.btnStopAuto.Size = new System.Drawing.Size(66, 24);
            this.btnStopAuto.TabIndex = 4;
            this.btnStopAuto.Text = "停止自动";
            this.btnStopAuto.Click += new System.EventHandler(this.btnStopAuto_Click);
            // 
            // LineTaskView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.Controls.Add(this.btnStopAuto);
            this.Controls.Add(this.btnAuto);
            this.Controls.Add(this.tipList);
            this.Controls.Add(this.btnExecute);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.gridViewBase1);
            this.Name = "LineTaskView";
            this.Size = new System.Drawing.Size(953, 711);
            ((System.ComponentModel.ISupportInitialize)(this.btnSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnExecute)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tipList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAuto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnStopAuto)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private UI.Common.Controls.GridViewBase gridViewBase1;
        private Telerik.WinControls.UI.RadButton btnSearch;
        private Telerik.WinControls.UI.RadButton btnExecute;
        private Telerik.WinControls.UI.RadListControl tipList;
        private Telerik.WinControls.UI.RadButton btnAuto;
        private Telerik.WinControls.UI.RadButton btnStopAuto;
    }
}
