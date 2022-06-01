using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WCS.DataBase;
using WCS.model;

namespace WCS.OPC
{
    public class OpcHsc
    {
        const int hs_cpu = 4;
        const int rgv_cpu = 5;
        const int error_cpu = 6;

        #region 堆垛机
        /// <summary>
        /// 给堆垛机出库站台写任务
        /// </summary>
        /// <param name="ddjNo"></param>
        /// <param name="pilerNo"></param>
        /// <param name="target"></param>
        public static bool WriteSCStationOutTask(int ddjNo, int pilerNo, int target)
        {
            bool success = true;
            var deviceNo = "";
            var itemNo = 0;
            if (ddjNo == 1)
            {
                deviceNo = "GT128";
            }
            else if (ddjNo == 2)
            {
                deviceNo = "GT125";
            }
            else if (ddjNo == 3)
            {
                deviceNo = "GT122";
            }
            else if (ddjNo == 4)
            {
                deviceNo = "GT119";
            }

            itemNo = WCSSql.GetItemNoByDeviceNo(deviceNo);
            success &= OPCExecute.AsyncWrite(hs_cpu, itemNo + 1, pilerNo);
            success &= OPCExecute.AsyncWrite(hs_cpu, itemNo + 2, target);
            return success;
        }

        public static bool OutStationHasBoard(int ddjNo)
        {
            var deviceNo = "";
            var itemNo = 0;
            if (ddjNo == 1)
            {
                deviceNo = "GT128";
            }
            else if (ddjNo == 2)
            {
                deviceNo = "GT125";
            }
            else if (ddjNo == 3)
            {
                deviceNo = "GT122";
            }
            else if (ddjNo == 4)
            {
                deviceNo = "GT119";
            }

            itemNo = WCSSql.GetItemNoByDeviceNo(deviceNo);
            return bool.Parse(OPCExecute.AsyncRead(hs_cpu, itemNo).ToString());
        }

        /// <summary>
        /// 堆垛机出库站台是否空闲
        /// </summary>
        /// <param name="ddjNo"></param>
        /// <returns></returns>
        public static bool IsOutStationFree(int ddjNo)
        {
            var deviceNo = "";
            var itemNo = 0;
            if (ddjNo == 1)
            {
                deviceNo = "GT128";
            }
            else if (ddjNo == 2)
            {
                deviceNo = "GT125";
            }
            else if (ddjNo == 3)
            {
                deviceNo = "GT122";
            }
            else if (ddjNo == 4)
            {
                deviceNo = "GT119";
            }

            itemNo = WCSSql.GetItemNoByDeviceNo(deviceNo);
            return bool.Parse(OPCExecute.AsyncRead(hs_cpu, itemNo).ToString());
        }

        /// <summary>
        /// 入库站台是否有入库请求
        /// </summary>
        /// <param name="ddjNo"></param>
        /// <returns></returns>
        public static bool IsStationInRequest(int ddjNo)
        {
            var deviceNo = "";
            var itemNo = 0;
            if (ddjNo == 1)
            {
                deviceNo = "GT114";
            }
            else if (ddjNo == 2)
            {
                deviceNo = "GT111";
            }
            else if (ddjNo == 3)
            {
                deviceNo = "GT108";
            }
            else if (ddjNo == 4)
            {
                deviceNo = "GT105";
            }

            itemNo = WCSSql.GetItemNoByDeviceNo(deviceNo);
            return bool.Parse(OPCExecute.AsyncRead(hs_cpu, itemNo + 3).ToString());
        }

        /// <summary>
        /// 读取入库站台垛号
        /// </summary>
        /// <param name="ddjNo"></param>
        /// <returns></returns>
        public static int ReadStationPiler(int ddjNo)
        {
            var deviceNo = "";
            if (ddjNo == 1)
            {
                deviceNo = "GT114";
            }
            else if (ddjNo == 2)
            {
                deviceNo = "GT111";
            }
            else if (ddjNo == 3)
            {
                deviceNo = "GT108";
            }
            else if (ddjNo == 4)
            {
                deviceNo = "GT105";
            }

            return ReadPLCPilerNo(deviceNo);
        }
        #endregion

        #region 输送设备
        /// <summary>
        /// 读取PLC的GT101滚筒的前进指令
        /// </summary>
        /// <returns></returns>
        public static bool GT101GoHeadCmd()
        {
            return OPCExecute.AsyncRead(hs_cpu, 340).ToString() == "1";
        }

