using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCS;

namespace WCSToWMSDemo
{
    public class DemoSql
    {
        public static DataTable GetTaskDemo(string where)
        {
            var sql1 = $@"select SeqID,NordIDCN=case NordID when 1 then '整盘入库' when 3 then '整盘出库' when 5 then '空盘入库' when 6 then '空盘出库' else '未知的类型' end,
                    NwkStatusCN=case  NwkStatus when 1 then '已分配' when 10 then '执行中' when 12 then '已过汇流口' when 99 then '已完成' when 999 then '已取消' else '未知的状态' end,
                    NPalletID,CPosidFrom,CPosidTo,Roadway,NoptStation,NlotID,NpackOrder,Height,CustomerName,DoptDate,DStartDate,DFinishDate 
                    from[dbo].[Wcs_Task]
                where 1 = 1 {where}  order by SeqID desc";

            return Sql.GetSQL(sql1).Tables[0];
        }

        public static DataTable GetDdjStatus()
        {
            var sql1 = "Select * from [dbo].[Wcs_DdjStatue]";

            return Sql.GetSQL(sql1).Tables[0];
        }



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

        #region WMS UpdateTask
        public static string UpdateWMSTask(string seqid, int status)
        {
            ServiceReference1.interface2wcsServiceClient client = new ServiceReference1.interface2wcsServiceClient();
            ServiceReference1.updateTaskStatus uts = new ServiceReference1.updateTaskStatus();
            uts.SEQID = seqid;
            uts.NWKSTATUS = status;
            uts.NWKSTATUSSpecified = true;
            uts.NWKMESSAGE = "WCS更新成功";
            ServiceReference1.updateTaskStatusResponse result = client.updateTaskStatus(uts);
            if (result.STATUS == 0)
            {
                string sql = "";
                if (status == 99)
                {
                    sql = string.Format(" UPDATE Wcs_Task SET NwkStatus = '{0}',DFinishDate = GETDATE() WHERE seqid = '{1}' AND NwkStatus < 98 ", status, seqid);
                }
                else
                {
                    sql = string.Format(" UPDATE Wcs_Task SET NwkStatus = '{0}',DStartDate = GETDATE() WHERE seqid = '{1}' AND NwkStatus < 98 ", status, seqid);
                }
                ExecSQL(sql);
                return "";
            }
            else
                return result.STATUS + result.MESSAGE;
        }
        #endregion

        #region WMS UpdateScStatus
        public static string UpdateScStatus(string ddjNo, int status, string num)
        {
            ServiceReference1.interface2wcsServiceClient client = new ServiceReference1.interface2wcsServiceClient();
            ServiceReference1.updateScStatus uss = new ServiceReference1.updateScStatus();
            uss.SEQID = ddjNo;
            uss.NWKSTATUS = status;
            uss.NWKSTATUSSpecified = true;
            uss.NWKMESSAGE = "WCS更新成功";
            ServiceReference1.updateScStatusResponse result = client.updateScStatus(uss);
            if (result.STATUS == 0)
            {
                string sql = $" update Wcs_DdjStatue set Status = {status},UpdateDate = GETDATE() where no = {num} ";
                ExecSQL(sql);
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
                ServiceReference1.interface2wcsServiceClient client = new ServiceReference1.interface2wcsServiceClient();
                ServiceReference1.applyTask aTask = new ServiceReference1.applyTask();
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

                ServiceReference1.applyTaskResponse result = client.applyTask(aTask);
                int status = result.STATUS;
                string error = result.MESSAGE;

                if (status != 0 || error.Length > 0)
                    throw new Exception(status + error);
                else
                {
                    if (result.DATA.SEQID == null) { return $"WMS返回的SEQID是NULL！请求类型：{taskType}"; }

                    string sql = string.Format("select count(1) from [dbo].[Wcs_Task] where SeqID={0}", result.DATA.SEQID);
                    if (GetSqlNum(sql) > 0)
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



        static void InsertTask(ServiceReference1.TaskInfo rt, int height)
        {
            int nordid = GI(rt.NORDID);
            bool isInware = IsInWare(nordid);
            int roadway = GetRoadway(isInware, rt.FROM_LOCATOR_CODE, rt.TO_LOCATOR_CODE);

            string sql = string.Format(@"INSERT INTO Wcs_Task
	(SeqID,
    NordID,NWorkID,NPalletID,CPosidFrom,CPosidTo,
	Roadway,NoptType,NoptStation,Npri,NlotID,
    CPackType,NwkStatus,CFinishMode,Cuser,Quantitity,
    Height,LedMsg,DoptDate,DStartDate,DFinishDate,CustomerName)
	VALUES
	('{0}','{1}','{2}','{3}','{4}',
	'{5}','{6}' ,'{7}','{8}','{9}',
	'{10}','{11}','{12}','{13}','{14}',
	'{15}','{16}','{17}',GETDATE(),null,null,'{18}')", GS(rt.SEQID),
    GI(rt.NORDID), GI(rt.NWORKID), GS(rt.NPALLETID), isInware ? "" : GS(rt.FROM_LOCATOR_CODE), isInware ? GS(rt.TO_LOCATOR_CODE) : "",
    roadway, GS(rt.NOPTTYPE), GI(rt.NOPTSTATION), GI(rt.NPRI), GI(rt.NLOTID),
    GS(rt.CPACKTYPE), GI(rt.NWKSTATUS), GS(rt.CFINISHMODE), GS(rt.CUSERID), GI(rt.QUANTITY),
    height, GS(rt.LEDMSG), GS(rt.CUSTOMER_NAME));
            int count = ExecSQL(sql);
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
    }
}
