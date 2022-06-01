namespace NJIS.FPZWS.LineControl.Drilling.Client.Forms
{
    partial class AlarmForm
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
            this.components = new System.ComponentModel.Container();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn1 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn2 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn3 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn4 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn5 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn6 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AlarmForm));
            this.radPanel1 = new Telerik.WinControls.UI.RadPanel();
            this.rbtnSearch = new Telerik.WinControls.UI.RadButton();
            this.rdtpETime = new Telerik.WinControls.UI.RadDateTimePicker();
            this.rdtpSTime = new Telerik.WinControls.UI.RadDateTimePicker();
            this.rgvMain = new Telerik.WinControls.UI.RadGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiDelete = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).BeginInit();
            this.radPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rbtnSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rdtpETime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rdtpSTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvMain.MasterTemplate)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radPanel1
            // 
            this.radPanel1.Controls.Add(this.rbtnSearch);
            this.radPanel1.Controls.Add(this.rdtpETime);
            this.radPanel1.Controls.Add(this.rdtpSTime);
            this.radPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.radPanel1.Location = new System.Drawing.Point(0, 0);
            this.radPanel1.Name = "radPanel1";
            this.radPanel1.Size = new System.Drawing.Size(976, 55);
            this.radPanel1.TabIndex = 3;
            // 
            // rbtnSearch
            // 
            this.rbtnSearch.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtnSearch.Location = new System.Drawing.Point(352, 12);
            this.rbtnSearch.Name = "rbtnSearch";
            this.rbtnSearch.Size = new System.Drawing.Size(110, 24);
            this.rbtnSearch.TabIndex = 2;
            this.rbtnSearch.Text = "查询";
            // 
            // rdtpETime
            // 
            this.rdtpETime.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdtpETime.Location = new System.Drawing.Point(182, 12);
            this.rdtpETime.Name = "rdtpETime";
            this.rdtpETime.Size = new System.Drawing.Size(164, 27);
            this.rdtpETime.TabIndex = 1;
            this.rdtpETime.TabStop = false;
            this.rdtpETime.Text = "2018年11月28日";
            this.rdtpETime.Value = new System.DateTime(2018, 11, 28, 15, 21, 58, 188);
            // 
            // rdtpSTime
            // 
            this.rdtpSTime.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdtpSTime.Location = new System.Drawing.Point(12, 12);
            this.rdtpSTime.Name = "rdtpSTime";
            this.rdtpSTime.Size = new System.Drawing.Size(164, 27);
            this.rdtpSTime.TabIndex = 0;
            this.rdtpSTime.TabStop = false;
            this.rdtpSTime.Text = "2018年11月28日";
            this.rdtpSTime.Value = new System.DateTime(2018, 11, 28, 15, 21, 54, 770);
            // 
            // rgvMain
            // 
            this.rgvMain.ContextMenuStrip = this.contextMenuStrip1;
            this.rgvMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rgvMain.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rgvMain.Location = new System.Drawing.Point(0, 55);
            // 
            // 
            // 
            this.rgvMain.MasterTemplate.AllowAddNewRow = false;
            this.rgvMain.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            gridViewTextBoxColumn1.FieldName = "StarTime";
            gridViewTextBoxColumn1.HeaderText = "开始时间";
            gridViewTextBoxColumn1.MaxWidth = 150;
            gridViewTextBoxColumn1.Name = "StarTime";
            gridViewTextBoxColumn1.Width = 150;
            gridViewTextBoxColumn2.FieldName = "EndTime";
            gridViewTextBoxColumn2.HeaderText = "结束时间";
            gridViewTextBoxColumn2.MaxWidth = 150;
            gridViewTextBoxColumn2.Name = "EndTime";
            gridViewTextBoxColumn2.Width = 150;
            gridViewTextBoxColumn3.FieldName = "AlarmId";
            gridViewTextBoxColumn3.HeaderText = "标识";
            gridViewTextBoxColumn3.IsVisible = false;
            gridViewTextBoxColumn3.Name = "AlarmId";
            gridViewTextBoxColumn3.Width = 134;
            gridViewTextBoxColumn4.HeaderText = "类别";
            gridViewTextBoxColumn4.MaxWidth = 150;
            gridViewTextBoxColumn4.Name = "Category";
            gridViewTextBoxColumn4.Width = 150;
            gridViewTextBoxColumn5.HeaderText = "参数";
            gridViewTextBoxColumn5.MinWidth = 150;
            gridViewTextBoxColumn5.Name = "ParamName";
            gridViewTextBoxColumn5.Width = 150;
            gridViewTextBoxColumn6.HeaderText = "描述";
            gridViewTextBoxColumn6.Name = "Value";
            gridViewTextBoxColumn6.Width = 359;
            this.rgvMain.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2,
            gridViewTextBoxColumn3,
            gridViewTextBoxColumn4,
            gridViewTextBoxColumn5,
            gridViewTextBoxColumn6});
            this.rgvMain.MasterTemplate.EnableFiltering = true;
            this.rgvMain.MasterTemplate.EnableGrouping = false;
            this.rgvMain.MasterTemplate.ShowFilteringRow = false;
            this.rgvMain.MasterTemplate.ShowHeaderCellButtons = true;
            this.rgvMain.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.rgvMain.Name = "rgvMain";
            this.rgvMain.ShowHeaderCellButtons = true;
            this.rgvMain.Size = new System.Drawing.Size(976, 593);
            this.rgvMain.TabIndex = 4;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiDelete});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(181, 48);
            // 
            // tsmiDelete
            // 
            this.tsmiDelete.Name = "tsmiDelete";
            this.tsmiDelete.Size = new System.Drawing.Size(180, 22);
            this.tsmiDelete.Text = "清除";
            this.tsmiDelete.Click += new System.EventHandler(this.tsmiDelete_Click);
            // 
            // AlarmForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(976, 648);
            this.Controls.Add(this.rgvMain);
            this.Controls.Add(this.radPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AlarmForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "报警";
            this.Load += new System.EventHandler(this.AlarmForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).EndInit();
            this.radPanel1.ResumeLayout(false);
            this.radPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rbtnSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rdtpETime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rdtpSTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvMain.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvMain)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Telerik.WinControls.UI.RadPanel radPanel1;
        private Telerik.WinControls.UI.RadButton rbtnSearch;
        private Telerik.WinControls.UI.RadDateTimePicker rdtpETime;
        private Telerik.WinControls.UI.RadDateTimePicker rdtpSTime;
        private Telerik.WinControls.UI.RadGridView rgvMain;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsmiDelete;
    }
}