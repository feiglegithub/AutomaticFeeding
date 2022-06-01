// ************************************************************************************
//  解决方案：NJIS.FPZWS.LineControl.Drilling
//  项目名称：NJIS.Dapper.Repositories
//  文 件 名：DapperRepository.FindById.cs
//  创建时间：2019-08-30 9:49
//  作    者：
//  说    明：
//  修改时间：2019-08-30 9:38
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

using System.Data;
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
        public virtual TEntity FindById(object id, IDbTransaction transaction)
        {
            var queryResult = SqlGenerator.GetSelectById(id);
            return Connection.QuerySingleOrDefault<TEntity>(queryResult.GetSql(), queryResult.Param, transaction);
        }


        public virtual Task<TEntity> FindByIdAsync(object id, IDbTransaction transaction)
        {
            var queryResult = SqlGenerator.GetSelectById(id);
            return Connection.QuerySingleOrDefaultAsync<TEntity>(queryResult.GetSql(), queryResult.Param, transaction);
        }

        public virtual TEntity FindById(object id)
        {
            return FindById(id, null);
        }


        public virtual Task<TEntity> FindByIdAsync(object id)
        {
            return FindByIdAsync(id, null);
        }
    }
}