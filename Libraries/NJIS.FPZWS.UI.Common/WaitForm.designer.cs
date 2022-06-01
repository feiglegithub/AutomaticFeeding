namespace NJIS.FPZWS.UI.Common
{
    partial class WaitForm
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
            this.donutShape1 = new Telerik.WinControls.Tests.DonutShape();
            this.customShape1 = new Telerik.WinControls.OldShapeEditor.CustomShape();
            this.ellipseShape1 = new Telerik.WinControls.EllipseShape();
            this.chamferedRectShape1 = new Telerik.WinControls.ChamferedRectShape();
            this.radWaitingBar1 = new Telerik.WinControls.UI.RadWaitingBar();
            this.dotsRingWaitingBarIndicatorElement1 = new Telerik.WinControls.UI.DotsRingWaitingBarIndicatorElement();
            ((System.ComponentModel.ISupportInitialize)(this.radWaitingBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // customShape1
            // 
            this.customShape1.Dimension = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // radWaitingBar1
            // 
            this.radWaitingBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radWaitingBar1.Location = new System.Drawing.Point(0, 0);
            this.radWaitingBar1.Name = "radWaitingBar1";
            this.radWaitingBar1.Size = new System.Drawing.Size(1011, 814);
            this.radWaitingBar1.TabIndex = 2;
            this.radWaitingBar1.Text = "radWaitingBar1";
            this.radWaitingBar1.WaitingIndicators.Add(this.dotsRingWaitingBarIndicatorElement1);
            this.radWaitingBar1.WaitingSpeed = 50;
            this.radWaitingBar1.WaitingStyle = Telerik.WinControls.Enumerations.WaitingBarStyles.DotsRing;
            // 
            // dotsRingWaitingBarIndicatorElement1
            // 
            this.dotsRingWaitingBarIndicatorElement1.BorderBoxStyle = Telerik.WinControls.BorderBoxStyle.SingleBorder;
            this.dotsRingWaitingBarIndicatorElement1.BorderDashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            this.dotsRingWaitingBarIndicatorElement1.DotRadius = 20;
            this.dotsRingWaitingBarIndicatorElement1.DrawBackgroundImage = true;
            this.dotsRingWaitingBarIndicatorElement1.DrawBorder = false;
            this.dotsRingWaitingBarIndicatorElement1.DrawImage = true;
            this.dotsRingWaitingBarIndicatorElement1.DrawText = true;
            this.dotsRingWaitingBarIndicatorElement1.ElementColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(58)))), ((int)(((byte)(131)))));
            this.dotsRingWaitingBarIndicatorElement1.ElementColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(113)))), ((int)(((byte)(141)))), ((int)(((byte)(181)))));
            this.dotsRingWaitingBarIndicatorElement1.ElementColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(148)))), ((int)(((byte)(185)))));
            this.dotsRingWaitingBarIndicatorElement1.ElementColor4 = System.Drawing.Color.Transparent;
            this.dotsRingWaitingBarIndicatorElement1.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dotsRingWaitingBarIndicatorElement1.GradientPercentage = 0.5F;
            this.dotsRingWaitingBarIndicatorElement1.GradientStyle = Telerik.WinControls.GradientStyles.Linear;
            this.dotsRingWaitingBarIndicatorElement1.InnerRadius = 8;
            this.dotsRingWaitingBarIndicatorElement1.LastDotRadius = 4;
            this.dotsRingWaitingBarIndicatorElement1.Name = "dotsRingWaitingBarIndicatorElement1";
            this.dotsRingWaitingBarIndicatorElement1.NumberOfColors = 2;
            this.dotsRingWaitingBarIndicatorElement1.Opacity = 0.8D;
            this.dotsRingWaitingBarIndicatorElement1.PositionOffset = new System.Drawing.SizeF(0F, 0F);
            this.dotsRingWaitingBarIndicatorElement1.Radius = 80;
            this.dotsRingWaitingBarIndicatorElement1.ShowHorizontalLine = false;
            this.dotsRingWaitingBarIndicatorElement1.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
            this.dotsRingWaitingBarIndicatorElement1.Text = "加载中...";
            this.dotsRingWaitingBarIndicatorElement1.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            // 
            // WaitForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1011, 814);
            this.Controls.Add(this.radWaitingBar1);
            this.Name = "WaitForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "WaitForm";
            this.Load += new System.EventHandler(this.WaitForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radWaitingBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Telerik.WinControls.Tests.DonutShape donutShape1;
        private Telerik.WinControls.OldShapeEditor.CustomShape customShape1;
        private Telerik.WinControls.EllipseShape ellipseShape1;
        private Telerik.WinControls.ChamferedRectShape chamferedRectShape1;
        private Telerik.WinControls.UI.RadWaitingBar radWaitingBar1;
        private Telerik.WinControls.UI.DotsRingWaitingBarIndicatorElement dotsRingWaitingBarIndicatorElement1;
    }
}