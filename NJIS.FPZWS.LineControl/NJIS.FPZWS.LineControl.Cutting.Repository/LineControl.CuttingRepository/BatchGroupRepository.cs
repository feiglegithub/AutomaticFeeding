﻿using NJIS.FPZWS.LineControl.Cutting.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJIS.Dapper.Repositories;
using NJIS.Dapper.Repositories.SqlGenerator;

namespace NJIS.FPZWS.LineControl.Cutting.Repository.LineControl.CuttingRepository
{
    public class BatchGroupRepository : DapperRepository<BatchGroup>
    {
        private const string ConnectionString = @"Data Source=.;User Id=sa;Password=!Q@W#E$R5t6y7u8i;Initial Catalog=NJIS.FPZWS.LineControl.Cutting;";

        public BatchGroupRepository() : this(ConnectionString/*DbSettings.Current.ConnectionString*/)
        {
        }
        public BatchGroupRepository(string connectString) : base(new SqlConnection(connectString)) { }
        //public AllTaskRepository(IDbConnection connection, ESqlConnector sqlConnector) : base(connection, sqlConnector) { }
        public BatchGroupRepository(IDbConnection connection, ISqlGenerator<BatchGroup> sqlGenerator) : base(connection, sqlGenerator) { }
        public BatchGroupRepository(IDbConnection connection, SqlGeneratorConfig config) : base(connection, config) { }
    }
}
