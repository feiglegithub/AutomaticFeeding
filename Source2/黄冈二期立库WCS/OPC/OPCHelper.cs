using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WCS.DataBase;
using WCS.model;

namespace WCS
{
    public class OPCHelper
    {
        #region 堆垛机
        //给堆垛机写任务
        public static bool WriteTaskToSC(Wcs_Task ti)
        {
            var cpuNo = ti.Roadway + 5;

            OPCExecute.AsyncWrite(cpuNo, 29, ti.NordID);  //给堆垛机写任务类型
            OPCExecute.AsyncWrite(cpuNo, 18, 12 - ti.Roadway); //给堆垛机写巷道号(WMS巷道号与电气巷道号相反)
            OPCExecute.AsyncWrite(cpuNo, 22, ti.FromRow);        //出发行(1左,2右) 
            OPCExecute.AsyncWrite(cpuNo, 23, ti.FromColumn);  //出发列
            OPCExecute.AsyncWrite(cpuNo, 24, ti.FromLayer);      //出发层
            OPCExecute.AsyncWrite(cpuNo, 25, ti.ToRow);             //目标行(1左,2右)
            OPCExecute.AsyncWrite(cpuNo, 26, ti.ToColumn);       //目标列
            OPCExecute.AsyncWrite(cpuNo, 27, ti.ToLayer);           //目标层
            OPCExecute.AsyncWrite(cpuNo, 19, ti.SeqID); //给堆垛机写任务号
            OPCExecute.AsyncWrite(cpuNo, 20, ti.NPalletID); //给堆垛机写托盘号
            OPCExecute.AsyncWrite(cpuNo, 21, 1);      //给堆垛机写任务时，置1 任务完成时，置6

            //WcsSqlB.InsertLog($"任务号：{ti.SeqID}，写1的记录时间：{DateTime.Now.ToString()}", 2);
  
            if (WaitAccept(ti.Roadway))
            {
                WCSFeedBack(ti.Roadway, 0);
                return true;
            }
            return false;
        }

        //等待堆垛机接收任务,等10秒
        static bool WaitAccept(int scNo)
        {
            int ct = 0;
            var rlt = false;
            while (rlt == false)
            {
                rlt = IsAcceptTask(scNo);
                if (rlt) { return true; }
                Thread.Sleep(100);

                if (ct > 100)
                    return false;

                ct++;
            }

            return false;
        }

        //等待堆垛机变成忙碌状态
        static void WaitBusy(int scNo)
        {
            int ct = 0;
            while (IsFree(scNo))
            {
                Thread.Sleep(100);

                if (ct > 100)
                    break;

                ct++;
            }
        }

        //堆垛机是否报警 true 报警;   false 正常
        public static bool IsAlarm(int scNo)
        {
            return Convert.ToBoolean(OPCExecute.AsyncRead(scNo + 5, 1));
        }

        //堆垛机是否自动 true 自动;  false 手动
        public static bool IsAuto(int scNo)
        {
            return Convert.ToBoolean(OPCExecute.AsyncRead(scNo + 5, 0));
        }

        //堆垛机是否空闲 true 空闲; false 忙碌
        public static bool IsFree(int scNo)
        {
            return Convert.ToInt32(OPCExecute.AsyncRead(scNo + 5, 2)) == 0;
        }

        //堆垛机是否收到任务
        public static bool IsAcceptTask(int scNo)
        {
            return Convert.ToBoolean(OPCExecute.AsyncRead(scNo + 5, 7));
        }

        //堆垛机是否完成任务
        public static bool IsFinishTask(int scNo)
        {
            return Convert.ToBoolean(OPCExecute.AsyncRead(scNo + 5, 8));
        }

        public static int ReadSCTStatus(int scNo)
        {
            //var i = Convert.ToInt32(OPCExecute.AsyncRead(scNo + 5, 21));
            //return i;
            return Convert.ToInt32(OPCExecute.AsyncRead(scNo + 5, 21));
        }

