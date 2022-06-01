using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJIS.FPZWS.LineControl.Cutting.Model;
using NJIS.FPZWS.LineControl.Cutting.Server.Client.ViewModels;

namespace NJIS.FPZWS.LineControl.Cutting.Server.Client.Helpers
{
    /// <summary>
    /// 建垛算法
    /// </summary>
    public class CreatedStackArithmeticHelper
    {
        /// <summary>
        /// 创建方案
        /// </summary>
        /// <param name="deviceCount">设备数</param>
        /// <param name="batchName">批次号</param>
        /// <param name="allTasks">所有板材任务</param>
        /// <param name="stackMaxBookCount">垛的最大板材数</param>
        /// <returns></returns>
        public SolutionInfo CreatedSolutionInfo(int deviceCount,string batchName,List<AllTask> allTasks,int stackMaxBookCount=40)
        {
            SolutionInfo si = new SolutionInfo(batchName,deviceCount);
            return si;
        }

        public List<DeviceStackInfo> CreatedDeviceStackInfos(int deviceCount, List<AllTask> allTasks)
        {
            List<DeviceStackInfo> deviceStackInfos = new List<DeviceStackInfo>();
            var colorCount = from allTask in allTasks
                group allTask by new {allTask.RawMaterialID}
                into t
                select new {Count = t.Sum(item => item.BookNum), RawMaterialID = t.Key.RawMaterialID};

            colorCount = colorCount.OrderByDescending(item => item.Count);
            

           var avgTime = allTasks.Sum(item => item.TotalTime) / (deviceCount * 1.0);

            var tmpList = new List<AllTask>(allTasks);
            Dictionary<string,List<AllTask>> dictionary = new Dictionary<string, List<AllTask>>();

            for (int i = 1; i <=deviceCount; i++)
            {
                var deviceName = i.ToString();
                DeviceStackInfo dsi = new DeviceStackInfo(deviceName);
                if (!dictionary.ContainsKey(deviceName))
                {
                    dictionary.Add(i.ToString(), new List<AllTask>());
                }
                var enumerator = colorCount.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    var rawMaterialId = enumerator.Current.RawMaterialID;
                    var list = tmpList.FindAll(item => item.RawMaterialID == rawMaterialId);
                    list = list.OrderByDescending(item => item.TotalTime).ToList();
                    int curTotalTime = 0;

                    foreach (var allTask in list)
                    {
                        if (curTotalTime + allTask.TotalTime > avgTime)
                        {
                            break;
                        }
                        curTotalTime += allTask.TotalTime;
                        dictionary[deviceName].Add(allTask);
                        tmpList.Remove(allTask);
                    }
                }

               


            }

            return null;
        }

        //public List<StackInfo> CreatedStackInfos(List<AllTask> allTasks, int stackMaxBookCount = 40)
        //{
            
        //}
    }
}
