using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using NJIS.FPZWS.LineControl.Cutting.ContractPlus;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;
using NJIS.FPZWS.LineControl.CuttingDevice.Views;
using NJIS.FPZWS.UI.Common.Message;
using NJIS.FPZWS.Wcf.Client;
using System.Configuration;
using NJIS.FPZWS.LineControl.CuttingDevice.Helpers;
using NJIS.FPZWS.LineControl.Cutting.ServicePlus;
using System.Text;
using NJIS.FPZWS.LineControl.CuttingDevice.Utils;

namespace NJIS.FPZWS.LineControl.CuttingDevice.LocalServices
{
    /// <summary>
    /// 监听服务
    /// </summary>
    public class ListenService : ILocalService
    {

        private static ListenService _instance = null;
        private static readonly object ObjLock = new object();
        PlcOperatorHelper plc = PlcOperatorHelper.GetInstance();
        LineControlCuttingServicePlus lineControlCuttingServicePlus = new LineControlCuttingServicePlus();
        int sleepTime = int.Parse(ConfigurationSettings.AppSettings["sleepTime"]);

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
        private IFileContract FileContract => _fileContract ?? (_fileContract =WcfClient.GetProxy<IFileContract>());
        private ILineControlCuttingContractPlus _Contract = null;
        private ILineControlCuttingContractPlus Contract =>
            _Contract ??(_Contract = WcfClient.GetProxy<ILineControlCuttingContractPlus>());
        private readonly string _mdbDownLoadPath = Path.Combine(Directory.GetCurrentDirectory(), "DownLoadMDBs");
        private readonly List<Thread> _threads = new List<Thread>();
        private List<Pattern> _convertingPatterns = null;
        private static readonly object Lockobj = new object();

        public void Start()
        {

            Thread th = new Thread(CheckStationRequestLoop);
            th.IsBackground = true;
            _threads.Add(th);

            Thread th2 = new Thread(CheckManualAllocationSawFileLoop);
            th2.IsBackground = true;
            _threads.Add(th2);

            //th = new Thread(CheckConverted);
            //th.IsBackground = true;
            //_threads.Add(th);
            //CheckUnloadMdb();
            ServiceInit();
            foreach (var thread in _threads)
            {
                thread.Start();
            }
        }

        public void Stop()
        {
            foreach (var thread in _threads)
            {
                thread.Abort();
            }
        }

        private void ServiceInit()
        {
        }




        #region old code

        private void CheckConverted()
        {
            while (true)
            {
                try
                {
                    var convertingPatterns = Contract.GetPatternsByDevice(CuttingSetting.Current.CurrDeviceName,
                        PatternStatus.ConvertingSaw);


                    for (int i = 0; i < convertingPatterns.Count;)
                    {
                        var pattern = convertingPatterns[i];
                        if (!CheckConverted(pattern))
                        {
                            i++;
                            continue;
                        }

                        pattern.Status = Convert.ToInt32(PatternStatus.Converted);
                        pattern.UpdatedTime = DateTime.Now;
                        Contract.BulkUpdatePatternStatus(new List<Pattern>() {pattern});
                        convertingPatterns.RemoveAt(i);
                        //RemoveConvertingSaw(spiltMdbResult);
                    }

                }
                catch (Exception e)
                {
                    BroadcastMessage.Send(MainForm.ErrorMessage, e);
                }
                finally
                {
                    Thread.Sleep(10000);
                }

            }
        }

