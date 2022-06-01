namespace NJIS.FPZWS.LineControl.CuttingDevice.Views.Controls.ModuleControl
{
    partial class CurTaskDetailControlOld
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
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.txtCurTask = new Telerik.WinControls.UI.RadTextBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.gvwOffParts = new NJIS.FPZWS.UI.Common.Controls.GridViewBase();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gvwPatterns = new NJIS.FPZWS.UI.Common.Controls.GridViewBase();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.gvwParts = new NJIS.FPZWS.UI.Common.Controls.GridViewBase();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.radLabel3 = new Telerik.WinControls.UI.RadLabel();
            this.txtNextTask = new Telerik.WinControls.UI.RadTextBox();
            this.btnDeviceError = new Telerik.WinControls.UI.RadButton();
            this.btnPatternError = new Telerik.WinControls.UI.RadButton();
            this.radLabel5 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel6 = new Telerik.WinControls.UI.RadLabel();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurTask)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNextTask)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnDeviceError)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnPatternError)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel6)).BeginInit();
            this.SuspendLayout();
            // 
            // radLabel1
            // 
            this.radLabel1.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radLabel1.Location = new System.Drawing.Point(3, 3);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(57, 25);
            this.radLabel1.TabIndex = 1;
            this.radLabel1.Text = "任务：";
            // 
            // txtCurTask
            // 
            this.txtCurTask.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtCurTask.Location = new System.Drawing.Point(66, 3);
            this.txtCurTask.Name = "txtCurTask";
            this.txtCurTask.Size = new System.Drawing.Size(213, 32);
            this.txtCurTask.TabIndex = 5;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 44F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28F));
            this.tableLayoutPanel2.Controls.Add(this.groupBox2, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.groupBox3, 1, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(6, 68);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1061, 497);
            this.tableLayoutPanel2.TabIndex = 4;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.gvwOffParts);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(767, 0);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(300, 480);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "余料";
            // 
            // gvwOffParts
            // 
            this.gvwOffParts.AllowAddNewRow = false;
            this.gvwOffParts.AllowCheckSort = false;
            this.gvwOffParts.AllowDeleteRow = false;
            this.gvwOffParts.AllowEditRow = true;
            this.gvwOffParts.AllowSelectAll = true;
            this.gvwOffParts.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.gvwOffParts.ChangedValueForeColor = System.Drawing.Color.DeepPink;
            this.gvwOffParts.DataSource = null;
            this.gvwOffParts.DefaultValueForeColor = System.Drawing.Color.Black;
            this.gvwOffParts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvwOffParts.EnableFiltering = true;
            this.gvwOffParts.EnablePaging = false;
            this.gvwOffParts.EnableSorting = true;
            this.gvwOffParts.Location = new System.Drawing.Point(3, 17);
            this.gvwOffParts.Margin = new System.Windows.Forms.Padding(1);
            this.gvwOffParts.Name = "gvwOffParts";
            this.gvwOffParts.NewAddRowForeColor = System.Drawing.Color.Fuchsia;
            this.gvwOffParts.PageSize = 20;
            this.gvwOffParts.ReadOnly = true;
            this.gvwOffParts.ShowCheckBox = false;
            this.gvwOffParts.ShowRowHeaderColumn = false;
            this.gvwOffParts.ShowRowNumber = true;
            this.gvwOffParts.Size = new System.Drawing.Size(294, 460);
            this.gvwOffParts.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.gvwPatterns);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(469, 480);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "锯切图";
            // 
            // gvwPatterns
            // 
            this.gvwPatterns.AllowAddNewRow = false;
            this.gvwPatterns.AllowCheckSort = false;
            this.gvwPatterns.AllowDeleteRow = false;
            this.gvwPatterns.AllowEditRow = true;
            this.gvwPatterns.AllowSelectAll = true;
            this.gvwPatterns.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.gvwPatterns.ChangedValueForeColor = System.Drawing.Color.DeepPink;
            this.gvwPatterns.DataSource = null;
            this.gvwPatterns.DefaultValueForeColor = System.Drawing.Color.Black;
            this.gvwPatterns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvwPatterns.EnableFiltering = true;
            this.gvwPatterns.EnablePaging = false;
            this.gvwPatterns.EnableSorting = true;
            this.gvwPatterns.Location = new System.Drawing.Point(3, 17);
            this.gvwPatterns.Margin = new System.Windows.Forms.Padding(1);
            this.gvwPatterns.Name = "gvwPatterns";
            this.gvwPatterns.NewAddRowForeColor = System.Drawing.Color.Fuchsia;
            this.gvwPatterns.PageSize = 20;
            this.gvwPatterns.ReadOnly = true;
            this.gvwPatterns.ShowCheckBox = false;
            this.gvwPatterns.ShowRowHeaderColumn = false;
            this.gvwPatterns.ShowRowNumber = true;
            this.gvwPatterns.Size = new System.Drawing.Size(463, 460);
            this.gvwPatterns.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.gvwParts);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(469, 0);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(298, 480);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "板件清单";
            // 
            // gvwParts
            // 
            this.gvwParts.AllowAddNewRow = false;
            this.gvwParts.AllowCheckSort = false;
            this.gvwParts.AllowDeleteRow = false;
            this.gvwParts.AllowEditRow = true;
            this.gvwParts.AllowSelectAll = true;
            this.gvwParts.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.gvwParts.ChangedValueForeColor = System.Drawing.Color.DeepPink;
            this.gvwParts.DataSource = null;
            this.gvwParts.DefaultValueForeColor = System.Drawing.Color.Black;
            this.gvwParts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvwParts.EnableFiltering = true;
            this.gvwParts.EnablePaging = false;
            this.gvwParts.EnableSorting = true;
            this.gvwParts.Location = new System.Drawing.Point(3, 17);
            this.gvwParts.Margin = new System.Windows.Forms.Padding(1);
            this.gvwParts.Name = "gvwParts";
            this.gvwParts.NewAddRowForeColor = System.Drawing.Color.Fuchsia;
            this.gvwParts.PageSize = 20;
            this.gvwParts.ReadOnly = true;
            this.gvwParts.ShowCheckBox = false;
            this.gvwParts.ShowRowHeaderColumn = false;
            this.gvwParts.ShowRowNumber = true;
            this.gvwParts.Size = new System.Drawing.Size(292, 460);
            this.gvwParts.TabIndex = 1;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.radLabel1);
            this.flowLayoutPanel1.Controls.Add(this.txtCurTask);
            this.flowLayoutPanel1.Controls.Add(this.radLabel3);
            this.flowLayoutPanel1.Controls.Add(this.txtNextTask);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(6, 26);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1061, 39);
            this.flowLayoutPanel1.TabIndex = 9;
            // 
            // radLabel3
            // 
            this.radLabel3.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radLabel3.Location = new System.Drawing.Point(285, 3);
            this.radLabel3.Name = "radLabel3";
            this.radLabel3.Size = new System.Drawing.Size(112, 31);
            this.radLabel3.TabIndex = 1;
            this.radLabel3.Text = "下一任务：";
            // 
            // txtNextTask
            // 
            this.txtNextTask.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtNextTask.Location = new System.Drawing.Point(403, 3);
            this.txtNextTask.Name = "txtNextTask";
            this.txtNextTask.Size = new System.Drawing.Size(253, 32);
            this.txtNextTask.TabIndex = 5;
            // 
            // btnDeviceError
            // 
            this.btnDeviceError.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeviceError.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDeviceError.Location = new System.Drawing.Point(9, 568);
            this.btnDeviceError.Name = "btnDeviceError";
            this.btnDeviceError.Size = new System.Drawing.Size(107, 28);
            this.btnDeviceError.TabIndex = 27;
            this.btnDeviceError.Text = "设备异常";
            this.btnDeviceError.Click += new System.EventHandler(this.btnDeviceError_Click);
            // 
            // btnPatternError
            // 
            this.btnPatternError.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPatternError.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPatternError.Location = new System.Drawing.Point(908, 568);
            this.btnPatternError.Name = "btnPatternError";
            this.btnPatternError.Size = new System.Drawing.Size(160, 28);
            this.btnPatternError.TabIndex = 28;
            this.btnPatternError.Text = "提交异常工件";
            this.btnPatternError.Click += new System.EventHandler(this.btnPatternError_Click);
            // 
            // radLabel5
            // 
            this.radLabel5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.radLabel5.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radLabel5.ForeColor = System.Drawing.Color.Red;
            this.radLabel5.Location = new System.Drawing.Point(9, 602);
            this.radLabel5.Name = "radLabel5";
            this.radLabel5.Size = new System.Drawing.Size(337, 25);
            this.radLabel5.TabIndex = 29;
            this.radLabel5.Text = "设备发生故障需要维修无法继续生产时，提交";
            this.radLabel5.Visible = false;
            // 
            // radLabel6
            // 
            this.radLabel6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.radLabel6.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radLabel6.ForeColor = System.Drawing.Color.Red;
            this.radLabel6.Location = new System.Drawing.Point(623, 602);
            this.radLabel6.Name = "radLabel6";
            this.radLabel6.Size = new System.Drawing.Size(436, 25);
            this.radLabel6.TabIndex = 30;
            this.radLabel6.Text = "当前锯切任务完毕后，如果有大板切割时发生异常时，提交";
            // 
            // CurTaskDetailControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.radLabel6);
            this.Controls.Add(this.radLabel5);
            this.Controls.Add(this.btnDeviceError);
            this.Controls.Add(this.btnPatternError);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "CurTaskDetailControl";
            this.Size = new System.Drawing.Size(1073, 636);
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurTask)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNextTask)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnDeviceError)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnPatternError)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel6)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadTextBox txtCurTask;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.GroupBox groupBox2;
        private FPZWS.UI.Common.Controls.GridViewBase gvwOffParts;
        private System.Windows.Forms.GroupBox groupBox1;
        private FPZWS.UI.Common.Controls.GridViewBase gvwPatterns;
        private System.Windows.Forms.GroupBox groupBox3;
        private FPZWS.UI.Common.Controls.GridViewBase gvwParts;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private Telerik.WinControls.UI.RadLabel radLabel3;
        private Telerik.WinControls.UI.RadTextBox txtNextTask;
        private Telerik.WinControls.UI.RadLabel radLabel5;
        private Telerik.WinControls.UI.RadButton btnDeviceError;
        private Telerik.WinControls.UI.RadButton btnPatternError;
        private Telerik.WinControls.UI.RadLabel radLabel6;
    }
}
