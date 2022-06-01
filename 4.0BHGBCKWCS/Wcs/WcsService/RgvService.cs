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
    public class RgvService:ServiceBase<RgvService>,IRgvContract
    {
        private RgvService() { }

        public int BulkInsertRgvTasks(List<RGV_Task> rgvTasks)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                if (rgvTasks == null || rgvTasks.Count == 0) return 0;
                var rgvTask = rgvTasks[0];
                string insertSql = string.Format(
                    "INSERT INTO [dbo].[{0}] ([{1}],[{2}],[{3}],[{4}],[{5}]) VALUES (@{1},@{2},@{3},@{4},@{5})",
                    nameof(RGV_Task), nameof(rgvTask.FromPosition), nameof(rgvTask.ToPosition), nameof(rgvTask.Status),
                    nameof(rgvTask.PilerNo), nameof(rgvTask.TaskType));

                return con.Execute(insertSql, rgvTasks);
            }
        }

        public List<RGV_Task> GetUnBeingRgvTasks()
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                RGV_Task rt;
                string sql = $"select * from [dbo].[{nameof(RGV_Task)}] where  [{nameof(rt.Status)}]=1 order by [{nameof(rt.CreateTime)}] ASC";
                return con.Query<RGV_Task>(sql).ToList();
            }
        }

        public List<RGV_Task> GetRgvTasksByTaskId(int taskId)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                RGV_Task rt;
                string sql = $"select * from [dbo].[{nameof(RGV_Task)}] where  [{nameof(rt.Status)}]<=20  and  [{nameof(rt.RTaskId)}]={taskId}";
                return con.Query<RGV_Task>(sql).ToList();
            }
        }

        public int InsertRgvTask(RGV_Task rgvTask)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                
                string insertSql = string.Format(
                    "INSERT INTO [dbo].[{0}] ([{1}],[{2}],[{3}],[{4}],[{5}]) VALUES (@{1},@{2},@{3},@{4},@{5})",
                    nameof(RGV_Task), nameof(rgvTask.FromPosition), nameof(rgvTask.ToPosition), nameof(rgvTask.Status),
                    nameof(rgvTask.PilerNo), nameof(rgvTask.TaskType));

                return con.Execute(insertSql, rgvTask);
            }
        }

        public bool UpdateRgvTask(RGV_Task rgvTask)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                if (rgvTask == null) return true;
                string updateSql = string.Format("UPDATE [dbo].[{0}] SET [{1}]=@{1},[{2}]=@{2},[{3}]=@{3} WHERE [{4}]=@{4}",
                    nameof(RGV_Task), nameof(rgvTask.FinishTime), nameof(rgvTask.StartTime), nameof(rgvTask.Status),
                    nameof(rgvTask.RTaskId));
                return con.Execute(updateSql, rgvTask) > 0;
            }
        }

        public bool UpdateRgvTasks(List<RGV_Task> rgvTasks)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                if (rgvTasks == null || rgvTasks.Count==0) return true;
                RGV_Task rgvTask;
                string updateSql = string.Format("UPDATE [dbo].[{0}] SET [{1}]=@{1},[{2}]=@{2},[{3}]=@{3} WHERE [{4}]=@{4}",
                    nameof(RGV_Task), nameof(rgvTask.FinishTime), nameof(rgvTask.StartTime), nameof(rgvTask.Status),
                    nameof(rgvTask.RTaskId));
                return con.Execute(updateSql, rgvTasks) > 0;
            }
        }
    }
}
