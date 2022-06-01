using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCS.DataBase;

namespace WCS.Mod
{
    public class InWareDemo
    {
        static int[] nodes = { 4164, 5129 };

        public static void Finished()
        {
            foreach (int node in nodes)
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
                    //OPCHelper.WriteTarget(node, 1);
                    continue;
                }
                //OPCHelper.WriteTarget(node, 1);
                WcsSqlB.UpdateTaskStatus(task.SeqID, 98);
                WcsSqlB.InsertLog($"出口{pot}：任务完成！托盘号[{device.DPalletId}]！", 1, device.DPalletId);


                //Step5:写任务给设备
                var t = WcsSqlB.GetTaskInfoByPallet(device.DPalletId);
                if (t != null)
                {
                    OPCHelper.WriteStationOutTask(t, node);
                    WcsSqlB.UpdateTaskStatus(t.SeqID, 21);
                    WcsSqlB.InsertLog($"{pot}：入库开始！任务号：{t.SeqID}，目标：{t.Target}", 1);
                }
                else
                {
                    WcsSqlB.InsertLog($"出口{pot}：测试完成！",1);
                }
            }
        }
    }
}
