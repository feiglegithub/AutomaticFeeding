using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using WcsModel;
using WCS.model;
using ManualTask = WCS.model.ManualTask;

namespace WCS.DataBase
{
    public class WCSSql
    {
        static string connectstring = AppCommon.WCSConnstr;
        //static string connect_time = AppCommon.WCSConnstr2;

        //插入日志记录
        public static void InsertLog(string msg, string type, int pilerno = 0, string device = "")
        {
            var para1 = new SqlParameter("@Type", SqlDbType.VarChar);
            para1.Value = type;
            var para2 = new SqlParameter("@PilerNo", SqlDbType.BigInt);
            para2.Value = pilerno;
            var para3 = new SqlParameter("@Msg", SqlDbType.NVarChar);
            para3.Value = msg;
            var para4 = new SqlParameter("@Device", SqlDbType.VarChar);
            para4.Value = device;
            try
            {
                SqlHelper.RunProc("wcs_proc_InsertLog", new SqlParameter[] { para1, para2, para3, para4 }, connectstring);
            }
            catch { }
        }

        /// <summary>
        /// 插入堆垛车请求日志
        /// </summary>
        /// <param name="pilerNo">垛号</param>
        /// <param name="requestType">请求类型，1下发任务，99完成任务</param>
        public static void InsertRGVRequestLog(int pilerNo, int requestType)
        {
            var para1 = new SqlParameter("@RequestType", SqlDbType.Int);
            para1.Value = requestType;
            var para2 = new SqlParameter("@PilerNo", SqlDbType.Int);
            para2.Value = pilerNo;
            SqlHelper.RunProc("InsertRGVRequestLog", new[] { para1, para2}, connectstring);
                
        }

        /// <summary>
        /// 插入堆垛车请求日志
        /// </summary>
        /// <param name="pilerNo">垛号</param>
        /// <param name="requestType">请求类型，1下发任务，99完成任务</param>
        /// <param name="fromPosition">起始位置</param>
        /// <param name="toPosition">目标位置</param>
        public static void InsertRGVRequestLog(int pilerNo, int requestType,string fromPosition,string toPosition)
        {
            var para1 = new SqlParameter("@RequestType", SqlDbType.Int);
            para1.Value = requestType;
            var para2 = new SqlParameter("@PilerNo", SqlDbType.Int);
            para2.Value = pilerNo;
            var para3 = new SqlParameter("@FromPosition", SqlDbType.Int);
            para1.Value = requestType;
            var para4 = new SqlParameter("@ToPosition", SqlDbType.Int);
            para4.Value = pilerNo;
            SqlHelper.RunProc("InsertRGVRequestLog", new[] { para1, para2 }, connectstring);

        }

        //重载
        public static void InsertLog(string msg, string startDate, string endDate, int pilerno)
        {
            string sql1 = $@"insert into [dbo].[WCS_Log_Error](PilerNo,Msg,StartDate,EndDate) values({pilerno},'{msg}','{startDate}','{endDate}')";

            Sql.ExecSQL(sql1);
        }

        //获取堆垛机故障信息
        public static string GetSCErrorMsg(string ids)
        {
            if (ids == "") { return ""; }
            string msgs = "";
            ids = ids.Remove(ids.Length - 1, 1);
            string sql1 = $"select * from [dbo].[WCS_DdjErrorDic] where ItemNo in ({ids})";
            var dt = Sql.GetSQL(sql1).Tables[0];
            int i = 0; //给堆垛机最多显示3条故障
            foreach (DataRow dr in dt.Rows)
            {
                if (i == 3) { break; }
                msgs = msgs + dr["ItemMsg"].ToString();
                i++;
            }
            return msgs.Replace("\r\n", "");
        }

        //获取最近30条日志记录
        public static DataTable GetLogLst(string type)
        {
            string sql1 = "";
            if (type.Equals("LOG"))
            {
                sql1 = "select top 30 ID,PilerNo,Msg,StartDate from [dbo].[Wcs_Log_Opt] where [Type]='LOG' order by ID DESC";
            }
            else
            {
                sql1 = "select top 30 ID,Times,Msg,StartDate,EndDate from HGWCSBC.[dbo].WCS_Log_Error order by ID DESC";
            }
            return Sql.GetSQL(sql1).Tables[0];
        }

        #region 测试代码
        public static TaskInfo Demo_GetLastTask(int ddjno, int r)
        {
            string sql1 = $"select top 1 * from HuangGangBWMS.[dbo].[WMS_Task] where TaskType = 1 and DdjNo = {ddjno} and TaskStatus=98 and LEFT(ToPosition,1)={r} order by TaskId desc";
            var dt = SqlHelper.GetSQL(sql1, connectstring).Tables[0];
            var row = dt.Rows[0];
            var task = GetTaskObject(row);
            return task;
        }

