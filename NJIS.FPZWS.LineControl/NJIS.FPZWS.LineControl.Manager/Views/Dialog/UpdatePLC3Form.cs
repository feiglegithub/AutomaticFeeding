using NJIS.FPZWS.LineControl.Cutting.ModelPlus;
using NJIS.FPZWS.LineControl.Cutting.ServicePlus;
using NJIS.FPZWS.LineControl.Manager.Helpers;
using NJIS.FPZWS.LineControl.Manager.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NJIS.FPZWS.LineControl.Manager.Views.Dialog
{
    public partial class UpdatePLC3Form : Form
    {
        string text;
        int x;
        int y;
        
        static string PlcIp = ConfigurationSettings.AppSettings["PlcIp"];
        //PlcOperatorHelper plc = new PlcOperatorHelper(PlcIp);
        PlcOperatorHelper plc = PlcOperatorHelper.GetInstance();
        static ushort sawNoLength = ushort.Parse(ConfigurationSettings.AppSettings["sawNoLength"]);

        LineControlCuttingServicePlus lineControlCuttingServicePlus = new LineControlCuttingServicePlus();

        string SawNumber;
        int SumCount;

        public UpdatePLC3Form(string controlText, int x, int y)
        {
            this.text = controlText;
            this.x = x;
            this.y = y;

            InitializeComponent();

            if (!plc.CheckConnect())
                plc.Connect(PlcIp);

            SawNumber = plc.ReadString(ConfigurationSettings.AppSettings[text + "SawNumberAddr"], sawNoLength);
            SumCount = plc.ReadLong(ConfigurationSettings.AppSettings[text + "SumCountAddr"]);

            textBoxSawNumber.Text = SawNumber;
            textBoxSumCount.Text = SumCount.ToString();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (plc.CheckConnect())
                writePlc();
            else
            {
                plc.Connect(PlcIp);
                if (plc.CheckConnect())
                    writePlc();
                else
                    MessageBox.Show("无法连接PLC");
            }
        }

        private void writePlc()
        {
            string str = null;
            if (!textBoxSawNumber.Text.Equals(SawNumber))
            {
                PublicUtils.PLCWriteString(plc, ConfigurationSettings.AppSettings[text + "SawNumberAddr"],
                    textBoxSawNumber.Text, sawNoLength);
                str += "SawNumber:" + textBoxSawNumber.Text;
            }
            if (!textBoxSumCount.Text.Equals(SumCount.ToString()))
            {
                PublicUtils.PLCWriteInt(plc, ConfigurationSettings.AppSettings[text + "SumCountAddr"],
                    int.Parse(textBoxSumCount.Text));
                str += " SumCount:" + textBoxSumCount.Text;
            }


            if (!string.IsNullOrEmpty(str))
                lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(text, TriggerType.LineControl, "手动" +
                    "修改站台属性值：" + str, LogType.GENERAL));
            this.Close();
        }

        private void textBoxSumCount_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = PublicUtils.isNotNumberAndNotDelete(e);
        }

        private void UpdatePLC3Form_Load(object sender, EventArgs e)
        {
            this.Text = text;
            this.Location = new Point(x, y);
        }
    }
}
