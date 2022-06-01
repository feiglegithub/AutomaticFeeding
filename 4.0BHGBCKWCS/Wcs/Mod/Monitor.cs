using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WCS
{
    class Monitor
    {
        static int[] HSarray = { 2,2,2,2,2,2,2,2,2,2,2 };
        public static void HsMonitor()
        {
            //List<int> dmgInfoList = new List<int>();

            //dmgInfoList.Add(OpcHs.ReadError_5(0));
            //dmgInfoList.Add(OpcHs.ReadError_5(1));
            //dmgInfoList.Add(OpcHs.ReadError_5(2));
            //dmgInfoList.Add(OpcHs.ReadError_5(3));
            //dmgInfoList.Add(OpcHs.ReadError_5(4));
            //dmgInfoList.Add(OpcHs.ReadError_5(5));
            //dmgInfoList.Add(OpcHs.ReadError_5(6));
            //dmgInfoList.Add(OpcHs.ReadError_5(7));
            //dmgInfoList.Add(OpcHs.ReadError_5(8));
            //dmgInfoList.Add(OpcHs.ReadError_5(9));
            //dmgInfoList.Add(OpcHs.ReadError_5(10));

            //HSErrorLog(dmgInfoList);

            //MonitorSql.ChangeHsInfo(dmgInfoList);

        }

        //地面柜错误日志
        static DateTime[] HsTime = {
                            DateTime.MinValue,DateTime.MinValue,DateTime.MinValue,DateTime.MinValue
                            ,DateTime.MinValue,DateTime.MinValue,DateTime.MinValue,DateTime.MinValue
                            ,DateTime.MinValue,DateTime.MinValue,DateTime.MinValue};
        static void HSErrorLog(List<int> dList) 
        {
            for (int i = 0; i < dList.Count; i++) 
            {
                if (dList[i] != 2) 
                {
                    HSarray[i] = dList[i];
                    if (HsTime[i] == DateTime.MinValue)
                    {
                        HsTime[i] = Sql.GetTime();
                    }
                }

                if (HSarray[i] != 2 && dList[i] == 2 && GetHsState()) 
                {
                    //Sql.InsertErrorLog(string.Format("{0}地面柜故障：{1}", GetHs1NameByNum(i), GetDmgError(HSarray[i])), HsTime[i]);
                    HSarray[i] = 2;
                    HsTime[i] = DateTime.MinValue;
                }
            }
        }

        static bool GetHsState()
        {

            if (OpcBaseManage.opcBase.GetCpu(0).isConnect == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        static string GetDmgError(int type)
        {
            switch (type)
            {
                case 1:
                    return "急停";
                case 2:
                    return "";
                case 3:
                    return "手动模式";
                case 4:
                    return "线体故障";
                default:
                    return "未定义故障";
            }
        }

        public static void HsInError()
        {
            //try
            //{
            //    int error = OpcHs.ReadError_301();
            //    ShowInError(error,301);

            //    //int error1 = OpcHs.ReadError_112();
            //    //ShowInError(error1,112);

            //    //int error2 = OpcHs.ReadError_206();
            //    //ShowInError(error2,206);
            //}
            //catch { }
        }

        static void ShowInError(int error, int port)
        {
            //string pallet = OpcHs.InWareReadPallet(port);
            //switch (error)
            //{
            //    case 0:
            //        break;
            //    case 1:
            //        LogWrite.WriteError("入口：" + port + "无法识别条码!");
            //        break;
            //    case 2:
            //        LogWrite.WriteError("入口：" + port + "货物超长! " + (pallet == "" ? "" : ("托盘号：" + pallet)));
            //        break;
            //    case 3:
            //        LogWrite.WriteError("入口：" + port + "货物超宽! " + (pallet == "" ? "" : ("托盘号：" + pallet)));
            //        break;
            //    case 4:
            //        LogWrite.WriteError("入口：" + port + "货物超高! " + (pallet == "" ? "" : ("托盘号：" + pallet)));
            //        break;
            //    case 5:
            //        LogWrite.WriteError("入口：" + port + "货物超重! " + (pallet == "" ? "" : ("托盘号：" + pallet)));
            //        break;
            //    default:
            //        LogWrite.WriteError("入口：" + port + "未定义错误!");
            //        break;
            //}
        }

        public static void RGVMonitor() 
        {
            //bool pos1 = OpcHs.GetRGVPos1();
            //bool pos2 = OpcHs.GetRGVPos2();
            //bool pos3 = OpcHs.GetRGVPos3();
            //bool pos4 = OpcHs.GetRGVPos4();

            //bool auto_started = OpcHs.Get_auto_started();
            //bool manual_mode = OpcHs.Get_manual_mode();
            //bool rgv_idle = OpcHs.Get_rgv_idle();
            //bool ph_loading = OpcHs.Get_ph_loading();
            //int laser_value = OpcHs.Get_laser_value();
            //string errormsg = OpcHs.GetRGVErrorMsg();
            //int des_word = OpcHs.Get_Des_Word();

            //LogRGVErrorMsg(errormsg);

            //MonitorSql.ChangeRgvInfo(pos1, pos2, pos3, pos4, auto_started, manual_mode, rgv_idle, ph_loading, laser_value,des_word,errormsg);
        }

        static string rgv_state = "";
        static DateTime rgv_start_errortime = DateTime.MinValue;
        static void LogRGVErrorMsg(string rmsglst)
        {
            if (rmsglst != "")
            {
                rgv_state = rmsglst;
                if (rgv_start_errortime == DateTime.MinValue)
                {
                    rgv_start_errortime = Sql.GetTime();
                }
            }

            if (rgv_state != "" && rmsglst == "")
            {
                string msgs = "";
                foreach (string s in rgv_state.Split('|'))
                {
                    //msgs = msgs + MonitorSql.GetRGVErrorMsg(s) + "|";
                }
                if (msgs.Length > 0) 
                {
                    msgs = msgs.Remove(msgs.Length - 1, 1);
                }

                Sql.InsertErrorLog(string.Format("1号RGV故障：{0}", msgs), rgv_start_errortime);
                rgv_start_errortime = DateTime.MinValue;
                rgv_state = "";
            }
        }

        static List<string> ddjErrorList = new List<string>();
        static long[] ddjarray = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        static DateTime[] ddjSTime = { DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue };
        static string[] ddjerrorlist = { "", "", "", "", "", "", "", "", "" };
        public static void ScMonitor()
        {
            List<DdjInfo> ddjInfoList = new List<DdjInfo>();

            ddjErrorList = MonitorSql.GetDdjErrorList();


            for (int ddj = 1; ddj <= AppCommon.DdjNum; ddj++)
            {
                DdjInfo ds = new DdjInfo();
                ds.No = ddj;
                ds.Pallet = OpcSc.RTask(ddj);
                ds.TaskType = OpcSc.RTaskType(ddj);
                ds.IsAuto = OpcSc.IsAuto(ddj);
                ds.IsFree = OpcSc.RIsFree(ddj);
                ds.IsFinished = OpcSc.IsFinished(ddj);
                ds.FromWare = OpcSc.RFrom(ddj);
                ds.ToWare = OpcSc.RTo(ddj);
                ds.CurrentLocation = OpcSc.RLocation(ddj);
                //ds.ErrorMsg = OpcSc.GetError(ddj);
                ds.ErrorList = OpcSc.GetSCErrorMsg(ddj);
                ds.IsActivation = OpcSc.RIsActivation(ddj);
                //ds.IsFirstOutFree = OpcHs.IsStationFree_1(ddj);
                //ds.IsSecondInFree = OpcHs.IsStationIn_2(ddj);
                //ds.IsSecondOutFree = OpcHs.IsStationFree_2(ddj);

                ddjInfoList.Add(ds);

                //LogDDJErrorMsg(ddj, ds.ErrorMsg);
                LogDDJErrorList(ddj, ds.ErrorList);
            }

            MonitorSql.ChangeScInfo(ddjInfoList);
        }


        #region 堆垛机错误日志
        static void LogDDJErrorMsg(int ddj, long ErrorMsg)
        {
            if (ErrorMsg != 0)
            {
                ddjarray[ddj - 1] = ErrorMsg;
                if (ddjSTime[ddj - 1] == DateTime.MinValue)
                {
                    ddjSTime[ddj - 1] = Sql.GetTime();
                }
            }
            if (ddjarray[ddj - 1] != 0 && ErrorMsg == 0)
            {
                Sql.InsertErrorLog(string.Format("{0}号堆垛机故障：{1} 托盘号：{2}", ddj, GetErrorMsg(ddjarray[ddj - 1]), OpcSc.RTask(ddj)), ddjSTime[ddj - 1]);
                ddjarray[ddj - 1] = 0;
                ddjSTime[ddj - 1] = DateTime.MinValue;
            }
        }

        static void LogDDJErrorList(int ddj, string msglst) 
        {
            if (msglst != "") 
            {
                ddjerrorlist[ddj - 1] = msglst;
                if (ddjSTime[ddj - 1] == DateTime.MinValue)
                {
                    ddjSTime[ddj - 1] = Sql.GetTime();
                }
            }
            if (ddjerrorlist[ddj - 1] != "" && msglst == "")
            {
                string msg = "";
                string str = ddjerrorlist[ddj - 1];
                str = str.Remove(str.Length - 1, 1);
                foreach (string s in str.Split(','))
                {
                    int index = int.Parse(s) - 34 - 1;
                    msg = msg + ddjErrorList[index] + "|";
                }

                if (msg.Length > 0)
                {
                    msg = msg.Remove(msg.Length - 1, 1);
                }

                Sql.InsertErrorLog(string.Format("{0}号堆垛机故障：{1} 托盘号：{2}", ddj, msg, OpcSc.RTask(ddj)), ddjSTime[ddj - 1]);

                ddjerrorlist[ddj - 1] = "";
                ddjSTime[ddj - 1] = DateTime.MinValue;
            }
        }

        static string GetErrorMsg(long errorCode)
        {
            if (errorCode == 0)
                return "";
            string code = Convert.ToString(errorCode, 2);

            return ddjErrorList[code.Length - 1];
        }
        #endregion
    }
}
