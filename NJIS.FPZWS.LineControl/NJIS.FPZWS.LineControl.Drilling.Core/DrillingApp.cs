//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Core
//   文 件 名：DrillingApp.cs
//   创建时间：2018-11-20 15:20
//   作    者：
//   说    明：
//   修改时间：2018-11-20 15:20
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using NJIS.FPZWS.Common;
using NJIS.FPZWS.Common.Initialize;
using NJIS.FPZWS.Log;
using NJIS.FPZWS.Log.Implement.Log4Net;

namespace NJIS.FPZWS.LineControl.Drilling.Core
{
    public class DrillingApp : AppBase<DrillingApp>, IService
    {
        private readonly ILogger _logger = LogManager.GetLogger(typeof(DrillingApp));

        public override bool Start()
        {
            try
            {
                if (!base.Start()) return false;
                if (string.IsNullOrEmpty(DrillingSetting.Current.DrillingBuilder))
                {
                    _logger.Info($"找不到排钻创建器{DrillingSetting.Current.DrillingBuilder}失败");
                    return false;
                }
                var type = Type.GetType(DrillingSetting.Current.DrillingBuilder);
                if (type == null)
                {
                    _logger.Info($"加载排钻创建器{DrillingSetting.Current.DrillingBuilder}失败");
                    return false;
                }
                var builder = Activator.CreateInstance(type) as DrillingBuilder;
                if (builder == null)
                {
                    _logger.Info($"构建排钻创建器{DrillingSetting.Current.DrillingBuilder}失败");
                    return false;
                }

                try
                {
                    var dc = DrillingContext.Instance;
                    if (!dc.Build(builder))
                    {
                        _logger.Info($"创建排钻上下文失败");
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                }
                _logger.Info($"排钻启动成功");
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return true;
        }
    }
}