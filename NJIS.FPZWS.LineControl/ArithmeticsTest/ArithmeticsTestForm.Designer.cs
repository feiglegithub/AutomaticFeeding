namespace ArithmeticsTest
{
    partial class ArithmeticsTestForm
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
            Telerik.WinControls.UI.RadListDataItem radListDataItem11 = new Telerik.WinControls.UI.RadListDataItem();
            Telerik.WinControls.UI.RadListDataItem radListDataItem12 = new Telerik.WinControls.UI.RadListDataItem();
            Telerik.WinControls.UI.RadListDataItem radListDataItem13 = new Telerik.WinControls.UI.RadListDataItem();
            Telerik.WinControls.UI.RadListDataItem radListDataItem14 = new Telerik.WinControls.UI.RadListDataItem();
            Telerik.WinControls.UI.RadListDataItem radListDataItem15 = new Telerik.WinControls.UI.RadListDataItem();
            Telerik.WinControls.UI.RadListDataItem radListDataItem16 = new Telerik.WinControls.UI.RadListDataItem();
            Telerik.WinControls.UI.RadListDataItem radListDataItem17 = new Telerik.WinControls.UI.RadListDataItem();
            Telerik.WinControls.UI.RadListDataItem radListDataItem18 = new Telerik.WinControls.UI.RadListDataItem();
            Telerik.WinControls.UI.RadListDataItem radListDataItem19 = new Telerik.WinControls.UI.RadListDataItem();
            Telerik.WinControls.UI.RadListDataItem radListDataItem20 = new Telerik.WinControls.UI.RadListDataItem();
            this.btnCreated = new Telerik.WinControls.UI.RadButton();
            this.gvbTask = new NJIS.FPZWS.UI.Common.Controls.GridViewBase();
            this.gvbDevice = new NJIS.FPZWS.UI.Common.Controls.GridViewBase();
            this.btnSearch = new Telerik.WinControls.UI.RadButton();
            this.btnSolutions = new Telerik.WinControls.UI.RadButton();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.topCount = new Telerik.WinControls.UI.RadDropDownList();
            this.btnMdb = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.btnCreated)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSolutions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.topCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMdb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCreated
            // 
            this.btnCreated.Location = new System.Drawing.Point(1153, 12);
            this.btnCreated.Name = "btnCreated";
            this.btnCreated.Size = new System.Drawing.Size(72, 24);
            this.btnCreated.TabIndex = 0;
            this.btnCreated.Text = "计算方案";
            this.btnCreated.Click += new System.EventHandler(this.btnCreated_Click);
            // 
            // gvbTask
            // 
            this.gvbTask.AllowAddNewRow = false;
            this.gvbTask.AllowCheckSort = false;
            this.gvbTask.AllowDeleteRow = false;
            this.gvbTask.AllowEditRow = true;
            this.gvbTask.AllowSelectAll = true;
            this.gvbTask.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.gvbTask.ChangedValueForeColor = System.Drawing.Color.DeepPink;
            this.gvbTask.DataSource = null;
            this.gvbTask.DefaultValueForeColor = System.Drawing.Color.Black;
            this.gvbTask.EnableFiltering = true;
            this.gvbTask.EnablePaging = false;
            this.gvbTask.EnableSorting = true;
            this.gvbTask.Location = new System.Drawing.Point(575, 43);
            this.gvbTask.Name = "gvbTask";
            this.gvbTask.NewAddRowForeColor = System.Drawing.Color.Fuchsia;
            this.gvbTask.PageSize = 20;
            this.gvbTask.ReadOnly = false;
            this.gvbTask.ShowCheckBox = false;
            this.gvbTask.ShowRowHeaderColumn = false;
            this.gvbTask.ShowRowNumber = true;
            this.gvbTask.Size = new System.Drawing.Size(773, 653);
            this.gvbTask.TabIndex = 1;
            // 
            // gvbDevice
            // 
            this.gvbDevice.AllowAddNewRow = false;
            this.gvbDevice.AllowCheckSort = false;
            this.gvbDevice.AllowDeleteRow = false;
            this.gvbDevice.AllowEditRow = true;
            this.gvbDevice.AllowSelectAll = true;
            this.gvbDevice.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.gvbDevice.ChangedValueForeColor = System.Drawing.Color.DeepPink;
            this.gvbDevice.DataSource = null;
            this.gvbDevice.DefaultValueForeColor = System.Drawing.Color.Black;
            this.gvbDevice.EnableFiltering = true;
            this.gvbDevice.EnablePaging = false;
            this.gvbDevice.EnableSorting = true;
            this.gvbDevice.Location = new System.Drawing.Point(12, 43);
            this.gvbDevice.Name = "gvbDevice";
            this.gvbDevice.NewAddRowForeColor = System.Drawing.Color.Fuchsia;
            this.gvbDevice.PageSize = 20;
            this.gvbDevice.ReadOnly = false;
            this.gvbDevice.ShowCheckBox = false;
            this.gvbDevice.ShowRowHeaderColumn = false;
            this.gvbDevice.ShowRowNumber = true;
            this.gvbDevice.Size = new System.Drawing.Size(547, 653);
            this.gvbDevice.TabIndex = 1;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(1086, 13);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(61, 24);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "查询";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnSolutions
            // 
            this.btnSolutions.Enabled = false;
            this.btnSolutions.Location = new System.Drawing.Point(1231, 12);
            this.btnSolutions.Name = "btnSolutions";
            this.btnSolutions.Size = new System.Drawing.Size(77, 24);
            this.btnSolutions.TabIndex = 3;
            this.btnSolutions.Text = "查看方案";
            this.btnSolutions.Click += new System.EventHandler(this.btnSolutions_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(777, 14);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 21);
            this.textBox1.TabIndex = 4;
            // 
            // topCount
            // 
            this.topCount.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
            radListDataItem11.Text = "1";
            radListDataItem12.Text = "2";
            radListDataItem13.Text = "3";
            radListDataItem14.Text = "4";
            radListDataItem15.Text = "5";
            radListDataItem16.Text = "6";
            radListDataItem17.Text = "7";
            radListDataItem18.Text = "8";
            radListDataItem19.Text = "9";
            radListDataItem20.Text = "10";
            this.topCount.Items.Add(radListDataItem11);
            this.topCount.Items.Add(radListDataItem12);
            this.topCount.Items.Add(radListDataItem13);
            this.topCount.Items.Add(radListDataItem14);
            this.topCount.Items.Add(radListDataItem15);
            this.topCount.Items.Add(radListDataItem16);
            this.topCount.Items.Add(radListDataItem17);
            this.topCount.Items.Add(radListDataItem18);
            this.topCount.Items.Add(radListDataItem19);
            this.topCount.Items.Add(radListDataItem20);
            this.topCount.Location = new System.Drawing.Point(922, 14);
            this.topCount.Name = "topCount";
            this.topCount.Size = new System.Drawing.Size(125, 20);
            this.topCount.TabIndex = 5;
            this.topCount.Text = "5";
            // 
            // btnMdb
            // 
            this.btnMdb.Location = new System.Drawing.Point(592, 10);
            this.btnMdb.Name = "btnMdb";
            this.btnMdb.Size = new System.Drawing.Size(64, 24);
            this.btnMdb.TabIndex = 6;
            this.btnMdb.Text = "Mdb";
            this.btnMdb.Click += new System.EventHandler(this.btnMdb_Click);
            // 
            // ArithmeticsTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1360, 708);
            this.Controls.Add(this.btnMdb);
            this.Controls.Add(this.topCount);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnSolutions);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.gvbTask);
            this.Controls.Add(this.gvbDevice);
            this.Controls.Add(this.btnCreated);
            this.Name = "ArithmeticsTestForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "算法测试";
            ((System.ComponentModel.ISupportInitialize)(this.btnCreated)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSolutions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.topCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMdb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadButton btnCreated;
        private NJIS.FPZWS.UI.Common.Controls.GridViewBase gvbDevice;
        private NJIS.FPZWS.UI.Common.Controls.GridViewBase gvbTask;
        private Telerik.WinControls.UI.RadButton btnSearch;
        private Telerik.WinControls.UI.RadButton btnSolutions;
        private System.Windows.Forms.TextBox textBox1;
        private Telerik.WinControls.UI.RadDropDownList topCount;
        private Telerik.WinControls.UI.RadButton btnMdb;
    }
}
