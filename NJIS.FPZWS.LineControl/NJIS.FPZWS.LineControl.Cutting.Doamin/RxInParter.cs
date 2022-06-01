using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;
using NJIS.FPZWS.LineControl.Cutting.Contract;
using NJIS.FPZWS.LineControl.Cutting.Core;
using NJIS.FPZWS.LineControl.Cutting.Domain.Cache;
using NJIS.FPZWS.LineControl.Cutting.Model;
using NJIS.FPZWS.LineControl.Cutting.Service;
using NJIS.FPZWS.Log;

namespace NJIS.FPZWS.LineControl.Cutting.Domain
{
    public class RxInParter: InParter
    {
        private static readonly ILineControlCuttingContract _service = new LineControlCuttingService();// ServiceLocator.Current.GetInstance<ILineControlCuttingContract>();
        private readonly ILogger _log = LogManager.GetLogger<RxCuttingBuilder>();

        public override ControlPartInfo InPart(string partId, int position)
        {
            var pi = PartInfoCache.GetPartInfo(partId);
            if (pi == null)
            {
                _log.Info("找不到板件");
                return null;
            }

            //var pi = PartInfoCache.GetPartInfo(cuttingTaskDetail);

            return pi;
        }
    }
}
