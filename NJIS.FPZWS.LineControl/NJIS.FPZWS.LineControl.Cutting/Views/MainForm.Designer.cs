namespace NJIS.FPZWS.LineControl.Cutting.UI.Views
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
            this.radPageView1 = new Telerik.WinControls.UI.RadPageView();
            this.radPageViewPage3 = new Telerik.WinControls.UI.RadPageViewPage();
            this.curTaskDetailControl1 = new NJIS.FPZWS.LineControl.Cutting.UI.Views.Controls.ModuleControl.CurTaskDetailControl();
            this.radPageViewPage1 = new Telerik.WinControls.UI.RadPageViewPage();
            this.cuttingSimulationControl1 = new NJIS.FPZWS.LineControl.Cutting.UI.Views.Controls.ModuleControl.CuttingSimulationControl();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.迷你模式ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.完整模式ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.取消窗体至前ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.窗体至顶层ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.取消自动刷新ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.启用自动刷新ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.radPageView1)).BeginInit();
            this.radPageView1.SuspendLayout();
            this.radPageViewPage3.SuspendLayout();
            this.radPageViewPage1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radPageView1
            // 
            this.radPageView1.Controls.Add(this.radPageViewPage3);
            this.radPageView1.Controls.Add(this.radPageViewPage1);
            this.radPageView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPageView1.Location = new System.Drawing.Point(0, 0);
            this.radPageView1.Name = "radPageView1";
            this.radPageView1.SelectedPage = this.radPageViewPage3;
            this.radPageView1.Size = new System.Drawing.Size(1161, 635);
            this.radPageView1.TabIndex = 25;
            this.radPageView1.Text = "radPageView1";
            this.radPageView1.ViewMode = Telerik.WinControls.UI.PageViewMode.Backstage;
            ((Telerik.WinControls.UI.StripViewItemContainer)(this.radPageView1.GetChildAt(0).GetChildAt(0))).TextOrientation = System.Windows.Forms.Orientation.Horizontal;
            ((Telerik.WinControls.UI.StripViewItemContainer)(this.radPageView1.GetChildAt(0).GetChildAt(0))).Shape = null;
            ((Telerik.WinControls.UI.StripViewItemContainer)(this.radPageView1.GetChildAt(0).GetChildAt(0))).MinSize = new System.Drawing.Size(100, 0);
            ((Telerik.WinControls.UI.StripViewItemLayout)(this.radPageView1.GetChildAt(0).GetChildAt(0).GetChildAt(0))).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            ((Telerik.WinControls.UI.RadPageViewStripItem)(this.radPageView1.GetChildAt(0).GetChildAt(0).GetChildAt(0).GetChildAt(0))).Title = "当前任务详情";
            ((Telerik.WinControls.UI.RadPageViewStripItem)(this.radPageView1.GetChildAt(0).GetChildAt(0).GetChildAt(0).GetChildAt(0))).Description = "当前任务详情";
            ((Telerik.WinControls.UI.RadPageViewStripItem)(this.radPageView1.GetChildAt(0).GetChildAt(0).GetChildAt(0).GetChildAt(0))).Text = "当前任务详情";
            ((Telerik.WinControls.UI.RadPageViewStripItem)(this.radPageView1.GetChildAt(0).GetChildAt(0).GetChildAt(0).GetChildAt(0))).Font = new System.Drawing.Font("微软雅黑", 12F);
            ((Telerik.WinControls.UI.RadPageViewStripItem)(this.radPageView1.GetChildAt(0).GetChildAt(0).GetChildAt(0).GetChildAt(1))).Title = "模拟器";
            ((Telerik.WinControls.UI.RadPageViewStripItem)(this.radPageView1.GetChildAt(0).GetChildAt(0).GetChildAt(0).GetChildAt(1))).Description = "模拟器";
            ((Telerik.WinControls.UI.RadPageViewStripItem)(this.radPageView1.GetChildAt(0).GetChildAt(0).GetChildAt(0).GetChildAt(1))).Text = "模拟器";
            ((Telerik.WinControls.UI.RadPageViewStripItem)(this.radPageView1.GetChildAt(0).GetChildAt(0).GetChildAt(0).GetChildAt(1))).Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            ((Telerik.WinControls.UI.RadPageViewStripItem)(this.radPageView1.GetChildAt(0).GetChildAt(0).GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Hidden;
            ((Telerik.WinControls.UI.StripViewButtonsPanel)(this.radPageView1.GetChildAt(0).GetChildAt(0).GetChildAt(1))).ButtonsSize = new System.Drawing.Size(16, 16);
            ((Telerik.WinControls.UI.StripViewButtonsPanel)(this.radPageView1.GetChildAt(0).GetChildAt(0).GetChildAt(1))).GradientStyle = Telerik.WinControls.GradientStyles.Linear;
            ((Telerik.WinControls.UI.StripViewButtonsPanel)(this.radPageView1.GetChildAt(0).GetChildAt(0).GetChildAt(1))).ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            ((Telerik.WinControls.UI.StripViewButtonsPanel)(this.radPageView1.GetChildAt(0).GetChildAt(0).GetChildAt(1))).SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
            ((Telerik.WinControls.UI.RadPageViewContentAreaElement)(this.radPageView1.GetChildAt(0).GetChildAt(1))).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            ((Telerik.WinControls.UI.RadPageViewLabelElement)(this.radPageView1.GetChildAt(0).GetChildAt(3))).BorderDashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
            ((Telerik.WinControls.UI.RadPageViewLabelElement)(this.radPageView1.GetChildAt(0).GetChildAt(3))).Text = "当前任务详情";
            ((Telerik.WinControls.UI.RadPageViewLabelElement)(this.radPageView1.GetChildAt(0).GetChildAt(3))).EnableElementShadow = false;
            // 
            // radPageViewPage3
            // 
            this.radPageViewPage3.Controls.Add(this.curTaskDetailControl1);
            this.radPageViewPage3.Description = "当前任务详情";
            this.radPageViewPage3.ItemSize = new System.Drawing.SizeF(115F, 45F);
            this.radPageViewPage3.Location = new System.Drawing.Point(145, 4);
            this.radPageViewPage3.Name = "radPageViewPage3";
            this.radPageViewPage3.Size = new System.Drawing.Size(1012, 627);
            this.radPageViewPage3.Text = "当前任务详情";
            this.radPageViewPage3.Title = "当前任务详情";
            // 
            // curTaskDetailControl1
            // 
            this.curTaskDetailControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.curTaskDetailControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.curTaskDetailControl1.IsAutoRefresh = true;
            this.curTaskDetailControl1.Location = new System.Drawing.Point(0, 0);
            this.curTaskDetailControl1.Name = "curTaskDetailControl1";
            this.curTaskDetailControl1.Size = new System.Drawing.Size(1012, 627);
            this.curTaskDetailControl1.TabIndex = 0;
            // 
            // radPageViewPage1
            // 
            this.radPageViewPage1.Controls.Add(this.cuttingSimulationControl1);
            this.radPageViewPage1.Description = "模拟器";
            this.radPageViewPage1.ItemSize = new System.Drawing.SizeF(115F, 45F);
            this.radPageViewPage1.Location = new System.Drawing.Point(145, 4);
            this.radPageViewPage1.Name = "radPageViewPage1";
            this.radPageViewPage1.Size = new System.Drawing.Size(1012, 627);
            this.radPageViewPage1.Text = "模拟器";
            this.radPageViewPage1.Title = "模拟器";
            // 
            // cuttingSimulationControl1
            // 
            this.cuttingSimulationControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.cuttingSimulationControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cuttingSimulationControl1.Location = new System.Drawing.Point(0, 0);
            this.cuttingSimulationControl1.Name = "cuttingSimulationControl1";
            this.cuttingSimulationControl1.Size = new System.Drawing.Size(1012, 627);
            this.cuttingSimulationControl1.TabIndex = 0;
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
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1161, 635);
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
            ((System.ComponentModel.ISupportInitialize)(this.radPageView1)).EndInit();
            this.radPageView1.ResumeLayout(false);
            this.radPageViewPage3.ResumeLayout(false);
            this.radPageViewPage1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadPageView radPageView1;
        private Telerik.WinControls.UI.RadPageViewPage radPageViewPage3;
        private Controls.ModuleControl.CurTaskDetailControl curTaskDetailControl1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 迷你模式ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 完整模式ToolStripMenuItem;
        private Telerik.WinControls.UI.RadPageViewPage radPageViewPage1;
        private Controls.ModuleControl.CuttingSimulationControl cuttingSimulationControl1;
        private System.Windows.Forms.ToolStripMenuItem 取消窗体至前ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 窗体至顶层ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 取消自动刷新ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 启用自动刷新ToolStripMenuItem;
    }
}
