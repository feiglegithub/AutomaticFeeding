using NJIS.FPZWS.LineControl.PartMill.Contract;
using NJIS.FPZWS.LineControl.PartMill.Model;
using NJIS.FPZWS.LineControl.PartMill.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace NJIS.FPZWS.LineControl.PartMill.Service
{
    public class PartMillService : IContract
    {
        public List<line_config> GetLineConfigs()
        {
            var rep = new line_configRepository();
            return rep.FindAll(item => item.id > 0).ToList();
        }

        public List<read_labelfile_config> GetReadLabelFileConfigs()
        {
            var rep = new read_labelfile_configRepository();
            return rep.FindAll(item => item.is_enable).ToList();
        }

        public bool SaveUpFileInfo(List<object> objects)
        {
            if (objects == null || objects.Count == 0) return true;
            stack_list sl;
            file_info fi;
            var stackList = objects.FindAll(item => item.GetType() == typeof(stack_list));
            var fileInfos = objects.FindAll(item => item.GetType() == typeof(file_info));
            fileInfos.ForEach(file =>
            {
                if (file is file_info fileInfo)
                {
                    fileInfo.label_file_fullname = Path.Combine(@"Files", fileInfo.label_name);
                    fileInfo.nc_file_fullname = Path.Combine(@"Files", fileInfo.nc_name);
                }
            });
            var stackListInsertSql =
                GetInsertSql<stack_list>(nameof(sl.id), nameof(sl.created_time), nameof(sl.updated_time),nameof(sl.stack_name));
            var fileInfosInsertSql =
                GetInsertSql<file_info>(nameof(fi.id), nameof(fi.created_time), nameof(fi.updated_time));
            int ret = 0;
            var rep = new line_configRepository();
            if (rep.Connection.State != ConnectionState.Open)
            {
                rep.Connection.Open();
            }
            var tran = rep.Connection.BeginTransaction();
            try
            {
                ret+=rep.Connection.Execute(stackListInsertSql, stackList);
                ret+=rep.Connection.Execute(fileInfosInsertSql, fileInfos);
                
            }
            catch (Exception e)
            {
                tran.Rollback();
                throw;
            }

            if (ret != objects.Count)
            {
                tran.Rollback();
                return false;
            }
            tran.Commit();

            return true;
        }

        public List<line_task> GetLineTasks()
        {
            string store = "get_unfinished_line_task";
            var rep= new line_taskRepository();
            //var list = rep.FindAll(item => item.task_status < 99);
            //var data = rep.Connection.ExecuteScalar(store, null, null, null, CommandType.StoredProcedure);
            var list = rep.Connection.Query<line_task>(store, null, null, true, null, CommandType.StoredProcedure);
            return list.ToList();
        }

        public List<mh_task> GetMhTasks()
        {
            string store = "get_unfinished_mh_task";
            var rep = new mh_taskRepository();
            var list = rep.Connection.Query<mh_task>(store, null, null, true, null, CommandType.StoredProcedure);
            return list.ToList();
        }

        public Tuple<bool, string> BeginLineTask(line_task lineTask)
        {
            string store = "begin_line_task";
            var param = new { id_arg = lineTask.id, result=-1,msg="" };
            var rep = new line_taskRepository();

            var para = new DynamicParameters();
            para.Add("@"+nameof(param.id_arg), param.id_arg);

            para.Add("@" + nameof(param.result), param.result, DbType.Int32,
                ParameterDirection.Output);
            para.Add("@" + nameof(param.msg), param.msg, DbType.String, ParameterDirection.Output);
            rep.Connection.Execute(store, para, null, null,
                CommandType.StoredProcedure);

            var result = para.Get<int>(nameof(param.result)) == 0;
            var msg = para.Get<string>(nameof(param.msg));
            return new Tuple<bool, string>(result,msg);

        }

        public Tuple<bool, string> BeginMhTask(mh_task mhTask)
        {
            string store = "begin_mh_task";
            var param = new { id_arg = mhTask.id, result = -1, msg = "" };
            var rep = new mh_taskRepository();

            var para = new DynamicParameters();
            para.Add("@" + nameof(param.id_arg), param.id_arg);

            para.Add("@" + nameof(param.result), param.result, DbType.Int32,
                ParameterDirection.Output);
            para.Add("@" + nameof(param.msg), param.msg, DbType.String, ParameterDirection.Output);
            rep.Connection.Execute(store, para, null, null,
                CommandType.StoredProcedure);

            var result = para.Get<int>("@" + nameof(param.result)) == 0;
            var msg = para.Get<string>("@" + nameof(param.msg));
            return new Tuple<bool, string>(result, msg);
        }

        public ResponseModel FinishedLineTask(int position)
        {
            string store = "finished_line_task";
            var param = new {position_arg=position, result = -1, msg = "", address = "", return_value = (short)0 };
            var rep = new line_taskRepository();
            var para = new DynamicParameters();
            para.Add("@" + nameof(param.position_arg), param.position_arg, DbType.Int32,
                ParameterDirection.Input);
            para.Add("@" + nameof(param.result), param.result, DbType.Int32,
                ParameterDirection.Output);
            para.Add("@" + nameof(param.msg), param.msg, DbType.String, ParameterDirection.Output);
            para.Add("@" + nameof(param.address), param.address, DbType.String, ParameterDirection.Output);
            para.Add("@" + nameof(param.return_value), param.return_value, DbType.Int16, ParameterDirection.Output);
            rep.Connection.Execute(store, para, null, null,
                CommandType.StoredProcedure);
            var result = para.Get<int>("@" + nameof(param.result)) == 0;
            var msg = para.Get<string>("@" + nameof(param.msg));
            var address = para.Get<string>("@" + nameof(param.address));
            var value = para.Get<short>("@" + nameof(param.return_value));
            return new ResponseModel() { Address = address,Msg = msg,Result = result,ReturnValue = value};
        }

        public ResponseModel LineAcceptTask()
        {
            string store = "line_accept_task";
            var param = new { result = -1, msg = "", address = "", return_value = (short)0 };
            var rep = new line_taskRepository();
            var para = new DynamicParameters();
            para.Add("@" + nameof(param.result), param.result, DbType.Int32,
                ParameterDirection.Output);
            para.Add("@" + nameof(param.msg), param.msg, DbType.String, ParameterDirection.Output);

            rep.Connection.Execute(store, para, null, null,
                CommandType.StoredProcedure);
            var result = para.Get<int>("@" + nameof(param.result)) == 0;
            var msg = para.Get<string>("@" + nameof(param.msg));
            return new ResponseModel() { Address = "", Msg = msg, Result = result, ReturnValue = 0 };
        }

        public ResponseModel FinishedMhTask()
        {
            string store = "finished_mh_task";
            var param = new {  result = -1, msg = "",address="",return_value=(short)0 };
            var rep = new mh_taskRepository();

            var para = new DynamicParameters();
            para.Add("@" + nameof(param.result), param.result, DbType.Int32,
                ParameterDirection.Output);
            para.Add("@" + nameof(param.msg), param.msg, DbType.String, ParameterDirection.Output);
            para.Add("@" + nameof(param.address), param.address, DbType.String, ParameterDirection.Output);
            para.Add("@" + nameof(param.return_value), param.return_value, DbType.Int16, ParameterDirection.Output);
            rep.Connection.Execute(store, para, null, null,
                CommandType.StoredProcedure);

            var result = para.Get<int>("@" + nameof(param.result)) == 0;
            var msg = para.Get<string>("@" + nameof(param.msg));
            var address = para.Get<string>("@" + nameof(param.address));
            var value = para.Get<short>("@" + nameof(param.return_value));
            return new ResponseModel(){Address = address,Msg = msg,Result = result,ReturnValue = value};
        }

        public ResponseModel CreatedBackPilerTask()
        {
            string store = "created_back_piler_task";
            var param = new { result = -1, msg = "", address = "", return_value = (short)0 };
            var rep = new mh_taskRepository();

            var para = new DynamicParameters();
            para.Add("@" + nameof(param.result), param.result, DbType.Int32,
                ParameterDirection.Output);
            para.Add("@" + nameof(param.msg), param.msg, DbType.String, ParameterDirection.Output);
            para.Add("@" + nameof(param.address), param.address, DbType.String, ParameterDirection.Output);
            para.Add("@" + nameof(param.return_value), param.return_value, DbType.Int16, ParameterDirection.Output);
            rep.Connection.Execute(store, para, null, null,
                CommandType.StoredProcedure);

            var result = para.Get<int>("@" + nameof(param.result)) == 0;
            var msg = para.Get<string>("@" + nameof(param.msg));
            var address = para.Get<string>("@" + nameof(param.address));
            var value = para.Get<short>("@" + nameof(param.return_value));
            return new ResponseModel() { Address = address, Msg = msg, Result = result, ReturnValue = value };
        }

        public string GetInsertSql<T>(params string[] noContainProperties)
        {
            var type = typeof(T);
            
            var propertyInfos = type.GetProperties().ToList();
            if (noContainProperties != null && noContainProperties.Length > 0)
            {
                propertyInfos.RemoveAll(item => noContainProperties.Contains(item.Name));
            }

            string tag = @"@";

            var fields = propertyInfos.ConvertAll(item => item.Name);
            var values = propertyInfos.ConvertAll(item => tag+item.Name);
            var fieldsPart = string.Join(",", fields);
            var valuesPart = string.Join(",", values);
            string insertSql = string.Format("insert INTO {0} ({1}) values ({2})", type.Name, fieldsPart, valuesPart);
            return insertSql+";";

        }
    }
}
