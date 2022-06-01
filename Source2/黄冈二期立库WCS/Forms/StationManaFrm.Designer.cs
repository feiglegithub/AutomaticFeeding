namespace WCS.Forms
{
    partial class StationManaFrm
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
            this.btnSave = new System.Windows.Forms.Button();
            this.cboStatus = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cboSType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSelect = new System.Windows.Forms.Button();
            this.dvTasks = new System.Windows.Forms.DataGridView();
            this.WMSNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BufferC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BufferM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.State = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Remark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.plHead.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dvTasks)).BeginInit();
            this.SuspendLayout();
            // 
            // plHead
            // 
            this.plHead.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(70)))), ((int)(((byte)(95)))));
            this.plHead.Controls.Add(this.btnSave);
            this.plHead.Controls.Add(this.cboStatus);
            this.plHead.Controls.Add(this.label4);
            this.plHead.Controls.Add(this.cboSType);
            this.plHead.Controls.Add(this.label3);
            this.plHead.Controls.Add(this.btnSelect);
            this.plHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.plHead.Location = new System.Drawing.Point(0, 0);
            this.plHead.Name = "plHead";
            this.plHead.Size = new System.Drawing.Size(1920, 44);
            this.plHead.TabIndex = 1;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(70)))), ((int)(((byte)(95)))));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSave.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSave.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnSave.Location = new System.Drawing.Point(528, 6);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 29);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "保存修改";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // cboStatus
            // 
            this.cboStatus.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.cboStatus.FormattingEnabled = true;
            this.cboStatus.Items.AddRange(new object[] {
            "",
            "启用",
            "禁用"});
            this.cboStatus.Location = new System.Drawing.Point(291, 8);
            this.cboStatus.Name = "cboStatus";
            this.cboStatus.Size = new System.Drawing.Size(98, 25);
            this.cboStatus.TabIndex = 14;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label4.Location = new System.Drawing.Point(211, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 21);
            this.label4.TabIndex = 13;
            this.label4.Text = "站台状态";
            // 
            // cboSType
            // 
            this.cboSType.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.cboSType.FormattingEnabled = true;
            this.cboSType.Items.AddRange(new object[] {
            "",
            "入口",
            "出口"});
            this.cboSType.Location = new System.Drawing.Point(92, 8);
            this.cboSType.Name = "cboSType";
            this.cboSType.Size = new System.Drawing.Size(93, 25);
            this.cboSType.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 21);
            this.label3.TabIndex = 11;
            this.label3.Text = "站台类型";
            // 
            // btnSelect
            // 
            this.btnSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(70)))), ((int)(((byte)(95)))));
            this.btnSelect.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSelect.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSelect.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnSelect.Location = new System.Drawing.Point(415, 6);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(90, 29);
            this.btnSelect.TabIndex = 6;
            this.btnSelect.Text = "查 询";
            this.btnSelect.UseVisualStyleBackColor = false;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
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
            this.WMSNo,
            this.BufferC,
            this.BufferM,
            this.SType,
            this.State,
            this.Remark});
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
            this.dvTasks.Location = new System.Drawing.Point(0, 44);
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
            this.dvTasks.Size = new System.Drawing.Size(1920, 956);
            this.dvTasks.TabIndex = 2;
            this.dvTasks.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dvTasks_CellValueChanged);
            // 
            // WMSNo
            // 
            this.WMSNo.DataPropertyName = "WMSNo";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.WMSNo.DefaultCellStyle = dataGridViewCellStyle2;
            this.WMSNo.FillWeight = 10F;
            this.WMSNo.HeaderText = "站台号";
            this.WMSNo.Name = "WMSNo";
            this.WMSNo.ReadOnly = true;
            // 
            // BufferC
            // 
            this.BufferC.DataPropertyName = "BufferC";
            this.BufferC.FillWeight = 10F;
            this.BufferC.HeaderText = "缓存数量";
            this.BufferC.Name = "BufferC";
            // 
            // BufferM
            // 
            this.BufferM.DataPropertyName = "BufferM";
            this.BufferM.FillWeight = 15F;
            this.BufferM.HeaderText = "最大缓存数量";
            this.BufferM.Name = "BufferM";
            this.BufferM.ReadOnly = true;
            // 
            // SType
            // 
            this.SType.DataPropertyName = "SType";
            this.SType.FillWeight = 10F;
            this.SType.HeaderText = "类型";
            this.SType.Name = "SType";
            this.SType.ReadOnly = true;
            // 
            // State
            // 
            this.State.DataPropertyName = "State";
            this.State.FillWeight = 10F;
            this.State.HeaderText = "状态";
            this.State.Name = "State";
            this.State.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.State.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // Remark
            // 
            this.Remark.DataPropertyName = "Remark";
            this.Remark.FillWeight = 50F;
            this.Remark.HeaderText = "描述";
            this.Remark.Name = "Remark";
            // 
            // StationManaFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1920, 1000);
            this.Controls.Add(this.dvTasks);
            this.Controls.Add(this.plHead);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "StationManaFrm";
            this.Text = "StationManaFrm";
            this.plHead.ResumeLayout(false);
            this.plHead.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dvTasks)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel plHead;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ComboBox cboStatus;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboSType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.DataGridView dvTasks;
        private System.Windows.Forms.DataGridViewTextBoxColumn WMSNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn BufferC;
        private System.Windows.Forms.DataGridViewTextBoxColumn BufferM;
        private System.Windows.Forms.DataGridViewTextBoxColumn SType;
        private System.Windows.Forms.DataGridViewCheckBoxColumn State;
        private System.Windows.Forms.DataGridViewTextBoxColumn Remark;
    }
}