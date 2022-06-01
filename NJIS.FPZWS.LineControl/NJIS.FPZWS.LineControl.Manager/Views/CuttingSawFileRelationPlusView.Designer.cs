namespace NJIS.FPZWS.LineControl.Manager.Views
{
    partial class CuttingSawFileRelationPlusView
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
            this.radDateTimePicker1 = new Telerik.WinControls.UI.RadDateTimePicker();
            this.radButtonSearch = new Telerik.WinControls.UI.RadButton();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.radTextBoxControlSAWFileName = new Telerik.WinControls.UI.RadTextBoxControl();
            this.radButton2Search = new Telerik.WinControls.UI.RadButton();
            this.radButtonSave = new Telerik.WinControls.UI.RadButton();
            this.gridViewBase1 = new NJIS.FPZWS.UI.Common.Controls.GridViewBase();
            this.radButtonPushAgain = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDateTimePicker1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxControlSAWFileName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton2Search)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonSave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonPushAgain)).BeginInit();
            this.SuspendLayout();
            // 
            // radLabel1
            // 
            this.radLabel1.Location = new System.Drawing.Point(3, 6);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(54, 18);
            this.radLabel1.TabIndex = 0;
            this.radLabel1.Text = "创建日期";
            // 
            // radDateTimePicker1
            // 
            this.radDateTimePicker1.Location = new System.Drawing.Point(63, 5);
            this.radDateTimePicker1.Name = "radDateTimePicker1";
            this.radDateTimePicker1.Size = new System.Drawing.Size(164, 20);
            this.radDateTimePicker1.TabIndex = 1;
            this.radDateTimePicker1.TabStop = false;
            this.radDateTimePicker1.Text = "2020年12月14日";
            this.radDateTimePicker1.Value = new System.DateTime(2020, 12, 14, 11, 17, 41, 779);
            // 
            // radButtonSearch
            // 
            this.radButtonSearch.Location = new System.Drawing.Point(233, 3);
            this.radButtonSearch.Name = "radButtonSearch";
            this.radButtonSearch.Size = new System.Drawing.Size(110, 24);
            this.radButtonSearch.TabIndex = 2;
            this.radButtonSearch.Text = "查询";
            this.radButtonSearch.Click += new System.EventHandler(this.radButtonSearch_Click);
            // 
            // radLabel2
            // 
            this.radLabel2.Location = new System.Drawing.Point(349, 6);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(65, 18);
            this.radLabel2.TabIndex = 4;
            this.radLabel2.Text = "SAW文件名";
            // 
            // radTextBoxControlSAWFileName
            // 
            this.radTextBoxControlSAWFileName.Location = new System.Drawing.Point(420, 5);
            this.radTextBoxControlSAWFileName.Name = "radTextBoxControlSAWFileName";
            this.radTextBoxControlSAWFileName.Size = new System.Drawing.Size(150, 20);
            this.radTextBoxControlSAWFileName.TabIndex = 5;
            // 
            // radButton2Search
            // 
            this.radButton2Search.Location = new System.Drawing.Point(576, 3);
            this.radButton2Search.Name = "radButton2Search";
            this.radButton2Search.Size = new System.Drawing.Size(110, 24);
            this.radButton2Search.TabIndex = 6;
            this.radButton2Search.Text = "查询";
            this.radButton2Search.Click += new System.EventHandler(this.radButton2Search_Click);
            // 
            // radButtonSave
            // 
            this.radButtonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.radButtonSave.Location = new System.Drawing.Point(847, 3);
            this.radButtonSave.Name = "radButtonSave";
            this.radButtonSave.Size = new System.Drawing.Size(110, 24);
            this.radButtonSave.TabIndex = 7;
            this.radButtonSave.Text = "保存";
            this.radButtonSave.Click += new System.EventHandler(this.radButtonSave_Click);
            // 
            // gridViewBase1
            // 
            this.gridViewBase1.AllowAddNewRow = false;
            this.gridViewBase1.AllowCheckSort = false;
            this.gridViewBase1.AllowDeleteRow = false;
            this.gridViewBase1.AllowEditRow = true;
            this.gridViewBase1.AllowSelectAll = true;
            this.gridViewBase1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridViewBase1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.gridViewBase1.ChangedValueForeColor = System.Drawing.Color.DeepPink;
            this.gridViewBase1.DataSource = null;
            this.gridViewBase1.DefaultValueForeColor = System.Drawing.Color.Black;
            this.gridViewBase1.EnableFiltering = true;
            this.gridViewBase1.EnablePaging = false;
            this.gridViewBase1.EnableSorting = true;
            this.gridViewBase1.Location = new System.Drawing.Point(3, 33);
            this.gridViewBase1.Name = "gridViewBase1";
            this.gridViewBase1.NewAddRowForeColor = System.Drawing.Color.Fuchsia;
            this.gridViewBase1.PageSize = 20;
            this.gridViewBase1.ReadOnly = false;
            this.gridViewBase1.ShowCheckBox = false;
            this.gridViewBase1.ShowRowHeaderColumn = false;
            this.gridViewBase1.ShowRowNumber = true;
            this.gridViewBase1.Size = new System.Drawing.Size(954, 599);
            this.gridViewBase1.TabIndex = 8;
            // 
            // radButtonPushAgain
            // 
            this.radButtonPushAgain.Location = new System.Drawing.Point(692, 3);
            this.radButtonPushAgain.Name = "radButtonPushAgain";
            this.radButtonPushAgain.Size = new System.Drawing.Size(110, 24);
            this.radButtonPushAgain.TabIndex = 9;
            this.radButtonPushAgain.Text = "补推";
            this.radButtonPushAgain.Click += new System.EventHandler(this.radButtonPushAgain_Click);
            // 
            // CuttingSawFileRelationPlusView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.Controls.Add(this.radButtonPushAgain);
            this.Controls.Add(this.gridViewBase1);
            this.Controls.Add(this.radButtonSave);
            this.Controls.Add(this.radButton2Search);
            this.Controls.Add(this.radTextBoxControlSAWFileName);
            this.Controls.Add(this.radLabel2);
            this.Controls.Add(this.radButtonSearch);
            this.Controls.Add(this.radDateTimePicker1);
            this.Controls.Add(this.radLabel1);
            this.Name = "CuttingSawFileRelationPlusView";
            this.Size = new System.Drawing.Size(960, 635);
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDateTimePicker1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxControlSAWFileName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton2Search)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonSave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonPushAgain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadDateTimePicker radDateTimePicker1;
        private Telerik.WinControls.UI.RadButton radButtonSearch;
        private Telerik.WinControls.UI.RadLabel radLabel2;
        private Telerik.WinControls.UI.RadTextBoxControl radTextBoxControlSAWFileName;
        private Telerik.WinControls.UI.RadButton radButton2Search;
        private Telerik.WinControls.UI.RadButton radButtonSave;
        private UI.Common.Controls.GridViewBase gridViewBase1;
        private Telerik.WinControls.UI.RadButton radButtonPushAgain;
    }
}
