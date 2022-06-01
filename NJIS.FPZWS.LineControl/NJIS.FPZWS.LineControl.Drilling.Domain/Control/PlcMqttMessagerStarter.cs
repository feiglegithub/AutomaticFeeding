//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Domain
//   文 件 名：PlcMqttMessagerStarter.cs
//   创建时间：2018-11-29 8:25
//   作    者：
//   说    明：
//   修改时间：2018-11-29 8:25
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using NJIS.FPZWS.Common.Initialize;
using NJIS.FPZWS.Log;

namespace NJIS.FPZWS.LineControl.Drilling.Domain.Control
{
    public class PlcMqttMessagerStarter : IModularStarter
    {
        private readonly ILogger _logger = LogManager.GetLogger(typeof(PlcMqttMessagerStarter));

        public void Start()
        {
            _logger.Info("启动Emqtt 消息消息发送器");
            //MessageManager.AddMessagerAdapter(new EmqttMessagerAdapter());
        }

        public void Stop()
        {
            _logger.Info("停止Emqtt消息发送器");
        }

        public StarterLevel Level { get; }
    }
}