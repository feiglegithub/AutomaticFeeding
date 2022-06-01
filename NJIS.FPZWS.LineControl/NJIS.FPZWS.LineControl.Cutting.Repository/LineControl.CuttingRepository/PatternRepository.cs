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
    public class PatternRepository : DapperRepository<Pattern>
    {
        private const string ConnectionString = @"Data Source=.;User Id=sa;Password=!Q@W#E$R5t6y7u8i;Initial Catalog=NJIS.FPZWS.LineControl.Cutting;";

        public PatternRepository() : this(ConnectionString/*DbSettings.Current.ConnectionString*/)
        {
        }
        public PatternRepository(string connectString) : base(new SqlConnection(connectString)) { }
        //public AllTaskRepository(IDbConnection connection, ESqlConnector sqlConnector) : base(connection, sqlConnector) { }
        public PatternRepository(IDbConnection connection, ISqlGenerator<Pattern> sqlGenerator) : base(connection, sqlGenerator) { }
        public PatternRepository(IDbConnection connection, SqlGeneratorConfig config) : base(connection, config) { }
    }
}
