//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Domain
//   文 件 名：InPartCommand.cs
//   创建时间：2018-11-22 8:45
//   作    者：
//   说    明：
//   修改时间：2018-11-22 8:45
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

#region

using System;
using Microsoft.Practices.ServiceLocation;
using NJIS.FPZWS.LineControl.Drilling.Contract;
using NJIS.FPZWS.LineControl.Drilling.Core;
using NJIS.FPZWS.LineControl.Drilling.Domain.Control.Entitys;
using NJIS.FPZWS.LineControl.Drilling.Message;
using NJIS.FPZWS.LineControl.PLC;
using NJIS.FPZWS.Log;
using NJIS.FPZWS.MqttClient;

#endregion

namespace NJIS.FPZWS.LineControl.Drilling.Domain.Control.Commands
{
    /// <summary>
    ///     操作结果反馈
    /// </summary>
    public class InPartCommand : DrillingCommandBase<InPartInputEntity, InPartOutputEntity>
    {
        private readonly ILogger _log = LogManager.GetLogger(typeof(InPartCommand).Name);
        private readonly IDrillingContract _service = ServiceLocator.Current.GetInstance<IDrillingContract>();

        public InPartCommand() : base("InPartCommand")
        {
        }

        public override bool LoadInput(IPlcConnector plc)
        {
            //重置输出
            Output.PartId = "";
            Output.TriggerOut = Input.TriggerOut;
            Output.BatchName = "";
            Output.Length = 0;
            Output.Place = 0;
            Output.Width = 0;
            Output.IsNg = 10;
            Output.Res = 100;
            Output.DrillingRoute = 0;

            return base.LoadInput(plc);
        }

        protected override EntityBase Execute(IPlcConnector plc)
        {
            try
            {
                if (DrillingContext.Instance.Sploter != null)
                {
                    var isSplot = DrillingContext.Instance.Sploter.IsSplot(Input.PartId);
                    if (isSplot)
                    {
                        Output.IsNg = 20;
                    }
                }

                var pi = DrillingContext.Instance.InParter.InPart(Input.PartId, Input.Position);

                if (pi != null)
                {
                    Output.PartId = pi.PartId;
                    Output.BatchName = pi.BatchName;
                    // 10:      不打孔，20：单面孔，30：双面孔
                    Output.DrillingRoute = int.Parse(pi.DrillingRoute);
                    Output.Res = 10;
                    Output.Thickness = pi.Thickness;
                    Output.TriggerOut = Input.TriggerIn;
                    Output.IsNg = pi.IsNg ? 20 : 10;
                    //Output.
                    Output.Width = pi.Width;
                    Output.Length = pi.Length;

                    // 1转角；2不转角
                    Output.Rotation = pi.Rotation ? 1 : 2;
                    MqttManager.Current.Publish(EmqttSetting.Current.PcsInitQueueRep, new PartInfoQueueArgs
                    {
                        BatchName = pi.BatchName,
                        OrderNumber = pi.OrderNumber,
                        DrillingRouting = pi.DrillingRoute,
                        Length = pi.Length,
                        Width = pi.Width,
                        PcsMessage = pi.PcsMessage,
                        PartId = pi.PartId,
                        Pcs = Output.TriggerOut,
                        Plc = Input.TriggerIn,
                        Place = pi.Place,
                        Status = pi.Status,
                        IsNg = Output.IsNg,
                        NextPlace = pi.NextPlace
                    });
                }
                else
                {
                    MqttManager.Current.Publish(EmqttSetting.Current.PcsAlarmRep,
                        new PcsLogicAlarmArgs($"找不到板件{Input.PartId}"));
                    //找不到板件
                    Output.Res = 20;
                    Output.TriggerOut = Input.TriggerIn;
                }
            }
            catch (Exception e)
            {
                MqttManager.Current.Publish(EmqttSetting.Current.PcsAlarmRep, new PcsErrorAlarmArgs(e.ToString()));
                _log.Error(e);
                Output.Res = 100;
            }

            return Output;
        }
    }
}