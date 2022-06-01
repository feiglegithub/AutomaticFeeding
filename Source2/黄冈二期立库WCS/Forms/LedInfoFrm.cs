using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WCS.DataBase;

namespace WCS.Forms
{
    public partial class LedInfoFrm : Form
    {
        public LedInfoFrm()
        {
            InitializeComponent();
            dvLedInfo.AutoGenerateColumns = false;
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            btnSelect.Enabled = false;
            int lPort = -1;
            if (string.IsNullOrWhiteSpace(txtPort.Text.Trim()))
            {
                lPort = 0;
            }
            else
            {
                var convertResult = int.TryParse(txtPort.Text.Trim(), out lPort);
                if (!convertResult)
                {
                    this.BeginInvoke((Action)(() => MessageBox.Show($"站台号：{txtPort.Text.Trim()}不正确")));
                    btnSelect.Enabled = true;
                    return;
                }
            }
            var startTime = dtpStart.Value;
            var endTime = dtpEnd.Value;
            if (startTime > endTime)
            {
                this.BeginInvoke((Action)(() => MessageBox.Show("开始时间不能大于结束时间")));
                btnSelect.Enabled = true;
                return;
            }
            try
            {
                //var data = WcsSqlB.GetLedInfo(startTime, endTime, lPort);
                var data = WcsSqlB.GetLedInfo(startTime, endTime, lPort, this.checkIsNew.Checked);

                dvLedInfo.DataSource = data;
               
            }
           catch(Exception ex)
            {
                this.BeginInvoke((Action)(() => MessageBox.Show(ex.Message)));
            }
            btnSelect.Enabled = true;

        }


        private void btnStartLed_Click(object sender, EventArgs e)
        {
            foreach(var process in Process.GetProcesses())
            {
                if(process.ProcessName.Contains("LEDAPPConsole"))
                {
                    if(MessageBox.Show(this,"LED程序已经启动，是否关闭然后再次打开","提示", MessageBoxButtons.YesNo ) == DialogResult.Yes)
                    {
                        process.Kill();
                        break;
                        
                    }
                    else
                    {
                        return;
                    }
                }
            }

            Process.Start(@"E:\Project\Source2\LEDLinXin\LEDAPPConsole\bin\Debug\LEDAPPConsole.exe");
        }

        private void dvLedInfo_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1)
            {
                var column = dvLedInfo.Columns[e.ColumnIndex];
                var row = dvLedInfo.Rows[e.RowIndex];
                if (column.Name == "Operation")
                {
                    try
                    {
                        //var data = row.DataGridView.DataSource as DataTable;
                        {

                            var cell = row.Cells["LID"];
                            var lid = Convert.ToInt32(cell.Value.ToString());
                            WcsSqlB.ReSendLedMsg(lid);
                            this.BeginInvoke((Action)(() => MessageBox.Show(this, "重发成功，稍后会刷新led")));
                        }
                    }
                    catch (Exception ex)
                    {
                        this.BeginInvoke((Action)(() => MessageBox.Show(this, "重发异常" + ex.Message)));
                        return;
                    }
                }
            }
        }

        private void dvLedInfo_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1)
            {
                var column = dvLedInfo.Columns[e.ColumnIndex];
                var row = dvLedInfo.Rows[e.RowIndex];
                if (column.Name == "Operation")
                {

                    var cell = row.Cells["Operation"];
                    cell.Value = (object)("重发");
                    try
                    {

                    }
                    catch (Exception ex)
                    {
                        this.BeginInvoke((Action)(() => MessageBox.Show(this, "重发异常" + ex.Message)));
                        return;
                    }
                }
            }
        }
    }
}
