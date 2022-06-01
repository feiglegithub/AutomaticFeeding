using System.Data;
using System.Data.SqlClient;
using NJIS.Dapper.Repositories;
using NJIS.Dapper.Repositories.SqlGenerator;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;

namespace NJIS.FPZWS.LineControl.Cutting.RepositoryPlus
{
    public class PatternRepository : DapperRepository<Pattern>
    {
        private const string ConnectionString = @"Data Source=.;User Id=sa;Password=!Q@W#E$R5t6y7u8i;Initial Catalog=NJIS.FPZWS.LineControl.Cutting;";

        public PatternRepository() : this(DbSettings.Current.PlusConnectionString)
        {
        }
        public PatternRepository(string connectString) : base(new SqlConnection(connectString)) { }
        //public AllTaskRepository(IDbConnection connection, ESqlConnector sqlConnector) : base(connection, sqlConnector) { }
        public PatternRepository(IDbConnection connection, ISqlGenerator<Pattern> sqlGenerator) : base(connection, sqlGenerator) { }
        public PatternRepository(IDbConnection connection, SqlGeneratorConfig config) : base(connection, config) { }
    }
}
