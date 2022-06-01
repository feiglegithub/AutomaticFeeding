using Contract;
using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using WcsModel;

namespace WcsService
{
    public class EmptyBoardRequestService : ServiceBase<EmptyBoardRequestService>, IEmptyBoardRequestContract
    {
        private EmptyBoardRequestService() { }
        public List<EmptyBoardRequest> GetEmptyBoardInStockRequests()
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                EmptyBoardRequest e;
                string sql =
                    $"SELECT * FROM [dbo].[{nameof(EmptyBoardRequest)}] WHERE [{nameof(e.IsNeed)}]=0 AND [{nameof(e.IsRequest)}]=0";
                return con.Query<EmptyBoardRequest>(sql).ToList();
            }
            
        }

        public List<EmptyBoardRequest> GetEmptyBoardOutStockRequests()
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                EmptyBoardRequest e;
                string sql =
                    $"SELECT * FROM [dbo].[{nameof(EmptyBoardRequest)}] WHERE [{nameof(e.IsNeed)}]=1 AND [{nameof(e.IsRequest)}]=0";
                return con.Query<EmptyBoardRequest>(sql).ToList();
            }
        }

        public List<EmptyBoardRequest> GetEmptyBoardRequests()
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                EmptyBoardRequest e;
                string sql =
                    $"SELECT * FROM [dbo].[{nameof(EmptyBoardRequest)}] WHERE [{nameof(e.IsRequest)}]=0";
                return con.Query<EmptyBoardRequest>(sql).ToList();
            }
        }

        public bool UpdatedEmptyBoardRequest(EmptyBoardRequest emptyBoardRequest)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                string updatedSql = string.Format("UPDATE [dbo].[{0}] SET [{1}]=@{1},[{2}]=@{2} WHERE [{3}]=@{3}",
                    nameof(EmptyBoardRequest), nameof(emptyBoardRequest.IsRequest), nameof(emptyBoardRequest.ReqId),
                    nameof(emptyBoardRequest.Id));
                return con.Execute(updatedSql, emptyBoardRequest)>0;
            }
        }
    }
}
