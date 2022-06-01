using NJIS.FPZWS.LineControl.PLC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;
using NJIS.FPZWS.LineControl.Cutting.Contract;
using NJIS.FPZWS.LineControl.Cutting.Core;
using NJIS.FPZWS.LineControl.Cutting.Domain.Cache;
using NJIS.FPZWS.LineControl.Cutting.Domain.Control.Entitys;
using NJIS.FPZWS.Log;
using NJIS.FPZWS.MqttClient;
using NJIS.FPZWS.LineControl.Cutting.Message;
using NJIS.FPZWS.LineControl.Cutting.Message.AlarmArgs;
using NJIS.FPZWS.LineControl.Cutting.Model;
using NJIS.FPZWS.LineControl.Cutting.Service;
using System.Text.RegularExpressions;

namespace NJIS.FPZWS.LineControl.Cutting.Domain.Control.Commands
{
    public class InPartCommand : CommandBase<InPartInputEntity, InPartOutputEntity>
    {

        private readonly ILogger _log = LogManager.GetLogger(typeof(InPartCommand).Name);
        private readonly PartInfoService partInfoService = new PartInfoService();
        private readonly ILineControlCuttingContract _service = new LineControlCuttingService();// ServiceLocator.Current.GetInstance<ILineControlCuttingContract>();

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
                var pi = PartInfoCache.GetCuttingFeedBack(Input.PartId);
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

                    ReenterType rt = ReenterType.Normal;
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
                //var pi = CuttingContext.Instance.InParter.InPart(Input.PartId, Input.Position);
                //if (pi != null)
                //{
                //    Output.BatchName = pi.BatchName;
                //    Output.PartId = pi.PartId;
                //    Output.Length = Convert.ToInt32(pi.Length);
                //    Output.Width = Convert.ToInt32(pi.Width);
                //    Output.Thickness = Convert.ToInt32(pi.Thickness);

                //    Output.Result = 10;
                //    Output.TriggerOut = Input.TriggerIn;
                //    Output.PartType = 30;
                //    //var partInfo = PartInfoCache.GetCuttingTaskDetail(Input.PartId);

                //    if (pi != null)
                //    {
                //        //Output.Result = 10;
                //        ReenterType rt = ReenterType.Normal;
                //        CuttingPartReenter cpr = new CuttingPartReenter
                //        {
                //            BatchName = pi.BatchName,
                //            Length = Convert.ToDouble(pi.Length),
                //            Width = Convert.ToDouble(pi.Width)
                //        };

                //        rt = pi.PartId.Contains("X") ? ReenterType.Offcut : ReenterType.Normal;
                //        cpr.ReenterType = Convert.ToInt32(rt);
                //        //cpr.TaskDistributeId = (Guid)partInfo.TaskDistributeId;
                //        cpr.PartId = pi.PartId;

                //        if (!Input.IsNgRequest)
                //        {
                //            _service.BulkInsertCuttingPartReenters(new List<CuttingPartReenter>() { cpr });
                //            _log.Debug($"板件:{pi.PartId}扫入板成功，批次:{pi.BatchName}");
                //        }
                //        else
                //        {
                //            //检查是否是余料
                //            if (Regex.IsMatch(pi.PartId, "[a-zA-Z]"))
                //            {
                //                Output.PartType = 10;
                //                string partType = string.Empty;
                //                partType = "余料板";
                //                //cpr.ReenterType = Convert.ToInt32(ReenterType.Offcut);
                //            }
                //            else
                //            {
                //                //抽检器
                //                if (CuttingContext.Instance.Sploter != null)
                //                {
                //                    if (CuttingContext.Instance.Sploter.IsSpot(Input.PartId))
                //                    {
                //                        //暂时禁用抽检
                //                        //Output.PartType = 20;
                //                        Output.PartType = 30;
                //                        rt = ReenterType.Spot;
                //                        //记录
                //                        cpr.ReenterType = Convert.ToInt32(rt);
                //                    }
                //                }
                //                string partType = string.Empty;
                //                switch (Output.PartType)
                //                {
                //                    case 10: partType = "余料板"; break;
                //                    case 20: partType = "抽检板"; break;
                //                    case 30: partType = "正常板"; break;
                //                    case 40: partType = "找不到板件"; break;
                //                    default: partType = string.Empty; break;
                //                };
                //                _log.Debug($"板件:{pi.PartId}NG/抽检请求成功，批次:{pi.BatchName}，板件类型:{partType}");
                //            }
                //        }

