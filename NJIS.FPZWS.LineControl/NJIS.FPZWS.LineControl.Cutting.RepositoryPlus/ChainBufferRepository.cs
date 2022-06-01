using System.Data;
using System.Data.SqlClient;
using NJIS.Dapper.Repositories;
using NJIS.Dapper.Repositories.SqlGenerator;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;

namespace NJIS.FPZWS.LineControl.Cutting.RepositoryPlus
{
    public class ChainBufferRepository : DapperRepository<ChainBuffer>
    {
        //private const string ConnectionString = @"Data Source=10.30.18.231;User Id=sa;Password=!Q@W#E$R5t6y7u8i;Initial Catalog=NJIS.FPZWS.LineControl.Cutting;";
        public ChainBufferRepository() : this(DbSettings.Current.ConnectionString) { }
        public ChainBufferRepository(string connectString) : base(new SqlConnection(connectString)) { }
        //public ChainBufferRepository(IDbConnection connection, ESqlConnector sqlConnector) : base(connection, sqlConnector) { }
        public ChainBufferRepository(IDbConnection connection, ISqlGenerator<ChainBuffer> sqlGenerator) : base(connection, sqlGenerator) { }
        public ChainBufferRepository(IDbConnection connection, SqlGeneratorConfig config) : base(connection, config) { }
    }
}
