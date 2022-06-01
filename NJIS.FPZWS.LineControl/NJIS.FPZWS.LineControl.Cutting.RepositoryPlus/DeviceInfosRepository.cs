using System.Data;
using System.Data.SqlClient;
using NJIS.Dapper.Repositories;
using NJIS.Dapper.Repositories.SqlGenerator;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;

namespace NJIS.FPZWS.LineControl.Cutting.RepositoryPlus
{
    public partial class DeviceInfosRepository : DapperRepository<DeviceInfos>
    {
        //private const string ConnectionString = @"Data Source=10.30.18.231;User Id=sa;Password=!Q@W#E$R5t6y7u8i;Initial Catalog=NJIS.FPZWS.LineControl.Cutting;";
        private const string ConnectionString = @"Data Source=.;User Id=sa;Password=!Q@W#E$R5t6y7u8i;Initial Catalog=NJIS.FPZWS.LineControl.Cutting;";
        public DeviceInfosRepository() : this(DbSettings.PlusConnectionString2) { }
        public DeviceInfosRepository(string connectString) : base(new SqlConnection(connectString)) { }
        //public DeviceInfosRepository(IDbConnection connection, ESqlConnector sqlConnector) : base(connection, sqlConnector) { }
        public DeviceInfosRepository(IDbConnection connection, ISqlGenerator<DeviceInfos> sqlGenerator) : base(connection, sqlGenerator) { }
        public DeviceInfosRepository(IDbConnection connection, SqlGeneratorConfig config) : base(connection, config) { }
    }
   
}
