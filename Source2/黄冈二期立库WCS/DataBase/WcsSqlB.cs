using CommomLY1._0;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using WCS.model;

namespace WCS.DataBase
{
    public class WcsSqlB
    {
        //连接字符串
        static string connstr = AppCommon.WCSConnstr;

        /// <summary>
        /// 获取服务器时间
        /// </summary>
        /// <returns></returns>
        //public static DateTime DateTimeNow()
        //{
        //    return SqlBase.GetServiceDateTime(connstr2);
        //}

        #region WMS UpdateTask
        //更改WCS任务状态，并回写给WMS
        public static string UpdateWMSTask(string seqid, int status, int tasktype = 0)
        {
            WebService.interface2wcsServiceClient client = new WebService.interface2wcsServiceClient();
            WebService.updateTaskStatus uts = new WebService.updateTaskStatus();
            uts.SEQID = seqid;
            uts.NWKSTATUS = status;
            uts.NWKSTATUSSpecified = true;
            uts.NWKMESSAGE = "WCS更新成功";
            WebService.updateTaskStatusResponse result = client.updateTaskStatus(uts);
            if (result.STATUS == 0)
            {
                string sql = "";
                if (status == 12)
                {
                    //告诉WMS托盘已过汇流口
                    sql = string.Format(" UPDATE Wcs_Task SET NwkStatus = '{0}',Date_ext2=getdate()  WHERE seqid = '{1}' AND NwkStatus < 98 ", 32, seqid);
                }
                else if (status == 10)
                {
                    //告诉WMS任务开始执行
                    sql = string.Format(" UPDATE Wcs_Task SET NwkStatus = '{0}',DStartDate=getdate() WHERE seqid = '{1}' AND NwkStatus < 98 ", tasktype == 1 ? 20 : 30, seqid);
                }
                else if (status == 1)
                {
                    //还原任务
                    sql = string.Format(" UPDATE Wcs_Task SET NwkStatus = '{0}'  WHERE seqid = '{1}' AND NwkStatus < 98 ", 1, seqid);
                }
                SqlBase.ExecSql(sql, connstr);
                return "";
            }
            else
                return result.STATUS + result.MESSAGE;
        }
        #endregion

        #region WMS UpdateScStatus
        public static string UpdateScStatus(string ddjNo, int status, string num)
        {
            WebService.interface2wcsServiceClient client = new WebService.interface2wcsServiceClient();
            WebService.updateScStatus uss = new WebService.updateScStatus();
            uss.SEQID = ddjNo;
            uss.NWKSTATUS = status;
            uss.NWKSTATUSSpecified = true;
            uss.NWKMESSAGE = "WCS更新成功";
            WebService.updateScStatusResponse result = client.updateScStatus(uss);
            if (result.STATUS == 0)
            {
                string sql = $" update Wcs_DdjStatue set Status = {status},UpdateDate = getdate() where no = {num} ";
                SqlBase.ExecSql(sql ,connstr);
                return "";
            }
            else
                return result.STATUS + result.MESSAGE;
        }
        #endregion

