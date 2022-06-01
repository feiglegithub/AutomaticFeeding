using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using NJIS.FPZWS.LineControl.Cutting.Contract;
using NJIS.FPZWS.LineControl.Cutting.Model;
using NJIS.FPZWS.LineControl.Cutting.Service;
using NJIS.FPZWS.LineControl.Cutting.UI.Views;
using NJIS.FPZWS.UI.Common.Message;
using NJIS.FPZWS.Wcf.Client;
using Telerik.WinControls.UI;

namespace NJIS.FPZWS.LineControl.Cutting.UI.LocalServices
{
    /// <summary>
    /// 监听服务
    /// </summary>
    public class ListenService : ILocalService
    {

        private static ListenService _instance = null;
        private static readonly object ObjLock = new object();
        public static ILocalService Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (ObjLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new ListenService();
                        }
                    }
                }
                return _instance;
            }
        }
        private ListenService() { }
        private IFileContract _fileContract = null;
        private IFileContract FileContract => _fileContract ?? (_fileContract = CuttingSetting.Current.IsWcf ? WcfClient.GetProxy<IFileContract>() : new FileService());
        private ILineControlCuttingContract _lineControlCuttingContract = null;
        private ILineControlCuttingContract LineControlCuttingContract =>
            _lineControlCuttingContract ??
            (_lineControlCuttingContract = CuttingSetting.Current.IsWcf ? WcfClient.GetProxy<ILineControlCuttingContract>() : new LineControlCuttingService());
        private string _mdbDownLoadPath = Path.Combine(Directory.GetCurrentDirectory(), "DownLoadMDBs");
        private readonly List<Thread> _threads = new List<Thread>();
        private List<SpiltMDBResult> _convertingSpiltMdbResults = null;// = new List<SpiltMDBResult>();
        private readonly static object lockobj = new object();

        public void Start()
        {
            //MqttMessage.SubscribeMqttMessage();
            Thread th = new Thread(CheckUnloadMdb);
            th.IsBackground = true;
            _threads.Add(th);
            th = new Thread(CheckConverted);
            th.IsBackground = true;
            _threads.Add(th);
            //CheckUnloadMdb();
            ServiceInit();
            foreach (var thread in _threads)
            {
                thread.Start();
            }
        }

        public void Stop()
        {
            //MqttMessage.UnSubscribeMqttMessage();
            foreach (var thread in _threads)
            {
                thread.Abort();
            }
        }

        private void ServiceInit()
        {
            //Thread th = new Thread(() =>
            //{
            //    while (true)
            //    {
            //        Thread.Sleep(2000);
            //    }
            //})
            //{
            //    IsBackground = true
            //};
            //_threads.Add(th);
        }

        #region old code



        private void AddConvertingSaw(SpiltMDBResult spiltMdbResult)
        {
            lock (lockobj)
            {
                if (!_convertingSpiltMdbResults.Exists(item => item.ItemName == spiltMdbResult.ItemName))
                {
                    _convertingSpiltMdbResults.Add(spiltMdbResult);
                }
            }
        }

        private void RemoveConvertingSaw(SpiltMDBResult spiltMdbResult)
        {
            lock (lockobj)
            {
                if (!_convertingSpiltMdbResults.Exists(item => item.ItemName == spiltMdbResult.ItemName))
                {
                    _convertingSpiltMdbResults.RemoveAll(item => item.ItemName == spiltMdbResult.ItemName);
                }
            }
        }
        #endregion



        #region old code

        //private void CheckNeedToConvert()
        //{
        //    var results = LineControlCuttingContract.GetNeedToConvertTasks();
        //    var tmp = results.FindAll(item => item.DeviceName == CuttingSetting.Current.CurrDeviceName);
        //    foreach (var spiltMdbResult in tmp)
        //    {
        //        CopyMdb(spiltMdbResult);
        //        spiltMdbResult.FinishedStatus = Convert.ToInt32(FinishedStatus.CreatingSaw);
        //        LineControlCuttingContract.BulkUpdateFinishedStatus(new List<SpiltMDBResult> { spiltMdbResult });
        //        AddConvertingSaw(spiltMdbResult);
        //    }
        //}

        //private void CopyMdb(SpiltMDBResult result)
        //{
        //    string sourcePath = Path.Combine(_mdbDownLoadPath, result.PlanDate.ToString("yyyy-MM-dd"), result.BatchName, result.ItemName + ".mdb");
        //    string destPath = Path.Combine(CuttingSetting.Current.TmpMDBPath, result.ItemName + ".mdb");
        //    File.Copy(sourcePath, destPath, true);
        //}

        #endregion


        #region old code

        private void CheckConverted()
        {
            while (true)
            {
                try
                {
                    var results = LineControlCuttingContract.GetDeviceTopUnDownLoad(2, CuttingSetting.Current.CurrDeviceName);
                    _convertingSpiltMdbResults = results.FindAll(item => item.FinishedStatus == Convert.ToInt32(FinishedStatus.CreatingSaw)).ToList();


                    for (int i = 0; i < _convertingSpiltMdbResults.Count;)
                    {
                        var spiltMdbResult = _convertingSpiltMdbResults[i];
                        if (!CheckConverted(spiltMdbResult))
                        {
                            i++;
                            continue;
                        }

                        spiltMdbResult.FinishedStatus = Convert.ToInt32(FinishedStatus.CreatedSaw);
                        LineControlCuttingContract.BulkUpdateFinishedStatus(new List<SpiltMDBResult> { spiltMdbResult });
                        _convertingSpiltMdbResults.RemoveAt(i);
                        //RemoveConvertingSaw(spiltMdbResult);
                    }
                    Thread.Sleep(10000);
                }
                catch (Exception e)
                {
                    BroadcastMessage.Send(MainForm.ErrorMessage, e);
                    //Console.WriteLine(e);
                    //throw;
                }

            }
        }

        private bool CheckConverted(SpiltMDBResult spiltMdbResult)
        {

            return File.Exists(Path.Combine(CuttingSetting.Current.SawPath, spiltMdbResult.ItemName + ".saw"));
            //此处需要根据mdb转换后的saw文件来编写
            //string searchPattern = string.IsNullOrWhiteSpace(CuttingSetting.Current.SearchPattern)
            //    ? (spiltMdbResult.ItemName + ".saw")
            //    : CuttingSetting.Current.SearchPattern;
            //var files = Directory.GetFiles(CuttingSetting.Current.SawPath, searchPattern);
            //return files.Length > 0;
        }
        #endregion


        private void CheckUnloadMdb()
        {

            while (true)
            {
                try
                {
                    var results = LineControlCuttingContract.GetDeviceTopUnDownLoad(2
                        , CuttingSetting.Current.CurrDeviceName);
                    //var unloadMdbs = results.FindAll(item => item.FinishedStatus == Convert.ToInt32(FinishedStatus.MdbUnloaded));
                    var unloadMdbs = results.FindAll(item => (item.MdbStatus == Convert.ToInt32(FinishedStatus.MdbUnloaded)
                                                              || item.MdbStatus == Convert.ToInt32(FinishedStatus.MdbLoading))
                                                             && item.FinishedStatus == Convert.ToInt32(FinishedStatus.NeedToSaw));

                    //var unCopys = results.FindAll(item => item.FinishedStatus == Convert.ToInt32(FinishedStatus.NeedToSaw));
                    if (unloadMdbs.Count > 0)
                    {
                        if (!DownLoadMdb(unloadMdbs)) continue;
                        unloadMdbs.ForEach(item => item.MdbStatus = Convert.ToInt32(FinishedStatus.MdbLoaded));
                        LineControlCuttingContract.BulkUpdateMdbStatus(unloadMdbs);
                        //WorkStationContract.BulkUpdateTaskAndMdbStatus(unloadMdbs);
                    }
                    var unCopys = results.FindAll(item => item.FinishedStatus == Convert.ToInt32(FinishedStatus.NeedToSaw));
                    if (unCopys.Count > 0)
                    {
                        if (CopyMdb(unCopys))
                        {
                            unCopys.ForEach(item => item.FinishedStatus = Convert.ToInt32(FinishedStatus.CreatingSaw));
                            LineControlCuttingContract.BulkUpdateFinishedStatus(unCopys);
                        }
                    }

                }
                catch (Exception e)
                {
                    BroadcastMessage.Send(MainForm.ErrorMessage, e);
                    Thread.Sleep(10000);
                }
                Thread.Sleep(10000);

            }



        }

        private bool CopyMdb(List<SpiltMDBResult> unloads)
        {
            try
            {
                foreach (var item in unloads)
                {
                    string sourcePath = Path.Combine(_mdbDownLoadPath, item.PlanDate.ToString("yyyy-MM-dd"), item.BatchName, item.ItemName + ".mdb");
                    //string destPath = Path.Combine(CuttingSetting.Current.SawPath, item.ItemName + ".mdb");
                    string destPath = Path.Combine(CuttingSetting.Current.TmpMDBPath, item.ItemName + ".mdb");
                    File.Copy(sourcePath, destPath, true);
                }

                return true;
            }
            catch (Exception e)
            {
                BroadcastMessage.Send(MainForm.ErrorMessage, e);
                return false;
            }


        }

        private bool DownLoadMdb(List<SpiltMDBResult> downLoadMdbResults)
        {
            var memoryStreams = FileContract.DownLoadFile(downLoadMdbResults);
            foreach (var item in memoryStreams)
            {
                SpiltMDBResult spiltMdbResult = item.Key;
                var ms = item.Value;
                byte[] sizeBytes = new byte[ms.Length];

                string mdbFullName = Path.Combine(_mdbDownLoadPath, spiltMdbResult.PlanDate.ToString("yyyy-MM-dd"),
                    spiltMdbResult.BatchName, spiltMdbResult.ItemName + ".mdb");
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
    }
}
