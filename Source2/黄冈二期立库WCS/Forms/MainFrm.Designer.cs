namespace WCS
{
    partial class MainFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainFrm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tsmHome = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmTask = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmMonitor = new System.Windows.Forms.ToolStripMenuItem();
            this.一楼发货区ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.二楼理货区ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.空盘回流ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.站台管理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.日志管理ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.lED信息监控ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(35)))), ((int)(((byte)(50)))));
            this.panel1.Controls.Add(this.pictureBox3);
            this.panel1.Controls.Add(this.pictureBox2);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1920, 36);
            this.panel1.TabIndex = 0;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel_head_MouseDown);
            // 
            // pictureBox3
            // 
            this.pictureBox3.Dock = System.Windows.Forms.DockStyle.Right;
            this.pictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox3.Image")));
            this.pictureBox3.Location = new System.Drawing.Point(1800, 0);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(40, 36);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox3.TabIndex = 2;
            this.pictureBox3.TabStop = false;
            this.pictureBox3.Click += new System.EventHandler(this.pic_minsize_Click);
            this.pictureBox3.MouseEnter += new System.EventHandler(this.pic_MouseEnter);
            this.pictureBox3.MouseLeave += new System.EventHandler(this.pic_MouseLeave);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Right;
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(1840, 0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(40, 36);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pic_maxsize_Click);
            this.pictureBox2.MouseEnter += new System.EventHandler(this.pic_MouseEnter);
            this.pictureBox2.MouseLeave += new System.EventHandler(this.pic_MouseLeave);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Right;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(1880, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(40, 36);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pic_exit_Click);
            this.pictureBox1.MouseEnter += new System.EventHandler(this.pic_MouseEnter);
            this.pictureBox1.MouseLeave += new System.EventHandler(this.pic_MouseLeave);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.menuStrip1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 36);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1920, 28);
            this.panel2.TabIndex = 1;
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(105)))), ((int)(((byte)(120)))));
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmHome,
            this.tsmTask,
            this.tsmMonitor,
            this.站台管理ToolStripMenuItem,
            this.日志管理ToolStripMenuItem1,
            this.lED信息监控ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1920, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // tsmHome
            // 
            this.tsmHome.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tsmHome.ForeColor = System.Drawing.Color.Silver;
            this.tsmHome.Name = "tsmHome";
            this.tsmHome.Size = new System.Drawing.Size(54, 24);
            this.tsmHome.Text = "首页";
            this.tsmHome.Click += new System.EventHandler(this.tsmHome_Click);
            // 
            // tsmTask
            // 
            this.tsmTask.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(105)))), ((int)(((byte)(120)))));
            this.tsmTask.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold);
            this.tsmTask.ForeColor = System.Drawing.Color.Silver;
            this.tsmTask.Name = "tsmTask";
            this.tsmTask.Size = new System.Drawing.Size(86, 24);
            this.tsmTask.Text = "任务管理";
            this.tsmTask.Click += new System.EventHandler(this.tsmTask_Click);
            // 
            // tsmMonitor
            // 
            this.tsmMonitor.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.一楼发货区ToolStripMenuItem,
            this.二楼理货区ToolStripMenuItem,
            this.空盘回流ToolStripMenuItem});
            this.tsmMonitor.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold);
            this.tsmMonitor.ForeColor = System.Drawing.Color.Silver;
            this.tsmMonitor.Name = "tsmMonitor";
            this.tsmMonitor.Size = new System.Drawing.Size(86, 24);
            this.tsmMonitor.Text = "设备监控";
            // 
            // 一楼发货区ToolStripMenuItem
            // 
            this.一楼发货区ToolStripMenuItem.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.一楼发货区ToolStripMenuItem.ForeColor = System.Drawing.Color.Silver;
            this.一楼发货区ToolStripMenuItem.Name = "一楼发货区ToolStripMenuItem";
            this.一楼发货区ToolStripMenuItem.Size = new System.Drawing.Size(149, 24);
            this.一楼发货区ToolStripMenuItem.Text = "一楼发货区";
            this.一楼发货区ToolStripMenuItem.Click += new System.EventHandler(this.一楼发货区ToolStripMenuItem_Click);
            // 
            // 二楼理货区ToolStripMenuItem
            // 
            this.二楼理货区ToolStripMenuItem.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.二楼理货区ToolStripMenuItem.ForeColor = System.Drawing.Color.Silver;
            this.二楼理货区ToolStripMenuItem.Name = "二楼理货区ToolStripMenuItem";
            this.二楼理货区ToolStripMenuItem.Size = new System.Drawing.Size(149, 24);
            this.二楼理货区ToolStripMenuItem.Text = "二楼理货区";
            this.二楼理货区ToolStripMenuItem.Click += new System.EventHandler(this.二楼理货区ToolStripMenuItem_Click);
            // 
            // 空盘回流ToolStripMenuItem
            // 
            this.空盘回流ToolStripMenuItem.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold);
            this.空盘回流ToolStripMenuItem.ForeColor = System.Drawing.Color.Silver;
            this.空盘回流ToolStripMenuItem.Name = "空盘回流ToolStripMenuItem";
            this.空盘回流ToolStripMenuItem.Size = new System.Drawing.Size(149, 24);
            this.空盘回流ToolStripMenuItem.Text = "空托盘回流";
            this.空盘回流ToolStripMenuItem.Click += new System.EventHandler(this.空盘回流ToolStripMenuItem_Click);
            // 
            // 站台管理ToolStripMenuItem
            // 
            this.站台管理ToolStripMenuItem.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold);
            this.站台管理ToolStripMenuItem.ForeColor = System.Drawing.Color.Silver;
            this.站台管理ToolStripMenuItem.Name = "站台管理ToolStripMenuItem";
            this.站台管理ToolStripMenuItem.Size = new System.Drawing.Size(86, 24);
            this.站台管理ToolStripMenuItem.Text = "站台管理";
            this.站台管理ToolStripMenuItem.Click += new System.EventHandler(this.站台管理ToolStripMenuItem_Click);
            // 
            // 日志管理ToolStripMenuItem1
            // 
            this.日志管理ToolStripMenuItem1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold);
            this.日志管理ToolStripMenuItem1.ForeColor = System.Drawing.Color.Silver;
            this.日志管理ToolStripMenuItem1.Name = "日志管理ToolStripMenuItem1";
            this.日志管理ToolStripMenuItem1.Size = new System.Drawing.Size(86, 24);
            this.日志管理ToolStripMenuItem1.Text = "日志管理";
            this.日志管理ToolStripMenuItem1.Click += new System.EventHandler(this.日志管理ToolStripMenuItem1_Click);
            // 
            // lED信息监控ToolStripMenuItem
            // 
            this.lED信息监控ToolStripMenuItem.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold);
            this.lED信息监控ToolStripMenuItem.ForeColor = System.Drawing.Color.Silver;
            this.lED信息监控ToolStripMenuItem.Name = "lED信息监控ToolStripMenuItem";
            this.lED信息监控ToolStripMenuItem.Size = new System.Drawing.Size(113, 24);
            this.lED信息监控ToolStripMenuItem.Text = "lED信息监控";
            this.lED信息监控ToolStripMenuItem.Click += new System.EventHandler(this.lED信息监控ToolStripMenuItem_Click);
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(1920, 1080);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximumSize = new System.Drawing.Size(1920, 1080);
            this.Name = "MainFrm";
            this.Text = "MainFrm";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsmHome;
        private System.Windows.Forms.ToolStripMenuItem tsmTask;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStripMenuItem tsmMonitor;
        private System.Windows.Forms.ToolStripMenuItem 一楼发货区ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 二楼理货区ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 站台管理ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 空盘回流ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 日志管理ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem lED信息监控ToolStripMenuItem;
    }
}