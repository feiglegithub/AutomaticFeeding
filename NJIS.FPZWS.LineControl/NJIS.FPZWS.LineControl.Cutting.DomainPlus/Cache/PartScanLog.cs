using System.Collections.Generic;
using NJIS.FPZWS.Common;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;

namespace NJIS.FPZWS.LineControl.Cutting.DomainPlus.Cache
{
    public class PartScanLog : Singleton<PartScanLog>
    {
        private static readonly  object _LockObj = new object();

        private static readonly List<CuttingPartScanLog> _cuttingPartScanLogs = new List<CuttingPartScanLog>();

        public List<CuttingPartScanLog> CuttingPartScanLogs => _cuttingPartScanLogs;

        public void Add(CuttingPartScanLog cuttingPartScanLog)
        {
            lock (_LockObj)
            {
                _cuttingPartScanLogs.Add(cuttingPartScanLog);
            }
        }

        public void Remove(CuttingPartScanLog cuttingPartScanLog)
        {
            lock (_LockObj)
            {
                _cuttingPartScanLogs.Remove(cuttingPartScanLog);
            }
        }

        public void Remove(List<CuttingPartScanLog> cuttingPartScanLogs)
        {
            lock (_LockObj)
            {
                _cuttingPartScanLogs.RemoveAll(item=> cuttingPartScanLogs.Exists(item1=>item1.Equals(item)));
            }
        }


    }
}
