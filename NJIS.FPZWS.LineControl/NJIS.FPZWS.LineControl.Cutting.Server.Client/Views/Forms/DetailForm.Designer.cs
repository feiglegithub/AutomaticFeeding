namespace NJIS.FPZWS.LineControl.Cutting.Server.Client.Views.Forms
{
    partial class DetailForm
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
            this.cuttingAPSSubControl1 = new NJIS.FPZWS.LineControl.Cutting.Server.Client.Views.Controls.SubControls.CuttingAPSSubControl();
            this.radPanel1 = new Telerik.WinControls.UI.RadPanel();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).BeginInit();
            this.radPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // cuttingAPSSubControl1
            // 
            this.cuttingAPSSubControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.cuttingAPSSubControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cuttingAPSSubControl1.Location = new System.Drawing.Point(0, 0);
            this.cuttingAPSSubControl1.Name = "cuttingAPSSubControl1";
            this.cuttingAPSSubControl1.Size = new System.Drawing.Size(1175, 630);
            this.cuttingAPSSubControl1.TabIndex = 0;
            // 
            // radPanel1
            // 
            this.radPanel1.Controls.Add(this.cuttingAPSSubControl1);
            this.radPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanel1.Location = new System.Drawing.Point(0, 0);
            this.radPanel1.Name = "radPanel1";
            this.radPanel1.Size = new System.Drawing.Size(1175, 630);
            this.radPanel1.TabIndex = 1;
            // 
            // DetailForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1175, 630);
            this.Controls.Add(this.radPanel1);
            this.Name = "DetailForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "计算详情";
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).EndInit();
            this.radPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.SubControls.CuttingAPSSubControl cuttingAPSSubControl1;
        private Telerik.WinControls.UI.RadPanel radPanel1;
    }
}
