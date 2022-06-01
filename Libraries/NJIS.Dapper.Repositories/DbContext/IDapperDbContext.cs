// ************************************************************************************
//  解决方案：NJIS.FPZWS.LineControl.Drilling
//  项目名称：NJIS.Dapper.Repositories
//  文 件 名：IDapperDbContext.cs
//  创建时间：2019-08-30 9:47
//  作    者：
//  说    明：
//  修改时间：2018-11-21 11:51
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System;
using System.Data;

#endregion

namespace NJIS.Dapper.Repositories.DbContext
{
    /// <summary>
    ///     Class is helper for use and close IDbConnection
    /// </summary>
    public interface IDapperDbContext : IDisposable
    {
        /// <summary>
        ///     Get opened DB Connection
        /// </summary>
        IDbConnection Connection { get; }

        /// <summary>
        ///     Open DB connection
        /// </summary>
        void OpenConnection();

        /// <summary>
        ///     Open DB connection and Begin transaction
        /// </summary>
        IDbTransaction BeginTransaction();
    }
}