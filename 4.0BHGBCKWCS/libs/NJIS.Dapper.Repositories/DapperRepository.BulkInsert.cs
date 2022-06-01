// ************************************************************************************
//  解决方案：NJIS.FPZWS.LineControl.Drilling
//  项目名称：NJIS.Dapper.Repositories
//  文 件 名：DapperRepository.BulkInsert.cs
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
        public virtual int BulkInsert(IEnumerable<TEntity> instances, IDbTransaction transaction = null)
        {
            if (SqlGenerator.Config.SqlProvider == SqlProvider.MSSQL)
            {
                var count = 0;
                var totalInstances = instances.Count();

                var properties =
                    (SqlGenerator.IsIdentity
                        ? SqlGenerator.SqlProperties.Where(p =>
                            !p.PropertyName.Equals(SqlGenerator.IdentitySqlProperty.PropertyName,
                                StringComparison.OrdinalIgnoreCase))
                        : SqlGenerator.SqlProperties).ToList();

                var exceededTimes = (int) Math.Ceiling(totalInstances * properties.Count / 2100d);
                if (exceededTimes > 1)
                {
                    var maxAllowedInstancesPerBatch = totalInstances / exceededTimes;

                    for (var i = 0; i <= exceededTimes; i++)
                    {
                        var items = instances.Skip(i * maxAllowedInstancesPerBatch).Take(maxAllowedInstancesPerBatch);
                        var msSqlQueryResult = SqlGenerator.GetBulkInsert(items);
                        count += Connection.Execute(msSqlQueryResult.GetSql(), msSqlQueryResult.Param, transaction);
                    }

                    return count;
                }
            }

            var queryResult = SqlGenerator.GetBulkInsert(instances);
            return Connection.Execute(queryResult.GetSql(), queryResult.Param, transaction);
        }


        public virtual async Task<int> BulkInsertAsync(IEnumerable<TEntity> instances,
            IDbTransaction transaction = null)
        {
            if (SqlGenerator.Config.SqlProvider == SqlProvider.MSSQL)
            {
                var count = 0;
                var totalInstances = instances.Count();

                var properties =
                    (SqlGenerator.IsIdentity
                        ? SqlGenerator.SqlProperties.Where(p =>
                            !p.PropertyName.Equals(SqlGenerator.IdentitySqlProperty.PropertyName,
                                StringComparison.OrdinalIgnoreCase))
                        : SqlGenerator.SqlProperties).ToList();

                var exceededTimes = (int) Math.Ceiling(totalInstances * properties.Count / 2100d);
                if (exceededTimes > 1)
                {
                    var maxAllowedInstancesPerBatch = totalInstances / exceededTimes;

                    for (var i = 0; i <= exceededTimes; i++)
                    {
                        var items = instances.Skip(i * maxAllowedInstancesPerBatch).Take(maxAllowedInstancesPerBatch);
                        var msSqlQueryResult = SqlGenerator.GetBulkInsert(items);
                        count += await Connection.ExecuteAsync(msSqlQueryResult.GetSql(), msSqlQueryResult.Param,
                            transaction);
                    }

                    return count;
                }
            }

            var queryResult = SqlGenerator.GetBulkInsert(instances);
            return await Connection.ExecuteAsync(queryResult.GetSql(), queryResult.Param, transaction);
        }
    }
}