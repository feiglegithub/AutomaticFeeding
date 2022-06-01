//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Domain
//   文 件 名：CacheStarter.cs
//   创建时间：2018-11-29 9:36
//   作    者：
//   说    明：
//   修改时间：2018-11-29 9:36
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;
using NJIS.FPZWS.Common.Initialize;
using NJIS.FPZWS.LineControl.Drilling.Service;
using NJIS.FPZWS.Log;

namespace NJIS.FPZWS.LineControl.Drilling.Domain.Cache
{
    public class CacheStarter : IModularStarter
    {
        private readonly ILogger _logger = LogManager.GetLogger(typeof(CacheStarter));

        public void Start()
        {
            _logger.Info($"加载缓存数据：S=>{DrillingSetting.Current.CacheDayStart},E=>{DrillingSetting.Current.CacheDayEnd}");
            var service = new DrillingService();

            var i = DrillingSetting.Current.CacheDayStart;
            while (i > DrillingSetting.Current.CacheDayEnd)
            {
                var drillings = service.FindDrillings(DateTime.Now.AddDays(i));
                foreach (var drilling in drillings)
                {
                    CacheManager.Current.Set(drilling.PartID, drilling);
                }

                i++;
            }
        }

        public void Stop()
        {
        }

        public StarterLevel Level { get; }
    }
}