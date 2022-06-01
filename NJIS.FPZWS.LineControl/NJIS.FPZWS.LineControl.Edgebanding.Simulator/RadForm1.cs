//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Edgebanding
//   项目名称：NJIS.FPZWS.LineControl.Edgebandingg.Monitor
//   文 件 名：RadForm1.cs
//   创建时间：2018-12-13 17:15
//   作    者：
//   说    明：
//   修改时间：2018-12-13 17:15
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using NJIS.FPZWS.LineControl.Edgebanding.Service;
using NJIS.PLC.Communication.Profinet.Siemens;
using Telerik.WinControls.UI;
using NJIS.FPZWS.UI.Common;

namespace NJIS.FPZWS.LineControl.Edgebanding.Simulator
{
    public partial class RadForm1 : RadForm
    {
        private readonly SiemensS7Net _siemensTcpNet;
        private bool IsConnect { get; set; }

        public RadForm1(SiemensPLCS siemensPlcs)
        {
            InitializeComponent();
            _siemensTcpNet = new SiemensS7Net(siemensPlcs);
        }

        private void WriteMsg(string msg)
        {
            txtMsg.InvokeExecute(() => { txtMsg.Text += $"{DateTime.Now.ToLongTimeString()}：{msg}\r\n"; });
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            IPAddress address;
            if (!IPAddress.TryParse(txtIpAddress.Text, out address))
            {
                WriteMsg("Ip地址输入不正确！");
                return;
            }
            
            _siemensTcpNet.IpAddress = txtIpAddress.Text;

            try
            {
                var connect = _siemensTcpNet.ConnectServer();
                if (connect.IsSuccess)
                {
                    IsConnect = true;
                    WriteMsg("连接成功！");
                    btnDisConnect.Enabled = true;
                    btnConnect.Enabled = false;
                }
                else
                {
                    WriteMsg("连接失败！");
                }
            }
            catch (Exception ex)
            {
                WriteMsg(ex.Message);
            }
        }

        private void btnDisConnect_Click(object sender, EventArgs e)
        {
            _siemensTcpNet.ConnectClose();
            IsConnect = false;
            btnDisConnect.Enabled = false;
            btnConnect.Enabled = true;
        }

        private void btnInPart10_Click(object sender, EventArgs e)
        {
            if (!IsConnect)
            {
                WriteMsg("未连接PLC！");
                return;
            }
            RequestTrigger("DB1.30000", "DB1.30004", "DB1.30010", txtPartId10.Text);
        }


        private void RequestTrigger(string triggerInAddress, string triggerOutAddress, string dataAddress, string rtb)
        {
            if (!IsConnect)
            {
                WriteMsg("未连接PLC！");
                return;
            }

            var gid = Guid.NewGuid().ToString();
            WriteMsg("++++++++++++++++++++++++++++++++++++++++++++++++++");
            var s = _siemensTcpNet.ReadInt32(triggerInAddress);
            if (s.IsSuccess)
            {
                WriteMsg($"读取变量[{triggerInAddress}]=>[{s.Content}],[{s.IsSuccess}]");

                var s1 = _siemensTcpNet.Write(dataAddress, rtb.Trim());
                WriteMsg($"写入变量[{dataAddress}]=>[{rtb.Trim()}],[{s1.IsSuccess}]");


                var s3 = _siemensTcpNet.Write(triggerOutAddress, s.Content);
                WriteMsg($"写入变量[{triggerOutAddress}]=>[{s.Content}],[{s3.IsSuccess}]");

                var v = s.Content + 1;
                var s2 = _siemensTcpNet.Write(triggerInAddress, v);
                WriteMsg($"写入变量[{triggerInAddress}]=>[{v}],[{s2.IsSuccess}]");
            }
            WriteMsg("++++++++++++++++++++++++++++++++++++++++++++++++++");
        }

        private List<string> PartIds { get; set; }

        private void radButton1_Click(object sender, EventArgs e)
        {
            if (!IsConnect)
            {
                WriteMsg("未连接PLC！");
                return;
            }
            if (radButton1.Text == "自动触发")
            {
                var ser = new EdgebandingService();
                PartIds = ser.FindPartIds(DateTime.Now.AddDays(-60));
            }

            if (PartIds.Count == 0)
            {

                WriteMsg("获取不到板件数据...");
                return;
            }
            WriteMsg($"板件数量{PartIds.Count}...");
            radButton1.Text = radButton1.Text == "自动触发" ? "自动触发.." : "自动触发";

            Task.Factory.StartNew(() =>
            {
                while (radButton1.Text == "自动触发.." && PartIds.Count > 0)
                {
                    Random rd = new Random();
                    var c = rd.Next(3000, 10000);
                    if (!IsConnect)
                    {
                        WriteMsg("未连接PLC！");
                    }
                    else
                    {
                        var partId = PartIds[0];
                        txtPartId10.Invoke(new Action(() =>
                        {
                            txtPartId10.Text = partId;
                        }));

                        RequestTrigger("DB1.30000", "DB1.30004", "DB1.30010", partId);

                        PartIds.RemoveAt(0);
                    }
                    System.Threading.Thread.Sleep(c);
                }
            });
        }

        private void RadForm1_Load(object sender, EventArgs e)
        {

        }
    }
}