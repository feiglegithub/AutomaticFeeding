using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using NJIS.FPZWS.LineControl.Manager.Helpers;
using System.Threading;
using NJIS.FPZWS.LineControl.Manager.Views.Dialog;

namespace NJIS.FPZWS.LineControl.Manager.Views
{
    public partial class DeviceStatusMonitorControl : UserControl
    {


        static string PlcIp;
        static string sleepTime;
        static ushort sawNoLength;

        double scaleX = 1;
        double scaleY = 1;

        float controlWidth;
        float controlHeight;

        ControlCollection controlCollection;

        //PlcOperatorHelper plc = new PlcOperatorHelper(PlcIp);
        PlcOperatorHelper plc = PlcOperatorHelper.GetInstance();
        //PlcOperatorHelper plc;

        Thread refreshStatusThread;

        public DeviceStatusMonitorControl()
        {
            InitializeComponent();
        }

        private void refreshStatus()
        {
            while (true)
            {
                //Thread.Sleep(int.Parse(sleepTime));
                Thread.Sleep(100);

                bool flag = plc.CheckConnect();
                if (flag)
                {
                    labelHeartbeat.Invoke(new Action(delegate
                    {
                        labelHeartbeat.Text = plc.ReadBool(ConfigurationSettings.AppSettings["heartbeatAddr"]).ToString();
                    }));

                    foreach (Control item in controlCollection)
                    {
                        //item.Invoke(new Action(delegate { item.Dispose(); }));
                        //item.Invoke(new Action(delegate () {
                        Type type = item.GetType();
                        if (type.Name.Equals("Button"))
                            item.BackColor = plc.ReadLong(ConfigurationSettings.AppSettings[item.Text]) == 0 ? Color.Gainsboro : Color.Lime;
                        //}));
                    }
                    //buttonDS401.BackColor = plc.ReadLong(Attribute1AddrDS401) == 0 ? Color.Gainsboro : Color.Lime;
                    //buttonDS402.BackColor = plc.ReadLong(Attribute1AddrDS402) == 0 ? Color.Gainsboro : Color.Lime;
                    //buttonDS403.BackColor = plc.ReadLong(Attribute1AddrDS403) == 0 ? Color.Gainsboro : Color.Lime;
                    //buttonDS404.BackColor = plc.ReadLong(Attribute1AddrDS404) == 0 ? Color.Gainsboro : Color.Lime;
                    //buttonDS405.BackColor = plc.ReadLong(Attribute1AddrDS405) == 0 ? Color.Gainsboro : Color.Lime;
                    //buttonDS406.BackColor = plc.ReadLong(Attribute1AddrDS406) == 0 ? Color.Gainsboro : Color.Lime;
                    //buttonDS407.BackColor = plc.ReadLong(Attribute1AddrDS407) == 0 ? Color.Gainsboro : Color.Lime;
                    //buttonDS408.BackColor = plc.ReadLong(Attribute1AddrDS408) == 0 ? Color.Gainsboro : Color.Lime;
                    //buttonDS409.BackColor = plc.ReadLong(Attribute1AddrDS409) == 0 ? Color.Gainsboro : Color.Lime;
                    //buttonFB401.BackColor = plc.ReadLong(Attribute1AddrFB401) == 0 ? Color.Gainsboro : Color.Lime;
                    //buttonFB402.BackColor = plc.ReadLong(Attribute1AddrFB402) == 0 ? Color.Gainsboro : Color.Lime;
                    //buttonGT401.BackColor = plc.ReadLong(Attribute1AddrGT401) == 0 ? Color.Gainsboro : Color.Lime;
                    //buttonGT402.BackColor = plc.ReadLong(Attribute1AddrGT402) == 0 ? Color.Gainsboro : Color.Lime;
                    //buttonGT403.BackColor = plc.ReadLong(Attribute1AddrGT403) == 0 ? Color.Gainsboro : Color.Lime;
                    //buttonGT404.BackColor = plc.ReadLong(Attribute1AddrGT404) == 0 ? Color.Gainsboro : Color.Lime;
                    //buttonGT405.BackColor = plc.ReadLong(Attribute1AddrGT405) == 0 ? Color.Gainsboro : Color.Lime;
                    //buttonGT406.BackColor = plc.ReadLong(Attribute1AddrGT406) == 0 ? Color.Gainsboro : Color.Lime;
                    //buttonGT407.BackColor = plc.ReadLong(Attribute1AddrGT407) == 0 ? Color.Gainsboro : Color.Lime;
                    //buttonGT408.BackColor = plc.ReadLong(Attribute1AddrGT408) == 0 ? Color.Gainsboro : Color.Lime;
                    //buttonGT409.BackColor = plc.ReadLong(Attribute1AddrGT409) == 0 ? Color.Gainsboro : Color.Lime;
                    //buttonGT410.BackColor = plc.ReadLong(Attribute1AddrGT410) == 0 ? Color.Gainsboro : Color.Lime;
                    //buttonGT411.BackColor = plc.ReadLong(Attribute1AddrGT411) == 0 ? Color.Gainsboro : Color.Lime;
                    //buttonGT412.BackColor = plc.ReadLong(Attribute1AddrGT412) == 0 ? Color.Gainsboro : Color.Lime;
                    //buttonGT413.BackColor = plc.ReadLong(Attribute1AddrGT413) == 0 ? Color.Gainsboro : Color.Lime;
                    //buttonGT414.BackColor = plc.ReadLong(Attribute1AddrGT414) == 0 ? Color.Gainsboro : Color.Lime;
                    //buttonGT415.BackColor = plc.ReadLong(Attribute1AddrGT415) == 0 ? Color.Gainsboro : Color.Lime;
                    //buttonGT416.BackColor = plc.ReadLong(Attribute1AddrGT416) == 0 ? Color.Gainsboro : Color.Lime;
                    //buttonGT417.BackColor = plc.ReadLong(Attribute1AddrGT417) == 0 ? Color.Gainsboro : Color.Lime;
                    //buttonGT418.BackColor = plc.ReadLong(Attribute1AddrGT418) == 0 ? Color.Gainsboro : Color.Lime;
                    //buttonGT419.BackColor = plc.ReadLong(Attribute1AddrGT419) == 0 ? Color.Gainsboro : Color.Lime;
                    //buttonGT420.BackColor = plc.ReadLong(Attribute1AddrGT420) == 0 ? Color.Gainsboro : Color.Lime;
                    //buttonGT421.BackColor = plc.ReadLong(Attribute1AddrGT421) == 0 ? Color.Gainsboro : Color.Lime;
                    //buttonLT401.BackColor = plc.ReadLong(Attribute1AddrLT401) == 0 ? Color.Gainsboro : Color.Lime;
                    //buttonSB401.BackColor = plc.ReadLong(Attribute1AddrSB401) == 0 ? Color.Gainsboro : Color.Lime;

                }
                else
                {
                    plc.Connect(PlcIp);
                }

            }

        }

