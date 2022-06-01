// ************************************************************************************
//  解决方案：NJIS.FPZWS.LineControl.Drilling
//  项目名称：NJIS.FPZWS.LineControl.Drilling.MprImport
//  文 件 名：MprImportApp.cs
//  创建时间：2019-08-30 11:26
//  作    者：
//  说    明：
//  修改时间：2019-08-24 8:00
//  修 改 人：
// Copyright © 2017 广州宁基智能系统有限公司. 版权所有
// *************************************************************************************

using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NJIS.FPZWS.Common;
using NJIS.FPZWS.LineControl.Drilling.Model;
using NJIS.FPZWS.LineControl.Drilling.Service;
using NJIS.FPZWS.Log;
using NJIS.FPZWS.Log.Implement.Log4Net;

namespace NJIS.FPZWS.LineControl.Drilling.MprImport
{
    public class MprImportApp : AppBase<MprImportApp>, IService
    {
        private static ILogger _logger = LogManager.GetLogger(nameof(MprImportApp));

        public override bool Start()
        {
            LogManager.AddLoggerAdapter(new Log4NetLoggerAdapter()); 
            _logger = LogManager.GetLogger(nameof(MprImportApp));
            base.Start();
            Task.Factory.StartNew(() =>
            {
                while (IsStart)
                {
                    _logger.Debug("执行文件清理");
                    try
                    {
                        if (!string.IsNullOrEmpty(MprImportSetting.Current.OneMprPath))
                        {
                            _logger.Debug($"清理Mpr 目录-{MprImportSetting.Current.OneMprPath}");
                            var files = Directory.GetFiles(MprImportSetting.Current.OneMprPath, "*.mpr",
                                    SearchOption.AllDirectories)
                                .Select(mbox => new FileInfo(mbox))
                                .Where(mbox =>
                                    mbox.CreationTime <
                                    DateTime.Now.AddDays(-MprImportSetting.Current.OneMprStorageDay));

                            foreach (var item in files)
                            {
                                try
                                {
                                    File.Delete(item.FullName);
                                }
                                catch (Exception)
                                {
                                    // ignore
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(MprImportSetting.Current.DoubleMprPath))
                        {
                            _logger.Debug($"清理Mpr 目录-{MprImportSetting.Current.DoubleMprPath}");
                            var files = Directory.GetFiles(MprImportSetting.Current.DoubleMprPath, "*.mpr",
                                    SearchOption.AllDirectories)
                                .Select(mbox => new FileInfo(mbox))
                                .Where(mbox =>
                                    mbox.CreationTime <
                                    DateTime.Now.AddDays(-MprImportSetting.Current.DoubleMprStorageDay));

                            foreach (var item in files)
                            {
                                try
                                {
                                    File.Delete(item.FullName);
                                }
                                catch (Exception)
                                {
                                    // ignore
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(MprImportSetting.Current.HomagCsvPath))
                        {
                            _logger.Debug($"清理Mpr 目录-{MprImportSetting.Current.HomagCsvPath}");
                            var files = Directory.GetFiles(MprImportSetting.Current.HomagCsvPath, "*.csv",
                                    SearchOption.AllDirectories)
                                .Select(mbox => new FileInfo(mbox))
                                .Where(mbox =>
                                    mbox.CreationTime <
                                    DateTime.Now.AddDays(-MprImportSetting.Current.HomagCsvCsvStoreDay));

                            foreach (var item in files)
                            {
                                try
                                {
                                    File.Delete(item.FullName);
                                }
                                catch (Exception)
                                {
                                    // ignore
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex);
                    }

                    Thread.Sleep(MprImportSetting.Current.MprInterval);
                }
            });
            Task.Factory.StartNew(() =>
            {
                while (IsStart)
                {
                    _logger.Debug("执行数据导入");
                    try
                    {
                        _logger.Debug($"获取批次数据-{MprImportSetting.Current.ImportDataForDay}");
                        var service = new DrillingService();
                        var batchs =
                            service.GetNotImportBatchs(DateTime.Now.AddDays(MprImportSetting.Current.ImportDataForDay), MprImportSetting.Current.Machine);

                        _logger.Debug($"共获取到批次数量-{batchs.Count}");
                        foreach (var item in batchs)
                        {
                            try
                            {
                                var way = 0;
                                if (!service.CheckDrillingImport(item, way, MprImportSetting.Current.Machine))
                                {
                                    ImportMprDown(item);
                                    service.SaveDrillingImport(new DrillingImport
                                    {
                                        BatchName = item,
                                        ImportStatus = 2,
                                        Machine = MprImportSetting.Current.Machine,
                                        Msg = "导入成功",
                                        Way = way
                                    });
                                }

                                way = 1;
                                if (!service.CheckDrillingImport(item, way, MprImportSetting.Current.Machine))
                                {
                                    ImportHomagData(item);
                                    service.SaveDrillingImport(new DrillingImport
                                    {
                                        BatchName = item,
                                        ImportStatus = 2,
                                        Machine = MprImportSetting.Current.Machine,
                                        Msg = "导入成功",
                                        Way = way
                                    });
                                }

                                way = 2;
                                if (!service.CheckDrillingImport(item, way, MprImportSetting.Current.Machine))
                                {
                                    ImportHomagCsv(item);
                                    service.SaveDrillingImport(new DrillingImport
                                    {
                                        BatchName = item,
                                        ImportStatus = 2,
                                        Machine = MprImportSetting.Current.Machine,
                                        Msg = "导入成功",
                                        Way = way
                                    });
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.Error(ex);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex);
                    }

                    Thread.Sleep(MprImportSetting.Current.MprInterval);
                }
            });
            return true;
        }

        public void ImportMprDown(string batchCode)
        {
            try
            {
                if (!MprImportSetting.Current.MprDownEnable) return;
                if (string.IsNullOrEmpty(batchCode)) return;
                var services = new DrillingService();

                OnWriteMessageToMprDown($"开始加载数据-{batchCode}");
                var datas = services.FindDrillings(batchCode);
                if (datas.Count > 0)
                {
                    OnWriteMessageToMprDown($"数据加载完成-{datas.Count}，开始生成Mpr文件。");
                    if (!string.IsNullOrEmpty(MprImportSetting.Current.OneMprPath))
                    {
                        OnWriteMessageToMprDown("开始生成单面孔文件。");
                        foreach (var drilling in datas)
                        {
                            if (drilling.DrillingRouting.Trim() == "1" || drilling.DrillingRouting.Trim() == "3")
                            {
                                File.WriteAllText(
                                    Path.Combine(MprImportSetting.Current.OneMprPath, drilling.PartID + ".mpr"),
                                    drilling.DrillingProgram);
                            }
                        }

                        OnWriteMessageToMprDown($"单面孔生成完成-{MprImportSetting.Current.OneMprPath}");
                        Process.Start(MprImportSetting.Current.OneMprPath);
                    }

                    if (!string.IsNullOrEmpty(MprImportSetting.Current.DoubleMprPath))
                    {
                        OnWriteMessageToMprDown("开始生成双面孔文件。");

                        foreach (var drilling in datas)
                        {
                            if (drilling.DrillingRouting.Trim() == "2" || drilling.DrillingRouting.Trim() == "4")
                            {
                                File.WriteAllText(
                                    Path.Combine(MprImportSetting.Current.DoubleMprPath, drilling.PartID + ".mpr"),
                                    drilling.DrillingProgram1);
                            }
                        }

                        OnWriteMessageToMprDown($"双面孔生成完成-{MprImportSetting.Current.DoubleMprPath}");
                        Process.Start(MprImportSetting.Current.DoubleMprPath);
                    }
                }
                else
                {
                    OnWriteMessageToMprDown("加载数据为空。");
                }
            }
            catch (Exception e)
            {
                OnWriteMessageToMprDown($"导入数据出错-{e.Message}");
                _logger.Error(e);
            }
        }

        public void ImportHomagData(string batchCode)
        {
            try
            {
                if (!MprImportSetting.Current.HomagDataServerEnable) return;
                if (string.IsNullOrEmpty(batchCode)) return;
                if (string.IsNullOrEmpty(MprImportSetting.Current.HomagDataService)) return;
                if (string.IsNullOrEmpty(MprImportSetting.Current.HomagDataDatabase)) return;
                if (string.IsNullOrEmpty(MprImportSetting.Current.HomagDataUser)) return;
                if (string.IsNullOrEmpty(MprImportSetting.Current.HomagDataPwd)) return;
                var services = new DrillingService();
                OnWriteMessageToHomagData($"开始加载数据-{batchCode}");
                var datas = services.FindDrillings(batchCode);
                if (datas.Count > 0)
                {
                    OnWriteMessageToHomagData($"数据加载完成，开始导入数据-{datas.Count}");

                    var sb = new StringBuilder();
                    foreach (var drilling in datas)
                    {
                        sb.AppendLine(
                            $"Delete [dbo].[Datensatz1Details] where UPI='{drilling.PartID}'");
                        sb.AppendLine(
                            $"Delete [dbo].[Datensatz1] where UPI='{drilling.PartID}'");
                        sb.AppendLine(
                            "INSERT INTO [dbo].[Datensatz1]([UPI],[decor],[materialFlow],[batchnumber],[customernumber]" +
                            ",[length],[width],[thickness],[quantity],[DATUM],[BENUTZER])    " +
                            $"VALUES('{drilling.PartID}', '{drilling.MaterialInformation}','{drilling.Materialflow}', '{drilling.BatchName}', '{drilling.OrderNumber}'," +
                            $"{drilling.FinishLength},{drilling.FinishWidth},{drilling.FinishThickness}, 1, getdate(), 'auto. CSV data import');");
                        sb.AppendLine(
                            "INSERT INTO [dbo].[Datensatz1Details]([UPI],[ThroughFeedNumber],[MprFile],[AssignmentMode],[MprReady],[ProcessStart],[ProcessReady]) " +
                            $"VALUES('{drilling.PartID}',1, '{drilling.PartID}.mpr',1, 0, getdate(), getdate());");
                        sb.AppendLine($"delete parttracking where upi='{drilling.PartID}'");
                        sb.AppendLine($"INSERT INTO [dbo].[parttracking]([UPI],[STATUS],[UpdateTimestamp],[BENUTZER]," +
                                      $"[ProcessStart],[ReadyTimestamp],[ReadyForExport],[ProcessingTimeInLine])  " +
                                      $"VALUES('{drilling.PartID}', 10, getdate(), 'mes import', getdate(), getdate(), 0, '000000');");
                    }

                    var connStr =
                        $"Data Source={MprImportSetting.Current.HomagDataService};Initial Catalog={MprImportSetting.Current.HomagDataDatabase};" +
                        $"User ID={MprImportSetting.Current.HomagDataUser};Pwd={MprImportSetting.Current.HomagDataPwd}";
                    using (var connection = new SqlConnection(connStr))
                    {
                        try
                        {
                            connection.Open();
                            var command = new SqlCommand(sb.ToString(), connection);
                            command.CommandTimeout = 6000;
                            command.CommandType = CommandType.Text;
                            command.ExecuteNonQuery();
                        }
                        catch (Exception exception)
                        {
                            OnWriteMessageToHomagData("导入数据失败" + exception.Message);
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }

                    OnWriteMessageToHomagData("导入数据完成");
                }
                else
                {
                    OnWriteMessageToHomagData("加载数据为空。");
                }

                OnWriteMessageToHomagData("数据导入完成");
            }
            catch (Exception e)
            {
                OnWriteMessageToHomagData($"导入数据出错-{e.Message}");
                _logger.Error(e);
            }
        }

        public void ImportHomagCsv(string batchCode)
        {
            try
            {
                if (!MprImportSetting.Current.HomagCsvCsvEnable) return;
                if (string.IsNullOrEmpty(batchCode)) return;
                if (string.IsNullOrEmpty(MprImportSetting.Current.HomagCsvPath)) return;

                OnWriteMessageToHomagCsv($"开始加载数据-{batchCode}");

                var services = new DrillingService();
                var datas = services.FindDrillings(batchCode);
                OnWriteMessageToHomagCsv($"加载数据完成-{batchCode}，开始生成csv文件-{datas.Count}");

                if (datas.Count > 0)
                {
                    var sb = new StringBuilder();
                    foreach (var partInfo in datas)
                    {
                        sb.AppendLine(
                            $"{partInfo.PartID};{partInfo.MaterialInformation};{partInfo.Materialflow};1000;{partInfo.OrderHeaderId};{partInfo.FinishLength};{partInfo.FinishWidth};{partInfo.FinishThickness};1;1;{partInfo.PartID}.mpr");
                    }

                    sb.Append("");

                    var mprFileResultFolder = MprImportSetting.Current.HomagCsvPath;
                    var batchPath = Path.Combine(mprFileResultFolder, $"{batchCode}.csv");
                    File.WriteAllText(batchPath, sb.ToString());

                    OnWriteMessageToHomagCsv($"文件生成完成-{MprImportSetting.Current.HomagCsvPath}");
                    Process.Start(MprImportSetting.Current.HomagCsvPath);
                }
                else
                {
                    OnWriteMessageToHomagCsv("加载数据为空。");
                }
            }
            catch (Exception e)
            {
                OnWriteMessageToHomagData($"导入数据出错-{e.Message}");
                _logger.Error(e);
            }
        }

        public event EventHandler<string> WriteMessageToHomagCsvMessage;

        protected void OnWriteMessageToHomagCsv(string msg)
        {
            _logger.Debug(msg);
            if (WriteMessageToHomagCsvMessage != null)
            {
                WriteMessageToHomagCsvMessage(this, msg);
            }
        }

        public event EventHandler<string> WriteMessageToHomagDataMessage;

        protected void OnWriteMessageToHomagData(string msg)
        {
            _logger.Debug(msg);
            if (WriteMessageToHomagDataMessage != null)
            {
                WriteMessageToHomagDataMessage(this, msg);
            }
        }

        public event EventHandler<string> WriteMessageToMprDownMessage;

        protected void OnWriteMessageToMprDown(string msg)
        {
            _logger.Debug(msg);
            if (WriteMessageToMprDownMessage != null)
            {
                WriteMessageToMprDownMessage(this, msg);
            }
        }
    }
}