        /// <summary>
        /// 给PLC反馈是否允许GT101滚筒向前进板  2允许   3不允许
        /// </summary>
        /// <param name="rlt"></param>
        public static void FeedBackGT101Cmd(int rlt)
        {
            OPCExecute.AsyncWrite(hs_cpu, 340, rlt);
        }

        /// <summary>
        /// 读取电气的请求信号
        /// </summary>
        /// <param name="stationNo"></param>
        /// <returns></returns>
        public static bool ReadPLCRequest(int stationNo)
        {
            var itemNo = WCSSql.GetItemNoByStationNo(stationNo);
            return bool.Parse(OPCExecute.AsyncRead(hs_cpu, itemNo + 3).ToString());
        }

        /// <summary>
        /// 读取到位完成信号
        /// </summary>
        /// <param name="stationNo"></param>
        /// <returns></returns>
        public static bool ReadIsTaskDone(int stationNo)
        {
            var itemNo = WCSSql.GetItemNoByStationNo(stationNo);
            switch (stationNo)
            {
                case 2001:
                case 2002:
                case 2004:
                case 2006:
                    return bool.Parse(OPCExecute.AsyncRead(hs_cpu, itemNo + 4).ToString());
                case 100:
                case 1001:
                case 3001:
                case 3002:
                case 3003:
                case 3004:
                case 3005:
                case 3006:
                case 3007:
                    return bool.Parse(OPCExecute.AsyncRead(hs_cpu, itemNo + 3).ToString());
            }

            return false;
        }

        public static bool ClearPLCRequest(int stationNo)
        {
            var itemNo = WCSSql.GetItemNoByStationNo(stationNo);
            bool success = true;
            switch (stationNo)
            {
                case 100:
                case 1001:
                case 3001:
                case 3002:
                case 3003:
                case 3004:
                case 3005:
                case 3006:
                case 3007:
                case 3011:
                case 3012:
                    success = OPCExecute.AsyncWrite(hs_cpu, itemNo + 3, 0);
                    break;
                case 2001:
                case 2002:
                case 2004:
                case 2006:
                    success = OPCExecute.AsyncWrite(hs_cpu, itemNo + 4, 0);
                    break;
            }

            return success;
        }

        /// <summary>
        /// 写任务数据给设备
        /// </summary>
        /// <param name="dno"></param>
        /// <param name="pilerNo"></param>
        /// <param name="target"></param>
        public static bool WriteDeviceData(string dno, int pilerNo, int target)
        {
            bool success = true;
            var itemNo = WCSSql.GetItemNoByDeviceNo(dno);
            success &= OPCExecute.AsyncWrite(hs_cpu, itemNo + 1, pilerNo);
            success &= OPCExecute.AsyncWrite(hs_cpu, itemNo + 2, target);
            return success;
        }

        /// <summary>
        /// 重载
        /// </summary>
        /// <param name="stationNo"></param>
        /// <param name="pilerNo"></param>
        /// <param name="target"></param>
        public static bool WriteDeviceData(int stationNo, int pilerNo, int target)
        {
            var itemNo = WCSSql.GetItemNoByStationNo(stationNo);
            bool success = true;
            success &= OPCExecute.AsyncWrite(hs_cpu, itemNo + 1, pilerNo);
            success &= OPCExecute.AsyncWrite(hs_cpu, itemNo + 2, target);
            
            if (stationNo == 2001 || stationNo == 2002 || stationNo == 2004)
            {
                OPCExecute.AsyncWrite(hs_cpu, itemNo + 5, 1);
            }
            else if(stationNo == 2003)
            {
                OPCExecute.AsyncWrite(hs_cpu, itemNo + 4, 1);
            }
            else if (stationNo == 2005)
            {
                OPCExecute.AsyncWrite(hs_cpu, itemNo + 3, 1);
            }

            return true;
        }
        /// <summary>
        /// 读取拣选工位是否出垛完成（入库时的出垛）
        /// </summary>
        /// <param name="stationNo"></param>
        /// <returns></returns>
        public static bool ReadSortingStationOut(int stationNo)
        {
            var itemNo = WCSSql.GetItemNoByStationNo(stationNo);
            if (stationNo == 2001)
            {
                itemNo = 341;
                return bool.Parse(OPCExecute.AsyncRead(hs_cpu, itemNo).ToString());
            }
            else if (stationNo == 2002)
            {
                itemNo = 342;
                return bool.Parse(OPCExecute.AsyncRead(hs_cpu, itemNo).ToString());
            }
            else if (stationNo == 2003)
            {
                itemNo = 343;
                return bool.Parse(OPCExecute.AsyncRead(hs_cpu, itemNo).ToString());
            }
            else if (stationNo == 2004)
            {
                itemNo = 344;
                return bool.Parse(OPCExecute.AsyncRead(hs_cpu, itemNo).ToString());
            }
            else if (stationNo == 2005)
            {
                itemNo = 345;
                return bool.Parse(OPCExecute.AsyncRead(hs_cpu, itemNo).ToString());
            }
            else
            {
                return false;
            }

            //if (stationNo == 2001 || stationNo == 2002 || stationNo == 2004)
            //{
            //    var obj = OPCExecute.AsyncRead(hs_cpu, itemNo + 3).ToString();
            //    return bool.Parse(obj);
            //}
            //else if (stationNo == 2003)
            //{
            //    //var obj = OPCExecute.AsyncRead(hs_cpu, itemNo + 3).ToString();
            //    //return bool.Parse(OPCExecute.AsyncRead(hs_cpu, itemNo + 3).ToString());
            //    return bool.Parse(OPCExecute.AsyncRead(hs_cpu, itemNo + 249).ToString());
            //}
            //else if (stationNo == 2005)
            //{
            //    var obj = OPCExecute.AsyncRead(hs_cpu, itemNo + 4).ToString();
            //    return bool.Parse(OPCExecute.AsyncRead(hs_cpu, itemNo + 4).ToString());
            //}
            //else
            //{
            //    return false;
            //}
        }

