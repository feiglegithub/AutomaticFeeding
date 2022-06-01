namespace ArithmeticsTest.Views
{
    partial class SimulatorView
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
            this.btnBegin = new Telerik.WinControls.UI.RadButton();
            this.txtBatchName = new Telerik.WinControls.UI.RadTextBox();
            this.txtColor = new Telerik.WinControls.UI.RadTextBox();
            this.txtDeviceName = new Telerik.WinControls.UI.RadTextBox();
            this.btnSearch = new Telerik.WinControls.UI.RadButton();
            this.btnDis = new Telerik.WinControls.UI.RadButton();
            this.dtpPlanDate = new Telerik.WinControls.UI.RadDateTimePicker();
            this.cmbSpeed = new Telerik.WinControls.UI.RadDropDownList();
            this.gridViewBase1 = new NJIS.FPZWS.UI.Common.Controls.GridViewBase();
            this.txtCurrent = new Telerik.WinControls.UI.RadTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.btnBegin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBatchName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeviceName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnDis)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpPlanDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrent)).BeginInit();
            this.SuspendLayout();
            // 
            // btnBegin
            // 
            this.btnBegin.Location = new System.Drawing.Point(353, 4);
            this.btnBegin.Name = "btnBegin";
            this.btnBegin.Size = new System.Drawing.Size(39, 24);
            this.btnBegin.TabIndex = 1;
            this.btnBegin.Text = "开始";
            this.btnBegin.Click += new System.EventHandler(this.btnBegin_Click);
            // 
            // txtBatchName
            // 
            this.txtBatchName.Location = new System.Drawing.Point(114, 5);
            this.txtBatchName.Name = "txtBatchName";
            this.txtBatchName.Size = new System.Drawing.Size(100, 20);
            this.txtBatchName.TabIndex = 2;
            // 
            // txtColor
            // 
            this.txtColor.Location = new System.Drawing.Point(220, 6);
            this.txtColor.Name = "txtColor";
            this.txtColor.Size = new System.Drawing.Size(77, 20);
            this.txtColor.TabIndex = 2;
            // 
            // txtDeviceName
            // 
            this.txtDeviceName.Location = new System.Drawing.Point(537, 5);
            this.txtDeviceName.Name = "txtDeviceName";
            this.txtDeviceName.Size = new System.Drawing.Size(40, 20);
            this.txtDeviceName.TabIndex = 5;
            this.txtDeviceName.Text = "1#";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(303, 5);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(44, 21);
            this.btnSearch.TabIndex = 6;
            this.btnSearch.Text = "查询";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnDis
            // 
            this.btnDis.Location = new System.Drawing.Point(398, 4);
            this.btnDis.Name = "btnDis";
            this.btnDis.Size = new System.Drawing.Size(40, 24);
            this.btnDis.TabIndex = 7;
            this.btnDis.Text = "调";
            this.btnDis.Click += new System.EventHandler(this.btnDis_Click);
            // 
            // dtpPlanDate
            // 
            this.dtpPlanDate.Location = new System.Drawing.Point(3, 5);
            this.dtpPlanDate.Name = "dtpPlanDate";
            this.dtpPlanDate.Size = new System.Drawing.Size(105, 20);
            this.dtpPlanDate.TabIndex = 8;
            this.dtpPlanDate.TabStop = false;
            this.dtpPlanDate.Text = "2020年1月8日";
            this.dtpPlanDate.Value = new System.DateTime(2020, 1, 8, 0, 0, 0, 0);
            // 
            // cmbSpeed
            // 
            this.cmbSpeed.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
            this.cmbSpeed.Location = new System.Drawing.Point(444, 6);
            this.cmbSpeed.Name = "cmbSpeed";
            this.cmbSpeed.Size = new System.Drawing.Size(43, 20);
            this.cmbSpeed.TabIndex = 9;
            // 
            // gridViewBase1
            // 
            this.gridViewBase1.AllowAddNewRow = false;
            this.gridViewBase1.AllowCheckSort = false;
            this.gridViewBase1.AllowDeleteRow = false;
            this.gridViewBase1.AllowEditRow = true;
            this.gridViewBase1.AllowSelectAll = true;
            this.gridViewBase1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.gridViewBase1.ChangedValueForeColor = System.Drawing.Color.DeepPink;
            this.gridViewBase1.DataSource = null;
            this.gridViewBase1.DefaultValueForeColor = System.Drawing.Color.Black;
            this.gridViewBase1.EnableFiltering = true;
            this.gridViewBase1.EnablePaging = false;
            this.gridViewBase1.EnableSorting = true;
            this.gridViewBase1.Location = new System.Drawing.Point(0, 31);
            this.gridViewBase1.Name = "gridViewBase1";
            this.gridViewBase1.NewAddRowForeColor = System.Drawing.Color.Fuchsia;
            this.gridViewBase1.PageSize = 20;
            this.gridViewBase1.ReadOnly = false;
            this.gridViewBase1.ShowCheckBox = false;
            this.gridViewBase1.ShowRowHeaderColumn = false;
            this.gridViewBase1.ShowRowNumber = true;
            this.gridViewBase1.Size = new System.Drawing.Size(574, 316);
            this.gridViewBase1.TabIndex = 0;
            // 
            // txtCurrent
            // 
            this.txtCurrent.Location = new System.Drawing.Point(491, 5);
            this.txtCurrent.Name = "txtCurrent";
            this.txtCurrent.ReadOnly = true;
            this.txtCurrent.Size = new System.Drawing.Size(40, 20);
            this.txtCurrent.TabIndex = 5;
            // 
            // SimulatorView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.Controls.Add(this.cmbSpeed);
            this.Controls.Add(this.dtpPlanDate);
            this.Controls.Add(this.btnDis);
            this.Controls.Add(this.btnBegin);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtCurrent);
            this.Controls.Add(this.txtDeviceName);
            this.Controls.Add(this.txtColor);
            this.Controls.Add(this.txtBatchName);
            this.Controls.Add(this.gridViewBase1);
            this.Name = "SimulatorView";
            this.Size = new System.Drawing.Size(577, 351);
            ((System.ComponentModel.ISupportInitialize)(this.btnBegin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBatchName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDeviceName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnDis)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpPlanDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrent)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private NJIS.FPZWS.UI.Common.Controls.GridViewBase gridViewBase1;
        private Telerik.WinControls.UI.RadButton btnBegin;
        private Telerik.WinControls.UI.RadTextBox txtBatchName;
        private Telerik.WinControls.UI.RadTextBox txtColor;
        private Telerik.WinControls.UI.RadTextBox txtDeviceName;
        private Telerik.WinControls.UI.RadButton btnSearch;
        private Telerik.WinControls.UI.RadButton btnDis;
        private Telerik.WinControls.UI.RadDateTimePicker dtpPlanDate;
        private Telerik.WinControls.UI.RadDropDownList cmbSpeed;
        private Telerik.WinControls.UI.RadTextBox txtCurrent;
    }
}
