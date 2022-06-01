namespace NJIS.FPZWS.LineControl.Cutting.UI.Views.Controls.ModuleControl
{
    partial class CurTaskDetailControl
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
            this.radGroupBox1 = new Telerik.WinControls.UI.RadGroupBox();
            this.gvwMaterials = new NJIS.FPZWS.UI.Common.Controls.GridViewBase();
            this.txtCurTask = new Telerik.WinControls.UI.RadTextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.gvwOffCuts = new NJIS.FPZWS.UI.Common.Controls.GridViewBase();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gvwPatterns = new NJIS.FPZWS.UI.Common.Controls.GridViewBase();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.gvwParts_UDI = new NJIS.FPZWS.UI.Common.Controls.GridViewBase();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.lbNextCuttingPattern = new Telerik.WinControls.UI.RadLabel();
            this.radLabel4 = new Telerik.WinControls.UI.RadLabel();
            this.lbCurCuttingPattern = new Telerik.WinControls.UI.RadLabel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.radLabel3 = new Telerik.WinControls.UI.RadLabel();
            this.txtBatchName = new Telerik.WinControls.UI.RadTextBox();
            this.radTextBox1 = new Telerik.WinControls.UI.RadTextBox();
            this.btnDeviceError = new Telerik.WinControls.UI.RadButton();
            this.btnPatternError = new Telerik.WinControls.UI.RadButton();
            this.radLabel5 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel6 = new Telerik.WinControls.UI.RadLabel();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox1)).BeginInit();
            this.radGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurTask)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbNextCuttingPattern)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbCurCuttingPattern)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBatchName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBox1)).BeginInit();
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
            // radGroupBox1
            // 
            this.radGroupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBox1.Controls.Add(this.gvwMaterials);
            this.radGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radGroupBox1.HeaderText = "物料";
            this.radGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.radGroupBox1.Margin = new System.Windows.Forms.Padding(0);
            this.radGroupBox1.Name = "radGroupBox1";
            this.radGroupBox1.Size = new System.Drawing.Size(1067, 1);
            this.radGroupBox1.TabIndex = 3;
            this.radGroupBox1.Text = "物料";
            // 
            // gvwMaterials
            // 
            this.gvwMaterials.AllowAddNewRow = false;
            this.gvwMaterials.AllowCheckSort = false;
            this.gvwMaterials.AllowDeleteRow = false;
            this.gvwMaterials.AllowEditRow = true;
            this.gvwMaterials.AllowSelectAll = true;
            this.gvwMaterials.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.gvwMaterials.ChangedValueForeColor = System.Drawing.Color.DeepPink;
            this.gvwMaterials.DataSource = null;
            this.gvwMaterials.DefaultValueForeColor = System.Drawing.Color.Black;
            this.gvwMaterials.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvwMaterials.EnableFiltering = true;
            this.gvwMaterials.EnablePaging = false;
            this.gvwMaterials.EnableSorting = true;
            this.gvwMaterials.Location = new System.Drawing.Point(2, 18);
            this.gvwMaterials.Margin = new System.Windows.Forms.Padding(0);
            this.gvwMaterials.Name = "gvwMaterials";
            this.gvwMaterials.NewAddRowForeColor = System.Drawing.Color.Fuchsia;
            this.gvwMaterials.PageSize = 20;
            this.gvwMaterials.ReadOnly = true;
            this.gvwMaterials.ShowCheckBox = false;
            this.gvwMaterials.ShowRowHeaderColumn = false;
            this.gvwMaterials.ShowRowNumber = true;
            this.gvwMaterials.Size = new System.Drawing.Size(1063, 0);
            this.gvwMaterials.TabIndex = 2;
            // 
            // txtCurTask
            // 
            this.txtCurTask.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtCurTask.Location = new System.Drawing.Point(66, 3);
            this.txtCurTask.Name = "txtCurTask";
            this.txtCurTask.Size = new System.Drawing.Size(213, 32);
            this.txtCurTask.TabIndex = 5;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.radGroupBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 71);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 0F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1067, 480);
            this.tableLayoutPanel1.TabIndex = 6;
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
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1067, 480);
            this.tableLayoutPanel2.TabIndex = 4;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.gvwOffCuts);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(767, 0);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(300, 480);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "余料";
            // 
            // gvwOffCuts
            // 
            this.gvwOffCuts.AllowAddNewRow = false;
            this.gvwOffCuts.AllowCheckSort = false;
            this.gvwOffCuts.AllowDeleteRow = false;
            this.gvwOffCuts.AllowEditRow = true;
            this.gvwOffCuts.AllowSelectAll = true;
            this.gvwOffCuts.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.gvwOffCuts.ChangedValueForeColor = System.Drawing.Color.DeepPink;
            this.gvwOffCuts.DataSource = null;
            this.gvwOffCuts.DefaultValueForeColor = System.Drawing.Color.Black;
            this.gvwOffCuts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvwOffCuts.EnableFiltering = true;
            this.gvwOffCuts.EnablePaging = false;
            this.gvwOffCuts.EnableSorting = true;
            this.gvwOffCuts.Location = new System.Drawing.Point(3, 17);
            this.gvwOffCuts.Margin = new System.Windows.Forms.Padding(1);
            this.gvwOffCuts.Name = "gvwOffCuts";
            this.gvwOffCuts.NewAddRowForeColor = System.Drawing.Color.Fuchsia;
            this.gvwOffCuts.PageSize = 20;
            this.gvwOffCuts.ReadOnly = true;
            this.gvwOffCuts.ShowCheckBox = false;
            this.gvwOffCuts.ShowRowHeaderColumn = false;
            this.gvwOffCuts.ShowRowNumber = true;
            this.gvwOffCuts.Size = new System.Drawing.Size(294, 460);
            this.gvwOffCuts.TabIndex = 0;
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
            this.groupBox3.Controls.Add(this.gvwParts_UDI);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(469, 0);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(298, 480);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "板件清单";
            // 
            // gvwParts_UDI
            // 
            this.gvwParts_UDI.AllowAddNewRow = false;
            this.gvwParts_UDI.AllowCheckSort = false;
            this.gvwParts_UDI.AllowDeleteRow = false;
            this.gvwParts_UDI.AllowEditRow = true;
            this.gvwParts_UDI.AllowSelectAll = true;
            this.gvwParts_UDI.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.gvwParts_UDI.ChangedValueForeColor = System.Drawing.Color.DeepPink;
            this.gvwParts_UDI.DataSource = null;
            this.gvwParts_UDI.DefaultValueForeColor = System.Drawing.Color.Black;
            this.gvwParts_UDI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvwParts_UDI.EnableFiltering = true;
            this.gvwParts_UDI.EnablePaging = false;
            this.gvwParts_UDI.EnableSorting = true;
            this.gvwParts_UDI.Location = new System.Drawing.Point(3, 17);
            this.gvwParts_UDI.Margin = new System.Windows.Forms.Padding(1);
            this.gvwParts_UDI.Name = "gvwParts_UDI";
            this.gvwParts_UDI.NewAddRowForeColor = System.Drawing.Color.Fuchsia;
            this.gvwParts_UDI.PageSize = 20;
            this.gvwParts_UDI.ReadOnly = true;
            this.gvwParts_UDI.ShowCheckBox = false;
            this.gvwParts_UDI.ShowRowHeaderColumn = false;
            this.gvwParts_UDI.ShowRowNumber = true;
            this.gvwParts_UDI.Size = new System.Drawing.Size(292, 460);
            this.gvwParts_UDI.TabIndex = 1;
            // 
            // radLabel2
            // 
            this.radLabel2.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radLabel2.Location = new System.Drawing.Point(456, 3);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(158, 31);
            this.radLabel2.TabIndex = 7;
            this.radLabel2.Text = "下一个锯切图号:";
            // 
            // lbNextCuttingPattern
            // 
            this.lbNextCuttingPattern.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbNextCuttingPattern.Location = new System.Drawing.Point(620, 3);
            this.lbNextCuttingPattern.Name = "lbNextCuttingPattern";
            this.lbNextCuttingPattern.Size = new System.Drawing.Size(21, 31);
            this.lbNextCuttingPattern.TabIndex = 8;
            this.lbNextCuttingPattern.Text = "0";
            // 
            // radLabel4
            // 
            this.radLabel4.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radLabel4.Location = new System.Drawing.Point(285, 3);
            this.radLabel4.Name = "radLabel4";
            this.radLabel4.Size = new System.Drawing.Size(138, 31);
            this.radLabel4.TabIndex = 7;
            this.radLabel4.Text = "当前锯切图号:";
            // 
            // lbCurCuttingPattern
            // 
            this.lbCurCuttingPattern.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbCurCuttingPattern.ForeColor = System.Drawing.Color.OrangeRed;
            this.lbCurCuttingPattern.Location = new System.Drawing.Point(429, 3);
            this.lbCurCuttingPattern.Margin = new System.Windows.Forms.Padding(3, 3, 3, 8);
            this.lbCurCuttingPattern.Name = "lbCurCuttingPattern";
            this.lbCurCuttingPattern.Size = new System.Drawing.Size(21, 31);
            this.lbCurCuttingPattern.TabIndex = 8;
            this.lbCurCuttingPattern.Text = "0";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.radLabel1);
            this.flowLayoutPanel1.Controls.Add(this.txtCurTask);
            this.flowLayoutPanel1.Controls.Add(this.radLabel4);
            this.flowLayoutPanel1.Controls.Add(this.lbCurCuttingPattern);
            this.flowLayoutPanel1.Controls.Add(this.radLabel2);
            this.flowLayoutPanel1.Controls.Add(this.lbNextCuttingPattern);
            this.flowLayoutPanel1.Controls.Add(this.radLabel3);
            this.flowLayoutPanel1.Controls.Add(this.txtBatchName);
            this.flowLayoutPanel1.Controls.Add(this.radTextBox1);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(6, 26);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1061, 39);
            this.flowLayoutPanel1.TabIndex = 9;
            // 
            // radLabel3
            // 
            this.radLabel3.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radLabel3.Location = new System.Drawing.Point(647, 3);
            this.radLabel3.Name = "radLabel3";
            this.radLabel3.Size = new System.Drawing.Size(71, 31);
            this.radLabel3.TabIndex = 1;
            this.radLabel3.Text = "批次：";
            // 
            // txtBatchName
            // 
            this.txtBatchName.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtBatchName.Location = new System.Drawing.Point(724, 3);
            this.txtBatchName.Name = "txtBatchName";
            this.txtBatchName.Size = new System.Drawing.Size(253, 32);
            this.txtBatchName.TabIndex = 5;
            // 
            // radTextBox1
            // 
            this.radTextBox1.Location = new System.Drawing.Point(3, 45);
            this.radTextBox1.Name = "radTextBox1";
            this.radTextBox1.Size = new System.Drawing.Size(100, 20);
            this.radTextBox1.TabIndex = 9;
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
            this.Controls.Add(this.radLabel6);
            this.Controls.Add(this.radLabel5);
            this.Controls.Add(this.btnDeviceError);
            this.Controls.Add(this.btnPatternError);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "CurTaskDetailControl";
            this.Size = new System.Drawing.Size(1073, 636);
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox1)).EndInit();
            this.radGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtCurTask)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbNextCuttingPattern)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbCurCuttingPattern)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBatchName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnDeviceError)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnPatternError)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel6)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private FPZWS.UI.Common.Controls.GridViewBase gvwMaterials;
        private Telerik.WinControls.UI.RadGroupBox radGroupBox1;
        private Telerik.WinControls.UI.RadTextBox txtCurTask;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Telerik.WinControls.UI.RadLabel radLabel2;
        private Telerik.WinControls.UI.RadLabel lbNextCuttingPattern;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.GroupBox groupBox2;
        private FPZWS.UI.Common.Controls.GridViewBase gvwOffCuts;
        private System.Windows.Forms.GroupBox groupBox1;
        private FPZWS.UI.Common.Controls.GridViewBase gvwPatterns;
        private System.Windows.Forms.GroupBox groupBox3;
        private FPZWS.UI.Common.Controls.GridViewBase gvwParts_UDI;
        private Telerik.WinControls.UI.RadLabel radLabel4;
        private Telerik.WinControls.UI.RadLabel lbCurCuttingPattern;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private Telerik.WinControls.UI.RadLabel radLabel3;
        private Telerik.WinControls.UI.RadTextBox txtBatchName;
        private Telerik.WinControls.UI.RadLabel radLabel5;
        private Telerik.WinControls.UI.RadButton btnDeviceError;
        private Telerik.WinControls.UI.RadButton btnPatternError;
        private Telerik.WinControls.UI.RadLabel radLabel6;
        private Telerik.WinControls.UI.RadTextBox radTextBox1;
    }
}
