using System;
using System.Configuration;
using System.Threading;
using NJIS.FPZWS.LineControl.Manager.Helpers;
using NJIS.FPZWS.LineControl.Manager.Interfaces;
using NJIS.FPZWS.UI.Common.Message;

namespace NJIS.FPZWS.LineControl.Manager.LocalServices
{
    /// <summary>
    /// PLC监听
    /// </summary>
    public class PlcListenService: ServiceBase<PlcListenService>
    {
        /// <summary>
        /// 回流Plc请求与回应数据 -- 广播消息标签
        /// </summary>
        public const string BroadcastBindingData = nameof(BroadcastBindingData);
        /// <summary>
        /// 回流Plc消息 -- 广播消息标签
        /// </summary>
        public const string BroadcastBindingPlcMsg = nameof(BroadcastBindingPlcMsg);

        private PlcOperatorHelper _Plc = null;
        string ip = ConfigurationSettings.AppSettings["NGPlcIp"];
        private PlcOperatorHelper Plc => _Plc ?? (_Plc = new PlcOperatorHelper(ip));

        private Thread thread = null;
        private Thread Th => thread ?? (thread = new Thread(ReadPlc) {IsBackground = true});

        public bool CanStart { get; set; } = false;

        private PlcListenService() { }
        public void Start()
        {
            if (!IsStart)
            {
                Th.Start();
                base.Start();
            }
        }

        public void Stop()
        {
            if (IsStart)
            {
                Th.Abort();
                base.Stop();
            }
            
        }

        private void ReadPlc()
        {
            int triggerIn = 0;
            int triggerOut = 0;
            bool isSendNormal = false;
            while (true)
            {
                if (!Plc.CheckConnect() && CanStart)
                {
                    var retConnect = Plc.Connect(ip);
                    if (retConnect)
                    {
                        try
                        {
                            var triIn = Plc.ReadLong(CuttingSerialPortSetting.Current.TriggerInDbAddr);
                            var triOut = Plc.ReadLong(CuttingSerialPortSetting.Current.TriggerOutDbAddr);
                            BroadcastMessage.Send(BroadcastBindingData, new Tuple<int, int>(triIn, triOut));
                            //if (triggerIn != triIn || triggerOut != triOut)
                            //{
                            //    triggerIn = triIn;
                            //    triggerOut = triOut;
                            //    BroadcastMessage.Send(BroadcastBindingData, new Tuple<int, int>(triggerIn, triggerOut));
                            //}
                            if (!isSendNormal)
                            {
                                BroadcastMessage.Send(BroadcastBindingPlcMsg, $"Plc（{Plc.Siemens.IpAddress}）通讯正常!");
                                isSendNormal = true;
                            }
                        }
                        catch (Exception e)
                        {
                            BroadcastMessage.Send(BroadcastBindingPlcMsg, $"Plc（{Plc.Siemens.IpAddress}）通讯异常:" + e.Message);
                            isSendNormal = false;
                        }

                    }
                    else
                    {
                        BroadcastMessage.Send(BroadcastBindingPlcMsg, $"通讯异常：无法连接 Plc（{Plc.Siemens.IpAddress}）");
                        isSendNormal = false;
                    }
                }
                else
                {
                    try
                    {
                        var triIn = Plc.ReadLong(CuttingSerialPortSetting.Current.TriggerInDbAddr);
                        var triOut = Plc.ReadLong(CuttingSerialPortSetting.Current.TriggerOutDbAddr);
                        BroadcastMessage.Send(BroadcastBindingData, new Tuple<int, int>(triIn, triOut));
                        //if (triggerIn != triIn || triggerOut != triOut)
                        //{D:\99_信息化\source\trunk\NJIS.FPZWS.LineControl\NJIS.FPZWS.LineControl.Manager\Views\SAWFileView.csD:\99_信息化\source\trunk\NJIS.FPZWS.LineControl\NJIS.FPZWS.LineControl.Manager\Views\SAWFileView.cs
                        //    triggerIn = triIn;
                        //    triggerOut = triOut;
                        //    BroadcastMessage.Send(BroadcastBindingData, new Tuple<int, int>(triggerIn, triggerOut));
                        //}
                        BroadcastMessage.Send(BroadcastBindingPlcMsg, $"Plc（{Plc.Siemens.IpAddress}）通讯正常!");
                        //if (!isSendNormal)
                        //{
                        //    BroadcastMessage.Send(BroadcastBindingPlcMsg, $"Plc（{Plc.Siemens.IpAddress}）通讯正常!");
                        //    isSendNormal = true;
                        //}
                    }
                    catch (Exception e)
                    {
                        BroadcastMessage.Send(BroadcastBindingPlcMsg, $"Plc（{Plc.Siemens.IpAddress}）通讯异常:" + e.Message);
                        isSendNormal = false;
                    }
                }

                Thread.Sleep(1000);
            }
        }
    }
}