        public static void SetSortingStationOut(int stationNo)
        {
            var itemNo = WCSSql.GetItemNoByStationNo(stationNo);
            if (stationNo == 2001)
            {
                itemNo = 341;
                OPCExecute.AsyncWrite(hs_cpu, itemNo, 1);
            }
            else if (stationNo == 2002)
            {
                itemNo = 342;
                OPCExecute.AsyncWrite(hs_cpu, itemNo, 1);

            }
            else if (stationNo == 2003)
            {
                itemNo = 343;
                OPCExecute.AsyncWrite(hs_cpu, itemNo, 1);

            }
            else if (stationNo == 2004)
            {
                itemNo = 344;
                OPCExecute.AsyncWrite(hs_cpu, itemNo, 1);

            }
            else if (stationNo == 2005)
            {
                itemNo = 345;
                OPCExecute.AsyncWrite(hs_cpu, itemNo, 1);

            }

        }

        public static bool ClearSortingStationOut(int stationNo)
        {
            var itemNo = WCSSql.GetItemNoByStationNo(stationNo);
            if (stationNo == 2001)
            {
                itemNo = 341;
                OPCExecute.AsyncWrite(hs_cpu, itemNo, 0);
                return true;
            }
            else if (stationNo == 2002)
            {
                itemNo = 342;
                OPCExecute.AsyncWrite(hs_cpu, itemNo, 0);
                return true;
            }
            else if (stationNo == 2003)
            {
                itemNo = 343;
                OPCExecute.AsyncWrite(hs_cpu, itemNo, 0);
                return true;
            }
            else if (stationNo == 2004)
            {
                itemNo = 344;
                OPCExecute.AsyncWrite(hs_cpu, itemNo, 0);
                return true;
            }
            else if (stationNo == 2005)
            {
                itemNo = 345;
                OPCExecute.AsyncWrite(hs_cpu, itemNo, 0);
                return true;
            }
            else
            {
                return false;
            }
            //bool success = true;
            //var itemNo = WCSSql.GetItemNoByStationNo(stationNo);
            //if (stationNo == 2001 || stationNo == 2002 || stationNo == 2004)
            //{
            //    var obj = OPCExecute.AsyncWrite(hs_cpu, itemNo + 3,0);

            //}
            //else if (stationNo == 2003)
            //{
            //    var obj = OPCExecute.AsyncWrite(hs_cpu, itemNo + 3,0);
            //    OPCExecute.AsyncWrite(hs_cpu, itemNo + 3, 0);
            //}
            //else if (stationNo == 2005)
            //{

            //    return OPCExecute.AsyncWrite(hs_cpu, itemNo + 4, 0);
            //}
            //else
            //{
            //    success = false;
            //}

            //return success;
        }
        public static int ReadPLCPilerNo(string dno)
        {
            var itemNo = WCSSql.GetItemNoByDeviceNo(dno);
            return int.Parse(OPCExecute.AsyncRead(hs_cpu, itemNo + 1).ToString());
        }

