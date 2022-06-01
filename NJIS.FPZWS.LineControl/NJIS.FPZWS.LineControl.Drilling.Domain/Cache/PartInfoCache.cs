//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Domain
//   文 件 名：PartInfoCache.cs
//   创建时间：2018-12-04 10:30
//   作    者：
//   说    明：
//   修改时间：2018-12-04 10:30
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;
using NJIS.FPZWS.LineControl.Drilling.Contract;
using NJIS.FPZWS.LineControl.Drilling.Core;

namespace NJIS.FPZWS.LineControl.Drilling.Domain.Cache
{
    public class PartInfoCache
    {
        private static readonly IDrillingContract _service = ServiceLocator.Current.GetInstance<IDrillingContract>();

        public static Model.Drilling GetDrilling(string partId)
        {
            var pi = CacheManager.Current.Get<Model.Drilling>(partId);
            if (pi == null)
            {
                pi = _service.FindDrilling(partId);
                if (pi != null)
                {
                    Task.Factory.StartNew(() =>
                    {
                        var pis = _service.FindDrillings(pi.BatchName);
                        foreach (var drilling in pis)
                        {
                            CacheManager.Current.Set(drilling.PartID, drilling);
                        }
                    });
                }
            }

            if (pi == null)
            {
                return null;
            }

            return pi;
        }


        public static PartInfo GetPartInfo(string partId)
        {
            var pi = GetDrilling(partId);
            if (pi == null) return null;
            return GetPartInfo(pi);
        }

        public static PartInfo GetPartInfo(Model.Drilling pi)
        {
            var partInfo = new PartInfo
            {
                BatchName = pi.BatchName,
                DrillingRoute = pi.DrillingRouting,
                Length = (int)(pi.FinishLength ?? 0),
                Width = (int)(pi.FinishWidth ?? 0),
                Thickness = 18, // 数据库中无字段，写固定值
                PartId = pi.PartID,
                Rotation = pi.DrillingRotation != 0,
                OrderNumber = pi.OrderNumber
            };

            return partInfo;
        }
    }
}