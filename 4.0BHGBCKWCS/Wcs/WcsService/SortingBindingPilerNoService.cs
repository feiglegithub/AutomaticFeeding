using Contract;
using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using WcsModel;

namespace WcsService
{
    public class SortingBindingPilerNoService : ServiceBase<SortingBindingPilerNoService> ,ISortingBindingPilerNoContract
    {
        private SortingBindingPilerNoService() { }
        public bool BulkInsertSortingBindingPilerNo(List<SortingBindingPilerNo> sortingBindingPilerNoes)
        {
            if (sortingBindingPilerNoes == null || sortingBindingPilerNoes.Count == 0)
                return true;
            using (var con = new SqlConnection(ConnectionString))
            {
                SortingBindingPilerNo s;
                string insertSql =
                    string.Format("INSERT INTO [dbo].[{0}] ([{1}],[{2}],[{3}],[{4}]) VALUES(@{1},@{2},@{3},@{4})",
                        nameof(SortingBindingPilerNo), nameof(s.ProductCode), nameof(s.GroupId), nameof(s.PilerNo),
                        nameof(s.SortingTaskId));
                return con.Execute(insertSql, sortingBindingPilerNoes)>0;
            }
        }

        public List<SortingBindingPilerNo> GetBindingPilerNoes(long groupId)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                SortingBindingPilerNo s;
                string selectSql = string.Format("SELECT * FROM [dbo].[{0}] WHERE  [{1}]={2}",
                    nameof(SortingBindingPilerNo), nameof(s.GroupId), groupId);
                return con.Query<SortingBindingPilerNo>(selectSql).ToList();
            }
        }
    }
}
