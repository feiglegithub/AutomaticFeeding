using Contract;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using WcsModel;

namespace WcsService
{
    public class OffBoardRequestService : ServiceBase<OffBoardRequestService>, IOffBoardRequestContract
    {
        private OffBoardRequestService() { }
        public List<OffBoardRequest> GetOffBoardRequests()
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                string store = "GetOffBoardRequest";
                return QueryList<OffBoardRequest>(con,store);
            }
        }

        public bool UpdatedOffBoardRequest(OffBoardRequest offBoardRequest)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                string store = "FinishedOffBoardRequest";
                var param = new { Id = offBoardRequest.Id,offBoardRequest.ReqId, Result = -1, Msg = "" };
                var para = new DynamicParameters();
                para.Add("@" + nameof(param.Id), param.Id);
                para.Add("@" + nameof(param.ReqId), param.ReqId);
                para.Add("@" + nameof(param.Result), param.Result, DbType.Int32,
                    ParameterDirection.Output);
                para.Add("@" + nameof(param.Msg), param.Msg, DbType.String, ParameterDirection.Output);
                con.Execute(store, para, null, null,
                    CommandType.StoredProcedure);

                var result = para.Get<int>(nameof(param.Result)) == 0;
                var msg = para.Get<string>(nameof(param.Msg));
                return result;
                
            }
        }
    }
}