        //堆垛机只有在 没报警、自动，空闲的状态下才能接收任务
        public static bool IsDdjCanRun(int scNo)
        {
            return !IsAlarm(scNo) && IsFree(scNo) && IsAuto(scNo);
        }

        //在确认堆垛机收到任务后，WCS写0；在确认堆垛机完成任务后，WCS写1；
        public static void WCSFeedBack(int scNo, int value)
        {
            OPCExecute.AsyncWrite(scNo + 5, 17, value);

            if (value == 1)
            {
                OPCExecute.AsyncWrite(scNo + 5, 21, 6);
            }
        }

        public static void WriteSix(int scNo)
        {
            OPCExecute.AsyncWrite(scNo + 5, 21, 6);
        }

        //读取当前列
        public static int ReadColumn(int scNo)
        {
            return Convert.ToInt32(OPCExecute.AsyncRead(scNo + 5, 3));
        }

        //读取当前层
        public static int ReadLayer(int scNo)
        {
            return Convert.ToInt32(OPCExecute.AsyncRead(scNo + 5, 4));
        }

        //读取任务ID
        public static int ReadSCTaskId(int scNo)
        {
            return Convert.ToInt32(OPCExecute.AsyncRead(scNo + 5, 5));
        }

        //读取托盘号
        public static string ReadSCPallet(int scNo)
        {
            var obj = OPCExecute.AsyncRead(scNo + 5, 6);
            return obj == null ? "" : obj.ToString();
        }

        //读取任务状态  0 任务执行中   1 任务已完成
        public static int ReadTaskStatus(int scNo)
        {
            return Convert.ToInt32(OPCExecute.AsyncRead(scNo + 5, 17));
        }

        //读取堆垛机的任务类型
        public static string ReadSCTaskType(int scNo)
        {
            var type = Convert.ToInt32(OPCExecute.AsyncRead(scNo + 5, 28));
            switch (type)
            {
                case 1:
                    return "整盘入库";
                case 3:
                    return "整盘出库";
                case 5:
                    return "空盘入库";
                case 6:
                    return "空盘出库";
            }

            return "";
        }

        //读取堆垛机错误信息
        public static int[] ReadSCCode(int scNo)
        {
            var cpu = scNo + 5;
            int[] arr1 = { 0, 0, 0, 0, 0, 0, 0 };
            arr1[0] = Convert.ToInt32(OPCExecute.AsyncRead(cpu, 9));
            arr1[1] = Convert.ToInt32(OPCExecute.AsyncRead(cpu, 10));
            arr1[2] = Convert.ToInt32(OPCExecute.AsyncRead(cpu, 11));
            arr1[3] = Convert.ToInt32(OPCExecute.AsyncRead(cpu, 12));
            arr1[4] = Convert.ToInt32(OPCExecute.AsyncRead(cpu, 13));
            arr1[5] = Convert.ToInt32(OPCExecute.AsyncRead(cpu, 14));
            arr1[6] = Convert.ToInt32(OPCExecute.AsyncRead(cpu, 15));
            return arr1;
        }

        //给设备写任务数据
        public static void WriteStationOutTask(Wcs_Task task, int no)
        {

            if (task == null) { return; }

            int itemNo = WcsSqlB.GetItemNoByDeviceNo(no);
            int cupNo = _getCpuNoByNo(no);

            OPCExecute.AsyncWrite(cupNo, itemNo, task.SeqID);
            OPCExecute.AsyncWrite(cupNo, itemNo + 1, task.Target);
            OPCExecute.AsyncWrite(cupNo, itemNo + 2, task.NPalletID);
        }
        #endregion

        #region 一区线控设备

        #endregion

        #region 二区线控设备
        #endregion

        #region 三区线控设备
        #endregion

        #region 四区线控设备
        #endregion

        #region 五区线控设备
        #endregion

        #region 六区线控设备
        #endregion

