using System;
using WCS.model;

namespace WCS
{
    public class OpcSc
    {
        static int start = -1;

        /// <summary>
        /// SC是否自动,1自动,0手动
        /// </summary>
        public static bool IsAuto(int SCNo)
        {
            var rlt_ddj = bool.Parse(OPCExecute.AsyncRead(SCNo - 1, 0).ToString());
            var rlt_dmg = bool.Parse(OPCExecute.AsyncRead(SCNo - 1, 1).ToString());

            return rlt_ddj && rlt_dmg;
        }

        /// <summary>
        /// SC是否空闲,1空闲,0正忙
        /// </summary>
        public static bool RIsFree(int SCNo)
        {
            //int ItemNo = 1;
            //int GroupNo = start + SCNo;
            //if (IsFinished(SCNo))
            //    return false;
            //else
            return bool.Parse(OPCExecute.AsyncRead(SCNo - 1, 2).ToString());
        }

        /// <summary>
        /// SC是否已激活
        /// </summary>
        public static bool RIsActivation(int SCNo)
        {
            //int ItemNo = 2;
            //int GroupNo = start + SCNo;
            return bool.Parse(OPCExecute.AsyncRead(SCNo - 1, 3).ToString());
        }

        //堆垛机 自动，空闲，激活 的状态下才能接收下一个任务
        public static bool CanRun(int SCNo)
        {
            return IsAuto(SCNo) && RIsFree(SCNo) && RIsActivation(SCNo);
        }

        /// <summary>
        ///读取等于1堆垛机已接受任务
        /// </summary>
        public static bool RTaskStatus(int SCNo)
        {
            //int ItemNo = 2;
            //int GroupNo = start + SCNo;
            return OPCExecute.AsyncRead(SCNo - 1, 18).ToString().Equals("1");
        }

        /// <summary>
        /// SC排
        /// </summary>
        public static int RRow(int SCNo)
        {
            //int itemNo = 3;
            //int GroupNo = start + SCNo;
            return int.Parse(OPCExecute.AsyncRead(SCNo-1, 4).ToString());
        }

        /// <summary>
        /// SC列
        /// </summary>
        public static int RColumn(int SCNo)
        {
            //int itemNo = 4;
            //int GroupNo = start + SCNo;
            return int.Parse(OPCExecute.AsyncRead(SCNo - 1, 5).ToString());
        }

        /// <summary>
        /// SC层
        /// </summary>
        public static int RLayer(int SCNo)
        {
            //int itemNo = 5;
            //int GroupNo = start + SCNo;
            return int.Parse(OPCExecute.AsyncRead(SCNo - 1, 6).ToString());
        }

        /// <summary>
        /// 任务号/托盘号
        /// </summary>
        public static string RTask(int SCNo)
        {
            //int itemNo = 6;
            //int GroupNo = start + SCNo;
            //byte[] byteArr = (byte[])OPCExecute.AsyncRead(GroupNo, itemNo);

            //string barcode = System.Text.ASCIIEncoding.Default.GetString(byteArr, 2, byteArr.Length - 2);
            //barcode = barcode.Substring(0, 11);
            //barcode = barcode.Replace("\0","");
            //return barcode;
            return OPCExecute.AsyncRead(SCNo - 1, 8).ToString();
        }

        /// <summary>
        /// 任务类型
        /// </summary>
        public static int RTaskType(int SCNo)
        {
            //int itemNo = 7;
            // int GroupNo = start + SCNo;
            return int.Parse(OPCExecute.AsyncRead(SCNo - 1, 7).ToString());
        }

        /// <summary>
        /// SC任务是否完成,1完成，0未完成
        /// </summary>
        public static bool IsFinished(int SCNo)
        {
            //int itemNo = 16;
            //nt GroupNo = start + SCNo;
            return OPCExecute.AsyncRead(SCNo - 1, 17).ToString() == "1";
        }

        //堆垛机完成任务时，给DB12,INT142写0
        public static void ClearTaskFinished(int SCNo)
        {
            //int itemNo = 16;
            //int GroupNo = start + SCNo;
            OPCExecute.AsyncWrite(SCNo - 1, 29, 0);

            //WaitDdjFree(SCNo);
        }

