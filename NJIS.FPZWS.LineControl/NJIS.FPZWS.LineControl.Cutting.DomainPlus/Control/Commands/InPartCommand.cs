using NJIS.FPZWS.LineControl.Cutting.ContractPlus;
using NJIS.FPZWS.LineControl.Cutting.Core;
using NJIS.FPZWS.LineControl.Cutting.DomainPlus.Cache;
using NJIS.FPZWS.LineControl.Cutting.DomainPlus.Control.Entitys;
using NJIS.FPZWS.LineControl.Cutting.Message;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;
using NJIS.FPZWS.LineControl.Cutting.ServicePlus;
using NJIS.FPZWS.LineControl.PLC;
using NJIS.FPZWS.Log;
using NJIS.FPZWS.MqttClient;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace NJIS.FPZWS.LineControl.Cutting.DomainPlus.Control.Commands
{
    public class InPartCommand : CommandBase<InPartInputEntity, InPartOutputEntity>
    {

        private readonly ILogger _log = LogManager.GetLogger(typeof(InPartCommand).Name);
 
        private readonly ILineControlCuttingContractPlus _service = new LineControlCuttingServicePlus();// ServiceLocator.Current.GetInstance<ILineControlCuttingContract>();

        public InPartCommand() : base("InPartCommand")
        {
        }

        public override bool LoadInput(IPlcConnector plc)
        {
            //重置输出
            Output.PartId = Input.PartId;
            Output.TriggerOut = Input.TriggerIn;
            Output.BatchName = "Default";
            Output.Length = 0;
            //Output.Place = 0;
            Output.Width = 0;
            Output.PartType = 30;
            Output.Result = 10;
            return base.LoadInput(plc);
        }

        protected override EntityBase Execute(IPlcConnector plc)
        {
            Output.PartId = Input.PartId;
            try
            {
                var pi = PartInfoCache.GetScanPartInfo(Input.PartId);
                if (pi == null)
                {
                    Output.PartType = 40;
                    Output.TriggerOut = Input.TriggerIn;
                    _log.Error($"找不到板件{Input.PartId}");
                    PartScanLog.Current.Add(new CuttingPartScanLog() { PartId = Input.PartId, BatchName = Output.BatchName, InteractionPoints = Input.InteractionPoints, Result = Output.Result, PartType = Output.PartType, Length = Output.Length, Width = Output.Width, Thickness = Output.Thickness });

                    MqttManager.Current.Publish(EmqttSettings.Current.PcsInitQueueRep, new PartInfoQueueArgs
                    {
                        BatchName = pi.BatchName,
                        Length = pi.Length,
                        Width = pi.Width,
                        PcsMessage = "Ng入板",
                        PartId = pi.PartId,
                        PartType = ReenterType.other,
                        Position = Input.Position,
                        Pcs = Output.TriggerOut,
                        Plc = Input.TriggerIn,
                    });

                    return Output;
                }
                else
                {
                    Output.BatchName = pi.BatchName;
                    Output.PartId = pi.PartId;
                    Output.Length = Convert.ToInt32(pi.Length);
                    Output.Width = Convert.ToInt32(pi.Width);
                    Output.Thickness = Convert.ToInt32(pi.Thickness);

                    Output.Result = 10;
                    Output.TriggerOut = Input.TriggerIn;
                    Output.PartType = 30;

                    Core.ReenterType rt = ReenterType.Normal;
                    CuttingPartReenter cpr = new CuttingPartReenter
                    {
                        BatchName = pi.BatchName,
                        Length = Convert.ToDouble(pi.Length),
                        Width = Convert.ToDouble(pi.Width)
                    };

                    rt = pi.PartId.Contains("X") ? ReenterType.Offcut : ReenterType.Normal;
                    cpr.ReenterType = Convert.ToInt32(rt);
                    //cpr.TaskDistributeId = (Guid)partInfo.TaskDistributeId;
                    cpr.PartId = pi.PartId;

                    if (!Input.IsNgRequest)
                    {
                        _service.BulkInsertCuttingPartReenters(new List<CuttingPartReenter>() { cpr });
                        _log.Debug($"板件:{pi.PartId}扫入板成功，批次:{pi.BatchName}");
                    }
                    else
                    {
                        //检查是否是余料
                        if (Regex.IsMatch(pi.PartId, "[a-zA-Z]"))
                        {
                            Output.PartType = 10;
                            string partType = string.Empty;
                            partType = "余料板";
                            //cpr.ReenterType = Convert.ToInt32(ReenterType.Offcut);
                        }
                        else
                        {
                            //抽检器
                            if (CuttingContext.Instance.Sploter != null)
                            {
                                if (CuttingContext.Instance.Sploter.IsSpot(Input.PartId))
                                {
                                    //暂时禁用抽检
                                    //Output.PartType = 20;
                                    Output.PartType = 30;
                                    rt = ReenterType.Spot;
                                    //记录
                                    cpr.ReenterType = Convert.ToInt32(rt);
                                }
                            }
                            string partType = string.Empty;
                            switch (Output.PartType)
                            {
                                case 10: partType = "余料板"; break;
                                case 20: partType = "抽检板"; break;
                                case 30: partType = "正常板"; break;
                                case 40: partType = "找不到板件"; break;
                                default: partType = string.Empty; break;
                            };
                            _log.Debug($"板件:{pi.PartId}NG/抽检请求成功，批次:{pi.BatchName}，板件类型:{partType}");
                        }
                    }

                    MqttManager.Current.Publish(EmqttSettings.Current.PcsInitQueueRep, new PartInfoQueueArgs
                    {
                        BatchName = pi.BatchName,
                        Length = pi.Length,
                        Width = pi.Width,
                        PcsMessage = "扫码入板",
                        PartId = pi.PartId,
                        PartType = rt,
                        Position = Input.Position,
                        Pcs = Output.TriggerOut,
                        Plc = Input.TriggerIn,
                    });


                }
                

            }
            catch (Exception e)
            {
                _log.Error(e.Message);
            }

            PartScanLog.Current.Add(new CuttingPartScanLog() { PartId = Output.PartId, BatchName = Output.BatchName, InteractionPoints = Input.InteractionPoints, Result = Output.Result, PartType = Output.PartType ,Length = Output.Length,Width = Output.Width,Thickness = Output.Thickness});


            return Output;
        }
    }
}
