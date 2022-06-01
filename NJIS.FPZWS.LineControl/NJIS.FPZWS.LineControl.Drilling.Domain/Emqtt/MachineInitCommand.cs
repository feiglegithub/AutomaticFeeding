//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Domain
//   文 件 名：MachineInitCommand.cs
//   创建时间：2019-08-19 8:59
//   作    者：
//   说    明：
//   修改时间：2019-08-19 8:59
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;
using System.Linq;
using Microsoft.Practices.ServiceLocation;
using NJIS.FPZWS.LineControl.Drilling.Contract;
using NJIS.FPZWS.LineControl.Drilling.Message;
using NJIS.FPZWS.Log;
using NJIS.FPZWS.MqttClient;

namespace NJIS.FPZWS.LineControl.Drilling.Domain.Emqtt
{
    public class MachineInitCommand : EmqttCommandBase
    {
        private readonly ILogger _log = LogManager.GetLogger(typeof(NgCommand).Name);
        private readonly IDrillingContract _service = ServiceLocator.Current.GetInstance<IDrillingContract>();

        public override string SendTopic => EmqttSetting.Current.PcsMachineInitRep;

        public override string ReceiveTopic => EmqttSetting.Current.PcsMachineInitReq;

        public override void Execute(MqttMessageBase input)
        {
            try
            {
                var dts = _service.FindAllMachine();
                var datas = (from ps in dts
                    select new MachineArgs
                    {
                        Code = ps.Code,
                        SN = ps.SN,
                        Status = ps.Status.GetHashCode(),
                        Name = ps.Name   ,
                        IsProcessDouble=ps.IsProcessDouble,
                        IsProcessSingle=ps.IsProcessSingle
                    }).ToList();
                MqttManager.Current.Publish($"{SendTopic}", datas);
            }
            catch (Exception e)
            {
                SendAlarmMsg(new PcsErrorAlarmArgs(e.ToString()));
                _log.Error(e);
            }
        }
    }
}