// ************************************************************************************
//  解决方案：NJIS.FPZWS.LineControl.Drilling
//  项目名称：NJIS.Dapper.Repositories
//  文 件 名：DapperRepository.FindAllBetween.cs
//  创建时间：2019-08-30 9:49
//  作    者：
//  说    明：
//  修改时间：2019-08-30 9:38
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

using System;
using System.Collections.Generic;
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
        private const string _dateTimeFormat = "yyyy-MM-dd HH:mm:ss";


        public IEnumerable<TEntity> FindAllBetween(object from, object to, Expression<Func<TEntity, object>> btwField,
            IDbTransaction transaction)
        {
            return FindAllBetween(from, to, btwField, null, transaction);
        }


        public IEnumerable<TEntity> FindAllBetween(DateTime from, DateTime to,
            Expression<Func<TEntity, object>> btwField, IDbTransaction transaction)
        {
            return FindAllBetween(from, to, btwField, null, transaction);
        }


        public IEnumerable<TEntity> FindAllBetween(
            DateTime from,
            DateTime to,
            Expression<Func<TEntity, object>> btwField,
            Expression<Func<TEntity, bool>> predicate,
            IDbTransaction transaction)
        {
            var fromString = from.ToString(_dateTimeFormat);
            var toString = to.ToString(_dateTimeFormat);
            return FindAllBetween(fromString, toString, btwField, predicate, transaction);
        }


        public IEnumerable<TEntity> FindAllBetween(
            object from,
            object to,
            Expression<Func<TEntity, object>> btwField,
            Expression<Func<TEntity, bool>> predicate,
            IDbTransaction transaction)
        {
            var queryResult = SqlGenerator.GetSelectBetween(from, to, btwField, predicate);
            return Connection.Query<TEntity>(queryResult.GetSql(), queryResult.Param, transaction);
        }


        public Task<IEnumerable<TEntity>> FindAllBetweenAsync(object from, object to,
            Expression<Func<TEntity, object>> btwField, IDbTransaction transaction)
        {
            return FindAllBetweenAsync(from, to, btwField, null, transaction);
        }


        public Task<IEnumerable<TEntity>> FindAllBetweenAsync(DateTime from, DateTime to,
            Expression<Func<TEntity, object>> btwField, IDbTransaction transaction)
        {
            return FindAllBetweenAsync(from, to, btwField, null, transaction);
        }


        public Task<IEnumerable<TEntity>> FindAllBetweenAsync(
            DateTime from,
            DateTime to,
            Expression<Func<TEntity, object>> btwField,
            Expression<Func<TEntity, bool>> predicate,
            IDbTransaction transaction)
        {
            return FindAllBetweenAsync(from.ToString(_dateTimeFormat), to.ToString(_dateTimeFormat), btwField,
                predicate, transaction);
        }


        public Task<IEnumerable<TEntity>> FindAllBetweenAsync(
            object from,
            object to,
            Expression<Func<TEntity, object>> btwField,
            Expression<Func<TEntity, bool>> predicate,
            IDbTransaction transaction)
        {
            var queryResult = SqlGenerator.GetSelectBetween(from, to, btwField, predicate);
            return Connection.QueryAsync<TEntity>(queryResult.GetSql(), queryResult.Param, transaction);
        }


        public IEnumerable<TEntity> FindAllBetween(object from, object to, Expression<Func<TEntity, object>> btwField)
        {
            return FindAllBetween(from, to, btwField, transaction: null);
        }


        public IEnumerable<TEntity> FindAllBetween(DateTime from, DateTime to,
            Expression<Func<TEntity, object>> btwField)
        {
            return FindAllBetween(from, to, btwField, transaction: null);
        }


        public IEnumerable<TEntity> FindAllBetween(DateTime from, DateTime to,
            Expression<Func<TEntity, object>> btwField, Expression<Func<TEntity, bool>> predicate)
        {
            return FindAllBetween(from, to, btwField, predicate, null);
        }


        public IEnumerable<TEntity> FindAllBetween(object from, object to, Expression<Func<TEntity, object>> btwField,
            Expression<Func<TEntity, bool>> predicate)
        {
            return FindAllBetween(from, to, btwField, predicate, null);
        }


        public Task<IEnumerable<TEntity>> FindAllBetweenAsync(object from, object to,
            Expression<Func<TEntity, object>> btwField)
        {
            return FindAllBetweenAsync(from, to, btwField, transaction: null);
        }


        public Task<IEnumerable<TEntity>> FindAllBetweenAsync(DateTime from, DateTime to,
            Expression<Func<TEntity, object>> btwField)
        {
            return FindAllBetweenAsync(from, to, btwField, transaction: null);
        }


        public Task<IEnumerable<TEntity>> FindAllBetweenAsync(DateTime from, DateTime to,
            Expression<Func<TEntity, object>> btwField, Expression<Func<TEntity, bool>> predicate)
        {
            return FindAllBetweenAsync(from, to, btwField, predicate, null);
        }


        public Task<IEnumerable<TEntity>> FindAllBetweenAsync(object from, object to,
            Expression<Func<TEntity, object>> btwField, Expression<Func<TEntity, bool>> predicate)
        {
            return FindAllBetweenAsync(from, to, btwField, predicate, null);
        }
    }
}