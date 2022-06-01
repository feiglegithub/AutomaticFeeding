namespace NJIS.FPZWS.LineControl.Drilling.Client.Forms
{
    partial class PartInfoQueueForm
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
            Telerik.WinControls.UI.ConditionalFormattingObject conditionalFormattingObject1 = new Telerik.WinControls.UI.ConditionalFormattingObject();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn4 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.ConditionalFormattingObject conditionalFormattingObject2 = new Telerik.WinControls.UI.ConditionalFormattingObject();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn5 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn6 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn7 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn8 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn9 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn10 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn11 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn12 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.ConditionalFormattingObject conditionalFormattingObject3 = new Telerik.WinControls.UI.ConditionalFormattingObject();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn13 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.ConditionalFormattingObject conditionalFormattingObject4 = new Telerik.WinControls.UI.ConditionalFormattingObject();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn14 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PartInfoQueueForm));
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
            this.rgvMain.MasterTemplate.AllowAddNewRow = false;
            this.rgvMain.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            gridViewTextBoxColumn1.HeaderText = "时间";
            gridViewTextBoxColumn1.Name = "CreatedTime";
            gridViewTextBoxColumn1.Width = 69;
            gridViewTextBoxColumn2.HeaderText = "板件";
            gridViewTextBoxColumn2.Name = "PartId";
            gridViewTextBoxColumn2.Width = 131;
            conditionalFormattingObject1.ApplyToRow = true;
            conditionalFormattingObject1.CellBackColor = System.Drawing.Color.Empty;
            conditionalFormattingObject1.CellForeColor = System.Drawing.Color.Empty;
            conditionalFormattingObject1.Name = "NewCondition";
            conditionalFormattingObject1.RowBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            conditionalFormattingObject1.RowForeColor = System.Drawing.Color.Empty;
            conditionalFormattingObject1.TValue1 = "200";
            gridViewTextBoxColumn3.ConditionalFormattingObjectList.Add(conditionalFormattingObject1);
            gridViewTextBoxColumn3.HeaderText = "当前位置";
            gridViewTextBoxColumn3.Name = "Position";
            gridViewTextBoxColumn3.Width = 88;
            conditionalFormattingObject2.ApplyToRow = true;
            conditionalFormattingObject2.CellBackColor = System.Drawing.Color.Empty;
            conditionalFormattingObject2.CellForeColor = System.Drawing.Color.Empty;
            conditionalFormattingObject2.Name = "NewCondition";
            conditionalFormattingObject2.RowBackColor = System.Drawing.Color.Lime;
            conditionalFormattingObject2.RowForeColor = System.Drawing.Color.Empty;
            conditionalFormattingObject2.TValue1 = "1000";
            gridViewTextBoxColumn4.ConditionalFormattingObjectList.Add(conditionalFormattingObject2);
            gridViewTextBoxColumn4.FieldName = "NextPlace";
            gridViewTextBoxColumn4.HeaderText = "下一位置";
            gridViewTextBoxColumn4.Name = "NextPlace";
            gridViewTextBoxColumn4.Width = 89;
            gridViewTextBoxColumn5.HeaderText = "批次";
            gridViewTextBoxColumn5.Name = "BatchName";
            gridViewTextBoxColumn5.Width = 69;
            gridViewTextBoxColumn6.HeaderText = "订单";
            gridViewTextBoxColumn6.Name = "OrderNumber";
            gridViewTextBoxColumn6.Width = 69;
            gridViewTextBoxColumn7.HeaderText = "长";
            gridViewTextBoxColumn7.Name = "FinishLength";
            gridViewTextBoxColumn7.Width = 39;
            gridViewTextBoxColumn8.HeaderText = "宽";
            gridViewTextBoxColumn8.Name = "FinishWidth";
            gridViewTextBoxColumn8.Width = 47;
            gridViewTextBoxColumn9.HeaderText = "打孔类型";
            gridViewTextBoxColumn9.Name = "DrillingRouting";
            gridViewTextBoxColumn9.Width = 81;
            gridViewTextBoxColumn10.HeaderText = "PLC";
            gridViewTextBoxColumn10.Name = "TriggerIn";
            gridViewTextBoxColumn10.Width = 53;
            gridViewTextBoxColumn11.HeaderText = "PCS";
            gridViewTextBoxColumn11.Name = "TriggerOut";
            gridViewTextBoxColumn11.Width = 53;
            conditionalFormattingObject3.CellBackColor = System.Drawing.Color.Yellow;
            conditionalFormattingObject3.CellForeColor = System.Drawing.Color.Empty;
            conditionalFormattingObject3.Name = "NewCondition";
            conditionalFormattingObject3.RowBackColor = System.Drawing.Color.Empty;
            conditionalFormattingObject3.RowForeColor = System.Drawing.Color.Empty;
            conditionalFormattingObject3.TValue1 = "20";
            gridViewTextBoxColumn12.ConditionalFormattingObjectList.Add(conditionalFormattingObject3);
            gridViewTextBoxColumn12.HeaderText = "IsNg";
            gridViewTextBoxColumn12.Name = "IsNg";
            gridViewTextBoxColumn12.Width = 47;
            conditionalFormattingObject4.ApplyToRow = true;
            conditionalFormattingObject4.CellBackColor = System.Drawing.Color.Empty;
            conditionalFormattingObject4.CellForeColor = System.Drawing.Color.Empty;
            conditionalFormattingObject4.ConditionType = Telerik.WinControls.UI.ConditionTypes.GreaterOrEqual;
            conditionalFormattingObject4.Name = "NewCondition";
            conditionalFormattingObject4.RowBackColor = System.Drawing.Color.Red;
            conditionalFormattingObject4.RowForeColor = System.Drawing.Color.Empty;
            conditionalFormattingObject4.TValue1 = "1000";
            gridViewTextBoxColumn13.ConditionalFormattingObjectList.Add(conditionalFormattingObject4);
            gridViewTextBoxColumn13.HeaderText = "状态";
            gridViewTextBoxColumn13.Name = "Status";
            gridViewTextBoxColumn13.Width = 53;
            gridViewTextBoxColumn14.HeaderText = "消息";
            gridViewTextBoxColumn14.Name = "Msg";
            gridViewTextBoxColumn14.Width = 196;
            this.rgvMain.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2,
            gridViewTextBoxColumn3,
            gridViewTextBoxColumn4,
            gridViewTextBoxColumn5,
            gridViewTextBoxColumn6,
            gridViewTextBoxColumn7,
            gridViewTextBoxColumn8,
            gridViewTextBoxColumn9,
            gridViewTextBoxColumn10,
            gridViewTextBoxColumn11,
            gridViewTextBoxColumn12,
            gridViewTextBoxColumn13,
            gridViewTextBoxColumn14});
            this.rgvMain.MasterTemplate.EnableFiltering = true;
            this.rgvMain.MasterTemplate.MultiSelect = true;
            this.rgvMain.MasterTemplate.ShowFilteringRow = false;
            this.rgvMain.MasterTemplate.ShowHeaderCellButtons = true;
            this.rgvMain.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.rgvMain.Name = "rgvMain";
            this.rgvMain.ShowGroupPanel = false;
            this.rgvMain.ShowHeaderCellButtons = true;
            this.rgvMain.Size = new System.Drawing.Size(1092, 711);
            this.rgvMain.TabIndex = 6;
            // 
            // PartInfoQueueForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1092, 711);
            this.Controls.Add(this.rgvMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PartInfoQueueForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "生产队列";
            this.Load += new System.EventHandler(this.PartInfoQueueForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.rgvMain.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadGridView rgvMain;
    }
}