using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NJIS.FPZWS.UI.Common.Message.Extensions;
using NJIS.FPZWS.UI.Common.Message.Extensions.Interfaces;
using Telerik.WinControls;

namespace NJIS.FPZWS.LineControl.Cutting.Simulator
{
    public partial class ReadPlcControl : UserControl//,IView
    {
        public const string ReceivePartId = nameof(ReceivePartId);
        private PlcOperator Plc => plcOperator ?? (plcOperator = PlcOperator.GetInstance()); 

        private PlcOperator plcOperator = null;
        //private readonly MainFormPresenter _presenter = MainFormPresenter.GetInstance();
        public ReadPlcControl()
        {
            InitializeComponent();
            cmbDataTpye.SelectedIndex = 0;
            //this.RegisterTipsMessage();
            //this.Register<string>(ReceivePartId,ExecuteBindingPartId);
            //this.BindingPresenter(_presenter);
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
                else if(value==1)
                {
                    cmbDataTpye.SelectedIndex = 1;
                }
            }
        }

        public string AddrText
        {
            get => txtDB.Text;
            set => txtDB.Text = value;
        }

        public new string Text
        {
            get => txtContent.Text;
            set => txtContent.Text = value;
        }

        public new Color ForeColor
        {
            get => btnReadPartId.ForeColor;
            set => btnReadPartId.ForeColor = value;
        }

        public string ButtonText
        {
            get => btnReadPartId.Text;
            set => btnReadPartId.Text = value;
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

        private void btnReadPartId_Click(object sender, EventArgs e)
        {
            string addr;

            if (!CheckAddr(out addr))
            {
                RadMessageBox.Show("DB块地址格式不对");
            }

            txtContent.Text = cmbDataTpye.SelectedIndex == 0 ? Plc.ReadString("DB20.1", 20) : Plc.ReadLong(addr).ToString();
        }
    }
}
