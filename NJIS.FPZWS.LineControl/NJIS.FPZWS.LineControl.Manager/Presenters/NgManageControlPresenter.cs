using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using NJIS.FPZWS.LineControl.Cutting.ContractPlus;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;
using NJIS.FPZWS.LineControl.Cutting.ServicePlus;
using NJIS.FPZWS.LineControl.Manager.Helpers;
using NJIS.FPZWS.LineControl.Manager.LocalServices;
using NJIS.FPZWS.LineControl.Manager.Utils;
using NJIS.FPZWS.LineControl.Manager.ViewModels;
using NJIS.FPZWS.UI.Common.Message;
using NJIS.FPZWS.Wcf.Client;

namespace NJIS.FPZWS.LineControl.Manager.Presenters
{
    public class NgManageControlPresenter:PresenterBase
    {
        LineControlCuttingServicePlus lineControlCuttingServicePlus = new LineControlCuttingServicePlus();

        public const string GetData = nameof(GetData);

        public const string BindingData = nameof(BindingData);

        public const string BindingSerialMsg = nameof(BindingSerialMsg);

        public const string BindingPlcMsg = nameof(BindingPlcMsg);

        public const string UpdatedSerialSetting = nameof(UpdatedSerialSetting);

        public const string RequestReenter = nameof(RequestReenter);

        public const string BindingRequestResult = nameof(BindingRequestResult);

        public const string BeginPlcListen = nameof(BeginPlcListen);
        public const string BeginSerialListen = nameof(BeginSerialListen);

        //public const string Write = nameof(Write);

        private int plcRequest = 0;
        private int plcResponse = 0;

        private PlcOperatorHelper _Plc = null;
        //private PlcOperatorHelper Plc => _Plc ?? (_Plc = PlcOperatorHelper.GetInstance());
        //private PlcOperatorHelper Plc => _Plc ?? (_Plc = new PlcOperatorHelper(ConfigurationSettings.AppSettings["NGPlcIp"]));
        static string ip = ConfigurationSettings.AppSettings["NGPlcIp"];
        PlcOperatorHelper Plc = new PlcOperatorHelper(ip);

        private SerialPortHelper _serialPortHelper = null;
        private SerialPortHelper SerialHelper =>
            _serialPortHelper ?? (_serialPortHelper = SerialPortHelper.GetInstance());

        private ILineControlCuttingContractPlus _contract = null;

        //private ILineControlCuttingContractPlus Contract =>
        //    _contract ?? (_contract =WcfClient.GetProxy<ILineControlCuttingContractPlus>());
        private LineControlCuttingServicePlus Contract = new LineControlCuttingServicePlus();

        public NgManageControlPresenter()
        {
            Register();
        }

        private void ExecutePartReenter(object sender,string partId)
        {
            if (!Plc.CheckConnect())
            {
                Plc.Connect(ip);
            }

            if (plcRequest > plcResponse)
            {
                    var partDetail = GetPartDetailByPartId(partId);
                    if (partDetail == null)
                    {
                        SendTipsMessage($"找不到板件{partId}", sender);
                        Send(BindingData, sender, false);
                        return;
                    }

                    Send(BindingData, partDetail);
                    bool writeSuccess = false;
                    while (!writeSuccess)
                    {
                        writeSuccess = true;

                        Plc.Write(CuttingSerialPortSetting.Current.BatchNameDbAddr, partDetail.BatchName, 28);
                        string BatchName = Plc.ReadString(CuttingSerialPortSetting.Current.BatchNameDbAddr, 28);
                        writeSuccess = partDetail.BatchName == BatchName;
                        if (!writeSuccess) continue;
                        //{
                        //    lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog("NG", TriggerType.LineControl, $"回馈批次号失败" +
                        //        $"({CuttingSerialPortSetting.Current.BatchNameDbAddr})={partDetail.BatchName}", LogType.ABNORMAL));
                        //    continue;
                        //}
                        //else
                        //{
                        //    lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog("NG", TriggerType.LineControl, "回馈批次号成功" +
                        //        $"({CuttingSerialPortSetting.Current.BatchNameDbAddr})={partDetail.BatchName}", LogType.GENERAL));
                        //}

                        Plc.Write(CuttingSerialPortSetting.Current.PartIdDbAddr, partDetail.PartId, 18);
                        writeSuccess &= partDetail.PartId == Plc.ReadString(CuttingSerialPortSetting.Current.PartIdDbAddr, 18);
                        if (!writeSuccess) continue;


                        if (partDetail.PartId.Contains("X"))
                        {
                            Plc.Write(CuttingSerialPortSetting.Current.ResultDbAddr, 20);
                            writeSuccess &= 20 == Plc.ReadLong(CuttingSerialPortSetting.Current.ResultDbAddr);
                            if (!writeSuccess) continue;
                        }
                        //else if (partDetail.IsNg)
                        //{
                        //    Plc.Write(CuttingSerialPortSetting.Current.ResultDbAddr, 20);
                        //    writeSuccess &= 20 == Plc.ReadLong(CuttingSerialPortSetting.Current.ResultDbAddr);
                        //    if (!writeSuccess) continue;
                        //}
                        else
                        {
                            Plc.Write(CuttingSerialPortSetting.Current.ResultDbAddr, 10);
                            writeSuccess &= 10 == Plc.ReadLong(CuttingSerialPortSetting.Current.ResultDbAddr);
                            if (!writeSuccess) continue;
                        }

                        Plc.Write(CuttingSerialPortSetting.Current.TriggerOutDbAddr, plcRequest);
                        writeSuccess &= plcRequest == Plc.ReadLong(CuttingSerialPortSetting.Current.TriggerOutDbAddr);
                    }
                    Send(BindingData, sender, true);    
            }
        }