        //获取一条出库任务
        public static TaskInfo Demo_GetOutTask(int ddjno)
        {
            string sql1 = $"select top 1 * from HuangGangBWMS.[dbo].[WMS_Task] where TaskType=2 and DdjNo={ddjno} and TaskStatus=1";
            var dt = SqlHelper.GetSQL(sql1, connectstring).Tables[0];
            if (dt.Rows.Count > 0)
            {
                var row = dt.Rows[0];
                var task = GetTaskObject(row);
                return task;
            }
            return null;
        }

        //创建入库任务
        public static void CreateDemoTask(int ddjNo, string positon, int pilerno)
        {
            string sql1 = $@"insert into HuangGangBWMS.[dbo].[WMS_Task](PilerNo,TaskType,ToPosition,DdjNo,HasUpProtect,TaskStatus,CreateTime)
                values({pilerno}, 1, '{positon}', {ddjNo}, 1, 1, GETDATE())";
            Sql.ExecSQL(sql1);
        }

        //创建出库任务
        public static void CreateDemoTask2(int ddjNo, string positon)
        {
            var ran = new Random();
            var pilerno = ran.Next(1000, 99999999);
            string sql1 = $@"insert into HuangGangBWMS.[dbo].[WMS_Task](PilerNo,TaskType,FromPosition,ToPosition,DdjNo,HasUpProtect,TaskStatus,CreateTime)
                values({pilerno}, 2, '{positon}',{105 - ddjNo} ,{ddjNo}, 1, 1, GETDATE())";
            Sql.ExecSQL(sql1);
        }
        #endregion

        ////获取服务器(10.30.3.116)的时间
        //public static string GetServiceTime()
        //{
        //    string sql1 = " select GETDATE() ";
        //    var ds = Sql.GetSQL2(sql1);
        //    if (ds != null)
        //    {
        //        return ds.Tables[0].Rows[0][0].ToString();
        //    }
        //    else
        //    {
        //        return "2018-10-1";
        //    }
        //}

        //创建一条任务请求记录
        public static void CreateWcsRequest(RequestInfo req)
        {
            var para1 = new SqlParameter("@ReqType", SqlDbType.Int);
            para1.Value = req.ReqType;
            var para2 = new SqlParameter("@TaskId", SqlDbType.BigInt);
            para2.Value = req.TaskId;
            var para3 = new SqlParameter("@FromPosition", SqlDbType.VarChar);
            para3.Value = req.FromPosition == null ? "" : req.FromPosition;
            var para4 = new SqlParameter("@ToPosition", SqlDbType.VarChar);
            para4.Value = req.ToPosition == null ? "" : req.ToPosition;
            var para5 = new SqlParameter("@ProductCode", SqlDbType.VarChar);
            para5.Value = req.ProductCode == null ? "" : req.ProductCode;
            var para6 = new SqlParameter("@Amount", SqlDbType.Int);
            para6.Value = req.Amount;
            var para7 = new SqlParameter("@PilerNo", SqlDbType.Int);
            para7.Value = req.PilerNo;
            var para8 = new SqlParameter("@ReqId", SqlDbType.BigInt);
            para8.Value = req.ReqId;
            //var para9 = new SqlParameter("@DateTime", SqlDbType.DateTime);
            //para9.Value = GetServiceTime();
            try
            {
                SqlHelper.RunProc("wcs_proc_CreateRequest", new SqlParameter[] { para1, para2, para3, para4, para5, para6, para7, para8 }, connectstring);
            }
            catch (Exception ex)
            {
                req.ErrorMsg = ex.Message;
            }
        }

        //WCS创建任务需求,返回新创建的请求编号
        public static long GetNextReqId()
        {
            string sql1 = "select isnull(MAX(ReqId),0)+1 from HGWCSBC.[dbo].WCS_Request";
            var ds = Sql.GetSQL(sql1);
            return int.Parse(ds.Tables[0].Rows[0][0].ToString());          
        }

        //更新WMS反馈的状态
        public static int UpdateRequestResult(long ReqId, int Response, string ErrorMsg)
        {
            string sql = $"update WCS_Request set RequestStatus={Response},ResponseTime=GETDATE(),ErrorMsg='{ErrorMsg}' where ReqId={ReqId}";

            return SqlHelper.ExecSQL(sql, connectstring);
        }

        //public static void Delete



        //查询任务列表
        public static DataSet SelectTaskList(string Condition)
        {
            string sql = $"select wt.*,[StackName],TaskTypeCn= case wt.TaskType when 1 then '入库' when 2 then '出库' when 3 then '拣选' when 4 then '上料' when 5 then '整库' else '未知' end from HuangGangBWMS.[dbo].[WMS_Task] wt LEFT JOIN [HuangGangBWMS].[dbo].[PreparationTask] b ON wt.[TaskId]=b.[TaskId] where 1=1 {Condition} order by TaskId desc";
            return SqlHelper.GetSQL(sql, connectstring);
        }

