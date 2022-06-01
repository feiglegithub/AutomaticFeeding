using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using WCS.model;

namespace WCS.DataBase
{
    public class WCSSql
    {
        static string connectstring = AppCommon.WCSConnstr;
        static string connect_time = AppCommon.WCSConnstr2;

        //插入日志记录
        public static void InsertLog(string msg, string type, int pilerno = 0)
        {
            var para1 = new SqlParameter("@Type", SqlDbType.VarChar);
            para1.Value = type;
            var para2 = new SqlParameter("@PilerNo", SqlDbType.BigInt);
            para2.Value = pilerno;
            var para3 = new SqlParameter("@Msg", SqlDbType.NVarChar);
            para3.Value = msg;

            try
            {
                SqlHelper.RunProc("wcs_proc_InsertLog", new SqlParameter[] { para1, para2, para3 }, connectstring);
            }
            catch { }
        }

        //重载
        public static void InsertLog(string msg, string type, string startDate, string endDate, int pilerno)
        {
            string sql1 = $@"insert into [dbo].[Wcs_Log_Opt]([Type],PilerNo,Msg,StartDate,EndDate) values('{type}',{pilerno},
                '{msg}','{startDate}','{endDate}')";

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
            return msgs;
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
                sql1 = "select top 30 ID,Times,Msg,StartDate,EndDate from [dbo].[Wcs_Log_Opt] where [Type]='ERROR' order by ID DESC";
            }
            return Sql.GetSQL(sql1).Tables[0];
        }

        #region 测试代码
        public static TaskInfo Demo_GetLastTask(int ddjno, int r)
        {
            //string sql1 = $"select top 1 * from HuangGangBWMS.[dbo].[WMS_Task] where TaskType = 1 and DdjNo = {ddjno} and TaskStatus=98 and LEFT(ToPosition,1)={r} order by TaskId desc";
            //var dt = SqlHelper.GetSQL(sql1, connectstring).Tables[0];
            //var row = dt.Rows[0];
            //var task = GetTaskObject(row);
            //return task;
            return null;
        }

        //获取一条出库任务
        public static TaskInfo Demo_GetOutTask(int ddjno)
        {
            string sql1 = $"select top 1 * from HuangGangBWMS.[dbo].[WMS_Task] where TaskType=2 and DdjNo={ddjno} and TaskStatus=1";
            var dt = SqlHelper.GetSQL(sql1, connectstring).Tables[0];
            //if (dt.Rows.Count > 0)
            //{
            //    var row = dt.Rows[0];
            //    var task = GetTaskObject(row);
            //    return task;
            //}
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

        //获取服务器(10.30.3.116)的时间
        public static string GetServiceTime()
        {
            string sql1 = " select GETDATE() ";
            var ds = Sql.GetSQL2(sql1);
            if (ds != null)
            {
                return ds.Tables[0].Rows[0][0].ToString();
            }
            else
            {
                return "2018-10-1";
            }
        }

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
            string sql = $"select *,TaskTypeCn= case TaskType when 1 then '入库' when 2 then '出库' when 3 then '拣选' else '未知' end from HuangGangBWMS.[dbo].[WMS_Task] where 1=1 {Condition} order by TaskId desc";
            return SqlHelper.GetSQL(sql, connectstring);
        }

        //手动过账任务
        public static int FinishTaskByHand(string taskid)
        {
            string sql = $"update HuangGangBWMS.[dbo].[WMS_Task] set TaskStatus=98,FinishTime=Getdate(),ErrorMsg='手动过账完成' where TaskId={taskid} and TaskStatus<98";

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
                    sql = $"update HuangGangBWMS.dbo.WMS_Task set TaskStatus={Status},StartTime=getdate(),ErrorMsg=ISNULL(ErrorMsg,'')+'{msg}|' where TaskId={TaskId}";
                    break;
                case "ddj":
                    sql = $"update HuangGangBWMS.dbo.WMS_Task set TaskStatus={Status},DdjTime=getdate(),ErrorMsg=ISNULL(ErrorMsg,'')+'{msg}|' where TaskId={TaskId}";
                    break;
                case "finish":
                    sql = $"update HuangGangBWMS.dbo.WMS_Task set TaskStatus={Status},FinishTime=getdate(),ErrorMsg=ISNULL(ErrorMsg,'')+'{msg}|' where TaskId={TaskId}";
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
            //var task = GetTaskObject(row);

            message = "";
            //return task;

            return null;
        }

        static void GetTaskObject(DataRow row)
        {
            //var task = new TaskInfo()
            //{
            //    TaskId = Convert.ToInt64(row["TaskId"].ToString()),
            //    ReqId = row["ReqId"] == DBNull.Value ? 0 : Convert.ToInt64(row["ReqId"].ToString()),
            //    PilerNo = row["PilerNo"] == DBNull.Value ? 0 : Convert.ToInt32(row["PilerNo"].ToString()),
            //    ProductCode = row["ProductCode"].ToString(),
            //    Amount = row["Amount"] == DBNull.Value ? 0 : Convert.ToInt32(row["Amount"].ToString()),
            //    TaskType = Convert.ToInt32(row["TaskType"].ToString()),
            //    Priority = row["Priority"] == DBNull.Value ? 0 : Convert.ToInt32(row["Priority"].ToString()),
            //    FromPosition = row["FromPosition"].ToString(),
            //    ToPosition = row["ToPosition"].ToString(),
            //    TaskStatus = Convert.ToInt32(row["TaskType"].ToString()),
            //    CreateTime = Convert.ToDateTime(row["CreateTime"].ToString()),
            //    ErrorMsg = row["ErrorMsg"].ToString(),
            //};

            //if (row["StartTime"] != DBNull.Value)
            //{
            //    task.StartTime = Convert.ToDateTime(row["StartTime"]);
            //}
            //if (row["DdjTime"] != DBNull.Value)
            //{
            //    task.DdjTime = Convert.ToDateTime(row["DdjTime"]);
            //}
            //if (row["FinishTime"] != DBNull.Value)
            //{
            //    task.FinishTime = Convert.ToDateTime(row["FinishTime"]);
            //}

            //if (task.FromPosition.IndexOf(".") > 0)
            //{
            //    task.ddj = (Convert.ToInt32(task.FromPosition.Split('.')[0]) + 1) / 2;
            //}
            //if (task.ToPosition.IndexOf(".") > 0)
            //{
            //    task.ddj = (Convert.ToInt32(task.ToPosition.Split('.')[0]) + 1) / 2;
            //}
            //if (task.TaskType == 1)
            //{
            //    task.target = 105 - task.ddj;
            //    task.ToRow = int.Parse(task.ToPosition.Split('.')[0]);
            //    task.ToColumn = int.Parse(task.ToPosition.Split('.')[1]);
            //    task.ToLayer = int.Parse(task.ToPosition.Split('.')[2]);
            //}
            //else if (task.TaskType == 2)
            //{
            //    task.target = int.Parse(task.ToPosition);
            //    if (task.target >= 3001) { task.target = 1001; }
            //    task.FromRow = int.Parse(task.FromPosition.Split('.')[0]);
            //    task.FromColumn = int.Parse(task.FromPosition.Split('.')[1]);
            //    task.FromLayer = int.Parse(task.FromPosition.Split('.')[2]);
            //}

            //return task;
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
            string sql1 = "select top 1 * from HuangGangBWMS.[dbo].[WMS_Task] where  [TaskType]=4  and  [TaskStatus]=1 order by CreateTime ASC";
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
                rgv_task.TaskType = 3; //上料任务
                return rgv_task;
            }
        }


        //更改RGV任务状态
        public static void UpdateRGVTaskStatus(int status, int Rid)
        {
            string sql1 = $"update [dbo].[RGV_Task] set [Status]={status} where RTaskId={Rid}";
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
            string sql = "select * from [dbo].[WMS_Task] where TaskType=1 and ReqId is null and TaskStatus=1";
            var dt = SqlHelper.GetSQL(sql, connectstring).Tables[0];
            if (dt.Rows.Count == 0)
            {
                msg = "WMS没有分配入库任务！";
                return null;
            }
            else if (dt.Rows.Count > 1)
            {
                msg = "WMS重复分配入库任务！";
                return null;
            }

            var row = dt.Rows[0];
            //var task = GetTaskObject(row);

            msg = "";
            return null;
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
                sql = $"select * from HuangGangBWMS.[dbo].[WMS_Task] where TaskStatus<98 and TaskStatus>1 and PilerNo={pilerNo}";
            }
            var dt = SqlHelper.GetSQL(sql, connectstring).Tables[0];

            if (dt.Rows.Count == 0)
            {
                return null;
            }
            else
            {
                // return GetTaskObject(dt.Rows[0]);
                return null;
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
                                select top 1 TaskId from HuangGangBWMS.[dbo].[WMS_Task] where TaskType = 3 And TaskStatus = 1 order by TaskId desc
                                ) group by TaskId,ProductCode";
            return SqlHelper.GetSQL(sql, connectstring).Tables[0];
        }

        //获取使用率最小的花色
        public static string GetLessUseProduct(List<string> lst)
        {
            var para1 = new SqlParameter("@productcode1", SqlDbType.VarChar);
            para1.Value = lst[0];
            var para2 = new SqlParameter("@productcode2", SqlDbType.VarChar);
            para2.Value = lst[1];
            var para3 = new SqlParameter("@productcode3", SqlDbType.VarChar);
            para3.Value = lst.Count >= 3 ? lst[2] : "";
            var para4 = new SqlParameter("@less_productcode", SqlDbType.VarChar);
            para4.Direction = ParameterDirection.Output;

            try
            {
                return SqlHelper.ProcOutput("wcs_proc_CalculateProduct",
                    new SqlParameter[] { para1, para2, para3, para4 }, "@less_productcode", connectstring).ToString();
            }
            catch
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

        //将拣选工位与站台进行绑定 2001,2002,2004
        public static void BindSortIdToStation(string stationNo, long sortId)
        {
            string sql1 = $"update [dbo].[SortStationStatus] set SortId={sortId} where StationNo={stationNo}";
            Sql.ExecSQL(sql1);
        }

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

                if (Convert.ToInt32(dr["SortStatus"].ToString()) != 0)
                {
                    return $"拣选任务【{TaskId}】明细数据错误，请找WMS系统核对！";
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
            string sql = $"update HuangGangBWMS.dbo.WMS_SortInfo set SortStatus=1 where TaskId={TaskId} and StackIndex={stackIndex}";
            return SqlHelper.ExecSQL(sql, connectstring);
        }
        #endregion

        #region 调用WMS接口
        public static WMSFeedBack RequestTask(RequestInfo req)
        {
            var feedback = new WMSFeedBack();

            return feedback;
        }
        #endregion

        //获取目标位为2005的有效任务数
        public static int GetEmptyPadTask()
        {
            string sql1 = "select 1 from [HGWCSBC].[dbo].[WMS_Task] where ToPosition='2005' and TaskType=2 and TaskStatus<98";
            var dt = SqlHelper.GetSQL(sql1, connectstring).Tables[0];        

            return dt.Rows.Count;
        }
    } 
}
