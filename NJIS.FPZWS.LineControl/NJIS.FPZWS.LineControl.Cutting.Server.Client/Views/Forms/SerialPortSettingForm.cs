using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using IView = NJIS.FPZWS.UI.Common.Message.Extensions.Interfaces.IView;

namespace NJIS.FPZWS.LineControl.Cutting.Server.Client.Views.Forms
{
    public partial class SerialPortSettingForm : RadForm,IView
    {
        public SerialPortSettingForm()
        {
            InitializeComponent();
            cmbBaudRate.DataSource = BaudRateList;
            cmbDataBits.DataSource = DataBitsList;
            cmbParity.DataSource = ParityList;
            cmbStopBits.DataSource = StopBitsList;

            SetCmbListBinding(cmbBaudRate);
            SetCmbListBinding(cmbDataBits);
            SetCmbListBinding(cmbParity);
            SetCmbListBinding(cmbStopBits);

            cmbSerialPortList.SelectedValue = CuttingSerialPortSetting.Current.PortName;
            cmbBaudRate.SelectedValue = CuttingSerialPortSetting.Current.BaudRate;
            cmbDataBits.SelectedValue = CuttingSerialPortSetting.Current.DataBits;
            cmbParity.SelectedValue = CuttingSerialPortSetting.Current.Parity;
            cmbStopBits.SelectedValue = CuttingSerialPortSetting.Current.StopBits;
            switchIsAuto.Value = CuttingSerialPortSetting.Current.IsAuto;
            txtPlcIp.Text = CuttingSerialPortSetting.Current.PlcIp;

        }

        public CuttingSerialPortSetting SerialPortSetting { get; private set; } = null;

        private void SetCmbListBinding(ComboBox cmb)
        {
            cmb.DisplayMember = "Item1";
            cmb.ValueMember = "Item2";
        }

        private List<Tuple<int,int>> BaudRateList = new List<Tuple<int, int>>()
        {
            new Tuple<int,int>(600,600),
            new Tuple<int,int>(1200,1200),
            new Tuple<int,int>(2400,2400),
            new Tuple<int,int>(4800,4800),
            new Tuple<int,int>(9600,9600),
            new Tuple<int,int>(19200,19200),
            new Tuple<int,int>(38400,38400),
            new Tuple<int,int>(43000,43000),
            new Tuple<int,int>(56000,56000),
            new Tuple<int,int>(57600,57600),
            new Tuple<int,int>(115200,115200),
        };

        private List<Tuple<int, int>> DataBitsList = new List<Tuple<int, int>>()
        {
            new Tuple<int,int>(5,5),
            new Tuple<int,int>(6,6),
            new Tuple<int,int>(7,7),
            new Tuple<int,int>(8,8),
        };

        private List<Tuple<string, string>> ParityList = new List<Tuple<string, string>>()
        {
            new Tuple<string,string>("None",Parity.None.ToString()),
            new Tuple<string,string>("Even",Parity.Even.ToString()),
            new Tuple<string,string>("Mark",Parity.Mark.ToString()),
            new Tuple<string,string>("Odd",Parity.Odd.ToString()),
            new Tuple<string,string>("Space",Parity.Space.ToString()),
        };

        private List<Tuple<string, string>> StopBitsList = new List<Tuple<string, string>>()
        {
            new Tuple<string, string>("1", StopBits.One.ToString()),
            new Tuple<string, string>("1.5", StopBits.OnePointFive.ToString()),
            new Tuple<string, string>("2", StopBits.Two.ToString()),
        };

        /// <summary>
        /// 绑定数据
        /// </summary>
        /// <param name="serialNameList"></param>
        public void BindingData(List<string> serialNameList)
        {
            cmbSerialPortList.DataSource = serialNameList;
            if (serialNameList.Count > 0)
            {
                if (serialNameList.Contains(CuttingSerialPortSetting.Current.PortName))
                {
                    cmbSerialPortList.SelectedIndex = serialNameList.FindIndex(s=>s== CuttingSerialPortSetting.Current.PortName);
                }
                else
                {
                    cmbSerialPortList.SelectedIndex = 0;
                }
            }
            
            cmbBaudRate.SelectedValue = CuttingSerialPortSetting.Current.BaudRate;
            cmbDataBits.SelectedValue = CuttingSerialPortSetting.Current.DataBits;
            cmbParity.SelectedValue = CuttingSerialPortSetting.Current.Parity;
            cmbStopBits.SelectedValue = CuttingSerialPortSetting.Current.StopBits;
            switchIsAuto.Value = CuttingSerialPortSetting.Current.IsAuto;
            txtPlcIp.Text = CuttingSerialPortSetting.Current.PlcIp;

        }

        private void btnSure_Click(object sender, EventArgs e)
        {
            try
            {
                string portName = cmbSerialPortList.SelectedValue.ToString().Trim();
                int baudRate = int.Parse(cmbBaudRate.SelectedValue.ToString());
                int dataBits = int.Parse(cmbDataBits.SelectedValue.ToString());
                string parity = cmbParity.SelectedValue.ToString();
                string stopBits = cmbStopBits.SelectedValue.ToString();

                var ret = IPAddress.TryParse(txtPlcIp.Text.Trim(), out IPAddress ipAddress);
                if (!ret)
                {
                    RadMessageBox.Show(this, "Ip地址格式不正确");
                    return;
                }

                SerialPortSetting = new CuttingSerialPortSetting();
                SerialPortSetting.PortName = portName;
                SerialPortSetting.BaudRate = baudRate;
                SerialPortSetting.StopBits = stopBits;
                SerialPortSetting.DataBits = dataBits;
                SerialPortSetting.Parity = parity;
                SerialPortSetting.IsAuto = switchIsAuto.Value;
                SerialPortSetting.PlcIp = txtPlcIp.Text.Trim();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception exception)
            {
                RadMessageBox.Show(this, exception.Message);
                return;
            }

            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
