namespace NJIS.FPZWS.LineControl.Cutting.Server.Client.Views.Controls.ModuleControl
{
    partial class PushTaskControl
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
            this.dtpPlanDate = new Telerik.WinControls.UI.RadDateTimePicker();
            this.btnGetStackList = new Telerik.WinControls.UI.RadButton();
            this.btnPush = new Telerik.WinControls.UI.RadButton();
            this.gridViewBase1 = new NJIS.FPZWS.UI.Common.Controls.GridViewBase();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpPlanDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnGetStackList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnPush)).BeginInit();
            this.SuspendLayout();
            // 
            // radLabel1
            // 
            this.radLabel1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radLabel1.Location = new System.Drawing.Point(3, 3);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(90, 25);
            this.radLabel1.TabIndex = 23;
            this.radLabel1.Text = "计划日期：";
            // 
            // dtpPlanDate
            // 
            this.dtpPlanDate.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpPlanDate.Location = new System.Drawing.Point(99, 2);
            this.dtpPlanDate.Name = "dtpPlanDate";
            this.dtpPlanDate.Size = new System.Drawing.Size(164, 27);
            this.dtpPlanDate.TabIndex = 22;
            this.dtpPlanDate.TabStop = false;
            this.dtpPlanDate.Text = "2018年10月20日";
            this.dtpPlanDate.Value = new System.DateTime(2018, 10, 20, 0, 0, 0, 0);
            // 
            // btnGetStackList
            // 
            this.btnGetStackList.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnGetStackList.Location = new System.Drawing.Point(269, 1);
            this.btnGetStackList.Name = "btnGetStackList";
            this.btnGetStackList.Size = new System.Drawing.Size(88, 28);
            this.btnGetStackList.TabIndex = 20;
            this.btnGetStackList.Text = "查询堆垛";
            this.btnGetStackList.Click += new System.EventHandler(this.btnGetStackList_Click);
            // 
            // btnPush
            // 
            this.btnPush.Enabled = false;
            this.btnPush.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnPush.Location = new System.Drawing.Point(363, 1);
            this.btnPush.Name = "btnPush";
            this.btnPush.Size = new System.Drawing.Size(61, 28);
            this.btnPush.TabIndex = 20;
            this.btnPush.Text = "推送";
            this.btnPush.Click += new System.EventHandler(this.btnPush_Click);
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
            this.gridViewBase1.Location = new System.Drawing.Point(3, 34);
            this.gridViewBase1.Name = "gridViewBase1";
            this.gridViewBase1.NewAddRowForeColor = System.Drawing.Color.Fuchsia;
            this.gridViewBase1.PageSize = 20;
            this.gridViewBase1.ReadOnly = true;
            this.gridViewBase1.ShowCheckBox = false;
            this.gridViewBase1.ShowRowHeaderColumn = false;
            this.gridViewBase1.ShowRowNumber = true;
            this.gridViewBase1.Size = new System.Drawing.Size(669, 379);
            this.gridViewBase1.TabIndex = 21;
            // 
            // PushTaskControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.Controls.Add(this.radLabel1);
            this.Controls.Add(this.dtpPlanDate);
            this.Controls.Add(this.btnPush);
            this.Controls.Add(this.btnGetStackList);
            this.Controls.Add(this.gridViewBase1);
            this.Name = "PushTaskControl";
            this.Size = new System.Drawing.Size(675, 416);
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpPlanDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnGetStackList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnPush)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadDateTimePicker dtpPlanDate;
        private Telerik.WinControls.UI.RadButton btnGetStackList;
        private UI.Common.Controls.GridViewBase gridViewBase1;
        private Telerik.WinControls.UI.RadButton btnPush;
    }
}
