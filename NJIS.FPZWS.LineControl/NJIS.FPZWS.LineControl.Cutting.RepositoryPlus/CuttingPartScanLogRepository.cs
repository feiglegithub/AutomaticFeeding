using System.Data;
using System.Data.SqlClient;
using NJIS.Dapper.Repositories;
using NJIS.Dapper.Repositories.SqlGenerator;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;

namespace NJIS.FPZWS.LineControl.Cutting.RepositoryPlus
{
    public class CuttingPartScanLogRepository : DapperRepository<CuttingPartScanLog>
    {
        //private const string ConnectionString = @"Data Source=10.30.18.231;User Id=sa;Password=!Q@W#E$R5t6y7u8i;Initial Catalog=NJIS.FPZWS.LineControl.Cutting;";

        public CuttingPartScanLogRepository() : this(DbSettings.Current.PlusConnectionString) { }
        public CuttingPartScanLogRepository(string connectString) : base(new SqlConnection(connectString)) { }
        //public CuttingPartScanLogRepository(IDbConnection connection, ESqlConnector sqlConnector) : base(connection, sqlConnector) { }
        public CuttingPartScanLogRepository(IDbConnection connection, ISqlGenerator<CuttingPartScanLog> sqlGenerator) : base(connection, sqlGenerator) { }
        public CuttingPartScanLogRepository(IDbConnection connection, SqlGeneratorConfig config) : base(connection, config) { }
    }
}
