using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contract;
using Dapper;
using WcsModel;

namespace WcsService
{
    public class SortingTaskStatusService :ServiceBase<SortingTaskStatusService>, ISortingTaskStatusContract
    {
        private SortingTaskStatusService() { }
        public List<SortingTaskStatus> GetCurTaskSortingTaskStatuses()
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                string store = "GetSortingTaskStatus";
                return con.Query<SortingTaskStatus>(store, null, null, true, null, CommandType.StoredProcedure)
                    .ToList();
            }
        }

        public List<SortingTaskStatus> GetUnCreatedSolutionSortingTaskStatuses()
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                string store = "GetUnCreatedSolutionSortingTask";
                return con.Query<SortingTaskStatus>(store, null, null, true, null, CommandType.StoredProcedure)
                    .ToList();
            }
        }
    }
}
