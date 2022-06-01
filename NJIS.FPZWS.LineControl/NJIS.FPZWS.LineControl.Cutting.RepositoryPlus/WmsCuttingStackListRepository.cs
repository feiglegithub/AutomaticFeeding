using System.Data;
using System.Data.SqlClient;
using NJIS.Dapper.Repositories;
using NJIS.Dapper.Repositories.SqlGenerator;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;

namespace NJIS.FPZWS.LineControl.Cutting.RepositoryPlus
{
    public class WmsCuttingStackListRepository : DapperRepository<WMSCuttingStackList>
    {
        //private const string ConnectionString = @"Data Source=10.30.18.231;User Id=sa;Password=!Q@W#E$R5t6y7u8i;Initial Catalog=NJIS.FPZWS.LineControl.Cutting;";
        private const string ConnectionString = @"Data Source=.;User Id=sa;Password=!Q@W#E$R5t6y7u8i;Initial Catalog=NJIS.FPZWS.LineControl.Cutting;";
        public WmsCuttingStackListRepository() : this(DbSettings.Current.ConnectionString) { }
        public WmsCuttingStackListRepository(string connectString) : base(new SqlConnection(connectString)) { }
        //public WMSCuttingStackListRepository(IDbConnection connection, ESqlConnector sqlConnector) : base(connection, sqlConnector) { }
        public WmsCuttingStackListRepository(IDbConnection connection, ISqlGenerator<WMSCuttingStackList> sqlGenerator) : base(connection, sqlGenerator) { }
        public WmsCuttingStackListRepository(IDbConnection connection, SqlGeneratorConfig config) : base(connection, config) { }
    }
}
