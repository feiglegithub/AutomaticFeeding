using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJIS.FPZWS.LineControl.Cutting.ContractPlus;
using NJIS.FPZWS.LineControl.Cutting.Core;
using NJIS.FPZWS.LineControl.Cutting.ServicePlus;

namespace NJIS.FPZWS.LineControl.Cutting.DomainPlus
{
    public class RxCollector : CollectorBase
    {
        private static ILineControlCuttingContractPlus _lineControlCuttingContract = null;

        private static ILineControlCuttingContractPlus Contract =>
            _lineControlCuttingContract ?? (_lineControlCuttingContract = new LineControlCuttingServicePlus());

        public RxCollector(string deviceName) : base(deviceName) { }
        public override void Execute()
        {
            Contract.SyncPartFeedBack(DeviceName);
        }
    }
}
