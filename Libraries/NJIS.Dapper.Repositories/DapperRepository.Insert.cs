// ************************************************************************************
//  解决方案：NJIS.FPZWS.LineControl.Drilling
//  项目名称：NJIS.Dapper.Repositories
//  文 件 名：DapperRepository.Insert.cs
//  创建时间：2019-08-30 9:49
//  作    者：
//  说    明：
//  修改时间：2019-08-30 9:38
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace NJIS.Dapper.Repositories
{
    /// <summary>
    ///     Base Repository
    /// </summary>
    public partial class DapperRepository<TEntity>
        where TEntity : class
    {
        public virtual bool Insert(TEntity instance, IDbTransaction transaction)
        {
            var queryResult = SqlGenerator.GetInsert(instance);
            if (SqlGenerator.IsIdentity)
            {
                var newId = Connection.Query<long>(queryResult.GetSql(), queryResult.Param, transaction)
                    .FirstOrDefault();
                return SetValue(newId, instance);
            }

            return Connection.Execute(queryResult.GetSql(), instance, transaction) > 0;
        }


        public virtual async Task<bool> InsertAsync(TEntity instance, IDbTransaction transaction)
        {
            var queryResult = SqlGenerator.GetInsert(instance);
            if (SqlGenerator.IsIdentity)
            {
                var newId = (await Connection.QueryAsync<long>(queryResult.GetSql(), queryResult.Param, transaction))
                    .FirstOrDefault();
                return SetValue(newId, instance);
            }

            return await Connection.ExecuteAsync(queryResult.GetSql(), instance, transaction) > 0;
        }

        public virtual bool Insert(TEntity instance)
        {
            return Insert(instance, null);
        }


        public virtual Task<bool> InsertAsync(TEntity instance)
        {
            return InsertAsync(instance, null);
        }

        private bool SetValue(long newId, TEntity instance)
        {
            var added = newId > 0;
            if (added)
            {
                var newParsedId = Convert.ChangeType(newId, SqlGenerator.IdentitySqlProperty.PropertyInfo.PropertyType);
                SqlGenerator.IdentitySqlProperty.PropertyInfo.SetValue(instance, newParsedId);
            }

            return added;
        }
    }
}