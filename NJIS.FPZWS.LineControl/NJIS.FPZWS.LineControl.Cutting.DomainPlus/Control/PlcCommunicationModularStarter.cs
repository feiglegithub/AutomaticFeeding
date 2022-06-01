using System;
using System.Linq;
using System.Threading;
using NJIS.FPZWS.Common.Initialize;
using NJIS.FPZWS.LineControl.Cutting.ContractPlus;
using NJIS.FPZWS.LineControl.Cutting.DomainPlus.Cache;
using NJIS.FPZWS.LineControl.Cutting.ServicePlus;
using NJIS.FPZWS.LineControl.PLC;
using NJIS.FPZWS.Log;

namespace NJIS.FPZWS.LineControl.Cutting.DomainPlus.Control
{
    public class PlcCommunicationModularStarter : IModularStarter
    {
        private static readonly ILogger _logger = LogManager.GetLogger(typeof(PlcCommunicationModularStarter));
        private readonly PlcCommandExecutorBase _pce = new PlcCommandExecutorBase();
        private static readonly ILineControlCuttingContractPlus _service = new LineControlCuttingServicePlus();
        /// <summary>
        /// 扫码记录
        /// </summary>
        private static readonly Thread partScanLogThread = new Thread(ScanLog){IsBackground = true};
        public void Start()
        {
            _logger.Info("初始化PLC 执行器");
            if (_pce.Init())
            {
                _logger.Info("启动PLC执行器");
                _pce.Start();
                partScanLogThread.Start();
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
            partScanLogThread.Abort();
        }

        private static void ScanLog()
        {
            while (true)
            {
                if (PartScanLog.Current.CuttingPartScanLogs.Count > 0)
                {
                    var ret = true;
                    try
                    {
                        ret = _service.BulkInsertCuttingPartScanLog(PartScanLog.Current.CuttingPartScanLogs.ToList());
                    }
                    catch (Exception e)
                    {
                        _logger.Info("扫码记录写入数据库异常："+e.Message);
                        ret = false;
                    }
                    if (ret)
                    {
                        PartScanLog.Current.Remove(PartScanLog.Current.CuttingPartScanLogs.ToList());
                    }
                }
                Thread.Sleep(5000);
            }
        }

        public StarterLevel Level { get; }
    }

    public class PlcCommandExecutor : PlcCommandExecutorBase
    {
    }

}