        //获取堆垛机执行任务的状态
        public static bool GetTaskStatus(int SCNo)
        {
            return OPCExecute.AsyncRead(SCNo - 1, 29).ToString() == "1";
        }

        /// <summary>
        /// SC货叉1任务写入入库任务
        /// </summary>
        /// <param name="item"></param>
        /// <param name="SCNo">堆垛机号</param>
        /// <param name="OptNo">堆垛机操作台号</param>
        /// <returns></returns>
        public static bool WTaskIn(Wcs_Task taskInfo, int SCNo)
        {
            if (taskInfo.ToLayer == 0)
                throw new Exception("目标层不能为0");
            int GroupNo = start + SCNo;

            //OPCExecute.AsyncWrite(GroupNo, 20, taskInfo.PilerNo);//写入货叉1工作序号（WORK NUMBER）
            OPCExecute.AsyncWrite(GroupNo, 21, 1);//写入货叉1出发HS位（FROM HOME STAND NUMBER）
            OPCExecute.AsyncWrite(GroupNo, 26, taskInfo.ToRow);//写入货叉1目的“行”号
            OPCExecute.AsyncWrite(GroupNo, 27, taskInfo.ToColumn);//写入货叉1目的“列”号
            OPCExecute.AsyncWrite(GroupNo, 28, taskInfo.ToLayer);//写入货叉1目的“层”号
            OPCExecute.AsyncWrite(GroupNo, 19, 1);//写入货叉1当前工作命令代码  1 入库

            //等待堆垛机接收任务
            WaitDdjBusy(SCNo);
            //WaitDdjAcceptTask(GroupNo);

            var isDdjAccept = SCAcceptTask(GroupNo);
            if (isDdjAccept)
            {
                //读取 DB12,INT98等于1时，写1
                OPCExecute.AsyncWrite(GroupNo, 29, 1);
            }

            return isDdjAccept;
        }

        //等待堆垛机忙碌
        static void WaitDdjBusy(int SCNo)
        {
            int ct2 = 0;
            while (RIsFree(SCNo))
            {
                System.Threading.Thread.Sleep(100);
                ct2++;

                if (ct2 >= 100) { break; }
            }
        }

        //等待堆垛机接收任务，最多等待时间10S
        static void WaitDdjAcceptTask(int GroupNo)
        {
            int ct = 0;
            while (!SCAcceptTask(GroupNo))
            {
                System.Threading.Thread.Sleep(100);
                ct++;

                if (ct >= 100) { break; }
            }
        }

        static bool SCAcceptTask(int GroupNo)
        {
            return OPCExecute.AsyncRead(GroupNo, 18).ToString() == "1";
        }

        /// <summary>
        /// SC货叉1任务写入出库任务----需要根据实际情况做调整
        /// </summary>
        /// <param name="item"></param>
        /// <param name="SCNo">堆垛机号</param>
        /// <param name="OptNo">堆垛机操作台号</param>
        /// <returns></returns>
        public static bool WTaskOut(Wcs_Task taskInfo, int SCNo)
        {
            int GroupNo = start + SCNo;

            //OPCExecute.AsyncWrite(GroupNo, 20, taskInfo.PilerNo);//写入货叉1工作序号（WORK NUMBER）
            OPCExecute.AsyncWrite(GroupNo, 25, 2);//写入货叉1目标HS位（FROM HOME STAND NUMBER）
            OPCExecute.AsyncWrite(GroupNo, 22, taskInfo.FromRow);//写入货叉1出发“行”号
            OPCExecute.AsyncWrite(GroupNo, 23, taskInfo.FromColumn);//写入货叉1出发“列”号
            OPCExecute.AsyncWrite(GroupNo, 24, taskInfo.FromLayer);//写入货叉1出发“层”号
            OPCExecute.AsyncWrite(GroupNo, 19, 2);//写入货叉1当前工作命令代码  2 出库

            //等待堆垛机接收任务
            //WaitDdjAcceptTask(GroupNo);
            WaitDdjBusy(SCNo);

            var IsDdjAccept = SCAcceptTask(GroupNo);
            if (IsDdjAccept)
            {
                //读取 DB12,INT98等于1时，写1
                OPCExecute.AsyncWrite(GroupNo, 29, 1);
            }

            return IsDdjAccept;
        }