        private void bindClickEvent()
        {
            foreach (Control item in controlCollection)
            {
                Type type = item.GetType();
                if (type.Name.Equals("Button"))
                    item.Click += Item_Click;
            }
        }

        private void Item_Click(object sender, EventArgs e)
        {
            Button mButton = (Button)sender;
            string text = mButton.Text;
            int tag = int.Parse(mButton.Tag.ToString());


            switch (tag)
            {
                case 4:
                    //int Type = plc.ReadLong(ConfigurationSettings.AppSettings[text + "TypeAddr"]);
                    //string SawNumber = plc.ReadString(ConfigurationSettings.AppSettings[text + "SawNumberAddr"], sawNoLength);
                    //int SumCount = plc.ReadLong(ConfigurationSettings.AppSettings[text + "SumCountAddr"]);

                    UpdatePLC4Form updatePLC4Form = new UpdatePLC4Form(text, mButton.Location.X,
                        mButton.Location.Y + mButton.Height * 2);
                    updatePLC4Form.ShowDialog();
                    break;
                case 2:
                    if (text.Equals("SB401"))
                    {
                        UpdatePLC2SB401Form updatePLC2SB401Form = new UpdatePLC2SB401Form(text, mButton.Location.X - mButton.Width / 2,
                        mButton.Location.Y + mButton.Height * 2);
                        updatePLC2SB401Form.ShowDialog();
                    }
                    else
                    {
                        UpdatePLC2Form updatePLC2Form = new UpdatePLC2Form(text, mButton.Location.X,
                        mButton.Location.Y + mButton.Height * 2);
                        updatePLC2Form.ShowDialog();
                    }
                    break;
                case 3:
                    UpdatePLC3Form updatePLC3Form = new UpdatePLC3Form(text, mButton.Location.X,
                        mButton.Location.Y + mButton.Height * 2);
                    updatePLC3Form.ShowDialog();
                    break;
                default:
                    break;
            }
        }

