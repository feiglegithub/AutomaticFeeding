using NJIS.FPZWS.LineControl.CuttingDevice.Views.Controls.ModuleControl;

namespace NJIS.FPZWS.LineControl.CuttingDevice.Views
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.迷你模式ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.完整模式ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.取消窗体至前ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.窗体至顶层ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.取消自动刷新ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.启用自动刷新ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.radPageView1 = new Telerik.WinControls.UI.RadPageView();
            this.radPageViewPage1 = new Telerik.WinControls.UI.RadPageViewPage();
            this.sawFileListControl1 = new NJIS.FPZWS.LineControl.CuttingDevice.Views.Controls.SawFileListControl();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radPageView1)).BeginInit();
            this.radPageView1.SuspendLayout();
            this.radPageViewPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.迷你模式ToolStripMenuItem,
            this.完整模式ToolStripMenuItem,
            this.取消窗体至前ToolStripMenuItem,
            this.窗体至顶层ToolStripMenuItem,
            this.取消自动刷新ToolStripMenuItem,
            this.启用自动刷新ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(161, 136);
            // 
            // 迷你模式ToolStripMenuItem
            // 
            this.迷你模式ToolStripMenuItem.Name = "迷你模式ToolStripMenuItem";
            this.迷你模式ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.迷你模式ToolStripMenuItem.Text = "迷你模式";
            this.迷你模式ToolStripMenuItem.Click += new System.EventHandler(this.迷你模式ToolStripMenuItem_Click);
            // 
            // 完整模式ToolStripMenuItem
            // 
            this.完整模式ToolStripMenuItem.Name = "完整模式ToolStripMenuItem";
            this.完整模式ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.完整模式ToolStripMenuItem.Text = "完整模式";
            this.完整模式ToolStripMenuItem.Click += new System.EventHandler(this.完整模式ToolStripMenuItem_Click);
            // 
            // 取消窗体至前ToolStripMenuItem
            // 
            this.取消窗体至前ToolStripMenuItem.Name = "取消窗体至前ToolStripMenuItem";
            this.取消窗体至前ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.取消窗体至前ToolStripMenuItem.Text = "取消窗体至顶层";
            this.取消窗体至前ToolStripMenuItem.Click += new System.EventHandler(this.取消窗体至前ToolStripMenuItem_Click);
            // 
            // 窗体至顶层ToolStripMenuItem
            // 
            this.窗体至顶层ToolStripMenuItem.Name = "窗体至顶层ToolStripMenuItem";
            this.窗体至顶层ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.窗体至顶层ToolStripMenuItem.Text = "窗体至顶层";
            this.窗体至顶层ToolStripMenuItem.Click += new System.EventHandler(this.窗体至顶层ToolStripMenuItem_Click);
            // 
            // 取消自动刷新ToolStripMenuItem
            // 
            this.取消自动刷新ToolStripMenuItem.Name = "取消自动刷新ToolStripMenuItem";
            this.取消自动刷新ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.取消自动刷新ToolStripMenuItem.Text = "取消自动刷新";
            this.取消自动刷新ToolStripMenuItem.Click += new System.EventHandler(this.取消自动刷新ToolStripMenuItem_Click);
            // 
            // 启用自动刷新ToolStripMenuItem
            // 
            this.启用自动刷新ToolStripMenuItem.Name = "启用自动刷新ToolStripMenuItem";
            this.启用自动刷新ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.启用自动刷新ToolStripMenuItem.Text = "启用自动刷新";
            this.启用自动刷新ToolStripMenuItem.Click += new System.EventHandler(this.启用自动刷新ToolStripMenuItem_Click);
            // 
            // radPageView1
            // 
            this.radPageView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radPageView1.Controls.Add(this.radPageViewPage1);
            this.radPageView1.Location = new System.Drawing.Point(12, 12);
            this.radPageView1.Name = "radPageView1";
            this.radPageView1.SelectedPage = this.radPageViewPage1;
            this.radPageView1.Size = new System.Drawing.Size(1046, 645);
            this.radPageView1.TabIndex = 1;
            this.radPageView1.Text = "radPageView1";
            // 
            // radPageViewPage1
            // 
            this.radPageViewPage1.Controls.Add(this.sawFileListControl1);
            this.radPageViewPage1.ItemSize = new System.Drawing.SizeF(80F, 24F);
            this.radPageViewPage1.Location = new System.Drawing.Point(10, 33);
            this.radPageViewPage1.Name = "radPageViewPage1";
            this.radPageViewPage1.Size = new System.Drawing.Size(1025, 601);
            this.radPageViewPage1.Text = "Saw文件列表";
            // 
            // sawFileListControl1
            // 
            this.sawFileListControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sawFileListControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.sawFileListControl1.Location = new System.Drawing.Point(3, 3);
            this.sawFileListControl1.Name = "sawFileListControl1";
            this.sawFileListControl1.Size = new System.Drawing.Size(1019, 595);
            this.sawFileListControl1.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1070, 669);
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Controls.Add(this.radPageView1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "MainForm";
            this.TopMost = true;
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radPageView1)).EndInit();
            this.radPageView1.ResumeLayout(false);
            this.radPageViewPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 迷你模式ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 完整模式ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 取消窗体至前ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 窗体至顶层ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 取消自动刷新ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 启用自动刷新ToolStripMenuItem;
        private Telerik.WinControls.UI.RadPageView radPageView1;
        private Telerik.WinControls.UI.RadPageViewPage radPageViewPage1;
        private Controls.SawFileListControl sawFileListControl1;
    }
}
