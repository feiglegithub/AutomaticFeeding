//  ************************************************************************************
//   解决方案：NJIS.FPZWS.LineControl.Drilling
//   项目名称：NJIS.FPZWS.LineControl.Drilling.Repository
//   文 件 名：DrillingRepository.cs
//   创建时间：2018-11-26 9:30
//   作    者：
//   说    明：
//   修改时间：2018-11-26 9:30
//   修 改 人：
//  Copyright © 2017 广州宁基智能系统有限公司. 版权所有
//  *************************************************************************************

using System.Data;
using Dapper;

namespace NJIS.FPZWS.LineControl.Drilling.Repository
{
    public partial class DrillingRepository
    {
        public DataTable ExecuteProcToDataTable(string procName, object args)
        {
            var table = new DataTable();
            var reader = Connection.ExecuteReader(procName, args, commandType: CommandType.StoredProcedure, commandTimeout: 10);
            table.Load(reader);
            return table;
        }
    }
}