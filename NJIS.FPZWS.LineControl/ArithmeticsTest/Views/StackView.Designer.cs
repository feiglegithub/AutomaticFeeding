namespace ArithmeticsTest.Views
{
    partial class StackView
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
            this.gvbStack = new NJIS.FPZWS.UI.Common.Controls.GridViewBase();
            this.btnSearch = new Telerik.WinControls.UI.RadButton();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.dtpPlanDate = new Telerik.WinControls.UI.RadDateTimePicker();
            this.isDataBase = new Telerik.WinControls.UI.RadCheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.btnSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpPlanDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.isDataBase)).BeginInit();
            this.SuspendLayout();
            // 
            // gvbStack
            // 
            this.gvbStack.AllowAddNewRow = false;
            this.gvbStack.AllowCheckSort = false;
            this.gvbStack.AllowDeleteRow = false;
            this.gvbStack.AllowEditRow = true;
            this.gvbStack.AllowSelectAll = true;
            this.gvbStack.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gvbStack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.gvbStack.ChangedValueForeColor = System.Drawing.Color.DeepPink;
            this.gvbStack.DataSource = null;
            this.gvbStack.DefaultValueForeColor = System.Drawing.Color.Black;
            this.gvbStack.EnableFiltering = true;
            this.gvbStack.EnablePaging = false;
            this.gvbStack.EnableSorting = true;
            this.gvbStack.Location = new System.Drawing.Point(3, 28);
            this.gvbStack.Name = "gvbStack";
            this.gvbStack.NewAddRowForeColor = System.Drawing.Color.Fuchsia;
            this.gvbStack.PageSize = 20;
            this.gvbStack.ReadOnly = false;
            this.gvbStack.ShowCheckBox = false;
            this.gvbStack.ShowRowHeaderColumn = false;
            this.gvbStack.ShowRowNumber = true;
            this.gvbStack.Size = new System.Drawing.Size(1087, 818);
            this.gvbStack.TabIndex = 15;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(230, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(56, 19);
            this.btnSearch.TabIndex = 14;
            this.btnSearch.Text = "查询";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // radLabel1
            // 
            this.radLabel1.Location = new System.Drawing.Point(16, 3);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(54, 18);
            this.radLabel1.TabIndex = 13;
            this.radLabel1.Text = "计划日期";
            // 
            // dtpPlanDate
            // 
            this.dtpPlanDate.Location = new System.Drawing.Point(76, 2);
            this.dtpPlanDate.Name = "dtpPlanDate";
            this.dtpPlanDate.Size = new System.Drawing.Size(128, 20);
            this.dtpPlanDate.TabIndex = 12;
            this.dtpPlanDate.TabStop = false;
            this.dtpPlanDate.Text = "2020年1月8日";
            this.dtpPlanDate.Value = new System.DateTime(2020, 1, 8, 0, 0, 0, 0);
            // 
            // isDataBase
            // 
            this.isDataBase.CheckState = System.Windows.Forms.CheckState.Checked;
            this.isDataBase.Location = new System.Drawing.Point(311, 4);
            this.isDataBase.Name = "isDataBase";
            this.isDataBase.Size = new System.Drawing.Size(68, 18);
            this.isDataBase.TabIndex = 16;
            this.isDataBase.Text = "查数据库";
            this.isDataBase.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On;
            // 
            // StackView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.Controls.Add(this.isDataBase);
            this.Controls.Add(this.gvbStack);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.radLabel1);
            this.Controls.Add(this.dtpPlanDate);
            this.Name = "StackView";
            this.Size = new System.Drawing.Size(1093, 849);
            ((System.ComponentModel.ISupportInitialize)(this.btnSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpPlanDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.isDataBase)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private NJIS.FPZWS.UI.Common.Controls.GridViewBase gvbStack;
        private Telerik.WinControls.UI.RadButton btnSearch;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadDateTimePicker dtpPlanDate;
        private Telerik.WinControls.UI.RadCheckBox isDataBase;
    }
}
