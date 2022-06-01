using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NJIS.FPZWS.LineControl.Cutting.ServicePlus;
using NJIS.FPZWS.LineControl.Cutting.ModelPlus;
using System.Collections.Generic;
using System.Text;
using NJIS.FPZWS.LineControl.Manager;
using System.IO;
using NJIS.FPZWS.LineControl.Manager.Helpers;
using System.Configuration;
using NJIS.FPZWS.LineControl.Manager.WMSWebReference;
using NJIS.FPZWS.LineControl.Manager.Utils;
using NJIS.FPZWS.LineControl.CuttingDevice;
using System.Windows.Forms;
using NJIS.FPZWS.LineControl.Manager.Views.Dialog;
using NJIS.FPZWS.LineControl.Cutting.Service;
using NJIS.FPZWS.LineControl.Manager.Service;

namespace ServicePlusTest3
{
    [TestClass]
    public class LineControlCuttingServicePlusTest
    {
        LineControlCuttingServicePlus lineControlCuttingServicePlus = new LineControlCuttingServicePlus();
        LineControlCuttingService lineControlCuttingService = new LineControlCuttingService();

        [TestMethod]
        public void TestGetCuttingSawFileRelationPlusBySawFileName()
        {
            List<CuttingSawFileRelationPlus> listCuttingSawFileRelationPlus = lineControlCuttingServicePlus.GetCuttingSawFileRelationPlusBySawFileName("image.jpg");
            if (listCuttingSawFileRelationPlus.Count<1)
            {
                return;
            }
            CuttingSawFileRelationPlus cuttingSawFileRelationPlus = listCuttingSawFileRelationPlus[0];

            //byte[] b = Encoding.UTF8.GetBytes(cuttingSawFileRelationPlus.SawFile);

            //FileStream fs = new FileStream($"D:/TestFile/download/{cuttingSawFileRelationPlus.SawFileName}", FileMode.Create);
            //fs.Write(b,0,b.Length);
        }

        [TestMethod]
        public void TestGetCuttingSawFileRelationPlusByBatchStackName()
        {
            List<CuttingSawFileRelationPlus> listCuttingSawFileRelationPlus = lineControlCuttingServicePlus.
                GetCuttingSawFileRelationPlusByStackName("1",SawType.TYPE1);
            int count = listCuttingSawFileRelationPlus.Count;
            
            StringBuilder sb = new StringBuilder();
            List<string> list = new List<string>();
            string[] sawFileName = new string[listCuttingSawFileRelationPlus.Count];
            string sawFileNameStr = "";

            foreach (var item in listCuttingSawFileRelationPlus)
            {
                list.Add(item.SawFileName);
            }

            for (int i = 0; i < listCuttingSawFileRelationPlus.Count; i++)
            {
                //sawFileName[i] = listCuttingSawFileRelationPlus[i].SawFileName;
                sawFileNameStr += listCuttingSawFileRelationPlus[i].SawFileName;
                if (i < listCuttingSawFileRelationPlus.Count-1)
                    sawFileNameStr += ",";
            }
            Console.WriteLine(sawFileNameStr);
        }

        [TestMethod]
        public void TestGetMinSawIndexCuttingSawFileRelationByBatchStackName()
        {
            //List<CuttingSawFileRelationPlus> listCuttingSawFileRelationPlus = lineControlCuttingServicePlus.GetCuttingSawFileRelationPlusByBatchStackName("1");
            int stackNo = 1;
            List<CuttingSawFileRelation> listCuttingSawFileRelation = lineControlCuttingServicePlus.GetMinStackIndexCuttingSawFileRelationByStackName(stackNo.ToString());
            Console.WriteLine(listCuttingSawFileRelation.Count);
        }

        [TestMethod]
        public void TestBulkInsertCuttingSawFileRelationPlus()
        {
            List<CuttingSawFileRelation> listCuttingSawFileRelation = lineControlCuttingServicePlus.GetCuttingSawFileRelationByBatchNameAndMinStackIndex("1");
            if (listCuttingSawFileRelation.Count < 1)
                return;
            List<CuttingSawFileRelationPlus> listCuttingSawFileRelationPlus = new List<CuttingSawFileRelationPlus>();
            foreach (var item in listCuttingSawFileRelation)
            {
                CuttingSawFileRelationPlus cuttingSawFileRelationPlus = new CuttingSawFileRelationPlus();
                cuttingSawFileRelationPlus.BatchName = item.BatchName;
                cuttingSawFileRelationPlus.StackName = item.StackName;
                cuttingSawFileRelationPlus.BoardCount = item.BoardCount;
                cuttingSawFileRelationPlus.CreatedTime = item.CreatedTime;
                //cuttingSawFileRelationPlus.Id = item.Id;
                cuttingSawFileRelationPlus.PlanDate = item.PlanDate;
                cuttingSawFileRelationPlus.SawFile = item.SawFile;
                cuttingSawFileRelationPlus.SawFileName = item.SawFileName;
                cuttingSawFileRelationPlus.StackIndex = item.StackIndex;
                cuttingSawFileRelationPlus.SawType = item.SawType;
                cuttingSawFileRelationPlus.Status = 0;
                cuttingSawFileRelationPlus.TaskDistributeId = item.TaskDistributeId;
                cuttingSawFileRelationPlus.TaskId = item.TaskId;
                cuttingSawFileRelationPlus.UpdatedTime = item.UpdatedTime;

                listCuttingSawFileRelationPlus.Add(cuttingSawFileRelationPlus);
            }
           
            bool flag = lineControlCuttingServicePlus.BulkInsertCuttingSawFileRelationPlus(listCuttingSawFileRelationPlus);

            Console.WriteLine(flag);
        }

