//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Domain
//   文 件 名：PLcCommunicationModularStarter.cs
//   创建时间：2018-11-20 17:12
//   作    者：
//   说    明：
//   修改时间：2018-11-20 17:12
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using NJIS.FPZWS.Common.Initialize;
using NJIS.FPZWS.LineControl.PLC;
using NJIS.FPZWS.Log;

namespace NJIS.FPZWS.LineControl.Drilling.Domain.Control
{
    public class PLcCommunicationModularStarter : IModularStarter
    {
        private readonly ILogger _logger = LogManager.GetLogger(typeof(PLcCommunicationModularStarter));
        private readonly PlcCommandExecutorBase _pce = new PlcCommandExecutorBase();

        public void Start()
        {
            _logger.Info("初始化PLC 执行器");
            if (_pce.Init())
            {
                _logger.Info("启动PLC执行器");
                _pce.Start();
            }
            else
            {
                _logger.Info("初始化PLC 执行器失败");
            }
        }

        public void Stop()
        {
            _logger.Info("停止PLC 执行器");
            _pce.Stop();
        }

        public StarterLevel Level { get; }
    }

    public class PlcCommandExecutor : PlcCommandExecutorBase
    {
    }
}