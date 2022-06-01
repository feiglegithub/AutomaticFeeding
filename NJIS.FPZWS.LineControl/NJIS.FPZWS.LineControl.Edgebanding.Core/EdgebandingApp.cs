//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Edgebanding
//   项目名称：NJIS.FPZWS.LineControl.Edgebanding.Core
//   文 件 名：EdgebandingApp.cs
//   创建时间：2018-12-13 14:35
//   作    者：
//   说    明：
//   修改时间：2018-12-13 14:35
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using NJIS.FPZWS.Common;
using NJIS.FPZWS.Log;
using NJIS.FPZWS.Log.Implement.Log4Net;

namespace NJIS.FPZWS.LineControl.Edgebanding.Core
{
    public class EdgebandingApp : AppBase<EdgebandingApp>, IService
    {
        private ILogger _log = LogManager.GetLogger(typeof(EdgebandingApp).Name);

        public override bool Start()
        {
            if (!base.Start()) return false;
            LogManager.AddLoggerAdapter(new Log4NetLoggerAdapter());
            _log = LogManager.GetLogger(typeof(EdgebandingApp).Name);
            _log.Info("服务启动");
            return base.Start();
        }

        public override bool Stop()
        {
            _log.Info("服务停止");
            return base.Stop();
        }
    }
}