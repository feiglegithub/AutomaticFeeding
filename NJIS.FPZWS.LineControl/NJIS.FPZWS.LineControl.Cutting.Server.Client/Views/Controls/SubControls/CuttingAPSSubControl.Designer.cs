namespace NJIS.FPZWS.LineControl.Cutting.Server.Client.Views.Controls.SubControls
{
    partial class CuttingAPSSubControl
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.radGroupBox6 = new Telerik.WinControls.UI.RadGroupBox();
            this.gridViewBase3 = new NJIS.FPZWS.UI.Common.Controls.GridViewBase();
            this.radGroupBox4 = new Telerik.WinControls.UI.RadGroupBox();
            this.gridViewBase2 = new NJIS.FPZWS.UI.Common.Controls.GridViewBase();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.cmbBatch = new System.Windows.Forms.ComboBox();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.cmbDevice = new System.Windows.Forms.ComboBox();
            this.radLabel3 = new Telerik.WinControls.UI.RadLabel();
            this.cmbStack = new System.Windows.Forms.ComboBox();
            this.radLabel4 = new Telerik.WinControls.UI.RadLabel();
            this.txtTotalTime = new Telerik.WinControls.UI.RadTextBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox6)).BeginInit();
            this.radGroupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox4)).BeginInit();
            this.radGroupBox4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalTime)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 47.85992F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 52.14008F));
            this.tableLayoutPanel1.Controls.Add(this.radGroupBox6, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.radGroupBox4, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 19);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(910, 503);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // radGroupBox6
            // 
            this.radGroupBox6.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBox6.Controls.Add(this.gridViewBase3);
            this.radGroupBox6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radGroupBox6.HeaderText = "任务详细";
            this.radGroupBox6.Location = new System.Drawing.Point(438, 3);
            this.radGroupBox6.Name = "radGroupBox6";
            this.radGroupBox6.Size = new System.Drawing.Size(469, 497);
            this.radGroupBox6.TabIndex = 38;
            this.radGroupBox6.Text = "任务详细";
            // 
            // gridViewBase3
            // 
            this.gridViewBase3.AllowAddNewRow = false;
            this.gridViewBase3.AllowCheckSort = false;
            this.gridViewBase3.AllowDeleteRow = false;
            this.gridViewBase3.AllowEditRow = true;
            this.gridViewBase3.AllowSelectAll = true;
            this.gridViewBase3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.gridViewBase3.ChangedValueForeColor = System.Drawing.Color.DeepPink;
            this.gridViewBase3.DataSource = null;
            this.gridViewBase3.DefaultValueForeColor = System.Drawing.Color.Black;
            this.gridViewBase3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridViewBase3.EnableFiltering = true;
            this.gridViewBase3.EnablePaging = false;
            this.gridViewBase3.EnableSorting = true;
            this.gridViewBase3.Location = new System.Drawing.Point(2, 18);
            this.gridViewBase3.Name = "gridViewBase3";
            this.gridViewBase3.NewAddRowForeColor = System.Drawing.Color.Fuchsia;
            this.gridViewBase3.PageSize = 20;
            this.gridViewBase3.ReadOnly = false;
            this.gridViewBase3.ShowCheckBox = false;
            this.gridViewBase3.ShowRowHeaderColumn = false;
            this.gridViewBase3.ShowRowNumber = true;
            this.gridViewBase3.Size = new System.Drawing.Size(465, 477);
            this.gridViewBase3.TabIndex = 0;
            // 
            // radGroupBox4
            // 
            this.radGroupBox4.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBox4.Controls.Add(this.gridViewBase2);
            this.radGroupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radGroupBox4.HeaderText = "计算结果";
            this.radGroupBox4.Location = new System.Drawing.Point(3, 3);
            this.radGroupBox4.Name = "radGroupBox4";
            this.radGroupBox4.Size = new System.Drawing.Size(429, 497);
            this.radGroupBox4.TabIndex = 37;
            this.radGroupBox4.Text = "计算结果";
            // 
            // gridViewBase2
            // 
            this.gridViewBase2.AllowAddNewRow = false;
            this.gridViewBase2.AllowCheckSort = false;
            this.gridViewBase2.AllowDeleteRow = false;
            this.gridViewBase2.AllowEditRow = true;
            this.gridViewBase2.AllowSelectAll = true;
            this.gridViewBase2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.gridViewBase2.ChangedValueForeColor = System.Drawing.Color.DeepPink;
            this.gridViewBase2.DataSource = null;
            this.gridViewBase2.DefaultValueForeColor = System.Drawing.Color.Black;
            this.gridViewBase2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridViewBase2.EnableFiltering = true;
            this.gridViewBase2.EnablePaging = false;
            this.gridViewBase2.EnableSorting = true;
            this.gridViewBase2.Location = new System.Drawing.Point(2, 18);
            this.gridViewBase2.Name = "gridViewBase2";
            this.gridViewBase2.NewAddRowForeColor = System.Drawing.Color.Fuchsia;
            this.gridViewBase2.PageSize = 20;
            this.gridViewBase2.ReadOnly = false;
            this.gridViewBase2.ShowCheckBox = false;
            this.gridViewBase2.ShowRowHeaderColumn = false;
            this.gridViewBase2.ShowRowNumber = true;
            this.gridViewBase2.Size = new System.Drawing.Size(425, 477);
            this.gridViewBase2.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(0, 41);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(916, 525);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "计算详情";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.Controls.Add(this.radLabel2);
            this.flowLayoutPanel1.Controls.Add(this.cmbBatch);
            this.flowLayoutPanel1.Controls.Add(this.radLabel1);
            this.flowLayoutPanel1.Controls.Add(this.cmbDevice);
            this.flowLayoutPanel1.Controls.Add(this.radLabel3);
            this.flowLayoutPanel1.Controls.Add(this.cmbStack);
            this.flowLayoutPanel1.Controls.Add(this.radLabel4);
            this.flowLayoutPanel1.Controls.Add(this.txtTotalTime);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(1);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(913, 36);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // radLabel2
            // 
            this.radLabel2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radLabel2.Location = new System.Drawing.Point(3, 3);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(41, 25);
            this.radLabel2.TabIndex = 0;
            this.radLabel2.Text = "批次";
            // 
            // cmbBatch
            // 
            this.cmbBatch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBatch.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbBatch.FormattingEnabled = true;
            this.cmbBatch.Location = new System.Drawing.Point(50, 3);
            this.cmbBatch.Name = "cmbBatch";
            this.cmbBatch.Size = new System.Drawing.Size(207, 29);
            this.cmbBatch.TabIndex = 1;
            // 
            // radLabel1
            // 
            this.radLabel1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radLabel1.Location = new System.Drawing.Point(263, 3);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(41, 25);
            this.radLabel1.TabIndex = 0;
            this.radLabel1.Text = "设备";
            // 
            // cmbDevice
            // 
            this.cmbDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDevice.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbDevice.FormattingEnabled = true;
            this.cmbDevice.Location = new System.Drawing.Point(310, 3);
            this.cmbDevice.Name = "cmbDevice";
            this.cmbDevice.Size = new System.Drawing.Size(175, 29);
            this.cmbDevice.TabIndex = 1;
            // 
            // radLabel3
            // 
            this.radLabel3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radLabel3.Location = new System.Drawing.Point(491, 3);
            this.radLabel3.Name = "radLabel3";
            this.radLabel3.Size = new System.Drawing.Size(24, 25);
            this.radLabel3.TabIndex = 0;
            this.radLabel3.Text = "垛";
            // 
            // cmbStack
            // 
            this.cmbStack.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStack.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbStack.FormattingEnabled = true;
            this.cmbStack.Location = new System.Drawing.Point(521, 3);
            this.cmbStack.Name = "cmbStack";
            this.cmbStack.Size = new System.Drawing.Size(171, 29);
            this.cmbStack.TabIndex = 1;
            // 
            // radLabel4
            // 
            this.radLabel4.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.radLabel4.Location = new System.Drawing.Point(698, 3);
            this.radLabel4.Name = "radLabel4";
            this.radLabel4.Size = new System.Drawing.Size(41, 25);
            this.radLabel4.TabIndex = 2;
            this.radLabel4.Text = "耗时";
            // 
            // txtTotalTime
            // 
            this.txtTotalTime.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.txtTotalTime.Location = new System.Drawing.Point(745, 5);
            this.txtTotalTime.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.txtTotalTime.Name = "txtTotalTime";
            this.txtTotalTime.ReadOnly = true;
            this.txtTotalTime.Size = new System.Drawing.Size(100, 27);
            this.txtTotalTime.TabIndex = 3;
            // 
            // CuttingAPSSubControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.groupBox1);
            this.Name = "CuttingAPSSubControl";
            this.Size = new System.Drawing.Size(919, 569);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox6)).EndInit();
            this.radGroupBox6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox4)).EndInit();
            this.radGroupBox4.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalTime)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Telerik.WinControls.UI.RadGroupBox radGroupBox4;
        private Telerik.WinControls.UI.RadGroupBox radGroupBox6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private Telerik.WinControls.UI.RadLabel radLabel2;
        private Telerik.WinControls.UI.RadLabel radLabel3;
        private System.Windows.Forms.ComboBox cmbBatch;
        private System.Windows.Forms.ComboBox cmbStack;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private System.Windows.Forms.ComboBox cmbDevice;
        private UI.Common.Controls.GridViewBase gridViewBase3;
        private UI.Common.Controls.GridViewBase gridViewBase2;
        private Telerik.WinControls.UI.RadLabel radLabel4;
        private Telerik.WinControls.UI.RadTextBox txtTotalTime;
    }
}