        public static DeviceData ReadDeviceDataByNo(string dno)
        {
            var itemNo = WCSSql.GetItemNoByDeviceNo(dno);
            var dd = new DeviceData();

            dd.Staus = OPCExecute.AsyncRead(hs_cpu, itemNo).ToString() == "True" ? true : false;
            var item_PilerNo = OPCExecute.AsyncRead(hs_cpu, itemNo + 1).ToString();
            var item_Target = OPCExecute.AsyncRead(hs_cpu, itemNo + 2).ToString();
            dd.PilerNo = item_PilerNo == "" ? 0 : int.Parse(item_PilerNo);
            dd.Target = item_Target == "" ? 0 : int.Parse(item_Target);

            return dd;
        }

        public static int ReadPilerNoByStationNo(int stationNo)
        {
            var itemNo = WCSSql.GetItemNoByStationNo(stationNo);
            return int.Parse(OPCExecute.AsyncRead(hs_cpu, itemNo + 1).ToString());
        }

        /// <summary>
        /// 设备监控
        /// </summary>
        public static void MonitorDevice()
        {
            var dt = WCSSql.GetDeviceData();
            string item_PilerNo = "";
            string item_Target = "";
            int item_Staus = 0;
            var itemNo = 0;
            StringBuilder sb = new StringBuilder();

            foreach (DataRow dr in dt.Rows)
            {
                itemNo = int.Parse(dr["ItemNo"].ToString());
                item_Staus = OPCExecute.AsyncRead(hs_cpu, itemNo).ToString() == "True" ? 1 : 0;
                item_PilerNo = OPCExecute.AsyncRead(hs_cpu, itemNo + 1).ToString();
                item_Target = OPCExecute.AsyncRead(hs_cpu, itemNo + 2).ToString();

                sb.Append($"update HGWCSBC.[dbo].[WCS_DeviceMonitor] set PilerNo={(item_PilerNo == "" ? "0" : item_PilerNo)},[Target]={(item_Target == "" ? "0" : item_Target)},Staus={item_Staus} where ItemNo={itemNo};");
            }

            Sql.ExecSQL(sb.ToString());
        }

        /// <summary>
        /// 监控设备的错误信息
        /// </summary>
        /// <param name="dno"></param>
        /// <returns></returns>
        public static string ReadDeiviceErrorMsg(string dno)
        {
            var msg = "";
            var itemNo = WCSSql.GetItemNoByDeviceNo2(dno);
            if (dno.Substring(0, 2) == "DS")
            {
                //DS 有四种故障
                if (bool.Parse(OPCExecute.AsyncRead(error_cpu, itemNo).ToString()))
                {
                    msg += "超时|";
                }
                if (bool.Parse(OPCExecute.AsyncRead(error_cpu, itemNo + 1).ToString()))
                {
                    msg += "LT变频器故障|";
                }
                if (bool.Parse(OPCExecute.AsyncRead(error_cpu, itemNo + 2).ToString()))
                {
                    msg += "GT变频器故障|";
                }
                if (bool.Parse(OPCExecute.AsyncRead(error_cpu, itemNo + 3).ToString()))
                {
                    msg += "SJ变频器故障|";
                }
            }
            else
            {
                //LT GT 有两种故障
                if (bool.Parse(OPCExecute.AsyncRead(error_cpu, itemNo).ToString()))
                {
                    msg += "超时|";
                }
                if (bool.Parse(OPCExecute.AsyncRead(error_cpu, itemNo + 1).ToString()))
                {
                    msg += "变频器故障|";
                }
            }

            if (msg.Length > 0)
            {
                msg = msg.Remove(msg.Length - 1, 1);
            }

            return msg;
        }
        #endregion

        #region RGV
        public static bool RGVCanDo()
        {
            return IsRGVAuto() && IsRGVActication() && IsRGVFree();
        }

        /// <summary>
        /// RGV是否自动
        /// </summary>
        /// <returns></returns>
        public static bool IsRGVAuto()
        {
            return bool.Parse(OPCExecute.AsyncRead(rgv_cpu, 5).ToString());
        }

        /// <summary>
        /// RGV是否激活
        /// </summary>
        /// <returns></returns>
        public static bool IsRGVActication()
        {
            return bool.Parse(OPCExecute.AsyncRead(rgv_cpu, 4).ToString());
        }

        /// <summary>
        /// RGV是否空闲
        /// </summary>
        /// <returns></returns>
        public static bool IsRGVFree()
        {
            return bool.Parse(OPCExecute.AsyncRead(rgv_cpu, 6).ToString());
        }

        /// <summary>
        /// RGV任务完成信号
        /// </summary>
        /// <returns></returns>
        public static bool ReadRGVFinished()
        {
            return bool.Parse(OPCExecute.AsyncRead(rgv_cpu, 13).ToString());
        }