        [TestMethod]
        public void TestInsertCuttingSawFileRelationPlus()
        {
            //FileStream fs = new FileStream("D:/TestFile/file/宁基智能-设备故障维修单20201225.xlsx", FileMode.Open);
            //MemoryStream ms = new MemoryStream();
            //fs.CopyTo(ms);
            //byte[] b = new byte[fs.Length];
            //fs.Read(b, 0, b.Length);

            StreamReader sr = new StreamReader("D:/TestFile/file/宁基智能-设备故障维修单20201225.xlsx");

            CuttingSawFileRelationPlus cuttingSawFileRelationPlus = new CuttingSawFileRelationPlus();
            cuttingSawFileRelationPlus.BatchName = "4";
            cuttingSawFileRelationPlus.StackName = "4";
            cuttingSawFileRelationPlus.BoardCount = 3;
            cuttingSawFileRelationPlus.CreatedTime = DateTime.Now;
            cuttingSawFileRelationPlus.DeviceName = "0-240-07-8012";
            cuttingSawFileRelationPlus.Id = 4;
            cuttingSawFileRelationPlus.PlanDate = DateTime.Now;
            cuttingSawFileRelationPlus.SawFile = "";
            cuttingSawFileRelationPlus.SawFileName = "宁基智能-设备故障维修单20201225.xlsx";
            cuttingSawFileRelationPlus.StackIndex = 4;
            cuttingSawFileRelationPlus.SawType = 4;
            cuttingSawFileRelationPlus.Status = (int)CuttingSawFileRelationPlusStatus.Assigned;
            cuttingSawFileRelationPlus.TaskDistributeId = Guid.NewGuid();
            cuttingSawFileRelationPlus.TaskId = Guid.NewGuid();
            cuttingSawFileRelationPlus.UpdatedTime = DateTime.Now;


            bool flag = lineControlCuttingServicePlus.InsertCuttingSawFileRelationPlus(cuttingSawFileRelationPlus);
            Assert.IsTrue(flag);
        }

        [TestMethod]
        public void TestByteStringConversion()
        {
            FileStream fs = new FileStream("D:/TestFile/file/宁基智能-设备故障维修单20201225.xlsx", FileMode.Open);
            byte[] b = new byte[fs.Length];
            fs.Read(b, 0, b.Length);

            string s = Encoding.Default.GetString(b);

            byte[] b2 = Encoding.Default.GetBytes(s);

            FileStream fs2 = new FileStream("D:/TestFile/download/宁基智能-设备故障维修单20201225.xlsx",FileMode.Create);
            fs2.Write(b2, 0, b2.Length);
        }

        [TestMethod]
        public void TestInsertPLCLog()
        {
            PLCLog plcLog = new PLCLog();
            plcLog.Detail = "测试";
            plcLog.Station = "10";
            plcLog.TriggerType = TriggerType.LineControl.GetFinishStatusDescription().Item2;

            bool flag = lineControlCuttingServicePlus.InsertPLCLog(plcLog);

            Console.WriteLine(flag);
        }

        [TestMethod]
        public void TestEnum()
        {
            Console.WriteLine((int)NJIS.FPZWS.LineControl.Manager.PLCResultEnum.ABNORMAL);
        }

        [TestMethod]
        public void TestUpdateCuttingSawFileRelationPlus()
        {
            List<CuttingSawFileRelationPlus> listCuttingSawFileRelationPlus = lineControlCuttingServicePlus.
                GetCuttingSawFileRelationPlusBySawFileName("宁基智能-设备故障维修单20201225.xlsx");
            CuttingSawFileRelationPlus cuttingSawFileRelationPlus = listCuttingSawFileRelationPlus[0];
            cuttingSawFileRelationPlus.DeviceName = "test";
            cuttingSawFileRelationPlus.Status = (int)CuttingSawFileRelationPlusStatus.Downloading;
            cuttingSawFileRelationPlus.UpdatedTime = DateTime.Now;
            lineControlCuttingServicePlus.UpdateCuttingSawFileRelationPlus(cuttingSawFileRelationPlus);
        }

        [TestMethod]
        public void TestGetCuttingSawFileRelationPlusByDeviceNameAndStatus()
        {
            List<CuttingSawFileRelationPlus> listCuttingSawFileRelationPlus = lineControlCuttingServicePlus.
                GetCuttingSawFileRelationPlusByDeviceNameAndStatus("test",CuttingSawFileRelationPlusStatus.Downloading);
            if (listCuttingSawFileRelationPlus.Count > 0)
            {
                CuttingSawFileRelationPlus cuttingSawFileRelationPlus = listCuttingSawFileRelationPlus[0];
            }
        }

