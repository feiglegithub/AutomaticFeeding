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

namespace NJIS.FPZWS.LineControl.Cutting.Repository
{
    public class TaskDistributeRepository : DapperRepository<TaskDistribute>
    {
        private const string ConnectionString = @"Data Source=10.10.14.51;User Id=sa;Password=!Q@W#E$R5t6y7u8i;Initial Catalog=MesDataCenter;";
        public TaskDistributeRepository(string connectString = ConnectionString) : base(new SqlConnection(connectString)) { }
        //public TaskDistributeRepository(IDbConnection connection, ESqlConnector sqlConnector) : base(connection, sqlConnector) { }
        public TaskDistributeRepository(IDbConnection connection, ISqlGenerator<TaskDistribute> sqlGenerator) : base(connection, sqlGenerator) { }
        public TaskDistributeRepository(IDbConnection connection, SqlGeneratorConfig config) : base(connection, config) { }
    }
}