        private void bindMouseEnter()
        {
            foreach (Control item in controlCollection)
            {
                Type type = item.GetType();
                if (type.Name.Equals("Button"))
                    item.MouseEnter += Item_MouseEnter;
            }
        }

        private void Item_MouseEnter(object sender, EventArgs e)
        {
            Button mButton = (Button)sender;
            string text = mButton.Text;
            int tag = int.Parse(mButton.Tag.ToString());
            string tip = "";

            switch (tag)
            {
                case 4:
                    int PilerNumber = plc.ReadLong(ConfigurationSettings.AppSettings[text + "PilerNumberAddr"]);
                    int Target = plc.ReadLong(ConfigurationSettings.AppSettings[text + "TargetAddr"]);
                    int SumCount = plc.ReadLong(ConfigurationSettings.AppSettings[text + "SumCountAddr"]);
                    //int CarryPanel = plc.ReadLong(ConfigurationSettings.AppSettings[text + "CarryPanelAddr"]);

                    tip = $"PilerNumber:{PilerNumber}\rTarget:{Target}\rSumCount:{SumCount}";
                    break;
                case 2:
                    if (text.Equals("SB401"))
                    {
                        int SumCount2 = plc.ReadLong(ConfigurationSettings.AppSettings[text + "SumCountAddr"]);
                        tip = $"SumCount:{SumCount2}";
                    }
                    else
                    {
                        //int Type = plc.ReadLong(ConfigurationSettings.AppSettings[text + "TypeAddr"]);
                        //string SawNumber = plc.ReadString(ConfigurationSettings.AppSettings[text + "SawNumberAddr"], sawNoLength);
                        //int SumCount2 = plc.ReadLong(ConfigurationSettings.AppSettings[text + "SumCountAddr"]);

                        int PilerNumber2 = plc.ReadLong(ConfigurationSettings.AppSettings[text + "PilerNumberAddr"]);
                        tip = $"PilerNumber:{PilerNumber2}";
                    }
                    break;
                case 3:
                    int SumCount3 = plc.ReadLong(ConfigurationSettings.AppSettings[text + "SumCountAddr"]);

                    tip = $"SumCount:{SumCount3}";
                    break;
                default:
                    break;
            }

            //toolTip1.Show(tip, mButton);
            label1.Visible = true;
            label1.Text = tip;
            label1.Location = new Point(mButton.Location.X + 30, mButton.Location.Y - 30);
        }

        private void bindMouseHover()
        {
            foreach (Control item in controlCollection)
            {
                Type type = item.GetType();
                if (type.Name.Equals("Button"))
                    item.MouseHover += Item_MouseHover;
            }
        }

        private void Item_MouseHover(object sender, EventArgs e)
        {
            Button mButton = (Button)sender;
            string text = mButton.Text;
            int tag = int.Parse(mButton.Tag.ToString());
            string tip = "";

            switch (tag)
            {
                case 4:
                    int Type = plc.ReadLong(ConfigurationSettings.AppSettings[text + "TypeAddr"]);
                    string SawNumber = plc.ReadString(ConfigurationSettings.AppSettings[text + "SawNumberAddr"], sawNoLength);
                    int SumCount = plc.ReadLong(ConfigurationSettings.AppSettings[text + "SumCountAddr"]);
                    //int CarryPanel = plc.ReadLong(ConfigurationSettings.AppSettings[text + "CarryPanelAddr"]);

                    tip = $"Type:{Type}\rSawNumber:{SawNumber}\rSumCount:{SumCount}";
                    break;
                case 2:
                    if (text.Equals("SB401"))
                    {
                        int SumCount2 = plc.ReadLong(ConfigurationSettings.AppSettings[text + "SumCountAddr"]);
                        tip = $"SumCount:{SumCount2}";
                    }
                    else
                    {
                        //int Type = plc.ReadLong(ConfigurationSettings.AppSettings[text + "TypeAddr"]);
                        //string SawNumber = plc.ReadString(ConfigurationSettings.AppSettings[text + "SawNumberAddr"], sawNoLength);
                        //int SumCount2 = plc.ReadLong(ConfigurationSettings.AppSettings[text + "SumCountAddr"]);

                        int PilerNumber2 = plc.ReadLong(ConfigurationSettings.AppSettings[text + "PilerNumberAddr"]);
                        tip = $"PilerNumber:{PilerNumber2}";
                    }
                    break;
                case 3:
                    string SawNumber2 = plc.ReadString(ConfigurationSettings.AppSettings[text + "SawNumberAddr"], sawNoLength);
                    int SumCount3 = plc.ReadLong(ConfigurationSettings.AppSettings[text + "SumCountAddr"]);

                    tip = $"SawNumber:{SawNumber2}\rSumCount:{SumCount3}";
                    break;
                default:
                    break;
            }

            //toolTip1.Show(tip, mButton);
            label1.Visible = true;
            label1.Text = tip;
            label1.Location = new Point(mButton.Location.X, mButton.Location.Y + mButton.Height);
        }

