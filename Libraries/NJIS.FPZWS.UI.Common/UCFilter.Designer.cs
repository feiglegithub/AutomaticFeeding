namespace NJIS.FPZWS.UI.Common
{
    sealed partial class UcFilter
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
            this.MyFilter = new Telerik.WinControls.UI.RadDataFilter();
            this.Btn_Export = new Telerik.WinControls.UI.RadButton();
            this.Btn_Search = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.MyFilter)).BeginInit();
            this.MyFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Btn_Export)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Btn_Search)).BeginInit();
            this.SuspendLayout();
            // 
            // MyFilter
            // 
            this.MyFilter.AllowDefaultContextMenu = true;
            this.MyFilter.AllowShowFocusCues = true;
            this.MyFilter.Controls.Add(this.Btn_Export);
            this.MyFilter.Controls.Add(this.Btn_Search);
            this.MyFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MyFilter.Location = new System.Drawing.Point(0, 0);
            this.MyFilter.Name = "MyFilter";
            this.MyFilter.Size = new System.Drawing.Size(385, 150);
            this.MyFilter.TabIndex = 0;
            this.MyFilter.Text = "Filter";
            this.MyFilter.NodeRemoved += new Telerik.WinControls.UI.RadTreeView.RadTreeViewEventHandler(this.Filter_NodeRemoved);
            this.MyFilter.NodeAdded += new Telerik.WinControls.UI.RadTreeView.RadTreeViewEventHandler(this.Filter_NodeAdded);
            this.MyFilter.Click += new System.EventHandler(this.MyFilter_Click);
            this.MyFilter.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MyFilter_MouseClick);
            // 
            // Btn_Export
            // 
            this.Btn_Export.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Btn_Export.Location = new System.Drawing.Point(217, 123);
            this.Btn_Export.Name = "Btn_Export";
            this.Btn_Export.Size = new System.Drawing.Size(60, 24);
            this.Btn_Export.TabIndex = 1;
            this.Btn_Export.Text = "导出";
            this.Btn_Export.Visible = false;
            this.Btn_Export.Click += new System.EventHandler(this.Btn_Export_Click);
            // 
            // Btn_Search
            // 
            this.Btn_Search.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Btn_Search.Location = new System.Drawing.Point(283, 123);
            this.Btn_Search.Name = "Btn_Search";
            this.Btn_Search.Size = new System.Drawing.Size(60, 24);
            this.Btn_Search.TabIndex = 0;
            this.Btn_Search.Text = "查询";
            this.Btn_Search.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // UcFilter
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.MyFilter);
            this.MinimumSize = new System.Drawing.Size(385, 150);
            this.Name = "UcFilter";
            this.Size = new System.Drawing.Size(385, 150);
            ((System.ComponentModel.ISupportInitialize)(this.MyFilter)).EndInit();
            this.MyFilter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Btn_Export)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Btn_Search)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Telerik.WinControls.UI.RadButton Btn_Search;
        public Telerik.WinControls.UI.RadDataFilter MyFilter;
        private Telerik.WinControls.UI.RadButton Btn_Export;
    }
}
