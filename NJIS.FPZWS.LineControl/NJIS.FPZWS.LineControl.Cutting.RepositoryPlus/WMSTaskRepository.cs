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
    public class WMSTaskRepository : DapperRepository<WMS_Task>
    {
        public WMSTaskRepository() : this(DbSettings.HuangGangBWMSConnectionString){}
        public WMSTaskRepository(string connectString) : base(new SqlConnection(connectString)) { }
        public WMSTaskRepository(IDbConnection connection, ISqlGenerator<WMS_Task> sqlGenerator) : base(connection, sqlGenerator) { }
        public WMSTaskRepository(IDbConnection connection, SqlGeneratorConfig config) : base(connection, config) { }
    }
}
