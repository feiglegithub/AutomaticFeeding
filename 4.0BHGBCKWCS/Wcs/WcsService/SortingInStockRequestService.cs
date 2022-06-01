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
    public class SortingInStockRequestService : ServiceBase<SortingInStockRequestService>, ISortingInStockRequestContract
    {
        private SortingInStockRequestService() { }
        public List<SortingInStockRequest> GetSortingInStockRequests()
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                
                string store = "GetSortingInStockRequests";
                return QueryList<SortingInStockRequest>(con,store);
            }
        }

        public bool UpdatedRequestStatus(SortingInStockRequest sortingInStockRequest)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                string updatedSql = string.Format("UPDATE [dbo].[{0}] SET [{1}]=@{1},[{2}]=@{2} WHERE [{3}]=@{3}",
                    nameof(SortingInStockRequest),nameof(sortingInStockRequest.ReqId), nameof(sortingInStockRequest.IsRequest),
                    nameof(sortingInStockRequest.Id));
                return con.Execute(updatedSql, sortingInStockRequest) > 0;
            }
        }
    }
}
