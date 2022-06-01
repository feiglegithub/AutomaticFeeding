using System.Data;
using System.Data.SqlClient;
using NJIS.Dapper.Repositories;
using NJIS.Dapper.Repositories.SqlGenerator;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;

namespace NJIS.FPZWS.LineControl.Cutting.RepositoryPlus
{
    public class StackLogRepository : DapperRepository<StackLog>
    {
        private const string ConnectionString = @"Data Source=.;User Id=sa;Password=!Q@W#E$R5t6y7u8i;Initial Catalog=NJIS.FPZWS.LineControl.Cutting;";

        public StackLogRepository() : this(DbSettings.Current.PlusConnectionString)
        {
        }
        public StackLogRepository(string connectString) : base(new SqlConnection(connectString)) { }
        //public AllTaskRepository(IDbConnection connection, ESqlConnector sqlConnector) : base(connection, sqlConnector) { }
        public StackLogRepository(IDbConnection connection, ISqlGenerator<StackLog> sqlGenerator) : base(connection, sqlGenerator) { }
        public StackLogRepository(IDbConnection connection, SqlGeneratorConfig config) : base(connection, config) { }
    }
    
}
