using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contract;
using Dapper;
using WcsModel;

namespace WcsService
{
    public class WmsService : ServiceBase<WmsService>, IWmsTaskContract
    {
        private WmsService() { }
        public List<WMS_Task> GetBoardInStockTask()
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                string sql = "select * from [HuangGangBWMS].[dbo].[WMS_Task] where TaskType=1 and ReqId is null and TaskStatus=1 and FromPosition=105 order by TaskId";
                return con.Query<WMS_Task>(sql).ToList();
            }
        }

        public List<WMS_Task> GetTaskByFromStationNo(int fromStationNo, int taskType)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                string sql = $"select * from [HuangGangBWMS].[dbo].[WMS_Task] where TaskType={taskType} AND TaskStatus=1 and FromPosition='{fromStationNo}' order by TaskId";
                return con.Query<WMS_Task>(sql).ToList();
            }
        }

        public List<WMS_Task> GetWmsTasksByStackNo(int pilerNo, int taskType = 0)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                string sql = $"select * from [HuangGangBWMS].[dbo].[WMS_Task] where (0={taskType} OR TaskType={taskType}) and TaskStatus<98 and PilerNo={pilerNo}";
                return con.Query<WMS_Task>(sql).ToList();
            }
        }

        public bool BulkUpdateWmsTasks(List<WMS_Task> wmsTasks)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                if (wmsTasks == null || wmsTasks.Count == 0) return true;
                WMS_Task wms;
                string updateSql = string.Format("UPDATE [HuangGangBWMS].[dbo].[WMS_Task] SET [{0}]=@{0},[{1}]=@{1},[{2}]=@{2},[{3}]=@{3},[{4}]=@{4},[{5}]=@{5} WHERE [{6}]=@{6}",nameof(wms.DdjNo),nameof(wms.DdjTime),nameof(wms.ErrorMsg),nameof(wms.FinishTime),nameof(wms.StartTime),nameof(wms.TaskStatus),nameof(wms.TaskId));
                return con.Execute(updateSql, wmsTasks) > 0;
            }
        }

        public bool UpdateWmsTask(WMS_Task wmsTask)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                if (wmsTask == null) return true;
                WMS_Task wms;
                string updateSql = string.Format("UPDATE [HuangGangBWMS].[dbo].[WMS_Task] SET [{0}]=@{0},[{1}]=@{1},[{2}]=@{2},[{3}]=@{3},[{4}]=@{4},[{5}]=@{5} WHERE [{6}]=@{6}", nameof(wms.DdjNo), nameof(wms.DdjTime), nameof(wms.ErrorMsg), nameof(wms.FinishTime), nameof(wms.StartTime), nameof(wms.TaskStatus), nameof(wms.TaskId));
                return con.Execute(updateSql, wmsTask) > 0;
            }
        }

        public List<WMS_Task> GetWmsTasksByReqId(long reqId)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                string sql = $"select * from [HuangGangBWMS].[dbo].[WMS_Task] where [ReqId]={reqId}";
                return con.Query<WMS_Task>(sql).ToList();
            }
        }

        public List<WMS_Task> GetWmsTasksByDdjNo(int ddjNo, int taskType = 0)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                WMS_Task w;
                string sql = $"select * from HuangGangBWMS.[dbo].[WMS_Task] where (0={taskType} OR TaskType={taskType}) and DdjNo={ddjNo} and TaskStatus=1 AND [ToPosition]<>'2003'";
                return con.Query<WMS_Task>(sql).ToList();
            }
        }

        public List<WMS_Task> GetWmsTasksByTaskId(long taskId)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                WMS_Task w;
                string sql = $"select * from HuangGangBWMS.[dbo].[WMS_Task] where TaskId={taskId}";
                return con.Query<WMS_Task>(sql).ToList();
            }
        }

        public List<WMS_Task> GetRgvWmsTasks()
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                string sql =
                    "select * from HuangGangBWMS.[dbo].[WMS_Task] where  [TaskType] IN (4,6)  and  [TaskStatus]=1 order by CreateTime ASC";
                return con.Query<WMS_Task>(sql).ToList();
            }
        }

        public List<WMS_Task> GetRgvWmsTasksByStackNo(int stackNo)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                string sql = $"select * from [HuangGangBWMS].[dbo].[WMS_Task] where TaskType IN (4,6) and TaskStatus<98 and PilerNo={stackNo}";
                return con.Query<WMS_Task>(sql).ToList();
            }
        }

        public List<WMS_Task> GetEmptyPadTasks()
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                string sql = "select * from [HuangGangBWMS].[dbo].[WMS_Task] where ToPosition='2005' and TaskType=2 and TaskStatus<98";
                return con.Query<WMS_Task>(sql).ToList();
            }
        }
    }
}
