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
            this.button1 = new System.Windows.Forms.Button();
            this.txtCell = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnSCControl = new System.Windows.Forms.Button();
            this.txtNoptStation = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnReback = new System.Windows.Forms.Button();
            this.btnFinishByHand = new System.Windows.Forms.Button();
            this.txtDdj = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtPallet = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtTaskId = new System.Windows.Forms.TextBox();
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbPages = new System.Windows.Forms.Label();
            this.lbPageIndex = new System.Windows.Forms.Label();
            this.lkbNext = new System.Windows.Forms.LinkLabel();
            this.lkbPrev = new System.Windows.Forms.LinkLabel();
            this.lbRowsCount = new System.Windows.Forms.Label();
            this.dvTasks = new System.Windows.Forms.DataGridView();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.SeqID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NPalletID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NordCN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NwkStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NoptStation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Roadway = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CPosidFrom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CPosidTo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NlotID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NpackOrder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustomerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DoptDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DStartDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Date_ext1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Date_ext2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Date_ext3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DFinishDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Msg = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.plHead.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dvTasks)).BeginInit();
            this.SuspendLayout();
            // 
            // plHead
            // 
            this.plHead.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(70)))), ((int)(((byte)(95)))));
            this.plHead.Controls.Add(this.button1);
            this.plHead.Controls.Add(this.txtCell);
            this.plHead.Controls.Add(this.label6);
            this.plHead.Controls.Add(this.btnExport);
            this.plHead.Controls.Add(this.btnSCControl);
            this.plHead.Controls.Add(this.txtNoptStation);
            this.plHead.Controls.Add(this.label5);
            this.plHead.Controls.Add(this.btnReback);
            this.plHead.Controls.Add(this.btnFinishByHand);
            this.plHead.Controls.Add(this.txtDdj);
            this.plHead.Controls.Add(this.label11);
            this.plHead.Controls.Add(this.txtPallet);
            this.plHead.Controls.Add(this.label10);
            this.plHead.Controls.Add(this.txtTaskId);
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
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(70)))), ((int)(((byte)(95)))));
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.button1.Location = new System.Drawing.Point(1770, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(71, 25);
            this.button1.TabIndex = 30;
            this.button1.Text = "RFID控制";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtCell
            // 
            this.txtCell.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.txtCell.Location = new System.Drawing.Point(1160, 12);
            this.txtCell.Name = "txtCell";
            this.txtCell.Size = new System.Drawing.Size(68, 25);
            this.txtCell.TabIndex = 29;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label6.Location = new System.Drawing.Point(1112, 14);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(42, 21);
            this.label6.TabIndex = 28;
            this.label6.Text = "仓位";
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(70)))), ((int)(((byte)(95)))));
            this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExport.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExport.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnExport.Location = new System.Drawing.Point(1847, 12);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(72, 25);
            this.btnExport.TabIndex = 27;
            this.btnExport.Text = "导出数据";
            this.btnExport.UseVisualStyleBackColor = false;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnSCControl
            // 
            this.btnSCControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSCControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(70)))), ((int)(((byte)(95)))));
            this.btnSCControl.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSCControl.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSCControl.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnSCControl.Location = new System.Drawing.Point(1681, 12);
            this.btnSCControl.Name = "btnSCControl";
            this.btnSCControl.Size = new System.Drawing.Size(83, 25);
            this.btnSCControl.TabIndex = 26;
            this.btnSCControl.Text = "堆垛机控制";
            this.btnSCControl.UseVisualStyleBackColor = false;
            this.btnSCControl.Click += new System.EventHandler(this.btnSCControl_Click);
            // 
            // txtNoptStation
            // 
            this.txtNoptStation.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.txtNoptStation.Location = new System.Drawing.Point(1401, 11);
            this.txtNoptStation.Name = "txtNoptStation";
            this.txtNoptStation.Size = new System.Drawing.Size(58, 25);
            this.txtNoptStation.TabIndex = 25;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label5.Location = new System.Drawing.Point(1337, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 21);
            this.label5.TabIndex = 24;
            this.label5.Text = "站台号";
            // 
            // btnReback
            // 
            this.btnReback.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReback.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(70)))), ((int)(((byte)(95)))));
            this.btnReback.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnReback.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnReback.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnReback.Location = new System.Drawing.Point(1606, 12);
            this.btnReback.Name = "btnReback";
            this.btnReback.Size = new System.Drawing.Size(69, 25);
            this.btnReback.TabIndex = 23;
            this.btnReback.Text = "还原任务";
            this.btnReback.UseVisualStyleBackColor = false;
            this.btnReback.Click += new System.EventHandler(this.btnReback_Click);
            // 
            // btnFinishByHand
            // 
            this.btnFinishByHand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFinishByHand.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(70)))), ((int)(((byte)(95)))));
            this.btnFinishByHand.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnFinishByHand.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnFinishByHand.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnFinishByHand.Location = new System.Drawing.Point(1529, 12);
            this.btnFinishByHand.Name = "btnFinishByHand";
            this.btnFinishByHand.Size = new System.Drawing.Size(71, 25);
            this.btnFinishByHand.TabIndex = 7;
            this.btnFinishByHand.Text = "过账任务";
            this.btnFinishByHand.UseVisualStyleBackColor = false;
            this.btnFinishByHand.Click += new System.EventHandler(this.btnFinishByHand_Click);
            // 
            // txtDdj
            // 
            this.txtDdj.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.txtDdj.Location = new System.Drawing.Point(1282, 12);
            this.txtDdj.Name = "txtDdj";
            this.txtDdj.Size = new System.Drawing.Size(49, 25);
            this.txtDdj.TabIndex = 20;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label11.Location = new System.Drawing.Point(1234, 14);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(42, 21);
            this.label11.TabIndex = 19;
            this.label11.Text = "巷道";
            // 
            // txtPallet
            // 
            this.txtPallet.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.txtPallet.Location = new System.Drawing.Point(1038, 12);
            this.txtPallet.Name = "txtPallet";
            this.txtPallet.Size = new System.Drawing.Size(68, 25);
            this.txtPallet.TabIndex = 18;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label10.Location = new System.Drawing.Point(974, 14);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(58, 21);
            this.label10.TabIndex = 17;
            this.label10.Text = "托盘号";
            // 
            // txtTaskId
            // 
            this.txtTaskId.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.txtTaskId.Location = new System.Drawing.Point(897, 12);
            this.txtTaskId.Name = "txtTaskId";
            this.txtTaskId.Size = new System.Drawing.Size(71, 25);
            this.txtTaskId.TabIndex = 16;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label9.Location = new System.Drawing.Point(833, 14);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(58, 21);
            this.label9.TabIndex = 15;
            this.label9.Text = "任务号";
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
            this.cboStatus.Location = new System.Drawing.Point(740, 12);
            this.cboStatus.Name = "cboStatus";
            this.cboStatus.Size = new System.Drawing.Size(87, 25);
            this.cboStatus.TabIndex = 14;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label4.Location = new System.Drawing.Point(660, 14);
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
            "整盘入库",
            "空盘入库",
            "整盘出库",
            "空盘出库"});
            this.cboTaskType.Location = new System.Drawing.Point(561, 13);
            this.cboTaskType.Name = "cboTaskType";
            this.cboTaskType.Size = new System.Drawing.Size(93, 25);
            this.cboTaskType.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label3.Location = new System.Drawing.Point(481, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 21);
            this.label3.TabIndex = 11;
            this.label3.Text = "任务类型";
            // 
            // dtpEnd1
            // 
            this.dtpEnd1.CalendarFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpEnd1.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dtpEnd1.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.dtpEnd1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEnd1.Location = new System.Drawing.Point(316, 13);
            this.dtpEnd1.Name = "dtpEnd1";
            this.dtpEnd1.Size = new System.Drawing.Size(159, 25);
            this.dtpEnd1.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label2.Location = new System.Drawing.Point(236, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 21);
            this.label2.TabIndex = 9;
            this.label2.Text = "结束时间";
            // 
            // dtpStart1
            // 
            this.dtpStart1.CalendarFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpStart1.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dtpStart1.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.dtpStart1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStart1.Location = new System.Drawing.Point(72, 12);
            this.dtpStart1.Margin = new System.Windows.Forms.Padding(1, 3, 3, 1);
            this.dtpStart1.Name = "dtpStart1";
            this.dtpStart1.Size = new System.Drawing.Size(158, 25);
            this.dtpStart1.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Location = new System.Drawing.Point(3, 13);
            this.label1.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
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
            this.btnSelect.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSelect.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnSelect.Location = new System.Drawing.Point(1470, 12);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(53, 25);
            this.btnSelect.TabIndex = 6;
            this.btnSelect.Text = "查 询";
            this.btnSelect.UseVisualStyleBackColor = false;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(70)))), ((int)(((byte)(95)))));
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.dvTasks);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 50);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1920, 950);
            this.panel1.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbPages);
            this.groupBox1.Controls.Add(this.lbPageIndex);
            this.groupBox1.Controls.Add(this.lkbNext);
            this.groupBox1.Controls.Add(this.lkbPrev);
            this.groupBox1.Controls.Add(this.lbRowsCount);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 900);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1920, 50);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // lbPages
            // 
            this.lbPages.AutoSize = true;
            this.lbPages.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lbPages.Location = new System.Drawing.Point(234, 23);
            this.lbPages.Name = "lbPages";
            this.lbPages.Size = new System.Drawing.Size(53, 12);
            this.lbPages.TabIndex = 4;
            this.lbPages.Text = "总页数：";
            // 
            // lbPageIndex
            // 
            this.lbPageIndex.AutoSize = true;
            this.lbPageIndex.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lbPageIndex.Location = new System.Drawing.Point(154, 23);
            this.lbPageIndex.Name = "lbPageIndex";
            this.lbPageIndex.Size = new System.Drawing.Size(53, 12);
            this.lbPageIndex.TabIndex = 3;
            this.lbPageIndex.Text = "当前页：";
            // 
            // lkbNext
            // 
            this.lkbNext.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lkbNext.AutoSize = true;
            this.lkbNext.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lkbNext.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.lkbNext.LinkColor = System.Drawing.Color.Yellow;
            this.lkbNext.Location = new System.Drawing.Point(88, 23);
            this.lkbNext.Name = "lkbNext";
            this.lkbNext.Size = new System.Drawing.Size(41, 12);
            this.lkbNext.TabIndex = 2;
            this.lkbNext.TabStop = true;
            this.lkbNext.Text = "下一页";
            this.lkbNext.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lkbNext_LinkClicked);
            // 
            // lkbPrev
            // 
            this.lkbPrev.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lkbPrev.AutoSize = true;
            this.lkbPrev.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lkbPrev.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.lkbPrev.LinkColor = System.Drawing.Color.Yellow;
            this.lkbPrev.Location = new System.Drawing.Point(25, 23);
            this.lkbPrev.Name = "lkbPrev";
            this.lkbPrev.Size = new System.Drawing.Size(41, 12);
            this.lkbPrev.TabIndex = 1;
            this.lkbPrev.TabStop = true;
            this.lkbPrev.Text = "上一页";
            this.lkbPrev.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lkbPrev_LinkClicked);
            // 
            // lbRowsCount
            // 
            this.lbRowsCount.AutoSize = true;
            this.lbRowsCount.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lbRowsCount.Location = new System.Drawing.Point(318, 23);
            this.lbRowsCount.Name = "lbRowsCount";
            this.lbRowsCount.Size = new System.Drawing.Size(53, 12);
            this.lbRowsCount.TabIndex = 0;
            this.lbRowsCount.Text = "总行数：";
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
            this.SeqID,
            this.NPalletID,
            this.NordCN,
            this.NwkStatus,
            this.NoptStation,
            this.Roadway,
            this.CPosidFrom,
            this.CPosidTo,
            this.NlotID,
            this.NpackOrder,
            this.CustomerName,
            this.DoptDate,
            this.DStartDate,
            this.Date_ext1,
            this.Date_ext2,
            this.Date_ext3,
            this.DFinishDate,
            this.Msg});
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
            this.dvTasks.Size = new System.Drawing.Size(1920, 950);
            this.dvTasks.TabIndex = 0;
            // 
            // SeqID
            // 
            this.SeqID.DataPropertyName = "SeqID";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.SeqID.DefaultCellStyle = dataGridViewCellStyle2;
            this.SeqID.FillWeight = 70F;
            this.SeqID.HeaderText = "任务ID";
            this.SeqID.Name = "SeqID";
            this.SeqID.ReadOnly = true;
            // 
            // NPalletID
            // 
            this.NPalletID.DataPropertyName = "NPalletID";
            this.NPalletID.FillWeight = 80F;
            this.NPalletID.HeaderText = "托盘号";
            this.NPalletID.Name = "NPalletID";
            this.NPalletID.ReadOnly = true;
            // 
            // NordCN
            // 
            this.NordCN.DataPropertyName = "NordCN";
            this.NordCN.FillWeight = 80F;
            this.NordCN.HeaderText = "任务类型";
            this.NordCN.Name = "NordCN";
            this.NordCN.ReadOnly = true;
            // 
            // NwkStatus
            // 
            this.NwkStatus.DataPropertyName = "NwkStatus";
            this.NwkStatus.FillWeight = 80F;
            this.NwkStatus.HeaderText = "任务状态";
            this.NwkStatus.Name = "NwkStatus";
            this.NwkStatus.ReadOnly = true;
            // 
            // NoptStation
            // 
            this.NoptStation.DataPropertyName = "NoptStation";
            this.NoptStation.FillWeight = 70F;
            this.NoptStation.HeaderText = "站台号";
            this.NoptStation.Name = "NoptStation";
            // 
            // Roadway
            // 
            this.Roadway.DataPropertyName = "Roadway";
            this.Roadway.FillWeight = 50F;
            this.Roadway.HeaderText = "巷道";
            this.Roadway.Name = "Roadway";
            this.Roadway.ReadOnly = true;
            // 
            // CPosidFrom
            // 
            this.CPosidFrom.DataPropertyName = "CPosidFrom";
            this.CPosidFrom.FillWeight = 70F;
            this.CPosidFrom.HeaderText = "起始位";
            this.CPosidFrom.Name = "CPosidFrom";
            this.CPosidFrom.ReadOnly = true;
            // 
            // CPosidTo
            // 
            this.CPosidTo.DataPropertyName = "CPosidTo";
            this.CPosidTo.FillWeight = 70F;
            this.CPosidTo.HeaderText = "目标位";
            this.CPosidTo.Name = "CPosidTo";
            this.CPosidTo.ReadOnly = true;
            // 
            // NlotID
            // 
            this.NlotID.DataPropertyName = "NlotID";
            this.NlotID.FillWeight = 70F;
            this.NlotID.HeaderText = "批次号";
            this.NlotID.Name = "NlotID";
            // 
            // NpackOrder
            // 
            this.NpackOrder.DataPropertyName = "NpackOrder";
            this.NpackOrder.FillWeight = 80F;
            this.NpackOrder.HeaderText = "装车顺序";
            this.NpackOrder.Name = "NpackOrder";
            this.NpackOrder.ReadOnly = true;
            // 
            // CustomerName
            // 
            this.CustomerName.DataPropertyName = "CustomerName";
            this.CustomerName.FillWeight = 70F;
            this.CustomerName.HeaderText = "区域";
            this.CustomerName.Name = "CustomerName";
            this.CustomerName.ReadOnly = true;
            // 
            // DoptDate
            // 
            this.DoptDate.DataPropertyName = "DoptDate";
            this.DoptDate.HeaderText = "创建时间";
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
            // Date_ext1
            // 
            this.Date_ext1.DataPropertyName = "Date_ext1";
            this.Date_ext1.HeaderText = "堆垛机时间";
            this.Date_ext1.Name = "Date_ext1";
            this.Date_ext1.ReadOnly = true;
            // 
            // Date_ext2
            // 
            this.Date_ext2.DataPropertyName = "Date_ext2";
            this.Date_ext2.HeaderText = "汇流口时间";
            this.Date_ext2.Name = "Date_ext2";
            // 
            // Date_ext3
            // 
            this.Date_ext3.DataPropertyName = "Date_ext3";
            this.Date_ext3.HeaderText = "WCS完成时间";
            this.Date_ext3.Name = "Date_ext3";
            this.Date_ext3.ReadOnly = true;
            // 
            // DFinishDate
            // 
            this.DFinishDate.DataPropertyName = "DFinishDate";
            this.DFinishDate.HeaderText = "WMS完成时间";
            this.DFinishDate.Name = "DFinishDate";
            // 
            // Msg
            // 
            this.Msg.DataPropertyName = "Msg";
            this.Msg.HeaderText = "信息";
            this.Msg.Name = "Msg";
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
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
        private System.Windows.Forms.TextBox txtTaskId;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtPallet;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtDdj;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnFinishByHand;
        private System.Windows.Forms.Button btnReback;
        private System.Windows.Forms.TextBox txtNoptStation;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lbRowsCount;
        private System.Windows.Forms.LinkLabel lkbPrev;
        private System.Windows.Forms.LinkLabel lkbNext;
        private System.Windows.Forms.Label lbPages;
        private System.Windows.Forms.Label lbPageIndex;
        private System.Windows.Forms.Button btnSCControl;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TextBox txtCell;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridViewTextBoxColumn SeqID;
        private System.Windows.Forms.DataGridViewTextBoxColumn NPalletID;
        private System.Windows.Forms.DataGridViewTextBoxColumn NordCN;
        private System.Windows.Forms.DataGridViewTextBoxColumn NwkStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn NoptStation;
        private System.Windows.Forms.DataGridViewTextBoxColumn Roadway;
        private System.Windows.Forms.DataGridViewTextBoxColumn CPosidFrom;
        private System.Windows.Forms.DataGridViewTextBoxColumn CPosidTo;
        private System.Windows.Forms.DataGridViewTextBoxColumn NlotID;
        private System.Windows.Forms.DataGridViewTextBoxColumn NpackOrder;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn DoptDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn DStartDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Date_ext1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Date_ext2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Date_ext3;
        private System.Windows.Forms.DataGridViewTextBoxColumn DFinishDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Msg;
    }
}