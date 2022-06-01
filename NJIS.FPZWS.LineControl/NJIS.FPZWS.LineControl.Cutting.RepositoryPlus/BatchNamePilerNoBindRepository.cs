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
    public class BatchNamePilerNoBindRepository : DapperRepository<BatchNamePilerNoBind>
    {
        public BatchNamePilerNoBindRepository() : this(DbSettings.PlusConnectionString2){}
        public BatchNamePilerNoBindRepository(string connectString) : base(new SqlConnection(connectString)) { }
        public BatchNamePilerNoBindRepository(IDbConnection connection, ISqlGenerator<BatchNamePilerNoBind> sqlGenerator) : base(connection, sqlGenerator) { }
        public BatchNamePilerNoBindRepository(IDbConnection connection, SqlGeneratorConfig config) : base(connection, config) { }
    }
}
