using WCS.DataBase;

namespace WCS.Mod
{
    public class LoopControl
    {
        static int[] loopNodes = { 1118, 1127, 1131, 1135, 1139, 1143, 1149, 1153, 1157, 1161, 1165 };

        public static void Loop()
        {
            try
            {
                foreach (int node in loopNodes)
                {
                    var device = OPCHelper.GetDeviceByNo(node);
                    //就绪状态
                    if (device.DTarget != 999) { continue; }

                    //判断是否能走出循环区
                    var rlt = WcsSqlB.LoopC(device.DWorkId);
                    //var rlt = true;
                    if (rlt)
                    {
                        var task = WcsSqlB.GetTaskInfoBySeqId(device.DWorkId);
                        if (task == null)
                        {
                            WcsSqlB.InsertLog($"汇流口{node}：找不到任务！托盘号[{device.DPalletId}]！", 2, device.DPalletId);
                            continue;
                        }

                        OPCHelper.WriteTarget(node, WcsSqlB.GetDeviceNoByWmsNo(task.NoptStation));
                        WcsSqlB.UpdateWMSTask(task.SeqID.ToString(), 12);
                        WcsSqlB.InsertLog($"托盘号[{device.DPalletId}]走出汇流口[{node}]", 1);
                    }
                    else
                    {
                        //继续循环
                        OPCHelper.WriteTarget(node, node + 1);
                    }
                }
            }
            catch { }
        }

        static void Demo(int dno, int dno2)
        {
            var d = OPCHelper.GetDeviceByNo(dno);
            //就绪状态
            if (d.DTarget == 999)
            {
                if (d.DWorkId == 1)
                {
                    OPCHelper.WriteTarget(dno, dno + 1);
                    OPCHelper.WriteTaskId(dno, 2);
                }
                else
                {
                    OPCHelper.WriteTarget(dno, dno2);
                    OPCHelper.WriteTaskId(dno, 1);
                }
            }
        }

        public static void LoopDemo()
        {
            Demo(1118, 1127);
            Demo(1127, 1131);
            Demo(1131, 1135);
            Demo(1135, 1139);
            Demo(1139, 1143);
            Demo(1143, 1149);
            Demo(1149, 1153);
            Demo(1153, 1157);
            Demo(1157, 1161);
            Demo(1161, 1165);
            Demo(1165, 1118);
        }
    }
}
