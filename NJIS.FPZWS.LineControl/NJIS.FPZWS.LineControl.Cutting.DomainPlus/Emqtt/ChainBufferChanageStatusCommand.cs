using System;
using NJIS.FPZWS.LineControl.Cutting.ContractPlus;
using NJIS.FPZWS.LineControl.Cutting.Message;
using NJIS.FPZWS.LineControl.Cutting.ServicePlus;
using NJIS.FPZWS.Log;
using NJIS.FPZWS.MqttClient;

namespace NJIS.FPZWS.LineControl.Cutting.DomainPlus.Emqtt
{
    /// <summary>
    ///     缓存架状态切换命令
    /// </summary>
    public class ChainBufferChanageStatusCommand : EmqttCommandBase
    {
        private readonly ILogger _log = LogManager.GetLogger(typeof(ChainBufferChanageStatusCommand).Name);
        private readonly ILineControlCuttingContractPlus _service = new LineControlCuttingServicePlus();// ServiceLocator.Current.GetInstance<ILineControlCuttingContract>();

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
