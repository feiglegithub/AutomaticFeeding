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
    public class SortingDetailService : ServiceBase<SortingDetailService>, ISortingDetailContract
    {
        private SortingDetailService(){}
        public bool BulkInsertSortingDetail(List<SortingDetail> sortingDetails)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                if (sortingDetails == null || sortingDetails.Count == 0) return true;
                SortingDetail s;
                string insertSql = string.Format(
                    "INSERT INTO [dbo].[{0}] ([{1}],[{2}],[{3}],[{4}],[{5}],[{6}],[{7}],[{8}],[{9}],[{10}]) VALUES(@{1},@{2},@{3},@{4},@{5},@{6},@{7},@{8},@{9},@{10})",
                    nameof(SortingDetail), nameof(s.IsReverseSorting), nameof(s.StartPilerNo),
                    nameof(s.SortingTaskId), nameof(s.ProductCode), nameof(s.OperationCount),
                    nameof(s.ParallelSortingTaskId), nameof(s.MainPilerIsFinished), nameof(s.ParallelPilerIsFinished),nameof(s.BookCount),nameof(s.GroupId));
                return con.Execute(insertSql, sortingDetails) > 0;
            }

            //new SortingDetail()
            //{
            //    IsReverseSorting = true,
            //    MainPilerNo = 0,
            //    ParallelPilerNo = 0,
            //    StartPilerNo = unBegin.PilerNo,
            //    SortingTaskId = 0,
            //    ProductCode = unBegin.ProductCode,
            //    OperationCount = unBegin.OperationCount,
            //    ParallelSortingTaskId = unBegin.SortingTaskId,
            //    MainPilerIsFinished = false,
            //    ParallelPilerIsFinished = true
            //});
        }
    }
}
