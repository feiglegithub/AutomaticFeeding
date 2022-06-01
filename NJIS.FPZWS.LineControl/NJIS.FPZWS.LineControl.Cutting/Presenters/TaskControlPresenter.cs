using NJIS.FPZWS.LineControl.Cutting.Contract;
using NJIS.FPZWS.UI.Common.Message;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NJIS.FPZWS.LineControl.Cutting.Message;
using NJIS.FPZWS.Wcf.Client;
using NJIS.FPZWS.LineControl.Cutting.Model;
using NJIS.FPZWS.LineControl.Cutting.Service;
using NJIS.FPZWS.LineControl.Cutting.UI.Views.Controls;
using NJIS.FPZWS.LineControl.Cutting.UI.Views.Controls.ModuleControl;

namespace NJIS.FPZWS.LineControl.Cutting.UI.Presenters
{
    public class TaskControlPresenter: PresenterBase<TaskControlPresenter>
    {
        /// <summary>
        /// 获取堆垛信息 -- 接收信息类型：DataTime 生产时间
        /// </summary>
        public const string GetStackLists = nameof(GetStackLists);

        public const string GetMdbFile = nameof(GetMdbFile);

        public const string ConvertToSaw = nameof(ConvertToSaw);

        public const string GetDeviceName = nameof(GetDeviceName);
        public const string TranSpondItemName = nameof(TranSpondItemName);

        private const string CheckSawConvertResult = nameof(CheckSawConvertResult);

        private List<SpiltMDBResult> _downLoadMdbResults = null;

        private List<SpiltMDBResult> _currTaskList = null;

        private string _mdbDownLoadPath = Path.Combine(Directory.GetCurrentDirectory(), "DownLoadMDBs");

        private ILineControlCuttingContract _lineControlCuttingContract = null;
        private ILineControlCuttingContract LineControlCuttingContract => _lineControlCuttingContract??(_lineControlCuttingContract = CuttingSetting.Current.IsWcf ? WcfClient.GetProxy<ILineControlCuttingContract>(): new LineControlCuttingService()); 

        private IFileContract _fileContract = null;
        private IFileContract FileContract => _fileContract ?? (_fileContract = CuttingSetting.Current.IsWcf ? WcfClient.GetProxy<IFileContract>(): new FileService());

        private void Register()
        {
            Register<DateTime>(GetStackLists, ExecuteGetStackLists);
            Register<List<SpiltMDBResult>>(GetMdbFile, ExecuteGetMdbFile);
            Register<string>(GetDeviceName, ExecuteGetDeviceName);
            Register<string>(ConvertToSaw, ExecuteConvertToSaw);
            Register<List<SpiltMDBResult>>(CheckSawConvertResult, ExecuteCheckSawConvertResult);

            //Register<List<AssigningTaskArgs>>(EmqttSettings.Current.PcsDownMdbRep,list =>
            //{
            //    var tasks = list.Find(item => item.DeviceName == CuttingSetting.Current.CurrDeviceName);
            //    if (tasks != null)
            //    {
            //        ExecuteGetMdbFile(tasks.TaskList);
            //        SendTipsMessage($"{tasks.PlanDate:yyyy-MM-dd}有{tasks.TaskList.Count}个任务更新，请手动刷新查看最新生产任务");
            //    }
            //}, EExecutionMode.Asynchronization, true);
            //Register<List<PushTaskArgs>>(EmqttSettings.Current.PcsTaskRep,list =>
            //{
            //    var tasks = list.Find(item => item.DeviceName == CuttingSetting.Current.CurrDeviceName);
            //    if (tasks != null)
            //    {
            //        SendTipsMessage("收到一条推送任务");
            //        ExecuteConvertToSaw(tasks.PushTask.ItemName);
            //    }
            //}, EExecutionMode.Asynchronization, true);
        }


        public TaskControlPresenter()
        {
            Register(); 
        }

        private void ExecuteGetDeviceName(object sender,string strDisableMessage = "")
        {
            var recipient = sender;
            Send(TaskControl.ReceiveDeviceId, recipient, CuttingSetting.Current.CurrDeviceName);
        }

        private void ExecuteCheckSawConvertResult(List<SpiltMDBResult> spiltMdbResults)
        {
            do
            {
                for (int i = 0; i < spiltMdbResults.Count;)
                {
                    var result = spiltMdbResults[i];
                    string sawPath = Path.Combine(_mdbDownLoadPath, CuttingSetting.Current.SawPath,
                        result.ItemName + ".saw");
                    if (File.Exists(sawPath))
                    {
                        result.FinishedStatus = Convert.ToInt32(FinishedStatus.CreatedSaw);
                        LineControlCuttingContract.BulkUpdateFinishedStatus(new List<SpiltMDBResult>() {result});
                        spiltMdbResults.RemoveAt(i);
                        SendSelf(GetStackLists, result.PlanDate);
                        continue;
                    }

                    i++;
                }

                Thread.Sleep(1000);
            } while (spiltMdbResults.Count > 0);
        }

