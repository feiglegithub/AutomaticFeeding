using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;
using NJIS.FPZWS.LineControl.Cutting.Contract;
using NJIS.FPZWS.LineControl.Cutting.Message;
using NJIS.FPZWS.Log;
using NJIS.FPZWS.MqttClient;
using NJIS.FPZWS.Wcf.Client;

namespace NJIS.FPZWS.LineControl.Cutting.Domain.Emqtt
{
    public class PartInfoPositionCommand:EmqttCommandBase
    {
        private readonly ILineControlCuttingContract _lineControlCuttingContract =
            ServiceLocator.Current.GetInstance<ILineControlCuttingContract>();

        private readonly ILogger _log = LogManager.GetLogger<PartInfoPositionCommand>();

        public override string SendTopic => EmqttSettings.Current.PcsPartInfoPositionRep;
        public override string ReceiveTopic => EmqttSettings.Current.PcsPartInfoPositionReq;

        public override void Execute(MqttMessageBase input)
        {
            throw new NotImplementedException();
        }
    }
}
