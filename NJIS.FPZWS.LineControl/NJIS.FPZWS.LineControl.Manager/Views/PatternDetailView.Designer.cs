namespace NJIS.FPZWS.LineControl.Manager.Views
{
    partial class PatternDetailView
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
            this.gvbPatternDetail = new NJIS.FPZWS.UI.Common.Controls.GridViewBase();
            this.btnSearch = new Telerik.WinControls.UI.RadButton();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.txtBatchName = new Telerik.WinControls.UI.RadTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.btnSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBatchName)).BeginInit();
            this.SuspendLayout();
            // 
            // gvbPatternDetail
            // 
            this.gvbPatternDetail.AllowAddNewRow = false;
            this.gvbPatternDetail.AllowCheckSort = false;
            this.gvbPatternDetail.AllowDeleteRow = false;
            this.gvbPatternDetail.AllowEditRow = true;
            this.gvbPatternDetail.AllowSelectAll = true;
            this.gvbPatternDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gvbPatternDetail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.gvbPatternDetail.ChangedValueForeColor = System.Drawing.Color.DeepPink;
            this.gvbPatternDetail.DataSource = null;
            this.gvbPatternDetail.DefaultValueForeColor = System.Drawing.Color.Black;
            this.gvbPatternDetail.EnableFiltering = true;
            this.gvbPatternDetail.EnablePaging = false;
            this.gvbPatternDetail.EnableSorting = true;
            this.gvbPatternDetail.Location = new System.Drawing.Point(0, 33);
            this.gvbPatternDetail.Name = "gvbPatternDetail";
            this.gvbPatternDetail.NewAddRowForeColor = System.Drawing.Color.Fuchsia;
            this.gvbPatternDetail.PageSize = 20;
            this.gvbPatternDetail.ReadOnly = false;
            this.gvbPatternDetail.ShowCheckBox = false;
            this.gvbPatternDetail.ShowRowHeaderColumn = false;
            this.gvbPatternDetail.ShowRowNumber = true;
            this.gvbPatternDetail.Size = new System.Drawing.Size(946, 730);
            this.gvbPatternDetail.TabIndex = 15;
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnSearch.Location = new System.Drawing.Point(243, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(50, 24);
            this.btnSearch.TabIndex = 14;
            this.btnSearch.Text = "查询";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // radLabel1
            // 
            this.radLabel1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.radLabel1.Location = new System.Drawing.Point(11, 3);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(57, 25);
            this.radLabel1.TabIndex = 13;
            this.radLabel1.Text = "批次号";
            // 
            // txtBatchName
            // 
            this.txtBatchName.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.txtBatchName.Location = new System.Drawing.Point(74, 0);
            this.txtBatchName.Name = "txtBatchName";
            this.txtBatchName.Size = new System.Drawing.Size(163, 27);
            this.txtBatchName.TabIndex = 16;
            // 
            // PatternDetailView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.Controls.Add(this.txtBatchName);
            this.Controls.Add(this.gvbPatternDetail);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.radLabel1);
            this.Name = "PatternDetailView";
            this.Size = new System.Drawing.Size(949, 762);
            ((System.ComponentModel.ISupportInitialize)(this.btnSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBatchName)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private NJIS.FPZWS.UI.Common.Controls.GridViewBase gvbPatternDetail;
        private Telerik.WinControls.UI.RadButton btnSearch;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadTextBox txtBatchName;
    }
}