        [TestMethod]
        public void TestDownloadSawFile()
        {
            //if (!Directory.Exists("D:/machine1/prod_data/Cadv45/data/Runs"))
            //{
            //    try
            //    {
            //        Directory.CreateDirectory("D:/machine1/prod_data/Cadv45/data/Runs");

            //    }
            //    catch (Exception e)
            //    {
            //        //lineControlCuttingServicePlus.InsertPLCLog(newPLCLog(station, TriggerType.LineControl, $"{CuttingSetting.Current.SawPath}目录" +
            //        //    $"不存在，创建文件夹异常：{e.Message}", LogType.ABNORMAL));
            //        //return false;
            //        Console.WriteLine(e.Message);
            //    }
            //}

            List<CuttingSawFileRelationPlus> listCuttingSawFileRelationPlus = lineControlCuttingServicePlus.
                GetCuttingSawFileRelationPlusBySawFileName("105369(1)_1.saw");
            CuttingSawFileRelationPlus cuttingSawFileRelationPlus = listCuttingSawFileRelationPlus[0];

            string sawFile = cuttingSawFileRelationPlus.SawFile;
            //StreamWriter sw = new StreamWriter($"D:/machine1/prod_data/Cadv45/data/Runs/{cuttingSawFileRelationPlus.SawFileName}");
            //sw.Write(sawFile);
            //sw.Flush();
            //sw.Close();

            if (!Directory.Exists(CuttingSetting.Current.TmpMDBPath))
            {
                
                    Directory.CreateDirectory(CuttingSetting.Current.TmpMDBPath);
                
            }

          
                //FileStream fs = new FileStream(CuttingSetting.Current.SawPath + "\\" + sawFileName, FileMode.Create, FileAccess.Write);
                string filePath = CuttingSetting.Current.TmpMDBPath + "\\" + cuttingSawFileRelationPlus.SawFileName;

            //byte[] b = Encoding.UTF8.GetBytes(sawFile);
            //byte[] b = Encoding.GetEncoding(866).GetBytes(sawFile);
            
            byte[] b =  Encoding.Default.GetBytes(sawFile);
            FileStream fileStream = new FileStream(filePath,FileMode.Create);
            fileStream.Write(b, 0, b.Length);
            fileStream.Flush();
            fileStream.Close();
                //StreamWriter sw = new StreamWriter(filePath,Encoding.);
                //sw.Write(sawFile);
                //sw.Flush();
                //sw.Close();

        }

        [TestMethod]
        public void TestReadFile()
        {
            FileStream fs = new FileStream(@"D:\transfer_data\cadpool\Online\105369(1)_1.saw", FileMode.Open);
            byte[] bytes = new byte[fs.Length];
            fs.Read(bytes, 0, bytes.Length);

            string s = Encoding.GetEncoding(866).GetString(bytes);

            Console.WriteLine(s);
        }

        [TestMethod]
        public void TestEncoding()
        {
            //FileStream fs = new FileStream(@"D:\99_信息化\105369(1).saw", FileMode.Open);
            Console.WriteLine(Encoding.Default.EncodingName);
            Console.WriteLine(Encoding.Default.CodePage);
        }

        [TestMethod]
        public void TestGetBatchGroupPlusByStatus()
        {
            List<BatchGroupPlus> listBatchGroupPlus = lineControlCuttingServicePlus.GetBatchGroupPlusByStatusAndMinBatchIndex(BatchGroupPlusStatus.WaitingForProduction);
            Console.WriteLine(listBatchGroupPlus.Count);
        }

        [TestMethod]
        public void TestBulkUpdateBatchGroupPlus()
        {
            BatchGroupPlus batchGroupPlus = new BatchGroupPlus();
            batchGroupPlus.LineId = 1;
            batchGroupPlus.BatchIndex = 1;
            batchGroupPlus.BatchName = "1";
            batchGroupPlus.CreatedTime = Convert.ToDateTime("2021-01-28");
            batchGroupPlus.PlanDate = Convert.ToDateTime("2021-01-28");
            batchGroupPlus.StartLoadingTime = DateTime.Now;
            batchGroupPlus.Status = (int)BatchGroupPlusStatus.InProduction;
            batchGroupPlus.UpdatedTime = DateTime.Now;

            List<BatchGroupPlus> listBatchGroupPlus = new List<BatchGroupPlus>();
            listBatchGroupPlus.Add(batchGroupPlus);

            lineControlCuttingServicePlus.BulkUpdateBatchGroupPlus(listBatchGroupPlus);
        }

        [TestMethod]
        public void TestPLCReadLong()
        {
            string ip = ConfigurationSettings.AppSettings["PlcIp"];
            //string TriggerInDbAddr = ConfigurationSettings.AppSettings["TriggerInDbAddr"];
            string TrrigerOutAddr = "DB30.24";

            PlcOperatorHelper plc = PlcOperatorHelper.GetInstance();
            if (plc.Connect(ip))
            {
                int value = plc.ReadLong(TrrigerOutAddr);
                Console.Write(value);
            }
            else
            {
                Console.Write("连接失败");
            }
        }

