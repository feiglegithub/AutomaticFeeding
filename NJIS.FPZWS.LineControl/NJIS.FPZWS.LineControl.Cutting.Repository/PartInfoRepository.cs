using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJIS.Dapper.Repositories;
using NJIS.Dapper.Repositories.SqlGenerator;
using NJIS.FPZWS.LineControl.Cutting.Model;

namespace NJIS.FPZWS.LineControl.Cutting.Repository
{
    public class PartInfoRepository : DapperRepository<PartInfo>
    {
        public PartInfoRepository():this(DbSettings.Current.ConnectionString)
        {

        }
        //private const string ConnectionString = @"Data Source=10.10.14.51;User Id=sa;Password=!Q@W#E$R5t6y7u8i;Initial Catalog=MesDataCenter;";
        public PartInfoRepository(string connectString) : base(new SqlConnection(connectString)) { }
        //public PartInfoRepository(IDbConnection connection, ESqlConnector sqlConnector) : base(connection, sqlConnector) { }
        public PartInfoRepository(IDbConnection connection, ISqlGenerator<PartInfo> sqlGenerator) : base(connection, sqlGenerator) { }
        public PartInfoRepository(IDbConnection connection, SqlGeneratorConfig config) : base(connection, config) { }
    }
}
