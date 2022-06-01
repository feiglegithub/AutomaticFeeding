namespace NJIS.FPZWS.LineControl.PartMill.Simulator.Controls
{
    partial class LineCommandControl
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
            this.fromList = new Telerik.WinControls.UI.RadDropDownList();
            this.targetList = new Telerik.WinControls.UI.RadDropDownList();
            this.btnExecute = new Telerik.WinControls.UI.RadButton();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel3 = new Telerik.WinControls.UI.RadLabel();
            this.txtAmount = new Telerik.WinControls.UI.RadTextBox();
            this.txtPilerNo = new Telerik.WinControls.UI.RadTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.fromList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.targetList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnExecute)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPilerNo)).BeginInit();
            this.SuspendLayout();
            // 
            // fromList
            // 
            this.fromList.Location = new System.Drawing.Point(27, 5);
            this.fromList.Name = "fromList";
            this.fromList.Size = new System.Drawing.Size(125, 20);
            this.fromList.TabIndex = 0;
            this.fromList.Text = "radDropDownList1";
            // 
            // targetList
            // 
            this.targetList.Location = new System.Drawing.Point(229, 5);
            this.targetList.Name = "targetList";
            this.targetList.Size = new System.Drawing.Size(125, 20);
            this.targetList.TabIndex = 0;
            this.targetList.Text = "radDropDownList1";
            // 
            // btnExecute
            // 
            this.btnExecute.Location = new System.Drawing.Point(360, 3);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(49, 22);
            this.btnExecute.TabIndex = 2;
            this.btnExecute.Text = "执行";
            // 
            // radLabel1
            // 
            this.radLabel1.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radLabel1.Location = new System.Drawing.Point(168, 0);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(55, 31);
            this.radLabel1.TabIndex = 3;
            this.radLabel1.Text = "==>";
            // 
            // radLabel2
            // 
            this.radLabel2.Location = new System.Drawing.Point(27, 32);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(42, 18);
            this.radLabel2.TabIndex = 4;
            this.radLabel2.Text = "垛号：";
            // 
            // radLabel3
            // 
            this.radLabel3.Location = new System.Drawing.Point(181, 32);
            this.radLabel3.Name = "radLabel3";
            this.radLabel3.Size = new System.Drawing.Size(42, 18);
            this.radLabel3.TabIndex = 5;
            this.radLabel3.Text = "数量：";
            // 
            // txtAmount
            // 
            this.txtAmount.Location = new System.Drawing.Point(243, 32);
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.Size = new System.Drawing.Size(100, 20);
            this.txtAmount.TabIndex = 6;
            // 
            // txtPilerNo
            // 
            this.txtPilerNo.Location = new System.Drawing.Point(75, 31);
            this.txtPilerNo.Name = "txtPilerNo";
            this.txtPilerNo.Size = new System.Drawing.Size(100, 20);
            this.txtPilerNo.TabIndex = 6;
            // 
            // LineCommandControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.Controls.Add(this.txtPilerNo);
            this.Controls.Add(this.txtAmount);
            this.Controls.Add(this.radLabel3);
            this.Controls.Add(this.radLabel2);
            this.Controls.Add(this.targetList);
            this.Controls.Add(this.radLabel1);
            this.Controls.Add(this.btnExecute);
            this.Controls.Add(this.fromList);
            this.Name = "LineCommandControl";
            this.Size = new System.Drawing.Size(417, 64);
            ((System.ComponentModel.ISupportInitialize)(this.fromList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.targetList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnExecute)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPilerNo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadDropDownList fromList;
        private Telerik.WinControls.UI.RadDropDownList targetList;
        private Telerik.WinControls.UI.RadButton btnExecute;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadLabel radLabel2;
        private Telerik.WinControls.UI.RadLabel radLabel3;
        private Telerik.WinControls.UI.RadTextBox txtAmount;
        private Telerik.WinControls.UI.RadTextBox txtPilerNo;
    }
}