                //        MqttManager.Current.Publish(EmqttSettings.Current.PcsInitQueueRep, new PartInfoQueueArgs
                //        {
                //            BatchName = pi.BatchName,
                //            Length = pi.Length,
                //            Width = pi.Width,
                //            PcsMessage = "扫码入板",
                //            PartId = pi.PartId,
                //            PartType = rt,
                //            Position = Input.Position,
                //            Pcs = Output.TriggerOut,
                //            Plc = Input.TriggerIn,
                //        });
                //    }

                //}
                //else
                //{

                //    //检查是否是余料
                //    if (Regex.IsMatch(Input.PartId, "[a-zA-Z]"))
                //    //if (Input.PartId.Contains("XHB"))
                //    {
                //        Output.PartType = 10;
                //        string partType = string.Empty;
                //        partType = "余料板";
                //        Output.BatchName = "Default";
                //        Output.Result = 10;
                //        Output.PartId = Input.PartId;
                //        Output.TriggerOut = Input.TriggerIn;
                //        PartScanLog.Current.Add(new CuttingPartScanLog() { PartId = Output.PartId, BatchName = Output.BatchName, InteractionPoints = Input.InteractionPoints, Result = Output.Result, PartType = Output.PartType });
                //        return Output;
                //    }
                //    PartInfo partInfo = PartInfoCache.GetScanPartInfo(Input.PartId);
                //    if (partInfo != null)
                //    {
                //        Output.PartId = Input.PartId;
                //        Output.BatchName = partInfo.BatchCode;
                //        Output.PartType = 30;

                //        Output.Result = 10;
                //        Output.TriggerOut = Input.TriggerIn;
                //        _log.Debug($"无法在开料数据中找到{Input.PartId}，在数据中心检索到该板件");
                //        if (!Input.IsNgRequest)
                //        {
                //            //_service.BulkInsertCuttingPartReenters(new List<CuttingPartReenter>() { cpr });
                //            _log.Debug($"板件:{partInfo.PartId}扫入板成功，批次:{partInfo.BatchCode}");
                //        }
                //        else
                //        {
                //            //检查是否是余料
                //            //if (Regex.IsMatch(partInfo.PartId, "[a-zA-Z]"))
                //            if (Input.PartId.Contains("XHB"))
                //            {
                //                Output.PartType = 10;
                //                string partType = string.Empty;
                //                partType = "余料板";
                //                //cpr.ReenterType = Convert.ToInt32(ReenterType.Offcut);
                //            }
                //            else
                //            {
                //                //抽检器
                //                if (CuttingContext.Instance.Sploter != null)
                //                {
                //                    if (CuttingContext.Instance.Sploter.IsSpot(Input.PartId))
                //                    {
                //                        Output.PartType = 30;
                //                        //rt = ReenterType.Spot;
                //                        ////记录
                //                        //cpr.ReenterType = Convert.ToInt32(rt);
                //                    }
                //                }
                //                string partType = string.Empty;
                //                switch (Output.PartType)
                //                {
                //                    case 10:
                //                        partType = "余料板";
                //                        break;
                //                    case 20:
                //                        partType = "抽检板";
                //                        break;
                //                    case 30:
                //                        partType = "正常板";
                //                        break;
                //                    default:
                //                        partType = string.Empty;
                //                        break;
                //                }

                //                _log.Debug($"板件:{partInfo.PartId}NG/抽检请求成功，批次:{partInfo.BatchCode}，板件类型:{partType}");
                //            }
                //        }

                //    }
                //    else
                //    {
                //        MqttManager.Current.Publish(EmqttSettings.Current.PcsAlarmRep,
                //            new PcsLogicAlarmArgs($"找不到板件{Input.PartId}"));
                //        Output.BatchName = "Default";
                //        Output.PartType = 20;
                //        //找不到板件，停止
                //        Output.Result = 10;
                //        Output.PartId = Input.PartId;
                //        Output.TriggerOut = Input.TriggerIn;
                //        _log.Debug($"找不到板件{Input.PartId}");
                //    }

                //}

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
