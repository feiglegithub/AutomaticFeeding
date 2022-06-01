namespace NJIS.FPZWS.LineControl.Cutting.UI.Views.Controls.ModuleControl
{
    partial class TaskControl
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
            this.lbDeviceName = new Telerik.WinControls.UI.RadLabel();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.btnGetMDB = new Telerik.WinControls.UI.RadButton();
            this.btnGetStackList = new Telerik.WinControls.UI.RadButton();
            this.dtpPlanDate = new Telerik.WinControls.UI.RadDateTimePicker();
            this.radGroupBox1 = new Telerik.WinControls.UI.RadGroupBox();
            this.gridViewBase1 = new NJIS.FPZWS.UI.Common.Controls.GridViewBase();
            this.itemNamesControl1 = new NJIS.FPZWS.LineControl.Cutting.UI.Views.Controls.ItemNamesControl();
            ((System.ComponentModel.ISupportInitialize)(this.lbDeviceName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnGetMDB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnGetStackList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpPlanDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox1)).BeginInit();
            this.radGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbDeviceName
            // 
            this.lbDeviceName.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbDeviceName.Location = new System.Drawing.Point(149, 8);
            this.lbDeviceName.Name = "lbDeviceName";
            this.lbDeviceName.Size = new System.Drawing.Size(85, 25);
            this.lbDeviceName.TabIndex = 24;
            this.lbDeviceName.Text = "radLabel2";
            // 
            // radLabel1
            // 
            this.radLabel1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radLabel1.Location = new System.Drawing.Point(35, 3);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(107, 25);
            this.radLabel1.TabIndex = 23;
            this.radLabel1.Text = "当前设备号：";
            // 
            // btnGetMDB
            // 
            this.btnGetMDB.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnGetMDB.Location = new System.Drawing.Point(497, 8);
            this.btnGetMDB.Name = "btnGetMDB";
            this.btnGetMDB.Size = new System.Drawing.Size(91, 24);
            this.btnGetMDB.TabIndex = 22;
            this.btnGetMDB.Text = "获取MDB";
            this.btnGetMDB.Click += new System.EventHandler(this.btnGetMDB_Click);
            // 
            // btnGetStackList
            // 
            this.btnGetStackList.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetStackList.Location = new System.Drawing.Point(435, 8);
            this.btnGetStackList.Name = "btnGetStackList";
            this.btnGetStackList.Size = new System.Drawing.Size(56, 24);
            this.btnGetStackList.TabIndex = 21;
            this.btnGetStackList.Text = "查询";
            this.btnGetStackList.Click += new System.EventHandler(this.btnGetStackList_Click);
            // 
            // dtpPlanDate
            // 
            this.dtpPlanDate.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpPlanDate.Location = new System.Drawing.Point(265, 4);
            this.dtpPlanDate.Name = "dtpPlanDate";
            this.dtpPlanDate.Size = new System.Drawing.Size(164, 27);
            this.dtpPlanDate.TabIndex = 20;
            this.dtpPlanDate.TabStop = false;
            this.dtpPlanDate.Text = "2018年10月20日";
            this.dtpPlanDate.Value = new System.DateTime(2018, 10, 20, 0, 0, 0, 0);
            // 
            // radGroupBox1
            // 
            this.radGroupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.radGroupBox1.Controls.Add(this.itemNamesControl1);
            this.radGroupBox1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radGroupBox1.HeaderText = "本地MDB";
            this.radGroupBox1.Location = new System.Drawing.Point(3, 37);
            this.radGroupBox1.Name = "radGroupBox1";
            this.radGroupBox1.Size = new System.Drawing.Size(264, 503);
            this.radGroupBox1.TabIndex = 25;
            this.radGroupBox1.Text = "本地MDB";
            // 
            // gridViewBase1
            // 
            this.gridViewBase1.AllowAddNewRow = false;
            this.gridViewBase1.AllowDeleteRow = false;
            this.gridViewBase1.AllowEditRow = true;
            this.gridViewBase1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridViewBase1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.gridViewBase1.DataSource = null;
            this.gridViewBase1.Location = new System.Drawing.Point(271, 55);
            this.gridViewBase1.Name = "gridViewBase1";
            this.gridViewBase1.ReadOnly = true;
            this.gridViewBase1.ShowCheckBox = false;
            this.gridViewBase1.ShowRowHeaderColumn = false;
            this.gridViewBase1.ShowRowNumber = true;
            this.gridViewBase1.Size = new System.Drawing.Size(488, 483);
            this.gridViewBase1.TabIndex = 26;
            // 
            // itemNamesControl1
            // 
            this.itemNamesControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.itemNamesControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.itemNamesControl1.Location = new System.Drawing.Point(2, 18);
            this.itemNamesControl1.Name = "itemNamesControl1";
            this.itemNamesControl1.Size = new System.Drawing.Size(260, 483);
            this.itemNamesControl1.TabIndex = 0;
            // 
            // TaskControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.Controls.Add(this.gridViewBase1);
            this.Controls.Add(this.radGroupBox1);
            this.Controls.Add(this.lbDeviceName);
            this.Controls.Add(this.radLabel1);
            this.Controls.Add(this.btnGetMDB);
            this.Controls.Add(this.btnGetStackList);
            this.Controls.Add(this.dtpPlanDate);
            this.Name = "TaskControl";
            this.Size = new System.Drawing.Size(767, 543);
            ((System.ComponentModel.ISupportInitialize)(this.lbDeviceName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnGetMDB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnGetStackList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpPlanDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox1)).EndInit();
            this.radGroupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadLabel lbDeviceName;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadButton btnGetMDB;
        private Telerik.WinControls.UI.RadButton btnGetStackList;
        private Telerik.WinControls.UI.RadDateTimePicker dtpPlanDate;
        private Telerik.WinControls.UI.RadGroupBox radGroupBox1;
        private ItemNamesControl itemNamesControl1;
        private FPZWS.UI.Common.Controls.GridViewBase gridViewBase1;
    }
}
