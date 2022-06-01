using NJIS.Dapper.Repositories;
using System.Data;
using System.Data.SqlClient;
using NJIS.Dapper.Repositories.SqlGenerator;
using MySql.Data.MySqlClient;
using NJIS.FPZWS.LineControl.PartMill.Model;

namespace NJIS.FPZWS.LineControl.PartMill.Repository
{
    public class line_configRepository: DapperRepository<line_config>
    {
        private const string ConnectionString = @"Server=127.0.0.1;Database=pcs_business;Uid=sa;Pwd=!Q@W#E$R5t6y7u8i;";
        public line_configRepository() : this(ConnectionString, SqlProvider.MySQL) { }
        public line_configRepository(string connectString, SqlProvider sqlProvider) : base(new MySqlConnection(connectString), sqlProvider) { }
        //public CutPartInfoCollectorRepository(IDbConnection connection, ESqlConnector sqlConnector) : base(connection, sqlConnector) { }
        public line_configRepository(IDbConnection connection, ISqlGenerator<line_config> sqlGenerator) : base(connection, sqlGenerator) { }
        public line_configRepository(IDbConnection connection, SqlGeneratorConfig config) : base(connection, config) { }
    }
}