        [TestMethod]
        public void TestPLCReadStr()
        {
            string ip = ConfigurationSettings.AppSettings["PlcIp"];

            PlcOperatorHelper plc = PlcOperatorHelper.GetInstance();
            if (plc.Connect(ip))
            {
                string value = plc.ReadString("",20);
                Console.Write(value);
            }
            else
            {
                Console.Write("连接失败");
            }
        }

        [TestMethod]
        public void TestPLCWriteInt()
        {
            string ip = ConfigurationSettings.AppSettings["PlcIp"];
            PlcOperatorHelper plc = PlcOperatorHelper.GetInstance();

            string TriggerOutAddr = "DB30.0";

            string TriggerInAddr = "DB30.12";
            string ResultAddr = "DB30.16";

            if (plc.Connect(ip))
            {
                plc.Write(ResultAddr, 10);
                plc.Write(TriggerInAddr, 537);
            }
            else
            {
                Console.Write("连接失败");
            }
        }

        [TestMethod]
        public void TestRequestPiler()
        {
            List<BatchGroupPlus> listBatchGroupPlus = new List<BatchGroupPlus>();
            BatchGroupPlus batchGroupPlus = new BatchGroupPlus();
            batchGroupPlus.BatchIndex = 1;
            batchGroupPlus.BatchName = "1";
            batchGroupPlus.CreatedTime = DateTime.Now;
            batchGroupPlus.LineId = 1;
            batchGroupPlus.PlanDate = DateTime.Now;
            batchGroupPlus.StartLoadingTime = DateTime.Now;
            batchGroupPlus.Status = 1;
            batchGroupPlus.UpdatedTime = DateTime.Now;
            listBatchGroupPlus.Add(batchGroupPlus);

            BatchGroupPlus batchGroupPlus1 = listBatchGroupPlus[0];
            batchGroupPlus1.BatchIndex = 2;

            BatchGroupPlus batchGroupPlus2 = listBatchGroupPlus[0];
            Console.Write(batchGroupPlus2.BatchIndex);
        }

        [TestMethod]
        public void TestConvertBool()
        {
            bool flag = false;
            Console.Write(Convert.ToInt16(flag));
        }

        [TestMethod]
        public void TestWMSRequest2()
        {
            WMSService wmsService = new WMSService();
            //向WMS要料
            WMSInParams wmsInParams = new WMSInParams();
            wmsInParams.ReqType = 5;//要料
            wmsInParams.ProductCode = "TEST_18_218S";
            wmsInParams.Count = 40;
            wmsInParams.ToStation = 1003;

            ResultMsg resultMsg = wmsService.ApplyWMSTask(wmsInParams);

            Console.WriteLine($"Status:{resultMsg.Status}");
            Console.WriteLine($"ReqId:{resultMsg.ReqId}");
            Console.WriteLine($"Message:{resultMsg.Message}");
        }

        [TestMethod]
        public void TestWMSRequest()
        {
            //WMSService wmsService = new WMSService();
            ////向WMS要料
            //WMSInParams wmsInParams = new WMSInParams();
            //wmsInParams.ReqType = 2;//要料
            //wmsInParams.ProductCode = "TEST_18_218S";
            //wmsInParams.Count = 15;
            //wmsInParams.ToStation = 1003;

            //ResultMsg resultMsg = wmsService.ApplyWMSTask(wmsInParams);

            //Console.WriteLine($"Status:{resultMsg.Status}");
            //Console.WriteLine($"ReqId:{resultMsg.ReqId}");
            //Console.WriteLine($"Message:{resultMsg.Message}");
            //地址块
            string TriggerOutAddr1002 = ConfigurationSettings.AppSettings["TriggerOutAddr1002"];
            string PlatformNoAddr1002 = ConfigurationSettings.AppSettings["PlatformNoAddr1002"];

            string TriggerInAddr1002 = ConfigurationSettings.AppSettings["TriggerInAddr1002"];
            string ResultAddr1002 = ConfigurationSettings.AppSettings["ResultAddr1002"];

            int TriggerOutValue = 0;//PLC触发值

            bool flag = requestPilerByStatus(BatchGroupPlusStatus.WaitingForProduction, ResultAddr1002, TriggerInAddr1002, TriggerOutValue);
            Console.WriteLine(flag);

            
        }

