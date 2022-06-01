using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCS.DataBase;
using WCS.model;

namespace WCS.Mod
{
    /// <summary>
    /// 空托盘调度管理
    /// </summary>
    public class EmptyControl
    {
        static InWareStaion iws1247 = new InWareStaion("2003", 2003, 1247, 600,"");
        static InWareStaion iws1255 = new InWareStaion("2003", 2003, 1255, 600,"");
        static InWareStaion iws1250 = new InWareStaion("2003", 2003, 1250, 600,"");
        static InWareStaion iws1285 = new InWareStaion("2004", 2004, 1285, 600,"");
        static InWareStaion iws1281 = new InWareStaion("2004", 2004, 1281, 600,"");
        //static int[] arr0 = { 1247,1255 }
        static int[] arr1 = { 2009, 2011, 2016, 2022 };  //自动补充空托盘的出口
        static int[] arr2 = { 1287, 1257, 1246 };  //码盘机判断

        public static void EptyRequestAll()
        {
            EptyRequest(iws1247, 1247);
            EptyRequest(iws1255, 1255);
            EptyRequest(iws1285, 1285);

            EptyRequestOut();
            MPChannel();
        }

        public static void EptyRequestOut()
        {
            //自动从立库里补充空托盘
            foreach(int wmsno in arr1)
            {
                var rlt = WcsSqlB.EmptyAutoOut(wmsno);
                if (rlt)
                {
                    var msg = WcsSqlB.CreateEmptyOutTask(wmsno);
                    if (msg.Length > 0)
                    {
                        WcsSqlB.InsertLog($"{wmsno}空托盘出库申请失败！{msg}", 2);
                    }
                    else
                    {
                        WcsSqlB.InsertLog($"空托盘出库申请成功！目标：{wmsno}", 1);
                    }
                }
            }
        }

        public static void EptyRequest(InWareStaion iws, int dno)
        {
            try
            {
                var n = OPCHelper.GetDeviceByNo(dno);
                if (n.DTarget != 999) { return; }

                var to = WcsSqlB.EmptyControl(dno, n.DWorkId);
                if (to == 0)
                {
                    if (string.IsNullOrWhiteSpace(n.DPalletId) && !n.DPalletId.StartsWith("TPZ"))
                    {
                        WcsSqlB.InsertLog($"{iws.StationName}：空托盘[{n.DPalletId}]入库失败！不合法", 2);
                        return;
                    }
                    //空盘入库申请
                    var msg = WcsSqlB.CreateEmptyInTask(n.DPalletId, iws.StationNo, 2);
                    if (msg.Length > 0)
                    {
                        WcsSqlB.InsertLog($"{iws.StationName}：空托盘[{n.DPalletId}]入库失败！{msg}", 2);
                    }
                    else
                    {
                        //Step5:写任务给设备
                        var task = WcsSqlB.GetTaskInfoByPallet(n.DPalletId);
                        OPCHelper.WriteStationOutTask(task, dno);
                        WcsSqlB.UpdateWMSTask(task.SeqID.ToString(), 10, 1);
                        WcsSqlB.InsertLog($"{iws.StationName}：空托盘入库开始！托盘号：{n.DPalletId}，目标：{task.Target}", 1);
                    }
                }
                else
                {
                    //直接向(4077,3053,4097,5029,6065,6082)补充空托盘
                    OPCHelper.WriteTarget(dno, to);
                    WcsSqlB.InsertLog($"{iws.StationName}：向目标：{to}补充空托盘", 1);
                }
            }
            catch { }
        }

        //2013口进空托盘，不入库
        public static void EptyIn2095()
        {
            var n = OPCHelper.GetDeviceByNo(2095);
            if (n.DTarget != 999) { return; }

            var pallet = WcsSqlB.ReadRFIDPallet(2013);
            OPCHelper.WritePallet(2013, pallet);
            OPCHelper.WriteTarget(2013, 1);
            OPCHelper.WriteTaskId(2013, 998);
        }

        //判断托盘类型，选择指定的码盘机码盘
        public static void MPChannel()
        {
            foreach(int dno in arr2)
            {
                var n = OPCHelper.GetDeviceByNo(dno);
                if (n.DTarget != 999) { continue; }
                var tp = WcsSqlB.GetPalletType(n.DPalletId);
                var target_mp = 0;
                if (tp == "1")
                {
                    //插矮杆托盘
                    if (dno == 1287)
                    {
                        target_mp = 1281;
                    }
                    else
                    {
                        target_mp = 1250;
                    }
                }
                else //if (tp == "2")
                {
                    //插高杆托盘
                    if (dno == 1287)
                    {
                        target_mp = 1285;
                    }
                    else if (dno == 1257)
                    {
                        target_mp = 1255;
                    }
                    else if (dno == 1246)
                    {
                        target_mp = 1247;
                    }
                }

                OPCHelper.WriteTarget(dno, target_mp);
            }
        }
    }
}
