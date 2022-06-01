using System.Data;
using System.Data.SqlClient;
using NJIS.Dapper.Repositories;
using NJIS.Dapper.Repositories.SqlGenerator;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;

namespace NJIS.FPZWS.LineControl.Cutting.RepositoryPlus
{
    public class PatternLogRepository : DapperRepository<PatternLog>
    {
        private const string ConnectionString = @"Data Source=.;User Id=sa;Password=!Q@W#E$R5t6y7u8i;Initial Catalog=NJIS.FPZWS.LineControl.Cutting;";

        public PatternLogRepository() : this(DbSettings.Current.PlusConnectionString)
        {
        }
        public PatternLogRepository(string connectString) : base(new SqlConnection(connectString)) { }
        //public AllTaskRepository(IDbConnection connection, ESqlConnector sqlConnector) : base(connection, sqlConnector) { }
        public PatternLogRepository(IDbConnection connection, ISqlGenerator<PatternLog> sqlGenerator) : base(connection, sqlGenerator) { }
        public PatternLogRepository(IDbConnection connection, SqlGeneratorConfig config) : base(connection, config) { }
    }
    
}
