using NJIS.Dapper.Repositories;
using NJIS.Dapper.Repositories.SqlGenerator;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;
using System.Data;
using System.Data.SqlClient;

namespace NJIS.FPZWS.LineControl.Cutting.RepositoryPlus
{
    public class PatternFeedBackRepository : DapperRepository<PatternFeedBack>
    {
        private const string ConnectionString = @"Data Source=.;User Id=sa;Password=!Q@W#E$R5t6y7u8i;Initial Catalog=NJIS.FPZWS.LineControl.Cutting;";

        public PatternFeedBackRepository() : this(DbSettings.Current.PlusConnectionString)
        {
        }
        public PatternFeedBackRepository(string connectString) : base(new SqlConnection(connectString)) { }
        //public AllTaskRepository(IDbConnection connection, ESqlConnector sqlConnector) : base(connection, sqlConnector) { }
        public PatternFeedBackRepository(IDbConnection connection, ISqlGenerator<PatternFeedBack> sqlGenerator) : base(connection, sqlGenerator) { }
        public PatternFeedBackRepository(IDbConnection connection, SqlGeneratorConfig config) : base(connection, config) { }
    }
}
