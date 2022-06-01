//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Domain
//   文 件 名：PartInfoPositionCommand.cs
//   创建时间：2018-12-04 17:58
//   作    者：
//   说    明：
//   修改时间：2018-12-04 17:58
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

#region

using System;
using System.Linq;
using Microsoft.Practices.ServiceLocation;
using NJIS.FPZWS.LineControl.Drilling.Contract;
using NJIS.FPZWS.LineControl.Drilling.Message;
using NJIS.FPZWS.LineControl.Drilling.Model;
using NJIS.FPZWS.Log;
using NJIS.FPZWS.MqttClient;

#endregion

namespace NJIS.FPZWS.LineControl.Drilling.Domain.Emqtt
{
    /// <summary>
    ///     缓存架初始化命令
    /// </summary>
    public class MachineUpdateCommand : EmqttCommandBase
    {
        private readonly ILogger _log = LogManager.GetLogger(typeof(MachineUpdateCommand).Name);
        private readonly IDrillingContract _service = ServiceLocator.Current.GetInstance<IDrillingContract>();

        public override string SendTopic => EmqttSetting.Current.PcsNgRep;

        public override string ReceiveTopic => EmqttSetting.Current.PcsMachineUpdateReq;

        public override void Execute(MqttMessageBase input)
        {
            if (string.IsNullOrEmpty(input.Data.ToString())) return;

            var entity = input.DeserializeData<MachineArgs>();
            try
            {
                if (!_service.UpdateMachine(new PcsMachine()
                {
                    Code = entity.Code,
                    Status = entity.Status,
                    IsProcessDouble = entity.IsProcessDouble,
                    IsProcessSingle = entity.IsProcessSingle
                }))
                {
                    SendAlarmMsg(new PcsErrorAlarmArgs($"手动调整设备{entity.Code} 状态失败。"));
                    return;
                }
                SendMsg($"手动设备{entity.Code} 状态调整为{entity.Status}->IsProcessDouble:{entity.IsProcessDouble}->IsProcessSingle:{entity.IsProcessSingle}");
            }
            catch (Exception e)
            {
                SendAlarmMsg(new PcsErrorAlarmArgs(e.ToString()));
                _log.Error(e);
            }
        }
    }
}