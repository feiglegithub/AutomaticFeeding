namespace ArithmeticsTest.Views
{
    partial class ExecuteMsgView
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
            this.gvbExecute = new NJIS.FPZWS.UI.Common.Controls.GridViewBase();
            this.SuspendLayout();
            // 
            // gvbExecute
            // 
            this.gvbExecute.AllowAddNewRow = false;
            this.gvbExecute.AllowCheckSort = false;
            this.gvbExecute.AllowDeleteRow = false;
            this.gvbExecute.AllowEditRow = true;
            this.gvbExecute.AllowSelectAll = true;
            this.gvbExecute.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.gvbExecute.ChangedValueForeColor = System.Drawing.Color.DeepPink;
            this.gvbExecute.DataSource = null;
            this.gvbExecute.DefaultValueForeColor = System.Drawing.Color.Black;
            this.gvbExecute.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvbExecute.EnableFiltering = true;
            this.gvbExecute.EnablePaging = false;
            this.gvbExecute.EnableSorting = true;
            this.gvbExecute.Location = new System.Drawing.Point(0, 0);
            this.gvbExecute.Name = "gvbExecute";
            this.gvbExecute.NewAddRowForeColor = System.Drawing.Color.Fuchsia;
            this.gvbExecute.PageSize = 20;
            this.gvbExecute.ReadOnly = false;
            this.gvbExecute.ShowCheckBox = false;
            this.gvbExecute.ShowRowHeaderColumn = false;
            this.gvbExecute.ShowRowNumber = true;
            this.gvbExecute.Size = new System.Drawing.Size(987, 745);
            this.gvbExecute.TabIndex = 0;
            // 
            // ExecuteMsgView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gvbExecute);
            this.Name = "ExecuteMsgView";
            this.Size = new System.Drawing.Size(987, 745);
            this.ResumeLayout(false);

        }

        #endregion

        private NJIS.FPZWS.UI.Common.Controls.GridViewBase gvbExecute;
    }
}
