using NJIS.Dapper.Repositories;
using NJIS.Dapper.Repositories.SqlGenerator;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.LineControl.Cutting.RepositoryPlus
{
    public class CuttingStackListRepository : DapperRepository<CuttingStackList>
    {
        public CuttingStackListRepository(string connectString) : base(new SqlConnection(connectString)) { }
        public CuttingStackListRepository() : this(DbSettings.ConnectionString2) { }
        public CuttingStackListRepository(IDbConnection connection, ISqlGenerator<CuttingStackList> sqlGenerator) : base(connection, sqlGenerator) { }
        public CuttingStackListRepository(IDbConnection connection, SqlGeneratorConfig config) : base(connection, config) { }
    }
}
