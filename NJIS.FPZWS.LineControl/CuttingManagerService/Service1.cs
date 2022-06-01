using NJIS.FPZWS.LineControl.Cutting.ServicePlus;
using NJIS.FPZWS.LineControl.Manager.Helpers;
using NJIS.FPZWS.LineControl.Manager.WMSWebReference;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CuttingManagerService
{
    public partial class Service1 : ServiceBase
    {
        Thread requestBoardThread;
        Thread requestPilerThread;
        Thread inPlaceThread;

        static string PlcIp = ConfigurationSettings.AppSettings["PlcIp"];

        ushort sawNoLength = Convert.ToUInt16(ConfigurationSettings.AppSettings["sawNoLength"]);

        PlcOperatorHelper plc = new PlcOperatorHelper(PlcIp);

        LineControlCuttingServicePlus lineControlCuttingServicePlus = new LineControlCuttingServicePlus();

        WMSService wmsService = new WMSService();

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
        }

        protected override void OnStop()
        {
        }

        /// <summary>
        /// 循环检查请求上垛
        /// </summary>
        private void requestPilerLoop()
        {
            while (true)
            {
                requestPiler();

                Thread.Sleep(int.Parse(ConfigurationSettings.AppSettings["sleepTime"]));
            }
        }

        /// <summary>
        /// 监控1002空闲要料请求
        /// </summary>
        private void requestPiler()
        {
            bool plcIsConnect = plc.CheckConnect();
            if (plcIsConnect)
            {
                //地址块
                string TriggerOutAddr1002 = ConfigurationSettings.AppSettings["TriggerOutAddr1002"];
                string PlatformNoAddr1002 = ConfigurationSettings.AppSettings["PlatformNoAddr1002"];

                string TriggerInAddr1002 = ConfigurationSettings.AppSettings["TriggerInAddr1002"];
                string ResultAddr1002 = ConfigurationSettings.AppSettings["ResultAddr1002"];

                //值
                int TriggerOutValue = plc.ReadLong(TriggerOutAddr1002);//PLC触发值
                int TriggerInValue = plc.ReadLong(TriggerInAddr1002);//线控触发值

                int triggerDifference = TriggerOutValue - TriggerInValue;

                //TriggerOutValue自增异常
                if (triggerDifference != 1 && triggerDifference != 0)
                {
                    lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog("1002", TriggerType.PLC,
                        $"TriggerOut值自增异常，自增量：{triggerDifference}，TriggerOut={TriggerOutValue}，TriggerIn={TriggerInValue}",
                        LogType.ABNORMAL));

                    return;
                }

                if (triggerDifference == 1)
                {
                    lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog("1002", TriggerType.PLC, $"1002号工位/站台要料请求，请求信息：" +
                        $"TriggerOut={TriggerOutValue}，TriggerIn={TriggerInValue}", LogType.GENERAL));

                    //检查是否有生产中的批次板件数量是否全部上完，是：不向WMS请求要料，返回false；否：向WMS请求要料，返回true
                    bool reqFlag = requestPilerByStatus(BatchGroupPlusStatus.InProduction, ResultAddr1002, TriggerInAddr1002, TriggerOutValue);
                    //如果没有生产中的批次，检查是否有待生产的批次，有：向WMS请求要料
                    if (!reqFlag)
                        requestPilerByStatus(BatchGroupPlusStatus.WaitingForProduction, ResultAddr1002, TriggerInAddr1002, TriggerOutValue);
                }

            }
            else
            {
                //string PlcIp = ConfigurationSettings.AppSettings["PlcIp"];

                plcIsConnect = plc.Connect(PlcIp);
                if (plcIsConnect)
                {
                    requestPiler();
                }
                else
                {
                    lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(PlcIp, TriggerType.LineControl,
                        $"连接PLC失败：{PlcIp}", LogType.ABNORMAL));
                }
            }
        }

        private bool requestPilerByStatus(BatchGroupPlusStatus batchGroupPlusStatus, string ResultAddr1002, string TriggerInAddr1002, int TriggerOutValue)
        {
            bool needPiler = false;//是否还需要上料，异常返回true（防止执行待生产要料）

            //根据BatchGroupPlusStatus（生产中、待生产）获取一条数据
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
                    //plc.Write(ResultAddr1002, (int)PLCResultEnum.ABNORMAL);
                    PublicUtils.PLCWriteInt(plc, ResultAddr1002, (int)PLCResultEnum.ABNORMAL);
                    lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog("1002", TriggerType.LineControl, $"回馈Result：" +
                        $"{(int)PLCResultEnum.ABNORMAL},异常信息：BatchProductionDetails表中不存在批次号：{batchName}", LogType.ABNORMAL));

                    //plc.Write(TriggerInAddr1002, TriggerOutValue);
                    PublicUtils.PLCWriteInt(plc, TriggerInAddr1002, TriggerOutValue);
                    lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog("1002", TriggerType.LineControl, $"回馈TriggerIn：{TriggerOutValue}"
                        , LogType.GENERAL));
                    return true;
                }
                BatchProductionDetails batchProductionDetails = listBatchProdutionDetails[0];

                //仍需板件数量是否大于零
                if (batchProductionDetails.DifferenceNumber > 0)
                {
                    needPiler = true;//需要上料

                    //获取垛生产顺序最小的未要料数据
                    List<CuttingStackList> listCuttingStackList = lineControlCuttingServicePlus.
                        GetUnRequestCuttingStackListByBatchNameAndMinStackProductIndex(batchName);
                    if (listCuttingStackList.Count > 0)
                    {
                        int requestCount = 0;
                        CuttingStackList cuttingStackList = listCuttingStackList[0];
                        string stackName = cuttingStackList.StackName;

                        List<CuttingSawFileRelation> listCuttingSawFileRelation = lineControlCuttingServicePlus.
                            GetCuttingSawFileRelationByStackName(stackName);
                        foreach (var item in listCuttingSawFileRelation)
                        {
                            requestCount += item.BoardCount;
                        }

                        if (requestCount > 0)
                        {
                            //向WMS要料
                            WMSInParams wmsInParams = new WMSInParams();
                            wmsInParams.ReqType = 5;//要料
                            wmsInParams.ProductCode = cuttingStackList.RawMaterialID;//batchProductionDetails.ProductCode;
                            wmsInParams.Count = requestCount;
                            wmsInParams.ToStation = 1003;

                            ResultMsg resultMsg = wmsService.ApplyWMSTask(wmsInParams);

                            bool reqFlag = resultMsg.Status == 200;

                            if (reqFlag)
                            {
                                lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog("1002", TriggerType.LineControl, $"{batchName}批次向" +
                                    $"WMS要料成功，请求信息：ReqType={wmsInParams.ReqType},ProductCode={wmsInParams.ProductCode}，Count={requestCount}，" +
                                    $"ToStation={wmsInParams.ToStation}；WMS回馈信息：Status={resultMsg.Status},ReqId={resultMsg.ReqId}，" +
                                    $"Message={resultMsg.Message}", LogType.GENERAL));

                                batchGroupPlus.StartLoadingTime = DateTime.Now;
                                batchGroupPlus.Status = (int)BatchGroupPlusStatus.InProduction;
                                batchGroupPlus.UpdatedTime = DateTime.Now;
                                //更新批次状态
                                lineControlCuttingServicePlus.BulkUpdateBatchGroupPlus(listBatchGroupPlus);

                                //更新批次生产详情状态
                                batchProductionDetails.Status = (int)BatchGroupPlusStatus.InProduction;
                                lineControlCuttingServicePlus.BulkUpdateBatchProductionDetails(listBatchProdutionDetails);

                                //更新batchProductionDetails表仍需数量
                                List<WMS_Task> listWMSTask = lineControlCuttingServicePlus.GetWMSTaskByReqId(resultMsg.ReqId);
                                if (listWMSTask.Count > 0)
                                {
                                    WMS_Task wmsTask = listWMSTask[0];
                                    int differenceNumber = batchProductionDetails.DifferenceNumber - (int)wmsTask.Amount;
                                    batchProductionDetails.DifferenceNumber = differenceNumber > 0 ? differenceNumber : 0;
                                    lineControlCuttingServicePlus.BulkUpdateBatchProductionDetails(listBatchProdutionDetails);

                                    //新增MES垛和WMS垛绑定信息
                                    BatchNamePilerNoBind batchNamePilerNoBind = new BatchNamePilerNoBind();
                                    batchNamePilerNoBind.BatchName = batchName;
                                    batchNamePilerNoBind.Count = (int)wmsTask.Amount;
                                    batchNamePilerNoBind.HasUpProtect = (bool)wmsTask.HasUpProtect;
                                    batchNamePilerNoBind.PilerNo = (int)wmsTask.PilerNo;
                                    batchNamePilerNoBind.ProductCode = wmsTask.ProductCode;
                                    batchNamePilerNoBind.TaskId = wmsTask.TaskId;
                                    batchNamePilerNoBind.StackName = stackName;
                                    batchNamePilerNoBind.ReqId = (long)wmsTask.ReqId;
                                    bool flag = lineControlCuttingServicePlus.InsertBatchNamePilerNoBind(batchNamePilerNoBind);

                                    if (flag)
                                    {
                                        //plc.Write(ResultAddr1002, (int)PLCResultEnum.NORMAL);
                                        PublicUtils.PLCWriteInt(plc, ResultAddr1002, (int)PLCResultEnum.NORMAL);
                                        lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog("1002", TriggerType.LineControl, $"回馈Result：" +
                                            $"{(int)PLCResultEnum.NORMAL}", LogType.GENERAL));

                                        //plc.Write(TriggerInAddr1002, TriggerOutValue);
                                        PublicUtils.PLCWriteInt(plc, TriggerInAddr1002, TriggerOutValue);
                                        lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog("1002", TriggerType.LineControl, $"回馈TriggerIn：" +
                                            $"{TriggerOutValue}", LogType.GENERAL));
                                    }
                                    else
                                    {
                                        //plc.Write(ResultAddr1002, (int)PLCResultEnum.ABNORMAL);
                                        PublicUtils.PLCWriteInt(plc, ResultAddr1002, (int)PLCResultEnum.ABNORMAL);
                                        lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog("1002", TriggerType.LineControl, $"回馈Result：" +
                                            $"{(int)PLCResultEnum.ABNORMAL}，异常信息：BatchNamePilerNoBind表数据新增失败！，", LogType.GENERAL));

                                        //plc.Write(TriggerInAddr1002, TriggerOutValue);
                                        PublicUtils.PLCWriteInt(plc, TriggerInAddr1002, TriggerOutValue);
                                        lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog("1002", TriggerType.LineControl, $"回馈TriggerIn：" +
                                            $"{TriggerOutValue}", LogType.GENERAL));
                                    }

                                }
                                else
                                {
                                    //plc.Write(ResultAddr1002, (int)PLCResultEnum.ABNORMAL);
                                    PublicUtils.PLCWriteInt(plc, ResultAddr1002, (int)PLCResultEnum.ABNORMAL);
                                    lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog("1002", TriggerType.LineControl, $"回馈Result：" +
                                        $"{(int)PLCResultEnum.ABNORMAL}，异常信息：WMS_Task表中ReqId：{resultMsg.ReqId}不存在，任务终止！", LogType.ABNORMAL));

                                    //plc.Write(TriggerInAddr1002, TriggerOutValue);
                                    PublicUtils.PLCWriteInt(plc, TriggerInAddr1002, TriggerOutValue);
                                    lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog("1002", TriggerType.LineControl, $"回馈TriggerIn：" +
                                        $"{TriggerOutValue}", LogType.GENERAL));
                                }
                            }
                            else
                            {
                                //plc.Write(ResultAddr1002, (int)PLCResultEnum.ABNORMAL);
                                PublicUtils.PLCWriteInt(plc, ResultAddr1002, (int)PLCResultEnum.ABNORMAL);
                                lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog("1002", TriggerType.LineControl, $"回馈Result：" +
                                    $"{(int)PLCResultEnum.ABNORMAL},异常信息：{batchName}批次向WMS要料失败," +
                                    $"请求信息：ProductCode={wmsInParams.ProductCode}，Count={requestCount}，" +
                                    $"ToStation={wmsInParams.ToStation}；WMS回馈信息：Status={resultMsg.Status},ReqId={resultMsg.ReqId}，" +
                                    $"Message={resultMsg.Message}", LogType.ABNORMAL));

                                //plc.Write(TriggerInAddr1002, TriggerOutValue);
                                PublicUtils.PLCWriteInt(plc, TriggerInAddr1002, TriggerOutValue);
                                lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog("1002", TriggerType.LineControl, $"回馈TriggerIn：{TriggerOutValue}"
                                    , LogType.GENERAL));
                            }
                        }
                        else
                        {
                            //plc.Write(ResultAddr1002, (int)PLCResultEnum.ABNORMAL);
                            PublicUtils.PLCWriteInt(plc, ResultAddr1002, (int)PLCResultEnum.ABNORMAL);
                            lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog("1002", TriggerType.LineControl, $"回馈Result：" +
                                $"{(int)PLCResultEnum.ABNORMAL},异常信息：CuttingSawFileRelation表中垛号为：{stackName}的板件数量为：{requestCount}",
                                LogType.ABNORMAL));

                            //plc.Write(TriggerInAddr1002, TriggerOutValue);
                            PublicUtils.PLCWriteInt(plc, TriggerInAddr1002, TriggerOutValue);
                            lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog("1002", TriggerType.LineControl, $"回馈TriggerIn：{TriggerOutValue}"
                                , LogType.GENERAL));
                        }
                    }
                    else
                    {
                        //plc.Write(ResultAddr1002, (int)PLCResultEnum.ABNORMAL);
                        PublicUtils.PLCWriteInt(plc, ResultAddr1002, (int)PLCResultEnum.ABNORMAL);
                        lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog("1002", TriggerType.LineControl, $"回馈Result：" +
                            $"{(int)PLCResultEnum.ABNORMAL},异常信息：CuttingStackList表中不存在未要料的数据，批次号：{batchName}", LogType.ABNORMAL));

                        //plc.Write(TriggerInAddr1002, TriggerOutValue);
                        PublicUtils.PLCWriteInt(plc, TriggerInAddr1002, TriggerOutValue);
                        lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog("1002", TriggerType.LineControl, $"回馈TriggerIn：{TriggerOutValue}"
                            , LogType.GENERAL));
                    }
                }
            }
            else
            {
                if (batchGroupPlusStatus == BatchGroupPlusStatus.WaitingForProduction)
                {
                    PublicUtils.PLCWriteInt(plc, ResultAddr1002, (int)PLCResultEnum.RETURN);
                    lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog("1002", TriggerType.LineControl, $"回馈Result：" +
                        $"{(int)PLCResultEnum.RETURN},详细信息：无生产任务！", LogType.GENERAL));

                    PublicUtils.PLCWriteInt(plc, TriggerInAddr1002, TriggerOutValue);
                    lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog("1002", TriggerType.LineControl, $"回馈TriggerIn：{TriggerOutValue}"
                        , LogType.GENERAL));
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

            return needPiler;
        }

        /// <summary>
        /// 循环检查10、11到位请求
        /// </summary>
        private void inPlaceLoop()
        {
            while (true)
            {
                inPlace();

                Thread.Sleep(int.Parse(ConfigurationSettings.AppSettings["sleepTime"]));
            }
        }

        /// <summary>
        /// 10、11到位
        /// </summary>
        private void inPlace()
        {
            bool plcIsConnect = plc.CheckConnect();
            if (plcIsConnect)
            {
                //10地址块
                string TriggerOutAddr10 = ConfigurationSettings.AppSettings["TriggerOutAddr10"];
                string TriggerInAddr10 = ConfigurationSettings.AppSettings["TriggerInAddr10"];
                string ResultAddr10 = ConfigurationSettings.AppSettings["ResultAddr10"];

                //11地址块
                string TriggerOutAddr11 = ConfigurationSettings.AppSettings["TriggerOutAddr11"];
                string TriggerInAddr11 = ConfigurationSettings.AppSettings["TriggerInAddr11"];
                string ResultAddr11 = ConfigurationSettings.AppSettings["ResultAddr11"];

                //10值
                int TriggerOutValue10 = plc.ReadLong(TriggerOutAddr10);//10号站台PLC触发值
                int TriggerInValue10 = plc.ReadLong(TriggerInAddr10);//10号站台线控触发值

                //11值
                int TriggerOutValue11 = plc.ReadLong(TriggerOutAddr11);//11号站台PLC触发值
                int TriggerInValue11 = plc.ReadLong(TriggerInAddr11);//11号站台线控触发值

                int triggerDifference10 = TriggerOutValue10 - TriggerInValue10;

                //TriggerOutValue10自增异常
                if (triggerDifference10 != 1 && triggerDifference10 != 0)
                    lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog("10", TriggerType.PLC,
                        $"TriggerOut值自增异常，自增量：{triggerDifference10}，TriggerOut={TriggerOutValue10}，TriggerIn={TriggerInValue10}",
                        LogType.ABNORMAL));

                if (triggerDifference10 == 1)
                {
                    //地址块
                    string PlatformNoAddr = ConfigurationSettings.AppSettings["PlatformNoAddr10"];
                    string PilerNoAddr = ConfigurationSettings.AppSettings["PilerNoAddr10"];
                    string SumCountAddr = ConfigurationSettings.AppSettings["SumCountAddr10"];
                    string CarryPanelAddr = ConfigurationSettings.AppSettings["CarryPanelAddr10"];

                    string ResultAddr = ConfigurationSettings.AppSettings["ResultAddr10"];

                    //值
                    int PlatformNo = plc.ReadLong(PlatformNoAddr);
                    int PilerNo = plc.ReadLong(PilerNoAddr);
                    //int SumCount = plc.ReadLong(SumCountAddr);
                    //int CarryPanel = plc.ReadLong(CarryPanelAddr);

                    //inPlaceExamine("10", ResultAddr, TriggerInAddr10, PlatformNo, PilerNo, SumCount, CarryPanel, TriggerOutValue10);
                    inPlaceResponse("10", ResultAddr, TriggerInAddr10, PlatformNo, PilerNo, SumCountAddr, CarryPanelAddr, TriggerOutValue10);
                }

                int triggerDifference11 = TriggerOutValue11 - TriggerInValue11;
                //TriggerOutValue11自增异常
                if (triggerDifference11 != 1 && triggerDifference11 != 0)
                    lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog("11", TriggerType.PLC,
                        $"TriggerOut值自增异常，自增量：{triggerDifference11}，TriggerOut={TriggerOutValue11}，TriggerIn={TriggerInValue11}",
                        LogType.ABNORMAL));

                if (triggerDifference11 == 1)
                {
                    //地址块
                    string PlatformNoAddr = ConfigurationSettings.AppSettings["PlatformNoAddr11"];
                    string PilerNoAddr = ConfigurationSettings.AppSettings["PilerNoAddr11"];
                    string SumCountAddr = ConfigurationSettings.AppSettings["SumCountAddr11"];
                    string CarryPanelAddr = ConfigurationSettings.AppSettings["CarryPanelAddr11"];

                    string ResultAddr = ConfigurationSettings.AppSettings["ResultAddr11"];

                    //值
                    int PlatformNo = plc.ReadLong(PlatformNoAddr);
                    int PilerNo = plc.ReadLong(PilerNoAddr);
                    //int SumCount = plc.ReadLong(SumCountAddr);
                    //int CarryPanel = plc.ReadLong(CarryPanelAddr);

                    //inPlaceExamine("11", ResultAddr, TriggerInAddr11, PlatformNo, PilerNo, SumCount, CarryPanel, TriggerOutValue11);
                    inPlaceResponse("11", ResultAddr, TriggerInAddr11, PlatformNo, PilerNo, SumCountAddr, CarryPanelAddr, TriggerOutValue11);
                }
            }
            else
            {
                //string PlcIp = ConfigurationSettings.AppSettings["PlcIp"];

                plcIsConnect = plc.Connect(PlcIp);
                if (plcIsConnect)
                {
                    inPlace();
                }
                else
                {
                    lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(PlcIp, TriggerType.LineControl,
                        $"连接PLC失败：{PlcIp}", LogType.ABNORMAL));
                }
            }
        }

        /// <summary>
        /// 10\11到位PLC信息验证
        /// </summary>
        /// <param name="station"></param>
        /// <param name="ResultAddr"></param>
        /// <param name="TriggerInAddr"></param>
        /// <param name="PlatformNo"></param>
        /// <param name="PilerNo"></param>
        /// <param name="SumCount"></param>
        /// <param name="CarryPanel"></param>
        /// <param name="TriggerOutValue"></param>
        /// <returns></returns>
        private bool inPlaceExamine(string station, string ResultAddr, string TriggerInAddr, int PlatformNo, int PilerNo, int SumCount, int CarryPanel,
            int TriggerOutValue)
        {
            lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.PLC, $"{station}号工位/站台到位请求，到位信息：PlatformNo={PlatformNo}" +
                $",PilerNo={PilerNo},SumCount={SumCount},CarryPanel={CarryPanel}", LogType.GENERAL));

            bool flag = false;
            /*
             根据垛号从WMS_Task表获取数据
             比对参数是否相等
             */
            List<WMS_Task> listWMSTask = lineControlCuttingServicePlus.GetWMSTaskByPilerNo(PilerNo);
            if (listWMSTask.Count > 0)
            {
                WMS_Task wmsTask = listWMSTask[0];
                int hasUpProtect = Convert.ToInt16(wmsTask.HasUpProtect);

                if (wmsTask.Amount == SumCount && hasUpProtect == CarryPanel)
                {
                    flag = true;

                    //获取一条等级最高的生产中的批次信息
                    List<BatchGroupPlus> listBatchGroupPlus = lineControlCuttingServicePlus.GetBatchGroupPlusByStatusAndMinBatchIndex(BatchGroupPlusStatus.InProduction);
                    if (listBatchGroupPlus.Count > 0)
                    {
                        string batchName = listBatchGroupPlus[0].BatchName;

                        BatchNamePilerNoBind batchNamePilerNoBind = new BatchNamePilerNoBind();
                        batchNamePilerNoBind.BatchName = batchName;
                        batchNamePilerNoBind.Count = SumCount;
                        batchNamePilerNoBind.HasUpProtect = Convert.ToBoolean(CarryPanel);
                        batchNamePilerNoBind.PilerNo = PilerNo;
                        batchNamePilerNoBind.ProductCode = wmsTask.ProductCode;
                        batchNamePilerNoBind.TaskId = wmsTask.TaskId;
                        lineControlCuttingServicePlus.InsertBatchNamePilerNoBind(batchNamePilerNoBind);

                        //plc.Write(ResultAddr, (int)PLCResultEnum.NORMAL);
                        PublicUtils.PLCWriteInt(plc, ResultAddr, (int)PLCResultEnum.NORMAL);
                        lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.LineControl,
                        $"回馈Result：{(int)PLCResultEnum.NORMAL}", LogType.GENERAL));
                    }
                    else
                    {
                        //plc.Write(ResultAddr, (int)PLCResultEnum.ABNORMAL);
                        PublicUtils.PLCWriteInt(plc, ResultAddr, (int)PLCResultEnum.ABNORMAL);
                        lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.LineControl,
                    $"回馈Result：{(int)PLCResultEnum.NORMAL},异常信息：BatchGroupPlus中没有生产中的批次信息，BatchNamePilerNoBind数据新增失败", LogType.ABNORMAL));
                    }



                    //plc.Write(TriggerInAddr, TriggerOutValue);
                    PublicUtils.PLCWriteInt(plc, TriggerInAddr, TriggerOutValue);
                    lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.LineControl,
                    $"回馈TriggerIn：{TriggerOutValue}", LogType.GENERAL));
                }
                else
                {
                    flag = false;

                    //plc.Write(ResultAddr, (int)PLCResultEnum.ABNORMAL);
                    PublicUtils.PLCWriteInt(plc, ResultAddr, (int)PLCResultEnum.ABNORMAL);
                    lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.LineControl,
                    $"回馈Result：{(int)PLCResultEnum.ABNORMAL}，异常信息：PLC携带信息与WMS_Task信息不匹配，PlatformNo：{PlatformNo}，" +
                    $"PilerNo：{PilerNo},SumCount：{SumCount}|{wmsTask.Amount}，CarryPanel：{CarryPanel}|{hasUpProtect}", LogType.ABNORMAL));

                    //plc.Write(TriggerInAddr, TriggerOutValue);
                    PublicUtils.PLCWriteInt(plc, TriggerInAddr, TriggerOutValue);
                    lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.LineControl,
                    $"回馈TriggerIn：{TriggerOutValue}", LogType.GENERAL));
                }
            }
            else
            {
                flag = false;

                //plc.Write(ResultAddr, (int)PLCResultEnum.ABNORMAL);
                PublicUtils.PLCWriteInt(plc, ResultAddr, (int)PLCResultEnum.ABNORMAL);
                lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.LineControl,
                $"回馈Result：{(int)PLCResultEnum.ABNORMAL}，异常信息：WMS_Task表中不存在垛号：{PilerNo}", LogType.ABNORMAL));

                //plc.Write(TriggerInAddr, TriggerOutValue);
                PublicUtils.PLCWriteInt(plc, TriggerInAddr, TriggerOutValue);
                lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.LineControl,
                $"回馈TriggerIn：{TriggerOutValue}", LogType.GENERAL));
            }
            return flag;
        }

        /// <summary>
        /// 10\11到位PLC信息反馈
        /// </summary>
        /// <param name="station"></param>
        /// <param name="ResultAddr"></param>
        /// <param name="TriggerInAddr"></param>
        /// <param name="PlatformNo"></param>
        /// <param name="PilerNo"></param>
        /// <param name="SumCountAddr"></param>
        /// <param name="CarryPanelAddr"></param>
        /// <param name="TriggerOutValue"></param>
        private void inPlaceResponse(string station, string ResultAddr, string TriggerInAddr, int PlatformNo, int PilerNo, string SumCountAddr, string CarryPanelAddr,
            int TriggerOutValue)
        {
            lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.PLC, $"{station}号工位/站台" +
                $"到位请求，到位信息：PlatformNo={PlatformNo},PilerNo={PilerNo}", LogType.GENERAL));

            List<BatchNamePilerNoBind> listBatchNamePilerNoBind = lineControlCuttingServicePlus
                .GetBatchNamePilerNoBindByPilerNo(PilerNo);
            if (listBatchNamePilerNoBind.Count < 1)
            {
                //plc.Write(ResultAddr, (int)PLCResultEnum.ABNORMAL);
                PublicUtils.PLCWriteInt(plc, ResultAddr, (int)PLCResultEnum.ABNORMAL);
                lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.LineControl,
                $"回馈Result：{(int)PLCResultEnum.ABNORMAL}，异常信息：BatchNamePilerNoBind表中不存在垛号：{PilerNo}", LogType.ABNORMAL));

                //plc.Write(TriggerInAddr, TriggerOutValue);
                PublicUtils.PLCWriteInt(plc, TriggerInAddr, TriggerOutValue);
                lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.LineControl,
                $"回馈TriggerIn：{TriggerOutValue}", LogType.GENERAL));
                return;
            }

            BatchNamePilerNoBind batchNamePilerNoBind = listBatchNamePilerNoBind[0];
            long reqId = batchNamePilerNoBind.ReqId;

            List<WMS_Task> listWMSTask = lineControlCuttingServicePlus.GetWMSTaskByReqId(reqId);
            if (listWMSTask.Count > 0)
            {
                WMS_Task wmsTask = listWMSTask[0];
                //int hasUpProtect = Convert.ToInt16(wmsTask.HasUpProtect);

                //获取一条等级最高的生产中的批次信息
                //List<BatchGroupPlus> listBatchGroupPlus = lineControlCuttingServicePlus.
                //GetBatchGroupPlusByStatusAndMinBatchIndex(BatchGroupPlusStatus.InProduction);
                //if (listBatchGroupPlus.Count > 0)
                //{
                //    string batchName = listBatchGroupPlus[0].BatchName;

                //    BatchNamePilerNoBind batchNamePilerNoBind = new BatchNamePilerNoBind();
                //    batchNamePilerNoBind.BatchName = batchName;
                //    batchNamePilerNoBind.Count = (int)wmsTask.Amount;
                //    batchNamePilerNoBind.HasUpProtect = (bool)wmsTask.HasUpProtect;
                //    batchNamePilerNoBind.PilerNo = PilerNo;
                //    batchNamePilerNoBind.ProductCode = wmsTask.ProductCode;
                //    batchNamePilerNoBind.TaskId = wmsTask.TaskId;
                //    lineControlCuttingServicePlus.InsertBatchNamePilerNoBind(batchNamePilerNoBind);

                //    plc.Write(SumCountAddr, (int)wmsTask.Amount);
                //    lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.LineControl,
                //$"回馈SumCount：{(int)wmsTask.Amount}", LogType.GENERAL));

                //int CarryPanelForPlc = Convert.ToInt16(wmsTask.HasUpProtect);
                ////WMSHasUpProtect=0代表没有保护板，因plc不能识别0故转换成2
                //if (!(bool)wmsTask.HasUpProtect)
                //{
                //    CarryPanelForPlc = 2;
                //}

                //plc.Write(CarryPanelAddr, CarryPanelForPlc);
                //lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.LineControl,
                //$"回馈CarryPanel：{CarryPanelForPlc},WMS实际HasUpProtect={wmsTask.HasUpProtect}", LogType.GENERAL));

                //plc.Write(ResultAddr, (int)PLCResultEnum.NORMAL);
                //    lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.LineControl,
                //    $"回馈Result：{(int)PLCResultEnum.NORMAL}", LogType.GENERAL));
                //}
                //else
                //{
                //    plc.Write(ResultAddr, (int)PLCResultEnum.ABNORMAL);
                //    lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.LineControl,
                //$"回馈Result：{(int)PLCResultEnum.ABNORMAL},异常信息：BatchGroupPlus中没有生产中的批次信息，BatchNamePilerNoBind数据新增失败", LogType.ABNORMAL));
                //}

                //plc.Write(SumCountAddr, (int)wmsTask.Amount);
                PublicUtils.PLCWriteInt(plc, SumCountAddr, batchNamePilerNoBind.Count);
                lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.LineControl,
            $"回馈SumCount：{batchNamePilerNoBind.Count}", LogType.GENERAL));

                int CarryPanelForPlc = Convert.ToInt16(batchNamePilerNoBind.HasUpProtect);
                //WMSHasUpProtect=0代表没有保护板，因plc不能识别0故转换成2
                if (!(bool)batchNamePilerNoBind.HasUpProtect)
                {
                    CarryPanelForPlc = 2;
                }

                //plc.Write(CarryPanelAddr, CarryPanelForPlc);
                PublicUtils.PLCWriteInt(plc, CarryPanelAddr, CarryPanelForPlc);
                lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.LineControl,
                $"回馈CarryPanel：{CarryPanelForPlc},WMS实际HasUpProtect={wmsTask.HasUpProtect}", LogType.GENERAL));

                //plc.Write(ResultAddr, (int)PLCResultEnum.NORMAL);
                PublicUtils.PLCWriteInt(plc, ResultAddr, (int)PLCResultEnum.NORMAL);
                lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.LineControl,
                $"回馈Result：{(int)PLCResultEnum.NORMAL}", LogType.GENERAL));

                //plc.Write(TriggerInAddr, TriggerOutValue);
                PublicUtils.PLCWriteInt(plc, TriggerInAddr, TriggerOutValue);
                lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.LineControl,
                    $"回馈TriggerIn：{TriggerOutValue}", LogType.GENERAL));

            }
            else
            {

                //plc.Write(ResultAddr, (int)PLCResultEnum.ABNORMAL);
                PublicUtils.PLCWriteInt(plc, ResultAddr, (int)PLCResultEnum.ABNORMAL);
                lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.LineControl,
                $"回馈Result：{(int)PLCResultEnum.ABNORMAL}，异常信息：WMS_Task表中不存在ReqId：{reqId}", LogType.ABNORMAL));

                //plc.Write(TriggerInAddr, TriggerOutValue);
                PublicUtils.PLCWriteInt(plc, TriggerInAddr, TriggerOutValue);
                lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.LineControl,
                $"回馈TriggerIn：{TriggerOutValue}", LogType.GENERAL));
            }
        }

        /// <summary>
        /// 循环检查请求推板
        /// </summary>
        private void requestBoardLoop()
        {
            while (true)
            {
                requestBoard();

                Thread.Sleep(int.Parse(ConfigurationSettings.AppSettings["sleepTime"]));
            }
        }

        /// <summary>
        /// 监控12、13站台推板请求
        /// </summary>
        private void requestBoard()
        {
            //List<PLCLog> listPLCLog = new List<PLCLog>();

            bool plcIsConnect = plc.CheckConnect();
            if (plcIsConnect)
            {
                //地址块
                string TriggerOutAddr12 = ConfigurationSettings.AppSettings["TriggerOutAddr12"];
                string TriggerInAddr12 = ConfigurationSettings.AppSettings["TriggerInAddr12"];
                string ResultAddr12 = ConfigurationSettings.AppSettings["ResultAddr12"];
                string PLCResultAddr12 = ConfigurationSettings.AppSettings["PLCResultAddr12"];

                //值
                int TriggerOutValue12 = plc.ReadLong(TriggerOutAddr12);//12号站台PLC触发值
                int TriggerInValue12 = plc.ReadLong(TriggerInAddr12);//12号站台线控触发值

                int triggerDifference12 = TriggerOutValue12 - TriggerInValue12;

                //TriggerOutValue10自增异常
                if (triggerDifference12 != 1 && triggerDifference12 != 0)
                {
                    //lineControlCuttingServicePlus.InsertPLCLog(newPLCLog("10", TriggerType.PLC, 
                    //    $"TriggerOut值自增异常，TriggerOut={TriggerOutValue10}，TriggerIn={TriggerInValue10}",LogType.ABNORMAL));

                    //plc.Write(ResultAddr10, (int)PLCResultEnum.ABNORMAL);
                    //lineControlCuttingServicePlus.InsertPLCLog(newPLCLog("10", TriggerType.LineControl, 
                    //    $"回馈Result：{PLCResultEnum.ABNORMAL}，异常信息：TriggerOut值自增异常，自增量：{triggerDifference10}，TriggerOut={TriggerOutValue10}，" +
                    //    $"TriggerIn={TriggerInValue10}",LogType.ABNORMAL));

                    //plc.Write(TriggerInAddr10, TriggerOutValue10);
                    //lineControlCuttingServicePlus.InsertPLCLog(newPLCLog("10",TriggerType.LineControl,$"回馈TriggerIn：{TriggerOutValue10}"
                    //    ,LogType.GENERAL));

                    lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog("12", TriggerType.PLC,
                        $"TriggerOut值自增异常，自增量：{triggerDifference12}，TriggerOut={TriggerOutValue12}，TriggerIn={TriggerInValue12}",
                        LogType.ABNORMAL));
                }

                //12号站台请求推板
                if (triggerDifference12 == 1)
                {
                    //地址块
                    //当前站台号
                    //string PlatformNoAddr = ConfigurationSettings.AppSettings["PlatformNoAddr12"];
                    //板垛号
                    string PilerNoAddr = ConfigurationSettings.AppSettings["PilerNoAddr12"];
                    //计数
                    string CountAddr = ConfigurationSettings.AppSettings["CountAddr12"];
                    //板件总数
                    //string SumCountAddr = ConfigurationSettings.AppSettings["SumCountAddr12"];
                    //携带保护板
                    //string CarryPanelAddr = ConfigurationSettings.AppSettings["CarryPanelAddr12"];

                    //锯切图编号
                    string SawNoAddr = ConfigurationSettings.AppSettings["SawNoAddr12"];
                    //推板张数
                    string PushCountAddr = ConfigurationSettings.AppSettings["PushCountAddr12"];

                    requestBoardAction(PilerNoAddr, CountAddr, "12", SawNoAddr, PushCountAddr, ResultAddr12,
                        TriggerInAddr12, TriggerOutValue12, PLCResultAddr12);
                }
                else
                {//13号站台
                    //地址块
                    string TriggerOutAddr13 = ConfigurationSettings.AppSettings["TriggerOutAddr13"];
                    string TriggerInAddr13 = ConfigurationSettings.AppSettings["TriggerInAddr13"];
                    string ResultAdrr13 = ConfigurationSettings.AppSettings["ResultAdrr13"];
                    string PLCResultAdrr13 = ConfigurationSettings.AppSettings["PLCResultAdrr13"];

                    //值
                    //13号站台PLC触发值
                    int TriggerOutValue13 = plc.ReadLong(TriggerOutAddr13);
                    //13号站台线控触发值
                    int TriggerInValue13 = plc.ReadLong(TriggerInAddr13);
                    int triggerDifference13 = TriggerOutValue13 - TriggerInValue13;

                    if (triggerDifference13 == 1)//13号站台请求推板
                    {
                        //string PlatformNoAddr = ConfigurationSettings.AppSettings["PlatformNoAddr13"];
                        string PilerNoAddr = ConfigurationSettings.AppSettings["PilerNoAddr13"];
                        string CountAddr = ConfigurationSettings.AppSettings["CountAddr13"];
                        //string SumCountAddr = ConfigurationSettings.AppSettings["SumCountAddr13"];
                        //string CarryPanelAddr = ConfigurationSettings.AppSettings["CarryPanelAddr13"];

                        string SawNoAddr = ConfigurationSettings.AppSettings["SawNoAddr13"];
                        string PushCountAddr = ConfigurationSettings.AppSettings["PushCountAddr13"];

                        requestBoardAction(PilerNoAddr, CountAddr, "13", SawNoAddr, PushCountAddr, ResultAdrr13,
                            TriggerInAddr13, TriggerOutValue13, PLCResultAdrr13);
                    }
                    else if (triggerDifference13 != 0)
                    {
                        //lineControlCuttingServicePlus.InsertPLCLog(newPLCLog("11", TriggerType.PLC, $"TriggerOut值自增异常，" +
                        //    $"TriggerOut={TriggerOutValue11}，TriggerIn={TriggerInValue11}", LogType.ABNORMAL));
                        //plc.Write(ResultAdrr11, (int)PLCResultEnum.ABNORMAL);
                        //lineControlCuttingServicePlus.InsertPLCLog(newPLCLog("11", TriggerType.LineControl, $"回馈Result：{(int)PLCResultEnum.ABNORMAL}" +
                        //    $",异常信息：TriggerOut值自增异常，自增量：{triggerDifference11}，TriggerOut={TriggerOutValue11}，TriggerIn={TriggerInValue11}", LogType.ABNORMAL));

                        //plc.Write(TriggerInAddr11, TriggerOutValue11);
                        //lineControlCuttingServicePlus.InsertPLCLog(newPLCLog("11",TriggerType.LineControl,$"回馈TriggerIn：{TriggerOutValue11}",
                        //    LogType.GENERAL));

                        lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog("13", TriggerType.LineControl, $"TriggerOut值自增异常，" +
                            $"自增量：{triggerDifference13}，TriggerOut={TriggerOutValue13}，TriggerIn={TriggerInValue13}", LogType.ABNORMAL));
                    }


                }
            }
            else
            {
                //string PlcIp = ConfigurationSettings.AppSettings["PlcIp"];

                plcIsConnect = plc.Connect(PlcIp);
                if (plcIsConnect)
                {
                    requestBoard();
                }
                else
                {
                    lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(PlcIp, TriggerType.LineControl,
                        $"连接PLC失败：{PlcIp}", LogType.ABNORMAL));
                }
            }

            //lineControlCuttingServicePlus.BulkInsertPLCLog(listPLCLog);
        }

        /// <summary>
        /// 请求推板任务1
        /// </summary>
        /// <param name="PlatformNoAddr">当前站台号</param>
        /// <param name="PilerNoAddr">板垛号</param>
        /// <param name="SumCountAddr">板件总数</param>
        /// <param name="CarryPanelAddr">携带保护板</param>
        /// <param name="station">站台编号</param>
        /// <param name="SawNoAddr">锯切图编号</param>
        /// <param name="PushCountAddr">推板张数</param>
        /// <param name="ResultAddr">执行结果</param>
        /// <param name="TriggerInAddr">线控触发值</param>
        /// <param name="TriggerOutValue">PLC触发值</param>
        private void requestBoardAction(string PlatformNoAddr, string PilerNoAddr, string SumCountAddr, string CarryPanelAddr, string station,
            string SawNoAddr, string PushCountAddr, string ResultAddr, string TriggerInAddr, int TriggerOutValue)
        {
            /*
             商议修改
             增加到位请求，检证plc携带信息是否正确
             推板请求逻辑变更
             收到推板请求后到BatchGroupPlus里捞一条等级最高的未生产或者生产中的批次，根据这个批次到CuttingSawFileRelation里捞一条未分配的锯切图，然后下发给推板机构
             */
            int PlatformNo = plc.ReadLong(PlatformNoAddr);
            int PilerNo = plc.ReadLong(PilerNoAddr);
            int SumCount = plc.ReadLong(SumCountAddr);
            int CarryPanel = plc.ReadLong(CarryPanelAddr);

            lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.PLC, $"{station}号工位/站台触发请求，PlatformNo：{PlatformNo}" +
                $"，PilerNo：{PilerNo}，SumCount：{SumCount}，CarryPanel：{ CarryPanel}", LogType.GENERAL));

            /*
             获取一条生产中的等级最高的批次
             */

            //获取一条MES CuttingSawFileRelation表中的数据
            List<CuttingSawFileRelation> listCuttingSawFileRelation = lineControlCuttingServicePlus.GetMinStackIndexCuttingSawFileRelationByStackName(PilerNo.ToString());
            if (listCuttingSawFileRelation.Count < 1)
            {
                //plc.Write(ResultAddr, (int)PLCResultEnum.ABNORMAL);
                PublicUtils.PLCWriteInt(plc, ResultAddr, (int)PLCResultEnum.ABNORMAL);
                lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.LineControl, $"回馈Result：{(int)PLCResultEnum.ABNORMAL}," +
                    $"异常信息：MES CuttingSawFileRelation表中没有该垛未分配数据，垛号：{PilerNo}"
                    , LogType.ABNORMAL));

                //plc.Write(TriggerInAddr, TriggerOutValue);
                PublicUtils.PLCWriteInt(plc, TriggerInAddr, TriggerOutValue);
                lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.LineControl, $"回馈TriggerIn：{TriggerOutValue}", LogType.GENERAL));
                return;
            }

            CuttingSawFileRelation cuttingSawFileRelation = listCuttingSawFileRelation[0];

            //将MES CuttingSawFileRelation表中的数据插入线控CuttingSawFileRelationPlus表中
            List<CuttingSawFileRelationPlus> listCuttingSawFileRelationPlus = new List<CuttingSawFileRelationPlus>();
            //foreach (var item in listCuttingSawFileRelation)
            //{
            CuttingSawFileRelationPlus cuttingSawFileRelationPlus = new CuttingSawFileRelationPlus();
            cuttingSawFileRelationPlus.BatchName = cuttingSawFileRelation.BatchName;
            cuttingSawFileRelationPlus.StackName = cuttingSawFileRelation.StackName;
            cuttingSawFileRelationPlus.BoardCount = cuttingSawFileRelation.BoardCount;
            cuttingSawFileRelationPlus.CreatedTime = cuttingSawFileRelation.CreatedTime;
            cuttingSawFileRelationPlus.Id = cuttingSawFileRelation.Id;
            cuttingSawFileRelationPlus.PlanDate = cuttingSawFileRelation.PlanDate;
            cuttingSawFileRelationPlus.SawFile = cuttingSawFileRelation.SawFile;
            cuttingSawFileRelationPlus.SawFileName = cuttingSawFileRelation.SawFileName;
            cuttingSawFileRelationPlus.StackIndex = cuttingSawFileRelation.StackIndex;
            cuttingSawFileRelationPlus.SawType = cuttingSawFileRelation.SawType;
            cuttingSawFileRelationPlus.Status = (int)CuttingSawFileRelationPlusStatus.Assigned;
            cuttingSawFileRelationPlus.TaskDistributeId = cuttingSawFileRelation.TaskDistributeId;
            cuttingSawFileRelationPlus.TaskId = cuttingSawFileRelation.TaskId;
            cuttingSawFileRelationPlus.UpdatedTime = cuttingSawFileRelation.UpdatedTime;

            listCuttingSawFileRelationPlus.Add(cuttingSawFileRelationPlus);
            //}

            bool flag = lineControlCuttingServicePlus.BulkInsertCuttingSawFileRelationPlus(listCuttingSawFileRelationPlus);
            if (!flag)
            {
                //plc.Write(ResultAddr, (int)PLCResultEnum.ABNORMAL);
                PublicUtils.PLCWriteInt(plc, ResultAddr, (int)PLCResultEnum.ABNORMAL);
                lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.LineControl, $"回馈Result：{(int)PLCResultEnum.ABNORMAL}," +
                    $"异常信息：将MES CuttingSawFileRelation表中的数据插入线控CuttingSawFileRelationPlus表失败！CuttingSawFileRelation Id：{cuttingSawFileRelation.Id}", LogType.ABNORMAL));
            }
            else
            {
                //plc.Write(SawNoAddr, cuttingSawFileRelationPlus.SawFileName, sawNoLength);
                PublicUtils.PLCWriteString(plc, SawNoAddr, cuttingSawFileRelationPlus.SawFileName, sawNoLength);
                //plc.Write(PushCountAddr, cuttingSawFileRelationPlus.BoardCount);
                PublicUtils.PLCWriteInt(plc, PushCountAddr, cuttingSawFileRelationPlus.BoardCount);
                //plc.Write(ResultAddr,(int)PLCResultEnum.NORMAL);
                PublicUtils.PLCWriteInt(plc, ResultAddr, (int)PLCResultEnum.NORMAL);

                lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.LineControl, $"回馈SawNo：" +
                    $"{cuttingSawFileRelationPlus.SawFileName}，PushCount：{cuttingSawFileRelationPlus.BoardCount},Result:{(int)PLCResultEnum.NORMAL}", LogType.GENERAL));
            }
            //plc.Write(TriggerInAddr, TriggerOutValue);
            PublicUtils.PLCWriteInt(plc, TriggerInAddr, TriggerOutValue);
            lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.LineControl, $"回馈TriggerIn：{TriggerOutValue}", LogType.GENERAL));
        }

        /// <summary>
        /// 请求推板任务2
        /// </summary>
        /// <param name="PilerNoAddr">板垛号</param>
        /// <param name="CountAddr">计数</param>
        /// <param name="station">站台编号</param>
        /// <param name="SawNoAddr">锯切图编号</param>
        /// <param name="PushCountAddr">推板张数</param>
        /// <param name="ResultAddr">执行结果地址块</param>
        /// <param name="TriggerInAddr">线控触发值地址块</param>
        /// <param name="TriggerOutValue">PLC触发值</param>
        private void requestBoardAction(string PilerNoAddr, string CountAddr, string station,
            string SawNoAddr, string PushCountAddr, string ResultAddr, string TriggerInAddr, int TriggerOutValue, string PLCResultAddr)
        {

            int PilerNo = plc.ReadLong(PilerNoAddr);
            int Count = plc.ReadLong(CountAddr);

            if (Count <= 0)
            {
                lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.PLC, $"{station}号工位/站台推板请求，" +
                $"请求信息：PilerNo={PilerNo}，Count={Count},该请求不给与响应", LogType.GENERAL));
                return;
            }

            lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.PLC, $"{station}号工位/站台推板请求，" +
                $"请求信息：PilerNo={PilerNo}，Count={Count}", LogType.GENERAL));

            //获取板垛与批次关联数据
            List<BatchNamePilerNoBind> listBatchNamePilerNoBind = lineControlCuttingServicePlus
                .GetBatchNamePilerNoBindByPilerNo(PilerNo);
            BatchNamePilerNoBind batchNamePilerNoBind;

            if (listBatchNamePilerNoBind.Count > 0)
            {
                batchNamePilerNoBind = listBatchNamePilerNoBind[0];
                //如果电气板垛数量计数和系统板垛数量计数不一致中止任务
                if (!(Count == batchNamePilerNoBind.Count))
                {
                    //plc.Write(ResultAddr, (int)PLCResultEnum.ABNORMAL);
                    PublicUtils.PLCWriteInt(plc, ResultAddr, (int)PLCResultEnum.ABNORMAL);
                    lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.LineControl, $"回馈Result：" +
                        $"{(int)PLCResultEnum.ABNORMAL}，异常信息：PLC统计板件剩余数量：{Count}与系统统计板件剩余数量：{batchNamePilerNoBind.Count}" +
                        $"不相符，任务中断！",
                        LogType.ABNORMAL));

                    //plc.Write(TriggerInAddr, TriggerOutValue);
                    PublicUtils.PLCWriteInt(plc, TriggerInAddr, TriggerOutValue);
                    lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.LineControl, $"回馈TriggerIn：" +
                        $"{TriggerOutValue}", LogType.GENERAL));
                    return;
                }
            }
            else
            {
                //plc.Write(ResultAddr, (int)PLCResultEnum.ABNORMAL);
                PublicUtils.PLCWriteInt(plc, ResultAddr, (int)PLCResultEnum.ABNORMAL);
                lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.LineControl, $"回馈Result：" +
                    $"{(int)PLCResultEnum.ABNORMAL}，异常信息：BatchNamePilerNoBind表中不存在垛号：{PilerNo}，系统无法获取该垛绑定的批次号，任务中断！",
                    LogType.ABNORMAL));

                //plc.Write(TriggerInAddr, TriggerOutValue);
                PublicUtils.PLCWriteInt(plc, TriggerInAddr, TriggerOutValue);
                lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.LineControl, $"回馈TriggerIn：" +
                    $"{TriggerOutValue}", LogType.GENERAL));
                return;
            }

            //余料回库业务
            List<BatchGroupPlus> listBatchGroupPlus2 = lineControlCuttingServicePlus.GetBatchGroupPlusByBatchName(batchNamePilerNoBind.BatchName);
            if (listBatchGroupPlus2.Count > 0)
            {
                BatchGroupPlus batchGroupPlus = listBatchGroupPlus2[0];
                if (batchGroupPlus.Status == (int)BatchGroupPlusStatus.ProductionIsCompleted)
                {
                    requestWMSBack(station, batchNamePilerNoBind.ProductCode, Count, PilerNo, ResultAddr, batchNamePilerNoBind.StackName, TriggerInAddr,
                        TriggerOutValue);

                    return;
                }
            }
            else
            {
                //plc.Write(ResultAddr, (int)PLCResultEnum.ABNORMAL);
                PublicUtils.PLCWriteInt(plc, ResultAddr, (int)PLCResultEnum.ABNORMAL);
                lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.LineControl, $"回馈Result：" +
                    $"{(int)PLCResultEnum.ABNORMAL}，异常信息：BatchGroupPlus表中不存在批次号：{batchNamePilerNoBind.BatchName}，系统无法获取该垛：" +
                    $"{PilerNo}绑定批次生产完成状态，任务中断！", LogType.ABNORMAL));

                //plc.Write(TriggerInAddr, TriggerOutValue);
                PublicUtils.PLCWriteInt(plc, TriggerInAddr, TriggerOutValue);
                lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.LineControl, $"回馈TriggerIn：" +
                    $"{TriggerOutValue}", LogType.GENERAL));
                return;
            }

            /*
             获取一条生产中的等级最高的批次
             */
            //List<BatchGroupPlus> listBatchGroupPlus = lineControlCuttingServicePlus.GetBatchGroupPlusByStatusAndMinBatchIndex(BatchGroupPlusStatus.InProduction);

            //if(listBatchGroupPlus.Count > 0)
            //{
            //BatchGroupPlus batchGroupPlus = listBatchGroupPlus[0];

            //根据批次号获取一条顺序最高未下发的锯切图数据
            //List<CuttingSawFileRelation> listCuttingSawFileRelation = lineControlCuttingServicePlus.
            //    GetCuttingSawFileRelationByBatchNameAndMinSawIndex(batchGroupPlus.BatchName);




            //根据批次号获取一条顺序最高未下发的锯切图数据
            //List<CuttingSawFileRelation> listCuttingSawFileRelation = lineControlCuttingServicePlus.
            //        GetCuttingSawFileRelationByBatchNameAndMinStackIndex(batchNamePilerNoBind.BatchName);

            //查询CuttingSawFileRelationPlus表中是否有未分配的saw文件
            List<CuttingSawFileRelationPlus> listCuttingSawFileRelationPlus2 = lineControlCuttingServicePlus.
                GetCuttingSawFileRelationPlusByStackNameAndStatus(batchNamePilerNoBind.StackName,
                CuttingSawFileRelationPlusStatus.Unassigned);
            //如果CuttingSawFileRelationPlus表中有未分配的saw文件
            if (listCuttingSawFileRelationPlus2.Count > 0)
            {
                CuttingSawFileRelationPlus cuttingSawFileRelationPlus = listCuttingSawFileRelationPlus2[0];

                //板件数量不能满足锯切图需要的数量
                if (Count < cuttingSawFileRelationPlus.BoardCount)
                {
                    //plc.Write(ResultAddr, (int)PLCResultEnum.ABNORMAL);
                    PublicUtils.PLCWriteInt(plc, ResultAddr, (int)PLCResultEnum.ABNORMAL);
                    lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.LineControl, $"回馈Result：" +
                        $"{(int)PLCResultEnum.ABNORMAL}，异常信息：剩余板件数量：{Count}不能满足锯切图需要的数量：{cuttingSawFileRelationPlus.BoardCount}，" +
                        $"任务中断！", LogType.ABNORMAL));

                    //plc.Write(TriggerInAddr, TriggerOutValue);
                    PublicUtils.PLCWriteInt(plc, TriggerInAddr, TriggerOutValue);
                    lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.LineControl, $"回馈TriggerIn：" +
                        $"{TriggerOutValue}", LogType.GENERAL));
                    return;
                }

                //更新锯切图状态为已分配
                cuttingSawFileRelationPlus.Status = (int)CuttingSawFileRelationPlusStatus.Assigned;
                lineControlCuttingServicePlus.BulkUpdatedCuttingSawFileRelationPlus(listCuttingSawFileRelationPlus2);

                //回馈PLC推板请求结果并更新数据
                feedBackRequestBoardAndUpdateDB(station, listBatchNamePilerNoBind, cuttingSawFileRelationPlus.BoardCount, SawNoAddr,
                    cuttingSawFileRelationPlus.SawFileName, PushCountAddr, ResultAddr, TriggerInAddr, TriggerOutValue, PLCResultAddr,
                    listCuttingSawFileRelationPlus2);
            }
            else
            {
                //根据MES垛号获取一条顺序最小未下发的锯切图数据
                List<CuttingSawFileRelation> listCuttingSawFileRelation = lineControlCuttingServicePlus.
                    GetCuttingSawFileRelationByStackNameAndMinStackIndex(batchNamePilerNoBind.StackName);

                //没有锯切图可用,则认为是余料，申请回库
                if (listCuttingSawFileRelation.Count < 1)
                {
                    requestWMSBack(station, batchNamePilerNoBind.ProductCode, Count, PilerNo, ResultAddr, batchNamePilerNoBind.StackName, TriggerInAddr,
                            TriggerOutValue);
                    /*
                     如果仍需数量为零，Count>0，执行余料回库
                     返回
                     */
                    //List<BatchProductionDetails> listBatchProductionDetails = lineControlCuttingServicePlus.GetBatchProductionDetailsByBatchName(batchGroupPlus.BatchName);
                    //if (listBatchProductionDetails.Count > 0)
                    //{
                    //    BatchProductionDetails batchProductionDetails = listBatchProductionDetails[0];
                    //    if (batchProductionDetails.DifferenceNumber == 0 && Count > 0)
                    //    {
                    //        plc.Write(ResultAddr, (int)PLCResultEnum.RETURN);
                    //        lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.LineControl, $"回馈Result：{(int)PLCResultEnum.RETURN}," +
                    //            $"退垛回库", LogType.ABNORMAL));

                    //        plc.Write(TriggerInAddr, TriggerOutValue);
                    //        lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.LineControl, $"回馈TriggerIn：{TriggerOutValue}", LogType.GENERAL));

                    //        return;
                    //    }
                    //}

                    //plc.Write(ResultAddr, (int)PLCResultEnum.ABNORMAL);
                    //lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.LineControl, $"回馈Result：{(int)PLCResultEnum.ABNORMAL}," +
                    //    $"异常信息：MES CuttingSawFileRelation表中没有该批次未分配数据，批次号：{batchNamePilerNoBind.BatchName}"
                    //    , LogType.ABNORMAL));

                    //plc.Write(TriggerInAddr, TriggerOutValue);
                    //lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.LineControl, $"回馈TriggerIn：{TriggerOutValue}", LogType.GENERAL));
                    return;
                }

                CuttingSawFileRelation cuttingSawFileRelation = listCuttingSawFileRelation[0];

                //板件数量不能满足锯切图需要的数量
                if (Count < cuttingSawFileRelation.BoardCount)
                {
                    //根据批次号，板件数量匹配锯切图
                    //listCuttingSawFileRelation = lineControlCuttingServicePlus.GetCuttingSawFileRelationByBatchNameAndBoardCount(batchNamePilerNoBind.BatchName, Count);

                    //if (listCuttingSawFileRelation.Count < 1)
                    //{
                    //plc.Write(ResultAddr, (int)PLCResultEnum.ABNORMAL);
                    PublicUtils.PLCWriteInt(plc, ResultAddr, (int)PLCResultEnum.ABNORMAL);
                    lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.LineControl, $"回馈Result：" +
                        $"{(int)PLCResultEnum.ABNORMAL}，异常信息：剩余板件数量：{Count}不能满足锯切图需要的数量：{cuttingSawFileRelation.BoardCount}，" +
                        $"任务中断！", LogType.ABNORMAL));

                    //plc.Write(TriggerInAddr, TriggerOutValue);
                    PublicUtils.PLCWriteInt(plc, TriggerInAddr, TriggerOutValue);
                    lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.LineControl, $"回馈TriggerIn：" +
                        $"{TriggerOutValue}", LogType.GENERAL));
                    return;
                    //}

                    //cuttingSawFileRelation = listCuttingSawFileRelation[0];
                }

                //将MES CuttingSawFileRelation表中的数据插入线控CuttingSawFileRelationPlus表中
                //bool flag = CuttingSawFileRelationPlusDBUtils.AddAssignedByCuttingSawFileRelation(lineControlCuttingServicePlus
                //    , cuttingSawFileRelation);
                List<CuttingSawFileRelationPlus> listCuttingSawFileRelationPlus = new List<CuttingSawFileRelationPlus>();
                //foreach (var item in listCuttingSawFileRelation)
                //{
                CuttingSawFileRelationPlus cuttingSawFileRelationPlus = new CuttingSawFileRelationPlus();
                cuttingSawFileRelationPlus.BatchName = cuttingSawFileRelation.BatchName;
                cuttingSawFileRelationPlus.StackName = cuttingSawFileRelation.StackName;
                cuttingSawFileRelationPlus.BoardCount = cuttingSawFileRelation.BoardCount;
                //cuttingSawFileRelationPlus.CreatedTime = cuttingSawFileRelation.CreatedTime;
                cuttingSawFileRelationPlus.CreatedTime = DateTime.Now;
                //cuttingSawFileRelationPlus.Id = cuttingSawFileRelation.Id;
                cuttingSawFileRelationPlus.PlanDate = cuttingSawFileRelation.PlanDate;
                cuttingSawFileRelationPlus.SawFile = cuttingSawFileRelation.SawFile;
                cuttingSawFileRelationPlus.SawFileName = cuttingSawFileRelation.SawFileName;
                cuttingSawFileRelationPlus.StackIndex = cuttingSawFileRelation.StackIndex;
                cuttingSawFileRelationPlus.SawType = cuttingSawFileRelation.SawType;
                cuttingSawFileRelationPlus.Status = (int)CuttingSawFileRelationPlusStatus.Assigned;
                cuttingSawFileRelationPlus.TaskDistributeId = cuttingSawFileRelation.TaskDistributeId;
                cuttingSawFileRelationPlus.TaskId = cuttingSawFileRelation.TaskId;
                cuttingSawFileRelationPlus.UpdatedTime = DateTime.Now;

                listCuttingSawFileRelationPlus.Add(cuttingSawFileRelationPlus);
                //}

                bool flag = lineControlCuttingServicePlus.BulkInsertCuttingSawFileRelationPlus(listCuttingSawFileRelationPlus);
                if (!flag)
                {
                    //plc.Write(ResultAddr, (int)PLCResultEnum.ABNORMAL);
                    PublicUtils.PLCWriteInt(plc, ResultAddr, (int)PLCResultEnum.ABNORMAL);
                    lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.LineControl, $"回馈Result：{(int)PLCResultEnum.ABNORMAL}," +
                        $"异常信息：将MES CuttingSawFileRelation表中的数据插入线控CuttingSawFileRelationPlus表失败！CuttingSawFileRelation Id：{cuttingSawFileRelation.Id}", LogType.ABNORMAL));

                    //plc.Write(TriggerInAddr, TriggerOutValue);
                    PublicUtils.PLCWriteInt(plc, TriggerInAddr, TriggerOutValue);
                    lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.LineControl, $"回馈TriggerIn：{TriggerOutValue}", LogType.GENERAL));
                }
                else
                {
                    //回馈PLC推板请求结果并更新数据
                    feedBackRequestBoardAndUpdateDB(station, listBatchNamePilerNoBind, cuttingSawFileRelationPlus.BoardCount, SawNoAddr,
                        cuttingSawFileRelationPlus.SawFileName, PushCountAddr, ResultAddr, TriggerInAddr, TriggerOutValue, PLCResultAddr,
                    listCuttingSawFileRelationPlus);
                    //更新板垛剩余数量
                    //batchNamePilerNoBind.Count -= cuttingSawFileRelation.BoardCount;
                    //lineControlCuttingServicePlus.BulkUpdateBatchNamePilerNoBindByPilerNo(listBatchNamePilerNoBind);

                    //plc.Write(SawNoAddr, cuttingSawFileRelationPlus.SawFileName);
                    //lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.LineControl, $"回馈SawNo：" +
                    //    $"{cuttingSawFileRelationPlus.SawFileName}", LogType.GENERAL));

                    //plc.Write(PushCountAddr, cuttingSawFileRelationPlus.BoardCount);
                    //lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.LineControl, $"回馈PushCount：" +
                    //    $"{cuttingSawFileRelationPlus.BoardCount}", LogType.GENERAL));

                    //plc.Write(ResultAddr, (int)PLCResultEnum.NORMAL);
                    //lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.LineControl, $"回馈Result:" +
                    //    $"{(int)PLCResultEnum.NORMAL}", LogType.GENERAL));




                    ////所有锯切图做完，更新批次状态已完成
                    //List<CuttingSawFileRelation> listCuttingSawFileRelation2 = lineControlCuttingServicePlus.
                    //    GetCuttingSawFileRelationByBatchNameAndMinStackIndex(batchNamePilerNoBind.BatchName);
                    //if (listCuttingSawFileRelation2.Count < 1)
                    //{
                    //    List<BatchGroupPlus> listBatchGroupPlus = new List<BatchGroupPlus>();
                    //    BatchGroupPlus batchGroupPlus = new BatchGroupPlus();

                    //    batchGroupPlus.BatchName = batchNamePilerNoBind.BatchName;
                    //    batchGroupPlus.Status = (int)BatchGroupPlusStatus.ProductionIsCompleted;
                    //    batchGroupPlus.UpdatedTime = DateTime.Now;

                    //    listBatchGroupPlus.Add(batchGroupPlus);

                    //    lineControlCuttingServicePlus.BulkUpdateBatchGroupPlusStatusByBatchName(listBatchGroupPlus);

                    //    BatchProductionDetails batchProductionDetails = new BatchProductionDetails();
                    //    batchProductionDetails.BatchName = batchGroupPlus.BatchName;
                    //    batchProductionDetails.Status = (int)BatchGroupPlusStatus.ProductionIsCompleted;
                    //    lineControlCuttingServicePlus.UpdateBatchProductionDetailsStatusByBatchName(batchProductionDetails);

                    //    lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.LineControl,
                    //        $"所有锯切图已下发，更新BatchGroupPlus、BatchProductionDetails Status已完成({(int)BatchGroupPlusStatus.ProductionIsCompleted})"
                    //        , LogType.GENERAL));
                    //}
                }

            }

            //}
            //else
            //{
            //    plc.Write(ResultAddr, (int)PLCResultEnum.ABNORMAL);
            //    lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.LineControl, $"回馈Result：{(int)PLCResultEnum.ABNORMAL}," +
            //        $"异常信息：BatchGroupPlus表没有生产中的批次", LogType.ABNORMAL));

            //    plc.Write(TriggerInAddr, TriggerOutValue);
            //    lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.LineControl, $"回馈TriggerIn：{TriggerOutValue}", LogType.GENERAL));
            //}
        }

        //向WMS请求余料回库
        private void requestWMSBack(string station, string ProductCode, int Count, int PilerNo, string ResultAddr, string StackName, string TriggerInAddr, int TriggerOutValue)
        {
            //向WMS请求余料回库
            WMSInParams wmsInParams = new WMSInParams();
            wmsInParams.ReqType = 4;//余料回库
            wmsInParams.ProductCode = ProductCode;
            wmsInParams.Count = Count;
            wmsInParams.FromStation = 2021;
            wmsInParams.PilerNo = PilerNo;

            ResultMsg resultMsg = wmsService.ApplyWMSTask(wmsInParams);

            if (resultMsg.Status == 200)
            {
                lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.LineControl, $"向WMS请求余料回库成功，" +
                    $"请求信息：ReqType=4，ProductCode={ProductCode},Count={Count}," +
                    $"FromStation={wmsInParams.FromStation},PilerNo：{PilerNo}；WMS回馈信息：Status={resultMsg.Status}," +
                    $"ReqId={resultMsg.ReqId}，Message={resultMsg.Message}", LogType.GENERAL));

                //plc.Write(ResultAddr, (int)PLCResultEnum.RETURN);
                PublicUtils.PLCWriteInt(plc, ResultAddr, (int)PLCResultEnum.RETURN);
                lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.LineControl, $"回馈Result：" +
                    $"{(int)PLCResultEnum.RETURN},{StackName}垛已生产完成，WMS垛号：{PilerNo}余料回库，数量：{Count}", LogType.GENERAL));

            }
            else
            {
                //plc.Write(ResultAddr, (int)PLCResultEnum.ABNORMAL);
                PublicUtils.PLCWriteInt(plc, ResultAddr, (int)PLCResultEnum.ABNORMAL);
                lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.LineControl, $"回馈Result：" +
                    $"{(int)PLCResultEnum.ABNORMAL},异常信息：向WMS请求余料回库失败，请求信息：ReqType=4，" +
                    $"ProductCode={ProductCode},Count={Count},FromStation={wmsInParams.FromStation},PilerNo：{PilerNo}；" +
                    $"WMS回馈信息：Status={resultMsg.Status},ReqId={resultMsg.ReqId}，Message={resultMsg.Message}", LogType.ABNORMAL));
            }

            //plc.Write(TriggerInAddr, TriggerOutValue);
            PublicUtils.PLCWriteInt(plc, TriggerInAddr, TriggerOutValue);
            lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.LineControl, $"回馈TriggerIn：" +
                $"{TriggerOutValue}", LogType.GENERAL));
        }

        /// <summary>
        /// 回馈PLC推板请求结果并更新数据
        /// </summary>
        /// <param name="station"></param>
        /// <param name="listBatchNamePilerNoBind"></param>
        /// <param name="boardCount"></param>
        /// <param name="SawNoAddr"></param>
        /// <param name="sawFileName"></param>
        /// <param name="PushCountAddr"></param>
        /// <param name="ResultAddr"></param>
        /// <param name="TriggerInAddr"></param>
        /// <param name="TriggerOutValue"></param>
        /// <param name="PLCResultAddr"></param>
        /// <param name="listCuttingSawFileRelationPlus"></param>
        private void feedBackRequestBoardAndUpdateDB(string station, List<BatchNamePilerNoBind> listBatchNamePilerNoBind,
            int boardCount, string SawNoAddr, string sawFileName, string PushCountAddr, string ResultAddr, string TriggerInAddr,
            int TriggerOutValue, string PLCResultAddr, List<CuttingSawFileRelationPlus> listCuttingSawFileRelationPlus)
        {
            //plc.Write(SawNoAddr, sawFileName, sawNoLength);
            PublicUtils.PLCWriteString(plc, SawNoAddr, sawFileName, sawNoLength);
            lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.LineControl, $"回馈SawNo：" +
                $"{sawFileName}", LogType.GENERAL));

            //plc.Write(PushCountAddr, boardCount);
            PublicUtils.PLCWriteInt(plc, PushCountAddr, boardCount);
            lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.LineControl, $"回馈PushCount：" +
                $"{boardCount}", LogType.GENERAL));

            //plc.Write(ResultAddr, (int)PLCResultEnum.NORMAL);
            PublicUtils.PLCWriteInt(plc, ResultAddr, (int)PLCResultEnum.NORMAL);
            lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.LineControl, $"回馈Result:" +
                $"{(int)PLCResultEnum.NORMAL}", LogType.GENERAL));

            //plc.Write(TriggerInAddr, TriggerOutValue);
            PublicUtils.PLCWriteInt(plc, TriggerInAddr, TriggerOutValue);
            lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.LineControl, $"回馈TriggerIn：{TriggerOutValue}", LogType.GENERAL));

            while (true)
            {
                List<CuttingSawFileRelationPlus> listCuttingSawFileRelationPlus2 = lineControlCuttingServicePlus
                    .GetCuttingSawFileRelationPlusBySawFileName(sawFileName);
                CuttingSawFileRelationPlus cuttingSawFileRelationPlus2 = listCuttingSawFileRelationPlus2[0];

                if (cuttingSawFileRelationPlus2.Status == (int)CuttingSawFileRelationPlusStatus.PushCompleted)
                {
                    lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.PLC, $"{station}号站" +
                        $"台板垛推出，此动作为系统手动完成", LogType.GENERAL));
                    PushBoardSuccess(station, boardCount, listBatchNamePilerNoBind);

                    break;
                }

                int plcResult = plc.ReadLong(PLCResultAddr);
                if (plcResult == 2)//成功推出
                {
                    lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.PLC, $"{station}号站台板垛推出，Result" +
                        $"({PLCResultAddr})={plcResult}", LogType.GENERAL));

                    PushBoardSuccess(station, boardCount, listBatchNamePilerNoBind);

                    cuttingSawFileRelationPlus2.Status = (int)CuttingSawFileRelationPlusStatus.PushCompleted;
                    lineControlCuttingServicePlus.BulkUpdatedCuttingSawFileRelationPlus(listCuttingSawFileRelationPlus);

                    //BatchNamePilerNoBind batchNamePilerNoBind = listBatchNamePilerNoBind[0];
                    ////更新板垛剩余数量
                    //batchNamePilerNoBind.Count -= boardCount;
                    //lineControlCuttingServicePlus.BulkUpdateBatchNamePilerNoBindByPilerNo(listBatchNamePilerNoBind);

                    ////所有锯切图做完，更新批次状态已完成
                    //List<CuttingSawFileRelation> listCuttingSawFileRelation2 = lineControlCuttingServicePlus.
                    //    GetCuttingSawFileRelationByBatchNameAndMinStackIndex(batchNamePilerNoBind.BatchName);
                    //if (listCuttingSawFileRelation2.Count < 1)
                    //{
                    //    List<BatchGroupPlus> listBatchGroupPlus = new List<BatchGroupPlus>();
                    //    BatchGroupPlus batchGroupPlus = new BatchGroupPlus();

                    //    batchGroupPlus.BatchName = batchNamePilerNoBind.BatchName;
                    //    batchGroupPlus.Status = (int)BatchGroupPlusStatus.ProductionIsCompleted;
                    //    batchGroupPlus.UpdatedTime = DateTime.Now;

                    //    listBatchGroupPlus.Add(batchGroupPlus);

                    //    lineControlCuttingServicePlus.BulkUpdateBatchGroupPlusStatusByBatchName(listBatchGroupPlus);

                    //    BatchProductionDetails batchProductionDetails = new BatchProductionDetails();
                    //    batchProductionDetails.BatchName = batchGroupPlus.BatchName;
                    //    batchProductionDetails.Status = (int)BatchGroupPlusStatus.ProductionIsCompleted;
                    //    lineControlCuttingServicePlus.UpdateBatchProductionDetailsStatusByBatchName(batchProductionDetails);

                    //    lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.LineControl,
                    //        $"所有锯切图已下发，更新BatchGroupPlus、BatchProductionDetails Status已完成({(int)BatchGroupPlusStatus.ProductionIsCompleted})"
                    //        , LogType.GENERAL));
                    //}
                    break;
                }
                else if (plcResult == 3)//失败，更新新增的锯切图状态为未分配，使其可用
                {
                    CuttingSawFileRelationPlus cuttingSawFileRelationPlus = listCuttingSawFileRelationPlus[0];
                    cuttingSawFileRelationPlus.Status = (int)CuttingSawFileRelationPlusStatus.Unassigned;
                    lineControlCuttingServicePlus.BulkUpdatedCuttingSawFileRelationPlus(listCuttingSawFileRelationPlus);

                    lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.PLC, $"{station}号站台推板失败，" +
                        $"Result({PLCResultAddr})={plcResult}，系统恢复锯切图{cuttingSawFileRelationPlus.SawFileName}状态未分配" +
                        $"({cuttingSawFileRelationPlus.Status})", LogType.ABNORMAL));
                    break;
                }

                Thread.Sleep(int.Parse(ConfigurationSettings.AppSettings["pollingPLCResultTime"]));
            }


        }

        private void PushBoardSuccess(string station, int boardCount, List<BatchNamePilerNoBind> listBatchNamePilerNoBind)
        {
            BatchNamePilerNoBind batchNamePilerNoBind = listBatchNamePilerNoBind[0];
            //更新板垛剩余数量
            batchNamePilerNoBind.Count -= boardCount;
            lineControlCuttingServicePlus.BulkUpdateBatchNamePilerNoBindByPilerNo(listBatchNamePilerNoBind);

            //所有锯切图做完，更新批次状态已完成
            List<CuttingSawFileRelation> listCuttingSawFileRelation2 = lineControlCuttingServicePlus.
                GetCuttingSawFileRelationByBatchNameAndMinStackIndex(batchNamePilerNoBind.BatchName);
            if (listCuttingSawFileRelation2.Count < 1)
            {
                List<BatchGroupPlus> listBatchGroupPlus = new List<BatchGroupPlus>();
                BatchGroupPlus batchGroupPlus = new BatchGroupPlus();

                batchGroupPlus.BatchName = batchNamePilerNoBind.BatchName;
                batchGroupPlus.Status = (int)BatchGroupPlusStatus.ProductionIsCompleted;
                batchGroupPlus.UpdatedTime = DateTime.Now;

                listBatchGroupPlus.Add(batchGroupPlus);

                lineControlCuttingServicePlus.BulkUpdateBatchGroupPlusStatusByBatchName(listBatchGroupPlus);

                BatchProductionDetails batchProductionDetails = new BatchProductionDetails();
                batchProductionDetails.BatchName = batchGroupPlus.BatchName;
                batchProductionDetails.Status = (int)BatchGroupPlusStatus.ProductionIsCompleted;
                lineControlCuttingServicePlus.UpdateBatchProductionDetailsStatusByBatchName(batchProductionDetails);

                lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog(station, TriggerType.LineControl,
                    $"{batchGroupPlus.BatchName}批次所有锯切图已下发，更新BatchGroupPlus、BatchProductionDetails Status已完成({(int)BatchGroupPlusStatus.ProductionIsCompleted})"
                    , LogType.GENERAL));
            }
        }

    }
}
