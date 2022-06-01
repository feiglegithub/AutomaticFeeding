namespace NJIS.FPZWS.LineControl.Manager.Views
{
    partial class BatchGroupView
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
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.dtpPlanDate = new Telerik.WinControls.UI.RadDateTimePicker();
            this.gvbBatchGroup = new NJIS.FPZWS.UI.Common.Controls.GridViewBase();
            this.btnSave = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.btnSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpPlanDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSave)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnSearch.ForeColor = System.Drawing.Color.Black;
            this.btnSearch.Location = new System.Drawing.Point(246, 4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(56, 23);
            this.btnSearch.TabIndex = 13;
            this.btnSearch.Text = "查询";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // radLabel1
            // 
            this.radLabel1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.radLabel1.ForeColor = System.Drawing.Color.Black;
            this.radLabel1.Location = new System.Drawing.Point(8, 3);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(74, 25);
            this.radLabel1.TabIndex = 12;
            this.radLabel1.Text = "计划日期";
            // 
            // dtpPlanDate
            // 
            this.dtpPlanDate.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.dtpPlanDate.ForeColor = System.Drawing.Color.Black;
            this.dtpPlanDate.Location = new System.Drawing.Point(97, 3);
            this.dtpPlanDate.Name = "dtpPlanDate";
            this.dtpPlanDate.Size = new System.Drawing.Size(143, 27);
            this.dtpPlanDate.TabIndex = 11;
            this.dtpPlanDate.TabStop = false;
            this.dtpPlanDate.Text = "2020年1月8日";
            this.dtpPlanDate.Value = new System.DateTime(2020, 1, 8, 0, 0, 0, 0);
            // 
            // gvbBatchGroup
            // 
            this.gvbBatchGroup.AllowAddNewRow = false;
            this.gvbBatchGroup.AllowCheckSort = false;
            this.gvbBatchGroup.AllowDeleteRow = false;
            this.gvbBatchGroup.AllowEditRow = true;
            this.gvbBatchGroup.AllowSelectAll = true;
            this.gvbBatchGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gvbBatchGroup.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.gvbBatchGroup.ChangedValueForeColor = System.Drawing.Color.DeepPink;
            this.gvbBatchGroup.DataSource = null;
            this.gvbBatchGroup.DefaultValueForeColor = System.Drawing.Color.Black;
            this.gvbBatchGroup.EnableFiltering = true;
            this.gvbBatchGroup.EnablePaging = false;
            this.gvbBatchGroup.EnableSorting = true;
            this.gvbBatchGroup.Location = new System.Drawing.Point(3, 34);
            this.gvbBatchGroup.Name = "gvbBatchGroup";
            this.gvbBatchGroup.NewAddRowForeColor = System.Drawing.Color.Fuchsia;
            this.gvbBatchGroup.PageSize = 20;
            this.gvbBatchGroup.ReadOnly = false;
            this.gvbBatchGroup.ShowCheckBox = false;
            this.gvbBatchGroup.ShowRowHeaderColumn = false;
            this.gvbBatchGroup.ShowRowNumber = true;
            this.gvbBatchGroup.Size = new System.Drawing.Size(949, 671);
            this.gvbBatchGroup.TabIndex = 7;
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnSave.ForeColor = System.Drawing.Color.Black;
            this.btnSave.Location = new System.Drawing.Point(887, 7);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(56, 23);
            this.btnSave.TabIndex = 13;
            this.btnSave.Text = "保存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // BatchGroupView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.radLabel1);
            this.Controls.Add(this.dtpPlanDate);
            this.Controls.Add(this.gvbBatchGroup);
            this.Name = "BatchGroupView";
            this.Size = new System.Drawing.Size(955, 708);
            ((System.ComponentModel.ISupportInitialize)(this.btnSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpPlanDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSave)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UI.Common.Controls.GridViewBase gvbBatchGroup;
        private Telerik.WinControls.UI.RadButton btnSearch;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadDateTimePicker dtpPlanDate;
        private Telerik.WinControls.UI.RadButton btnSave;
    }
}
