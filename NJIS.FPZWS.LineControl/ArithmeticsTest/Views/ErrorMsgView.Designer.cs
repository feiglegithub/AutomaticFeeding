namespace ArithmeticsTest.Views
{
    partial class ErrorMsgView
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
            this.gvbMsg = new NJIS.FPZWS.UI.Common.Controls.GridViewBase();
            this.SuspendLayout();
            // 
            // gvbMsg
            // 
            this.gvbMsg.AllowAddNewRow = false;
            this.gvbMsg.AllowCheckSort = false;
            this.gvbMsg.AllowDeleteRow = false;
            this.gvbMsg.AllowEditRow = true;
            this.gvbMsg.AllowSelectAll = true;
            this.gvbMsg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.gvbMsg.ChangedValueForeColor = System.Drawing.Color.DeepPink;
            this.gvbMsg.DataSource = null;
            this.gvbMsg.DefaultValueForeColor = System.Drawing.Color.Black;
            this.gvbMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvbMsg.EnableFiltering = true;
            this.gvbMsg.EnablePaging = false;
            this.gvbMsg.EnableSorting = true;
            this.gvbMsg.Location = new System.Drawing.Point(0, 0);
            this.gvbMsg.Name = "gvbMsg";
            this.gvbMsg.NewAddRowForeColor = System.Drawing.Color.Fuchsia;
            this.gvbMsg.PageSize = 20;
            this.gvbMsg.ReadOnly = false;
            this.gvbMsg.ShowCheckBox = false;
            this.gvbMsg.ShowRowHeaderColumn = false;
            this.gvbMsg.ShowRowNumber = true;
            this.gvbMsg.Size = new System.Drawing.Size(1024, 812);
            this.gvbMsg.TabIndex = 0;
            // 
            // ErrorMsgView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gvbMsg);
            this.Name = "ErrorMsgView";
            this.Size = new System.Drawing.Size(1024, 812);
            this.ResumeLayout(false);

        }

        #endregion

        private NJIS.FPZWS.UI.Common.Controls.GridViewBase gvbMsg;
    }
}
