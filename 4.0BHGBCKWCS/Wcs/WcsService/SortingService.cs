using Contract;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using WcsModel;

namespace WcsService
{
    public class SortingService : ServiceBase<SortingService>, ISortingContract
    {

        private SortingService():base() { }

        //public List<SortInfo> GetSortInfos()
        //{
        //    string store = "GetSortingTaskDetail";

        //    return con.Query<SortInfo>(store, null, null, true, null, CommandType.StoredProcedure).ToList();
        //}

        //public List<PilerSortingStatus> GetPilerSortingStatuses()
        //{
        //    string store = "GetCurSortingStatus";

        //    return con.Query<PilerSortingStatus>(store, null, null, true, null, CommandType.StoredProcedure).ToList();
        //}

        public List<SortingStationInfo> GetSortingStationInfos()
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                string store = "GetSortingStationInfo";
                return con.Query<SortingStationInfo>(store, null, null, true, null, CommandType.StoredProcedure).ToList();
            }
        }

        public List<BatchSortingList> GetUnFinishedBatchSortingLists()
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                string store = "GetUnBeginBatchSortingLists";
                return con.Query<BatchSortingList>(store, null, null, true, null, CommandType.StoredProcedure).ToList();
            }
        }

        public bool SortTaskBegin(long sortTaskId, int stackIndex)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                string store = "SortTaskIndexBegin";
                con.Execute(store, new {TaskId = sortTaskId, StackIndex = stackIndex}, null, 7200,
                    CommandType.StoredProcedure);
                return true;
            }
        }

        public bool SortTaskFinished(long sortTaskId, int stackIndex)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                string store = "SortTaskIndexFinished";
                con.Execute(store, new { TaskId = sortTaskId, StackIndex = stackIndex }, null, 7200,
                    CommandType.StoredProcedure);
                return true;
            }
        }

    }
}
