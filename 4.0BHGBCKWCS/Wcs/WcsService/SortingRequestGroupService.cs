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
    public class SortingRequestGroupService : ServiceBase<SortingRequestGroupService>, ISortingRequestGroupContract
    {
        private SortingRequestGroupService() { }
        public bool UpdatedGroupIsCreatedSolution(List<long> groupIds)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                if (groupIds == null || groupIds.Count == 0) return true;
                SortingRequestGroup s;
                string updatedSql = string.Format("UPDATE [dbo].[{0}] SET [{1}]=1 WHERE [{2}] IN ({3})",
                    nameof(SortingRequestGroup), nameof(s.IsCreatedSolution), nameof(s.GroupId),
                    string.Join(",", groupIds));
                con.Execute(updatedSql);
                return true;
            }
        }
    }
}