        /// <summary>
        /// RGV任务接收信号
        /// </summary>
        /// <returns></returns>
        public static bool ReadRGVAccepted()
        {
            return bool.Parse(OPCExecute.AsyncRead(rgv_cpu, 12).ToString());
        }

        public static int ReadRGVPilerNo()
        {
            return int.Parse(OPCExecute.AsyncRead(rgv_cpu, 9).ToString());
        }

        public static bool ReadRGVTaskStatus()
        {
            return bool.Parse(OPCExecute.AsyncRead(rgv_cpu, 3).ToString());
        }

        public static string ReadRGVPostion()
        {
            return OPCExecute.AsyncRead(rgv_cpu, 8).ToString();
        }

        public static string ReadRGVFromPostion()
        {
            return OPCExecute.AsyncRead(rgv_cpu, 10).ToString();
        }

        public static string ReadRGVToPostion()
        {
            return OPCExecute.AsyncRead(rgv_cpu, 11).ToString();
        }

        public static bool ClearRGVTask()
        {
            bool success = true;
            success &= OPCExecute.AsyncWrite(rgv_cpu, 0, 0);
            success &= OPCExecute.AsyncWrite(rgv_cpu, 1, 0);
            success &= OPCExecute.AsyncWrite(rgv_cpu, 2, 0);
            return success;
        }

        public static bool FeedBackRGV()
        {
            //ClearRGVTask();
            return OPCExecute.AsyncWrite(rgv_cpu, 3, 0);
        }

