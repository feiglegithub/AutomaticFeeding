using Contract;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace WcsService
{
    public class WcsLogSevice : ServiceBase<WcsLogSevice>, IWcsLogContract
    {
        private WcsLogSevice() { }
        public int InsertWcsErrorLog(string msg, int pilerNo = 0, string device = "")
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                string store = "wcs_proc_InsertLog";
                var param = new { Type = "ERROR", PilerNo = pilerNo, Msg = msg, Device=device };
                var para = new DynamicParameters();
                para.Add("@" + nameof(param.Type), param.Type);
                para.Add("@" + nameof(param.PilerNo), param.PilerNo, DbType.Int64);
                para.Add("@" + nameof(param.Msg), param.Msg, DbType.String);
                para.Add("@" + nameof(param.Device), param.Device, DbType.String);
                return con.Execute(store, para, null, null,
                    CommandType.StoredProcedure);
            }
        }

        public int InsertWcsLog(string msg, int pilerNo = 0, string device = "")
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                string store = "wcs_proc_InsertLog";
                var param = new { Type = "LOG", PilerNo = pilerNo, Msg = msg, Device = device };
                var para = new DynamicParameters();
                para.Add("@" + nameof(param.Type), param.Type);
                para.Add("@" + nameof(param.PilerNo), param.PilerNo, DbType.Int64);
                para.Add("@" + nameof(param.Msg), param.Msg, DbType.String);
                para.Add("@" + nameof(param.Device), param.Device, DbType.String);
                return con.Execute(store, para, null, null,
                    CommandType.StoredProcedure);
            }
        }
    }
}
