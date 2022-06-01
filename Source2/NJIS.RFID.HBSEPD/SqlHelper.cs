using CommomLY1._0;
using System.Data;

namespace NJIS.RFID.HBSEPD
{
    public class SqlHelper
    {
        static string connstr = "Persist Security Info =true; Password=!Q@W#E$R5t6y7u8i;User ID = sa ; Initial Catalog = HGWCSB; Data Source =.";

        public static DataTable GetRFIDConfigs()
        {
            var sql1 = "select * from RFID_Config";
            return SqlBase.GetSqlDs(sql1, connstr).Tables[0];
        }

        public static void UpdateRFID(string ip,string pallet)
        {
            var sql = $"update RFID_Config set RPallet='{pallet}',RTime=GETDATE() where ReaderIp='{ip}'";
            SqlBase.ExecSql(sql, connstr);
        }
    }
}
