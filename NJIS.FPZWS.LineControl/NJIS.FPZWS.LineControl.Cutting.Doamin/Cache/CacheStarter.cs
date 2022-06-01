using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Cutting.Domain.Cache
{
    public class CacheStarter //: IModularStarter
    {
        //private readonly ILogger _logger = LogManager.GetLogger(typeof(CacheStarter));

        //public void Start()
        //{
        //    _logger.Info($"加载缓存数据：S=>{DrillingSetting.Current.CacheDayStart},E=>{DrillingSetting.Current.CacheDayEnd}");
        //    var service = new DrillingService();

        //    var i = DrillingSetting.Current.CacheDayStart;
        //    while (i > DrillingSetting.Current.CacheDayEnd)
        //    {
        //        var drillings = service.FindDrillings(DateTime.Now.AddDays(i));
        //        foreach (var drilling in drillings)
        //        {
        //            CacheManager.Current.Set(drilling.PartID, drilling);
        //        }

        //        i++;
        //    }
        //}

        //public void Stop()
        //{
        //}

        //public StarterLevel Level { get; }
    }
}
