﻿using System;
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
    public class CuttingCheckRuleRepository : DapperRepository<CuttingCheckRule>
    {
        //private const string ConnectionString = @"Data Source=10.30.18.231;User Id=sa;Password=!Q@W#E$R5t6y7u8i;Initial Catalog=NJIS.FPZWS.LineControl.Cutting;";

        public CuttingCheckRuleRepository() : this(DbSettings.Current.ConnectionString) { }
        public CuttingCheckRuleRepository(string connectString) : base(new SqlConnection(connectString)) { }
        //public CuttingCheckRuleRepository(IDbConnection connection, ESqlConnector sqlConnector) : base(connection, sqlConnector) { }
        public CuttingCheckRuleRepository(IDbConnection connection, ISqlGenerator<CuttingCheckRule> sqlGenerator) : base(connection, sqlGenerator) { }
        public CuttingCheckRuleRepository(IDbConnection connection, SqlGeneratorConfig config) : base(connection, config) { }
    }
}
