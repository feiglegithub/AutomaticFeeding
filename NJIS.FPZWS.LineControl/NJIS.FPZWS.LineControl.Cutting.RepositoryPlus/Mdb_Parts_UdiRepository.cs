using System.Data;
using System.Data.SqlClient;
using NJIS.Dapper.Repositories;
using NJIS.Dapper.Repositories.SqlGenerator;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;

namespace NJIS.FPZWS.LineControl.Cutting.RepositoryPlus
{
    public class Mdb_Parts_UdiRepository : DapperRepository<Mdb_Parts_Udi>
    {
        private const string ConnectionString = @"Data Source=.;User Id=sa;Password=!Q@W#E$R5t6y7u8i;Initial Catalog=NJIS.FPZWS.LineControl.Cutting;";

        public Mdb_Parts_UdiRepository() : this(DbSettings.Current.PlusConnectionString)
        {
        }
        public Mdb_Parts_UdiRepository(string connectString) : base(new SqlConnection(connectString)) { }
        //public AllTaskRepository(IDbConnection connection, ESqlConnector sqlConnector) : base(connection, sqlConnector) { }
        public Mdb_Parts_UdiRepository(IDbConnection connection, ISqlGenerator<Mdb_Parts_Udi> sqlGenerator) : base(connection, sqlGenerator) { }
        public Mdb_Parts_UdiRepository(IDbConnection connection, SqlGeneratorConfig config) : base(connection, config) { }
    }
}