using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WCS.DataBase;

namespace WCS.Forms
{
    public partial class StationManaFrm : Form
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
        public StationManaFrm()
        {
            InitializeComponent();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            var type = "";
            var state = "";
            if (this.cboSType.Text == "入口")
            {
                type = "I";
            }
            else if (this.cboSType.Text == "出口")
            {
                type = "O";
            }

            if (this.cboStatus.Text == "启用")
            {
                state = "1";
            }
            else if (this.cboStatus.Text == "禁用")
            {
                state = "0";
            }

            var dt = WcsSqlB.GetStationInfo(type, state);
            this.dvTasks.DataSource = dt;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dic.Count > 0)
            {
                WcsSqlB.UpdateStationInfo(dic);
                dic.Clear();

                btnSelect_Click(sender, e);
            }
        }

        private void dvTasks_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) { return; }
            var row = this.dvTasks.Rows[e.RowIndex];
            var wno = row.Cells["WMSNo"].Value.ToString();
            var buffer = row.Cells["BufferC"].Value.ToString();
            int b;
            if (int.TryParse(buffer, out b))
            {
                if (b > int.Parse(row.Cells["BufferM"].Value.ToString()))
                {
                    MessageBox.Show("设置的缓存数量不能超出限制数量！");
                    return;
                }
                else if (b <= 0)
                {
                    MessageBox.Show("请输入大于0的整数！");
                    return;
                }
            }
            else
            {
                MessageBox.Show("请输入大于0的整数！");
                return;
            }

            var state = row.Cells["State"].Value.ToString() == "True" ? "1" : "0";
            var remark = row.Cells["Remark"].Value.ToString();
            if (dic.ContainsKey(wno))
            {
                dic[wno] = $"{buffer}|{state}|{remark}";
            }
            else
            {
                dic.Add(wno, $"{buffer}|{state}|{remark}");
            }
        }
    }
}
