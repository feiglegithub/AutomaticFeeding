//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace NJIS.FPZWS.LineControl.Drilling.Repository
{
    using NJIS.FPZWS.LineControl.Drilling.Model;
    using System.Data;
    using System;
    using NJIS.Dapper.Repositories;
    using NJIS.Dapper.Repositories.SqlGenerator;
    using System.Data.SqlClient;
    
    public partial class PcsPartInfoQueueRepository : DapperRepository<PcsPartInfoQueue>
    {
    		public PcsPartInfoQueueRepository(string connectString) : base(new SqlConnection(connectString)){} 
    	public PcsPartInfoQueueRepository(IDbConnection connection, SqlProvider sqlProvider) : base(connection, sqlProvider){} 
    	public PcsPartInfoQueueRepository(IDbConnection connection, ISqlGenerator<PcsPartInfoQueue> sqlGenerator) : base(connection, sqlGenerator){} 
    	public PcsPartInfoQueueRepository(IDbConnection connection, SqlGeneratorConfig config) : base(connection, config){}
    }
    
}
