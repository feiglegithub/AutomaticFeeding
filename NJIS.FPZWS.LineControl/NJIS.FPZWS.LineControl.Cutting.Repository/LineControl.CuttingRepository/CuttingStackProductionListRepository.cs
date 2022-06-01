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
    public class CuttingStackProductionListRepository : DapperRepository<CuttingStackProductionList>
    {
        //private const string ConnectionString = @"Data Source=10.30.18.231;User Id=sa;Password=!Q@W#E$R5t6y7u8i;Initial Catalog=NJIS.FPZWS.LineControl.Cutting;";
        private const string ConnectionString = @"Data Source=.;User Id=sa;Password=!Q@W#E$R5t6y7u8i;Initial Catalog=NJIS.FPZWS.LineControl.Cutting;";
        public CuttingStackProductionListRepository() : this(ConnectionString/*DbSettings.Current.ConnectionString*/) { }
        public CuttingStackProductionListRepository(string connectString) : base(new SqlConnection(connectString)) { }
        //public CuttingStackProductionListRepository(IDbConnection connection, ESqlConnector sqlConnector) : base(connection, sqlConnector) { }
        public CuttingStackProductionListRepository(IDbConnection connection, ISqlGenerator<CuttingStackProductionList> sqlGenerator) : base(connection, sqlGenerator) { }
        public CuttingStackProductionListRepository(IDbConnection connection, SqlGeneratorConfig config) : base(connection, config) { }
    }
}
