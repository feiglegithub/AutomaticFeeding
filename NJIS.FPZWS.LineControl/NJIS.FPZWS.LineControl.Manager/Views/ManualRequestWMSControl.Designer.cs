namespace NJIS.FPZWS.LineControl.Manager.Views
{
    partial class ManualRequestWMSControl
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.radGroupBox1 = new Telerik.WinControls.UI.RadGroupBox();
            this.radButtonOutAction = new Telerik.WinControls.UI.RadButton();
            this.textBoxOutResult = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxRequestCount = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxOutProductCode = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxOutStation = new System.Windows.Forms.ComboBox();
            this.radGroupBox2 = new Telerik.WinControls.UI.RadGroupBox();
            this.radButtonInAction = new Telerik.WinControls.UI.RadButton();
            this.textBoxReturnResult = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxReturnCount = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.comboBoxReturnProductCode = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxPilerNo = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBoxReturnStation = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox1)).BeginInit();
            this.radGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonOutAction)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox2)).BeginInit();
            this.radGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonInAction)).BeginInit();
            this.SuspendLayout();
            // 
            // radGroupBox1
            // 
            this.radGroupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBox1.Controls.Add(this.radButtonOutAction);
            this.radGroupBox1.Controls.Add(this.textBoxOutResult);
            this.radGroupBox1.Controls.Add(this.label4);
            this.radGroupBox1.Controls.Add(this.textBoxRequestCount);
            this.radGroupBox1.Controls.Add(this.label3);
            this.radGroupBox1.Controls.Add(this.comboBoxOutProductCode);
            this.radGroupBox1.Controls.Add(this.label2);
            this.radGroupBox1.Controls.Add(this.label1);
            this.radGroupBox1.Controls.Add(this.comboBoxOutStation);
            this.radGroupBox1.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.radGroupBox1.HeaderText = "要料出库";
            this.radGroupBox1.Location = new System.Drawing.Point(42, 110);
            this.radGroupBox1.Name = "radGroupBox1";
            this.radGroupBox1.Size = new System.Drawing.Size(595, 718);
            this.radGroupBox1.TabIndex = 0;
            this.radGroupBox1.Text = "要料出库";
            // 
            // radButtonOutAction
            // 
            this.radButtonOutAction.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.radButtonOutAction.Location = new System.Drawing.Point(468, 660);
            this.radButtonOutAction.Name = "radButtonOutAction";
            this.radButtonOutAction.Size = new System.Drawing.Size(93, 35);
            this.radButtonOutAction.TabIndex = 9;
            this.radButtonOutAction.Text = "执行";
            this.radButtonOutAction.Click += new System.EventHandler(this.radButtonOutAction_Click);
            // 
            // textBoxOutResult
            // 
            this.textBoxOutResult.BackColor = System.Drawing.Color.White;
            this.textBoxOutResult.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.textBoxOutResult.Location = new System.Drawing.Point(126, 255);
            this.textBoxOutResult.Multiline = true;
            this.textBoxOutResult.Name = "textBoxOutResult";
            this.textBoxOutResult.ReadOnly = true;
            this.textBoxOutResult.Size = new System.Drawing.Size(435, 399);
            this.textBoxOutResult.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.label4.Location = new System.Drawing.Point(20, 255);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 35);
            this.label4.TabIndex = 7;
            this.label4.Text = "反馈结果";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBoxRequestCount
            // 
            this.textBoxRequestCount.BackColor = System.Drawing.SystemColors.Info;
            this.textBoxRequestCount.Font = new System.Drawing.Font("Segoe UI", 15F);
            this.textBoxRequestCount.Location = new System.Drawing.Point(126, 190);
            this.textBoxRequestCount.Name = "textBoxRequestCount";
            this.textBoxRequestCount.Size = new System.Drawing.Size(435, 34);
            this.textBoxRequestCount.TabIndex = 6;
            this.textBoxRequestCount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxRequestCount_KeyPress);
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.label3.Location = new System.Drawing.Point(20, 190);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 35);
            this.label3.TabIndex = 5;
            this.label3.Text = "需求数量";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // comboBoxOutProductCode
            // 
            this.comboBoxOutProductCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxOutProductCode.Font = new System.Drawing.Font("Segoe UI", 15F);
            this.comboBoxOutProductCode.FormattingEnabled = true;
            this.comboBoxOutProductCode.Location = new System.Drawing.Point(126, 120);
            this.comboBoxOutProductCode.Name = "comboBoxOutProductCode";
            this.comboBoxOutProductCode.Size = new System.Drawing.Size(435, 36);
            this.comboBoxOutProductCode.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.label2.Location = new System.Drawing.Point(20, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 35);
            this.label2.TabIndex = 3;
            this.label2.Text = "出库站台";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.label1.Location = new System.Drawing.Point(20, 121);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 35);
            this.label1.TabIndex = 2;
            this.label1.Text = "物料编码";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // comboBoxOutStation
            // 
            this.comboBoxOutStation.BackColor = System.Drawing.Color.White;
            this.comboBoxOutStation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxOutStation.Font = new System.Drawing.Font("Segoe UI", 15F);
            this.comboBoxOutStation.FormattingEnabled = true;
            this.comboBoxOutStation.Items.AddRange(new object[] {
            "3006",
            "3007",
            "3008"});
            this.comboBoxOutStation.Location = new System.Drawing.Point(126, 52);
            this.comboBoxOutStation.Name = "comboBoxOutStation";
            this.comboBoxOutStation.Size = new System.Drawing.Size(435, 36);
            this.comboBoxOutStation.TabIndex = 1;
            // 
            // radGroupBox2
            // 
            this.radGroupBox2.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.radGroupBox2.Controls.Add(this.radButtonInAction);
            this.radGroupBox2.Controls.Add(this.textBoxReturnResult);
            this.radGroupBox2.Controls.Add(this.label9);
            this.radGroupBox2.Controls.Add(this.textBoxReturnCount);
            this.radGroupBox2.Controls.Add(this.label8);
            this.radGroupBox2.Controls.Add(this.comboBoxReturnProductCode);
            this.radGroupBox2.Controls.Add(this.label7);
            this.radGroupBox2.Controls.Add(this.textBoxPilerNo);
            this.radGroupBox2.Controls.Add(this.label6);
            this.radGroupBox2.Controls.Add(this.comboBoxReturnStation);
            this.radGroupBox2.Controls.Add(this.label5);
            this.radGroupBox2.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.radGroupBox2.HeaderText = "余料回库";
            this.radGroupBox2.Location = new System.Drawing.Point(658, 110);
            this.radGroupBox2.Name = "radGroupBox2";
            this.radGroupBox2.Size = new System.Drawing.Size(598, 718);
            this.radGroupBox2.TabIndex = 1;
            this.radGroupBox2.Text = "余料回库";
            // 
            // radButtonInAction
            // 
            this.radButtonInAction.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.radButtonInAction.Location = new System.Drawing.Point(471, 660);
            this.radButtonInAction.Name = "radButtonInAction";
            this.radButtonInAction.Size = new System.Drawing.Size(93, 35);
            this.radButtonInAction.TabIndex = 10;
            this.radButtonInAction.Text = "执行";
            this.radButtonInAction.Click += new System.EventHandler(this.radButtonInAction_Click);
            // 
            // textBoxReturnResult
            // 
            this.textBoxReturnResult.BackColor = System.Drawing.Color.White;
            this.textBoxReturnResult.Font = new System.Drawing.Font("Segoe UI", 15F);
            this.textBoxReturnResult.Location = new System.Drawing.Point(129, 318);
            this.textBoxReturnResult.Multiline = true;
            this.textBoxReturnResult.Name = "textBoxReturnResult";
            this.textBoxReturnResult.ReadOnly = true;
            this.textBoxReturnResult.Size = new System.Drawing.Size(435, 336);
            this.textBoxReturnResult.TabIndex = 10;
            // 
            // label9
            // 
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.label9.Location = new System.Drawing.Point(23, 318);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(100, 35);
            this.label9.TabIndex = 10;
            this.label9.Text = "反馈结果";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBoxReturnCount
            // 
            this.textBoxReturnCount.BackColor = System.Drawing.SystemColors.Info;
            this.textBoxReturnCount.Font = new System.Drawing.Font("Segoe UI", 15F);
            this.textBoxReturnCount.Location = new System.Drawing.Point(129, 255);
            this.textBoxReturnCount.Name = "textBoxReturnCount";
            this.textBoxReturnCount.Size = new System.Drawing.Size(435, 34);
            this.textBoxReturnCount.TabIndex = 12;
            this.textBoxReturnCount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxReturnCount_KeyPress);
            // 
            // label8
            // 
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.label8.Location = new System.Drawing.Point(23, 255);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(100, 35);
            this.label8.TabIndex = 11;
            this.label8.Text = "余垛数量";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // comboBoxReturnProductCode
            // 
            this.comboBoxReturnProductCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxReturnProductCode.Font = new System.Drawing.Font("Segoe UI", 15F);
            this.comboBoxReturnProductCode.FormattingEnabled = true;
            this.comboBoxReturnProductCode.Location = new System.Drawing.Point(129, 122);
            this.comboBoxReturnProductCode.Name = "comboBoxReturnProductCode";
            this.comboBoxReturnProductCode.Size = new System.Drawing.Size(435, 36);
            this.comboBoxReturnProductCode.TabIndex = 10;
            // 
            // label7
            // 
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.label7.Location = new System.Drawing.Point(23, 122);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(100, 35);
            this.label7.TabIndex = 10;
            this.label7.Text = "物料编码";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBoxPilerNo
            // 
            this.textBoxPilerNo.BackColor = System.Drawing.SystemColors.Info;
            this.textBoxPilerNo.Font = new System.Drawing.Font("Segoe UI", 15F);
            this.textBoxPilerNo.Location = new System.Drawing.Point(129, 190);
            this.textBoxPilerNo.Name = "textBoxPilerNo";
            this.textBoxPilerNo.Size = new System.Drawing.Size(435, 34);
            this.textBoxPilerNo.TabIndex = 7;
            this.textBoxPilerNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxPilerNo_KeyPress);
            // 
            // label6
            // 
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.label6.Location = new System.Drawing.Point(23, 192);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 35);
            this.label6.TabIndex = 2;
            this.label6.Text = "WMS垛号";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // comboBoxReturnStation
            // 
            this.comboBoxReturnStation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxReturnStation.Font = new System.Drawing.Font("Segoe UI", 15F);
            this.comboBoxReturnStation.FormattingEnabled = true;
            this.comboBoxReturnStation.Items.AddRange(new object[] {
            "3006",
            "3007",
            "3008",
            "2021"});
            this.comboBoxReturnStation.Location = new System.Drawing.Point(129, 52);
            this.comboBoxReturnStation.Name = "comboBoxReturnStation";
            this.comboBoxReturnStation.Size = new System.Drawing.Size(435, 36);
            this.comboBoxReturnStation.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.label5.Location = new System.Drawing.Point(23, 52);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 35);
            this.label5.TabIndex = 0;
            this.label5.Text = "入库站台";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ManualRequestWMSControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.Controls.Add(this.radGroupBox2);
            this.Controls.Add(this.radGroupBox1);
            this.Name = "ManualRequestWMSControl";
            this.Size = new System.Drawing.Size(1292, 863);
            this.Load += new System.EventHandler(this.ManualRequestWMSControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox1)).EndInit();
            this.radGroupBox1.ResumeLayout(false);
            this.radGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonOutAction)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox2)).EndInit();
            this.radGroupBox2.ResumeLayout(false);
            this.radGroupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonInAction)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadGroupBox radGroupBox1;
        private System.Windows.Forms.TextBox textBoxOutResult;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxRequestCount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxOutProductCode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxOutStation;
        private Telerik.WinControls.UI.RadButton radButtonOutAction;
        private Telerik.WinControls.UI.RadGroupBox radGroupBox2;
        private System.Windows.Forms.ComboBox comboBoxReturnProductCode;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxPilerNo;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBoxReturnStation;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxReturnResult;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBoxReturnCount;
        private System.Windows.Forms.Label label8;
        private Telerik.WinControls.UI.RadButton radButtonInAction;
    }
}
