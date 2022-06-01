using NJIS.FPZWS.LineControl.Cutting.ContractPlus;
using NJIS.FPZWS.LineControl.Cutting.Core;
using NJIS.FPZWS.LineControl.Cutting.DomainPlus.Cache;
using NJIS.FPZWS.LineControl.Cutting.ServicePlus;
using NJIS.FPZWS.Log;

namespace NJIS.FPZWS.LineControl.Cutting.DomainPlus
{
    public class RxInParter: InParter
    {
        private static readonly ILineControlCuttingContractPlus _service = new LineControlCuttingServicePlus();// ServiceLocator.Current.GetInstance<ILineControlCuttingContract>();
        private readonly ILogger _log = LogManager.GetLogger<RxCuttingBuilder>();

        public override ControlPartInfo InPart(string partId, int position)
        {
            var pi = PartInfoCache.GetScanPartInfo(partId);
            if (pi == null)
            {
                _log.Info("找不到板件");
                return null;
            }

            ControlPartInfo cpi = new ControlPartInfo()
            {
                BatchName = pi.BatchName,
                DeviceName = pi.DeviceName,
                Length = pi.Length,
                PartId = pi.PartId,
                Width = pi.Width
            };

            return cpi;
        }
    }
}
