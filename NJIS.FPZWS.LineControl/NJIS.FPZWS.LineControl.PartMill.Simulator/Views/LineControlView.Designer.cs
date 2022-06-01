namespace NJIS.FPZWS.LineControl.PartMill.Simulator.Views
{
    partial class LineControlView
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
            NJIS.FPZWS.LineControl.PartMill.Simulator.Models.LineModel lineModel1 = new NJIS.FPZWS.LineControl.PartMill.Simulator.Models.LineModel();
            NJIS.FPZWS.LineControl.PartMill.Simulator.Models.LineModel lineModel2 = new NJIS.FPZWS.LineControl.PartMill.Simulator.Models.LineModel();
            NJIS.FPZWS.LineControl.PartMill.Simulator.Models.LineModel lineModel3 = new NJIS.FPZWS.LineControl.PartMill.Simulator.Models.LineModel();
            NJIS.FPZWS.LineControl.PartMill.Simulator.Models.LineModel lineModel4 = new NJIS.FPZWS.LineControl.PartMill.Simulator.Models.LineModel();
            NJIS.FPZWS.LineControl.PartMill.Simulator.Models.LineModel lineModel5 = new NJIS.FPZWS.LineControl.PartMill.Simulator.Models.LineModel();
            NJIS.FPZWS.LineControl.PartMill.Simulator.Models.LineModel lineModel6 = new NJIS.FPZWS.LineControl.PartMill.Simulator.Models.LineModel();
            this.radGroupBox1 = new Telerik.WinControls.UI.RadGroupBox();
            this.btnListen = new Telerik.WinControls.UI.RadButton();
            this.btnStop = new Telerik.WinControls.UI.RadButton();
            this.lineModelControl4 = new NJIS.FPZWS.LineControl.PartMill.Simulator.Controls.LineModelControl();
            this.lineModelControl6 = new NJIS.FPZWS.LineControl.PartMill.Simulator.Controls.LineModelControl();
            this.lineModelControl5 = new NJIS.FPZWS.LineControl.PartMill.Simulator.Controls.LineModelControl();
            this.lineModelControl3 = new NJIS.FPZWS.LineControl.PartMill.Simulator.Controls.LineModelControl();
            this.lineModelControl2 = new NJIS.FPZWS.LineControl.PartMill.Simulator.Controls.LineModelControl();
            this.lineModelControl1 = new NJIS.FPZWS.LineControl.PartMill.Simulator.Controls.LineModelControl();
            this.lineCommandControl1 = new NJIS.FPZWS.LineControl.PartMill.Simulator.Controls.LineCommandControl();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox1)).BeginInit();
            this.radGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnListen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnStop)).BeginInit();
            this.SuspendLayout();
            // 
            // radGroupBox1
            // 
            this.radGroupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBox1.Controls.Add(this.lineModelControl4);
            this.radGroupBox1.Controls.Add(this.lineModelControl6);
            this.radGroupBox1.Controls.Add(this.lineModelControl5);
            this.radGroupBox1.Controls.Add(this.lineModelControl3);
            this.radGroupBox1.Controls.Add(this.lineModelControl2);
            this.radGroupBox1.Controls.Add(this.lineModelControl1);
            this.radGroupBox1.HeaderText = "radGroupBox1";
            this.radGroupBox1.Location = new System.Drawing.Point(3, 72);
            this.radGroupBox1.Name = "radGroupBox1";
            this.radGroupBox1.Size = new System.Drawing.Size(957, 648);
            this.radGroupBox1.TabIndex = 1;
            this.radGroupBox1.Text = "radGroupBox1";
            // 
            // btnListen
            // 
            this.btnListen.Location = new System.Drawing.Point(499, 19);
            this.btnListen.Name = "btnListen";
            this.btnListen.Size = new System.Drawing.Size(70, 24);
            this.btnListen.TabIndex = 2;
            this.btnListen.Text = "开始监听";
            this.btnListen.Click += new System.EventHandler(this.btnListen_Click);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(580, 20);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(70, 24);
            this.btnStop.TabIndex = 2;
            this.btnStop.Text = "停止监听";
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // lineModelControl4
            // 
            this.lineModelControl4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            lineModel1.Amount = ((short)(0));
            lineModel1.BackupShort = ((short)(0));
            lineModel1.BackupString = null;
            lineModel1.HasBoard = false;
            lineModel1.IsFinished = false;
            lineModel1.NeedRun = false;
            lineModel1.PilerNo = 0;
            lineModel1.Target = ((short)(0));
            this.lineModelControl4.Data = lineModel1;
            this.lineModelControl4.LineName = NJIS.FPZWS.LineControl.PartMill.Simulator.Models.ELineName.Gt5004.ToString();
            this.lineModelControl4.Location = new System.Drawing.Point(5, 325);
            this.lineModelControl4.Name = "lineModelControl4";
            this.lineModelControl4.Size = new System.Drawing.Size(266, 298);
            this.lineModelControl4.TabIndex = 0;
            // 
            // lineModelControl6
            // 
            this.lineModelControl6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            lineModel2.Amount = ((short)(0));
            lineModel2.BackupShort = ((short)(0));
            lineModel2.BackupString = null;
            lineModel2.HasBoard = false;
            lineModel2.IsFinished = false;
            lineModel2.LineName = NJIS.FPZWS.LineControl.PartMill.Simulator.Models.ELineName.Gt5006.ToString();
            lineModel2.NeedRun = false;
            lineModel2.PilerNo = 0;
            lineModel2.Target = ((short)(0));
            this.lineModelControl6.Data = lineModel2;
            this.lineModelControl6.LineName = NJIS.FPZWS.LineControl.PartMill.Simulator.Models.ELineName.Gt5006.ToString();
            this.lineModelControl6.Location = new System.Drawing.Point(577, 325);
            this.lineModelControl6.Name = "lineModelControl6";
            this.lineModelControl6.Size = new System.Drawing.Size(266, 298);
            this.lineModelControl6.TabIndex = 0;
            // 
            // lineModelControl5
            // 
            this.lineModelControl5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            lineModel3.Amount = ((short)(0));
            lineModel3.BackupShort = ((short)(0));
            lineModel3.BackupString = null;
            lineModel3.HasBoard = false;
            lineModel3.IsFinished = false;
            lineModel3.LineName = NJIS.FPZWS.LineControl.PartMill.Simulator.Models.ELineName.Gt5005.ToString();
            lineModel3.NeedRun = false;
            lineModel3.PilerNo = 0;
            lineModel3.Target = ((short)(0));
            this.lineModelControl5.Data = lineModel3;
            this.lineModelControl5.LineName = NJIS.FPZWS.LineControl.PartMill.Simulator.Models.ELineName.Gt5005.ToString();
            this.lineModelControl5.Location = new System.Drawing.Point(287, 325);
            this.lineModelControl5.Name = "lineModelControl5";
            this.lineModelControl5.Size = new System.Drawing.Size(266, 298);
            this.lineModelControl5.TabIndex = 0;
            // 
            // lineModelControl3
            // 
            this.lineModelControl3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            lineModel4.Amount = ((short)(0));
            lineModel4.BackupShort = ((short)(0));
            lineModel4.BackupString = null;
            lineModel4.HasBoard = false;
            lineModel4.IsFinished = false;
            lineModel4.LineName = NJIS.FPZWS.LineControl.PartMill.Simulator.Models.ELineName.Xz5003.ToString();
            lineModel4.NeedRun = false;
            lineModel4.PilerNo = 0;
            lineModel4.Target = ((short)(0));
            this.lineModelControl3.Data = lineModel4;
            this.lineModelControl3.LineName = NJIS.FPZWS.LineControl.PartMill.Simulator.Models.ELineName.Xz5003.ToString();
            this.lineModelControl3.Location = new System.Drawing.Point(577, 21);
            this.lineModelControl3.Name = "lineModelControl3";
            this.lineModelControl3.Size = new System.Drawing.Size(266, 298);
            this.lineModelControl3.TabIndex = 0;
            // 
            // lineModelControl2
            // 
            this.lineModelControl2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            lineModel5.Amount = ((short)(0));
            lineModel5.BackupShort = ((short)(0));
            lineModel5.BackupString = null;
            lineModel5.HasBoard = false;
            lineModel5.IsFinished = false;
            lineModel5.LineName = NJIS.FPZWS.LineControl.PartMill.Simulator.Models.ELineName.Gt5002.ToString();
            lineModel5.NeedRun = false;
            lineModel5.PilerNo = 0;
            lineModel5.Target = ((short)(0));
            this.lineModelControl2.Data = lineModel5;
            this.lineModelControl2.LineName = NJIS.FPZWS.LineControl.PartMill.Simulator.Models.ELineName.Gt5002.ToString();
            this.lineModelControl2.Location = new System.Drawing.Point(287, 21);
            this.lineModelControl2.Name = "lineModelControl2";
            this.lineModelControl2.Size = new System.Drawing.Size(266, 298);
            this.lineModelControl2.TabIndex = 0;
            // 
            // lineModelControl1
            // 
            this.lineModelControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            lineModel6.Amount = ((short)(0));
            lineModel6.BackupShort = ((short)(0));
            lineModel6.BackupString = null;
            lineModel6.HasBoard = false;
            lineModel6.IsFinished = false;
            lineModel6.LineName = NJIS.FPZWS.LineControl.PartMill.Simulator.Models.ELineName.Gt5001.ToString();
            lineModel6.NeedRun = false;
            lineModel6.PilerNo = 0;
            lineModel6.Target = ((short)(0));
            this.lineModelControl1.Data = lineModel6;
            this.lineModelControl1.LineName = NJIS.FPZWS.LineControl.PartMill.Simulator.Models.ELineName.Gt5001.ToString();
            this.lineModelControl1.Location = new System.Drawing.Point(5, 21);
            this.lineModelControl1.Name = "lineModelControl1";
            this.lineModelControl1.Size = new System.Drawing.Size(266, 298);
            this.lineModelControl1.TabIndex = 0;
            // 
            // lineCommandControl1
            // 
            this.lineCommandControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.lineCommandControl1.Location = new System.Drawing.Point(3, 14);
            this.lineCommandControl1.Name = "lineCommandControl1";
            this.lineCommandControl1.SelectedFromItem = null;
            this.lineCommandControl1.SelectedTargetItem = null;
            this.lineCommandControl1.Size = new System.Drawing.Size(417, 65);
            this.lineCommandControl1.TabIndex = 0;
            // 
            // LineControlView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnListen);
            this.Controls.Add(this.radGroupBox1);
            this.Controls.Add(this.lineCommandControl1);
            this.Name = "LineControlView";
            this.Size = new System.Drawing.Size(963, 720);
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox1)).EndInit();
            this.radGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnListen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnStop)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.LineCommandControl lineCommandControl1;
        private Telerik.WinControls.UI.RadGroupBox radGroupBox1;
        private Controls.LineModelControl lineModelControl4;
        private Controls.LineModelControl lineModelControl5;
        private Controls.LineModelControl lineModelControl3;
        private Controls.LineModelControl lineModelControl2;
        private Controls.LineModelControl lineModelControl1;
        private Controls.LineModelControl lineModelControl6;
        private Telerik.WinControls.UI.RadButton btnListen;
        private Telerik.WinControls.UI.RadButton btnStop;
    }
}