        private void Register()
        {
            Register<string>(GetData,GetPartDetail);
            //同步执行获取端口信息
            Register<int>(GetData, ExecuteGetPortNames, EExecutionMode.Synchronization);
            Register<CuttingSerialPortSetting>(UpdatedSerialSetting,(setting)=>
            {
                CuttingSerialPortSetting.Current.IsAuto = setting.IsAuto;
                CuttingSerialPortSetting.Current.PortName = setting.PortName;
                CuttingSerialPortSetting.Current.StopBits = setting.StopBits;
                CuttingSerialPortSetting.Current.Parity = setting.Parity;
                CuttingSerialPortSetting.Current.BaudRate = setting.BaudRate;
                CuttingSerialPortSetting.Current.DataBits = setting.DataBits;
                CuttingSerialPortSetting.Current.PlcIp = setting.PlcIp;
                CuttingSerialPortSetting.Current.Save();
                SerialHelper.UpdateSerialSettings(setting);
            });

            Register<string>(SerialListenService.BroadcastBindingPartId, (partId) =>
            {
                Send(BindingData, partId);
                var partInfos = Contract.GetPartFeedBacksByPartId(partId);
                if (partInfos.Count == 0)
                {
                    SendTipsMessage($"找不到板件{partId}");
                    Send(BindingData, false);
                    return;
                }
                PartFeedBack partInfo = partInfos[0];
                Send(BindingData, partInfo);

            }, EExecutionMode.Asynchronization, true);
            Register<string>(SerialListenService.BroadcastBindingSerialMsg, (s) => Send(BindingSerialMsg, s), EExecutionMode.Asynchronization, true);

            Register<Tuple<int,int>>(PlcListenService.BroadcastBindingData, (tuple) =>
            {
                plcRequest = tuple.Item1;
                plcResponse = tuple.Item2;
                Send(BindingData, tuple);
            }, EExecutionMode.Asynchronization,true);
            Register<string>(PlcListenService.BroadcastBindingPlcMsg,(s)=> Send(BindingPlcMsg, s), EExecutionMode.Asynchronization, true);
            this.Register<string>(RequestReenter, ExecutePartReenter);

            //Register<string>(Write, s =>
            //{
            //    var ret = Plc.Write(CuttingSerialPortSetting.Current.BatchNameDbAddr, s);
            //});

        }

        

        private void ExecuteGetPortNames(object sender,int disableValue=0)
        {
            
            try
            {
                var portNames = SerialHelper.GetPortNames();
                Send(BindingData, sender, portNames);
            }
            catch (Exception e)
            {
                SendTipsMessage(e.Message, sender);
                Send(BindingData, sender, new List<string>());
            }

        }

        private PartFeedBack GetPartDetailByPartId(string partId)
        {
            bool isOffPart = false;
            var partDetails = Contract.GetPartFeedBacksByPartId(partId);
            //var partDetails = feedBacks.ConvertAll(item => new PatternDetail
            //{
            //    BatchName = item.BatchName, DeviceName = item.DeviceName, Color = "", IsNg = false, Width = item.Width,Length = item.Length,PartId = partId
            //});
            //var partDetails = LineControlCuttingContract.GetBatchTaskDetailsByPartId(partId);
            var partDetail = partDetails.FirstOrDefault(item => item.PartId == partId);
            isOffPart = partId.Contains("X");
            if (partDetail == null && !isOffPart)
            {
                var partInfos = Contract.GetPartFeedBacksByPartId(partId);
                if (partInfos.Count == 0)
                {
                    return null;
                }

                partDetail = partInfos.FirstOrDefault(item => !string.IsNullOrWhiteSpace(item.BatchName));

                //partDetail = new PatternDetail
                //{
                //    BatchName = partInfo?.BatchName,
                //    Color = partInfo?.Color,
                //    IsOffPart = false,
                //    IsNg = false,
                //    Width = partInfo?.Width,
                //    Length = partInfo?.Length,
                //    DeviceName = "",
                //    PartId = partId
                //};

            }

            //partDetail = partDetail ?? new PatternDetail
            //{
            //    BatchName = "Default",
            //    IsOffPart = isOffPart,
            //    Color = "",
            //    Width = 0,
            //    Length = 0,
            //    DeviceName = "",
            //    PartId = partId,
            //    IsNg = false
            //};
            return partDetail;
        }

        private void GetPartDetail(object sender, string partId)
        {
            try
            {
                var partDetail = GetPartDetailByPartId(partId);
                if (partDetail == null)
                {
                    SendTipsMessage($"找不到板件{partId}", sender);
                }
                Send(BindingData, sender, partDetail);
            }
            catch (Exception e)
            {
                SendTipsMessage(e.Message, sender);
                Send(BindingData, sender, (PatternDetail)null);
            }
        }
    }
}
