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
    public class SerialListenService : ServiceBase<SerialListenService>
    {
        /// <summary>
        /// 串口扫码数据 -- 广播消息标签
        /// </summary>
        public const string BroadcastBindingPartId = nameof(BroadcastBindingPartId);
        /// <summary>
        /// 回流串口消息 -- 广播消息标签
        /// </summary>
        public const string BroadcastBindingSerialMsg = nameof(BroadcastBindingSerialMsg);

        private SerialPortHelper _serialPortHelper = null;
        private SerialPortHelper SerialHelper =>
            _serialPortHelper ?? (_serialPortHelper = SerialPortHelper.GetInstance());

        private SerialListenService() { }

        private Thread thread = null;
        private Thread Th => thread ?? (thread = new Thread(ReadPartId) { IsBackground = true });

        public bool CanStart { get; set; } = false;

        public void Start()
        {
            Th.Start();
        }

        public void Stop()
        {
            Th.Abort();
        }


        private void ReadPartId()
        {
            bool portExists = true;
            bool isSendNormal = false;
            while (true)
            {

                if (CuttingSerialPortSetting.Current.IsAuto && CanStart)
                {
                    if (SerialHelper.GetPortNames().Contains(SerialHelper.CurSerialPortName))
                    {
                        portExists = true;
                        var partId = string.Empty;
                        try
                        {
                            partId = SerialHelper.ReadData();
                        }
                        catch (Exception e)
                        {
                            BroadcastMessage.Send(BroadcastBindingSerialMsg, "串口：" + SerialHelper.CurSerialPortName + e.Message);
                            portExists = false;
                            isSendNormal = false;
                        }
                        
                        if (!string.IsNullOrWhiteSpace(partId))
                        {
                            SerialHelper.ClearBuffer();
                            BroadcastMessage.Send(BroadcastBindingPartId, partId);

                        }

                        if (!isSendNormal)
                        {
                            BroadcastMessage.Send(BroadcastBindingSerialMsg, "串口：" + SerialHelper.CurSerialPortName + "通讯正常！");
                            isSendNormal = true;
                        }
                    }
                    else
                    {
                        if (portExists)
                        {
                            BroadcastMessage.Send(BroadcastBindingSerialMsg, "串口：" + SerialHelper.CurSerialPortName + "不存在，请检查串口设置！");
                            portExists = false;
                            isSendNormal = false;
                        }
                    }

                }

                Thread.Sleep(1000);
            }
        }
    }
}
