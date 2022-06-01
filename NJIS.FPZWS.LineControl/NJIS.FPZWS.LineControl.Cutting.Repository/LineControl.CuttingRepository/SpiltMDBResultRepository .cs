using System.Data;
using System.Data.SqlClient;
using NJIS.Dapper.Repositories;
using NJIS.Dapper.Repositories.SqlGenerator;
using NJIS.FPZWS.LineControl.Cutting.Model;

namespace NJIS.FPZWS.LineControl.Cutting.Repository.LineControl.CuttingRepository
{
    public class SpiltMDBResultRepository : DapperRepository<SpiltMDBResult>
    {
        //private const string ConnectionString = @"Data Source=10.30.18.231;User Id=sa;Password=!Q@W#E$R5t6y7u8i;Initial Catalog=NJIS.FPZWS.LineControl.Cutting;";
        public SpiltMDBResultRepository() : this(DbSettings.Current.ConnectionString) { }
        public SpiltMDBResultRepository(string connectString) : base(new SqlConnection(connectString)) { }
        //public SpiltMDBResultRepository(IDbConnection connection, ESqlConnector sqlConnector) : base(connection, sqlConnector) { }
        public SpiltMDBResultRepository(IDbConnection connection, ISqlGenerator<SpiltMDBResult> sqlGenerator) : base(connection, sqlGenerator) { }
        public SpiltMDBResultRepository(IDbConnection connection, SqlGeneratorConfig config) : base(connection, config) { }
    }
}