        private void ExecuteConvertToSaw(/*object sender,*/ string mdbName)
        {
            //object recipient = sender;
            var result = _currTaskList.Find(item => item.ItemName == mdbName);
            CopyMdb(result);
            result.FinishedStatus=Convert.ToInt32(FinishedStatus.CreatingSaw);
            var ret = LineControlCuttingContract.BulkUpdateFinishedStatus(new List<SpiltMDBResult>(){ result });
            Send(ItemNameControl.ConvertResult/*,recipient*/, mdbName);
            _currTaskList = LineControlCuttingContract.GetDeviceMDBResults(result.PlanDate, CuttingSetting.Current.CurrDeviceName);
            Send(TaskControl.ReceiveDatas, _currTaskList);
            SendSelf(CheckSawConvertResult,new List<SpiltMDBResult>(){result});

            #region Old Code
            //Thread th = new Thread(() =>
            //{
            //    string sawPath = Path.Combine(_mdbDownLoadPath, CuttingSetting.Current.SawPath, result.ItemName + ".saw");
            //    while (true)
            //    {
            //        if (File.Exists(sawPath))
            //        {
            //            result.FinishedStatus = Convert.ToInt32(FinishedStatus.CreatedSaw);
            //            WorkStationContract.BulkUpdateFinishedStatus(new List<SpiltMDBResult>() { result });
            //            _currTaskList = WorkStationContract.GetDeviceMDBResults(result.PlanDate, CuttingSetting.Current.CurrDeviceName);
            //            Send(TaskControl.ReceiveDatas, _currTaskList);
            //            break;
            //            ;
            //        }
            //        Thread.Sleep(1000);
            //    }
            //    Thread.CurrentThread.Abort();
            //});
            //th.IsBackground = true;
            //th.Start();
            #endregion


        }

        private void CopyMdb(SpiltMDBResult result)
        {
            string sourcePath = Path.Combine(_mdbDownLoadPath, result.PlanDate.ToString("yyyy-MM-dd"), result.BatchName, result.ItemName+ ".mdb");
            string destPath = Path.Combine(CuttingSetting.Current.TmpMDBPath, result.ItemName + ".mdb");
            File.Copy(sourcePath, destPath,true);
        }

        private async Task AsyncDownLoadMdb()
        {
            bool result = await Task.Run((() =>
            {
                var ret = DownLoadMdb();
                return ret;
            }));
        }

        private bool DownLoadMdb()
        {
            var memoryStreams = FileContract.DownLoadFile(_downLoadMdbResults);
            foreach (var item in memoryStreams)
            {
                SpiltMDBResult spiltMdbResult = item.Key;
                var ms = item.Value;
                byte[] sizeBytes = new byte[ms.Length];

                string mdbFullName = Path.Combine(_mdbDownLoadPath, spiltMdbResult.PlanDate.ToString("yyyy-MM-dd"), spiltMdbResult.BatchName, spiltMdbResult.ItemName + ".mdb");
                string path = Path.GetDirectoryName(mdbFullName);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                FileStream fs = new FileStream(mdbFullName, FileMode.Create, FileAccess.Write);

                int ret = ms.Read(sizeBytes, 0, sizeBytes.Length);
                if (ret > 0)
                {
                    fs.Write(sizeBytes, 0, ret);
                }
                fs.Flush();
                fs.Close();
            }
            return true;
           
        }

