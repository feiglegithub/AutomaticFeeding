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
    public class VRequestMaterialService : ServiceBase<VRequestMaterialService>, IVRequestMaterialContract
    {
        private VRequestMaterialService() { }
        public List<V_RequestMaterial> GetRequestMaterials()
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                return con.Query<V_RequestMaterial>($"SELECT * FROM [dbo].[{nameof(V_RequestMaterial)}]").ToList();
            }
        }

        public bool InsertGroupRequestId(long groupId, long reqId)
        {

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Execute("InsertGroupRequestId", new { GroupId = groupId, ReqId = reqId }, null, null,
                    CommandType.StoredProcedure);
                return true;
            }

           
        }
    }
}