        #region 线控综合
        public static void MonitorDevice()
        {
            var dt = WcsSqlB.GetAllDevice();

            var sb = new StringBuilder();
            foreach (DataRow dr in dt.Rows)
            {
                var did = int.Parse(dr["DId"].ToString());
                int cupNo = _getCpuNoByNo(did);
                var itemNo = int.Parse(dr["ItemNo"].ToString());

                var DWorkId = OPCExecute.AsyncRead(cupNo, itemNo).ToString();
                var DTarget = OPCExecute.AsyncRead(cupNo, itemNo + 1).ToString();
                var obj = OPCExecute.AsyncRead(cupNo, itemNo + 2);
                var DPalletId = obj == null ? "" : obj.ToString();
                var DErrorNo = OPCExecute.AsyncRead(cupNo, itemNo + 3).ToString();

                sb.Append($"update Wcs_Device set DWorkId={DWorkId},DPalletId='{DPalletId}',DTarget={DTarget},DErrorNo={DErrorNo} where DId={did};");

                var msgcn = AppCommon.GetDeviceErrorMsg(int.Parse(DErrorNo));
                if (msgcn.IndexOf("超时") > -1 || msgcn.IndexOf("传感器异常") > -1)
                {
                    WcsSqlB.InsertLog($"设备故障[{did}]：{msgcn}", 2, "", did.ToString());
                }
            }

            WcsSqlB.UpdateAllDecice(sb.ToString());
        }

        public static Wcs_Device GetDeviceByNo(int no)
        {
            var d = new Wcs_Device();
            int itemNo = WcsSqlB.GetItemNoByDeviceNo(no);
            int cupNo = _getCpuNoByNo(no);

            d.DWorkId = int.Parse(OPCExecute.AsyncRead(cupNo, itemNo).ToString());
            d.DTarget = int.Parse(OPCExecute.AsyncRead(cupNo, itemNo + 1).ToString());
            var obj = OPCExecute.AsyncRead(cupNo, itemNo + 2);
            d.DPalletId = obj == null ? "" : obj.ToString();
            d.DErrorNo = int.Parse(OPCExecute.AsyncRead(cupNo, itemNo + 3).ToString());
            return d;
        }

        public static int ReadTarget(int no)
        {
            int itemNo = WcsSqlB.GetItemNoByDeviceNo(no);
            int cupNo = _getCpuNoByNo(no);

            return int.Parse(OPCExecute.AsyncRead(cupNo, itemNo + 1).ToString());
        }

        public static string ReadPallet(int no)
        {
            int itemNo = WcsSqlB.GetItemNoByDeviceNo(no);
            int cupNo = _getCpuNoByNo(no);

            var obj =  OPCExecute.AsyncRead(cupNo, itemNo + 2).ToString();
            return obj == null ? "" : obj.ToString();
        }

        public static int ReadTaskId(int no)
        {
            int itemNo = WcsSqlB.GetItemNoByDeviceNo(no);
            int cupNo = _getCpuNoByNo(no);

            return int.Parse(OPCExecute.AsyncRead(cupNo, itemNo).ToString());
        }

        //给设备写目标值
        public static void WriteTarget(int no, int target)
        {
            int itemNo = WcsSqlB.GetItemNoByDeviceNo(no);
            int cupNo = _getCpuNoByNo(no);

            OPCExecute.AsyncWrite(cupNo, itemNo + 1, target);
        }

        //给设备写任务号
        public static void WriteTaskId(int no, int taskid)
        {
            int itemNo = WcsSqlB.GetItemNoByDeviceNo(no);
            int cupNo = _getCpuNoByNo(no);

            OPCExecute.AsyncWrite(cupNo, itemNo, taskid);
        }

        //给设备写托盘号
        public static void WritePallet(int no, string pallet)
        {
            int itemNo = WcsSqlB.GetItemNoByDeviceNo(no);
            int cupNo = _getCpuNoByNo(no);

            OPCExecute.AsyncWrite(cupNo, itemNo + 2, pallet);
        }

        static int _getCpuNoByNo(int no)
        {
            return no / 1000 - 1;
        }
        #endregion
    }
}
