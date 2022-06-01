// ************************************************************************************
//  解决方案：NJIS.FPZWS.LineControl.Drilling
//  项目名称：NJIS.Dapper.Repositories
//  文 件 名：DapperDbContext.cs
//  创建时间：2019-08-30 9:47
//  作    者：
//  说    明：
//  修改时间：2018-11-21 11:51
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

#region

using System.Data;

#endregion

namespace NJIS.Dapper.Repositories.DbContext
{
    /// <summary>
    ///     Class is helper for use and close IDbConnection
    /// </summary>
    public class DapperDbContext : IDapperDbContext
    {
        /// <summary>
        ///     DB Connection for internal use
        /// </summary>
        protected readonly IDbConnection InnerConnection;

        /// <summary>
        ///     Constructor
        /// </summary>
        protected DapperDbContext(IDbConnection connection)
        {
            InnerConnection = connection;
        }

        /// <summary>
        ///     Get opened DB Connection
        /// </summary>
        public virtual IDbConnection Connection
        {
            get
            {
                OpenConnection();
                return InnerConnection;
            }
        }

        /// <summary>
        ///     Open DB connection
        /// </summary>
        public void OpenConnection()
        {
            if (InnerConnection.State != ConnectionState.Open && InnerConnection.State != ConnectionState.Connecting)
                InnerConnection.Open();
        }

        /// <summary>
        ///     Open DB connection and Begin transaction
        /// </summary>
        public virtual IDbTransaction BeginTransaction()
        {
            return Connection.BeginTransaction();
        }

        /// <summary>
        ///     Close DB connection
        /// </summary>
        public void Dispose()
        {
            if (InnerConnection != null && InnerConnection.State != ConnectionState.Closed)
                InnerConnection.Close();
        }
    }
}