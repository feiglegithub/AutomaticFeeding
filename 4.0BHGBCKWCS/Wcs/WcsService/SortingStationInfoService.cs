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
    public class SortingStationInfoService : ServiceBase<SortingStationInfoService>, ISortingStationInfoContract
    {
        private SortingStationInfoService() { }
        public List<SortingStationInfo> GetSortingStationInfos()
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                string sql = $"SELECT * FROM [dbo].[{nameof(SortingStationInfo)}]";
                
                return con.Query<SortingStationInfo>(sql).ToList();
            }
        }

        public List<SortingStationInfo> GetSortingStationInfos(int stationNo)
        {
            using ( var con = new SqlConnection(ConnectionString))
            {
                SortingStationInfo s;
                string sql =
                    $"SELECT * FROM [dbo].[{nameof(SortingStationInfo)}] WHERE [{nameof(s.StationNo)}]={stationNo}";
                return con.Query<SortingStationInfo>(sql).ToList();
            }
        }

        public bool UpdatedSortingStationInfo(SortingStationInfo sortingStationInfo)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                SortingStationInfo s;
                string updatedSql =
                    string.Format("UPDATE [dbo].[{0}] SET [{1}]=@{1},[{2}]=@{2},[{3}]=@{3},[{4}]=@{4},[{5}]=@{5} WHERE [{6}]=@{6}",
                        nameof(SortingStationInfo), nameof(s.BookCount), nameof(s.Color), nameof(s.HasUpBoard),
                        nameof(s.IsOuting), nameof(s.PilerNo),nameof(s.StationNo));
                return con.Execute(updatedSql, sortingStationInfo)>0;
            }

        }
    }
}