        //手动过账任务
        public static int FinishTaskByHand(string taskid)
        {
            string sql = $"update HuangGangBWMS.[dbo].[WMS_Task] set TaskStatus=98,FinishTime=GETDATE(),ErrorMsg='手动过账完成' where TaskId={taskid} and TaskStatus<98";

            return SqlHelper.ExecSQL(sql, connectstring);
        }

        //查询请求列表
        public static DataSet SelectReqList(string Condition)
        {
            string sql = $"select * from [dbo].[WCS_Request] where 1=1 {Condition}";
            return SqlHelper.GetSQL(sql, connectstring);
        }

        //更新WCS任务中间表状态
        public static int UpdateTaskStatus(long TaskId, int Status, string timetype, string msg = "")
        {
            string sql = "";
            switch (timetype)
            {
                case "start":
                    sql = $"update HuangGangBWMS.dbo.WMS_Task set TaskStatus={Status},StartTime=GETDATE() where TaskId={TaskId}";
                    break;
                case "ddj":
                    sql = $"update HuangGangBWMS.dbo.WMS_Task set TaskStatus={Status},DdjTime=GETDATE()  where TaskId={TaskId}";
                    break;
                case "finish":
                    sql = $"update HuangGangBWMS.dbo.WMS_Task set TaskStatus={Status},FinishTime=GETDATE()  where TaskId={TaskId}";
                    break;
            }
            return SqlHelper.ExecSQL(sql, connectstring);
        }

        //设置属性
        public static int SetWcsConfig(string key, string value)
        {
            string sql = $"update HuangGangBWMS.[dbo].[WMS_WCSConfig] set Value='{value}' where Keys='{key}'";
            return SqlHelper.ExecSQL(sql, connectstring);
        }

        //根据WCS请求编号获取WMS生成的任务
        public static TaskInfo GetTaskByReqId(long ReqId, ref string message)
        {
            string sql = $"select * from HuangGangBWMS.[dbo].[WMS_Task] where ReqId={ReqId}";
            var dt = SqlHelper.GetSQL(sql, connectstring).Tables[0];
            if (dt.Rows.Count == 0)
            {
                message = $"WMS没有分配对应的任务，ReqID={ReqId}！";
                return null;
            }
            else if (dt.Rows.Count > 1)
            {
                message = $"WMS重复分配任务，ReqID={ReqId}！";
                return null;
            }

            var row = dt.Rows[0];
            var task = GetTaskObject(row);

            message = "";
            return task;
        }

        static TaskInfo GetTaskObject(DataRow row)
        {
            var task = new TaskInfo()
            {
                TaskId = Convert.ToInt64(row["TaskId"].ToString()),
                ReqId = row["ReqId"] == DBNull.Value ? 0 : Convert.ToInt64(row["ReqId"].ToString()),
                PilerNo = row["PilerNo"] == DBNull.Value ? 0 : Convert.ToInt32(row["PilerNo"].ToString()),
                ProductCode = row["ProductCode"].ToString(),
                Amount = row["Amount"] == DBNull.Value ? 0 : Convert.ToInt32(row["Amount"].ToString()),
                TaskType = Convert.ToInt32(row["TaskType"].ToString()),
                Priority = row["Priority"] == DBNull.Value ? 0 : Convert.ToInt32(row["Priority"].ToString()),
                FromPosition = row["FromPosition"].ToString(),
                ToPosition = row["ToPosition"].ToString(),
                TaskStatus = Convert.ToInt32(row["TaskType"].ToString()),
                CreateTime = Convert.ToDateTime(row["CreateTime"].ToString()),
                ErrorMsg = row["ErrorMsg"].ToString(),
            };

            if (row["StartTime"] != DBNull.Value)
            {
                task.StartTime = Convert.ToDateTime(row["StartTime"]);
            }
            if (row["DdjTime"] != DBNull.Value)
            {
                task.DdjTime = Convert.ToDateTime(row["DdjTime"]);
            }
            if (row["FinishTime"] != DBNull.Value)
            {
                task.FinishTime = Convert.ToDateTime(row["FinishTime"]);
            }

            if (row["HasUpProtect"] != DBNull.Value)
            {
                task.HasUpProtect = Convert.ToBoolean(row["HasUpProtect"]);
            }
            else
            {
                task.HasUpProtect = false;
            }

            if (task.FromPosition.IndexOf(".") > 0)
            {
                task.ddj = (Convert.ToInt32(task.FromPosition.Split('.')[0]) + 1) / 2;
            }
            if (task.ToPosition.IndexOf(".") > 0)
            {
                task.ddj = (Convert.ToInt32(task.ToPosition.Split('.')[0]) + 1) / 2;
            }
            if (task.TaskType == 1)
            {
                task.target = 105 - task.ddj;
                task.ToRow = int.Parse(task.ToPosition.Split('.')[0]);
                task.ToColumn = int.Parse(task.ToPosition.Split('.')[1]);
                task.ToLayer = int.Parse(task.ToPosition.Split('.')[2]);
            }
            else if (task.TaskType == 2)
            {
                task.target = int.Parse(task.ToPosition);
                if (task.target >= 3001) { task.target = 1001; }
                task.FromRow = int.Parse(task.FromPosition.Split('.')[0]);
                task.FromColumn = int.Parse(task.FromPosition.Split('.')[1]);
                task.FromLayer = int.Parse(task.FromPosition.Split('.')[2]);
            }

            return task;
        }

