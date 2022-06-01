namespace NJIS.FPZWS.LineControl.PartMill.Simulator
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
            this.radPageView1 = new Telerik.WinControls.UI.RadPageView();
            this.radPageViewPage1 = new Telerik.WinControls.UI.RadPageViewPage();
            this.radPageViewPage2 = new Telerik.WinControls.UI.RadPageViewPage();
            this.machineHandControl1 = new NJIS.FPZWS.LineControl.PartMill.Simulator.Views.MachineHandView();
            this.radPageViewPage3 = new Telerik.WinControls.UI.RadPageViewPage();
            this.lineTaskView1 = new NJIS.FPZWS.LineControl.PartMill.Simulator.Views.LineTaskView();
            this.radPageViewPage4 = new Telerik.WinControls.UI.RadPageViewPage();
            this.machineHandTaskView1 = new NJIS.FPZWS.LineControl.PartMill.Simulator.Views.MachineHandTaskView();
            ((System.ComponentModel.ISupportInitialize)(this.radPageView1)).BeginInit();
            this.radPageView1.SuspendLayout();
            this.radPageViewPage2.SuspendLayout();
            this.radPageViewPage3.SuspendLayout();
            this.radPageViewPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radPageView1
            // 
            this.radPageView1.Controls.Add(this.radPageViewPage1);
            this.radPageView1.Controls.Add(this.radPageViewPage2);
            this.radPageView1.Controls.Add(this.radPageViewPage3);
            this.radPageView1.Controls.Add(this.radPageViewPage4);
            this.radPageView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPageView1.Location = new System.Drawing.Point(0, 0);
            this.radPageView1.Name = "radPageView1";
            this.radPageView1.SelectedPage = this.radPageViewPage1;
            this.radPageView1.Size = new System.Drawing.Size(1155, 787);
            this.radPageView1.TabIndex = 1;
            this.radPageView1.Text = "radPageView1";
            ((Telerik.WinControls.UI.RadPageViewStripItem)(this.radPageView1.GetChildAt(0).GetChildAt(0).GetChildAt(0).GetChildAt(0))).Title = "滚筒线控制";
            ((Telerik.WinControls.UI.RadPageViewStripItem)(this.radPageView1.GetChildAt(0).GetChildAt(0).GetChildAt(0).GetChildAt(0))).Description = "滚筒线控制";
            ((Telerik.WinControls.UI.RadPageViewStripItem)(this.radPageView1.GetChildAt(0).GetChildAt(0).GetChildAt(0).GetChildAt(0))).Text = "滚筒线控制";
            ((Telerik.WinControls.UI.RadPageViewStripItem)(this.radPageView1.GetChildAt(0).GetChildAt(0).GetChildAt(0).GetChildAt(1))).Title = "机械手控制";
            ((Telerik.WinControls.UI.RadPageViewStripItem)(this.radPageView1.GetChildAt(0).GetChildAt(0).GetChildAt(0).GetChildAt(1))).Description = "机械手控制";
            ((Telerik.WinControls.UI.RadPageViewStripItem)(this.radPageView1.GetChildAt(0).GetChildAt(0).GetChildAt(0).GetChildAt(1))).Text = "机械手控制";
            ((Telerik.WinControls.UI.RadPageViewStripItem)(this.radPageView1.GetChildAt(0).GetChildAt(0).GetChildAt(0).GetChildAt(2))).Title = "线体任务";
            ((Telerik.WinControls.UI.RadPageViewStripItem)(this.radPageView1.GetChildAt(0).GetChildAt(0).GetChildAt(0).GetChildAt(2))).Description = "线体任务";
            ((Telerik.WinControls.UI.RadPageViewStripItem)(this.radPageView1.GetChildAt(0).GetChildAt(0).GetChildAt(0).GetChildAt(2))).Text = "线体任务";
            ((Telerik.WinControls.UI.RadPageViewStripItem)(this.radPageView1.GetChildAt(0).GetChildAt(0).GetChildAt(0).GetChildAt(3))).Title = "机械手任务";
            ((Telerik.WinControls.UI.RadPageViewStripItem)(this.radPageView1.GetChildAt(0).GetChildAt(0).GetChildAt(0).GetChildAt(3))).Description = "机械手任务";
            ((Telerik.WinControls.UI.RadPageViewStripItem)(this.radPageView1.GetChildAt(0).GetChildAt(0).GetChildAt(0).GetChildAt(3))).Text = "机械手任务";
            // 
            // radPageViewPage1
            // 
            this.radPageViewPage1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.radPageViewPage1.Description = "滚筒线控制";
            this.radPageViewPage1.ItemSize = new System.Drawing.SizeF(75F, 28F);
            this.radPageViewPage1.Location = new System.Drawing.Point(10, 37);
            this.radPageViewPage1.Name = "radPageViewPage1";
            this.radPageViewPage1.Size = new System.Drawing.Size(1134, 739);
            this.radPageViewPage1.Text = "滚筒线控制";
            this.radPageViewPage1.Title = "滚筒线控制";
            // 
            // radPageViewPage2
            // 
            this.radPageViewPage2.Controls.Add(this.machineHandControl1);
            this.radPageViewPage2.Description = "机械手控制";
            this.radPageViewPage2.ItemSize = new System.Drawing.SizeF(75F, 28F);
            this.radPageViewPage2.Location = new System.Drawing.Point(10, 37);
            this.radPageViewPage2.Name = "radPageViewPage2";
            this.radPageViewPage2.Size = new System.Drawing.Size(1134, 739);
            this.radPageViewPage2.Text = "机械手控制";
            this.radPageViewPage2.Title = "机械手控制";
            // 
            // machineHandControl1
            // 
            this.machineHandControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.machineHandControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.machineHandControl1.Location = new System.Drawing.Point(0, 0);
            this.machineHandControl1.Name = "machineHandControl1";
            this.machineHandControl1.Size = new System.Drawing.Size(1134, 739);
            this.machineHandControl1.TabIndex = 0;
            // 
            // radPageViewPage3
            // 
            this.radPageViewPage3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.radPageViewPage3.Controls.Add(this.lineTaskView1);
            this.radPageViewPage3.Description = "线体任务";
            this.radPageViewPage3.ItemSize = new System.Drawing.SizeF(64F, 28F);
            this.radPageViewPage3.Location = new System.Drawing.Point(10, 37);
            this.radPageViewPage3.Name = "radPageViewPage3";
            this.radPageViewPage3.Size = new System.Drawing.Size(1134, 739);
            this.radPageViewPage3.Text = "线体任务";
            this.radPageViewPage3.Title = "线体任务";
            // 
            // lineTaskView1
            // 
            this.lineTaskView1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.lineTaskView1.Location = new System.Drawing.Point(31, 25);
            this.lineTaskView1.Name = "lineTaskView1";
            this.lineTaskView1.Size = new System.Drawing.Size(953, 711);
            this.lineTaskView1.TabIndex = 0;
            // 
            // radPageViewPage4
            // 
            this.radPageViewPage4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.radPageViewPage4.Controls.Add(this.machineHandTaskView1);
            this.radPageViewPage4.Description = "机械手任务";
            this.radPageViewPage4.ItemSize = new System.Drawing.SizeF(75F, 28F);
            this.radPageViewPage4.Location = new System.Drawing.Point(10, 37);
            this.radPageViewPage4.Name = "radPageViewPage4";
            this.radPageViewPage4.Size = new System.Drawing.Size(1134, 739);
            this.radPageViewPage4.Text = "机械手任务";
            this.radPageViewPage4.Title = "机械手任务";
            // 
            // machineHandTaskView1
            // 
            this.machineHandTaskView1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.machineHandTaskView1.Location = new System.Drawing.Point(51, 29);
            this.machineHandTaskView1.Name = "machineHandTaskView1";
            this.machineHandTaskView1.Size = new System.Drawing.Size(1027, 679);
            this.machineHandTaskView1.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1155, 787);
            this.Controls.Add(this.radPageView1);
            this.Name = "MainForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "套铣模拟器";
            ((System.ComponentModel.ISupportInitialize)(this.radPageView1)).EndInit();
            this.radPageView1.ResumeLayout(false);
            this.radPageViewPage2.ResumeLayout(false);
            this.radPageViewPage3.ResumeLayout(false);
            this.radPageViewPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Views.MachineHandView machineHandControl1;
        private Telerik.WinControls.UI.RadPageView radPageView1;
        private Telerik.WinControls.UI.RadPageViewPage radPageViewPage1;
        private Telerik.WinControls.UI.RadPageViewPage radPageViewPage2;
        private Telerik.WinControls.UI.RadPageViewPage radPageViewPage3;
        private Telerik.WinControls.UI.RadPageViewPage radPageViewPage4;
        private Views.LineTaskView lineTaskView1;
        private Views.MachineHandTaskView machineHandTaskView1;
    }
}
