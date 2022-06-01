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
    public class NgInitCommand : EmqttCommandBase
    {
        private readonly ILogger _log = LogManager.GetLogger(typeof(NgInitCommand).Name);
        private readonly IDrillingContract _service = ServiceLocator.Current.GetInstance<IDrillingContract>();

        public override string SendTopic => EmqttSetting.Current.PcsNgRep;

        public override string ReceiveTopic => EmqttSetting.Current.PcsNgInitReq;

        public override void Execute(MqttMessageBase input)
        {
            try
            {
                var dts = _service.FindNgs(100);
                var datas = (from ps in dts
                    select new PcsNgArgs()
                    {
                        PartId = ps.PartId,
                        CreatedTime = ps.CreatedTime,
                        UpdatedTime = ps.CreatedTime,
                        Status = ps.Status,
                        Msg = ps.Msg
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