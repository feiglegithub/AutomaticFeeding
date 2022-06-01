namespace WCSToWMSDemo
{
    partial class cboType
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbStatus = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button7 = new System.Windows.Forms.Button();
            this.txtEmptyPort = new System.Windows.Forms.TextBox();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.btnSelect = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.dt2 = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dt1 = new System.Windows.Forms.DateTimePicker();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dvTask = new System.Windows.Forms.DataGridView();
            this.SeqID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NordID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NwkStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NPalletID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CPosidFrom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CPosidTo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Roadway = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NoptStation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NlotID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NpackOrder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Height = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustomerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DoptDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DStartDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DFinishDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dvTask)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbType);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.cbStatus);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.button7);
            this.panel1.Controls.Add(this.txtEmptyPort);
            this.panel1.Controls.Add(this.button6);
            this.panel1.Controls.Add(this.button5);
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.btnSelect);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.dt2);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.dt1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1521, 47);
            this.panel1.TabIndex = 0;
            // 
            // cbType
            // 
            this.cbType.FormattingEnabled = true;
            this.cbType.Items.AddRange(new object[] {
            "",
            "整盘入库",
            "整盘出库",
            "空盘入库",
            "空盘出库"});
            this.cbType.Location = new System.Drawing.Point(389, 15);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(84, 20);
            this.cbType.TabIndex = 16;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(479, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 15;
            this.label4.Text = "任务状态";
            // 
            // cbStatus
            // 
            this.cbStatus.FormattingEnabled = true;
            this.cbStatus.Items.AddRange(new object[] {
            "",
            "已分配",
            "执行中",
            "已过汇流口",
            "已完成",
            "已取消"});
            this.cbStatus.Location = new System.Drawing.Point(538, 15);
            this.cbStatus.Name = "cbStatus";
            this.cbStatus.Size = new System.Drawing.Size(84, 20);
            this.cbStatus.TabIndex = 14;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(330, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 13;
            this.label3.Text = "任务类型";
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(1063, 15);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(86, 23);
            this.button7.TabIndex = 12;
            this.button7.Text = "堆垛机控制";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // txtEmptyPort
            // 
            this.txtEmptyPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEmptyPort.Location = new System.Drawing.Point(1421, 16);
            this.txtEmptyPort.Name = "txtEmptyPort";
            this.txtEmptyPort.Size = new System.Drawing.Size(89, 21);
            this.txtEmptyPort.TabIndex = 11;
            // 
            // button6
            // 
            this.button6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button6.Location = new System.Drawing.Point(1321, 15);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(94, 23);
            this.button6.TabIndex = 10;
            this.button6.Text = "空盘下架申请";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(800, 15);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(85, 23);
            this.button5.TabIndex = 9;
            this.button5.Text = "更新汇流口";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(982, 15);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 8;
            this.button4.Text = "上架申请";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(628, 14);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(75, 23);
            this.btnSelect.TabIndex = 7;
            this.btnSelect.Text = "查询";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(891, 15);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(85, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "更新已完成";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(709, 14);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(85, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "更新执行中";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dt2
            // 
            this.dt2.Location = new System.Drawing.Point(200, 13);
            this.dt2.Name = "dt2";
            this.dt2.Size = new System.Drawing.Size(124, 21);
            this.dt2.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(177, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "至";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "时间";
            // 
            // dt1
            // 
            this.dt1.Location = new System.Drawing.Point(47, 13);
            this.dt1.Name = "dt1";
            this.dt1.Size = new System.Drawing.Size(124, 21);
            this.dt1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dvTask);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 47);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1521, 646);
            this.panel2.TabIndex = 1;
            // 
            // dvTask
            // 
            this.dvTask.AllowUserToAddRows = false;
            this.dvTask.AllowUserToDeleteRows = false;
            this.dvTask.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dvTask.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SeqID,
            this.NordID,
            this.NwkStatus,
            this.NPalletID,
            this.CPosidFrom,
            this.CPosidTo,
            this.Roadway,
            this.NoptStation,
            this.NlotID,
            this.NpackOrder,
            this.Height,
            this.CustomerName,
            this.DoptDate,
            this.DStartDate,
            this.DFinishDate});
            this.dvTask.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dvTask.Location = new System.Drawing.Point(0, 0);
            this.dvTask.Name = "dvTask";
            this.dvTask.RowTemplate.Height = 23;
            this.dvTask.Size = new System.Drawing.Size(1521, 646);
            this.dvTask.TabIndex = 0;
            // 
            // SeqID
            // 
            this.SeqID.DataPropertyName = "SeqID";
            this.SeqID.HeaderText = "任务ID";
            this.SeqID.Name = "SeqID";
            this.SeqID.ReadOnly = true;
            // 
            // NordID
            // 
            this.NordID.DataPropertyName = "NordIDCN";
            this.NordID.HeaderText = "任务类型";
            this.NordID.Name = "NordID";
            this.NordID.ReadOnly = true;
            // 
            // NwkStatus
            // 
            this.NwkStatus.DataPropertyName = "NwkStatusCN";
            this.NwkStatus.HeaderText = "WMS状态";
            this.NwkStatus.Name = "NwkStatus";
            this.NwkStatus.ReadOnly = true;
            // 
            // NPalletID
            // 
            this.NPalletID.DataPropertyName = "NPalletID";
            this.NPalletID.HeaderText = "托盘编号";
            this.NPalletID.Name = "NPalletID";
            this.NPalletID.ReadOnly = true;
            // 
            // CPosidFrom
            // 
            this.CPosidFrom.DataPropertyName = "CPosidFrom";
            this.CPosidFrom.HeaderText = "起始位";
            this.CPosidFrom.Name = "CPosidFrom";
            this.CPosidFrom.ReadOnly = true;
            // 
            // CPosidTo
            // 
            this.CPosidTo.DataPropertyName = "CPosidTo";
            this.CPosidTo.HeaderText = "目标位";
            this.CPosidTo.Name = "CPosidTo";
            this.CPosidTo.ReadOnly = true;
            // 
            // Roadway
            // 
            this.Roadway.DataPropertyName = "Roadway";
            this.Roadway.HeaderText = "巷道";
            this.Roadway.Name = "Roadway";
            this.Roadway.ReadOnly = true;
            // 
            // NoptStation
            // 
            this.NoptStation.DataPropertyName = "NoptStation";
            this.NoptStation.HeaderText = "站台号";
            this.NoptStation.Name = "NoptStation";
            this.NoptStation.ReadOnly = true;
            // 
            // NlotID
            // 
            this.NlotID.DataPropertyName = "NlotID";
            this.NlotID.HeaderText = "波次号";
            this.NlotID.Name = "NlotID";
            this.NlotID.ReadOnly = true;
            // 
            // NpackOrder
            // 
            this.NpackOrder.DataPropertyName = "NpackOrder";
            this.NpackOrder.HeaderText = "装车顺序";
            this.NpackOrder.Name = "NpackOrder";
            this.NpackOrder.ReadOnly = true;
            // 
            // Height
            // 
            this.Height.DataPropertyName = "Height";
            this.Height.HeaderText = "托盘高度";
            this.Height.Name = "Height";
            this.Height.ReadOnly = true;
            // 
            // CustomerName
            // 
            this.CustomerName.DataPropertyName = "CustomerName";
            this.CustomerName.HeaderText = "区域";
            this.CustomerName.Name = "CustomerName";
            this.CustomerName.ReadOnly = true;
            // 
            // DoptDate
            // 
            this.DoptDate.DataPropertyName = "DoptDate";
            this.DoptDate.HeaderText = "下发时间";
            this.DoptDate.Name = "DoptDate";
            this.DoptDate.ReadOnly = true;
            // 
            // DStartDate
            // 
            this.DStartDate.DataPropertyName = "DStartDate";
            this.DStartDate.HeaderText = "开始时间";
            this.DStartDate.Name = "DStartDate";
            this.DStartDate.ReadOnly = true;
            // 
            // DFinishDate
            // 
            this.DFinishDate.DataPropertyName = "DFinishDate";
            this.DFinishDate.HeaderText = "结束时间";
            this.DFinishDate.Name = "DFinishDate";
            this.DFinishDate.ReadOnly = true;
            // 
            // cboType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1521, 693);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "cboType";
            this.Text = "WCS与WMS接口测试";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dvTask)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dvTask;
        private System.Windows.Forms.DateTimePicker dt2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dt1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.TextBox txtEmptyPort;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn SeqID;
        private System.Windows.Forms.DataGridViewTextBoxColumn NordID;
        private System.Windows.Forms.DataGridViewTextBoxColumn NwkStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn NPalletID;
        private System.Windows.Forms.DataGridViewTextBoxColumn CPosidFrom;
        private System.Windows.Forms.DataGridViewTextBoxColumn CPosidTo;
        private System.Windows.Forms.DataGridViewTextBoxColumn Roadway;
        private System.Windows.Forms.DataGridViewTextBoxColumn NoptStation;
        private System.Windows.Forms.DataGridViewTextBoxColumn NlotID;
        private System.Windows.Forms.DataGridViewTextBoxColumn NpackOrder;
        private System.Windows.Forms.DataGridViewTextBoxColumn Height;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn DoptDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn DStartDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn DFinishDate;
    }
}

