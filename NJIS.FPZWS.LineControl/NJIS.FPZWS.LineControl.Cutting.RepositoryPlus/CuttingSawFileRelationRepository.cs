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
    public class CuttingSawFileRelationRepository : DapperRepository<CuttingSawFileRelation>
    {
        public CuttingSawFileRelationRepository(string connectString) : base(new SqlConnection(connectString)) { }
        public CuttingSawFileRelationRepository() : this(DbSettings.ConnectionString2) { }
        public CuttingSawFileRelationRepository(IDbConnection connection, ISqlGenerator<CuttingSawFileRelation> sqlGenerator) : base(connection, sqlGenerator) { }
        public CuttingSawFileRelationRepository(IDbConnection connection, SqlGeneratorConfig config) : base(connection, config) { }
    }
}
