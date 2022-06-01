namespace WCS.Forms
{
    partial class LogFrm
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
            this.txtLike = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dtpEnd1 = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpStart1 = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDNo = new System.Windows.Forms.TextBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.cboLType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSelect = new System.Windows.Forms.Button();
            this.dvLog = new System.Windows.Forms.DataGridView();
            this.RId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Msg = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DeviceNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LogDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EndDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.plHead.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dvLog)).BeginInit();
            this.SuspendLayout();
            // 
            // plHead
            // 
            this.plHead.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(70)))), ((int)(((byte)(95)))));
            this.plHead.Controls.Add(this.txtLike);
            this.plHead.Controls.Add(this.label5);
            this.plHead.Controls.Add(this.dtpEnd1);
            this.plHead.Controls.Add(this.label2);
            this.plHead.Controls.Add(this.dtpStart1);
            this.plHead.Controls.Add(this.label1);
            this.plHead.Controls.Add(this.txtDNo);
            this.plHead.Controls.Add(this.btnExport);
            this.plHead.Controls.Add(this.label4);
            this.plHead.Controls.Add(this.cboLType);
            this.plHead.Controls.Add(this.label3);
            this.plHead.Controls.Add(this.btnSelect);
            this.plHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.plHead.Location = new System.Drawing.Point(0, 0);
            this.plHead.Name = "plHead";
            this.plHead.Size = new System.Drawing.Size(1920, 44);
            this.plHead.TabIndex = 2;
            // 
            // txtLike
            // 
            this.txtLike.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtLike.Location = new System.Drawing.Point(929, 12);
            this.txtLike.Name = "txtLike";
            this.txtLike.Size = new System.Drawing.Size(197, 23);
            this.txtLike.TabIndex = 22;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label5.Location = new System.Drawing.Point(850, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(74, 21);
            this.label5.TabIndex = 21;
            this.label5.Text = "日志信息";
            // 
            // dtpEnd1
            // 
            this.dtpEnd1.CalendarFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpEnd1.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.dtpEnd1.Location = new System.Drawing.Point(329, 11);
            this.dtpEnd1.Name = "dtpEnd1";
            this.dtpEnd1.Size = new System.Drawing.Size(151, 25);
            this.dtpEnd1.TabIndex = 20;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label2.Location = new System.Drawing.Point(249, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 21);
            this.label2.TabIndex = 19;
            this.label2.Text = "结束时间";
            // 
            // dtpStart1
            // 
            this.dtpStart1.CalendarFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpStart1.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.dtpStart1.Location = new System.Drawing.Point(92, 12);
            this.dtpStart1.Name = "dtpStart1";
            this.dtpStart1.Size = new System.Drawing.Size(151, 25);
            this.dtpStart1.TabIndex = 18;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 21);
            this.label1.TabIndex = 17;
            this.label1.Text = "开始时间";
            // 
            // txtDNo
            // 
            this.txtDNo.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtDNo.Location = new System.Drawing.Point(765, 13);
            this.txtDNo.Name = "txtDNo";
            this.txtDNo.Size = new System.Drawing.Size(71, 23);
            this.txtDNo.TabIndex = 16;
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(70)))), ((int)(((byte)(95)))));
            this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExport.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExport.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnExport.Location = new System.Drawing.Point(1251, 11);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(90, 26);
            this.btnExport.TabIndex = 15;
            this.btnExport.Text = "导 出";
            this.btnExport.UseVisualStyleBackColor = false;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
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
            this.label4.Text = "设备编号";
            // 
            // cboLType
            // 
            this.cboLType.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.cboLType.FormattingEnabled = true;
            this.cboLType.Items.AddRange(new object[] {
            "",
            "LOG",
            "ERROR"});
            this.cboLType.Location = new System.Drawing.Point(577, 12);
            this.cboLType.Name = "cboLType";
            this.cboLType.Size = new System.Drawing.Size(93, 25);
            this.cboLType.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label3.Location = new System.Drawing.Point(497, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 21);
            this.label3.TabIndex = 11;
            this.label3.Text = "日志类型";
            // 
            // btnSelect
            // 
            this.btnSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(70)))), ((int)(((byte)(95)))));
            this.btnSelect.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSelect.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSelect.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnSelect.Location = new System.Drawing.Point(1144, 11);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(90, 26);
            this.btnSelect.TabIndex = 6;
            this.btnSelect.Text = "查 询";
            this.btnSelect.UseVisualStyleBackColor = false;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // dvLog
            // 
            this.dvLog.AllowUserToAddRows = false;
            this.dvLog.AllowUserToDeleteRows = false;
            this.dvLog.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dvLog.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(70)))), ((int)(((byte)(95)))));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(90)))), ((int)(((byte)(115)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微软雅黑", 11F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dvLog.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dvLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dvLog.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.RId,
            this.Msg,
            this.DeviceNo,
            this.LType,
            this.LogDate,
            this.EndDate});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(70)))), ((int)(((byte)(95)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dvLog.DefaultCellStyle = dataGridViewCellStyle3;
            this.dvLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dvLog.EnableHeadersVisualStyles = false;
            this.dvLog.Location = new System.Drawing.Point(0, 44);
            this.dvLog.Name = "dvLog";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(70)))), ((int)(((byte)(95)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dvLog.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dvLog.RowTemplate.Height = 23;
            this.dvLog.Size = new System.Drawing.Size(1920, 956);
            this.dvLog.TabIndex = 3;
            this.dvLog.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dvLog_CellContentClick);
            // 
            // RId
            // 
            this.RId.DataPropertyName = "RId";
            this.RId.FillWeight = 3F;
            this.RId.HeaderText = "行号";
            this.RId.Name = "RId";
            // 
            // Msg
            // 
            this.Msg.DataPropertyName = "Msg";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Msg.DefaultCellStyle = dataGridViewCellStyle2;
            this.Msg.FillWeight = 11.07868F;
            this.Msg.HeaderText = "日志信息";
            this.Msg.Name = "Msg";
            this.Msg.ReadOnly = true;
            // 
            // DeviceNo
            // 
            this.DeviceNo.DataPropertyName = "DeviceNo";
            this.DeviceNo.FillWeight = 2.76967F;
            this.DeviceNo.HeaderText = "设备编号";
            this.DeviceNo.Name = "DeviceNo";
            // 
            // LType
            // 
            this.LType.DataPropertyName = "LType";
            this.LType.FillWeight = 2.76967F;
            this.LType.HeaderText = "日志类型";
            this.LType.Name = "LType";
            // 
            // LogDate
            // 
            this.LogDate.DataPropertyName = "LogDate";
            this.LogDate.FillWeight = 2.76967F;
            this.LogDate.HeaderText = "开始时间";
            this.LogDate.Name = "LogDate";
            this.LogDate.ReadOnly = true;
            // 
            // EndDate
            // 
            this.EndDate.DataPropertyName = "EndDate";
            this.EndDate.FillWeight = 2.76967F;
            this.EndDate.HeaderText = "结束时间";
            this.EndDate.Name = "EndDate";
            this.EndDate.ReadOnly = true;
            // 
            // LogFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(70)))), ((int)(((byte)(95)))));
            this.ClientSize = new System.Drawing.Size(1920, 1000);
            this.Controls.Add(this.dvLog);
            this.Controls.Add(this.plHead);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "LogFrm";
            this.Text = "LogFrm";
            this.plHead.ResumeLayout(false);
            this.plHead.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dvLog)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel plHead;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboLType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.DataGridView dvLog;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.TextBox txtDNo;
        private System.Windows.Forms.DateTimePicker dtpEnd1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpStart1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtLike;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridViewTextBoxColumn RId;
        private System.Windows.Forms.DataGridViewTextBoxColumn Msg;
        private System.Windows.Forms.DataGridViewTextBoxColumn DeviceNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn LType;
        private System.Windows.Forms.DataGridViewTextBoxColumn LogDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn EndDate;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}