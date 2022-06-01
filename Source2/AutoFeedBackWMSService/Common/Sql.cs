using Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace AutoFeedBackWMSService.Common
{
    public class Sql
    {
        public static string sqlConnect = "Password=!Q@W#E$R5t6y7u8i;Persist Security Info=True;User ID=sa;Initial Catalog=HGWCSB;Data Source=.";
        
        public static List<TASKINFO> GetTaskList(string SEQID, out RMsg rm)
        {
            try
            {
                if (!string.IsNullOrEmpty(SEQID))
                {
                    string sql = string.Format("select top 1 * from Wcs_Task where SEQID='{0}';", SEQID);
                    DataSet ds = GetSQL(sql);
                    List<TASKINFO> list_TaskInfo = Sql.DataToList(ds);
                    if (list_TaskInfo != null && list_TaskInfo.Count > 0)
                    {
                        rm = new RMsg() { isOk = true, errorMsg = "成功！" };
                        return list_TaskInfo;
                    }
                    else
                    {
                        rm = new RMsg() { isOk = false, errorMsg = "未获取到任务信息！" };
                        return null;
                    }
                }
                else
                {
                    rm = new RMsg() { isOk = false, errorMsg = "传入的参数不能为空！" };
                    return null;
                }
            }
            catch (Exception e)
            {
                rm = new RMsg() { isOk = false, errorMsg = e.Message };
                return null;
            }
        }

        public static string GetLEDContent(int InStation)
        {
            var sql1 = $@"select top 1 * from
                    (
                    select Msg,'LogDate'=StartDate from [dbo].[Wcs_ErrorLog] where Msg like '{InStation}入口%' and DATEDIFF(second,StartDate,getdate())<100
                    union all
                    select Msg,LogDate from [dbo].Wcs_Log where Msg like '{InStation}入口%' and DATEDIFF(second,LogDate,getdate())<100
                    ) as t
                    order by LogDate desc";

            var dt = GetSQL(sql1).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["Msg"].ToString();
            }
            else
            {
                return "";
            }
        }

        public static string GetLED(string LID, out RMsg rm)
        {
            try
            {
                if (!string.IsNullOrEmpty(LID))
                {
                    string sql = string.Format(@" select top 1 * from
                         (
                         select Msg,[Date] from [dbo].[Wcs_Log_Opt]
                         union all 
                         select Msg=ErrorMsg,[Date]=ErrorDate from [dbo].[Wcs_Log_Error]
                         ) as T 
                         where T.Msg like '%{0}%' and T.Msg like '%入口%' 
                         and DATEDIFF(SECOND,T.[Date],GETDATE())<=10
                         order by T.Date desc", LID);

                    DataSet ds = GetSQL(sql);
                    string msg = ds.Tables[0].Rows[0]["Msg"].ToString();
                    rm = new RMsg() { isOk = true, errorMsg = "成功！" };
                    return msg;
                }
                else
                {
                    rm = new RMsg() { isOk = false, errorMsg = "传入的参数不能为空！" };
                    return "";
                }
            }
            catch (Exception e)
            {
                rm = new RMsg() { isOk = false, errorMsg = e.Message };
                return "";
            }
        }

        static List<TASKINFO> DataToList(DataSet ds)
        {
            if (ds == null || ds.Tables[0].Rows.Count == 0)
                return null;
            List<TASKINFO> list_TaskInfo = new List<TASKINFO>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TASKINFO task = new TASKINFO();
                task.SEQID = ToString(dr["SeqID"]);
                task.NORDID = ToInt(dr["NordID"]);
                task.NWORKID = ToInt(dr["NWORKID"]);
                task.NPALLETID = ToString(dr["NPalletID"]);
                task.FROM_LOCATOR_CODE = ToString(dr["CPosidFrom"]);
                task.TO_LOCATOR_CODE = ToString(dr["CPosidTo"]);
                task.NOPTTYPE = ToString(dr["NOptType"]);
                task.NOPTSTATION = ToInt(dr["NOptStation"]);
                task.NPACKORDER = ToInt(dr["NpackOrder"]);
                task.NPRI = ToInt(dr["NPri"]);
                task.NLOTID = ToInt(dr["NLotId"]);
                task.CPACKTYPE = ToString(dr["CPACKTYPE"]);
                task.NWKSTATUS = ToInt(dr["NWkStatus"]);
                task.CFINISHMODE = ToString(dr["CFinishMode"]);
                task.CUSERID = ToString(dr["CUser"]);
                task.QUANTITY = ToInt(dr["Quantitity"]);
                task.HEIGHT = ToInt(dr["Height"]);
                task.LEDMSG = ToString(dr["LedMsg"]);

                list_TaskInfo.Add(task);
            }

            return list_TaskInfo;
        }

        static int ToInt(object o)
        {
            try
            {
                return Convert.ToInt32(o);
            }
            catch (Exception e)
            {
                //LogWrite.WriteLog("Sql\r\n ToInt \r\n " + e.Message);
                return -1;
            }
        }

        static string ToString(object o)
        {
            try
            {
                return o.ToString();
            }
            catch (Exception e)
            {
                //LogWrite.WriteLog("Sql\r\n ToString \r\n " + e.Message);
                return "";
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

        public static string GetTaskListSql(List<TASKINFO> list_TaskInfo)
        {
            try
            {
                if (list_TaskInfo == null || list_TaskInfo.Count == 0)
                    return string.Empty;

                string sql = "";
                foreach (var i in list_TaskInfo)
                {
                    bool isInware = IsInWare(i.NORDID);
                    int roadway = GetRoadway(isInware, i.FROM_LOCATOR_CODE, i.TO_LOCATOR_CODE);

                    sql += string.Format(@"  INSERT INTO Wcs_Task 
(SeqID,NordID,NWorkID,NPalletID,CPosidFrom,
CPosidTo,NoptType,NoptStation,Npri,NlotID,
CPackType,NwkStatus,CFinishMode,Cuser,Quantitity,
Height,LedMsg,DoptDate,NpackOrder,Roadway,CustomerName)
VALUES ('{0}','{1}','{2}','{3}','{4}',
        '{5}','{6}','{7}','{8}','{9}',
        '{10}','{11}','{12}','{13}','{14}',
        '{15}','{16}',{17},'{18}','{19}','{20}');",
                        i.SEQID, i.NORDID, i.NWORKID, i.NPALLETID, isInware ? "" : i.FROM_LOCATOR_CODE,
                        isInware ? i.TO_LOCATOR_CODE : "", i.NOPTTYPE, i.NOPTSTATION, i.NPRI, i.NLOTID,
                        i.CPACKTYPE, i.NWKSTATUS, i.CFINISHMODE, i.CUSERID, i.QUANTITY,
                        i.HEIGHT, i.LEDMSG, "GETDATE()", i.NPACKORDER, roadway, i.CUSTOMER_NAME);
                }

                return sql;
            }
            catch (Exception e)
            {
                LogWrite.WriteLog("Sql\r\n GetTaskListSql \r\n " + e.Message);
            }

            return string.Empty;
        }

        public static int ExecSQL(string sql)
        {
            if (string.IsNullOrEmpty(sql))
                return 0;
            SqlConnection myConnection = new SqlConnection(sqlConnect);
            myConnection.Open();
            SqlCommand tempSqlCommand = new SqlCommand(sql, myConnection);
            int intNumber = tempSqlCommand.ExecuteNonQuery();//返回数据库中影响的行数
            myConnection.Close();
            myConnection.Dispose();
            return intNumber;
        }

        public static DataSet GetSQL(string sql)
        {
            DataSet ds = new DataSet();
            SqlConnection myConnection = new SqlConnection(sqlConnect);
            myConnection.Open();

            SqlCommand dbCmd = new SqlCommand(sql, myConnection);
            SqlDataAdapter dbAdapt = new SqlDataAdapter(sql, myConnection);
            dbAdapt.SelectCommand = dbCmd;
            dbAdapt.Fill(ds, "ds");
            myConnection.Close();
            myConnection.Dispose();
            return ds;
        }

    }
}