        #region RGV
        //创建一条RGV任务,并且获取任务ID
        public static void CreateRGVTask(RGVTask task)
        {
            string sql1 = $@"insert into [dbo].[RGV_Task](TaskType,FromPosition,ToPosition,PilerNo,[Status],CreateTime)
                values({task.TaskType},{task.FromPosition},{task.ToPosition},{task.PilerNo},1,GETDATE())";

             Sql.ExecSQL(sql1);
        }

        //获取RGV任务列表
        public static DataTable GetRGVTaskList(string where)
        {
            string sql1 = $"select * from [dbo].[RGV_Task] where 1=1 {where} order by CreateTime desc";
            return Sql.GetSQL(sql1).Tables[0];
        }

        //按时间顺序获取一条最早的RGV任务
        public static RGVTask GetNextRGVTask()
        {
            string sql1 = "select top 1 * from [dbo].[RGV_Task] where  [Status]=1 order by CreateTime ASC";
            var ds = Sql.GetSQL(sql1);
            if (ds.Tables[0].Rows.Count <= 0)
            {
                return null;
            }
            else
            {
                var row = ds.Tables[0].Rows[0];
                var rgv_task = new RGVTask();
                rgv_task.RTaskId = int.Parse(row["RTaskId"].ToString());
                rgv_task.FromPosition = int.Parse(row["FromPosition"].ToString());
                rgv_task.ToPosition = int.Parse(row["ToPosition"].ToString());
                rgv_task.PilerNo = int.Parse(row["PilerNo"].ToString());
                return rgv_task;
            }
        }

        //在WMS任务表中获取上料任务
        public static RGVTask GetRGVTaskFromWMS()
        {
            string sql1 = "select top 1 * from HuangGangBWMS.[dbo].[WMS_Task] where  [TaskType] IN (4,6)  and  [TaskStatus]=1 order by CreateTime ASC";
            var dt = Sql.GetSQL(sql1).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                return null;
            }
            else
            {
                var row = dt.Rows[0];
                var rgv_task = new RGVTask();
                rgv_task.RTaskId = int.Parse(row["TaskId"].ToString());
                rgv_task.FromPosition = int.Parse(row["FromPosition"].ToString());
                rgv_task.ToPosition = int.Parse(row["ToPosition"].ToString());
                rgv_task.PilerNo = int.Parse(row["PilerNo"].ToString());
                rgv_task.TaskType = int.Parse(row["TaskType"].ToString())==4?3:13; //上料任务:3为缓存架到设备，13是设备到另一个设备
                return rgv_task;
            }
        }

