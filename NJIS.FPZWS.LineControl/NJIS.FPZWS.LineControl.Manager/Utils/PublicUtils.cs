using NJIS.FPZWS.LineControl.Cutting.ModelPlus;
using NJIS.FPZWS.LineControl.Cutting.ServicePlus;
using NJIS.FPZWS.LineControl.Manager.Helpers;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace NJIS.FPZWS.LineControl.Manager.Utils
{
    public class PublicUtils
    {
        public static PLCLog newPLCLog(string station, TriggerType triggerType, string detail, LogType logType)
        {
            string version = "20.4.28";

            PLCLog plcLog = new PLCLog();
            plcLog.Station = station;
            //plcLog.TriggerType = triggerType.GetFinishStatusDescription().Item2;
            plcLog.TriggerType = triggerType.GetFinishStatusDescription().Item2;
            plcLog.Detail = $"开料管理客户端版本：{version}|{detail}";
            plcLog.LogType = logType.GetFinishStatusDescription().Item2;
            return plcLog;
        }

        /// <summary>
        /// 循环向plc写入int值，直到写入成功
        /// </summary>
        /// <param name="plc"></param>
        /// <param name="addr"></param>
        /// <param name="value"></param>
        public static void PLCWriteInt(PlcOperatorHelper plc,string addr,int value)
        {
            while (true)
            {
                plc.Write(addr, value);

                if (value == plc.ReadLong(addr))
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 循环向plc写入string值，直到写入成功
        /// </summary>
        /// <param name="plc"></param>
        /// <param name="addr"></param>
        /// <param name="value"></param>
        /// <param name="length"></param>
        public static void PLCWriteString(PlcOperatorHelper plc, string addr,string value,ushort length)
        {
            while (true)
            {
                plc.Write(addr, value, length);
                if (value.Equals(plc.ReadString(addr,length)))
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 手动下发Saw文件
        /// </summary>
        /// <param name="lineControlCuttingServicePlus"></param>
        /// <param name="cuttingSawFileRelation"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool ManuallyIssueSawFile(LineControlCuttingServicePlus lineControlCuttingServicePlus
            ,CuttingSawFileRelation cuttingSawFileRelation,out string error)
        {
            error = "";
            bool flag = false;

            List<BatchNamePilerNoBind> listBatchNamePilerNoBind = lineControlCuttingServicePlus
                .GetBatchNamePilerNoBindByStackName(cuttingSawFileRelation.StackName);

            if (listBatchNamePilerNoBind.Count < 1)
            {
                error = $"BatchNamePilerNoBind表中不存在垛号：{cuttingSawFileRelation.StackName}";
                return flag;
            }

            flag = CuttingSawFileRelationPlusDBUtils.AddAssignedByCuttingSawFileRelation(lineControlCuttingServicePlus,
                    cuttingSawFileRelation);

            if (flag)
            {
                BatchNamePilerNoBind batchNamePilerNoBind = listBatchNamePilerNoBind[0];
                //更新板垛剩余数量
                batchNamePilerNoBind.Count -= cuttingSawFileRelation.BoardCount;
                lineControlCuttingServicePlus.BulkUpdateBatchNamePilerNoBindByPilerNo(listBatchNamePilerNoBind);

                //已分配给1-5号锯的锯切图名称
                string sawFileName = getSawFileName(lineControlCuttingServicePlus, batchNamePilerNoBind.BatchName);

                //已分配给6号锯的垛
                string stackNames = getStackName6(lineControlCuttingServicePlus, batchNamePilerNoBind.BatchName);

                List<CuttingSawFileRelation> listCuttingSawFileRelation2 = lineControlCuttingServicePlus.
                    GetCuttingSawFileRelation(batchNamePilerNoBind.BatchName, sawFileName, stackNames, SawType.TYPE1,
                    "SawFileName");
                //所有锯切图做完，更新批次状态已完成
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

                    lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog("System", TriggerType.LineControl,
                        $"所有锯切图已下发，更新BatchGroupPlus、BatchProductionDetails Status已完成({(int)BatchGroupPlusStatus.ProductionIsCompleted})"
                        , LogType.GENERAL));
                }

                lineControlCuttingServicePlus.InsertPLCLog(PublicUtils.newPLCLog("System", TriggerType.LineControl, $"手动下发" +
                    $"锯切图：{cuttingSawFileRelation.SawFileName}", LogType.GENERAL));
            }
            else
                error = "数据库新增动作失败！";

            return flag;
        }

        /// <summary>
        /// 根据cuttingSawFileRelationPlus修改板垛数量，并更新批次及批次详情信息的状态
        /// </summary>
        /// <param name="lineControlCuttingServicePlus"></param>
        /// <param name="cuttingSawFileRelation"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static void updatePilerCount(LineControlCuttingServicePlus lineControlCuttingServicePlus
            , CuttingSawFileRelationPlus cuttingSawFileRelationPlus)
        {
            List<BatchNamePilerNoBind> listBatchNamePilerNoBind = lineControlCuttingServicePlus
                .GetBatchNamePilerNoBindByStackName(cuttingSawFileRelationPlus.StackName);

            //if (cuttingSawFileRelationPlus.Status != (int)CuttingSawFileRelationPlusStatus.Unassigned &&
            //    cuttingSawFileRelationPlus.Status != (int)CuttingSawFileRelationPlusStatus.Assigned)
            //    return;

            if (listBatchNamePilerNoBind.Count > 0 && cuttingSawFileRelationPlus.SawType != 1) {
                BatchNamePilerNoBind batchNamePilerNoBind = listBatchNamePilerNoBind[0];

                //如果修改状态为未分配，更新BatchNamePilerNoBind表中的数量
                if (cuttingSawFileRelationPlus.Status == (int)CuttingSawFileRelationPlusStatus.Unassigned)
                    batchNamePilerNoBind.Count += cuttingSawFileRelationPlus.BoardCount;
                //如果修改未分配状态为其他状态，更新BatchNamePilerNoBind表中的数量
                else
                {
                    List<CuttingSawFileRelationPlus> listCuttingSawFileRelationPlus = lineControlCuttingServicePlus.
                        GetCuttingSawFileRelationPlusBySawFileName(cuttingSawFileRelationPlus.SawFileName);
                    CuttingSawFileRelationPlus cuttingSawFileRelationPlus2 = listCuttingSawFileRelationPlus[0];
                    if(cuttingSawFileRelationPlus2.Status == (int)CuttingSawFileRelationPlusStatus.Unassigned)
                        batchNamePilerNoBind.Count -= cuttingSawFileRelationPlus.BoardCount;
                }

                //更新板垛剩余数量
                lineControlCuttingServicePlus.BulkUpdateBatchNamePilerNoBindByPilerNo(listBatchNamePilerNoBind);
            }
            
            //已分配给1-5号锯的锯切图名称
            string sawFileName = getSawFileName(lineControlCuttingServicePlus, cuttingSawFileRelationPlus.BatchName);

            //已分配给6号锯的垛
            string stackNames = getStackName6(lineControlCuttingServicePlus, cuttingSawFileRelationPlus.BatchName);

            List<CuttingSawFileRelation> listCuttingSawFileRelation2 = lineControlCuttingServicePlus.
            GetCuttingSawFileRelation(cuttingSawFileRelationPlus.BatchName, sawFileName, stackNames, SawType.TYPE1,
            "SawFileName");

            List<BatchGroupPlus> listBatchGroupPlus = new List<BatchGroupPlus>();
            BatchGroupPlus batchGroupPlus = new BatchGroupPlus();

            BatchProductionDetails batchProductionDetails = new BatchProductionDetails();

            //所有锯切图做完，更新批次状态已完成
            if (listCuttingSawFileRelation2.Count < 1)
            {
                batchGroupPlus.BatchName = cuttingSawFileRelationPlus.BatchName;
                batchGroupPlus.Status = (int)BatchGroupPlusStatus.ProductionIsCompleted;
                batchGroupPlus.UpdatedTime = DateTime.Now;

                listBatchGroupPlus.Add(batchGroupPlus);
                
                batchProductionDetails.BatchName = batchGroupPlus.BatchName;
                batchProductionDetails.Status = (int)BatchGroupPlusStatus.ProductionIsCompleted;
            }
            else
            {
                batchGroupPlus.BatchName = cuttingSawFileRelationPlus.BatchName;
                batchGroupPlus.Status = (int)BatchGroupPlusStatus.InProduction;
                batchGroupPlus.UpdatedTime = DateTime.Now;

                listBatchGroupPlus.Add(batchGroupPlus);

                batchProductionDetails.BatchName = batchGroupPlus.BatchName;
                batchProductionDetails.Status = (int)BatchGroupPlusStatus.InProduction;
            }

            lineControlCuttingServicePlus.BulkUpdateBatchGroupPlusStatusByBatchName(listBatchGroupPlus);

            lineControlCuttingServicePlus.UpdateBatchProductionDetailsStatusByBatchName(batchProductionDetails);
        }

        /// <summary>
        /// 根据批次号获取CuttingSawFileRelationPlus表中已分配给1-5号锯的所有锯切图名字，但不包含未分配数据
        /// </summary>
        /// <param name="lineControlCuttingServicePlus"></param>
        /// <param name="batchName"></param>
        /// <returns></returns>
        public static string getSawFileName(LineControlCuttingServicePlus lineControlCuttingServicePlus, string batchName)
        {
            List<CuttingSawFileRelationPlus> listCuttingSawFileRelationPlus =
                lineControlCuttingServicePlus.GetCuttingSawFileRelationPlus(batchName, SawType.TYPE1,
                CuttingSawFileRelationPlusStatus.Unassigned);
            //已下发的锯切图名称
            string sawFileName = "";
            for (int i = 0; i < listCuttingSawFileRelationPlus.Count; i++)
            {
                sawFileName += $"'{listCuttingSawFileRelationPlus[i].SawFileName}'";
                if (i < listCuttingSawFileRelationPlus.Count - 1)
                    sawFileName += ",";
            }
            if (string.IsNullOrEmpty(sawFileName))
                sawFileName += "''";

            return sawFileName;
        }

        /// <summary>
        /// 根据批次号获取CuttingSawFileRelationPlus表中已经分配给6号锯的垛名字
        /// </summary>
        /// <param name="lineControlCuttingServicePlus"></param>
        /// <param name="batchName"></param>
        /// <returns></returns>
        public static string getStackName6(LineControlCuttingServicePlus lineControlCuttingServicePlus, string batchName)
        {
            string stackNames = "";
            List<CuttingSawFileRelationPlus> listCuttingSawFileRelationPlus = lineControlCuttingServicePlus.
                GetCuttingSawFileRelationPlus(batchName, SawType.TYPE2,CuttingSawFileRelationPlusStatus.Unassigned);

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

            return stackNames;
        }

        public static bool isNotNumberAndNotDelete(KeyPressEventArgs e)
        {
            bool flag = false;
            if (!char.IsNumber(e.KeyChar) && e.KeyChar != (char)8)
                flag = true;
            return flag;
        }
    }
}
