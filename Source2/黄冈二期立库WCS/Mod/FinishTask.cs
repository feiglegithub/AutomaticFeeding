using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCS.DataBase;

namespace WCS.Mod
{
    public class FinishTask
    {
        //整盘出口
        static int[] arr1 ={ 2090,2078,2065,2052,2039,2028,2018,2008,1244,1238,1232,1226,1220,1210,1205,1199,1193,1187,1181};

        //外协件,空盘出口
        static int[] arr2 = { 4164, 5129 };

        //空盘出库口
        static int[] arr3 = { 4097, 5029, 6082, 6065, 3053, 4077 };

        static int floor = 1;

        public static void FinisheAllTask()
        {
            Finish_1F();

            Finish_2F();

            Finish_Empty();
        }

        //出库任务完成处理
        static void Finish_1F()
        {
            int port = 1000;
            foreach (int node in arr1)
            {
                port++;
                var device = OPCHelper.GetDeviceByNo(node);
                //就绪状态
                if (device.DTarget != 999) { continue; }

                var task = WcsSqlB.GetTaskInfoBySeqId(device.DWorkId);
                if (task == null)
                {
                    WcsSqlB.InsertLog($"出口{port}：找不到任务[{device.DWorkId}]！", 2);
                    OPCHelper.WriteTarget(node, 1);
                    continue;
                }

                OPCHelper.WriteTarget(node, 1);
                WcsSqlB.UpdateTaskStatus(task.SeqID, 98);
                WcsSqlB.InsertLog($"出口{port}：任务完成！托盘号[{device.DPalletId}]！", 1, device.DPalletId);
            }
        }

        //二楼出库任务完成处理
        static void Finish_2F()
        {
            foreach (int node in arr2)
            {
                var device = OPCHelper.GetDeviceByNo(node);
                //就绪状态
                if (device.DTarget != 999) { continue; }

                var pot = 0;
                if (node == 4164) { pot = 2006; } else { pot = 2005; }

                var task = WcsSqlB.GetTaskInfoBySeqId(device.DWorkId);
                if (task == null)
                {
                    WcsSqlB.InsertLog($"出口{pot}：找不到任务[{device.DWorkId}]！", 2);
                    OPCHelper.WriteTarget(node, 1);
                    continue;
                }

                OPCHelper.WriteTarget(node, 1);
                WcsSqlB.UpdateTaskStatus(task.SeqID, 98);
                WcsSqlB.InsertLog($"出口{pot}：任务完成！托盘号[{device.DPalletId}]！", 1, device.DPalletId);
            }
        }

        //空盘出库口
        static void Finish_Empty()
        {
            foreach (int node in arr3)
            {
                var device = OPCHelper.GetDeviceByNo(node);
                if (device.DTarget != 999) { continue; }

                var rt = WcsSqlB.ClearEmptyBuffer(device.DWorkId, node);

                if (node == 4097 || node == 5029)
                {
                    OPCHelper.WriteTarget(node, 0);
                    OPCHelper.WriteTaskId(node, 0);
                    continue;
                }

                if (rt > 0 && node == 6082)
                {
                    OPCHelper.WriteTarget(node, WcsSqlB.EmptyOut2016(floor));
                    floor++;
                    if (floor > 3) { floor = 1; }
                    continue;
                }

                if (rt == 0)
                {
                    var task = WcsSqlB.GetTaskInfoBySeqId(device.DWorkId);

                    if (task == null)
                    {
                        //WcsSqlB.InsertLog($"出口{node}：找不到任务[{device.DWorkId}]！", 2);
                        OPCHelper.WriteTarget(node, 1);
                        continue;
                    }

                    //如果是去到6082的杂项下架，则默认出到厂房A二楼
                    if (task.NordID == 3 && node == 6082)
                    {
                        OPCHelper.WriteTarget(node, 6014);
                    }
                    else if (task.NordID == 6 && node == 6082)
                    {
                        OPCHelper.WriteTarget(node, WcsSqlB.EmptyOut2016(floor));
                        floor++;
                        if (floor > 3) { floor = 1; }
                    }
                    else
                    {
                        OPCHelper.WriteTarget(node, 1);
                    }

                    WcsSqlB.UpdateTaskStatus(task.SeqID, 98);
                    WcsSqlB.InsertLog($"出口{node}：任务完成！托盘号[{device.DPalletId}]！", 1, device.DPalletId);
                }
            }
        }
    }
}
