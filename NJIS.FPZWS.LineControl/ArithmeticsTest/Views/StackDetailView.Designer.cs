namespace ArithmeticsTest.Views
{
    partial class StackDetailView
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
            this.gvbStackDetail = new NJIS.FPZWS.UI.Common.Controls.GridViewBase();
            this.btnSearch = new Telerik.WinControls.UI.RadButton();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.dtpPlanDate = new Telerik.WinControls.UI.RadDateTimePicker();
            ((System.ComponentModel.ISupportInitialize)(this.btnSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpPlanDate)).BeginInit();
            this.SuspendLayout();
            // 
            // gvbStackDetail
            // 
            this.gvbStackDetail.AllowAddNewRow = false;
            this.gvbStackDetail.AllowCheckSort = false;
            this.gvbStackDetail.AllowDeleteRow = false;
            this.gvbStackDetail.AllowEditRow = true;
            this.gvbStackDetail.AllowSelectAll = true;
            this.gvbStackDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gvbStackDetail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.gvbStackDetail.ChangedValueForeColor = System.Drawing.Color.DeepPink;
            this.gvbStackDetail.DataSource = null;
            this.gvbStackDetail.DefaultValueForeColor = System.Drawing.Color.Black;
            this.gvbStackDetail.EnableFiltering = true;
            this.gvbStackDetail.EnablePaging = false;
            this.gvbStackDetail.EnableSorting = true;
            this.gvbStackDetail.Location = new System.Drawing.Point(0, 28);
            this.gvbStackDetail.Name = "gvbStackDetail";
            this.gvbStackDetail.NewAddRowForeColor = System.Drawing.Color.Fuchsia;
            this.gvbStackDetail.PageSize = 20;
            this.gvbStackDetail.ReadOnly = false;
            this.gvbStackDetail.ShowCheckBox = false;
            this.gvbStackDetail.ShowRowHeaderColumn = false;
            this.gvbStackDetail.ShowRowNumber = true;
            this.gvbStackDetail.Size = new System.Drawing.Size(938, 724);
            this.gvbStackDetail.TabIndex = 15;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(235, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(56, 19);
            this.btnSearch.TabIndex = 14;
            this.btnSearch.Text = "查询";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // radLabel1
            // 
            this.radLabel1.Location = new System.Drawing.Point(21, 3);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(54, 18);
            this.radLabel1.TabIndex = 13;
            this.radLabel1.Text = "计划日期";
            // 
            // dtpPlanDate
            // 
            this.dtpPlanDate.Location = new System.Drawing.Point(81, 2);
            this.dtpPlanDate.Name = "dtpPlanDate";
            this.dtpPlanDate.Size = new System.Drawing.Size(128, 20);
            this.dtpPlanDate.TabIndex = 12;
            this.dtpPlanDate.TabStop = false;
            this.dtpPlanDate.Text = "2020年1月8日";
            this.dtpPlanDate.Value = new System.DateTime(2020, 1, 8, 0, 0, 0, 0);
            // 
            // StackDetailView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.Controls.Add(this.gvbStackDetail);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.radLabel1);
            this.Controls.Add(this.dtpPlanDate);
            this.Name = "StackDetailView";
            this.Size = new System.Drawing.Size(941, 755);
            ((System.ComponentModel.ISupportInitialize)(this.btnSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpPlanDate)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private NJIS.FPZWS.UI.Common.Controls.GridViewBase gvbStackDetail;
        private Telerik.WinControls.UI.RadButton btnSearch;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadDateTimePicker dtpPlanDate;
    }
}
