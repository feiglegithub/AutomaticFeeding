using NJIS.Dapper.Repositories;
using NJIS.FPZWS.LineControl.Cutting.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJIS.Dapper.Repositories.SqlGenerator;

namespace NJIS.FPZWS.LineControl.Cutting.Repository.LineControl.CuttingRepository
{
    public class WMSStacktFeedBackRepository : DapperRepository<WMSStacktFeedBack>
    {
        //private const string ConnectionString = @"Data Source=10.30.18.231;User Id=sa;Password=!Q@W#E$R5t6y7u8i;Initial Catalog=NJIS.FPZWS.LineControl.Cutting;";

        private const string ConnectionString = @"Data Source=.;User Id=sa;Password=!Q@W#E$R5t6y7u8i;Initial Catalog=NJIS.FPZWS.LineControl.Cutting;";

        public WMSStacktFeedBackRepository() : this(ConnectionString/*DbSettings.Current.ConnectionString*/) { }
        public WMSStacktFeedBackRepository(string connectString) : base(new SqlConnection(connectString)) { }
        //public WMSStacktFeedBackRepository(IDbConnection connection, ESqlConnector sqlConnector) : base(connection, sqlConnector) { }
        public WMSStacktFeedBackRepository(IDbConnection connection, ISqlGenerator<WMSStacktFeedBack> sqlGenerator) : base(connection, sqlGenerator) { }
        public WMSStacktFeedBackRepository(IDbConnection connection, SqlGeneratorConfig config) : base(connection, config) { }
    }
}