        private void bindMouseLeave()
        {
            foreach (Control item in controlCollection)
            {
                Type type = item.GetType();
                if (type.Name.Equals("Button"))
                    item.MouseLeave += Item_MouseLeave;
            }
        }

        private void Item_MouseLeave(object sender, EventArgs e)
        {
            label1.Visible = false;
        }

        private void DeviceStatusMonitorControl_Load(object sender, EventArgs e)
        {
            PlcIp = ConfigurationSettings.AppSettings["PlcIp"];
            sleepTime = ConfigurationSettings.AppSettings["sleepTime"];
            sawNoLength = ushort.Parse(ConfigurationSettings.AppSettings["sawNoLength"]);

            controlCollection = this.Controls;

            refreshStatusThread = new Thread(new ThreadStart(refreshStatus));
            refreshStatusThread.Start();

            bindClickEvent();
            //bindMouseEnter();
            bindMouseHover();
            bindMouseLeave();

            this.Disposed += DeviceStatusMonitorControl_Disposed;
        }

        private void DeviceStatusMonitorControl_Disposed(object sender, EventArgs e)
        {
            refreshStatusThread.Abort();
        }

        private void DeviceStatusMonitorControl_Resize(object sender, EventArgs e)
        {
            //Control mControl = (Control)sender;
            //int controlWidth2 = mControl.Width;
            //int controlHeight2 = mControl.Height;

            //if (controlWidth2 < 1 || controlHeight2 < 1)
            //{
            //    return;
            //}

            //int width = (int)(controlWidth2 * 0.09);
            //int height = (int)(controlHeight2 * 0.07);

            //int width2 = (int)(controlWidth2 * 0.04);
            //int height2 = (int)(controlHeight2 * 0.06);

            //int width3 = (int)(controlWidth2 * 0.07);

            //if (controlWidth > 0)
            //{
            //    scaleX = controlWidth2 / controlWidth;
            //    scaleY = controlHeight2 / controlHeight;

            //    controlWidth = mControl.Width;
            //    controlHeight = mControl.Height;
            //}

            //foreach (var item in controlCollection)
            //{
            //    if (item is Button button)
            //    {
            //        if (button.Text.Contains("滚筒") && !button.Text.Equals("FB401_滚筒") && !button.Text.Equals("FB402_滚筒"))
            //        {
            //            button.Width = width2;
            //            button.Height = height2;
            //            button.Location = new Point((int)(button.Location.X * scaleX), (int)(button.Location.Y * scaleY));
            //        }
            //        else if (button.Text.Equals("SB401"))
            //        {
            //            button.Width = width3;
            //            button.Height = height;
            //            button.Location = new Point((int)(button.Location.X * scaleX), (int)(button.Location.Y * scaleY));
            //        }
            //        else
            //        {
            //            button.Width = width;
            //            button.Height = height;
            //            button.Location = new Point((int)(button.Location.X * scaleX), (int)(button.Location.Y * scaleY));
            //        }

            //    }
            //}
        }

        private void DeviceStatusMonitorControl_VisibleChanged(object sender, EventArgs e)
        {
            //controlWidth = this.Width;
            //controlHeight = this.Height;
        }
    }
}
