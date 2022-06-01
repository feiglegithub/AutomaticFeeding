using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJIS.FPZWS.LineControl.Cutting.Model;

namespace NJIS.FPZWS.LineControl.Cutting.Domain.MDB
{
    internal class CuttingTaskArithmetic
    {
        /// <summary>
        /// 分配任务
        /// </summary>
        /// <param name="taskList"></param>
        /// <param name="cuttingDeviceInfos"></param>
        public static void AssigningTask(List<SpiltMDBResult> taskList, List<DeviceInfos> cuttingDeviceInfos)
        {
            //taskList.Sort((x, y) => string.Compare(x.BatchName, y.BatchName, StringComparison.Ordinal));
            //if (cuttingDeviceInfos != null)
            //{
            //    var enumerator = cuttingDeviceInfos.GetEnumerator();
            //    enumerator.MoveNext();
            //    foreach (var taskMdbResult in taskList)
            //    {
            //        taskMdbResult.DeviceName = enumerator.Current.DeviceName;
            //        taskMdbResult.FinishedStatus = Convert.ToInt32(FinishedStatus.MdbUnloaded);
            //        if (!enumerator.MoveNext())
            //        {
            //            enumerator = cuttingDeviceInfos.GetEnumerator();
            //            enumerator.MoveNext();
            //        }
            //    }
            //}
        }
    }
}
