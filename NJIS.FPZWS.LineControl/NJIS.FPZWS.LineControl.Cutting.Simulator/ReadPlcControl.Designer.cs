namespace NJIS.FPZWS.LineControl.Cutting.Simulator
{
    partial class ReadPlcControl
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
            this.txtContent = new Telerik.WinControls.UI.RadTextBox();
            this.btnReadPartId = new Telerik.WinControls.UI.RadButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.cmbDataTpye = new System.Windows.Forms.ComboBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.txtDB = new Telerik.WinControls.UI.RadTextBox();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.txtOffset = new Telerik.WinControls.UI.RadTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.txtContent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnReadPartId)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOffset)).BeginInit();
            this.SuspendLayout();
            // 
            // txtContent
            // 
            this.txtContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtContent.Location = new System.Drawing.Point(1, 1);
            this.txtContent.Margin = new System.Windows.Forms.Padding(1);
            this.txtContent.Name = "txtContent";
            this.txtContent.Size = new System.Drawing.Size(116, 21);
            this.txtContent.TabIndex = 4;
            // 
            // btnReadPartId
            // 
            this.btnReadPartId.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnReadPartId.Location = new System.Drawing.Point(119, 24);
            this.btnReadPartId.Margin = new System.Windows.Forms.Padding(1);
            this.btnReadPartId.Name = "btnReadPartId";
            this.btnReadPartId.Size = new System.Drawing.Size(63, 21);
            this.btnReadPartId.TabIndex = 3;
            this.btnReadPartId.Text = "读";
            this.btnReadPartId.Click += new System.EventHandler(this.btnReadPartId_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.tableLayoutPanel1.Controls.Add(this.txtContent, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnReadPartId, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.cmbDataTpye, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(183, 46);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // cmbDataTpye
            // 
            this.cmbDataTpye.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbDataTpye.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDataTpye.FormattingEnabled = true;
            this.cmbDataTpye.Items.AddRange(new object[] {
            "string",
            "long"});
            this.cmbDataTpye.Location = new System.Drawing.Point(119, 1);
            this.cmbDataTpye.Margin = new System.Windows.Forms.Padding(1);
            this.cmbDataTpye.Name = "cmbDataTpye";
            this.cmbDataTpye.Size = new System.Drawing.Size(63, 20);
            this.cmbDataTpye.TabIndex = 5;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.radLabel1);
            this.flowLayoutPanel1.Controls.Add(this.txtDB);
            this.flowLayoutPanel1.Controls.Add(this.radLabel2);
            this.flowLayoutPanel1.Controls.Add(this.txtOffset);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 23);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(118, 23);
            this.flowLayoutPanel1.TabIndex = 6;
            // 
            // radLabel1
            // 
            this.radLabel1.Location = new System.Drawing.Point(1, 1);
            this.radLabel1.Margin = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(21, 18);
            this.radLabel1.TabIndex = 6;
            this.radLabel1.Text = "DB";
            // 
            // txtDB
            // 
            this.txtDB.Location = new System.Drawing.Point(22, 1);
            this.txtDB.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.txtDB.Name = "txtDB";
            this.txtDB.Size = new System.Drawing.Size(39, 20);
            this.txtDB.TabIndex = 5;
            this.txtDB.Text = "450";
            // 
            // radLabel2
            // 
            this.radLabel2.Location = new System.Drawing.Point(61, 1);
            this.radLabel2.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(9, 18);
            this.radLabel2.TabIndex = 6;
            this.radLabel2.Text = ".";
            // 
            // txtOffset
            // 
            this.txtOffset.Location = new System.Drawing.Point(70, 1);
            this.txtOffset.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.txtOffset.Name = "txtOffset";
            this.txtOffset.Size = new System.Drawing.Size(35, 20);
            this.txtOffset.TabIndex = 5;
            this.txtOffset.Text = "0";
            // 
            // ReadPlcControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ReadPlcControl";
            this.Size = new System.Drawing.Size(183, 46);
            ((System.ComponentModel.ISupportInitialize)(this.txtContent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnReadPartId)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOffset)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Telerik.WinControls.UI.RadTextBox txtContent;
        private Telerik.WinControls.UI.RadButton btnReadPartId;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ComboBox cmbDataTpye;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadTextBox txtDB;
        private Telerik.WinControls.UI.RadLabel radLabel2;
        private Telerik.WinControls.UI.RadTextBox txtOffset;
    }
}
