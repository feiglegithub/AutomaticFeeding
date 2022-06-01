using NJIS.Dapper.Repositories;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;
using System.Data;
using System.Data.SqlClient;
using NJIS.Dapper.Repositories.SqlGenerator;

namespace NJIS.FPZWS.LineControl.Cutting.RepositoryPlus
{
    public class CuttingStackProductionListRepository : DapperRepository<CuttingStackProductionList>
    {
        //private const string ConnectionString = @"Data Source=10.30.18.231;User Id=sa;Password=!Q@W#E$R5t6y7u8i;Initial Catalog=NJIS.FPZWS.LineControl.Cutting;";
        private const string ConnectionString = @"Data Source=.;User Id=sa;Password=!Q@W#E$R5t6y7u8i;Initial Catalog=NJIS.FPZWS.LineControl.Cutting;";
        public CuttingStackProductionListRepository() : this(DbSettings.Current.ConnectionString) { }
        public CuttingStackProductionListRepository(string connectString) : base(new SqlConnection(connectString)) { }
        //public CuttingStackProductionListRepository(IDbConnection connection, ESqlConnector sqlConnector) : base(connection, sqlConnector) { }
        public CuttingStackProductionListRepository(IDbConnection connection, ISqlGenerator<CuttingStackProductionList> sqlGenerator) : base(connection, sqlGenerator) { }
        public CuttingStackProductionListRepository(IDbConnection connection, SqlGeneratorConfig config) : base(connection, config) { }
    }
}
