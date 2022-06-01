using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;
using NJIS.FPZWS.LineControl.Cutting.Contract;
using NJIS.FPZWS.LineControl.Cutting.Message;
using NJIS.FPZWS.LineControl.Cutting.Service;
using NJIS.FPZWS.Log;
using NJIS.FPZWS.MqttClient;
using NJIS.FPZWS.MqttClient.Setting;

namespace NJIS.FPZWS.LineControl.Cutting.Domain.Emqtt
{
    /// <summary>
    ///     缓存架状态切换命令
    /// </summary>
    public class ChainBufferChanageStatusCommand : EmqttCommandBase
    {
        private readonly ILogger _log = LogManager.GetLogger(typeof(ChainBufferChanageStatusCommand).Name);
        private readonly ILineControlCuttingContract _service = new LineControlCuttingService();// ServiceLocator.Current.GetInstance<ILineControlCuttingContract>();

        public override string SendTopic => EmqttSettings.Current.PcsChanageStatusChainBufferRep;

        public override string ReceiveTopic => EmqttSettings.Current.PcsChanageStatusChainBufferReq;

        public override void Execute(MqttMessageBase input)
        {
            try
            {
                MqttManager.Current.Publish($"{SendTopic}", null);
            }
            catch (Exception e)
            {
                SendMsg($"初始化队列数据=>{e.Message}");
            }
        }
    }
}
