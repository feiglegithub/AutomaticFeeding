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
    class TaskManagerRepository : DapperRepository<TaskManager>
    {
        public TaskManagerRepository() : this(DbSettings.PlusConnectionString2) { }
        public TaskManagerRepository(string connectString) : base(new SqlConnection(connectString)) { }
        public TaskManagerRepository(IDbConnection connection, ISqlGenerator<TaskManager> sqlGenerator) : base(connection, sqlGenerator) { }
        public TaskManagerRepository(IDbConnection connection, SqlGeneratorConfig config) : base(connection, config) { }
    }
}
