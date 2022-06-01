using System.Data;
using System.Data.SqlClient;
using NJIS.Dapper.Repositories;
using NJIS.Dapper.Repositories.SqlGenerator;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;

namespace NJIS.FPZWS.LineControl.Cutting.RepositoryPlus
{
    public class StackDetailRepository : DapperRepository<StackDetail>
    {
        private const string ConnectionString = @"Data Source=.;User Id=sa;Password=!Q@W#E$R5t6y7u8i;Initial Catalog=NJIS.FPZWS.LineControl.Cutting;";

        public StackDetailRepository() : this(DbSettings.Current.PlusConnectionString)
        {
        }
        public StackDetailRepository(string connectString) : base(new SqlConnection(connectString)) { }
        //public AllTaskRepository(IDbConnection connection, ESqlConnector sqlConnector) : base(connection, sqlConnector) { }
        public StackDetailRepository(IDbConnection connection, ISqlGenerator<StackDetail> sqlGenerator) : base(connection, sqlGenerator) { }
        public StackDetailRepository(IDbConnection connection, SqlGeneratorConfig config) : base(connection, config) { }
    }
}