        private void ExecuteGetMdbFile(List<SpiltMDBResult> spiltMdbResults)
        {
            foreach (var item in spiltMdbResults)
            {
                item.DeviceName = CuttingSetting.Current.CurrDeviceName;
            }

            _downLoadMdbResults = spiltMdbResults;

            var task = AsyncDownLoadMdb();
            ExecuteGetStackLists(_downLoadMdbResults[0].PlanDate);
            task.Wait();
            var ex = task.Exception;
            if (ex != null)
            {
                SendTipsMessage("MDB文件下载失败："+ex.Message);
                return;
            }
            
            #region  Old Code

            //var memoryStreams = FileContract.DownLoadFile(spiltMdbResults);
            //foreach (var item in memoryStreams)
            //{
            //    string itemName = item.Key;
            //    var ms = item.Value;
            //    byte[] sizeBytes = new byte[ms.Length];

            //    string mdbFullName = Path.Combine(_mdbDownLoadPath, itemName + ".mdb");
            //    if (!Directory.Exists(_mdbDownLoadPath))
            //    {
            //        Directory.CreateDirectory(_mdbDownLoadPath);
            //    }
            //   FileStream fs = new FileStream(Path.Combine(_mdbDownLoadPath, itemName + ".mdb"), FileMode.Create, FileAccess.Write);

            //    int ret = ms.Read(sizeBytes, 0, sizeBytes.Length);
            //    if (ret > 0)
            //    {
            //        fs.Write(sizeBytes, 0, ret);
            //    }
            //    fs.Flush();
            //    fs.Close();
            //}


            #endregion

            foreach (var item in spiltMdbResults)
            {
                //item.FinishedStatus = Convert.ToInt32(FinishedStatus.MdbLoaded);
                item.MdbStatus = Convert.ToInt32(FinishedStatus.MdbLoaded);
            }
            //WorkStationContract.BulkUpdateFinishedStatus(spiltMdbResults);
            //WorkStationContract.BulkUpdateTaskAndMdbStatus(spiltMdbResults);
            LineControlCuttingContract.BulkUpdateMdbStatus(spiltMdbResults);

            string files =  string.Join("\n", _downLoadMdbResults.ToList().ConvertAll(item => item.ItemName));
            SendTipsMessage("文件下载完成:\n" + files);

            ExecuteGetStackLists(_downLoadMdbResults[0].PlanDate);
            //try
            //{
                
            //    //var resultList = WorkStationContract.GetDeviceMDBResults(_downLoadMdbResults[0].PlanDate, CuttingSetting.Current.CurrDeviceName);
            //    //Send(TaskControl.ReceiveDatas, resultList);
            //}
            //catch (Exception e)
            //{
            //    SendTipsMessage("获取数据失败：" + e.Message);
            //}
        }


        private void ExecuteGetStackLists(/*object sender,*/DateTime planTime)
        {
            //var recipient = sender;
            try
            {
                _currTaskList = LineControlCuttingContract.GetDeviceMDBResults(planTime, CuttingSetting.Current.CurrDeviceName);
                if (HasCreatingSawTask(_currTaskList))
                {
                    var tmpTasks = _currTaskList.FindAll(task =>
                        task.FinishedStatus == Convert.ToInt32(FinishedStatus.CreatingSaw));
                    SendSelf(CheckSawConvertResult, tmpTasks);
                }
                Send(TaskControl.ReceiveDatas, _currTaskList);
                string path = Path.Combine(_mdbDownLoadPath, planTime.ToString("yyyy-MM-dd"));
                if (Directory.Exists(path))
                {
                    var files = Directory.GetFiles(path, "*.mdb", SearchOption.AllDirectories).ToList();
                    for (int i = 0; i < files.Count; i++)
                    {
                        files[i] = Path.GetFileNameWithoutExtension(files[i]);
                    }
                    files.RemoveAll(item => !_currTaskList.Exists(reslut => reslut.ItemName == item));
                    var result = _currTaskList.FindAll(item => files.Contains(item.ItemName));
                    Send(ItemNamesControl.ReceiveFilesName, result);
                }
            }
            catch(Exception e)
            {
                SendTipsMessage("获取数据失败：" + e.Message/*, recipient*/);
            }

        }

        /// <summary>
        /// 检查是否存在正在转换的saw的任务
        /// </summary>
        /// <param name="taskList"></param>
        /// <returns></returns>
        private bool HasCreatingSawTask(List<SpiltMDBResult> taskList)
        {
            return taskList.Exists(task => task.FinishedStatus == Convert.ToInt32(FinishedStatus.CreatingSaw));
        }

        //private AccessDB GetAccessDB(string accessFileName)
        //{
        //    if(AccessFileInfo==null)
        //    {
        //        AccessFileInfo = new FileInfo(accessFileName);
        //        accessDB = new AccessDB(accessFileName);
        //    }
        //    else
        //    {
        //        FileInfo fileinfo = new FileInfo(accessFileName);
        //        if (AccessFileInfo.FullName!= fileinfo.FullName)
        //        {
        //            AccessFileInfo = fileinfo;
        //            accessDB = new AccessDB(accessFileName);
        //        }
        //    }

        //    return accessDB;
        //}

        //public void ExecuteGetDatas(string accessFileName)
        //{
        //    var accessDB = GetAccessDB(accessFileName);
        //    var ds = accessDB.GetDatas();
        //    Send(SpiltForm.ReceiveDatas, ds);
            
        //}
    }
}
