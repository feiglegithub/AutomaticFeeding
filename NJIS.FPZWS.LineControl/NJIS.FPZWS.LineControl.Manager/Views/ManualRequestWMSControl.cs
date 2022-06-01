using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NJIS.FPZWS.LineControl.Manager.WMSWebReference;
using System.Configuration;

namespace NJIS.FPZWS.LineControl.Manager.Views
{
    public partial class ManualRequestWMSControl : UserControl
    {
        WMSService wmsService;
        
        public ManualRequestWMSControl()
        {
            InitializeComponent();

            wmsService = new WMSService();
        }

        private void keyPressHandled(KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && e.KeyChar != (char)8)
            {
                e.Handled = true;
            }
        }

        private void textBoxRequestCount_KeyPress(object sender, KeyPressEventArgs e)
        {
            keyPressHandled(e);
        }

        private void textBoxPilerNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            keyPressHandled(e);
        }

        private void textBoxReturnCount_KeyPress(object sender, KeyPressEventArgs e)
        {
            keyPressHandled(e);
        }

        private void radButtonOutAction_Click(object sender, EventArgs e)
        {
            //向WMS要料
            WMSInParams wmsInParams = new WMSInParams();
            wmsInParams.ReqType = 5;//要料
            wmsInParams.ProductCode = comboBoxOutProductCode.Text;
            wmsInParams.Count = int.Parse(textBoxRequestCount.Text);
            wmsInParams.ToStation = int.Parse(comboBoxOutStation.Text);

            ResultMsg resultMsg = wmsService.ApplyWMSTask(wmsInParams);

            textBoxOutResult.Text = resultMsg.Message;
        }

        private void radButtonInAction_Click(object sender, EventArgs e)
        {
            //向WMS请求余料回库
            WMSInParams wmsInParams = new WMSInParams();
            wmsInParams.ReqType = 4;//余料回库
            wmsInParams.ProductCode = comboBoxReturnProductCode.Text;
            wmsInParams.Count = int.Parse(textBoxReturnCount.Text);
            wmsInParams.FromStation = int.Parse(comboBoxReturnStation.Text);
            wmsInParams.PilerNo = int.Parse(textBoxPilerNo.Text);

            ResultMsg resultMsg = wmsService.ApplyWMSTask(wmsInParams);

            textBoxReturnResult.Text = resultMsg.Message;
        }

        private void ManualRequestWMSControl_Load(object sender, EventArgs e)
        {
            string productCode = ConfigurationManager.AppSettings["productCode"].Replace("\r\n","");
            productCode = productCode.Replace(" ", "");
            string[] productCodes = productCode.Split(',');

            if (!string.IsNullOrEmpty(productCode) && productCodes.Length < 1)
                productCodes[0] = productCode;

            comboBoxOutProductCode.Items.AddRange(productCodes);
            comboBoxReturnProductCode.Items.AddRange(productCodes);
        }
    }
}
