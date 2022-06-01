using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJIS.Dapper.Repositories;
using NJIS.Dapper.Repositories.SqlGenerator;
using NJIS.FPZWS.LineControl.Cutting.Model;

namespace NJIS.FPZWS.LineControl.Cutting.Repository.LineControl.CuttingRepository
{
    public class CuttingManualLabelPrinterRepository : DapperRepository<CuttingManualLabelPrinter>
    {
        //private const string ConnectionString = @"Data Source=10.30.18.231;User Id=sa;Password=!Q@W#E$R5t6y7u8i;Initial Catalog=NJIS.FPZWS.LineControl.Cutting;";
        public CuttingManualLabelPrinterRepository() : this(DbSettings.Current.ConnectionString) { }
        public CuttingManualLabelPrinterRepository(string connectString) : base(new SqlConnection(connectString)) { }
        //public CuttingManualLabelPrinterRepository(IDbConnection connection, ESqlConnector sqlConnector) : base(connection, sqlConnector) { }
        public CuttingManualLabelPrinterRepository(IDbConnection connection, ISqlGenerator<CuttingManualLabelPrinter> sqlGenerator) : base(connection, sqlGenerator) { }
        public CuttingManualLabelPrinterRepository(IDbConnection connection, SqlGeneratorConfig config) : base(connection, config) { }
    }
}
