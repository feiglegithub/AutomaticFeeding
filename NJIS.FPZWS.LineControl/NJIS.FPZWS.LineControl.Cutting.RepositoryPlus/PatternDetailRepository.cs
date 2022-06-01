using System.Data;
using System.Data.SqlClient;
using NJIS.Dapper.Repositories;
using NJIS.Dapper.Repositories.SqlGenerator;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;

namespace NJIS.FPZWS.LineControl.Cutting.RepositoryPlus
{
    public class PatternDetailRepository : DapperRepository<PatternDetail>
    {
        private const string ConnectionString = @"Data Source=.;User Id=sa;Password=!Q@W#E$R5t6y7u8i;Initial Catalog=NJIS.FPZWS.LineControl.Cutting;";

        public PatternDetailRepository() : this(DbSettings.Current.PlusConnectionString)
        {
        }
        public PatternDetailRepository(string connectString) : base(new SqlConnection(connectString)) { }
        //public AllTaskRepository(IDbConnection connection, ESqlConnector sqlConnector) : base(connection, sqlConnector) { }
        public PatternDetailRepository(IDbConnection connection, ISqlGenerator<PatternDetail> sqlGenerator) : base(connection, sqlGenerator) { }
        public PatternDetailRepository(IDbConnection connection, SqlGeneratorConfig config) : base(connection, config) { }
    }
}