        private bool requestPilerByStatus(BatchGroupPlusStatus batchGroupPlusStatus, string ResultAddr1002, string TriggerInAddr1002, int TriggerOutValue)
        {
            string ip = ConfigurationSettings.AppSettings["PlcIp"];

            PlcOperatorHelper plc = PlcOperatorHelper.GetInstance();

            if (plc.Connect(ip))
            {
                WMSService wmsService = new WMSService();

                //向WMS请求要料结果标识
                bool reqFlag = false;

                //获取一条生产中等级最高的批次信息
                List<BatchGroupPlus> listBatchGroupPlus = lineControlCuttingServicePlus.GetBatchGroupPlusByStatusAndMinBatchIndex(batchGroupPlusStatus);
                if (listBatchGroupPlus.Count > 0)
                {
                    BatchGroupPlus batchGroupPlus = listBatchGroupPlus[0];
                    string batchName = batchGroupPlus.BatchName;

                    //根据批次号获取一条批次生产详细数据
                    List<BatchProductionDetails> listBatchProdutionDetails = lineControlCuttingServicePlus.
                        GetBatchProductionDetailsByBatchName(batchName);
                    if (listBatchProdutionDetails.Count < 1)
                    {
                        lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog("1002", TriggerType.LineControl,
                            $"BatchProductionDetails表中不存在批次号：{batchName}", LogType.ABNORMAL));
                        return reqFlag;
                    }
                    BatchProductionDetails batchProductionDetails = listBatchProdutionDetails[0];

                    //仍需板件数量是否大于零
                    if (batchProductionDetails.DifferenceNumber > 0)
                    {
                        //向WMS要料
                        WMSInParams wmsInParams = new WMSInParams();
                        wmsInParams.ReqType = 2;//要料
                        wmsInParams.ProductCode = batchProductionDetails.ProductCode;
                        wmsInParams.Count = batchProductionDetails.Total;
                        wmsInParams.ToStation = 1003;

                        ResultMsg resultMsg = wmsService.ApplyWMSTask(wmsInParams);

                        reqFlag = resultMsg.Status == 200;

                        if (reqFlag)
                        {
                            lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog("1002", TriggerType.LineControl, $"{batchName}批次向WMS要料成功，" +
                                $"请求信息：ProductCode={batchProductionDetails.ProductCode}，Count={batchProductionDetails.Total}，" +
                                $"ToStation={wmsInParams.ToStation}；WMS回馈信息：Status={resultMsg.Status},ReqId={resultMsg.ReqId}，" +
                                $"Message={resultMsg.Message}", LogType.GENERAL));

                            batchGroupPlus.StartLoadingTime = DateTime.Now;
                            batchGroupPlus.Status = (int)BatchGroupPlusStatus.InProduction;
                            batchGroupPlus.UpdatedTime = DateTime.Now;
                            //更新批次状态
                            lineControlCuttingServicePlus.BulkUpdateBatchGroupPlus(listBatchGroupPlus);

                            plc.Write(ResultAddr1002, (int)NJIS.FPZWS.LineControl.Manager.PLCResultEnum.NORMAL);
                            lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog("1002", TriggerType.LineControl, $"回馈Result：{(int)NJIS.FPZWS.LineControl.Manager.PLCResultEnum.NORMAL}"
                                , LogType.GENERAL));

                            plc.Write(TriggerInAddr1002, TriggerOutValue);
                            lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog("1002", TriggerType.LineControl, $"回馈TriggerIn：{TriggerOutValue}"
                                , LogType.GENERAL));

                            List<WMS_Task> listWMSTask = lineControlCuttingServicePlus.GetWMSTaskByReqId(resultMsg.ReqId);
                            if (listWMSTask.Count > 0)
                            {
                                WMS_Task wmsTask = listWMSTask[0];
                                int differenceNumber = batchProductionDetails.DifferenceNumber - (int)wmsTask.Amount;
                                batchProductionDetails.DifferenceNumber = differenceNumber > 0 ? differenceNumber : 0;
                                lineControlCuttingServicePlus.BulkUpdateBatchProductionDetails(listBatchProdutionDetails);
                            }
                            else
                            {
                                lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog("1002", TriggerType.LineControl,
                                    $"WMS_Task表中ReqId：{resultMsg.ReqId}不存在，BatchProductionDetails表仍需数量更新失败", LogType.ABNORMAL));
                            }
                        }
                        else
                        {
                            plc.Write(ResultAddr1002, (int)NJIS.FPZWS.LineControl.Manager.PLCResultEnum.ABNORMAL);
                            lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog("1002", TriggerType.LineControl, $"回馈Result：" +
                                $"{(int)NJIS.FPZWS.LineControl.Manager.PLCResultEnum.ABNORMAL},异常信息：{batchName}批次向WMS要料失败," +
                                $"请求信息：ProductCode={batchProductionDetails.ProductCode}，Count={batchProductionDetails.Total}，" +
                                $"ToStation={wmsInParams.ToStation}；WMS回馈信息：Status={resultMsg.Status},ReqId={resultMsg.ReqId}，" +
                                $"Message={resultMsg.Message}", LogType.ABNORMAL));