        #region 监控使用点

        static int RStartHS(int SCNo)
        {
            //int itemNo = 8;
            //int GroupNo = start + SCNo;
            return int.Parse(OPCExecute.AsyncRead(SCNo - 1, 9).ToString());
        }

        static int RStartR(int SCNo)
        {
            //int itemNo = 9;
            //int GroupNo = start + SCNo;
            return int.Parse(OPCExecute.AsyncRead(SCNo - 1, 10).ToString());
        }

        static int RStartColumn(int SCNo)
        {
            //int itemNo = 10;
            //int GroupNo = start + SCNo;
            return int.Parse(OPCExecute.AsyncRead(SCNo - 1, 11).ToString());
        }

        static int RStartLayer(int SCNo)
        {
            //int itemNo = 11;
            //int GroupNo = start + SCNo;
            return int.Parse(OPCExecute.AsyncRead(SCNo - 1, 12).ToString());
        }

        static int REndHS(int SCNo)
        {
            //int itemNo = 12;
            //int GroupNo = start + SCNo;
            return int.Parse(OPCExecute.AsyncRead(SCNo - 1, 13).ToString());
        }

        static int REndR(int SCNo)
        {
            //int itemNo = 13;
            //int GroupNo = start + SCNo;
            return int.Parse(OPCExecute.AsyncRead(SCNo - 1, 14).ToString());
        }

        static int REndColumn(int SCNo)
        {
            //int itemNo = 14;
            //int GroupNo = start + SCNo;
            return int.Parse(OPCExecute.AsyncRead(SCNo - 1, 15).ToString());
        }

        static int REndLayer(int SCNo)
        {
            //int itemNo = 15;
            //int GroupNo = start + SCNo;
            return int.Parse(OPCExecute.AsyncRead(SCNo - 1, 16).ToString());
        }

        public static string RFrom(int SCNo)
        {
            int taskType = RTaskType(SCNo);
            string rt = "";
            switch (taskType)
            { 
                case 1:
                    rt = RStartHS(SCNo) + "";
                    break;
                case 2:
                    rt = RStartR(SCNo) + "." + RStartColumn(SCNo) + "." + RStartLayer(SCNo);
                    break;
                case 3:
                    rt = RStartR(SCNo) + "." + RStartColumn(SCNo) + "." + RStartLayer(SCNo);
                    break;
                case 4:
                    rt = RStartHS(SCNo) + "";
                    break;
                default:
                    break;
            }
            return rt;
        }

        public static string RTo(int SCNo)
        {
            int taskType = RTaskType(SCNo);
            string rt = "";
            switch (taskType)
            {
                case 1:
                    rt = REndR(SCNo) + "." + REndColumn(SCNo) + "." + REndLayer(SCNo);
                    break;
                case 2:
                    rt = REndHS(SCNo) + "";
                    break;
                case 3:
                    rt = REndR(SCNo) + "." + REndColumn(SCNo) + "." + REndLayer(SCNo);
                    break;
                case 4:
                    rt = REndHS(SCNo) + "";
                    break;
                default:
                    break;
            }
            return rt;
        }

        public static string RLocation(int SCNo)
        {
            return RRow(SCNo) + "." + RColumn(SCNo) + "." + RLayer(SCNo);
        }

        public static string GetSCErrorMsg(int SCNo) 
        {
            string msg = "";
            int GroupNo = start + SCNo;

            for (int i = 30; i <= 76; i++) 
            {
                bool rlt = bool.Parse(OPCExecute.AsyncRead(GroupNo, i).ToString());
                if (rlt) 
                {
                    msg = msg + i.ToString() + ",";
                }
            }

            return msg;
        }  
        #endregion
    }
}
