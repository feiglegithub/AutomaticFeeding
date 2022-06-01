namespace ArithmeticsTest.Views
{
    partial class CommandMsgView
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
            this.gvbCommand = new NJIS.FPZWS.UI.Common.Controls.GridViewBase();
            this.SuspendLayout();
            // 
            // gvbCommand
            // 
            this.gvbCommand.AllowAddNewRow = false;
            this.gvbCommand.AllowCheckSort = false;
            this.gvbCommand.AllowDeleteRow = false;
            this.gvbCommand.AllowEditRow = true;
            this.gvbCommand.AllowSelectAll = true;
            this.gvbCommand.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.gvbCommand.ChangedValueForeColor = System.Drawing.Color.DeepPink;
            this.gvbCommand.DataSource = null;
            this.gvbCommand.DefaultValueForeColor = System.Drawing.Color.Black;
            this.gvbCommand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvbCommand.EnableFiltering = true;
            this.gvbCommand.EnablePaging = false;
            this.gvbCommand.EnableSorting = true;
            this.gvbCommand.Location = new System.Drawing.Point(0, 0);
            this.gvbCommand.Name = "gvbCommand";
            this.gvbCommand.NewAddRowForeColor = System.Drawing.Color.Fuchsia;
            this.gvbCommand.PageSize = 20;
            this.gvbCommand.ReadOnly = false;
            this.gvbCommand.ShowCheckBox = false;
            this.gvbCommand.ShowRowHeaderColumn = false;
            this.gvbCommand.ShowRowNumber = true;
            this.gvbCommand.Size = new System.Drawing.Size(759, 647);
            this.gvbCommand.TabIndex = 0;
            // 
            // CommandMsgView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gvbCommand);
            this.Name = "CommandMsgView";
            this.Size = new System.Drawing.Size(759, 647);
            this.ResumeLayout(false);

        }

        #endregion

        private NJIS.FPZWS.UI.Common.Controls.GridViewBase gvbCommand;
    }
}
