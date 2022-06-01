using System.Data;
using System.Data.SqlClient;
using NJIS.Dapper.Repositories;
using NJIS.Dapper.Repositories.SqlGenerator;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;

namespace NJIS.FPZWS.LineControl.Cutting.RepositoryPlus
{
    public class BatchGroupLogRepository : DapperRepository<BatchGroupLog>
    {
        private const string ConnectionString = @"Data Source=.;User Id=sa;Password=!Q@W#E$R5t6y7u8i;Initial Catalog=NJIS.FPZWS.LineControl.Cutting;";

        public BatchGroupLogRepository() : this(DbSettings.Current.PlusConnectionString)
        {
        }
        public BatchGroupLogRepository(string connectString) : base(new SqlConnection(connectString)) { }
        //public AllTaskRepository(IDbConnection connection, ESqlConnector sqlConnector) : base(connection, sqlConnector) { }
        public BatchGroupLogRepository(IDbConnection connection, ISqlGenerator<BatchGroupLog> sqlGenerator) : base(connection, sqlGenerator) { }
        public BatchGroupLogRepository(IDbConnection connection, SqlGeneratorConfig config) : base(connection, config) { }
    }
    
}
