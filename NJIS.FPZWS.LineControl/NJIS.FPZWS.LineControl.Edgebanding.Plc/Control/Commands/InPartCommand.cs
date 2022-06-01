//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Edgebanding
//   项目名称：NJIS.FPZWS.LineControl.Edgebanding.Plc
//   文 件 名：InPartCommand.cs
//   创建时间：2018-12-13 16:06
//   作    者：
//   说    明：
//   修改时间：2018-12-13 16:06
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;
using Microsoft.Practices.ServiceLocation;
using NJIS.FPZWS.LineControl.Edgebanding.Contract;
using NJIS.FPZWS.LineControl.Edgebanding.Emqtt;
using NJIS.FPZWS.LineControl.Edgebanding.Model;
using NJIS.FPZWS.LineControl.Edgebanding.Plc.Control.Entitys;
using NJIS.FPZWS.LineControl.PLC;
using NJIS.FPZWS.Log;
using NJIS.FPZWS.MqttClient;

namespace NJIS.FPZWS.LineControl.Edgebanding.Plc.Control.Commands
{
    public class InPartCommand : EdgebandingCommandBase<InPartInputEntity, InPartOutputEntity>
    {
        private readonly IEdgebandingContract _iEdgebandingContract =
            ServiceLocator.Current.GetInstance<IEdgebandingContract>();

        private readonly ILogger _log = LogManager.GetLogger(typeof(InPartCommand).Name);

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
            Output.Width = 0;
            Output.Thickness = 0;
            Output.Res = 0;

            return base.LoadInput(plc);
        }

        public override EntityBase ExecuteCommand(IPlcConnector plc)
        {
            try
            {
                var pi = _iEdgebandingContract.FindEdgebanding(m => m.BarCode == Input.PartId);
                if (pi != null)
                {
                    Output.Res = 10;
                    Output.BatchName = pi.BatchName;
                    Output.Length = (int)pi.Length;
                    Output.PartId = pi.BarCode;
                    Output.Thickness = (int)pi.Thickness;
                    Output.Width = (int)pi.Width;
                    Output.TriggerOut = Input.TriggerIn;

                    _iEdgebandingContract.InsertEdgebandQueue(new PcsEdgebandQueue()
                    {
                        CreatedTime = DateTime.Now,
                        PartId = Input.PartId,
                        TriggerIn = Input.TriggerIn,
                        TriggerOut = Input.TriggerOut
                    });


                    MqttManager.Current.Publish(EmqttSetting.Current.PcsInitQueueRep, new PartInfoQueueArgs
                    {
                        BatchName = pi.BatchName,
                        OrderNumber = pi.OrderNumber,
                        Length = (int)pi.Length,
                        Width = (int)pi.Width,
                        PcsMessage = "入板扫码",
                        PartId = pi.BarCode,
                        Pcs = Output.TriggerIn,
                        Plc = Input.TriggerIn,
                        C1_CORNER = pi.C1_CORNER,
                        C1_EDGE = pi.C1_EDGE,
                        C1_EDGECODE = pi.C1_EDGECODE,
                        C1_FORMAT = pi.C1_FORMAT,
                        C1_GROOVE = pi.C1_GROOVE,
                        C2_CORNER = pi.C2_CORNER,
                        C2_EDGE = pi.C2_EDGE,
                        C2_EDGECODE = pi.C2_EDGECODE,
                        C2_FORMAT = pi.C2_FORMAT,

                        L1_CORNER = pi.L1_CORNER,
                        L1_EDGE = pi.L1_EDGE,
                        L1_EDGECODE = pi.L1_EDGECODE,
                        L1_FORMAT = pi.L1_FORMAT,
                        L1_GROOVE = pi.L1_GROOVE,
                        L2_CORNER = pi.L2_CORNER,
                        L2_EDGE = pi.L2_EDGE,
                        L2_EDGECODE = pi.L2_EDGECODE,
                        L2_FORMAT = pi.L2_FORMAT
                    });
                }
                else
                {
                    Output.TriggerOut = Input.TriggerIn;
                    var msg = $"找不到板件{Input.PartId}";
                    _log.Info(msg);
                    MqttManager.Current.Publish(EmqttSetting.Current.PcsAlarmRep, new PcsLogicAlarmArgs(msg));
                }
            }
            catch (Exception e)
            {
                MqttManager.Current.Publish(EmqttSetting.Current.PcsAlarmRep, new PcsErrorAlarmArgs(e.ToString()));
                _log.Error(e);
                Output.Res = 10;
            }

            return base.ExecuteCommand(plc);
        }
    }
}