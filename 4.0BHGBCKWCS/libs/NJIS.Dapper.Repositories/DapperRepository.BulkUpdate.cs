// ************************************************************************************
//  解决方案：NJIS.FPZWS.LineControl.Drilling
//  项目名称：NJIS.Dapper.Repositories
//  文 件 名：DapperRepository.BulkUpdate.cs
//  创建时间：2019-08-30 9:48
//  作    者：
//  说    明：
//  修改时间：2019-08-30 9:38
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using NJIS.Dapper.Repositories.SqlGenerator;

namespace NJIS.Dapper.Repositories
{
    /// <summary>
    ///     Base Repository
    /// </summary>
    public partial class DapperRepository<TEntity>
        where TEntity : class
    {
        public bool BulkUpdate(IEnumerable<TEntity> instances, IDbTransaction transaction)
        {
            if (SqlGenerator.Config.SqlProvider == SqlProvider.MSSQL)
            {
                var count = 0;
                var totalInstances = instances.Count();

                var properties = SqlGenerator.SqlProperties.ToList();

                var exceededTimes = (int) Math.Ceiling(totalInstances * properties.Count / 2100d);
                if (exceededTimes > 1)
                {
                    var maxAllowedInstancesPerBatch = totalInstances / exceededTimes;

                    for (var i = 0; i <= exceededTimes; i++)
                    {
                        var items = instances.Skip(i * maxAllowedInstancesPerBatch).Take(maxAllowedInstancesPerBatch);
                        var msSqlQueryResult = SqlGenerator.GetBulkUpdate(items);
                        count += Connection.Execute(msSqlQueryResult.GetSql(), msSqlQueryResult.Param, transaction);
                    }

                    return count > 0;
                }
            }

            var queryResult = SqlGenerator.GetBulkUpdate(instances);
            var result = Connection.Execute(queryResult.GetSql(), queryResult.Param, transaction) > 0;
            return result;
        }


        public async Task<bool> BulkUpdateAsync(IEnumerable<TEntity> instances, IDbTransaction transaction)
        {
            if (SqlGenerator.Config.SqlProvider == SqlProvider.MSSQL)
            {
                var count = 0;
                var totalInstances = instances.Count();

                var properties = SqlGenerator.SqlProperties.ToList();

                var exceededTimes = (int) Math.Ceiling(totalInstances * properties.Count / 2100d);
                if (exceededTimes > 1)
                {
                    var maxAllowedInstancesPerBatch = totalInstances / exceededTimes;

                    for (var i = 0; i <= exceededTimes; i++)
                    {
                        var items = instances.Skip(i * maxAllowedInstancesPerBatch).Take(maxAllowedInstancesPerBatch);
                        var msSqlQueryResult = SqlGenerator.GetBulkUpdate(items);
                        count += await Connection.ExecuteAsync(msSqlQueryResult.GetSql(), msSqlQueryResult.Param,
                            transaction);
                    }

                    return count > 0;
                }
            }

            var queryResult = SqlGenerator.GetBulkUpdate(instances);
            var result = await Connection.ExecuteAsync(queryResult.GetSql(), queryResult.Param, transaction) > 0;
            return result;
        }

        public bool BulkUpdate(IEnumerable<TEntity> instances)
        {
            return BulkUpdate(instances, null);
        }


        public Task<bool> BulkUpdateAsync(IEnumerable<TEntity> instances)
        {
            return BulkUpdateAsync(instances, null);
        }
    }
}