// ************************************************************************************
//  解决方案：NJIS.FPZWS.LineControl.Drilling
//  项目名称：NJIS.Dapper.Repositories
//  文 件 名：DapperRepository.cs
//  创建时间：2019-08-30 9:48
//  作    者：
//  说    明：
//  修改时间：2018-11-21 11:51
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Dapper;
using NJIS.Dapper.Repositories.SqlGenerator;

#endregion

namespace NJIS.Dapper.Repositories
{
    /// <summary>
    ///     Base Repository
    /// </summary>
    public partial class DapperRepository<TEntity> : IDapperRepository<TEntity> where TEntity : class
    {
        private const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        ///     Constructor
        /// </summary>
        public DapperRepository(IDbConnection connection)
        {
            Connection = connection;
            SqlGenerator = new SqlGenerator<TEntity>(SqlProvider.MSSQL);
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        public DapperRepository(IDbConnection connection, SqlProvider sqlProvider)
        {
            Connection = connection;
            SqlGenerator = new SqlGenerator<TEntity>(sqlProvider);
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        public DapperRepository(IDbConnection connection, ISqlGenerator<TEntity> sqlGenerator)
        {
            Connection = connection;
            SqlGenerator = sqlGenerator;
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        public DapperRepository(IDbConnection connection, SqlGeneratorConfig config)
        {
            Connection = connection;
            SqlGenerator = new SqlGenerator<TEntity>(config);
        }


        public IDbConnection Connection { get; }


        public ISqlGenerator<TEntity> SqlGenerator { get; }

        //-------------------扩展方法------------------------------------
        /// <summary>
        ///     执行存储过程
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> QueryProcedure(string procName, object param)
        {
            return Connection.Query<TEntity>(procName, param, null, true, 7200, CommandType.StoredProcedure);
        }

        public Task<IEnumerable<TEntity>> QueryProcedureAsync(string procName, object param)
        {
            return Task.FromResult(QueryProcedure(procName, param));
        }

        public bool ExecuteProcedure(string procName, object param)
        {
            return Connection.Execute(procName, param, null, 7200, CommandType.StoredProcedure) > 0;
        }

        public Task<bool> ExecuteProcedureAsync(string procName, object param)
        {
            return Task.FromResult(ExecuteProcedure(procName, param));
        }

        public IEnumerable<TEntity> QueryList(string sql, object param)
        {
            return Connection.Query<TEntity>(sql, param, null, true, 7200, CommandType.Text);
        }

        public Task<IEnumerable<TEntity>> QueryListAsync(string sql, object param)
        {
            return Task.FromResult(QueryList(sql, param));
        }

        public TEntity QueryFirst(string sql, object param)
        {
            return Connection.Query<TEntity>(sql, param, null, true, 7200, CommandType.Text).FirstOrDefault();
        }

        public Task<TEntity> QueryFirstAsync(string sql, object param)
        {
            return Task.FromResult(QueryFirst(sql, param));
        }

        public bool ExecuteSql(string sql, object param)
        {
            return Connection.Execute(sql, param, null, 7200, CommandType.Text) > 0;
        }

        public Task<int> ExecuteSqlAsync(string sql, object param)
        {
            return Connection.ExecuteAsync(sql, param, null, 7200, CommandType.Text);
        }

        public bool FindAny(Expression<Func<TEntity, bool>> predicate, IDbTransaction transaction = null)
        {
            var queryResult = SqlGenerator.GetSelectFirst(predicate);
            var obj = Connection
                .Query<TEntity>(queryResult.GetSql(), queryResult.Param, transaction, true, 7200, CommandType.Text)
                .FirstOrDefault();
            return obj != null;
        }

        public bool FindAny(string sql, object param, IDbTransaction transaction = null)
        {
            var count = Connection.Query<int>(sql, param, null, true, 7200, CommandType.Text).FirstOrDefault();
            return count > 0;
        }

        public object ExecuteScalar(string sql, object param = null, IDbTransaction transaction = null)
        {
            return Connection.ExecuteScalar(sql, param, transaction);
        }

        public Task<object> ExecuteScalarAsync(string sql, object param = null, IDbTransaction transaction = null)
        {
            return Connection.ExecuteScalarAsync(sql, param, transaction);
        }

        public virtual IEnumerable<TEntity> FindAll2(IDbTransaction transaction = null)
        {
            return FindAll2(null, transaction);
        }


        public virtual IEnumerable<TEntity> FindAll2(Expression<Func<TEntity, bool>> predicate,
            IDbTransaction transaction = null)
        {
            var queryResult = SqlGenerator.GetSelectAll(predicate);
            return Connection.Query<TEntity>(queryResult.GetSql(), queryResult.Param, transaction);
        }


        public virtual Task<IEnumerable<TEntity>> FindAllAsync2(IDbTransaction transaction = null)
        {
            return FindAllAsync2(null, transaction);
        }


        public virtual Task<IEnumerable<TEntity>> FindAllAsync2(Expression<Func<TEntity, bool>> predicate,
            IDbTransaction transaction = null)
        {
            var queryResult = SqlGenerator.GetSelectAll(predicate);
            return Connection.QueryAsync<TEntity>(queryResult.GetSql(), queryResult.Param, transaction);
        }

        public virtual IEnumerable<TEntity> FindAllBySQL(string sql)
        {
            return Connection.Query<TEntity>(sql);
        }
    }
}