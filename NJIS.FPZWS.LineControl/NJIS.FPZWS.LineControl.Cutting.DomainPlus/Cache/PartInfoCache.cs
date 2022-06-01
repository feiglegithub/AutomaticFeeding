using System;
using System.Linq;
using NJIS.FPZWS.LineControl.Cutting.ContractPlus;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;
using NJIS.FPZWS.LineControl.Cutting.ServicePlus;

namespace NJIS.FPZWS.LineControl.Cutting.DomainPlus.Cache
{
    public class PartInfoCache
    {
        private static readonly ILineControlCuttingContractPlus _service = new LineControlCuttingServicePlus();// ServiceLocator.Current.GetInstance<ILineControlCuttingContract>(); //new LineControlCuttingService();
        
        public static PartFeedBack GetScanPartInfo(string partId)
        {
            var pi = CacheManager.Current.Get<PartFeedBack>(partId);
            if (pi == null)
            {
                var partInfos = _service.GetPartFeedBacksByPartId(partId);
                if (partInfos.Exists(item=>item.BatchName=="Default"))
                {
                    return partInfos.OrderByDescending(item=>item.CreatedTime).First(item => item.BatchName == "Default");
                }
                partInfos.RemoveAll(item => string.IsNullOrWhiteSpace(item.BatchName) || item.BatchName=="Default");
                if (partInfos.Count == 0) return null;
                else
                {
                    var tmpPartInfos = _service.GetPartFeedBacksByBatchName(partInfos[0].BatchName);
                    foreach (var partInfo in tmpPartInfos)
                    {
                        CacheManager.Current.Set(partInfo.PartId, partInfo);
                    }

                    return partInfos[0];
                }
            }
            else
            {
                return pi;
            }
            

        }


    }
}
