namespace NJIS.FPZWS.LineControl.Manager.Views
{
    partial class NotCuttingDataControl
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
            this.radTextBoxBatchName = new Telerik.WinControls.UI.RadTextBox();
            this.radButtonSearch = new Telerik.WinControls.UI.RadButton();
            this.gridViewBase1 = new NJIS.FPZWS.UI.Common.Controls.GridViewBase();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxBatchName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            this.SuspendLayout();
            // 
            // radTextBoxBatchName
            // 
            this.radTextBoxBatchName.Location = new System.Drawing.Point(50, 3);
            this.radTextBoxBatchName.Name = "radTextBoxBatchName";
            this.radTextBoxBatchName.Size = new System.Drawing.Size(150, 20);
            this.radTextBoxBatchName.TabIndex = 1;
            // 
            // radButtonSearch
            // 
            this.radButtonSearch.Location = new System.Drawing.Point(206, 3);
            this.radButtonSearch.Name = "radButtonSearch";
            this.radButtonSearch.Size = new System.Drawing.Size(110, 24);
            this.radButtonSearch.TabIndex = 2;
            this.radButtonSearch.Text = "查询";
            this.radButtonSearch.Click += new System.EventHandler(this.radButtonSearch_Click);
            // 
            // gridViewBase1
            // 
            this.gridViewBase1.AllowAddNewRow = false;
            this.gridViewBase1.AllowCheckSort = false;
            this.gridViewBase1.AllowDeleteRow = false;
            this.gridViewBase1.AllowEditRow = true;
            this.gridViewBase1.AllowSelectAll = true;
            this.gridViewBase1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridViewBase1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.gridViewBase1.ChangedValueForeColor = System.Drawing.Color.DeepPink;
            this.gridViewBase1.DataSource = null;
            this.gridViewBase1.DefaultValueForeColor = System.Drawing.Color.Black;
            this.gridViewBase1.EnableFiltering = true;
            this.gridViewBase1.EnablePaging = false;
            this.gridViewBase1.EnableSorting = true;
            this.gridViewBase1.Location = new System.Drawing.Point(5, 33);
            this.gridViewBase1.Name = "gridViewBase1";
            this.gridViewBase1.NewAddRowForeColor = System.Drawing.Color.Fuchsia;
            this.gridViewBase1.PageSize = 20;
            this.gridViewBase1.ReadOnly = false;
            this.gridViewBase1.ShowCheckBox = false;
            this.gridViewBase1.ShowRowHeaderColumn = false;
            this.gridViewBase1.ShowRowNumber = true;
            this.gridViewBase1.Size = new System.Drawing.Size(922, 583);
            this.gridViewBase1.TabIndex = 3;
            // 
            // radLabel1
            // 
            this.radLabel1.Location = new System.Drawing.Point(3, 6);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(42, 18);
            this.radLabel1.TabIndex = 4;
            this.radLabel1.Text = "批次号";
            // 
            // NotCuttingDataControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.Controls.Add(this.radLabel1);
            this.Controls.Add(this.gridViewBase1);
            this.Controls.Add(this.radButtonSearch);
            this.Controls.Add(this.radTextBoxBatchName);
            this.Name = "NotCuttingDataControl";
            this.Size = new System.Drawing.Size(930, 619);
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxBatchName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Telerik.WinControls.UI.RadTextBox radTextBoxBatchName;
        private Telerik.WinControls.UI.RadButton radButtonSearch;
        private UI.Common.Controls.GridViewBase gridViewBase1;
        private Telerik.WinControls.UI.RadLabel radLabel1;
    }
}