        #region CreateTask
        static string WMSCreateTask(int taskType, string pallet, int station, int height)
        {
            try
            {
                WebService.interface2wcsServiceClient client = new WebService.interface2wcsServiceClient();
                WebService.applyTask aTask = new WebService.applyTask();
                aTask.HEIGHT = height;
                aTask.HEIGHTSpecified = true;
                aTask.NPALLETID = pallet;
                aTask.NORDID = taskType;
                aTask.NORDIDSpecified = true;
                aTask.NOPTSTATION = station.ToString();
                aTask.CSYSTYPE = "WCS";
                aTask.CWHID = "HBB_ZNC";

                if (taskType == 5)  //空托盘入库
                {
                    aTask.QUANTITYSpecified = true;
                    aTask.QUANTITY = 5;
                }

                WebService.applyTaskResponse result = client.applyTask(aTask);
                int status = result.STATUS;
                string error = result.MESSAGE;

                if (status != 0 /*|| error != null*/)
                    throw new Exception(status + error);
                else
                {
                    if (result.DATA.SEQID == null) { return $"WMS返回的SEQID是NULL！请求类型：{taskType}"; }

                    string sql = string.Format("select count(1) from [dbo].[Wcs_Task] where SeqID={0}", result.DATA.SEQID);
                    if (Convert.ToInt32(SqlBase.GetSqlScalar(sql, connstr)) > 0)
                    {
                        return "已存在任务列表中！";
                    }
                    InsertTask(result.DATA, height);
                    return "";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        static bool IsInWare(int nordid)
        {
            bool isInware;
            switch (nordid)
            {
                case 1:
                case 2:
                case 5:
                    isInware = true;
                    break;
                case 3:
                case 4:
                case 6:
                    isInware = false;
                    break;
                default:
                    isInware = false;
                    break;
            }
            return isInware;
        }

        static int GetRoadway(bool isInware, string fromWare, string toWare)
        {
            int roadway = 0;
            if (!isInware)
            {
                if (!string.IsNullOrEmpty(fromWare))
                {
                    roadway = int.Parse(fromWare.Substring(1, 2));
                    roadway = (roadway + 1) / 2;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(toWare))
                {
                    roadway = int.Parse(toWare.Substring(1, 2));
                    roadway = (roadway + 1) / 2;
                }
            }
            return roadway;
        }

        static void InsertTask(WebService.TaskInfo rt, int height)
        {
            int nordid = GI(rt.NORDID);
            bool isInware = IsInWare(nordid);
            int roadway = GetRoadway(isInware, rt.FROM_LOCATOR_CODE, rt.TO_LOCATOR_CODE);

            string sql = string.Format(@"INSERT INTO Wcs_Task
	            (SeqID,NordID,NWorkID,NPalletID,CPosidFrom,CPosidTo,
	            Roadway,NoptType,NoptStation,Npri,NlotID,
                CPackType,NwkStatus,CFinishMode,Cuser,Quantitity,
                Height,DoptDate,CustomerName)
	            VALUES
	            ('{0}','{1}','{2}','{3}','{4}',
	            '{5}','{6}' ,'{7}','{8}','{9}',
	            '{10}','{11}','{12}','{13}','{14}',
	            '{15}','{16}',GETDATE(),'{17}')", GS(rt.SEQID),
                GI(rt.NORDID), GI(rt.NWORKID), GS(rt.NPALLETID), isInware ? "" : GS(rt.FROM_LOCATOR_CODE), isInware ? GS(rt.TO_LOCATOR_CODE) : "",
                roadway, GS(rt.NOPTTYPE), GI(rt.NOPTSTATION), GI(rt.NPRI), GI(rt.NLOTID),
                GS(rt.CPACKTYPE), GI(rt.NWKSTATUS), GS(rt.CFINISHMODE), GS(rt.CUSERID), GI(rt.QUANTITY),
                height, GS(rt.CUSTOMER_NAME));
            int count = SqlBase.ExecSql(sql, connstr);
            if (count != 1)
                throw new Exception("插入WMS任务失败！");
        }

        static int GI(int? value)
        {
            return value.HasValue ? value.Value : 0;
        }
        static string GS(string value)
        {
            return value == null ? "" : value;
        }

        public static string CreateEmptyOutTask(int station)
        {
            return WMSCreateTask(6, "", station, 0);
        }
        public static string CreateEmptyInTask(string pallet, int station, int height)
        {
            return WMSCreateTask(5, pallet, station, height);
        }
        public static string CreateInTask(string pallet, int station, int height)
        {
            return WMSCreateTask(1, pallet, station, height);
        }
        #endregion

        #region WCS 业务
        //手动过账任务
        public static int FinishTaskByHand(string SeqId)
        {
            var sql = $"update Wcs_Task set NwkStatus=98,Msg='手动过账' where SeqID = {SeqId}";
            return SqlBase.ExecSql(sql, connstr);
        }

        public static DataTable GetDdjStatus()
        {
            var sql1 = "Select * from [dbo].[Wcs_DdjStatue]";

            return SqlBase.GetSqlDs(sql1,connstr).Tables[0];
        }

        //手动还原任务
        public static int ReBackTaskByHand(string SeqId)
        {
            var sql = $"update Wcs_Task set NwkStatus=1 where SeqID = {SeqId}";
            //同时更新WMS状态
            UpdateWMSTask(SeqId, 1);
            return SqlBase.ExecSql(sql, connstr);
        }

        public static DataTable GetStationInfo(string type,string state)
        {
            var sql = "select WMSNo,BufferC,BufferM,SType,[State],Remark from [dbo].[Wcs_NoMap] where 1=1 ";
            if (type.Length > 0) { sql += $" and SType='{type}'"; }
            if (state.Length > 0) { sql += $" and [State]={state}"; }
            sql += " order by WMSNo ASC";
            return SqlBase.GetSqlDs(sql, connstr).Tables[0];
        }

        public static Wcs_NoMap GetStationByNo(int no)
        {
            var sql = $"select * from [dbo].[Wcs_NoMap] where WMSNo={no}";
            var dt = SqlBase.GetSqlDs(sql, connstr).Tables[0];
            return SqlToEntity.ToSingleModel<Wcs_NoMap>(dt);
        }

        //更改站台信息
        public static void UpdateStationInfo(Dictionary<string, string> dic)
        {
            var sql = "";
            foreach(KeyValuePair<string,string> item in dic)
            {
                var arr = item.Value.Split('|');
                sql += $"update [Wcs_NoMap] set BufferC={arr[0]},[State]={arr[1]},Remark='{arr[2]}' where WMSNo={item.Key};";
            }

            SqlBase.ExecSql(sql, connstr);
        }

        public static int ClearEmptyBuffer(int seqid, int no)
        {
            var sql1 = $"delete Wcs_EmptyBuffer where EmptyOutStationNo={no} and NWorkId={seqid}";
            return SqlBase.ExecSql(sql1, connstr);
        }

        public static int EmptyControl(int staionNo, int seqid)
        {
            var para1 = new SqlParameter("@StationNo", SqlDbType.Int);
            para1.Value = staionNo;
            var para2 = new SqlParameter("@SeqId", SqlDbType.Int);
            para2.Value = seqid;
            var para3 = new SqlParameter("@Rlt", SqlDbType.Int);
            para3.Direction = ParameterDirection.Output;
            var paras = new SqlParameter[] { para1, para2, para3 };
            SqlBase.ExecProc("proc_wcs_emptycontrol", paras, connstr);

            return Convert.ToInt32(paras[2].Value);
        }

        //计算是否需要向WMS申请空托盘出库
        public static bool EmptyAutoOut(int noptstation)
        {
            var para1 = new SqlParameter("@WmsNo", SqlDbType.Int);
            para1.Value = noptstation;
            var para2 = new SqlParameter("@Rlt", SqlDbType.Bit);
            para2.Direction = ParameterDirection.Output;
            var paras = new SqlParameter[] { para1, para2 };
            SqlBase.ExecProc("proc_wcs_emptyautoout", paras, connstr);

            return Convert.ToBoolean(paras[1].Value);
        }

        public static DataTable SelectTaskList(string condition)
        {
            var sql1 = $"select SeqID,NordCN=case NordID when 1 then  '整盘入库' when 3 then '整盘出库' when 5 then '空盘入库' when 6 then '空盘出库' else '' end,NwkStatus,NPalletID,CPosidFrom,CPosidTo,Roadway,NoptStation,NlotID,NpackOrder,DoptDate,DStartDate,DFinishDate,CustomerName,Date_ext1,Date_ext2,Date_ext3,Msg from Wcs_Task where 1=1 {condition} order by DoptDate asc ";

            return SqlBase.GetSqlDs(sql1, connstr).Tables[0];
        }

        public static DataTable GetAllDevice()
        {
            var sql1 = "select * from Wcs_Device";
            return SqlBase.GetSqlDs(sql1, connstr).Tables[0];
        }

        public static DataTable GetLedInfo(DateTime startTime, DateTime endTime, int lPort, bool isNew = false)
        {
            string store = "WMS_LED_Content";
            return SqlBase.ExecProcDataSet(store, new SqlParameter[]
            {
                new SqlParameter {ParameterName = "@StartTime", DbType = DbType.DateTime, SqlValue = startTime},
                new SqlParameter {ParameterName = "@EndTime", DbType = DbType.DateTime, SqlValue = endTime},
                new SqlParameter {ParameterName = "@LPort", DbType = DbType.Int32, SqlValue = lPort},
                 new SqlParameter {ParameterName = "@IsNew", DbType = DbType.Boolean, SqlValue = isNew},
            }, connstr).Tables[0];
        }

        public static void ReSendLedMsg(int lid)
        {
            string store = "ResendLedMsg";
            SqlBase.ExecProc(store, new SqlParameter[] { new SqlParameter { ParameterName = "@Lid", DbType = DbType.Int32, SqlValue = lid } }, connstr);
            return;
        }

        //获取所有有数据的设备
        public static DataTable GetRunDevice()
        {
            var sql1 = "select * from Wcs_Device where DWorkId>0 or DErrorNo>0";
            return SqlBase.GetSqlDs(sql1, connstr).Tables[0];
        }

        public static Wcs_Device GetWD(string dno)
        {
            var sql1 = $"select top 1 * from Wcs_Device where DNo={dno} and (DWorkId>0 or DErrorNo>0)";
            var dt = SqlBase.GetSqlDs(sql1, connstr).Tables[0];
            if (dt.Rows.Count == 1)
            {
                return SqlToEntity.ToSingleModel<Wcs_Device>(dt);
            }
            else
            {
                return null;
            }
        }

        public static void UpdateAllDecice(string sql_update)
        {
            SqlBase.ExecSql(sql_update, connstr);
        }

        public static Wcs_Task GetObjectData(DataTable dt)
        {
            var item = SqlToEntity.ToSingleModel<Wcs_Task>(dt);
            if (item.NordID == 3 || item.NordID == 6)
            {
                //出库
                item.FromRow = int.Parse(item.CPosidFrom.Substring(1, 2)) % 2 == 0 ? 1 : 2;
                item.FromColumn = int.Parse(item.CPosidFrom.Substring(4, 2));
                item.FromLayer = int.Parse(item.CPosidFrom.Substring(7, 2));

                if (item.NoptStation > 2000)
                {
                    //出库到二楼
                    item.ToRow = 2;
                    item.ToColumn = 27;
                    item.ToLayer = 18;
                    item.Target = GetDeviceNoByWmsNo(item.NoptStation);
                }
                else
                {
                    //出库到一楼发货区
                    item.ToRow = 2;
                    item.ToColumn = 27;
                    item.ToLayer = 16;
                }

                //11巷道少了两列
                if (item.Roadway == 11) { item.ToColumn = 25; }

                switch (item.NoptStation)
                {
                    case 1001:
                    case 1002:
                    case 1003:
                    case 1004:
                    case 1005:
                    case 1006:
                    case 1007:
                    case 1008:
                        item.Target = 1118;
                        break;
                    case 1009:
                        item.Target = 1127;
                        break;
                    case 1010:
                        item.Target = 1131;
                        break;
                    case 1011:
                        item.Target = 1135;
                        break;
                    case 1012:
                        item.Target = 1139;
                        break;
                    case 1013:
                        item.Target = 1143;
                        break;
                    case 1014:
                        item.Target = 1149;
                        break;
                    case 1015:
                        item.Target = 1153;
                        break;
                    case 1016:
                        item.Target = 1157;
                        break;
                    case 1017:
                        item.Target = 1161;
                        break;
                    case 1018:
                    case 1019:
                        item.Target = 1165;
                        break;
                    case 1020:
                        item.Target = 1176;
                        break;
                }
            }
            else if (item.NordID == 1 || item.NordID == 5)
            {
                //入库
                item.FromRow = 2;
                item.FromColumn = 27;
                item.FromLayer = 17;
                //11巷道少了两列
                if (item.Roadway == 11) { item.FromColumn = 25; }

                item.ToRow = int.Parse(item.CPosidTo.Substring(1, 2)) % 2 == 0 ? 1 : 2;
                item.ToColumn = int.Parse(item.CPosidTo.Substring(4, 2));
                item.ToLayer = int.Parse(item.CPosidTo.Substring(7, 2));

                if (item.Roadway == 1)
                {
                    item.Target = 5036;
                }
                else if (item.Roadway == 2)
                {
                    item.Target = 5044;
                }
                else if (item.Roadway == 3)
                {
                    item.Target = 5052;
                }
                else if (item.Roadway == 4)
                {
                    item.Target = 5060;
                }
                else if (item.Roadway == 5)
                {
                    item.Target = 5068;
                }
                else if (item.Roadway == 6)
                {
                    item.Target = 5076;
                }
                else if (item.Roadway == 7)
                {
                    item.Target = 4037;
                }
                else if (item.Roadway == 8)
                {
                    item.Target = 4030;
                }
                else if (item.Roadway == 9)
                {
                    item.Target = 4023;
                }
                else if (item.Roadway == 10)
                {
                    item.Target = 4016;
                }
                else if (item.Roadway == 11)
                {
                    item.Target = 4009;
                }
            }

            return item;
        }

        //根据任务号，获取正在执行中的任务
        public static Wcs_Task GetTaskInfoBySeqId(int seqid)
        {
            string sql = $"select * from [dbo].[Wcs_Task] where SeqID={seqid} and NwkStatus<98";

            var dt = SqlBase.GetSqlDs(sql, connstr).Tables[0];

            if (dt.Rows.Count == 1)
            {
                return GetObjectData(dt);
            }
            else
            {
                return null;
            }
        }

        //根据托盘号，获取正在执行中的任务
        public static Wcs_Task GetTaskInfoByPallet(string pallet)
        {
            string sql = $"select * from [dbo].[Wcs_Task] where NPalletID='{pallet}' and NwkStatus=1";

            var dt = SqlBase.GetSqlDs(sql, connstr).Tables[0];

            if (dt.Rows.Count == 1)
            {
                return GetObjectData(dt);
            }
            else
            {
                return null;
            }
        }

        //插入日志记录
        public static void InsertLog(string msg, int type, string pallet = "", string deviceno = "")
        {
            var para1 = new SqlParameter("@Type", SqlDbType.Int);
            para1.Value = type;
            var para2 = new SqlParameter("@Pallet", SqlDbType.NVarChar);
            para2.Value = pallet;
            var para3 = new SqlParameter("@Msg", SqlDbType.NVarChar);
            para3.Value = msg;
            var para4 = new SqlParameter("@DeviceNo", SqlDbType.VarChar);
            para4.Value = deviceno;
            try
            {
                SqlBase.ExecProc("proc_wcs_writelog", new SqlParameter[] { para1, para2, para3, para4 }, connstr);
            }
            catch { }
        }

        //堆垛机故障报警
        public static void InsertScError(string msg, string start, string end)
        {
            var sql1 = $"insert into Wcs_ErrorLog(Msg,StartDate,EndDate)values('{msg}','{start}','{end}')";
            SqlBase.ExecSql(sql1, connstr);
        }

        //统计出库信息
        public static DataTable GetOutInfo()
        {
            var ds = SqlBase.ExecProcDataSet("proc_wcs_outinfo", null, connstr);
            return ds.Tables[0];
        }

        //获取执行中的状态超过3分钟的任务，属于不正常滞留的任务
        public static DataTable GetTaskDelay()
        {
            var sql1 = @"select top 3 * from
                (
                select SeqID,NwkStatus,NPalletID,T=Date_ext1 from [dbo].[Wcs_Task] where NwkStatus=31 and DATEDIFF(minute,Date_ext1,getdate())>5 and NordID<>6
                union all
                select SeqID,NwkStatus,NPalletID,T=DStartDate from [dbo].[Wcs_Task] where NwkStatus=30 and DATEDIFF(minute,DStartDate,getdate())>5
                union all
                select SeqID,NwkStatus,NPalletID,T=DStartDate from [dbo].[Wcs_Task] where NwkStatus=20 and DATEDIFF(minute,DStartDate,getdate())>5
                union all
                select SeqID,NwkStatus,NPalletID,T=Date_ext1 from [dbo].[Wcs_Task] where NwkStatus=21 and DATEDIFF(minute,Date_ext1,getdate())>5
                ) as tb order by T ASC";
            return SqlBase.GetSqlDs(sql1, connstr).Tables[0];
        }

        //获取日志记录
        public static DataTable GetLog()
        {
            var sql1 = $@"select top 30 * from
                        (
                        select Msg, StartDate = LogDate, EndDate = null, T = 'LOG' from[dbo].[Wcs_Log]
                        union all
                        select Msg, StartDate, EndDate, T = 'ERROR' from Wcs_ErrorLog
                        ) as A order by StartDate desc";

            return SqlBase.GetSqlDs(sql1, connstr).Tables[0];
        }

        //获取最新的一条错误信息
        public static string GetLastErrorMsg()
        {
            var sql1 = "select top 1 Msg from Wcs_ErrorLog where dateadd(second,3,StartDate)>=getdate() or dateadd(second,3,EndDate)>=getdate() order by [SId] desc";
            var obj = SqlBase.GetSqlScalar(sql1, connstr);
            if (obj == null) { return ""; }
            else { return obj.ToString(); }
        }

        //更改WCS任务状态,无需回写给WMS
        public static void UpdateTaskStatus(int SeqId, int NwkStatus)
        {
            var sql = "";
            if (NwkStatus == 21 || NwkStatus == 31)
            {
                sql = $" UPDATE Wcs_Task SET NwkStatus = {NwkStatus}, Date_ext1=getdate()  WHERE seqid ={SeqId} AND NwkStatus < 98 ";
            }
            else if (NwkStatus == 98)
            {
                sql = $" UPDATE Wcs_Task SET NwkStatus = {NwkStatus}, Date_ext3=getdate()  WHERE seqid ={SeqId} AND NwkStatus < 98 ";
            }

            SqlBase.ExecSql(sql, connstr);
        }

        //一楼出库(1001到1020出口)算法
        public static Wcs_Task GetOutTask1(int scNo)
        {
            var ds = SqlBase.ExecProcDataSet("proc_wcs_out_dispatcher", null, connstr);

            if (ds.Tables[0].Rows.Count == 0)
            {
                return null;
            }

            string sql = "";
            var noptstation = "";
            var lotid = "";
            var packorder = "";
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                //找最小的波次号中的最小装车类别
                noptstation = dr["WMSNo"].ToString();
                sql = $"select top 1 * from [dbo].[Wcs_Task] where NoptStation={noptstation} and NwkStatus=1 order by NlotID ASC,NpackOrder ASC";
                var dt = SqlBase.GetSqlDs(sql, connstr).Tables[0];
                if (dt.Rows.Count == 1)
                {
                    lotid = dt.Rows[0]["NlotID"].ToString(); //最小的批次号
                    packorder = dt.Rows[0]["NpackOrder"].ToString(); //最小的装箱顺序

                    //根据最小的批次号和装车顺序匹配堆垛机
                    var sql2 = $"select top 1 * from [dbo].[Wcs_Task] where NoptStation={noptstation} and NwkStatus=1 and Roadway={scNo} and NlotID={lotid} and NpackOrder={packorder}";
                    var dt2 = SqlBase.GetSqlDs(sql2, connstr).Tables[0];
                    if (dt2.Rows.Count == 1)
                    {
                        var sql3 = $"select 1 from Wcs_Task where NoptStation={noptstation} and (NlotID<{lotid} or (NlotID={lotid} and NpackOrder<{packorder})) and NordID=3 and NwkStatus<=30";
                        var dt3 = SqlBase.GetSqlDs(sql3, connstr).Tables[0];
                        if (dt3.Rows.Count == 0)
                        {
                            return GetObjectData(dt2);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    continue;
                }
            }

            return null;
        }
        
        //二楼出库，理货区间（整盘出库和空托盘出库）
        public static Wcs_Task GetOutTask2(int scNo)
        {
            var sql = $"select top 1 * from [dbo].[Wcs_Task] where NordID in (3,6) and Roadway={scNo} and NwkStatus=1 and NoptStation>2000 order by SeqID asc";

            var dt = SqlBase.GetSqlDs(sql, connstr).Tables[0];

            if (dt.Rows.Count == 1)
            {
                return GetObjectData(dt);
            }
            else
            {
                return null;
            }
        }

        //更改堆垛机
        public static void UpdateDdjInfo(Wcs_DdjInfo ddj)
        {
            var sql = $@"UPDATE [dbo].[Wcs_DdjInfo]
                                   SET [AlarmStatus] = {(ddj.AlarmStatus ? 1 : 0)}
                                          ,[AutoStatus] = {(ddj.AutoStatus ? 1 : 0)}
                                          ,[IsFree] = {(ddj.IsFree ? 1 : 0)}
                                          ,[CColumn] = {ddj.CColumn}
                                          ,[CLayer] = {ddj.CLayer}
                                          ,[TaskId] = {ddj.TaskId}
                                          ,[NPalletID] = '{ddj.NPalletID}'
                                          ,[TaskStatus] = {(ddj.TaskStatus ? 1 : 0)}
                                   WHERE ScNo={ddj.ScNo}";

            SqlBase.ExecSql(sql, connstr);
        }

        //通过设备编号获取ItemNo
        public static int GetItemNoByDeviceNo(int dno)
        {
            var sql1 = "select ItemNo from Wcs_Device where DId=" + dno.ToString();
            var obj = SqlBase.GetSqlScalar(sql1, connstr);
            if (obj == DBNull.Value) { throw new Exception($"设备编号{dno}不存在！"); }
            return Convert.ToInt32(obj);
        }

        //监控设备是否报警
        public static int GetDeviceErrorCode(string dno)
        {
            var sql1 = $"select top 1 DErrorNo from Wcs_Device where DNo={dno} and DErrorNo>0";
            var obj = SqlBase.GetSqlScalar(sql1, connstr);
            if (obj == DBNull.Value) { throw new Exception($"设备编号{dno}不存在！"); }
            if (obj == null) { return 0; }
            return Convert.ToInt32(obj);
        }

        //空托盘到了6082后，根据线体实际缓存计算要出到几楼
        public static int EmptyOut2016(int floor)
        {
            var para1 = new SqlParameter("@floor", SqlDbType.Int);
            para1.Value = floor;
            var para2 = new SqlParameter("@target", SqlDbType.Int);
            para2.Direction = ParameterDirection.Output;
            var paras = new SqlParameter[] { para1, para2 };
            SqlBase.ExecProc("proc_wcs_empty2016", paras, connstr);

            return Convert.ToInt32(paras[1].Value);
        }

        public static Wcs_Device GetDeviceMonitorByNo(string dno)
        {
            var sql1 = $"select top 1 * from Wcs_Device where DNo={dno} and (DWorkId>0 or DErrorNo>0)";
            var dt = SqlBase.GetSqlDs(sql1, connstr).Tables[0];
            if (dt.Rows.Count == 1)
            {
                return SqlToEntity.ToSingleModel<Wcs_Device>(dt);
            }

            return null;
        }

        //通过WMS逻辑编号找对应的物理设备编号
        public static int GetDeviceNoByWmsNo(int wmsno)
        {
            var sql1 = "select DeviceNo from [dbo].[Wcs_NoMap] where WMSNo=" + wmsno.ToString();
            var obj = SqlBase.GetSqlScalar(sql1, connstr);
            return Convert.ToInt32(obj);
        }

        //过汇流口判断
        public static bool LoopC(int seqId)
        {
            var para1 = new SqlParameter("@seqid", SqlDbType.Int);
            para1.Value = seqId;
            var para2 = new SqlParameter("@rlt", SqlDbType.Bit);
            para2.Direction = ParameterDirection.Output;
            var paras = new SqlParameter[] { para1, para2 };
            SqlBase.ExecProc("proc_wcs_loopcontrol", paras, connstr);

            return Convert.ToBoolean(paras[1].Value);
        }

        //转化堆垛机错误信息
        public static string GetSCMsg(int[] arr)
        {
            var msgs = "";

            for(int i = 0; i < arr.Length; i++)
            {
                if (arr[i] > 0)
                {
                    var code2 = Convert.ToString(arr[i], 2);

                    for (int j = 0; j < code2.Length; j++)
                    {
                        if (code2[j] == '1')
                        {
                            msgs += GetSCMsgCN(i + 1, code2.Length - j - 1) + "|";
                        }
                    }
                }
            }

            return msgs;
        }

        static string GetSCMsgCN(int area,int idx)
        {
            var sql = $"select ErrorMsgC=ErrorMsg+'('+(case EType when 1 then '设备原因' when 2 then '人为原因' else '未知' end)+')' from Wcs_DdjErrorDic where Area={area} and indx={idx}";

            return SqlBase.GetSqlScalar(sql, connstr).ToString();
        }

        public static DataRow GetDeviceMonitor(int dno)
        {
            var sql1 = $"select * from Wcs_Device where DNo = {dno} and DWorkId>0";
            var dt = SqlBase.GetSqlDs(sql1, connstr).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0];
            }
            else
            {
                return null;
            }
        }

        public DataTable GetPort()
        {
            var sql1 = "select * from Wcs_NoMap";
            return SqlBase.GetSqlDs(sql1, connstr).Tables[0];
        }

        public static void UpdateSC1FStaionOut(int scNo, int tid)
        {
            var sql1 = $"update Wcs_DdjInfo set Station1FOut={(tid > 0 ? 1 : 0)} where ScNo={scNo}";
            SqlBase.ExecSql(sql1, connstr);
        }

        public static void UpdateSC2FStaionOut(int scNo, int tid)
        {
            var sql1 = $"update Wcs_DdjInfo set Station2FOut={(tid > 0 ? 1 : 0)} where ScNo={scNo}";
            SqlBase.ExecSql(sql1, connstr);
        }

        public static void UpdateSC2FStaionIn(int scNo, int target)
        {
            var sql1 = $"update Wcs_DdjInfo set Station2FIn={(target == 999 ? 1 : 0)} where ScNo={scNo}";
            SqlBase.ExecSql(sql1, connstr);
        }
        #endregion

        public static void InsertVoice(string content)
        {
            var sql1 = $"insert into Voice_ContentList(VoiceContent) values('{content}')";
            SqlBase.ExecSql(sql1, connstr);
        }

        //语音播报
        public static string ReadVoice()
        {
            var sql1 = "select top 1 Msg from Wcs_ErrorLog where len(DeviceNo)>0 and (dateadd(SECOND,6,StartDate)>=GETDATE() or dateadd(SECOND,6,EndDate)>=GETDATE())  order by [SId] desc";
            var obj = SqlBase.GetSqlScalar(sql1, connstr);
            if (obj != null)
            {
                return obj.ToString();
            }

            return "";
        }

        //读取托盘号
        public static string ReadRFIDPallet(int inStation)
        {
            var sql1 = $"select top 1 NPallet from [dbo].[RFID_ScanLog] where DATEDIFF(second,ScanTime,getdate())<40 and InStation={inStation} order by ScanTime  desc";
            var obj = SqlBase.GetSqlScalar(sql1, connstr);
            return obj == null ? "" : obj.ToString();

            //var sql1 = $"select top 1 RPallet from RFID_Config where StationNo={inStation} and DATEDIFF(second,RTime,getdate())< { 5 + ct / 2 }";
            //var dt = SqlBase.GetSqlDs(sql1, connstr).Tables[0];
            //if (dt.Rows.Count > 0)
            //{
            //    return dt.Rows[0]["RPallet"].ToString();
            //}

            //return "";
        }

        //更新RFID读取P托盘号的状态
        public static void UpdateRFIDPalletStatus(int inStation, string pallet)
        {
            var sql1 = $"update RFID_Config set RStatus=1 where StationNo={inStation} and RPallet='{pallet}'";
            SqlBase.ExecSql(sql1, connstr);
        }

        //更新RFID读取P托盘号的状态
        public static void UpdateRFIDPalletStatus(int inStation)
        {
            var sql1 = $"update RFID_Config set RStatus=1 where StationNo={inStation} ";
            SqlBase.ExecSql(sql1, connstr);
        }

        //获取托盘类型
        public static string GetPalletType(string pallet)
        {
            var sql = $"select NType from [dbo].[Wcs_PalletInfo] where NPalletID='{pallet}'";
            var obj = SqlBase.GetSqlScalar(sql, connstr);
            if (obj != null) { return obj.ToString(); }
            return "";
        }

        public static Wcs_Task GetTaskByPallet(string pallet)
        {
            var sql = $"select top 1 * from Wcs_Task where NPalletID='{pallet}' and NwkStatus=1 and NordID=1";
            var dt = SqlBase.GetSqlDs(sql, connstr).Tables[0];
            if (dt.Rows.Count == 1)
            {
                return GetObjectData(dt);
            }
            else
            {
                return null;
            }
        }

        //查询日志信息
        public static DataTable  SelectLog(string where)
        {
            var sql1 = $@"select RId=ROW_NUMBER() over(order by LogDate desc),Msg,DeviceNo,LogDate,EndDate,LType from
                (
                select Msg, LogDate, DeviceNo = null, EndDate = null, LType = 'LOG' from [dbo].[Wcs_Log]
                union all
                select Msg, LogDate = StartDate, DeviceNo, EndDate, LType = 'ERROR' from [dbo].[Wcs_ErrorLog]
                ) as T where 1 = 1 {where} ";

            return SqlBase.GetSqlDs(sql1, connstr).Tables[0];
        }

        //0 入库口的屏   1 发货口内屏    2 发货口外屏
        public static void InsertLED(string content, int port, int type)
        {
            var sql = $"insert into LED_Content(LContent,LPort,LType) values('{content}',{port},{type})";
            SqlBase.ExecSql(sql, connstr);
        }

        //获取要发送的LED屏信息
        public static List<LED_Content> GetLEDSendList()
        {
            var sql = "select c.*,l.LIP,DeviceId=l.LID,Port from [dbo].[LED_Content] c left join LED_Config l on c.LPort=l.LPort and c.LType=l.LType where LStatus=1";
            var dt = SqlBase.GetSqlDs(sql, connstr).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return SqlToEntity.ToListModel<LED_Content>(dt);
            }

            return null;
        }

        //更新LED发送的状态
        public static void UpdateLEDStatus(LED_Content content)
        {
            var sql = $"update [LED_Content] set LStatus={content.LStatus},LSendDate=GETDATE() where LID={content.LID}";
            SqlBase.ExecSql(sql, connstr);
        }

        #region 测试代码
        public static bool CreateOutTask(Wcs_Task wt)
        {
            var ran = new Random();
            var seqid = ran.Next(1000, 2000000000);

            var sql1_insert = $@"insert into Wcs_Task
                (SeqID,NordID,NwkStatus,NPalletID,CPosidFrom,CPosidTo,Roadway,NoptStation,NlotID,NpackOrder,DoptDate)
                values({seqid}, 3, 1, '{wt.NPalletID}', '{wt.CPosidTo}', '', {wt.Roadway}, {(wt.NoptStation)}, 0, 0, GETDATE())";

            SqlBase.ExecSql(sql1_insert, connstr);

            return true;
        }

        //创建入库任务
        public static bool CreateInTask(Wcs_Task wt)
        {
            var ran = new Random();
            var seqid = ran.Next(1000, 2000000000);

            var sql = $"select top 1 CposidTo from Wcs_Task where Roadway={wt.Roadway} and NordID=1 and LEFT(CposidTo,3)='{wt.CPosidFrom.Substring(0, 3)}' order by DoptDate desc";
            var perf_CPosidTo = SqlBase.GetSqlScalar(sql, connstr).ToString();

            var r = int.Parse(perf_CPosidTo.Substring(1, 2));
            var c = int.Parse(perf_CPosidTo.Substring(4, 2));
            var l = int.Parse(perf_CPosidTo.Substring(7, 2));

            if (r >= 21 && c == 24 && l == 15)
            {
                c = 1;
                l = 0;
            }

            if (r <= 20 && c == 26 && l == 15)
            {
                c = 1;
                l = 0;
            }

            if (l < 15)
            {
                l++;
                if (l >= 9 && l <= 12)
                {
                    l = 13;
                }
            }
            else
            {
                l = 1;
                c = c + 1;
            }

            var next_CPosidTo = $"J{r.ToString().PadLeft(2, '0') }.{c.ToString().PadLeft(2, '0')}.{l.ToString().PadLeft(2, '0')}";

            var sql1_insert = $@"insert into Wcs_Task
                (SeqID,NordID,NwkStatus,NPalletID,CPosidFrom,CPosidTo,Roadway,NoptStation,NlotID,NpackOrder,DoptDate)
                values({seqid}, 1, 1, '{wt.NPalletID}', '', '{next_CPosidTo}', {wt.Roadway}, {wt.NoptStation}, 0, 0, GETDATE())";

            SqlBase.ExecSql(sql1_insert, connstr);

            return true;
        }

        public static Wcs_Task GetNextTask(int scno)
        {
            string sql = $"select top 1 * from [dbo].[Wcs_Task] where Roadway={scno} and NwkStatus=1 and NordID=3";

            var dt = SqlBase.GetSqlDs(sql, connstr).Tables[0];

            if (dt.Rows.Count == 1)
            {
                return GetObjectData(dt);
            }
            else
            {
                return null;
            }
        }

        public static void F(string sql1)
        {
            SqlBase.ExecSql(sql1, connstr);
        }
        #endregion
    }
}
