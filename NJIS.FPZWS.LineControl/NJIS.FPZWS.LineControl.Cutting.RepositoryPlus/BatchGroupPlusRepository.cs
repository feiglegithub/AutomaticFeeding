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
    public class BatchGroupPlusRepository : DapperRepository<BatchGroupPlus>
    {
        public BatchGroupPlusRepository() : this(DbSettings.PlusConnectionString2){}
        public BatchGroupPlusRepository(string connectString) : base(new SqlConnection(connectString)) { }
        public BatchGroupPlusRepository(IDbConnection connection, ISqlGenerator<BatchGroupPlus> sqlGenerator) : base(connection, sqlGenerator) { }
        public BatchGroupPlusRepository(IDbConnection connection, SqlGeneratorConfig config) : base(connection, config) { }
    }
}
