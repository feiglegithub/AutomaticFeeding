using System.Data;
using System.Data.SqlClient;
using NJIS.Dapper.Repositories;
using NJIS.Dapper.Repositories.SqlGenerator;
using NJIS.FPZWS.LineControl.Cutting.Model;

namespace NJIS.FPZWS.LineControl.Cutting.Repository.LineControl.CuttingRepository
{
    public class CuttingSawFileRelationRepository : DapperRepository<CuttingSawFileRelation>
    {
        //private const string ConnectionString = @"Data Source=10.30.18.231;User Id=sa;Password=!Q@W#E$R5t6y7u8i;Initial Catalog=NJIS.FPZWS.LineControl.Cutting;";
        public CuttingSawFileRelationRepository() : this(DbSettings.ConnectionString2) { }
        public CuttingSawFileRelationRepository(string connectString) : base(new SqlConnection(connectString)) { }
        //public CuttingStackListRepository(IDbConnection connection, ESqlConnector sqlConnector) : base(connection, sqlConnector) { }
        public CuttingSawFileRelationRepository(IDbConnection connection, ISqlGenerator<CuttingSawFileRelation> sqlGenerator) : base(connection, sqlGenerator) { }
        public CuttingSawFileRelationRepository(IDbConnection connection, SqlGeneratorConfig config) : base(connection, config) { }
    }

}
