namespace WCS.Forms
{
    partial class TaskFrm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.plHead = new System.Windows.Forms.Panel();
            this.btnInWare = new System.Windows.Forms.Button();
            this.txtToPosition = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.btnFinishByHand = new System.Windows.Forms.Button();
            this.txtDdj = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtFromPostion = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtPilerNo = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cboStatus = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cboTaskType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dtpEnd1 = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpStart1 = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSelect = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dvTasks = new System.Windows.Forms.DataGridView();
            this.TaskId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReqId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PilerNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StackName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProductCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TaskType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Priority = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DdjNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FromPosition = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ToPosition = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HasUpProtect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.TaskStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StartTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DdjTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FinishTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ErrorMsg = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.plHead.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dvTasks)).BeginInit();
            this.SuspendLayout();
            // 
            // plHead
            // 
            this.plHead.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(70)))), ((int)(((byte)(95)))));
            this.plHead.Controls.Add(this.btnInWare);
            this.plHead.Controls.Add(this.txtToPosition);
            this.plHead.Controls.Add(this.label13);
            this.plHead.Controls.Add(this.btnFinishByHand);
            this.plHead.Controls.Add(this.txtDdj);
            this.plHead.Controls.Add(this.label11);
            this.plHead.Controls.Add(this.txtFromPostion);
            this.plHead.Controls.Add(this.label10);
            this.plHead.Controls.Add(this.txtPilerNo);
            this.plHead.Controls.Add(this.label9);
            this.plHead.Controls.Add(this.cboStatus);
            this.plHead.Controls.Add(this.label4);
            this.plHead.Controls.Add(this.cboTaskType);
            this.plHead.Controls.Add(this.label3);
            this.plHead.Controls.Add(this.dtpEnd1);
            this.plHead.Controls.Add(this.label2);
            this.plHead.Controls.Add(this.dtpStart1);
            this.plHead.Controls.Add(this.label1);
            this.plHead.Controls.Add(this.btnSelect);
            this.plHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.plHead.Location = new System.Drawing.Point(0, 0);
            this.plHead.Name = "plHead";
            this.plHead.Size = new System.Drawing.Size(1920, 50);
            this.plHead.TabIndex = 0;
            // 
            // btnInWare
            // 
            this.btnInWare.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInWare.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(70)))), ((int)(((byte)(95)))));
            this.btnInWare.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnInWare.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnInWare.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnInWare.Location = new System.Drawing.Point(1696, 10);
            this.btnInWare.Name = "btnInWare";
            this.btnInWare.Size = new System.Drawing.Size(90, 29);
            this.btnInWare.TabIndex = 25;
            this.btnInWare.Text = "板材入库";
            this.btnInWare.UseVisualStyleBackColor = false;
            this.btnInWare.Click += new System.EventHandler(this.btnInWare_Click);
            // 
            // txtToPosition
            // 
            this.txtToPosition.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.txtToPosition.Location = new System.Drawing.Point(1250, 13);
            this.txtToPosition.Name = "txtToPosition";
            this.txtToPosition.Size = new System.Drawing.Size(69, 25);
            this.txtToPosition.TabIndex = 24;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label13.Location = new System.Drawing.Point(1186, 14);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(58, 21);
            this.label13.TabIndex = 23;
            this.label13.Text = "目标位";
            // 
            // btnFinishByHand
            // 
            this.btnFinishByHand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFinishByHand.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(70)))), ((int)(((byte)(95)))));
            this.btnFinishByHand.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnFinishByHand.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnFinishByHand.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnFinishByHand.Location = new System.Drawing.Point(1585, 11);
            this.btnFinishByHand.Name = "btnFinishByHand";
            this.btnFinishByHand.Size = new System.Drawing.Size(90, 29);
            this.btnFinishByHand.TabIndex = 7;
            this.btnFinishByHand.Text = "过账任务";
            this.btnFinishByHand.UseVisualStyleBackColor = false;
            this.btnFinishByHand.Click += new System.EventHandler(this.btnFinishByHand_Click);
            // 
            // txtDdj
            // 
            this.txtDdj.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.txtDdj.Location = new System.Drawing.Point(1379, 13);
            this.txtDdj.Name = "txtDdj";
            this.txtDdj.Size = new System.Drawing.Size(69, 25);
            this.txtDdj.TabIndex = 20;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label11.Location = new System.Drawing.Point(1331, 14);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(42, 21);
            this.label11.TabIndex = 19;
            this.label11.Text = "巷道";
            // 
            // txtFromPostion
            // 
            this.txtFromPostion.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.txtFromPostion.Location = new System.Drawing.Point(1106, 13);
            this.txtFromPostion.Name = "txtFromPostion";
            this.txtFromPostion.Size = new System.Drawing.Size(69, 25);
            this.txtFromPostion.TabIndex = 18;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label10.Location = new System.Drawing.Point(1042, 14);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(58, 21);
            this.label10.TabIndex = 17;
            this.label10.Text = "起始位";
            // 
            // txtPilerNo
            // 
            this.txtPilerNo.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.txtPilerNo.Location = new System.Drawing.Point(932, 13);
            this.txtPilerNo.Name = "txtPilerNo";
            this.txtPilerNo.Size = new System.Drawing.Size(100, 25);
            this.txtPilerNo.TabIndex = 16;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label9.Location = new System.Drawing.Point(884, 14);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(42, 21);
            this.label9.TabIndex = 15;
            this.label9.Text = "垛号";
            // 
            // cboStatus
            // 
            this.cboStatus.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.cboStatus.FormattingEnabled = true;
            this.cboStatus.Items.AddRange(new object[] {
            "",
            "已下发",
            "执行中",
            "WCS已完成",
            "WMS已完成",
            "已取消"});
            this.cboStatus.Location = new System.Drawing.Point(765, 13);
            this.cboStatus.Name = "cboStatus";
            this.cboStatus.Size = new System.Drawing.Size(113, 25);
            this.cboStatus.TabIndex = 14;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label4.Location = new System.Drawing.Point(685, 14);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 21);
            this.label4.TabIndex = 13;
            this.label4.Text = "任务状态";
            // 
            // cboTaskType
            // 
            this.cboTaskType.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.cboTaskType.FormattingEnabled = true;
            this.cboTaskType.Items.AddRange(new object[] {
            "",
            "入库",
            "出库",
            "上料",
            "拣选",
            "垛转移"});
            this.cboTaskType.Location = new System.Drawing.Point(566, 13);
            this.cboTaskType.Name = "cboTaskType";
            this.cboTaskType.Size = new System.Drawing.Size(113, 25);
            this.cboTaskType.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label3.Location = new System.Drawing.Point(486, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 21);
            this.label3.TabIndex = 11;
            this.label3.Text = "任务类型";
            // 
            // dtpEnd1
            // 
            this.dtpEnd1.CalendarFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpEnd1.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.dtpEnd1.Location = new System.Drawing.Point(329, 13);
            this.dtpEnd1.Name = "dtpEnd1";
            this.dtpEnd1.Size = new System.Drawing.Size(151, 25);
            this.dtpEnd1.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label2.Location = new System.Drawing.Point(249, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 21);
            this.label2.TabIndex = 9;
            this.label2.Text = "结束时间";
            // 
            // dtpStart1
            // 
            this.dtpStart1.CalendarFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpStart1.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.dtpStart1.Location = new System.Drawing.Point(92, 13);
            this.dtpStart1.Name = "dtpStart1";
            this.dtpStart1.Size = new System.Drawing.Size(151, 25);
            this.dtpStart1.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 21);
            this.label1.TabIndex = 7;
            this.label1.Text = "开始时间";
            // 
            // btnSelect
            // 
            this.btnSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(70)))), ((int)(((byte)(95)))));
            this.btnSelect.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSelect.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSelect.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnSelect.Location = new System.Drawing.Point(1474, 11);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(90, 29);
            this.btnSelect.TabIndex = 6;
            this.btnSelect.Text = "查 询";
            this.btnSelect.UseVisualStyleBackColor = false;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dvTasks);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 50);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1920, 950);
            this.panel1.TabIndex = 1;
            // 
            // dvTasks
            // 
            this.dvTasks.AllowUserToAddRows = false;
            this.dvTasks.AllowUserToDeleteRows = false;
            this.dvTasks.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dvTasks.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(70)))), ((int)(((byte)(95)))));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(90)))), ((int)(((byte)(115)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微软雅黑", 11F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dvTasks.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dvTasks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dvTasks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TaskId,
            this.ReqId,
            this.PilerNo,
            this.StackName,
            this.ProductCode,
            this.Amount,
            this.TaskType,
            this.Priority,
            this.DdjNo,
            this.FromPosition,
            this.ToPosition,
            this.HasUpProtect,
            this.TaskStatus,
            this.CreateTime,
            this.StartTime,
            this.DdjTime,
            this.FinishTime,
            this.ErrorMsg});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(70)))), ((int)(((byte)(95)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dvTasks.DefaultCellStyle = dataGridViewCellStyle3;
            this.dvTasks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dvTasks.EnableHeadersVisualStyles = false;
            this.dvTasks.Location = new System.Drawing.Point(0, 0);
            this.dvTasks.Name = "dvTasks";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(70)))), ((int)(((byte)(95)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dvTasks.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dvTasks.RowTemplate.Height = 23;
            this.dvTasks.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dvTasks.Size = new System.Drawing.Size(1920, 950);
            this.dvTasks.TabIndex = 0;
            // 
            // TaskId
            // 
            this.TaskId.DataPropertyName = "TaskId";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.TaskId.DefaultCellStyle = dataGridViewCellStyle2;
            this.TaskId.FillWeight = 70F;
            this.TaskId.HeaderText = "任务ID";
            this.TaskId.Name = "TaskId";
            this.TaskId.ReadOnly = true;
            // 
            // ReqId
            // 
            this.ReqId.DataPropertyName = "ReqId";
            this.ReqId.FillWeight = 70F;
            this.ReqId.HeaderText = "请求ID";
            this.ReqId.Name = "ReqId";
            this.ReqId.ReadOnly = true;
            // 
            // PilerNo
            // 
            this.PilerNo.DataPropertyName = "PilerNo";
            this.PilerNo.FillWeight = 70F;
            this.PilerNo.HeaderText = "垛号";
            this.PilerNo.Name = "PilerNo";
            this.PilerNo.ReadOnly = true;
            // 
            // StackName
            // 
            this.StackName.DataPropertyName = "StackName";
            this.StackName.HeaderText = "垛号";
            this.StackName.Name = "StackName";
            this.StackName.ReadOnly = true;
            // 
            // ProductCode
            // 
            this.ProductCode.DataPropertyName = "ProductCode";
            this.ProductCode.HeaderText = "花色";
            this.ProductCode.Name = "ProductCode";
            this.ProductCode.ReadOnly = true;
            // 
            // Amount
            // 
            this.Amount.DataPropertyName = "Amount";
            this.Amount.FillWeight = 66F;
            this.Amount.HeaderText = "数量";
            this.Amount.Name = "Amount";
            this.Amount.ReadOnly = true;
            // 
            // TaskType
            // 
            this.TaskType.DataPropertyName = "TaskTypeCn";
            this.TaskType.FillWeight = 90F;
            this.TaskType.HeaderText = "任务类型";
            this.TaskType.Name = "TaskType";
            this.TaskType.ReadOnly = true;
            // 
            // Priority
            // 
            this.Priority.DataPropertyName = "Priority";
            this.Priority.FillWeight = 80F;
            this.Priority.HeaderText = "优先级";
            this.Priority.Name = "Priority";
            this.Priority.ReadOnly = true;
            // 
            // DdjNo
            // 
            this.DdjNo.DataPropertyName = "DdjNo";
            this.DdjNo.FillWeight = 60F;
            this.DdjNo.HeaderText = "巷道";
            this.DdjNo.Name = "DdjNo";
            this.DdjNo.ReadOnly = true;
            // 
            // FromPosition
            // 
            this.FromPosition.DataPropertyName = "FromPosition";
            this.FromPosition.HeaderText = "起始位";
            this.FromPosition.Name = "FromPosition";
            this.FromPosition.ReadOnly = true;
            // 
            // ToPosition
            // 
            this.ToPosition.DataPropertyName = "ToPosition";
            this.ToPosition.HeaderText = "目标位";
            this.ToPosition.Name = "ToPosition";
            this.ToPosition.ReadOnly = true;
            // 
            // HasUpProtect
            // 
            this.HasUpProtect.DataPropertyName = "HasUpProtect";
            this.HasUpProtect.HeaderText = "是否有保护板";
            this.HasUpProtect.Name = "HasUpProtect";
            this.HasUpProtect.ReadOnly = true;
            // 
            // TaskStatus
            // 
            this.TaskStatus.DataPropertyName = "TaskStatus";
            this.TaskStatus.HeaderText = "任务状态";
            this.TaskStatus.Name = "TaskStatus";
            this.TaskStatus.ReadOnly = true;
            // 
            // CreateTime
            // 
            this.CreateTime.DataPropertyName = "CreateTime";
            this.CreateTime.HeaderText = "创建时间";
            this.CreateTime.Name = "CreateTime";
            this.CreateTime.ReadOnly = true;
            // 
            // StartTime
            // 
            this.StartTime.DataPropertyName = "StartTime";
            this.StartTime.HeaderText = "开始时间";
            this.StartTime.Name = "StartTime";
            this.StartTime.ReadOnly = true;
            // 
            // DdjTime
            // 
            this.DdjTime.DataPropertyName = "DdjTime";
            this.DdjTime.HeaderText = "堆垛机时间";
            this.DdjTime.Name = "DdjTime";
            this.DdjTime.ReadOnly = true;
            // 
            // FinishTime
            // 
            this.FinishTime.DataPropertyName = "FinishTime";
            this.FinishTime.HeaderText = "完成时间";
            this.FinishTime.Name = "FinishTime";
            this.FinishTime.ReadOnly = true;
            // 
            // ErrorMsg
            // 
            this.ErrorMsg.DataPropertyName = "ErrorMsg";
            this.ErrorMsg.HeaderText = "异常信息";
            this.ErrorMsg.Name = "ErrorMsg";
            this.ErrorMsg.ReadOnly = true;
            // 
            // TaskFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1920, 1000);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.plHead);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "TaskFrm";
            this.Text = "TaskFrm";
            this.plHead.ResumeLayout(false);
            this.plHead.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dvTasks)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel plHead;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.DataGridView dvTasks;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpStart1;
        private System.Windows.Forms.DateTimePicker dtpEnd1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboTaskType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboStatus;
        private System.Windows.Forms.TextBox txtPilerNo;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtFromPostion;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtDdj;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnFinishByHand;
        private System.Windows.Forms.TextBox txtToPosition;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btnInWare;
        private System.Windows.Forms.DataGridViewTextBoxColumn TaskId;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReqId;
        private System.Windows.Forms.DataGridViewTextBoxColumn PilerNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn StackName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn Amount;
        private System.Windows.Forms.DataGridViewTextBoxColumn TaskType;
        private System.Windows.Forms.DataGridViewTextBoxColumn Priority;
        private System.Windows.Forms.DataGridViewTextBoxColumn DdjNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn FromPosition;
        private System.Windows.Forms.DataGridViewTextBoxColumn ToPosition;
        private System.Windows.Forms.DataGridViewCheckBoxColumn HasUpProtect;
        private System.Windows.Forms.DataGridViewTextBoxColumn TaskStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreateTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn StartTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn DdjTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn FinishTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn ErrorMsg;
    }
}