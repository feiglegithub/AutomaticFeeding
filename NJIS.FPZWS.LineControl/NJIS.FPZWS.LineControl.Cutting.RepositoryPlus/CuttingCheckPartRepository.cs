﻿using System.Data;
using System.Data.SqlClient;
using NJIS.Dapper.Repositories;
using NJIS.Dapper.Repositories.SqlGenerator;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;

namespace NJIS.FPZWS.LineControl.Cutting.RepositoryPlus
{
    public class CuttingCheckPartRepository:DapperRepository<CuttingCheckPart>
    {
        //private const string ConnectionString = @"Data Source=10.30.18.231;User Id=sa;Password=!Q@W#E$R5t6y7u8i;Initial Catalog=NJIS.FPZWS.LineControl.Cutting;";
        public CuttingCheckPartRepository() : this(DbSettings.Current.ConnectionString) { }
        public CuttingCheckPartRepository(string connectString) : base(new SqlConnection(connectString)) { }
        //public CuttingCheckPartRepository(IDbConnection connection, ESqlConnector sqlConnector) : base(connection, sqlConnector) { }
        public CuttingCheckPartRepository(IDbConnection connection, ISqlGenerator<CuttingCheckPart> sqlGenerator) : base(connection, sqlGenerator) { }
        public CuttingCheckPartRepository(IDbConnection connection, SqlGeneratorConfig config) : base(connection, config) { }
    }
}
