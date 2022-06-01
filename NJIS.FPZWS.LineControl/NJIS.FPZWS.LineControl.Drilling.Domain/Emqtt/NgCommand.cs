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
    public class NgCommand : EmqttCommandBase
    {
        private readonly ILogger _log = LogManager.GetLogger(typeof(NgCommand).Name);
        private readonly IDrillingContract _service = ServiceLocator.Current.GetInstance<IDrillingContract>();

        public override string SendTopic => EmqttSetting.Current.PcsNgRep;

        public override string ReceiveTopic => EmqttSetting.Current.PcsNgReq;

        public override void Execute(MqttMessageBase input)
        {
            if (string.IsNullOrEmpty(input.Data.ToString())) return;

            var entity = input.DeserializeData<PcsNgArgs>();
            try
            {
                var ng = _service.FindNg(entity.PartId);
                if (ng != null)
                {
                    SendAlarmMsg(new PcsErrorAlarmArgs($"板件{entity.PartId}已存在,状态{ng.Status}，请勿重复插入"));
                    return;
                }

                var drilling = _service.FindDrilling(entity.PartId);
                if (drilling == null)
                {
                    SendAlarmMsg(new PcsErrorAlarmArgs($"板件{entity.PartId}不存在，不能重入。"));
                    return;
                }
                _service.InsertNg(new PcsNg()
                {
                    CreatedTime = entity.CreatedTime,
                    Msg = entity.Msg,
                    PartId = entity.PartId,
                    Status = 0,
                    UpdatedTime = entity.UpdatedTime
                });

                var dts = _service.FindNgs(20);
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