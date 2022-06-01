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
    public class UsersRepository : DapperRepository<users>
    {
        public UsersRepository() : this(DbSettings.PlusConnectionString2){}
        public UsersRepository(string connectString) : base(new SqlConnection(connectString)) { }
        public UsersRepository(IDbConnection connection, ISqlGenerator<users> sqlGenerator) : base(connection, sqlGenerator) { }
        public UsersRepository(IDbConnection connection, SqlGeneratorConfig config) : base(connection, config) { }
    }
}
