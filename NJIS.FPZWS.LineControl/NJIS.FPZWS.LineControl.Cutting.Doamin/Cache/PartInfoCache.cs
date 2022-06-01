using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;
using NJIS.FPZWS.LineControl.Cutting.Contract;
using NJIS.FPZWS.LineControl.Cutting.Core;
using NJIS.FPZWS.LineControl.Cutting.Model;
using NJIS.FPZWS.LineControl.Cutting.Service;

namespace NJIS.FPZWS.LineControl.Cutting.Domain.Cache
{
    public class PartInfoCache
    {
        private static readonly PartInfoService partInfoService = new PartInfoService();
        private static readonly ILineControlCuttingContract _service = new LineControlCuttingService();// ServiceLocator.Current.GetInstance<ILineControlCuttingContract>(); //new LineControlCuttingService();

        public static CuttingTaskDetail GetCuttingTaskDetail(string partId)
        {
            var pi = CacheManager.Current.Get<CuttingTaskDetail>(partId);
            if (pi == null)
            {
                var pis = _service.GetBatchTaskDetailsByPartId(partId);
                foreach (var cutting in pis)
                {
                    CacheManager.Current.Set(cutting.PartId, cutting);
                }

            }
            pi = CacheManager.Current.Get<CuttingTaskDetail>(partId);

            return pi;
        }


        public static ControlPartInfo GetPartInfo(string partId)
        {
            var pi = GetCuttingTaskDetail(partId);
            if (pi == null) return null;
            return GetPartInfo(pi);
        }

        public static ControlPartInfo GetPartInfo(CuttingTaskDetail pi)
        {
            var partInfo = new ControlPartInfo
            {
                BatchName = pi.BatchName,
                PartId = pi.PartId,
                DeviceName = pi.DeviceName,
                Length = Convert.ToDouble(pi.Length),
                Width = Convert.ToDouble(pi.Width)

                //OrderNumber = pi.
            };

            return partInfo;
        }

        public static PartInfo GetScanPartInfo(string partId)
        {
            var pi = CacheManager.Current.Get<PartInfo>(partId);
            if (pi == null)
            {
                var partInfos = partInfoService.GetInfos(partId);
                partInfos.RemoveAll(item => string.IsNullOrWhiteSpace(item.BatchCode));
                if (partInfos.Count == 0) return null;
                else
                {
                    var tmpPartInfos = partInfoService.GetInfosByBatchName(partInfos[0].BatchCode);
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

        public static CuttingFeedBack GetCuttingFeedBack(string partId)
        {
            var pi = CacheManager.Current.Get<CuttingFeedBack>(partId);
            if (pi == null)
            {
                var partInfos = _service.GetCuttingFeedBacksByPartId(partId);
                partInfos.RemoveAll(item => string.IsNullOrWhiteSpace(item.BatchName));
                if (partInfos.Count == 0) return null;
                else
                {
                    var partInfo = partInfos[0];
                    if (partInfo.PartId.Contains("X")) return partInfo;
                    var tmpPartInfos = _service.GetCuttingFeedBacksByBatchName(partInfo.BatchName);
                    foreach (var part in tmpPartInfos)
                    {
                        CacheManager.Current.Set(part.PartId, part);
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
