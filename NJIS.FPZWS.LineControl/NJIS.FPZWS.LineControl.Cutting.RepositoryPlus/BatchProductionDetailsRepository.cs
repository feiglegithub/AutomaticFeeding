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
    public class BatchProductionDetailsRepository : DapperRepository<BatchProductionDetails>
    {
        public BatchProductionDetailsRepository() : this(DbSettings.PlusConnectionString2){}
        public BatchProductionDetailsRepository(string connectString) : base(new SqlConnection(connectString)) { }
        public BatchProductionDetailsRepository(IDbConnection connection, ISqlGenerator<BatchProductionDetails> sqlGenerator) : base(connection, sqlGenerator) { }
        public BatchProductionDetailsRepository(IDbConnection connection, SqlGeneratorConfig config) : base(connection, config) { }
    }
}
