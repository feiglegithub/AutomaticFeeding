using Contract;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using WcsModel;

namespace WcsService
{
    public class MachineHandTaskLogService:ServiceBase<MachineHandTaskLogService>, IMachineHandTaskLogContract
    {
        private MachineHandTaskLogService() { }
        public ExecuteResult BeginMachineHandTask(long id)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                string store = "BeginMachineHandTask";
                var param = new { Id = id, Result = -1, Msg = "" };
                var para = new DynamicParameters();
                para.Add("@" + nameof(param.Id), param.Id);

                para.Add("@" + nameof(param.Result), param.Result, DbType.Int32,
                    ParameterDirection.Output);
                para.Add("@" + nameof(param.Msg), param.Msg, DbType.String, ParameterDirection.Output);
                con.Execute(store, para, null, null,
                    CommandType.StoredProcedure);

                var result = para.Get<int>(nameof(param.Result)) == 0;
                var msg = para.Get<string>(nameof(param.Msg));
                return new ExecuteResult() { Result = result, Msg = msg };
            }
        }

        public ExecuteResult FinishedMachineHandTask(long id)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                string store = "FinishedMachineHandTask";
                var param = new { Id = id, Result = -1, Msg = "" };
                var para = new DynamicParameters();
                para.Add("@" + nameof(param.Id), param.Id);

                para.Add("@" + nameof(param.Result), param.Result, DbType.Int32,
                    ParameterDirection.Output);
                para.Add("@" + nameof(param.Msg), param.Msg, DbType.String, ParameterDirection.Output);
                con.Execute(store, para, null, null,
                    CommandType.StoredProcedure);

                var result = para.Get<int>(nameof(param.Result)) == 0;
                var msg = para.Get<string>(nameof(param.Msg));
                return new ExecuteResult(){Result = result,Msg = msg};
            }

        }

        public List<MachineHandTaskLog> GetMachineHandTaskLogs()
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                
                string store = "GetMachineHandTask";
                return QueryList<MachineHandTaskLog>(con,store);
            }
            //return con.Query<MachineHandTaskLog>(store, null, null, true, null, CommandType.StoredProcedure)
            //    .ToList();
        }

        public ExecuteResult MachineHandCanBegin(long id)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                string store = "MachineHandTaskCanBegin";
                var param = new { Id = id, Result = -1, Msg = "" };
                var para = new DynamicParameters();
                para.Add("@" + nameof(param.Id), param.Id);

                para.Add("@" + nameof(param.Result), param.Result, DbType.Int32,
                    ParameterDirection.Output);
                para.Add("@" + nameof(param.Msg), param.Msg, DbType.String, ParameterDirection.Output);
                con.Execute(store, para, null, null,
                    CommandType.StoredProcedure);

                var result = para.Get<int>(nameof(param.Result)) == 0;
                var msg = para.Get<string>(nameof(param.Msg));
                return new ExecuteResult() { Result = result, Msg = msg };
            }
        }

        public bool UpdatedMachineHand(MachineHandTaskLog machineHandTaskLog)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                string updatedSql = string.Format("UPDATE [dbo].[{0}] SET [{1}]=@{1} WHERE [{2}]=@{2}",
                    nameof(MachineHandTaskLog), nameof(machineHandTaskLog.Status), nameof(machineHandTaskLog.Id));
                con.Execute(updatedSql, machineHandTaskLog);
                return true;
            }

        }
    }
}
