using Contract;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using WcsModel;

namespace WcsService
{
    public class GroupLinkTaskService : ServiceBase<GroupLinkTaskService>, IGroupLinkTaskContract
    {
        private GroupLinkTaskService() { }
        public List<GroupLinkTask> GetUnCreatedSolutionGroupLinkTask()
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                string store = "GetUnCreatedSolutionGroupTaskLink";
                return con.Query<GroupLinkTask>(store, null, null, true, null, CommandType.StoredProcedure)
                    .ToList();
            }
        }

        public bool UpdatedGroupLinkTask(GroupLinkTask groupLinkTask)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                string updatedSql = string.Format("UPDATE [dbo].[{0}] SET [{1}]=@{1},[{2}]=@{2} WHERE [{3}]=@{3}",
                    nameof(GroupLinkTask), nameof(groupLinkTask.Status), nameof(groupLinkTask.ReqId),
                    nameof(groupLinkTask.Id));
                return con.Execute(updatedSql, groupLinkTask) > 0;
            }

        }

        public List<GroupLinkTask> GetGroupLinkTasksByPilerNo(int pilerNo)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                GroupLinkTask groupLink;
                string sql = $"SELECT * FROM [dbo].[{nameof(GroupLinkTask)}] WHERE [{nameof(groupLink.PilerNo)}]={pilerNo}";
                return con.Query<GroupLinkTask>(sql).ToList();
            }
        }
    }
}
