using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using NJIS.Dapper.Repositories;
using NJIS.Dapper.Repositories.SqlGenerator;
using NJIS.FPZWS.LineControl.PartMill.Model;

namespace NJIS.FPZWS.LineControl.PartMill.Repository
{
    public class line_taskRepository: DapperRepository<line_task>
    {

        
        private const string ConnectionString = @"Server=127.0.0.1;Database=pcs_business;Uid=sa;Pwd=!Q@W#E$R5t6y7u8i;";
        public line_taskRepository() : this(ConnectionString,SqlProvider.MySQL) { }
        public line_taskRepository(string connectString, SqlProvider sqlProvider) : base(new MySqlConnection(connectString), sqlProvider) { }
        public line_taskRepository(IDbConnection connection, ISqlGenerator<line_task> sqlGenerator) : base(connection, sqlGenerator) { }
        public line_taskRepository(IDbConnection connection, SqlGeneratorConfig config) : base(connection, config) { }
    }
}
