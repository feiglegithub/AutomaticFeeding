// ************************************************************************************
//  解决方案：NJIS.FPZWS.LineControl.Drilling
//  项目名称：NJIS.Dapper.Repositories
//  文 件 名：DapperRepository.Find.cs
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
        public virtual TEntity Find(IDbTransaction transaction)
        {
            return Find(null, transaction);
        }


        public virtual TEntity Find(Expression<Func<TEntity, bool>> predicate, IDbTransaction transaction)
        {
            var queryResult = SqlGenerator.GetSelectFirst(predicate);
            return Connection.QueryFirstOrDefault<TEntity>(queryResult.GetSql(), queryResult.Param, transaction);
        }


        public virtual Task<TEntity> FindAsync(IDbTransaction transaction)
        {
            return FindAsync(null, transaction);
        }


        public virtual Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate, IDbTransaction transaction)
        {
            var queryResult = SqlGenerator.GetSelectFirst(predicate);
            return Connection.QueryFirstOrDefaultAsync<TEntity>(queryResult.GetSql(), queryResult.Param, transaction);
        }

        public virtual TEntity Find()
        {
            return Find(null, null);
        }


        public virtual TEntity Find(Expression<Func<TEntity, bool>> predicate)
        {
            return Find(predicate, null);
        }


        public virtual Task<TEntity> FindAsync()
        {
            return FindAsync(null, null);
        }


        public virtual Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return FindAsync(predicate, null);
        }
    }
}