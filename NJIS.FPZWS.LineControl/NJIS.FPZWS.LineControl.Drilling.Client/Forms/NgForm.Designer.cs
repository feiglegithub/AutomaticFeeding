namespace NJIS.FPZWS.LineControl.Drilling.Client.Forms
{
    partial class NgForm
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
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NgForm));
            this.rgvMain = new Telerik.WinControls.UI.RadGridView();
            ((System.ComponentModel.ISupportInitialize)(this.rgvMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvMain.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // rgvMain
            // 
            this.rgvMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rgvMain.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rgvMain.Location = new System.Drawing.Point(0, 0);
            // 
            // 
            // 
            this.rgvMain.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            gridViewTextBoxColumn1.FieldName = "PartId";
            gridViewTextBoxColumn1.HeaderText = "板件";
            gridViewTextBoxColumn1.Name = "PartId";
            gridViewTextBoxColumn1.Width = 302;
            gridViewTextBoxColumn2.FieldName = "Status";
            gridViewTextBoxColumn2.HeaderText = "状态";
            gridViewTextBoxColumn2.Name = "Status";
            gridViewTextBoxColumn2.Width = 307;
            gridViewTextBoxColumn3.FieldName = "Msg";
            gridViewTextBoxColumn3.HeaderText = "消息";
            gridViewTextBoxColumn3.Name = "Msg";
            gridViewTextBoxColumn3.Width = 290;
            this.rgvMain.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2,
            gridViewTextBoxColumn3});
            this.rgvMain.MasterTemplate.EnableFiltering = true;
            this.rgvMain.MasterTemplate.MultiSelect = true;
            this.rgvMain.MasterTemplate.ShowFilteringRow = false;
            this.rgvMain.MasterTemplate.ShowHeaderCellButtons = true;
            this.rgvMain.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.rgvMain.Name = "rgvMain";
            this.rgvMain.ShowGroupPanel = false;
            this.rgvMain.ShowHeaderCellButtons = true;
            this.rgvMain.Size = new System.Drawing.Size(918, 478);
            this.rgvMain.TabIndex = 7;
            // 
            // NgForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(918, 478);
            this.Controls.Add(this.rgvMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "NgForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "NG";
            this.Load += new System.EventHandler(this.MsgForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.rgvMain.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadGridView rgvMain;
    }
}