        /// <summary>
        /// 给RGV写任务
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="pilerNo"></param>
        /// <returns></returns>
        public static bool WriteRGVTask(int from, int to, int pilerNo)
        {
            OPCExecute.AsyncWrite(rgv_cpu, 0, pilerNo);
            OPCExecute.AsyncWrite(rgv_cpu, 1, from);
            OPCExecute.AsyncWrite(rgv_cpu, 2, to);

            int ct = 0; //最多等3s
            while (IsRGVFree())
            {
                Thread.Sleep(500);
                ct++;
                if (ct >= 20)
                    break;
            }

            if (ReadRGVAccepted())
            {
                ClearRGVTask();
                OPCExecute.AsyncWrite(rgv_cpu, 3, 1);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取RGV的错误信息
        /// </summary>
        /// <returns></returns>
        public static string GetRGVErrorMsg()
        {
            var code_10 = int.Parse(OPCExecute.AsyncRead(rgv_cpu, 14).ToString());
            var code_2 = Convert.ToString(code_10, 2);//十进制转二进制
            var msglst = "";
            // 1000
            for (int i = 0; i < code_2.Length; i++)
            {
                if (code_2[code_2.Length - 1 - i] == '1')
                {
                    msglst += WCSSql.GetRGVErrorCommentByIndex(i) + "|";
                }
            }

            return msglst;
        }

        /// <summary>
        /// 获取机械手的错误信息
        /// </summary>
        /// <param name="jno"></param>
        /// <returns></returns>
        public static string GetJXSErrorMsg(int jno)
        {
            var itemNo = 0;
            if (jno == 1)
            {
                itemNo = 34;
            }
            else if (jno == 2)
            {
                itemNo = 45;
            }
            else if (jno == 3)
            {
                itemNo = 50;
            }
            var code_10 = int.Parse(OPCExecute.AsyncRead(rgv_cpu, itemNo).ToString());
            return GetMSgByNo(code_10, jno);
        }

        static string GetMSgByNo(int errorNo,int jno)
        {
            if (errorNo == 0) { return ""; }
            if (jno == 1 && errorNo == 1) { return "取放工位号读取错误"; }
            if (jno == 1) { errorNo--; }
            switch (errorNo)
            {
                case 1:
                    return "横X轴报错";
                case 2:
                    return "纵Z轴报错";
                case 3:
                    return "真空信号负压报警";
                case 4:
                    return "横X轴硬限位到位";
                case 5:
                    return "纵Y轴硬限位到位";
                case 6:
                    return "横X轴软限位到位";
                case 7:
                    return "横Y轴软限位到位";
                case 8:
                    return "放板数计算有误";
                case 9:
                    return "急停";
                default:
                    return "未知错误";
            }
        }
        #endregion

        #region 机械手
        /// <summary>
        /// 读取机械手是否空闲
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        public static bool RMFree(int no)
        {
            var itemNo = 0;
            if (no == 1)
            {
                itemNo = 35;
            }
            else if (no == 2)
            {
                itemNo = 44;
            }
            else if (no == 3)
            {
                itemNo = 49;
            }

            return OPCExecute.AsyncRead(rgv_cpu, itemNo).ToString() == "1";
        }

        /// <summary>
        /// 读取机械手是否激活
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        public static bool RMActivity(int no)
        {
            var itemNo = 0;
            if (no == 1)
            {
                itemNo = 36;
            }
            else if (no == 2)
            {
                itemNo = 42;
            }
            else if (no == 3)
            {
                itemNo = 47;
            }

            return bool.Parse(OPCExecute.AsyncRead(rgv_cpu, itemNo).ToString());
        }

        /// <summary>
        /// 读取机械手是否自动
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        public static bool RMAuto(int no)
        {
            var itemNo = 0;
            if (no == 1)
            {
                itemNo = 33;
            }
            else if (no == 2)
            {
                itemNo = 43;
            }
            else if (no == 3)
            {
                itemNo = 48;
            }

            return OPCExecute.AsyncRead(rgv_cpu, itemNo).ToString() == "1";
        }

        /// <summary>
        /// 读取RGV抓板工位
        /// </summary>
        /// <returns></returns>
        public static int RMFrom()
        {
            return int.Parse(OPCExecute.AsyncRead(rgv_cpu, 37).ToString());
        }

        /// <summary>
        /// 读取RGV放板工位
        /// </summary>
        /// <returns></returns>
        public static int RMTo()
        {
            return int.Parse(OPCExecute.AsyncRead(rgv_cpu, 38).ToString());
        }

        /// <summary>
        /// 读取GT308空垫板数量
        /// </summary>
        /// <returns></returns>
        public static string ReadGT308Borads()
        {
            return OPCExecute.AsyncRead(rgv_cpu, 46).ToString();
        }

        /// <summary>
        /// 读取GT318空垫板数量
        /// </summary>
        /// <returns></returns>
        public static string ReadGT318Borads()
        {
            return OPCExecute.AsyncRead(rgv_cpu, 51).ToString();
        }

        public static bool RMCanDo(int no)
        {
            return RMFree(no) && RMActivity(no) && RMAuto(no);
        }

        /// <summary>
        /// 机械手已接收任务
        /// </summary>
        /// <returns></returns>
        public static bool ReadMAcceptTask()
        {
            return bool.Parse(OPCExecute.AsyncRead(rgv_cpu, 39).ToString());
        }

        public static void ClearMhTask()
        {
            OPCExecute.AsyncWrite(rgv_cpu, 20, 0);
            OPCExecute.AsyncWrite(rgv_cpu, 21, 0);
        }

        /// <summary>
        /// 给机械手写任务
        /// </summary>
        /// <param name="fromStation"></param>
        /// <param name="toStation"></param>
        /// <returns></returns>
        public static bool WriteToMainpulator(int fromStation, int toStation)
        {
            
            OPCExecute.AsyncWrite(rgv_cpu, 20, fromStation);
            OPCExecute.AsyncWrite(rgv_cpu, 21, toStation);
            
            int ct = 0; //最多等3s
            while (RMFree(1))
            {
                Thread.Sleep(100);

                if (ct >= 30)
                    break;

                ct++;
            }

            var rlt = ReadMAcceptTask();

            OPCExecute.AsyncWrite(rgv_cpu, 20, 0);
            OPCExecute.AsyncWrite(rgv_cpu, 21, 0);
            WCSSql.AddSortStationCount(fromStation, toStation);

            return rlt;
        }

        /// <summary>
        /// 机械手是否完成
        /// </summary>
        /// <returns></returns>
        public static bool RStop()
        {
            return bool.Parse(OPCExecute.AsyncRead(rgv_cpu, 52).ToString());
        }

        /// <summary>
        /// 为了防止机械手重复抓板，先清除WCS写的任务
        /// </summary>
        public static void ClearMainpulatorTask()
        {
            OPCExecute.AsyncWrite(rgv_cpu, 20, 0);
            OPCExecute.AsyncWrite(rgv_cpu, 21, 0);
        }

        /// <summary>
        /// 获取设备GT216，GT217上共缓存的垛数
        /// </summary>
        /// <returns></returns>
        public static int ReadEmptyBuffersCount()
        {
            var itemNo = WCSSql.GetItemNoByDeviceNo("GT217");
            return int.Parse(OPCExecute.AsyncRead(hs_cpu, itemNo + 7).ToString());
        }

        /// <summary>
        /// 读取入库指令2001,2002,2003,2004,2005
        /// </summary>
        /// <param name="station"></param>
        /// <returns></returns>
        public static bool ReadInWareCmd(int station)
        {
            var itemNo = WCSSql.GetItemNoByStationNo(station);

            switch (station)
            {
                case 2001:
                case 2002:
                case 2004:
                    return bool.Parse(OPCExecute.AsyncRead(hs_cpu, itemNo + 5).ToString());
                case 2003:
                    return bool.Parse(OPCExecute.AsyncRead(hs_cpu, itemNo + 4).ToString());
                case 2005:
                    return bool.Parse(OPCExecute.AsyncRead(hs_cpu, itemNo + 3).ToString());
            }

            return false;
        }

        /// <summary>
        /// 读取GT217退回指令
        /// </summary>
        /// <returns></returns>
        public static bool ReadGT217GoBackCmd()
        {
            var itemNo = WCSSql.GetItemNoByDeviceNo("GT217");
            return bool.Parse(OPCExecute.AsyncRead(hs_cpu, itemNo + 5).ToString());
        }

        /// <summary>
        /// 写GT217退回指令
        /// </summary>
        public static void WriteGT217GoBackCmd()
        {
            var itemNo = WCSSql.GetItemNoByDeviceNo("GT217");
            OPCExecute.AsyncWrite(hs_cpu, itemNo + 5, 1);
        }

        /// <summary>
        /// 读取GT216前进指令
        /// </summary>
        /// <returns></returns>
        public static bool ReadGT216GoHeadCmd()
        {
            var itemNo = WCSSql.GetItemNoByDeviceNo("GT216");
            return bool.Parse(OPCExecute.AsyncRead(hs_cpu, itemNo + 5).ToString());
        }

        /// <summary>
        /// 写GT216前进指令
        /// </summary>
        public static void WriteGT216GoBackCmd()
        {
            var itemNo = WCSSql.GetItemNoByDeviceNo("GT216");
            OPCExecute.AsyncWrite(hs_cpu, itemNo + 5, 1);
        }

        /// <summary>
        /// 给拣选工位写花色2001,2002,2004
        /// </summary>
        /// <param name="stationNo"></param>
        /// <param name="ProductCode"></param>
        public static bool WriteProductCodeToStaion(int stationNo, string ProductCode)
        {
            var itemNo = WCSSql.GetItemNoByStationNo(stationNo);
            return OPCExecute.AsyncWrite(hs_cpu, itemNo + 6, ProductCode);
        }

        /// <summary>
        /// 读取拣选工位的花色信息2001,2002,2004
        /// </summary>
        /// <param name="stationNo"></param>
        /// <returns></returns>
        public static string ReadStaionProductCode(int stationNo)
        {
            var itemNo = WCSSql.GetItemNoByStationNo(stationNo);
            return OPCExecute.AsyncRead(hs_cpu, itemNo + 6).ToString();
        }

        //给机械手写任务
        public static bool TestWriteToMainpulator(int fromStation, int toStation)
        {
            OPCExecute.AsyncWrite(rgv_cpu, 20, fromStation);
            OPCExecute.AsyncWrite(rgv_cpu, 21, toStation);

            int ct = 0; //最多等3s
            while (RMFree(1))
            {
                Thread.Sleep(100);

                if (ct >= 30)
                    break;

                ct++;
            }

            var rlt = ReadMAcceptTask();
            if (rlt)
            {
                OPCExecute.AsyncWrite(rgv_cpu, 20, 0);
                OPCExecute.AsyncWrite(rgv_cpu, 21, 0);
            }
            return rlt;
        }


        /// <summary>
        /// 给工位写板件数量2001,2002,2003,2004,2005,2006
        /// </summary>
        /// <param name="stationNo"></param>
        /// <param name="Count"></param>
        public static bool WriteBoardCountToStation(int stationNo, int Count)
        {
            bool success = true;
            var itemNo = 0;
            if (stationNo < 2006)
            {
                itemNo = stationNo - 2001 + 15;
                success &= OPCExecute.AsyncWrite(rgv_cpu, itemNo, Count);
                success &= OPCExecute.AsyncWrite(rgv_cpu, 27, 1); //给Rinitial写1

                int ct = 0; //等待3S
                while (RRinitial() == false)
                {
                    Thread.Sleep(100);

                    if (ct >= 30)
                    {
                        return false;
                    }

                    ct++;
                }

                if (RRinitial())
                {
                    var ret = OPCExecute.AsyncWrite(rgv_cpu, 27, 0); //给Rinitial写0
                    ret = ret ? ret : OPCExecute.AsyncWrite(rgv_cpu, 27, 0);
                    ret = OPCExecute.AsyncWrite(rgv_cpu, itemNo, 0);
                    ret = ret ? ret : OPCExecute.AsyncWrite(rgv_cpu, itemNo, 0);
                }

                
            }
            else if (stationNo == 2006)
            {
                itemNo = WCSSql.GetItemNoByStationNo(stationNo);
                success &= OPCExecute.AsyncWrite(hs_cpu, itemNo + 6, Count);
            }
            return success;
        }


        /// <summary>
        /// 给工位写板件数量2001,2002,2003,2004,2005,2006
        /// </summary>
        /// <param name="stationNo"></param>
        /// <param name="Count"></param>
        public static bool WriteBoradsCountToStaion(int stationNo, int Count)
        {
            bool success = true;
            var itemNo = 0;
            if (stationNo < 2006)
            {
                itemNo = stationNo - 2001 + 15;
                OPCExecute.AsyncWrite(rgv_cpu, itemNo, Count);
                OPCExecute.AsyncWrite(rgv_cpu, 27, 1); //给Rinitial写1

                int ct = 0; //等待3S
                while (RRinitial() == false)
                {
                    Thread.Sleep(100);

                    if (ct >= 30)
                    {
                        break;
                    }

                    ct++;
                }

                if (RRinitial())
                {
                    OPCExecute.AsyncWrite(rgv_cpu, 27, 0); //给Rinitial写0
                    OPCExecute.AsyncWrite(rgv_cpu, itemNo, 0);
                }

                //设置WCS存储的板件数
                WCSSql.SetSortStationCount(stationNo, Count);
            }
            else if (stationNo == 2006)
            {
                itemNo = WCSSql.GetItemNoByStationNo(stationNo);
                OPCExecute.AsyncWrite(hs_cpu, itemNo + 6, Count);
            }

            return true;
        }

        /// <summary>
        /// 给机械板件手清除指令2001,2002,2003,2004,2005
        /// </summary>
        /// <param name="stationNo"></param>
        public static bool ClearSortStation(int stationNo)
        {
            bool success = true;
            var itemNo = stationNo - 2001 + 22;
            success &= OPCExecute.AsyncWrite(rgv_cpu, itemNo, 1);
            itemNo = stationNo - 2001 + 15;
            OPCExecute.AsyncWrite(rgv_cpu, itemNo, 0);
            //Item item = OpcBaseManage.opcBase.GetItem(0, itemNo);
            //item.Write(1);

            //等待机械手清除完成反馈信号
            int ct = 0;
            while (!RClearFeed())
            {
                Thread.Sleep(100);

                if (ct >= 30)
                {
                    success = false;
                    break;
                }

                ct++;
            }

            //机械手有反馈后，清掉指令
            WCSSql.SetSortStationCount(stationNo, 0);
            if (RClearFeed())
            {
                success &= OPCExecute.AsyncWrite(rgv_cpu, itemNo, 0);
            }

            return success;
        }

        /// <summary>
        /// 读取机械手清除完成信号
        /// </summary>
        /// <returns></returns>
        public static bool RClearFeed()
        {
            return bool.Parse(OPCExecute.AsyncRead(rgv_cpu, 41).ToString());
        }

        /// <summary>
        /// 读取机械手的反馈信号
        /// </summary>
        /// <returns></returns>
        public static bool RRinitial()
        {
            return bool.Parse(OPCExecute.AsyncRead(rgv_cpu, 40).ToString());
        }

        /// <summary>
        /// 告诉PLC是否有上保护板3001,3002,3003,3004,3005,3006,3007
        /// </summary>
        /// <param name="stationNo"></param>
        /// <param name="flg"></param>
        public static bool WriteUpToCutStation(int stationNo, int flg)
        {
            var itemNo = WCSSql.GetItemNoByStationNo(stationNo);
            return OPCExecute.AsyncWrite(hs_cpu, itemNo + 4, flg);
        }

        /// <summary>
        /// 读取2001,2002,2003,2004,2005,2006工位的板件数量(包含上下保护板)
        /// </summary>
        /// <param name="stationNo"></param>
        /// <returns></returns>
        public static int ReadBoradsCount(int stationNo)
        {
            if (stationNo == 2006)
            {
                var itemNo = WCSSql.GetItemNoByDeviceNo("GT216");
                return int.Parse(OPCExecute.AsyncRead(hs_cpu, itemNo + 6).ToString());
            }
            else
            {
                return int.Parse(OPCExecute.AsyncRead(rgv_cpu, stationNo - 2001 + 28).ToString());
            }
        }
        #endregion
    }
}
