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
    public class read_labelfile_configRepository : DapperRepository<read_labelfile_config>
    {
        private const string ConnectionString = @"Server=127.0.0.1;Database=pcs_business;Uid=sa;Pwd=!Q@W#E$R5t6y7u8i;";
        public read_labelfile_configRepository() : this(ConnectionString, SqlProvider.MySQL) { }
        public read_labelfile_configRepository(string connectString, SqlProvider sqlProvider) : base(new MySqlConnection(connectString), sqlProvider) { }
        //public CutPartInfoCollectorRepository(IDbConnection connection, ESqlConnector sqlConnector) : base(connection, sqlConnector) { }
        public read_labelfile_configRepository(IDbConnection connection, ISqlGenerator<read_labelfile_config> sqlGenerator) : base(connection, sqlGenerator) { }
        public read_labelfile_configRepository(IDbConnection connection, SqlGeneratorConfig config) : base(connection, config) { }
    }
}
