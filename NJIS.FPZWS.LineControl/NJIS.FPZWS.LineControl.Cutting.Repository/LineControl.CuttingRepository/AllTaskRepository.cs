using System.Data;
using System.Data.SqlClient;
using NJIS.Dapper.Repositories;
using NJIS.Dapper.Repositories.SqlGenerator;
using NJIS.FPZWS.LineControl.Cutting.Model;

namespace NJIS.FPZWS.LineControl.Cutting.Repository.LineControl.CuttingRepository
{
    public class AllTaskRepository:DapperRepository<AllTask>
    {
        private const string ConnectionString = @"Data Source=10.30.3.122;User Id=sa;Password=!Q@W#E$R5t6y7u8i;Initial Catalog=WorkStation;";

        public AllTaskRepository():this(ConnectionString/*DbSettings.Current.ConnectionString*/)
        {
        }
        public AllTaskRepository(string connectString) : base(new SqlConnection(connectString)) { }
        //public AllTaskRepository(IDbConnection connection, ESqlConnector sqlConnector) : base(connection, sqlConnector) { }
        public AllTaskRepository(IDbConnection connection, ISqlGenerator<AllTask> sqlGenerator) : base(connection, sqlGenerator) { }
        public AllTaskRepository(IDbConnection connection, SqlGeneratorConfig config) : base(connection, config) { }
    }
}
