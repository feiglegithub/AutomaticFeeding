using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls;

namespace NJIS.FPZWS.LineControl.Cutting.Simulator
{
    public partial class WritePlcNewControl : UserControl
    {
        private PlcOperator Plc => plcOperator ?? (plcOperator = PlcOperator.GetInstance());

        private PlcOperator plcOperator = null;

        //private PlcOperator Plc = PlcOperator.Current;
        //private readonly MainFormPresenter _presenter = MainFormPresenter.GetInstance();
        public WritePlcNewControl()
        {
            InitializeComponent();
            cmbDataTpye.SelectedIndex = 0;
            //this.RegisterTipsMessage();
            //this.Register<string>(ReceivePartId,ExecuteBindingPartId);
            //this.BindingPresenter(_presenter);
        }

        public string AddrText
        {
            get => txtDB.Text;
            set => txtDB.Text = value;
        }

        public int DefaultType
        {
            get => cmbDataTpye.SelectedIndex;
            set
            {
                if (value == 0)
                {
                    cmbDataTpye.SelectedIndex = 0;
                }
                else if (value == 1)
                {
                    cmbDataTpye.SelectedIndex = 1;
                }
            }
        }

        public new string Text
        {
            get => txtContent.Text;
            set => txtContent.Text = value;
        }

        public new Color ForeColor
        {
            get => btnWrite.ForeColor;
            set => btnWrite.ForeColor = value;
        }

        public string ButtonText
        {
            get => btnWrite.Text;
            set => btnWrite.Text = value;
        }

        //private void ExecuteBindingPartId(string partId)
        //{
        //    this.txtPartId.Text = partId;
        //}

        private bool CheckAddr(out string addr)
        {
            try
            {
                int db = Convert.ToInt32(txtDB.Text.Trim());
                int offset = Convert.ToInt32(txtOffset.Text.Trim());
                addr = "DB" + db + "." + offset;
                return true;
            }
            catch (Exception e)
            {
                addr = "";
                return false;
            }

        }


        private void btnWrite_Click(object sender, EventArgs e)
        {
            string addr;
            if (!CheckAddr(out addr))
            {
                RadMessageBox.Show("DB块地址不对");
                return;
            }

            if (cmbDataTpye.SelectedIndex == 0)
            {
                string content = txtContent.Text.Trim();
                Plc.Write(addr, content, 20);
            }
            else
            {
                int value = Convert.ToInt32(txtContent.Text.Trim());
                Plc.Write(addr, value);
            }
            txtContent.Text = cmbDataTpye.SelectedIndex == 0 ? Plc.ReadString(addr, 20) : Plc.ReadLong(addr).ToString();
        }
    
    }
}
