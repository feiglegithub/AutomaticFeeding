using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace WCS
{
    public class MonitorSql
    {
        /// <summary>
        /// 添加地面柜信息到数据库进行监控显示
        /// </summary>
        /// <param name="hsName"></param>
        /// <param name="hsInfo"></param>
        public static void ChangeHsInfo(List<int> dmgInfoList)
        {
            string sql = "";
            for (int i = 0; i < dmgInfoList.Count; i++)
            {
                 sql += string.Format("update Wcs_HsInfo set ErrorMsg = {0} where no = {1}; ", dmgInfoList[i], i + 1);
            }

            Sql.ExecSQL(sql);
        }

        //单独更新外购件入库区的地面柜状态
        public static void UpdateHsStatus(int ErrorMsg) 
        {
            string sql = string.Format("update dbo.Wcs_HsInfo2 set ErrorMsg={0} where [No]=16", ErrorMsg);
            Sql.ExecSQL(sql);
        }

        public static List<int> GetRoadWay() 
        {
            try
            {
                string sql = @"select Roadway from [dbo].[Wcs_Task] where NwkStatus=1 and NordID=3
                    group by Roadway
                    order by count(1) desc";

                DataSet ds = Sql.GetSQL(sql);

                List<int> lst = new List<int>();
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    lst.Add(int.Parse(r["Roadway"].ToString()));
                }

                return lst;
            }
            catch 
            {
                return null;
            }
        }

        public static List<string> GetDdjErrorList()
        {
            try
            {
                List<string> ddjErrorList = new List<string>();
                string sql = "select * from Wcs_DdjErrorList order by ErrorAddress ASC";
                DataSet ds = Sql.GetSQL(sql);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ddjErrorList.Add(dr["ErrorMsg"].ToString());
                }
                return ddjErrorList;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 实时更新一楼发货口 正忙/空闲 状态信息
        /// </summary>
        /// <param name="exportList"></param>
        public static void ChangeExportState(List<bool> exportList)
        {
            StringBuilder sbSql = new StringBuilder();
            int i = 101;
            foreach (bool item in exportList)
            {
                sbSql.AppendFormat("update dbo.Wcs_StationStatue set statue={0} where port={1}; ", item ? 1 : 0, i);
                i++;
            }

            Sql.ExecSQL(sbSql.ToString());
        }

        /// <summary>
        /// 添加堆垛机信息到数据库
        /// </summary>
        /// <param name="di"></param>
        public static void ChangeScInfo(List<DdjInfo> ddjInfoList)
        {
            string sql = "";
            foreach (DdjInfo di in ddjInfoList)
            {
                sql += string.Format(@"  update Wcs_DdjInfo
	set IsAuto = '{0}',IsFree = '{1}',IsFinish = '{2}',Pallet = '{3}',TaskType = '{4}',
	CurrentLocation = '{5}',FromWare = '{6}',ToWare = '{7}',ErrorMsg = '{8}',IsActivation = '{9}',
    IsFisrtOutFree = '{10}',IsSecondInFree = '{11}',IsSecondOutFree = '{12}',ErrorMsgList = '{14}',UpdateDate = GETDATE()
	where No = '{13}';",
                     di.IsAuto, di.IsFree, di.IsFinished, di.Pallet, di.TaskType,
                     di.CurrentLocation, di.FromWare, di.ToWare, di.ErrorMsg, di.IsActivation,
                     di.IsFirstOutFree, di.IsSecondInFree, di.IsSecondOutFree, di.No, di.ErrorList);
            }

            Sql.ExecSQL(sql);
        }

        /// <summary>
        /// 获取设备块信息
        /// </summary>
        /// <returns></returns>
        public static DataSet GetDeviceInfo(string where)
        {
            string sql = string.Format("select * from [dbo].[Wcs_Device] where {0} and LEN(DeviceNo)=6", where);

            return Sql.GetSQL(sql);
        }

        //获取外购件入库设备信息
        public static DataSet GetDeviceInfo()
        {
            string sql = "select * from [dbo].[Wcs_Device] where LEN(DeviceNo)=7";

            return Sql.GetSQL(sql);
        }

        public static void ChangeDeviceOrderState(int Id)
        {
            string sql = "update [Wcs_OrderByHand] set IsRun=1 where Id = " + Id.ToString();

            Sql.ExecSQL(sql);
        }

//        /// <summary>
//        /// 如果堆垛机有故障，自动重置出口优先级
//        /// </summary>
//        public static void ReSetNoptStationWeight() 
//        {
//            string sql1 = @"select count(1) from [dbo].[Wcs_DdjStatue] s
//                        inner join [dbo].[Wcs_DdjInfo] d on s.No=d.No
//                        where s.Status=1 and d.ErrorMsg>0";
//            if (Sql.GetSqlNum(sql1) > 0) 
//            {
//                string sql2 = "update [dbo].[Wcs_StationStatue] set [power]=0";
//                Sql.ExecSQL(sql2);
//            }
//        }

        /// <summary>
        /// 获取设备手动下发的指令
        /// </summary>
        /// <returns></returns>
        public static DataSet GetDeviceOrder() 
        {
            string sql = @"select a.Id,a.DeviceNo,a.Pallet,a.[Target],b.ItemNo,b.CPU from [Wcs_OrderByHand]  a
                inner join [dbo].[Wcs_Device] b on a.DeviceNo=b.DeviceNo
                where a.IsRun=0  ";

            return Sql.GetSQL(sql);
        }

        /// <summary>
        /// 实时更新设备块信息
        /// </summary>
        /// <param name="dic"></param>
        public static void ChangeDeviceInfo(Dictionary<string, string> dic)
        {
            //key=365|3  value=0|0|TPZ00213101|105
            StringBuilder sbSql = new StringBuilder();
            string itemno, cpu, empty, PalletPresent, pallet, target;
            foreach (KeyValuePair<string, string> item in dic)
            {
                itemno = item.Key.Split('|')[0];
                cpu = item.Key.Split('|')[1];
                empty = item.Value.Split('|')[0];
                PalletPresent = item.Value.Split('|')[1];
                pallet = item.Value.Split('|')[2];
                target = item.Value.Split('|')[3];
                sbSql.AppendFormat(@" update [dbo].[Wcs_Device] set [Empty]={0},[PalletPresent]={1},[Pallet]='{2}',
                    [target]={3} where ItemNo={4} and CPU={5};", empty, PalletPresent, pallet, target, itemno, cpu);
            }

            Sql.ExecSQL(sbSql.ToString());
        }
    }
}
