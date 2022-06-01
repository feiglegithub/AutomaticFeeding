using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WCS.DataBase;

namespace WCS.Forms
{
    public partial class LogFrm : Form
    {
        public LogFrm()
        {
            InitializeComponent();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            var where = $" and LogDate between '{this.dtpStart1.Value.ToString("yyyy-MM-dd 0:00:00")}' and '{this.dtpEnd1.Value.ToString("yyyy-MM-dd 23:59:59")}' ";
            if (this.cboLType.Text != "")
            {
                where += $" and LType='{this.cboLType.Text}' ";
            }
            if(this.txtDNo.Text != "")
            {
                where += $" and DeviceNo='{this.txtDNo.Text}' ";
            }
            if (this.txtLike.Text != "")
            {
                where += $" and Msg like '%{this.txtLike.Text}%' ";
            }

            this.dvLog.DataSource = WcsSqlB.SelectLog(where);
            this.dvLog.Columns["LogDate"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss";
            this.dvLog.Columns["EndDate"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss";
        }

        //dt 要导出的DataTable
        //fullFileName CSV的保存路径 例：D:\\1.csv
        public static bool SaveCSV(DataTable dt, string fullFileName)
        {
            bool r = false;
            FileStream fs = new FileStream(fullFileName, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
            string data = "";

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                data += dt.Columns[i].ColumnName.ToString();
                if (i < dt.Columns.Count - 1)
                {
                    data += ",";
                }
            }
            sw.WriteLine(data);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                data = "";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    data += dt.Rows[i][j].ToString().Replace(",", "，");
                    //data = data.Replace(",", "，");
                    if (j < dt.Columns.Count - 1)
                    {
                        data += ",";
                    }
                }
                sw.WriteLine(data);
            }

            sw.Close();
            fs.Close();

            r = true;
            return r;
        }
        //导出CSV
        private void btnExport_Click(object sender, EventArgs e)
        {
            if (this.dvLog.Rows.Count > 0)
            {
                var rlt = this.folderBrowserDialog1.ShowDialog();
                if (rlt == DialogResult.OK)
                {
                    //this.folderBrowserDialog1.SelectedPath
                    var path = $@"{this.folderBrowserDialog1.SelectedPath}\{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv";
                    if(SaveCSV((DataTable)this.dvLog.DataSource, path))
                    {
                        MessageBox.Show("数据导出成功！");
                    }
                }
            }
        }

        private void dvLog_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
