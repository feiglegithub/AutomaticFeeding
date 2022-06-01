using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCS.DataBase;
using WCS.model;

namespace WCS.Mod
{
    public class EmptyControlDemo
    {
        static InWareStaion iws1247 = new InWareStaion("2003", 2003, 1247, 600,"");
        static InWareStaion iws1255 = new InWareStaion("2003", 2003, 1255, 600,"");
        static InWareStaion iws1285 = new InWareStaion("2004", 2004, 1285, 600,"");

        public static void EptyRequestAll()
        {
            EptyRequest(iws1247, 1247);
            EptyRequest(iws1255, 1255);
            EptyRequest(iws1285, 1285);
        }

        public static void EptyRequest(InWareStaion iws, int dno)
        {
            var n = OPCHelper.GetDeviceByNo(dno);
            if (n.DTarget != 999) { return; }
            var wt = WcsSqlB.GetTaskByPallet(n.DPalletId);
            if (wt == null)
            {
                WcsSqlB.InsertLog($"测试程序{iws.StationName}：找不到任务，托盘号{n.DPalletId}", 2);
                return;
            }
            OPCHelper.WriteTaskId(dno, wt.SeqID);
            OPCHelper.WriteTarget(dno, wt.Target);
            WcsSqlB.InsertLog($"测试程序{iws.StationName}：向堆垛机：{wt.Roadway}补充空托盘", 1);
        }
    }
}
