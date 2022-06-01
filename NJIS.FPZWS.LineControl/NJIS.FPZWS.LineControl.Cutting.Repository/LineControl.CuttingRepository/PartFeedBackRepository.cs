using NJIS.Dapper.Repositories;
using NJIS.Dapper.Repositories.SqlGenerator;
using NJIS.FPZWS.LineControl.Cutting.Model;
using System.Data;
using System.Data.SqlClient;

namespace NJIS.FPZWS.LineControl.Cutting.Repository.LineControl.CuttingRepository
{
    public class PartFeedBackRepository : DapperRepository<PartFeedBack>
    {
        private const string ConnectionString = @"Data Source=.;User Id=sa;Password=!Q@W#E$R5t6y7u8i;Initial Catalog=NJIS.FPZWS.LineControl.Cutting;";

        public PartFeedBackRepository() : this(ConnectionString/*DbSettings.Current.ConnectionString*/)
        {
        }
        public PartFeedBackRepository(string connectString) : base(new SqlConnection(connectString)) { }
        public PartFeedBackRepository(IDbConnection connection, ISqlGenerator<PartFeedBack> sqlGenerator) : base(connection, sqlGenerator) { }
        public PartFeedBackRepository(IDbConnection connection, SqlGeneratorConfig config) : base(connection, config) { }
    }
}
