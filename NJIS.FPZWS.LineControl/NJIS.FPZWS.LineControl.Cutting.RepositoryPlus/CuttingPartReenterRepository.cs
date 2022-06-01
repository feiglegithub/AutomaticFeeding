using System.Data;
using System.Data.SqlClient;
using NJIS.Dapper.Repositories;
using NJIS.Dapper.Repositories.SqlGenerator;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;

namespace NJIS.FPZWS.LineControl.Cutting.RepositoryPlus
{
    public class CuttingPartReenterRepository : DapperRepository<CuttingPartReenter>
    {
        //private const string ConnectionString = @"Data Source=10.30.18.231;User Id=sa;Password=!Q@W#E$R5t6y7u8i;Initial Catalog=NJIS.FPZWS.LineControl.Cutting;";

        public CuttingPartReenterRepository() : this(DbSettings.Current.PlusConnectionString) { }
        public CuttingPartReenterRepository(string connectString) : base(new SqlConnection(connectString)) { }
        //public CuttingPartReenterRepository(IDbConnection connection, ESqlConnector sqlConnector) : base(connection, sqlConnector) { }
        public CuttingPartReenterRepository(IDbConnection connection, ISqlGenerator<CuttingPartReenter> sqlGenerator) : base(connection, sqlGenerator) { }
        public CuttingPartReenterRepository(IDbConnection connection, SqlGeneratorConfig config) : base(connection, config) { }
    }
}
