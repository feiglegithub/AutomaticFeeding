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
    public class CutPartInfoCollectorRepository: DapperRepository<CutPartInfoCollector>
    {
        //private const string ConnectionString = @"Data Source=10.30.18.231;User Id=sa;Password=!Q@W#E$R5t6y7u8i;Initial Catalog=NJIS.FPZWS.LineControl.Cutting;";
        public CutPartInfoCollectorRepository() : this(DbSettings.Current.ConnectionString) { }
        public CutPartInfoCollectorRepository(string connectString) : base(new SqlConnection(connectString)) { }
        //public CutPartInfoCollectorRepository(IDbConnection connection, ESqlConnector sqlConnector) : base(connection, sqlConnector) { }
        public CutPartInfoCollectorRepository(IDbConnection connection, ISqlGenerator<CutPartInfoCollector> sqlGenerator) : base(connection, sqlGenerator) { }
        public CutPartInfoCollectorRepository(IDbConnection connection, SqlGeneratorConfig config) : base(connection, config) { }
    }
}
