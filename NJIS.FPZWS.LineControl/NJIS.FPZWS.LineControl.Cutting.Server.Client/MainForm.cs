using NJIS.FPZWS.LineControl.Cutting.Model;
using NJIS.FPZWS.LineControl.Cutting.Server.Client.Presenters;
using NJIS.FPZWS.UI.Common.Message.Extensions;
using NJIS.FPZWS.UI.Common.Message.Extensions.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NJIS.FPZWS.LineControl.Cutting.Server.Client.LocalServices;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace NJIS.FPZWS.LineControl.Cutting.Server.Client
{
    public partial class MainForm : Telerik.WinControls.UI.RadForm
    {
        public MainForm()
        {
            InitializeComponent();
            this.Shown += MainForm_Shown;
            this.Disposed += MainForm_Disposed;
        }

        private void MainForm_Disposed(object sender, EventArgs e)
        {
            MqttMessage.UnSubscribeMqttMessage();
            SerialListenService.GetInstance().Stop();
            PlcListenService.GetInstance().Stop();
            CurTaskListenService.GetInstance().Stop();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            MqttMessage.SubscribeMqttMessage();
            SerialListenService.GetInstance().Start();
            PlcListenService.GetInstance().Start();
            CurTaskListenService.GetInstance().Start();
        }
    }
}
