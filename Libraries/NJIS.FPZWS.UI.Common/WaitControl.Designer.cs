namespace NJIS.FPZWS.UI.Common
{
    partial class WaitControl
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
            this.radWaitingBar1 = new Telerik.WinControls.UI.RadWaitingBar();
            this.dotsRingWaitingBarIndicatorElement1 = new Telerik.WinControls.UI.DotsRingWaitingBarIndicatorElement();
            ((System.ComponentModel.ISupportInitialize)(this.radWaitingBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // radWaitingBar1
            // 
            this.radWaitingBar1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.radWaitingBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radWaitingBar1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radWaitingBar1.Location = new System.Drawing.Point(0, 0);
            this.radWaitingBar1.Name = "radWaitingBar1";
            this.radWaitingBar1.Size = new System.Drawing.Size(420, 370);
            this.radWaitingBar1.TabIndex = 3;
            this.radWaitingBar1.Text = "radWaitingBar1";
            this.radWaitingBar1.WaitingIndicators.Add(this.dotsRingWaitingBarIndicatorElement1);
            this.radWaitingBar1.WaitingSpeed = 50;
            this.radWaitingBar1.WaitingStyle = Telerik.WinControls.Enumerations.WaitingBarStyles.DotsRing;
            // 
            // dotsRingWaitingBarIndicatorElement1
            // 
            this.dotsRingWaitingBarIndicatorElement1.BorderBoxStyle = Telerik.WinControls.BorderBoxStyle.SingleBorder;
            this.dotsRingWaitingBarIndicatorElement1.BorderDashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            this.dotsRingWaitingBarIndicatorElement1.DisabledTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
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
            this.dotsRingWaitingBarIndicatorElement1.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.dotsRingWaitingBarIndicatorElement1.UseCompatibleTextRendering = false;
            // 
            // WaitControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.radWaitingBar1);
            this.Name = "WaitControl";
            this.Size = new System.Drawing.Size(420, 370);
            ((System.ComponentModel.ISupportInitialize)(this.radWaitingBar1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadWaitingBar radWaitingBar1;
        private Telerik.WinControls.UI.DotsRingWaitingBarIndicatorElement dotsRingWaitingBarIndicatorElement1;
    }
}
