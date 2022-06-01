namespace NJIS.FPZWS.LineControl.Drilling.Client.Forms
{
    partial class PartInfoTraceForm
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
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn1 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn2 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn3 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn4 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PartInfoTraceForm));
            this.radPanel1 = new Telerik.WinControls.UI.RadPanel();
            this.rtxtUpi = new Telerik.WinControls.UI.RadTextBox();
            this.rbtnSearch = new Telerik.WinControls.UI.RadButton();
            this.rgvMain = new Telerik.WinControls.UI.RadGridView();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).BeginInit();
            this.radPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rtxtUpi)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtnSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvMain.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radPanel1
            // 
            this.radPanel1.Controls.Add(this.rtxtUpi);
            this.radPanel1.Controls.Add(this.rbtnSearch);
            this.radPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.radPanel1.Location = new System.Drawing.Point(0, 0);
            this.radPanel1.Name = "radPanel1";
            this.radPanel1.Size = new System.Drawing.Size(1141, 52);
            this.radPanel1.TabIndex = 0;
            // 
            // rtxtUpi
            // 
            this.rtxtUpi.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtxtUpi.Location = new System.Drawing.Point(12, 12);
            this.rtxtUpi.Name = "rtxtUpi";
            this.rtxtUpi.NullText = "UPI";
            this.rtxtUpi.Size = new System.Drawing.Size(182, 27);
            this.rtxtUpi.TabIndex = 3;
            // 
            // rbtnSearch
            // 
            this.rbtnSearch.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtnSearch.Location = new System.Drawing.Point(200, 12);
            this.rbtnSearch.Name = "rbtnSearch";
            this.rbtnSearch.Size = new System.Drawing.Size(110, 27);
            this.rbtnSearch.TabIndex = 2;
            this.rbtnSearch.Text = "查询";
            this.rbtnSearch.Click += new System.EventHandler(this.rbtnSearch_Click);
            // 
            // rgvMain
            // 
            this.rgvMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rgvMain.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rgvMain.Location = new System.Drawing.Point(0, 52);
            // 
            // 
            // 
            this.rgvMain.MasterTemplate.AllowAddNewRow = false;
            this.rgvMain.MasterTemplate.AllowCellContextMenu = false;
            this.rgvMain.MasterTemplate.AllowColumnHeaderContextMenu = false;
            this.rgvMain.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            gridViewTextBoxColumn1.FieldName = "Time";
            gridViewTextBoxColumn1.HeaderText = "时间";
            gridViewTextBoxColumn1.Name = "Time";
            gridViewTextBoxColumn1.Width = 264;
            gridViewTextBoxColumn2.FieldName = "PartID";
            gridViewTextBoxColumn2.HeaderText = "板件";
            gridViewTextBoxColumn2.Name = "PartID";
            gridViewTextBoxColumn2.Width = 304;
            gridViewTextBoxColumn3.FieldName = "Position";
            gridViewTextBoxColumn3.HeaderText = "位置";
            gridViewTextBoxColumn3.Name = "Position";
            gridViewTextBoxColumn3.Width = 107;
            gridViewTextBoxColumn4.HeaderText = "消息";
            gridViewTextBoxColumn4.Name = "Msg";
            gridViewTextBoxColumn4.Width = 448;
            this.rgvMain.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2,
            gridViewTextBoxColumn3,
            gridViewTextBoxColumn4});
            this.rgvMain.MasterTemplate.EnableFiltering = true;
            this.rgvMain.MasterTemplate.MultiSelect = true;
            this.rgvMain.MasterTemplate.ShowFilteringRow = false;
            this.rgvMain.MasterTemplate.ShowHeaderCellButtons = true;
            this.rgvMain.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.rgvMain.Name = "rgvMain";
            this.rgvMain.ReadOnly = true;
            this.rgvMain.ShowGroupPanel = false;
            this.rgvMain.ShowHeaderCellButtons = true;
            this.rgvMain.Size = new System.Drawing.Size(1141, 621);
            this.rgvMain.TabIndex = 6;
            // 
            // PartInfoTraceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1141, 673);
            this.Controls.Add(this.rgvMain);
            this.Controls.Add(this.radPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PartInfoTraceForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "板件追踪";
            this.Load += new System.EventHandler(this.SearchForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).EndInit();
            this.radPanel1.ResumeLayout(false);
            this.radPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rtxtUpi)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtnSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvMain.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadPanel radPanel1;
        private Telerik.WinControls.UI.RadGridView rgvMain;
        private Telerik.WinControls.UI.RadButton rbtnSearch;
        private Telerik.WinControls.UI.RadTextBox rtxtUpi;
    }
}