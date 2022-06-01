using NJIS.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using WcsConfig;

namespace WcsService
{
    public class ServiceBase
    {
        protected IDbConnection connection { get; set; }

        private string ConnectionString { get; set; } = WcsSettings.Current.DbConnectString;

        public ServiceBase()
        {
            connection = new SqlConnection(ConnectionString);
        }
    }

    public class ServiceBase<T>: Singleton<T>
    where T:class
    {
        //private  IDbConnection _connection = null;
        //protected IDbConnection connection { get; set; }
        // = new SqlConnection(ConnectionString);

        protected static string ConnectionString { get; } = WcsSettings.Current.DbConnectString;

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// /// <param name="connection"></param>
        /// <param name="store"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public List<T> QueryList<T>(IDbConnection connection ,string store, object para = null)
        {
            return connection.Query<T>(store, para, null, true, null, CommandType.StoredProcedure).ToList();
        }

       
    }

   
}
