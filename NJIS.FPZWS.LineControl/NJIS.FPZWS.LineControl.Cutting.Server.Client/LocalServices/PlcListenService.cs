using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NJIS.FPZWS.LineControl.Cutting.Server.Client.Helpers;
using NJIS.FPZWS.LineControl.Cutting.Server.Client.Interfaces;
using NJIS.FPZWS.UI.Common.Message;

namespace NJIS.FPZWS.LineControl.Cutting.Server.Client.LocalServices
{
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
        private PlcOperatorHelper Plc => _Plc ?? (_Plc = PlcOperatorHelper.GetInstance());

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
                    var retConnect = Plc.Connect(CuttingSerialPortSetting.Current.PlcIp);
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
                        //{
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
