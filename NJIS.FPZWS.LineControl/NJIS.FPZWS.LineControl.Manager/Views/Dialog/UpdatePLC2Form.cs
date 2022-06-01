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
    public partial class UpdatePLC2Form : Form
    {
        string text;
        int x;
        int y;

        int PilerNumber;
        
        static string PlcIp = ConfigurationSettings.AppSettings["PlcIp"];
        //PlcOperatorHelper plc = new PlcOperatorHelper(PlcIp);
        PlcOperatorHelper plc = PlcOperatorHelper.GetInstance();

        //static ushort sawNoLength = ushort.Parse(ConfigurationSettings.AppSettings["sawNoLength"]);

        LineControlCuttingServicePlus lineControlCuttingServicePlus = new LineControlCuttingServicePlus();

        public UpdatePLC2Form(string controlText, int x, int y)
        {
            this.text = controlText;
            this.x = x;
            this.y = y;

            InitializeComponent();

            if (!plc.CheckConnect())
            {
                plc.Connect(PlcIp);
            }

            PilerNumber = plc.ReadLong(ConfigurationSettings.AppSettings[text+"PilerNumberAddr"]);

            textBoxPilerNumber.Text = PilerNumber.ToString();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = PublicUtils.isNotNumberAndNotDelete(e);
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
            if (!textBoxPilerNumber.Text.Equals(PilerNumber.ToString()))
            {
                PublicUtils.PLCWriteInt(plc, ConfigurationSettings.AppSettings[text + "PilerNumberAddr"],
                    int.Parse(textBoxPilerNumber.Text));
                lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(text, TriggerType.LineControl, "手动" +
                    "修改站台属性值：" + textBoxPilerNumber.Text, LogType.GENERAL));
                this.Close();
            }
        }

        private void UpdatePLC2Form_Load(object sender, EventArgs e)
        {
            this.Text = text;
            this.Location = new Point(x, y);
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
