namespace NJIS.FPZWS.LineControl.Drilling.Client.Forms
{
    partial class CommandForm
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
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn5 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CommandForm));
            this.rgvTaskOfBuffer = new Telerik.WinControls.UI.RadGridView();
            ((System.ComponentModel.ISupportInitialize)(this.rgvTaskOfBuffer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvTaskOfBuffer.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // rgvTaskOfBuffer
            // 
            this.rgvTaskOfBuffer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rgvTaskOfBuffer.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rgvTaskOfBuffer.Location = new System.Drawing.Point(0, 0);
            // 
            // 
            // 
            this.rgvTaskOfBuffer.MasterTemplate.AllowAddNewRow = false;
            this.rgvTaskOfBuffer.MasterTemplate.AllowCellContextMenu = false;
            this.rgvTaskOfBuffer.MasterTemplate.AllowColumnHeaderContextMenu = false;
            this.rgvTaskOfBuffer.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            gridViewTextBoxColumn1.HeaderText = "开始时间";
            gridViewTextBoxColumn1.Name = "column1";
            gridViewTextBoxColumn1.Width = 103;
            gridViewTextBoxColumn2.HeaderText = "结束时间";
            gridViewTextBoxColumn2.Name = "column3";
            gridViewTextBoxColumn2.Width = 103;
            gridViewTextBoxColumn3.FieldName = "CommandCode";
            gridViewTextBoxColumn3.HeaderText = "命令";
            gridViewTextBoxColumn3.Name = "CommandCode";
            gridViewTextBoxColumn3.Width = 166;
            gridViewTextBoxColumn4.FieldName = "Trigger";
            gridViewTextBoxColumn4.HeaderText = "请求";
            gridViewTextBoxColumn4.Name = "Trigger";
            gridViewTextBoxColumn4.Width = 284;
            gridViewTextBoxColumn5.HeaderText = "响应";
            gridViewTextBoxColumn5.Name = "column2";
            gridViewTextBoxColumn5.Width = 302;
            this.rgvTaskOfBuffer.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2,
            gridViewTextBoxColumn3,
            gridViewTextBoxColumn4,
            gridViewTextBoxColumn5});
            this.rgvTaskOfBuffer.MasterTemplate.EnableFiltering = true;
            this.rgvTaskOfBuffer.MasterTemplate.MultiSelect = true;
            this.rgvTaskOfBuffer.MasterTemplate.ShowFilteringRow = false;
            this.rgvTaskOfBuffer.MasterTemplate.ShowHeaderCellButtons = true;
            this.rgvTaskOfBuffer.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.rgvTaskOfBuffer.Name = "rgvTaskOfBuffer";
            this.rgvTaskOfBuffer.ShowGroupPanel = false;
            this.rgvTaskOfBuffer.ShowHeaderCellButtons = true;
            this.rgvTaskOfBuffer.Size = new System.Drawing.Size(975, 632);
            this.rgvTaskOfBuffer.TabIndex = 5;
            // 
            // CommandForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(975, 632);
            this.Controls.Add(this.rgvTaskOfBuffer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CommandForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "命令(PLC)";
            this.Load += new System.EventHandler(this.CommandForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.rgvTaskOfBuffer.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvTaskOfBuffer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadGridView rgvTaskOfBuffer;
    }
}