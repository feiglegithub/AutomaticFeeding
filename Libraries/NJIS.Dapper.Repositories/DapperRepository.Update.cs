// ************************************************************************************
//  解决方案：NJIS.FPZWS.LineControl.Drilling
//  项目名称：NJIS.Dapper.Repositories
//  文 件 名：DapperRepository.Update.cs
//  创建时间：2019-08-30 9:49
//  作    者：
//  说    明：
//  修改时间：2019-08-30 9:38
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

using System;
using System.Data;
using System.Linq.Expressions;
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
        public virtual bool Update(TEntity instance, IDbTransaction transaction)
        {
            var sqlQuery = SqlGenerator.GetUpdate(instance);
            var updated = Connection.Execute(sqlQuery.GetSql(), instance, transaction) > 0;
            return updated;
        }


        public virtual async Task<bool> UpdateAsync(TEntity instance, IDbTransaction transaction)
        {
            var sqlQuery = SqlGenerator.GetUpdate(instance);
            var updated = await Connection.ExecuteAsync(sqlQuery.GetSql(), instance, transaction) > 0;
            return updated;
        }

        public virtual bool Update(TEntity instance)
        {
            return Update(instance, null);
        }


        public virtual Task<bool> UpdateAsync(TEntity instance)
        {
            return UpdateAsync(instance, null);
        }


        public virtual bool Update(Expression<Func<TEntity, bool>> predicate, TEntity instance)
        {
            return Update(predicate, instance, null);
        }


        public virtual bool Update(Expression<Func<TEntity, bool>> predicate, TEntity instance,
            IDbTransaction transaction)
        {
            var sqlQuery = SqlGenerator.GetUpdate(predicate, instance);
            var updated = Connection.Execute(sqlQuery.GetSql(), sqlQuery.Param, transaction) > 0;
            return updated;
        }


        public virtual Task<bool> UpdateAsync(Expression<Func<TEntity, bool>> predicate, TEntity instance)
        {
            return UpdateAsync(predicate, instance, null);
        }


        public virtual async Task<bool> UpdateAsync(Expression<Func<TEntity, bool>> predicate, TEntity instance,
            IDbTransaction transaction)
        {
            var sqlQuery = SqlGenerator.GetUpdate(predicate, instance);
            var updated = await Connection.ExecuteAsync(sqlQuery.GetSql(), sqlQuery.Param, transaction) > 0;
            return updated;
        }
    }
}