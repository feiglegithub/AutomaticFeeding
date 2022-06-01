using System;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.Data.OleDb;
using System.Collections;

namespace WCS
{
    public abstract class OracleHelper
    {
        public static readonly string ConnectionStringLocalTransaction = "Data Source=topprod;User Id=wms01;Password=wms01;";
        //Data Source=sky;user=system;password=manager
        //"Data Source=TROPPED;Persist Security Info=True;User ID=wms01;Password=wms01;Unicode=True";
        //System.Configuration.ConfigurationSettings.AppSettings["OrclConn"].ToString();
        //public static readonly string WareHouseID = System.Configuration.ConfigurationSettings.AppSettings["WareHouseID"].ToString();
        public static OracleConnection conn = new OracleConnection(ConnectionStringLocalTransaction);

        private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());

        public static int ExecuteNonQuery(CommandType cmdType, string cmdText, params OracleParameter[] commandParameters)
        {
            OracleCommand cmd = new OracleCommand();
            using (OracleConnection connection = new OracleConnection(ConnectionStringLocalTransaction))
            {
                PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
        }

        public static OracleDataReader ExecuteReader(CommandType cmdType, string cmdText, params OracleParameter[] commandParameters)
        {
            OracleCommand cmd = new OracleCommand();
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                OracleDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;
            }
            catch
            {
                conn.Close();
                return null;
            }
        }

        public static DataSet ExecuteDataSet(CommandType cmdType, string cmdText, params OracleParameter[] commandParameters)
        {
            OracleCommand cmd = new OracleCommand();
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                OracleDataAdapter oda = new OracleDataAdapter(cmd);
                DataSet ds = new DataSet();
                oda.Fill(ds);
                cmd.Parameters.Clear();
                return ds;
            }
            catch (Exception ex)
            {
                //LogWrite.WriteLogToMain(string.Format("接口程序从WMS导入任务到WCS失败，错误信息{0}", ex.Message.ToString()));
                conn.Close();
                return null;
            }
        }

        public static string[] ExecuteProcedure(CommandType cmdType, string cmdText, params OracleParameter[] commandParameters)
        {
            OracleCommand cmd = new OracleCommand();
            string[] returnv = new string[commandParameters.Length];
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                cmd.ExecuteNonQuery();
                for (int i = 0; i < returnv.Length; i++)
                {
                    returnv[i] = cmd.Parameters[i].Value.ToString();
                }
                return returnv;
            }
            catch
            {
                conn.Close();
                return returnv;
            }
        }

        public static object ExecuteScalar(CommandType cmdType, string cmdText, params OracleParameter[] commandParameters)
        {
            OracleCommand cmd = new OracleCommand();

            using (OracleConnection conn = new OracleConnection(ConnectionStringLocalTransaction))
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }
        }

        public static void CacheParameters(string cacheKey, params OracleParameter[] commandParameters)
        {
            parmCache[cacheKey] = commandParameters;
        }

        public static OracleParameter[] GetCachedParameters(string cacheKey)
        {
            OracleParameter[] cachedParms = (OracleParameter[])parmCache[cacheKey];

            if (cachedParms == null)
                return null;
            OracleParameter[] clonedParms = new OracleParameter[cachedParms.Length];
            for (int i = 0, j = cachedParms.Length; i < j; i++)
                clonedParms[i] = (OracleParameter)((ICloneable)cachedParms[i]).Clone();

            return clonedParms;
        }

        private static void PrepareCommand(OracleCommand cmd, OracleConnection conn, OracleTransaction trans, CommandType cmdType, string cmdText, OracleParameter[] commandParameters)
        {
            
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                cmd.Connection = conn;
                cmd.CommandText = cmdText;
                cmd.CommandType = cmdType;

                if (trans != null)
                    cmd.Transaction = trans;
                if (commandParameters != null)
                {
                    foreach (OracleParameter parm in commandParameters)
                        cmd.Parameters.Add(parm);
                }
        }


        public static object GetObjValueBySql(string sSql)
        {
            object objX = null;
            OracleConnection conn = new OracleConnection(ConnectionStringLocalTransaction);
            try
            {
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = sSql;
                conn.Open();
                objX = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                string sX = ex.Message;
                return null;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
            return objX;
        }

       

    }
}
