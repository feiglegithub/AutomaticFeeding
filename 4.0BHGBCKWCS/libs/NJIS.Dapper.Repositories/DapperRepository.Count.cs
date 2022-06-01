// ************************************************************************************
//  解决方案：NJIS.FPZWS.LineControl.Drilling
//  项目名称：NJIS.Dapper.Repositories
//  文 件 名：DapperRepository.Count.cs
//  创建时间：2019-08-30 9:48
//  作    者：
//  说    明：
//  修改时间：2019-08-30 8:38
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
    public partial class DapperRepository<TEntity>
    {
        public virtual int Count()
        {
            return Count(transaction: null);
        }


        public virtual int Count(IDbTransaction transaction)
        {
            return Count(null, transaction);
        }


        public virtual int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return Count(predicate, transaction: null);
        }


        public virtual int Count(Expression<Func<TEntity, bool>> predicate, IDbTransaction transaction)
        {
            var queryResult = SqlGenerator.GetCount(predicate);
            return Connection.QueryFirstOrDefault<int>(queryResult.GetSql(), queryResult.Param, transaction);
        }

        public virtual int Count(Expression<Func<TEntity, object>> distinctField)
        {
            return Count(distinctField, null);
        }


        public virtual int Count(Expression<Func<TEntity, object>> distinctField, IDbTransaction transaction)
        {
            return Count(null, distinctField, transaction);
        }


        public virtual int Count(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> distinctField)
        {
            return Count(predicate, distinctField, null);
        }


        public virtual int Count(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> distinctField, IDbTransaction transaction)
        {
            var queryResult = SqlGenerator.GetCount(predicate, distinctField);
            return Connection.QueryFirstOrDefault<int>(queryResult.GetSql(), queryResult.Param, transaction);
        }


        public virtual Task<int> CountAsync()
        {
            return CountAsync(transaction: null);
        }


        public virtual Task<int> CountAsync(IDbTransaction transaction)
        {
            return CountAsync(null, transaction);
        }


        public virtual Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return CountAsync(predicate, transaction: null);
        }


        public virtual Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, IDbTransaction transaction)
        {
            var queryResult = SqlGenerator.GetCount(predicate);
            return Connection.QueryFirstOrDefaultAsync<int>(queryResult.GetSql(), queryResult.Param, transaction);
        }


        public virtual Task<int> CountAsync(Expression<Func<TEntity, object>> distinctField)
        {
            return CountAsync(distinctField, null);
        }


        public virtual Task<int> CountAsync(Expression<Func<TEntity, object>> distinctField, IDbTransaction transaction)
        {
            return CountAsync(null, distinctField, transaction);
        }


        public virtual Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> distinctField)
        {
            return CountAsync(predicate, distinctField, null);
        }


        public virtual Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> distinctField, IDbTransaction transaction)
        {
            var queryResult = SqlGenerator.GetCount(predicate, distinctField);
            return Connection.QueryFirstOrDefaultAsync<int>(queryResult.GetSql(), queryResult.Param, transaction);
        }
    }
}