                            plc.Write(TriggerInAddr1002, TriggerOutValue);
                            lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog("1002", TriggerType.LineControl, $"回馈TriggerIn：{TriggerOutValue}"
                                , LogType.GENERAL));
                        }
                    }
                }

                //List<BatchGroupPlus> listBatchGroupPlus2 = lineControlCuttingServicePlus.GetBatchGroupPlusByStatusAndMinBatchIndex(BatchGroupPlusStatus.
                //    WaitingForProduction);
                //BatchGroupPlus batchGroupPlus2 = listBatchGroupPlus2[0];

                //if (reqFlag)
                //{
                //    BatchGroupPlus batchGroupPlus3 = new BatchGroupPlus();
                //    batchGroupPlus3.LineId = batchGroupPlus2.LineId;
                //    batchGroupPlus3.BatchIndex = batchGroupPlus2.BatchIndex;
                //    batchGroupPlus3.BatchName = batchGroupPlus2.BatchName;
                //    batchGroupPlus3.CreatedTime = batchGroupPlus2.CreatedTime;
                //    batchGroupPlus3.PlanDate = batchGroupPlus2.PlanDate;
                //    batchGroupPlus3.StartLoadingTime = DateTime.Now;
                //    batchGroupPlus3.Status = (int)BatchGroupPlusStatus.InProduction;
                //    batchGroupPlus3.UpdatedTime = DateTime.Now;

                //    List<BatchGroupPlus> listBatchGroupPlus3 = new List<BatchGroupPlus>();
                //    listBatchGroupPlus.Add(batchGroupPlus3);

                //    lineControlCuttingServicePlus.BulkUpdateBatchGroupPlus(listBatchGroupPlus3);

                //    plc.Write(ResultAddr1002, (int)PLCResultEnum.NORMAL);
                //    lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog("1002", TriggerType.LineControl, $"回馈Result：{(int)PLCResultEnum.NORMAL}"
                //        , LogType.GENERAL));

                //    plc.Write(TriggerInAddr1002, TriggerOutValue);
                //    lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog("1002", TriggerType.LineControl, $"回馈TriggerIn：{TriggerOutValue}"
                //        , LogType.GENERAL));
                //}
                //else
                //{
                //    plc.Write(ResultAddr1002, (int)PLCResultEnum.ABNORMAL);
                //    lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog("1002", TriggerType.LineControl, $"回馈Result：" +
                //        $"{(int)PLCResultEnum.ABNORMAL},异常信息：向WMS要料失败"
                //        , LogType.ABNORMAL));

                //    plc.Write(TriggerInAddr1002, TriggerOutValue);
                //    lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog("1002", TriggerType.LineControl, $"回馈TriggerIn：{TriggerOutValue}"
                //        , LogType.GENERAL));
                //}

                return reqFlag;
            }
            else
            {
                Console.Write("连接失败");
                return false;
            }

            
        }

        [TestMethod]
        public void TestWMSBack()
        {
            WMSService wmsService = new WMSService();
            //向WMS要料
            WMSInParams wmsInParams = new WMSInParams();
            wmsInParams.ReqType = 4;//余料回库
            wmsInParams.ProductCode = "TEST_18_218S";
            wmsInParams.Count = 15;
            wmsInParams.FromStation = 2006;
            wmsInParams.PilerNo = 78695;

            ResultMsg resultMsg = wmsService.ApplyWMSTask(wmsInParams);

            Console.WriteLine($"Status:{resultMsg.Status}");
            Console.WriteLine($"ReqId:{resultMsg.ReqId}");
            Console.WriteLine($"Message:{resultMsg.Message}");
        }

        [TestMethod]
        public void TestGetCuttingSawFileRelationPlusByCreateTime()
        {
            List<CuttingSawFileRelationPlus> listCuttingSawFileRelation = lineControlCuttingServicePlus.
                GetCuttingSawFileRelationPlusByCreateTime(DateTime.Now);
            
            Console.WriteLine(listCuttingSawFileRelation.Count);
        }

        [TestMethod]
        public void TestGetCuttingSawFileRelationByBatchNameAndMinSawIndex()
        {
            List<CuttingSawFileRelation> listCuttingSawFileRelation2 = lineControlCuttingServicePlus.
                        GetCuttingSawFileRelationByBatchNameAndMinStackIndex("HB_RX210313A1-1(1)");
            Console.WriteLine(listCuttingSawFileRelation2.Count);
        }

        [TestMethod]
        public void TestGetBatchProductionDetailsByBatchName()
        {
            List<BatchProductionDetails> list = lineControlCuttingServicePlus.GetBatchProductionDetailsByBatchName("HB_RX210308D1-2(1)");
            Console.WriteLine(list.Count);
        }

        [TestMethod]
        public void TestBulkUpdateBatchNamePilerNoBindByPilerNo()
        {
            List<BatchNamePilerNoBind> listBatchNamePilerNoBind = new List<BatchNamePilerNoBind>();
            BatchNamePilerNoBind batchNamePilerNoBind = new BatchNamePilerNoBind();
            batchNamePilerNoBind.BatchName = "HB_RX210308D1-2(1)";
            batchNamePilerNoBind.Count = 0;
            batchNamePilerNoBind.CreateTime = DateTime.Parse("2021-03-10 20:18 下午");
            batchNamePilerNoBind.HasUpProtect = true;
            batchNamePilerNoBind.Id = 19;
            batchNamePilerNoBind.PilerNo = 78781;
            batchNamePilerNoBind.ProductCode = "PA_18_982S";
            batchNamePilerNoBind.TaskId = 612736;
            listBatchNamePilerNoBind.Add(batchNamePilerNoBind);
            lineControlCuttingServicePlus.BulkUpdateBatchNamePilerNoBindByPilerNo(listBatchNamePilerNoBind);
        }

        [TestMethod]
        public void TestGetCuttingSawFileRelationByBatchNameAndBoardCount()
        {
            List<CuttingSawFileRelation> list = lineControlCuttingServicePlus.
                GetCuttingSawFileRelationByBatchNameAndBoardCount("HB_RX210308D1-2(1)", 1);
            Console.WriteLine(list.Count);
        }

        [TestMethod]
        public void TestGetUnRequestCuttingStackListByBatchName()
        {
            List<CuttingStackList> list = lineControlCuttingServicePlus.
                GetUnRequestCuttingStackListByBatchNameAndMinStackProductIndex("HB_RX210414A1-2(1)", CuttingStackListBatchType.AUTO);
            Console.WriteLine(list.Count);
        }

        [TestMethod]
        public void TestGetCuttingSawFileRelationByStackName()
        {
            List<CuttingSawFileRelation> list = lineControlCuttingServicePlus.
                GetCuttingSawFileRelationByStackName("HB_RX210316A1-3(1)-001", SawType.TYPE1);
            Console.WriteLine(list.Count);
        }

        [TestMethod]
        public void TestGetCuttingSawFileRelationByStackNameAndMinStackIndex()
        {
            List<CuttingSawFileRelation> listCuttingSawFileRelation = lineControlCuttingServicePlus.
                GetCuttingSawFileRelationByStackNameAndMinStackIndex("HB_RX210313A1-1(1)");
            Console.WriteLine(listCuttingSawFileRelation.Count);
        }

        [TestMethod]
        public void TestGetUsers()
        {
            List<users> listUsers = lineControlCuttingServicePlus.GetUsers();
            Console.WriteLine(listUsers.Count);
        }

        [TestMethod]
        public void TestGetSawNoLength()
        {
            ushort sawNoLength = Convert.ToUInt16(ConfigurationSettings.AppSettings["sawNoLength"]);
            Console.WriteLine(sawNoLength);
        }

        [TestMethod]
        public void TestMessageBox()
        {
            DialogResult dialogResult = MessageBox.Show("测试","测试2",MessageBoxButtons.OKCancel);
            Console.WriteLine(dialogResult);
        }

        [TestMethod]
        public void TestPasswordValitionForm()
        {
            PasswordValidationForm passwordValidationForm = new PasswordValidationForm();
            passwordValidationForm.ShowDialog();
            string psw = passwordValidationForm.psw;
            Console.WriteLine(passwordValidationForm.psw);
            Console.WriteLine(passwordValidationForm.dialogResult);
            Console.WriteLine(passwordValidationForm.result);
        }

        [TestMethod]
        public void TestGetCuttingStackListByPlanDate()
        {
            List<NJIS.FPZWS.LineControl.Cutting.Model.CuttingStackList> list = lineControlCuttingService.GetCuttingStackListByPlanDate(DateTime.Now.Date);
            Console.WriteLine(list[0].BatchName);
        }

        [TestMethod]
        public void TestGetPartFeedBackByBatchName()
        {
            List<PartFeedBack> list = lineControlCuttingServicePlus.GetPartFeedBackByBatchName("HB_RX210318A1-1(1)");
            Console.WriteLine(list[0].BatchName);
        }

        [TestMethod]
        public void TestGetCuttingSawFileRelationByPlacDate()
        {
            List<NJIS.FPZWS.LineControl.Cutting.Model.CuttingSawFileRelation> list = lineControlCuttingService.GetCuttingSawFileRelationByPlanDate(DateTime.Now.Date);
            Console.WriteLine(list[0].BatchName);
        }

        [TestMethod]
        public void TestGetCuttingSawFileRelationPlusByDeviceNameAndCreatedTime()
        {
            List<CuttingSawFileRelationPlus> list = lineControlCuttingServicePlus.GetCuttingSawFileRelationPlusByDeviceNameAndCreatedTime("0-240-07-8018", DateTime.Now);
            Console.WriteLine(list[0].SawFileName);
        }

        [TestMethod]
        public void TestGetCuttingSawFileRelationPlusByBatchNameAndSawType()
        {
            List<CuttingSawFileRelationPlus> list = lineControlCuttingServicePlus.
                GetCuttingSawFileRelationPlusByBatchNameAndSawType("HB_RX210326A1-9(1)", SawType.TYPE1);
            Console.WriteLine(list.Count);
        }

        [TestMethod]
        public void TestGetCuttingSawFileRelation()
        {
            //string stackNames = "";
            //List<CuttingSawFileRelationPlus> listCuttingSawFileRelationPlus = lineControlCuttingServicePlus.
            //    GetCuttingSawFileRelationPlusByBatchNameAndSawType("HB_RX210326A1-9(1)", SawType.TYPE2);
            //for (int i = 0; i < listCuttingSawFileRelationPlus.Count; i++)
            //{
            //    CuttingSawFileRelationPlus cuttingSawFileRelationPlus = listCuttingSawFileRelationPlus[0];
            //    stackNames += $"'{cuttingSawFileRelationPlus.StackName}'";

            //    if (i < listCuttingSawFileRelationPlus.Count - 1)
            //        stackNames += ",";
            //}

            //if (string.IsNullOrEmpty(stackNames))
            //    stackNames = "''";

            //List<CuttingSawFileRelation> list = lineControlCuttingServicePlus.
            //    GetCuttingSawFileRelation("HB_RX210326A1-9(1)", stackNames, SawType.TYPE1);

            //List<CuttingSawFileRelation> list = lineControlCuttingServicePlus.
            //GetCuttingSawFileRelation("HB_RX210414A1-1(1)", SawType.TYPE2);

            string batchName = "HB_RX210507A1-1(1)";
            //已下发给1-5号锯的锯切图名称
            string sawFileName = NJIS.FPZWS.LineControl.Manager.Service.Utils.PublicUtils.
                getSawFileName(lineControlCuttingServicePlus, batchName);

            //已下发给6号锯的垛
            string stackNames = NJIS.FPZWS.LineControl.Manager.Service.Utils.PublicUtils.
                getStackName6(lineControlCuttingServicePlus, batchName);

            List <CuttingSawFileRelation> list = lineControlCuttingServicePlus.
                GetCuttingSawFileRelation(batchName, sawFileName, stackNames, SawType.TYPE1,"SawFileName");

            Console.WriteLine(list.Count);
        }

        [TestMethod]
        public void TestGetMinStackProductIndexCuttingStackList()
        {
            string batchName = "HB_RX210415A1-4(1)";

            string stackNames = "";
            List<BatchNamePilerNoBind> listBatchNamePilerNoBind = lineControlCuttingServicePlus.
                GetBatchNamePilerNoBindByBatchName(batchName);
            for (int i = 0; i < listBatchNamePilerNoBind.Count; i++)
            {
                stackNames += $"'{listBatchNamePilerNoBind[i].StackName}'";
                if (i < listBatchNamePilerNoBind.Count - 1)
                {
                    stackNames += ",";
                }
            }

            List<CuttingSawFileRelationPlus> listCuttingSawFileRelationPlus = lineControlCuttingServicePlus.
                GetCuttingSawFileRelationPlusByBatchNameAndSawType(batchName, SawType.TYPE2);

            if (listCuttingSawFileRelationPlus.Count > 0)
                stackNames += ",";

            for (int i = 0; i < listCuttingSawFileRelationPlus.Count; i++)
            {
                stackNames += $"'{listCuttingSawFileRelationPlus[i].StackName}'";
                if (i < listCuttingSawFileRelationPlus.Count - 1)
                {
                    stackNames += ",";
                }
            }

            if (string.IsNullOrEmpty(stackNames))
                stackNames = "''";

            //获取垛生产顺序最小的未要料数据
            //List<CuttingStackList> listCuttingStackList = lineControlCuttingServicePlus.
            //    GetUnRequestCuttingStackListByBatchNameAndMinStackProductIndex(batchName,
            //    CuttingStackListBatchType.AUTO);
            List<CuttingStackList> listCuttingStackList = lineControlCuttingServicePlus.
                GetMinStackProductIndexCuttingStackList(batchName, stackNames, CuttingStackListBatchType.AUTO);

            Console.WriteLine(listCuttingStackList.Count);
        }

        [TestMethod]
        public void TestGetCuttingSawFileRelationPlus()
        {
            List<CuttingSawFileRelationPlus> list;
            //list = lineControlCuttingServicePlus.GetCuttingSawFileRelationPlus("HB_RX210415A1-4(1)-001",
            //    CuttingSawFileRelationPlusStatus.FeedingComplete,SawType.TYPE1);
            //list = lineControlCuttingServicePlus.GetCuttingSawFileRelationPlus(DateTime.Now.Date, "StackName", SawType.TYPE2);
            list = lineControlCuttingServicePlus.GetCuttingSawFileRelationPlus(DateTime.Now.Date, "StackName");
            Console.WriteLine(list.Count);
        }

        [TestMethod]
        public void TestGetStackNames()
        {
            string stackNames = NJIS.FPZWS.LineControl.Manager.Service.Utils.PublicUtils.
                getStackName(lineControlCuttingServicePlus, "HB_RX210415A1-4(1)");
            Console.WriteLine(stackNames);
        }
        
        [TestMethod]
        public void TestGetBatchGroupPlus()
        {
            List<BatchGroupPlus> listBatchGroupPlus = lineControlCuttingServicePlus.GetBatchGroupPlus(DateTime.Now.Date, "BatchName");
            Console.WriteLine(listBatchGroupPlus.Count);
        }

        [TestMethod]
        public void TestGetDeviceInfosByPlaceNo()
        {
            List<NJIS.FPZWS.LineControl.Cutting.Model.DeviceInfos> list = lineControlCuttingService.
                GetDeviceInfosByPlaceNo("3006");
            Console.WriteLine(list.Count);
        }

        [TestMethod]
        public void TestRequestPilerByStatus()
        {
            CuttingManagerService cuttingManagerService = new CuttingManagerService();
            cuttingManagerService.requestPilerByStatus(BatchGroupPlusStatus.WaitingForProduction,"","",2);
        }

        [TestMethod]
        public void TestGetNotCuttingData()
        {
            List< NJIS.FPZWS.LineControl.Cutting.Model.NotCuttingData> list = lineControlCuttingService.
                GetNotCuttingData("HB_RX210427A1-2(1)","");
            Console.WriteLine(list.Count);
        }
    }
}
