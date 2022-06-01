using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace WCS
{   
    public class Sql
    {
        #region BaseSql
        public static int ExecSQL(string sql)
        {
            SqlConnection myConnection = new SqlConnection(AppCommon.WCSConnstr);
            myConnection.Open();
            SqlCommand tempSqlCommand = new SqlCommand(sql, myConnection);
            int intNumber = tempSqlCommand.ExecuteNonQuery();//返回数据库中影响的行数
            myConnection.Close();
            myConnection.Dispose();
            return intNumber;
        }

        public static int GetSqlNum(string sql)
        {
            DataSet ds = GetSQL(sql);
            return int.Parse(ds.Tables[0].Rows[0][0].ToString());
        }

        public static DataSet GetSQL(string sql)
        {
            DataSet ds = new DataSet();
            SqlConnection myConnection = new SqlConnection(AppCommon.WCSConnstr);
            myConnection.Open();

            SqlCommand dbCmd = new SqlCommand(sql, myConnection);
            SqlDataAdapter dbAdapt = new SqlDataAdapter(sql, myConnection);
            dbAdapt.SelectCommand = dbCmd;
            dbAdapt.Fill(ds, "ds");
            myConnection.Close();
            myConnection.Dispose();
            if (ds == null || ds.Tables == null || ds.Tables.Count == 0)
                throw new Exception("无法获取到数据：" + sql);
            return ds;
        }
        #endregion

        #region WcsSql
        public static DataSet GetTask(string pallet)
        {
            //string Sql = string.Format("SELECT * FROM Wcs_Task WHERE NPalletID = '{0}' AND NwkStatus < 98", pallet);
            string Sql = string.Format(@" SELECT * FROM Wcs_Task WHERE NwkStatus < 98 AND NPALLETID = '{0}' 
                union
            SELECT * FROM Wcs_Task_ByHand WHERE NwkStatus < 98 AND NPALLETID = '{0}' order by DoptDate", pallet);
            return GetSQL(Sql);
        }

        //入库专用，测试是否有问题
        public static DataSet GetTask(string pallet, bool isIn)
        {
            //string Sql = string.Format("SELECT * FROM Wcs_Task WHERE NPalletID = '{0}' AND NwkStatus < 98", pallet);
            string Sql = string.Format(" SELECT * FROM Wcs_Task WHERE NwkStatus = 1 AND NPALLETID = '{0}' ", pallet);
            return GetSQL(Sql);
        }

        //测试程序   临时使用
        public static DataSet GetTaskIn(string pilerno)
        {
            string sql1 = $"select * from [dbo].[WMS_Task] where PilerNo={pilerno} and TaskType=1 and TaskStatus<=20";
            return GetSQL(sql1);
        }

     

        public static void LogDdjRunDateTime(string type, DateTime datetime, string taskno)
        {
            string sql = "";
            if (type.Equals("S"))
            {
                //记录堆垛机开始执行时间
                sql = string.Format(" UPDATE Wcs_Task SET Ddj_StartDate = '{1}' WHERE SeqID={0}", taskno, datetime.ToString());
            }
            else
            {
                //记录堆垛机结束时间
                sql = string.Format(" UPDATE Wcs_Task SET Ddj_EndDate = '{1}' WHERE SeqID={0}", taskno, datetime.ToString());
            }

            ExecSQL(sql);
        }

        //需按照优先级修改
        public static DataSet GetTaskOut_2(int ddj)
        {
            string sql_2 = "SELECT  count(1)  FROM Wcs_Task where (NordID = 3 or NordID = 6) and NwkStatus<98 and NwkStatus>1 and noptstation = 205";  //205口缓存量
            string sql_3 = "SELECT  count(1)  FROM Wcs_Task where NordID = 3 and NwkStatus<98 and NwkStatus>1 and noptstation = 204";  //204标准件出口缓存量
            string sql_1 = string.Format(@"SELECT top 1 *  FROM Wcs_Task where (NordID = 3 or NordID = 6) and roadway = '{0}' and NwkStatus = 1 and noptstation/100=2 ", ddj);
            if (GetSqlNum(sql_2) > 0)
            {
                sql_1 = sql_1 + " and noptstation<>205";
            }
            if (GetSqlNum(sql_3) >= 3)
            {
                sql_1 = sql_1 + " and (noptstation<>204 or NordID = 6)";
            }
            //else
            //{
            //    sql_1 = string.Format(@"SELECT top 1 *  FROM Wcs_Task where (NordID = 3 or NordID = 6) and roadway = '{0}' and NwkStatus = 1 and noptstation/100=2", ddj);

            //}
            return GetSQL(sql_1);
        }

        //获取手动下架的任务
        public static DataSet GetTaskOut_ByWCS(int ddj)
        {
            string sql = string.Format("select top 1 * from [dbo].Wcs_Task_ByHand where NwkStatus=1 and Roadway={0}", ddj);
            return GetSQL(sql);
        }

        //需按照优先级修改
        public static DataSet GetTaskOut_1(int ddj)
        {
            DataSet ds = SQLDB();

            if (ds != null && ds.Tables.Count>0 && ds.Tables[0].Rows.Count > 0) 
            {
                string notstation = "";
                string nlotid = "";
                string NpackOrder = "";
                string seqid = "";
                foreach (DataRow dr in ds.Tables[0].Rows) 
                {
                    notstation = dr[0].ToString();
                    nlotid = dr[1].ToString();

                    string sql_1 = string.Format("SELECT  MIN(NpackOrder) FROM Wcs_Task WHERE NwkStatus = 1 AND NoptStation = {0} AND NlotID  = {1}", notstation, nlotid);
                    DataSet ds2 = GetSQL(sql_1);
                    if (ds2 != null && ds2.Tables.Count>0 && ds2.Tables[0].Rows.Count > 0) 
                    {
                        NpackOrder = ds2.Tables[0].Rows[0][0].ToString();

                        string sql_2 = string.Format(@"SELECT top 1 SeqID FROM Wcs_Task  
			                WHERE NoptStation  = {0}
			                AND NlotID = {1} 
			                AND NpackOrder = {2} 
			                AND NwkStatus = 1 AND Roadway = {3}
			                AND NoptStation > 100 and NoptStation < 119", notstation, nlotid, NpackOrder, ddj);
                        DataSet ds3 = GetSQL(sql_2);
                        if (ds3 != null && ds3.Tables.Count > 0 && ds3.Tables[0].Rows.Count > 0) 
                        {
                            seqid = ds3.Tables[0].Rows[0][0].ToString();
                            if (seqid != "") 
                            {
                                string sql_3 = string.Format("select 1 from Wcs_Task where NoptStation={0} and ( nlotid < {1} or (nlotid = {1} and npackorder < {2}) ) and nwkstatus<=30", notstation, nlotid, NpackOrder);
                                DataSet ds4 = GetSQL(sql_3);
                                if (ds4 != null && ds4.Tables.Count > 0 && ds4.Tables[0].Rows.Count == 0)
                                {
                                    break;
                                }
                                else 
                                {
                                    seqid = "";
                                    continue;
                                }
                            }
                        }
                    }
                }

                if (string.IsNullOrEmpty(seqid))
                    return null;

                string Sql = string.Format(@"SELECT *  FROM Wcs_Task where seqid = '{0}'", seqid);
                return GetSQL(Sql);
            }
            //SqlParameter[] para = new SqlParameter[1];
            //for (int i = 0; i < para.Length; i++)
            //{
            //    para[i] = new SqlParameter();
            //}
            //para[0] = new SqlParameter("@DdjNo", SqlDbType.Int);
            //para[0].Value = ddj;
            ////para[1] = new SqlParameter("@SeqID", SqlDbType.NVarChar, 50);
            ////para[1].Direction = ParameterDirection.Output;
            //DataSet ds = SqlHelper.ProcOutput("P_GetFYTask2", para, "@SeqID");
            //if (string.IsNullOrEmpty(seqid))
                return null;

            //string Sql = string.Format(@"SELECT *  FROM Wcs_Task where seqid = '{0}'", seqid);
            //return GetSQL(Sql);
        }

        public static DataSet SQLDB()
        {
            try
            {
                SqlConnection conn = new SqlConnection(AppCommon.WCSConnstr);

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "P_GetFYTask2";
                cmd.CommandType = CommandType.StoredProcedure;

                //SqlParameter myParm = new SqlParameter("@DdjNo", SqlDbType.Int);
                //myParm.Value = ddj;
                //cmd.Parameters.Add(myParm);

                SqlDataAdapter myCommand = new SqlDataAdapter();

                myCommand.SelectCommand = cmd;
                DataSet myDataSet = new DataSet();
                myCommand.Fill(myDataSet, "Results");

                return myDataSet;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally 
            {
                
            }
        } 



        public static void TestUpdateTask(string pallet, bool isIn)
        {
            string sql;
                sql = string.Format(" UPDATE Wcs_Task SET NwkStatus = '1',DFinishDate = GETDATE() WHERE NPalletID = '{0}' and nordid = '{1}' ", pallet, isIn ? 1 : 3);

            ExecSQL(sql);
        }

        public static void UpdateTaskState(string pallet, int state, string CFinishMode)
        {
            if (CFinishMode != null && CFinishMode.Equals("手动下架"))
            {
                UpdateTaskStateByHand(pallet, state);
                return;
            }

            string sql;
            if (state == 98)
            {
                sql = string.Format(" UPDATE Wcs_Task SET NwkStatus = '{0}',DFinishDate = GETDATE() WHERE NPalletID = '{1}' AND NwkStatus < 98 ", state, pallet);
            }
            else
            {
                sql = string.Format(" UPDATE Wcs_Task SET NwkStatus = '{0}',DStartDate = GETDATE() WHERE NPalletID = '{1}' AND NwkStatus < 98 ", state, pallet);
            }
            ExecSQL(sql);
        }

        //更新手动出库任务状态 
        public static void UpdateTaskStateByHand(string pallet, int state)
        {
            string sql;
            if (state == 98)
            {
                sql = string.Format(" UPDATE Wcs_Task_ByHand SET NwkStatus = '{0}',DFinishDate = GETDATE() WHERE NPalletID = '{1}' AND NwkStatus < 98 ", 99, pallet);
            }
            else
            {
                sql = string.Format(" UPDATE Wcs_Task_ByHand SET NwkStatus = '{0}',DStartDate = GETDATE() WHERE NPalletID = '{1}' AND NwkStatus < 98 ", state, pallet);
            }
            ExecSQL(sql);
        }

        //测试程序使用 填入一个出库程序
//        public static void InsertIntoOutTask(TaskInfo taskInfo)
//        {
//            string Sql = string.Format(@"INSERT INTO Wcs_Task
//	(NordID,NWorkID,NPalletID,CPosidFrom,
//	CPosidTo,NoptType,NoptStation,Npri,NlotID,
//	CPackType,NwkStatus,CFinishMode,Cuser,Quantitity,
//	Height,LedMsg,DoptDate,DStartDate,DFinishDate)
//	VALUES
//	(3,1,'{0}','{1}',
//	'','','{2}' ,1,1,
//	'Box',1,'Auto','Admin',5,
//	1,'LedMsgShowToYou',GETDATE(),null,null)",taskInfo.PallteNo,taskInfo.ToWareCell,taskInfo.InOut_Port);

//            ExecSQL(Sql);
//        }

        public static DateTime GetTime()
        {
            string sql = "select GETDATE() time";
            DataRow dr = GetSQL(sql).Tables[0].Rows[0];
            return Convert.ToDateTime(dr["time"].ToString());
        }

        public static void InsertSysLog(string msg)
        {
            msg = msg.Replace('\'', '\"');
            string Sql = string.Format("insert into Wcs_Log_Opt(Type,Msg,Date) values ('WcsOpt','{0}',GetDate())", msg);
            ExecSQL(Sql);

        }

        public static void InsertErrorLog(string errorMsg)
        {
            errorMsg = errorMsg.Replace('\'', '\"');
            string Sql = string.Format("insert into Wcs_Log_Error(ErrorType,ErrorMsg,ErrorDate) values ('WcsError','{0}',GetDate())", errorMsg);
            ExecSQL(Sql);
        }

        public static void InsertErrorLog(string errorMsg, DateTime SDateTime)
        {
            errorMsg = errorMsg.Replace('\'', '\"');
            string Sql = string.Format("insert into Wcs_Log_Error(ErrorType,ErrorMsg,ErrorDate,ErrorSDate) values ('WcsError','{0}',GetDate(),'{1}')", errorMsg, SDateTime);
            ExecSQL(Sql);
        }

        public static void UpdateErrorLog(string errorMsg)
        {
            string sql = string.Format(@"update Wcs_Log_Error set ErrorDate = GETDATE(),errorCount += 1 where ID = 
(select top 1 ID from Wcs_Log_Error where ErrorMsg = '{0}' order by ErrorDate desc)", errorMsg);
            ExecSQL(sql);
        }

        public static string GetErrorMsg(string hsName, int errorCode)
        {
            string Sql = string.Format("SELECT * FROM Wcs_HsErrorInfo where HsName = '{0}' and HsErrorCode = '{1}' ", hsName, errorCode);
            DataSet ds = GetSQL(Sql);
            return ds.Tables[0].Rows[0]["HsErrorInfo"].ToString();
        }

        public static bool IsOutLoop(string pallet) 
        {
            //防止重复的托盘流到同一个出货口
            string sql3 = string.Format("select count(1) from wcs_task where npalletid = '{0}' and nwkstatus = 32", pallet);
            if (GetSqlNum(sql3) > 0)
            {
                return true;
            }

            return false;
        }

        public static bool IsCanOutLoop(string pallet)
        {           
            string sql = string.Format("select top 1 * from wcs_task where npalletid = '{0}' and nwkstatus < 98", pallet);
            DataSet ds = GetSQL(sql);
            int station = int.Parse(ds.Tables[0].Rows[0]["NoptStation"].ToString());
            int nlotid = int.Parse(ds.Tables[0].Rows[0]["nlotid"].ToString());
            int npackorder = int.Parse(ds.Tables[0].Rows[0]["npackorder"].ToString());

            string sql2 = string.Format("select count(1) from wcs_task where noptstation = {0} and ( nlotid < {1} or (nlotid = {1} and npackorder < {2}) ) and nwkstatus < 32", station, nlotid, npackorder);
            int num = GetSqlNum(sql2);
            if (num != 0)
                return false;
            return true;
        }

        public static int GetTaskNum(int station)
        {
            string sql = string.Format("select count(1) from wcs_task where noptstation = '{0}' and nwkstatus < 98", station);
            return GetSqlNum(sql);
        }

        public static int GetTaskNum(int station, int status)
        {
            string sql = string.Format("select count(1) from wcs_task where noptstation = '{0}' and nwkstatus = '{1}'", station, status);
            return GetSqlNum(sql);
        }
        #endregion

        #region Webservice
        #region CreateTask
        //static string WMSCreateTask(int taskType,string pallet,int station,int height)
        //{
        //    try
        //    {
        //        WebService.interface2wcsServiceClient client = new WebService.interface2wcsServiceClient();
        //        WebService.applyTask aTask = new WebService.applyTask();
        //        aTask.HEIGHT = height;
        //        aTask.HEIGHTSpecified = true;
        //        aTask.NPALLETID = pallet;
        //        aTask.NORDID = taskType;
        //        aTask.NORDIDSpecified = true;
        //        aTask.NOPTSTATION = station.ToString();
        //        aTask.CSYSTYPE = "WCS";
        //        aTask.CWHID = "ZJ_ZNC";

        //        if (taskType == 5)  //空托盘入库
        //        {
        //            aTask.QUANTITYSpecified = true;
        //            aTask.QUANTITY = 4;
        //        }

        //        WebService.applyTaskResponse result = client.applyTask(aTask);
        //        int status = result.STATUS;
        //        string error = result.MESSAGE;

        //        if (error == null) 
        //        {
                
        //        }
        //        else if (error.Equals("没有可分配货位")) 
        //        {
        //            return error;
        //        }

        //        if (status != 0)
        //            throw new Exception(status + error);
        //        else
        //        {
        //            if (result.DATA.SEQID == null) 
        //            {
        //                status = -1;
        //                return "WMS没有返回任务ID!";
        //            }
        //            string sql = string.Format("select count(1) from [dbo].[Wcs_Task] where SeqID={0}", result.DATA.SEQID);
        //            if (GetSqlNum(sql) > 0)
        //            {
        //                return error;
        //            }
        //            else
        //            {
        //                InsertTask(result.DATA, height);
        //                return error;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //}

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

        static int  GetRoadway(bool isInware,string fromWare, string toWare)
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

        

 //       static void InsertTask(WebService.TaskInfo rt,int height)
 //       {
 //           int nordid = GI(rt.NORDID);
 //           bool isInware = IsInWare(nordid);
 //           int roadway = GetRoadway(isInware, rt.FROM_LOCATOR_CODE, rt.TO_LOCATOR_CODE);

 //           string sql = string.Format(@"INSERT INTO Wcs_Task
	//(SeqID,
 //   NordID,NWorkID,NPalletID,CPosidFrom,CPosidTo,
	//Roadway,NoptType,NoptStation,Npri,NlotID,
 //   CPackType,NwkStatus,CFinishMode,Cuser,Quantitity,
 //   Height,LedMsg,DoptDate,DStartDate,DFinishDate,CustomerName)
	//VALUES
	//('{0}','{1}','{2}','{3}','{4}',
	//'{5}','{6}' ,'{7}','{8}','{9}',
	//'{10}','{11}','{12}','{13}','{14}',
	//'{15}','{16}','{17}',GETDATE(),null,null,'{18}')", GS(rt.SEQID),
 //   GI(rt.NORDID), GI(rt.NWORKID), GS(rt.NPALLETID), isInware ? "" : GS(rt.FROM_LOCATOR_CODE), isInware ? GS(rt.TO_LOCATOR_CODE) : "",
 //   roadway, GS(rt.NOPTTYPE), GI(rt.NOPTSTATION), GI(rt.NPRI), GI(rt.NLOTID),
 //   GS(rt.CPACKTYPE), GI(rt.NWKSTATUS), GS(rt.CFINISHMODE), GS(rt.CUSERID), GI(rt.QUANTITY),
 //   height, GS(rt.LEDMSG), GS(rt.CUSTOMER_NAME));
 //           int count = ExecSQL(sql);
 //           if (count != 1)
 //               throw new Exception("插入WMS任务失败！");
 //       }

        static int GI(int? value)
        {
            return value.HasValue ? value.Value : 0;
        }
        static string GS(string value)
        {
            return value == null ? "" : value;
        }

        //public static string CreateEmptyOutTask(int station)
        //{
        //    return WMSCreateTask(6, "", station, 0);
        //}
        //public static string CreateEmptyInTask(string pallet, int station, int height)
        //{
        //    return WMSCreateTask(5, pallet, station, height);
        //}
        //public static string CreateInTask(string pallet, int station, int height)
        //{
        //    return WMSCreateTask(1, pallet, station, height);
        //}
        #endregion

        //#region UpdateTask
        //public static string UpdateWMSTask(string seqid,int status)
        //{
        //    WebService.interface2wcsServiceClient client = new WebService.interface2wcsServiceClient();
        //    WebService.updateTaskStatus uts = new WebService.updateTaskStatus();
        //    uts.SEQID = seqid;
        //    uts.NWKSTATUS = status;
        //    uts.NWKSTATUSSpecified = true;
        //    uts.NWKMESSAGE = "WCS更新成功";
        //    WebService.updateTaskStatusResponse result = client.updateTaskStatus(uts);
        //    if (result.STATUS == 0)
        //    {
        //        string sql = string.Format(" UPDATE Wcs_Task SET NwkStatus = '{0}',DStartDate = GETDATE() WHERE seqid = '{1}' AND NwkStatus < 98 ",status, seqid);
            
        //        ExecSQL(sql);
        //        return "";
        //    }
        //    else
        //        return result.STATUS + result.MESSAGE;
        //}
        //#endregion

        //#region UpdateScStatus
        //public static string UpdateScStatus(string ddjNo, int status)
        //{
        //    WebService.interface2wcsServiceClient client = new WebService.interface2wcsServiceClient();
        //    WebService.updateScStatus uss = new WebService.updateScStatus();
        //    uss.SEQID = ddjNo;
        //    uss.NWKSTATUS = status;
        //    uss.NWKSTATUSSpecified = true;
        //    uss.NWKMESSAGE = "WCS更新成功";
        //    WebService.updateScStatusResponse result = client.updateScStatus(uss);
        //    if (result.STATUS == 0)
        //        return "";
        //    else
        //        return result.STATUS + result.MESSAGE;
        //}
        //#endregion

        #endregion
    }
}
