using NJIS.Dapper.Repositories;
using NJIS.Dapper.Repositories.SqlGenerator;
using NJIS.FPZWS.Platform.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJIS.FPZWS.Platform.Repository
{
    public class EdgebandingRepository : DapperRepository<Edgebanding>
    {
        public EdgebandingRepository() : base(new SqlConnection(DbConnectSetting.Current.DbConnect))
        {
        }

        public EdgebandingRepository(IDbConnection connection) : base(connection)
        {
        }

        public EdgebandingRepository(IDbConnection connection, ESqlConnector sqlConnector)
            : base(connection, sqlConnector)
        {
        }

        public EdgebandingRepository(IDbConnection connection, ISqlGenerator<Edgebanding> sqlGenerator)
            : base(connection, sqlGenerator)
        {
        }
    }
}