        //找RGV报警错误信息
        public static string GetRGVErrorCommentByIndex(int index)
        {
            var sql1 = $"select Comment from [dbo].[WCS_RGVErrorDic] where SortIndex={index}";

            var dt = Sql.GetSQL(sql1).Tables[0];

            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["Comment"].ToString();
            }
            else
            {
                return "未知";
            }
        }

        //更改RGV任务状态
        public static void UpdateRGVTaskStatus(int status, int Rid)
        {
            string sql1 = "";
            if (status == 20)
            {
                sql1 = $"update [dbo].[RGV_Task] set [Status]={status},StartTime=getdate() where RTaskId={Rid}";
            }
            else if (status == 98)
            {
                sql1 = $"update [dbo].[RGV_Task] set [Status]={status},FinishTime=getdate() where RTaskId={Rid}";
            }
            Sql.ExecSQL(sql1);
        }

        //根据垛号获取RGV任务
        public static RGVTask GetRGVTaskByPilerNo(int plierNo)
        {
            string sql1 = $"select * from [dbo].[RGV_Task] where  [Status]<=20  and  PilerNo={plierNo}";
            var ds = Sql.GetSQL(sql1);
            if (ds.Tables[0].Rows.Count <= 0)
            {
                return null;
            }
            else
            {
                var row = ds.Tables[0].Rows[0];
                var rgv_task = new RGVTask();
                rgv_task.RTaskId = int.Parse(row["RTaskId"].ToString());
                rgv_task.FromPosition = int.Parse(row["FromPosition"].ToString());
                rgv_task.ToPosition = int.Parse(row["ToPosition"].ToString());
                rgv_task.PilerNo = int.Parse(row["PilerNo"].ToString());
                return rgv_task;
            }
        }
        #endregion

        #region 板材入库
        //获取由WMS扫码分配的入库任务
        public static TaskInfo GetInWareTaskFromWMS(ref string msg)
        {
            string sql = "select top 1 * from HuangGangBWMS.[dbo].[WMS_Task] where TaskType=1 and ReqId is null and TaskStatus=1 and FromPosition=105 order by TaskId";
            var dt = SqlHelper.GetSQL(sql, connectstring).Tables[0];
            if (dt.Rows.Count == 0)
            {
                msg = "WMS没有分配入库任务！";
                return null;
            }
            //else if (dt.Rows.Count > 1)
            //{
            //    msg = "WMS重复分配入库任务！";
            //    return null;
            //}

            var row = dt.Rows[0];
            var task = GetTaskObject(row);

            msg = "";
            return task;
        }

        //获取即将从105入口入库的任务列表
        public static DataTable GetInWareList()
        {
            var sql1 = "select PilerNo,ProductCode,CreateTime from HuangGangBWMS.[dbo].[WMS_Task] where TaskType=1 and ReqId is null and TaskStatus=1 and FromPosition=105 order by TaskId";

            return SqlHelper.GetSQL(sql1, connectstring).Tables[0];
        }

        //获取所有花色
        public static DataTable GetProductCode()
        {
            var sql1 = "select ProductCode from HGWCSBC.dbo.ProductCodeWeight order by [Weight]";

            return SqlHelper.GetSQL(sql1, connectstring).Tables[0];
        }

        //根据垛号获取正在执行中的任务   1<TaskStatus<98
        public static TaskInfo GetTaskByPilerNo(int pilerNo, int taskType = 0)
        {
            string sql = "";
            if (taskType > 0)
            {
                sql = $"select * from HuangGangBWMS.[dbo].[WMS_Task] where TaskType={taskType}  and TaskStatus<98 and PilerNo={pilerNo}";
            }
            else
            {
                sql = $"select * from HuangGangBWMS.[dbo].[WMS_Task] where TaskStatus<98 and PilerNo={pilerNo}";
            }
            var dt = SqlHelper.GetSQL(sql, connectstring).Tables[0];

            if (dt.Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return GetTaskObject(dt.Rows[0]);
            }
        }
        #endregion

        #region 拣选
        //获取当前正在执行的拣选任务
        public static string GetCurrentSortTaskId(out long TaskId)
        {
            string sql = "select * from HuangGangBWMS.[dbo].[WMS_Task] where TaskType=3 and TaskStatus=20";
            var dt = SqlHelper.GetSQL(sql, connectstring).Tables[0];

            if (dt.Rows.Count == 0)
            {
                TaskId = 0;
                return "";
            }
            if (dt.Rows.Count > 1)
            {
                TaskId = 0;
                return "当前有多条正在执行的拣选任务！请手动处理！";
            }
            TaskId = Convert.ToInt64(dt.Rows[0]["TaskId"]);
            return "";
        }

        //获取新的拣选任务的要料信息
        public static DataTable GetNewSortNeedInfo()
        {
            string sql = @"select TaskId,ProductCode,Amount=count(1) from HuangGangBWMS.[dbo].[WMS_SortInfo] where TaskId in
                                (
                                select top 1 TaskId from HuangGangBWMS.[dbo].[WMS_Task] where TaskType = 3 And TaskStatus = 1 order by TaskId 
                                ) group by TaskId,ProductCode";
            return SqlHelper.GetSQL(sql, connectstring).Tables[0];
        }

        //计算WCS存储的拣选工位的板件数量
        public static void AddSortStationCount(int from, int to)
        {
            string sql1 = $@"update SortStationStatus set Amount=Amount-1 where StationNo={from};
                                        update SortStationStatus set Amount=Amount+1 where StationNo={to};";
            SqlHelper.ExecSQL(sql1, connectstring);
        }

        //设置WCS存储的拣选工位的板件数量
        public static void SetSortStationCount(int stationNo,int amount)
        {
            string sql1 = $"update SortStationStatus set Amount={amount} where StationNo={stationNo}";
            SortingStationInfo ssi;
            string sql2 =
                $"UPDATE [dbo].[{nameof(SortingStationInfo)}] SET [{nameof(ssi.BookCount)}]={amount} WHERE [{nameof(ssi.StationNo)}]={stationNo}";
            SqlHelper.ExecSQL(sql1, connectstring);
            SqlHelper.ExecSQL(sql2, connectstring);
        }

        ////检验PLC存储的板件数量与WCS存储的数量是否一致
        //public static bool CheckSortCount(int stationNo, int plcCount)
        //{
        //    string sql1 = $"select * from SortStationStatus where StationNo={stationNo} and Amount={plcCount}";
        //    var dt = SqlHelper.GetSQL(sql1, connectstring).Tables[0];
        //    if (dt.Rows.Count > 0)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //获取使用率最小的花色
        public static string GetLessUseProduct(List<string> lst)
        {
            var para1 = new SqlParameter("@productcode1", SqlDbType.VarChar);
            para1.Value = lst[0];
            var para2 = new SqlParameter("@productcode2", SqlDbType.VarChar);
            para2.Value = lst[1];
            var para3 = new SqlParameter("@productcode3", SqlDbType.VarChar);
            para3.Value = lst.Count >= 3 ? lst[2] : "";
            var para4 = new SqlParameter("@less_productcode", SqlDbType.VarChar, 30);
            para4.Direction = ParameterDirection.Output;

            try
            {
                return SqlHelper.ProcOutput("wcs_proc_CalculateProduct",
                    new SqlParameter[] { para1, para2, para3, para4 }, "@less_productcode", connectstring).ToString();
            }
            catch(Exception ex)
            {
                return lst[0];
            }
        }

        //判断拣选任务是否已完成   0 未完成   1 完成    998 异常，拣选数量与实际不一致 
        public static int CheckSortTaskIsFinished(long TaskId, int plc_amount)
        {
            if (plc_amount <= 1) { return 0; }
            plc_amount--;
            var para1 = new SqlParameter("@taskId", SqlDbType.BigInt);
            para1.Value = TaskId;
            var para2 = new SqlParameter("@plc_amount", SqlDbType.Int);
            para2.Value = plc_amount;
            var para3 = new SqlParameter("@result", SqlDbType.Int);
            para3.Direction = ParameterDirection.Output;

            try
            {
                return Convert.ToInt32(SqlHelper.ProcOutput("wcs_proc_CheckHasFinishedSortTask",
                    new SqlParameter[] { para1, para2, para3 }, "@result", connectstring));
            }
            catch
            {
                return 999;
            }
        }

        //更新拣选明细状态
        public static int UpdateSortItemStatus(long TaskId, int StackIndex)
        {
            string sql = $"update HuangGangBWMS.dbo.WMS_SortInfo set SortStatus=1 where TaskId={TaskId} and StackIndex={StackIndex}";
            return SqlHelper.ExecSQL(sql, connectstring);
        }

        //获取拣选工位绑定的 任务ID
        public static long GetSortStationTaskId(int stationNo)
        {
            string sql1 = "select SortId from [dbo].[SortStationStatus] where StationNo=" + stationNo.ToString();

            var ds = Sql.GetSQL(sql1);
            return Convert.ToInt64(ds.Tables[0].Rows[0][0]);
        }

        
        //public static long GetSortStationTaskId2(int stationNo)
        //{
        //    string sql1 = "select SortId2 from [dbo].[SortStationStatus] where StationNo=" + stationNo.ToString();

        //    var ds = Sql.GetSQL(sql1);
        //    return Convert.ToInt64(ds.Tables[0].Rows[0][0]);
        //}

        //将拣选工位与站台进行绑定 2001,2002,2004，拣选要料申请成功时绑定，以免重复向WMS申请要料出库
        public static void BindSortIdToStation(string stationNo, long sortId)
        {
            string sql1 = $"update [dbo].[SortStationStatus] set SortId={sortId} where StationNo={stationNo}";
            Sql.ExecSQL(sql1);
        }

        //余料回库申请成功时绑定，以免重复向WMS申请余料回库
        //public static void BindSortId2ToStation(string stationNo, long sortId)
        //{
        //    string sql1 = $"update [dbo].[SortStationStatus] set SortId2={sortId} where StationNo={stationNo}";
        //    Sql.ExecSQL(sql1);
        //}

        //每次开始拣选前，必须保证WMS下发的数据准确无误
        public static string CheckSortDetail(long TaskId)
        {
            string sql = $"select * from HuangGangBWMS.dbo.WMS_SortInfo where TaskId={TaskId} order by StackIndex ";
            var dt = SqlHelper.GetSQL(sql, connectstring).Tables[0];

            if (dt.Rows.Count == 0)
            {
                return $"拣选任务【{TaskId}】没有明细数据，请找WMS系统核对";
            }

            var index = 1;
            foreach (DataRow dr in dt.Rows)
            {
                if (Convert.ToInt32(dr["StackIndex"].ToString()) != index)
                {
                    return $"拣选任务【{TaskId}】明细数据错误，请找WMS系统核对！";
                }

                if (Convert.ToBoolean(dr["SortStatus"]))
                {
                    return $"拣选任务【{TaskId}】状态数据错误，请找WMS系统核对！";
                }
                index++;
            }

            return "";
        }

        //获取下一块板的花色
        public static string GetNextProductCode(long TaskId, int stackIndex)
        {
            string sql = $"select ProductCode from HuangGangBWMS.dbo.WMS_SortInfo where TaskId={TaskId} and StackIndex={stackIndex}";
            return SqlHelper.GetSqlItem(sql, connectstring);
        }

        //设置拣选板的状态
        public static int SetSortInfoStatus(long TaskId, int stackIndex)
        {
            string sql = $"update HuangGangBWMS.dbo.WMS_SortInfo set SortStatus=1,SortTime=GETDATE() where TaskId={TaskId} and StackIndex={stackIndex}";
            return SqlHelper.ExecSQL(sql, connectstring);
        }
        #endregion

        #region 调用WMS接口
        public static WMSFeedBack RequestTask(RequestInfo req)
        {
            var feedback = new WMSFeedBack();
            try
            {
                var client = new WebServiceDemo.WMSServiceSoapClient();
                var para = new WebServiceDemo.WMSInParams();
                para.ReqType = req.ReqType;
                if (req.ReqType == 1)
                {
                    //拣选入库请求
                    para.TaskId = req.TaskId;
                    para.FromStation = int.Parse(req.FromPosition);
                }
                else if (req.ReqType == 2)
                {
                    //要料请求
                    para.ProductCode = req.ProductCode;
                    para.Count = req.Amount;
                    para.ToStation = int.Parse(req.ToPosition);
                }
                else if (req.ReqType == 3)
                {
                    //空垫板入库请求
                    para.Count = req.Amount;
                    para.FromStation = int.Parse(req.FromPosition);
                }
                else if (req.ReqType == 4)
                {
                    //余料回库请求
                    para.Count = req.Amount;
                    para.FromStation = int.Parse(req.FromPosition);
                    para.PilerNo = req.PilerNo;
                }

                var wms_rlt = client.ApplyWMSTask(para);
                
                feedback.Status = wms_rlt.Status;
                feedback.Message = wms_rlt.Message;
                feedback.ReqId = wms_rlt.ReqId;

                if (feedback.Status == 200)
                {
                    //只有WMS反馈成功才插入数据库
                    req.ReqId = feedback.ReqId;
                    CreateWcsRequest(req);
                }
            }
            catch (Exception ex)
            {
                feedback.Status = -1;
                feedback.Message = $"WCS请求任务失败：{ex.Message}";
            }

            return feedback;
        }
        #endregion

        //获取目标位为2005的有效任务数
        public static int GetEmptyPadTask()
        {
            string sql1 = "select 1 from HuangGangBWMS.[dbo].[WMS_Task] where ToPosition='2005' and TaskType=2 and TaskStatus<98";
            var dt = SqlHelper.GetSQL(sql1, connectstring).Tables[0];        

            return dt.Rows.Count;
        }

        //获取最新的一条错误信息
        public static string GetLastErrorMsg()
        {
            var sql1 = "select top 1 Msg from WCS_Log_Error where dateadd(second,5,StartDate)>=getdate() or dateadd(second,5,EndDate)>=getdate() order by [ID] desc";
            var dt = SqlHelper.GetSQL(sql1, connectstring).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString();
            }
            else
            {
                return "";
            }
        }

        public static DataTable GetDeviceData()
        {
            var sql1 = $" select * from HGWCSBC.dbo.[WCS_DeviceMonitor] ";

            return Sql.GetSQL(sql1).Tables[0];
        }

        //获取有状态的设备
        public static DataTable GetRunDevice()
        {
            var sql1 = $" select * from [WCS_DeviceMonitor] where Staus=1 ";
            return Sql.GetSQL(sql1).Tables[0];
        }

        //public static string GetErrorMsgNow()
        //{
        //    var sql1 = $"select top 1 Msg from [dbo].[WCS_Log_Opt] where (DATEDIFF(second,StartDate,GETDATE())<=10 or DATEDIFF(second,EndDate,GETDATE())<=10) and [Type]='ERROR' ";

        //    var dt = Sql.GetSQL(sql1).Tables[0];

        //    if (dt.Rows.Count <= 0)
        //    {
        //        return "";
        //    }
        //    else
        //    {
        //        return dt.Rows[0]["Msg"].ToString();
        //    }
        //}

        public static void UpdateSCInfo(int ddjNo, SCInfo info)
        {
            var sql1 = $"update HGWCSBC.[dbo].[WCS_DdjInfo] set IsActivation = {info.IsActivation}, IsAuto = {info.IsAuto},IsFree={info.IsFree},PilerNo = {info.PilerNo}, TaskType = {info.TaskType}, TaskStatus = {info.TaskStatus}, CurrentPos = '{info.CurrentPos}',FromPos='{info.FromPos}', ToPos = '{info.ToPos}',OutStationState = {info.OutStationState}, InStationState = {info.InStationState},ErrorMsg ='{info.ErrorMsg}' where DdjNo = {ddjNo}";
            Sql.ExecSQL(sql1);
        }

        public static SCInfo GetSCInfo(int ddjno)
        {
            var sql1 = $"SELECT [DdjNo] ,[IsActivation],[IsAuto],[IsFree],[PilerNo],[TaskType],[TaskStatus],[CurrentPos],[FromPos],[ToPos],[OutStationState],[InStationState],[ErrorMsg]  FROM [HGWCSBC].[dbo].[WCS_DdjInfo] where DdjNo = {ddjno}";
            var dt = Sql.GetSQL(sql1).Tables[0];
            var sc = new SCInfo();
            if (dt.Rows.Count > 0)
            {
                var dr = dt.Rows[0];
                sc.IsActivation = Convert.ToInt32(dr["IsActivation"]);
                sc.IsAuto = Convert.ToInt32(dr["IsAuto"]);
                sc.IsFree = Convert.ToInt32(dr["IsFree"]);
                sc.PilerNo = Convert.ToInt32(dr["PilerNo"]);
                sc.TaskType = Convert.ToInt32(dr["TaskType"]);
                sc.TaskStatus = Convert.ToInt32(dr["TaskStatus"]);
                sc.CurrentPos = dr["CurrentPos"].ToString();
                sc.FromPos = dr["FromPos"].ToString();
                sc.ToPos = dr["ToPos"].ToString();
                sc.OutStationState = Convert.ToInt32(dr["OutStationState"]);
                sc.InStationState = Convert.ToInt32(dr["InStationState"]);
                sc.ErrorMsg = dr["ErrorMsg"].ToString();
                return sc;
            }

            return null;
        }

        public static int GetItemNoByStationNo(int stationNo)
        {
            var sql1 = $@"select d.ItemNo from HGWCSBC.[dbo].[WCS_DeviceMonitor] d 
                    left join HGWCSBC.[dbo].WCS_Map m on d.DeviceNo = m.DeviceNo
                    where m.StationNo = {stationNo}";
            var dt = Sql.GetSQL(sql1);
            return int.Parse(dt.Tables[0].Rows[0][0].ToString());
        }

        public static int GetItemNoByDeviceNo(string dno)
        {
            var sql1 = $"select ItemNo from HGWCSBC.[dbo].[WCS_DeviceMonitor] where DeviceNo='{dno}'";
            var dt = Sql.GetSQL(sql1);
            return int.Parse(dt.Tables[0].Rows[0][0].ToString());
        }

        public static int GetItemNoByDeviceNo2(string dno)
        {
            var sql1 = $"select ItemNo from HGWCSBC.[dbo].[WCS_DeviceAlerm] where DeviceNo='{dno}'";
            var dt = Sql.GetSQL(sql1);
            return int.Parse(dt.Tables[0].Rows[0][0].ToString());
        }

        public static DataTable GetAllDevice()
        {
            var sql1 = "select * from HGWCSBC.[dbo].[WCS_DeviceMonitor]";
            var ds = Sql.GetSQL(sql1);
            return ds.Tables[0];
        }

        public static void F(string sql1)
        {
            Sql.ExecSQL(sql1);
        }

        /// <summary>
        /// 创建67电子锯手动上料任务
        /// </summary>
        /// <param name="toPosition"></param>
        /// <param name="pilerNo"></param>
        /// <param name="fromPosition"></param>
        /// <param name="taskType"></param>
        public static void CreatedCutting67ManualTask( string toPosition, int pilerNo=0, string fromPosition="105", int taskType=10)
        {
            var para1 = new SqlParameter("@PilerNo", SqlDbType.Int);
            para1.Value = pilerNo;
            var para2 = new SqlParameter("@TaskType", SqlDbType.Int);
            para2.Value = taskType;

            var para3 = new SqlParameter("@FromPosition", SqlDbType.VarChar,30);
            para1.Value = fromPosition;
            var para4 = new SqlParameter("@ToPosition", SqlDbType.VarChar,30);
            para2.Value = toPosition;
            SqlHelper.RunProc("CreatedManualTask", new SqlParameter[]{para1,para2,para3,para4}, connectstring);
        }

        public static bool DeleteRgvTask(int rTaskId)
        {
            string deleteSql = $"DELETE RGV_Task WHERE RTaskId={rTaskId}";
            return SqlHelper.ExecSQL(deleteSql,  connectstring) > 0;
        }

        public static ManualTask GetCutting67ManualTask(int taskStatus=0,int pilerNo=0)
        {
            string sql =
                $"SELECT TOP 1 [TaskId],PilerNo,TaskType,FromPosition,ToPosition,[TaskStatus] FROM [HGWCSBC].[dbo].[ManualTask]  WHERE [TaskStatus]={taskStatus} AND [TaskType]=10 AND (0={pilerNo} OR PilerNo={pilerNo}) ORDER BY TaskId";
            var dataSet = SqlHelper.ExecuteDataset(connectstring, CommandType.Text, sql);
            var dt = dataSet.Tables[0];
            if (dt.Rows.Count == 0) return null;
            var row = dt.Rows[0];
            return  new ManualTask()
            {
                TaskId = Convert.ToInt64(row["TaskId"].ToString()),
                PilerNo = Convert.ToInt32(row["PilerNo"].ToString()),
                TaskStatus = Convert.ToInt32(row["TaskStatus"].ToString()),
                FromPosition = row["FromPosition"].ToString(),
                ToPosition = row["ToPosition"].ToString(),
                TaskType = Convert.ToInt32(row["TaskType"].ToString())
            };
        }

        public static bool UpdateCutting67ManualTask(long taskId,int taskStatus)
        {
            string sql =$"UPDATE [HGWCSBC].[dbo].[ManualTask] SET [TaskStatus]={taskStatus} WHERE [TaskId]={taskId}";
            return SqlHelper.ExecSQL(sql, null, connectstring)>0;

        }
    }
}
