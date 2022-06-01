using System.Data;
using System.Data.SqlClient;
using NJIS.Dapper.Repositories;
using NJIS.Dapper.Repositories.SqlGenerator;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;

namespace NJIS.FPZWS.LineControl.Cutting.RepositoryPlus
{
    public class WmsStacktFeedBackRepository : DapperRepository<WMSStacktFeedBack>
    {
        //private const string ConnectionString = @"Data Source=10.30.18.231;User Id=sa;Password=!Q@W#E$R5t6y7u8i;Initial Catalog=NJIS.FPZWS.LineControl.Cutting;";

        private const string ConnectionString = @"Data Source=.;User Id=sa;Password=!Q@W#E$R5t6y7u8i;Initial Catalog=NJIS.FPZWS.LineControl.Cutting;";

        public WmsStacktFeedBackRepository() : this(DbSettings.Current.ConnectionString) { }
        public WmsStacktFeedBackRepository(string connectString) : base(new SqlConnection(connectString)) { }
        //public WMSStacktFeedBackRepository(IDbConnection connection, ESqlConnector sqlConnector) : base(connection, sqlConnector) { }
        public WmsStacktFeedBackRepository(IDbConnection connection, ISqlGenerator<WMSStacktFeedBack> sqlGenerator) : base(connection, sqlGenerator) { }
        public WmsStacktFeedBackRepository(IDbConnection connection, SqlGeneratorConfig config) : base(connection, config) { }
    }
}
