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
    public class CuttingSawFileRelationPlusRepository : DapperRepository<CuttingSawFileRelationPlus>
    {
        public CuttingSawFileRelationPlusRepository(string connectString) : base(new SqlConnection(connectString)) { }
        public CuttingSawFileRelationPlusRepository() : this(DbSettings.PlusConnectionString2) { }
        public CuttingSawFileRelationPlusRepository(IDbConnection connection, ISqlGenerator<CuttingSawFileRelationPlus> sqlGenerator) : base(connection, sqlGenerator) { }
        public CuttingSawFileRelationPlusRepository(IDbConnection connection, SqlGeneratorConfig config) : base(connection, config) { }
    }
}
