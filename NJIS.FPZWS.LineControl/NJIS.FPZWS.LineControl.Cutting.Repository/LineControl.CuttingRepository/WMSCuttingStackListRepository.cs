using NJIS.Dapper.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJIS.Dapper.Repositories.SqlGenerator;
using NJIS.FPZWS.LineControl.Cutting.Model;

namespace NJIS.FPZWS.LineControl.Cutting.Repository.LineControl.CuttingRepository
{
    public class WMSCuttingStackListRepository : DapperRepository<WMSCuttingStackList>
    {
        //private const string ConnectionString = @"Data Source=10.30.18.231;User Id=sa;Password=!Q@W#E$R5t6y7u8i;Initial Catalog=NJIS.FPZWS.LineControl.Cutting;";
        private const string ConnectionString = @"Data Source=.;User Id=sa;Password=!Q@W#E$R5t6y7u8i;Initial Catalog=NJIS.FPZWS.LineControl.Cutting;";
        public WMSCuttingStackListRepository() : this(ConnectionString/*DbSettings.Current.ConnectionString*/) { }
        public WMSCuttingStackListRepository(string connectString) : base(new SqlConnection(connectString)) { }
        //public WMSCuttingStackListRepository(IDbConnection connection, ESqlConnector sqlConnector) : base(connection, sqlConnector) { }
        public WMSCuttingStackListRepository(IDbConnection connection, ISqlGenerator<WMSCuttingStackList> sqlGenerator) : base(connection, sqlGenerator) { }
        public WMSCuttingStackListRepository(IDbConnection connection, SqlGeneratorConfig config) : base(connection, config) { }
    }
}
