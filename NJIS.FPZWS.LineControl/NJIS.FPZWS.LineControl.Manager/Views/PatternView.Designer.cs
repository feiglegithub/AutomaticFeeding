namespace NJIS.FPZWS.LineControl.Manager.Views
{
    partial class PatternView
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
            this.btnSearch = new Telerik.WinControls.UI.RadButton();
            this.gvbPattern = new NJIS.FPZWS.UI.Common.Controls.GridViewBase();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpPlanDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSearch)).BeginInit();
            this.SuspendLayout();
            // 
            // radLabel1
            // 
            this.radLabel1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radLabel1.ForeColor = System.Drawing.Color.Black;
            this.radLabel1.Location = new System.Drawing.Point(13, 3);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(74, 25);
            this.radLabel1.TabIndex = 9;
            this.radLabel1.Text = "计划日期";
            // 
            // dtpPlanDate
            // 
            this.dtpPlanDate.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpPlanDate.ForeColor = System.Drawing.Color.Black;
            this.dtpPlanDate.Location = new System.Drawing.Point(93, 3);
            this.dtpPlanDate.Name = "dtpPlanDate";
            this.dtpPlanDate.Size = new System.Drawing.Size(137, 27);
            this.dtpPlanDate.TabIndex = 8;
            this.dtpPlanDate.TabStop = false;
            this.dtpPlanDate.Text = "2020年1月8日";
            this.dtpPlanDate.Value = new System.DateTime(2020, 1, 8, 0, 0, 0, 0);
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearch.ForeColor = System.Drawing.Color.Black;
            this.btnSearch.Location = new System.Drawing.Point(236, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(56, 25);
            this.btnSearch.TabIndex = 10;
            this.btnSearch.Text = "查询";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // gvbPattern
            // 
            this.gvbPattern.AllowAddNewRow = false;
            this.gvbPattern.AllowCheckSort = false;
            this.gvbPattern.AllowDeleteRow = false;
            this.gvbPattern.AllowEditRow = true;
            this.gvbPattern.AllowSelectAll = true;
            this.gvbPattern.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gvbPattern.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.gvbPattern.ChangedValueForeColor = System.Drawing.Color.DeepPink;
            this.gvbPattern.DataSource = null;
            this.gvbPattern.DefaultValueForeColor = System.Drawing.Color.Black;
            this.gvbPattern.EnableFiltering = true;
            this.gvbPattern.EnablePaging = false;
            this.gvbPattern.EnableSorting = true;
            this.gvbPattern.Location = new System.Drawing.Point(0, 34);
            this.gvbPattern.Name = "gvbPattern";
            this.gvbPattern.NewAddRowForeColor = System.Drawing.Color.Fuchsia;
            this.gvbPattern.PageSize = 20;
            this.gvbPattern.ReadOnly = false;
            this.gvbPattern.ShowCheckBox = false;
            this.gvbPattern.ShowRowHeaderColumn = false;
            this.gvbPattern.ShowRowNumber = true;
            this.gvbPattern.Size = new System.Drawing.Size(952, 732);
            this.gvbPattern.TabIndex = 11;
            // 
            // PatternView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.Controls.Add(this.gvbPattern);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.radLabel1);
            this.Controls.Add(this.dtpPlanDate);
            this.Name = "PatternView";
            this.Size = new System.Drawing.Size(955, 769);
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpPlanDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSearch)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadDateTimePicker dtpPlanDate;
        private Telerik.WinControls.UI.RadButton btnSearch;
        private NJIS.FPZWS.UI.Common.Controls.GridViewBase gvbPattern;
    }
}