        private bool CheckConverted(Pattern pattern)
        {

            return File.Exists(Path.Combine(CuttingSetting.Current.SawPath, pattern.MdbName + ".saw"));
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
                    var unloadMdbs = Contract.GetPatternsByDevice(CuttingSetting.Current.CurrDeviceName,PatternStatus.UndistributedButUnLoad);
                    unloadMdbs.AddRange(Contract.GetPatternsByDevice(CuttingSetting.Current.CurrDeviceName, PatternStatus.Loading));



                    if (unloadMdbs.Count > 0)
                    {
                        if (!DownLoadMdb(unloadMdbs)) continue;
                        unloadMdbs.ForEach(item =>
                        {
                            item.Status = Convert.ToInt32(PatternStatus.Loaded);
                            item.UpdatedTime = DateTime.Now;
                        });
                        Contract.BulkUpdatePatternStatus(unloadMdbs);
                        
                    }
                    var unCopys = Contract.GetPatternsByDevice(CuttingSetting.Current.CurrDeviceName, PatternStatus.UnConvertSaw);
                    if (unCopys.Count > 0)
                    {
                        if (CopyMdb(unCopys))
                        {
                            unCopys.ForEach(item =>
                            {
                                item.Status = Convert.ToInt32(PatternStatus.ConvertingSaw);
                                item.UpdatedTime = DateTime.Now;
                            });
                            Contract.BulkUpdatePatternStatus(unCopys);
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

        private bool CopyMdb(List<Pattern> unloads)
        {
            try
            {
                foreach (var item in unloads)
                {
                    string sourcePath = Path.Combine(_mdbDownLoadPath, item.PlanDate.ToString("yyyy-MM-dd"), item.BatchName, item.MdbName + ".mdb");
                    string destPath = Path.Combine(CuttingSetting.Current.TmpMDBPath, item.MdbName + ".mdb");
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

        private bool DownLoadMdb(List<Pattern> downLoadMdbs)
        {
            foreach (var pattern in downLoadMdbs)
            {
                var memoryStream = FileContract.DownLoadMdb(pattern);
                var ms = memoryStream;
                byte[] sizeBytes = new byte[ms.Length];

                string mdbFullName = Path.Combine(_mdbDownLoadPath, pattern.PlanDate.ToString("yyyy-MM-dd"),
                    pattern.BatchName, pattern.MdbName + ".mdb");
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

        /// <summary>
        /// 循环执行检查手动分配saw文件
        /// </summary>
        private void CheckManualAllocationSawFileLoop()
        {
            while (true)
            {
                //int sleepTime = int.Parse(ConfigurationSettings.AppSettings["sleepTime"]);

                //CheckStationRequest();
                try
                {
                    CheckManualAllocationSawFile();
                }
                catch (Exception)
                {
                    //预防服务器重启或者关机导致无法访问数据库而卡死
                }
                

                Thread.Sleep(sleepTime);
            }
        }

        /// <summary>
        /// 循环检查站台请求
        /// </summary>
        private void CheckStationRequestLoop()
        {
            while (true)
            {
                try
                {
                    CheckStationRequest();
                }
                catch (Exception)
                {

                    //预防服务器重启或者关机导致无法访问数据库而卡死
                }

                Thread.Sleep(sleepTime);
            }
        }

        /// <summary>
        /// 检查站台请求
        /// </summary>
        private void CheckStationRequest()
        {
            
                bool plcIsConnect = plc.CheckConnect();
                if (plcIsConnect)
                {
                    //地址块
                    string TriggerOutAddr = ConfigurationSettings.AppSettings["TriggerOutAddr"];
                    string MachineNoAddr = ConfigurationSettings.AppSettings["MachineNoAddr"];
                    string SawNoAddr = ConfigurationSettings.AppSettings["SawNoAddr"];
                    string SumCountAddr = ConfigurationSettings.AppSettings["SumCountAddr"];
                    string PLCResultAddr = ConfigurationSettings.AppSettings["PLCResultAddr"];

                    string TriggerInAddr = ConfigurationSettings.AppSettings["TriggerInAddr"];
                    string ResultAddr = ConfigurationSettings.AppSettings["ResultAddr"];

                    //值
                    int TriggerOutValue = plc.ReadLong(TriggerOutAddr);
                    int TriggerInValue = plc.ReadLong(TriggerInAddr);
                    int TriggerDifference = TriggerOutValue - TriggerInValue;

                    ushort sawNoLength = Convert.ToUInt16(ConfigurationSettings.AppSettings["sawNoLength"]);

                    int MachineNo = plc.ReadLong(MachineNoAddr);
                    string SawNo = plc.ReadString(SawNoAddr, sawNoLength);
                    int SumCount = plc.ReadLong(SumCountAddr);

                    if (TriggerDifference == 1)
                    {
                        lineControlCuttingServicePlus.InsertPLCLog(newPLCLog(MachineNo.ToString(), TriggerType.PLC, $"MachineNo({MachineNoAddr})：{MachineNo}发起请" +
                            $"求，请求信息：SawNo({SawNoAddr})={SawNo}，SumCount({SumCountAddr})={SumCount}，TriggerIn({TriggerInAddr})={TriggerInValue}，" +
                            $"TriggerOut({TriggerOutAddr})={TriggerOutValue}", LogType.GENERAL));

                        CheckUnloadSawFile(SawNo, ResultAddr, TriggerInAddr, TriggerOutValue, MachineNo, SumCount, PLCResultAddr);
                    }
                    else if (TriggerDifference != 0)
                    {
                        //plc.Write(ResultAddr, (int)PLCResultEnum.ABNORMAL);
                        //lineControlCuttingServicePlus.InsertPLCLog(newPLCLog(MachineNo.ToString(),TriggerType.PLC,$"回馈Result：{(int)PLCResultEnum.ABNORMAL}，" +
                        //    $"异常信息：TriggerOut值自增异常，自增量：{TriggerDifference}，TriggerOut={TriggerOutValue}，TriggerIn={TriggerInValue}",LogType.ABNORMAL));

                        lineControlCuttingServicePlus.InsertPLCLog(newPLCLog(MachineNo.ToString(), TriggerType.PLC, $"TriggerOut值自增异常，" +
                            $"自增量：{TriggerDifference}，TriggerOut({TriggerOutAddr})={TriggerOutValue}，TriggerIn({TriggerInAddr})={TriggerInValue}", LogType.ABNORMAL));

                    }
                }
                else
                {
                    string ip = ConfigurationSettings.AppSettings["plcIp"];
                    plcIsConnect = plc.Connect(ip);
                    if (plcIsConnect)
                    {
                        CheckStationRequest();
                    }
                    else
                    {
                        lineControlCuttingServicePlus.InsertPLCLog(newPLCLog(CuttingSetting.Current.CurrDeviceName, TriggerType.LineControl,
                            $"连接PLC失败：{ip}", LogType.ABNORMAL));
                    }
                }
        }

        /// <summary>
        /// 根据saw文件名检查可下载的saw文件
        /// </summary>
        /// <param name="SawNo">saw文件名</param>
        /// <param name="ResultAddr">plc Result地址块</param>
        /// <param name="TriggerInAddr">plc trigger地址块</param>
        /// <param name="TriggerOutValue">plc triggerout地址块</param>
        /// <param name="MachineNo">站台号</param>
        private void CheckUnloadSawFile(string SawNo,string ResultAddr,string TriggerInAddr,int TriggerOutValue,int MachineNo,int SumCount,
            string PLCResultAddr)
        {
            
                List<CuttingSawFileRelationPlus> listCuttingSawFileRelationPlus = lineControlCuttingServicePlus
                .GetCuttingSawFileRelationPlusBySawFileName(SawNo);
                if (listCuttingSawFileRelationPlus.Count > 0)
                {
                    CuttingSawFileRelationPlus cuttingSawFileRelationPlus = listCuttingSawFileRelationPlus[0];
                    int status = cuttingSawFileRelationPlus.Status;
                    int boardCount = cuttingSawFileRelationPlus.BoardCount;

                    //检查saw文件是否已下载或者下载中或者未分配
                    if (status == (int)CuttingSawFileRelationPlusStatus.Downloading || status ==
                        (int)CuttingSawFileRelationPlusStatus.Downloaded || status == (int)CuttingSawFileRelationPlusStatus
                        .Unassigned || status == (int)CuttingSawFileRelationPlusStatus.FeedingComplete)
                    {
                        string description = " ";

                        switch (status)
                        {
                            case (int)CuttingSawFileRelationPlusStatus.Downloading:
                                description = CuttingSawFileRelationPlusStatus.Downloading.GetFinishStatusDescription().Item2;
                                break;
                            case (int)CuttingSawFileRelationPlusStatus.Downloaded:
                                description = CuttingSawFileRelationPlusStatus.Downloaded.GetFinishStatusDescription().Item2;
                                break;
                            case (int)CuttingSawFileRelationPlusStatus.Unassigned:
                                description = CuttingSawFileRelationPlusStatus.Unassigned.GetFinishStatusDescription().Item2;
                                break;
                            case (int)CuttingSawFileRelationPlusStatus.FeedingComplete:
                                description = CuttingSawFileRelationPlusStatus.FeedingComplete.GetFinishStatusDescription().Item2;
                                break;
                            default:
                                break;
                        }

                        //plc.Write(ResultAddr, (int)PLCResultEnum.ABNORMAL);
                        PublicUtils.PLCWriteInt(plc, ResultAddr, (int)PLCResultEnum.ABNORMAL);
                        lineControlCuttingServicePlus.InsertPLCLog(newPLCLog(MachineNo.ToString(), TriggerType.LineControl, $"回馈Result：{(int)PLCResultEnum.ABNORMAL}，" +
                            $"异常信息：该Saw文件：{SawNo}{description},不允许下载，如确需下载，请手动分配！", LogType.ABNORMAL));

                        //plc.Write(TriggerInAddr, TriggerOutValue);
                        PublicUtils.PLCWriteInt(plc, TriggerInAddr, TriggerOutValue);
                        lineControlCuttingServicePlus.InsertPLCLog(newPLCLog(MachineNo.ToString(), TriggerType.LineControl, $"回馈TriggerIn：{TriggerOutValue}", LogType.GENERAL));
                    }
                    else
                    {
                        //plc板件数量与系统板件数量不相符
                        if (SumCount != cuttingSawFileRelationPlus.BoardCount)
                        {
                            //plc.Write(ResultAddr, (int)PLCResultEnum.ABNORMAL);
                            PublicUtils.PLCWriteInt(plc, ResultAddr, (int)PLCResultEnum.ABNORMAL);
                            lineControlCuttingServicePlus.InsertPLCLog(newPLCLog(MachineNo.ToString(), TriggerType.LineControl, $"回馈Result：{(int)PLCResultEnum.ABNORMAL}，" +
                                $"异常信息：{MachineNo}站台/工位携带的Saw文件：{SawNo}板件数量值：{SumCount}与系统Saw文件：" +
                                $"{cuttingSawFileRelationPlus.SawFileName}对应数量值：{cuttingSawFileRelationPlus.BoardCount}不相符！任务中断！", LogType.ABNORMAL));

                            //plc.Write(TriggerInAddr, TriggerOutValue);
                            PublicUtils.PLCWriteInt(plc, TriggerInAddr, TriggerOutValue);
                            lineControlCuttingServicePlus.InsertPLCLog(newPLCLog(MachineNo.ToString(), TriggerType.LineControl, $"回馈TriggerIn：{TriggerOutValue}", LogType.GENERAL));
                            return;
                        }

                        bool dbFlag = false;

                        string sawFile = cuttingSawFileRelationPlus.SawFile;
                        string sawFileName = cuttingSawFileRelationPlus.SawFileName;

                        //更新锯切图状态下载中
                        cuttingSawFileRelationPlus.DeviceName = CuttingSetting.Current.CurrDeviceName;
                        cuttingSawFileRelationPlus.Status = (int)CuttingSawFileRelationPlusStatus.Downloading;
                        cuttingSawFileRelationPlus.UpdatedTime = DateTime.Now;
                        dbFlag = lineControlCuttingServicePlus.UpdateCuttingSawFileRelationPlus(cuttingSawFileRelationPlus);

                        //if (!dbFlag)
                        //{
                        //    plc.Write(ResultAddr, (int)PLCResultEnum.ABNORMAL);
                        //    lineControlCuttingServicePlus.InsertPLCLog(newPLCLog(MachineNo.ToString(), TriggerType.LineControl, $"回馈Result：{PLCResultEnum.ABNORMAL}，" +
                        //        $"异常信息：CuttingSawFileRelationPlus状态更新失败！Saw文件：{SawNo}下载中止！重新下载请手动分配", LogType.ABNORMAL));

                        //    plc.Write(TriggerInAddr, TriggerOutValue);
                        //    lineControlCuttingServicePlus.InsertPLCLog(newPLCLog(MachineNo.ToString(), TriggerType.LineControl, $"回馈TriggerIn：{TriggerOutValue}", LogType.GENERAL));
                        //    return;
                        //}

                        bool downloadFlag = DownloadSawFile(CuttingSetting.Current.CurrDeviceName, sawFile, sawFileName);
                        if (downloadFlag)
                        {
                            //plc.Write(ResultAddr, (int)PLCResultEnum.NORMAL);
                            PublicUtils.PLCWriteInt(plc, ResultAddr, (int)PLCResultEnum.NORMAL);
                            lineControlCuttingServicePlus.InsertPLCLog(newPLCLog(MachineNo.ToString(), TriggerType.LineControl, $"回馈Result：{(int)PLCResultEnum.NORMAL}" +
                                $",详情：Saw文件：{SawNo}下载完成,文件路径：{CuttingSetting.Current.TmpMDBPath + "\\" + sawFileName}", LogType.GENERAL));

                            //plc.Write(TriggerInAddr, TriggerOutValue);
                            PublicUtils.PLCWriteInt(plc, TriggerInAddr, TriggerOutValue);
                            lineControlCuttingServicePlus.InsertPLCLog(newPLCLog(MachineNo.ToString(), TriggerType.LineControl, $"回馈TriggerIn：{TriggerOutValue}", LogType.GENERAL));

                            //更新锯切图状态下载完成
                            cuttingSawFileRelationPlus.Status = (int)CuttingSawFileRelationPlusStatus.Downloaded;
                            cuttingSawFileRelationPlus.UpdatedTime = DateTime.Now;
                            dbFlag = lineControlCuttingServicePlus.UpdateCuttingSawFileRelationPlus(cuttingSawFileRelationPlus);

                            while (true)
                            {
                                List<CuttingSawFileRelationPlus> listCuttingSawFileRelationPlus2 = lineControlCuttingServicePlus.
                                    GetCuttingSawFileRelationPlusBySawFileName(sawFileName);
                                CuttingSawFileRelationPlus cuttingSawFileRelationPlus2 = listCuttingSawFileRelationPlus2[0];

                                if (cuttingSawFileRelationPlus2.Status == (int)CuttingSawFileRelationPlusStatus.FeedingComplete)
                                {
                                    lineControlCuttingServicePlus.InsertPLCLog(newPLCLog(MachineNo.ToString(), TriggerType.PLC, $"{MachineNo.ToString()}号" +
                                        $"站台进料成功，此动作为系统手动完成", LogType.GENERAL));
                                    //cuttingSawFileRelationPlus2.Status = (int)CuttingSawFileRelationPlusStatus.FeedingComplete;
                                    //lineControlCuttingServicePlus.BulkUpdatedCuttingSawFileRelationPlus(listCuttingSawFileRelationPlus2);
                                    break;
                                }

                                int plcResult = plc.ReadLong(PLCResultAddr);
                                if (plcResult == 2)//进料成功
                                {
                                    lineControlCuttingServicePlus.InsertPLCLog(newPLCLog(MachineNo.ToString(), TriggerType.PLC, $"{MachineNo.ToString()}号" +
                                        $"站台进料成功，Result({PLCResultAddr})={plcResult}", LogType.GENERAL));

                                    cuttingSawFileRelationPlus2.Status = (int)CuttingSawFileRelationPlusStatus.FeedingComplete;
                                    lineControlCuttingServicePlus.BulkUpdatedCuttingSawFileRelationPlus(listCuttingSawFileRelationPlus2);
                                    break;
                                }
                                else if (plcResult == 3)//进料失败
                                {
                                    cuttingSawFileRelationPlus.Status = (int)CuttingSawFileRelationPlusStatus.Assigned;
                                    lineControlCuttingServicePlus.UpdateCuttingSawFileRelationPlus(cuttingSawFileRelationPlus);

                                    lineControlCuttingServicePlus.InsertPLCLog(newPLCLog(MachineNo.ToString(), TriggerType.PLC, $"{MachineNo.ToString()}号" +
                                        $"站台进料失败，Result({PLCResultAddr})={plcResult}，系统恢复锯切图{cuttingSawFileRelationPlus.SawFileName}状态已分" +
                                        $"配({cuttingSawFileRelationPlus.Status})", LogType.ABNORMAL));
                                    break;
                                }

                                Thread.Sleep(int.Parse(ConfigurationSettings.AppSettings["pollingPLCResultTime"]));
                            }
                        }
                        else
                        {
                            //plc.Write(ResultAddr, (int)PLCResultEnum.ABNORMAL);
                            PublicUtils.PLCWriteInt(plc, ResultAddr, (int)PLCResultEnum.ABNORMAL);
                            lineControlCuttingServicePlus.InsertPLCLog(newPLCLog(MachineNo.ToString(), TriggerType.LineControl, $"回馈Result：{(int)PLCResultEnum.ABNORMAL}" +
                                $",详情：Saw文件：{SawNo}下载失败！重新下载请手动分配或者PLC重新出发", LogType.ABNORMAL));

                            //plc.Write(TriggerInAddr, TriggerOutValue);
                            PublicUtils.PLCWriteInt(plc, TriggerInAddr, TriggerOutValue);
                            lineControlCuttingServicePlus.InsertPLCLog(newPLCLog(MachineNo.ToString(), TriggerType.LineControl, $"回馈TriggerIn：{TriggerOutValue}", LogType.GENERAL));

                            //更新锯切图状态下载失败
                            cuttingSawFileRelationPlus.Status = (int)CuttingSawFileRelationPlusStatus.Fail;
                            cuttingSawFileRelationPlus.UpdatedTime = DateTime.Now;
                            dbFlag = lineControlCuttingServicePlus.UpdateCuttingSawFileRelationPlus(cuttingSawFileRelationPlus);
                        }
                    }
                }
                else
                {
                    if (String.IsNullOrEmpty(SawNo))
                    {
                        lineControlCuttingServicePlus.InsertPLCLog(newPLCLog(MachineNo.ToString(), TriggerType.LineControl, $"SawNo为空，" +
                            $"MachineNo={MachineNo}的SawNo地址块={ConfigurationSettings.AppSettings["SawNoAddr"]}", LogType.ABNORMAL));
                    }

                    //plc.Write(ResultAddr, (int)PLCResultEnum.ABNORMAL);
                    PublicUtils.PLCWriteInt(plc, ResultAddr, (int)PLCResultEnum.ABNORMAL);
                    lineControlCuttingServicePlus.InsertPLCLog(newPLCLog(MachineNo.ToString(), TriggerType.LineControl, $"回馈Result：{(int)PLCResultEnum.ABNORMAL}，" +
                        $"异常信息：Saw文件：{SawNo}不存在", LogType.ABNORMAL));

                    //plc.Write(TriggerInAddr, TriggerOutValue);
                    PublicUtils.PLCWriteInt(plc, TriggerInAddr, TriggerOutValue);
                    lineControlCuttingServicePlus.InsertPLCLog(newPLCLog(MachineNo.ToString(), TriggerType.LineControl, $"回馈TriggerIn：{TriggerOutValue}", LogType.GENERAL));
                }

        }

        /// <summary>
        /// 检查手动分配saw文件
        /// </summary>
        private void CheckManualAllocationSawFile()
        {
            List<CuttingSawFileRelationPlus> listCuttingSawFileRelationPlus = lineControlCuttingServicePlus.
                GetCuttingSawFileRelationPlusByDeviceNameAndStatus(CuttingSetting.Current.CurrDeviceName,
                CuttingSawFileRelationPlusStatus.ManualAllocation);
            if (listCuttingSawFileRelationPlus.Count > 0)
            {
                CuttingSawFileRelationPlus cuttingSawFileRelationPlus = listCuttingSawFileRelationPlus[0];
                cuttingSawFileRelationPlus.Status = (int)CuttingSawFileRelationPlusStatus.Downloading;
                cuttingSawFileRelationPlus.UpdatedTime = DateTime.Now;
                lineControlCuttingServicePlus.UpdateCuttingSawFileRelationPlus(cuttingSawFileRelationPlus);

                string sawFile = cuttingSawFileRelationPlus.SawFile;
                string sawFileName = cuttingSawFileRelationPlus.SawFileName;

                bool flag = DownloadSawFile(CuttingSetting.Current.CurrDeviceName,sawFile, sawFileName);
                if (flag)
                {
                    cuttingSawFileRelationPlus.Status = (int)CuttingSawFileRelationPlusStatus.Downloaded;
                    cuttingSawFileRelationPlus.UpdatedTime = DateTime.Now;
                    lineControlCuttingServicePlus.UpdateCuttingSawFileRelationPlus(cuttingSawFileRelationPlus);

                    lineControlCuttingServicePlus.InsertPLCLog(newPLCLog(CuttingSetting.Current.CurrDeviceName, TriggerType.LineControl, $"手动分配" +
                        $"SAW文件：{sawFileName}下载成功，文件路径：{CuttingSetting.Current.TmpMDBPath + "\\" + sawFileName}", LogType.GENERAL));
                }
                else
                {
                    cuttingSawFileRelationPlus.Status = (int)CuttingSawFileRelationPlusStatus.Fail;
                    cuttingSawFileRelationPlus.UpdatedTime = DateTime.Now;
                    lineControlCuttingServicePlus.UpdateCuttingSawFileRelationPlus(cuttingSawFileRelationPlus);

                    lineControlCuttingServicePlus.InsertPLCLog(newPLCLog(CuttingSetting.Current.CurrDeviceName,TriggerType.LineControl, "手动分配" +
                        "SAW文件：{sawFileName}下载失败", LogType.ABNORMAL));
                }
            }
        }

        /// <summary>
        /// 下载saw文件
        /// </summary>
        /// <param name="sawFile">saw文件</param>
        /// <returns>布尔值</returns>
        private bool DownloadSawFile(string station,string sawFile,string sawFileName)
        {
            if (!Directory.Exists(CuttingSetting.Current.TmpMDBPath))
            {
                try
                {
                    Directory.CreateDirectory(CuttingSetting.Current.TmpMDBPath);
                    
                }
                catch (Exception e)
                {
                    lineControlCuttingServicePlus.InsertPLCLog(newPLCLog(station, TriggerType.LineControl, $"{CuttingSetting.Current.TmpMDBPath}目录" +
                        $"不存在，创建文件夹异常：{e.Message}", LogType.ABNORMAL));
                    return false;
                }
            }

            try
            {
                //FileStream fs = new FileStream(CuttingSetting.Current.SawPath + "\\" + sawFileName, FileMode.Create, FileAccess.Write);
                string filePath = CuttingSetting.Current.TmpMDBPath + "\\" + sawFileName;

                //StreamWriter sw = new StreamWriter(filePath);
                //sw.Write(sawFile);
                //sw.Flush();
                //sw.Close();

                //byte[] b = Encoding.UTF8.GetBytes(sawFile);
                //byte[] b = Encoding.GetEncoding(866).GetBytes(sawFile);
                byte[] b = Encoding.Default.GetBytes(sawFile);
                FileStream fileStream = new FileStream(filePath, FileMode.Create);
                fileStream.Write(b, 0, b.Length);
                fileStream.Flush();
                fileStream.Close();
            }
            catch (Exception e)
            {
                lineControlCuttingServicePlus.InsertPLCLog(newPLCLog(station, TriggerType.LineControl, $"创建saw文件失败：{e.Message}", LogType.ABNORMAL));
                return false;
            }

            return true;
        }

        private PLCLog newPLCLog(string station, TriggerType triggerType, string detail, LogType logType)
        {
            string version = "4.3.6";

            PLCLog plcLog = new PLCLog();
            plcLog.Station = station;
            //plcLog.TriggerType = triggerType.GetFinishStatusDescription().Item2;
            plcLog.TriggerType = triggerType.GetFinishStatusDescription().Item2;
            plcLog.Detail = $"开料锯软件版本：{version}|{detail}";
            plcLog.LogType = logType.GetFinishStatusDescription().Item2;
            return plcLog;
        }
    }
}
